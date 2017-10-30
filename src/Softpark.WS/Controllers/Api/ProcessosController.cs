using Softpark.Infrastructure.Extras;
using Softpark.Models;
using Softpark.WS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;

namespace Softpark.WS.Controllers.Api
{
    /// <summary>
    /// Endpoints dos processos de transmissão de dados provenientes do APP
    /// </summary>
    [Route("/api/processos")]
    [System.Web.Mvc.OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class ProcessosController : BaseApiController
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        /// <summary>
        /// Este construtor é usado para o sistema de documentação poder gerar o swagger e o Help
        /// </summary>
        protected ProcessosController() : base(new DomainContainer()) { }

        /// <summary>
        /// Este construtor é inicializado pelo asp.net usando injeção de dependência
        /// </summary>
        /// <param name="domain">Domínio do banco inicializado por injeção de dependência</param>
        public ProcessosController(DomainContainer domain) : base(domain)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
        }

        /// <summary>
        /// Propriedade de registro do log4net
        /// </summary>
        private static log4net.ILog Log { get; set; } = log4net.LogManager.GetLogger(typeof(ProcessosController));

        /// <summary>
        /// Este método busca uma FichaVisitaDomiciliarMaster com menos de 99 registros com o mesmo cabeçalho ou cria uma se não encontrar
        /// </summary>
        /// <param name="token">Token de origem</param>
        /// <returns></returns>
        private async Task<FichaVisitaDomiciliarMaster> GetOrCreateMaster(Guid token)
        {
            // busca o token
            var origem = await Domain.OrigemVisita.FindAsync(token);

            if (origem == null || origem.finalizado)
            {
                throw new InvalidOperationException("Token inválido. Inicie o processo de transmissão.");
            }

            // busca um cabeçalho no token
            var header = await Domain.UnicaLotacaoTransport.FirstOrDefaultAsync(h => h.token == token);

            if (header == null)
            {
                throw new InvalidOperationException("Token inválido. Inicie o processo de transmissão.");
            }

            // busca ou cria uma ficha
            var master = header.FichaVisitaDomiciliarMaster.FirstOrDefault(m => m.FichaVisitaDomiciliarChild.Count < 99) ??
                         Domain.FichaVisitaDomiciliarMaster.Create();

            // verifica se é uma nova ficha, se não, retorna a encontrada
            if (master.uuidFicha != null) return master;

            // o tipo de origem do cds é sempre 3 (sistema externo)
            master.tpCdsOrigem = 3;
            // atrela o cabeçalho à ficha
            master.UnicaLotacaoTransport = header;

            // gera o ID da ficha master
            master.uuidFicha = header.cnes + '-' + Guid.NewGuid();
            // adiciona a ficha ao banco de dados
            Domain.FichaVisitaDomiciliarMaster.Add(master);

            // salva a transação do banco de dados
            await Domain.SaveChangesAsync();

            // retorna a ficha
            return master;
        }

        /// <summary>
        /// Endpoint para registro do cabeçalho para os cadastros não atômicos
        /// </summary>
        /// <remarks>
        /// Todas as fichas usarão esse cabeçalho
        /// </remarks>
        /// <param name="header">ViewModel com os dados do cabeçalho</param>
        /// <returns>Token para envio das fichas não atômicas</returns>
        [Route("enviar/cabecalho")]
        [Route("api/processos/enviar/cabecalho")]
        [HttpPost, ResponseType(typeof(Guid))]
        public async Task<IHttpActionResult> EnviarCabecalho([FromBody, Required] UnicaLotacaoTransportCadastroViewModel header)
        {
            #region LOG4NET
            // registra no l4n a chamada do endpoint
            Log.Info("----");
            Log.Info("enviar/cabecalho");
            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(header));
            #endregion

            // cria um novo token
            var origem = Domain.OrigemVisita.Create();

            origem.token = Guid.NewGuid();
            origem.id_tipo_origem = 1;
            origem.enviarParaThrift = true;
            origem.enviado = false;

            Domain.OrigemVisita.Add(origem);

            // cria um cabeçalho
            var transport = header.ToModel(Domain);

            transport.id = Guid.NewGuid();
            transport.OrigemVisita = origem;
            transport.token = origem.token;

            transport.Validar(Domain);

            Domain.UnicaLotacaoTransport.Add(transport);

            await Domain.SaveChangesAsync();

            // registra o token no l4n
            Log.Info(origem.token);

