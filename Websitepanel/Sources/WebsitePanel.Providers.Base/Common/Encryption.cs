//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Web;
using System.Xml.Serialization;
using System.Web.Services.Protocols;
using System.Collections.Generic;
using System.Linq;

namespace WebsitePanel.Providers {

	[XmlRoot("Security")]
	public class EncryptionSession : SecureSoapHeader {

		public EncryptionSession() { StoreInfo = false; Disabled = false; ID = new Random().Next(); }

		public const int Timeout = 60000;
		[XmlIgnore]
		public string KeyFile { get; set; } = null;
		[XmlIgnore]
		public string PublicKey { get { return Encryption.PublicKey(); } }
		[XmlIgnore]
		public bool StoreInfo { get; set; }
		public string RemoteKey { get; set; }
		public string RemoteKeyHash { get; set; }
		public string SymmetricKey { get; set; }
		public string Signature { get; set; }
		public bool Disabled { get; set; }
		public int ID { get; set; }
		public DateTime Expires { get; set; } = DateTime.Now.AddMilliseconds(Timeout);
		public DateTime Created { get; set; } = DateTime.Now;

		bool keyChecked = false;
		public void ValidateKey() {
			if (!keyChecked) {
				if (RemoteKeyHash != Encryption.KeyHash(KeyFile)) throw new InvalidEncryptionKeyException();
				keyChecked = true;
			}
		}

		RijndaelManaged encrypt, decrypt;
		internal ICryptoTransform Encryptor {
			get {
				if (encrypt == null) {
					encrypt = new RijndaelManaged() { BlockSize = 128, KeySize = 256, Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 };
					encrypt.GenerateKey();
					encrypt.GenerateIV();
					RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
					rsa.FromXmlString(RemoteKey);
					RemoteKeyHash = Encryption.PublicKeyHash(RemoteKey);
					var m = new MemoryStream();
					using (var kw = new BinaryWriter(m)) {
						kw.Write((Int16)encrypt.Key.Length); kw.Write(encrypt.Key); kw.Write((Int16)encrypt.IV.Length); kw.Write(encrypt.IV);
						kw.Close();
						SymmetricKey = Convert.ToBase64String(rsa.Encrypt(m.ToArray(), true));
					}
				}
				return encrypt.CreateEncryptor();
			}
		}
		internal ICryptoTransform Decryptor {
			get {
				if (decrypt == null) {
					decrypt = new RijndaelManaged() { BlockSize = 128, KeySize = 256, Mode = CipherMode.CBC, Padding = PaddingMode.PKCS7 };
				}
				if (SymmetricKey == null) {
					var ec = Encryptor;
				} 
				var rsa = Encryption.ReadKey(KeyFile);
				using (var r = new BinaryReader(new MemoryStream(rsa.Decrypt(Convert.FromBase64String(SymmetricKey), true)))) {
					decrypt.Key = r.ReadBytes(r.ReadInt16());
					decrypt.IV = r.ReadBytes(r.ReadInt16());
				}
				return decrypt.CreateDecryptor();
			}
		}

		public void Init() { var te = Encryptor; var td = Decryptor; }

		public byte[] Encrypt(byte[] data) {
			if (Disabled) return data;
			var m = new MemoryStream();
			using (var w = new BinaryWriter(m)) {
				var e = Encryptor;
				if (StoreInfo) {
					w.Write(RemoteKeyHash);
					w.Write(SymmetricKey);
				}
				w.Write((Int32)data.Length);
				using (var we = new BinaryWriter(new CryptoStream(m, e, CryptoStreamMode.Write))) we.Write(data);
				return m.ToArray();
			}
		}
      public string EncryptBase64(byte[] data) => Convert.ToBase64String(Encrypt(data));
		public byte[] Decrypt(byte[] data) {
			if (Disabled) return data;
			var m = new MemoryStream(data);
			using (var r = new BinaryReader(m)) {
				if (StoreInfo) {
					RemoteKeyHash = r.ReadString();
					SymmetricKey = r.ReadString();
				}
				ValidateKey();
				using (var re = new BinaryReader(new CryptoStream(m, Decryptor, CryptoStreamMode.Read))) return re.ReadBytes(r.ReadInt32());
			}
		}
      public byte[] DecryptBase64(string data) => Decrypt(Convert.FromBase64String(data));
		public string Sign(byte[] data) {
			var rsa = Encryption.ReadKey(KeyFile);
			return Convert.ToBase64String(rsa.SignData(data, new MD5CryptoServiceProvider()));
		}
		public string Sign(string data) => Sign(Encoding.Unicode.GetBytes(data));
      public bool Verify(byte[] data, string signature) {
			RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
			rsa.FromXmlString(RemoteKey);
			return rsa.VerifyData(data, new MD5CryptoServiceProvider(), Convert.FromBase64String(signature));
		}
		public bool Verify(string data, string signature) => Verify(Encoding.Unicode.GetBytes(data), signature);
	}

	/// <summary>
	/// A class that provides asymmetric encryption of strings.
	/// </summary>
	public static class Encryption {

