#if Net

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Services.Protocols;
using System.Xml;
using Microsoft.Web.Services3;

namespace WebsitePanel.Server.Client.Common {

	public class SystemWebServiceProxyBase: WebServicesClientProtocol, IServiceProxyBase {
		bool IServiceProxyBase.AllowAutoRedirect { get { return base.AllowAutoRedirect; } set { base.AllowAutoRedirect = value; } }
		string IServiceProxyBase.ConnectionGroupName { get { return base.ConnectionGroupName; } set { base.ConnectionGroupName = value; } }
		CookieContainer IServiceProxyBase.CookieContainer { get { return base.CookieContainer; } set { base.CookieContainer = value; } }
		ICredentials IServiceProxyBase.Credentials { get { return base.Credentials; } set { base.Credentials = value; } }
		bool IServiceProxyBase.EnableDecompression { get { return base.EnableDecompression; } set { base.EnableDecompression = value; } }
		bool IServiceProxyBase.PreAuthenticate { get { return base.PreAuthenticate; } set { base.PreAuthenticate = value; } }
		IWebProxy IServiceProxyBase.Proxy { get { return base.Proxy; } set { base.Proxy = value; } }
		Encoding IServiceProxyBase.RequestEncoding { get { return base.RequestEncoding; } set { base.RequestEncoding = value; } }
		bool IServiceProxyBase.RequireMtom { get { return base.RequireMtom; } set { base.RequireMtom = value; } }
		SoapProtocolVersion IServiceProxyBase.SoapVersion { get { return base.SoapVersion; } set { base.SoapVersion = value; } }
		int IServiceProxyBase.Timeout { get { return base.Timeout; } set { base.Timeout = value; } }
		bool IServiceProxyBase.UnsafeAuthenticatedConnectionSharing { get { return base.UnsafeAuthenticatedConnectionSharing; } set { base.UnsafeAuthenticatedConnectionSharing = value; } }
		string IServiceProxyBase.Url { get { return base.Url; } set { base.Url = value; } }
		bool IServiceProxyBase.UseDefaultCredentials { get { return base.UseDefaultCredentials; } set { base.UseDefaultCredentials = value; } }
		string IServiceProxyBase.UserAgent { get { return base.UserAgent; } set { base.UserAgent = value; } }
		void IServiceProxyBase.Abort() { base.Abort(); }
		IAsyncResult IServiceProxyBase.BeginInvoke(string methodName, object[] parameters, AsyncCallback callback, object asyncState) {
			return base.BeginInvoke(methodName, parameters, callback, asyncState);
		}
		void IServiceProxyBase.CancelAsync(object userState) { base.CancelAsync(userState); }
		void IServiceProxyBase.Discover() { base.Discover(); }
		object[] IServiceProxyBase.EndInvoke(IAsyncResult asyncResult) { return base.EndInvoke(asyncResult); }
		XmlReader IServiceProxyBase.GetReaderForMessage(SoapClientMessage message, int bufferSize) { return base.GetReaderForMessage(message, bufferSize); }
		WebRequest IServiceProxyBase.GetWebRequest(Uri uri) { return base.GetWebRequest(uri); }
		XmlWriter IServiceProxyBase.GetWriterForMessage(SoapClientMessage message, int bufferSize) { return base.GetWriterForMessage(message, bufferSize); }
		object[] IServiceProxyBase.Invoke(string methodName, object[] parameters) { return base.Invoke(methodName, parameters); }
		void IServiceProxyBase.InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback) { base.InvokeAsync(methodName, parameters, callback); }
		void IServiceProxyBase.InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback, object userState) { base.InvokeAsync(methodName, parameters, callback, userState); }
	}

}

#endif