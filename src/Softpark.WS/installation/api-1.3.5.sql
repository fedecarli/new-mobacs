ALTER TABLE [dbo].[UF]
		ADD DNE CHAR(2) NULL;
GO

 UPDATE [dbo].[UF]
	SET UF.DNE = REPLACE(STR(_uf.Dne, 2), SPACE(1), '0')
   FROM (SELECT ROW_NUMBER() OVER (
		ORDER BY (CASE WHEN __uf.UF BETWEEN 'AC' AND 'ES' THEN 1
					   WHEN __uf.UF = 'RR' THEN 2
					   WHEN __uf.UF BETWEEN 'GO' AND 'RJ' THEN 3
					   WHEN __uf.UF = 'RN' THEN 4
					   WHEN __uf.UF = 'RS' THEN 5
					   WHEN __uf.UF = 'RO' THEN 6
					   WHEN __uf.UF = 'TO' THEN 7
					   WHEN __uf.UF BETWEEN 'SC' AND 'SP' THEN 8
					   ELSE 9 END) ASC, DesUF ASC
		) AS Dne, __uf.UF FROM [dbo].[UF] AS __uf) AS _uf
  WHERE _uf.UF = UF.UF;
GO

ALTER VIEW [api].[VW_IdentificacaoUsuarioCidadao] AS
	 SELECT _iuc.id AS id
			,LTRIM(RTRIM(cad.NomeSocial)) COLLATE Latin1_General_CI_AI AS nomeSocial
			,COALESCE(LTRIM(RTRIM(cid.CodIbge)), '3547304') COLLATE Latin1_General_CI_AI AS codigoIgbeMunicipioNascimento
			,(CASE WHEN NomeMae IS NULL OR LEN(NomeMae) = 0 OR MaeDesconhecida > 0 THEN 1 ELSE 0 END) AS
				desconheceNomeMae
			,LTRIM(RTRIM(_email.EMail)) COLLATE Latin1_General_CI_AI AS emailCidadao
			,COALESCE(_cpf.Nacionalidade, 10) AS nacionalidadeCidadao
			,LTRIM(RTRIM(cad.Nome)) COLLATE Latin1_General_CI_AI AS nomeCidadao
			,(CASE WHEN NomeMae IS NULL OR LEN(NomeMae) = 0 THEN NULL ELSE NomeMae END)
			 COLLATE Latin1_General_CI_AI AS nomeMaeCidadao
			,LTRIM(RTRIM(cns.Numero)) COLLATE Latin1_General_CI_AI AS cnsCidadao
			,LTRIM(RTRIM(NULL)) COLLATE Latin1_General_CI_AI AS cnsResponsavelFamiliar
			,(LTRIM(RTRIM(tel.DDD)) + LTRIM(RTRIM(tel.NumTel)))
				COLLATE Latin1_General_CI_AI AS telefoneCelular
			,LTRIM(RTRIM(pis.Numero)) COLLATE Latin1_General_CI_AI AS numeroNisPisPasep
			,(CASE WHEN cid.CodIbge IS NOT NULL THEN 31 ELSE NULL END) AS paisNascimento
			,_iuc.racaCorCidadao AS racaCorCidadao
			,(CASE _cpf.Sexo WHEN 'M' THEN 0 WHEN 'F' THEN 1 ELSE 4 END) AS sexoCidadao
			,_iuc.statusEhResponsavel AS statusEhResponsavel
			,_iuc.etnia AS etnia
			,(CASE WHEN NomeMae IS NULL OR LEN(NomeMae) = 0 THEN NULL ELSE NomeMae END)
			 COLLATE Latin1_General_CI_AI AS nomePaiCidadao
			,(CASE WHEN NomePai IS NULL OR LEN(NomePai) = 0 OR PaiDesconhecido > 0 THEN 1 ELSE 0 END) AS
				desconheceNomePai
			,_iuc.portariaNaturalizacao AS portariaNaturalizacao
			,_iuc.microarea AS microarea
			,_iuc.stForaArea AS stForaArea
			,CONVERT(DATE, _cpf.DtNasc) AS dataNascimentoCidadao
			,_iuc.dtNaturalizacao AS dtNaturalizacao
			,_iuc.dtEntradaBrasil AS dtEntradaBrasil
	   FROM [dbo].[ASSMED_Cadastro] AS cad
  
  LEFT JOIN [dbo].[ASSMED_CadTelefones] AS tel
		 ON cad.Codigo = tel.Codigo
		AND tel.TipoTel = 'C'
		AND LEN(tel.NumTel) BETWEEN 8 AND 9

  LEFT JOIN [dbo].[ASSMED_CadastroDocPessoal] AS pis
		 ON cad.Codigo = pis.Codigo
		AND pis.CodTpDocP = 7
		AND pis.Numero IS NOT NULL

  LEFT JOIN [dbo].[ASSMED_CadastroDocPessoal] AS cns
		 ON cad.Codigo = cns.Codigo
		AND cns.CodTpDocP = 6
		AND cns.Numero is not null

