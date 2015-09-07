using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsitePanel.Utils;

namespace WebsitePanel.Providers.OS
{
	public class Linux : HostingServiceProviderBase, IOperatingSystem
	{
		public override bool IsInstalled ()
		{
			return true;
		}

		public bool FileExists (string path)
		{
			return FileUtils.FileExists (path);
		}

		public bool DirectoryExists (string path)
		{
			return FileUtils.DirectoryExists (path);
		}

		public SystemFile GetFile (string path)
		{
			return FileUtils.GetFile (path);
		}

		public SystemFile[] GetFiles (string path)
		{
			return FileUtils.GetFiles (path);
		}

		public SystemFile[] GetDirectoriesRecursive (string rootFolder, string path)
		{
			return FileUtils.GetDirectoriesRecursive (rootFolder, path);
		}

		public SystemFile[] GetFilesRecursive (string rootFolder, string path)
		{
			return FileUtils.GetFilesRecursive (rootFolder, path);
		}

		public SystemFile[] GetFilesRecursiveByPattern (string rootFolder, string path, string pattern)
		{
			return FileUtils.GetFilesRecursiveByPattern (rootFolder, path, pattern);
		}
	}
}
