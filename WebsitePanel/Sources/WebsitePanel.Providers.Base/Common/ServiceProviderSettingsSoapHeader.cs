// Copyright (c) 2015, Outercurve Foundation.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  Outercurve Foundation  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Services.Protocols;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

namespace WebsitePanel.Providers
{

	public class InvalidEncryptionKeyException: SoapHeaderException {
		public InvalidEncryptionKeyException(string msg): base(msg, new System.Xml.XmlQualifiedName("InvalidEncryptionKey", "http://smbsaas/websitepanel/server/")) { }
   }

	/// <summary>
	/// Summary description for ServiceProviderSettings.
	/// </summary>
	public class ServiceProviderSettingsSoapHeader : SoapHeader, IEncrypted {

		bool deserialized = false;
		string[] settings;

		[SoapElement(IsNullable = true)]
		public string EncryptedSettings = null;

		[SoapElement(IsNullable = true)]
		public string KeyHash = null;

		public string[] Settings {
			get { Decrypt(); return settings; }
			set { settings = value; }
		}

		string publicKey = null;
		public string PublicKey {
			get {
				return publicKey ?? (publicKey = Settings.FirstOrDefault(s => s.StartsWith("Client:PublicKey="))?.Substring("Client:PublicKey=".Length));
			}
		}

		public static Action<ServiceProviderSettingsSoapHeader> CheckSecurity;

		/// <summary>
		/// This property is just a flag telling us that this SOAP header should be encrypted.
		/// </summary>
		[XmlAttribute("SecureHeader", Namespace = "http://smbsaas/websitepanel/server/")]
		public bool SecureHeader;

		public void Decrypt() {
			lock (this) {
				if (!deserialized && SecureHeader && !string.IsNullOrEmpty(EncryptedSettings) && !string.IsNullOrEmpty(KeyHash)) {
					if (KeyHash != AsymmetricEncryption.KeyHash()) throw new InvalidEncryptionKeyException("Invalid encryption key for this server.");
					var data = AsymmetricEncryption.DecryptBase64(EncryptedSettings);
					using (var r = new BinaryReader(new MemoryStream(data))) {
						var settings = new List<string>();
						while (r.BaseStream.CanRead) settings.Add(r.ReadString());
						Settings = settings.ToArray();
						if (CheckSecurity != null) CheckSecurity(this);
					}
				}
            EncryptedSettings = null;
            KeyHash = null;
				deserialized = true;
			}
		}

		public void Encrypt(string publicKey) {
			if (SecureHeader) {
				using (var w = new BinaryWriter(new MemoryStream())) {
					foreach (var txt in Settings) w.Write(txt);
					EncryptedSettings = AsymmetricEncryption.EncryptBase64(((MemoryStream)w.BaseStream).ToArray(), publicKey);
					KeyHash = AsymmetricEncryption.PublicKeyHash(publicKey);
               Settings = null;
				}
			}
		}
	}
}
