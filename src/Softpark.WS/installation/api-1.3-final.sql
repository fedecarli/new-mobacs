SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [api].[OrigemVisita]
	  ADD [enviarParaThrift] BIT NOT NULL DEFAULT 1;
GO

-- =============================================
-- Author:		Elton Schivei Costa
-- Create date: 2017/04/04
-- Description:	Função de Conversão de Data em DateTime para Epoch
-- =============================================
CREATE FUNCTION [api].[DateTime2Epoch]
(
	-- Add the parameters for the function here
	@datetime DATETIME
)
RETURNS BIGINT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result BIGINT;

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = CONVERT(BIGINT, DATEDIFF(ss,  '01-01-1970 00:00:00', CONVERT(VARCHAR, DATEADD(hh, DATEDIFF(hh, GETDATE(), GETUTCDATE()), @datetime), 121)));

	-- Return the result of the function
	RETURN @Result;
END
GO

-- =============================================
-- Author:		Elton Schivei Costa
-- Create date: 2017/04/04
-- Description:	Função de Conversão de Data em Epoch para DateTime
-- =============================================
CREATE FUNCTION [api].[Epoch2DateTime]
(
	-- Add the parameters for the function here
	@epoch BIGINT
)
RETURNS DATETIME
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result DATETIME;

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = DATEADD(hh, DATEDIFF(hh, GETDATE(), GETUTCDATE()), DATEADD(ss, @epoch, '01-01-1970 00:00:00'));

	-- Return the result of the function
	RETURN @Result;
END
GO

-- =============================================
-- Author:		Elton Schivei Costa
-- Create date: 2017/04/04
-- Description:	View para visualização de pessoas do assmed_cadastro como identificação de ficha
-- =============================================
CREATE VIEW [api].[VW_IdentificacaoUsuarioCidadao] AS
	 SELECT LTRIM(RTRIM(cad.Nome)) AS nomeCidadao
			,LTRIM(RTRIM(cad.NomeSocial)) AS nomeSocial
			,LTRIM(RTRIM(pis.Numero)) AS numeroNisPisPasep
			,LTRIM(RTRIM(cns.Numero)) AS cnsCidadao
			,_cpf.DtNasc AS dataNascimentoCidadao
			,(CASE WHEN NomeMae IS NULL OR LEN(NomeMae) = 0 THEN NULL ELSE NomeMae END) AS
				nomePaiCidadao
			,(CASE WHEN NomeMae IS NULL OR LEN(NomeMae) = 0 THEN NULL ELSE NomeMae END) AS
				nomeMaeCidadao
			,(CASE WHEN NomeMae IS NULL OR LEN(NomeMae) = 0 OR MaeDesconhecida > 0 THEN 1 ELSE 0 END) AS
				desconheceNomeMae
			,(CASE WHEN NomePai IS NULL OR LEN(NomePai) = 0 OR PaiDesconhecido > 0 THEN 1 ELSE 0 END) AS
				desconheceNomePai
			,cad.[Codigo]
			,cad.[NumContrato]
	   FROM [dbo].[ASSMED_Cadastro] AS cad

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
			MaeDesconhecida, PaiDesconhecido FROM ASSMED_CadastroPF AS cpf
		 WHERE cpf.Codigo = cad.Codigo
		UNION
		SELECT DtNasc, LTRIM(RTRIM(NomePai)) AS NomePai, LTRIM(RTRIM(NomeMae)) AS NomeMae,
			MaeDesconhecida, PaiDesconhecido FROM ASSMED_PesFisica  AS apf
		 WHERE apf.Codigo = cad.Codigo
	 ) AS a WHERE a.DtNasc IS NOT NULL
) AS _cpf;
GO

DROP PROCEDURE [PR_INT_EnviarCabecalho];
GO

-- =============================================
-- Author:		Elton Schivei Costa
-- Create date: 2017/04/04
-- Description:	Procedure para geração de cabeçalho de ficha e token
-- =============================================
CREATE PROCEDURE [api].[PR_INT_EnviarCabecalho] (
	@profissionalCNS NCHAR(15),
	@cboCodigo_2002 NCHAR(6),
	@cnes NCHAR(7),
	@dataAtendimento BIGINT,
	@codigoIbgeMunicipio NCHAR(7),
	@ine NCHAR(11) = NULL,
	@tipoOrigem INT = 3,
	@enviarParaThrift BIT = 1
) AS
BEGIN
BEGIN TRANSACTION
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

	COMMIT TRANSACTION;
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

