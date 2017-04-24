ALTER TABLE [api].[FichaVisitaDomiciliarChild]
		ADD [latitude] NVARCHAR(20) NULL,
			[longitude] NVARCHAR(20) NULL
GO

ALTER TABLE [api].[CadastroDomiciliar]
		ADD [latitude] NVARCHAR(20) NULL,
			[longitude] NVARCHAR(20) NULL
GO

ALTER TABLE [api].[CadastroIndividual]
		ADD [latitude] NVARCHAR(20) NULL,
			[longitude] NVARCHAR(20) NULL
GO

DECLARE @cmd NVARCHAR(500);

DROP TABLE [api].[FichaVisitaDomiciliarChild_MotivoVisita];

SELECT @cmd = 'ALTER TABLE [api].[FichaVisitaDomiciliarChild] DROP CONSTRAINT ' + [name]
  FROM sys.indexes i
 WHERE i.object_id = OBJECT_ID('[api].[FichaVisitaDomiciliarChild]');

IF (@cmd IS NOT NULL)
BEGIN
	EXECUTE sp_executesql @cmd;
END

ALTER TABLE [api].[FichaVisitaDomiciliarChild]
DROP COLUMN [childId];

ALTER TABLE [api].[FichaVisitaDomiciliarChild]
		ADD [childId] UNIQUEIDENTIFIER NOT NULL DEFAULT (NEWID()) PRIMARY KEY;

CREATE TABLE [api].[FichaVisitaDomiciliarChild_MotivoVisita] (
	[childId] UNIQUEIDENTIFIER NOT NULL,
	[codigo]  BIGINT NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[childId] ASC,
		[codigo] ASC
	) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

ALTER TABLE [api].[FichaVisitaDomiciliarChild_MotivoVisita]  WITH CHECK ADD  CONSTRAINT [FK_FichaVisitaDomiciliarChild_Motivos] FOREIGN KEY([childId])
REFERENCES [api].[FichaVisitaDomiciliarChild] ([childId]);

ALTER TABLE [api].[FichaVisitaDomiciliarChild_MotivoVisita] CHECK CONSTRAINT [FK_FichaVisitaDomiciliarChild_Motivos];

ALTER TABLE [api].[FichaVisitaDomiciliarChild_MotivoVisita]  WITH CHECK ADD  CONSTRAINT [FK_MotivoVisita_FichaVisitaDomiciliarChildren] FOREIGN KEY([codigo])
REFERENCES [dbo].[SIGSM_MotivoVisita] ([codigo]);

ALTER TABLE [api].[FichaVisitaDomiciliarChild_MotivoVisita] CHECK CONSTRAINT [FK_MotivoVisita_FichaVisitaDomiciliarChildren];
GO

ALTER TABLE [api].[IdentificacaoUsuarioCidadao]
	   DROP COLUMN [dataNascimentoCidadao]
GO

ALTER TABLE [api].[IdentificacaoUsuarioCidadao]
		ADD [dataNascimentoCidadao] DATE NOT NULL
GO

ALTER TABLE [api].[IdentificacaoUsuarioCidadao]
	   DROP COLUMN [dtNaturalizacao]
GO

ALTER TABLE [api].[IdentificacaoUsuarioCidadao]
		ADD [dtNaturalizacao] DATE NULL
GO

ALTER TABLE [api].[IdentificacaoUsuarioCidadao]
	   DROP COLUMN [dtEntradaBrasil]
GO

ALTER TABLE [api].[IdentificacaoUsuarioCidadao]
		ADD [dtEntradaBrasil] DATE NULL
GO

ALTER TABLE [api].[FamiliaRow]
	   DROP COLUMN [dataNascimentoResponsavel]
GO

ALTER TABLE [api].[FamiliaRow]
		ADD [dataNascimentoResponsavel] DATE NULL
GO

ALTER TABLE [api].[FamiliaRow]
	   DROP COLUMN [resideDesde]
GO

ALTER TABLE [api].[FamiliaRow]
		ADD [resideDesde] DATE NULL
GO

ALTER TABLE [api].[FichaVisitaDomiciliarChild]
	   DROP COLUMN [dtNascimento]
GO

ALTER TABLE [api].[FichaVisitaDomiciliarChild]
		ADD [dtNascimento] DATE NULL
GO

ALTER TABLE [api].[SaidaCidadaoCadastro]
	   DROP COLUMN [dataObito]
GO

ALTER TABLE [api].[SaidaCidadaoCadastro]
		ADD [dataObito] DATE NULL
GO

ALTER TABLE [api].[UnicaLotacaoTransport]
	   DROP COLUMN [dataAtendimento]
GO

ALTER TABLE [api].[UnicaLotacaoTransport]
		ADD [dataAtendimento] DATE NOT NULL
GO