		static bool keyCreated = false;

		/// <summary>
		/// The default path to the applications key file.
		/// </summary>
		public static string KeyFile {
			get { return HttpContext.Current.Server.MapPath("~/App_Data/EncryptionKey.data"); }
		}
		/// <summary>
		/// Generates a key file with an asymetric key.
		/// </summary>
		/// <param name="targetFile">The file name of the key file to generate.</param>
		/// <returns></returns>
		public static string GenerateKey(string targetFile = null) {
			if (targetFile == null) targetFile = KeyFile;

			var dir = Path.GetDirectoryName(targetFile);

			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

			RSACryptoServiceProvider Algorithm = new RSACryptoServiceProvider();

			// Save the private key
			string CompleteKey = Algorithm.ToXmlString(true);
			byte[] KeyBytes = Encoding.UTF8.GetBytes(CompleteKey);

			KeyBytes = ProtectedData.Protect(KeyBytes,
								 null, DataProtectionScope.LocalMachine);

			using (FileStream fs = new FileStream(targetFile, FileMode.Create)) {
				fs.Write(KeyBytes, 0, KeyBytes.Length);
			}

			// Return the public key
			return Algorithm.ToXmlString(false);
		}

		/// <summary>
		/// Reads a key file
		/// </summary>
		/// <param name="algorithm">The encryption algorithm.</param>
		/// <param name="keyFile">The filename of the key file.</param>
		public static RSACryptoServiceProvider ReadKey(string keyFile = null) {
			if (keyFile == null) {
				keyFile = KeyFile;
				if (!keyCreated	&& (!Directory.Exists(Path.GetDirectoryName(keyFile)) || !File.Exists(keyFile))) {
					GenerateKey(keyFile); 
				}
				keyCreated = true;
			}

			var algorithm = new RSACryptoServiceProvider();

			byte[] KeyBytes;

			using (FileStream fs = new FileStream(keyFile, FileMode.Open)) {
				KeyBytes = new byte[fs.Length];
				fs.Read(KeyBytes, 0, (int)fs.Length);
			}

			KeyBytes = ProtectedData.Unprotect(KeyBytes, null, DataProtectionScope.LocalMachine);

			algorithm.FromXmlString(Encoding.UTF8.GetString(KeyBytes));

			return algorithm;
		}

		public static string PublicKey(string keyFile = null) {
			var algorithm = ReadKey(keyFile);
			// Return the public key
			return algorithm.ToXmlString(false);
		}


		/// <summary>
		/// Encrypts a string using a public key string.
		/// </summary>
		/// <param name="data">The string to encrypt.</param>
		/// <param name="publicKey">The public key.</param>
		/// <returns>A byte array containing the encrypted data.</returns>
		public static byte[] Encrypt(byte[] data, string publicKey) => new EncryptionSession() { RemoteKey = publicKey, StoreInfo = true }.Encrypt(data);
		public static string EncryptBase64(byte[] data, string publicKey) => Convert.ToBase64String(Encrypt(data, publicKey));

		/// <summary>
		/// Decrypts a byte array into a string using a key file.
		/// </summary>
		/// <param name="data">The byte array with the encrypted data.</param>
		/// <param name="keyFile">The key file.</param>
		/// <returns>The decrypted string.</returns>
		public static byte[] Decrypt(byte[] data, string keyFile = null) => new EncryptionSession() { KeyFile = keyFile, StoreInfo = true }.Decrypt(data);
		public static byte[] DecryptBase64(string data, string keyFile = null) => Decrypt(Convert.FromBase64String(data), keyFile);
		public static string Sign(byte[] data, string keyFile = null) => new EncryptionSession() { KeyFile = keyFile, StoreInfo = true }.Sign(data);
		public static string Sign(string data, string keyFile = null) => new EncryptionSession() { KeyFile = keyFile, StoreInfo = true }.Sign(data);
		public static bool Verify(byte[] data,string signature, string publicKey) => new EncryptionSession() { RemoteKey = publicKey, StoreInfo = true }.Verify(data, signature);
		public static bool Verfiy(string data, string signature, string publicKey) => new EncryptionSession() { RemoteKey = publicKey, StoreInfo = true }.Verify(data, signature);

		public static string PublicKeyHash(string publicKey = null) => Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(publicKey ?? PublicKey())));
		public static string KeyHash(string keyFile = null) => PublicKeyHash(PublicKey(keyFile));

	}

	public static class EncrypionExtensions {

		public static IEnumerable<IEncrypted> Encryptables(this IEncrypted self) {
			var type = self.GetType();
			return
				type.GetFields().Where(f => typeof(IEncrypted).IsAssignableFrom(f.FieldType)) // field of type IEncrypted
					.Select(f => (IEncrypted)f.GetValue(self))
				.Concat(
				type.GetProperties().Where(p => typeof(IEncrypted).IsAssignableFrom(p.PropertyType) && !p.GetIndexParameters().Any()) // properties of type IEncrypted
					.Select(p => (IEncrypted)p.GetValue(self, null)));
		}

	}
}
