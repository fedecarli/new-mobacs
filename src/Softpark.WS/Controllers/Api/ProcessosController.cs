using Softpark.Infrastructure.Extras;
using Softpark.Models;
using Softpark.WS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;
using static Softpark.Infrastructure.Extensions.WithStatement;

namespace Softpark.WS.Controllers.Api
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class UsuarioVM
    {
        public ASSMED_Cadastro cad { get; set; }
        public ASSMED_Usuario usu { get; set; }
        public ASSMED_Acesso acs { get; set; }
        public VW_Profissional profissional { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Endpoints dos processos de transmissão de dados provenientes do APP
    /// </summary>
    [RoutePrefix("api/processos")]
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
        /// <returns></returns>
        private async Task<FichaVisitaDomiciliarMaster> CreateMaster(UsuarioVM acesso, DateTime dataAtendimento)
        {
            var prof = acesso.profissional;

            var contrato = await Domain.ASSMED_Contratos.FirstAsync();

            var header = await Domain.UnicaLotacaoTransport
                .FirstOrDefaultAsync(x => x.FichaVisitaDomiciliarMaster.Count < 99
                && x.dataAtendimento == dataAtendimento
                && x.cboCodigo_2002 == prof.CBO.Trim()
                && x.cnes == prof.CNES.Trim()
                && x.codigoIbgeMunicipio == contrato.CodigoIbgeMunicipio
                && (prof.INE == null || x.ine == prof.INE.Trim())
                && x.profissionalCNS == prof.CNS.Trim()) ??
                await CriarCabecalho(acesso, dataAtendimento);

            // busca ou cria uma ficha
            var master =
                header.FichaVisitaDomiciliarMaster.FirstOrDefault() ??
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
        [HttpPost, ResponseType(typeof(Guid))]
        [Obsolete("Este processo está obsoleto, realize a autenticação do profissional pelo endpoint '/api/processos/autenticar'.", true)]
        public IHttpActionResult EnviarCabecalho([FromBody, Required] UnicaLotacaoTransportCadastroViewModel header)
        {
            return BadRequest("Este processo está obsoleto, realize a autenticação do profissional pelo endpoint '/api/processos/autenticar'.");
        }

        private string HashPassword(string openPassword, HashAlgorithm hash)
        {
            var d = hash.ComputeHash(Encoding.UTF8.GetBytes(openPassword));
            var sb = new StringBuilder();
            for (int i = 0; i < d.Length; i++) sb.Append(d[i].ToString("x2"));
            return sb.ToString();
        }

        /// <summary>
        /// Endpoint para autenticação de profissional
        /// </summary>
        /// <param name="vm"></param>
        /// <returns>Dados do profissional</returns>
        [Route("autenticar")]
        [HttpPost, ResponseType(typeof(DadosAcessoViewModel))]
        public async Task<IHttpActionResult> AutenticarProfissional([FromBody, Required] AutenticacaoViewModel vm)
        {
            #region LOG4NET
            // registra no l4n a chamada do endpoint
            Log.Info("----");
            Log.Info("api/processos/autenticar");
            // não fazer log do dado serializado por questões de segurança
            #endregion

            var usuario = vm.Usuario.Trim();

            var senha = vm.Senha;

            // busca a configuração do algoritmo de encriptação
            var alg = await Domain.SIGSM_ServicoSerializador_Config.FindAsync("authCryptAlg");

            // monta o FullName do objeto de algoritmo
            var hashAlg = "System.Security.Cryptography." + (alg?.Valor?.ToUpper() ?? "MD5");

            // faz a busca pela definição do objeto (Reflection)
            var hashType = Type.GetType(hashAlg);

            // busca a definição do método de Criação do Objeto
            var create = hashType.GetMethods(BindingFlags.Public | BindingFlags.Static).First(x => x.GetParameters().Count() == 0 && x.Name == "Create");

            // realiza a encriptação da senha para equiparação com o banco de dados
            using (var hash = create.Invoke(null, null) as HashAlgorithm)
                senha = HashPassword(senha, hash);

            var u = await (from prf in Domain.VW_Profissional
                           join cad in Domain.ASSMED_Cadastro
                                    on prf.Codigo equals cad.Codigo
                           join usu in Domain.ASSMED_Usuario
                                    on prf.CodUsu equals usu.CodUsu
                           where prf.CodUsu != null
                              && usu.Login != null
                              && usu.Senha != null
                              && usu.Ativo == 1
                              && (usu.Login.Trim() == usuario
                              || prf.CNS == usuario)
                              && prf.CNES == vm.CNES.Trim()
#if !DEBUG
                              && prf.CBO == "515105" // somente se for um AGENTE COMUNITÁRIO DE SAÚDE
#endif
                              && usu.Senha == senha
                           select new UsuarioVM { cad = cad, usu = usu, profissional = prf })
                      .SingleOrDefaultAsync();

            if (u == null)
            {
                return BadRequest("Usuário, CNS, Senha ou CNES inválidos.");
            }

            var acesso = Domain.ASSMED_Acesso.Create();

            var tokenAcesso = Guid.NewGuid();

            acesso.EMail = $"{u.profissional.CBO}|{u.profissional.CNES}|{u.profissional.INE}";
            acesso.DtAcesso = DateTime.Now;
            acesso.CodUsu = u.usu.CodUsu;
            acesso.Validou = "S";
            acesso.NumIP = System.Web.HttpContext.Current.Request.UserHostAddress;
            acesso.ASPSESSIONIDQASRTRQT = $"{tokenAcesso}";
            acesso.DtUltVer = DateTime.Now;

            u.acs = acesso;

            Domain.ASSMED_Acesso.Add(acesso);
            await Domain.SaveChangesAsync();
            var cfg = await Domain.SIGSM_ServicoSerializador_Config.FindAsync("limiteHorasToken");

            var horas = Convert.ToInt32(cfg?.Valor ?? (2 * 24).ToString());

            var dadosAcesso = new DadosAcessoViewModel
            {
                TokenAcesso = tokenAcesso,
                DadosAtrelados = u.profissional,
                ValidoAte = acesso.DtUltVer.Value.AddHours(horas).AddMilliseconds(-1)
            };

            // retorna o token gerado
            return Ok(dadosAcesso);
        }

        /// <summary>
        /// Endpoint para enviar uma ficha de visita (child) para cadastro não atômico
        /// </summary>
        /// <param name="child">ViewModel com os dados da ficha</param>
        /// <returns>Verdadeiro se concluído com sucesso</returns>
        [Route("enviar/visita/child")]
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

            var acesso = await BuscarAcesso(child.token ?? Guid.Empty);

            if (acesso == null)
            {
                ModelState.AddModelError(nameof(child.token), "Token Inválido.");
                return BadRequest(ModelState);
            }

            // busca ou cria uma ficha master
            var master = await CreateMaster(acesso, child.DataAtendimento);

            // realiza o DataBinding da ViewModel para a Model
            var ficha = child.ToModel(Domain);

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
                var processo = await Domain.SIGSM_Transmissao_Processos.FindAsync(new Guid(master.uuidFicha.Replace($"{master.UnicaLotacaoTransport.cnes}-", "")));

                if (processo != null)
                {
                    var logs = processo.SIGSM_Transmissao_Processos_Log.ToArray();

                    processo.SIGSM_Transmissao_Processos_Log.Clear();

                    Domain.SIGSM_Transmissao_Processos_Log.RemoveRange(logs);

                    Domain.SIGSM_Transmissao_Processos.Remove(processo);
                }

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

            var acesso = await BuscarAcesso(cadInd.token);

            if (acesso == null)
            {
                ModelState.AddModelError(nameof(cadInd.token), "Token Inválido.");
                return BadRequest(ModelState);
            }

            // processa o modelo de dados
            var cad = (await ProcessarIndividuos(new[] { cadInd }, acesso, true))
                .FirstOrDefault();

            await Domain.SaveChangesAsync();

            var assmed = await GerarCadastroAssmed(cad, acesso);

            if (assmed != null)
            {
                cad.IdentificacaoUsuarioCidadao1.ASSMED_Cadastro1 = assmed;
                cad.IdentificacaoUsuarioCidadao1.Codigo = assmed.Codigo;
                cad.IdentificacaoUsuarioCidadao1.num_contrato = assmed.NumContrato;

                await Domain.SaveChangesAsync();
            }

            Log.Info(Ok(true));
            return Ok(true);
        }

        /// <summary>
        /// Endpoint para envio da ficha de Cadastro Domiciliar não atômico
        /// </summary>
        /// <param name="cadDom">ViewModel contendo os dados de cadastro domiciliar</param>
        /// <returns>Verdadeiro se concluído com sucesso</returns>
        [Route("enviar/cadastro/domiciliar")]
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

            var acesso = await BuscarAcesso(cadDom.token);

            if (acesso == null)
            {
                ModelState.AddModelError(nameof(cadDom.token), "Token Inválido.");
                return BadRequest(ModelState);
            }

            // busca o cabeçalho
            var header = await CriarCabecalho(acesso, cadDom.DataAtendimento);

            // realiza o DataBind da ViewModel para a Model
            var cad = await cadDom.ToModel(Domain);
            cad.tpCdsOrigem = 3;
            cad.UnicaLotacaoTransport = header;

            // valida o modelo
            await cad.Validar(Domain);

            // registra o model
            Domain.CadastroDomiciliar.Add(cad);

            // salva em banco de dados
            await Domain.SaveChangesAsync();

            await GerarCadastroAssmed(cad, acesso);

            await Domain.SaveChangesAsync();

            Log.Info(Ok(true));
            return Ok(true);
        }

        /// <summary>
        /// Endpoint para cadastramento atômico
        /// </summary>
        /// <param name="token">Token de acesso</param>
        /// <param name="cadastros">ViewModel Collection com os dados de cabeçalhos, fichas de visitas, cadastros individuais e cadastros domiciliares</param>
        /// <returns>Verdadeiro se concluído com sucesso</returns>
        [Route("enviar/cadastro/atomico/{token:guid}")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> CadastramentoAtomico([FromUri] Guid token, [FromBody, Required] AtomicTransporViewModel[] cadastros)
        {
#region L4N
            Log.Info("-----");
            Log.Info($"POST api/processos/enviar/cadastro/atomico/{token}");

            var serializer = new JavaScriptSerializer();
            Log.Info(serializer.Serialize(cadastros));
#endregion

            var cads = new List<CadastroIndividual>();
            var doms = new List<CadastroDomiciliar>();

            var acesso = await BuscarAcesso(token);

            if (acesso == null)
            {
                return BadRequest("Token Inválido");
            }

            try
            {
                // percorre a coleção de ViewModels
                foreach (var cadastro in cadastros)
                {
                    // processa os cadastros individuais de responsáveis
                    cads.AddRange(await ProcessarIndividuos(cadastro.individuos.Where(x => x.identificacaoUsuarioCidadao != null &&
                    x.identificacaoUsuarioCidadao.statusEhResponsavel), acesso));

                    // processa os cadastros individuais de dependentes
                    cads.AddRange(await ProcessarIndividuos(cadastro.individuos.Where(x => x.identificacaoUsuarioCidadao == null ||
                    !x.identificacaoUsuarioCidadao.statusEhResponsavel), acesso));

                    // processa os cadastros domiciliares
                    foreach (var domicilio in cadastro.domicilios)
                    {
                        // realiza o DataBind do cabeçalho
                        var header = await CriarCabecalho(acesso, domicilio.DataAtendimento);

                        var cad = await domicilio.ToModel(Domain);
                        cad.tpCdsOrigem = 3;
                        cad.UnicaLotacaoTransport = header;

                        Domain.CadastroDomiciliar.Add(cad);

                        doms.Add(cad);
                    }

                    var masters = new List<FichaVisitaDomiciliarMaster>();

                    // processa as fichas de visitas
                    foreach (var child in cadastro.visitas)
                    {
                        var master = await CreateMaster(acesso, child.DataAtendimento);

                        var ficha = child.ToModel(Domain);

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

                cads.ForEach(async cad =>
                {

                    var assmed = await GerarCadastroAssmed(cad, acesso);

                    if (assmed != null)
                    {
                        cad.IdentificacaoUsuarioCidadao1.ASSMED_Cadastro1 = assmed;
                        cad.IdentificacaoUsuarioCidadao1.Codigo = assmed.Codigo;
                        cad.IdentificacaoUsuarioCidadao1.num_contrato = assmed.NumContrato;
                    }
                });

                doms.ForEach(async cad => await GerarCadastroAssmed(cad, acesso));

                await Domain.SaveChangesAsync();
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
        /// <param name="acesso"></param>
        /// <param name="validar"></param>
        /// <returns></returns>
        private async Task<IEnumerable<CadastroIndividual>> ProcessarIndividuos(IEnumerable<PrimitiveCadastroIndividualViewModel> individuos, UsuarioVM acesso, bool validar = false)
        {
            var cads = new List<CadastroIndividual>();

            // percorre os individuos
            foreach (var individuo in individuos)
            {
                var header = await CriarCabecalho(acesso, individuo.DataAtendimento);

                // realiza o DataBind
                var cad = await individuo.ToModel(Domain);
                cad.tpCdsOrigem = 3;
                cad.UnicaLotacaoTransport = header;

                CleanStrings(cad);

                if (validar) cad.Validar(Domain);

                // registra o modelo
                Domain.CadastroIndividual.Add(cad);

                var proc = await Domain.SIGSM_Transmissao_Processos.FindAsync(cad.uuidFichaOriginadora);

                if (proc != null)
                {
                    var logs = proc.SIGSM_Transmissao_Processos_Log.ToArray();

                    proc.SIGSM_Transmissao_Processos_Log.Clear();

                    Domain.SIGSM_Transmissao_Processos_Log.RemoveRange(logs);

                    Domain.SIGSM_Transmissao_Processos.Remove(proc);
                }

                var codigo = cad.IdentificacaoUsuarioCidadao1?.Codigo;
                var cpf = cad.IdentificacaoUsuarioCidadao1?.CPF;
                var rg = cad.IdentificacaoUsuarioCidadao1?.RG;
                var cns = cad.IdentificacaoUsuarioCidadao1?.cnsCidadao;
                var pis = cad.IdentificacaoUsuarioCidadao1?.numeroNisPisPasep;

                var assmedCadastro = cad.IdentificacaoUsuarioCidadao1?.ASSMED_Cadastro1 ??
                    (await Domain.CadastroIndividual.FindAsync(cad.uuidFichaOriginadora))?.IdentificacaoUsuarioCidadao1?.ASSMED_Cadastro1 ??
                    await (Domain.ASSMED_CadastroDocPessoal.Where(x => x.Numero != null && x.Numero.Trim().Length > 0 && x.Numero == cns && x.CodTpDocP == 6)
                            .Select(x => x.ASSMED_Cadastro).FirstOrDefaultAsync() ??
                        Domain.ASSMED_PesFisica.Where(x => x.CPF == cpf).Select(x => x.ASSMED_Cadastro).FirstOrDefaultAsync() ??
                        Domain.ASSMED_CadastroDocPessoal.Where(x => x.Numero != null && x.Numero.Trim().Length > 0 && x.Numero == rg && x.CodTpDocP == 1)
                            .Select(x => x.ASSMED_Cadastro).FirstOrDefaultAsync() ??
                        Domain.ASSMED_CadastroDocPessoal.Where(x => x.Numero != null && x.Numero.Trim().Length > 0 && x.Numero == pis && x.CodTpDocP == 7)
                            .Select(x => x.ASSMED_Cadastro).FirstOrDefaultAsync());

                CadastroIndividual dadoAnterior = null;

                if (assmedCadastro != null)
                {
                    codigo = assmedCadastro.Codigo;

                    // Busca a última ficha do cadastro
                    var ultimaFicha = await Domain.CadastroIndividual.SingleOrDefaultAsync(x => x.identificacaoUsuarioCidadao == assmedCadastro.IdFicha);

                    dadoAnterior = cad.id != cad.uuidFichaOriginadora ?
                        (await Domain.CadastroIndividual.SingleOrDefaultAsync(x => x.id == cad.uuidFichaOriginadora)) : null;

                    // Verifica se a ficha de origem existe E possui uma ficha de origem E
                    // se ainda não foi enviada E o tipo de origem não é a API
                    while (ultimaFicha != null &&
                        ultimaFicha.uuidFichaOriginadora != null &&
                        ultimaFicha.uuidFichaOriginadora != ultimaFicha.id &&
                        !ultimaFicha.UnicaLotacaoTransport.OrigemVisita.enviado &&
                        ultimaFicha.UnicaLotacaoTransport.OrigemVisita.id_tipo_origem != 1)
                    {
                        // marca a ficha de origem para não ser enviada
                        ultimaFicha.UnicaLotacaoTransport.OrigemVisita.enviarParaThrift = false;

                        // busca a origem da origem
                        var uf = await Domain.CadastroIndividual.FindAsync(ultimaFicha.uuidFichaOriginadora);

                        // atribui a ficha de origem da origem à origem da ficha à ser criada
                        // se existir
                        if (uf != null)
                            ultimaFicha = uf;
                    }

                    if (ultimaFicha != null &&
                        !ultimaFicha.UnicaLotacaoTransport.OrigemVisita.enviado &&
                        ultimaFicha.UnicaLotacaoTransport.OrigemVisita.id_tipo_origem != 1)
                        ultimaFicha.UnicaLotacaoTransport.OrigemVisita.enviarParaThrift = false;

                    if (ultimaFicha != null)
                    {
                        cad.uuidFichaOriginadora = ultimaFicha.id;

                        cad.fichaAtualizada = true;
                    }
                }

                if (cad.IdentificacaoUsuarioCidadao1 != null)
                {
                    cad.IdentificacaoUsuarioCidadao1.Codigo = assmedCadastro?.Codigo ?? cad.IdentificacaoUsuarioCidadao1?.Codigo;

                    cad.IdentificacaoUsuarioCidadao1.ASSMED_Cadastro1 = assmedCadastro;

                    if (assmedCadastro != null)
                    {
                        assmedCadastro.IdFicha = cad.IdentificacaoUsuarioCidadao1.id;
                    }
                }

                cads.Add(cad);
            }

            return cads;
        }


        private async Task GerarCadastroAssmed(CadastroDomiciliar cad, UsuarioVM acesso)
        {
            if (cad.enderecoLocalPermanencia == null) return;

            var end = cad.EnderecoLocalPermanencia1;

            var codtplog = (await Domain.TB_MS_TIPO_LOGRADOURO.FirstOrDefaultAsync(x => x.CO_TIPO_LOGRADOURO != null && x.CO_TIPO_LOGRADOURO.Trim() != end.tipoLogradouroNumeroDne))?.CO_TIPO_LOGRADOURO.Trim() ?? null;

            int? tplog = null;
            if (int.TryParse(codtplog, out int tl))
                tplog = tl;

            var cid = await Domain.Cidade.FirstOrDefaultAsync(x => x.CodIbge != null && x.CodIbge.Trim() == end.codigoIbgeMunicipio);

            var uf = await Domain.UF.FirstOrDefaultAsync(x => x.DNE != null && x.DNE.Trim() == end.numeroDneUf);

            foreach (var fam in cad.FamiliaRow)
            {
                if (1 == await Domain.ASSMED_CadastroDocPessoal.CountAsync(x => x.Numero != null && x.Numero.Trim() == fam.numeroCnsResponsavel.Trim() && x.CodTpDocP == 6))
                {
                    var resp = await Domain.ASSMED_CadastroDocPessoal.Where(x => x.Numero != null && x.Numero.Trim() == fam.numeroCnsResponsavel.Trim() && x.CodTpDocP == 6)
                        .Select(x => x.ASSMED_Cadastro)
                        .SingleOrDefaultAsync();

                    if (resp == null) return;

                    var depends = await Domain.IdentificacaoUsuarioCidadao.Where(x => x.cnsResponsavelFamiliar == fam.numeroCnsResponsavel &&
                    x.cnsResponsavelFamiliar != x.cnsCidadao && x.Codigo != null)
                    .SelectMany(x => x.ASSMED_Cadastro)
                    .ToListAsync();

                    var item = resp.ASSMED_Endereco.Max(x => x.ItemEnd) + 1;

                    var respEnd = resp.ASSMED_Endereco.Where(x => x.TipoEnd == "R").OrderByDescending(x => x.ItemEnd).FirstOrDefault();
                    respEnd = new ASSMED_Endereco
                    {
                        Codigo = resp.Codigo,
                        NumContrato = resp.NumContrato,
                        ItemEnd = item,
                        ASSMED_Cadastro = resp,
                        Bairro = end.bairro,
                        CEP = end.cep,
                        CodCidade = cid?.CodCidade,
                        CodTpLogra = tplog,
                        Complemento = end.complemento,
                        Corresp = respEnd?.Corresp,
                        ENDAREAMICRO = end.microarea,
                        EnderecoLocalPermanencia = end,
                        ENDREFERENCIA = end.pontoReferencia,
                        ENDSEMAREA = end.stForaArea ? 1 : 0,
                        IdFicha = end.id,
                        Latitude = cad.latitude,
                        Longitude = cad.longitude,
                        Logradouro = end.nomeLogradouro,
                        NomeCidade = cid?.NomeCidade,
                        Numero = end.numero,
                        SEMNUMERO = end.stSemNumero ? 1 : 0,
                        TipoEnd = "R",
                        UF = uf?.UF1,
                        ENDAREA = respEnd?.ENDAREA
                    };

                    resp.ASSMED_Endereco.Add(respEnd);

                    foreach (var depend in depends)
                    {
                        item = depend.ASSMED_Endereco.Max(x => x.ItemEnd) + 1;

                        var respDep = depend.ASSMED_Endereco.Where(x => x.TipoEnd == "R")
                            .OrderByDescending(x => x.ItemEnd).FirstOrDefault();

                        respDep = new ASSMED_Endereco
                        {
                            Codigo = depend.Codigo,
                            NumContrato = depend.NumContrato,
                            ItemEnd = item,
                            ASSMED_Cadastro = depend,
                            Bairro = end.bairro,
                            CEP = end.cep,
                            CodCidade = cid?.CodCidade,
                            CodTpLogra = tplog,
                            Complemento = end.complemento,
                            Corresp = respDep?.Corresp,
                            ENDAREAMICRO = end.microarea,
                            EnderecoLocalPermanencia = end,
                            ENDREFERENCIA = end.pontoReferencia,
                            ENDSEMAREA = end.stForaArea ? 1 : 0,
                            IdFicha = end.id,
                            Latitude = cad.latitude,
                            Longitude = cad.longitude,
                            Logradouro = end.nomeLogradouro,
                            NomeCidade = cid?.NomeCidade,
                            Numero = end.numero,
                            SEMNUMERO = end.stSemNumero ? 1 : 0,
                            TipoEnd = "R",
                            UF = uf?.UF1,
                            ENDAREA = respDep?.ENDAREA
                        };

                        depend.ASSMED_Endereco.Add(respDep);
                    }
                }
            }
        }

        private async Task<ASSMED_Cadastro> GerarCadastroAssmed(CadastroIndividual cad, UsuarioVM acesso)
        {
            if (cad.IdentificacaoUsuarioCidadao1 == null) return null;

            var iden = cad.IdentificacaoUsuarioCidadao1;

            var requester = HttpContext.Current?.Request?.UserHostAddress ?? "http://localhost";

            var def = cad.InformacoesSocioDemograficas1?.DeficienciasCidadao.FirstOrDefault()?.id_tp_deficiencia_cidadao;
            var curso = cad.InformacoesSocioDemograficas1?.grauInstrucaoCidadao;
            curso = (await Domain.TP_Curso.SingleOrDefaultAsync(x => x.codigo == curso))?.id_tp_curso;

            var codigoIbgeMunicipioNascimento = iden?.codigoIbgeMunicipioNascimento;

            var cidade = (await Domain.Cidade.FirstOrDefaultAsync(x => x.CodIbge == codigoIbgeMunicipioNascimento && x.CodIbge != null));

            var deficiencia = (await Domain.TP_Deficiencia.SingleOrDefaultAsync(x => x.codigo == def))?.id_tp_deficiencia;

            var paisNascimento = iden?.paisNascimento;

            var nacao = (await Domain.Nacionalidade.FirstOrDefaultAsync(x => x.codigo == paisNascimento && x.codigo != null))?.CodNacao;

            var cpf = iden?.CPF;
            if (cpf != null)
            {
                cpf = Regex.Replace(cpf, "([0-9]{3})([0-9]{3})([0-9]{3})([0-9]{2})", "$1.$2.$3-$4");
            }

            var assmed = iden.ASSMED_Cadastro1;

            var pessoa = assmed?.ASSMED_PesFisica;

            var codigo = assmed?.Codigo;

            var idUsuario = acesso.usu.CodUsu;

            var novo = codigo == null;

            ASSMED_CadastroDocPessoal cns = null;
            ASSMED_CadastroDocPessoal rg = null;
            ASSMED_CadastroDocPessoal pis = null;

            if (novo)
            {
                if (iden.cnsCidadao != null)
                {
                    var _cns = iden.cnsCidadao;

                    var numCnss = await Domain.ASSMED_CadastroDocPessoal.CountAsync(x => x.Numero == _cns && x.CodTpDocP == 6);

                    if (numCnss == 1)
                    {
                        cns = await Domain.ASSMED_CadastroDocPessoal.FirstOrDefaultAsync(x => x.Numero == _cns && x.CodTpDocP == 6);

                        assmed = cns.ASSMED_Cadastro;

                        pessoa = assmed.ASSMED_PesFisica;

                        codigo = assmed.Codigo;

                        novo = false;
                    }
                }

                if (novo && cpf != null)
                {
                    var numCpfs = await Domain.ASSMED_PesFisica.CountAsync(x => x.CPF == cpf || x.CPF == iden.CPF);

                    if (numCpfs == 1)
                    {
                        pessoa = await Domain.ASSMED_PesFisica.SingleAsync(x => x.CPF == cpf || x.CPF == iden.CPF);

                        assmed = pessoa.ASSMED_Cadastro;

                        codigo = assmed.Codigo;

                        novo = false;
                    }
                }

                if (novo && iden.RG != null)
                {
                    var _rg = iden.RG;
                    var _rg2 = Regex.Replace(@"([.\/-])", "", iden.RG);

                    var numRgs = await Domain.ASSMED_CadastroDocPessoal.CountAsync(x => (x.Numero == _rg || x.Numero == _rg2) && x.CodTpDocP == 1);

                    if (numRgs == 1)
                    {
                        rg = await Domain.ASSMED_CadastroDocPessoal.SingleAsync(x => (x.Numero == _rg || x.Numero == _rg2) && x.CodTpDocP == 1);

                        assmed = rg.ASSMED_Cadastro;

                        pessoa = assmed.ASSMED_PesFisica;

                        codigo = assmed.Codigo;

                        novo = false;
                    }
                }

                if (novo && iden.numeroNisPisPasep != null)
                {
                    var _pis = iden.numeroNisPisPasep;
                    var _pis2 = Regex.Replace(@"([.\/-])", "", iden.numeroNisPisPasep);

                    var numPiss = await Domain.ASSMED_CadastroDocPessoal.CountAsync(x => (x.Numero == _pis || x.Numero == _pis2) && x.CodTpDocP == 7);

                    if (numPiss == 1)
                    {
                        pis = await Domain.ASSMED_CadastroDocPessoal.SingleAsync(x => (x.Numero == _pis || x.Numero == _pis2) && x.CodTpDocP == 7);

                        assmed = rg.ASSMED_Cadastro;

                        pessoa = assmed.ASSMED_PesFisica;

                        codigo = assmed.Codigo;

                        novo = false;
                    }
                }

                if (assmed != null)
                {
                    cad.IdentificacaoUsuarioCidadao1.ASSMED_Cadastro1 = assmed;
                    assmed.IdentificacaoUsuarioCidadao = cad.IdentificacaoUsuarioCidadao1;
                }

                if (assmed?.IdentificacaoUsuarioCidadao != null)
                {
                    cad.uuidFichaOriginadora = assmed.IdentificacaoUsuarioCidadao.CadastroIndividual.FirstOrDefault()?.id;

                    cad.fichaAtualizada = true;
                }
            }

            if (novo)
            {
                codigo = await Domain.ASSMED_Cadastro.MaxAsync(x => x.Codigo + 1);
            }

            var telefone = assmed?.ASSMED_CadTelefones.LastOrDefault(x => x.TipoTel == "C");

            var email = assmed?.ASSMED_CadEmails.LastOrDefault(x => x.EMail == iden.emailCidadao);

            cns = cns ?? assmed?.ASSMED_CadastroDocPessoal.LastOrDefault(x => x.CodTpDocP == 6);

            rg = rg ?? assmed?.ASSMED_CadastroDocPessoal.LastOrDefault(x => x.CodTpDocP == 1);

            pis = pis ?? assmed?.ASSMED_CadastroDocPessoal.LastOrDefault(x => x.CodTpDocP == 7);

            var np = pessoa == null;

            With(ref pessoa, () => new ASSMED_PesFisica
            {
                CodCor = iden.racaCorCidadao,
                CodDeficiencia = deficiencia,
                CodEscola = curso,
                CodEtnia = iden.etnia,
                CodNacao = nacao,
                CPF = cpf,
                DtNasc = iden.dataNascimentoCidadao,
                DtObto = cad.SaidaCidadaoCadastro1?.dataObito?.ToString("yyyy-MM-dd HH:mm:ss.sss"),
                Deficiente = cad.InformacoesSocioDemograficas1?.DeficienciasCidadao.Any() == true ? "S" : "N",
                MaeDesconhecida = iden.desconheceNomeMae ? 1 : 0,
                NomeMae = iden.nomeMaeCidadao,
                PaiDesconhecido = iden.desconheceNomePai ? 1 : 0,
                NomePai = iden.nomePaiCidadao,
                EstCivil = iden.EstadoCivil,
                MUNICIPIONASC = cidade?.CodCidade,
                Nacionalidade = iden.nacionalidadeCidadao,
                GENERO = cad.InformacoesSocioDemograficas1?.identidadeGeneroCidadao,
                ESTRANGEIRADATA = iden.dtEntradaBrasil,
                NATURALIZADADATA = iden.dtNaturalizacao,
                NATURALIZACAOPORTARIA = iden.portariaNaturalizacao,
                Sexo = iden.sexoCidadao == 0 ? "M" : iden.sexoCidadao == 1 ? "F" : null,
                FALECIDO = cad.SaidaCidadaoCadastro1?.dataObito != null ? 1 : 0,
                OBITODATA = cad.SaidaCidadaoCadastro1?.dataObito,
                OBITONUMERO = cad.SaidaCidadaoCadastro1?.numeroDO,
                ESCOLARIDADE = curso,
                MuniNacao = cidade?.NomeCidade,
                ORIENTACAO = cad.InformacoesSocioDemograficas1?.orientacaoSexualCidadao,
                UfNacao = cidade?.UF
            }, np ? new string[0] : new[] {
                nameof(ASSMED_PesFisica.Codigo),
                nameof(ASSMED_PesFisica.NumContrato),
                nameof(ASSMED_PesFisica.ASSMED_Cadastro)
            });

            With(ref assmed, () => new ASSMED_Cadastro
            {
                IdentificacaoUsuarioCidadao = iden,
                CodUsu = idUsuario,
                DtAtualizacao = DateTime.Now,
                DtSistema = DateTime.Now,
                IdFicha = iden.id,
                Nome = iden.nomeCidadao,
                NomeSocial = iden.nomeSocial,
                NumIP = requester,
                Tipo = "F"
            }, novo ? new string[0] : new[] {
                nameof(ASSMED_Cadastro.Codigo),
                nameof(ASSMED_Cadastro.NumContrato),
                nameof(ASSMED_Cadastro.ASSMED_CadastroDocPessoal),
                nameof(ASSMED_Cadastro.ASSMED_CadEmails),
                nameof(ASSMED_Cadastro.ASSMED_CadTelefones),
                nameof(ASSMED_Cadastro.ASSMED_Endereco),
                nameof(ASSMED_Cadastro.ASSMED_PesFisica),
                nameof(ASSMED_Cadastro.IdentificacaoUsuarioCidadao),
                nameof(ASSMED_Cadastro.IdentificacaoUsuarioCidadao1),
                nameof(ASSMED_Cadastro.IdFicha)
            });

            if (novo)
            {
                assmed.Codigo = (decimal)codigo;
                assmed.NumContrato = 22;
            }

            if (np)
            {
                pessoa.ASSMED_Cadastro = assmed;
                pessoa.NumContrato = 22;
                pessoa.Codigo = assmed.Codigo;
                assmed.ASSMED_PesFisica = pessoa;
            }

            if (iden.telefoneCelular != null && iden.telefoneCelular.Length >= 10)
            {
                var n = telefone == null;

                With(ref telefone, () => new ASSMED_CadTelefones
                {
                    ASSMED_Cadastro = assmed,
                    CodUsu = idUsuario,
                    DDD = Convert.ToInt32(iden.telefoneCelular.Substring(0, 2)),
                    DtSistema = DateTime.Now,
                    NumIP = requester,
                    NumTel = iden.telefoneCelular.Substring(2),
                    TipoTel = "C"
                }, n ? new string[0] : new[] {
                    nameof(ASSMED_CadTelefones.Codigo),
                    nameof(ASSMED_CadTelefones.NumContrato),
                    nameof(ASSMED_CadTelefones.IDTelefone)
                });

                if (n)
                {
                    telefone.NumContrato = 22;
                    telefone.Codigo = assmed.Codigo;
                    assmed.ASSMED_CadTelefones.Add(telefone);
                }
            }

            if (email == null && iden.emailCidadao != null)
            {
                With(ref email, () => new ASSMED_CadEmails
                {
                    Codigo = assmed.Codigo,
                    NumContrato = assmed.NumContrato,
                    ASSMED_Cadastro = assmed,
                    CodUsu = idUsuario,
                    EMail = iden.emailCidadao,
                    TipoEMail = "P",
                    DtSistema = DateTime.Now,
                    NumIP = requester
                });

                assmed.ASSMED_CadEmails.Add(email);
            }

            if (iden.cnsCidadao != null)
            {
                var n = cns == null;

                With(ref cns, () => new ASSMED_CadastroDocPessoal
                {
                    ASSMED_Cadastro = assmed,
                    CodTpDocP = 6,
                    CodUsu = idUsuario,
                    DtSistema = DateTime.Now,
                    Numero = iden.cnsCidadao,
                    NumIP = requester
                }, n ? new string[0] : new[] {
                    nameof(ASSMED_CadastroDocPessoal.Codigo),
                    nameof(ASSMED_CadastroDocPessoal.NumContrato)
                });

                if (n)
                {
                    cns.NumContrato = 22;
                    cns.Codigo = assmed.Codigo;
                    assmed.ASSMED_CadastroDocPessoal.Add(cns);
                }
            }

            if (iden.RG != null)
            {
                var n = rg == null;

                With(ref rg, () => new ASSMED_CadastroDocPessoal
                {
                    ASSMED_Cadastro = assmed,
                    CodTpDocP = 1,
                    CodUsu = idUsuario,
                    DtSistema = DateTime.Now,
                    Numero = iden.RG,
                    NumIP = requester
                }, n ? new string[0] : new[] { nameof(ASSMED_CadastroDocPessoal.Codigo), nameof(ASSMED_CadastroDocPessoal.NumContrato) });

                if (n)
                {
                    rg.NumContrato = 22;
                    rg.Codigo = assmed.Codigo;
                    assmed.ASSMED_CadastroDocPessoal.Add(rg);
                }
            }

            if (iden.numeroNisPisPasep != null)
            {
                var n = pis == null;

                With(ref pis, () => new ASSMED_CadastroDocPessoal
                {
                    ASSMED_Cadastro = assmed,
                    CodTpDocP = 7,
                    CodUsu = idUsuario,
                    DtSistema = DateTime.Now,
                    Numero = iden.numeroNisPisPasep,
                    NumIP = requester
                }, n ? new string[0] : new[] { nameof(ASSMED_CadastroDocPessoal.Codigo), nameof(ASSMED_CadastroDocPessoal.NumContrato) });

                if (n)
                {
                    rg.NumContrato = 22;
                    rg.Codigo = assmed.Codigo;
                    assmed.ASSMED_CadastroDocPessoal.Add(pis);
                }
            }

            pessoa.ASSMED_Cadastro = assmed;
            assmed.ASSMED_PesFisica = pessoa;

            if (novo)
            {
                Domain.ASSMED_Cadastro.Add(assmed);

                if (np)
                {
                    Domain.ASSMED_PesFisica.Add(pessoa);
                }
            }

            return assmed;
        }

        private static void Clear(object obj)
        {
            if (obj != null)
                foreach (PropertyInfo pi in obj.GetType().GetProperties())
                {
                    if (pi.PropertyType.Equals(typeof(string)))
                    {
                        var val = pi.GetValue(obj);

                        if (val == null || string.IsNullOrEmpty(val.ToString().Trim()) || string.IsNullOrWhiteSpace(val.ToString().Trim()))
                            pi.SetValue(obj, null);
                        else
                            pi.SetValue(obj, val.ToString().Trim());
                    }
                }
        }

        private void CleanStrings(CadastroIndividual cad)
        {
            Clear(cad.UnicaLotacaoTransport);
            Clear(cad);
            Clear(cad.IdentificacaoUsuarioCidadao1);
            Clear(cad.InformacoesSocioDemograficas1);
            Clear(cad.CondicoesDeSaude1);
            Clear(cad.EmSituacaoDeRua1);
            Clear(cad.SaidaCidadaoCadastro1);
        }

        private void CleanStrings(CadastroDomiciliar cad)
        {
            Clear(cad.UnicaLotacaoTransport);
            Clear(cad);
            Clear(cad.CondicaoMoradia1);
            Clear(cad.EnderecoLocalPermanencia1);
            cad.FamiliaRow.ToList().ForEach(Clear);
            Clear(cad.InstituicaoPermanencia1);
        }

        private void CleanStrings(FichaVisitaDomiciliarMaster cad)
        {
            Clear(cad.UnicaLotacaoTransport);
            Clear(cad);
            cad.FichaVisitaDomiciliarChild.ToList().ForEach(Clear);
        }
    }
}