OUTER APPLY (
	SELECT DISTINCT a.* FROM (
		SELECT DtNasc, LTRIM(RTRIM(NomePai)) AS NomePai, LTRIM(RTRIM(NomeMae)) AS NomeMae,
			MaeDesconhecida, PaiDesconhecido, UfNacao, MuniNacao, Nacionalidade,
			Sexo
		  FROM ASSMED_CadastroPF AS cpf
		 WHERE cpf.Codigo = cad.Codigo
		UNION
		SELECT DtNasc, LTRIM(RTRIM(NomePai)) AS NomePai, LTRIM(RTRIM(NomeMae)) AS NomeMae,
			MaeDesconhecida, PaiDesconhecido, UfNacao, MuniNacao, Nacionalidade,
			Sexo
		  FROM ASSMED_PesFisica  AS apf
		 WHERE apf.Codigo = cad.Codigo
	 ) AS a WHERE a.DtNasc IS NOT NULL
) AS _cpf

OUTER APPLY (
	SELECT TOP 1 c.* FROM [dbo].ASSMED_CadEmails AS c WHERE c.EMail IS NOT NULL
	AND c.Codigo = cad.Codigo
) AS _email

  LEFT JOIN [dbo].[Cidade] AS cid
		 ON cid.NomeCidade COLLATE Latin1_General_CI_AI = _cpf.MuniNacao COLLATE Latin1_General_CI_AI
		AND cid.UF COLLATE Latin1_General_CI_AI = _cpf.UfNacao COLLATE Latin1_General_CI_AI
  
OUTER APPLY (
			 SELECT TOP 1 iuc.*
			   FROM [api].[IdentificacaoUsuarioCidadao] AS iuc
		 INNER JOIN [api].[CadastroIndividual] AS ci
				 ON iuc.id = ci.identificacaoUsuarioCidadao
		 INNER JOIN [api].[UnicaLotacaoTransport] AS ut
				 ON ci.headerTransport = ut.id
		 INNER JOIN [api].[OrigemVisita] AS o
				 ON ut.token = o.token
			  WHERE o.finalizado = 1
				AND iuc.cnsCidadao COLLATE Latin1_General_CI_AI = cns.Numero COLLATE Latin1_General_CI_AI
				 OR iuc.numeroNisPisPasep COLLATE Latin1_General_CI_AI = pis.Numero COLLATE Latin1_General_CI_AI
				 OR (iuc.nomeCidadao COLLATE Latin1_General_CI_AI = cad.Nome COLLATE Latin1_General_CI_AI
				AND iuc.nomeMaeCidadao COLLATE Latin1_General_CI_AI = _cpf.NomeMae COLLATE Latin1_General_CI_AI
				AND iuc.dataNascimentoCidadao = CONVERT(DATE, _cpf.DtNasc))
		   ORDER BY ut.dataAtendimento DESC
) AS _iuc
	  WHERE cad.Tipo IN ('F', 'C');
GO

