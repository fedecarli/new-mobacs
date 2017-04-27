using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Softpark.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Table("SYSLANGUAGES", Schema = "sys")]
    public class Language
    {
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int lcid;
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string name;
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class DomainContainer
    {
        public DomainContainer(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        private static DomainContainer _current;

        /// <summary>
        /// 
        /// </summary>
        public Language Language
            => _current.Set<Language>().SqlQuery("SELECT lcid, name FROM SYS.SYSLANGUAGES WHERE NAME =(SELECT @@LANGUAGE)").SingleOrDefault();

        /// <summary>
        /// 
        /// </summary>
        public static DomainContainer Current => InstallApp();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        public static void Reopen()
        {
            _current = new DomainContainer();
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public virtual IQueryable<VW_Profissional> VW_Profissional =>
            Database.SqlQuery<VW_Profissional>("SELECT * FROM [api].[VW_Profissional]").AsQueryable();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static ICollection<Type> MapOdata()
        {
            var tdc = typeof(DomainContainer);
            return tdc.GetProperties().Where(x => x.DeclaringType != null && x.DeclaringType.Name == typeof(DbSet<>).Name)
                .SelectMany(x => x.DeclaringType.GenericTypeArguments).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public virtual IQueryable<VW_IdentificacaoUsuarioCidadao> VW_IdentificacaoUsuarioCidadao
            => Database
                .SqlQuery<VW_IdentificacaoUsuarioCidadao>("SELECT * FROM [api].[VW_IdentificacaoUsuarioCidadao] WHERE id IS NOT NULL")
                .AsQueryable();
    }
}
