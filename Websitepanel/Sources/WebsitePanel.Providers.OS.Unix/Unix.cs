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
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

using WebsitePanel.Server.Utils;
using WebsitePanel.Providers.Utils;
using WebsitePanel.Providers.DomainLookup;
using WebsitePanel.Providers.DNS;
using System.Text.RegularExpressions;
using System.Linq;

namespace WebsitePanel.Providers.OS
{
    public class Unix: HostingServiceProviderBase, IOperatingSystem
    {

        #region Properties
			//TODO
        protected string UsersHome
        {
            get { return FileUtils.EvaluateSystemVariables(ProviderSettings["UsersHome"]); }
        }
        #endregion

        #region Files
        public virtual string CreatePackageFolder(string initialPath)
        {
            return FileUtils.CreatePackageFolder(initialPath);
        }

        public virtual bool FileExists(string path)
        {
            return FileUtils.FileExists(path);
        }

        public virtual bool DirectoryExists(string path)
        {
            return FileUtils.DirectoryExists(path);
        }

        public virtual SystemFile GetFile(string path)
        {
            return FileUtils.GetFile(path);
        }

        public virtual SystemFile[] GetFiles(string path)
        {
            return FileUtils.GetFiles(path);
        }

        public virtual SystemFile[] GetDirectoriesRecursive(string rootFolder, string path)
        {
            return FileUtils.GetDirectoriesRecursive(rootFolder, path);
        }

        public virtual SystemFile[] GetFilesRecursive(string rootFolder, string path)
        {
            return FileUtils.GetFilesRecursive(rootFolder, path);
        }

        public virtual SystemFile[] GetFilesRecursiveByPattern(string rootFolder, string path, string pattern)
        {
            return FileUtils.GetFilesRecursiveByPattern(rootFolder, path, pattern);
        }

        public virtual byte[] GetFileBinaryContent(string path)
        {
            return FileUtils.GetFileBinaryContent(path);
        }

		public virtual byte[] GetFileBinaryContentUsingEncoding(string path, string encoding)
		{
			return FileUtils.GetFileBinaryContent(path, encoding);
		}

        public virtual byte[] GetFileBinaryChunk(string path, int offset, int length)
        {
            return FileUtils.GetFileBinaryChunk(path, offset, length);
        }

        public virtual string GetFileTextContent(string path)
        {
            return FileUtils.GetFileTextContent(path);
        }

        public virtual void CreateFile(string path)
        {
            FileUtils.CreateFile(path);
        }

        public virtual void CreateDirectory(string path)
        {
            FileUtils.CreateDirectory(path);
        }

        public virtual void ChangeFileAttributes(string path, DateTime createdTime, DateTime changedTime)
        {
            FileUtils.ChangeFileAttributes(path, createdTime, changedTime);
        }

        public virtual void DeleteFile(string path)
        {
            FileUtils.DeleteFile(path);
        }

        public virtual void DeleteFiles(string[] files)
        {
            FileUtils.DeleteFiles(files);
        }

        public virtual void DeleteEmptyDirectories(string[] directories)
        {
            FileUtils.DeleteEmptyDirectories(directories);
        }

		public virtual void UpdateFileBinaryContent(string path, byte[] content)
		{
			FileUtils.UpdateFileBinaryContent(path, content);
		}

        public virtual void UpdateFileBinaryContentUsingEncoding(string path, byte[] content, string encoding)
        {
            FileUtils.UpdateFileBinaryContent(path, content, encoding);
        }

        public virtual void AppendFileBinaryContent(string path, byte[] chunk)
        {
            FileUtils.AppendFileBinaryContent(path, chunk);
        }

        public virtual void UpdateFileTextContent(string path, string content)
        {
            FileUtils.UpdateFileTextContent(path, content);
        }

        public virtual void MoveFile(string sourcePath, string destinationPath)
        {
            FileUtils.MoveFile(sourcePath, destinationPath);
        }

        public virtual void CopyFile(string sourcePath, string destinationPath)
        {
            FileUtils.CopyFile(sourcePath, destinationPath);
        }

        public virtual void ZipFiles(string zipFile, string rootPath, string[] files)
        {
            FileUtils.ZipFiles(zipFile, rootPath, files);
        }

