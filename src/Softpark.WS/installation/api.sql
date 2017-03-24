ALTER TABLE [dbo].[SIGSM_Visita_Domiciliar_Paciente]
  ADD altura_acompanhamento_profissional DECIMAL(4, 1) NULL,
	  cns_cidadao NCHAR(15) NULL,
	  desfecho TINYINT NOT NULL DEFAULT 0,
	  microarea NCHAR(2) NULL,
	  peso_acompanhamento_nutricional DECIMAL(6, 3) NULL,
	  st_fora_area BIT NOT NULL DEFAULT 0,
	  tipo_de_imovel INT NOT NULL DEFAULT 0;
GO
CREATE TABLE [dbo].[SIGSM_MotivoVisita](
	[codigo] bigint NOT NULL PRIMARY KEY,
	[nome] nvarchar(80) NOT NULL,
	[observacoes] nvarchar(255) NOT NULL,
	[campo] nvarchar(120) NOT NULL
);
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (1, N'Cadastramento / Atualização', N'#TIPO_VISITA', N'cadastramento_atualizacao')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (2, N'Consulta', N'#BUSCA_ATIVA', N'consulta')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (3, N'Exame', N'#BUSCA_ATIVA', N'exame')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (4, N'Vacina', N'#BUSCA_ATIVA', N'vacina')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (5, N'Gestante', N'#ACOMPANHAMENTO', N'gestante')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (6, N'Puérpera', N'#ACOMPANHAMENTO', N'puerpera')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (7, N'Recém-nascido', N'#ACOMPANHAMENTO', N'recem_nascido')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (8, N'Criança', N'#ACOMPANHAMENTO', N'crianca')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (9, N'Pessoa com desnutrição', N'#ACOMPANHAMENTO', N'pessoa_com_desnutricao')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (10, N'Pessoa em reabilitação ou com deficiência', N'#ACOMPANHAMENTO', N'pessoa_em_reabilitacao_ou_com_deficiencia')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (11, N'Pessoa com hipertensão', N'#ACOMPANHAMENTO', N'pessoa_com_hipertensao')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (12, N'Pessoa com diabetes', N'#ACOMPANHAMENTO', N'pessoa_com_diabetes')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (13, N'Pessoa com asma', N'#ACOMPANHAMENTO', N'pessoa_com_asma')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (14, N'Pessoa com DPOC / enfisema', N'#ACOMPANHAMENTO', N'pessoa_com_dpoc_enfisema')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (15, N'Pessoa com câncer', N'#ACOMPANHAMENTO', N'pessoa_com_cancer')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (16, N'Pessoa com outras doenças crônicas', N'#ACOMPANHAMENTO', N'pessoa_com_outras_doencas_cronicas')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (17, N'Pessoa com hanseníase', N'#ACOMPANHAMENTO', N'pessoa_com_hanseniase')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (18, N'Pessoa com tuberculose', N'#ACOMPANHAMENTO', N'pessoa_com_tuberculose')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (19, N'Domiciliados / Acamados', N'#ACOMPANHAMENTO', N'domiciliados_acamados')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (20, N'Condições de vulnerabilidade social', N'#ACOMPANHAMENTO', N'condicoes_de_vulnerabilidade_social')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (21, N'Condicionalidades do bolsa família', N'#ACOMPANHAMENTO', N'condicionalidades_de_bolsa_familia_a')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (22, N'Saúde mental', N'#ACOMPANHAMENTO', N'saude_mental')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (23, N'Usuário de álcool', N'#ACOMPANHAMENTO', N'usuario_de_alcool')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (24, N'Usuário de outras drogas', N'#ACOMPANHAMENTO', N'usuario_de_outras_drogas')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (25, N'Egresso de internação', N'#OUTROS', N'egresso_de_internacao')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (27, N'Convite para atividades coletivas / campanha de saúde', N'#OUTROS', N'convite_para_atividades_coletivas_campanha_de_saude')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (28, N'Outros', N'#OUTROS', N'outros')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (29, N'Visita periódica', N'#TIPO_VISITA', N'visita_periodica')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (30, N'Condicionalidades do bolsa família', N'#BUSCA_ATIVA', N'condicionalidades_de_bolsa_familia_b')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (31, N'Orientação / Prevenção', N'#OUTROS', N'orientacao_prevencao')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (32, N'Sintomáticos respiratórios', N'#ACOMPANHAMENTO', N'sintomaticos_respiratorios')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (33, N'Tabagista', N'#ACOMPANHAMENTO', N'tabagista')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (34, N'Ação educativa', N'#CONTROLE_AMBIENTAL_E_VETORIAL', N'acao_educativa')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (35, N'Imóvel com foco', N'#CONTROLE_AMBIENTAL_E_VETORIAL', N'imovel_com_foco')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (36, N'Ação mecânica', N'#CONTROLE_AMBIENTAL_E_VETORIAL', N'acao_mecanica')
GO
INSERT [dbo].[SIGSM_MotivoVisita] ([codigo], [nome], [observacoes], [campo]) VALUES (37, N'Tratamento focal', N'#CONTROLE_AMBIENTAL_E_VETORIAL', N'tratamento_focal')
GO

ALTER TABLE [dbo].[TP_Doenca_Respiratoria]
		ADD codigo int NOT NULL DEFAULT 0;
GO
  UPDATE [dbo].[TP_Doenca_Respiratoria] SET codigo = 29 + id_tp_doenca_respiratoria;
GO
CREATE UNIQUE INDEX UQ_TP_Doenca_Respiratoria_Codigo ON [dbo].[TP_Doenca_Respiratoria]([codigo]);
GO

ALTER TABLE [dbo].[TP_Doenca_Cardiaca]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Doenca_Cardiaca] SET codigo = 23 + id_tp_doenca_cardiaca;
GO
CREATE UNIQUE INDEX UQ_TP_Doenca_Cardiaca_Codigo ON [dbo].[TP_Doenca_Cardiaca]([codigo]);
GO

ALTER TABLE [dbo].[TP_Doenca_Renal]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Doenca_Renal] SET codigo = 26 + id_tp_doenca_renal;
GO
CREATE UNIQUE INDEX UQ_TP_Doenca_Renal_Codigo ON [dbo].[TP_Doenca_Renal]([codigo]);
GO

CREATE TABLE [dbo].[TP_Consideracao_Peso] (
	id_tp_consideracao_peso int primary key identity,
	codigo int unique not null,
	descricao nvarchar(60) not null,
	observacao nvarchar(512) null
);
GO
INSERT INTO [dbo].[TP_Consideracao_Peso] VALUES (21, 'Abaixo do peso', NULL);
INSERT INTO [dbo].[TP_Consideracao_Peso] VALUES (22, 'Peso adequado', NULL);
INSERT INTO [dbo].[TP_Consideracao_Peso] VALUES (23, 'Acima do peso', NULL);
GO

ALTER TABLE [dbo].[TP_Origem_Alimentacao]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Origem_Alimentacao] SET codigo = 36 + id_tp_origem_alimentacao;
GO
CREATE UNIQUE INDEX UQ_TP_Origem_Alimentacao_Codigo ON [dbo].[TP_Origem_Alimentacao]([codigo]);
GO

CREATE TABLE [dbo].[TP_Quantas_Vezes_Alimentacao] (
	id_tp_quantas_vezes_alimentacao int primary key identity,
	codigo int unique not null,
	descricao nvarchar(60) not null,
	observacao nvarchar(512) null
);
GO
INSERT INTO [dbo].[TP_Quantas_Vezes_Alimentacao] VALUES (34, '1 vez', NULL);
INSERT INTO [dbo].[TP_Quantas_Vezes_Alimentacao] VALUES (35, '2 ou 3 vezes', NULL);
INSERT INTO [dbo].[TP_Quantas_Vezes_Alimentacao] VALUES (36, 'mais de 3 vezes', NULL);
GO

ALTER TABLE [dbo].[TP_Sit_Rua]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Sit_Rua] SET codigo = 16 + id_tp_sit_rua;
GO
CREATE UNIQUE INDEX UQ_TP_Sit_Rua_Codigo ON [dbo].[TP_Sit_Rua]([codigo]);
GO

ALTER TABLE [dbo].[TP_Higiene_Pessoal]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Higiene_Pessoal] SET codigo = 41 + id_tp_higiene_pessoal;
GO
CREATE UNIQUE INDEX UQ_TP_Higiente_Pessoal ON [dbo].[TP_Higiene_Pessoal]([codigo]);
GO

CREATE TABLE [dbo].[TP_Nacionalidade] (
	id_tp_nacionalidade int primary key identity,
	codigo int unique not null,
	descricao nvarchar(60) not null,
	observacao nvarchar(512) null
);
GO
INSERT INTO [dbo].[TP_Nacionalidade] VALUES (1, 'Brasileira', NULL);
INSERT INTO [dbo].[TP_Nacionalidade] VALUES (2, 'Naturalizado', NULL);
INSERT INTO [dbo].[TP_Nacionalidade] VALUES (3, 'Estrangeiro', NULL);
GO

CREATE TABLE [dbo].[Paises] (
	codigo int primary key,
	nome nvarchar(80)
);
GO

