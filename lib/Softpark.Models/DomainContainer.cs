using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Threading.Tasks;
using Softpark.Infrastructure.Extras;
using Softpark.Models;
using System.ComponentModel.DataAnnotations;

namespace Softpark.Models
{
    [Table("SYSLANGUAGES", Schema = "sys")]
    public class Language
    {
        public int lcid;
        public string name;
    }

    public partial class DomainContainer
    {
        private static DomainContainer _current;

        public Language Language
            => _current.Set<Language>().SqlQuery("SELECT lcid, name FROM SYS.SYSLANGUAGES WHERE NAME =(SELECT @@LANGUAGE)").SingleOrDefault();

        public static DomainContainer Current
        {
            get
            {
                return InstallApp();
            }
        }

        public static DomainContainer InstallApp()
        {
            var db = _current ?? (_current = new DomainContainer());

            //var dir = HttpContext.Current.Server.MapPath("~/installation");

            //var path = Directory.GetFiles(dir, "api.sql").First();
            //var locked = Directory.GetFiles(dir, "lock").Any();

            //if (File.Exists(path) && !locked)
            //{
            //    db.Database.ExecuteSqlCommand(File.ReadAllText(path));

            //    File.Create($"{dir}/lock").Close();
            //}

            return db;
        }

        public static void Reopen()
        {
            _current = new DomainContainer();
        }

        public virtual IQueryable<VW_Profissional> VW_Profissional
        {
            get
            {
                return from cad in ASSMED_Cadastro
                       join doc in ASSMED_CadastroDocPessoal on cad.Codigo equals doc.Codigo
                       join cred in AS_Credenciados on new { Codigo = cad.Codigo } equals new { Codigo = (decimal)cred.Codigo }
                       join vinc in AS_CredenciadosVinc on cred.CodCred equals vinc.CodCred
                       join cbo in AS_ProfissoesTab on vinc.CodProfTab equals cbo.CodProfTab
                       join par in AS_SetoresPar on new { CNESLocal = vinc.CNESLocal } equals new { CNESLocal = par.CNES }
                       join setor in Setores
                             on new { par.CodSetor, par.Codigo }
                         equals new { setor.CodSetor, Codigo = (decimal)setor.Codigo }
                       let ine = SetoresINEs.FirstOrDefault(si => si.CodSetor == setor.CodSetor)
                       where
                         doc.CodTpDocP == 6 &&
                         cbo.CodProfissao > 0 &&
                         (((cbo.CodProfTab ?? "").ToString().TrimEnd()).TrimStart()).Length > 0 &&
                         (((doc.Numero ?? "").ToString().TrimEnd()).TrimStart()).Length > 0 &&
                         (((vinc.CNESLocal ?? "").ToString().TrimEnd()).TrimStart()).Length > 0
                       group new { cad, doc, cbo, vinc, setor, ine } by new
                       {
                           cad.Nome,
                           doc.Numero,
                           cbo.CodProfTab,
                           cbo.DesProfTab,
                           vinc.CNESLocal,
                           setor.DesSetor,
                           INE = ine != null ? ine.Numero : "",
                           Descricao = ine != null ? ine.Descricao : "",
                           cad.CodUsu
                       } into g
                       select new VW_Profissional
                       {
                           CNS = g.Key.Numero.Trim(),
                           Nome = g.Key.Nome.Trim(),
                           CBO = g.Key.CodProfTab.Trim(),
                           Profissao = g.Key.DesProfTab.Trim(),
                           CNES = g.Key.CNESLocal.Trim(),
                           Unidade = g.Key.DesSetor.Trim(),
                           INE = g.Key.INE.Trim(),
                           Equipe = g.Key.Descricao.Trim(),
                           CodUsuario = g.Key.CodUsu
                       };
            }
        }

        public static ICollection<Type> MapOdata()
        {
            var tdc = typeof(DomainContainer);
            return tdc.GetProperties().Where(x => x.DeclaringType.Name == typeof(DbSet<>).Name)
                .SelectMany(x => x.DeclaringType.GenericTypeArguments).ToList();
        }
    }
}