-- View para listar os domicílios com base nos endereços de cadastros individuais
CREATE VIEW [api].[VW_Enderecos] AS
	 SELECT STUFF(STUFF([CEP], PATINDEX('%[^0-9]%', [CEP]), 1, ''), PATINDEX('%[^0-9]%', STUFF([CEP], PATINDEX('%[^0-9]%', [CEP]), 1, '')), 1, '') COLLATE Latin1_General_CI_AI AS CEP
			,COALESCE([Logradouro], '') COLLATE Latin1_General_CI_AI AS Logradouro
			,(CASE WHEN PATINDEX('%[-]%', [Bairro]) > 0 THEN SUBSTRING(LTRIM(RTRIM(COALESCE([Bairro], ''))), 1, PATINDEX('%[-]%', [Bairro]) - 1) ELSE Bairro END) COLLATE Latin1_General_CI_AI AS Bairro
			,COALESCE([Complemento], '') COLLATE Latin1_General_CI_AI AS Complemento
			,COALESCE([Numero], '') COLLATE Latin1_General_CI_AI AS Numero
			,[CodTpLogra]
			,[CodCidade]
			,[Codigo]
	   FROM [dbo].[ASSMED_Endereco]
	  WHERE TipoEnd = 'R' AND LEN(LTRIM(RTRIM(COALESCE(Logradouro, '')))) > 0
		AND CodCidade IS NOT NULL
		AND CodCidade > 0
		AND CEP IS NOT NULL
		AND LEN(STUFF(STUFF([CEP], PATINDEX('%[^0-9]%', [CEP]), 1, ''), PATINDEX('%[^0-9]%', STUFF([CEP], PATINDEX('%[^0-9]%', [CEP]), 1, '')), 1, '')) = 8
		AND STUFF(STUFF([CEP], PATINDEX('%[^0-9]%', [CEP]), 1, ''), PATINDEX('%[^0-9]%', STUFF([CEP], PATINDEX('%[^0-9]%', [CEP]), 1, '')), 1, '') <> '00000000'
		AND PATINDEX('%[^0-9]%', STUFF(STUFF([CEP], PATINDEX('%[^0-9]%', [CEP]), 1, ''), PATINDEX('%[^0-9]%', STUFF([CEP], PATINDEX('%[^0-9]%', [CEP]), 1, '')), 1, '')) < 1
		AND LEN(COALESCE(RTRIM(LTRIM([Logradouro])), '')) > 0
		AND LEN((CASE WHEN PATINDEX('%[-]%', [Bairro]) > 0 THEN SUBSTRING(LTRIM(RTRIM(COALESCE([Bairro], ''))), 1, PATINDEX('%[-]%', [Bairro]) - 1) ELSE Bairro END)) > 0;
GO

CREATE VIEW [api].[VW_enderecoLocalPermanencia] AS
	 SELECT bairro,
			cep,
			codigoIbgeMunicipio,
			complemento,
			nomeLogradouro,
			numero,
			numeroDneUf,
			telefoneContato,
			telelefoneResidencia,
			tipoLogradouroNumeroDne,
			stSemNumero,
			pontoReferencia,
			microarea,
			stForaArea
	   FROM [api].[EnderecoLocalPermanencia]
CROSS APPLY (
			SELECT e.CEP, e.Logradouro, e.Bairro, e.Complemento, e.Numero, logr.CO_TIPO_LOGRADOURO AS tipoLogradouroNumeroDne,
			c.CodIbge, u.DNE FROM [api].[VW_Enderecos] AS e
			INNER JOIN [dbo].[Cidade] AS c ON e.CodCidade = c.CodCidade
			INNER JOIN [dbo].[UF] AS u ON c.UF = u.UF
			INNER JOIN [dbo].[TB_MS_TIPO_LOGRADOURO] AS logr
			ON logr.CO_TIPO_LOGRADOURO COLLATE Latin1_General_CI_AI =
			CONVERT(NVARCHAR(5), e.CodTpLogra) COLLATE Latin1_General_CI_AI
			GROUP BY e.CEP, e.Logradouro, e.Bairro, e.Complemento, e.Numero, logr.CO_TIPO_LOGRADOURO,
			c.CodIbge, u.DNE) AS _log;
GO

