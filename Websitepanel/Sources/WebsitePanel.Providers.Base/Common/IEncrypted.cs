using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsitePanel.Providers {

   public interface IEncrypted {
      void Decrypt(EncryptionSession decryptor);
      void Encrypt(EncryptionSession encryptor);
   }

}
