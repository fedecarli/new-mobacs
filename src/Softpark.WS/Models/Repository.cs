using Softpark.Infrastructure.Extras;
using Softpark.Models;
using Softpark.WS.Controllers.Api;
using Softpark.WS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Softpark.WS
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Repository : IDisposable
    {
        private readonly bool _autodispose = false;
        private readonly Lazy<DomainContainer> _context = new Lazy<DomainContainer>(CreateContext);

        public Repository(bool autodispose = false)
        {
            _autodispose = autodispose;
        }

        public async Task<UnicaLotacaoTransport> GetHeader(Guid token)
        {
            var header = WithContext(c => c.UnicaLotacaoTransport.FirstOrDefaultAsync(x => x.token == token));

            await Task.WhenAll(header);

            var headerTransport = await header;

            if (headerTransport == null)
                throw new ArgumentException("Token inválido", nameof(token));

            return headerTransport;
        }

        public async Task<FichaVisitaDomiciliarChildCadastroViewModel[]> GetVisitas(Guid token, string microarea)
        {
            var header = GetHeader(token);

            await Task.WhenAll(header);

            var headerToken = await header;

            var visitas = WithContext(c => c.UnicaLotacaoTransport
                .Where(u => u.profissionalCNS == headerToken.profissionalCNS && u.OrigemVisita.finalizado)
                .SelectMany(f => f.FichaVisitaDomiciliarMaster)
                .SelectMany(f => f.FichaVisitaDomiciliarChild).ToListAsync());

            await Task.WhenAll(visitas);

            var fichas = await visitas;

            FichaVisitaDomiciliarChildCadastroViewModelCollection results = fichas.ToArray();

            if (microarea != null && Regex.IsMatch(microarea, "^([0-9][0-9])$"))
            {
                results = results.Where(r => r.microarea == null || r.microarea == microarea).ToArray();
            }

            return results.ToArray();
        }

        public async Task<ProfissionalViewModel[]> GetProfissionais()
        {
            var profs = WithContext(Domain => Domain.VW_Profissional.ToListAsync());

            await Task.WhenAll(profs);

            var profissionais = await profs;

            var ps = new Dictionary<string, ProfissionalViewModel>();

            Func<VW_Profissional, ProfissionalViewModel> __ = prof =>
            {
                var _ = new ProfissionalViewModel();

                var cns = prof?.CNS?.Trim();

                if (cns != null)
                    ps.Add(cns, _);

                return _;
            };

            profissionais.ForEach(prof =>
            {
                var p = ps.ContainsKey(prof.CNS) ? ps[prof.CNS] : __.Invoke(prof);

                p.CNS = prof.CNS?.Trim();
                p.Nome = prof.Nome?.Trim();

                p.Append(prof);
            });

            return ps.Values.ToArray();
        }

        public async Task<FichaVisitaDomiciliarMaster> GetOrCreateMaster(Guid token)
        {
            var transport = GetHeader(token);

            var masterVisita = await GetModel(c => c.FichaVisitaDomiciliarMaster.FirstOrDefaultAsync(m => m.FichaVisitaDomiciliarChild.Count < 99), async r =>
            {
                if (r == null)
                {
                    return await Create(c => c.FichaVisitaDomiciliarMaster, async (c, e) =>
                    {
                        transport.Wait();

                        var header = await transport;

                        e.tpCdsOrigem = 3;
                        e.headerTransport = header.id;
                        e.uuidFicha = header.cnes + '-' + Guid.NewGuid();
                        return e;
                    });
                }

                return r;
            });

            await Task.WhenAll(transport, masterVisita);

            return await masterVisita;
        }

        public async Task<TE> GetModel<T, TE>(Func<DomainContainer, Task<T>> func, Func<T, TE> f2)
        {
            var models = GetModel(func);

            await Task.WhenAll(models);

            var results = await models;

            var te = f2(results);

            return te;
        }

        public async Task<TE> GetModel<T, TE>(Func<DomainContainer, DbSet<T>> func, Func<List<T>, TE> f2) where T : class
        {
            var models = GetModel(func);

            await Task.WhenAll(models);

            var results = await models;

            var te = f2(results);

            return te;
        }

        public async Task<List<T>> GetModel<T>(Func<DomainContainer, DbSet<T>> func) where T : class
        {
            var models = WithContext(c => func(c).ToListAsync());

            await Task.WhenAll(models);

            return await models;
        }

        public async Task<T> GetModel<T>(Func<DomainContainer, Task<T>> func)
        {
            var models = WithContext(func);

            await Task.WhenAll(models);

            return await models;
        }

        public async Task<GetCadastroIndividualViewModel[]> GetPacientes(Guid token, string microarea)
        {
            var header = GetHeader(token);

            header.Wait();

            var headerTransport = await header;

            var profissionalCNS = headerTransport.profissionalCNS;

            var ids = WithContext(c => c.VW_IdentificacaoUsuarioCidadao.Where(x => x.id != null).Select(x => x.id).ToListAsync());

            var pessoas = WithContext(c =>
                            (from pc in c.VW_profissional_cns
                             join cad in c.VW_ultimo_cadastroIndividual
                             on pc.CodigoCidadao equals cad.Codigo
                             where pc.cnsProfissional.Trim() == profissionalCNS.Trim()
                             select new { pc, cad }).ToListAsync());

            await Task.WhenAll(ids, pessoas);

            var people = await pessoas;

            var idProf = people.FirstOrDefault()?.pc.IdProfissional;

            var profs = WithContext(c => c.ProfCidadaoVincAgendaProd
                .Where(x =>
                    x.AgendamentoMarcado == true &&
                    x.DataCarregado == null &&
                    x.FichaGerada == true &&
                    x.ProfCidadaoVinc.IdProfissional == idProf).ToListAsync());

            profs.Wait();

            var profissionais = await profs;

            var idsCids = profissionais.Select(x => x.ProfCidadaoVinc.IdCidadao).ToArray();

            var cads = people.Where(x => idsCids.Contains(x.pc.IdCidadao))
                .Select(x => x.cad.idCadastroIndividual).ToArray();

            var aIds = await ids;

            var cadastros = WithContext(c => c.CadastroIndividual
                .Where(x => x.identificacaoUsuarioCidadao != null && aIds.Contains(x.identificacaoUsuarioCidadao.Value)
                            && cads.Contains(x.id)).ToListAsync());

            cadastros.Wait();

            CadastroIndividualViewModelCollection results = await cadastros;

            if (microarea != null && Regex.IsMatch(microarea, "^([0-9][0-9])$"))
            {
                results = results.Where(r => r.identificacaoUsuarioCidadao?.microarea == null || r.identificacaoUsuarioCidadao.microarea == microarea).ToArray();
            }

            var rs = results.ToArray();

            var ps = profissionais.Where(x => people.Any(z => x.IdVinc == z.pc.IdVinc)).ToList();

            var _save = SaveChanges();

            _save.Wait();

            await _save;

            await WithContext(c => ps.ForEach(x => c.PR_EncerrarAgenda(x.IdAgendaProd, false, false)));

            return rs;
        }

        public async Task<GetCadastroDomiciliarViewModel[]> GetDomicilios(Guid token, string microarea)
        {
            var header = GetHeader(token);

            //Alteração Cristiano, David 
            var doms = WithContext(async Domain =>
            {
                header.Wait();

                var headerToken = await header;

                return await (from pc in Domain.VW_profissional_cns
                              join pcv in Domain.ProfCidadaoVinc on pc.IdVinc equals pcv.IdVinc
                              join agenda in Domain.ProfCidadaoVincAgendaProd on pcv.IdVinc equals agenda.IdVinc
                              join cad in Domain.VW_ultimo_cadastroDomiciliar on pc.CodigoCidadao equals cad.Codigo
                              join cd in Domain.CadastroDomiciliar on cad.idCadastroDomiciliar equals cd.id
                              join ultCadIdv in Domain.VW_ultimo_cadastroIndividual on cad.Codigo equals ultCadIdv.Codigo
                              join cadIdv in Domain.CadastroIndividual on ultCadIdv.idCadastroIndividual equals cadIdv.id
                              join idtUserCid in Domain.IdentificacaoUsuarioCidadao on cadIdv.identificacaoUsuarioCidadao equals idtUserCid.id
                              where pc.cnsProfissional.Trim() == headerToken.profissionalCNS.Trim() &&
                                agenda.AgendamentoMarcado == true &&
                                agenda.DataCarregadoDomiciliar == null &&
                                agenda.FichaDomiciliarGerada == true &&
                                idtUserCid.cnsResponsavelFamiliar == null
                              select new { cd, agenda }).ToListAsync();
            });

            doms.Wait();

            var domicilios = await doms;

            var cadastros = domicilios.Select(x => x.cd).ToArray();

            CadastroDomiciliarViewModelCollection results = cadastros;

            if (microarea != null && Regex.IsMatch(microarea, "^([0-9][0-9])$"))
            {
                results = results.Where(r => r.enderecoLocalPermanencia?.microarea == null || r.enderecoLocalPermanencia?.microarea == microarea).ToArray();
            }

            var _save = SaveChanges();

            _save.Wait();

            await _save;

            await WithContext(c => domicilios.ForEach(x => c.PR_EncerrarAgenda(x.agenda.IdAgendaProd, false, true)));

            return results.ToArray();
        }

        public async Task CreateFichasVisita(Guid token, IEnumerable<FichaVisitaDomiciliarChildCadastroViewModel> fichas, bool validar = true)
        {
            var tfichas = new List<Task<FichaVisitaDomiciliarChild>>();

            foreach (var child in fichas)
            {
                var masterVisita = GetOrCreateMaster(token);

                masterVisita.Wait();

                var master = await masterVisita;

                var fichaVisita = Create(c => c.FichaVisitaDomiciliarChild, async (c, f) =>
                {
                    var ficha = child.ToModel(ref f);

                    foreach (var motivoId in child.motivosVisita)
                    {
                        var model = c.SIGSM_MotivoVisita.FindAsync(motivoId);

                        model.Wait();

                        var motivo = await model;

                        if (motivo != null)
                            ficha.SIGSM_MotivoVisita.Add(motivo);
                    }

                    if (ficha.dtNascimento != null)
                        ficha.dtNascimento.Value.IsValidBirthDateTime(master.UnicaLotacaoTransport.dataAtendimento);

                    ficha.uuidFicha = master.uuidFicha;

                    if (validar) ficha.Validar();

                    return ficha;
                });

                tfichas.Add(fichaVisita);
            }

            await Task.WhenAll(tfichas);

            tfichas.ForEach(async f => await f);
        }

        private Task<T> WithContext<T>(Func<DomainContainer, Task<T>> func)
        {
            return func(_context.Value);
        }

        private Task WithContext(Action<DomainContainer> func)
        {
            return Task.Run(() =>
            {
                func(_context.Value);
            });
        }

        public Task<T> Create<T>(Func<DomainContainer, DbSet<T>> func, Func<DomainContainer, T, Task<T>> then) where T : class
        {
            return Task.Run(() =>
            {
                DbSet<T> table = func(_context.Value);

                var entity = table.Create();

                table.Add(entity);

                return then(_context.Value, entity);
            });
        }

        public Task SaveChanges()
        {
            return _context.Value.SaveChangesAsync();
        }

        private static DomainContainer CreateContext()
        {
            var c = new DomainContainer();
            c.Configuration.LazyLoadingEnabled = false;
            return c;
        }

        public void Dispose()
        {
            if (_context.IsValueCreated)
                _context.Value.Dispose();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
