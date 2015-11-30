using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace WebsitePanel.Providers {

	public class InvalidSignatureException : SoapException {
		public InvalidSignatureException() : base("Invalid message signature", new System.Xml.XmlQualifiedName("InvalidMessageSignature")) { }
	}
	public class ExpiredSignatureException : SoapException {
		public ExpiredSignatureException() : base("Expired message signature", new System.Xml.XmlQualifiedName("ExpiredMessageSignature")) { }
	}

	public class EncryptAndSignSoapExtension : SoapExtension {

		public IEnumerable AllObjects(SoapMessage msg) {
			var server = msg is SoapServerMessage;
			foreach (var h in msg.Headers) yield return h;
			foreach (var par in msg.MethodInfo.Parameters) {
				if (par.IsIn) yield return msg.GetInParameterValue(par.Position);
				if (par.IsOut) yield return msg.GetOutParameterValue(par.Position);
			}
			if (msg.Exception == null && ((server && msg.Stage == SoapMessageStage.BeforeSerialize) || (!server && msg.Stage == SoapMessageStage.AfterDeserialize))) yield return msg.GetReturnValue();
		}

		public override object GetInitializer(Type serviceType) => null;
		public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute) => null;
		public override void Initialize(object initializer) { }

		public string Hash(SoapMessage msg, EncryptionSession session) {
			StringBuilder txt = new StringBuilder();
			var signature = session.Signature;
			session.Signature = "";
			foreach (var x in AllObjects(msg)) { // serialize all objects to calculate hash 
				var xs = new XmlSerializer(x.GetType());
				using (var w = new StringWriter(txt)) xs.Serialize(w, x);
			}
			session.Signature = signature;
#if DEBUG
			var text = txt.ToString();
			var hash = Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.Unicode.GetBytes(txt.ToString())));
			System.Diagnostics.Debug.WriteLine("Hash:\r\n " + hash);
			System.Diagnostics.Debug.WriteLine(text);
#endif
			return txt.ToString();
		}

		public override void ProcessMessage(SoapMessage msg) {

			if (msg.Stage == SoapMessageStage.BeforeSerialize) {

				var session = msg.Headers.OfType<EncryptionSession>().FirstOrDefault();

				if (session != null && session.RemoteKey != null) {

					// encrypt all IEncrypted data, calculate hash and sign.
					foreach (IEncrypted x in AllObjects(msg).OfType<IEncrypted>()) x.Encrypt(session); // encrypt all IEncryped objects
																																  // sign message: calculate hash
					session.RemoteKeyHash = Encryption.PublicKeyHash(session.RemoteKey);
					session.RemoteKey = session.PublicKey;  // set remote RemoteKey to our public key.
					session.Signature = session.Sign(Hash(msg, session)); // sign message
				}
			}
		}
	}

	public class DecryptAndVerifySoapExtension : EncryptAndSignSoapExtension {

		public override void ProcessMessage(SoapMessage msg) {

			if (msg.Stage == SoapMessageStage.AfterDeserialize) {

				var session = msg.Headers.OfType<EncryptionSession>().FirstOrDefault();

				if (session != null && session.RemoteKey != null) {

					// compare signature with hash.
					if (!session.Verify(Hash(msg, session), session.Signature)) throw new InvalidSignatureException();

					// decrypt all IEncrypted data, calculate hash and check signature.
					if (session.Expires < DateTime.Now) throw new ExpiredSignatureException();
					foreach (IEncrypted x in AllObjects(msg).OfType<IEncrypted>()) x.Decrypt(session); // encrypt all IEncryped objects
				}
			}
		}
	}
}
