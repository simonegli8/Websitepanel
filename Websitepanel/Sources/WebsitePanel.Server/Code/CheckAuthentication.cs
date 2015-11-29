using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;

namespace WebsitePanel.Server {

	public class CheckAuthentication : SoapExtension {

		public override object GetInitializer(Type serviceType) => null;
		public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute) => null;
		public override void Initialize(object initializer) { }

		public override void ProcessMessage(SoapMessage msg) {
			switch (msg.Stage) {
			case SoapMessageStage.AfterDeserialize:
				var server = (msg as SoapServerMessage)?.Server;
				if (server != null && !server.GetType().GetCustomAttributes(true).Any(a => Regex.IsMatch(a.GetType().FullName, "^Microsoft.Web.Services[23]?.Policy$"))) {
					var auth = msg.Headers.OfType<AuthenticationSoapHeader>().FirstOrDefault();
					if (auth == null || auth.Password != ServerConfiguration.Security.Password) throw new InvalidPasswordException();
				}
				break;
			default: break;
         }
		}
	}

}
