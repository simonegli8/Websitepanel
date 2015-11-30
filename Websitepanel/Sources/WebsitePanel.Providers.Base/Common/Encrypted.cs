using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Schema;

namespace WebsitePanel.Providers {


	/// <summary>
	/// A generic class for encrypted xml serialization. Before serialization Encrypt(publicKey) has to be called, to encrypt the content.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Encrypted<T> : IEncrypted {

		[XmlElement(IsNullable = true)]
		public T Value { get; set; }

		[XmlElement(IsNullable = true)]
		public string EncryptedValue;

		public virtual void Decrypt(EncryptionSession decryptor) {
			if (decryptor.Disabled) return;
			lock (this) {
				if (!string.IsNullOrEmpty(EncryptedValue)) {
					if (typeof(T) == typeof(byte[])) Value = (T)(object)decryptor.DecryptBase64(EncryptedValue);
					else if (typeof(T) == typeof(string)) Value = (T)(object)Encoding.UTF8.GetString(decryptor.DecryptBase64(EncryptedValue));
					else {
						using (var r = new StreamReader(new DeflateStream(new MemoryStream(decryptor.DecryptBase64(EncryptedValue)), CompressionMode.Decompress), Encoding.UTF8)) {
							var xs = new XmlSerializer(typeof(T));
							Value = (T)xs.Deserialize(r);
						}
					}
					EncryptedValue = null;
				} else Value = default(T);
			}
         foreach (var e in this.Encryptables()) e.Decrypt(decryptor);
		}

	public virtual void Encrypt(EncryptionSession encryptor) {
			if (encryptor.Disabled) return;
			if (Value == null) EncryptedValue = null;
			else {
				byte[] data;
				if (typeof(T) == typeof(byte[])) data = (byte[])(object)Value;
				else if (typeof(T) == typeof(string)) data = Encoding.UTF8.GetBytes((string)(object)Value);
				else {
					using (var m = new MemoryStream())
					using (var w = new StreamWriter(new DeflateStream(m, CompressionMode.Compress), Encoding.UTF8)) {
						var xs = new XmlSerializer(typeof(T));
						xs.Serialize(w, Value);
						w.Close();
						data = m.ToArray();
					}
				}
				EncryptedValue = encryptor.EncryptBase64(data);
				Value = default(T);
			}
			foreach (var e in this.Encryptables()) e.Encrypt(encryptor);
		}

		public static implicit operator T(Encrypted<T> val) => val.Value;
		public static implicit operator Encrypted<T>(T val) => new Encrypted<T> { Value = val };
	}

}