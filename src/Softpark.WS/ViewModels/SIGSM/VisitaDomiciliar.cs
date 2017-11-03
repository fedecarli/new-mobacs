using Newtonsoft.Json;
using Softpark.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
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
        public FichaVisitaDomiciliarChildListagemViewModel[] FichasChild { get; set; }

        /// <summary>
        /// Finalizado?
        /// </summary>
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
            var da = (await domain.FichaVisitaDomiciliarMaster.FindAsync(UuidFicha));
            var cad = da ?? domain.FichaVisitaDomiciliarMaster.Create();

            if (da != null)
            {
                int.TryParse(CabecalhoTransporte.ine, out int _ine);
                var ine = await domain.SetoresINEs.SingleOrDefaultAsync(x => x.CodINE == _ine);

                var header = da.UnicaLotacaoTransport;
                header.cboCodigo_2002 = CabecalhoTransporte.cboCodigo_2002;
                header.cnes = CabecalhoTransporte.cnes;
                header.dataAtendimento = CabecalhoTransporte.dataAtendimento;
                header.ine = ine?.Numero?.Trim();
                header.profissionalCNS = CabecalhoTransporte.profissionalCNS;
                cad.UnicaLotacaoTransport = header;
            }
            else
            {
                cad.UnicaLotacaoTransport = CabecalhoTransporte;
            }

            if (cad.UnicaLotacaoTransport.OrigemVisita == null)
            {
                cad.UnicaLotacaoTransport.OrigemVisita = domain.OrigemVisita.Create();

                cad.UnicaLotacaoTransport.OrigemVisita.finalizado = false;
                cad.UnicaLotacaoTransport.OrigemVisita.id_tipo_origem = 2;
                cad.UnicaLotacaoTransport.OrigemVisita.token = Guid.NewGuid();
                cad.UnicaLotacaoTransport.OrigemVisita.enviado = false;
                cad.UnicaLotacaoTransport.OrigemVisita.enviarParaThrift = true;
                cad.UnicaLotacaoTransport.OrigemVisita.UnicaLotacaoTransport.Add(cad.UnicaLotacaoTransport);

                domain.OrigemVisita.Add(cad.UnicaLotacaoTransport.OrigemVisita);
            }

            var newCad = UuidFicha == null || string.IsNullOrEmpty(UuidFicha.Trim());

            cad.uuidFicha = !newCad ? UuidFicha : ($"{CabecalhoTransporte.cnes}-{Guid.NewGuid()}");
            cad.headerTransport = cad.UnicaLotacaoTransport.id;
            cad.tpCdsOrigem = 3;

            if (Finalizado)
            {
                cad.Validar();
                if (cad.FichaVisitaDomiciliarChild.Count <= 0)
                    throw new ValidationException("Não é possível finalizar uma ficha sem fichas filhas.");
                cad.UnicaLotacaoTransport.OrigemVisita.finalizado = Finalizado;
            }

            if (newCad)
                domain.FichaVisitaDomiciliarMaster.Add(cad);

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

            FichaVisitaDomiciliarChildListagemViewModelCollection fichas =
                model.FichaVisitaDomiciliarChild.ToArray();

            FichasChild = fichas.ToArray();

            CabecalhoTransporte = model.UnicaLotacaoTransport;
        }

        internal async Task<FichaVisitaDomiciliarMaster> LimparESalvarDados(DomainContainer domain, UrlHelper url)
        {
            var restDn = JsonConvert.SerializeObject(this);

            CleanStrings();

            var dadoAnterior = await domain.FichaVisitaDomiciliarMaster.FindAsync(UuidFicha);

            if (dadoAnterior != null && dadoAnterior.UnicaLotacaoTransport.OrigemVisita.finalizado)
                throw new ValidationException("Não é possível alterar os dados de uma ficha finalizada.");

            DetalheFichaVisitaDomiciliarMasterVW da = null;

            if (dadoAnterior != null)
                da = dadoAnterior;

            var restDa = da == null ? null : JsonConvert.SerializeObject(da);

            var cad = await ToModel(domain);

            var orig = cad.UnicaLotacaoTransport.OrigemVisita;

            var rastro = new RastroFicha
            {
                CodUsu = Convert.ToInt32(ASPSessionVar.Read("idUsuario")),
                DataModificacao = DateTime.Now,
                DadoAnterior = restDa,
                DadoAtual = restDn
            };

            domain.RastroFicha.Add(rastro);

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
            }

            rastro.token = orig.token;

            cad.UnicaLotacaoTransport.OrigemVisita = orig;
            
            cad.Validar();

            try
            {
                Guid.TryParse(cad.uuidFicha.Replace(cad.UnicaLotacaoTransport.cnes + "-", ""), out Guid id);
                var proc = await domain.SIGSM_Transmissao_Processos.FindAsync(id);
                
                domain.SIGSM_Transmissao_Processos_Log.RemoveRange(proc.SIGSM_Transmissao_Processos_Log);

                domain.SIGSM_Transmissao_Processos.Remove(proc);

                await domain.SaveChangesAsync();
            }
            catch
            {
                throw;
            }

            return await domain.FichaVisitaDomiciliarMaster.FindAsync(cad.uuidFicha);
        }
    }
}