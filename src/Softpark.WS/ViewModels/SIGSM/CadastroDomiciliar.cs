using Newtonsoft.Json;
using Softpark.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http.Routing;
using static Softpark.Infrastructure.Extensions.WithStatement;

namespace Softpark.WS.ViewModels.SIGSM
{
    /// <summary>
    /// VM de listagem de cadastros
    /// </summary>
    public class CadastroDomiciliarVM
    {
        /// <summary>
        /// Endereco
        /// </summary>
        public string Endereco { get; set; }

        /// <summary>
        /// Complemento
        /// </summary>
        public string Complemento { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        public string Telefone { get; set; }

        /// <summary>
        /// Número
        /// </summary>
        public string Numero { get; set; }

        /// <summary>
        /// Cidadão
        /// </summary>
        public string Responsavel { get; set; }

        /// <summary>
        /// Cidadão
        /// </summary>
        public decimal Codigo { get; set; }
        internal Guid? IdFicha { get; set; }
    }

    /// <summary>
    /// ficha do detalhe de identificação
    /// </summary>
    public class DetalheEnderecoLocalPermanenciaViewModel : EnderecoLocalPermanenciaViewModel
    {
        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator DetalheEnderecoLocalPermanenciaViewModel(EnderecoLocalPermanencia model)
        {
            var vm = new DetalheEnderecoLocalPermanenciaViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(EnderecoLocalPermanencia model)
        {
            if (model == null) return;

            bairro = model.bairro;
            cep = model.cep;
            codigoIbgeMunicipio = model.codigoIbgeMunicipio;
            complemento = model.complemento;
            nomeLogradouro = model.nomeLogradouro;
            numero = model.numero;
            numeroDneUf = model.numeroDneUf;
            telefoneContato = model.telefoneContato;
            telefoneResidencia = telelefoneResidencia = model.telefoneResidencia;
            tipoLogradouroNumeroDne = model.tipoLogradouroNumeroDne;
            stSemNumero = model.stSemNumero;
            pontoReferencia = model.pontoReferencia;
            microarea = model.microarea;
            stForaArea = model.stForaArea;
        }

        internal override EnderecoLocalPermanencia ToModel(DomainContainer domain)
        {
            var iuc = base.ToModel(domain);

            return iuc;
        }
    }

    /// <summary>
    /// Ficha de detalhe do cadastro individual
    /// </summary>
    public class DetalheCadastroDomiciliarVW : CadastroDomiciliarViewModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid uuid { get; set; }

        /// <summary>
        /// Identificação do usuário cidadão
        /// </summary>
        public new DetalheEnderecoLocalPermanenciaViewModel enderecoLocalPermanencia { get; set; } = null;

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static async Task<DetalheCadastroDomiciliarVW> ToVM(CadastroDomiciliar model, DomainContainer db)
        {
            var vm = new DetalheCadastroDomiciliarVW();

            if (model != null)
                vm.uuid = model.id;

            vm.ApplyModel(model);

            vm.enderecoLocalPermanencia = model.EnderecoLocalPermanencia1;

            if (vm.enderecoLocalPermanencia != null)
            {
                if (vm.enderecoLocalPermanencia.cep != null)
                    vm.enderecoLocalPermanencia.cep = Regex.Replace(vm.enderecoLocalPermanencia.cep, "^([0-9]{5})([0-9]{3})$", "$1-$2");

                if (vm.enderecoLocalPermanencia.codigoIbgeMunicipio != null)
                    vm.enderecoLocalPermanencia.codigoIbgeMunicipio = (await db.Cidade.FirstOrDefaultAsync(x => x.CodIbge == vm.enderecoLocalPermanencia.codigoIbgeMunicipio && x.CodIbge != null))?.CodCidade.ToString();

                if (vm.enderecoLocalPermanencia.numeroDneUf != null)
                    vm.enderecoLocalPermanencia.numeroDneUf = (await db.UF.FirstOrDefaultAsync(x => x.DNE == vm.enderecoLocalPermanencia.numeroDneUf))?.UF1;
            }

            return vm;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public override async Task<CadastroDomiciliar> ToModel(DomainContainer domain)
        {
            var cad = await base.ToModel(domain);

            cad.EnderecoLocalPermanencia1 = enderecoLocalPermanencia.ToModel(domain);

            return cad;
        }
    }

