using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;

namespace WebsitePanel.Providers {

	public class Encrypted<T> : IEncryptedSerializable {

		T val;

		[XmlIgnore]
		public T Value {
			get { Decrypt(); return val; }
			set { val = value; }
		}

		public string EncryptedValue;
		[XmlAttribute]
		public string KeyHash;

		public void Decrypt() {
			lock (this) {
				if (!string.IsNullOrEmpty(EncryptedValue) && !string.IsNullOrEmpty(KeyHash)) {
					if (KeyHash != AsymmetricEncryption.KeyHash()) throw new InvalidEncryptionKeyException("Invalid encryption key for this server.");
					using (var r = new StreamReader(new DeflateStream(new MemoryStream(AsymmetricEncryption.DecryptBase64(EncryptedValue)), CompressionMode.Decompress), Encoding.UTF8)) {
						var xs = new XmlSerializer(typeof(T));
						Value = (T)xs.Deserialize(r);
					}
				}
				EncryptedValue = null;
				KeyHash = null;
			}
		}

		public void Encrypt(string publicKey) {
			using (var m = new MemoryStream())
			using (var w = new StreamWriter(new DeflateStream(m, CompressionMode.Compress))) {
				var xs = new XmlSerializer(typeof(T));
				xs.Serialize(w, val);
				w.Close();
				EncryptedValue = AsymmetricEncryption.EncryptBase64(m.ToArray(), publicKey);
			}
			KeyHash = AsymmetricEncryption.PublicKeyHash(publicKey);
			Value = default(T);
		}

		public static implicit operator T(Encrypted<T> val) => val.Value;
		public static implicit operator Encrypted<T>(T val) => new Encrypted<T> { Value = val };
	}
}