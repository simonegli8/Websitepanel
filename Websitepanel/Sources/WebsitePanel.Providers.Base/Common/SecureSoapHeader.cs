using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;

namespace WebsitePanel.Providers {

	public class SecureSoapHeader : SoapHeader, IEncrypted {
		public virtual void Decrypt(EncryptionSession decryptor) { foreach (var e in this.Encryptables()) e.Decrypt(decryptor); }
		public virtual void Encrypt(EncryptionSession encryptor) { foreach (var e in this.Encryptables()) e.Encrypt(encryptor); }
	}
}
