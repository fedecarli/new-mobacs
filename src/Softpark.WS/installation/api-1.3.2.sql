CREATE TABLE [api].[RastroFicha](
	[token] [uniqueidentifier] NOT NULL,
	[CodUsu] [int] NOT NULL,
	[DataModificacao] [datetime] NOT NULL,
	[DadoAnterior] [ntext] NULL,
	[DadoAtual] [ntext] NULL,
 CONSTRAINT [PK_RastroFicha] PRIMARY KEY CLUSTERED 
(
	[token] ASC,
	[CodUsu] ASC,
	[DataModificacao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [api].[RastroFicha]  WITH CHECK ADD  CONSTRAINT [FK_RastroFicha_OrigemVisita] FOREIGN KEY([token])
REFERENCES [api].[OrigemVisita] ([token])
GO

ALTER TABLE [api].[RastroFicha] CHECK CONSTRAINT [FK_RastroFicha_OrigemVisita]
GO

/****** Script do comando SelectTopNRows de SSMS  ******/
CREATE VIEW [api].[VW_VISITAS] AS
	 SELECT _child.childId,
			_child.uuidFicha AS masterId,
			_child.turno,
			_child.numProntuario,
			_ind.NOME COLLATE Latin1_General_CI_AI AS NomePaciente,
			_child.cnsCidadao,
			[api].[Epoch2DateTime](_child.dtNascimento) AS dtNascimento,
			_child.sexo,
			_child.statusVisitaCompartilhadaOutroProfissional,
			_child.desfecho,
			_child.microarea,
			_child.stForaArea,
			_child.tipoDeImovel,
			_child.pesoAcompanhamentoNutricional,
			_child.alturaAcompanhamentoNutricional,
			_profiss.CNS AS cnsProfissional,
			_profiss.Nome COLLATE Latin1_General_CI_AI AS nomeProfissional,
			_profiss.CBO,
			_profiss.Profissao COLLATE Latin1_General_CI_AI AS Profissao,
			_profiss.CNES,
			_profiss.Unidade COLLATE Latin1_General_CI_AI AS Unidade,
			_profiss.INE,
			_profiss.Equipe COLLATE Latin1_General_CI_AI AS Equipe,
			_header.codigoIbgeMunicipio,
			[api].[Epoch2DateTime](_header.dataAtendimento) AS dataAtendimento,
			_rastro.token,
			_rastro.CodUsu,
			_rastro.DataModificacao,
			_user.Nome COLLATE Latin1_General_CI_AI AS Usuario,
			_origem.enviado,
			_origem.finalizado
	   FROM [api].[FichaVisitaDomiciliarChild] AS _child
 INNER JOIN [api].[FichaVisitaDomiciliarMaster] AS _master
		 ON _child.uuidFicha = _master.uuidFicha
 INNER JOIN [api].[UnicaLotacaoTransport] _header
		 ON _master.headerTransport = _header.id
 INNER JOIN [api].[OrigemVisita] _origem
		 ON _header.token = _origem.token
 INNER JOIN [api].[VW_Profissional] _profiss
		 ON _header.cboCodigo_2002 COLLATE Latin1_General_CI_AI = _profiss.CBO COLLATE Latin1_General_CI_AI
		AND _header.cnes COLLATE Latin1_General_CI_AI = _profiss.CNES COLLATE Latin1_General_CI_AI
		AND _header.ine COLLATE Latin1_General_CI_AI = _profiss.INE COLLATE Latin1_General_CI_AI
		AND _header.profissionalCNS COLLATE Latin1_General_CI_AI = _profiss.CNS COLLATE Latin1_General_CI_AI
OUTER APPLY ( SELECT TOP 1 *
		        FROM [api].[RastroFicha] AS _rastro
		       WHERE _rastro.token = _header.token
	        ORDER BY _rastro.DataModificacao DESC) AS _rastro
  LEFT JOIN [dbo].[ASSMED_Usuario] _user
		 ON _rastro.CodUsu = _user.CodUsu
  LEFT JOIN [dbo].[VW_INDIVIDUAIS] AS _ind
		 ON LTRIM(RTRIM(_child.cnsCidadao)) COLLATE Latin1_General_CI_AI = LTRIM(RTRIM(_ind.CNS)) COLLATE Latin1_General_CI_AI
	  WHERE _origem.enviarParaThrift = 1
GO

ALTER TABLE [api].[OrigemVisita]
		ADD [enviado] BIT NOT NULL DEFAULT 0;
GO

ALTER TABLE [dbo].[SIGSM_Visita_Domiciliar_Paciente]
	  ALTER COLUMN [st_fora_area] BIT NULL;
GO

ALTER TABLE [dbo].[SIGSM_Visita_Domiciliar_Paciente]
		ADD [acao_educacional] BIT NULL,
			[imovel_com_foco]  BIT NULL,
			[acao_mecanica]	   BIT NULL,
			[tratamento_focal] BIT NULL;
GO
