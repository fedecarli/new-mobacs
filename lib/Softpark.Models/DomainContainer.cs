using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
        private static string DbConn
        {
            get
            {
                var conn = ConfigurationManager.AppSettings["defaultDbConn"];

                var asp = ConfigurationManager.AppSettings["aspDbConnPath"];

                if (asp != null && File.Exists(asp))
                {
                    asp = File.ReadAllText(asp)?.Replace("\r\n", "\n").Replace("\r", "\n");

                    var lines = asp.Split("\n"[0]);

                    string ambiente = null;
                    bool inBlock = false;

                    var conf = new Dictionary<string, string> {
                        { "dbProvider", null },
                        { "dbServidor", null },
                        { "dbUsuario", null },
                        { "dbSenha", null },
                        { "db", null }
                    };

                    var regVar = "^(.*)(dbProvider|dbServidor|dbUsuario|dbSenha|db)(.+=[^\"]+\")([^\"]+)(\".*)$";
                    var regAmb = "^(.*Ambiente=\")([^\"]+)(\".*)$";
                    var regCas = "^(.*case \"{0}\".*)$";

                    foreach (var line in lines)
                    {

                        if (ambiente == null && Regex.IsMatch(line, regAmb, RegexOptions.IgnoreCase))
                        {
                            ambiente = Regex.Replace(line, regAmb, "$2", RegexOptions.IgnoreCase);
                        }
                        else if (ambiente != null && inBlock && Regex.IsMatch(line, regVar, RegexOptions.IgnoreCase))
                        {
                            var k = Regex.Replace(line, regVar, "$2", RegexOptions.IgnoreCase);
                            var v = Regex.Replace(line, regVar, "$4", RegexOptions.IgnoreCase);
                            conf[k] = v;
                        }
                        else if (ambiente != null && !inBlock && Regex.IsMatch(line, string.Format(regCas, ambiente), RegexOptions.IgnoreCase))
                        {
                            inBlock = true;
                        }
                        else if (ambiente != null && inBlock &&
                            (Regex.IsMatch(line, string.Format(regCas, ".+")) || line.StartsWith("end select")))
                        {
                            inBlock = false;
                        }
                    }

                    if (conf.All(x => x.Value != null))
                    {
                        conn = $"Data Source={conf["dbServidor"]};Initial Catalog={conf["db"]};User Id={conf["dbUsuario"]};Password={conf["dbSenha"]}";
                    }
                }

                return conn;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DomainContainer() :
            base(string.Format(ConfigurationManager.ConnectionStrings[ContainerName]
                .ConnectionString, DbConn))
        {
            _ctorDomainContainer();
        }

        private static DomainContainer _current;

        /// <summary>
        /// Static Instance
        /// </summary>
        public static DomainContainer Current
        {
            get => _current ?? (_current = new DomainContainer());
            set => _current = value;
        }

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
            Database.SqlQuery<VW_Profissional>("SELECT id, CNS, Nome, CBO, Profissao, CNES, Unidade, INE, Equipe, CodUsu FROM [api].[VW_Profissional]");
    }
}
