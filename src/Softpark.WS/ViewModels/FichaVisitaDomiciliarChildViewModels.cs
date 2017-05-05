using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ComponentModel;
using Softpark.Models;
using System;
using Softpark.WS.Validators;
using Softpark.Infrastructure.Extras;

namespace Softpark.WS.ViewModels
{
    /// <summary>
    /// Turno de atendimento
    /// </summary>
    [Serializable]
    public enum Turno
    {
        /// <summary>
        /// 1 - Manhã
        /// </summary>
        [EnumMember(Value = nameof(Manhã))]
        Manhã = 1,
        /// <summary>
        /// 2 - Tarde
        /// </summary>
        [EnumMember(Value = nameof(Tarde))]
        Tarde = 2,
        /// <summary>
        /// 3 - Noite
        /// </summary>
        [EnumMember(Value = nameof(Noite))]
        Noite = 3
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class FichaVisitaDomiciliarChildConsultaViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTime DataAtendimento { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CodigoIbgeMunicipio { get; set; }

        /// <summary>
        /// Token da transmissão
        /// </summary>
        [DataMember(Name = nameof(OrigemVisita.token))]
        public Guid? Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VW_Profissional Profissional { get; set; }
        
        /// <summary>
        /// Turno da visita
        /// </summary>
        [Required]
        [DataMember(Name = nameof(FichaVisitaDomiciliarChild.turno))]
        public Turno Turno { get; set; }

        /// <summary>
        /// Número do prontuário
        /// </summary>
        [RegularExpression(@"^([a-zA-Z0-9]*)$", ErrorMessage = "O campo numProntuario aceita somente letras e números.")]
        [StringLength(30, MinimumLength = 0, ErrorMessage = "O campo numProntuario deve ter entre 0 e 30 caracteres.")]
        [DataMember(Name = nameof(FichaVisitaDomiciliarChild.numProntuario))]
        public string NumProntuario { get; set; }

        /// <summary>
        /// Cns do Cidadão
        /// </summary>
        [RegularExpression(@"^([12789])([0-9]+)$", ErrorMessage = "O campo cnsCidadao deve iniciar com 1, 2, 7, 8 ou 9 e deve conter somente números.")]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "O campo cnsCidadao deve ter exatamente 15 caracteres")]
        [DataMember(Name = nameof(FichaVisitaDomiciliarChild.cnsCidadao))]
        [CnsValidation(true, ErrorMessage = "CNS inválido")]
        public string CnsCidadao { get; set; }

        /// <summary>
        /// Data de Nascimento
        /// </summary>
        [DataMember(Name = nameof(FichaVisitaDomiciliarChild.dtNascimento))]
        public DateTime? DtNascimento { get; set; }

        /// <summary>
        /// Sexo do cidadão
        /// </summary>
        [DataMember(Name = nameof(FichaVisitaDomiciliarChild.sexo))]
        public TP_Sexo Sexo { get; set; }

        /// <summary>
        /// Motivos da visita
        /// </summary>
        [DataMember(Name = nameof(FichaVisitaDomiciliarChildCadastroViewModel.motivosVisita))]
        [MotivoVisitaValidation(ErrorMessage = "Um ou mais motivos estão inválidos.")]
        public ICollection<SIGSM_MotivoVisita> MotivosVisita { get; set; } = new HashSet<SIGSM_MotivoVisita>();

        /// <summary>
        /// Desfecho da visita
        /// </summary>
        [DataMember(Name = nameof(FichaVisitaDomiciliarChild.desfecho))]
        [Required]
        [Range(1, 3)]
        public long Desfecho { get; set; }

        /// <summary>
        /// Micro área do atendimento
        /// </summary>
        [DataMember(Name = nameof(FichaVisitaDomiciliarChild.microarea))]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O campo microarea deve ter exatamente 2 digitos.")]
        [RegularExpression("^([0-9][0-9])$")]
        public string Microarea { get; set; }

        /// <summary>
        /// Fora de área
        /// </summary>
        [DataMember(Name = nameof(FichaVisitaDomiciliarChild.stForaArea))]
        public bool StForaArea { get; set; }

        /// <summary>
        /// Tipo de imóvel da visita
        /// </summary>
        [DataMember(Name = nameof(FichaVisitaDomiciliarChild.tipoDeImovel))]
        [Required]
        [RegularExpression(@"^([1-9]|1[012]|99)$", ErrorMessage = "O campo tipoDeImovel espera um tipo válido. Consulte http://esusab.github.io/integracao/docs/dicionario/dicionario.html#tipodeimovel.")]
        public TP_Imovel TipoDeImovel { get; set; }

