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
		   ORDER BY ut.dataAtendimento DESC
) AS _iuc;
GO


-- =============================================
-- Author:		Elton Schivei Costa
-- Create date: 2017/04/04
-- Update date: 2017/04/23
-- Description:	Procedure para integrar cadastros individuais com o assmed_cadastro e profissionais
-- =============================================
CREATE PROCEDURE [api].[PR_INT_CadastroIndividual_AssmedCadastro] (
	-- cabecalho
	 @profissionalCNS NCHAR(15)
	,@cboCodigo_2002 NCHAR(6)
	,@cnes NCHAR(7)
	,@codigoIbgeMunicipio NCHAR(7)
	,@ine NCHAR(11) = NULL
	-- IdentificacaoUsuarioCidadao
	,@nomeSocial NVARCHAR(140) = NULL
	,@codigoIgbeMunicipioNascimento NCHAR(7) = '3547304'
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
	,@idCadastro UNIQUEIDENTIFIER OUT
) AS
BEGIN
	SET NOCOUNT ON;
BEGIN TRANSACTION
BEGIN TRY
	DECLARE @token UNIQUEIDENTIFIER,
			@headerTransport UNIQUEIDENTIFIER,
			@idCadastroIndividual UNIQUEIDENTIFIER,
			@idIdentificacaoUsuarioCidadao UNIQUEIDENTIFIER,
			@Codigo DECIMAL(18, 0),
			@num_contrato INT,
			@dataAtendimento BIGINT;
	SELECT @dataAtendimento = [api].[DateTime2Epoch](GETDATE());
	DECLARE @tokenResult TABLE ([token] UNIQUEIDENTIFIER);

	INSERT INTO @tokenResult EXEC [api].[PR_INT_EnviarCabecalho] @profissionalCNS, @cboCodigo_2002, @cnes, @dataAtendimento, @codigoIbgeMunicipio, @ine, 3, 0;
	SELECT TOP 1 @token = [token] FROM @tokenResult;
			
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
	
	 SELECT TOP 1 @Codigo = cad.[Codigo]
	   FROM [api].[VW_IdentificacaoUsuarioCidadao] AS cad
	  WHERE cad.[cnsCidadao] = LTRIM(RTRIM(@cnsCidadao));

	IF @Codigo IS NULL
		RAISERROR ('Cadastro do Paciente não encontrado no SIGSM.', 1, 1);

	 SELECT @nomeSocial = COALESCE([nomeSocial], @nomeSocial),
			@codigoIbgeMunicipioNascimento = COALESCE([codigoIbgeMunicipioNascimento], @codigoIbgeMunicipioNascimento),
			@dataNascimentoCidadao = COALESCE([dataNascimentoCidadao], @dataNascimentoCidadao),
			@desconheceNomeMae = COALESCE([desconheceNomeMae], @desconheceNomeMae),
			@emailCidadao = COALESCE([emailCidadao], @emailCidadao),
			@nacionalidadeCidadao = COALESCE([nacionalidadeCidadao], @nacionalidadeCidadao),
			@nomeCidadao = COALESCE([nomeCidadao], @nomeCidadao),
			@nomeMaeCidadao = COALESCE([nomeMaeCidadao], @nomeMaeCidadao),
			@cnsResponsavelFamiliar = COALESCE([cnsResponsavelFamiliar], @cnsResponsavelFamiliar),
			@telefoneCelular = COALESCE([telefoneCelular], @telefoneCelular),
			@numeroNisPisPasep = COALESCE([numeroNisPisPasep], @numeroNisPisPasep),
			@paisNascimento = COALESCE([paisNascimento], @paisNascimento),
			@racaCorCidadao = COALESCE([racaCorCidadao], @racaCorCidadao),
			@sexoCidadao = COALESCE([sexoCidadao], @sexoCidadao),
			@statusEhResponsavel = COALESCE([statusEhResponsavel], @statusEhResponsavel),
			@etnia = COALESCE([etnia], @etnia),
			@num_contrato = 22,
			@nomePaiCidadao = COALESCE([nomePaiCidadao], @nomePaiCidadao),
			@desconheceNomePai = COALESCE([desconheceNomePai], @desconheceNomePai),
			@dtNaturalizacao = COALESCE([dtNaturalizacao], @dtNaturalizacao),
			@portariaNaturalizacao = COALESCE([portariaNaturalizacao], @portariaNaturalizacao),
			@dtEntradaBrasil = COALESCE([dtEntradaBrasil], @dtEntradaBrasil),
			@microarea = COALESCE([microarea], @microarea),
			@stForaArea = COALESCE([stForaArea], @stForaArea)
	   FROM [api].[VW_IdentificacaoUsuarioCidadao]
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

	COMMIT TRANSACTION;

	SET @idCadastro = @idCadastroIndividual;

	RETURN 0;
END TRY
BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
	
	ROLLBACK TRANSACTION;

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
