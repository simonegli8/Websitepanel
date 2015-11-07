using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WebsitePanel.Server.Client.Common {


	public class ServerProxyBase<W, N> where W: Microsoft.Web.Services3.WebServicesClientProtocol, N:  {

		public static Dictionary<string, bool> UseWSEList = new Dictionary<string, bool>();

		static ServerProxyBase() {
			using (var r = new BinaryReader(new FileStream(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "WSPServerClient"), "ServerWSE.data"), FileMode.OpenOrCreate))) {
				while (r.BaseStream.CanRead) {
					UseWSEList.Add(r.ReadString(), r.ReadBoolean());
				}
			}
		}

		private static bool UseWSE(string url) {
			bool use;
			if (UseWSEList.TryGetValue(url, out use)) return use;
			else {
				var web = new AutoDiscovery.AutoDiscovery();
            web.Url = url;
				var version = web.EndGetServerVersion();
				if (version > "2.1.0.938") {
					use = web.SupportsWSE();
				} else use = false;

				UseWSEList.Add(url, use);
				using (var w = new BinaryWriter(new FileStream(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "WSPServerClient"), "ServerWSE.data"), FileMode.Create))) {
					foreach (var k in UseWSEList) {
						w.Write(k.Key); w.Write(k.Value);
					}
				}
				return use;
			}
   	}

		Microsoft.Web.Services3.WebServicesClientProtocol
	}

	public class ServerProxyWSEBase<T>: Microsoft.Web.Services3.WebServicesClientProtocol {
	}

}
