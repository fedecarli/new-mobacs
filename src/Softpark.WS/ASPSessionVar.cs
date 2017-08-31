using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Routing;

namespace Softpark.WS
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class WebServiceClient : WebClient
    {
        private readonly CookieContainer _container = new CookieContainer();

        public CookieContainer Cookies { get { return _container; } }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request is HttpWebRequest webRequest)
            {
                //webRequest.CookieContainer = _container;
            }
            return request;
        }
    }

    public class ASPSessionVar
    {
        private UrlHelper _helper;

        private static Dictionary<string, string> _vars = new Dictionary<string, string>();
        private System.Web.Mvc.UrlHelper url;

        public ASPSessionVar(UrlHelper helper)
        {
            _helper = helper;
        }

        public ASPSessionVar(System.Web.Mvc.UrlHelper url)
        {
            this.url = url;
        }

        public static string Read(string varName, UrlHelper helper)
        {
            return (new ASPSessionVar(helper).Read(varName));
        }

        public static string Read(string varName, System.Web.Mvc.UrlHelper url)
        {
            return (new ASPSessionVar(url).Read(varName));
        }

        public string Read(string varName)
        {
            if (_vars.ContainsKey(varName)) return _vars[varName];

            var uri = "";

            if (_helper != null)
            {
                uri = _helper.Content($"~/../../SessionVar.asp?SessionVar={varName}");
            }
            else if (url != null)
            {
                uri = url.Content($"~/../../SessionVar.asp?SessionVar={varName}");
            }

            var client = new WebServiceClient();

            for (int i = 0; i < HttpContext.Current.Request.Headers.Keys.Count; i++)
            {
                var key = HttpContext.Current.Request.Headers.Keys[i];

                if ((new[] { "Cookie", "Host", "User-Agent" }).Contains(key))
                {
                    var value = HttpContext.Current.Request.Headers[key];

                    client.Headers.Add(key, value);
                }
            }

            string data = null;

            try
            {
                using (var s = client.OpenRead(uri))
                using (var sr = new StreamReader(s))
                {
                    data = sr.ReadToEnd();
                }

                _vars[varName] = data;

                return data;
            }
            catch (Exception e)
            {
                throw e;

            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}