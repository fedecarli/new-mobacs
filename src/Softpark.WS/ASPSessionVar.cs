using Softpark.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

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

    public static class ASPSessionVar
    {
        private static Dictionary<string, string> _variables = new Dictionary<string, string>();

        public static string Read(string varName, bool fromCache = false)
        {
            if (fromCache && _variables.ContainsKey(varName.ToLower()))
                return _variables[varName.ToLower()];

            var db = DomainContainer.Current;

            var cfg = db?.SIGSM_ServicoSerializador_Config?.Find("sessionLocation");

            var uri = cfg?.Valor ?? "/sigsm/v2/SessionVar.asp";

            uri += $"?SessionVar={varName}";

            if (uri[0] == '/')
                uri = $"http://{HttpContext.Current.Request.Url.Host}{uri}";

            try
            {
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

                using (var s = client.OpenRead(uri))
                using (var sr = new StreamReader(s))
                {
                    data = sr.ReadToEnd();
                }

                return data;
            }
            catch (Exception e)
            {
                throw new Exception($"Erro ao tentar buscar a sessão em: {uri}.", e);
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}