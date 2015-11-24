using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace WebsitePanel.Providers.Common {

	public class EncryptedString : IEncryptedSerializable {

		string text;

		[XmlIgnore]
		public string Value {
			get { Decrypt(); return text; }
			set { text = value; }
		}

		public string EncryptedValue;
		[XmlAttribute]
		public string KeyHash;

		public void Decrypt() {
			lock (this) {
				if (!string.IsNullOrEmpty(EncryptedValue) && !string.IsNullOrEmpty(KeyHash)) {
					if (KeyHash != AsymmetricEncryption.KeyHash()) throw new InvalidEncryptionKeyException("Invalid encryption key for this server.");
					Value = Encoding.UTF8.GetString(AsymmetricEncryption.DecryptBase64(EncryptedValue));
				}
				EncryptedValue = null;
				KeyHash = null;
			}
		}

		public void Encrypt(string publicKey) {
			EncryptedValue = AsymmetricEncryption.EncryptBase64(Encoding.UTF8.GetBytes(text), publicKey);
			KeyHash = AsymmetricEncryption.PublicKeyHash(publicKey);
			Value = null;
		}

		public static implicit operator string(EncryptedString txt) => txt.Value;
		public static implicit operator EncryptedString(string txt) => new EncryptedString { Value = txt };
	}
}