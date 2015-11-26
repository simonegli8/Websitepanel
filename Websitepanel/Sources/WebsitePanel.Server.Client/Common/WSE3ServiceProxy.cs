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

namespace WebsitePanel.Server.Client {

	public class WSE3ServiceProxy : WebServicesClientProtocol, IServiceProxy {
		bool IServiceProxy.AllowAutoRedirect { get { return base.AllowAutoRedirect; } set { base.AllowAutoRedirect = value; } }
		string IServiceProxy.ConnectionGroupName { get { return base.ConnectionGroupName; } set { base.ConnectionGroupName = value; } }
		CookieContainer IServiceProxy.CookieContainer { get { return base.CookieContainer; } set { base.CookieContainer = value; } }
		ICredentials IServiceProxy.Credentials { get { return base.Credentials; } set { base.Credentials = value; } }
		bool IServiceProxy.EnableDecompression { get { return base.EnableDecompression; } set { base.EnableDecompression = value; } }
		bool IServiceProxy.PreAuthenticate { get { return base.PreAuthenticate; } set { base.PreAuthenticate = value; } }
		IWebProxy IServiceProxy.Proxy { get { return base.Proxy; } set { base.Proxy = value; } }
		Encoding IServiceProxy.RequestEncoding { get { return base.RequestEncoding; } set { base.RequestEncoding = value; } }
		bool IServiceProxy.RequireMtom { get { return base.RequireMtom; } set { base.RequireMtom = value; } }
		SoapProtocolVersion IServiceProxy.SoapVersion { get { return base.SoapVersion; } set { base.SoapVersion = value; } }
		int IServiceProxy.Timeout { get { return base.Timeout; } set { base.Timeout = value; } }
		bool IServiceProxy.UnsafeAuthenticatedConnectionSharing { get { return base.UnsafeAuthenticatedConnectionSharing; } set { base.UnsafeAuthenticatedConnectionSharing = value; } }
		string IServiceProxy.Url { get { return base.Url; } set { base.Url = value; } }
		bool IServiceProxy.UseDefaultCredentials { get { return base.UseDefaultCredentials; } set { base.UseDefaultCredentials = value; } }
		string IServiceProxy.UserAgent { get { return base.UserAgent; } set { base.UserAgent = value; } }
		void IServiceProxy.Abort() { base.Abort(); }
		IAsyncResult IServiceProxy.BeginInvoke(string methodName, object[] parameters, AsyncCallback callback, object asyncState) {
			return base.BeginInvoke(methodName, parameters, callback, asyncState);
		}
		void IServiceProxy.CancelAsync(object userState) { base.CancelAsync(userState); }
		void IServiceProxy.Discover() { base.Discover(); }
		object[] IServiceProxy.EndInvoke(IAsyncResult asyncResult) => base.EndInvoke(asyncResult);
		XmlReader IServiceProxy.GetReaderForMessage(SoapClientMessage message, int bufferSize) => base.GetReaderForMessage(message, bufferSize);
		WebRequest IServiceProxy.GetWebRequest(Uri uri) => base.GetWebRequest(uri);
		WebResponse IServiceProxy.GetWebResponse(WebRequest request) => base.GetWebResponse(request);
		XmlWriter IServiceProxy.GetWriterForMessage(SoapClientMessage message, int bufferSize) => base.GetWriterForMessage(message, bufferSize);
		object[] IServiceProxy.Invoke(string methodName, object[] parameters) => base.Invoke(methodName, parameters);
		void IServiceProxy.InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback) { base.InvokeAsync(methodName, parameters, callback); }
		void IServiceProxy.InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback, object userState) { base.InvokeAsync(methodName, parameters, callback, userState); }
	}

}

#endif