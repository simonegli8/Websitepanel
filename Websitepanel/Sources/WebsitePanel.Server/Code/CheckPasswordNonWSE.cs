using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebsitePanel.Providers;
using WebsitePanel.Server;
using System.Web.Services.Protocols;

namespace WebsitePanel.Server {

   public class InvalidPasswordException : SoapHeaderException {
      public InvalidPasswordException(string msg) : base(msg, new System.Xml.XmlQualifiedName("InvalidPassword", "http://smbsaas/websitepanel/server/")) { }
   }

   public class CheckPasswordNonWSE {

      public static void Init() {

         ServiceProviderSettingsSoapHeader.CheckSecurity = (header) => {
            var token = header.Settings.FirstOrDefault(setting => setting.StartsWith("Server:Password="));
            if (token != null) {
               token = token.Substring(token.IndexOf('=')+1);
               if (token != ServerConfiguration.Security.Password) throw new InvalidPasswordException("Invalid server password.");
            }
         };

      }
   }
}
