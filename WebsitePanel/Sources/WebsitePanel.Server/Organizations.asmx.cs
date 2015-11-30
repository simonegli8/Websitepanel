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
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using WebsitePanel.Providers;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.OS;
using WebsitePanel.Providers.ResultObjects;
using WebsitePanel.Server.Utils;

namespace WebsitePanel.Server
{
    /// <summary>
    /// Summary description for Organizations
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class Organizations : HostingServiceProviderWebService
    {

        private IOrganization Organization
        {
            get { return (IOrganization)Provider; }
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public bool OrganizationExists(string organizationId)
        {
            try
            {
                Log.WriteStart("'{0}' OrganizationExists", ProviderSettings.ProviderName);
                bool ret = Organization.OrganizationExists(organizationId);
                Log.WriteEnd("'{0}' OrganizationExists", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't CreateOrganization '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }


        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public Organization CreateOrganization(string organizationId)
        {
            try
            {
                Log.WriteStart("'{0}' CreateOrganization", ProviderSettings.ProviderName);
                Organization ret = Organization.CreateOrganization(organizationId);
                Log.WriteEnd("'{0}' CreateOrganization", ProviderSettings.ProviderName);
                return ret;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("Can't CreateOrganization '{0}' provider", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void DeleteOrganization(string organizationId)
        {
            Organization.DeleteOrganization(organizationId);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public int CreateUser(string organizationId, string loginName, string displayName, string upn, Encrypted<string> password, bool enabled)
        {
            return Organization.CreateUser(organizationId, loginName, displayName, upn, password, enabled);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void DisableUser(string loginName, string organizationId)
        {
            Organization.DisableUser(loginName, organizationId);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void DeleteUser(string loginName, string organizationId)
        {
            Organization.DeleteUser(loginName, organizationId);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public OrganizationUser GetUserGeneralSettings(string loginName, string organizationId)
        {
            return Organization.GetUserGeneralSettings(loginName, organizationId);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public int CreateSecurityGroup(string organizationId, string groupName)
        {
            return Organization.CreateSecurityGroup(organizationId, groupName);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public OrganizationSecurityGroup GetSecurityGroupGeneralSettings(string groupName, string organizationId)
        {
            return Organization.GetSecurityGroupGeneralSettings(groupName, organizationId);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void DeleteSecurityGroup(string groupName, string organizationId)
        {
            Organization.DeleteSecurityGroup(groupName, organizationId);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void SetSecurityGroupGeneralSettings(string organizationId, string groupName, string[] memberAccounts, string notes)
        {
            Organization.SetSecurityGroupGeneralSettings(organizationId, groupName, memberAccounts, notes);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void AddObjectToSecurityGroup(string organizationId, string accountName, string groupName)
        {
            Organization.AddObjectToSecurityGroup(organizationId, accountName, groupName);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void DeleteObjectFromSecurityGroup(string organizationId, string accountName, string groupName)
        {
            Organization.DeleteObjectFromSecurityGroup(organizationId, accountName, groupName);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void SetUserGeneralSettings(string organizationId, string accountName, string displayName, Encrypted<string> password,
            bool hideFromAddressBook, bool disabled, bool locked, string firstName, string initials, string lastName,
				Encrypted<string> address, Encrypted<string> city, Encrypted<string> state, Encrypted<string> zip, Encrypted<string> country, Encrypted<string> jobTitle,
            string company, string department, string office, string managerAccountName,
				Encrypted<string> businessPhone, Encrypted<string> fax, Encrypted<string> homePhone, Encrypted<string> mobilePhone, Encrypted<string> pager,
				Encrypted<string> webPage, string notes, string externalEmail, 
            bool userMustChangePassword)
        {
            Organization.SetUserGeneralSettings(organizationId, accountName, displayName, password, hideFromAddressBook,
                disabled, locked, firstName, initials, lastName, address, city, state, zip, country, jobTitle,
                company, department, office, managerAccountName, businessPhone, fax, homePhone,
                mobilePhone, pager, webPage, notes, externalEmail, userMustChangePassword);
        }


        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void SetUserPassword(string organizationId, string accountName, Encrypted<string> password)
        {
            Organization.SetUserPassword(organizationId, accountName, password);
        }


        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void SetUserPrincipalName(string organizationId, string accountName, string userPrincipalName)
        {
            Organization.SetUserPrincipalName(organizationId, accountName, userPrincipalName);
        }


        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void DeleteOrganizationDomain(string organizationDistinguishedName, string domain)
        {
            Organization.DeleteOrganizationDomain(organizationDistinguishedName, domain);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void CreateOrganizationDomain(string organizationDistinguishedName, string domain)
        {
            Organization.CreateOrganizationDomain(organizationDistinguishedName, domain);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public PasswordPolicyResult GetPasswordPolicy()
        {
            return Organization.GetPasswordPolicy();
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public string GetSamAccountNameByUserPrincipalName(string organizationId, string userPrincipalName)
        {
            return Organization.GetSamAccountNameByUserPrincipalName(organizationId, userPrincipalName);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public bool DoesSamAccountNameExist(string accountName)
        {
            return Organization.DoesSamAccountNameExist(accountName);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public MappedDrive[] GetDriveMaps(string organizationId)
        {
            return Organization.GetDriveMaps(organizationId);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public int CreateMappedDrive(string organizationId, string drive, string labelAs, string path)
        {
            return Organization.CreateMappedDrive(organizationId, drive, labelAs, path);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void DeleteMappedDrive(string organizationId, string drive)
        {
           Organization.DeleteMappedDrive(organizationId, drive);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void DeleteMappedDriveByPath(string organizationId, string path)
        {
            Organization.DeleteMappedDriveByPath(organizationId, path);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void DeleteMappedDrivesGPO(string organizationId)
        {
            Organization.DeleteMappedDrivesGPO(organizationId);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void SetDriveMapsTargetingFilter(string organizationId, ExchangeAccount[] accounts, string folderName)
        {
            Organization.SetDriveMapsTargetingFilter(organizationId, accounts, folderName);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void ChangeDriveMapFolderPath(string organizationId, string oldFolder, string newFolder)
        {
            Organization.ChangeDriveMapFolderPath(organizationId, oldFolder, newFolder);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public List<OrganizationUser> GetOrganizationUsersWithExpiredPassword(string organizationId, int daysBeforeExpiration)
        {
            return Organization.GetOrganizationUsersWithExpiredPassword(organizationId, daysBeforeExpiration);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public void ApplyPasswordSettings(string organizationId, OrganizationPasswordSettings passwordSettings)
        {
            Organization.ApplyPasswordSettings(organizationId, passwordSettings);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public bool CheckPhoneNumberIsInUse(string phoneNumber, string userSamAccountName = null)
        {
           return Organization.CheckPhoneNumberIsInUse(phoneNumber, userSamAccountName);
        }

        [WebMethod, SoapHeader("settings"), SoapHeader("auth"), SoapHeader("encryption")]
        public OrganizationUser GetOrganizationUserWithExtraData(string loginName, string organizationId)
        {
            return Organization.GetOrganizationUserWithExtraData(loginName, organizationId);
        }
    }
}
