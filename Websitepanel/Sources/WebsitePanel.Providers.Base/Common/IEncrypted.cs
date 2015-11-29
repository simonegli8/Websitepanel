using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsitePanel.Providers {

   public interface IEncrypted {
      void Decrypt(EncryptionSession session);
      void Encrypt(EncryptionSession session);
   }

}
