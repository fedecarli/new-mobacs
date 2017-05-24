using Softpark.Infrastructure.Extras;
using Softpark.Models;
using Softpark.WS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Softpark.WS.Controllers.Api
{
    /// <summary>
    /// Endpoints dos processos de importação de dados
    /// </summary>
    [Route("/api/processos")]
    [System.Web.Mvc.OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class ProcessosController : BaseApiController
    {
        /// <summary>
        /// Cadastrar cabeçalho das fichas
        /// </summary>
        /// <remarks>
        /// Todas as fichas usarão esse cabeçalho
        /// </remarks>
        /// <param name="header">Dados à serem enviados</param>
        /// <returns>Token para envio das fichas</returns>
        [Route("enviar/cabecalho")]
        [HttpPost, ResponseType(typeof(Guid))]
        public async Task<IHttpActionResult> EnviarCabecalho([FromBody, Required] UnicaLotacaoTransportCadastroViewModel header)
        {
            if (!ModelState.IsValid)
            {
                Log.Fatal(ModelState);
                return BadRequest(ModelState);
            }

            using (var Domain = Repository)
            {
                var origem = Domain.Create(c => c.OrigemVisita, async (db, o) =>
                {
                    return await Task.Run(() =>
                    {
                        o.token = Guid.NewGuid();
                        o.id_tipo_origem = 1;
                        o.enviarParaThrift = true;
                        o.enviado = false;

                        return o;
                    });
                });

                var headerTransport = Domain.Create(c => c.UnicaLotacaoTransport, async (c, ult) =>
                {
                    return await Task.Run(async () =>
                    {
                        var transport = header.ToModel(ref ult);

                        transport.id = Guid.NewGuid();
                        origem.Wait();
                        var o = await origem;
                        transport.OrigemVisita = o;
                        transport.token = o.token;

                        return transport;
                    });
                });

                await Task.WhenAll(origem, headerTransport);

                var cabecalho = await headerTransport;

                return Ok(cabecalho.token);
            }
        }

        /// <summary>
        /// Envia uma ficha de visita (child) para cadastro
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        [Route("enviar/visita/child")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> EnviarFichaVisita([FromBody, Required] FichaVisitaDomiciliarChildCadastroViewModel child)
        {
            if (!ModelState.IsValid)
            {
                Log.Fatal(ModelState);
                return BadRequest(ModelState);
            }

            using (var Domain = Repository)
            {   
                try
                {
                    await Domain.CreateFichasVisita(child.token ?? Guid.Empty, new[] { child }, true);

                    await Domain.SaveChanges();
                }
                catch (Exception e)
                {
                    var ex = ((System.Data.Entity.Validation.DbEntityValidationException)e).EntityValidationErrors;
                    throw new Exception(ex.First().ValidationErrors.First().ErrorMessage, e);
                }
            }

            return Ok(true);
        }

        /// <summary>
        /// Cadastro Individual
        /// </summary>
        /// <param name="cadInd"></param>
        /// <returns></returns>
        [Route("enviar/cadastro/individual")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> EnviarCadastroIndividual([FromBody, Required] CadastroIndividualViewModel cadInd)
        {
            Log.Info("-----");
            Log.Info("POST enviar/cadastro/individual");
            Log.Info($"TOKEN {cadInd.token}");

            if (!ModelState.IsValid)
            {
                Log.Fatal(ModelState);
                return BadRequest(ModelState);
            }

            using (var Domain = Repository)
            {
                try
                {
                    await Domain.CreateFichasIndividuais(cadInd.token, new[] { cadInd }, true);

                    await Domain.SaveChanges();
                }
                catch (Exception e)
                {
                    var ex = ((System.Data.Entity.Validation.DbEntityValidationException)e).EntityValidationErrors;
                    throw new Exception(ex.First().ValidationErrors.First().ErrorMessage, e);
                }
            }

            return Ok(true);
        }

        /// <summary>
        /// Cadastro Domiciliar
        /// </summary>
        /// <param name="cadDom"></param>
        /// <returns></returns>
        [Route("enviar/cadastro/domiciliar")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> EnviarCadastroDomiciliar([FromBody, Required] CadastroDomiciliarViewModel cadDom)
        {
            Log.Info("-----");
            Log.Info("POST enviar/cadastro/domiciliar");
            Log.Info($"TOKEN {cadDom.token}");

            if (!ModelState.IsValid)
            {
                Log.Fatal(ModelState);
                return BadRequest(ModelState);
            }

            using (var Domain = Repository)
            {
                try
                {
                    await Domain.CreateFichasDomiciliares(cadDom.token, new[] { cadDom }, true);

                    await Domain.SaveChanges();
                }
                catch (Exception e)
                {
                    var ex = ((System.Data.Entity.Validation.DbEntityValidationException)e).EntityValidationErrors;
                    throw new Exception(ex.First().ValidationErrors.First().ErrorMessage, e);
                }
            }

            return Ok(true);
        }

        /// <summary>
        /// Cadastro Atômico
        /// </summary>
        /// <param name="cadastros"></param>
        /// <returns></returns>
        [Route("api/processos/enviar/cadastro/atomico")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> CadastramentoAtomico([FromBody, Required] AtomicTransporViewModel[] cadastros)
        {
            Log.Info("-----");
            Log.Info("POST api/processos/enviar/cadastro/atomico");

            using (var Domain = Repository)
            {
                try
                {
                    var token = Guid.NewGuid();

                    var origem = Domain.Create(c => c.OrigemVisita, (c, o) => {
                        o.token = token;
                        o.id_tipo_origem = 1;
                        o.enviarParaThrift = true;

                        return Task.FromResult(o);
                    });

                    var tasks = new List<Task>();

                    foreach (var cadastro in cadastros)
                    {
                        var header = Domain.Create(c => c.UnicaLotacaoTransport, async (c, transport) =>
                        {
                            transport = cadastro.cabecalho.ToModel(ref transport);
                            origem.Wait();
                            var orig = await origem;
                            transport.OrigemVisita = orig;
                            transport.token = orig.token;
                            return transport;
                        });

                        tasks.Add(header);

                        var fichasResp = cadastro.individuos.Where(x => x.identificacaoUsuarioCidadao != null && x.identificacaoUsuarioCidadao.statusEhResponsavel);

                        var cadsResp = Domain.CreateFichasIndividuais(header, fichasResp);

                        tasks.Add(cadsResp);

                        var fichasNRes = cadastro.individuos.Where(x => x.identificacaoUsuarioCidadao == null || !x.identificacaoUsuarioCidadao.statusEhResponsavel);

                        var cadsNRes = Domain.CreateFichasIndividuais(header, fichasNRes);

                        tasks.Add(cadsNRes);

                        var cadsDoms = Domain.CreateFichasDomiciliares(header, cadastro.domicilios);

                        await Domain.CreateFichasVisita(header, cadastro.visitas, false, true);
                    }

                    await Domain.SaveChanges();
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    var ex = ((System.Data.Entity.Validation.DbEntityValidationException)e).EntityValidationErrors;
                    throw new Exception(ex.First().ValidationErrors.First().ErrorMessage, e);
                }
            }

            return Ok(true);
        }

        /// <summary>
        /// Finaliza a transmissão de dados (encerra o token)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("encerrar/{token}")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> FinalizarTransmissao([FromUri, Required] Guid token)
        {
            using (var Domain = Repository)
            {
                var origem = await Domain.GetModel(c => c.OrigemVisita.FindAsync(token));

                if (origem == null || origem.finalizado)
                {
                    throw new ValidationException("Token inválido. Inicie o processo de transmissão.");
                }
                else if (origem.UnicaLotacaoTransport.Sum(x => x.FichaVisitaDomiciliarMaster.Count + x.CadastroDomiciliar.Count + x.CadastroIndividual.Count) > 0)
                {
                    await Domain.Execute(c => c.PR_ProcessarFichasAPI(token));
                }
                else
                {
                    origem.finalizado = true;
                    await Domain.SaveChanges();
                }
            }

            return Ok(true);
        }
    }
}