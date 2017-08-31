using Softpark.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator UnicaLotacaoTransport(FormCadastroDomiciliar model) =>
            model.CabecalhoTransporte;

        //public async Task<Guid> LimparESalvarDados(DomainContainer db, UrlHelper url)
        //{
        //    var codigo = CadastroDomiciliar.enderecoLocalPermanencia?.Codigo;
        //    var cns = CadastroDomiciliar.familiaRow.FirstOrDefault().numeroCnsResponsavel;

        //    var assmedEndereco = await (db.ASSMED_Endereco.OrderByDescending(x => x.ItemEnd).Where(x => x.Codigo == codigo).FirstOrDefaultAsync() ??
        //        db.ASSMED_CadastroDocPessoal.Where(x => x.Numero != null && x.Numero.Trim().Length > 0 && x.Numero == cns && x.CodTpDocP == 6)
        //            .SelectMany(x => x.ASSMED_Cadastro.ASSMED_Endereco)
        //            .OrderByDescending(x => x.ItemEnd).FirstOrDefaultAsync());

        //    CadastroDomiciliar.uuid = Guid.NewGuid();
        //    CadastroDomiciliar.uuidFichaOriginadora = CadastroDomiciliar.uuid;

        //    if (assmedEndereco != null)
        //    {
        //        codigo = assmedEndereco.Codigo;

        //        var ultimaFicha = await db.CadastroDomiciliar.SingleOrDefaultAsync(x => x.enderecoLocalPermanencia == assmedEndereco.IdFicha);

        //        if (ultimaFicha != null)
        //        {
        //            CadastroDomiciliar.uuidFichaOriginadora = ultimaFicha.id;
        //        }
        //    }

        //    var profissional = db.VW_Profissional.Where(x => x.CNS == CabecalhoTransporte.profissionalCNS &&
        //    x.CNES == CabecalhoTransporte.cnes).ToArray().FirstOrDefault(x => x.INE == CabecalhoTransporte.ine || x.INE == null);

        //    if (profissional != null)
        //    {
        //        CabecalhoTransporte.cboCodigo_2002 = profissional.CBO;
        //    }

        //    if (CadastroDomiciliar.enderecoLocalPermanencia != null)
        //    {
        //        CadastroDomiciliar.enderecoLocalPermanencia.Codigo = codigo;

        //        var iden = CadastroIndividual.identificacaoUsuarioCidadao;

        //        var codCidade = iden.codigoIbgeMunicipioNascimento != null && !string.IsNullOrEmpty(iden.codigoIbgeMunicipioNascimento.Trim()) ?
        //            Convert.ToInt32(iden.codigoIbgeMunicipioNascimento) : 0;

        //        var cidade = await db.Cidade.SingleOrDefaultAsync(x => x.CodCidade == codCidade);

        //        iden.codigoIbgeMunicipioNascimento = cidade?.CodIbge;

        //        var pais = await db.Nacionalidade.SingleOrDefaultAsync(x => x.CodNacao == iden.paisNascimento);

        //        iden.paisNascimento = pais?.codigo;

        //        var nacionalidade = await db.TP_Nacionalidade.SingleOrDefaultAsync(x => x.id_tp_nacionalidade == iden.nacionalidadeCidadao);

        //        iden.nacionalidadeCidadao = nacionalidade?.codigo ?? 1;
        //    }

        //    CabecalhoTransporte.cboCodigo_2002 = profissional.CBO;

        //    CadastroDomiciliar cad = await CadastroDomiciliar.ToModel(domain);

        //    var header = cad.UnicaLotacaoTransport;

        //    var origem = domain.OrigemVisita.Create();

        //    header.OrigemVisita = origem;
        //    origem.finalizado = Finalizado;

        //    header.Validar(domain);

        //    await cad.Validar(domain);

        //    origem.enviarParaThrift = Finalizado;
        //    origem.UnicaLotacaoTransport.Add(header);
        //    domain.OrigemVisita.Add(origem);

        //    await domain.SaveChangesAsync();

        //    return cad.UnicaLotacaoTransport.id;
        //}
    }
}