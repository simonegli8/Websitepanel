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

namespace WebsitePanel.Providers {

	/// <summary>
	/// A class that provides asymmetric encryption of strings.
	/// </summary>
	public static class AsymmetricEncryption {

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
		private static RSACryptoServiceProvider ReadKey(string keyFile = null) {
			if (keyFile == null) {
				keyFile = KeyFile;
				if (!keyCreated && !File.Exists(keyFile)) {
					GenerateKey(keyFile); keyCreated = true;
				}
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
			if (keyFile == null) keyFile = KeyFile;
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
		public static byte[] Encrypt(byte[] data, string publicKey) {
			// Create the algorithm based on the key
			RSACryptoServiceProvider Algorithm = new RSACryptoServiceProvider();
			Algorithm.FromXmlString(publicKey);

			// Now encrypt the data
			return Algorithm.Encrypt(data, true);
		}

		public static string EncryptBase64(byte[] data, string publicKey) => Convert.ToBase64String(Encrypt(data, publicKey));

		/// <summary>
		/// Decrypts a byte array into a string using a key file.
		/// </summary>
		/// <param name="data">The byte array with the encrypted data.</param>
		/// <param name="keyFile">The key file.</param>
		/// <returns>The decrypted string.</returns>
		public static byte[] Decrypt(byte[] data, string keyFile = null) {
			RSACryptoServiceProvider Algorithm = ReadKey(keyFile);
			return Algorithm.Decrypt(data, true);
		}

		public static byte[] DecryptBase64(string data, string keyFile = null) => Decrypt(Convert.FromBase64String(data), keyFile);

		public static string PublicKeyHash(string publicKey) => Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(publicKey)));
		public static string KeyHash(string keyFile = null) => PublicKeyHash(PublicKey(keyFile));
	}
}
