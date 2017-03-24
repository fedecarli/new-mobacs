using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using Softpark.Models;
using System.Web.Http.OData.Query;

namespace Softpark.WS.Controllers.Api.odata
{
    public class ProfissionalController : ODataController
    {
        private DomainContainer db = new DomainContainer();

        // GET: odata/Profissional
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public IQueryable<VW_Profissional> GetProfissional()
        {
            return db.VW_Profissional;
        }

        // GET: odata/Profissional(5)
        [EnableQuery(
            AllowedArithmeticOperators = AllowedArithmeticOperators.All,
            AllowedFunctions = AllowedFunctions.SubstringOf | AllowedFunctions.ToLower | AllowedFunctions.IndexOf,
            AllowedLogicalOperators = AllowedLogicalOperators.All,
            AllowedQueryOptions = AllowedQueryOptions.All,
            EnableConstantParameterization = true,
            HandleNullPropagation = HandleNullPropagationOption.Default
        )]
        public IQueryable<VW_Profissional> GetASSMED_Cadastro([FromODataUri] string key)
        {
            return db.VW_Profissional.Where(vw => vw.CNS == key);
        }

        //// PUT: odata/Profissional(5)
        //public async Task<IHttpActionResult> Put([FromODataUri] int key, Delta<ASSMED_Cadastro> patch)
        //{
        //    Validate(patch.GetEntity());

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    ASSMED_Cadastro aSSMED_Cadastro = await db.ASSMED_Cadastro.FindAsync(key);
        //    if (aSSMED_Cadastro == null)
        //    {
        //        return NotFound();
        //    }

        //    patch.Put(aSSMED_Cadastro);

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ASSMED_CadastroExists(key))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Updated(aSSMED_Cadastro);
        //}

        //// POST: odata/Profissional
        //public async Task<IHttpActionResult> Post(ASSMED_Cadastro aSSMED_Cadastro)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.ASSMED_Cadastro.Add(aSSMED_Cadastro);

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (ASSMED_CadastroExists(aSSMED_Cadastro.NumContrato))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Created(aSSMED_Cadastro);
        //}

        //// PATCH: odata/Profissional(5)
        //[AcceptVerbs("PATCH", "MERGE")]
        //public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<ASSMED_Cadastro> patch)
        //{
        //    Validate(patch.GetEntity());

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    ASSMED_Cadastro aSSMED_Cadastro = await db.ASSMED_Cadastro.FindAsync(key);
        //    if (aSSMED_Cadastro == null)
        //    {
        //        return NotFound();
        //    }

