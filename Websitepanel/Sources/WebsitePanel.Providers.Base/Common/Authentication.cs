using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebsitePanel.Providers;
using WebsitePanel.Server;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace WebsitePanel.Providers {

	public class InvalidPasswordException : SoapHeaderException {
		public InvalidPasswordException() : base("Invalid server password", new System.Xml.XmlQualifiedName("InvalidPassword", "http://smbsaas/websitepanel/server/")) { }
	}
	[XmlRoot("Authentication")]
	public class AuthenticationSoapHeader : SecureSoapHeader {
		public AuthenticationSoapHeader(): base() { }
		public Encrypted<string> Username { get; set; }
		public Encrypted<string> Password { get; set; }
	}

}
