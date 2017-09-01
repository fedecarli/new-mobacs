using Softpark.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Routing;

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
        /// Código ASSMED_Cadastro
        /// </summary>
        public decimal? Codigo { get; set; }

        /// <summary>
        /// Contrato
        /// </summary>
        public int? NumContrato { get; private set; }

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
            Codigo = model.Codigo;
            NumContrato = model.num_contrato;
        }
        
        internal override EnderecoLocalPermanencia ToModel(DomainContainer domain)
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
        public static implicit operator DetalheCadastroDomiciliarVW(CadastroDomiciliar model)
        {
            var vm = new DetalheCadastroDomiciliarVW();

            if (model != null)
                vm.uuid = model.id;

            vm.ApplyModel(model);

            vm.enderecoLocalPermanencia = model.EnderecoLocalPermanencia1;

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
        public static implicit operator FormCadastroDomiciliar(CadastroDomiciliar model)
        {
            return new FormCadastroDomiciliar
            {
                CadastroDomiciliar = model,
                CabecalhoTransporte = model.UnicaLotacaoTransport
            };
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator CadastroDomiciliar(FormCadastroDomiciliar model)
        {
            var tsk = Task.Run(async () =>
            {
                var cad = await model.CadastroDomiciliar.ToModel(DomainContainer.Current);

                cad.UnicaLotacaoTransport = model;

                if (cad.EnderecoLocalPermanencia1 != null)
                {
                    var cod = model.CadastroDomiciliar.enderecoLocalPermanencia?.Codigo ?? 0;

                    var asc = await DomainContainer.Current.ASSMED_Endereco.Where(x => x.Codigo == cod)
                    .OrderByDescending(x => x.ItemEnd).FirstOrDefaultAsync();

                    cad.EnderecoLocalPermanencia1.Codigo = asc?.Codigo;
                    cad.EnderecoLocalPermanencia1.ASSMED_Endereco1 = asc;

                    if (asc != null)
                    {
                        asc.IdFicha = cad.EnderecoLocalPermanencia1.id;
                    }
                }

                return cad;
            });

            tsk.Wait();

            return tsk.Result;
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
            CleanStrings();

            CabecalhoTransporte.codigoIbgeMunicipio = db.ASSMED_Contratos.First().CodigoIbgeMunicipio;

            var codigo = CadastroDomiciliar.enderecoLocalPermanencia?.Codigo;
            var cns = CadastroDomiciliar.familiaRow.FirstOrDefault().numeroCnsResponsavel;

            var assmedEndereco = await (db.ASSMED_Endereco.OrderByDescending(x => x.ItemEnd).Where(x => x.Codigo == codigo).FirstOrDefaultAsync() ??
                db.ASSMED_CadastroDocPessoal.Where(x => x.Numero != null && x.Numero.Trim().Length > 0 && x.Numero == cns && x.CodTpDocP == 6)
                    .SelectMany(x => x.ASSMED_Cadastro.ASSMED_Endereco)
                    .OrderByDescending(x => x.ItemEnd).FirstOrDefaultAsync());

            CadastroDomiciliar.uuid = Guid.NewGuid();
            CadastroDomiciliar.uuidFichaOriginadora = CadastroDomiciliar.uuid;

            if (assmedEndereco != null)
            {
                codigo = assmedEndereco.Codigo;

                var ultimaFicha = await db.CadastroDomiciliar.SingleOrDefaultAsync(x => x.enderecoLocalPermanencia == assmedEndereco.IdFicha);

                if (ultimaFicha != null)
                {
                    CadastroDomiciliar.uuidFichaOriginadora = ultimaFicha.id;
                }
            }

            var profissional = db.VW_Profissional.Where(x => x.CNS == CabecalhoTransporte.profissionalCNS &&
            x.CNES == CabecalhoTransporte.cnes).ToArray().FirstOrDefault(x => x.INE == CabecalhoTransporte.ine || x.INE == null);

            if (profissional != null)
            {
                CabecalhoTransporte.cboCodigo_2002 = profissional.CBO;
            }

            if (CadastroDomiciliar.enderecoLocalPermanencia != null)
            {
                CadastroDomiciliar.enderecoLocalPermanencia.Codigo = codigo;

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

            CabecalhoTransporte.cboCodigo_2002 = profissional.CBO;

            CleanStrings();

            var cad = await CadastroDomiciliar.ToModel(db);

            cad.UnicaLotacaoTransport = this;
            cad.DataRegistro = DateTime.Now;

            if (cad.EnderecoLocalPermanencia1 != null)
            {
                var cod = CadastroDomiciliar.enderecoLocalPermanencia?.Codigo ?? 0;

                cad.EnderecoLocalPermanencia1.Codigo = assmedEndereco?.Codigo;
                cad.EnderecoLocalPermanencia1.ASSMED_Endereco1 = assmedEndereco;

                if (assmedEndereco != null)
                {
                    assmedEndereco.IdFicha = cad.EnderecoLocalPermanencia1.id;
                }
            }

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

            var header = cad.UnicaLotacaoTransport;

            header.CadastroDomiciliar.Add(cad);
            cad.UnicaLotacaoTransport = header;

            var origem = db.OrigemVisita.Create();
            origem.token = Guid.NewGuid();

            header.OrigemVisita = origem;

            header.Validar(db);

            await cad.Validar(db);

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

            await db.SaveChangesAsync();

            return cad.UnicaLotacaoTransport.id;
        }
    }
}