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
					using (var r = new StreamReader(new MemoryStream(AsymmetricEncryption.DecryptBase64(EncryptedData)), Encoding.UTF8)) {
						var xs = new XmlSerializer(typeof(T));
						Value = (T)xs.Deserialize(r);
					}
				}
				EncryptedData = null;
				KeyHash = null;
			}
		}

		public void Encrypt(string publicKey) {
			using (var m = new MemoryStream())
			using (var w = new StreamWriter(m)) {
				var xs = new XmlSerializer(typeof(T));
				xs.Serialize(w, val);
				EncryptedData = AsymmetricEncryption.EncryptBase64(m.ToArray(), publicKey);
			}
			KeyHash = AsymmetricEncryption.PublicKeyHash(publicKey);
			Value = default(T);
		}

		public static implicit operator T(EncryptedValue<T> val) => val.Value;
		public static implicit operator EncryptedValue<T>(T val) => new EncryptedValue<T> { Value = val };
	}
}