-- =============================================
-- Author:		Elton Schivei Costa
-- Create date: 2017/04/04
-- Update date: 2017/04/24
-- Description:	Procedure para geração de cabeçalho de ficha e token
-- =============================================
CREATE PROCEDURE [api].[PR_INT_EnviarCabecalho] (
	@profissionalCNS NCHAR(15),
	@cboCodigo_2002 NCHAR(6),
	@cnes NCHAR(7),
	@dataAtendimento DATE,
	@codigoIbgeMunicipio NCHAR(7),
	@ine NCHAR(11) = NULL,
	@tipoOrigem INT = 3,
	@enviarParaThrift BIT = 1
) AS
BEGIN
BEGIN TRANSACTION T_PR_INT_EnviarCabecalho
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @token UNIQUEIDENTIFIER;
	SET @token = NEWID();

	INSERT INTO [api].[OrigemVisita] ([token], [finalizado], [id_tipo_origem], [enviarParaThrift]) VALUES
		(@token, 0, @tipoOrigem, @enviarParaThrift);

	INSERT INTO [api].[UnicaLotacaoTransport] ([id], [profissionalCNS], [cboCodigo_2002]
			, [cnes], [ine], [dataAtendimento], [codigoIbgeMunicipio], [token]) VALUES
			(NEWID(), @profissionalCNS, @cboCodigo_2002, @cnes, @ine, @dataAtendimento,
			@codigoIbgeMunicipio, @token);

	SELECT @token as token;

	COMMIT TRANSACTION T_PR_INT_EnviarCabecalho;
