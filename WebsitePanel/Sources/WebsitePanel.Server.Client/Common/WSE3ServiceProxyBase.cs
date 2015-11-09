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

	public class WSE3ServiceProxyBase : WebServicesClientProtocol, IServiceProxy {
		bool IServiceProxy.AllowAutoRedirect { get { return this.AllowAutoRedirect; } set { this.AllowAutoRedirect = value; } }
		string IServiceProxy.ConnectionGroupName { get { return this.ConnectionGroupName; } set { this.ConnectionGroupName = value; } }
		CookieContainer IServiceProxy.CookieContainer { get { return this.CookieContainer; } set { this.CookieContainer = value; } }
		ICredentials IServiceProxy.Credentials { get { return this.Credentials; } set { this.Credentials = value; } }
		bool IServiceProxy.EnableDecompression { get { return this.EnableDecompression; } set { this.EnableDecompression = value; } }
		bool IServiceProxy.PreAuthenticate { get { return this.PreAuthenticate; } set { this.PreAuthenticate = value; } }
		IWebProxy IServiceProxy.Proxy { get { return this.Proxy; } set { this.Proxy = value; } }
		Encoding IServiceProxy.RequestEncoding { get { return this.RequestEncoding; } set { this.RequestEncoding = value; } }
		bool IServiceProxy.RequireMtom { get { return this.RequireMtom; } set { this.RequireMtom = value; } }
		SoapProtocolVersion IServiceProxy.SoapVersion { get { return this.SoapVersion; } set { this.SoapVersion = value; } }
		int IServiceProxy.Timeout { get { return this.Timeout; } set { this.Timeout = value; } }
		bool IServiceProxy.UnsafeAuthenticatedConnectionSharing { get { return this.UnsafeAuthenticatedConnectionSharing; } set { this.UnsafeAuthenticatedConnectionSharing = value; } }
		string IServiceProxy.Url { get { return this.Url; } set { this.Url = value; } }
		bool IServiceProxy.UseDefaultCredentials { get { return this.UseDefaultCredentials; } set { this.UseDefaultCredentials = value; } }
		string IServiceProxy.UserAgent { get { return this.UserAgent; } set { this.UserAgent = value; } }
		void IServiceProxy.Abort() { this.Abort(); }
		IAsyncResult IServiceProxy.BeginInvoke(string methodName, object[] parameters, AsyncCallback callback, object asyncState) {
			return this.BeginInvoke(methodName, parameters, callback, asyncState);
		}
		void IServiceProxy.CancelAsync(object userState) { this.CancelAsync(userState); }
		void IServiceProxy.Discover() { this.Discover(); }
		object[] IServiceProxy.EndInvoke(IAsyncResult asyncResult) => this.EndInvoke(asyncResult);
		XmlReader IServiceProxy.GetReaderForMessage(SoapClientMessage message, int bufferSize) => this.GetReaderForMessage(message, bufferSize);
		WebRequest IServiceProxy.GetWebRequest(Uri uri) => this.GetWebRequest(uri);
		WebResponse IServiceProxy.GetWebResponse(WebRequest request) => this.GetWebResponse(request);
		XmlWriter IServiceProxy.GetWriterForMessage(SoapClientMessage message, int bufferSize) => this.GetWriterForMessage(message, bufferSize);
		object[] IServiceProxy.Invoke(string methodName, object[] parameters) => this.Invoke(methodName, parameters);
		void IServiceProxy.InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback) { this.InvokeAsync(methodName, parameters, callback); }
		void IServiceProxy.InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback, object userState) { this.InvokeAsync(methodName, parameters, callback, userState); }
	}

}

#endif