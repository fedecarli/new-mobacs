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
using System.Linq;
using System.Data.SqlClient;

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
            ForEach(async item => childs.Add(await item.ToModel(db)));
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
        /// 
        /// </summary>
        public decimal? Codigo { get; set; }

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
        public decimal? pesoAcompanhamentoNutricional { get; set; } = null;

        /// <summary>
        /// Altura do paciente
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public decimal? alturaAcompanhamentoNutricional { get; set; } = null;

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

            alturaAcompanhamentoNutricional = model.alturaAcompanhamentoNutricional;
            cnsCidadao = model.cnsCidadao;
            desfecho = model.desfecho;
            dtNascimento = model.dtNascimento;
            microarea = model.microarea;
            numProntuario = model.numProntuario;
            pesoAcompanhamentoNutricional = model.pesoAcompanhamentoNutricional;
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
            id = model.childId;
            Codigo = model.Codigo;
            motivosVisita = model.SIGSM_MotivoVisita.Select(x => x.codigo).ToArray();
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
            fvdc.Codigo = Codigo;
            fvdc.SIGSM_MotivoVisita = motivosVisita.Select(x => domain.SIGSM_MotivoVisita.Find(x))
                .Where(x => x != null).ToList();

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
            Clear(this);
        }

        internal async Task<FichaVisitaDomiciliarChild> LimparESalvarDados(DomainContainer domain, UrlHelper url, FichaVisitaDomiciliarMaster ficha)
        {
            CleanStrings();

            var isNew = id == null;
            var child = ficha.FichaVisitaDomiciliarChild.FirstOrDefault(x => x.childId == id)
                ?? domain.FichaVisitaDomiciliarChild.Create();

            child.alturaAcompanhamentoNutricional = alturaAcompanhamentoNutricional;
            child.childId = id ?? Guid.NewGuid();
            child.cnsCidadao = cnsCidadao;
            child.dtNascimento = dtNascimento;
            child.DataRegistro = DateTime.Now;
            child.desfecho = desfecho;
            child.FichaVisitaDomiciliarMaster = ficha;
            child.Justificativa = Justificativa;
            child.latitude = latitude;
            child.longitude = longitude;
            child.microarea = !string.IsNullOrEmpty(microarea?.Trim()) ? microarea?.Trim() : null;
            child.nomeCidadao = nomeCidadao;
            child.numProntuario = numProntuario;
            child.pesoAcompanhamentoNutricional = pesoAcompanhamentoNutricional;
            child.sexo = sexo;
            child.SIGSM_MotivoVisita.Clear();
            motivosVisita.Select(x => domain.SIGSM_MotivoVisita.Find(x))
                .Where(x => x != null).ToList().ForEach(child.SIGSM_MotivoVisita.Add);
            child.statusVisitaCompartilhadaOutroProfissional = statusVisitaCompartilhadaOutroProfissional;
            child.stForaArea = stForaArea || child.microarea == null;
            child.tipoDeImovel = tipoDeImovel;
            child.turno = turno;
            child.uuidFicha = ficha.uuidFicha;
            child.Codigo = Codigo;
            child.Validar();

            if (isNew)
            {
                domain.FichaVisitaDomiciliarChild.Add(child);
                ficha.FichaVisitaDomiciliarChild.Add(child);
            }

            Guid.TryParse(ficha.uuidFicha.Replace(ficha.UnicaLotacaoTransport.cnes + "-", ""), out Guid idf);
            var proc = await domain.SIGSM_Transmissao_Processos.FindAsync(idf);

            if (proc != null)
            {
                proc?.SIGSM_Transmissao_Processos_Log.Clear();

                domain.SIGSM_Transmissao_Processos.Remove(proc);
            }

            await domain.SaveChangesAsync();

            return child;
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
        public void AddRange(FichaVisitaDomiciliarChild[] models) =>
            models.ToList().ForEach(x => Add(x));
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

        /// <summary>
        /// Codigo ASSMED
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Cns do Cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string CNS { get; set; }

        /// <summary>
        /// Nome do Cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string NOM { get; set; }

        /// <summary>
        /// Data Nascimento do Cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string DTN { get; set; }

        /// <summary>
        /// Nome Mãe do Cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string MAE { get; set; }

        /// <summary>
        /// CPF do Cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string CPF { get; set; }

        /// <summary>
        /// SEXO do Cidadão
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string SEXO { get; set; }

        /// <summary>
        /// Uuid Child
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Guid uuId { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator FichaVisitaDomiciliarChildListagemViewModel(FichaVisitaDomiciliarChild model)
        {
            return model == null ? null : DomainContainer.Current.Database.SqlQuery<FichaVisitaDomiciliarChildListagemViewModel>(@"
             SELECT TOP 1
	                COALESCE(CAST(COALESCE(c.Codigo, cd.Codigo) AS VARCHAR), '') AS id,
	                COALESCE(c.cnsCidadao, '') AS CNS,
	                COALESCE(c.nomeCidadao, COALESCE(cd.Nome, '') COLLATE Latin1_General_CI_AI) AS NOM,
	                COALESCE(CONVERT(VARCHAR,(CONVERT(DATE,COALESCE(c.dtNascimento, COALESCE(pf1.DtNasc,pf2.DtNasc)),103)),103), '') AS DTN,
	                COALESCE(COALESCE(pf1.NomeMae,pf2.NomeMae) COLLATE Latin1_General_CI_AI, '') AS MAE,
	                COALESCE(COALESCE(pf1.CPF, pf2.CPF) COLLATE Latin1_General_CI_AI, '') AS CPF,
                    (CASE WHEN (c.sexo IS NULL OR c.sexo = 4) AND COALESCE(pf1.Sexo, pf2.Sexo) COLLATE Latin1_General_CI_AI = 'F' THEN '1'
                          WHEN (c.sexo IS NULL OR c.sexo = 4) AND COALESCE(pf1.Sexo, pf2.Sexo) COLLATE Latin1_General_CI_AI = 'M' THEN '0'
                          ELSE COALESCE(CAST(c.sexo AS VARCHAR), '4') END) AS SEXO,
	                c.childId AS uuId
               FROM api.FichaVisitaDomiciliarChild as c
          LEFT JOIN ASSMED_CadastroDocPessoal AS d
                 ON c.cnsCidadao COLLATE Latin1_General_CI_AI = LTRIM(RTRIM(d.Numero)) COLLATE Latin1_General_CI_AI
          LEFT JOIN ASSMED_Cadastro AS cd
                 ON d.Codigo = cd.Codigo
                 OR c.Codigo = cd.Codigo
          LEFT JOIN ASSMED_CadastroPF AS pf1
                 ON cd.Codigo = pf1.Codigo
          LEFT JOIN ASSMED_PesFisica AS pf2
                 ON cd.Codigo = pf2.Codigo
              WHERE c.childId = @id
           ORDER BY cd.Nome COLLATE Latin1_General_CI_AI,
                    c.cnsCidadao,
                    cd.Codigo", new SqlParameter("@id", model.childId)).Single();
        }
#pragma warning restore IDE1006 // Naming Styles
    }
}