        //    patch.Patch(aSSMED_Cadastro);

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ASSMED_CadastroExists(key))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Updated(aSSMED_Cadastro);
        //}

        //// DELETE: odata/Profissional(5)
        //public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        //{
        //    ASSMED_Cadastro aSSMED_Cadastro = await db.ASSMED_Cadastro.FindAsync(key);
        //    if (aSSMED_Cadastro == null)
        //    {
        //        return NotFound();
        //    }

        //    db.ASSMED_Cadastro.Remove(aSSMED_Cadastro);
        //    await db.SaveChangesAsync();

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// GET: odata/Profissional(5)/ACADEMIA_FichaTreino
        //[EnableQuery]
        //public IQueryable<ACADEMIA_FichaTreino> GetACADEMIA_FichaTreino([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ACADEMIA_FichaTreino);
        //}

        //// GET: odata/Profissional(5)/AF_ContaCad
        //[EnableQuery]
        //public IQueryable<AF_ContaCad> GetAF_ContaCad([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.AF_ContaCad);
        //}

        //// GET: odata/Profissional(5)/AF_ContaCadSld
        //[EnableQuery]
        //public SingleResult<AF_ContaCadSld> GetAF_ContaCadSld([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.ASSMED_Cadastro.Where(m => m.NumContrato == key).Select(m => m.AF_ContaCadSld));
        //}

        //// GET: odata/Profissional(5)/ASSMED_AtendCidadao
        //[EnableQuery]
        //public IQueryable<ASSMED_AtendCidadao> GetASSMED_AtendCidadao([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_AtendCidadao);
        //}

        //// GET: odata/Profissional(5)/ASSMED_CadastroAcaoSocial
        //[EnableQuery]
        //public IQueryable<ASSMED_CadastroAcaoSocial> GetASSMED_CadastroAcaoSocial([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_CadastroAcaoSocial);
        //}

        //// GET: odata/Profissional(5)/ASSMED_CadastroConv
        //[EnableQuery]
        //public IQueryable<ASSMED_CadastroConv> GetASSMED_CadastroConv([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_CadastroConv);
        //}

        //// GET: odata/Profissional(5)/ASSMED_CadastroCertidoes
        //[EnableQuery]
        //public IQueryable<ASSMED_CadastroCertidoes> GetASSMED_CadastroCertidoes([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_CadastroCertidoes);
        //}

        //// GET: odata/Profissional(5)/ASSMED_CadastroDocPessoal
        //[EnableQuery]
        //public IQueryable<ASSMED_CadastroDocPessoal> GetASSMED_CadastroDocPessoal([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_CadastroDocPessoal);
        //}

        //// GET: odata/Profissional(5)/ASSMED_CadastroFoto
        //[EnableQuery]
        //public IQueryable<ASSMED_CadastroFoto> GetASSMED_CadastroFoto([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_CadastroFoto);
        //}

        //// GET: odata/Profissional(5)/ASSMED_CadastroQuest
        //[EnableQuery]
        //public IQueryable<ASSMED_CadastroQuest> GetASSMED_CadastroQuest([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_CadastroQuest);
        //}

        //// GET: odata/Profissional(5)/ASSMED_CadastroQuest1
        //[EnableQuery]
        //public IQueryable<ASSMED_CadastroQuest> GetASSMED_CadastroQuest1([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_CadastroQuest1);
        //}

        //// GET: odata/Profissional(5)/ASSMED_CadastroSetor
        //[EnableQuery]
        //public IQueryable<ASSMED_CadastroSetor> GetASSMED_CadastroSetor([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_CadastroSetor);
        //}

        //// GET: odata/Profissional(5)/ASSMED_CadastroTitular
        //[EnableQuery]
        //public SingleResult<ASSMED_CadastroTitular> GetASSMED_CadastroTitular([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.ASSMED_Cadastro.Where(m => m.NumContrato == key).Select(m => m.ASSMED_CadastroTitular));
        //}

        //// GET: odata/Profissional(5)/ASSMED_CadastroVacina
        //[EnableQuery]
        //public IQueryable<ASSMED_CadastroVacina> GetASSMED_CadastroVacina([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_CadastroVacina);
        //}

        //// GET: odata/Profissional(5)/ASSMED_CadEmails
        //[EnableQuery]
        //public IQueryable<ASSMED_CadEmails> GetASSMED_CadEmails([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_CadEmails);
        //}

        //// GET: odata/Profissional(5)/ASSMED_CadTelefones
        //[EnableQuery]
        //public IQueryable<ASSMED_CadTelefones> GetASSMED_CadTelefones([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_CadTelefones);
        //}

        //// GET: odata/Profissional(5)/ASSMED_Endereco
        //[EnableQuery]
        //public IQueryable<ASSMED_Endereco> GetASSMED_Endereco([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_Endereco);
        //}

        //// GET: odata/Profissional(5)/ASSMED_PesFisica
        //[EnableQuery]
        //public SingleResult<ASSMED_PesFisica> GetASSMED_PesFisica([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.ASSMED_Cadastro.Where(m => m.NumContrato == key).Select(m => m.ASSMED_PesFisica));
        //}

        //// GET: odata/Profissional(5)/ASSMED_PesJuridica
        //[EnableQuery]
        //public SingleResult<ASSMED_PesJuridica> GetASSMED_PesJuridica([FromODataUri] int key)
        //{
        //    return SingleResult.Create(db.ASSMED_Cadastro.Where(m => m.NumContrato == key).Select(m => m.ASSMED_PesJuridica));
        //}

        //// GET: odata/Profissional(5)/ASSMED_SALAS
        //[EnableQuery]
        //public IQueryable<ASSMED_SALAS> GetASSMED_SALAS([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_SALAS);
        //}

        //// GET: odata/Profissional(5)/ASSMED_SALASPRESENCA
        //[EnableQuery]
        //public IQueryable<ASSMED_SALASPRESENCA> GetASSMED_SALASPRESENCA([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.ASSMED_SALASPRESENCA);
        //}

        //// GET: odata/Profissional(5)/Atendimento_Individual_Profissional_Saude
        //[EnableQuery]
        //public IQueryable<Atendimento_Individual_Profissional_Saude> GetAtendimento_Individual_Profissional_Saude([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.Atendimento_Individual_Profissional_Saude);
        //}

        //// GET: odata/Profissional(5)/IMO_ImovelProprietario
        //[EnableQuery]
        //public IQueryable<IMO_ImovelProprietario> GetIMO_ImovelProprietario([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.IMO_ImovelProprietario);
        //}

        //// GET: odata/Profissional(5)/IMO_Parceiros
        //[EnableQuery]
        //public IQueryable<IMO_Parceiros> GetIMO_Parceiros([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.IMO_Parceiros);
        //}

        //// GET: odata/Profissional(5)/Atendimento_Individual_Usuario
        //[EnableQuery]
        //public IQueryable<Atendimento_Individual_Usuario> GetAtendimento_Individual_Usuario([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.Atendimento_Individual_Usuario);
        //}

        //// GET: odata/Profissional(5)/SIGSM_Procedimento_Usuario
        //[EnableQuery]
        //public IQueryable<SIGSM_Procedimento_Usuario> GetSIGSM_Procedimento_Usuario([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.SIGSM_Procedimento_Usuario);
        //}

        //// GET: odata/Profissional(5)/SIGSM_Atendimento_Domiciliar_Paciente
        //[EnableQuery]
        //public IQueryable<SIGSM_Atendimento_Domiciliar_Paciente> GetSIGSM_Atendimento_Domiciliar_Paciente([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.SIGSM_Atendimento_Domiciliar_Paciente);
        //}

        //// GET: odata/Profissional(5)/SIGSM_Atendimento_Domiciliar_Profissional
        //[EnableQuery]
        //public IQueryable<SIGSM_Atendimento_Domiciliar_Profissional> GetSIGSM_Atendimento_Domiciliar_Profissional([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.SIGSM_Atendimento_Domiciliar_Profissional);
        //}

        //// GET: odata/Profissional(5)/SIGSM_Atendimento_Odontologico_Profissional
        //[EnableQuery]
        //public IQueryable<SIGSM_Atendimento_Odontologico_Profissional> GetSIGSM_Atendimento_Odontologico_Profissional([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.SIGSM_Atendimento_Odontologico_Profissional);
        //}

        //// GET: odata/Profissional(5)/SIGSM_Atividade_Coletiva
        //[EnableQuery]
        //public IQueryable<SIGSM_Atividade_Coletiva> GetSIGSM_Atividade_Coletiva([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.SIGSM_Atividade_Coletiva);
        //}

        //// GET: odata/Profissional(5)/SIGSM_Atividade_Coletiva_Profissional_Saude
        //[EnableQuery]
        //public IQueryable<SIGSM_Atividade_Coletiva_Profissional_Saude> GetSIGSM_Atividade_Coletiva_Profissional_Saude([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.SIGSM_Atividade_Coletiva_Profissional_Saude);
        //}

        //// GET: odata/Profissional(5)/SIGSM_Atividade_Coletiva_Usuario
        //[EnableQuery]
        //public IQueryable<SIGSM_Atividade_Coletiva_Usuario> GetSIGSM_Atividade_Coletiva_Usuario([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.SIGSM_Atividade_Coletiva_Usuario);
        //}

        //// GET: odata/Profissional(5)/SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente
        //[EnableQuery]
        //public IQueryable<SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente> GetSIGSM_Avaliacao_Elegibilidade_Admissao_Paciente([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente);
        //}

        //// GET: odata/Profissional(5)/SIGSM_Avaliacao_Elegibilidade_Admissao_Profissional
        //[EnableQuery]
        //public IQueryable<SIGSM_Avaliacao_Elegibilidade_Admissao_Profissional> GetSIGSM_Avaliacao_Elegibilidade_Admissao_Profissional([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.SIGSM_Avaliacao_Elegibilidade_Admissao_Profissional);
        //}

        //// GET: odata/Profissional(5)/SIGSM_Marcadores_Consumo_Alimentar
        //[EnableQuery]
        //public IQueryable<SIGSM_Marcadores_Consumo_Alimentar> GetSIGSM_Marcadores_Consumo_Alimentar([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.SIGSM_Marcadores_Consumo_Alimentar);
        //}

        //// GET: odata/Profissional(5)/SIGSM_Procedimento
        //[EnableQuery]
        //public IQueryable<SIGSM_Procedimento> GetSIGSM_Procedimento([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.SIGSM_Procedimento);
        //}

        //// GET: odata/Profissional(5)/SIGSM_Visita_Domiciliar_Paciente
        //[EnableQuery]
        //public IQueryable<SIGSM_Visita_Domiciliar_Paciente> GetSIGSM_Visita_Domiciliar_Paciente([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.SIGSM_Visita_Domiciliar_Paciente);
        //}

        //// GET: odata/Profissional(5)/SIGSM_Visita_Domiciliar_Profissional
        //[EnableQuery]
        //public IQueryable<SIGSM_Visita_Domiciliar_Profissional> GetSIGSM_Visita_Domiciliar_Profissional([FromODataUri] int key)
        //{
        //    return db.ASSMED_Cadastro.Where(m => m.NumContrato == key).SelectMany(m => m.SIGSM_Visita_Domiciliar_Profissional);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool ASSMED_CadastroExists(int key)
        //{
        //    return db.ASSMED_Cadastro.Count(e => e.NumContrato == key) > 0;
        //}
    }
}
