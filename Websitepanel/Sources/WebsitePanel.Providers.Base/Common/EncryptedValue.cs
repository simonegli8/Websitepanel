using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WebsitePanel.Providers {

	public class EncryptedValue<T> : IEncryptedSerializable {

		T val;

		[XmlIgnore]
		public T Value {
			get { Decrypt(); return val; }
			set { val = value; }
		}

		public string EncryptedData;
		[XmlAttribute]
		public string KeyHash;

		public void Decrypt() {
			lock (this) {
				if (!string.IsNullOrEmpty(EncryptedData) && !string.IsNullOrEmpty(KeyHash)) {
					if (KeyHash != AsymmetricEncryption.KeyHash()) throw new InvalidEncryptionKeyException("Invalid encryption key for this server.");
					using (var m = new MemoryStream(AsymmetricEncryption.DecryptBase64(EncryptedData))) {
						var f = new BinaryFormatter();
						Value = (T)f.Deserialize(m);
					}
				}
				EncryptedData = null;
				KeyHash = null;
			}
		}

		public void Encrypt(string publicKey) {
			using (var m = new MemoryStream()) {
				var f = new BinaryFormatter();
				f.Serialize(m, val);
				EncryptedData = AsymmetricEncryption.EncryptBase64(m.ToArray(), publicKey);
			}
			KeyHash = AsymmetricEncryption.PublicKeyHash(publicKey);
			Value = default(T);
		}

		public static implicit operator T(EncryptedValue<T> val) => val.Value;
		public static implicit operator EncryptedValue<T>(T val) => new EncryptedValue<T> { Value = val };
	}
}