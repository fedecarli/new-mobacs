using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
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
        // ReSharper disable once InconsistentNaming
        public virtual DbRawSqlQuery<VW_Profissional> VW_Profissional =>
            Database.SqlQuery<VW_Profissional>("SELECT CBO, CNES, CNS, INE, Equipe, Nome, Profissao, Unidade FROM [api].[VW_Profissional]");
    }
}
