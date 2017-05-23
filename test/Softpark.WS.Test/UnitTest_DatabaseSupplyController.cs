using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Rest;
using System.Threading.Tasks;

namespace Softpark.WS.Test
{
    [TestClass]
    public class UnitTest_DatabaseSupplyController
    {
        private DatabaseSupply _client;

        public UnitTest_DatabaseSupplyController()
        {
            ITokenProvider provider = new StringTokenProvider("", "Bearer");
            ServiceClientCredentials credentials = new TokenCredentials(provider);
            TestClient client = new TestClient(credentials, new RetryDelegatingHandler());
            _client = new DatabaseSupply(client);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpOperationException), AllowDerivedTypes = true)]
        public async Task TestMethod_GetEntities()
        {
            try
            {
                var result = await _client.GetEntitiesAsync("teste");

                throw new AssertFailedException("Value not expected.");
            }
            catch(HttpOperationException e)
            {
                if (e.Response.Content.Contains("O modelo solicitado é inválido.\r\nParameter name: modelo"))
                    throw e;
                else
                    throw new AssertFailedException("Value not expected.", e);
            }
            catch(Exception e)
            {
                throw new AssertFailedException("Value not expected.", e);
            }
        }
    }
}