        /// <summary>
        /// Peso do paciente
        /// </summary>
        [DataMember(Name = nameof(FichaVisitaDomiciliarChild.pesoAcompanhamentoNutricional))]
        [Range(0.5, 500, ErrorMessage = "O campo pesoAcompanhamentoNutricional deve ter entre 0.5 e 500 Kg.")]
        [DefaultValue(null)]
        public decimal? PesoAcompanhamentoNutricional { get; set; }

        /// <summary>
        /// Altura do paciente
        /// </summary>
        [DataMember(Name = nameof(FichaVisitaDomiciliarChild.alturaAcompanhamentoNutricional))]
        [Range(20, 250, ErrorMessage = "O campo alturaAcompanhamentoNutricional deve ter entre 20 e 250 cm.")]
        [DefaultValue(null)]
        public decimal? AlturaAcompanhamentoNutricional { get; set; }

        /// <summary>
        /// Visita compartilhada
        /// </summary>
        [DataMember(Name = nameof(FichaVisitaDomiciliarChild.statusVisitaCompartilhadaOutroProfissional))]
        public bool StatusVisitaCompartilhadaOutroProfissional { get; set; }
    }

    /// <summary>
    /// 
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
    }

    /// <summary>
    /// FichaVisitaDomiciliarChild DTO
    /// </summary>
    /// <remarks>
    /// http://esusab.github.io/integracao/docs/dicionario-fvd.html
    /// </remarks>
    [DataContract(Name = nameof(FichaVisitaDomiciliarChild))]
    public class FichaVisitaDomiciliarChildCadastroViewModel
    {
#pragma warning disable IDE1006 // Naming Styles
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Token da transmissão
        /// </summary>
        [Required]
        [DataMember(Name = nameof(OrigemVisita.token))]
        public Guid? token { get; set; }

        /// <summary>
        /// Turno da visita
        /// </summary>
        [Required]
        [DataMember(Name = nameof(turno))]
        [RegularExpression(@"^([123])$", ErrorMessage = "O campo turno espera pelos valores 1, 2 ou 3.")]
        [Range(1, 3, ErrorMessage = "O campo turno espera pelos valores 1, 2 ou 3.")]
        // ReSharper disable once InconsistentNaming
        public long turno { get; set; }

        /// <summary>
        /// Número do prontuário
        /// </summary>
        [RegularExpression(@"^([a-zA-Z0-9]*)$", ErrorMessage = "O campo numProntuario aceita somente letras e números.")]
        [StringLength(30, MinimumLength = 0, ErrorMessage = "O campo numProntuario deve ter entre 0 e 30 caracteres.")]
        [DataMember(Name = nameof(numProntuario))]
        // ReSharper disable once InconsistentNaming
        public string numProntuario { get; set; }

        /// <summary>
        /// Cns do Cidadão
        /// </summary>
        [RegularExpression(@"^([12789])([0-9]+)$", ErrorMessage = "O campo cnsCidadao deve iniciar com 1, 2, 7, 8 ou 9 e deve conter somente números.")]
        [StringLength(15, MinimumLength = 15, ErrorMessage = "O campo cnsCidadao deve ter exatamente 15 caracteres")]
        [DataMember(Name = nameof(cnsCidadao))]
        [CnsValidation(true, ErrorMessage = "CNS inválido")]
        // ReSharper disable once InconsistentNaming
        public string cnsCidadao { get; set; }

        /// <summary>
        /// Data de Nascimento
        /// </summary>
        [DataMember(Name = nameof(dtNascimento))]
        [CustomValidation(typeof(Epoch), nameof(Epoch.ValidateESUSDate), ErrorMessage = "Data de nascimento inválida.")]
        // ReSharper disable once InconsistentNaming
        public long? dtNascimento { get; set; }

        /// <summary>
        /// Sexo do cidadão
        /// </summary>
        [DataMember(Name = nameof(sexo))]
        [RegularExpression(@"^([014])$", ErrorMessage = "O campo sexo espera pelos valores 0, 1 ou 4.")]
        // ReSharper disable once InconsistentNaming
        public long? sexo { get; set; }

        /// <summary>
        /// Motivos da visita
        /// </summary>
        [DataMember(Name = nameof(motivosVisita))]
        [MotivoVisitaValidation(ErrorMessage = "Um ou mais motivos estão inválidos.")]
        // ReSharper disable once InconsistentNaming
        public ISet<long> motivosVisita { get; set; } = new HashSet<long>();

        /// <summary>
        /// Desfecho da visita
        /// </summary>
        [DataMember(Name = nameof(desfecho))]
        [Required]
        [Range(1, 3, ErrorMessage = "O campo desfecho espera pelos valores 1, 2 ou 3.")]
        // ReSharper disable once InconsistentNaming
        public long desfecho { get; set; }

        /// <summary>
        /// Micro área do atendimento
        /// </summary>
        [DataMember(Name = nameof(microarea))]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O campo microarea deve ter exatamente 2 digitos.")]
        // ReSharper disable once InconsistentNaming
        public string microarea { get; set; }

        /// <summary>
        /// Fora de área
        /// </summary>
        [DataMember(Name = nameof(stForaArea))]
        // ReSharper disable once InconsistentNaming
        public bool stForaArea { get; set; }

        /// <summary>
        /// Tipo de imóvel da visita
        /// </summary>
        [DataMember(Name = nameof(tipoDeImovel))]
        [Required]
        [RegularExpression(@"^([1-9]|1[012]|99)$", ErrorMessage = "O campo tipoDeImovel espera um tipo válido. Consulte http://esusab.github.io/integracao/docs/dicionario/dicionario.html#tipodeimovel.")]
        // ReSharper disable once InconsistentNaming
        public long tipoDeImovel { get; set; }

        /// <summary>
        /// Peso do paciente
        /// </summary>
        [DataMember(Name = nameof(pesoAcompanhamentoNutricional))]
        [Range(0.5, 500, ErrorMessage = "O campo pesoAcompanhamentoNutricional deve ter entre 0.5 e 500 Kg.")]
        [DefaultValue(null)]
        // ReSharper disable once InconsistentNaming
        public double? pesoAcompanhamentoNutricional { get; set; }

        /// <summary>
        /// Altura do paciente
        /// </summary>
        [DataMember(Name = nameof(alturaAcompanhamentoNutricional))]
        [Range(20, 250, ErrorMessage = "O campo alturaAcompanhamentoNutricional deve ter entre 20 e 250 cm.")]
        [DefaultValue(null)]
        // ReSharper disable once InconsistentNaming
        public double? alturaAcompanhamentoNutricional { get; set; }

        /// <summary>
        /// Visita compartilhada
        /// </summary>
        [DataMember(Name = nameof(statusVisitaCompartilhadaOutroProfissional))]
        // ReSharper disable once InconsistentNaming
        public bool statusVisitaCompartilhadaOutroProfissional { get; set; }

        /// <summary>
        /// Latitude de demarcação do início do cadastro
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string latitude { get; set; }

        /// <summary>
        /// Latitude de demarcação do início do cadastro
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string longitude { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator FichaVisitaDomiciliarChildCadastroViewModel(FichaVisitaDomiciliarChild model)
        {
            var vm = new FichaVisitaDomiciliarChildCadastroViewModel();

            vm.ApplyModel(model);

            return vm;
        }

        private void ApplyModel(FichaVisitaDomiciliarChild model)
        {
            if (model == null) return;

            alturaAcompanhamentoNutricional = model.alturaAcompanhamentoNutricional == null ? null : new double?(Convert.ToDouble(model.alturaAcompanhamentoNutricional));
            cnsCidadao = model.cnsCidadao;
            desfecho = model.desfecho;
            dtNascimento = model.dtNascimento?.ToUnix();
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
        }

        internal FichaVisitaDomiciliarChild ToModel()
        {
            var fvdc = DomainContainer.Current.FichaVisitaDomiciliarChild.Create();

            fvdc.childId = Guid.NewGuid();
            fvdc.alturaAcompanhamentoNutricional = alturaAcompanhamentoNutricional == null || alturaAcompanhamentoNutricional <= 0 ? (decimal?)null : Convert.ToDecimal(alturaAcompanhamentoNutricional);
            fvdc.cnsCidadao = cnsCidadao;
            fvdc.desfecho = desfecho;
            fvdc.dtNascimento = dtNascimento?.FromUnix();
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

            return fvdc;
        }
#pragma warning restore IDE1006 // Naming Styles
    }
}
