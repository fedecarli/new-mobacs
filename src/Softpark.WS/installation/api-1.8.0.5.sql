CREATE TABLE [api].[Documentos] (
	Id uniqueidentifier NOT NULL DEFAULT (NEWID()) PRIMARY KEY,
	IdIdentificacaoUsuarioCidadao uniqueidentifier NOT NULL REFERENCES api.IdentificacaoUsuarioCidadao (Id),
	NumContrato int NOT NULL DEFAULT (22),
	IdTipoDocumento int NOT NULL,
	TipoArquivo varchar(60) NOT NULL,
	Tamanho bigint NOT NULL,
	[Data] datetime NOT NULL DEFAULT (GETDATE()),
	Arquivo varbinary(MAX)
) ON [PRIMARY]
GO

ALTER TABLE api.Documentos
	 ADD FOREIGN KEY (NumContrato, IdTipoDocumento) REFERENCES ASSMED_TipoDocPessoal (NumContrato, CodTpDocP)
GO