INSERT INTO [dbo].[Paises] VALUES (1 , 'AFEGANISTÃO');
INSERT INTO [dbo].[Paises] VALUES (2 , 'ÁFRICA DO SUL');
INSERT INTO [dbo].[Paises] VALUES (3 , 'ALBÂNIA');
INSERT INTO [dbo].[Paises] VALUES (4 , 'ALEMANHA');
INSERT INTO [dbo].[Paises] VALUES (5 , 'ANDORRA');
INSERT INTO [dbo].[Paises] VALUES (6 , 'ANGOLA');
INSERT INTO [dbo].[Paises] VALUES (7 , 'ANGUILLA');
INSERT INTO [dbo].[Paises] VALUES (8 , 'ANTÁRCTICA');
INSERT INTO [dbo].[Paises] VALUES (9 , 'ANTIGUA E BARBUDA');
INSERT INTO [dbo].[Paises] VALUES (10 , 'ANTILHAS HOLANDESAS');
INSERT INTO [dbo].[Paises] VALUES (11 , 'ARÁBIA SAUDITA');
INSERT INTO [dbo].[Paises] VALUES (12 , 'ARGÉLIA');
INSERT INTO [dbo].[Paises] VALUES (13 , 'ARGENTINA');
INSERT INTO [dbo].[Paises] VALUES (14 , 'ARMÊNIA');
INSERT INTO [dbo].[Paises] VALUES (15 , 'ARUBA');
INSERT INTO [dbo].[Paises] VALUES (16 , 'AUSTRÁLIA');
INSERT INTO [dbo].[Paises] VALUES (17 , 'ÁUSTRIA');
INSERT INTO [dbo].[Paises] VALUES (18 , 'AZERBAIDJÃO');
INSERT INTO [dbo].[Paises] VALUES (19 , 'BAHAMAS');
INSERT INTO [dbo].[Paises] VALUES (20 , 'BANGLADESH');
INSERT INTO [dbo].[Paises] VALUES (21 , 'BARBADOS');
INSERT INTO [dbo].[Paises] VALUES (22 , 'BAREIN');
INSERT INTO [dbo].[Paises] VALUES (23 , 'BELARUS');
INSERT INTO [dbo].[Paises] VALUES (24 , 'BÉLGICA');
INSERT INTO [dbo].[Paises] VALUES (25 , 'BELIZE');
INSERT INTO [dbo].[Paises] VALUES (26 , 'BENIN');
INSERT INTO [dbo].[Paises] VALUES (27 , 'BERMUDA');
INSERT INTO [dbo].[Paises] VALUES (28 , 'BOLÍVIA');
INSERT INTO [dbo].[Paises] VALUES (29 , 'BÓSNIA-HERZEGÓVINA');
INSERT INTO [dbo].[Paises] VALUES (30 , 'BOTSWANA');
INSERT INTO [dbo].[Paises] VALUES (31 , 'BRASIL');
INSERT INTO [dbo].[Paises] VALUES (32 , 'BRUNEI');
INSERT INTO [dbo].[Paises] VALUES (33 , 'BULGÁRIA');
INSERT INTO [dbo].[Paises] VALUES (34 , 'BURKINA FASSO');
INSERT INTO [dbo].[Paises] VALUES (35 , 'BURUNDI');
INSERT INTO [dbo].[Paises] VALUES (36 , 'BUTÃO');
INSERT INTO [dbo].[Paises] VALUES (37 , 'CABO VERDE');
INSERT INTO [dbo].[Paises] VALUES (38 , 'CAMARÕES');
INSERT INTO [dbo].[Paises] VALUES (39 , 'CAMBOJA');
INSERT INTO [dbo].[Paises] VALUES (40 , 'CANADÁ');
INSERT INTO [dbo].[Paises] VALUES (41 , 'CATAR');
INSERT INTO [dbo].[Paises] VALUES (42 , 'CAZAQUISTÃO');
INSERT INTO [dbo].[Paises] VALUES (43 , 'CHADE');
INSERT INTO [dbo].[Paises] VALUES (44 , 'CHILE');
INSERT INTO [dbo].[Paises] VALUES (45 , 'CHINA');
INSERT INTO [dbo].[Paises] VALUES (46 , 'CHIPRE');
INSERT INTO [dbo].[Paises] VALUES (47 , 'CINGAPURA');
INSERT INTO [dbo].[Paises] VALUES (48 , 'COLÔMBIA');
INSERT INTO [dbo].[Paises] VALUES (49 , 'COMORES');
INSERT INTO [dbo].[Paises] VALUES (50 , 'CONGO');
INSERT INTO [dbo].[Paises] VALUES (51 , 'CORÉIA DO NORTE');
INSERT INTO [dbo].[Paises] VALUES (52 , 'CORÉIA DO SUL');
INSERT INTO [dbo].[Paises] VALUES (53 , 'COSTA DO MARFIM');
INSERT INTO [dbo].[Paises] VALUES (54 , 'COSTA RICA');
INSERT INTO [dbo].[Paises] VALUES (55 , 'CROÁCIA');
INSERT INTO [dbo].[Paises] VALUES (56 , 'CUBA');
INSERT INTO [dbo].[Paises] VALUES (57 , 'DINAMARCA');
INSERT INTO [dbo].[Paises] VALUES (58 , 'DJIBUTI');
INSERT INTO [dbo].[Paises] VALUES (59 , 'DOMINICA');
INSERT INTO [dbo].[Paises] VALUES (60 , 'EGITO');
INSERT INTO [dbo].[Paises] VALUES (61 , 'EL SALVADOR');
INSERT INTO [dbo].[Paises] VALUES (62 , 'EMIRADOS ÁRABES UNIDOS');
INSERT INTO [dbo].[Paises] VALUES (63 , 'EQUADOR');
INSERT INTO [dbo].[Paises] VALUES (64 , 'ERITRÉIA');
INSERT INTO [dbo].[Paises] VALUES (65 , 'ESLOVÁQUIA');
INSERT INTO [dbo].[Paises] VALUES (66 , 'ESLOVÊNIA');
INSERT INTO [dbo].[Paises] VALUES (67 , 'ESPANHA');
INSERT INTO [dbo].[Paises] VALUES (68 , 'ESTADOS UNIDOS DA AMÉRICA');
INSERT INTO [dbo].[Paises] VALUES (69 , 'ESTÔNIA');
INSERT INTO [dbo].[Paises] VALUES (70 , 'ETIÓPIA');
INSERT INTO [dbo].[Paises] VALUES (71 , 'FEDERAÇÃO RUSSA');
INSERT INTO [dbo].[Paises] VALUES (72 , 'FIJI');
INSERT INTO [dbo].[Paises] VALUES (73 , 'FILIPINAS');
INSERT INTO [dbo].[Paises] VALUES (74 , 'FINLÂNDIA');
INSERT INTO [dbo].[Paises] VALUES (75 , 'FRANÇA');
INSERT INTO [dbo].[Paises] VALUES (76 , 'FRANÇA METROPOLITANA');
INSERT INTO [dbo].[Paises] VALUES (77 , 'GABÃO');
INSERT INTO [dbo].[Paises] VALUES (78 , 'GÂMBIA');
INSERT INTO [dbo].[Paises] VALUES (79 , 'GANA');
INSERT INTO [dbo].[Paises] VALUES (80 , 'GEÓRGIA');
INSERT INTO [dbo].[Paises] VALUES (81 , 'GIBRALTAR');
INSERT INTO [dbo].[Paises] VALUES (82 , 'GRÃ-BRETANHA');
INSERT INTO [dbo].[Paises] VALUES (83 , 'GRANADA');
INSERT INTO [dbo].[Paises] VALUES (84 , 'GRÉCIA');
INSERT INTO [dbo].[Paises] VALUES (85 , 'GROENLÂNDIA');
INSERT INTO [dbo].[Paises] VALUES (86 , 'GUADALUPE');
INSERT INTO [dbo].[Paises] VALUES (87 , 'GUAM');
INSERT INTO [dbo].[Paises] VALUES (88 , 'GUATEMALA');
INSERT INTO [dbo].[Paises] VALUES (89 , 'GUIANA');
INSERT INTO [dbo].[Paises] VALUES (90 , 'GUIANA FRANCESA');
INSERT INTO [dbo].[Paises] VALUES (91 , 'GUINÉ');
INSERT INTO [dbo].[Paises] VALUES (92 , 'GUINÉ EQUATORIAL');
INSERT INTO [dbo].[Paises] VALUES (93 , 'GUINÉ-BISSAU');
INSERT INTO [dbo].[Paises] VALUES (94 , 'HAITI');
INSERT INTO [dbo].[Paises] VALUES (95 , 'HOLANDA');
INSERT INTO [dbo].[Paises] VALUES (96 , 'HONDURAS');
INSERT INTO [dbo].[Paises] VALUES (97 , 'HONG KONG');
INSERT INTO [dbo].[Paises] VALUES (98 , 'HUNGRIA');
INSERT INTO [dbo].[Paises] VALUES (99 , 'IÊMEN');
INSERT INTO [dbo].[Paises] VALUES (100, 'ILHA BOUVET');
INSERT INTO [dbo].[Paises] VALUES (101, 'ILHA CHRISTMAS');
INSERT INTO [dbo].[Paises] VALUES (102, 'ILHA NORFOLK');
INSERT INTO [dbo].[Paises] VALUES (103, 'ILHAS CAYMAN');
INSERT INTO [dbo].[Paises] VALUES (104, 'ILHAS COCOS');
INSERT INTO [dbo].[Paises] VALUES (105, 'ILHAS COOK');
INSERT INTO [dbo].[Paises] VALUES (106, 'ILHAS DE GUERNSEY');
INSERT INTO [dbo].[Paises] VALUES (107, 'ILHAS DE JERSEY');
INSERT INTO [dbo].[Paises] VALUES (108, 'ILHAS FAROE');
INSERT INTO [dbo].[Paises] VALUES (109, 'ILHAS GEÓRGIA DO SUL E ILHAS SANDWICH DO SUL');
INSERT INTO [dbo].[Paises] VALUES (110, 'ILHAS HEARD E MAC DONALD');
INSERT INTO [dbo].[Paises] VALUES (111, 'ILHAS MALVINAS');
INSERT INTO [dbo].[Paises] VALUES (112, 'ILHAS MARIANA');
INSERT INTO [dbo].[Paises] VALUES (113, 'ILHAS MARSHALL');
INSERT INTO [dbo].[Paises] VALUES (114, 'ILHAS PITCAIRN');
INSERT INTO [dbo].[Paises] VALUES (115, 'ILHAS REUNIÃO');
INSERT INTO [dbo].[Paises] VALUES (116, 'ILHAS SALOMÃO');
INSERT INTO [dbo].[Paises] VALUES (117, 'ILHAS SANTA HELENA');
INSERT INTO [dbo].[Paises] VALUES (118, 'ILHAS SVALBARD E JAN MAYEN');
INSERT INTO [dbo].[Paises] VALUES (119, 'ILHAS TOKELAU');
INSERT INTO [dbo].[Paises] VALUES (120, 'ILHAS TURKS E CAICOS');
INSERT INTO [dbo].[Paises] VALUES (121, 'ILHAS VIRGENS');
INSERT INTO [dbo].[Paises] VALUES (122, 'ILHAS VIRGENS BRITÂNICAS');
INSERT INTO [dbo].[Paises] VALUES (123, 'ILHAS WALLIS E FUTUNA');
INSERT INTO [dbo].[Paises] VALUES (124, 'ÍNDIA');
INSERT INTO [dbo].[Paises] VALUES (125, 'INDONÉSIA');
INSERT INTO [dbo].[Paises] VALUES (126, 'IRÃ');
INSERT INTO [dbo].[Paises] VALUES (127, 'IRAQUE');
INSERT INTO [dbo].[Paises] VALUES (128, 'IRLANDA');
INSERT INTO [dbo].[Paises] VALUES (129, 'ISLÂNDIA');
INSERT INTO [dbo].[Paises] VALUES (130, 'ISRAEL');
INSERT INTO [dbo].[Paises] VALUES (131, 'ITÁLIA');
INSERT INTO [dbo].[Paises] VALUES (132, 'IUGOSLÁVIA');
INSERT INTO [dbo].[Paises] VALUES (133, 'JAMAICA');
INSERT INTO [dbo].[Paises] VALUES (134, 'JAPÃO');
INSERT INTO [dbo].[Paises] VALUES (135, 'JORDÂNIA');
INSERT INTO [dbo].[Paises] VALUES (136, 'KIRIBATI');
INSERT INTO [dbo].[Paises] VALUES (137, 'KUWEIT');
INSERT INTO [dbo].[Paises] VALUES (138, 'LAOS');
INSERT INTO [dbo].[Paises] VALUES (139, 'LESOTO');
INSERT INTO [dbo].[Paises] VALUES (140, 'LETÔNIA');
INSERT INTO [dbo].[Paises] VALUES (141, 'LÍBANO');
INSERT INTO [dbo].[Paises] VALUES (142, 'LIBÉRIA');
INSERT INTO [dbo].[Paises] VALUES (143, 'LÍBIA');
INSERT INTO [dbo].[Paises] VALUES (144, 'LIECHTENSTEIN');
INSERT INTO [dbo].[Paises] VALUES (145, 'LITUÂNIA');
INSERT INTO [dbo].[Paises] VALUES (146, 'LUXEMBURGO');
INSERT INTO [dbo].[Paises] VALUES (147, 'MACAU');
INSERT INTO [dbo].[Paises] VALUES (148, 'MACEDÔNIA');
INSERT INTO [dbo].[Paises] VALUES (149, 'MADAGASCAR');
INSERT INTO [dbo].[Paises] VALUES (150, 'MALÁSIA');
INSERT INTO [dbo].[Paises] VALUES (151, 'MALAUÍ');
INSERT INTO [dbo].[Paises] VALUES (152, 'MALDIVAS');
INSERT INTO [dbo].[Paises] VALUES (153, 'MALI');
INSERT INTO [dbo].[Paises] VALUES (154, 'MALTA');
INSERT INTO [dbo].[Paises] VALUES (155, 'MARROCOS');
INSERT INTO [dbo].[Paises] VALUES (156, 'MARTINICA');
INSERT INTO [dbo].[Paises] VALUES (157, 'MAURÍCIO');
INSERT INTO [dbo].[Paises] VALUES (158, 'MAURITÂNIA');
INSERT INTO [dbo].[Paises] VALUES (159, 'MAYOTTE');
INSERT INTO [dbo].[Paises] VALUES (160, 'MÉXICO');
INSERT INTO [dbo].[Paises] VALUES (161, 'MIANMAR');
INSERT INTO [dbo].[Paises] VALUES (162, 'MICRONÉSIA');
INSERT INTO [dbo].[Paises] VALUES (163, 'MOÇAMBIQUE');
INSERT INTO [dbo].[Paises] VALUES (164, 'MOLDÁVIA');
INSERT INTO [dbo].[Paises] VALUES (165, 'MÔNACO');
INSERT INTO [dbo].[Paises] VALUES (166, 'MONGÓLIA');
INSERT INTO [dbo].[Paises] VALUES (167, 'MONTSERRAT');
INSERT INTO [dbo].[Paises] VALUES (168, 'NAMÍBIA');
INSERT INTO [dbo].[Paises] VALUES (169, 'NAURU');
INSERT INTO [dbo].[Paises] VALUES (170, 'NEPAL');
INSERT INTO [dbo].[Paises] VALUES (171, 'NICARÁGUA');
INSERT INTO [dbo].[Paises] VALUES (172, 'NIGER');
INSERT INTO [dbo].[Paises] VALUES (173, 'NIGÉRIA');
INSERT INTO [dbo].[Paises] VALUES (174, 'NIUE');
INSERT INTO [dbo].[Paises] VALUES (175, 'NORUEGA');
INSERT INTO [dbo].[Paises] VALUES (176, 'NOVA CALEDÔNIA');
INSERT INTO [dbo].[Paises] VALUES (177, 'NOVA ZELÂNDIA');
INSERT INTO [dbo].[Paises] VALUES (178, 'OMÃ');
INSERT INTO [dbo].[Paises] VALUES (179, 'PALAU');
INSERT INTO [dbo].[Paises] VALUES (180, 'PANAMÁ');
INSERT INTO [dbo].[Paises] VALUES (181, 'PAPUA NOVA GUINÉ');
INSERT INTO [dbo].[Paises] VALUES (182, 'PAQUISTÃO');
INSERT INTO [dbo].[Paises] VALUES (183, 'PARAGUAI');
INSERT INTO [dbo].[Paises] VALUES (184, 'PERU');
INSERT INTO [dbo].[Paises] VALUES (185, 'POLINÉSIA FRANCESA');
INSERT INTO [dbo].[Paises] VALUES (186, 'POLÔNIA');
INSERT INTO [dbo].[Paises] VALUES (187, 'PORTO RICO');
INSERT INTO [dbo].[Paises] VALUES (188, 'PORTUGAL');
INSERT INTO [dbo].[Paises] VALUES (189, 'QUÊNIA');
INSERT INTO [dbo].[Paises] VALUES (190, 'QUIRGUÍZIA');
INSERT INTO [dbo].[Paises] VALUES (191, 'REPÚBLICA CENTRO-AFRICANA');
INSERT INTO [dbo].[Paises] VALUES (192, 'REPÚBLICA DOMINICANA');
INSERT INTO [dbo].[Paises] VALUES (193, 'REPÚBLICA TCHECA');
INSERT INTO [dbo].[Paises] VALUES (194, 'ROMÊNIA');
INSERT INTO [dbo].[Paises] VALUES (195, 'RUANDA');
INSERT INTO [dbo].[Paises] VALUES (196, 'SAHARA OCIDENTAL');
INSERT INTO [dbo].[Paises] VALUES (197, 'SAMOA AMERICANA');
INSERT INTO [dbo].[Paises] VALUES (198, 'SAMOA OCIDENTAL');
INSERT INTO [dbo].[Paises] VALUES (199, 'SAN MARINO');
INSERT INTO [dbo].[Paises] VALUES (200, 'SANTA LÚCIA');
INSERT INTO [dbo].[Paises] VALUES (201, 'SÃO CRISTÓVÃO E NÉVIS');
INSERT INTO [dbo].[Paises] VALUES (202, 'SÃO PIERRE E MIQUELON');
INSERT INTO [dbo].[Paises] VALUES (203, 'SÃO TOMÉ E PRÍNCIPE');
INSERT INTO [dbo].[Paises] VALUES (204, 'SÃO VICENTE E GRANADINAS');
INSERT INTO [dbo].[Paises] VALUES (205, 'SEICHELES');
INSERT INTO [dbo].[Paises] VALUES (206, 'SENEGAL');
INSERT INTO [dbo].[Paises] VALUES (207, 'SERRA LEOA');
INSERT INTO [dbo].[Paises] VALUES (208, 'SÍRIA');
INSERT INTO [dbo].[Paises] VALUES (209, 'SOMÁLIA');
INSERT INTO [dbo].[Paises] VALUES (210, 'SRI LANKA');
INSERT INTO [dbo].[Paises] VALUES (211, 'SUAZILÂNDIA');
INSERT INTO [dbo].[Paises] VALUES (212, 'SUDÃO');
INSERT INTO [dbo].[Paises] VALUES (213, 'SUÉCIA');
INSERT INTO [dbo].[Paises] VALUES (214, 'SUÍÇA');
INSERT INTO [dbo].[Paises] VALUES (215, 'SURINAME');
INSERT INTO [dbo].[Paises] VALUES (216, 'TADJIQUISTÃO');
INSERT INTO [dbo].[Paises] VALUES (217, 'TAILÂNDIA');
INSERT INTO [dbo].[Paises] VALUES (218, 'TAIWAN');
INSERT INTO [dbo].[Paises] VALUES (219, 'TANZÂNIA');
INSERT INTO [dbo].[Paises] VALUES (220, 'TERRITÓRIOS FRANCESES MERIDIONAIS');
INSERT INTO [dbo].[Paises] VALUES (221, 'TIMOR LESTE');
INSERT INTO [dbo].[Paises] VALUES (222, 'TOGO');
INSERT INTO [dbo].[Paises] VALUES (223, 'TONGA');
INSERT INTO [dbo].[Paises] VALUES (224, 'TRINIDAD E TOBAGO');
INSERT INTO [dbo].[Paises] VALUES (225, 'TUNÍSIA');
INSERT INTO [dbo].[Paises] VALUES (226, 'TURCOMÊNIA');
INSERT INTO [dbo].[Paises] VALUES (227, 'TURQUIA');
INSERT INTO [dbo].[Paises] VALUES (228, 'TUVALU');
INSERT INTO [dbo].[Paises] VALUES (229, 'UCRÂNIA');
INSERT INTO [dbo].[Paises] VALUES (230, 'UGANDA');
INSERT INTO [dbo].[Paises] VALUES (231, 'URUGUAI');
INSERT INTO [dbo].[Paises] VALUES (232, 'UZBEQUISTÃO');
INSERT INTO [dbo].[Paises] VALUES (233, 'VANUATU');
INSERT INTO [dbo].[Paises] VALUES (234, 'VATICANO');
INSERT INTO [dbo].[Paises] VALUES (235, 'VENEZUELA');
INSERT INTO [dbo].[Paises] VALUES (236, 'VIETNÃ');
INSERT INTO [dbo].[Paises] VALUES (237, 'ZÂMBIA');
INSERT INTO [dbo].[Paises] VALUES (238, 'ZIMBÁBUE');
GO
INSERT INTO [dbo].[TP_Raca_Cor] VALUES ('Sem Informação' ,NULL ,1 ,GETDATE() ,NULL ,GETDATE())
GO

