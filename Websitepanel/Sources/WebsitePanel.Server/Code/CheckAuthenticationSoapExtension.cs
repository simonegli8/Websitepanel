using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;
using WebsitePanel.Providers;

namespace WebsitePanel.Server {

	public class PublicServiceAttribute: Attribute { }

	/// <summary>
	/// SoapExtension that checks the password for non WSE services.
	/// </summary>
	public class CheckAuthenticationSoapExtension : SoapExtension {

		public override object GetInitializer(Type serviceType) => null;
		public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute) => null;
		public override void Initialize(object initializer) { }

		public override void ProcessMessage(SoapMessage msg) {
			if (msg.Stage == SoapMessageStage.AfterDeserialize) {
				var server = (msg as SoapServerMessage)?.Server;
				if (server != null && !server.GetType().GetCustomAttributes(true).Any(a => Regex.IsMatch(a.GetType().FullName, "^Microsoft\\.Web\\.Services[23]?\\.Policy$"))) { // non WSE service
					var auth = msg.Headers.OfType<AuthenticationSoapHeader>().FirstOrDefault();
					if ((auth == null || auth.Password != ServerConfiguration.Security.Password) &&
						!server.GetType().GetCustomAttributes(typeof(PublicServiceAttribute), true).Any() &&
						!msg.MethodInfo.GetCustomAttributes(typeof(PublicServiceAttribute)).Any()) throw new InvalidPasswordException();
				}
         }
		}
	}

}