    /// <summary>
    /// Formulário de Cadastro
    /// </summary>
    public class FormCadastroDomiciliar
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
        public DetalheCadastroDomiciliarVW CadastroDomiciliar { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        public static async Task<FormCadastroDomiciliar> ToVM(CadastroDomiciliar model, DomainContainer db)
        {
            return new FormCadastroDomiciliar
            {
                CadastroDomiciliar = await DetalheCadastroDomiciliarVW.ToVM(model, db),
                CabecalhoTransporte = UnicaLotacaoTransportCadastroViewModel.ApplyModel(model.UnicaLotacaoTransport)
            };
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        /// <param name="db"></param>
        public static async Task<CadastroDomiciliar> ToModel(FormCadastroDomiciliar model, DomainContainer db)
        {
            var cad = await model.CadastroDomiciliar.ToModel(db);

            cad.UnicaLotacaoTransport = model;

            if (cad.FamiliaRow.Any())
            {
                foreach (var fam in cad.FamiliaRow)
                {
                    var asc = await db.ASSMED_CadastroDocPessoal
                        .FirstOrDefaultAsync(x => x.CodTpDocP == 6 && x.Numero == fam.numeroCnsResponsavel);

                    if (asc != null)
                    {
                        asc.ASSMED_Cadastro.IdFicha = cad.EnderecoLocalPermanencia1.id;
                    }
                }
            }

            return cad;
        }

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
            Clear(CadastroDomiciliar);
            Clear(CadastroDomiciliar.condicaoMoradia);
            Clear(CadastroDomiciliar.enderecoLocalPermanencia);
            CadastroDomiciliar.familiaRow.ToList().ForEach(Clear);
            Clear(CadastroDomiciliar.instituicaoPermanencia);
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator UnicaLotacaoTransport(FormCadastroDomiciliar model) =>
            model.CabecalhoTransporte;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<Guid> LimparESalvarDados(DomainContainer db, UrlHelper url)
        {
            var restDn = JsonConvert.SerializeObject(this);

            CleanStrings();

            CabecalhoTransporte.codigoIbgeMunicipio = db.ASSMED_Contratos.First().CodigoIbgeMunicipio;

            CadastroDomiciliar.uuid = Guid.NewGuid();
            CadastroDomiciliar.uuidFichaOriginadora = CadastroDomiciliar.uuidFichaOriginadora == null ||
                CadastroDomiciliar.uuidFichaOriginadora == Guid.Empty ? CadastroDomiciliar.uuid
                : CadastroDomiciliar.uuidFichaOriginadora;

            if (CabecalhoTransporte.ine != null)
            {
                int.TryParse(CabecalhoTransporte.ine, out int ine);

                var setorPar = (await db.AS_SetoresPar.FirstOrDefaultAsync(x => x.CNES != null && x.CNES.Trim() == CabecalhoTransporte.cnes))?.CodSetor;

                CabecalhoTransporte.ine = (await db.SetoresINEs.FirstOrDefaultAsync(x => x.CodINE == ine && x.CodSetor == setorPar))?.Numero;
            }

            var profissional = db.VW_Profissional
                .Where(x => x.CNS != null && x.CNS.Trim() == CabecalhoTransporte.profissionalCNS &&
                            x.CNES != null && x.CNES.Trim() == CabecalhoTransporte.cnes)
                            .ToArray()
                            .FirstOrDefault(x => x.INE == null || x.INE.Trim() == CabecalhoTransporte.ine);

            if (profissional != null)
            {
                CabecalhoTransporte.cboCodigo_2002 = profissional.CBO;
            }

            if (CadastroDomiciliar.enderecoLocalPermanencia != null)
            {
                var ende = CadastroDomiciliar.enderecoLocalPermanencia;

                var codCidade = ende.codigoIbgeMunicipio != null && !string.IsNullOrEmpty(ende.codigoIbgeMunicipio.Trim()) ?
                    Convert.ToInt32(ende.codigoIbgeMunicipio) : 0;

                var cidade = await db.Cidade.SingleOrDefaultAsync(x => x.CodCidade == codCidade);

                ende.codigoIbgeMunicipio = cidade?.CodIbge;

                var uf = await db.UF.SingleOrDefaultAsync(x => x.UF1 == ende.numeroDneUf);

                ende.numeroDneUf = uf?.DNE;
            }

            if (CadastroDomiciliar.condicaoMoradia != null)
            {
                var cond = CadastroDomiciliar.condicaoMoradia;

                var abast = await db.TP_Abastecimento_Agua.SingleOrDefaultAsync(x => x.id_tp_abastecimento_agua == cond.abastecimentoAgua);
                cond.abastecimentoAgua = abast?.codigo;

                var loca = await db.TP_Localizacao.SingleOrDefaultAsync(x => x.id_tp_localizacao == cond.localizacao);
                cond.localizacao = loca?.codigo;

                var acesso = await db.TP_Acesso_Domicilio.SingleOrDefaultAsync(x => x.id_tp_acesso_domicilio == cond.tipoAcessoDomicilio);
                cond.tipoAcessoDomicilio = acesso?.codigo;

                var situ = await db.TP_Situacao_Moradia.SingleOrDefaultAsync(x => x.id_tp_situacao_moradia == cond.situacaoMoradiaPosseTerra);
                cond.situacaoMoradiaPosseTerra = situ?.codigo;

                var morru = await db.TP_Situacao_Moradia_Rural.SingleOrDefaultAsync(x => x.id_tp_situacao_moradia_rural == cond.areaProducaoRural);
                cond.areaProducaoRural = morru != null ? (100 + morru.id_tp_situacao_moradia_rural) : (int?)null;

                var tipo = await db.TP_Domicilio.SingleOrDefaultAsync(x => x.id_tp_domicilio == cond.tipoDomicilio);
                cond.tipoDomicilio = tipo?.codigo;

                var cons = await db.TP_Construcao_Domicilio.SingleOrDefaultAsync(x => x.id_tp_construcao_domicilio == cond.materialPredominanteParedesExtDomicilio);
                cond.materialPredominanteParedesExtDomicilio = cons?.codigo;

                var agua = await db.TP_Tratamento_Agua.SingleOrDefaultAsync(x => x.id_tp_tratamento_agua == cond.aguaConsumoDomicilio);
                cond.aguaConsumoDomicilio = agua?.codigo;

                var lixo = await db.TP_Destino_Lixo.SingleOrDefaultAsync(x => x.id_tp_destino_lixo == cond.destinoLixo);
                cond.destinoLixo = lixo?.codigo;

                var esco = await db.TP_Escoamento_Esgoto.SingleOrDefaultAsync(x => x.id_tp_escoamento_esgoto == cond.formaEscoamentoBanheiro);
                cond.formaEscoamentoBanheiro = esco?.codigo;
            }

            if (CadastroDomiciliar.animalNoDomicilio != null)
            {
                CadastroDomiciliar.animalNoDomicilio = await db.TP_Animais.Where(x => CadastroDomiciliar.animalNoDomicilio.Contains(x.id_tp_animais))
                    .Select(x => x.codigo).ToArrayAsync();
            }
            
            CleanStrings();

            var cad = await CadastroDomiciliar.ToModel(db);

            cad.UnicaLotacaoTransport = this;
            cad.DataRegistro = DateTime.Now;

            if (cad.InstituicaoPermanencia1 != null && !((new long[] { 7, 8, 9, 10, 11 }).Contains(cad.tipoDeImovel) || cad.statusTermoRecusa))
            {
                if (cad.InstituicaoPermanencia1.stOutrosProfissionaisVinculados == false &&
                    cad.InstituicaoPermanencia1.nomeInstituicaoPermanencia == null &&
                    cad.InstituicaoPermanencia1.nomeResponsavelTecnico == null &&
                    cad.InstituicaoPermanencia1.telefoneResponsavelTecnico == null &&
                    cad.InstituicaoPermanencia1.cnsResponsavelTecnico == null &&
                    cad.InstituicaoPermanencia1.cargoInstituicao == null)
                {
                    cad.InstituicaoPermanencia1.CadastroDomiciliar = null;
                    db.InstituicaoPermanencia.Remove(cad.InstituicaoPermanencia1);
                    cad.InstituicaoPermanencia1 = null;
                }
            }

            cad.fichaAtualizada = cad.id != cad.uuidFichaOriginadora;

            var header = cad.UnicaLotacaoTransport;

            header.CadastroDomiciliar.Add(cad);
            cad.UnicaLotacaoTransport = header;

            var origem = db.OrigemVisita.Create();
            origem.token = Guid.NewGuid();

            var dadoAnterior = CadastroDomiciliar.uuid != CadastroDomiciliar.uuidFichaOriginadora ?
                (await db.CadastroDomiciliar.SingleOrDefaultAsync(x => x.id == CadastroDomiciliar.uuidFichaOriginadora)) : null;

            var da = dadoAnterior == null ? null :
                await ToVM(dadoAnterior, db);

            var restDa = da == null ? null : JsonConvert.SerializeObject(da);

            var rastro = new RastroFicha
            {
                CodUsu = Convert.ToInt32(ASPSessionVar.Read("idUsuario")),
                DataModificacao = DateTime.Now,
                OrigemVisita = origem,
                token = origem.token,
                DadoAnterior = restDa,
                DadoAtual = restDn
            };

            origem.id_tipo_origem = 2;
            origem.enviarParaThrift = true;
            origem.enviado = false;
            origem.RastroFicha.Add(rastro);
            origem.UnicaLotacaoTransport.Add(header);
            origem.finalizado = true;

            header.OrigemVisita = origem;

            header.Validar(db);

            await cad.Validar(db);

            db.OrigemVisita.Add(origem);

            await db.SaveChangesAsync();

            await GerarCadastroAssmed(cad, db, url);

            await db.SaveChangesAsync();

            return cad.id;
        }

        private async Task GerarCadastroAssmed(CadastroDomiciliar cad, DomainContainer db, UrlHelper url)
        {
            if (cad.enderecoLocalPermanencia == null) return;

            var end = cad.EnderecoLocalPermanencia1;

            var codtplog = (await db.TB_MS_TIPO_LOGRADOURO.FirstOrDefaultAsync(x => x.CO_TIPO_LOGRADOURO != null && x.CO_TIPO_LOGRADOURO.Trim() != end.tipoLogradouroNumeroDne))?.CO_TIPO_LOGRADOURO.Trim() ?? null;

            int? tplog = null;
            if (int.TryParse(codtplog, out int tl))
                tplog = tl;

            var cid = await db.Cidade.FirstOrDefaultAsync(x => x.CodIbge != null && x.CodIbge.Trim() == end.codigoIbgeMunicipio);

            var uf = await db.UF.FirstOrDefaultAsync(x => x.DNE != null && x.DNE.Trim() == end.numeroDneUf);

            foreach (var fam in cad.FamiliaRow)
            {
                if (1 == await db.ASSMED_CadastroDocPessoal.CountAsync(x => x.Numero != null && x.Numero == fam.numeroCnsResponsavel && x.CodTpDocP == 6))
                {
                    var resp = await db.ASSMED_CadastroDocPessoal.Where(x => x.Numero != null && x.Numero == fam.numeroCnsResponsavel && x.CodTpDocP == 6)
                        .Select(x => x.ASSMED_Cadastro)
                        .SingleOrDefaultAsync();

                    if (resp == null) return;

                    var depends = await db.IdentificacaoUsuarioCidadao.Where(x => x.cnsResponsavelFamiliar == fam.numeroCnsResponsavel &&
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
                        item = resp.ASSMED_Endereco.Max(x => x.ItemEnd) + 1;

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
            Dirty(CadastroDomiciliar);
            Dirty(CadastroDomiciliar.condicaoMoradia);
            Dirty(CadastroDomiciliar.enderecoLocalPermanencia);
            CadastroDomiciliar.familiaRow.ToList().ForEach(Dirty);
            Dirty(CadastroDomiciliar.instituicaoPermanencia);
        }

        internal async Task<FormCadastroDomiciliar> ToDetail(DomainContainer db)
        {
            if (CadastroDomiciliar.condicaoMoradia != null)
            {
                var cond = CadastroDomiciliar.condicaoMoradia;

                var abast = await db.TP_Abastecimento_Agua.SingleOrDefaultAsync(x => x.codigo == cond.abastecimentoAgua);
                cond.abastecimentoAgua = abast?.id_tp_abastecimento_agua;

                var loca = await db.TP_Localizacao.SingleOrDefaultAsync(x => x.codigo == cond.localizacao);
                cond.localizacao = loca?.id_tp_localizacao;

                var acesso = await db.TP_Acesso_Domicilio.SingleOrDefaultAsync(x => x.codigo == cond.tipoAcessoDomicilio);
                cond.tipoAcessoDomicilio = acesso?.id_tp_acesso_domicilio;

                var situ = await db.TP_Situacao_Moradia.SingleOrDefaultAsync(x => x.codigo == cond.situacaoMoradiaPosseTerra);
                cond.situacaoMoradiaPosseTerra = situ?.id_tp_situacao_moradia;

                cond.areaProducaoRural = cond.areaProducaoRural - 100 != null ? (cond.areaProducaoRural - 100) : null;

                var tipo = await db.TP_Domicilio.SingleOrDefaultAsync(x => x.codigo == cond.tipoDomicilio);
                cond.tipoDomicilio = tipo?.id_tp_domicilio;

                var cons = await db.TP_Construcao_Domicilio.SingleOrDefaultAsync(x => x.codigo == cond.materialPredominanteParedesExtDomicilio);
                cond.materialPredominanteParedesExtDomicilio = cons?.id_tp_construcao_domicilio;

                var agua = await db.TP_Tratamento_Agua.SingleOrDefaultAsync(x => x.codigo == cond.aguaConsumoDomicilio);
                cond.aguaConsumoDomicilio = agua?.id_tp_tratamento_agua;

                var lixo = await db.TP_Destino_Lixo.SingleOrDefaultAsync(x => x.codigo == cond.destinoLixo);
                cond.destinoLixo = lixo?.id_tp_destino_lixo;

                var esco = await db.TP_Escoamento_Esgoto.SingleOrDefaultAsync(x => x.codigo == cond.formaEscoamentoBanheiro);
                cond.formaEscoamentoBanheiro = esco?.id_tp_escoamento_esgoto;
            }

            if (CadastroDomiciliar.animalNoDomicilio != null)
            {
                CadastroDomiciliar.animalNoDomicilio = await db.TP_Animais.Where(x => CadastroDomiciliar.animalNoDomicilio.Contains(x.codigo))
                    .Select(x => x.id_tp_animais).ToArrayAsync();
            }

            DirtyStrings();

            return this;
        }
    }
}