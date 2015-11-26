﻿// Copyright (c) 2015, Outercurve Foundation.
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
using System.Reflection;
using System.Collections.Generic;
using System.Text;

#if Net
using Microsoft.Web.Services3;
using Microsoft.Web.Services3.Design;
#endif

using System.Web.Services.Protocols;

using WebsitePanel.Providers;

namespace WebsitePanel.Server.Client {
   public class ServerProxyConfigurator {
      private int timeout = -1;
      private string serverUrl = null;
      private string serverPassword = null;
      RemoteServerSettings serverSettings = new RemoteServerSettings();
      ServiceProviderSettings providerSettings = new ServiceProviderSettings();

      public WebsitePanel.Providers.RemoteServerSettings ServerSettings {
         get { return this.serverSettings; }
         set { this.serverSettings = value; }
      }

      public WebsitePanel.Providers.ServiceProviderSettings ProviderSettings {
         get { return this.providerSettings; }
         set { this.providerSettings = value; }
      }

      public string ServerUrl {
         get { return this.serverUrl; }
         set { this.serverUrl = value; }
      }

      public string ServerPassword {
         get { return this.serverPassword; }
         set { this.serverPassword = value; }
      }

      public int Timeout {
         get { return this.timeout; }
         set { this.timeout = value; }
      }

      public void Configure(SoapHttpClientProtocol proxy) {
         ServiceProxy service = proxy as ServiceProxy;
         SoapHttpClientProtocol wservice = null;
         if (service != null) {
            wservice = service.Service as SoapHttpClientProtocol;
            if (wservice != null && wservice != service) Configure(wservice);
         }

         // set proxy timeout
         if (service != null) service.Timeout = (timeout == -1) ? System.Threading.Timeout.Infinite : timeout * 1000;
         else proxy.Timeout = (timeout == -1) ? System.Threading.Timeout.Infinite : timeout * 1000;

#if Net
         // setup security assertion
         if (!String.IsNullOrEmpty(serverPassword) && (proxy is Microsoft.Web.Services3.WebServicesClientProtocol)) {
            ServerUsernameAssertion assert
                = new ServerUsernameAssertion(ServerSettings.ServerId, serverPassword);


            // create policy
            Policy policy = new Policy();
            policy.Assertions.Add(assert);

           ((Microsoft.Web.Services3.WebServicesClientProtocol)proxy).SetPolicy(policy);
         }
#endif
         // provider settings
         ServiceProviderSettingsSoapHeader settingsHeader = new ServiceProviderSettingsSoapHeader();
         List<string> settings = new List<string>();

         if (!String.IsNullOrEmpty(serverPassword) && service != null && service.NoWSE) {  // NonWSE service
            settings.Add("Server:Password=" + serverPassword);
            settingsHeader.SecureHeader = true;
        }

         // AD Settings
         settings.Add("AD:Enabled=" + ServerSettings.ADEnabled.ToString());
         settings.Add("AD:AuthenticationType=" + ServerSettings.ADAuthenticationType.ToString());
         settings.Add("AD:RootDomain=" + ServerSettings.ADRootDomain);
         settings.Add("AD:Username=" + ServerSettings.ADUsername);
         settings.Add("AD:Password=" + ServerSettings.ADPassword);

         // Server Settings
         settings.Add("Server:ServerId=" + ServerSettings.ServerId);
         settings.Add("Server:ServerName=" + ServerSettings.ServerName);

         // Provider Settings
         settings.Add("Provider:ProviderGroupID=" + ProviderSettings.ProviderGroupID.ToString());
         settings.Add("Provider:ProviderCode=" + ProviderSettings.ProviderCode);
         settings.Add("Provider:ProviderName=" + ProviderSettings.ProviderName);
         settings.Add("Provider:ProviderType=" + ProviderSettings.ProviderType);

			// Public key for response encryption
			settings.Add("Client:PublicKey=" + AsymmetricEncryption.PublicKey());
			
			// Custom Provider Settings
         foreach (string settingName in ProviderSettings.Settings.Keys) {
            settings.Add(settingName + "=" + ProviderSettings.Settings[settingName]);
         }

         // set header
         settingsHeader.Settings = settings.ToArray();
         FieldInfo field = proxy.GetType().GetField("ServiceProviderSettingsSoapHeaderValue");
			if (field != null) {
				field.SetValue(proxy, settingsHeader);
			}

         // configure proxy URL
         if (!String.IsNullOrEmpty(serverUrl)) {
            if (serverUrl.EndsWith("/"))
               serverUrl = serverUrl.Substring(0, serverUrl.Length - 1);
            if (service != null) service.Url = serverUrl + service.Url.Substring(service.Url.LastIndexOf('/'));
            else proxy.Url = serverUrl + proxy.Url.Substring(proxy.Url.LastIndexOf('/'));
         }

      }
   }
}
