using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Softpark.Models
{
    /// <summary>
    /// Linguagem do sistema
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
    /// Dominio da aplicação
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
        
        /// <summary>
        /// Coleção de profissionais
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public virtual DbRawSqlQuery<VW_Profissional> VW_Profissional =>
            Database.SqlQuery<VW_Profissional>("SELECT CBO, CNES, CNS, INE, Equipe, Nome, Profissao, Unidade, CodUsu FROM [api].[VW_Profissional]");
    }
}
