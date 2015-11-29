using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebsitePanel.Providers;
using WebsitePanel.Server;
using System.Web.Services.Protocols;

namespace WebsitePanel.Server {

   public class InvalidPasswordException : SoapHeaderException {
      public InvalidPasswordException() : base("Invalid server password", new System.Xml.XmlQualifiedName("InvalidPassword", "http://smbsaas/websitepanel/server/")) { }
   }

	public class AuthenticationSoapHeader: SoapHeader, IEncrypted {

		public Encrypted<string> Password { get; set; }

		public void Decrypt(EncryptionSession session) => Password.Encrypt(session);
		public void Encrypt(EncryptionSession session) => Password.Decrypt(session);
	}

}