CREATE TABLE [dbo].[TP_Sexo] (
	codigo int primary key,
	descricao nvarchar(60) not null,
	observacao nvarchar(512) null
);
GO
INSERT INTO [dbo].[TP_Sexo] VALUES (0, 'Masculino', NULL);
INSERT INTO [dbo].[TP_Sexo] VALUES (1, 'Feminino', NULL);
INSERT INTO [dbo].[TP_Sexo] VALUES (4, 'Ignorado', 'Apenas quando não foi informado o sexo do cidadão.');
GO
GO

ALTER TABLE [dbo].[TP_Deficiencia]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Deficiencia] SET codigo = 11 + id_tp_deficiencia;
GO
CREATE UNIQUE INDEX UQ_TP_Deficiencia_Codigo ON [dbo].[TP_Deficiencia](codigo);
GO

ALTER TABLE [dbo].[TP_Curso]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Curso] SET codigo = 50 + id_tp_curso;
GO
CREATE UNIQUE INDEX UQ_TP_Curso_Codigo ON [dbo].[TP_Curso](codigo);
GO

CREATE UNIQUE INDEX UQ_AS_ProfissoesTab_CodProfTab ON [dbo].[AS_ProfissoesTab] ([CodProfTab]);
GO

CREATE TABLE [dbo].[TP_Orientacao_Sexual] (
	codigo int primary key,
	descricao nvarchar(50) not null,
	observacoes nvarchar(512)
);
GO
INSERT INTO [dbo].[TP_Orientacao_Sexual] VALUES (148, 'Heterossexual', NULL);
INSERT INTO [dbo].[TP_Orientacao_Sexual] VALUES (153, 'Homossexual (gay / lésbica)', NULL);
INSERT INTO [dbo].[TP_Orientacao_Sexual] VALUES (154, 'Bissexual', NULL);
INSERT INTO [dbo].[TP_Orientacao_Sexual] VALUES (155, 'Outro', NULL);
GO

CREATE TABLE [dbo].[TP_Relacao_Parentesco] (
	codigo int primary key,
	descricao nvarchar(50) not null,
	observacoes nvarchar(512)
);
GO
INSERT INTO [dbo].[TP_Relacao_Parentesco] VALUES (137, 'Cônjuge / Companheiro(a)', NULL);
INSERT INTO [dbo].[TP_Relacao_Parentesco] VALUES (138, 'Filho(a)', NULL);
INSERT INTO [dbo].[TP_Relacao_Parentesco] VALUES (139, 'Enteado(a)', NULL);
INSERT INTO [dbo].[TP_Relacao_Parentesco] VALUES (140, 'Neto(a) / Bisneto(a)', NULL);
INSERT INTO [dbo].[TP_Relacao_Parentesco] VALUES (141, 'Pai / Mãe', NULL);
INSERT INTO [dbo].[TP_Relacao_Parentesco] VALUES (142, 'Sogro(a)', NULL);
INSERT INTO [dbo].[TP_Relacao_Parentesco] VALUES (143, 'Irmão / Irmã', NULL);
INSERT INTO [dbo].[TP_Relacao_Parentesco] VALUES (144, 'Genro / Nora', NULL);
INSERT INTO [dbo].[TP_Relacao_Parentesco] VALUES (145, 'Outro parente', NULL);
INSERT INTO [dbo].[TP_Relacao_Parentesco] VALUES (146, 'Não parente', NULL);
GO

ALTER TABLE [dbo].[TP_Crianca]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Crianca] SET codigo = 1 WHERE id_tp_crianca = 1;
UPDATE [dbo].[TP_Crianca] SET codigo = 2 WHERE id_tp_crianca = 2;
UPDATE [dbo].[TP_Crianca] SET codigo = 133 WHERE id_tp_crianca = 3;
UPDATE [dbo].[TP_Crianca] SET codigo = 3 WHERE id_tp_crianca = 4;
UPDATE [dbo].[TP_Crianca] SET codigo = 134 WHERE id_tp_crianca = 5;
UPDATE [dbo].[TP_Crianca] SET codigo = 4 WHERE id_tp_crianca = 6;
GO
CREATE UNIQUE INDEX UQ_TP_Crianca_Codigo ON [dbo].[TP_Crianca](codigo);
GO

