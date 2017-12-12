using Softpark.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using System.Text.RegularExpressions;
using static Softpark.Infrastructure.Extensions.WithStatement;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Softpark.WS.ViewModels.SIGSM
{
    /// <summary>
    /// VM de listagem de cadastros
    /// </summary>
    public class CadastroIndividualVM
    {
        /// <summary>
        /// Cidadão
        /// </summary>
        public string NomeCidadao { get; set; }

        /// <summary>
        /// Data nasciemnto
        /// </summary>
        public DateTime? DataNascimento { get; set; }

        /// <summary>
        /// Nome da mãe
        /// </summary>
        public string NomeMae { get; set; }

        /// <summary>
        /// CNS
        /// </summary>
        public string CnsCidadao { get; set; }

        /// <summary>
        /// Municipio de nascimento
        /// </summary>
        public string MunicipioNascimento { get; set; }

        /// <summary>
        /// Código
        /// </summary>
        public decimal Codigo { get; set; }

        internal Guid? IdFicha { get; set; }
    }

    /// <summary>
    /// ficha do detalhe de identificação
    /// </summary>
    public class DetalheIdentificacaoUsuarioCidadaoViewModel : IdentificacaoUsuarioCidadaoViewModel
    {
        /// <summary>
        /// Código ASSMED_Cadastro
        /// </summary>
        public decimal? Codigo { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator DetalheIdentificacaoUsuarioCidadaoViewModel(IdentificacaoUsuarioCidadao model)
        {
            var vm = new DetalheIdentificacaoUsuarioCidadaoViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator DetalheIdentificacaoUsuarioCidadaoViewModel(VW_IdentificacaoUsuarioCidadao model)
        {
            var vm = new DetalheIdentificacaoUsuarioCidadaoViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(VW_IdentificacaoUsuarioCidadao model)
        {
            if (model == null) return;

            id = model.id;
            Codigo = model.Codigo;
            nomeSocial = model.nomeSocial;
            codigoIbgeMunicipioNascimento = model.codigoIbgeMunicipioNascimento;
            dataNascimentoCidadao = model.dataNascimentoCidadao;
            desconheceNomeMae = model.desconheceNomeMae ?? false;
            emailCidadao = model.emailCidadao;
            nacionalidadeCidadao = model.nacionalidadeCidadao ?? 1;
            nomeCidadao = model.nomeCidadao;
            nomeMaeCidadao = model.nomeMaeCidadao;
            cnsCidadao = model.cnsCidadao;
            cnsResponsavelFamiliar = model.cnsResponsavelFamiliar;
            telefoneCelular = model.telefoneCelular;
            numeroNisPisPasep = model.numeroNisPisPasep;
            paisNascimento = model.paisNascimento;
            racaCorCidadao = model.racaCorCidadao;
            sexoCidadao = model.sexoCidadao;
            statusEhResponsavel = model.statusEhResponsavel ?? false;
            etnia = model.etnia;
            nomePaiCidadao = model.nomePaiCidadao;
            desconheceNomePai = model.desconheceNomePai ?? false;
            dtNaturalizacao = model.dtNaturalizacao;
            portariaNaturalizacao = model.portariaNaturalizacao;
            dtEntradaBrasil = model.dtEntradaBrasil;
            microarea = model.microarea;
            stForaArea = model.stForaArea ?? false;
            RG = model.RG;
            ComplementoRG = model.ComplementoRG;
            CPF = model.CPF;
            beneficiarioBolsaFamilia = model.beneficiarioBolsaFamilia ?? false;
            EstadoCivil = model.EstadoCivil;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(IdentificacaoUsuarioCidadao model)
        {
            if (model == null) return;

            id = model.id;
            Codigo = model.Codigo;
            nomeSocial = model.nomeSocial;
            codigoIbgeMunicipioNascimento = model.codigoIbgeMunicipioNascimento;
            dataNascimentoCidadao = model.dataNascimentoCidadao;
            desconheceNomeMae = model.desconheceNomeMae;
            emailCidadao = model.emailCidadao;
            nacionalidadeCidadao = model.nacionalidadeCidadao;
            nomeCidadao = model.nomeCidadao;
            nomeMaeCidadao = model.nomeMaeCidadao;
            cnsCidadao = model.cnsCidadao;
            cnsResponsavelFamiliar = model.cnsResponsavelFamiliar;
            telefoneCelular = model.telefoneCelular;
            numeroNisPisPasep = model.numeroNisPisPasep;
            paisNascimento = model.paisNascimento;
            racaCorCidadao = model.racaCorCidadao;
            sexoCidadao = model.sexoCidadao;
            statusEhResponsavel = model.statusEhResponsavel;
            etnia = model.etnia;
            nomePaiCidadao = model.nomePaiCidadao;
            desconheceNomePai = model.desconheceNomePai;
            dtNaturalizacao = model.dtNaturalizacao;
            portariaNaturalizacao = model.portariaNaturalizacao;
            dtEntradaBrasil = model.dtEntradaBrasil;
            microarea = model.microarea;
            stForaArea = model.stForaArea;
            RG = model.RG;
            ComplementoRG = model.ComplementoRG;
            CPF = model.CPF;
            beneficiarioBolsaFamilia = model.beneficiarioBolsaFamilia ?? false;
            EstadoCivil = model.EstadoCivil;
        }

        internal override IdentificacaoUsuarioCidadao ToModel(DomainContainer domain)
        {
            var iuc = base.ToModel(domain);

            iuc.Codigo = Codigo;
            iuc.num_contrato = 22;

            return iuc;
        }
    }

    /// <summary>
    /// Ficha de detalhe do cadastro individual
    /// </summary>
    public class DetalheCadastroIndividualVW : CadastroIndividualViewModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid uuid { get; set; }

        /// <summary>
        /// Identificação do usuário cidadão
        /// </summary>
        public new DetalheIdentificacaoUsuarioCidadaoViewModel identificacaoUsuarioCidadao { get; set; } = null;

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator DetalheCadastroIndividualVW(CadastroIndividual model)
        {
            var vm = new DetalheCadastroIndividualVW();

            if (model != null)
                vm.uuid = model.id;

            vm.ApplyModel(model);

            vm.identificacaoUsuarioCidadao = model.IdentificacaoUsuarioCidadao1;

            return vm;
        }

        internal override async Task<CadastroIndividual> ToModel(DomainContainer domain)
        {
            var cad = await base.ToModel(domain);

            cad.IdentificacaoUsuarioCidadao1 = identificacaoUsuarioCidadao.ToModel(domain);

            return cad;
        }
    }

    /// <summary>
    /// Formulário de Cadastro
    /// </summary>
    public class FormCadastroIndividual
    {
        /// <summary>
        /// Ficha finalizada?
        /// </summary>
        public bool Finalizado { get; set; }

        /// <summary>
        /// Cabeçalho de transporte
        /// </summary>
        public UnicaLotacaoTransportCadastroViewModel CabecalhoTransporte { get; set; }

        /// <summary>
        /// Ficha de Cadastro Individual
        /// </summary>
        public DetalheCadastroIndividualVW CadastroIndividual { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator FormCadastroIndividual(CadastroIndividual model)
        {
            return new FormCadastroIndividual
            {
                CadastroIndividual = model,
                CabecalhoTransporte = model.UnicaLotacaoTransport
            };
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        public static async Task<CadastroIndividual> ToModel(FormCadastroIndividual model, DomainContainer db)
        {
            var cad = await model.CadastroIndividual.ToModel(db);

            cad.UnicaLotacaoTransport = model;

            if (cad.IdentificacaoUsuarioCidadao1 != null)
            {
                var cod = model.CadastroIndividual.identificacaoUsuarioCidadao?.Codigo;

                var asc = await db.ASSMED_Cadastro.SingleOrDefaultAsync(x => x.Codigo == cod);

                cad.IdentificacaoUsuarioCidadao1.Codigo = asc?.Codigo;
                cad.IdentificacaoUsuarioCidadao1.ASSMED_Cadastro1 = asc;

                if (asc != null)
                {
                    asc.IdFicha = cad.IdentificacaoUsuarioCidadao1.id;
                }
            }

            return cad;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator UnicaLotacaoTransport(FormCadastroIndividual model) =>
            model.CabecalhoTransporte;

        private void Clear(object obj)
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

        private void CleanStrings()
        {
            Clear(CabecalhoTransporte);
            Clear(CadastroIndividual);
            Clear(CadastroIndividual.identificacaoUsuarioCidadao);
            Clear(CadastroIndividual.informacoesSocioDemograficas);
            Clear(CadastroIndividual.condicoesDeSaude);
            Clear(CadastroIndividual.emSituacaoDeRua);
            Clear(CadastroIndividual.saidaCidadaoCadastro);
        }
        
        /// <summary>
        /// Este método é responsável por tratar e processar os cadastros individuais
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        private async Task<CadastroIndividual> ProcessarIndividuo(DomainContainer domain)
        {
            // cria um token
            var origem = domain.OrigemVisita.Create();

            origem.token = Guid.NewGuid();
            origem.id_tipo_origem = 2;
            origem.enviarParaThrift = true;
            origem.enviado = false;
            origem.finalizado = true;

            domain.OrigemVisita.Add(origem);

            // realiza o DataBind do cabeçalho
            var h = domain.UnicaLotacaoTransport.Create();
            var header = CabecalhoTransporte;

            h.id = Guid.NewGuid();
            h.OrigemVisita = origem;
            h.token = origem.token;
            h.cboCodigo_2002 = header.cboCodigo_2002;
            h.cnes = header.cnes;
            h.codigoIbgeMunicipio = header.codigoIbgeMunicipio;
            h.dataAtendimento = header.dataAtendimento;
            h.ine = header.ine;
            h.profissionalCNS = header.profissionalCNS;

            domain.UnicaLotacaoTransport.Add(h);

            // realiza o DataBind
            var cad = await CadastroIndividual.ToModel(domain);
            cad.tpCdsOrigem = 3;
            cad.UnicaLotacaoTransport = h;
            cad.DataRegistro = DateTime.Now;
            
            // registra o modelo
            domain.CadastroIndividual.Add(cad);

            return cad;
        }

        /// <summary>
        /// Limpeza dos dados
        /// </summary>
        /// <param name="db"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Guid> LimparESalvarDados(DomainContainer db, UrlHelper url)
        {
            var restDn = JsonConvert.SerializeObject(this);

            CleanStrings();

            CabecalhoTransporte.codigoIbgeMunicipio = db.Database.SqlQuery<ASSMED_Contratos>("SELECT * FROM ASSMED_Contratos").First().CodigoIbgeMunicipio;
            
            if (CabecalhoTransporte.ine != null)
            {
                int.TryParse(CabecalhoTransporte.ine, out int ine);

                var setorPar = (await db.AS_SetoresPar.FirstOrDefaultAsync(x => x.CNES != null && x.CNES.Trim() == CabecalhoTransporte.cnes))?.CodSetor;

                CabecalhoTransporte.ine = (await db.SetoresINEs.FirstOrDefaultAsync(x => x.CodINE == ine && x.CodSetor == setorPar))?.Numero;
            }

            var profissional = db.GetProfissional("CadastroIndividual", CabecalhoTransporte.cnes, CabecalhoTransporte.ine, CabecalhoTransporte.profissionalCNS);

            if (profissional != null)
            {
                CabecalhoTransporte.cboCodigo_2002 = profissional.CBO;
            }

            var codigo = CadastroIndividual.identificacaoUsuarioCidadao?.Codigo;
            var cpf = CadastroIndividual.identificacaoUsuarioCidadao?.CPF;
            var rg = CadastroIndividual.identificacaoUsuarioCidadao?.RG;
            var cns = CadastroIndividual.identificacaoUsuarioCidadao?.cnsCidadao;
            var pis = CadastroIndividual.identificacaoUsuarioCidadao?.numeroNisPisPasep;

            // Busca a última ficha do cadastro
            var ultimaFicha = await db.CadastroIndividual.SingleOrDefaultAsync(x => x.id == CadastroIndividual.uuidFichaOriginadora);

            var assmedCadastro = ultimaFicha?.IdentificacaoUsuarioCidadao1?.ASSMED_Cadastro1 ??
                await (db.ASSMED_Cadastro.SingleOrDefaultAsync(x => x.Codigo == codigo) ??
                db.ASSMED_CadastroDocPessoal.Where(x => x.Numero != null && x.Numero.Trim().Length > 0 && x.Numero == cns && x.CodTpDocP == 6)
                    .Select(x => x.ASSMED_Cadastro).FirstOrDefaultAsync() ??
                db.ASSMED_PesFisica.Where(x => x.CPF == cpf).Select(x => x.ASSMED_Cadastro).FirstOrDefaultAsync() ??
                db.ASSMED_CadastroDocPessoal.Where(x => x.Numero != null && x.Numero.Trim().Length > 0 && x.Numero == rg && x.CodTpDocP == 1)
                    .Select(x => x.ASSMED_Cadastro).FirstOrDefaultAsync() ??
                db.ASSMED_CadastroDocPessoal.Where(x => x.Numero != null && x.Numero.Trim().Length > 0 && x.Numero == pis && x.CodTpDocP == 7)
                    .Select(x => x.ASSMED_Cadastro).FirstOrDefaultAsync());

            codigo = assmedCadastro?.Codigo;

            if (ultimaFicha == null && assmedCadastro != null)
                ultimaFicha = await db.CadastroIndividual.SingleOrDefaultAsync(x => x.identificacaoUsuarioCidadao == assmedCadastro.IdFicha);

            CadastroIndividual.uuid = Guid.NewGuid();
            CadastroIndividual.fichaAtualizada = true;

            if (assmedCadastro == null && CadastroIndividual.uuidFichaOriginadora == null)
            {
                CadastroIndividual.uuidFichaOriginadora = CadastroIndividual.uuid;
                CadastroIndividual.fichaAtualizada = false;
            }
            
            SIGSM_Transmissao_Processos proc = await db.SIGSM_Transmissao_Processos.FindAsync(ultimaFicha?.id);

            var updateAssmed = assmedCadastro == null || ultimaFicha == null || (ultimaFicha.UnicaLotacaoTransport.dataAtendimento <= CabecalhoTransporte.dataAtendimento);

            var dadoAnterior = ultimaFicha;

            if (assmedCadastro != null)
            {
                // Verifica se a ficha de origem existe E possui uma ficha de origem E
                // se ainda não foi enviada E o tipo de origem não é a API
                while (ultimaFicha != null &&
                    ultimaFicha.uuidFichaOriginadora != null &&
                    ultimaFicha.uuidFichaOriginadora != ultimaFicha.id &&
                    !ultimaFicha.UnicaLotacaoTransport.OrigemVisita.enviado &&
                    ultimaFicha.UnicaLotacaoTransport.OrigemVisita.id_tipo_origem != 1)
                {
                    // marca a ficha de origem para não ser enviada
                    if (updateAssmed)
                        ultimaFicha.UnicaLotacaoTransport.OrigemVisita.enviarParaThrift = false;

                    // busca a origem da origem
                    var uf = await db.CadastroIndividual.FindAsync(ultimaFicha.uuidFichaOriginadora);

                    // atribui a ficha de origem da origem à origem da ficha à ser criada
                    // se existir
                    if (uf != null)
                        ultimaFicha = uf;
                    else
                        break;
                }

                if (updateAssmed && ultimaFicha != null &&
                    !ultimaFicha.UnicaLotacaoTransport.OrigemVisita.enviado &&
                    ultimaFicha.UnicaLotacaoTransport.OrigemVisita.id_tipo_origem != 1)
                    ultimaFicha.UnicaLotacaoTransport.OrigemVisita.enviarParaThrift = false;

                if (ultimaFicha != null)
                {
                    CadastroIndividual.uuidFichaOriginadora = ultimaFicha.id;

                    CadastroIndividual.fichaAtualizada = true;
                }
            }

            if (CadastroIndividual.identificacaoUsuarioCidadao != null)
            {
                CadastroIndividual.identificacaoUsuarioCidadao.Codigo = codigo;

                var iden = CadastroIndividual.identificacaoUsuarioCidadao;

                int? codCidade = !string.IsNullOrEmpty(iden.codigoIbgeMunicipioNascimento?.Trim()) ?
                    Convert.ToInt32(iden.codigoIbgeMunicipioNascimento) : (int?)null;

                var cidade = await db.Cidade.SingleOrDefaultAsync(x => x.CodCidade == codCidade);

                iden.codigoIbgeMunicipioNascimento = cidade?.CodIbge;

                var pais = await db.Nacionalidade.SingleOrDefaultAsync(x => x.CodNacao == iden.paisNascimento);

                iden.paisNascimento = pais?.codigo;

                var nacionalidade = await db.TP_Nacionalidade.SingleOrDefaultAsync(x => x.id_tp_nacionalidade == iden.nacionalidadeCidadao);

                iden.nacionalidadeCidadao = nacionalidade?.codigo ?? 1;
            }

            if (CadastroIndividual.condicoesDeSaude != null)
            {
                var doencas =
                    CadastroIndividual.condicoesDeSaude.doencaCardiaca.Length == 0 ? new int[0] :
                    await db.TP_Doenca_Cardiaca.Where(x => CadastroIndividual.condicoesDeSaude.doencaCardiaca.Contains(x.id_tp_doenca_cardiaca))
                    .Select(x => x.codigo).Distinct().ToArrayAsync();

                CadastroIndividual.condicoesDeSaude.doencaCardiaca = doencas;

                doencas =
                    CadastroIndividual.condicoesDeSaude.doencaRespiratoria.Length == 0 ? new int[0] :
                    await db.TP_Doenca_Respiratoria.Where(x => CadastroIndividual.condicoesDeSaude.doencaRespiratoria.Contains(x.id_tp_doenca_respiratoria))
                    .Select(x => x.codigo).Distinct().ToArrayAsync();

                CadastroIndividual.condicoesDeSaude.doencaRespiratoria = doencas;

                doencas =
                    CadastroIndividual.condicoesDeSaude.doencaRins.Length == 0 ? new int[0] :
                    await db.TP_Doenca_Renal.Where(x => CadastroIndividual.condicoesDeSaude.doencaRins.Contains(x.id_tp_doenca_renal))
                    .Select(x => x.codigo).Distinct().ToArrayAsync();

                CadastroIndividual.condicoesDeSaude.doencaRins = doencas;
            }

            if (CadastroIndividual.emSituacaoDeRua != null)
            {
                var sit = await db.TP_Sit_Rua.SingleOrDefaultAsync(x => x.id_tp_sit_rua == CadastroIndividual.emSituacaoDeRua.tempoSituacaoRua);

                CadastroIndividual.emSituacaoDeRua.tempoSituacaoRua = sit?.codigo;

                var hig = await db.TP_Higiene_Pessoal.Where(x => CadastroIndividual.emSituacaoDeRua.higienePessoalSituacaoRua.Contains(x.id_tp_higiene_pessoal))
                    .Select(x => x.codigo).Distinct().ToArrayAsync();

                CadastroIndividual.emSituacaoDeRua.higienePessoalSituacaoRua = hig;

                var ori = await db.TP_Origem_Alimentacao.Where(x => CadastroIndividual.emSituacaoDeRua.origemAlimentoSituacaoRua.Contains(x.id_tp_origem_alimentacao))
                    .Select(x => x.codigo).Distinct().ToArrayAsync();

                CadastroIndividual.emSituacaoDeRua.origemAlimentoSituacaoRua = ori;
            }

            if (CadastroIndividual.informacoesSocioDemograficas != null)
            {
                var inst = await db.TP_Curso.SingleOrDefaultAsync(x => x.id_tp_curso == CadastroIndividual.informacoesSocioDemograficas.grauInstrucaoCidadao);

                CadastroIndividual.informacoesSocioDemograficas.grauInstrucaoCidadao = inst?.codigo;

                var defs = await db.TP_Deficiencia.Where(x => CadastroIndividual.informacoesSocioDemograficas.deficienciasCidadao.Contains(x.id_tp_deficiencia))
                    .Select(x => x.codigo).Distinct().ToArrayAsync();

                CadastroIndividual.informacoesSocioDemograficas.deficienciasCidadao = defs;

                var resp = await db.TP_Crianca.Where(x => CadastroIndividual.informacoesSocioDemograficas.responsavelPorCrianca.Contains(x.id_tp_crianca))
                    .Select(x => x.codigo).Distinct().ToArrayAsync();

                CadastroIndividual.informacoesSocioDemograficas.responsavelPorCrianca = resp;

                var merc = await db.TP_Sit_Mercado.SingleOrDefaultAsync(x => x.id_tp_sit_mercado == CadastroIndividual.informacoesSocioDemograficas.situacaoMercadoTrabalhoCidadao);

                CadastroIndividual.informacoesSocioDemograficas.situacaoMercadoTrabalhoCidadao = merc?.codigo;

                CadastroIndividual.informacoesSocioDemograficas.ocupacaoCodigoCbo2002 = CadastroIndividual.informacoesSocioDemograficas.ocupacaoCodigoCbo2002?.Trim();
            }

            CleanStrings();

            var cad = await ProcessarIndividuo(db);
            
            if (cad.IdentificacaoUsuarioCidadao1 != null)
            {
                cad.IdentificacaoUsuarioCidadao1.Codigo = assmedCadastro?.Codigo ?? CadastroIndividual.identificacaoUsuarioCidadao?.Codigo;

                if (CadastroIndividual.identificacaoUsuarioCidadao != null)
                    CadastroIndividual.identificacaoUsuarioCidadao.Codigo = cad.IdentificacaoUsuarioCidadao1.Codigo;

                cad.IdentificacaoUsuarioCidadao1.ASSMED_Cadastro1 = assmedCadastro;

                if (assmedCadastro != null && updateAssmed)
                {
                    assmedCadastro.IdFicha = cad.IdentificacaoUsuarioCidadao1.id;
                }
            }
            else if (CadastroIndividual.identificacaoUsuarioCidadao != null)
            {
                CadastroIndividual.identificacaoUsuarioCidadao.Codigo = assmedCadastro?.Codigo ?? cad.IdentificacaoUsuarioCidadao1?.Codigo;

                if (cad.IdentificacaoUsuarioCidadao1 != null)
                {
                    cad.IdentificacaoUsuarioCidadao1.Codigo = CadastroIndividual.identificacaoUsuarioCidadao.Codigo;

                    cad.IdentificacaoUsuarioCidadao1.ASSMED_Cadastro1 = assmedCadastro;
                }

                if (assmedCadastro != null && updateAssmed)
                {
                    assmedCadastro.IdFicha = cad.IdentificacaoUsuarioCidadao1.id;
                }
            }

            var header = cad.UnicaLotacaoTransport;
            
            cad.Validar(db);

            var da = dadoAnterior == null ? null :
                new FormCadastroIndividual
                {
                    CadastroIndividual = dadoAnterior,
                    CabecalhoTransporte = UnicaLotacaoTransportCadastroViewModel.ApplyModel(dadoAnterior.UnicaLotacaoTransport, db)
                };

            var restDa = da == null ? null : JsonConvert.SerializeObject(da);

            var rastro = new RastroFicha
            {
                CodUsu = Convert.ToInt32(ASPSessionVar.Read("idUsuario")),
                DataModificacao = DateTime.Now,
                token = header.token,
                DadoAnterior = restDa,
                DadoAtual = restDn
            };

            header.OrigemVisita.enviarParaThrift = updateAssmed;

            db.RastroFicha.Add(rastro);

            if (proc != null)
            {
                db.SIGSM_Transmissao_Processos_Log.RemoveRange(proc.SIGSM_Transmissao_Processos_Log);
                db.SIGSM_Transmissao_Processos.Remove(proc);
            }

            await db.SaveChangesAsync();

            if (updateAssmed)
            {
                var assmed = await GerarCadastroAssmed(cad, db, url);

                if (assmed != null)
                {
                    try
                    {
                        cad.IdentificacaoUsuarioCidadao1.ASSMED_Cadastro1 = assmed;
                        cad.IdentificacaoUsuarioCidadao1.Codigo = assmed.Codigo;
                        cad.IdentificacaoUsuarioCidadao1.num_contrato = assmed.NumContrato;

                        await db.SaveChangesAsync();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            else
            {
                throw new ValidationException("A Ficha foi salva como histórico, pois, já existe uma ficha mais atual para este cadastro.");
            }

            return cad.id;
        }

        private void Dirty(object obj)
        {
            if (obj != null)
                foreach (PropertyInfo pi in obj.GetType().GetProperties())
                {
                    if (pi.PropertyType.Equals(typeof(string)))
                    {
                        var val = pi.GetValue(obj);

                        if (val == null || string.IsNullOrEmpty(val.ToString().Trim()) || string.IsNullOrWhiteSpace(val.ToString().Trim()))
                            pi.SetValue(obj, string.Empty);
                        else
                            pi.SetValue(obj, val.ToString().Trim());
                    }
                }
        }

        private void DirtyStrings()
        {
            Dirty(CabecalhoTransporte);
            Dirty(CadastroIndividual);
            Dirty(CadastroIndividual.identificacaoUsuarioCidadao);
            Dirty(CadastroIndividual.informacoesSocioDemograficas);
            Dirty(CadastroIndividual.condicoesDeSaude);
            Dirty(CadastroIndividual.emSituacaoDeRua);
            Dirty(CadastroIndividual.saidaCidadaoCadastro);
        }

        internal FormCadastroIndividual ToDetail()
        {
            DirtyStrings();

            return this;
        }

        internal static async Task<ASSMED_Cadastro> GerarCadastroAssmed(CadastroIndividual cad, DomainContainer db, UrlHelper url)
        {
            if (cad.IdentificacaoUsuarioCidadao1 == null) return null;

            var iden = cad.IdentificacaoUsuarioCidadao1;

            var requester = (url.Request.Properties.ContainsKey("MS_HttpContext") ? url.Request.Properties["MS_HttpContext"] as HttpContextWrapper : null)?
                .Request.UserHostAddress ?? (HttpContext.Current?.Request?.UserHostAddress);

            var def = cad.InformacoesSocioDemograficas1?.DeficienciasCidadao.FirstOrDefault()?.id_tp_deficiencia_cidadao;
            var curso = cad.InformacoesSocioDemograficas1?.grauInstrucaoCidadao;
            curso = (await db.TP_Curso.SingleOrDefaultAsync(x => x.codigo == curso))?.id_tp_curso;

            var codigoIbgeMunicipioNascimento = iden?.codigoIbgeMunicipioNascimento;

            var cidade = (await db.Cidade.FirstOrDefaultAsync(x => x.CodIbge == codigoIbgeMunicipioNascimento && x.CodIbge != null));

            var deficiencia = (await db.TP_Deficiencia.SingleOrDefaultAsync(x => x.codigo == def))?.id_tp_deficiencia;

            var paisNascimento = iden?.paisNascimento;

            var nacao = (await db.Nacionalidade.FirstOrDefaultAsync(x => x.codigo == paisNascimento && x.codigo != null))?.CodNacao;

            var cpf = iden?.CPF;
            if (cpf != null)
            {
                cpf = Regex.Replace(cpf, "([0-9]{3})([0-9]{3})([0-9]{3})([0-9]{2})", "$1.$2.$3-$4");
            }

            var assmed = iden.ASSMED_Cadastro1;

            var pessoa = assmed?.ASSMED_PesFisica;

            var codigo = assmed?.Codigo;

            var idUsuario = Convert.ToInt32(ASPSessionVar.Read("idUsuario"));

            var novo = codigo == null;

            ASSMED_CadastroDocPessoal cns = null;
            ASSMED_CadastroDocPessoal rg = null;
            ASSMED_CadastroDocPessoal pis = null;
            
            if (iden.cnsCidadao != null && novo)
            {
                var _cns = iden.cnsCidadao;

                var numCnss = await db.ASSMED_CadastroDocPessoal.CountAsync(x => x.Numero == _cns && x.CodTpDocP == 6);

                if (numCnss == 1)
                {
                    cns = await db.ASSMED_CadastroDocPessoal.FirstOrDefaultAsync(x => x.Numero == _cns && x.CodTpDocP == 6);

                    assmed = cns.ASSMED_Cadastro;

                    pessoa = assmed.ASSMED_PesFisica;

                    codigo = assmed.Codigo;

                    novo = false;
                }
            }

            if (novo && cpf != null)
            {
                var numCpfs = await db.ASSMED_PesFisica.CountAsync(x => x.CPF == cpf || x.CPF == iden.CPF);

                if (numCpfs == 1)
                {
                    pessoa = await db.ASSMED_PesFisica.SingleAsync(x => x.CPF == cpf || x.CPF == iden.CPF);

                    assmed = pessoa.ASSMED_Cadastro;

                    codigo = assmed.Codigo;

                    novo = false;
                }
            }

            if (novo && iden.RG != null)
            {
                var _rg = iden.RG;
                var _rg2 = Regex.Replace(@"([.\/-])", "", iden.RG);

                var numRgs = await db.ASSMED_CadastroDocPessoal.CountAsync(x => (x.Numero == _rg || x.Numero == _rg2) && x.CodTpDocP == 1);

                if (numRgs == 1)
                {
                    rg = await db.ASSMED_CadastroDocPessoal.SingleAsync(x => (x.Numero == _rg || x.Numero == _rg2) && x.CodTpDocP == 1);

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

                var numPiss = await db.ASSMED_CadastroDocPessoal.CountAsync(x => (x.Numero == _pis || x.Numero == _pis2) && x.CodTpDocP == 7);

                if (numPiss == 1)
                {
                    pis = await db.ASSMED_CadastroDocPessoal.SingleAsync(x => (x.Numero == _pis || x.Numero == _pis2) && x.CodTpDocP == 7);

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

            if (novo)
            {
                codigo = await db.ASSMED_Cadastro.MaxAsync(x => x.Codigo + 1);
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
                
                if (n)
                {
                    cns = new ASSMED_CadastroDocPessoal
                    {
                        ASSMED_Cadastro = assmed,
                        CodTpDocP = 6,
                        CodUsu = idUsuario,
                        DtSistema = DateTime.Now,
                        Numero = iden.cnsCidadao,
                        NumIP = requester,
                        NumContrato = 22,
                        Codigo = assmed.Codigo
                    };
                    
                    assmed.ASSMED_CadastroDocPessoal.Add(cns);
                }
                else
                {
                    cns.CodUsu = idUsuario;
                    cns.DtSistema = DateTime.Now;
                    cns.Numero = iden.cnsCidadao;
                    cns.NumIP = requester;
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
                db.ASSMED_Cadastro.Add(assmed);

                if (np)
                {
                    db.ASSMED_PesFisica.Add(pessoa);
                }
            }

            return assmed;
        }

        internal static async Task<FormCadastroIndividual> Apply(CadastroIndividual data, DomainContainer db)
        {
            var form = new FormCadastroIndividual
            {
                CabecalhoTransporte = UnicaLotacaoTransportCadastroViewModel.ApplyModel(data.UnicaLotacaoTransport, db),
                CadastroIndividual = data
            };

            form.CabecalhoTransporte.profissionalNome = db.VW_Profissional.FirstOrDefault(x => x.CNS == form.CabecalhoTransporte.profissionalCNS)?.Nome;

            form.Finalizado = data.UnicaLotacaoTransport.OrigemVisita.finalizado;

            if (form.CadastroIndividual.identificacaoUsuarioCidadao != null)
            {
                var iden = form.CadastroIndividual.identificacaoUsuarioCidadao;

                var cidade = await db.Cidade.SingleOrDefaultAsync(x => x.CodIbge == iden.codigoIbgeMunicipioNascimento && x.CodIbge != null);

                iden.codigoIbgeMunicipioNascimento = cidade?.CodCidade.ToString();

                var pais = await db.Nacionalidade.FirstOrDefaultAsync(x => x.codigo == iden.paisNascimento && x.codigo != null);

                iden.paisNascimento = pais?.CodNacao;

                form.CadastroIndividual.identificacaoUsuarioCidadao.nacionalidadeCidadao = data.IdentificacaoUsuarioCidadao1.nacionalidadeCidadao;
            }

            if (form.CadastroIndividual.condicoesDeSaude != null)
            {
                var cond = form.CadastroIndividual.condicoesDeSaude;

                var doencas =
                    await db.TP_Doenca_Cardiaca.Where(x => cond.doencaCardiaca.Contains(x.codigo))
                    .Select(x => x.id_tp_doenca_cardiaca).Distinct().ToArrayAsync();

                form.CadastroIndividual.condicoesDeSaude.doencaCardiaca = doencas;

                doencas =
                    await db.TP_Doenca_Respiratoria.Where(x => cond.doencaRespiratoria.Contains(x.codigo))
                    .Select(x => x.id_tp_doenca_respiratoria).Distinct().ToArrayAsync();

                cond.doencaRespiratoria = doencas;

                doencas =
                    await db.TP_Doenca_Renal.Where(x => cond.doencaRins.Contains(x.codigo))
                    .Select(x => x.id_tp_doenca_renal).Distinct().ToArrayAsync();

                cond.doencaRins = doencas;
            }

            if (form.CadastroIndividual.emSituacaoDeRua != null)
            {
                var emsit = form.CadastroIndividual.emSituacaoDeRua;

                var sit = await db.TP_Sit_Rua.SingleOrDefaultAsync(x => x.codigo == emsit.tempoSituacaoRua);

                emsit.tempoSituacaoRua = sit?.id_tp_sit_rua;

                var hig = await db.TP_Higiene_Pessoal.Where(x => emsit.higienePessoalSituacaoRua.Contains(x.codigo))
                    .Select(x => x.id_tp_higiene_pessoal).Distinct().ToArrayAsync();

                emsit.higienePessoalSituacaoRua = hig;

                var ori = await db.TP_Origem_Alimentacao.Where(x => emsit.origemAlimentoSituacaoRua.Contains(x.codigo))
                    .Select(x => x.id_tp_origem_alimentacao).Distinct().ToArrayAsync();

                emsit.origemAlimentoSituacaoRua = ori;
            }

            if (form.CadastroIndividual.informacoesSocioDemograficas != null)
            {
                var info = form.CadastroIndividual.informacoesSocioDemograficas;

                var inst = await db.TP_Curso.SingleOrDefaultAsync(x => x.codigo == info.grauInstrucaoCidadao);

                info.grauInstrucaoCidadao = inst?.id_tp_curso;

                var defs = await db.TP_Deficiencia.Where(x => info.deficienciasCidadao.Contains(x.codigo))
                    .Select(x => x.id_tp_deficiencia).Distinct().ToArrayAsync();

                info.deficienciasCidadao = defs;

                var resp = await db.TP_Crianca.Where(x => info.responsavelPorCrianca.Contains(x.codigo))
                    .Select(x => x.id_tp_crianca).Distinct().ToArrayAsync();

                info.responsavelPorCrianca = resp;

                var merc = await db.TP_Sit_Mercado.SingleOrDefaultAsync(x => x.codigo == info.situacaoMercadoTrabalhoCidadao);

                info.situacaoMercadoTrabalhoCidadao = merc?.id_tp_sit_mercado;

                info.ocupacaoCodigoCbo2002 = info.ocupacaoCodigoCbo2002?.Trim();
            }

            return form;
        }
    }
}