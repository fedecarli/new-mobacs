SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE api.identificacaoUsuarioCidadao ADD ComplementoRG VARCHAR(4) NULL
GO
ALTER TABLE ASSMED_CadastroDocPessoal ADD ComplementoRG VARCHAR(4) NULL
GO

ALTER VIEW [api].[VW_IdentificacaoUsuarioCidadao] AS
	 SELECT _iuc.id AS id,
			 LTRIM(RTRIM(cnsProf.Numero)) AS cnsProfissional
			, LTRIM(RTRIM(credV.CodProfTab)) AS CBO
			, LTRIM(RTRIM(credV.CodSetor)) AS CNES
			, LTRIM(RTRIM(me.INE)) AS INE
			,LTRIM(RTRIM(cad.NomeSocial)) COLLATE Latin1_General_CI_AI AS nomeSocial
			,COALESCE(LTRIM(RTRIM(cid.CodIbge)), '3547304') COLLATE Latin1_General_CI_AI AS codigoIbgeMunicipioNascimento
			,CONVERT(BIT, (CASE WHEN _cpf.NomeMae IS NULL OR LEN(_cpf.NomeMae) = 0 OR _cpf.MaeDesconhecida > 0 THEN 1 ELSE 0 END)) AS desconheceNomeMae
			,LTRIM(RTRIM(_email.EMail)) COLLATE Latin1_General_CI_AI AS emailCidadao
			,COALESCE(_cpf.Nacionalidade, 1) AS nacionalidadeCidadao
			,LTRIM(RTRIM(cad.Nome)) COLLATE Latin1_General_CI_AI AS nomeCidadao
			,(CASE WHEN _cpf.NomeMae IS NULL OR LEN(_cpf.NomeMae) = 0 THEN NULL ELSE _cpf.NomeMae END)
			 COLLATE Latin1_General_CI_AI AS nomeMaeCidadao
			,LTRIM(RTRIM(cns.Numero)) COLLATE Latin1_General_CI_AI AS cnsCidadao
			,LTRIM(RTRIM(_iuc.cnsResponsavelFamiliar)) COLLATE Latin1_General_CI_AI AS cnsResponsavelFamiliar
			,(LTRIM(RTRIM(tel.DDD)) + LTRIM(RTRIM(tel.NumTel))) COLLATE Latin1_General_CI_AI AS telefoneCelular
			,LTRIM(RTRIM(pis.Numero)) COLLATE Latin1_General_CI_AI AS numeroNisPisPasep
			,(CASE WHEN cid.CodIbge IS NOT NULL THEN 31 ELSE NULL END) AS paisNascimento
			,ISNULL(CASE WHEN ISNULL(_cpf.CodCor,0) = 0 THEN 6 ELSE _cpf.CodCor END, 6) AS racaCorCidadao
			,(CASE _cpf.Sexo WHEN 'M' THEN 0 WHEN 'F' THEN 1 ELSE 4 END) AS sexoCidadao
			,CONVERT(BIT, CASE WHEN _iuc.statusEhResponsavel IS NOT NULL THEN _iuc.statusEhResponsavel WHEN LEN(ISNULL(cns.Numero, '')) > 0 THEN 1 ELSE 0 END) AS statusEhResponsavel
			,_iuc.etnia AS etnia
			,(CASE WHEN _cpf.NomePai IS NULL OR LEN(_cpf.NomePai) = 0 THEN NULL ELSE _cpf.NomePai END) COLLATE Latin1_General_CI_AI AS nomePaiCidadao
			,CONVERT(BIT, (CASE WHEN _cpf.NomePai IS NULL OR LEN(_cpf.NomePai) = 0 OR _cpf.PaiDesconhecido > 0 THEN 1 ELSE 0 END)) AS desconheceNomePai
			,_iuc.portariaNaturalizacao AS portariaNaturalizacao
			,RIGHT('00'+CONVERT(VARCHAR,COALESCE(_iuc.microarea, me.microarea)), 2) AS microarea
			,CONVERT(BIT, COALESCE(_iuc.stForaArea, 0)) AS stForaArea
			,CONVERT(DATE, _cpf.DtNasc) AS dataNascimentoCidadao
			,_iuc.dtNaturalizacao AS dtNaturalizacao
			,_iuc.dtEntradaBrasil AS dtEntradaBrasil
			, 22 AS num_contrato
			, cad.Codigo
			, cad.Justificativa
			, nrg.Numero COLLATE Latin1_General_CI_AI AS [RG]
			, nrg.ComplementoRG COLLATE Latin1_General_CI_AI AS [ComplementoRG]
			, _cpf.CPF COLLATE Latin1_General_CI_AI AS [CPF]
			, COALESCE((CASE WHEN _cpf.EstCivil = '' THEN 'I' ELSE _cpf.EstCivil END), 'I') COLLATE Latin1_General_CI_AI AS [EstadoCivil]
			, CAST(COALESCE(_iuc.beneficiarioBolsaFamilia, 0) AS BIT) AS [beneficiarioBolsaFamilia]
	   FROM [dbo].[ASSMED_Cadastro] AS cad WITH (NOLOCK)

  LEFT JOIN [dbo].[ASSMED_CadTelefones] AS tel WITH (NOLOCK)
		 ON cad.Codigo = tel.Codigo
		AND tel.TipoTel = 'C'
		AND LEN(tel.NumTel) BETWEEN 8 AND 9

  LEFT JOIN [dbo].[ASSMED_CadastroDocPessoal] AS pis WITH (NOLOCK)
		 ON cad.Codigo = pis.Codigo
		AND pis.CodTpDocP = 7
		AND pis.Numero IS NOT NULL

  LEFT JOIN [dbo].[ASSMED_CadastroDocPessoal] AS cns WITH (NOLOCK)
		 ON cad.Codigo = cns.Codigo
		AND cns.CodTpDocP = 6
		AND cns.Numero is not null

  LEFT JOIN [dbo].[ASSMED_CadastroDocPessoal] AS nrg WITH (NOLOCK)
		 ON cad.Codigo = nrg.Codigo
		AND nrg.CodTpDocP = 1
		AND nrg.Numero is not null

	LEFT JOIN dbo.ASSMED_PesFisica AS _cpf WITH (NOLOCK) ON _cpf.Codigo = cad.Codigo

	LEFT JOIN [dbo].ASSMED_CadEmails AS _email WITH (NOLOCK) ON _email.EMail IS NOT NULL AND cad.Codigo = _email.Codigo

  LEFT JOIN [dbo].[Cidade] AS cid WITH (NOLOCK)
		 ON cid.NomeCidade COLLATE Latin1_General_CI_AI = _cpf.MuniNacao COLLATE Latin1_General_CI_AI
		AND cid.UF COLLATE Latin1_General_CI_AI = _cpf.UfNacao COLLATE Latin1_General_CI_AI

	INNER JOIN dbo.ProfCidadaoVinc pcv WITH (NOLOCK) on cad.CodMunicipe = pcv.IdCidadao AND pcv.Marcado = 1
	INNER JOIN Unico.dbo.PtUsuario ptu WITH (NOLOCK) on ptu.[Login] = convert(varchar,pcv.IdProfissional)
	INNER JOIN Unico.dbo.Pessoas pes WITH (NOLOCK) on pes.CPF = ptu.CPF
	INNER JOIN dbo.ASSMED_Cadastro acs WITH (NOLOCK) on pes.CodMunicipe = acs.CodMunicipe
	INNER JOIN [dbo].[AS_Credenciados] cred WITH (NOLOCK) on acs.Codigo = cred.Codigo
	INNER JOIN [dbo].[AS_CredenciadosVinc] credV WITH (NOLOCK) on cred.CodCred = credV.CodCred
	INNER JOIN dbo.Microarea_equipe me WITH (NOLOCK) on me.Matricula = credV.Matricula AND credV.CodSetor = me.CNES
	LEFT JOIN [dbo].[ASSMED_CadastroDocPessoal] AS cnsProf WITH (NOLOCK) ON acs.Codigo = cnsProf.Codigo AND cnsProf.CodTpDocP = 6 AND cnsProf.Numero IS NOT NULL 

	LEFT JOIN (
		SELECT lastInd.Codigo, iuc.* 
		FROM api.VW_ultimo_cadastroIndividual lastInd WITH (NOLOCK)
		INNER JOIN [api].[CadastroIndividual] AS ci WITH (NOLOCK) ON lastInd.idCadastroIndividual = ci.id
		INNER JOIN [api].[IdentificacaoUsuarioCidadao] AS iuc WITH (NOLOCK) ON iuc.id = ci.identificacaoUsuarioCidadao
	) AS _iuc ON cad.Codigo = _iuc.Codigo

	  WHERE cad.Tipo IN ('F', 'C')

GO
