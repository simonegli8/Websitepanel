using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace WebsitePanel.Providers {

	public class InvalidSignatureException: SoapException {
		public InvalidSignatureException() : base("Invalid message signature", new System.Xml.XmlQualifiedName("InvalidMessageSignature")) { }
   }
	public class ExpiredSignatureException : SoapException {
		public ExpiredSignatureException() : base("Expired message signature", new System.Xml.XmlQualifiedName("ExpiredMessageSignature")) { }
	}

	public interface ISoapExtension {
		void ProcessMessage(SoapMessage msg);
   }

	public class EncryptAndSignSoapExtension: SoapExtension {

		public IEnumerable AllObjects(SoapMessage msg, bool input) {
			foreach (var h in msg.Headers) yield return h;
			foreach (var par in msg.MethodInfo.Parameters) {
				if (input && par.IsIn) yield return msg.GetInParameterValue(par.Position);
				if (par.IsOut) yield return msg.GetOutParameterValue(par.Position);
			}
			if (!input) yield return msg.GetReturnValue();
		}

		public override object GetInitializer(Type serviceType) => null;
		public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute) => null;
		public override void Initialize(object initializer) {  }

		public override void ProcessMessage(SoapMessage msg) {
			var server = (msg as SoapServerMessage)?.Server;
			var client = (msg as SoapClientMessage)?.Client;
			if (!(server != null && server.GetType().FullName.StartsWith("WebsitePanel.Server.") || client != null && client.GetType().FullName.StartsWith("WebsitePanel.Server"))) return;

			foreach (ISoapExtension x in AllObjects(msg, msg is SoapClientMessage).OfType<ISoapExtension>()) x.ProcessMessage(msg); // call custom extensions

			if (msg.Stage == SoapMessageStage.AfterSerialize || msg.Stage == SoapMessageStage.BeforeDeserialize) return;

			StringBuilder txt = new StringBuilder();

			var session = msg.Headers.OfType<EncryptionSession>().FirstOrDefault();
			var hasEncrypted = msg.Headers.OfType<IEncrypted>().Any();
			if (session != null && hasEncrypted) {
				switch (msg.Stage) {
				case SoapMessageStage.BeforeSerialize: // encrypt all IEncrypted data, calculate hash and sign.
					foreach (IEncrypted x in AllObjects(msg, msg is SoapClientMessage).OfType<IEncrypted>()) x.Encrypt(session); // encrypt all IEncryped objects
					// sign message, calculate hash
					session.Signature = "";
					foreach (var x in AllObjects(msg, msg is SoapClientMessage)) { // serialize all objects to calculate hash 
						var xs = new XmlSerializer(x.GetType());
						using (var w = new StringWriter(txt)) xs.Serialize(w, x);
					}
					session.Signature = Convert.ToBase64String(session.Decrypt(MD5.Create().ComputeHash(Encoding.Unicode.GetBytes(txt.ToString())))); // sign hash
					break;
				case SoapMessageStage.AfterDeserialize:
					if (session.Expires > DateTime.Now) throw new ExpiredSignatureException();
					foreach (IEncrypted x in AllObjects(msg, msg is SoapClientMessage).OfType<IEncrypted>()) x.Decrypt(session); // encrypt all IEncryped objects
					// sign message, calculate hash
					var sign = session.Encrypt(Convert.FromBase64String(session.Signature));
               session.Signature = "";
					foreach (var x in AllObjects(msg, msg is SoapClientMessage)) { // serialize all objects to calculate hash 
						var xs = new XmlSerializer(x.GetType());
						using (var w = new StringWriter(txt)) xs.Serialize(w, x);
					}
					var md5 = MD5.Create().ComputeHash(Encoding.Unicode.GetBytes(txt.ToString()));
					if (sign.Length != md5.Length)
					for (int i = 0; i < sign.Length; i++) {
						if (sign[i] != md5[i]) throw new InvalidSignatureException();
					}
					break;
				}
			} else if (session != null || hasEncrypted) throw new NotSupportedException();
		}
	}

}
