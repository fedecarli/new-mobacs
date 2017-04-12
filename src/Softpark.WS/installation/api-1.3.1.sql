-- =============================================
-- Author:		Elton Schivei Costa
-- Create date: 2017/04/07
-- Description:	Procedure para cadastro de credenciado
-- =============================================
ALTER PROCEDURE PR_INT_CadastroCredenciado (
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
	@codConv INT,
	@grupoID INT
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

	IF (SELECT COUNT([Email]) FROM [dbo].[ASSMED_Usuario] WHERE [Email] = @email) = 0
	BEGIN
		SELECT @codUsu = MAX([CodUsu]) + 1 FROM [dbo].[ASSMED_Usuario];

		INSERT INTO ASSMED_Usuario ([CodUsu], [Login], [Nome], [Senha], [DtSistema], [Email], [Ativo]) VALUES
			(@codUsu, @login, @nome, @md5Pass, GETDATE(), @email, 1);
	END
	ELSE
		SELECT @codUsu = [CodUsu] FROM [dbo].[ASSMED_Usuario] WHERE [Email] = @email;
		
	IF 0 = (SELECT COUNT(*) FROM Grupo_Usuario WHERE id_grupo = @grupoID AND CodUsu = @codUsu)
		INSERT INTO Grupo_Usuario([id_grupo], [CodUsu], [data_cadastro])VALUES(@grupoID,@codUsu,GETDATE());

	IF 0 = (SELECT COUNT(*) FROM ASSMED_ContratoUsuario WHERE [Email] = @email)
		INSERT INTO ASSMED_ContratoUsuario([Email], [NumContrato], [Administrador])VALUES(@email,22,'N');

	IF 0 = (SELECT COUNT(*) FROM ASSMED_SistemaUsuario WHERE [CodSistema] = 99 AND CodUsu = @codUsu)
		INSERT INTO ASSMED_SistemaUsuario([CodSistema], [CodUsu], [NumContrato])VALUES(99, @CodUsu, 22);
	
    IF (SELECT COUNT([CodCred]) FROM [dbo].[AS_Credenciados] WHERE [Codigo] = @codigo) > 0
        SELECT @codCred = [CodCred] FROM [dbo].[AS_Credenciados] WHERE [Codigo] = @codigo AND [NumContrato] = 22;
    ELSE
        SELECT @codCred = MAX([CodCred]) + 1 FROM [dbo].[AS_Credenciados];
	
    SELECT @itemU = MAX([ItemU]) + 1 FROM [dbo].[AS_CredenciadosUsu] WHERE [CodCred] = @codCred AND [NumContrato] = 22;
    
	SELECT @itemV = MAX([ItemVinc]) + 1 FROM [dbo].[AS_CredenciadosVinc] WHERE [CodCred] = @codCred AND [NumContrato] = 22;

    IF (SELECT COUNT([CodCred]) FROM [dbo].[AS_Credenciados] WHERE [CodCred] = @codCred) = 0
        INSERT INTO [dbo].[AS_Credenciados] ([NumContrato], [CodCred], [Codigo], [CodOrgao], [NumConselho])
            VALUES (22, @codCred, @codigo, @codOrgao, @numConselho);

	IF 0 = (SELECT COUNT(*) FROM [dbo].[AS_CredenciadosUsu] WHERE CodCred = @codCred AND CodUsuD = @codUsu)
		INSERT INTO [dbo].[AS_CredenciadosUsu] ([NumContrato], [CodCred], [ItemU], [CodUsuD], [DtInicio], [DtSistema], [CodUsu]) VALUES
			(22, @codCred, @itemU, @CodUsu, GETDATE(), GETDATE(), 1);
		
	SELECT @codTabProf = [CodTabProfissao] FROM [dbo].[AS_ProfissoesTab] WHERE [CodProfTab] = @codProfTab;

	IF 0 = (SELECT COUNT(*) FROM [dbo].[AS_CredenciadosVinc] WHERE CodCred = @codCred AND CodRegime = @regime AND CodProfTab = @codProfTab AND CodTabProfissao = @codTabProf
								AND Matricula = @matricula AND ISNULL(CodConv,'') = ISNULL(@codConv,'') AND CodINE = @codINE AND CNESLocal = @cnes)
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
