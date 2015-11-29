﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace WebsitePanel.Server.Client {

	public struct ServerInfo {
		public string Url;
		public bool SupportsWSE;
		public string PublicKey;

		public static ServerInfoCache Cache = new ServerInfoCache();
	}

	public class ServerInfoCache: KeyedCollection<string, ServerInfo>  {
		protected override string GetKeyForItem(ServerInfo item) => item.Url;

		const string CacheFileName = "~/App_Data/ServerInfoCache.data";
		static string CacheFile => HttpContext.Current.Server.MapPath(CacheFileName);

		public new ServerInfo this[string url] {
			get {
				if (!Contains(url)) {
					var info = new ServerInfo() { Url = url, SupportsWSE = true, PublicKey = null };
					var ad = new WebsitePanel.AutoDiscovery.AutoDiscovery();
					ad.Url = url + ad.Url.Substring(ad.Url.LastIndexOf('/'));
					ad.Timeout = 10000;
					info.SupportsWSE = ad.SupportsWSE();
					info.PublicKey = ad.EncryptionPublicKey();
					Add(info);
					Save();
				}
				return base[url];
			}
		}

		static void Load() {
			ServerInfo.Cache.Clear();
			if (File.Exists(CacheFile)) {
				using (var r = new BinaryReader(new FileStream(CacheFile, FileMode.Open, FileAccess.Read))) {
					while (r.BaseStream.CanRead) {
						var info = new ServerInfo();
						info.Url = r.ReadString();
						info.PublicKey = r.ReadString();
						info.SupportsWSE = r.ReadBoolean();
						ServerInfo.Cache.Add(info);
					}
				}
			}
		}
	
		public void Save() {
			if (!File.Exists(CacheFile)) {
				var dir = Path.GetDirectoryName(CacheFile);
				if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
			}
			using (var w = new BinaryWriter(new FileStream(CacheFile, FileMode.Create, FileAccess.Write))) {
				foreach (var info in ServerInfo.Cache) {
					w.Write(info.Url);
					w.Write(info.PublicKey);
					w.Write(info.SupportsWSE);
				}
			}
		}
	}
}
