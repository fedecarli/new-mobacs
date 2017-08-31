using Softpark.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http.Routing;

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

        private void CleanStrings()
        {
            if (CabecalhoTransporte != null)
                foreach (PropertyInfo pi in CabecalhoTransporte.GetType().GetProperties())
                {
                    if (pi.PropertyType.Equals(typeof(string)))
                    {
                        var val = pi.GetValue(CabecalhoTransporte);

                        if (val == null || string.IsNullOrEmpty(val.ToString().Trim()) || string.IsNullOrWhiteSpace(val.ToString().Trim()))
                            pi.SetValue(CabecalhoTransporte, null);
                    }
                }

            if (CadastroIndividual != null)
                foreach (PropertyInfo pi in CadastroIndividual.GetType().GetProperties())
                {
                    if (pi.PropertyType.Equals(typeof(string)))
                    {
                        var val = pi.GetValue(CadastroIndividual);

                        if (val == null || string.IsNullOrEmpty(val.ToString().Trim()) || string.IsNullOrWhiteSpace(val.ToString().Trim()))
                            pi.SetValue(CadastroIndividual, null);
                    }
                }

            if (CadastroIndividual.identificacaoUsuarioCidadao != null)
                foreach (PropertyInfo pi in CadastroIndividual.identificacaoUsuarioCidadao.GetType().GetProperties())
                {
                    if (pi.PropertyType.Equals(typeof(string)))
                    {
                        var val = pi.GetValue(CadastroIndividual.identificacaoUsuarioCidadao);

                        if (val == null || string.IsNullOrEmpty(val.ToString().Trim()) || string.IsNullOrWhiteSpace(val.ToString().Trim()))
                            pi.SetValue(CadastroIndividual.identificacaoUsuarioCidadao, null);
                    }
                }

            if (CadastroIndividual.informacoesSocioDemograficas != null)
                foreach (PropertyInfo pi in CadastroIndividual.informacoesSocioDemograficas.GetType().GetProperties())
                {
                    if (pi.PropertyType.Equals(typeof(string)))
                    {
                        var val = pi.GetValue(CadastroIndividual.informacoesSocioDemograficas);

                        if (val == null || string.IsNullOrEmpty(val.ToString().Trim()) || string.IsNullOrWhiteSpace(val.ToString().Trim()))
                            pi.SetValue(CadastroIndividual.informacoesSocioDemograficas, null);
                    }
                }

            if (CadastroIndividual.condicoesDeSaude != null)
                foreach (PropertyInfo pi in CadastroIndividual.condicoesDeSaude.GetType().GetProperties())
                {
                    if (pi.PropertyType.Equals(typeof(string)))
                    {
                        var val = pi.GetValue(CadastroIndividual.condicoesDeSaude);

                        if (val == null || string.IsNullOrEmpty(val.ToString().Trim()) || string.IsNullOrWhiteSpace(val.ToString().Trim()))
                            pi.SetValue(CadastroIndividual.condicoesDeSaude, null);
                    }
                }

            if (CadastroIndividual.emSituacaoDeRua != null)
                foreach (PropertyInfo pi in CadastroIndividual.emSituacaoDeRua.GetType().GetProperties())
                {
                    if (pi.PropertyType.Equals(typeof(string)))
                    {
                        var val = pi.GetValue(CadastroIndividual.emSituacaoDeRua);

                        if (val == null || string.IsNullOrEmpty(val.ToString().Trim()) || string.IsNullOrWhiteSpace(val.ToString().Trim()))
                            pi.SetValue(CadastroIndividual.emSituacaoDeRua, null);
                    }
                }

            if (CadastroIndividual.saidaCidadaoCadastro != null)
                foreach (PropertyInfo pi in CadastroIndividual.saidaCidadaoCadastro.GetType().GetProperties())
                {
                    if (pi.PropertyType.Equals(typeof(string)))
                    {
                        var val = pi.GetValue(CadastroIndividual.saidaCidadaoCadastro);

                        if (val == null || string.IsNullOrEmpty(val.ToString().Trim()) || string.IsNullOrWhiteSpace(val.ToString().Trim()))
                            pi.SetValue(CadastroIndividual.saidaCidadaoCadastro, null);
                    }
                }
        }

        /// <summary>
        /// Limpeza dos dados
        /// </summary>
        /// <param name="db"></param>
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
            }

            var cad = await CadastroIndividual.ToModel(db);
            
            cad.UnicaLotacaoTransport = this;

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

            var origem = db.OrigemVisita.Create();

            header.OrigemVisita = origem;
            origem.finalizado = Finalizado;

            header.Validar(db);

            cad.Validar(db);

            origem.enviarParaThrift = Finalizado;
            origem.UnicaLotacaoTransport.Add(header);
            db.OrigemVisita.Add(origem);

            await db.SaveChangesAsync();

            return cad.UnicaLotacaoTransport.id;
        }
    }
}