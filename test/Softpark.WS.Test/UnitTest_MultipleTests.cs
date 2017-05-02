using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Softpark.Infrastructure.Extras;
using Softpark.Models;
using Softpark.WS.ViewModels;

namespace Softpark.WS.Test
{
    [TestClass]
    public class UnitTestMultipleTests
    {
        private static async Task<HttpResponseMessage> CreateHeader(VW_Profissional profissional)
        {
            var client = new HttpClient();

            var data = new UnicaLotacaoTransportCadastroViewModel
            {
                profissionalCNS = profissional.CNS.Trim(),
                dataAtendimento = DateTime.Now.ToUnix(),
                cboCodigo_2002 = profissional.CBO.Trim(),
                cnes = profissional.CNES.Trim(),
                codigoIbgeMunicipio = "3547304",
                ine = profissional.INE?.Trim()
            };

            var jsonObject = JsonConvert.SerializeObject(data);

            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

            return await client.PostAsync(new Uri("http://localhost:63272/enviar/cabecalho"), content);
        }

        [TestMethod]
        public async Task TestMethod_0001()
        {
            var profissional = DomainContainer.Current.VW_Profissional
                .Where(x => x.CNS != null).Skip(0).Take(1).First();
            
            var message = await  CreateHeader(profissional);

            Assert.IsTrue(message.IsSuccessStatusCode, await message.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task TestMethod_0002()
        {
            var profissional = DomainContainer.Current.VW_Profissional
                .Where(x => x.CNS != null).Skip(1).Take(1).FirstOrDefault();

            var message = await CreateHeader(profissional);

            Assert.IsTrue(message.IsSuccessStatusCode, await message.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task TestMethod_0003()
        {
            var profissional = DomainContainer.Current.VW_Profissional
                .Where(x => x.CNS != null).Skip(2).Take(1).FirstOrDefault();

            var message = await CreateHeader(profissional);

            Assert.IsTrue(message.IsSuccessStatusCode, await message.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task TestMethod_0004()
        {
            var profissional = DomainContainer.Current.VW_Profissional
                .Where(x => x.CNS != null).Skip(3).Take(1).FirstOrDefault();

            var message = await CreateHeader(profissional);

            Assert.IsTrue(message.IsSuccessStatusCode, await message.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task TestMethod_0005()
        {
            var profissional = DomainContainer.Current.VW_Profissional
                .Where(x => x.CNS != null).Skip(4).Take(1).FirstOrDefault();

            var message = await CreateHeader(profissional);

            Assert.IsTrue(message.IsSuccessStatusCode, await message.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task TestMethod_0006()
        {
            var profissional = DomainContainer.Current.VW_Profissional
                .Where(x => x.CNS != null).Skip(5).Take(1).FirstOrDefault();

            var message = await CreateHeader(profissional);

            Assert.IsTrue(message.IsSuccessStatusCode, await message.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task TestMethod_0007()
        {
            var profissional = DomainContainer.Current.VW_Profissional
                .Where(x => x.CNS != null).Skip(6).Take(1).FirstOrDefault();

            var message = await CreateHeader(profissional);

            Assert.IsTrue(message.IsSuccessStatusCode, await message.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task TestMethod_0008()
        {
            var profissional = DomainContainer.Current.VW_Profissional
                .Where(x => x.CNS != null).Skip(7).Take(1).FirstOrDefault();

            var message = await CreateHeader(profissional);

            Assert.IsTrue(message.IsSuccessStatusCode, await message.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task TestMethod_0009()
        {
            var profissional = DomainContainer.Current.VW_Profissional
                .Where(x => x.CNS != null).Skip(8).Take(1).FirstOrDefault();

            var message = await CreateHeader(profissional);

            Assert.IsTrue(message.IsSuccessStatusCode, await message.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task TestMethod_0010()
        {
            var profissional = DomainContainer.Current.VW_Profissional
                .Where(x => x.CNS != null).Skip(9).Take(1).FirstOrDefault();

            var message = await CreateHeader(profissional);

            Assert.IsTrue(message.IsSuccessStatusCode, await message.Content.ReadAsStringAsync());
        }
    }
}

