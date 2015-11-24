using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsitePanel.Providers {

   public interface IEncryptedSerializable {
      void Decrypt();
      void Encrypt(string publicKey);
   }

}