DROP PROCEDURE [PR_INT_EnviarFicha];
GO

-- =============================================
-- Author:		Elton Schivei Costa
-- Create date: 2017/04/04
-- Description:	Procedure para envio de ficha de visita
-- =============================================
CREATE PROCEDURE [api].[PR_INT_EnviarFicha] (
	@token UNIQUEIDENTIFIER,
	@turno INT,
	@desfecho INT,
	@tipoDeImovel INT,
	@cnsCidadao NCHAR(15) = NULL,
	@dataNascimento BIGINT = NULL,
	@sexo INT = 4,
	@microarea NCHAR(2) = NULL,
	@stForaArea BIT = 0,
	@tipoOrigem INT = 3,
	@numProntuario NVARCHAR(30) = NULL,
	@pesoAcompanhamentoNutricional DECIMAL(6, 3) = NULL,
	@alturaAcompanhamentoNutricional DECIMAL(4, 1) = NULL,
	@statusVisitaCompartilhadaOutroProfissional BIT = 0,
	@motivos [api].[ListInteger] READONLY
) AS
BEGIN
BEGIN TRANSACTION
BEGIN TRY
	DECLARE @header UNIQUEIDENTIFIER,
			@master UNIQUEIDENTIFIER,
			@child  INT,
			@motivo INT;
	DECLARE motivo_cursor CURSOR FAST_FORWARD FOR SELECT [value] FROM @motivos;

	SET NOCOUNT ON;
	IF (SELECT COUNT(*) FROM [api].[OrigemVisita] WHERE token = @token AND finalizado = 0) = 0
	BEGIN
		RAISERROR ('Token inválido.', 1, 1);
	END

	SELECT @header = id FROM [api].[UnicaLotacaoTransport]
		WHERE token = @token;
	SET @master = NEWID();

	INSERT INTO [api].[FichaVisitaDomiciliarMaster]
		(uuidFicha, tpCdsOrigem, headerTransport) VALUES
		(@master, 3, @header);

	INSERT INTO [api].[FichaVisitaDomiciliarChild]
		(alturaAcompanhamentoNutricional, cnsCidadao, desfecho, dtNascimento,
		microarea, numProntuario, pesoAcompanhamentoNutricional, sexo,
		statusVisitaCompartilhadaOutroProfissional, stForaArea, tipoDeImovel, turno,
		uuidFicha) VALUES
		(@alturaAcompanhamentoNutricional, @cnsCidadao, @desfecho, @dataNascimento,
		@microarea, @numProntuario, @pesoAcompanhamentoNutricional, @sexo,
		@statusVisitaCompartilhadaOutroProfissional, @stForaArea, @tipoDeImovel, @turno,
		@master);
	
	SELECT @child = SCOPE_IDENTITY();

	OPEN motivo_cursor;
	FETCH NEXT FROM motivo_cursor INTO @motivo;
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF (SELECT COUNT(*) FROM [dbo].[SIGSM_MotivoVisita] WHERE [codigo] = @motivo) > 0
		BEGIN
			INSERT INTO [api].[FichaVisitaDomiciliarChild_MotivoVisita]
				(childId, codigo) VALUES (@child, @motivo);
		END

		FETCH NEXT FROM motivo_cursor INTO @motivo;
	END
	CLOSE motivo_cursor;
	DEALLOCATE motivo_cursor;

	COMMIT TRANSACTION;
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

