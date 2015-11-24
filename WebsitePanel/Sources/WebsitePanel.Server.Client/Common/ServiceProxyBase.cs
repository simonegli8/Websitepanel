using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Xml;
using System.Net;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Threading;
using System.IO;

namespace WebsitePanel.Server.Client.Common {

	public interface IServiceProxy {

		//public EndpointReference Destination { get; set; }
		//public Pipeline Pipeline { get; set; }
		//public SoapContext RequestSoapContext { get; }
		bool RequireMtom { get; set; }
		//public SoapContext ResponseSoapContext { get; }
		string Url { get; set; }
		bool UseDefaultCredentials { get; set; }

		//public TSecurityToken GetClientCredential<TSecurityToken>() where TSecurityToken : SecurityToken;
		//public TSecurityToken GetServiceCredential<TSecurityToken>() where TSecurityToken : SecurityToken;
		//public void SetClientCredential<TSecurityToken>(TSecurityToken clientToken) where TSecurityToken : SecurityToken;
		//public void SetPolicy(string policyName);
		//public void SetPolicy(Policy policy);
		//public void SetServiceCredential<TSecurityToken>(TSecurityToken serviceToken) where TSecurityToken : SecurityToken;
		//public XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize);
		//public WebRequest GetWebRequest(Uri uri);
		//public WebResponse GetWebResponse(WebRequest request);
		//public WebResponse GetWebResponse(WebRequest request, IAsyncResult result);
		//public XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize);
		SoapProtocolVersion SoapVersion { get; set; }
		void Discover();
		IAsyncResult BeginInvoke(string methodName, object[] parameters, AsyncCallback callback, object asyncState);
		object[] EndInvoke(IAsyncResult asyncResult);
		XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize);
		WebRequest GetWebRequest(Uri uri);
		WebResponse GetWebResponse(WebRequest request);
		XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize);
		object[] Invoke(string methodName, object[] parameters);
		void InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback);
		void InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback, object userState);
		bool AllowAutoRedirect { get; set; }
		//X509CertificateCollection ClientCertificates { get; }
		CookieContainer CookieContainer { get; set; }
		bool EnableDecompression { get; set; }
		IWebProxy Proxy { get; set; }
		bool UnsafeAuthenticatedConnectionSharing { get; set; }
		string UserAgent { get; set; }
		void CancelAsync(object userState);
		string ConnectionGroupName { get; set; }
		ICredentials Credentials { get; set; }
		bool PreAuthenticate { get; set; }
		Encoding RequestEncoding { get; set; }
		int Timeout { get; set; }
		void Abort();
	}

	// TODO when Invoke et al fails with a not found 404 error, check if the ServerSupportsWSE.cache is outdated.
	public class ServiceProxyBase : SoapHttpClientProtocol, IServiceProxy {

		const string NoWSEPath = "/NoWSE";

		string BaseUrl => base.Url.Substring(0, base.Url.LastIndexOf('/')).Substring(0, base.Url.LastIndexOf(NoWSEPath));

		string PublicKey => ServerInfo.Cache[BaseUrl].PublicKey;

		void RefreshCache() => ServerInfo.Cache.Remove(BaseUrl);

		bool UseWSE => (Type.GetType("Mono.Runtime") == null) && !base.Url.Contains(NoWSEPath) && ServerInfo.Cache[BaseUrl].SupportsWSE;

#if Net
		IServiceProxy wse = null;

		public IServiceProxy Service {
			get {
				if (!UseWSE) return this;
				if (wse != null) return wse;
				var type = GetType().FullName;
				var i = type.LastIndexOf('.');
				type = type.Substring(0, i) + ".WSE" + type.Substring(i);
				wse = (IServiceProxy)Activator.CreateInstance(Type.GetType(type));
				wse.Url = base.Url;
				wse.Timeout = base.Timeout;
				return wse;
			}
		}
#else
		public IServiceProxy Service => this;
#endif

		// Client Methods
		protected bool RequireMtom { get { return Service.RequireMtom; } set { Service.RequireMtom = value; } }
		public new string Url { get { return Service.Url; } set { base.Url = value; Service.Url = value; } }
		public new bool AllowAutoRedirect { get { return Service.AllowAutoRedirect; } set { Service.AllowAutoRedirect = value; } }
		public new string ConnectionGroupName { get { return Service.ConnectionGroupName; } set { Service.ConnectionGroupName = value; } }
		public new CookieContainer CookieContainer { get { return Service.CookieContainer; } set { Service.CookieContainer = value; } }
		public new ICredentials Credentials { get { return Service.Credentials; } set { Service.Credentials = value; } }
		public new bool EnableDecompression { get { return Service.EnableDecompression; } set { Service.EnableDecompression = value; } }
		public new bool PreAuthenticate { get { return Service.PreAuthenticate; } set { Service.PreAuthenticate = value; } }
		public new IWebProxy Proxy { get { return Service.Proxy; } set { Service.Proxy = value; } }
		public new Encoding RequestEncoding { get { return Service.RequestEncoding; } set { Service.RequestEncoding = value; } }
		public new SoapProtocolVersion SoapVersion { get { return Service.SoapVersion; } set { Service.SoapVersion = value; } }
		public new int Timeout { get { return Service.Timeout; } set { Service.Timeout = value; } }
		public new bool UnsafeAuthenticatedConnectionSharing { get { return Service.UnsafeAuthenticatedConnectionSharing; } set { Service.UnsafeAuthenticatedConnectionSharing = value; } }
		public new bool UseDefaultCredentials { get { return Service.UseDefaultCredentials; } set { Service.UseDefaultCredentials = value; } }
		public new string UserAgent { get { return Service.UserAgent; } set { Service.UserAgent = value; } }
		public new void Abort() { Service.Abort(); }
		public new IAsyncResult BeginInvoke(string methodName, object[] parameters, AsyncCallback callback, object asyncState) {
			try {
				return Service.BeginInvoke(methodName, parameters, callback, asyncState);
			} catch (WebException wex) when (wex.Status == WebExceptionStatus.ProtocolError && wex.Response != null && (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotFound)) {
				RefreshCache();
				return Service.BeginInvoke(methodName, parameters, callback, asyncState);
			} catch (SoapHeaderException shex) when (shex.Code.Name == "InvalidEncryptionKey") {
				RefreshCache();
				return Service.BeginInvoke(methodName, parameters, callback, asyncState);
			}
		}
		public new void CancelAsync(object userState) { Service.CancelAsync(userState); }
		public new void Discover() { Service.Discover(); }
		public new object[] EndInvoke(IAsyncResult asyncResult) {
			try {
				return Service.EndInvoke(asyncResult);
			} catch (WebException wex) when (wex.Status == WebExceptionStatus.ProtocolError && wex.Response != null && (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotFound)) {
				RefreshCache();
				return Service.EndInvoke(asyncResult);
			} catch (SoapHeaderException shex) when (shex.Code.Name == "InvalidEncryptionKey") {
				RefreshCache();
				return Service.EndInvoke(asyncResult);
			}
		}
		public new XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize) => Service.GetReaderForMessage(message, bufferSize);
		public new WebRequest GetWebRequest(Uri uri) => Service.GetWebRequest(uri);
		public new WebResponse GetWebResponse(WebRequest request) => Service.GetWebResponse(request);
		public new XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize) => Service.GetWriterForMessage(message, bufferSize);
		public new object[] Invoke(string methodName, object[] parameters) {
			try {
				return Service.Invoke(methodName, parameters);
			} catch (WebException wex) when (wex.Status == WebExceptionStatus.ProtocolError && wex.Response != null && (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotFound)) {
				RefreshCache();
				return Service.Invoke(methodName, parameters);
			} catch (SoapHeaderException shex) when (shex.Code.Name == "InvalidEncryptionKey") {
				RefreshCache();
				return Service.Invoke(methodName, parameters);
			}
		}

		public new void InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback) {
         try {
				Service.InvokeAsync(methodName, parameters, callback);
			} catch (WebException wex) when (wex.Status == WebExceptionStatus.ProtocolError && wex.Response != null && (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotFound)) {
				RefreshCache();
				Service.InvokeAsync(methodName, parameters, callback);
			} catch (SoapHeaderException shex) when (shex.Code.Name == "InvalidEncryptionKey") {
				RefreshCache();
				Service.InvokeAsync(methodName, parameters, callback);
			}
		}
		public new void InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback, object userState) {
         try {
				Service.InvokeAsync(methodName, parameters, callback, userState);
			} catch (WebException wex) when (wex.Status == WebExceptionStatus.ProtocolError && wex.Response != null && (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotFound)) {
				RefreshCache();
				Service.InvokeAsync(methodName, parameters, callback, userState);
			} catch (SoapHeaderException shex) when (shex.Code.Name == "InvalidEncryptionKey") {
				RefreshCache();
				Service.InvokeAsync(methodName, parameters, callback, userState);
			}
		}

		// IServiceProxy
		bool IServiceProxy.RequireMtom { get { return false; } set { } }
		string IServiceProxy.Url {
			get { return base.Url; }
			set {
				if (!value.Contains(NoWSEPath)) {
					var i = value.LastIndexOf('/');
					base.Url = value.Substring(0, i) + NoWSEPath + value.Substring(i);
				} else base.Url = value;
			}
		}
		bool IServiceProxy.AllowAutoRedirect { get { return base.AllowAutoRedirect; } set { base.AllowAutoRedirect = value; } }
		string IServiceProxy.ConnectionGroupName { get { return base.ConnectionGroupName; } set { base.ConnectionGroupName = value; } }
		CookieContainer IServiceProxy.CookieContainer { get { return base.CookieContainer; } set { base.CookieContainer = value; } }
		ICredentials IServiceProxy.Credentials { get { return base.Credentials; } set { base.Credentials = value; } }
		bool IServiceProxy.EnableDecompression { get { return base.EnableDecompression; } set { base.EnableDecompression = value; } }
		bool IServiceProxy.PreAuthenticate { get { return base.PreAuthenticate; } set { base.PreAuthenticate = value; } }
		IWebProxy IServiceProxy.Proxy { get { return base.Proxy; } set { base.Proxy = value; } }
		Encoding IServiceProxy.RequestEncoding { get { return base.RequestEncoding; } set { base.RequestEncoding = value; } }
		SoapProtocolVersion IServiceProxy.SoapVersion { get { return base.SoapVersion; } set { base.SoapVersion = value; } }
		int IServiceProxy.Timeout { get { return base.Timeout; } set { base.Timeout = value; } }
		bool IServiceProxy.UnsafeAuthenticatedConnectionSharing { get { return base.UnsafeAuthenticatedConnectionSharing; } set { base.UnsafeAuthenticatedConnectionSharing = value; } }
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
		object[] IServiceProxy.Invoke(string methodName, object[] parameters) {
			return base.Invoke(methodName, parameters);
		}
		void IServiceProxy.InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback) {
			base.InvokeAsync(methodName, parameters, callback);
		}
		void IServiceProxy.InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback, object userState) {
			base.InvokeAsync(methodName, parameters, callback, userState);
		}
	}

}