END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	
	ROLLBACK TRANSACTION T_PR_INT_EnviarCabecalho;

	SELECT 
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

	RAISERROR (
		@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
END CATCH
END
GO

-- =============================================
-- Author:		Elton Schivei Costa
-- Create date: 2017/04/04
-- Update date: 2017/04/24
-- Description:	Procedure para integrar cadastros individuais com o sigsm
-- Argumentos: @Codigo (codigo da pessoa/paciente, no ASSMED_Cadastro
--			   @profissionalCNS (CNS do profissional para associar dependência/responsabilidade pelo cadastro)
--		       @cboCodigo_2002 (Código do CBO do profissional)
--		       @cnes (Código do CNES da Unidade onde o profissional está alocado)
--		       @ine (Código do INE da Equipe onde o profissional está alocado - opcional)
-- =============================================
ALTER PROCEDURE [api].[PR_INT_CadastroIndividual_AssmedCadastro] (
	 @Codigo DECIMAL(18, 0)
	,@profissionalCNS NCHAR(15)
	,@cboCodigo_2002 NCHAR(6)
	,@cnes NCHAR(7)
	,@ine NCHAR(11) = NULL
) AS
BEGIN
	SET NOCOUNT ON;
BEGIN TRANSACTION T_PR_INT_CadastroIndividual_AssmedCadastro
BEGIN TRY
	IF @Codigo IS NULL
		RAISERROR ('Cadastro do Paciente não encontrado no SIGSM.', 1, 1);

	DECLARE @token UNIQUEIDENTIFIER,
			@headerTransport UNIQUEIDENTIFIER,
			@idCadastroIndividual UNIQUEIDENTIFIER,
			@idVwIden UNIQUEIDENTIFIER,
			@idIdentificacaoUsuarioCidadao UNIQUEIDENTIFIER,
			@num_contrato INT,
			@dataAtendimento DATE
			,@codigoIbgeMunicipio NCHAR(7) = '3547304'
			-- IdentificacaoUsuarioCidadao
			,@nomeSocial NVARCHAR(140) = NULL
			,@codigoIbgeMunicipioNascimento NCHAR(7) = '3547304'
			,@desconheceNomeMae BIT = 0
			,@emailCidadao NVARCHAR(200) = NULL
			,@nacionalidadeCidadao INT = 1
			,@nomeCidadao NVARCHAR(140) = NULL
			,@nomeMaeCidadao NVARCHAR(140) = NULL
			,@cnsCidadao NCHAR(30) = NULL
			,@cnsResponsavelFamiliar NCHAR(30) = NULL
			,@telefoneCelular NVARCHAR(22) = NULL
			,@numeroNisPisPasep NCHAR(22) = NULL
			,@paisNascimento INT = 31
			,@racaCorCidadao INT = 6
			,@sexoCidadao INT = 4
			,@statusEhResponsavel BIT = 0
			,@etnia INT = NULL
			,@nomePaiCidadao NVARCHAR(140) = NULL
			,@desconheceNomePai BIT = 0
			,@portariaNaturalizacao NVARCHAR(32) = NULL
			,@microarea NCHAR(2) = NULL
			,@stForaArea BIT = 0
			,@dataNascimentoCidadao DATE = NULL
			,@dtNaturalizacao DATE = NULL
			,@dtEntradaBrasil DATE = NULL
			,@condicoesDeSaude UNIQUEIDENTIFIER = NULL
			,@emSituacaoDeRua UNIQUEIDENTIFIER = NULL
			,@informacoesSocioDemograficas UNIQUEIDENTIFIER = NULL;

	SELECT @dataAtendimento = CONVERT(DATE, GETDATE());

	DECLARE @tokenResult TABLE ([token] UNIQUEIDENTIFIER);

	INSERT INTO @tokenResult EXEC [api].[PR_INT_EnviarCabecalho] @profissionalCNS, @cboCodigo_2002, @cnes, @dataAtendimento, @codigoIbgeMunicipio, @ine, 3, 0;
	SELECT TOP 1 @token = [token] FROM @tokenResult;
	DELETE FROM @tokenResult;
			
	SET @idIdentificacaoUsuarioCidadao = NEWID();
	SET @idCadastroIndividual = NEWID();

	IF (SELECT COUNT(*) FROM [api].[OrigemVisita] WHERE token = @token AND finalizado = 0) = 0
	BEGIN
		RAISERROR ('Token inválido.', 1, 1);
	END

	SELECT @headerTransport = [id] FROM [api].[UnicaLotacaoTransport] WHERE [token] = @token;

	IF (@headerTransport IS NULL)
	BEGIN
		RAISERROR('O token informado não possuí um cabeçalho de transporte.', 1, 1);
	END
	
	 SELECT @nomeSocial = COALESCE(vw.[nomeSocial], @nomeSocial),
			@codigoIbgeMunicipioNascimento = COALESCE(vw.[codigoIbgeMunicipioNascimento], @codigoIbgeMunicipioNascimento),
			@dataNascimentoCidadao = COALESCE(vw.[dataNascimentoCidadao], @dataNascimentoCidadao),
			@desconheceNomeMae = COALESCE(vw.[desconheceNomeMae], @desconheceNomeMae),
			@emailCidadao = COALESCE(vw.[emailCidadao], @emailCidadao),
			@nacionalidadeCidadao = COALESCE(vw.[nacionalidadeCidadao], @nacionalidadeCidadao),
			@nomeCidadao = COALESCE(vw.[nomeCidadao], @nomeCidadao),
			@nomeMaeCidadao = COALESCE(vw.[nomeMaeCidadao], @nomeMaeCidadao),
			@cnsResponsavelFamiliar = COALESCE(vw.[cnsResponsavelFamiliar], @cnsResponsavelFamiliar),
			@telefoneCelular = COALESCE(vw.[telefoneCelular], @telefoneCelular),
			@numeroNisPisPasep = COALESCE(vw.[numeroNisPisPasep], @numeroNisPisPasep),
			@paisNascimento = COALESCE(vw.[paisNascimento], @paisNascimento),
			@racaCorCidadao = COALESCE(vw.[racaCorCidadao], @racaCorCidadao),
			@sexoCidadao = COALESCE(vw.[sexoCidadao], @sexoCidadao),
			@statusEhResponsavel = COALESCE(vw.[statusEhResponsavel], @statusEhResponsavel),
			@etnia = COALESCE(vw.[etnia], @etnia),
			@num_contrato = 22,
			@nomePaiCidadao = COALESCE(vw.[nomePaiCidadao], @nomePaiCidadao),
			@desconheceNomePai = COALESCE(vw.[desconheceNomePai], @desconheceNomePai),
			@dtNaturalizacao = COALESCE(vw.[dtNaturalizacao], @dtNaturalizacao),
			@portariaNaturalizacao = COALESCE(vw.[portariaNaturalizacao], @portariaNaturalizacao),
			@dtEntradaBrasil = COALESCE(vw.[dtEntradaBrasil], @dtEntradaBrasil),
			@microarea = COALESCE(vw.[microarea], @microarea),
			@stForaArea = COALESCE(vw.[stForaArea], @stForaArea),
			@condicoesDeSaude = cad.[condicoesDeSaude],
			@emSituacaoDeRua = cad.[emSituacaoDeRua],
			@informacoesSocioDemograficas = cad.[informacoesSocioDemograficas]
	   FROM [api].[VW_IdentificacaoUsuarioCidadao] AS vw
  LEFT JOIN [api].[IdentificacaoUsuarioCidadao] AS iden
		 ON vw.id = iden.id
  LEFT JOIN [api].[CadastroIndividual] AS cad
		 ON iden.id = cad.identificacaoUsuarioCidadao
	  WHERE ([cnsCidadao] = LTRIM(RTRIM(@cnsCidadao)) AND [cnsCidadao] IS NOT NULL) OR
			([numeroNisPisPasep] = LTRIM(RTRIM(@numeroNisPisPasep)) AND [numeroNisPisPasep] IS NOT NULL);

	INSERT INTO [api].[IdentificacaoUsuarioCidadao] (id, nomeSocial, codigoIbgeMunicipioNascimento, dataNascimentoCidadao, desconheceNomeMae, emailCidadao, nacionalidadeCidadao, nomeCidadao,
		nomeMaeCidadao, cnsCidadao, cnsResponsavelFamiliar, telefoneCelular, numeroNisPisPasep, paisNascimento, racaCorCidadao, sexoCidadao, statusEhResponsavel, etnia, num_contrato,
		nomePaiCidadao, desconheceNomePai, dtNaturalizacao, portariaNaturalizacao, dtEntradaBrasil, microarea, stForaArea) VALUES
		(@idIdentificacaoUsuarioCidadao, @nomeSocial, @codigoIbgeMunicipioNascimento, @dataNascimentoCidadao, @desconheceNomeMae, @emailCidadao, @nacionalidadeCidadao, @nomeCidadao,
		@nomeMaeCidadao, @cnsCidadao, @cnsResponsavelFamiliar, @telefoneCelular, @numeroNisPisPasep, @paisNascimento, @racaCorCidadao, @sexoCidadao, @statusEhResponsavel, @etnia, @num_contrato,
		@nomePaiCidadao, @desconheceNomePai, @dtNaturalizacao, @portariaNaturalizacao, @dtEntradaBrasil, @microarea, @stForaArea);

	INSERT INTO [api].[CadastroIndividual] (id, fichaAtualizada, identificacaoUsuarioCidadao, statusTermoRecusaCadastroIndividualAtencaoBasica, tpCdsOrigem, headerTransport) VALUES
	(@idCadastroIndividual, 0, @idIdentificacaoUsuarioCidadao, 0, 3, @headerTransport);

	INSERT INTO @tokenResult EXEC [api].[PR_INT_EnviarCabecalho] @profissionalCNS, @cboCodigo_2002, @cnes, @dataAtendimento, @codigoIbgeMunicipio, @ine, 3, 0;
	SELECT TOP 1 @token = [token] FROM @tokenResult;
	DELETE FROM @tokenResult;
	
	-- TODO

	COMMIT TRANSACTION T_PR_INT_CadastroIndividual_AssmedCadastro;

	SET @idCadastro = @idCadastroIndividual;

	RETURN 0;
END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	
	ROLLBACK TRANSACTION T_PR_INT_CadastroIndividual_AssmedCadastro;

	SELECT 
		@ErrorMessage = ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE();

	RAISERROR (
		@ErrorMessage,
		@ErrorSeverity,
		@ErrorState);
END CATCH
END
GO
