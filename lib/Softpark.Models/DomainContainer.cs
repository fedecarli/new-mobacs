using Softpark.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        /// <summary>
        /// ASSMED Cadastros
        /// </summary>
        public virtual DbRawSqlQuery<VW_Cadastros> VW_Cadastros(string q, int limit)
        {
            string query = "";

            if (q != null && !string.IsNullOrEmpty(q.Trim()))
            {
                query = $@"
        AND (CAST(C.CODIGO AS VARCHAR(18)) = @q
         OR LTRIM(RTRIM(CONVERT(VARCHAR(10),CONVERT(date, COALESCE(ISNULL(C1.DTNASC,C2.DTNASC), ''),106),103))) COLLATE Latin1_General_CI_AI = @q
         OR LTRIM(RTRIM(COALESCE(ISNULL(C1.CPF,C2.CPF), ''))) COLLATE Latin1_General_CI_AI = @q
         OR REPLACE(REPLACE(LTRIM(RTRIM(COALESCE(ISNULL(C1.CPF,C2.CPF), ''))) COLLATE Latin1_General_CI_AI, '-', ''), '.', '') = @q
         OR LTRIM(RTRIM(C.NOME)) COLLATE Latin1_General_CI_AI LIKE '%' + @q + '%')";
            }

            return Database.SqlQuery<VW_Cadastros>($@"
     SELECT TOP {limit} C.CODIGO,
			LTRIM(RTRIM(COALESCE(D.NUMERO, ''))) COLLATE Latin1_General_CI_AI AS CNS,
			LTRIM(RTRIM(C.NOME)) COLLATE Latin1_General_CI_AI AS NOME,
			LTRIM(RTRIM(CONVERT(varchar(10),CONVERT(date, COALESCE(ISNULL(C1.DTNASC,C2.DTNASC), ''),106),103))) COLLATE Latin1_General_CI_AI AS DTNASC,
			LTRIM(RTRIM(COALESCE(ISNULL(C1.CPF,C2.CPF), ''))) COLLATE Latin1_General_CI_AI AS CPF,
			LTRIM(RTRIM(COALESCE(ISNULL(C1.NomeMae,C2.NomeMae), ''))) COLLATE Latin1_General_CI_AI AS NOMEMAE,
			(CASE ISNULL(C1.Sexo,C2.Sexo) WHEN 'M' THEN 0 WHEN 'F' THEN 1 ELSE 4 END) AS SEXO
	   FROM ASSMED_CADASTRO C
  LEFT JOIN ASSMED_CADASTROPF C1
		 ON C.CODIGO=C1.CODIGO
  LEFT JOIN ASSMED_PESFISICA C2
		 ON C.CODIGO=C2.CODIGO
  LEFT JOIN ASSMED_CadastroDocPessoal D
		 ON C.CODIGO=D.CODIGO AND D.CodTpDocP=6
	  WHERE Nome IS NOT NULL
		AND LEN(LTRIM(RTRIM(C.NOME)) COLLATE Latin1_General_CI_AI) > 0
		AND LTRIM(RTRIM(C.NOME)) COLLATE Latin1_General_CI_AI NOT LIKE '%*%' {query}
   ORDER BY C.Nome COLLATE Latin1_General_CI_AI", new SqlParameter("@q", q));
        }

        /// <summary>
        /// Fichas Visitas Master
        /// </summary>
        public virtual object[][] VW_FichasMasters(string search, int take, int skip, int ordCol, int ordDir, string cnes)
        {
            search = search != null && !string.IsNullOrEmpty(search.Trim()) ? search.Trim() : string.Empty;

            return Database.SqlQuery<VW_FichasMaster>(VW_FichasMaster.GetSearchCommand(search, ordCol, ordDir, take, skip),
                new SqlParameter("@skip", skip < 0 ? 0 : skip),
                new SqlParameter("@take", take < 10 ? 10 : take),
                new SqlParameter("@search", search),
                new SqlParameter("@cnes", cnes))
                .ToArray()
                .Select(x => new object[] { x.Data, x.Profissional, x.Profissao, x.Equipe, x.Status, x.UuidFicha })
                .ToArray();
        }

        /// <summary>
        /// Fichas Cadastros Individuais
        /// </summary>
        /// <param name="search"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="ordCol"></param>
        /// <param name="ordDir"></param>
        /// <returns></returns>
        public virtual object[][] VW_CadastroIndividuais(string search, int skip, int take, int ordCol, int ordDir)
        {
            search = search != null && !string.IsNullOrEmpty(search.Trim()) ? search.Trim() : null;

            var has_vuci = Database.SqlQuery<bool>(
                "SELECT CAST(COUNT(name) AS BIT) FROM sys.views WHERE name LIKE 'VW_ultimo_cadastroIndividual' AND schema_id = SCHEMA_ID('api')"
                ).Single();

            Guid sGuid = Guid.Empty;
            var isGuid = search != null && Guid.TryParse(search, out sGuid);
            var isDate = search != null && Regex.IsMatch(search, "^([0-9]{2}/[0-9]{2}/[0-9]{4})$", RegexOptions.IgnoreCase);
            var isCns = search != null && search.isValidCns();
            decimal cns = 0;
            var isNum = search != null && decimal.TryParse(search, out cns);

            return Database.SqlQuery<VW_CadastroIndividual>(VW_CadastroIndividual.GetSearchCommand(search, ordCol, ordDir, take, skip, has_vuci),
                new SqlParameter("@skip", skip < 0 ? 0 : skip),
                new SqlParameter("@take", take < 10 ? 10 : take),
                search == null ? new SqlParameter("@search", string.Empty) :
                isGuid ? new SqlParameter("@search", sGuid) :
                isDate ? new SqlParameter("@search", search) :
                isCns ? new SqlParameter("@search", search) :
                isNum ? new SqlParameter("@search", cns) :
                new SqlParameter("@search", search))
                .ToArray()
                .Select(x => new object[] { x.Nome, x.DtNasc, x.NomeMae, x.Cns, x.NomeCidade, x.Codigo })
                .ToArray();
        }

        /// <summary>
        /// Fichas Cadastros Individuais
        /// </summary>
        /// <param name="search"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="ordCol"></param>
        /// <param name="ordDir"></param>
        /// <returns></returns>
        public virtual object[][] VW_CadastroDomiciliares(string search, int skip, int take, int ordCol, int ordDir)
        {
            search = search != null && !string.IsNullOrEmpty(search.Trim()) ? search.Trim() : null;

            Guid sGuid = Guid.Empty;
            var isGuid = search != null && Guid.TryParse(search, out sGuid);

            return Database.SqlQuery<VW_CadastroDomiciliar>(VW_CadastroDomiciliar.GetSearchCommand(search, ordCol, ordDir, take, skip),
                new SqlParameter("@skip", skip < 0 ? 0 : skip),
                new SqlParameter("@take", take < 10 ? 10 : take),
                search == null ? new SqlParameter("@search", string.Empty) :
                isGuid ? new SqlParameter("@search", sGuid) :
                new SqlParameter("@search", search))
                .ToArray()
                .Select(x => new object[] { x.NomeLogradouro, x.Numero, x.Complemento, x.TelefoneResidencia, x.UuidFicha })
                .ToArray();
        }
    }
}
