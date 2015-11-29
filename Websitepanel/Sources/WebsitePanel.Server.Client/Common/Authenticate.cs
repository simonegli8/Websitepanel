using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;

namespace WebsitePanel.Server.Client {

	public class Authenticate : SoapExtension {

		public override object GetInitializer(Type serviceType) => null;
		public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute) => null;
		public override void Initialize(object initializer) { }

		public override void ProcessMessage(SoapMessage msg) {
			switch (msg.Stage) {
			case SoapMessageStage.AfterDeserialize:
				var client = (msg as SoapClientMessage)?.Client as ServiceProxy;
				if (client != null) {
					var auth = msg.Headers.OfType<AuthenticationSoapHeader>().FirstOrDefault();
					if (auth == null) {
						auth = new AuthenticationSoapHeader() { Password = client.Password };
						msg.Headers.Add(auth);
					}
				}
				break;
			default: break;
         }
		}
	}

}
