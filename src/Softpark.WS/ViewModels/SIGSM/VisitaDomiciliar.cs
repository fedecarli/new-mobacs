using Newtonsoft.Json;
using Softpark.Models;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http.Routing;

namespace Softpark.WS.ViewModels.SIGSM
{
    /// <summary>
    /// 
    /// </summary>
    public class DetalheFichaVisitaDomiciliarMasterVW
    {
        /// <summary>
        /// Cabeçalho de transporte
        /// </summary>
        public UnicaLotacaoTransportCadastroViewModel CabecalhoTransporte { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UuidFicha { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TpCdsOrigem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public FichaVisitaDomiciliarChildCadastroViewModel[] FichasChild { get; set; }

        public bool Finalizado { get; set; }

        /// <summary>
        /// DataBind
        /// </summary>
        /// <param name="model"></param>
        public static implicit operator DetalheFichaVisitaDomiciliarMasterVW(FichaVisitaDomiciliarMaster model)
        {
            var vm = new DetalheFichaVisitaDomiciliarMasterVW();

            vm.ApplyModel(model);

            return vm;
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
            Dirty(this);
            FichasChild?.ToList().ForEach(Dirty);
        }

        internal DetalheFichaVisitaDomiciliarMasterVW ToDetail()
        {
            DirtyStrings();

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        internal async Task<FichaVisitaDomiciliarMaster> ToModel(DomainContainer domain)
        {
            var cad = (await domain.FichaVisitaDomiciliarMaster.FindAsync(UuidFicha))
                ?? domain.FichaVisitaDomiciliarMaster.Create();

            cad.UnicaLotacaoTransport = CabecalhoTransporte;

            FichaVisitaDomiciliarChildCadastroViewModelCollection fichas = FichasChild;

            fichas.ToModels(cad.FichaVisitaDomiciliarChild, domain);

            cad.uuidFicha = UuidFicha != null ? UuidFicha :
                ($"{CabecalhoTransporte.cnes}-{Guid.NewGuid()}");

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
            Clear(this);
            FichasChild?.ToList().ForEach(Clear);
        }

        private void ApplyModel(FichaVisitaDomiciliarMaster model)
        {
            TpCdsOrigem = model.tpCdsOrigem;

            UuidFicha = model.uuidFicha;

            Finalizado = model.UnicaLotacaoTransport.OrigemVisita.finalizado;

            FichaVisitaDomiciliarChildCadastroViewModelCollection fichas =
                model.FichaVisitaDomiciliarChild.ToArray();

            CabecalhoTransporte = model.UnicaLotacaoTransport;
        }

        internal async Task<string> LimparESalvarDados(DomainContainer domain, UrlHelper url)
        {
            var restDn = JsonConvert.SerializeObject(this);

            CleanStrings();

            var cad = await ToModel(domain);

            var orig = cad.UnicaLotacaoTransport.OrigemVisita;

            if (cad.UnicaLotacaoTransport.OrigemVisita == null)
            {
                orig = domain.OrigemVisita.Create();
                orig.enviado = false;
                orig.enviarParaThrift = true;
                orig.finalizado = Finalizado;
                orig.id_tipo_origem = 2;
                orig.token = Guid.NewGuid();
                orig.UnicaLotacaoTransport.Add(cad.UnicaLotacaoTransport);
                domain.OrigemVisita.Add(orig);

                domain.FichaVisitaDomiciliarMaster.Add(cad);
            }

            cad.UnicaLotacaoTransport.OrigemVisita = orig;

            domain.FichaVisitaDomiciliarChild.AddRange(cad.FichaVisitaDomiciliarChild);

            cad.UnicaLotacaoTransport.Validar(domain);

            cad.Validar();

            cad.FichaVisitaDomiciliarChild.ToList().ForEach(x => x.Validar());

            var dadoAnterior = await domain.FichaVisitaDomiciliarMaster.FindAsync(cad.uuidFicha);

            DetalheFichaVisitaDomiciliarMasterVW da = dadoAnterior ?? null;

            var restDa = da == null ? null : JsonConvert.SerializeObject(da);

            var rastro = new RastroFicha
            {
                CodUsu = Convert.ToInt32(ASPSessionVar.Read("idUsuario")),
                DataModificacao = DateTime.Now,
                token = orig.token,
                DadoAnterior = restDa,
                DadoAtual = restDn
            };

            domain.RastroFicha.Add(rastro);

            await domain.SaveChangesAsync();

            return cad.uuidFicha;
        }
    }
}