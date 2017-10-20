using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel;
using Softpark.Models;
using System;
using Softpark.WS.Validators;
using Softpark.Infrastructure.Extras;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http.Routing;

namespace Softpark.WS.ViewModels
{
    /// <summary>
    /// ViewModel Collection de fichas de visita
    /// </summary>
    public class FichaVisitaDomiciliarChildCadastroViewModelCollection : List<FichaVisitaDomiciliarChildCadastroViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        public FichaVisitaDomiciliarChildCadastroViewModelCollection(FichaVisitaDomiciliarChild[] models)
        {
            AddRange(models);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public FichaVisitaDomiciliarChildCadastroViewModelCollection(IEnumerable<FichaVisitaDomiciliarChildCadastroViewModel> collection) : base(collection)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static implicit operator FichaVisitaDomiciliarChildCadastroViewModelCollection(FichaVisitaDomiciliarChild[] models)
        {
            return new FichaVisitaDomiciliarChildCadastroViewModelCollection(models);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static implicit operator FichaVisitaDomiciliarChildCadastroViewModelCollection(FichaVisitaDomiciliarChildCadastroViewModel[] models)
        {
            return new FichaVisitaDomiciliarChildCadastroViewModelCollection(models);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        public void AddRange(FichaVisitaDomiciliarChild[] models)
        {
            foreach (var model in models)
            {
                Add(model);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childs"></param>
        /// <param name="db"></param>
        public void ToModels(ICollection<FichaVisitaDomiciliarChild> childs, DomainContainer db) =>
            ForEach(item => childs.Add(item.ToModel(db)));
    }

    /// <summary>
    /// FichaVisitaDomiciliarChild DTO
    /// </summary>
    /// <remarks>
    /// http://esusab.github.io/integracao/docs/dicionario-fvd.html
    /// </remarks>
    public class FichaVisitaDomiciliarChildCadastroViewModel
    {
#pragma warning disable IDE1006 // Naming Styles
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Token da transmissão
        /// </summary>
        public Guid? token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? id { get; set; }

        /// <summary>
        /// Turno da visita
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public long turno { get; set; }

        /// <summary>
        /// Nome Cidadao
        /// </summary>
        public string nomeCidadao { get; set; }

        /// <summary>
        /// Número do prontuário
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string numProntuario { get; set; }

        /// <summary>
        /// Cns do Cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string cnsCidadao { get; set; }

        /// <summary>
        /// Data de Nascimento
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public DateTime? dtNascimento { get; set; }

        /// <summary>
        /// Sexo do cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public long? sexo { get; set; }

        /// <summary>
        /// Motivos da visita
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public long[] motivosVisita { get; set; } = new long[0];

        /// <summary>
        /// Desfecho da visita
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public long desfecho { get; set; }

        /// <summary>
        /// Micro área do atendimento
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string microarea { get; set; }

        /// <summary>
        /// Fora de área
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool stForaArea { get; set; }

        /// <summary>
        /// Tipo de imóvel da visita
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public long tipoDeImovel { get; set; }

        /// <summary>
        /// Peso do paciente
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public double? pesoAcompanhamentoNutricional { get; set; } = null;

        /// <summary>
        /// Altura do paciente
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public double? alturaAcompanhamentoNutricional { get; set; } = null;

        /// <summary>
        /// Visita compartilhada
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public bool statusVisitaCompartilhadaOutroProfissional { get; set; }

        /// <summary>
        /// Latitude de demarcação do início do cadastro
        /// </summary>
        public string latitude { get; set; } = null;

        /// <summary>
        /// Latitude de demarcação do início do cadastro
        /// </summary>
        public string longitude { get; set; } = null;

        /// <summary>
        /// Justificativa
        /// </summary>
        public string Justificativa { get; set; } = null;

        /// <summary>
        /// Data de registro da ficha no app
        /// </summary>
        public DateTime? DataRegistro { get; set; } = null;

        /// <summary>
        /// Data do Atendimento
        /// </summary>
        [Required]
        public DateTime DataAtendimento { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator FichaVisitaDomiciliarChildCadastroViewModel(FichaVisitaDomiciliarChild model)
        {
            var vm = new FichaVisitaDomiciliarChildCadastroViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        private void ApplyModel(FichaVisitaDomiciliarChild model)
        {
            if (model == null) return;

            alturaAcompanhamentoNutricional = model.alturaAcompanhamentoNutricional == null ? null : new double?(Convert.ToDouble(model.alturaAcompanhamentoNutricional));
            cnsCidadao = model.cnsCidadao;
            desfecho = model.desfecho;
            dtNascimento = model.dtNascimento;
            microarea = model.microarea;
            numProntuario = model.numProntuario;
            pesoAcompanhamentoNutricional = model.pesoAcompanhamentoNutricional == null ? null : new double?(Convert.ToDouble(pesoAcompanhamentoNutricional));
            sexo = model.sexo;
            statusVisitaCompartilhadaOutroProfissional = model.statusVisitaCompartilhadaOutroProfissional;
            stForaArea = model.stForaArea;
            tipoDeImovel = model.tipoDeImovel;
            turno = model.turno;
            latitude = model.latitude;
            longitude = model.longitude;
            Justificativa = model.Justificativa;
            DataRegistro = model.DataRegistro;
            nomeCidadao = model.nomeCidadao;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="domain"></param>
        internal async Task<FichaVisitaDomiciliarChild> ToModel(DomainContainer domain)
        {
            var fvdc = (await domain.FichaVisitaDomiciliarChild.FindAsync(id)) ??
                domain.FichaVisitaDomiciliarChild.Create();

            fvdc.childId = id ?? Guid.NewGuid();
            fvdc.alturaAcompanhamentoNutricional = alturaAcompanhamentoNutricional == null || alturaAcompanhamentoNutricional <= 0 ? (decimal?)null : Convert.ToDecimal(alturaAcompanhamentoNutricional);
            fvdc.cnsCidadao = cnsCidadao;
            fvdc.desfecho = desfecho;
            fvdc.dtNascimento = dtNascimento;
            fvdc.microarea = microarea;
            fvdc.numProntuario = numProntuario;
            fvdc.pesoAcompanhamentoNutricional = pesoAcompanhamentoNutricional == null || pesoAcompanhamentoNutricional <= 0 ? (decimal?)null : Convert.ToDecimal(pesoAcompanhamentoNutricional);
            fvdc.sexo = sexo;
            fvdc.statusVisitaCompartilhadaOutroProfissional = statusVisitaCompartilhadaOutroProfissional;
            fvdc.stForaArea = stForaArea;
            fvdc.tipoDeImovel = tipoDeImovel;
            fvdc.turno = turno;
            fvdc.latitude = latitude;
            fvdc.longitude = longitude;
            fvdc.Justificativa = Justificativa;
            fvdc.DataRegistro = DataRegistro;
            fvdc.nomeCidadao = nomeCidadao;

            domain.FichaVisitaDomiciliarChild.Add(fvdc);

            return fvdc;
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
            Dirty(this);
        }

        internal FichaVisitaDomiciliarChildCadastroViewModel ToDetail()
        {
            DirtyStrings();

            return this;
        }

        internal async Task<FichaVisitaDomiciliarChildCadastroViewModel> LimparESalvarDados(DomainContainer domain, UrlHelper url, FichaVisitaDomiciliarMaster ficha)
        {
            id
        }

#pragma warning restore IDE1006 // Naming Styles
    }

    /// <summary>
    /// ViewModel Collection de fichas de visita
    /// </summary>
    public class FichaVisitaDomiciliarChildListagemViewModelCollection : List<FichaVisitaDomiciliarChildListagemViewModel>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        public FichaVisitaDomiciliarChildListagemViewModelCollection(FichaVisitaDomiciliarChild[] models)
        {
            AddRange(models);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public FichaVisitaDomiciliarChildListagemViewModelCollection(IEnumerable<FichaVisitaDomiciliarChildListagemViewModel> collection) : base(collection)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static implicit operator FichaVisitaDomiciliarChildListagemViewModelCollection(FichaVisitaDomiciliarChild[] models)
        {
            return new FichaVisitaDomiciliarChildListagemViewModelCollection(models);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static implicit operator FichaVisitaDomiciliarChildListagemViewModelCollection(FichaVisitaDomiciliarChildListagemViewModel[] models)
        {
            return new FichaVisitaDomiciliarChildListagemViewModelCollection(models);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        public void AddRange(FichaVisitaDomiciliarChild[] models)
        {
            var i = 1;
            foreach (FichaVisitaDomiciliarChildListagemViewModel model in models)
            {
                model.numero = i++;
                Add(model);
            }
        }
    }

    /// <summary>
    /// FichaVisitaDomiciliarChild DTO Listagem
    /// </summary>
    /// <remarks>
    /// http://esusab.github.io/integracao/docs/dicionario-fvd.html
    /// </remarks>
    public class FichaVisitaDomiciliarChildListagemViewModel
    {
#pragma warning disable IDE1006 // Naming Styles
        // ReSharper disable once InconsistentNaming

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string uuidFicha { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        /// <summary>
        /// Posição na listagem
        /// </summary>
        public int numero { get; set; }

        /// <summary>
        /// Cns do Cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string cnsCidadao { get; set; }

        /// <summary>
        /// Cns do Cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string nomeCidadao { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        /// <param name="num"></param>
        private void ApplyModel(FichaVisitaDomiciliarChild model, int num)
        {
            if (model == null) return;

            cnsCidadao = model.cnsCidadao;
            uuidFicha = model.uuidFicha;
            numero = num;
            nomeCidadao = model.nomeCidadao;
        }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator FichaVisitaDomiciliarChildListagemViewModel(FichaVisitaDomiciliarChild model)
        {
            var vm = new FichaVisitaDomiciliarChildListagemViewModel();

            vm.ApplyModel(model, 0);

            return vm;
        }
#pragma warning restore IDE1006 // Naming Styles
    }
}
