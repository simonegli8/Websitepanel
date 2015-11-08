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

	interface IServiceProxy {

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


   public class ServiceProxyBase: SoapHttpClientProtocol, IServiceProxy {

		const string NoWSEPath = "/NoWSE";

#if Net
		WSE3ServiceProxyBase wse = null;
		Dictionary<string, bool> useWSE = null;
		readonly static string CacheFile = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "WebsitePanel"), "WSECache.data");

		void LoadCache() {
			useWSE = new Dictionary<string, bool>();
			var lines = File.ReadAllLines(CacheFile, Encoding.UTF8);
			foreach (var line in lines) {
				var use = line.Length > 0 && line[0] == '!';
				useWSE.Add(use ? line.Substring(1) : line, use);
			}
		}
		void SaveCache() {
			File.WriteAllLines(CacheFile, useWSE.AsEnumerable().Select(x => x.Value ? '!' + x.Key : x.Key).ToArray(), Encoding.UTF8);
		}

		bool UseWSE(string url) {
			if (useWSE == null) LoadCache();
			bool use;
			if (useWSE.TryGetValue(url, out use)) return use;
			var ad = new WebsitePanel.AutoDiscovery.AutoDiscovery();
			ad.Url = Url;
			useWSE[url] = use = ad.SupportsWSE();
			SaveCache();
			return use;
		}

		IServiceProxy Service => UseWSE(Url) ? (IServiceProxy)(wse ?? (wse = new WSE3ServiceProxyBase() { Url = this.Url })) : this;
#elif
		IServiceProxy Service => this;
#endif



		protected bool RequireMtom { get { return Service.RequireMtom; } set { Service.RequireMtom = value; } }
		new public string Url {
			get { return Service.Url; }
			set {
				this.Url = value;
#if Net
				wse = null;
#endif
				Service.Url = value;
			}
		}
		public override void Abort() { Service.Abort(); }
		//public override ObjRef CreateObjRef(Type requestedType) => Service.CreateObjRef(requestedType);
		//public override object InitializeLifetimeService() Service.InitializeLifetimeService();
		protected new IAsyncResult BeginInvoke(string methodName, Object[] parameters, AsyncCallback callback, Object asyncState) => Service.BeginInvoke(methodName, )
      }


		bool IServiceProxy.RequireMtom { get { return false; } set { } }
		string IServiceProxy.Url { get { return base.Url.EndsWith(NoWSEPath) ? base.Url.Substring(0, base.Url.Length - NoWSEPath.Length) : base.Url; } set { base.Url = value + NoWSEPath; } }
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
		object[] IServiceProxy.EndInvoke(IAsyncResult asyncResult) { return base.EndInvoke(asyncResult); }
		XmlReader IServiceProxy.GetReaderForMessage(SoapClientMessage message, int bufferSize) { return base.GetReaderForMessage(message, bufferSize); }
		WebRequest IServiceProxy.GetWebRequest(Uri uri) { return base.GetWebRequest(uri); }
		XmlWriter IServiceProxy.GetWriterForMessage(SoapClientMessage message, int bufferSize) { return base.GetWriterForMessage(message, bufferSize); }
		object[] IServiceProxy.Invoke(string methodName, object[] parameters) { return base.Invoke(methodName, parameters); }
		void IServiceProxy.InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback) { base.InvokeAsync(methodName, parameters, callback); }
		void IServiceProxy.InvokeAsync(string methodName, object[] parameters, SendOrPostCallback callback, object userState) { base.InvokeAsync(methodName, parameters, callback, userState); }
	}

}
