﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Softpark.Models
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

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
        public DomainContainer()
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
                INNER JOIN dbo.SIGSM_FichaProfissao AS b ON LTRIM(RTRIM(a.CBO)) = LTRIM(RTRIM(b.CBO))
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
                INNER JOIN dbo.SIGSM_FichaProfissao AS b ON LTRIM(RTRIM(a.CBO)) = LTRIM(RTRIM(b.CBO))
                LEFT JOIN dbo.SetoresINEs AS c ON a.INE COLLATE Latin1_General_CI_AI = LTRIM(RTRIM(c.Numero COLLATE Latin1_General_CI_AI))
                WHERE a.CNS IS NOT NULL AND b.Ficha = @ficha" +
                (string.IsNullOrEmpty(ine?.Trim()) ? "" : " AND a.INE = @ine") +
                (string.IsNullOrEmpty(cnes?.Trim()) ? "" : " AND a.CNES = @cnes") +
                (string.IsNullOrEmpty(nomeOuCns?.Trim()) ? "" : " AND (a.CNS = @cns OR a.Nome LIKE @nome)") +
                " ORDER BY Nome, Profissao, Unidade, Equipe";

            return Database.SqlQuery<VW_Profissional>(query, parameters: parameters.ToArray()).FirstOrDefault();
        }

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

        public static string ContainerName => "DomainContainer";
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AnimalNoDomicilio> AnimalNoDomicilio { get; set; }
        public virtual DbSet<CadastroDomiciliar> CadastroDomiciliar { get; set; }
        public virtual DbSet<CadastroIndividual> CadastroIndividual { get; set; }
        public virtual DbSet<CadastroIndividual_recusa> CadastroIndividual_recusa { get; set; }
        public virtual DbSet<CondicaoMoradia> CondicaoMoradia { get; set; }
        public virtual DbSet<CondicoesDeSaude> CondicoesDeSaude { get; set; }
        public virtual DbSet<DeficienciasCidadao> DeficienciasCidadao { get; set; }
        public virtual DbSet<DoencaCardiaca> DoencaCardiaca { get; set; }
        public virtual DbSet<DoencaRespiratoria> DoencaRespiratoria { get; set; }
        public virtual DbSet<DoencaRins> DoencaRins { get; set; }
        public virtual DbSet<EmSituacaoDeRua> EmSituacaoDeRua { get; set; }
        public virtual DbSet<EnderecoLocalPermanencia> EnderecoLocalPermanencia { get; set; }
        public virtual DbSet<FamiliaRow> FamiliaRow { get; set; }
        public virtual DbSet<FichaVisitaDomiciliarChild> FichaVisitaDomiciliarChild { get; set; }
        public virtual DbSet<FichaVisitaDomiciliarMaster> FichaVisitaDomiciliarMaster { get; set; }
        public virtual DbSet<HigienePessoalSituacaoRua> HigienePessoalSituacaoRua { get; set; }
        public virtual DbSet<IdentificacaoUsuarioCidadao> IdentificacaoUsuarioCidadao { get; set; }
        public virtual DbSet<InformacoesSocioDemograficas> InformacoesSocioDemograficas { get; set; }
        public virtual DbSet<InstituicaoPermanencia> InstituicaoPermanencia { get; set; }
        public virtual DbSet<Lote> Lote { get; set; }
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
        public virtual DbSet<ASSMED_Usuario> ASSMED_Usuario { get; set; }
        public virtual DbSet<Cidade> Cidade { get; set; }
        public virtual DbSet<Etnia> Etnia { get; set; }
        public virtual DbSet<ProfCidadaoVinc> ProfCidadaoVinc { get; set; }
        public virtual DbSet<ProfCidadaoVincAgendaProd> ProfCidadaoVincAgendaProd { get; set; }
        public virtual DbSet<Setores> Setores { get; set; }
        public virtual DbSet<SetoresINEs> SetoresINEs { get; set; }
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
        public virtual DbSet<ASSMED_CadastroDomiciliarVinc> ASSMED_CadastroDomiciliarVinc { get; set; }
        public virtual DbSet<ASSMED_CadastroIndividualVinc> ASSMED_CadastroIndividualVinc { get; set; }
        public virtual DbSet<Conflito> Conflito { get; set; }
        public virtual DbSet<Nacionalidade> Nacionalidade { get; set; }
        public virtual DbSet<TB_MS_TIPO_LOGRADOURO> TB_MS_TIPO_LOGRADOURO { get; set; }
        public virtual DbSet<TP_Conduta> TP_Conduta { get; set; }
        public virtual DbSet<TP_Nasf> TP_Nasf { get; set; }
        public virtual DbSet<VW_IdentificacaoUsuarioCidadao> VW_IdentificacaoUsuarioCidadao { get; set; }
        public virtual DbSet<VW_ultimo_cadastroDomiciliar> VW_ultimo_cadastroDomiciliar { get; set; }
        public virtual DbSet<VW_ultimo_cadastroIndividual> VW_ultimo_cadastroIndividual { get; set; }
        public virtual DbSet<VW_ConsultaCadastrosDomiciliares> VW_ConsultaCadastrosDomiciliares { get; set; }
        public virtual DbSet<VW_ConsultaCadastrosIndividuais> VW_ConsultaCadastrosIndividuais { get; set; }
        public virtual DbSet<VW_profissional_cns> VW_profissional_cns { get; set; }
        public virtual DbSet<Documentos> Documentos { get; set; }
        public virtual DbSet<ASSMED_TipoDocPessoal> ASSMED_TipoDocPessoal { get; set; }
    
        public virtual int PR_ProcessarFichasAPI(Nullable<System.Guid> token)
        {
            var tokenParameter = token.HasValue ?
                new ObjectParameter("token", token) :
                new ObjectParameter("token", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_ProcessarFichasAPI", tokenParameter);
        }
    
        public virtual int PR_EncerrarAgenda(Nullable<int> idAgendaProd, Nullable<bool> retorno, Nullable<bool> tipoFicha)
        {
            var idAgendaProdParameter = idAgendaProd.HasValue ?
                new ObjectParameter("idAgendaProd", idAgendaProd) :
                new ObjectParameter("idAgendaProd", typeof(int));
    
            var retornoParameter = retorno.HasValue ?
                new ObjectParameter("Retorno", retorno) :
                new ObjectParameter("Retorno", typeof(bool));
    
            var tipoFichaParameter = tipoFicha.HasValue ?
                new ObjectParameter("TipoFicha", tipoFicha) :
                new ObjectParameter("TipoFicha", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PR_EncerrarAgenda", idAgendaProdParameter, retornoParameter, tipoFichaParameter);
        }
    
        public virtual ObjectResult<PR_ConsultaCadastroIndividuais_Result3> PR_ConsultaCadastroIndividuais(string search, Nullable<int> skip, Nullable<int> take, Nullable<int> orderCol, Nullable<int> orderDirection, ObjectParameter total, ObjectParameter totalFiltered)
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
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<PR_ConsultaCadastroIndividuais_Result3>("PR_ConsultaCadastroIndividuais", searchParameter, skipParameter, takeParameter, orderColParameter, orderDirectionParameter, total, totalFiltered);
        }
    }
}