-- =============================================
-- Author:		Elton Schivei Costa
-- Create date: 2017/04/04
-- Description:	Procedure para integrar cadastros individuais com o assmed_cadastro e profissionais
-- =============================================
CREATE PROCEDURE [api].[PR_INT_CadastroIndividual_AssmedCadastro] (
	-- cabecalho
	@profissionalCNS NCHAR(15),
	@cboCodigo_2002 NCHAR(6),
	@cnes NCHAR(7),
	@codigoIbgeMunicipio NCHAR(7),
	@ine NCHAR(11) = NULL,
	-- IdentificacaoUsuarioCidadao
	@cnsCidadao NCHAR(30),
	@nomeSocial NVARCHAR(140) = NULL,
	@codigoIbgeMunicipioNascimento NCHAR(14) = NULL,
	@dataNascimentoCidadao INT = NULL,
	@desconheceNomeMae BIT = 0,
	@emailCidadao NVARCHAR(200) = NULL,
	@nacionalidadeCidadao INT = 1,
	@nomeCidadao NVARCHAR(140),
	@nomeMaeCidadao NVARCHAR(140) = NULL,
	@cnsResponsavelFamiliar NCHAR(30) = NULL,
	@telefoneCelular NVARCHAR(22) = NULL,
	@numeroNisPisPasep NCHAR(22) = NULL,
	@paisNascimento INT = NULL,
	@racaCorCidadao INT = 6,
	@sexoCidadao INT = 4,
	@statusEhResponsavel BIT = 0,
	@etnia INT = NULL,
	@nomePaiCidadao NVARCHAR(140) = NULL,
	@desconheceNomePai BIT = 0,
	@dtNaturalizacao INT = NULL,
	@portariaNaturalizacao NVARCHAR(32) = NULL,
	@dtEntradaBrasil INT = NULL,
	@microarea NCHAR(4) = NULL,
	@stForaArea BIT = 0
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
			@codigoIbgeMunicipioNascimento = @codigoIbgeMunicipioNascimento,
			@dataNascimentoCidadao = COALESCE([api].[DateTime2Epoch]([dataNascimentoCidadao]), @dataNascimentoCidadao),
			@desconheceNomeMae = COALESCE([desconheceNomeMae], @desconheceNomeMae),
			@emailCidadao = @emailCidadao,
			@nacionalidadeCidadao = @nacionalidadeCidadao,
			@nomeCidadao = COALESCE([nomeCidadao], @nomeCidadao),
			@nomeMaeCidadao = COALESCE([nomeMaeCidadao], @nomeMaeCidadao),
			@cnsResponsavelFamiliar = @cnsResponsavelFamiliar,
			@telefoneCelular = @telefoneCelular,
			@numeroNisPisPasep = COALESCE([numeroNisPisPasep], @numeroNisPisPasep),
			@paisNascimento = @paisNascimento,
			@racaCorCidadao = @racaCorCidadao,
			@sexoCidadao = @sexoCidadao,
			@statusEhResponsavel = @statusEhResponsavel,
			@etnia = @etnia,
			@num_contrato = [NumContrato],
			@nomePaiCidadao = COALESCE([nomePaiCidadao], @nomePaiCidadao),
			@desconheceNomePai = COALESCE([desconheceNomePai], @desconheceNomePai),
			@dtNaturalizacao = @dtNaturalizacao,
			@portariaNaturalizacao = @portariaNaturalizacao,
			@dtEntradaBrasil = @dtEntradaBrasil,
			@microarea = @microarea,
			@stForaArea = @stForaArea
	   FROM [api].[VW_IdentificacaoUsuarioCidadao]
	  WHERE [cnsCidadao] = LTRIM(RTRIM(@cnsCidadao)) AND [cnsCidadao] IS NOT NULL;

	INSERT INTO [api].[IdentificacaoUsuarioCidadao] (id, nomeSocial, codigoIbgeMunicipioNascimento, dataNascimentoCidadao, desconheceNomeMae, emailCidadao, nacionalidadeCidadao, nomeCidadao,
		nomeMaeCidadao, cnsCidadao, cnsResponsavelFamiliar, telefoneCelular, numeroNisPisPasep, paisNascimento, racaCorCidadao, sexoCidadao, statusEhResponsavel, etnia, num_contrato,
		nomePaiCidadao, desconheceNomePai, dtNaturalizacao, portariaNaturalizacao, dtEntradaBrasil, microarea, stForaArea) VALUES
		(@idIdentificacaoUsuarioCidadao, @nomeSocial, @codigoIbgeMunicipioNascimento, @dataNascimentoCidadao, @desconheceNomeMae, @emailCidadao, @nacionalidadeCidadao, @nomeCidadao,
		@nomeMaeCidadao, @cnsCidadao, @cnsResponsavelFamiliar, @telefoneCelular, @numeroNisPisPasep, @paisNascimento, @racaCorCidadao, @sexoCidadao, @statusEhResponsavel, @etnia, @num_contrato,
		@nomePaiCidadao, @desconheceNomePai, @dtNaturalizacao, @portariaNaturalizacao, @dtEntradaBrasil, @microarea, @stForaArea);

	INSERT INTO [api].[CadastroIndividual] (id, fichaAtualizada, identificacaoUsuarioCidadao, statusTermoRecusaCadastroIndividualAtencaoBasica, tpCdsOrigem, headerTransport) VALUES
	(@idCadastroIndividual, 0, @idIdentificacaoUsuarioCidadao, 0, 3, @headerTransport);

	COMMIT TRANSACTION;

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