CREATE TABLE [dbo].[TP_Identidade_Genero_Cidadao] (
	codigo INT primary key,
	descricao nvarchar(50) not null,
	observacao nvarchar(512)
)
GO
INSERT INTO [dbo].[TP_Identidade_Genero_Cidadao] VALUES (149, 'Homem transsexual', NULL);
INSERT INTO [dbo].[TP_Identidade_Genero_Cidadao] VALUES (150, 'Mulher transsexual', NULL);
INSERT INTO [dbo].[TP_Identidade_Genero_Cidadao] VALUES (156, 'Travesti', NULL);
INSERT INTO [dbo].[TP_Identidade_Genero_Cidadao] VALUES (151, 'Outro', NULL);
GO

ALTER TABLE [dbo].[TP_Sit_Mercado]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Sit_Mercado] SET codigo = 65 + id_tp_sit_mercado;
INSERT INTO [dbo].[TP_Sit_Mercado] VALUES ('Servidor público / Militar', NULL, 1, GETDATE(), NULL, GETDATE(), 147);
GO
CREATE UNIQUE INDEX UQ_TP_Sit_Mercado_Codigo ON [dbo].[TP_Sit_Mercado](codigo);
GO

CREATE TABLE [dbo].[TP_Motivo_Saida] (
	codigo int primary key,
	descricao nvarchar(50) not null,
	observacao nvarchar(512),
)
GO
INSERT INTO [dbo].[TP_Motivo_Saida] VALUES (135, 'Óbito', NULL);
INSERT INTO [dbo].[TP_Motivo_Saida] VALUES (136, 'Mudança de território', NULL);
GO

ALTER TABLE [dbo].[TP_Animais]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Animais] SET codigo = 127 + id_tp_animais;
UPDATE [dbo].[TP_Animais] SET ativo  = 0 WHERE id_tp_animais = 4;
GO
CREATE UNIQUE INDEX UQ_TP_Animais_Codigo ON [dbo].[TP_Animais](codigo);
GO

ALTER TABLE [dbo].[TP_Abastecimento_Agua]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Abastecimento_Agua] SET codigo = 116 + id_TP_Abastecimento_Agua;
GO
CREATE UNIQUE INDEX UQ_TP_Abastecimento_Agua_Codigo ON [dbo].[TP_Abastecimento_Agua](codigo);
GO
CREATE TABLE [dbo].[TP_Cond_Posse_Uso_Terra] (
	codigo INT primary key, descricao nvarchar(60) NOT NULL, observacoes nvarchar(512) )
GO
INSERT INTO [dbo].[TP_Cond_Posse_Uso_Terra] VALUES (101, 'Proprietário', NULL);
INSERT INTO [dbo].[TP_Cond_Posse_Uso_Terra] VALUES (102, 'Parceiro(a) / Meeiro(a)', NULL);
INSERT INTO [dbo].[TP_Cond_Posse_Uso_Terra] VALUES (103, 'Assentado(a)', NULL);
INSERT INTO [dbo].[TP_Cond_Posse_Uso_Terra] VALUES (104, 'Posseiro', NULL);
INSERT INTO [dbo].[TP_Cond_Posse_Uso_Terra] VALUES (105, 'Arrendatário(a)', NULL);
INSERT INTO [dbo].[TP_Cond_Posse_Uso_Terra] VALUES (106, 'Comodatário(a)', NULL);
INSERT INTO [dbo].[TP_Cond_Posse_Uso_Terra] VALUES (107, 'Beneficiário(a) do banco da terra', NULL);
INSERT INTO [dbo].[TP_Cond_Posse_Uso_Terra] VALUES (108, 'Não se aplica', NULL);
GO

ALTER TABLE [dbo].[TP_Destino_Lixo]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Destino_Lixo] SET codigo = 92 + id_TP_Destino_Lixo;
GO
CREATE UNIQUE INDEX UQ_TP_Destino_Lixo_Codigo ON [dbo].[TP_Destino_Lixo](codigo);
GO

ALTER TABLE [dbo].[TP_Escoamento_Esgoto]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Escoamento_Esgoto] SET codigo = 121 + id_TP_Escoamento_Esgoto;
GO
CREATE UNIQUE INDEX UQ_TP_Escoamento_Esgoto_Codigo ON [dbo].[TP_Escoamento_Esgoto](codigo);
GO

ALTER TABLE [dbo].[TP_Localizacao]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Localizacao] SET codigo = 82 + id_TP_Localizacao;
GO
CREATE UNIQUE INDEX UQ_TP_Localizacao_Codigo ON [dbo].[TP_Localizacao](codigo);
GO

ALTER TABLE [dbo].[TP_Construcao_Domicilio]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Construcao_Domicilio] SET codigo = 108 + id_TP_Construcao_Domicilio;
GO
CREATE UNIQUE INDEX UQ_TP_Construcao_Domicilio_Codigo ON [dbo].[TP_Construcao_Domicilio](codigo);
GO

ALTER TABLE [dbo].[TP_Situacao_Moradia]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Situacao_Moradia] SET codigo = 74 + id_TP_Situacao_Moradia;
GO
CREATE UNIQUE INDEX UQ_TP_Situacao_Moradia_Codigo ON [dbo].[TP_Situacao_Moradia](codigo);
GO

ALTER TABLE [dbo].[TP_Acesso_Domicilio]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Acesso_Domicilio] SET codigo = 88 + id_TP_Acesso_Domicilio;
GO
CREATE UNIQUE INDEX UQ_TP_Acesso_Domicilio_Codigo ON [dbo].[TP_Acesso_Domicilio](codigo);
GO

ALTER TABLE [dbo].[TP_Domicilio]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Domicilio] SET codigo = 84 + id_TP_Domicilio;
GO
CREATE UNIQUE INDEX UQ_TP_Domicilio_Codigo ON [dbo].[TP_Domicilio](codigo);
GO

ALTER TABLE [dbo].[TP_Tratamento_Agua]
		ADD codigo INT NOT NULL DEFAULT 0;
GO
UPDATE [dbo].[TP_Tratamento_Agua] SET codigo = 96 + id_TP_Tratamento_Agua;
INSERT INTO [dbo].[TP_Tratamento_Agua] VALUES ('Mineral', NULL, 1, GETDATE(), NULL, GETDATE(), 152);
GO
CREATE UNIQUE INDEX UQ_TP_Tratamento_Agua_Codigo ON [dbo].[TP_Tratamento_Agua](codigo);
GO

CREATE TABLE [dbo].[TP_Renda_Familiar] (
	codigo INT primary key, descricao nvarchar(60) NOT NULL, observacoes nvarchar(512) )
GO
INSERT INTO [dbo].[TP_Renda_Familiar] VALUES (1, '1/4 de salário mínimo', NULL);
INSERT INTO [dbo].[TP_Renda_Familiar] VALUES (2, 'Meio salário mínimo', NULL);
INSERT INTO [dbo].[TP_Renda_Familiar] VALUES (3, 'Um salário mínimo', NULL);
INSERT INTO [dbo].[TP_Renda_Familiar] VALUES (4, 'Dois salários mínimos', NULL);
INSERT INTO [dbo].[TP_Renda_Familiar] VALUES (7, 'Três salários mínimos', NULL);
INSERT INTO [dbo].[TP_Renda_Familiar] VALUES (5, 'Quatro salários mínimos', NULL);
INSERT INTO [dbo].[TP_Renda_Familiar] VALUES (6, 'Acima de quatro salários mínimos', NULL);
GO

CREATE TABLE [dbo].[TP_Imovel] (
	codigo INT primary key, descricao nvarchar(256) NOT NULL )
GO
INSERT INTO [dbo].[TP_Imovel] VALUES (1, 'Domicílio');
INSERT INTO [dbo].[TP_Imovel] VALUES (2, 'Comércio');
INSERT INTO [dbo].[TP_Imovel] VALUES (3, 'Terreno baldio');
INSERT INTO [dbo].[TP_Imovel] VALUES (4, 'Ponto Estratégico (cemitério, borracharia, ferro-velho, depósito de sucata ou materiais de construção, garagem de ônibus ou veículo de grande porte)');
INSERT INTO [dbo].[TP_Imovel] VALUES (5, 'Escola');
INSERT INTO [dbo].[TP_Imovel] VALUES (6, 'Creche');
INSERT INTO [dbo].[TP_Imovel] VALUES (7, 'Abrigo');
INSERT INTO [dbo].[TP_Imovel] VALUES (8, 'Instituição de longa permanência para idosos');
INSERT INTO [dbo].[TP_Imovel] VALUES (9, 'Unidade prisional');
INSERT INTO [dbo].[TP_Imovel] VALUES (10, 'Unidade de medida sócio educativa');
INSERT INTO [dbo].[TP_Imovel] VALUES (11, 'Delegacia');
INSERT INTO [dbo].[TP_Imovel] VALUES (12, 'Estabelecimento religioso');
INSERT INTO [dbo].[TP_Imovel] VALUES (99, 'Outros');
GO

