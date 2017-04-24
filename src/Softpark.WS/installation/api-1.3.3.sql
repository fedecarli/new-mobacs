ALTER TABLE [dbo].[Domicilio_Individual]
		ADD [st_fora_area] BIT NULL
GO

ALTER VIEW [api].[VW_Profissional] AS
		 SELECT ROW_NUMBER() OVER (ORDER BY cad.Nome COLLATE Latin1_General_CI_AI) as id,
				doc.Numero COLLATE Latin1_General_CI_AI as [CNS],
				cad.Nome COLLATE Latin1_General_CI_AI as [Nome],
 				cbo.CodProfTab COLLATE Latin1_General_CI_AI as [CBO],
				cbo.DesProfTab COLLATE Latin1_General_CI_AI as [Profissao],
				vinc.CNESLocal COLLATE Latin1_General_CI_AI AS [CNES],
				setor.DesSetor COLLATE Latin1_General_CI_AI AS [Unidade],
				ine.Numero COLLATE Latin1_General_CI_AI	   AS [INE],
				ine.Descricao COLLATE Latin1_General_CI_AI  AS [Equipe]
		   FROM [dbo].[ASSMED_Cadastro]					AS cad
	 INNER JOIN [dbo].[ASSMED_CadastroDocPessoal]		AS doc
			 ON cad.Codigo = doc.Codigo
	 INNER JOIN [dbo].[AS_CREDENCIADOS]					AS cred
			 ON cad.Codigo = cred.Codigo
	 INNER JOIN [dbo].[AS_CredenciadosVinc]				AS vinc
			 ON cred.CodCred = vinc.CodCred
	 INNER JOIN [dbo].[AS_ProfissoesTab]				AS cbo
			 ON vinc.CodProfTab COLLATE Latin1_General_CI_AI = cbo.CodProfTab  COLLATE Latin1_General_CI_AI
	 INNER JOIN [dbo].[AS_SetoresPar]					AS par
			 ON vinc.CNESLocal COLLATE Latin1_General_CI_AI = par.CNES COLLATE Latin1_General_CI_AI
	 INNER JOIN [dbo].[Setores]							AS setor
		  	 ON par.CodSetor = setor.CodSetor
			AND par.Codigo = setor.Codigo
	  LEFT JOIN [dbo].[SetoresINEs]						AS ine
			 ON setor.CodSetor = ine.CodSetor
	      WHERE doc.CodTpDocP = 6
		    AND cbo.CodProfissao IS NOT NULL
			AND LEN(LTRIM(RTRIM(COALESCE(doc.Numero COLLATE Latin1_General_CI_AI, '')))) > 0
			AND LEN(LTRIM(RTRIM(COALESCE(vinc.CNESLocal COLLATE Latin1_General_CI_AI, '')))) > 0
	   GROUP BY cad.Nome COLLATE Latin1_General_CI_AI,
				doc.Numero COLLATE Latin1_General_CI_AI,
				cbo.CodProfTab COLLATE Latin1_General_CI_AI,
				cbo.DesProfTab COLLATE Latin1_General_CI_AI,
				vinc.CNESLocal COLLATE Latin1_General_CI_AI,
				setor.DesSetor COLLATE Latin1_General_CI_AI,
				ine.Numero COLLATE Latin1_General_CI_AI,
				ine.Descricao COLLATE Latin1_General_CI_AI;

GO
