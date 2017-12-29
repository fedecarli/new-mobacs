using Softpark.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
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
    [Table("sys.SYSLANGUAGES")]
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
    public partial class DomainContainer : DbContext
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
        public static string ContainerName => "DomainContainer";

        private void _ctorDomainContainer()
        //: base("name=DomainContainer")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public DomainContainer() : this(string.Format(ConfigurationManager.ConnectionStrings[ContainerName]
                .ConnectionString, DbConn))
        {
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

        public virtual ObjectResult<PR_ConsultaCadastroIndividuais_Result> PR_ConsultaCadastroIndividuais(string search, Nullable<int> skip, Nullable<int> take, Nullable<int> orderCol, Nullable<int> orderDirection, ObjectParameter total, ObjectParameter totalFiltered)
        {
            var searchParameter = search != null ?
                new ObjectParameter("search", search) :
                new ObjectParameter("search", typeof(string));

            var skipParameter = skip.HasValue ?
                new ObjectParameter("skip", skip) :
                new ObjectParameter("skip", typeof(int));

            var takeParameter = take.HasValue ?
                new ObjectParameter("take", take) :
                new ObjectParameter("take", typeof(int));

            var orderColParameter = orderCol.HasValue ?
                new ObjectParameter("orderCol", orderCol) :
                new ObjectParameter("orderCol", typeof(int));

            var orderDirectionParameter = orderDirection.HasValue ?
                new ObjectParameter("orderDirection", orderDirection) :
                new ObjectParameter("orderDirection", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PR_ConsultaCadastroIndividuais_Result>("PR_ConsultaCadastroIndividuais", searchParameter, skipParameter, takeParameter, orderColParameter, orderDirectionParameter, total, totalFiltered);
        }

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
        public virtual IQueryable<VW_MenuSistema> Get_VW_MenuSistema(string page) =>
            VW_MenuSistema.Where(x => x.link == page || x.sublink == page);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual DbRawSqlQuery<VW_MenuSistema> Get_VW_MenuSistema(int idUsuario, int? idPai, int idSistema)
        {
            var qry = $@"
                     SELECT m.*
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
                            m.ordem,
                            m.id_sistema
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
                .Select(x => new object[] { x.NomeLogradouro, x.Numero, x.Complemento, x.TelefoneResidencia, x.UuidFicha?.ToString()??x.Codigo.ToString() })
                .ToArray();
        }

        public virtual DbSet<AnimalNoDomicilio> AnimalNoDomicilio { get; set; }
        public virtual DbSet<CadastroDomiciliar> CadastroDomiciliar { get; set; }
        public virtual DbSet<CadastroIndividual> CadastroIndividual { get; set; }
        public virtual DbSet<CondicaoMoradia> CondicaoMoradia { get; set; }
        public virtual DbSet<CondicoesDeSaude> CondicoesDeSaude { get; set; }
        public virtual DbSet<DeficienciasCidadao> DeficienciasCidadao { get; set; }
        public virtual DbSet<Documentos> Documentos { get; set; }
        public virtual DbSet<DoencaCardiaca> DoencaCardiaca { get; set; }
        public virtual DbSet<DoencaRespiratoria> DoencaRespiratoria { get; set; }
        public virtual DbSet<DoencaRins> DoencaRins { get; set; }
        public virtual DbSet<EmSituacaoDeRua> EmSituacaoDeRua { get; set; }
        public virtual DbSet<EnderecoLocalPermanencia> EnderecoLocalPermanencia { get; set; }
        public virtual DbSet<Etnia> Etnia { get; set; }
        public virtual DbSet<FamiliaRow> FamiliaRow { get; set; }
        public virtual DbSet<FichaVisitaDomiciliarChild> FichaVisitaDomiciliarChild { get; set; }
        public virtual DbSet<FichaVisitaDomiciliarMaster> FichaVisitaDomiciliarMaster { get; set; }
        public virtual DbSet<HigienePessoalSituacaoRua> HigienePessoalSituacaoRua { get; set; }
        public virtual DbSet<IdentificacaoUsuarioCidadao> IdentificacaoUsuarioCidadao { get; set; }
        public virtual DbSet<InformacoesSocioDemograficas> InformacoesSocioDemograficas { get; set; }
        public virtual DbSet<InstituicaoPermanencia> InstituicaoPermanencia { get; set; }
        public virtual DbSet<OrigemAlimentoSituacaoRua> OrigemAlimentoSituacaoRua { get; set; }
        public virtual DbSet<OrigemVisita> OrigemVisita { get; set; }
        public virtual DbSet<RastroFicha> RastroFicha { get; set; }
        public virtual DbSet<ResponsavelPorCrianca> ResponsavelPorCrianca { get; set; }
        public virtual DbSet<SaidaCidadaoCadastro> SaidaCidadaoCadastro { get; set; }
        public virtual DbSet<TipoOrigem> TipoOrigem { get; set; }
        public virtual DbSet<UnicaLotacaoTransport> UnicaLotacaoTransport { get; set; }
        public virtual DbSet<AS_Credenciados> AS_Credenciados { get; set; }
        public virtual DbSet<AS_CredenciadosUsu> AS_CredenciadosUsu { get; set; }
        public virtual DbSet<AS_CredenciadosVinc> AS_CredenciadosVinc { get; set; }
        public virtual DbSet<AS_Profissoes> AS_Profissoes { get; set; }
        public virtual DbSet<AS_ProfissoesTab> AS_ProfissoesTab { get; set; }
        public virtual DbSet<AS_SetoresPar> AS_SetoresPar { get; set; }
        public virtual DbSet<ASSMED_Acesso> ASSMED_Acesso { get; set; }
        public virtual DbSet<ASSMED_Cadastro> ASSMED_Cadastro { get; set; }
        public virtual DbSet<ASSMED_CadastroDocPessoal> ASSMED_CadastroDocPessoal { get; set; }
        public virtual DbSet<ASSMED_CadastroPF> ASSMED_CadastroPF { get; set; }
        public virtual DbSet<ASSMED_CadEmails> ASSMED_CadEmails { get; set; }
        public virtual DbSet<ASSMED_CadTelefones> ASSMED_CadTelefones { get; set; }
        public virtual DbSet<ASSMED_Contratos> ASSMED_Contratos { get; set; }
        public virtual DbSet<ASSMED_Endereco> ASSMED_Endereco { get; set; }
        public virtual DbSet<ASSMED_PesFisica> ASSMED_PesFisica { get; set; }
        public virtual DbSet<ASSMED_TipoDocPessoal> ASSMED_TipoDocPessoal { get; set; }
        public virtual DbSet<ASSMED_Usuario> ASSMED_Usuario { get; set; }
        public virtual DbSet<Cidade> Cidade { get; set; }
        public virtual DbSet<Nacionalidade> Nacionalidade { get; set; }
        public virtual DbSet<Setores> Setores { get; set; }
        public virtual DbSet<SetoresINEs> SetoresINEs { get; set; }
        public virtual DbSet<SIGSM_FichaProfissao> SIGSM_FichaProfissao { get; set; }
        public virtual DbSet<SIGSM_MicroArea_CredenciadoCidadao> SIGSM_MicroArea_CredenciadoCidadao { get; set; }
        public virtual DbSet<SIGSM_MicroArea_CredenciadoVinc> SIGSM_MicroArea_CredenciadoVinc { get; set; }
        public virtual DbSet<SIGSM_MicroArea_Unidade> SIGSM_MicroArea_Unidade { get; set; }
        public virtual DbSet<SIGSM_MicroAreas> SIGSM_MicroAreas { get; set; }
        public virtual DbSet<SIGSM_MotivoVisita> SIGSM_MotivoVisita { get; set; }
        public virtual DbSet<SIGSM_ServicoSerializador_Agenda> SIGSM_ServicoSerializador_Agenda { get; set; }
        public virtual DbSet<SIGSM_ServicoSerializador_Config> SIGSM_ServicoSerializador_Config { get; set; }
        public virtual DbSet<SIGSM_ServicoSerializador_Fichas> SIGSM_ServicoSerializador_Fichas { get; set; }
        public virtual DbSet<SIGSM_Transmissao> SIGSM_Transmissao { get; set; }
        public virtual DbSet<SIGSM_Transmissao_Processos> SIGSM_Transmissao_Processos { get; set; }
        public virtual DbSet<SIGSM_Transmissao_Processos_Log> SIGSM_Transmissao_Processos_Log { get; set; }
        public virtual DbSet<SIGSM_Transmissao_StatusGeracao> SIGSM_Transmissao_StatusGeracao { get; set; }
        public virtual DbSet<TP_Abastecimento_Agua> TP_Abastecimento_Agua { get; set; }
        public virtual DbSet<TP_Acesso_Domicilio> TP_Acesso_Domicilio { get; set; }
        public virtual DbSet<TP_Animais> TP_Animais { get; set; }
        public virtual DbSet<TP_Cond_Posse_Uso_Terra> TP_Cond_Posse_Uso_Terra { get; set; }
        public virtual DbSet<TP_Condicao_Avaliada> TP_Condicao_Avaliada { get; set; }
        public virtual DbSet<TP_Consideracao_Peso> TP_Consideracao_Peso { get; set; }
        public virtual DbSet<TP_Construcao_Domicilio> TP_Construcao_Domicilio { get; set; }
        public virtual DbSet<TP_Crianca> TP_Crianca { get; set; }
        public virtual DbSet<TP_Curso> TP_Curso { get; set; }
        public virtual DbSet<TP_Deficiencia> TP_Deficiencia { get; set; }
        public virtual DbSet<TP_Destino_Lixo> TP_Destino_Lixo { get; set; }
        public virtual DbSet<TP_Doenca_Cardiaca> TP_Doenca_Cardiaca { get; set; }
        public virtual DbSet<TP_Doenca_Renal> TP_Doenca_Renal { get; set; }
        public virtual DbSet<TP_Doenca_Respiratoria> TP_Doenca_Respiratoria { get; set; }
        public virtual DbSet<TP_Domicilio> TP_Domicilio { get; set; }
        public virtual DbSet<TP_Escoamento_Esgoto> TP_Escoamento_Esgoto { get; set; }
        public virtual DbSet<TP_EstadoCivil> TP_EstadoCivil { get; set; }
        public virtual DbSet<TP_Exames> TP_Exames { get; set; }
        public virtual DbSet<TP_Higiene_Pessoal> TP_Higiene_Pessoal { get; set; }
        public virtual DbSet<TP_Identidade_Genero_Cidadao> TP_Identidade_Genero_Cidadao { get; set; }
        public virtual DbSet<TP_Imovel> TP_Imovel { get; set; }
        public virtual DbSet<TP_Localizacao> TP_Localizacao { get; set; }
        public virtual DbSet<TP_Motivo_Saida> TP_Motivo_Saida { get; set; }
        public virtual DbSet<TP_Nacionalidade> TP_Nacionalidade { get; set; }
        public virtual DbSet<TP_Orientacao_Sexual> TP_Orientacao_Sexual { get; set; }
        public virtual DbSet<TP_Origem_Alimentacao> TP_Origem_Alimentacao { get; set; }
        public virtual DbSet<TP_Quantas_Vezes_Alimentacao> TP_Quantas_Vezes_Alimentacao { get; set; }
        public virtual DbSet<TP_Raca_Cor> TP_Raca_Cor { get; set; }
        public virtual DbSet<TP_Relacao_Parentesco> TP_Relacao_Parentesco { get; set; }
        public virtual DbSet<TP_Renda_Familiar> TP_Renda_Familiar { get; set; }
        public virtual DbSet<TP_Saida_Cadastro> TP_Saida_Cadastro { get; set; }
        public virtual DbSet<TP_Sexo> TP_Sexo { get; set; }
        public virtual DbSet<TP_Sexo_Genero> TP_Sexo_Genero { get; set; }
        public virtual DbSet<TP_Sit_Mercado> TP_Sit_Mercado { get; set; }
        public virtual DbSet<TP_Sit_Rua> TP_Sit_Rua { get; set; }
        public virtual DbSet<TP_Situacao_Moradia> TP_Situacao_Moradia { get; set; }
        public virtual DbSet<TP_Situacao_Moradia_Rural> TP_Situacao_Moradia_Rural { get; set; }
        public virtual DbSet<TP_Tratamento_Agua> TP_Tratamento_Agua { get; set; }
        public virtual DbSet<UF> UF { get; set; }
        public virtual DbSet<TB_MS_TIPO_LOGRADOURO> TB_MS_TIPO_LOGRADOURO { get; set; }
        public virtual DbSet<TP_Conduta> TP_Conduta { get; set; }
        public virtual DbSet<TP_Nasf> TP_Nasf { get; set; }
        public virtual DbSet<VW_Cadastros_Zoneamento> VW_Cadastros_Zoneamento { get; set; }
        public virtual DbSet<VW_ConsultaCadastrosDomiciliares> VW_ConsultaCadastrosDomiciliares { get; set; }
        public virtual DbSet<VW_ConsultaCadastrosIndividuais> VW_ConsultaCadastrosIndividuais { get; set; }
        public virtual DbSet<VW_MenuSistema> VW_MenuSistema { get; set; }
        public virtual DbSet<VW_SystemLanguage> VW_SystemLanguage { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CadastroDomiciliar>()
                .HasMany(e => e.AnimalNoDomicilio)
                .WithRequired(e => e.CadastroDomiciliar)
                .HasForeignKey(e => e.id_cadastro_domiciliar)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CadastroDomiciliar>()
                .HasMany(e => e.FamiliaRow)
                .WithMany(e => e.CadastroDomiciliar)
                .Map(m => m.ToTable("Familias", "api").MapLeftKey("id_cadatro_domiciliar").MapRightKey("id_familia_row"));

            modelBuilder.Entity<CondicaoMoradia>()
                .HasMany(e => e.CadastroDomiciliar)
                .WithOptional(e => e.CondicaoMoradia1)
                .HasForeignKey(e => e.condicaoMoradia);

            modelBuilder.Entity<CondicoesDeSaude>()
                .HasMany(e => e.CadastroIndividual)
                .WithOptional(e => e.CondicoesDeSaude1)
                .HasForeignKey(e => e.condicoesDeSaude);

            modelBuilder.Entity<CondicoesDeSaude>()
                .HasMany(e => e.DoencaCardiaca)
                .WithRequired(e => e.CondicoesDeSaude)
                .HasForeignKey(e => e.id_condicao_de_saude)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CondicoesDeSaude>()
                .HasMany(e => e.DoencaRespiratoria)
                .WithRequired(e => e.CondicoesDeSaude)
                .HasForeignKey(e => e.id_condicao_de_saude)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CondicoesDeSaude>()
                .HasMany(e => e.DoencaRins)
                .WithRequired(e => e.CondicoesDeSaude)
                .HasForeignKey(e => e.id_condicao_de_saude)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Documentos>()
                .Property(e => e.TipoArquivo)
                .IsUnicode(false);

            modelBuilder.Entity<EmSituacaoDeRua>()
                .HasMany(e => e.CadastroIndividual)
                .WithOptional(e => e.EmSituacaoDeRua1)
                .HasForeignKey(e => e.emSituacaoDeRua);

            modelBuilder.Entity<EmSituacaoDeRua>()
                .HasMany(e => e.HigienePessoalSituacaoRua)
                .WithRequired(e => e.EmSituacaoDeRua)
                .HasForeignKey(e => e.id_em_situacao_de_rua)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EmSituacaoDeRua>()
                .HasMany(e => e.OrigemAlimentoSituacaoRua)
                .WithRequired(e => e.EmSituacaoDeRua)
                .HasForeignKey(e => e.id_em_situacao_rua)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<EnderecoLocalPermanencia>()
                .Property(e => e.cep)
                .IsFixedLength();

            modelBuilder.Entity<EnderecoLocalPermanencia>()
                .Property(e => e.codigoIbgeMunicipio)
                .IsFixedLength();

            modelBuilder.Entity<EnderecoLocalPermanencia>()
                .Property(e => e.numeroDneUf)
                .IsFixedLength();

            modelBuilder.Entity<EnderecoLocalPermanencia>()
                .Property(e => e.tipoLogradouroNumeroDne)
                .IsFixedLength();

            modelBuilder.Entity<EnderecoLocalPermanencia>()
                .Property(e => e.microarea)
                .IsFixedLength();

            modelBuilder.Entity<EnderecoLocalPermanencia>()
                .HasMany(e => e.CadastroDomiciliar)
                .WithOptional(e => e.EnderecoLocalPermanencia1)
                .HasForeignKey(e => e.enderecoLocalPermanencia);

            modelBuilder.Entity<EnderecoLocalPermanencia>()
                .HasMany(e => e.ASSMED_Endereco)
                .WithOptional(e => e.EnderecoLocalPermanencia)
                .HasForeignKey(e => e.IdFicha);

            modelBuilder.Entity<FamiliaRow>()
                .Property(e => e.numeroCnsResponsavel)
                .IsFixedLength();

            modelBuilder.Entity<FichaVisitaDomiciliarChild>()
                .Property(e => e.uuidFicha)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FichaVisitaDomiciliarChild>()
                .Property(e => e.cnsCidadao)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FichaVisitaDomiciliarChild>()
                .Property(e => e.microarea)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FichaVisitaDomiciliarChild>()
                .Property(e => e.pesoAcompanhamentoNutricional)
                .HasPrecision(6, 3);

            modelBuilder.Entity<FichaVisitaDomiciliarChild>()
                .Property(e => e.alturaAcompanhamentoNutricional)
                .HasPrecision(4, 1);

            modelBuilder.Entity<FichaVisitaDomiciliarChild>()
                .Property(e => e.nomeCidadao)
                .IsUnicode(false);

            modelBuilder.Entity<FichaVisitaDomiciliarChild>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<FichaVisitaDomiciliarChild>()
                .HasMany(e => e.SIGSM_MotivoVisita)
                .WithMany(e => e.FichaVisitaDomiciliarChild)
                .Map(m => m.ToTable("FichaVisitaDomiciliarChild_MotivoVisita", "api").MapLeftKey("childId").MapRightKey("codigo"));

            modelBuilder.Entity<FichaVisitaDomiciliarMaster>()
                .Property(e => e.uuidFicha)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<FichaVisitaDomiciliarMaster>()
                .HasMany(e => e.FichaVisitaDomiciliarChild)
                .WithRequired(e => e.FichaVisitaDomiciliarMaster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IdentificacaoUsuarioCidadao>()
                .Property(e => e.codigoIbgeMunicipioNascimento)
                .IsFixedLength();

            modelBuilder.Entity<IdentificacaoUsuarioCidadao>()
                .Property(e => e.cnsCidadao)
                .IsFixedLength();

            modelBuilder.Entity<IdentificacaoUsuarioCidadao>()
                .Property(e => e.cnsResponsavelFamiliar)
                .IsFixedLength();

            modelBuilder.Entity<IdentificacaoUsuarioCidadao>()
                .Property(e => e.numeroNisPisPasep)
                .IsFixedLength();

            modelBuilder.Entity<IdentificacaoUsuarioCidadao>()
                .Property(e => e.microarea)
                .IsFixedLength();

            modelBuilder.Entity<IdentificacaoUsuarioCidadao>()
                .Property(e => e.ComplementoRG)
                .IsUnicode(false);

            modelBuilder.Entity<IdentificacaoUsuarioCidadao>()
                .Property(e => e.CPF)
                .IsFixedLength();

            modelBuilder.Entity<IdentificacaoUsuarioCidadao>()
                .Property(e => e.EstadoCivil)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<IdentificacaoUsuarioCidadao>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<IdentificacaoUsuarioCidadao>()
                .HasMany(e => e.CadastroIndividual)
                .WithOptional(e => e.IdentificacaoUsuarioCidadao1)
                .HasForeignKey(e => e.identificacaoUsuarioCidadao);

            modelBuilder.Entity<IdentificacaoUsuarioCidadao>()
                .HasMany(e => e.Documentos)
                .WithRequired(e => e.IdentificacaoUsuarioCidadao)
                .HasForeignKey(e => e.IdIdentificacaoUsuarioCidadao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<IdentificacaoUsuarioCidadao>()
                .HasMany(e => e.ASSMED_Cadastro)
                .WithOptional(e => e.IdentificacaoUsuarioCidadao)
                .HasForeignKey(e => e.IdFicha);

            modelBuilder.Entity<InformacoesSocioDemograficas>()
                .Property(e => e.ocupacaoCodigoCbo2002)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<InformacoesSocioDemograficas>()
                .HasMany(e => e.CadastroIndividual)
                .WithOptional(e => e.InformacoesSocioDemograficas1)
                .HasForeignKey(e => e.informacoesSocioDemograficas);

            modelBuilder.Entity<InformacoesSocioDemograficas>()
                .HasMany(e => e.DeficienciasCidadao)
                .WithRequired(e => e.InformacoesSocioDemograficas)
                .HasForeignKey(e => e.id_informacoes_socio_demograficas)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InformacoesSocioDemograficas>()
                .HasMany(e => e.ResponsavelPorCrianca)
                .WithRequired(e => e.InformacoesSocioDemograficas)
                .HasForeignKey(e => e.id_informacoes_sociodemograficas)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InstituicaoPermanencia>()
                .Property(e => e.cnsResponsavelTecnico)
                .IsFixedLength();

            modelBuilder.Entity<InstituicaoPermanencia>()
                .HasMany(e => e.CadastroDomiciliar)
                .WithOptional(e => e.InstituicaoPermanencia1)
                .HasForeignKey(e => e.instituicaoPermanencia);

            modelBuilder.Entity<OrigemVisita>()
                .HasMany(e => e.UnicaLotacaoTransport)
                .WithRequired(e => e.OrigemVisita)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SaidaCidadaoCadastro>()
                .Property(e => e.numeroDO)
                .IsFixedLength();

            modelBuilder.Entity<SaidaCidadaoCadastro>()
                .HasMany(e => e.CadastroIndividual)
                .WithOptional(e => e.SaidaCidadaoCadastro1)
                .HasForeignKey(e => e.saidaCidadaoCadastro);

            modelBuilder.Entity<TipoOrigem>()
                .HasMany(e => e.OrigemVisita)
                .WithRequired(e => e.TipoOrigem)
                .HasForeignKey(e => e.id_tipo_origem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UnicaLotacaoTransport>()
                .Property(e => e.profissionalCNS)
                .IsFixedLength();

            modelBuilder.Entity<UnicaLotacaoTransport>()
                .Property(e => e.cboCodigo_2002)
                .IsFixedLength();

            modelBuilder.Entity<UnicaLotacaoTransport>()
                .Property(e => e.cnes)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UnicaLotacaoTransport>()
                .Property(e => e.ine)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UnicaLotacaoTransport>()
                .Property(e => e.codigoIbgeMunicipio)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UnicaLotacaoTransport>()
                .HasMany(e => e.CadastroDomiciliar)
                .WithRequired(e => e.UnicaLotacaoTransport)
                .HasForeignKey(e => e.headerTransport)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UnicaLotacaoTransport>()
                .HasMany(e => e.CadastroIndividual)
                .WithRequired(e => e.UnicaLotacaoTransport)
                .HasForeignKey(e => e.headerTransport)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UnicaLotacaoTransport>()
                .HasMany(e => e.FichaVisitaDomiciliarMaster)
                .WithRequired(e => e.UnicaLotacaoTransport)
                .HasForeignKey(e => e.headerTransport)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AS_Credenciados>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AS_Credenciados>()
                .Property(e => e.CNES)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AS_Credenciados>()
                .Property(e => e.NumConselho)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AS_Credenciados>()
                .Property(e => e.UF)
                .IsUnicode(false);

            modelBuilder.Entity<AS_Credenciados>()
                .Property(e => e.Tratamento)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AS_Credenciados>()
                .HasMany(e => e.AS_CredenciadosUsu)
                .WithRequired(e => e.AS_Credenciados)
                .HasForeignKey(e => new { e.NumContrato, e.CodCred })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AS_Credenciados>()
                .HasMany(e => e.AS_CredenciadosVinc)
                .WithRequired(e => e.AS_Credenciados)
                .HasForeignKey(e => new { e.NumContrato, e.CodCred })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AS_Credenciados>()
                .HasMany(e => e.SIGSM_MicroArea_CredenciadoVinc)
                .WithRequired(e => e.AS_Credenciados)
                .HasForeignKey(e => new { e.NumContrato, e.CodCred })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AS_CredenciadosUsu>()
                .Property(e => e.NumIP)
                .IsUnicode(false);

            modelBuilder.Entity<AS_CredenciadosVinc>()
                .Property(e => e.CNESLocal)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AS_CredenciadosVinc>()
                .Property(e => e.Matricula)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AS_CredenciadosVinc>()
                .Property(e => e.CodProfTab)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AS_CredenciadosVinc>()
                .HasMany(e => e.SIGSM_MicroArea_CredenciadoVinc)
                .WithRequired(e => e.AS_CredenciadosVinc)
                .HasForeignKey(e => new { e.NumContrato, e.CodCred, e.ItemVinc })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AS_Profissoes>()
                .Property(e => e.CodProfissao)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AS_Profissoes>()
                .Property(e => e.DesProfissao)
                .IsUnicode(false);

            modelBuilder.Entity<AS_Profissoes>()
                .HasMany(e => e.AS_ProfissoesTab)
                .WithRequired(e => e.AS_Profissoes)
                .HasForeignKey(e => new { e.NumContrato, e.CodProfissao })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AS_ProfissoesTab>()
                .Property(e => e.CodProfissao)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AS_ProfissoesTab>()
                .Property(e => e.CodProfTab)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AS_ProfissoesTab>()
                .Property(e => e.DesProfTab)
                .IsUnicode(false);

            modelBuilder.Entity<AS_SetoresPar>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AS_SetoresPar>()
                .Property(e => e.CNES)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Acesso>()
                .Property(e => e.EMail)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Acesso>()
                .Property(e => e.Validou)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Acesso>()
                .Property(e => e.NumIP)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Acesso>()
                .Property(e => e.ASPSESSIONIDQASRTRQT)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .Property(e => e.Tipo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .Property(e => e.NumIP)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .Property(e => e.NomeSocial)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .Property(e => e.Justificativa)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .Property(e => e.MotivoHomologacao)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .Property(e => e.MicroArea)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .HasMany(e => e.IdentificacaoUsuarioCidadao1)
                .WithOptional(e => e.ASSMED_Cadastro1)
                .HasForeignKey(e => new { e.num_contrato, e.Codigo });

            modelBuilder.Entity<ASSMED_Cadastro>()
                .HasMany(e => e.ASSMED_CadastroDocPessoal)
                .WithRequired(e => e.ASSMED_Cadastro)
                .HasForeignKey(e => new { e.NumContrato, e.Codigo })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .HasMany(e => e.ASSMED_CadEmails)
                .WithRequired(e => e.ASSMED_Cadastro)
                .HasForeignKey(e => new { e.NumContrato, e.Codigo })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .HasMany(e => e.ASSMED_CadTelefones)
                .WithRequired(e => e.ASSMED_Cadastro)
                .HasForeignKey(e => new { e.NumContrato, e.Codigo })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .HasMany(e => e.ASSMED_Endereco)
                .WithRequired(e => e.ASSMED_Cadastro)
                .HasForeignKey(e => new { e.NumContrato, e.Codigo })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .HasOptional(e => e.ASSMED_PesFisica)
                .WithRequired(e => e.ASSMED_Cadastro);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .HasOptional(e => e.ASSMED_CadastroPF)
                .WithRequired(e => e.ASSMED_Cadastro);

            modelBuilder.Entity<ASSMED_Cadastro>()
                .HasMany(e => e.SIGSM_MicroArea_CredenciadoCidadao)
                .WithRequired(e => e.ASSMED_Cadastro)
                .HasForeignKey(e => new { e.NumContrato, e.Codigo })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ASSMED_CadastroDocPessoal>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ASSMED_CadastroDocPessoal>()
                .Property(e => e.Numero)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroDocPessoal>()
                .Property(e => e.Serie)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroDocPessoal>()
                .Property(e => e.Secao)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroDocPessoal>()
                .Property(e => e.Zona)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroDocPessoal>()
                .Property(e => e.UFOrgao)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroDocPessoal>()
                .Property(e => e.NumIP)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroDocPessoal>()
                .Property(e => e.ComplementoRG)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.CPF)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.Sexo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.EstCivil)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.NomePai)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.NomeMae)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.Deficiente)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.TpSangue)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.Doador)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.UfNacao)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.MuniNacao)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.NATURALIZACAOPORTARIA)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.OBITONUMERO)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadastroPF>()
                .Property(e => e.OCUPACAO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadEmails>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ASSMED_CadEmails>()
                .Property(e => e.EMail)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadEmails>()
                .Property(e => e.TipoEMail)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadEmails>()
                .Property(e => e.NumIP)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadTelefones>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ASSMED_CadTelefones>()
                .Property(e => e.NumTel)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadTelefones>()
                .Property(e => e.TipoTel)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadTelefones>()
                .Property(e => e.NumIP)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_CadTelefones>()
                .Property(e => e.Observacoes)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .Property(e => e.NomeContratante)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .Property(e => e.AuditaEntrada)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .Property(e => e.AuditaProg)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .Property(e => e.DesMarca)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .Property(e => e.DesCateg)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .Property(e => e.CtrRef)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .Property(e => e.Pasta)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .Property(e => e.Logo)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .Property(e => e.PastaCli)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .Property(e => e.PastaImagens)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .Property(e => e.CodigoIbgeMunicipio)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .HasMany(e => e.SIGSM_MicroArea_Unidade)
                .WithRequired(e => e.ASSMED_Contratos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .HasMany(e => e.SIGSM_MicroArea_CredenciadoVinc)
                .WithRequired(e => e.ASSMED_Contratos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ASSMED_Contratos>()
                .HasMany(e => e.SIGSM_MicroArea_CredenciadoCidadao)
                .WithRequired(e => e.ASSMED_Contratos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ASSMED_Endereco>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ASSMED_Endereco>()
                .Property(e => e.CEP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Endereco>()
                .Property(e => e.TipoEnd)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Endereco>()
                .Property(e => e.Corresp)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Endereco>()
                .Property(e => e.Logradouro)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Endereco>()
                .Property(e => e.Bairro)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Endereco>()
                .Property(e => e.Complemento)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Endereco>()
                .Property(e => e.NomeCidade)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Endereco>()
                .Property(e => e.UF)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Endereco>()
                .Property(e => e.Numero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Endereco>()
                .Property(e => e.Latitude)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Endereco>()
                .Property(e => e.Longitude)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Endereco>()
                .Property(e => e.MicroArea)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.CPF)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.Sexo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.DtObto)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.EstCivil)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.NomePai)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.NomeMae)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.Deficiente)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.TpSangue)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.Doador)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.UfNacao)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.MuniNacao)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.NATURALIZACAOPORTARIA)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.OBITONUMERO)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_PesFisica>()
                .Property(e => e.OCUPACAO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_TipoDocPessoal>()
                .Property(e => e.DesTpDocP)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_TipoDocPessoal>()
                .Property(e => e.Numero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_TipoDocPessoal>()
                .Property(e => e.Serie)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_TipoDocPessoal>()
                .Property(e => e.Secao)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_TipoDocPessoal>()
                .Property(e => e.Zona)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_TipoDocPessoal>()
                .Property(e => e.DtValidade)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_TipoDocPessoal>()
                .HasMany(e => e.Documentos)
                .WithRequired(e => e.ASSMED_TipoDocPessoal)
                .HasForeignKey(e => new { e.NumContrato, e.IdTipoDocumento })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ASSMED_TipoDocPessoal>()
                .HasMany(e => e.ASSMED_CadastroDocPessoal)
                .WithRequired(e => e.ASSMED_TipoDocPessoal)
                .HasForeignKey(e => new { e.NumContrato, e.CodTpDocP })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ASSMED_Usuario>()
                .Property(e => e.Login)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Usuario>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Usuario>()
                .Property(e => e.Senha)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Usuario>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Usuario>()
                .Property(e => e.NumIP)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Usuario>()
                .Property(e => e.CEP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Usuario>()
                .Property(e => e.NomeCidade)
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Usuario>()
                .Property(e => e.UF)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ASSMED_Usuario>()
                .Property(e => e.NumLog)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Cidade>()
                .Property(e => e.NomeCidade)
                .IsUnicode(false);

            modelBuilder.Entity<Cidade>()
                .Property(e => e.UF)
                .IsUnicode(false);

            modelBuilder.Entity<Cidade>()
                .Property(e => e.CodIbge)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Cidade>()
                .Property(e => e.Latitude)
                .IsUnicode(false);

            modelBuilder.Entity<Cidade>()
                .Property(e => e.Longitude)
                .IsUnicode(false);

            modelBuilder.Entity<Cidade>()
                .Property(e => e.Area)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Cidade>()
                .Property(e => e.Altitude)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Cidade>()
                .Property(e => e.Capital)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Cidade>()
                .Property(e => e.Fonteira)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Cidade>()
                .Property(e => e.Fronteira)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Setores>()
                .Property(e => e.DesSetor)
                .IsUnicode(false);

            modelBuilder.Entity<Setores>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Setores>()
                .Property(e => e.Latitude)
                .IsUnicode(false);

            modelBuilder.Entity<Setores>()
                .Property(e => e.Longitude)
                .IsUnicode(false);

            modelBuilder.Entity<Setores>()
                .Property(e => e.DesSetorRes)
                .IsUnicode(false);

            modelBuilder.Entity<Setores>()
                .HasMany(e => e.AS_CredenciadosVinc)
                .WithOptional(e => e.Setores)
                .HasForeignKey(e => new { e.NumContrato, e.CodSetor });

            modelBuilder.Entity<Setores>()
                .HasMany(e => e.AS_SetoresPar)
                .WithRequired(e => e.Setores)
                .HasForeignKey(e => new { e.NumContrato, e.CodSetor })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Setores>()
                .HasMany(e => e.SIGSM_MicroArea_Unidade)
                .WithRequired(e => e.Setores)
                .HasForeignKey(e => new { e.NumContrato, e.CodSetor })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Setores>()
                .HasMany(e => e.SetoresINEs)
                .WithOptional(e => e.Setores)
                .HasForeignKey(e => new { e.NumContrato, e.CodSetor });

            modelBuilder.Entity<SIGSM_FichaProfissao>()
                .Property(e => e.CBO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SIGSM_MicroArea_CredenciadoCidadao>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SIGSM_MicroArea_CredenciadoVinc>()
                .HasMany(e => e.SIGSM_MicroArea_CredenciadoCidadao)
                .WithRequired(e => e.SIGSM_MicroArea_CredenciadoVinc)
                .HasForeignKey(e => e.idMaCredVinc)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SIGSM_MicroArea_Unidade>()
                .Property(e => e.MicroArea)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SIGSM_MicroArea_Unidade>()
                .HasMany(e => e.SIGSM_MicroArea_CredenciadoVinc)
                .WithRequired(e => e.SIGSM_MicroArea_Unidade)
                .HasForeignKey(e => e.idMicroAreaUnidade)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SIGSM_MicroAreas>()
                .Property(e => e.Codigo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SIGSM_MicroAreas>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<SIGSM_MicroAreas>()
                .HasMany(e => e.ASSMED_Cadastro)
                .WithOptional(e => e.SIGSM_MicroAreas)
                .HasForeignKey(e => e.MicroArea);

            modelBuilder.Entity<SIGSM_MicroAreas>()
                .HasMany(e => e.ASSMED_Endereco)
                .WithOptional(e => e.SIGSM_MicroAreas)
                .HasForeignKey(e => e.MicroArea);

            modelBuilder.Entity<SIGSM_MicroAreas>()
                .HasMany(e => e.SIGSM_MicroArea_Unidade)
                .WithRequired(e => e.SIGSM_MicroAreas)
                .HasForeignKey(e => e.MicroArea)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SIGSM_ServicoSerializador_Agenda>()
                .Property(e => e.LogMessage)
                .IsUnicode(false);

            modelBuilder.Entity<SIGSM_Transmissao>()
                .HasMany(e => e.SIGSM_Transmissao_Processos)
                .WithRequired(e => e.SIGSM_Transmissao)
                .HasForeignKey(e => e.IdTransmissao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SIGSM_Transmissao_Processos>()
                .Property(e => e.CNES)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SIGSM_Transmissao_Processos>()
                .HasMany(e => e.SIGSM_Transmissao_Processos_Log)
                .WithRequired(e => e.SIGSM_Transmissao_Processos)
                .HasForeignKey(e => e.IdProcesso)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SIGSM_Transmissao_StatusGeracao>()
                .HasMany(e => e.SIGSM_Transmissao_Processos)
                .WithRequired(e => e.SIGSM_Transmissao_StatusGeracao)
                .HasForeignKey(e => e.IdStatusGeracao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TP_Cond_Posse_Uso_Terra>()
                .HasMany(e => e.CondicaoMoradia)
                .WithOptional(e => e.TP_Cond_Posse_Uso_Terra)
                .HasForeignKey(e => e.areaProducaoRural);

            modelBuilder.Entity<TP_Condicao_Avaliada>()
                .Property(e => e.tp_sub_grupos)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TP_EstadoCivil>()
                .Property(e => e.codigo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TP_EstadoCivil>()
                .Property(e => e.descricao)
                .IsUnicode(false);

            modelBuilder.Entity<TP_Identidade_Genero_Cidadao>()
                .HasMany(e => e.InformacoesSocioDemograficas)
                .WithOptional(e => e.TP_Identidade_Genero_Cidadao)
                .HasForeignKey(e => e.identidadeGeneroCidadao);

            modelBuilder.Entity<TP_Imovel>()
                .HasMany(e => e.CadastroDomiciliar)
                .WithRequired(e => e.TP_Imovel)
                .HasForeignKey(e => e.tipoDeImovel)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TP_Motivo_Saida>()
                .HasMany(e => e.SaidaCidadaoCadastro)
                .WithOptional(e => e.TP_Motivo_Saida)
                .HasForeignKey(e => e.motivoSaidaCidadao);

            modelBuilder.Entity<TP_Orientacao_Sexual>()
                .HasMany(e => e.InformacoesSocioDemograficas)
                .WithOptional(e => e.TP_Orientacao_Sexual)
                .HasForeignKey(e => e.orientacaoSexualCidadao);

            modelBuilder.Entity<TP_Raca_Cor>()
                .HasMany(e => e.IdentificacaoUsuarioCidadao)
                .WithRequired(e => e.TP_Raca_Cor)
                .HasForeignKey(e => e.racaCorCidadao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TP_Relacao_Parentesco>()
                .HasMany(e => e.InformacoesSocioDemograficas)
                .WithOptional(e => e.TP_Relacao_Parentesco)
                .HasForeignKey(e => e.relacaoParentescoCidadao);

            modelBuilder.Entity<TP_Renda_Familiar>()
                .HasMany(e => e.FamiliaRow)
                .WithOptional(e => e.TP_Renda_Familiar)
                .HasForeignKey(e => e.rendaFamiliar);

            modelBuilder.Entity<TP_Sexo>()
                .Property(e => e.sigla)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TP_Sexo>()
                .HasMany(e => e.IdentificacaoUsuarioCidadao)
                .WithRequired(e => e.TP_Sexo)
                .HasForeignKey(e => e.sexoCidadao)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UF>()
                .Property(e => e.UF1)
                .IsUnicode(false);

            modelBuilder.Entity<UF>()
                .Property(e => e.DesUF)
                .IsUnicode(false);

            modelBuilder.Entity<UF>()
                .Property(e => e.DNE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<UF>()
                .HasMany(e => e.AS_Credenciados)
                .WithOptional(e => e.UF1)
                .HasForeignKey(e => e.UF);

            modelBuilder.Entity<TB_MS_TIPO_LOGRADOURO>()
                .Property(e => e.CO_TIPO_LOGRADOURO)
                .IsUnicode(false);

            modelBuilder.Entity<TB_MS_TIPO_LOGRADOURO>()
                .Property(e => e.DS_TIPO_LOGRADOURO)
                .IsUnicode(false);

            modelBuilder.Entity<TB_MS_TIPO_LOGRADOURO>()
                .Property(e => e.DS_TIPO_LOGRADOURO_ABREV)
                .IsUnicode(false);

            modelBuilder.Entity<TP_Conduta>()
                .Property(e => e.tp_sub_grupos)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VW_Cadastros_Zoneamento>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<VW_Cadastros_Zoneamento>()
                .Property(e => e.MicroArea)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VW_ConsultaCadastrosDomiciliares>()
                .Property(e => e.Complemento)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ConsultaCadastrosDomiciliares>()
                .Property(e => e.Responsavel)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ConsultaCadastrosDomiciliares>()
                .Property(e => e.Numero)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<VW_ConsultaCadastrosDomiciliares>()
                .Property(e => e.Endereco)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ConsultaCadastrosDomiciliares>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<VW_ConsultaCadastrosIndividuais>()
                .Property(e => e.NomeCidadao)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ConsultaCadastrosIndividuais>()
                .Property(e => e.NomeMae)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ConsultaCadastrosIndividuais>()
                .Property(e => e.CnsCidadao)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ConsultaCadastrosIndividuais>()
                .Property(e => e.MunicipioNascimento)
                .IsUnicode(false);

            modelBuilder.Entity<VW_ConsultaCadastrosIndividuais>()
                .Property(e => e.Codigo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<VW_SystemLanguage>()
                .Property(e => e.dateformat)
                .IsFixedLength();
        }
    }
}
