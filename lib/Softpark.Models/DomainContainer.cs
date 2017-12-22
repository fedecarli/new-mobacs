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
using System.Web;

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
        public DomainContainer() : this(string.Format(ConfigurationManager.ConnectionStrings[ContainerName]
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
        /// Coleção de profissionais
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public virtual DbRawSqlQuery<VW_Profissional> VW_Profissionais(string cnes, string nomeOuCns, int limit)
        {
            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(cnes?.Trim()))
                parameters.Add(new SqlParameter("@cnes", cnes));

            if (!string.IsNullOrEmpty(nomeOuCns?.Trim()))
                parameters.Add(new SqlParameter("@cns", nomeOuCns));

            if (!string.IsNullOrEmpty(nomeOuCns?.Trim()))
                parameters.Add(new SqlParameter("@nome", $"%{nomeOuCns}%"));

            return Database.SqlQuery<VW_Profissional>("SELECT TOP " + limit +
                @" a.id, a.CNS, a.Nome, LTRIM(RTRIM(a.CBO)) AS CBO,
                    a.Profissao, a.CNES, a.Unidade,
                    CAST(c.CodINE AS VARCHAR(18)) AS INE,
                    (a.INE + '-' + a.Equipe) AS Equipe,
                    a.CodUsu
                FROM [api].[VW_Profissional] AS a
                LEFT JOIN dbo.SetoresINEs AS c ON a.INE COLLATE Latin1_General_CI_AI = LTRIM(RTRIM(c.Numero COLLATE Latin1_General_CI_AI))
                WHERE a.CNS IS NOT NULL" + (string.IsNullOrEmpty(cnes?.Trim()) ? "" : " AND a.CNES = @cnes") +
                (string.IsNullOrEmpty(nomeOuCns?.Trim()) ? "" : " AND (a.CNS = @cns OR a.Nome LIKE @nome)") +
                " ORDER BY a.Nome, a.Profissao, a.Unidade, a.Equipe", parameters: parameters.ToArray());
        }

        /// <summary>
        /// Coleção de profissionais
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public virtual DbRawSqlQuery<VW_Profissional> VW_Profissionais(string ficha, string cnes, string nomeOuCns, int limit)
        {
            var parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@ficha", ficha));

            if (!string.IsNullOrEmpty(cnes?.Trim()))
                parameters.Add(new SqlParameter("@cnes", cnes));

            if (!string.IsNullOrEmpty(nomeOuCns?.Trim()))
                parameters.Add(new SqlParameter("@cns", nomeOuCns));

            if (!string.IsNullOrEmpty(nomeOuCns?.Trim()))
                parameters.Add(new SqlParameter("@nome", $"%{nomeOuCns}%"));

            var query = "SELECT DISTINCT TOP " + limit +
                @" a.id, a.CNS, a.Nome, LTRIM(RTRIM(a.CBO)) AS CBO,
                    a.Profissao, a.CNES, a.Unidade,
                    COALESCE(CAST(c.CodINE AS VARCHAR(18)), '') AS INE,
                    COALESCE((a.INE + ' - ' + a.Equipe), '') AS Equipe,
                    a.CodUsu, CAST((CASE b.Ficha WHEN @ficha THEN 1 ELSE 0 END) AS BIT) AS Autorizado
                FROM [api].[VW_Profissional] AS a
                LEFT JOIN dbo.SIGSM_FichaProfissao AS b ON LTRIM(RTRIM(a.CBO)) = LTRIM(RTRIM(b.CBO)) AND b.Ficha = @ficha
                LEFT JOIN dbo.SetoresINEs AS c ON a.INE COLLATE Latin1_General_CI_AI = LTRIM(RTRIM(c.Numero COLLATE Latin1_General_CI_AI))
                WHERE a.CNS IS NOT NULL" +
                (string.IsNullOrEmpty(cnes?.Trim()) ? "" : " AND a.CNES = @cnes") +
                (string.IsNullOrEmpty(nomeOuCns?.Trim()) ? "" : " AND (a.CNS = @cns OR a.Nome LIKE @nome)") +
                " ORDER BY Nome, Profissao, Unidade, Equipe, Autorizado DESC";

            return ficha == null ? VW_Profissionais(cnes, nomeOuCns, limit) :
            Database.SqlQuery<VW_Profissional>(query, parameters: parameters.ToArray());
        }

        /// <summary>
        /// Buscar Profissional válido
        /// </summary>
        /// <param name="cnes"></param>
        /// <param name="ine"></param>
        /// <param name="cbo"></param>
        /// <param name="cns"></param>
        /// <returns></returns>
        public virtual VW_Profissional GetProfissionalMobile(string cnes, string ine, string cbo, string cns)
        {
            var parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(ine?.Trim()))
                parameters.Add(new SqlParameter("@ine", ine));

            parameters.Add(new SqlParameter("@cnes", cnes));

            parameters.Add(new SqlParameter("@cns", cnes));

            parameters.Add(new SqlParameter("@cbo", cbo));

            var query = "SELECT DISTINCT TOP 10" +
                @" a.id, a.CNS, a.Nome, LTRIM(RTRIM(a.CBO)) AS CBO,
                    a.Profissao, a.CNES, a.Unidade,
                    COALESCE(CAST(c.CodINE AS VARCHAR(18)), '') AS INE,
                    COALESCE((a.INE + ' - ' + a.Equipe), '') AS Equipe,
                    a.CodUsu
                FROM [api].[VW_Profissional] AS a
                INNER JOIN dbo.SIGSM_FichaProfissao AS b ON LTRIM(RTRIM(a.CBO)) = LTRIM(RTRIM(b.CBO)) AND b.Ficha IN ('CadastroIndividual', 'CadastroDomiciliar', 'VisitaDomiciliar')
                LEFT JOIN dbo.SetoresINEs AS c ON a.INE COLLATE Latin1_General_CI_AI = LTRIM(RTRIM(c.Numero COLLATE Latin1_General_CI_AI))
                WHERE a.CNS IS NOT NULL" +
                (string.IsNullOrEmpty(ine?.Trim()) ? "" : " AND a.INE = @ine") +
                " AND a.CNES = @cnes" +
                " AND a.CNS = @cns AND a.CBO = @cbo" +
                " ORDER BY Nome, Profissao, Unidade, Equipe";

            HttpContext.Current.Response.Write(query + $"[{ine},{cnes},{cns},{cbo}]");

            return Database.SqlQuery<VW_Profissional>(query, parameters: parameters.ToArray()).FirstOrDefault();
        }

        /// <summary>
        /// Buscar Profissional válido
        /// </summary>
        /// <param name="ficha"></param>
        /// <param name="cnes"></param>
        /// <param name="ine"></param>
        /// <param name="nomeOuCns"></param>
        /// <returns></returns>
        public virtual VW_Profissional GetProfissional(string ficha, string cnes, string ine, string nomeOuCns)
        {
            var parameters = new List<SqlParameter>();

            parameters.Add(new SqlParameter("@ficha", ficha));

            if (!string.IsNullOrEmpty(ine?.Trim()))
                parameters.Add(new SqlParameter("@ine", ine));

            if (!string.IsNullOrEmpty(cnes?.Trim()))
                parameters.Add(new SqlParameter("@cnes", cnes));

            if (!string.IsNullOrEmpty(nomeOuCns?.Trim()))
                parameters.Add(new SqlParameter("@cns", nomeOuCns));

            if (!string.IsNullOrEmpty(nomeOuCns?.Trim()))
                parameters.Add(new SqlParameter("@nome", $"%{nomeOuCns}%"));

            var query = "SELECT DISTINCT TOP 1" +
                @" a.id, a.CNS, a.Nome, LTRIM(RTRIM(a.CBO)) AS CBO,
                    a.Profissao, a.CNES, a.Unidade,
                    COALESCE(CAST(c.CodINE AS VARCHAR(18)), '') AS INE,
                    COALESCE((a.INE + ' - ' + a.Equipe), '') AS Equipe,
                    a.CodUsu
                FROM [api].[VW_Profissional] AS a
                INNER JOIN dbo.SIGSM_FichaProfissao AS b ON LTRIM(RTRIM(a.CBO)) = LTRIM(RTRIM(b.CBO)) AND b.Ficha = @ficha
                LEFT JOIN dbo.SetoresINEs AS c ON a.INE COLLATE Latin1_General_CI_AI = LTRIM(RTRIM(c.Numero COLLATE Latin1_General_CI_AI))
                WHERE a.CNS IS NOT NULL" +
                (string.IsNullOrEmpty(ine?.Trim()) ? "" : " AND a.INE = @ine") +
                (string.IsNullOrEmpty(cnes?.Trim()) ? "" : " AND a.CNES = @cnes") +
                (string.IsNullOrEmpty(nomeOuCns?.Trim()) ? "" : " AND (a.CNS = @cns OR a.Nome LIKE @nome)") +
                " ORDER BY Nome, Profissao, Unidade, Equipe";

            return Database.SqlQuery<VW_Profissional>(query, parameters: parameters.ToArray()).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual DbRawSqlQuery<VW_UsuariosACS> VW_UsuariosACS()
        {
            var qry = @"select distinct u.CodUsu, RTRIM(LTRIM(u.Login)) AS [Login], p.Nome,
u.Senha, RTRIM(LTRIM(u.Email)) AS Email, c.CodCred, RTRIM(LTRIM(cv.Matricula)) AS Matricula, cv.ItemVinc,
c.Codigo, p.CNS, p.INE, p.CNES, p.CBO
from ASSMED_Usuario u
inner join AS_CredenciadosUsu cu on u.CodUsu = cu.CodUsuD
inner join AS_Credenciados c on cu.CodCred = c.CodCred
inner join AS_CredenciadosVinc cv on c.CodCred = cv.CodCred
inner join ASSMED_Cadastro cad on c.Codigo = cad.Codigo
inner join ASSMED_CadastroDocPessoal cdp on cad.Codigo = cdp.Codigo
INNER JOIN dbo.SIGSM_FichaProfissao AS b ON LTRIM(RTRIM(cv.CodProfTab COLLATE Latin1_General_CI_AI)) = LTRIM(RTRIM(b.CBO)) AND b.Ficha IN ('CadastroIndividual', 'CadastroDomiciliar', 'VisitaDomiciliar')
LEFT JOIN dbo.SetoresINEs AS d ON cv.CodINE = d.CodINE
INNER JOIN dbo.Setores s ON cv.CodSetor = s.CodSetor
INNER JOIN dbo.AS_SetoresPar sp ON s.CodSetor = sp.CodSetor
INNER JOIN api.VW_Profissional p ON p.CNS = RTRIM(LTRIM(cdp.Numero)) COLLATE Latin1_General_CI_AI AND p.CNES = sp.CNES COLLATE Latin1_General_CI_AI
AND LTRIM(RTRIM(b.CBO)) = p.CBO AND (p.INE = RTRIM(LTRIM(d.Numero COLLATE Latin1_General_CI_AI)) OR p.INE IS NULL)
where cdp.CodTpDocP = 6 AND Ativo = 1 AND RTRIM(LTRIM(cv.CodProfTab)) like '515105'";

            try
            {
                return Database.SqlQuery<VW_UsuariosACS>(qry);
            }
            catch (Exception e)
            {
                throw new Exception(qry, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual DbRawSqlQuery<VW_MenuSistema> VW_MenuSistema(string page)
        {
            var qry = $"SELECT * FROM VW_MenuSistema WHERE link = @page or sublink = @page";

            try
            {
                return Database.SqlQuery<VW_MenuSistema>(qry, new SqlParameter("@page", page));
            }
            catch (Exception e)
            {
                throw new Exception(qry, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual DbRawSqlQuery<VW_MenuSistema> VW_MenuSistema(int idUsuario, int? idPai, int idSistema)
        {
            var qry = $@"
                     SELECT m.id_menu,
                            m.id_pai_indireto,
                            m.id_pai_direto,
                            m.link,
                            m.sublink,
                            m.icone,
                            m.descricao,
                            m.ordem
                       FROM VW_MenuSistema AS m
                 INNER JOIN grupo_menu AS gm
                         ON m.id_menu = gm.id_menu
                        AND (gm.excluir = 1
                         OR gm.atualizar = 1
                         OR gm.ler = 1
                         OR gm.cadastrar = 1
                         OR gm.imprimir = 1)
                 INNER JOIN grupo_usuario AS gu
                         ON gm.id_grupo = gu.id_grupo
                        AND gu.CodUsu = @idUsuario
                      WHERE m.id_sistema = @idSistema
                        AND m.id_pai_direto {(Nullable.Equals(null, idPai) ? "IS NULL" : $" = {idPai}")}
                   GROUP BY m.id_menu,
                            m.id_pai_indireto,
                            m.id_pai_direto,
                            m.link,
                            m.sublink,
                            m.icone,
                            m.descricao,
                            m.ordem
                   ORDER BY m.ordem";

            try
            {
                return Database.SqlQuery<VW_MenuSistema>(qry, new SqlParameter("@idUsuario", idUsuario),
                    new SqlParameter("@idSistema", idSistema));
            }
            catch (Exception e)
            {
                throw new Exception(qry, e);
            }
        }

        /// <summary>
        /// ASSMED Cadastros
        /// </summary>
        public virtual DbRawSqlQuery<VW_Cadastros> VW_Cadastros(string q, int limit)
        {
            string query = "",
                   qry = "";

            try
            {
                if (q != null && !string.IsNullOrEmpty(q.Trim()))
                {
                    query = $@"
        AND (CAST(C.CODIGO AS VARCHAR(18)) = @q
         OR LTRIM(RTRIM(CONVERT(VARCHAR(10),CONVERT(date, COALESCE(ISNULL(C1.DTNASC,C2.DTNASC), ''),106),103))) COLLATE Latin1_General_CI_AI = @q
         OR LTRIM(RTRIM(COALESCE(ISNULL(C1.CPF,C2.CPF), ''))) COLLATE Latin1_General_CI_AI = @q
         OR REPLACE(REPLACE(LTRIM(RTRIM(COALESCE(ISNULL(C1.CPF,C2.CPF), ''))) COLLATE Latin1_General_CI_AI, '-', ''), '.', '') = @q
         OR LTRIM(RTRIM(C.NOME)) COLLATE Latin1_General_CI_AI LIKE '%' + @q + '%')";
                }

                qry = $@"
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
   ORDER BY C.Nome COLLATE Latin1_General_CI_AI";

                return Database.SqlQuery<VW_Cadastros>(qry, new SqlParameter("@q", q));
            }
            catch (Exception e)
            {
                throw new Exception(qry, e);
            }
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
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool RunningInSantana()
        {
            return Database.SqlQuery<bool>(
                "SELECT CAST(COUNT(name) AS BIT) FROM sys.views WHERE name LIKE 'VW_ultimo_cadastroIndividual' AND schema_id = SCHEMA_ID('api')"
                ).Single();
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