            // retorna o token gerado
            return Ok(origem.token);
        }

        /// <summary>
        /// Endpoint para enviar uma ficha de visita (child) para cadastro não atômico
        /// </summary>
        /// <param name="child">ViewModel com os dados da ficha</param>
        /// <returns>Verdadeiro se concluído com sucesso</returns>
        [Route("enviar/visita/child")]
        [Route("api/processos/enviar/visita/child")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> EnviarFichaVisita([FromBody, Required] FichaVisitaDomiciliarChildCadastroViewModel child)
        {
            #region L4N
            Log.Info("-----");
            Log.Info($"GET api/visita/child/");

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(child));
            #endregion

            // valida o modelo de dados
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // busca ou cria uma ficha master
            var master = await GetOrCreateMaster(child.token ?? Guid.Empty);

            // realiza o DataBinding da ViewModel para a Model
            var ficha = await child.ToModel(Domain);

            // filtra os dados dos Motivos de Visita
            foreach (var motivoId in child.motivosVisita)
            {
                var motivo = await Domain.SIGSM_MotivoVisita.FindAsync(motivoId);

                if (motivo != null)
                    ficha.SIGSM_MotivoVisita.Add(motivo);
            }

            // registra a ficha
            Domain.FichaVisitaDomiciliarChild.Add(ficha);

            if (ficha.dtNascimento != null)
                ficha.dtNascimento.Value.IsValidBirthDateTime(master.UnicaLotacaoTransport.dataAtendimento);

            // atribui uma ficha master
            ficha.FichaVisitaDomiciliarMaster = master;

            // valida a ficha
            ficha.Validar();

            try
            {
                // salva a ficha em banco
                await Domain.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // em caso de erro, registra no l4n
                Log.Fatal(e.Message);

                if (e is DbEntityValidationException)
                {
                    var ex = (e as DbEntityValidationException).EntityValidationErrors;
                    throw new Exception(ex.First().ValidationErrors.First().ErrorMessage, e);
                }

                throw e;
            }

            Log.Info(Ok(true));
            return Ok(true);
        }

        /// <summary>
        /// Endpoint para enviar uma ficha de Cadastro Individual não atômico
        /// </summary>
        /// <param name="cadInd">ViewModel contendo os dados de cadastro individual</param>
        /// <returns>Verdadeiro se concluído com sucesso</returns>
        [Route("enviar/cadastro/individual")]
        [Route("api/processos/enviar/cadastro/individual")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> EnviarCadastroIndividual([FromBody, Required] CadastroIndividualViewModel cadInd)
        {
            #region L4N
            Log.Info("-----");
            Log.Info($"GET api/cadastro/individual/{cadInd.token}");

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(cadInd));
            #endregion

            // valida o modelo de dados
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // busca o token
            var origem = await Domain.OrigemVisita.FindAsync(cadInd.token);

            // busca o cabeçalho
            var header = origem?.UnicaLotacaoTransport?.FirstOrDefault();

            if (origem == null || origem.finalizado || header == null)
            {
                throw new InvalidOperationException("Token inválido. Inicie o processo de transmissão.");
            }

            // processa o modelo de dados
            await ProcessarIndividuos(new[] { cadInd }, header, true);

            // realiza o DataBind da ViewModel para a Model
            var cad = await cadInd.ToModel(Domain);
            cad.tpCdsOrigem = 3;
            cad.UnicaLotacaoTransport = header;

            // valida o cadastro
            cad.Validar(Domain);

            // registra o cadastro
            Domain.CadastroIndividual.Add(cad);

            // salva o cadastro no banco de dados
            await Domain.SaveChangesAsync();

            Log.Info(Ok(true));
            return Ok(true);
        }

        /// <summary>
        /// Endpoint para envio da ficha de Cadastro Domiciliar não atômico
        /// </summary>
        /// <param name="cadDom">ViewModel contendo os dados de cadastro domiciliar</param>
        /// <returns>Verdadeiro se concluído com sucesso</returns>
        [Route("enviar/cadastro/domiciliar")]
        [Route("api/processos/enviar/cadastro/domiciliar")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> EnviarCadastroDomiciliar([FromBody, Required] CadastroDomiciliarViewModel cadDom)
        {
            #region L4N
            Log.Info("-----");
            Log.Info("POST enviar/cadastro/domiciliar");
            Log.Info($"TOKEN {cadDom.token}");

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(cadDom));
            #endregion

            //Validate o modelo de dados
            if (!ModelState.IsValid)
            {
                Log.Fatal(ModelState);
                return BadRequest(ModelState);
            }

            // busca o token
            var origem = await Domain.OrigemVisita.FindAsync(cadDom.token);

            if (origem == null || origem.finalizado)
            {
                Log.Fatal("Token inválido. Inicie o processo de transmissão.");
                throw new ValidationException("Token inválido. Inicie o processo de transmissão.");
            }

            // busca o cabeçalho
            var header = origem.UnicaLotacaoTransport.FirstOrDefault();

            // realiza o DataBind da ViewModel para a Model
            var cad = await cadDom.ToModel(Domain);
            cad.tpCdsOrigem = 3;
            cad.UnicaLotacaoTransport = header;
            if (header == null)
            {
                Log.Fatal("Token inválido. Inicie o processo de transmissão.");
                throw new ValidationException("Token inválido. Inicie o processo de transmissão.");
            }

            // valida o modelo
            await cad.Validar(Domain);

            // registra o model
            Domain.CadastroDomiciliar.Add(cad);

            // salva em banco de dados
            await Domain.SaveChangesAsync();

            Log.Info(Ok(true));
            return Ok(true);
        }

        /// <summary>
        /// Endpoint para cadastramento atômico
        /// </summary>
        /// <param name="cadastros">ViewModel Collection com os dados de cabeçalhos, fichas de visitas, cadastros individuais e cadastros domiciliares</param>
        /// <returns>Verdadeiro se concluído com sucesso</returns>
        [Route("api/processos/enviar/cadastro/atomico")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> CadastramentoAtomico([FromBody, Required] AtomicTransporViewModel[] cadastros)
        {
            #region L4N
            Log.Info("-----");
            Log.Info("POST api/processos/enviar/cadastro/atomico");

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(cadastros));
            #endregion

            try
            {
                // percorre a coleção de ViewModels
                foreach (var cadastro in cadastros)
                {
                    // processa os cadastros individuais de responsáveis
                    await ProcessarIndividuos(cadastro.individuos.Where(x => x.identificacaoUsuarioCidadao != null &&
                    x.identificacaoUsuarioCidadao.statusEhResponsavel), cadastro.cabecalho.ToModel(Domain));

                    // processa os cadastros individuais de dependentes
                    await ProcessarIndividuos(cadastro.individuos.Where(x => x.identificacaoUsuarioCidadao == null ||
                    !x.identificacaoUsuarioCidadao.statusEhResponsavel), cadastro.cabecalho.ToModel(Domain));

                    // processa os cadastros domiciliares
                    foreach (var domicilio in cadastro.domicilios)
                    {
                        // cria um token
                        var origem = Domain.OrigemVisita.Create();

                        origem.token = Guid.NewGuid();
                        origem.id_tipo_origem = 1;
                        origem.enviarParaThrift = true;
                        origem.enviado = false;

                        Domain.OrigemVisita.Add(origem);
                        Log.Info($"TOKEN {origem.token}");

                        // realiza o DataBind do cabeçalho
                        var header = cadastro.cabecalho.ToModel(Domain);

                        header.id = Guid.NewGuid();
                        header.OrigemVisita = origem;
                        header.token = origem.token;

                        Domain.UnicaLotacaoTransport.Add(header);

                        var cad = await domicilio.ToModel(Domain);
                        cad.tpCdsOrigem = 3;
                        cad.UnicaLotacaoTransport = header;

                        Domain.CadastroDomiciliar.Add(cad);
                    }

                    var masters = new List<FichaVisitaDomiciliarMaster>();

                    // função para buscar ou gerar uma ficha master em memória
                    Func<FichaVisitaDomiciliarMaster> getOrCreateMaster = () =>
                    {
                        var master = masters.FirstOrDefault(x => x.FichaVisitaDomiciliarChild.Count < 99) ?? Domain.FichaVisitaDomiciliarMaster.Create();

                        if (master.uuidFicha != null) return master;

                        // cria um token
                        var origem = Domain.OrigemVisita.Create();

                        origem.token = Guid.NewGuid();
                        origem.id_tipo_origem = 1;
                        origem.enviarParaThrift = true;
                        origem.enviado = false;

                        Domain.OrigemVisita.Add(origem);
                        Log.Info($"TOKEN {origem.token}");

                        // realiza o DataBind do cabeçalho
                        var header = cadastro.cabecalho.ToModel(Domain);

                        header.id = Guid.NewGuid();
                        header.OrigemVisita = origem;
                        header.token = origem.token;

                        Domain.UnicaLotacaoTransport.Add(header);

                        master.tpCdsOrigem = 3;
                        master.UnicaLotacaoTransport = header;

                        master.uuidFicha = header.cnes + '-' + Guid.NewGuid();

                        Domain.FichaVisitaDomiciliarMaster.Add(master);

                        return master;
                    };

                    // processa as fichas de visitas
                    foreach (var child in cadastro.visitas)
                    {
                        var master = getOrCreateMaster();

                        var ficha = await child.ToModel(Domain);

                        foreach (var motivoId in child.motivosVisita)
                        {
                            var motivo = await Domain.SIGSM_MotivoVisita.FindAsync(motivoId);

                            if (motivo != null)
                                ficha.SIGSM_MotivoVisita.Add(motivo);
                        }

                        Domain.FichaVisitaDomiciliarChild.Add(ficha);

                        if (ficha.dtNascimento != null)
                            Epoch.ValidateBirthDateTime(ficha.dtNascimento.Value, master.UnicaLotacaoTransport.dataAtendimento);

                        ficha.FichaVisitaDomiciliarMaster = master;
                    }
                }
            }
            catch (Exception e)
            {
                // em caso de falha, registra no L4N
                Log.Fatal(e.Message);

                if (e is DbEntityValidationException)
                {
                    var ex = (e as DbEntityValidationException).EntityValidationErrors;
                    throw new Exception(ex.First().ValidationErrors.First().ErrorMessage, e);
                }

                throw e;
            }

            try
            {
                // salva as fichas em banco de dados
                await Domain.SaveChangesAsync();

                // finaliza o token
                //Domain.PR_ProcessarFichasAPI(origem.token);
            }
            catch (Exception e)
            {
                // em caso de falha, registra no L4N
                Log.Fatal(e.Message);

                if (e is DbEntityValidationException)
                {
                    var ex = (e as DbEntityValidationException).EntityValidationErrors;
                    throw new Exception(ex.First().ValidationErrors.First().ErrorMessage, e);
                }

                throw e;
            }

            return Ok(true);
        }

        /// <summary>
        /// Este método é responsável por tratar e processar os cadastros individuais
        /// </summary>
        /// <param name="individuos"></param>
        /// <param name="header"></param>
        /// <param name="validar"></param>
        /// <returns></returns>
        private async Task ProcessarIndividuos(IEnumerable<PrimitiveCadastroIndividualViewModel> individuos, UnicaLotacaoTransport header, bool validar = false)
        {
            // percorre os individuos
            foreach (var individuo in individuos)
            {
                // cria um token
                var origem = Domain.OrigemVisita.Create();

                origem.token = Guid.NewGuid();
                origem.id_tipo_origem = 1;
                origem.enviarParaThrift = true;
                origem.enviado = false;
                Log.Info($"TOKEN {origem.token}");

                Domain.OrigemVisita.Add(origem);

                // realiza o DataBind do cabeçalho
                var h = Domain.UnicaLotacaoTransport.Create();

                h.id = Guid.NewGuid();
                h.OrigemVisita = origem;
                h.token = origem.token;
                h.cboCodigo_2002 = header.cboCodigo_2002;
                h.cnes = header.cnes;
                h.codigoIbgeMunicipio = header.codigoIbgeMunicipio;
                h.dataAtendimento = header.dataAtendimento;
                h.ine = header.ine;
                h.profissionalCNS = header.profissionalCNS;

                Domain.UnicaLotacaoTransport.Add(h);

                // realiza o DataBind
                var cad = await individuo.ToModel(Domain);
                cad.tpCdsOrigem = 3;
                cad.UnicaLotacaoTransport = h;

                if (validar) cad.Validar(Domain);

                // registra o modelo
                Domain.CadastroIndividual.Add(cad);
            }
        }

        /// <summary>
        /// Endpoitn para finalizar a transmissão de dados (encerrar o token) de cadastros não atômicos
        /// </summary>
        /// <param name="token">Token gerado anteriormente</param>
        /// <returns>Verdadeiro se concluído com sucesso</returns>
        [Route("encerrar/{token}")]
        [Route("api/processos/encerrar/{token}")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> FinalizarTransmissao([FromUri, Required] Guid token)
        {
            #region L4N
            Log.Info("-----");
            Log.Info("POST api/encerrar/token");

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(token));
            #endregion

            var origem = await Domain.OrigemVisita.FindAsync(token);

            // verifica pela integridade do token informado
            if (origem == null || origem.finalizado)
            {
                throw new ValidationException("Token inválido. Inicie o processo de transmissão.");
            }
            // verifica se há fichas no token
            else if (origem.UnicaLotacaoTransport.Sum(x => x.FichaVisitaDomiciliarMaster.Count + x.CadastroDomiciliar.Count + x.CadastroIndividual.Count) > 0)
            {
                // finaliza o token
                Domain.PR_ProcessarFichasAPI(token);
            }
            else
            {
                try
                {
                    // finaliza o token sem processar fichas
                    origem.finalizado = true;
                    //await Domain.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Log.Warn("Erro ao tentar finalizar token.", e);
                }
            }

            Log.Info(Ok(true));
            return Ok(true);
        }
    }
}