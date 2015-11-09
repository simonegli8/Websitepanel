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
using System.Linq;
using System.Configuration;
using WebsitePanel.Providers;
using WebsitePanel.Providers.Common;
using WebsitePanel.Server.Utils;
using WebsitePanel.Providers.OS;
using System.Reflection;

namespace WebsitePanel.Server.Code {

	public class AutoDiscoveryHelper {
		public const string DisableAutoDiscovery = "DisableAutoDiscovery";
		public const string IsRemoteProxy = "IsRemoteProxy";
		public static BoolResult IsInstalled(string name) {

			Log.WriteStart("IsInstalled started. Name:{0}", name);
			BoolResult res = new BoolResult { IsSuccess = true };

			try {

				bool isRemoteProxy;

				if (bool.TryParse(ConfigurationManager.AppSettings[IsRemoteProxy], out isRemoteProxy)) isRemoteProxy = false;

				bool disableAutoDiscovery;

				if (!bool.TryParse(ConfigurationManager.AppSettings[DisableAutoDiscovery], out disableAutoDiscovery))
					disableAutoDiscovery = false;

				if (disableAutoDiscovery) {
					res.Value = true;
				} else {
					if (string.IsNullOrEmpty(name)) {
						res.IsSuccess = false;
						res.ErrorCodes.Add(ErrorCodes.PROVIDER_NANE_IS_NOT_SPECIFIED);
						return res;
					}
					Type providerType = Type.GetType(name);
					isRemoteProxy &= providerType.GetCustomAttributes(typeof(RemoteProxyAttribute), false).Any();
					var isInstalled = providerType.GetMethod("IsInstalled");
					if (!(isInstalled.GetCustomAttributes(typeof(NoAutodiscoveryAttribute), false).Any())) {
						IHostingServiceProvider provider = (IHostingServiceProvider)Activator.CreateInstance(providerType);
						res.Value = provider.IsInstalled();
					} else {
						res.Value = isRemoteProxy;
					}
				}
			} catch (Exception ex) {
				res.IsSuccess = true;
				res.ErrorCodes.Add(ErrorCodes.CANNOT_CREATE_PROVIDER_INSTANCE);
				res.Value = false;
				Log.WriteError(ex);
			} finally {
				Log.WriteEnd("IsInstalled ended. Name:{0}", name);
			}

			return res;
		}

		public static string GetServerVersion() {
			object[] attrs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), true);
			if (attrs.Length > 0)
				return ((AssemblyFileVersionAttribute)attrs[0]).Version;
			else
				return typeof(AutoDiscoveryHelper).Assembly.GetName().Version.ToString(3);
		}

		public static Runtimes Runtime() { return OS.Runtime; }
		public static Platforms Platform() { return OS.Platform; }
		public static bool SupportsWSE() { return OS.Runtime == Runtimes.Net; }

	}
}