        public virtual string[] UnzipFiles(string zipFile, string destFolder)
        {
            return FileUtils.UnzipFiles(zipFile, destFolder);
        }

        public virtual void CreateAccessDatabase(string databasePath)
        {
            FileUtils.CreateAccessDatabase(databasePath);
        }

        public UserPermission[] GetGroupNtfsPermissions(string path, UserPermission[] users, string usersOU)
        {	//TODO
            return SecurityUtils.GetGroupNtfsPermissions(path, users, ServerSettings, usersOU, null);
        }

        public void GrantGroupNtfsPermissions(string path, UserPermission[] users, string usersOU, bool resetChildPermissions)
        {	//TODO
            SecurityUtils.GrantGroupNtfsPermissions(path, users, resetChildPermissions, ServerSettings, usersOU, null);
        }

        public virtual void SetQuotaLimitOnFolder(string folderPath, string shareNameDrive, QuotaType quotaType, string quotaLimit, int mode, string wmiUserName, string wmiPassword)
        {	//TODO
            FileUtils.SetQuotaLimitOnFolder(folderPath, shareNameDrive, quotaLimit, mode, wmiUserName, wmiPassword);
        }

        public virtual Quota GetQuotaOnFolder(string folderPath, string wmiUserName, string wmiPassword)
        {	//TODO
            throw new NotImplementedException();
        }

        public virtual Dictionary<string, Quota> GetQuotasForOrganization(string folderPath, string wmiUserName, string wmiPassword)
        {	//TODO
            throw new NotImplementedException();
        }

        public virtual void DeleteDirectoryRecursive(string rootPath)
        {
            FileUtils.DeleteDirectoryRecursive(rootPath);
        }
        #endregion

        #region ODBC DSNs
        public virtual string[] GetInstalledOdbcDrivers()
        {
				throw new NotImplementedException();
		}

		public virtual string[] GetDSNNames()
      {
			throw new NotImplementedException();
		}

		public virtual SystemDSN GetDSN(string dsnName)
      {
			throw new NotImplementedException();
		}

		public virtual void CreateDSN(SystemDSN dsn)
      {
			throw new NotImplementedException();
      }

      public virtual void UpdateDSN(SystemDSN dsn)
      {
			throw new NotImplementedException();
		}

		public virtual void DeleteDSN(string dsnName)
      {
			throw new NotImplementedException();
		}
		#endregion

		#region Synchronizing
		public FolderGraph GetFolderGraph(string path)
        {
            if (!path.EndsWith("\\"))
                path += "\\";

            FolderGraph graph = new FolderGraph();
            graph.Hash = CalculateFileHash(path, path, graph.CheckSums);

            // copy hash to arrays
            graph.CheckSumKeys = new uint[graph.CheckSums.Count];
            graph.CheckSumValues = new FileHash[graph.CheckSums.Count];
            graph.CheckSums.Keys.CopyTo(graph.CheckSumKeys, 0);
            graph.CheckSums.Values.CopyTo(graph.CheckSumValues, 0);

            return graph;
        }

        public void ExecuteSyncActions(FileSyncAction[] actions)
        {
            // perform all operations but not delete ones
            foreach (FileSyncAction action in actions)
            {
                if (action.ActionType == SyncActionType.Create)
                {
                    FileUtils.CreateDirectory(action.DestPath);
                    continue;
                }
                else if (action.ActionType == SyncActionType.Copy)
                {
                    FileUtils.CopyFile(action.SrcPath, action.DestPath);
                }
                else if (action.ActionType == SyncActionType.Move)
                {
                    FileUtils.MoveFile(action.SrcPath, action.DestPath);
                }
            }

            // unzip file
            // ...after delete

            // delete files
            foreach (FileSyncAction action in actions)
            {
                if (action.ActionType == SyncActionType.Delete)
                {
                    FileUtils.DeleteFile(action.DestPath);
                }
            }
        }

