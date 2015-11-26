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

#if Net // machine generated

using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Microsoft.Web.Services3;

using WebsitePanel.Providers;
using WebsitePanel.Providers.Virtualization;
using WebsitePanel.Server.Utils;
using System.Collections.Generic;

namespace WebsitePanel.Server
{
    /// <summary>
    /// Summary description for VirtualizationServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class VirtualizationServer2012 : HostingServiceProviderWebService //, IVirtualizationServer2012
    {
        private IVirtualizationServer2012 VirtualizationProvider
        {
            get { return (IVirtualizationServer2012)Provider; }
        }

        #region Virtual Machines

        [WebMethod, SoapHeader("settings")]
        public Encrypted<VirtualMachine> GetVirtualMachine(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualMachine", ProviderSettings.ProviderName);
                VirtualMachine result = VirtualizationProvider.GetVirtualMachine(vmId);
                Log.WriteEnd("'{0}' GetVirtualMachine", ProviderSettings.ProviderName);
                return result.Encrypt(settings.PublicKey);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualMachine", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public Encrypted<VirtualMachine> GetVirtualMachineEx(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualMachineEx", ProviderSettings.ProviderName);
                VirtualMachine result = VirtualizationProvider.GetVirtualMachineEx(vmId);
                Log.WriteEnd("'{0}' GetVirtualMachineEx", ProviderSettings.ProviderName);
                return result.Encrypt(settings.PublicKey);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualMachineEx", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public Encrypted<List<VirtualMachine>> GetVirtualMachines()
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualMachines", ProviderSettings.ProviderName);
                List<VirtualMachine> result = VirtualizationProvider.GetVirtualMachines();
                Log.WriteEnd("'{0}' GetVirtualMachines", ProviderSettings.ProviderName);
                return result.Encrypt(settings.PublicKey);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualMachines", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public byte[] GetVirtualMachineThumbnailImage(string vmId, ThumbnailSize size)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualMachineThumbnailImage", ProviderSettings.ProviderName);
                byte[] result = VirtualizationProvider.GetVirtualMachineThumbnailImage(vmId, size);
                Log.WriteEnd("'{0}' GetVirtualMachineThumbnailImage", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualMachineThumbnailImage", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public Encrypted<VirtualMachine> CreateVirtualMachine(Encrypted<VirtualMachine> vm)
        {
            try
            {
                Log.WriteStart("'{0}' CreateVirtualMachine", ProviderSettings.ProviderName);
                VirtualMachine result = VirtualizationProvider.CreateVirtualMachine(vm);
                Log.WriteEnd("'{0}' CreateVirtualMachine", ProviderSettings.ProviderName);
                return result.Encrypt(settings.PublicKey);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateVirtualMachine", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public Encrypted<VirtualMachine> UpdateVirtualMachine(Encrypted<VirtualMachine> vm)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateVirtualMachine", ProviderSettings.ProviderName);
                VirtualMachine result = VirtualizationProvider.UpdateVirtualMachine(vm);
                Log.WriteEnd("'{0}' UpdateVirtualMachine", ProviderSettings.ProviderName);
                return result.Encrypt(settings.PublicKey);
			}
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateVirtualMachine", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult ChangeVirtualMachineState(string vmId, VirtualMachineRequestedState newState)
        {
            try
            {
                Log.WriteStart("'{0}' ChangeVirtualMachineState", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.ChangeVirtualMachineState(vmId, newState);
                Log.WriteEnd("'{0}' ChangeVirtualMachineState", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ChangeVirtualMachineState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ReturnCode ShutDownVirtualMachine(string vmId, bool force, string reason)
        {
            try
            {
                Log.WriteStart("'{0}' ShutDownVirtualMachine", ProviderSettings.ProviderName);
                ReturnCode result = VirtualizationProvider.ShutDownVirtualMachine(vmId, force, reason);
                Log.WriteEnd("'{0}' ShutDownVirtualMachine", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ShutDownVirtualMachine", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<ConcreteJob> GetVirtualMachineJobs(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualMachineJobs", ProviderSettings.ProviderName);
                List<ConcreteJob> result = VirtualizationProvider.GetVirtualMachineJobs(vmId);
                Log.WriteEnd("'{0}' GetVirtualMachineJobs", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualMachineJobs", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult RenameVirtualMachine(string vmId, string name)
        {
            try
            {
                Log.WriteStart("'{0}' RenameVirtualMachine", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.RenameVirtualMachine(vmId, name);
                Log.WriteEnd("'{0}' RenameVirtualMachine", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RenameVirtualMachine", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult DeleteVirtualMachine(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteVirtualMachine", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.DeleteVirtualMachine(vmId);
                Log.WriteEnd("'{0}' DeleteVirtualMachine", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteVirtualMachine", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult ExportVirtualMachine(string vmId, string exportPath)
        {
            try
            {
                Log.WriteStart("'{0}' ExportVirtualMachine", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.ExportVirtualMachine(vmId, exportPath);
                Log.WriteEnd("'{0}' ExportVirtualMachine", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ExportVirtualMachine", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Snapshots
        [WebMethod, SoapHeader("settings")]
        public List<VirtualMachineSnapshot> GetVirtualMachineSnapshots(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualMachineSnapshots", ProviderSettings.ProviderName);
                List<VirtualMachineSnapshot> result = VirtualizationProvider.GetVirtualMachineSnapshots(vmId);
                Log.WriteEnd("'{0}' GetVirtualMachineSnapshots", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualMachineSnapshots", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public VirtualMachineSnapshot GetSnapshot(string snapshotId)
        {
            try
            {
                Log.WriteStart("'{0}' GetSnapshot", ProviderSettings.ProviderName);
                VirtualMachineSnapshot result = VirtualizationProvider.GetSnapshot(snapshotId);
                Log.WriteEnd("'{0}' GetSnapshot", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSnapshot", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult CreateSnapshot(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' CreateSnapshot", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.CreateSnapshot(vmId);
                Log.WriteEnd("'{0}' CreateSnapshot", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateSnapshot", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult RenameSnapshot(string vmId, string snapshotId, string name)
        {
            try
            {
                Log.WriteStart("'{0}' RenameSnapshot", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.RenameSnapshot(vmId, snapshotId, name);
                Log.WriteEnd("'{0}' RenameSnapshot", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RenameSnapshot", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult ApplySnapshot(string vmId, string snapshotId)
        {
            try
            {
                Log.WriteStart("'{0}' ApplySnapshot", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.ApplySnapshot(vmId, snapshotId);
                Log.WriteEnd("'{0}' ApplySnapshot", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ApplySnapshot", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult DeleteSnapshot(string snapshotId)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteSnapshot", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.DeleteSnapshot(snapshotId);
                Log.WriteEnd("'{0}' DeleteSnapshot", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteSnapshot", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult DeleteSnapshotSubtree(string snapshotId)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteSnapshotSubtree", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.DeleteSnapshotSubtree(snapshotId);
                Log.WriteEnd("'{0}' DeleteSnapshotSubtree", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteSnapshotSubtree", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public byte[] GetSnapshotThumbnailImage(string snapshotId, ThumbnailSize size)
        {
            try
            {
                Log.WriteStart("'{0}' GetSnapshotThumbnailImage", ProviderSettings.ProviderName);
                byte[] result = VirtualizationProvider.GetSnapshotThumbnailImage(snapshotId, size);
                Log.WriteEnd("'{0}' GetSnapshotThumbnailImage", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSnapshotThumbnailImage", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Virtual Switches
        [WebMethod, SoapHeader("settings")]
        public List<VirtualSwitch> GetExternalSwitches(string computerName)
        {
            try
            {
                Log.WriteStart("'{0}' GetExternalSwitches", ProviderSettings.ProviderName);
                List<VirtualSwitch> result = VirtualizationProvider.GetExternalSwitches(computerName);
                Log.WriteEnd("'{0}' GetExternalSwitches", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetExternalSwitches", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<VirtualSwitch> GetSwitches()
        {
            try
            {
                Log.WriteStart("'{0}' GetSwitches", ProviderSettings.ProviderName);
                List<VirtualSwitch> result = VirtualizationProvider.GetSwitches();
                Log.WriteEnd("'{0}' GetSwitches", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetSwitches", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public bool SwitchExists(string switchId)
        {
            try
            {
                Log.WriteStart("'{0}' SwitchExists", ProviderSettings.ProviderName);
                bool result = VirtualizationProvider.SwitchExists(switchId);
                Log.WriteEnd("'{0}' SwitchExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SwitchExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public VirtualSwitch CreateSwitch(string name)
        {
            try
            {
                Log.WriteStart("'{0}' CreateSwitch", ProviderSettings.ProviderName);
                VirtualSwitch result = VirtualizationProvider.CreateSwitch(name);
                Log.WriteEnd("'{0}' CreateSwitch", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' CreateSwitch", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ReturnCode DeleteSwitch(string switchId)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteSwitch", ProviderSettings.ProviderName);
                ReturnCode result = VirtualizationProvider.DeleteSwitch(switchId);
                Log.WriteEnd("'{0}' DeleteSwitch", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteSwitch", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region DVD operations
        [WebMethod, SoapHeader("settings")]
        public string GetInsertedDVD(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetInsertedDVD", ProviderSettings.ProviderName);
                string result = VirtualizationProvider.GetInsertedDVD(vmId);
                Log.WriteEnd("'{0}' GetInsertedDVD", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetInsertedDVD", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult InsertDVD(string vmId, string isoPath)
        {
            try
            {
                Log.WriteStart("'{0}' InsertDVD", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.InsertDVD(vmId, isoPath);
                Log.WriteEnd("'{0}' InsertDVD", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' InsertDVD", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult EjectDVD(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' EjectDVD", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.EjectDVD(vmId);
                Log.WriteEnd("'{0}' EjectDVD", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' EjectDVD", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region KVP items
        [WebMethod, SoapHeader("settings")]
        public List<KvpExchangeDataItem> GetKVPItems(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetKVPItems", ProviderSettings.ProviderName);
                List<KvpExchangeDataItem> result = VirtualizationProvider.GetKVPItems(vmId);
                Log.WriteEnd("'{0}' GetKVPItems", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetKVPItems", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<KvpExchangeDataItem> GetStandardKVPItems(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetStandardKVPItems", ProviderSettings.ProviderName);
                List<KvpExchangeDataItem> result = VirtualizationProvider.GetStandardKVPItems(vmId);
                Log.WriteEnd("'{0}' GetStandardKVPItems", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetStandardKVPItems", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult AddKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            try
            {
                Log.WriteStart("'{0}' AddKVPItems", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.AddKVPItems(vmId, items);
                Log.WriteEnd("'{0}' AddKVPItems", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddKVPItems", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult RemoveKVPItems(string vmId, string[] itemNames)
        {
            try
            {
                Log.WriteStart("'{0}' RemoveKVPItems", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.RemoveKVPItems(vmId, itemNames);
                Log.WriteEnd("'{0}' RemoveKVPItems", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' RemoveKVPItems", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult ModifyKVPItems(string vmId, KvpExchangeDataItem[] items)
        {
            try
            {
                Log.WriteStart("'{0}' ModifyKVPItems", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.ModifyKVPItems(vmId, items);
                Log.WriteEnd("'{0}' ModifyKVPItems", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ModifyKVPItems", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Storage
        [WebMethod, SoapHeader("settings")]
        public VirtualHardDiskInfo GetVirtualHardDiskInfo(string vhdPath)
        {
            try
            {
                Log.WriteStart("'{0}' GetVirtualHardDiskInfo", ProviderSettings.ProviderName);
                VirtualHardDiskInfo result = VirtualizationProvider.GetVirtualHardDiskInfo(vhdPath);
                Log.WriteEnd("'{0}' GetVirtualHardDiskInfo", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetVirtualHardDiskInfo", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public MountedDiskInfo MountVirtualHardDisk(string vhdPath)
        {
            try
            {
                Log.WriteStart("'{0}' MountVirtualHardDisk", ProviderSettings.ProviderName);
                MountedDiskInfo result = VirtualizationProvider.MountVirtualHardDisk(vhdPath);
                Log.WriteEnd("'{0}' MountVirtualHardDisk", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' MountVirtualHardDisk", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ReturnCode UnmountVirtualHardDisk(string vhdPath)
        {
            try
            {
                Log.WriteStart("'{0}' UnmountVirtualHardDisk", ProviderSettings.ProviderName);
                ReturnCode result = VirtualizationProvider.UnmountVirtualHardDisk(vhdPath);
                Log.WriteEnd("'{0}' UnmountVirtualHardDisk", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UnmountVirtualHardDisk", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult ExpandVirtualHardDisk(string vhdPath, UInt64 sizeGB)
        {
            try
            {
                Log.WriteStart("'{0}' ExpandVirtualHardDisk", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.ExpandVirtualHardDisk(vhdPath, sizeGB);
                Log.WriteEnd("'{0}' ExpandVirtualHardDisk", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ExpandVirtualHardDisk", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public JobResult ConvertVirtualHardDisk(string sourcePath, string destinationPath, VirtualHardDiskType diskType)
        {
            try
            {
                Log.WriteStart("'{0}' ConvertVirtualHardDisk", ProviderSettings.ProviderName);
                JobResult result = VirtualizationProvider.ConvertVirtualHardDisk(sourcePath, destinationPath, diskType);
                Log.WriteEnd("'{0}' ConvertVirtualHardDisk", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ConvertVirtualHardDisk", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteRemoteFile(string path)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteRemoteFile", ProviderSettings.ProviderName);
                VirtualizationProvider.DeleteRemoteFile(path);
                Log.WriteEnd("'{0}' DeleteRemoteFile", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteRemoteFile", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ExpandDiskVolume(string diskAddress, string volumeName)
        {
            try
            {
                Log.WriteStart("'{0}' ExpandDiskVolume", ProviderSettings.ProviderName);
                VirtualizationProvider.ExpandDiskVolume(diskAddress, volumeName);
                Log.WriteEnd("'{0}' ExpandDiskVolume", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ExpandDiskVolume", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string ReadRemoteFile(string path)
        {
            try
            {
                Log.WriteStart("'{0}' ReadRemoteFile", ProviderSettings.ProviderName);
                string result = VirtualizationProvider.ReadRemoteFile(path);
                Log.WriteEnd("'{0}' ReadRemoteFile", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ReadRemoteFile", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void WriteRemoteFile(string path, string content)
        {
            try
            {
                Log.WriteStart("'{0}' WriteRemoteFile", ProviderSettings.ProviderName);
                VirtualizationProvider.WriteRemoteFile(path, content);
                Log.WriteEnd("'{0}' WriteRemoteFile", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' WriteRemoteFile", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Jobs
        [WebMethod, SoapHeader("settings")]
        public ConcreteJob GetJob(string jobId)
        {
            try
            {
                Log.WriteStart("'{0}' GetJob", ProviderSettings.ProviderName);
                ConcreteJob result = VirtualizationProvider.GetJob(jobId);
                Log.WriteEnd("'{0}' GetJob", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetJob", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public List<ConcreteJob> GetAllJobs()
        {
            try
            {
                Log.WriteStart("'{0}' GetAllJobs", ProviderSettings.ProviderName);
                List<ConcreteJob> result = VirtualizationProvider.GetAllJobs();
                Log.WriteEnd("'{0}' GetAllJobs", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetAllJobs", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ChangeJobStateReturnCode ChangeJobState(string jobId, ConcreteJobRequestedState newState)
        {
            try
            {
                Log.WriteStart("'{0}' ChangeJobState", ProviderSettings.ProviderName);
                ChangeJobStateReturnCode result = VirtualizationProvider.ChangeJobState(jobId, newState);
                Log.WriteEnd("'{0}' ChangeJobState", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ChangeJobState", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Configuration
        [WebMethod, SoapHeader("settings")]
        public int GetProcessorCoresNumber()
        {
            try
            {
                Log.WriteStart("'{0}' GetProcessorCoresNumber", ProviderSettings.ProviderName);
                int result = VirtualizationProvider.GetProcessorCoresNumber();
                Log.WriteEnd("'{0}' GetProcessorCoresNumber", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetProcessorCoresNumber", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        #endregion


        #region Replication

        [WebMethod, SoapHeader("settings")]
        public List<CertificateInfo> GetCertificates(string remoteServer)
        {
            try
            {
                Log.WriteStart("'{0}' GetCertificates", ProviderSettings.ProviderName);
                var result = VirtualizationProvider.GetCertificates(remoteServer);
                Log.WriteEnd("'{0}' GetCertificates", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetCertificates", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetReplicaServer(string remoteServer, string thumbprint, string storagePath)
        {
            try
            {
                Log.WriteStart("'{0}' SetReplicaServer", ProviderSettings.ProviderName);
                VirtualizationProvider.SetReplicaServer(remoteServer, thumbprint, storagePath);
                Log.WriteEnd("'{0}' SetReplicaServer", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SetReplicaServer", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UnsetReplicaServer(string remoteServer)
        {
            try
            {
                Log.WriteStart("'{0}' UnsetReplicaServer", ProviderSettings.ProviderName);
                VirtualizationProvider.UnsetReplicaServer(remoteServer);
                Log.WriteEnd("'{0}' UnsetReplicaServer", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UnsetReplicaServer", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ReplicationServerInfo GetReplicaServer(string remoteServer)
        {
            try
            {
                Log.WriteStart("'{0}' IsReplicaServer", ProviderSettings.ProviderName);
                var result = VirtualizationProvider.GetReplicaServer(remoteServer);
                Log.WriteEnd("'{0}' IsReplicaServer", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' IsReplicaServer", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void EnableVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            try
            {
                Log.WriteStart("'{0}' EnableVmReplication", ProviderSettings.ProviderName);
                VirtualizationProvider.EnableVmReplication(vmId, replicaServer, replication);
                Log.WriteEnd("'{0}' EnableVmReplication", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' EnableVmReplication", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void SetVmReplication(string vmId, string replicaServer, VmReplication replication)
        {
            try
            {
                Log.WriteStart("'{0}' SetVmReplication", ProviderSettings.ProviderName);
                VirtualizationProvider.SetVmReplication(vmId, replicaServer, replication);
                Log.WriteEnd("'{0}' SetVmReplication", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' SetVmReplication", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void TestReplicationServer(string vmId, string replicaServer, string localThumbprint)
        {
            try
            {
                Log.WriteStart("'{0}' TestReplicationServer", ProviderSettings.ProviderName);
                VirtualizationProvider.TestReplicationServer(vmId, replicaServer, localThumbprint);
                Log.WriteEnd("'{0}' TestReplicationServer", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' TestReplicationServer", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void StartInitialReplication(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' StartInitialReplication", ProviderSettings.ProviderName);
                VirtualizationProvider.StartInitialReplication(vmId);
                Log.WriteEnd("'{0}' StartInitialReplication", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' StartInitialReplication", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public VmReplication GetReplication(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetReplication", ProviderSettings.ProviderName);
                var result = VirtualizationProvider.GetReplication(vmId);
                Log.WriteEnd("'{0}' GetReplication", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetReplication", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DisableVmReplication(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' DisableVmReplication", ProviderSettings.ProviderName);
                VirtualizationProvider.DisableVmReplication(vmId);
                Log.WriteEnd("'{0}' DisableVmReplication", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DisableVmReplication", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public ReplicationDetailInfo GetReplicationInfo(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' GetReplicationInfo", ProviderSettings.ProviderName);
                var result = VirtualizationProvider.GetReplicationInfo(vmId);
                Log.WriteEnd("'{0}' GetReplicationInfo", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetReplicationInfo", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void PauseReplication(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' PauseReplication", ProviderSettings.ProviderName);
                VirtualizationProvider.PauseReplication(vmId);
                Log.WriteEnd("'{0}' PauseReplication", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' PauseReplication", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void ResumeReplication(string vmId)
        {
            try
            {
                Log.WriteStart("'{0}' ResumeReplication", ProviderSettings.ProviderName);
                VirtualizationProvider.ResumeReplication(vmId);
                Log.WriteEnd("'{0}' ResumeReplication", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ResumeReplication", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

    }
}

#endif