/****** Object:  Schema [api]    Script Date: 07/03/2017 06:34:34 ******/
CREATE SCHEMA [api]
GO
/****** Object:  Table [api].[AnimalNoDomicilio]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[AnimalNoDomicilio](
	[id_cadastro_domiciliar] [uniqueidentifier] NOT NULL,
	[id_tp_animal] [int] NOT NULL,
 CONSTRAINT [PK_AnimalNoDomicilio] PRIMARY KEY CLUSTERED 
(
	[id_cadastro_domiciliar] ASC,
	[id_tp_animal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[CadastroDomiciliar]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[CadastroDomiciliar](
	[id] [uniqueidentifier] NOT NULL,
	[condicaoMoradia] [uniqueidentifier] NULL,
	[enderecoLocalPermanencia] [uniqueidentifier] NULL,
	[fichaAtualizada] [bit] NOT NULL,
	[quantosAnimaisNoDomicilio] [int] NULL,
	[stAnimaisNoDomicilio] [bit] NOT NULL,
	[statusTermoRecusa] [bit] NOT NULL,
	[tpCdsOrigem] [int] NOT NULL,
	[uuidFichaOriginadora] [nvarchar](44) NULL,
	[tipoDeImovel] [int] NOT NULL,
	[instituicaoPermanencia] [uniqueidentifier] NULL,
	[headerTransport] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CadastroDomiciliar] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[CadastroIndividual]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[CadastroIndividual](
	[id] [uniqueidentifier] NOT NULL,
	[condicoesDeSaude] [uniqueidentifier] NULL,
	[emSituacaoDeRua] [uniqueidentifier] NULL,
	[fichaAtualizada] [bit] NOT NULL,
	[identificacaoUsuarioCidadao] [uniqueidentifier] NULL,
	[informacoesSocioDemograficas] [uniqueidentifier] NULL,
	[statusTermoRecusaCadastroIndividualAtencaoBasica] [bit] NOT NULL,
	[tpCdsOrigem] [int] NOT NULL,
	[uuidFichaOriginadora] [nchar](44) NULL,
	[saidaCidadaoCadastro] [uniqueidentifier] NULL,
	[headerTransport] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CadastroIndividual] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[CondicaoMoradia]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[CondicaoMoradia](
	[id] [uniqueidentifier] NOT NULL,
	[abastecimentoAgua] [int] NULL,
	[areaProducaoRural] [int] NULL,
	[destinoLixo] [int] NULL,
	[formaEscoamentoBanheiro] [int] NULL,
	[localizacao] [int] NULL,
	[materialPredominanteParedesExtDomicilio] [int] NULL,
	[nuComodos] [int] NULL,
	[nuMoradores] [int] NULL,
	[situacaoMoradiaPosseTerra] [int] NULL,
	[stDisponibilidadeEnergiaEletrica] [bit] NOT NULL,
	[tipoAcessoDomicilio] [int] NULL,
	[tipoDomicilio] [int] NULL,
	[aguaConsumoDomicilio] [int] NULL,
 CONSTRAINT [PK_CondicaoMoradia] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[CondicoesDeSaude]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[CondicoesDeSaude](
	[id] [uniqueidentifier] NOT NULL,
	[descricaoCausaInternacaoEm12Meses] [nvarchar](100) NULL,
	[descricaoOutraCondicao1] [nvarchar](100) NULL,
	[descricaoOutraCondicao2] [nvarchar](100) NULL,
	[descricaoOutraCondicao3] [nvarchar](100) NULL,
	[descricaoPlantasMedicinaisUsadas] [nvarchar](100) NULL,
	[maternidadeDeReferencia] [nvarchar](100) NULL,
	[situacaoPeso] [int] NULL,
	[statusEhDependenteAlcool] [bit] NOT NULL,
	[statusEhDependenteOutrasDrogas] [bit] NOT NULL,
	[statusEhFumante] [bit] NOT NULL,
	[statusEhGestante] [bit] NOT NULL,
	[statusEstaAcamado] [bit] NOT NULL,
	[statusEstaDomiciliado] [bit] NOT NULL,
	[statusTemDiabetes] [bit] NOT NULL,
	[statusTemDoencaRespiratoria] [bit] NOT NULL,
	[statusTemHanseniase] [bit] NOT NULL,
	[statusTemHipertensaoArterial] [bit] NOT NULL,
	[statusTemTeveCancer] [bit] NOT NULL,
	[statusTemTeveDoencasRins] [bit] NOT NULL,
	[statusTemTuberculose] [bit] NOT NULL,
	[statusTeveAvcDerrame] [bit] NOT NULL,
	[statusTeveDoencaCardiaca] [bit] NOT NULL,
	[statusTeveInfarto] [bit] NOT NULL,
	[statusTeveInternadoem12Meses] [bit] NOT NULL,
	[statusUsaOutrasPraticasIntegrativasOuComplementares] [bit] NOT NULL,
	[statusUsaPlantasMedicinais] [bit] NOT NULL,
	[statusDiagnosticoMental] [bit] NOT NULL,
 CONSTRAINT [PK_CondicoesDeSaude] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[DeficienciasCidadao]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[DeficienciasCidadao](
	[id_informacoes_socio_demograficas] [uniqueidentifier] NOT NULL,
	[id_tp_deficiencia_cidadao] [int] NOT NULL,
 CONSTRAINT [PK_DeficienciasCidadao] PRIMARY KEY CLUSTERED 
(
	[id_informacoes_socio_demograficas] ASC,
	[id_tp_deficiencia_cidadao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[DoencaCardiaca]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[DoencaCardiaca](
	[id_tp_doenca_cariaca] [int] NOT NULL,
	[id_condicao_de_saude] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_DoencaCardiaca] PRIMARY KEY CLUSTERED 
(
	[id_tp_doenca_cariaca] ASC,
	[id_condicao_de_saude] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[DoencaRespiratoria]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[DoencaRespiratoria](
	[id_tp_doenca_respiratoria] [int] NOT NULL,
	[id_condicao_de_saude] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_DoencaRespiratoria] PRIMARY KEY CLUSTERED 
(
	[id_tp_doenca_respiratoria] ASC,
	[id_condicao_de_saude] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[DoencaRins]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[DoencaRins](
	[id_tp_doenca_rins] [int] NOT NULL,
	[id_condicao_de_saude] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_DoencaRins] PRIMARY KEY CLUSTERED 
(
	[id_tp_doenca_rins] ASC,
	[id_condicao_de_saude] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[EmSituacaoDeRua]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[EmSituacaoDeRua](
	[id] [uniqueidentifier] NOT NULL,
	[grauParentescoFamiliarFrequentado] [nvarchar](100) NULL,
	[outraInstituicaoQueAcompanha] [nvarchar](100) NULL,
	[quantidadeAlimentacoesAoDiaSituacaoRua] [int] NULL,
	[statusAcompanhadoPorOutraInstituicao] [bit] NOT NULL,
	[statusPossuiReferenciaFamiliar] [bit] NOT NULL,
	[statusRecebeBeneficio] [bit] NOT NULL,
	[statusSituacaoRua] [bit] NOT NULL,
	[statusTemAcessoHigienePessoalSituacaoRua] [bit] NOT NULL,
	[statusVisitaFamiliarFrequentemente] [bit] NOT NULL,
	[tempoSituacaoRua] [int] NULL,
 CONSTRAINT [PK_EmSituacaoDeRua] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[EnderecoLocalPermanencia]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[EnderecoLocalPermanencia](
	[id] [uniqueidentifier] NOT NULL,
	[bairro] [nvarchar](72) NOT NULL,
	[cep] [nchar](8) NOT NULL,
	[codigoIbgeMunicipio] [nchar](7) NOT NULL,
	[complemento] [nvarchar](30) NULL,
	[nomeLogradouro] [nvarchar](72) NOT NULL,
	[numero] [nvarchar](10) NULL,
	[numeroDneUf] [nchar](2) NOT NULL,
	[telefoneContato] [nvarchar](11) NULL,
	[telelefoneResidencia] [nvarchar](11) NULL,
	[tipoLogradouroNumeroDne] [nchar](3) NOT NULL,
	[stSemNumero] [bit] NOT NULL,
	[pontoReferencia] [nvarchar](40) NULL,
	[microarea] [nchar](2) NULL,
	[stForaArea] [bit] NOT NULL,
 CONSTRAINT [PK_EnderecoLocalPermanencia] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[FamiliaRow]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[FamiliaRow](
	[id] [uniqueidentifier] NOT NULL,
	[dataNascimentoResponsavel] [int] NULL,
	[numeroCnsResponsavel] [nchar](15) NOT NULL,
	[numeroMembrosFamilia] [int] NULL,
	[numeroProntuario] [nvarchar](30) NULL,
	[rendaFamiliar] [int] NULL,
	[resideDesde] [int] NULL,
	[stMudanca] [bit] NOT NULL,
 CONSTRAINT [PK_FamiliaRow] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[Familias]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[Familias](
	[id_familia_row] [uniqueidentifier] NOT NULL,
	[id_cadatro_domiciliar] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Familias] PRIMARY KEY CLUSTERED 
(
	[id_familia_row] ASC,
	[id_cadatro_domiciliar] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[FichaVisitaDomiciliarChild]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[FichaVisitaDomiciliarChild](
	[childId] [bigint] IDENTITY(1,1) NOT NULL,
	[uuidFicha] [char](44) NOT NULL,
	[turno] [bigint] NOT NULL,
	[numProntuario] [nvarchar](30) NULL,
	[cnsCidadao] [char](15) NULL,
	[dtNascimento] [bigint] NULL,
	[sexo] [bigint] NULL,
	[statusVisitaCompartilhadaOutroProfissional] [bit] NOT NULL,
	[desfecho] [bigint] NOT NULL,
	[microarea] [char](2) NULL,
	[stForaArea] [bit] NOT NULL,
	[tipoDeImovel] [bigint] NOT NULL,
	[pesoAcompanhamentoNutricional] [decimal](6, 3) NULL,
	[alturaAcompanhamentoNutricional] [decimal](4, 1) NULL,
PRIMARY KEY CLUSTERED 
(
	[childId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[FichaVisitaDomiciliarChild_MotivoVisita]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[FichaVisitaDomiciliarChild_MotivoVisita](
	[childId] [bigint] NOT NULL,
	[codigo] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[childId] ASC,
	[codigo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[FichaVisitaDomiciliarMaster]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[FichaVisitaDomiciliarMaster](
	[uuidFicha] [char](44) NOT NULL,
	[tpCdsOrigem] [int] NOT NULL,
	[headerTransport] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[uuidFicha] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[HigienePessoalSituacaoRua]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[HigienePessoalSituacaoRua](
	[id_em_situacao_de_rua] [uniqueidentifier] NOT NULL,
	[codigo_higiene_pessoal] [int] NOT NULL,
 CONSTRAINT [PK_HigienePessoalSituacaoRua] PRIMARY KEY CLUSTERED 
(
	[id_em_situacao_de_rua] ASC,
	[codigo_higiene_pessoal] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[IdentificacaoUsuarioCidadao]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[IdentificacaoUsuarioCidadao](
	[id] [uniqueidentifier] NOT NULL,
	[nomeSocial] [nvarchar](70) NULL,
	[codigoIbgeMunicipioNascimento] [nchar](7) NULL,
	[dataNascimentoCidadao] [int] NOT NULL,
	[desconheceNomeMae] [bit] NOT NULL,
	[emailCidadao] [nvarchar](100) NULL,
	[nacionalidadeCidadao] [int] NOT NULL,
	[nomeCidadao] [nvarchar](70) NOT NULL,
	[nomeMaeCidadao] [nvarchar](70) NULL,
	[cnsCidadao] [nchar](15) NULL,
	[cnsResponsavelFamiliar] [nchar](15) NULL,
	[telefoneCelular] [nvarchar](11) NULL,
	[numeroNisPisPasep] [nchar](11) NULL,
	[paisNascimento] [int] NULL,
	[racaCorCidadao] [int] NOT NULL,
	[sexoCidadao] [int] NOT NULL,
	[statusEhResponsavel] [bit] NOT NULL,
	[etnia] [int] NULL,
	[num_contrato] [int] NULL,
	[nomePaiCidadao] [nvarchar](70) NULL,
	[desconheceNomePai] [bit] NOT NULL,
	[dtNaturalizacao] [int] NULL,
	[portariaNaturalizacao] [nvarchar](16) NULL,
	[dtEntradaBrasil] [int] NULL,
	[microarea] [nchar](2) NULL,
	[stForaArea] [bit] NOT NULL,
 CONSTRAINT [PK_IdentificacaoUsuarioCidadao] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[InformacoesSocioDemograficas]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[InformacoesSocioDemograficas](
	[id] [uniqueidentifier] NOT NULL,
	[grauInstrucaoCidadao] [int] NULL,
	[ocupacaoCodigoCbo2002] [char](10) NULL,
	[orientacaoSexualCidadao] [int] NULL,
	[povoComunidadeTradicional] [nvarchar](100) NULL,
	[relacaoParentescoCidadao] [int] NULL,
	[situacaoMercadoTrabalhoCidadao] [int] NULL,
	[statusDesejaInformarOrientacaoSexual] [bit] NOT NULL,
	[statusFrequentaBenzedeira] [bit] NOT NULL,
	[statusFrequentaEscola] [bit] NOT NULL,
	[statusMembroPovoComunidadeTradicional] [bit] NOT NULL,
	[statusParticipaGrupoComunitario] [bit] NOT NULL,
	[statusPossuiPlanoSaudePrivado] [bit] NOT NULL,
	[statusTemAlgumaDeficiencia] [bit] NOT NULL,
	[identidadeGeneroCidadao] [int] NULL,
	[statusDesejaInformarIdentidadeGenero] [bit] NOT NULL,
 CONSTRAINT [PK_InformacoesSocioDemograficas] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[InstituicaoPermanencia]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[InstituicaoPermanencia](
	[id] [uniqueidentifier] NOT NULL,
	[nomeInstituicaoPermanencia] [nvarchar](100) NULL,
	[stOutrosProfissionaisVinculados] [bit] NOT NULL,
	[nomeResponsavelTecnico] [nvarchar](70) NOT NULL,
	[cnsResponsavelTecnico] [nchar](15) NULL,
	[cargoInstituicao] [nvarchar](100) NULL,
	[telefoneResponsavelTecnico] [nvarchar](11) NULL,
 CONSTRAINT [PK_InstituicaoPermanencia] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[OrigemAlimentoSituacaoRua]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[OrigemAlimentoSituacaoRua](
	[id_em_situacao_rua] [uniqueidentifier] NOT NULL,
	[id_tp_origem_alimento] [int] NOT NULL,
 CONSTRAINT [PK_OrigemAlimentoSituacaoRua] PRIMARY KEY CLUSTERED 
(
	[id_em_situacao_rua] ASC,
	[id_tp_origem_alimento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[OrigemVisita]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[OrigemVisita](
	[token] [uniqueidentifier] NOT NULL,
	[finalizado] [bit] NOT NULL,
 CONSTRAINT [PK__OrigemVi__CA90DA7B5A836306] PRIMARY KEY CLUSTERED 
(
	[token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[ResponsavelPorCrianca]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[ResponsavelPorCrianca](
	[id_informacoes_sociodemograficas] [uniqueidentifier] NOT NULL,
	[id_tp_crianca] [int] NOT NULL,
 CONSTRAINT [PK_ResponsavelPorCrianca] PRIMARY KEY CLUSTERED 
(
	[id_informacoes_sociodemograficas] ASC,
	[id_tp_crianca] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[SaidaCidadaoCadastro]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[SaidaCidadaoCadastro](
	[id] [uniqueidentifier] NOT NULL,
	[motivoSaidaCidadao] [int] NULL,
	[dataObito] [int] NULL,
	[numeroDO] [nchar](9) NULL,
 CONSTRAINT [PK_SaidaCidadaoCadastro] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [api].[UnicaLotacaoTransport]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [api].[UnicaLotacaoTransport](
	[id] [uniqueidentifier] NOT NULL,
	[profissionalCNS] [nchar](15) NOT NULL,
	[cboCodigo_2002] [int] NOT NULL,
	[cnes] [char](7) NOT NULL,
	[ine] [char](10) NULL,
	[dataAtendimento] [bigint] NOT NULL,
	[codigoIbgeMunicipio] [char](7) NOT NULL,
	[token] [uniqueidentifier] NULL,
 CONSTRAINT [PK_UnicaLotacaoTransport] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [api].[VW_Profissional]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [api].[VW_Profissional] AS
		 SELECT ROW_NUMBER() OVER (ORDER BY cad.Nome) as id, doc.Numero as [CNS],
				cad.Nome as [Nome],
 				cbo.CodProfissao as [CBO],
				cbo.DesProfTab as [Profissao],
				vinc.CNESLocal AS [CNES],
				setor.DesSetor AS [Unidade],
				ine.Numero	   AS [INE],
				ine.Descricao  AS [Equipe]
		   FROM [dbo].[ASSMED_Cadastro]					AS cad
	 INNER JOIN [dbo].[ASSMED_CadastroDocPessoal]		AS doc
			 ON cad.Codigo = doc.Codigo
	 INNER JOIN [dbo].[AS_CREDENCIADOS]					AS cred
			 ON cad.Codigo = cred.Codigo
	 INNER JOIN [dbo].[AS_CredenciadosVinc]				AS vinc
			 ON cred.CodCred = vinc.CodCred
	 INNER JOIN [dbo].[AS_ProfissoesTab]				AS cbo
			 ON vinc.CodProfTab = cbo.CodProfTab
	 INNER JOIN [dbo].[AS_SetoresPar]					AS par
			 ON vinc.CNESLocal = par.CNES
	 INNER JOIN [dbo].[Setores]							AS setor
		  	 ON par.CodSetor = setor.CodSetor
			AND par.Codigo = setor.Codigo
	 INNER JOIN [dbo].[SetoresINEs]						AS ine
			 ON setor.CodSetor = ine.CodSetor
	      WHERE doc.CodTpDocP = 6
		    AND cbo.CodProfissao IS NOT NULL
			AND LEN(LTRIM(RTRIM(COALESCE(doc.Numero, '')))) > 0
			AND LEN(LTRIM(RTRIM(COALESCE(vinc.CNESLocal, '')))) > 0
	   GROUP BY cad.Nome,
				doc.Numero,
				cbo.CodProfissao,
				cbo.DesProfTab,
				vinc.CNESLocal,
				setor.DesSetor,
				ine.Numero,
				ine.Descricao;
GO
/****** Object:  View [api].[VW_Unidade]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [api].[VW_Unidade] AS
				select ROW_NUMBER() OVER (ORDER BY s.DesSetor) as id,
			    p.CNES as [CNES], s.DesSetor AS [Descricao], s.DesSetorRes AS [Observacao] from AS_SetoresPar as p
				inner join Setores as s on p.Codigo = s.Codigo
				and p.NumContrato = s.NumContrato
				and p.CodSetor = s.CodSetor
				where LEN(LTRIM(RTRIM(COALESCE(p.CNES, '')))) > 0;
GO
ALTER TABLE [api].[CadastroIndividual] ADD  CONSTRAINT [DF_CadastroIndividual_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [api].[CondicoesDeSaude] ADD  CONSTRAINT [DF_CondicoesDeSaude_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [api].[EmSituacaoDeRua] ADD  CONSTRAINT [DF_EmSituacaoDeRua_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [api].[EnderecoLocalPermanencia] ADD  CONSTRAINT [DF_EnderecoLocalPermanencia_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [api].[FichaVisitaDomiciliarChild] ADD  DEFAULT ((0)) FOR [statusVisitaCompartilhadaOutroProfissional]
GO
ALTER TABLE [api].[FichaVisitaDomiciliarChild] ADD  DEFAULT ((0)) FOR [stForaArea]
GO
ALTER TABLE [api].[IdentificacaoUsuarioCidadao] ADD  CONSTRAINT [DF_IdentificacaoUsuarioCidadao_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [api].[InformacoesSocioDemograficas] ADD  CONSTRAINT [DF_InformacoesSocioDemograficas_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [api].[InstituicaoPermanencia] ADD  CONSTRAINT [DF_InstituicaoPermanencia_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [api].[OrigemVisita] ADD  CONSTRAINT [DF__OrigemVis__token__25DDFE01]  DEFAULT (newid()) FOR [token]
GO
ALTER TABLE [api].[OrigemVisita] ADD  CONSTRAINT [DF_OrigemVisita_finalizado]  DEFAULT ((0)) FOR [finalizado]
GO
ALTER TABLE [api].[SaidaCidadaoCadastro] ADD  CONSTRAINT [DF_SaidaCidadaoCadastro_id]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [api].[UnicaLotacaoTransport] ADD  CONSTRAINT [DF__UnicaLota__token__15A79638]  DEFAULT (newid()) FOR [id]
GO
ALTER TABLE [api].[AnimalNoDomicilio]  WITH CHECK ADD  CONSTRAINT [FK_AnimalNoDomicilio_CadastroDomiciliar] FOREIGN KEY([id_cadastro_domiciliar])
REFERENCES [api].[CadastroDomiciliar] ([id])
GO
ALTER TABLE [api].[AnimalNoDomicilio] CHECK CONSTRAINT [FK_AnimalNoDomicilio_CadastroDomiciliar]
GO
ALTER TABLE [api].[AnimalNoDomicilio]  WITH CHECK ADD  CONSTRAINT [FK_AnimalNoDomicilio_TP_Animais] FOREIGN KEY([id_tp_animal])
REFERENCES [dbo].[TP_Animais] ([codigo])
GO
ALTER TABLE [api].[AnimalNoDomicilio] CHECK CONSTRAINT [FK_AnimalNoDomicilio_TP_Animais]
GO
ALTER TABLE [api].[CadastroDomiciliar]  WITH CHECK ADD  CONSTRAINT [FK_CadastroDomiciliar_CondicaoMoradia] FOREIGN KEY([condicaoMoradia])
REFERENCES [api].[CondicaoMoradia] ([id])
GO
ALTER TABLE [api].[CadastroDomiciliar] CHECK CONSTRAINT [FK_CadastroDomiciliar_CondicaoMoradia]
GO
ALTER TABLE [api].[CadastroDomiciliar]  WITH CHECK ADD  CONSTRAINT [FK_CadastroDomiciliar_EnderecoLocalPermanencia] FOREIGN KEY([enderecoLocalPermanencia])
REFERENCES [api].[EnderecoLocalPermanencia] ([id])
GO
ALTER TABLE [api].[CadastroDomiciliar] CHECK CONSTRAINT [FK_CadastroDomiciliar_EnderecoLocalPermanencia]
GO
ALTER TABLE [api].[CadastroDomiciliar]  WITH CHECK ADD  CONSTRAINT [FK_CadastroDomiciliar_InstituicaoPermanencia] FOREIGN KEY([instituicaoPermanencia])
REFERENCES [api].[InstituicaoPermanencia] ([id])
GO
ALTER TABLE [api].[CadastroDomiciliar] CHECK CONSTRAINT [FK_CadastroDomiciliar_InstituicaoPermanencia]
GO
ALTER TABLE [api].[CadastroDomiciliar]  WITH CHECK ADD  CONSTRAINT [FK_CadastroDomiciliar_TP_Imovel] FOREIGN KEY([tipoDeImovel])
REFERENCES [dbo].[TP_Imovel] ([codigo])
GO
ALTER TABLE [api].[CadastroDomiciliar] CHECK CONSTRAINT [FK_CadastroDomiciliar_TP_Imovel]
GO
ALTER TABLE [api].[CadastroDomiciliar]  WITH CHECK ADD  CONSTRAINT [FK_CadastroDomiciliar_UnicaLotacaoTransport] FOREIGN KEY([headerTransport])
REFERENCES [api].[UnicaLotacaoTransport] ([id])
GO
ALTER TABLE [api].[CadastroDomiciliar] CHECK CONSTRAINT [FK_CadastroDomiciliar_UnicaLotacaoTransport]
GO
ALTER TABLE [api].[CadastroIndividual]  WITH CHECK ADD  CONSTRAINT [FK_CadastroIndividual_CondicoesDeSaude] FOREIGN KEY([condicoesDeSaude])
REFERENCES [api].[CondicoesDeSaude] ([id])
GO
ALTER TABLE [api].[CadastroIndividual] CHECK CONSTRAINT [FK_CadastroIndividual_CondicoesDeSaude]
GO
ALTER TABLE [api].[CadastroIndividual]  WITH CHECK ADD  CONSTRAINT [FK_CadastroIndividual_EmSituacaoDeRua] FOREIGN KEY([emSituacaoDeRua])
REFERENCES [api].[EmSituacaoDeRua] ([id])
GO
ALTER TABLE [api].[CadastroIndividual] CHECK CONSTRAINT [FK_CadastroIndividual_EmSituacaoDeRua]
GO
ALTER TABLE [api].[CadastroIndividual]  WITH CHECK ADD  CONSTRAINT [FK_CadastroIndividual_IdentificacaoUsuarioCidadao] FOREIGN KEY([identificacaoUsuarioCidadao])
REFERENCES [api].[IdentificacaoUsuarioCidadao] ([id])
GO
ALTER TABLE [api].[CadastroIndividual] CHECK CONSTRAINT [FK_CadastroIndividual_IdentificacaoUsuarioCidadao]
GO
ALTER TABLE [api].[CadastroIndividual]  WITH CHECK ADD  CONSTRAINT [FK_CadastroIndividual_InformacoesSocioDemograficas] FOREIGN KEY([informacoesSocioDemograficas])
REFERENCES [api].[InformacoesSocioDemograficas] ([id])
GO
ALTER TABLE [api].[CadastroIndividual] CHECK CONSTRAINT [FK_CadastroIndividual_InformacoesSocioDemograficas]
GO
ALTER TABLE [api].[CadastroIndividual]  WITH CHECK ADD  CONSTRAINT [FK_CadastroIndividual_SaidaCidadaoCadastro] FOREIGN KEY([saidaCidadaoCadastro])
REFERENCES [api].[SaidaCidadaoCadastro] ([id])
GO
ALTER TABLE [api].[CadastroIndividual] CHECK CONSTRAINT [FK_CadastroIndividual_SaidaCidadaoCadastro]
GO
ALTER TABLE [api].[CadastroIndividual]  WITH CHECK ADD  CONSTRAINT [FK_CadastroIndividual_UnicaLotacaoTransport] FOREIGN KEY([headerTransport])
REFERENCES [api].[UnicaLotacaoTransport] ([id])
GO
ALTER TABLE [api].[CadastroIndividual] CHECK CONSTRAINT [FK_CadastroIndividual_UnicaLotacaoTransport]
GO
ALTER TABLE [api].[CondicaoMoradia]  WITH CHECK ADD  CONSTRAINT [FK_CondicaoMoradia_TP_Abastecimento_Agua] FOREIGN KEY([abastecimentoAgua])
REFERENCES [dbo].[TP_Abastecimento_Agua] ([codigo])
GO
ALTER TABLE [api].[CondicaoMoradia] CHECK CONSTRAINT [FK_CondicaoMoradia_TP_Abastecimento_Agua]
GO
ALTER TABLE [api].[CondicaoMoradia]  WITH CHECK ADD  CONSTRAINT [FK_CondicaoMoradia_TP_Acesso_Domicilio] FOREIGN KEY([tipoAcessoDomicilio])
REFERENCES [dbo].[TP_Acesso_Domicilio] ([codigo])
GO
ALTER TABLE [api].[CondicaoMoradia] CHECK CONSTRAINT [FK_CondicaoMoradia_TP_Acesso_Domicilio]
GO
ALTER TABLE [api].[CondicaoMoradia]  WITH CHECK ADD  CONSTRAINT [FK_CondicaoMoradia_TP_Cond_Posse_Uso_Terra] FOREIGN KEY([areaProducaoRural])
REFERENCES [dbo].[TP_Cond_Posse_Uso_Terra] ([codigo])
GO
ALTER TABLE [api].[CondicaoMoradia] CHECK CONSTRAINT [FK_CondicaoMoradia_TP_Cond_Posse_Uso_Terra]
GO
ALTER TABLE [api].[CondicaoMoradia]  WITH CHECK ADD  CONSTRAINT [FK_CondicaoMoradia_TP_Construcao_Domicilio] FOREIGN KEY([materialPredominanteParedesExtDomicilio])
REFERENCES [dbo].[TP_Construcao_Domicilio] ([codigo])
GO
ALTER TABLE [api].[CondicaoMoradia] CHECK CONSTRAINT [FK_CondicaoMoradia_TP_Construcao_Domicilio]
GO
ALTER TABLE [api].[CondicaoMoradia]  WITH CHECK ADD  CONSTRAINT [FK_CondicaoMoradia_TP_Destino_Lixo] FOREIGN KEY([destinoLixo])
REFERENCES [dbo].[TP_Destino_Lixo] ([codigo])
GO
ALTER TABLE [api].[CondicaoMoradia] CHECK CONSTRAINT [FK_CondicaoMoradia_TP_Destino_Lixo]
GO
ALTER TABLE [api].[CondicaoMoradia]  WITH CHECK ADD  CONSTRAINT [FK_CondicaoMoradia_TP_Domicilio] FOREIGN KEY([tipoDomicilio])
REFERENCES [dbo].[TP_Domicilio] ([codigo])
GO
ALTER TABLE [api].[CondicaoMoradia] CHECK CONSTRAINT [FK_CondicaoMoradia_TP_Domicilio]
GO
ALTER TABLE [api].[CondicaoMoradia]  WITH CHECK ADD  CONSTRAINT [FK_CondicaoMoradia_TP_Escoamento_Esgoto] FOREIGN KEY([formaEscoamentoBanheiro])
REFERENCES [dbo].[TP_Escoamento_Esgoto] ([codigo])
GO
ALTER TABLE [api].[CondicaoMoradia] CHECK CONSTRAINT [FK_CondicaoMoradia_TP_Escoamento_Esgoto]
GO
ALTER TABLE [api].[CondicaoMoradia]  WITH CHECK ADD  CONSTRAINT [FK_CondicaoMoradia_TP_Localizacao] FOREIGN KEY([localizacao])
REFERENCES [dbo].[TP_Localizacao] ([codigo])
GO
ALTER TABLE [api].[CondicaoMoradia] CHECK CONSTRAINT [FK_CondicaoMoradia_TP_Localizacao]
GO
ALTER TABLE [api].[CondicaoMoradia]  WITH CHECK ADD  CONSTRAINT [FK_CondicaoMoradia_TP_Situacao_Moradia] FOREIGN KEY([situacaoMoradiaPosseTerra])
REFERENCES [dbo].[TP_Situacao_Moradia] ([codigo])
GO
ALTER TABLE [api].[CondicaoMoradia] CHECK CONSTRAINT [FK_CondicaoMoradia_TP_Situacao_Moradia]
GO
ALTER TABLE [api].[CondicaoMoradia]  WITH CHECK ADD  CONSTRAINT [FK_CondicaoMoradia_TP_Tratamento_Agua] FOREIGN KEY([aguaConsumoDomicilio])
REFERENCES [dbo].[TP_Tratamento_Agua] ([codigo])
GO
ALTER TABLE [api].[CondicaoMoradia] CHECK CONSTRAINT [FK_CondicaoMoradia_TP_Tratamento_Agua]
GO
ALTER TABLE [api].[CondicoesDeSaude]  WITH CHECK ADD  CONSTRAINT [FK_CondicoesDeSaude_TP_Consideracao_Peso] FOREIGN KEY([situacaoPeso])
REFERENCES [dbo].[TP_Consideracao_Peso] ([codigo])
GO
ALTER TABLE [api].[CondicoesDeSaude] CHECK CONSTRAINT [FK_CondicoesDeSaude_TP_Consideracao_Peso]
GO
ALTER TABLE [api].[DeficienciasCidadao]  WITH CHECK ADD  CONSTRAINT [FK_DeficienciasCidadao_InformacoesSocioDemograficas] FOREIGN KEY([id_informacoes_socio_demograficas])
REFERENCES [api].[InformacoesSocioDemograficas] ([id])
GO
ALTER TABLE [api].[DeficienciasCidadao] CHECK CONSTRAINT [FK_DeficienciasCidadao_InformacoesSocioDemograficas]
GO
ALTER TABLE [api].[DeficienciasCidadao]  WITH CHECK ADD  CONSTRAINT [FK_DeficienciasCidadao_TP_Deficiencia] FOREIGN KEY([id_tp_deficiencia_cidadao])
REFERENCES [dbo].[TP_Deficiencia] ([codigo])
GO
ALTER TABLE [api].[DeficienciasCidadao] CHECK CONSTRAINT [FK_DeficienciasCidadao_TP_Deficiencia]
GO
ALTER TABLE [api].[DoencaCardiaca]  WITH CHECK ADD  CONSTRAINT [FK_DoencaCardiaca_CondicoesDeSaude] FOREIGN KEY([id_condicao_de_saude])
REFERENCES [api].[CondicoesDeSaude] ([id])
GO
ALTER TABLE [api].[DoencaCardiaca] CHECK CONSTRAINT [FK_DoencaCardiaca_CondicoesDeSaude]
GO
ALTER TABLE [api].[DoencaCardiaca]  WITH CHECK ADD  CONSTRAINT [FK_DoencaCardiaca_TP_Doenca_Cardiaca] FOREIGN KEY([id_tp_doenca_cariaca])
REFERENCES [dbo].[TP_Doenca_Cardiaca] ([codigo])
GO
ALTER TABLE [api].[DoencaCardiaca] CHECK CONSTRAINT [FK_DoencaCardiaca_TP_Doenca_Cardiaca]
GO
ALTER TABLE [api].[DoencaRespiratoria]  WITH CHECK ADD  CONSTRAINT [FK_DoencaRespiratoria_CondicoesDeSaude] FOREIGN KEY([id_condicao_de_saude])
REFERENCES [api].[CondicoesDeSaude] ([id])
GO
ALTER TABLE [api].[DoencaRespiratoria] CHECK CONSTRAINT [FK_DoencaRespiratoria_CondicoesDeSaude]
GO
ALTER TABLE [api].[DoencaRespiratoria]  WITH CHECK ADD  CONSTRAINT [FK_DoencaRespiratoria_TP_Doenca_Respiratoria] FOREIGN KEY([id_tp_doenca_respiratoria])
REFERENCES [dbo].[TP_Doenca_Respiratoria] ([codigo])
GO
ALTER TABLE [api].[DoencaRespiratoria] CHECK CONSTRAINT [FK_DoencaRespiratoria_TP_Doenca_Respiratoria]
GO
ALTER TABLE [api].[DoencaRins]  WITH CHECK ADD  CONSTRAINT [FK_DoencaRins_CondicoesDeSaude] FOREIGN KEY([id_condicao_de_saude])
REFERENCES [api].[CondicoesDeSaude] ([id])
GO
ALTER TABLE [api].[DoencaRins] CHECK CONSTRAINT [FK_DoencaRins_CondicoesDeSaude]
GO
ALTER TABLE [api].[DoencaRins]  WITH CHECK ADD  CONSTRAINT [FK_DoencaRins_TP_Doenca_Renal] FOREIGN KEY([id_tp_doenca_rins])
REFERENCES [dbo].[TP_Doenca_Renal] ([codigo])
GO
ALTER TABLE [api].[DoencaRins] CHECK CONSTRAINT [FK_DoencaRins_TP_Doenca_Renal]
GO
ALTER TABLE [api].[EmSituacaoDeRua]  WITH CHECK ADD  CONSTRAINT [FK_EmSituacaoDeRua_TP_Quantas_Vezes_Alimentacao] FOREIGN KEY([quantidadeAlimentacoesAoDiaSituacaoRua])
REFERENCES [dbo].[TP_Quantas_Vezes_Alimentacao] ([codigo])
GO
ALTER TABLE [api].[EmSituacaoDeRua] CHECK CONSTRAINT [FK_EmSituacaoDeRua_TP_Quantas_Vezes_Alimentacao]
GO
ALTER TABLE [api].[EmSituacaoDeRua]  WITH CHECK ADD  CONSTRAINT [FK_EmSituacaoDeRua_TP_Sit_Rua] FOREIGN KEY([tempoSituacaoRua])
REFERENCES [dbo].[TP_Sit_Rua] ([codigo])
GO
ALTER TABLE [api].[EmSituacaoDeRua] CHECK CONSTRAINT [FK_EmSituacaoDeRua_TP_Sit_Rua]
GO
ALTER TABLE [api].[FamiliaRow]  WITH CHECK ADD  CONSTRAINT [FK_FamiliaRow_TP_Renda_Familiar] FOREIGN KEY([rendaFamiliar])
REFERENCES [dbo].[TP_Renda_Familiar] ([codigo])
GO
ALTER TABLE [api].[FamiliaRow] CHECK CONSTRAINT [FK_FamiliaRow_TP_Renda_Familiar]
GO
ALTER TABLE [api].[Familias]  WITH CHECK ADD  CONSTRAINT [FK_Familias_CadastroDomiciliar] FOREIGN KEY([id_cadatro_domiciliar])
REFERENCES [api].[CadastroDomiciliar] ([id])
GO
ALTER TABLE [api].[Familias] CHECK CONSTRAINT [FK_Familias_CadastroDomiciliar]
GO
ALTER TABLE [api].[Familias]  WITH CHECK ADD  CONSTRAINT [FK_Familias_FamiliaRow] FOREIGN KEY([id_familia_row])
REFERENCES [api].[FamiliaRow] ([id])
GO
ALTER TABLE [api].[Familias] CHECK CONSTRAINT [FK_Familias_FamiliaRow]
GO
ALTER TABLE [api].[FichaVisitaDomiciliarChild]  WITH CHECK ADD  CONSTRAINT [FK_FichaVisitaDomiciliarChild_FichaVisitaDomiciliarMaster] FOREIGN KEY([uuidFicha])
REFERENCES [api].[FichaVisitaDomiciliarMaster] ([uuidFicha])
GO
ALTER TABLE [api].[FichaVisitaDomiciliarChild] CHECK CONSTRAINT [FK_FichaVisitaDomiciliarChild_FichaVisitaDomiciliarMaster]
GO
ALTER TABLE [api].[FichaVisitaDomiciliarChild_MotivoVisita]  WITH CHECK ADD  CONSTRAINT [FK_FichaVisitaDomiciliarChild_Motivos] FOREIGN KEY([childId])
REFERENCES [api].[FichaVisitaDomiciliarChild] ([childId])
GO
ALTER TABLE [api].[FichaVisitaDomiciliarChild_MotivoVisita] CHECK CONSTRAINT [FK_FichaVisitaDomiciliarChild_Motivos]
GO
ALTER TABLE [api].[FichaVisitaDomiciliarChild_MotivoVisita]  WITH CHECK ADD  CONSTRAINT [FK_MotivoVisita_FichaVisitaDomiciliarChildren] FOREIGN KEY([codigo])
REFERENCES [dbo].[SIGSM_MotivoVisita] ([codigo])
GO
ALTER TABLE [api].[FichaVisitaDomiciliarChild_MotivoVisita] CHECK CONSTRAINT [FK_MotivoVisita_FichaVisitaDomiciliarChildren]
GO
ALTER TABLE [api].[FichaVisitaDomiciliarMaster]  WITH CHECK ADD  CONSTRAINT [FK_FichaVisitaDomiciliarMaster_UnicaLotacaoTransport] FOREIGN KEY([headerTransport])
REFERENCES [api].[UnicaLotacaoTransport] ([id])
GO
ALTER TABLE [api].[FichaVisitaDomiciliarMaster] CHECK CONSTRAINT [FK_FichaVisitaDomiciliarMaster_UnicaLotacaoTransport]
GO
ALTER TABLE [api].[HigienePessoalSituacaoRua]  WITH CHECK ADD  CONSTRAINT [FK_HigienePessoalSituacaoRua_EmSituacaoDeRua] FOREIGN KEY([id_em_situacao_de_rua])
REFERENCES [api].[EmSituacaoDeRua] ([id])
GO
ALTER TABLE [api].[HigienePessoalSituacaoRua] CHECK CONSTRAINT [FK_HigienePessoalSituacaoRua_EmSituacaoDeRua]
GO
ALTER TABLE [api].[HigienePessoalSituacaoRua]  WITH CHECK ADD  CONSTRAINT [FK_HigienePessoalSituacaoRua_TP_Higiene_Pessoal] FOREIGN KEY([codigo_higiene_pessoal])
REFERENCES [dbo].[TP_Higiene_Pessoal] ([codigo])
GO
ALTER TABLE [api].[HigienePessoalSituacaoRua] CHECK CONSTRAINT [FK_HigienePessoalSituacaoRua_TP_Higiene_Pessoal]
GO
ALTER TABLE [api].[IdentificacaoUsuarioCidadao]  WITH CHECK ADD  CONSTRAINT [FK_IdentificacaoUsuarioCidadao_Etnia] FOREIGN KEY([etnia], [num_contrato])
REFERENCES [dbo].[Etnia] ([NumContrato], [CodEtnia])
GO
ALTER TABLE [api].[IdentificacaoUsuarioCidadao] CHECK CONSTRAINT [FK_IdentificacaoUsuarioCidadao_Etnia]
GO
ALTER TABLE [api].[IdentificacaoUsuarioCidadao]  WITH CHECK ADD  CONSTRAINT [FK_IdentificacaoUsuarioCidadao_Paises] FOREIGN KEY([paisNascimento])
REFERENCES [dbo].[Paises] ([codigo])
GO
ALTER TABLE [api].[IdentificacaoUsuarioCidadao] CHECK CONSTRAINT [FK_IdentificacaoUsuarioCidadao_Paises]
GO
ALTER TABLE [api].[IdentificacaoUsuarioCidadao]  WITH CHECK ADD  CONSTRAINT [FK_IdentificacaoUsuarioCidadao_TP_Nacionalidade] FOREIGN KEY([nacionalidadeCidadao])
REFERENCES [dbo].[TP_Nacionalidade] ([codigo])
GO
ALTER TABLE [api].[IdentificacaoUsuarioCidadao] CHECK CONSTRAINT [FK_IdentificacaoUsuarioCidadao_TP_Nacionalidade]
GO
ALTER TABLE [api].[IdentificacaoUsuarioCidadao]  WITH CHECK ADD  CONSTRAINT [FK_IdentificacaoUsuarioCidadao_TP_Raca_Cor] FOREIGN KEY([racaCorCidadao])
REFERENCES [dbo].[TP_Raca_Cor] ([id_tp_raca_cor])
GO
ALTER TABLE [api].[IdentificacaoUsuarioCidadao] CHECK CONSTRAINT [FK_IdentificacaoUsuarioCidadao_TP_Raca_Cor]
GO
ALTER TABLE [api].[IdentificacaoUsuarioCidadao]  WITH CHECK ADD  CONSTRAINT [FK_IdentificacaoUsuarioCidadao_TP_Sexo] FOREIGN KEY([sexoCidadao])
REFERENCES [dbo].[TP_Sexo] ([codigo])
GO
ALTER TABLE [api].[IdentificacaoUsuarioCidadao] CHECK CONSTRAINT [FK_IdentificacaoUsuarioCidadao_TP_Sexo]
GO
ALTER TABLE [api].[InformacoesSocioDemograficas]  WITH CHECK ADD  CONSTRAINT [FK_InformacoesSocioDemograficas_AS_ProfissoesTab] FOREIGN KEY([ocupacaoCodigoCbo2002])
REFERENCES [dbo].[AS_ProfissoesTab] ([CodProfTab])
GO
ALTER TABLE [api].[InformacoesSocioDemograficas] CHECK CONSTRAINT [FK_InformacoesSocioDemograficas_AS_ProfissoesTab]
GO
ALTER TABLE [api].[InformacoesSocioDemograficas]  WITH CHECK ADD  CONSTRAINT [FK_InformacoesSocioDemograficas_TP_Curso] FOREIGN KEY([grauInstrucaoCidadao])
REFERENCES [dbo].[TP_Curso] ([codigo])
GO
ALTER TABLE [api].[InformacoesSocioDemograficas] CHECK CONSTRAINT [FK_InformacoesSocioDemograficas_TP_Curso]
GO
ALTER TABLE [api].[InformacoesSocioDemograficas]  WITH CHECK ADD  CONSTRAINT [FK_InformacoesSocioDemograficas_TP_Identidade_Genero_Cidadao] FOREIGN KEY([identidadeGeneroCidadao])
REFERENCES [dbo].[TP_Identidade_Genero_Cidadao] ([codigo])
GO
ALTER TABLE [api].[InformacoesSocioDemograficas] CHECK CONSTRAINT [FK_InformacoesSocioDemograficas_TP_Identidade_Genero_Cidadao]
GO
ALTER TABLE [api].[InformacoesSocioDemograficas]  WITH CHECK ADD  CONSTRAINT [FK_InformacoesSocioDemograficas_TP_Orientacao_Sexual] FOREIGN KEY([orientacaoSexualCidadao])
REFERENCES [dbo].[TP_Orientacao_Sexual] ([codigo])
GO
ALTER TABLE [api].[InformacoesSocioDemograficas] CHECK CONSTRAINT [FK_InformacoesSocioDemograficas_TP_Orientacao_Sexual]
GO
ALTER TABLE [api].[InformacoesSocioDemograficas]  WITH CHECK ADD  CONSTRAINT [FK_InformacoesSocioDemograficas_TP_Relacao_Parentesco] FOREIGN KEY([relacaoParentescoCidadao])
REFERENCES [dbo].[TP_Relacao_Parentesco] ([codigo])
GO
ALTER TABLE [api].[InformacoesSocioDemograficas] CHECK CONSTRAINT [FK_InformacoesSocioDemograficas_TP_Relacao_Parentesco]
GO
ALTER TABLE [api].[InformacoesSocioDemograficas]  WITH CHECK ADD  CONSTRAINT [FK_InformacoesSocioDemograficas_TP_Sit_Mercado] FOREIGN KEY([situacaoMercadoTrabalhoCidadao])
REFERENCES [dbo].[TP_Sit_Mercado] ([codigo])
GO
ALTER TABLE [api].[InformacoesSocioDemograficas] CHECK CONSTRAINT [FK_InformacoesSocioDemograficas_TP_Sit_Mercado]
GO
ALTER TABLE [api].[OrigemAlimentoSituacaoRua]  WITH CHECK ADD  CONSTRAINT [FK_OrigemAlimentoSituacaoRua_EmSituacaoDeRua] FOREIGN KEY([id_em_situacao_rua])
REFERENCES [api].[EmSituacaoDeRua] ([id])
GO
ALTER TABLE [api].[OrigemAlimentoSituacaoRua] CHECK CONSTRAINT [FK_OrigemAlimentoSituacaoRua_EmSituacaoDeRua]
GO
ALTER TABLE [api].[OrigemAlimentoSituacaoRua]  WITH CHECK ADD  CONSTRAINT [FK_OrigemAlimentoSituacaoRua_TP_Origem_Alimentacao] FOREIGN KEY([id_tp_origem_alimento])
REFERENCES [dbo].[TP_Origem_Alimentacao] ([codigo])
GO
ALTER TABLE [api].[OrigemAlimentoSituacaoRua] CHECK CONSTRAINT [FK_OrigemAlimentoSituacaoRua_TP_Origem_Alimentacao]
GO
ALTER TABLE [api].[ResponsavelPorCrianca]  WITH CHECK ADD  CONSTRAINT [FK_ResponsavelPorCrianca_InformacoesSocioDemograficas] FOREIGN KEY([id_informacoes_sociodemograficas])
REFERENCES [api].[InformacoesSocioDemograficas] ([id])
GO
ALTER TABLE [api].[ResponsavelPorCrianca] CHECK CONSTRAINT [FK_ResponsavelPorCrianca_InformacoesSocioDemograficas]
GO
ALTER TABLE [api].[ResponsavelPorCrianca]  WITH CHECK ADD  CONSTRAINT [FK_ResponsavelPorCrianca_TP_Crianca] FOREIGN KEY([id_tp_crianca])
REFERENCES [dbo].[TP_Crianca] ([codigo])
GO
ALTER TABLE [api].[ResponsavelPorCrianca] CHECK CONSTRAINT [FK_ResponsavelPorCrianca_TP_Crianca]
GO
ALTER TABLE [api].[SaidaCidadaoCadastro]  WITH CHECK ADD  CONSTRAINT [FK_SaidaCidadaoCadastro_TP_Motivo_Saida] FOREIGN KEY([motivoSaidaCidadao])
REFERENCES [dbo].[TP_Motivo_Saida] ([codigo])
GO
ALTER TABLE [api].[SaidaCidadaoCadastro] CHECK CONSTRAINT [FK_SaidaCidadaoCadastro_TP_Motivo_Saida]
GO
ALTER TABLE [api].[UnicaLotacaoTransport]  WITH CHECK ADD  CONSTRAINT [FK_UnicaLotacaoTransport_OrigemVisita] FOREIGN KEY([token])
REFERENCES [api].[OrigemVisita] ([token])
GO
ALTER TABLE [api].[UnicaLotacaoTransport] CHECK CONSTRAINT [FK_UnicaLotacaoTransport_OrigemVisita]
GO
/****** Object:  StoredProcedure [api].[PR_ProcessarFichasAPI]    Script Date: 07/03/2017 06:34:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [api].[PR_ProcessarFichasAPI] (@token uniqueidentifier)
AS
BEGIN
BEGIN TRANSACTION
BEGIN TRY
	IF (@token IS NULL)
	BEGIN
		PRINT '';
		THROW 60000, 'Token inválido.', 1;
	END

	UPDATE [api].[OrigemVisita] SET finalizado = 1 WHERE token = @token;

	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION;
	THROW;
END CATCH
END
GO