        private FileHash CalculateFileHash(string rootFolder, string path, Dictionary<uint, FileHash> checkSums)
        {
            CRC32 crc32 = new CRC32();

            // check if this is a folder
            if (Directory.Exists(path))
            {
                FileHash folder = new FileHash();
                folder.IsFolder = true;
                folder.Name = Path.GetFileName(path);
                folder.FullName = path.Substring(rootFolder.Length - 1);

                // process child folders and files
                List<string> childFiles = new List<string>();
                childFiles.AddRange(Directory.GetDirectories(path));
                childFiles.AddRange(Directory.GetFiles(path));

                foreach (string childFile in childFiles)
                {
                    FileHash childHash = CalculateFileHash(rootFolder, childFile, checkSums);
                    folder.Files.Add(childHash);

                    // check sum
                    folder.CheckSum += childHash.CheckSum;
                    folder.CheckSum += ConvertCheckSumToInt(crc32.ComputeHash(Encoding.UTF8.GetBytes(childHash.Name)));

                    //Debug.WriteLine(folder.CheckSum + " : " + folder.FullName);
                }

                // move list to array
                folder.FilesArray = folder.Files.ToArray();

                if (!checkSums.ContainsKey(folder.CheckSum))
                    checkSums.Add(folder.CheckSum, folder);

                return folder;
            }

            FileHash file = new FileHash();
            file.Name = Path.GetFileName(path);
            file.FullName = path.Substring(rootFolder.Length - 1);

            // calculate CRC32
			using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				file.CheckSum = ConvertCheckSumToInt(
					crc32.ComputeHash(fs));
			}

            if (!checkSums.ContainsKey(file.CheckSum))
                checkSums.Add(file.CheckSum, file);

            //Debug.WriteLine(file.CheckSum + " : " + file.FullName);

            return file;
        }

        private uint ConvertCheckSumToInt(byte[] sumBytes)
        {
            uint checkSum = (uint)sumBytes[0] << 24;
            checkSum |= (uint)sumBytes[1] << 16;
            checkSum |= (uint)sumBytes[2] << 8;
            checkSum |= (uint)sumBytes[3] << 0;
            return checkSum;
        }
        #endregion

        #region HostingServiceProvider methods
        public override string[] Install()
        {
            List<string> messages = new List<string>();

            // create folder if it not exists
            try
            {
                if (!FileUtils.DirectoryExists(UsersHome))
                {
                    FileUtils.CreateDirectory(UsersHome);
                }
            }
            catch (Exception ex)
            {
                messages.Add(String.Format("Folder '{0}' could not be created: {1}",
                    UsersHome, ex.Message));
            }
            return messages.ToArray();
        }

        public override void DeleteServiceItems(ServiceProviderItem[] items)
        {
            foreach (ServiceProviderItem item in items)
            {
                try
                {
                    if (item is HomeFolder)
                        // delete home folder
                        DeleteFile(item.Name);
                }
                catch (Exception ex)
                {
                    Log.WriteError(String.Format("Error deleting '{0}' {1}", item.Name, item.GetType().Name), ex);
                }
            }
        }

        public override ServiceProviderItemDiskSpace[] GetServiceItemsDiskSpace(ServiceProviderItem[] items)
        {
            List<ServiceProviderItemDiskSpace> itemsDiskspace = new List<ServiceProviderItemDiskSpace>();
            foreach (ServiceProviderItem item in items)
            {
                if (item is HomeFolder)
                {
                    try
                    {
                        string path = item.Name;

                        Log.WriteStart(String.Format("Calculating '{0}' folder size", path));

                        // calculate disk space
                        ServiceProviderItemDiskSpace diskspace = new ServiceProviderItemDiskSpace();
                        diskspace.ItemId = item.Id;
                        diskspace.DiskSpace = FileUtils.CalculateFolderSize(path);
                        itemsDiskspace.Add(diskspace);

                        Log.WriteEnd(String.Format("Calculating '{0}' folder size", path));
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError(ex);
                    }
                }
            }
            return itemsDiskspace.ToArray();
        }
        #endregion

        public override bool IsInstalled()
        {
            return WebsitePanel.Server.Utils.OS.IsLinux || WebsitePanel.Server.Utils.OS.IsMac;                        
        }

        public virtual bool CheckFileServicesInstallation()
        {
            return WebsitePanel.Server.Utils.OS.CheckFileServicesInstallation();

        }

        public virtual bool InstallFsrmService()
        {
            return true;
        }
    }
}
