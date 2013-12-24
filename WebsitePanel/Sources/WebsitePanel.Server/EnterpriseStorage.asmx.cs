// Copyright (c) 2012, Outercurve Foundation.
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
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;

using WebsitePanel.Providers;
using WebsitePanel.Providers.EnterpriseStorage;
using WebsitePanel.Providers.OS;
using WebsitePanel.Server.Utils;

namespace WebsitePanel.Server
{
    /// <summary>
    /// Summary description for EnterpriseStorage
    /// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class EnterpriseStorage : HostingServiceProviderWebService, IEnterpriseStorage
    {
        private IEnterpriseStorage EnterpriseStorageProvider
        {
            get { return (IEnterpriseStorage)Provider; }
        }


        [WebMethod, SoapHeader("settings")]
        public SystemFile[] GetFolders(string organizationId)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolders", ProviderSettings.ProviderName);
                SystemFile[] result = EnterpriseStorageProvider.GetFolders(organizationId);
                Log.WriteEnd("'{0}' GetFolders", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolders", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile GetFolder(string organizationId, string folder)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolder", ProviderSettings.ProviderName);
                SystemFile result = EnterpriseStorageProvider.GetFolder(organizationId, folder);
                Log.WriteEnd("'{0}' GetFolder", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void CreateFolder(string organizationId, string folder)
        {
            try
            {
                Log.WriteStart("'{0}' CreateFolder", ProviderSettings.ProviderName);
                EnterpriseStorageProvider.CreateFolder(organizationId, folder);
                Log.WriteEnd("'{0}' CreateFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteFolder(string organizationId, string folder)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteFolder", ProviderSettings.ProviderName);
                EnterpriseStorageProvider.DeleteFolder(organizationId, folder);
                Log.WriteEnd("'{0}' DeleteFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool SetFolderWebDavRules(string organizationId, string folder, Providers.Web.WebDavFolderRule[] rules)
        {
            try
            {
                Log.WriteStart("'{0}' SetFolderWebDavRules", ProviderSettings.ProviderName);
                return EnterpriseStorageProvider.SetFolderWebDavRules(organizationId, folder, rules);
                Log.WriteEnd("'{0}' SetFolderWebDavRules", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SetFolderWebDavRules", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public Providers.Web.WebDavFolderRule[] GetFolderWebDavRules(string organizationId, string folder)
        {
            try
            {
                Log.WriteStart("'{0}' GetFolderWebDavRules", ProviderSettings.ProviderName);
                return EnterpriseStorageProvider.GetFolderWebDavRules(organizationId, folder);
                Log.WriteEnd("'{0}' GetFolderWebDavRules", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetFolderWebDavRules", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool CheckFileServicesInstallation()
        {
            try
            {
                Log.WriteStart("'{0}' CheckFileServicesInstallation", ProviderSettings.ProviderName);
                return EnterpriseStorageProvider.CheckFileServicesInstallation();
                Log.WriteEnd("'{0}' CheckFileServicesInstallation", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CheckFileServicesInstallation", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public SystemFile RenameFolder(string organizationId, string originalFolder, string newFolder)
        {
            try
            {
                Log.WriteStart("'{0}' RenameFolder", ProviderSettings.ProviderName);
                return EnterpriseStorageProvider.RenameFolder(organizationId, originalFolder, newFolder);
                Log.WriteEnd("'{0}' RenameFolder", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RenameFolder", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
    }
}