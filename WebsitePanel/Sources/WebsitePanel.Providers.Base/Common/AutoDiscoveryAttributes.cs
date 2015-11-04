using System;

namespace WebsitePanel.Providers {

	[AttributeUsage(AttributeTargets.Method)]
	public class NoAutodiscoveryAttribute : Attribute { }
	[AttributeUsage(AttributeTargets.Class)]
	public class RemoteProxyAttribute : Attribute { }

}