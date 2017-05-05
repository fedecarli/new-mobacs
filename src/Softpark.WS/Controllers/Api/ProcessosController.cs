﻿using Softpark.Infrastructure.Extras;
using Softpark.Models;
using Softpark.WS.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Softpark.WS.Controllers.Api
{
    /// <summary>
    /// Endpoints dos processos de importação de dados
    /// </summary>
    [Route("/api/processos")]
    [System.Web.Mvc.OutputCache(Duration = 0, VaryByParam = "*", NoStore = true)]
    [System.Web.Mvc.SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class ProcessosController : BaseApiController
    {
        private async Task<FichaVisitaDomiciliarMaster> GetOrCreateMaster(Guid token)
        {
            var origem = await Domain.OrigemVisita.FindAsync(token);

            if (origem == null || origem.finalizado)
            {
                throw new InvalidOperationException("Token inválido. Inicie o processo de transmissão.");
            }

            var header = await Domain.UnicaLotacaoTransport.FirstOrDefaultAsync(h => h.token == token);

            if (header == null)
            {
                throw new InvalidOperationException("Token inválido. Inicie o processo de transmissão.");
            }

            var master = header.FichaVisitaDomiciliarMaster.FirstOrDefault(m => m.FichaVisitaDomiciliarChild.Count < 99) ??
                         Domain.FichaVisitaDomiciliarMaster.Create();

            if (master.uuidFicha != null) return master;
            master.tpCdsOrigem = 3;
            master.UnicaLotacaoTransport = header;

            master.uuidFicha = header.cnes + '-' + Guid.NewGuid();
            Domain.FichaVisitaDomiciliarMaster.Add(master);

            await Domain.SaveChangesAsync();

            return master;
        }

        /// <summary>
        /// Cadastrar cabeçalho das fichas
        /// </summary>
        /// <remarks>
        /// Todas as fichas usarão esse cabeçalho
        /// </remarks>
        /// <param name="header">Dados à serem enviados</param>
        /// <returns>Token para envio das fichas</returns>
        [Route("enviar/cabecalho")]
        [HttpPost, ResponseType(typeof(Guid))]
        public async Task<IHttpActionResult> EnviarCabecalho([FromBody, Required] UnicaLotacaoTransportCadastroViewModel header)
        {
            var origem = Domain.OrigemVisita.Create();

            origem.token = Guid.NewGuid();
            origem.id_tipo_origem = 1;
            origem.enviarParaThrift = true;
            origem.enviado = false;

            Domain.OrigemVisita.Add(origem);

            var transport = header.ToModel();

            transport.id = Guid.NewGuid();
            transport.OrigemVisita = origem;
            transport.token = origem.token;

            transport.Validar();

            Domain.UnicaLotacaoTransport.Add(transport);

            await Domain.SaveChangesAsync();

            return Ok(origem.token);
        }

        /// <summary>
        /// Envia uma ficha de visita (child) para cadastro
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        [Route("enviar/visita/child")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> EnviarFichaVisita([FromBody, Required] FichaVisitaDomiciliarChildCadastroViewModel child)
        {
            var master = await GetOrCreateMaster(child.token ?? Guid.Empty);

            var ficha = child.ToModel();

            foreach (var motivoId in child.motivosVisita)
            {
                var motivo = await Domain.SIGSM_MotivoVisita.FindAsync(motivoId);

                if (motivo != null)
                    ficha.SIGSM_MotivoVisita.Add(motivo);
            }

            Domain.FichaVisitaDomiciliarChild.Add(ficha);

            if (ficha.dtNascimento != null)
                Epoch.ValidateBirthDate(child.dtNascimento ?? 0, master.UnicaLotacaoTransport.dataAtendimento.ToUnix());

            ficha.FichaVisitaDomiciliarMaster = master;

            ficha.Validar();

            try
            {
                await Domain.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var ex = ((System.Data.Entity.Validation.DbEntityValidationException)e).EntityValidationErrors;
                throw new Exception(ex.First().ValidationErrors.First().ErrorMessage, e);
            }

            return Ok(true);
        }

        /// <summary>
        /// Cadastro Individual
        /// </summary>
        /// <param name="cadInd"></param>
        /// <returns></returns>
        [Route("enviar/cadastro/individual")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> EnviarCadastroIndividual([FromBody, Required] CadastroIndividualViewModel cadInd)
        {
            var origem = await Domain.OrigemVisita.FindAsync(cadInd.token);

            var header = origem?.UnicaLotacaoTransport?.FirstOrDefault();

            if (origem == null || origem.finalizado || header == null)
            {
                throw new InvalidOperationException("Token inválido. Inicie o processo de transmissão.");
            }

            var cad = await cadInd.ToModel();
            cad.tpCdsOrigem = 3;
            cad.UnicaLotacaoTransport = header;

            cad.Validar();

            Domain.CadastroIndividual.Add(cad);

            int? idAgendaProd = null;

            if (cad.IdentificacaoUsuarioCidadao1 != null && cad.fichaAtualizada)
            {
                var cnsCidadao = cad.IdentificacaoUsuarioCidadao1.cnsCidadao;

                var cnsProfissional = header.profissionalCNS;

                var prod = Domain.VW_profissional_cns.FirstOrDefault(
                    x => x.cnsCidadao == cnsCidadao && x.cnsProfissional == cnsProfissional);

                if (prod == null)
                {
                    throw new ValidationException("Não foi encontrado uma ficha préviamente preenchida para a atualização desse cadastro.");
                }

                var agenda = Domain.ProfCidadaoVincAgendaProd
                    .FirstOrDefault(x => x.ProfCidadaoVinc.IdCidadao == prod.IdCidadao
                                         && x.ProfCidadaoVinc.IdProfissional == prod.IdProfissional);

                if (agenda?.IdAgendaProd == null)
                {
                    throw new ValidationException("Não foi encontrado uma ficha préviamente preenchida para a atualização desse cadastro.");
                }

                idAgendaProd = agenda.IdAgendaProd;

                agenda.DataCarregado = DateTime.Now;
            }

            await Domain.SaveChangesAsync();

            if (idAgendaProd != null)
            {
                Domain.PR_EncerrarAgenda(idAgendaProd, true, false);
            }

            return Ok(true);
        }

        /// <summary>
        /// Cadastro Domiciliar
        /// </summary>
        /// <param name="cadDom"></param>
        /// <returns></returns>
        [Route("enviar/cadastro/domiciliar")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> EnviarCadastroDomiciliar([FromBody, Required] CadastroDomiciliarViewModel cadDom)
        {
            var origem = await Domain.OrigemVisita.FindAsync(cadDom.token);

            if (origem == null || origem.finalizado)
            {
                throw new ValidationException("Token inválido. Inicie o processo de transmissão.");
            }

            var header = origem.UnicaLotacaoTransport.FirstOrDefault();
            var cad = await cadDom.ToModel();
            cad.tpCdsOrigem = 3;
            cad.UnicaLotacaoTransport = header;
            if (header == null) throw new ValidationException("Token inválido. Inicie o processo de transmissão.");

            await cad.Validar();

            Domain.CadastroDomiciliar.Add(cad);

            var idsAgendas = new List<int>();

            if (cad.fichaAtualizada)
            {
                var domicilios = from pc in Domain.VW_profissional_cns
                                 join ut in Domain.UnicaLotacaoTransport
                                 on pc.cnsProfissional.Trim() equals ut.profissionalCNS.Trim()
                                 join cadastro in Domain.VW_ultimo_cadastroDomiciliar
                                 on ut.token equals cadastro.token
                                 where pc.cnsProfissional.Trim() == header.profissionalCNS.Trim()
                                 select new { pc, cad = cadastro };

                var idProf = domicilios.FirstOrDefault()?.pc.IdProfissional;

                var profs = Domain.ProfCidadaoVincAgendaProd
                    .Where(x =>
                        x.DataCarregadoDomiciliar < DateTime.Now &&
                        x.FichaDomiciliarGerada == true &&
                        x.ProfCidadaoVinc.IdProfissional == idProf);

                var idsCids = profs.Select(x => x.ProfCidadaoVinc.IdCidadao).Distinct().ToArray();

                var prod = profs.First();

                if (prod == null)
                {
                    throw new ValidationException("Não foi encontrado uma ficha préviamente preenchida para a atualização desse cadastro.");
                }

                var all = domicilios.Where(x => idsCids.Contains(x.pc.IdCidadao)).ToArray()
                    .Select(x => new { x.cad, x.pc, ht = Domain.UnicaLotacaoTransport.Find(x.cad.headerTransport) })
                    .ToArray();

                foreach (var cadastro in all.Where(x => x.ht.cnes + '-' + x.cad.idCadastroDomiciliar == cad.uuidFichaOriginadora).Distinct())
                {
                    var agenda = Domain.ProfCidadaoVincAgendaProd
                        .FirstOrDefault(x => x.ProfCidadaoVinc.IdCidadao == cadastro.pc.IdCidadao
                                                && x.ProfCidadaoVinc.IdProfissional == cadastro.pc.IdProfissional);

                    if (agenda?.IdAgendaProd == null)
                    {
                        throw new ValidationException("Não foi encontrado uma ficha préviamente preenchida para a atualização desse cadastro.");
                    }

                    idsAgendas.Add(agenda.IdAgendaProd);
                    agenda.DataRetornoDomiciliar = DateTime.Now;
                }
            }

            await Domain.SaveChangesAsync();

            idsAgendas.ForEach(x => Domain.PR_EncerrarAgenda(x, true, true));

            return Ok(true);
        }

        /// <summary>
        /// Finaliza a transmissão de dados (encerra o token)
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [Route("encerrar/{token}")]
        [HttpPost, ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> FinalizarTransmissao([FromUri, Required] Guid token)
        {
            var origem = await Domain.OrigemVisita.FindAsync(token);

            if (origem == null || origem.finalizado)
            {
                throw new ValidationException("Token inválido. Inicie o processo de transmissão.");
            }

            Domain.PR_ProcessarFichasAPI(token);

            return Ok(true);
        }
    }
}