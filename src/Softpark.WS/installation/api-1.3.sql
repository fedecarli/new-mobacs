/**
 * Script de atualização para a versão 1.3
 */
BEGIN TRANSACTION
BEGIN TRY
	-- Nova tabela de tipo de origem
	CREATE TABLE [api].[TipoOrigem] (
		Id INT NOT NULL PRIMARY KEY IDENTITY,
		Descricao NVARCHAR(50) NOT NULL UNIQUE
	);

	-- Tipos de origem
	INSERT INTO [api].[TipoOrigem] (Descricao) VALUES
		('Mobile'),
		('SIGSM'),
		('Integração');

	-- Alteração da tabela de OrigemVisita para comportar o tipo de origem
	ALTER TABLE [api].[OrigemVisita]
		ADD id_tipo_origem INT NOT NULL DEFAULT 1;

	ALTER TABLE [api].[OrigemVisita]
		ADD CONSTRAINT FK_OrigemVisita_TipoOrigem_Id
			FOREIGN KEY ([id_tipo_origem]) REFERENCES [api].[TipoOrigem]([Id]);

	-- Criação Tipos de dados para suporte às procedure
	CREATE TYPE [api].[ListInteger] AS TABLE 
	(
		[value] INT PRIMARY KEY
	);

	CREATE TYPE [api].[ListUniques] AS TABLE 
	(
		[value] UNIQUEIDENTIFIER PRIMARY KEY
	);
		
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
GO

-- Criação de Stored Procedure Para Integração
CREATE PROCEDURE PR_INT_EnviarCabecalho (
	@profissionalCNS NCHAR(15),
	@cboCodigo_2002 NCHAR(6),
	@cnes NCHAR(7),
	@dataAtendimento BIGINT,
	@codigoIbgeMunicipio NCHAR(7),
	@ine NCHAR(11) = NULL,
	@tipoOrigem INT = 3
) AS
BEGIN
BEGIN TRANSACTION
BEGIN TRY
	SET NOCOUNT ON;
	DECLARE @token UNIQUEIDENTIFIER;
	SET @token = NEWID();

	INSERT INTO [api].[OrigemVisita] ([token], [finalizado], [id_tipo_origem]) VALUES
		(@token, 0, @tipoOrigem);

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

-- Criação de procedure para cadastrar ficha de visita
CREATE PROCEDURE PR_INT_EnviarFicha (
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

-- Criação de procedure para cadastrar Credenciado
CREATE PROCEDURE PR_INT_CadastroCredenciado (
	@codigo decimal(18, 0),
	@login NVARCHAR(80),
	@md5Pass NVARCHAR(80),
	@email NVARCHAR(80),
	@regime INT,
	@cnes NCHAR(7),
	@codOrgao INT = NULL,
	@numConselho NCHAR(10) = NULL,
	@codProfTab NCHAR(10),
	@codSetor INT,
	@matricula NCHAR(15) = NULL,
	@codINE INT = NULL,
	@codConv INT
) AS
BEGIN
BEGIN TRANSACTION
BEGIN TRY
	DECLARE @codUsu INT,
			@nome NVARCHAR(80),
			@codCred INT,
			@itemU INT,
			@itemV INT,
			@codTabProf INT;
	SELECT @nome = [Nome] FROM [dbo].[ASSMED_Cadastro] WHERE [Codigo] = @codigo;
	IF @nome IS NULL OR LEN(@nome) = 0
	BEGIN
		RAISERROR ('Cadastro de Pessoa não encontrado.', 1, 1);
	END

	SELECT @codUsu = MAX([CodUsu]) + 1 FROM [dbo].[ASSMED_Usuario];

	INSERT INTO ASSMED_Usuario ([CodUsu], [Login], [Nome], [Senha], [DtSistema], [Email], [Ativo]) VALUES
		(@codUsu, @login, @nome, @md5Pass, GETDATE(), @email, 1);
		
	INSERT INTO Grupo_Usuario([id_grupo], [CodUsu], [data_cadastro])VALUES(2,@codUsu,GETDATE());

	INSERT INTO ASSMED_ContratoUsuario([Email], [NumContrato], [Administrador])VALUES(@email,22,'N');

	INSERT INTO ASSMED_SistemaUsuario([CodSistema], [CodUsu], [NumContrato])VALUES(99, @CodUsu, 22);
	
	SELECT @codCred = MAX([CodCred]) + 1 FROM [dbo].[AS_Credenciados];
	SELECT @itemU = MAX([ItemU]) + 1 FROM [dbo].[AS_CredenciadosUsu];
	SELECT @itemV = MAX([ItemVinc]) + 1 FROM [dbo].[AS_CredenciadosVinc];

	INSERT INTO [dbo].[AS_Credenciados] ([NumContrato], [CodCred], [Codigo], [CodOrgao], [NumConselho]) VALUES (22, @codCred, @codigo, @codOrgao, @numConselho);

	INSERT INTO [dbo].[AS_CredenciadosUsu] ([NumContrato], [CodCred], [ItemU], [CodUsuD], [DtInicio], [DtSistema], [CodUsu]) VALUES
		(22, @codCred, @itemU, @CodUsu, GETDATE(), GETDATE(), 1);
		
	SELECT @codTabProf = [CodTabProfissao] FROM [dbo].[AS_ProfissoesTab] WHERE [CodProfTab] = @codProfTab;

	INSERT INTO [dbo].[AS_CredenciadosVinc] ([NumContrato], [CodCred], [ItemVinc], [CodRegime], [CodProfTab], [CodTabProfissao], [CodSetor], [CNESLocal],
				[Matricula], [DtInicio], [CodUsu], [CodConv], [CodINE]) VALUES
		(22, @codCred, @itemV, @regime, @codProfTab, @codTabProf, @codSetor, @cnes, @matricula, GETDATE(), @codUsu, @codConv, @codINE);

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
