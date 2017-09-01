using Softpark.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web.Http;
using System.Text.RegularExpressions;
using static Softpark.Infrastructure.Extensions.WithStatement;

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
        public static implicit operator CadastroIndividual(FormCadastroIndividual model)
        {
            var tsk = Task.Run(async () =>
            {
                var cad = await model.CadastroIndividual.ToModel(DomainContainer.Current);

                cad.UnicaLotacaoTransport = model;

                if (cad.IdentificacaoUsuarioCidadao1 != null)
                {
                    var cod = model.CadastroIndividual.identificacaoUsuarioCidadao?.Codigo ?? 0;

                    var asc = await DomainContainer.Current.ASSMED_Cadastro.SingleOrDefaultAsync(x => x.Codigo == cod);

                    cad.IdentificacaoUsuarioCidadao1.Codigo = asc?.Codigo;
                    cad.IdentificacaoUsuarioCidadao1.ASSMED_Cadastro1 = asc;

                    if (asc != null)
                    {
                        asc.IdFicha = cad.IdentificacaoUsuarioCidadao1.id;
                    }
                }

                return cad;
            });

            tsk.Wait();

            return tsk.Result;
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
        /// Limpeza dos dados
        /// </summary>
        /// <param name="db"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Guid> LimparESalvarDados(DomainContainer db, UrlHelper url)
        {
            CleanStrings();

            CabecalhoTransporte.codigoIbgeMunicipio = db.ASSMED_Contratos.First().CodigoIbgeMunicipio;

            var codigo = CadastroIndividual.identificacaoUsuarioCidadao?.Codigo;
            var cpf = CadastroIndividual.identificacaoUsuarioCidadao?.CPF;
            var rg = CadastroIndividual.identificacaoUsuarioCidadao?.RG;
            var pis = CadastroIndividual.identificacaoUsuarioCidadao?.numeroNisPisPasep;

            var assmedCadastro = await (db.ASSMED_Cadastro.SingleOrDefaultAsync(x => x.Codigo == codigo) ??
                db.ASSMED_PesFisica.Where(x => x.CPF == cpf).Select(x => x.ASSMED_Cadastro).FirstOrDefaultAsync() ??
                db.ASSMED_CadastroDocPessoal.Where(x => x.Numero != null && x.Numero.Trim().Length > 0 && x.Numero == rg && x.CodTpDocP == 1)
                    .Select(x => x.ASSMED_Cadastro).FirstOrDefaultAsync() ??
                db.ASSMED_CadastroDocPessoal.Where(x => x.Numero != null && x.Numero.Trim().Length > 0 && x.Numero == pis && x.CodTpDocP == 1)
                    .Select(x => x.ASSMED_Cadastro).FirstOrDefaultAsync());

            CadastroIndividual.uuid = Guid.NewGuid();
            CadastroIndividual.uuidFichaOriginadora = CadastroIndividual.uuid;

            if (assmedCadastro != null)
            {
                codigo = assmedCadastro.Codigo;

                var ultimaFicha = await db.CadastroIndividual.SingleOrDefaultAsync(x => x.identificacaoUsuarioCidadao == assmedCadastro.IdFicha);

                if (ultimaFicha != null)
                {
                    CadastroIndividual.uuidFichaOriginadora = ultimaFicha.id;

                    CadastroIndividual.fichaAtualizada = true;
                }
            }

            var profissional = db.VW_Profissional.Where(x => x.CNS == CabecalhoTransporte.profissionalCNS &&
            x.CNES == CabecalhoTransporte.cnes).ToArray().FirstOrDefault(x => x.INE == CabecalhoTransporte.ine || x.INE == null);

            if (profissional != null)
            {
                CabecalhoTransporte.cboCodigo_2002 = profissional.CBO;
            }

            if (CadastroIndividual.identificacaoUsuarioCidadao != null)
            {
                CadastroIndividual.identificacaoUsuarioCidadao.Codigo = codigo;

                var iden = CadastroIndividual.identificacaoUsuarioCidadao;

                var codCidade = iden.codigoIbgeMunicipioNascimento != null && !string.IsNullOrEmpty(iden.codigoIbgeMunicipioNascimento.Trim()) ?
                    Convert.ToInt32(iden.codigoIbgeMunicipioNascimento) : 0;

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

                var peso = await db.TP_Consideracao_Peso.SingleOrDefaultAsync(x => x.id_tp_consideracao_peso == CadastroIndividual.condicoesDeSaude.situacaoPeso);

                CadastroIndividual.condicoesDeSaude.situacaoPeso = peso?.codigo;
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

                var qtda = await db.TP_Quantas_Vezes_Alimentacao
                    .SingleOrDefaultAsync(x => x.id_tp_quantas_vezes_alimentacao == CadastroIndividual.emSituacaoDeRua.quantidadeAlimentacoesAoDiaSituacaoRua);

                CadastroIndividual.emSituacaoDeRua.quantidadeAlimentacoesAoDiaSituacaoRua = qtda?.codigo;
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
            }

            CleanStrings();

            var cad = await CadastroIndividual.ToModel(db);

            cad.UnicaLotacaoTransport = this;
            cad.DataRegistro = DateTime.Now;

            if (cad.IdentificacaoUsuarioCidadao1 != null)
            {
                var cod = CadastroIndividual.identificacaoUsuarioCidadao?.Codigo ?? 0;

                cad.IdentificacaoUsuarioCidadao1.Codigo = assmedCadastro?.Codigo;
                cad.IdentificacaoUsuarioCidadao1.ASSMED_Cadastro1 = assmedCadastro;

                if (assmedCadastro != null)
                {
                    assmedCadastro.IdFicha = cad.IdentificacaoUsuarioCidadao1.id;
                }
            }

            var header = cad.UnicaLotacaoTransport;

            header.CadastroIndividual.Add(cad);
            cad.UnicaLotacaoTransport = header;

            var origem = db.OrigemVisita.Create();
            origem.token = Guid.NewGuid();

            header.OrigemVisita = origem;

            header.Validar(db);

            cad.Validar(db);

            var rastro = new RastroFicha
            {
                CodUsu = Convert.ToInt32(ASPSessionVar.Read("idUsuario", url)),
                DataModificacao = DateTime.Now,
                OrigemVisita = origem,
                token = origem.token,
                DadoAnterior = null,
                DadoAtual = await url.Request.Content.ReadAsStringAsync()
            };

            origem.id_tipo_origem = 2;
            origem.enviarParaThrift = true;
            origem.enviado = false;
            origem.RastroFicha.Add(rastro);
            origem.UnicaLotacaoTransport.Add(header);
            db.OrigemVisita.Add(origem);

            await GerarCadastroAssmed(cad, db, url);

            await db.SaveChangesAsync();

            return cad.UnicaLotacaoTransport.id;
        }

        private async Task GerarCadastroAssmed(CadastroIndividual cad, DomainContainer db, UrlHelper url)
        {
            if (cad.IdentificacaoUsuarioCidadao1 == null) return;

            var iden = cad.IdentificacaoUsuarioCidadao1;

            var requester = (url.Request.Properties.ContainsKey("MS_HttpContext") ? url.Request.Properties["MS_HttpContext"] as HttpContextWrapper : null)?
                .Request.UserHostAddress ?? (HttpContext.Current?.Request?.UserHostAddress);

            var def = cad.InformacoesSocioDemograficas1?.DeficienciasCidadao.FirstOrDefault()?.id_tp_deficiencia_cidadao;
            var curso = cad.InformacoesSocioDemograficas1?.grauInstrucaoCidadao;
            curso = (await db.TP_Curso.SingleOrDefaultAsync(x => x.codigo == curso))?.id_tp_curso;

            var cidade = (await db.Cidade.FirstOrDefaultAsync(x => x.CodIbge == iden.codigoIbgeMunicipioNascimento));

            var deficiencia = (await db.TP_Deficiencia.SingleOrDefaultAsync(x => x.codigo == def))?.id_tp_deficiencia;

            var nacao = (await db.Nacionalidade.FirstOrDefaultAsync(x => x.codigo == iden.paisNascimento)).CodNacao;

            var cpf = iden.CPF;
            if (cpf != null)
            {
                cpf = Regex.Replace(cpf, "([0-9]{3})([0-9]{3})([0-9]{3})([0-9]{2})", "$1.$2.$3-$4");
            }

            var assmed = iden.ASSMED_Cadastro1;

            var pessoa = assmed?.ASSMED_PesFisica;

            var codigo = assmed?.Codigo;

            var idUsuario = Convert.ToInt32(ASPSessionVar.Read("idUsuario", url));

            var telefone = assmed.ASSMED_CadTelefones.LastOrDefault(x => x.TipoTel == "C");

            var email = assmed.ASSMED_CadEmails.LastOrDefault(x => x.TipoEMail == "P");

            var cns = assmed.ASSMED_CadastroDocPessoal.LastOrDefault(x => x.CodTpDocP == 6 && x.Numero != null && x.Numero.Trim().Length > 0);

            var rg = assmed.ASSMED_CadastroDocPessoal.LastOrDefault(x => x.CodTpDocP == 1 && x.Numero != null && x.Numero.Trim().Length > 0);

            var novo = codigo == null;

            if (novo)
            {
                if(cpf != null)
                {
                    var numCpfs = await db.ASSMED_PesFisica.CountAsync(x => x.CPF == cpf || x.CPF == iden.CPF);

                    if(numCpfs == 1)
                    {
                        pessoa = await db.ASSMED_PesFisica.FirstOrDefaultAsync(x => x.CPF == cpf || x.CPF == iden.CPF);

                        assmed = pessoa.ASSMED_Cadastro;
                    }
                }

                codigo = await db.ASSMED_Cadastro.MaxAsync(x => x.Codigo + 1);
            }

            var np = pessoa == null;

            With(ref pessoa, () => new ASSMED_PesFisica
            {
                Codigo = (decimal)codigo,
                NumContrato = 22,
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
                UfNacao = cidade.UF
            });

            With(ref assmed, () => new ASSMED_Cadastro
            {
                IdentificacaoUsuarioCidadao = iden,
                Codigo = (decimal)codigo,
                NumContrato = 22,
                CodUsu = idUsuario,
                ASSMED_PesFisica = pessoa,
                DtAtualizacao = DateTime.Now,
                DtSistema = DateTime.Now,
                IdFicha = iden.id,
                Nome = iden.nomeCidadao,
                NomeSocial = iden.nomeSocial,
                NumIP = requester,
                Tipo = "F"
            });

            pessoa.ASSMED_Cadastro = assmed;

            if (iden.telefoneCelular != null)
            {
                var n = telefone == null;

                With(ref telefone, () => new ASSMED_CadTelefones
                {
                    ASSMED_Cadastro = assmed,
                    Codigo = (decimal)codigo,
                    CodUsu = idUsuario,
                    DDD = Convert.ToInt32(iden.telefoneCelular.Substring(0, 2)),
                    DtSistema = DateTime.Now,
                    NumContrato = 22,
                    NumIP = requester,
                    NumTel = iden.telefoneCelular.Substring(2),
                    TipoTel = "C"
                });

                if (n)
                {
                    assmed.ASSMED_CadTelefones.Add(telefone);
                    db.ASSMED_CadTelefones.Add(telefone);
                }
            }

            if (iden.emailCidadao != null)
            {
                var n = email == null;

                With(ref email, () => new ASSMED_CadEmails
                {
                    ASSMED_Cadastro = assmed,
                    Codigo = (decimal)codigo,
                    CodUsu = idUsuario,
                    EMail = iden.emailCidadao,
                    TipoEMail = "P",
                    DtSistema = DateTime.Now,
                    NumContrato = 22,
                    NumIP = requester
                });

                if (n)
                {
                    assmed.ASSMED_CadEmails.Add(email);
                    db.ASSMED_CadEmails.Add(email);
                }
            }

            if (iden.cnsCidadao != null)
            {
                var n = cns == null;

                With(ref cns, () => new ASSMED_CadastroDocPessoal
                {
                    ASSMED_Cadastro = assmed,
                    Codigo = (decimal)codigo,
                    NumContrato = 22,
                    CodTpDocP = 6,
                    CodUsu = idUsuario,
                    DtSistema = DateTime.Now,
                    Numero = iden.cnsCidadao,
                    NumIP = requester
                });

                if (n)
                {
                    assmed.ASSMED_CadastroDocPessoal.Add(cns);
                    db.ASSMED_CadastroDocPessoal.Add(cns);
                }
            }

            if (iden.RG != null)
            {
                var n = rg == null;

                With(ref rg, () => new ASSMED_CadastroDocPessoal
                {
                    ASSMED_Cadastro = assmed,
                    Codigo = (decimal)codigo,
                    NumContrato = 22,
                    CodTpDocP = 1,
                    CodUsu = idUsuario,
                    DtSistema = DateTime.Now,
                    Numero = iden.RG,
                    NumIP = requester
                });

                if (n)
                {
                    assmed.ASSMED_CadastroDocPessoal.Add(rg);
                    db.ASSMED_CadastroDocPessoal.Add(rg);
                }
            }

            if (novo)
            {
                db.ASSMED_Cadastro.Add(assmed);
                pessoa.ASSMED_Cadastro = assmed;

                if (np)
                {
                    db.ASSMED_PesFisica.Add(pessoa);
                }

                cad.IdentificacaoUsuarioCidadao1.ASSMED_Cadastro1 = assmed;
            }
            
            cad.IdentificacaoUsuarioCidadao1.ASSMED_Cadastro1 = assmed;
        }

        internal static async Task<FormCadastroIndividual> Apply(CadastroIndividual data, DomainContainer db)
        {
            var form = new FormCadastroIndividual
            {
                CabecalhoTransporte = UnicaLotacaoTransportCadastroViewModel.ApplyModel(data.UnicaLotacaoTransport),
                CadastroIndividual = data
            };

            form.Finalizado = data.UnicaLotacaoTransport.OrigemVisita.finalizado;

            if (form.CadastroIndividual.identificacaoUsuarioCidadao != null)
            {
                var iden = form.CadastroIndividual.identificacaoUsuarioCidadao;

                var cidade = await db.Cidade.SingleOrDefaultAsync(x => x.CodIbge == iden.codigoIbgeMunicipioNascimento);

                iden.codigoIbgeMunicipioNascimento = cidade?.CodCidade.ToString();

                var pais = await db.Nacionalidade.SingleOrDefaultAsync(x => x.codigo == iden.paisNascimento);

                iden.paisNascimento = pais?.CodNacao;

                var nacionalidade = await db.TP_Nacionalidade.SingleOrDefaultAsync(x => x.codigo == iden.nacionalidadeCidadao);

                iden.nacionalidadeCidadao = nacionalidade?.id_tp_nacionalidade ?? 1;
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

                var peso = await db.TP_Consideracao_Peso.SingleOrDefaultAsync(x => x.codigo == cond.situacaoPeso);

                cond.situacaoPeso = peso?.id_tp_consideracao_peso;
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

                var qtda = await db.TP_Quantas_Vezes_Alimentacao
                    .SingleOrDefaultAsync(x => x.codigo == emsit.quantidadeAlimentacoesAoDiaSituacaoRua);

                emsit.quantidadeAlimentacoesAoDiaSituacaoRua = qtda?.id_tp_quantas_vezes_alimentacao;
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
            }

            return form;
        }
    }
}