SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE ASSMED_Contratos
		ADD CodigoIbgeMunicipio CHAR(7)
GO

DECLARE @codigoIbge NCHAR(7) = '3547304'

UPDATE ASSMED_Contratos SET CodigoIbgeMunicipio = @codigoIbge
GO

ALTER TABLE Nacionalidade ADD codigo int
GO
CREATE NONCLUSTERED INDEX [IX_Nacionalidade] ON [dbo].[Nacionalidade] 
(
	[codigo] ASC
) ON [PRIMARY]
GO

IF EXISTS (select 1 from sys.triggers where object_id = OBJECT_ID('[api].[TG_RastroFicha_ProcessarFicha]'))
	DROP TRIGGER [api].[TG_RastroFicha_ProcessarFicha]
GO

UPDATE ASSMED_CadastroPF SET EstCivil = 'I' WHERE EstCivil IS NULL OR EstCivil = ''
GO
UPDATE ASSMED_PesFisica SET EstCivil = 'I' WHERE EstCivil IS NULL OR EstCivil = ''
GO

alter table Atendimento_Individual_Usuario add cid10_02 nvarchar(10)
GO
alter table Atendimento_Individual_Usuario add RACIONALIDADE int
GO
alter table Atendimento_Individual_Usuario add PERIMETROCEFALICO DECIMAL(10,4)
GO

alter table SIGSM_Atividade_Coletiva_Usuario add SEXO int
GO
insert into SIGSM_Praticas_Temas_Para_Saude(id,nome)values(29,'Ações de combate ao Aedes aegypti')
GO
alter table SIGSM_Atividade_Coletiva add CodigoCnesUnidadeAtividade int
GO

update menu set descricao='Visita Domiciliar e Territorial' where id_menu=13
GO

alter table SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente add Turno int
GO
alter table SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente add nome_completo_pai varchar(50)
GO
alter table SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente add NAC_BRA_MUNICIPIONASC int
GO
alter table SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente add NAC_NAT_PORTARIA varchar(50)
GO
alter table SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente add NAC_NAT_DATA date
GO
alter table SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente add NAC_EST_PAIS int
GO
alter table SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente add NAC_EST_DATA date
GO
alter table SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente add ETNIA int
GO
alter table SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente add CNS_CUIDADOR varchar(15)
GO
alter table SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente add PontoReferencia varchar(256)
GO

INSERT INTO Paises VALUES
(239, 'ILHAS GUERNSEY'),
(240, 'JERSEY'),
(241, 'MONTENEGRO'),
(242, 'ESTADO DA PALESTINA'),
(243, 'SÉRVIA')
GO

INSERT INTO TB_MS_TIPO_LOGRADOURO VALUES
(738, 'ANEL VIÁRIO', 'ANEL-V', 738),
(739, 'SERVIDÃO DE PASSAGEM', 'SERV-PAS', 739),
(740, '17ª TRAVESSA', '17A TV', 740),
(741, 'ANTIGA ESTAÇÃO', 'ANTEST', 741),
(742, '20ª TRAVESSA', '20A TV', 742),
(743, '21ª TRAVESSA', '21A TV', 743),
(744, '22ª TRAVESSA', '22A TV', 744),
(745, 'CONJUNTO RESIDENCIAL', 'CJ-RES', 745),
(746, '2ª ALAMEDA', '2A AL', 746),
(747, 'VARIANTE DA ESTRADA', 'VAR-EST', 747),
(748, 'VIA MARGINAL', 'VIA-MARG', 748),
(749, 'MÓDULO COMERCIAL', 'MOD-COM', 749),
(750, 'NOVA AVENIDA', 'NV-AV', 750),
(751, 'GLEBA', 'GLEBA', 751)
GO

CREATE TABLE dbo.SIGSM_Turno (
	id int not null primary key,
	descricao varchar(15) not null,
	sigla char(1) not null
)
GO

INSERT INTO dbo.SIGSM_Turno VALUES (1, 'Manhã', 'M'), (2, 'Tarde', 'T'), (3, 'Noite', 'N')
GO

ALTER TABLE TP_Sexo ADD sigla char(1)
GO

UPDATE TP_Sexo SET sigla = SUBSTRING(descricao, 0, 1)
GO

CREATE TABLE dbo.SIGSM_LocalDeAtendimento (
	id int not null primary key,
	descricao varchar(60) not null,
	observacao varchar(255) null
)
GO

INSERT INTO dbo.SIGSM_LocalDeAtendimento VALUES (1, 'UBS', null),
(2, 'Unidade móvel', null),
(3, 'Rua', null),
(4, 'Domicílio', null),
(5, 'Escola / Creche', null),
(6, 'Outros', null),
(7, 'Polo (academia da saúde)', null),
(8, 'Instituição / Abrigo', null),
(9, 'Unidade prisional ou congêneres', null),
(10, 'Unidade socioeducativa', null),
(11, 'Hospital', 'Utilizado apenas na Ficha de Atendimento Domiciliar'),
(12, 'Unidade de pronto atendimento', 'Utilizado apenas na Ficha de Atendimento Domiciliar'),
(13, 'CACON / UNACON', 'Utilizado apenas na Ficha de Atendimento Domiciliar')
GO

CREATE TABLE dbo.SIGSM_ModalidadeAD (
	id int not null primary key,
	descricao varchar(60) not null,
	observacao varchar(255) null
)
GO

INSERT INTO dbo.SIGSM_ModalidadeAD VALUES (1, 'AD1', null),
(2, 'AD2', null),
(3, 'AD3', null),
(4, 'Inelegivel', 'Utilizado apenas na Ficha de Avaliação de Elegibilidade')
GO

ALTER TABLE dbo.Tipo_Atendimento ADD observacao varchar(255) null
GO
ALTER TABLE dbo.Tipo_Atendimento ALTER COLUMN tp_sub_grupos CHAR(2) null
GO

SET IDENTITY_INSERT [dbo].[Tipo_Atendimento] ON

INSERT INTO dbo.Tipo_Atendimento (id_tipo_atendimento, descricao, tp_sub_grupos, observacao) VALUES (7, 'Atendimento programado', 'AD', 'Utilizado apenas nas Fichas de Atendimento Domiciliar'),
(8, 'Atendimento não programado', 'AD', 'Utilizado apenas nas Fichas de Atendimento Domiciliar'),
(9, 'Visita domiciliar pós-óbito', 'AD', 'Utilizado apenas nas Fichas de Atendimento Domiciliar')

select id_tipo_atendimento, descricao, tp_sub_grupos, observacao from dbo.Tipo_Atendimento

SET IDENTITY_INSERT [dbo].[Tipo_Atendimento] OFF
GO

CREATE TABLE dbo.SIGSM_Atendimento_Domiciliar_SIGTAP (
	id varchar(15) NOT NULL primary key
)
GO

INSERT INTO dbo.SIGSM_Atendimento_Domiciliar_SIGTAP (id) VALUES ('0301070024'),
('0301050082'), ('0301070075'), ('0302040021'), ('0301050090'), ('0301070067'),
('0301100047'), ('0301100055'), ('0201020041'), ('0301100063'), ('0301100071'),
('0301100098'), ('0301100144'), ('0301100152'), ('0301100179'), ('0301100187'),
('0301050120'), ('0301070113'), ('0308010019'), ('0303190019')
GO

ALTER TABLE dbo.SIGSM_Domiciliar_Conduta_Motivo ADD codigo int
GO

UPDATE dbo.SIGSM_Domiciliar_Conduta_Motivo SET codigo = 1 WHERE Id_conduta = 2;
UPDATE dbo.SIGSM_Domiciliar_Conduta_Motivo SET codigo = 2 WHERE Id_conduta = 3;
UPDATE dbo.SIGSM_Domiciliar_Conduta_Motivo SET codigo = 3 WHERE Id_conduta = 1;
UPDATE dbo.SIGSM_Domiciliar_Conduta_Motivo SET codigo = 4 WHERE Id_conduta = 4;
UPDATE dbo.SIGSM_Domiciliar_Conduta_Motivo SET codigo = 5 WHERE Id_conduta = 5;
UPDATE dbo.SIGSM_Domiciliar_Conduta_Motivo SET codigo = 9 WHERE Id_conduta = 6;
insert into dbo.SIGSM_Domiciliar_Conduta_Motivo VALUES ('Permanência', 7);
GO

-- =============================================
-- Author:		Elton Schivei Costa
-- Create date: 2017-08-02
-- Description:	Converte string list de condições para uma string list com os código do esus
-- =============================================
CREATE FUNCTION dbo.FN_ParseCondicoesAvaliadas(@condicoes nvarchar(max))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @RESULT NVARCHAR(MAX)
	DECLARE @REST TABLE([Value] nvarchar(max))
	
	SELECT @RESULT = '<d>' + REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(@condicoes, '&', '&amp;'), '<', '&lt;'), '>', '&gt;'), '"', '&quot;'), '''', '&apos;'), ',', '</d><d>') + '</d>';

    DECLARE @TextXml XML;
    SELECT @TextXml = CAST(@RESULT AS XML);

	;WITH dd AS (
		SELECT REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(RTRIM(LTRIM(T.split.value('.', 'nvarchar(max)'))), '&amp;', '&'), '&lt;', '<'), '&gt;', '>'), '&quot;', '"'), '&apos;', '''') AS [Value]
		FROM @TextXml.nodes('/d') T(Split)
	)
	INSERT INTO @REST
	SELECT sdc.Id_condicoes
	  FROM dd
INNER JOIN SIGSM_Domiciliar_Condicoes AS sdc
		ON LTRIM(RTRIM(dd.[Value])) = CAST(sdc.Id_condicoes AS NVARCHAR)

	SELECT @RESULT = STUFF((
		SELECT ',' + r.[Value] FROM @REST AS r FOR XML PATH(''), TYPE
	).value('.', 'varchar(max)'), 1, 1, '')

	-- Return the result of the function
	RETURN @RESULT
END
GO

CREATE VIEW dbo.VW_FichaAtendimentoDomiciliarMaster AS
 select id as Id,
        3 as TpCdsOrigem,
        guid_esus as UuidFicha,
		CAST((CASE enviado WHEN 1 THEN 1 ELSE 0 END) AS BIT) as Enviado,
		data_atendimento AS DataAtendimento
    from SIGSM_Atendimento_Domiciliar
    where enviado = 0 or enviado is null
GO

CREATE VIEW dbo.VW_FichaAtendimentoDomiciliar_HeaderTransport AS
 select atendDomiciliar.guid_esus as UuidFicha,
	    ltrim(rtrim(dpe.Numero)) as ProfissionalCNS,
	    rtrim(ltrim(pro.cbo)) as CboCodigo_2002,
	    atendDomiciliar.codigo_cnes_unidade as Cnes,
	    snes.Numero as [Ine],
	    atendDomiciliar.data_atendimento as DataAtendimento,
	    COALESCE(pro.codibge_munic_nasc, cont.CodigoIbgeMunicipio) as CodigoIbgeMunicipio,
		atendDomiciliar.guid_esus AS Uuid_AtendimentoDomiciliar,
		atendDomiciliar.id As Id_AtendimentoDomiciliar
    from SIGSM_Atendimento_Domiciliar_Profissional pro
    left outer join ASSMED_Cadastro cad on pro.id_profissional_saude = cad.Codigo
    left join ASSMED_Contratos cont on cad.NumContrato = cont.NumContrato
    left outer join SIGSM_Atendimento_Domiciliar atendDomiciliar on atendDomiciliar.id = pro.id_atendimento_domiciliar
    INNER JOIN ASSMED_CadastroDocPessoal dpe on dpe.NumContrato = cad.NumContrato AND dpe.Codigo = cad.Codigo AND dpe.CodTpDocP = 6
    left join SetoresInes snes on (atendDomiciliar.codigo_equipe_ine = snes.CodINE)
GO

CREATE VIEW dbo.VW_FichaAtendimentoDomiciliarChild AS
 select ate.guid_esus as uuidFicha,
        tur.id AS turno,
        rtrim(ltrim(dpe.Numero)) as cnsCidadao,
	    pac.data_nascimento as dataNascimento,
	    sex.codigo AS sexo,
	    pac.local_atendimento as localDeAtendimento,
        mad.id AS atencaoDomiciliarModalidade,
	    ta.id_tipo_atendimento as tipoAtendimento,
	    dbo.FN_ParseCondicoesAvaliadas(pac.condicao) as condicoes,
		pac.cid as cid,
	    pac.ciap as ciap,                  
        sdcm.codigo as condutaDesfecho,
		ate.guid_esus AS uuidAtendimentoDomiciliar,
		ate.id As idAtendimentoDomiciliar,
		STUFF((
			select ',' + sadp.sigtap from SIGSM_Atendimento_Domiciliar_Procedimentos sadp
		     where sadp.sigtap IN (SELECT sigtap FROM dbo.SIGSM_Atendimento_Domiciliar_SIGTAP)
			 and sadp.id_atendimento = ate.id
			   FOR XML PATH(''), TYPE
		).value('.', 'varchar(max)'), 1, 1, '') AS procs,
		STUFF((
			select ',' + sadp.sigtap from SIGSM_Atendimento_Domiciliar_Procedimentos sadp
			 where sadp.sigtap NOT IN (SELECT sigtap FROM dbo.SIGSM_Atendimento_Domiciliar_SIGTAP)
			 and sadp.id_atendimento = ate.id
			   FOR XML PATH(''), TYPE
		).value('.', 'varchar(max)'), 1, 1, '') AS oprocs
    from SIGSM_Atendimento_Domiciliar_Paciente pac
	    left outer join SIGSM_Atendimento_Domiciliar ate on pac.id_atendimento_domiciliar = ate.id
	    left outer join ASSMED_Cadastro cad on pac.id_identificacao_usuario = cad.Codigo
	    left outer join ASSMED_PesFisica fis on cad.Codigo = fis.Codigo and cad.NumContrato = fis.NumContrato
	    INNER JOIN ASSMED_CadastroDocPessoal dpe on dpe.NumContrato = cad.NumContrato AND dpe.Codigo = cad.Codigo AND dpe.CodTpDocP = 6	
		left join dbo.SIGSM_Turno AS tur ON pac.turno = tur.sigla
		left join dbo.TP_Sexo AS sex ON pac.sexo = sex.sigla
		left join dbo.SIGSM_ModalidadeAD AS mad ON pac.modalidade_ad = mad.descricao
		left join dbo.Tipo_Atendimento AS ta ON (6 + pac.id_tipo_atendimento) = ta.id_tipo_atendimento
		left join dbo.SIGSM_Domiciliar_Conduta_Motivo AS sdcm ON pac.conduta_motivo_saida = sdcm.Id_conduta
    where ate.id_status = 2

GO

CREATE VIEW dbo.VW_FichaAtendimentoIndividualMaster AS
 select id as Id,
        guid_esus as UuidFicha,
        3 as TpCdsOrigem,
        data_atendimento as DataAtendimento,
        CAST(CASE enviado WHEN 1 THEN 1 ELSE 0 END AS BIT) as Enviado
    from Atendimento_Individual
    where  (enviado = 0 or enviado is null) and id_status = 2
GO
CREATE VIEW dbo.VW_FichaAtendimentoIndividual_HeaderTransport AS
 select atendIndividual.guid_esus as UuidFicha,
	    ltrim(rtrim(dpe.Numero)) as ProfissionalCNS,
	    rtrim(ltrim(pro.cbo)) as CboCodigo_2002,
	    atendIndividual.codigo_cnes_unidade as Cnes,
	    snes.Numero as Ine,
	    atendIndividual.data_atendimento as DataAtendimento,
	    pro.codibge_munic_nasc as CodigoIbgeMunicipio,
		atendIndividual.guid_esus AS Uuid_AtendimentoDomiciliar,
		atendIndividual.id As Id_AtendimentoDomiciliar
    from Atendimento_Individual_Profissional_Saude pro
    left outer join ASSMED_Cadastro cad on pro.id_profissional_saude = cad.Codigo
    left outer join Atendimento_Individual atendIndividual on atendIndividual.id = pro.id_atendimento_individual
    INNER JOIN ASSMED_CadastroDocPessoal dpe on dpe.NumContrato = cad.NumContrato AND dpe.Codigo = cad.Codigo AND dpe.CodTpDocP = 6
    left join SetoresInes snes on (atendIndividual.codigo_equipe_ine = snes.CodINE)
GO

CREATE TABLE dbo.SIGSM_Atendimento_Individual_ExamesAB (
	sigtap char(10),
	descricao varchar(255) not null,
	codigoAB char(7) not null primary key
)
GO
INSERT INTO dbo.SIGSM_Atendimento_Individual_ExamesAB VALUES
('0202010295', 'Colesterol total', 'ABEX002'),
('0202010317', 'Creatinina', 'ABEX003'),
('0202050017', 'EAS / EQU', 'ABEX027'),
('0211020036', 'Eletrocardiograma', 'ABEX004'),
('0202020355', 'Eletroforese de Hemoglobina', 'ABEX030'),
('0211080055', 'Espirometria', 'ABEX005'),
('0202080110', 'Exame de escarro', 'ABEX006'),
('0202010473', 'Glicemia', 'ABEX026'),
('0202010279', 'HDL', 'ABEX007'),
('0202010503', 'Hemoglobina glicada', 'ABEX008'),
('0202020380', 'Hemograma', 'ABEX028'),
('0202010287', 'LDL', 'ABEX009'),
(NULL, 'Retinografia/Fundo de olho com oftalmologista', 'ABEX013'),
('0202031110', 'Sorologia de Sífilis (VDRL)', 'ABEX019'),
('0202030903', 'Sorologia para Dengue', 'ABEX016'),
('0202030300', 'Sorologia para HIV', 'ABEX018'),
('0202120090', 'Teste indireto de antiglobulina humana (TIA)', 'ABEX031'),
('0211070149', 'Teste da orelhinha', 'ABEX020'),
('0202060217', 'Teste de gravidez', 'ABEX023'),
(NULL, 'Teste do olhinho', 'ABEX022'),
('0202110052', 'Teste do pezinho', 'ABEX021'),
('0205020143', 'Ultrassonografia obstétrica', 'ABEX024'),
('0202080080', 'Urocultura', 'ABEX029')
GO

ALTER TABLE dbo.Atendimento_Individual_Exames ADD codigoAB char(7)
GO

CREATE TABLE dbo.SIGSM_Atendiomento_Individual_Racionalidade (
	id int not null primary key,
	descricao varchar(60) not null
)
GO

INSERT INTO dbo.SIGSM_Atendiomento_Individual_Racionalidade VALUES
(1, 'Medicina Tradicional Chinesa'),
(2, 'Antroposofia aplicada à saúde'),
(3, 'Homeopatia'),
(4, 'Fitoterapia'),
(5, 'Ayurveda'),
(6, 'Outra')
GO

CREATE VIEW dbo.VW_FichaAtendimentoIndividualChild AS
 select usu.id as Id,
        ate.guid_esus as UuidFicha,
        usu.numero_prontuario as NumeroProntuario,
        usu.numero_cartao_sus as Cns,
        usu.data_nascimento as DataNascimento ,
        usu.local_atendimento as LocalDeAtendimento,
		sex.codigo AS Sexo,
        tur.id AS Turno,
        ta.id_tipo_atendimento as TipoAtendimento,
        usu.peso as PesoAcompanhamentoNutricional,
        usu.altura as AlturaAcompanhamentoNutricional,
        usu.crianca_aleitamento_materno as AleitamentoMaterno,
        usu.gestante_dum as DumDaGestante,
        usu.gestante_idade_gestacional as IdadeGestacional,
        mad.id As AtencaoDomiciliarModalidade,
		NULL AS ProblemaCondicaoAvaliada,
		STUFF((
			select ',' + saieab.codigoAB from Atendimento_Individual_Exames AS aie
			inner join SIGSM_Atendimento_Individual_ExamesAB AS saieab
			on aie.codigoAB = saieab.codigoAB
			WHERE aie.solicitado = 'S' and aie.id_atendimento = ate.id
			and aie.id_atendimento_usuario = usu.id
			   FOR XML PATH(''), TYPE
		).value('.', 'varchar(max)'), 1, 1, '') AS ExaSolicitados,
		STUFF((
			select ',' + saieab.codigoAB from Atendimento_Individual_Exames AS aie
			inner join SIGSM_Atendimento_Individual_ExamesAB AS saieab
			on aie.codigoAB = saieab.codigoAB
			WHERE aie.avaliado = 'A' and aie.id_atendimento = ate.id
			and aie.id_atendimento_usuario = usu.id
			   FOR XML PATH(''), TYPE
		).value('.', 'varchar(max)'), 1, 1, '') AS ExaAvaliados,
		CAST(CASE usu.vacinacao_em_dia WHEN 'Sim' THEN 1 ELSE 0 END AS BIT) as VacinaEmDia,
        CAST(CASE usu.ficou_em_observacao WHEN 'Sim' THEN 1 ELSE 0 END AS BIT) as FicouEmObservacao,
		STUFF((SELECT ','+CAST(nuasfs AS NVARCHAR(MAX)) FROM 
				Atendimento_Individual_Usuario u
				CROSS APPLY (
					VALUES
					(CASE WHEN ( avaliacao_diagnostico = 1 ) THEN 1 END),
					(CASE WHEN ( procedimentos_clinicos_terapeuticos = 1 ) THEN 2 END),
					(CASE WHEN ( prescricao_terapeutica = 1 ) THEN 3 END)
				) c ([nuasfs])
				WHERE u.id = usu.id
				FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, '') AS Nuasfs,
		STUFF((SELECT ','+CAST(condutas AS NVARCHAR(MAX)) FROM 
				Atendimento_Individual_Usuario u
				CROSS APPLY (
					VALUES
					(CASE WHEN ( retorno_para_consulta_agendada = 1 ) THEN 1 END),
					(CASE WHEN ( retorno_para_cuidado_continuado_programado = 1 ) THEN 2 END),
					(CASE WHEN ( agendamento_para_grupos = 1 ) THEN 12 END),
					(CASE WHEN ( agendamento_para_nasf = 1 ) THEN 3 END),
					(CASE WHEN ( alta_do_episodio = 1 ) THEN 9 END),
					(CASE WHEN ( encaminhamento_interno_no_dia = 1 ) THEN 11 END),
					(CASE WHEN ( encaminhamento_para_servico_especializado = 1 ) THEN 4 END),
					(CASE WHEN ( encaminhamento_para_caps = 1 ) THEN 5 END),
					(CASE WHEN ( encaminhamento_para_internacao_hospitalar = 1 ) THEN 6 END),
					(CASE WHEN ( encaminhamento_para_urgencia = 1 ) THEN 7 END),
					(CASE WHEN ( encaminhamento_para_servico_de_atencao_domiciliar = 1 ) THEN 8 END),
					(CASE WHEN ( encaminhamento_intersetorial = 1 ) THEN 10 END)
				) c ([condutas])
				WHERE u.id = usu.id
				FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, '') as condutas,
        CAST(CASE usu.gestante_gravidez_planejada WHEN 'Sim' THEN 1 ELSE 0 END AS BIT) AS StGravidezPlanejada,
        usu.gestante_partos as NuPartos,
        usu.gestante_gestas_previas as NuGestasPrevias,
        usu.racionalidade as RacionalidadeSaude,
		usu.perimetroCefalico as PerimetroCefalico
from Atendimento_Individual_Usuario usu left join Atendimento_Individual ate    on ( usu.id_atendimento_individual = ate.id )
                                        left join ASSMED_Cadastro cad           on ( usu.id_identificacao_usuario = cad.Codigo )
                                        left join ASSMED_CadastroDocPessoal dpe on ( usu.NumContrato = dpe.NumContrato )
																			    and dpe.Codigo = cad.Codigo
																			    and dpe.CodTpDocP = 6
										left join TP_Sexo sex on usu.sexo = sex.sigla
										left join SIGSM_Turno tur on usu.turno = tur.sigla
		left join dbo.SIGSM_ModalidadeAD AS mad ON usu.modalidade_ad = mad.descricao
		left join dbo.Tipo_Atendimento AS ta ON (6 + usu.id_tipo_atendimento) = ta.id_tipo_atendimento
where ate.id_status = 2
GO

CREATE VIEW VW_FichaAtendimentoIndividual_OutrosSia AS
	select distinct ate.id as IdAtendimento,
	                ate.guid_esus as UuidFicha,
	                exame.id_atendimento_usuario as IdPaciente,
	                tb.CodProcTab as CodigoExame,
	                LTRIM(RTRIM(p.DesProc)) as Nome,
	                CASE WHEN LTRIM(RTRIM(exame.solicitado)) = 'S' THEN 'S'
					     WHEN LTRIM(RTRIM(exame.avaliado)) = 'A' THEN 'A'
						 ELSE NULL END as solicitadoAvaliado
                from
	                AE_Tabelas t left join AS_TabelaProc tb on ( t.NumContrato = tb.NumContrato )
	                                                        and ( t.CodTab = tb.CodTab )
				                    left join AS_Procedimentos p    on ( p.NumContrato = tb.NumContrato )
				                                                    and ( p.CodProc = tb.CodProc )
				                    left join Atendimento_Individual_Exames exame on ( rtrim(ltrim(p.DesProc)) = rtrim(ltrim(exame.descricao) COLLATE Latin1_General_CI_AI))
				                    left join Atendimento_Individual ate          on ( ate.id = exame.id_atendimento )
                where ate.id_status = 2
                    and p.NumContrato = tb.NumContrato
                    and p.CodProc=tb.CodProc
                    and t.NumContrato = tb.NumContrato
                    and t.CodTab = tb.CodTab
                    and p.DesProc like '%' + Convert(varchar, exame.descricao) COLLATE Latin1_General_CI_AI + '%'
                    and ate.id = exame.id_atendimento
GO

UPDATE TP_Condicao_Avaliada SET codigo = c.codigoAB
FROM TP_Condicao_Avaliada ca
CROSS APPLY (
	VALUES
		('R96', 'Asma', 'ABP009'),
		('A77', 'Dengue', 'ABP019'),
		('T91', 'Desnutrição', 'ABP008'),
		('T90', 'Diabetes', 'ABP006'),
		('R95', 'DPOC', 'ABP010'),
		('A78', 'DST', 'ABP020'),
		('A78', 'Hanseníase', 'ABP018'),
		('K86', 'Hipertensão Arterial', 'ABP005'),
		('T82', 'Obesidade', 'ABP007'),
		('W78', 'Pré-natal', 'ABP001'),
		('A98', 'Puericultura', 'ABP004'),
		('W18', 'Puerpério (até 42 dias)', 'ABP002'),
		('Não possui', 'Câncer de Mama', 'ABP023'),
		('Não possui', 'Câncer do Colo do Útero', 'ABP022'),
		('K22', 'Risco cardiovascular', 'ABP024'),
		('A57', 'Reabilitação', 'ABP015'),
		('Não possui', 'Saúde Mental', 'ABP014'),
		('Não possui', 'Saúde Sexual e Reprodutiva', 'ABP003'),
		('P17', 'Tabagismo', 'ABP011'),
		('A70', 'Tuberculose', 'ABP017'),
		('P16', 'Usuário de álcool', 'ABP012'),
		('P19', 'Usuário de outras drogas', 'ABP013')
) c ([ciap2], [nome], [codigoAB])
WHERE c.nome = ca.descricao
GO

CREATE VIEW VW_FichaAtendimentoIndividual_ProblemaCondicaoAvaliacaoAI AS
 select ate.id as IdAtendimentoIndividual,
        ate.guid_esus as UuidFicha, 
		STUFF((SELECT ','+ca.codigo FROM 
			Atendimento_Individual_Usuario u
			CROSS APPLY (
				VALUES
				(CASE WHEN ( asma = 1 ) THEN 1 END),
				(CASE WHEN ( dengue = 1 ) THEN 18 END),
				(CASE WHEN ( desnutricao = 1 ) THEN 2 END),
				(CASE WHEN ( diabetes = 1 ) THEN 3 END),
				(CASE WHEN ( dpoc = 1 ) THEN 4 END),
				(CASE WHEN ( dst = 1 ) THEN 19 END),
				(CASE WHEN ( hanseniase = 1 ) THEN 17 END),
				(CASE WHEN ( hipertensao_arterial = 1 ) THEN 5 END),
				(CASE WHEN ( obesidade = 1 ) THEN 6 END),
				(CASE WHEN ( pre_natal = 1 ) THEN 7 END),
				(CASE WHEN ( puericultura = 1 ) THEN 8 END),
				(CASE WHEN ( puerperio = 1 ) THEN 9 END),
				(CASE WHEN ( cancer_mama = 1 ) THEN 21 END),
				(CASE WHEN ( cancer_colo_utero = 1 ) THEN 20 END),
				(CASE WHEN ( risco_cardiovascular = 1 ) THEN 22 END),
				(CASE WHEN ( reabilitacao = 1 ) THEN 15 END),
				(CASE WHEN ( saude_mental = 1 ) THEN 14 END),
				(CASE WHEN ( saude_sexual_reprodutiva = 1 ) THEN 10 END),
				(CASE WHEN ( tabagismo = 1 ) THEN 11 END),
				(CASE WHEN ( tuberculose = 1 ) THEN 16 END),
				(CASE WHEN ( usuario_alcool = 1 ) THEN 12 END),
				(CASE WHEN ( usuario_outras_drogas = 1 ) THEN 13 END)
			) c (condicoes)
			INNER JOIN TP_Condicao_Avaliada ca ON condicoes = ca.Id_tp_condicao_avaliada
			WHERE u.id = usu.id
			FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, '') as condicoes,
        usu.id as IdPaciente, 
	    rtrim(ltrim(usu.ciap2_01)) as OutroCiap1,
	    rtrim(ltrim(usu.ciap2_02)) as OutroCiap2,
	    rtrim(ltrim(usu.cid10_01)) as Cid10_01,
	    rtrim(ltrim(usu.cid10_02)) as Cid10_02
    from 	
        Atendimento_Individual_Usuario usu left join Atendimento_Individual ate on (usu.id_atendimento_individual = ate.id )
    where ate.id_status = 2 and
        usu.id_atendimento_individual = ate.id
GO

INSERT INTO SIGSM_Odontologico_Conduta VALUES ('Alta do episódio', NULL, NULL)
GO

CREATE VIEW VW_FichaAtendimentoOdontologicoMaster AS
select distinct A.id as ID,
                guid_esus as UuidFicha,
                3 as TpCdsOrigem,
                CAST(CASE enviado WHEN 1 THEN 1 ELSE 0 END AS BIT) as Enviado,
				data_atendimento AS DataAtendimento
            from SIGSM_Atendimento_Odontologico A left join SIGSM_Atendimento_Odontologico_Paciente B on (A.id = B.id_atendimento_odontologico )
            where 
                (enviado = 0 or enviado is null) and b.id is not null
                and id_status = 2
GO

CREATE TABLE SIGSM_Odontologico_Consultas (
	id int not null primary key identity,
	descricao varchar(120) not null
)
GO

INSERT INTO SIGSM_Odontologico_Consultas VALUES
('Primeira consulta odontológica programática'),
('Consulta de retorno em odontologia'),
('Consulta de manutenção em odontologia')
GO

CREATE VIEW VW_FichaAtendimentoOdontologicoChild AS
select distinct pac.id as Id,
                odo.guid_esus as UuidFicha,
                pac.data_nascimento as DtNascimento,
                pac.numero_cartao_sus as CnsCidadao,
                pac.numero_prontuario as NumProntuario,	
                CAST(COALESCE(pac.Gestante, 0) AS BIT) As Gestante,
                CAST(COALESCE(pac.paciente_necessidades_especiais, 0) AS BIT) As NecessidadesEspeciais,	
                pac.local_atendimento as LocalAtendimento,
                pac.id_tipo_atendimento as TipoAtendimento,
                REPLACE(LTRIM(RTRIM(pac.conduta)), ' ', '') as TiposEncamOdonto,
				STUFF((SELECT ','+CAST(nuasfs AS NVARCHAR(MAX)) FROM 
					SIGSM_Atendimento_Odontologico_Paciente u
					CROSS APPLY (
						VALUES
						(CASE WHEN ( escova_dental = 1 ) THEN 1 END),
						(CASE WHEN ( creme_dental = 1 ) THEN 2 END),
						(CASE WHEN ( fio_dental = 1 ) THEN 3 END)
					) c ([nuasfs])
					WHERE u.id = pac.id
				FOR XML PATH(''), TYPE).value('.', 'nvarchar(max)'), 1, 1, '') AS TiposFornecimOdonto,
                REPLACE(LTRIM(RTRIM(pac.vigilancia_saude_bucal)), ' ', '') as TiposVigilanciaSaudeBucal,
                REPLACE(LTRIM(RTRIM(CAST(pac.tipo_consulta AS VARCHAR))), ' ', '') as TiposConsultaOdonto,
                sex.codigo AS Sexo,
                tur.id AS Turno
            from SIGSM_Atendimento_Odontologico_Paciente pac
				left join SIGSM_Atendimento_Odontologico odo  on ( pac.id_atendimento_odontologico = odo.id )
                left join ASSMED_Cadastro cad                 on ( pac.id_identificacao_usuario = cad.Codigo )
                left join ASSMED_CadastroDocPessoal dpe       on ( dpe.NumContrato = cad.NumContrato )
                                                             and ( dpe.Codigo = cad.Codigo )
                                                             and ( dpe.CodTpDocP = 6 )
				left join TP_Sexo sex on pac.sexo = sex.sigla
				left join SIGSM_Turno tur on pac.turno = tur.sigla
GO

CREATE VIEW VW_FichaAtendimentoOdontologico_ProcedimentosRealizados AS
select distinct pac.id,
	            odo.guid_esus as UuidFicha,
	            SigTap as CoMsProcedimento,
	            Qtde as Quantidade,
	            pac.id_atendimento_odontologico		
            from SIGSM_Atendimento_Odontologico_Paciente pac left join SigTap_Atendimento_Odontologico sig on (pac.id = sig.id)
                                                                left join SIGSM_Atendimento_Odontologico odo on (pac.id_atendimento_odontologico = odo.id) 
            where Qtde is not null and sigtap is not null
            group by  
	            odo.guid_esus,  
	            pac.id,
	            pac.id_atendimento_odontologico,
	            SigTap,
	            Qtde
GO

CREATE VIEW VW_FichaAtendimentoOdontologico_OutrosSiaProcedimentos AS
select distinct pac.id,
	            odo.guid_esus as UuidFicha,
	            sigtap as CoMsProcedimento,
	            qtde as Quantidade
            from SIGSM_Atendimento_Odontologico_Procedimentos pro left join SIGSM_Atendimento_Odontologico_Paciente pac on (pac.id = pro.id_atendimento_usuario)
	                                                                left join SIGSM_Atendimento_Odontologico odo on (pac.id_atendimento_odontologico = odo.id) 
		WHERE Qtde is not null and sigtap IS NOT NULL
GO

CREATE VIEW VW_FichaAtendimentoOdontologico_HeaderTransport AS
select distinct odo.guid_esus as UuidFicha,
                dpe.Numero as ProfissionalCNS,
                rtrim(ltrim(pro.cbo)) as CboCodigo_2002,
                odo.codigo_cnes_unidade as Cnes,
                sti.Numero as Ine,
	            odo.data_atendimento as DataAtendimento,
	            pro.codibge_munic_nasc as CodigoIbgeMunicipio
            from SIGSM_Atendimento_Odontologico_Profissional pro
			left join SIGSM_Atendimento_Odontologico odo on ( pro.id_atendimento_odontologico = odo.id )
            left join ASSMED_Cadastro cad                on ( pro.id_profissional_saude = cad.Codigo )
            left join ASSMED_CadastroDocPessoal dpe      on ( dpe.NumContrato = cad.NumContrato ) 
			                                            and ( dpe.Codigo = cad.Codigo ) 
										                and ( dpe.CodTpDocP = 6 )
            left join SIGSM_Atendimento_Odontologico_Paciente pac on (pac.id_atendimento_odontologico = odo.id)  
            left join SetoresINEs sti                    on ( odo.codigo_equipe_ine = sti.CodINE )
GO

ALTER TABLE SIGSM_Praticas_Temas_Para_Saude ADD tipo char(1)
GO

UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'P' WHERE id = 1
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 2
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'P' WHERE id = 3
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'P' WHERE id = 4
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 5
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 6
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 7
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 8
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 9
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'P' WHERE id = 10
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 11
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'P' WHERE id = 12
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'P' WHERE id = 13
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 14
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 15
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 16
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 17
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 18
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 19
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 20
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 21
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'P' WHERE id = 22
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'P' WHERE id = 23
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'P' WHERE id = 24
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'P' WHERE id = 25
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'P' WHERE id = 26
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'P' WHERE id = 27
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'P' WHERE id = 28
UPDATE SIGSM_Praticas_Temas_Para_Saude SET tipo = 'T' WHERE id = 29
GO

INSERT INTO SIGSM_Praticas_Temas_Para_Saude VALUES
(30,'Outro procedimento coletivo','P')
GO

ALTER TABLE SIGSM_Atividade_Coletiva ADD turno int
GO

ALTER TABLE SIGSM_Atividade_Coletiva ADD procedimento char(10)
GO

CREATE TABLE SIGSM_Outros_Procedimentos_Coletivos (
	sigtap char(10) not null primary key,
	descricao varchar(120) not null
)
GO

INSERT INTO SIGSM_Outros_Procedimentos_Coletivos VALUES
('0101010044', 'PRÁTICAS CORPORAIS EM MEDICINA TRADICIONAL CHINESA'),
('0101020082', 'EVIDENCIAÇÃO DE PLACA BACTERIANA'),
('0101010052', 'TERAPIA COMUNITARIA'),
('0101010060', 'DANÇA CIRCULAR/BIODANÇA'),
('0101010079', 'IOGA'),
('0101010087', 'OFICINA DE MASSAGEM/AUTO-MASSAGEM'),
('0101020023', 'AÇÃO COLETIVA DE BOCHECHO FLUORADO'),
('0101020040', 'AÇÃO COLETIVA DE EXAME BUCAL COM FINALIDADE EPIDEMIOLÓGICA')
GO

CREATE VIEW VW_FichaAtividadeColetiva AS
 select ati.id as Id,
	    ati.guid_esus as UuidFicha,
	    ati.data_de_atividade as DtAtividadeColetiva,
	    ati.local_de_atividades as OutraLocalidade,
	    ati.numero_inep_escola_creche as Inep,
	    ati.numero_de_participantes as NumParticipantes,
	    ati.numero_de_avaliacoes_alteradas as NumAvaliacoesAlteradas,
	    isnull(ati.id_acao_estruturante_atividade, ati.id_acao_saude_atividade) as AtividadeTipo,
		STUFF((SELECT ',' + CAST(id_temas_reuniao AS NVARCHAR)
                 FROM SIGSM_Atividade_Coletiva_Temas_Reuniao
                WHERE id_atividade_coletiva  = ati.id
				  FOR XML PATH(''), TYPE
		).value('.', 'nvarchar(max)'), 1, 1, '') AS LstTemasParaReuniao,
		STUFF((SELECT  ',' + CAST(id_acao_saude_publico_alvo AS NVARCHAR)
                 FROM SIGSM_Atividade_Coletiva_Publico_Alvo
	            WHERE id_atividade_coletiva = ati.id
				  FOR XML PATH(''), TYPE
		).value('.', 'nvarchar(max)'), 1, 1, '') AS LstPublico,
        3 as TbCdsOrigem,
		ati.codigo_cnes_unidade as CnesLocalAtividade,
		ati.procedimento,
		ati.turno,
		STUFF((SELECT ',' + CAST(pt.id_praticas_temas_para_saude AS NVARCHAR)
                 FROM SIGSM_Atividade_Coletiva_Praticas_Temas_Para_Saude pt
		   INNER JOIN SIGSM_Praticas_Temas_Para_Saude pts
				   ON pt.id_praticas_temas_para_saude = pts.id
                WHERE id_atividade_coletiva  = ati.id AND pts.tipo = 'P'
				  FOR XML PATH(''), TYPE
		).value('.', 'nvarchar(max)'), 1, 1, '') AS LstPraticasSaude,
		STUFF((SELECT ',' + CAST(pt.id_praticas_temas_para_saude AS NVARCHAR)
                 FROM SIGSM_Atividade_Coletiva_Praticas_Temas_Para_Saude pt
		   INNER JOIN SIGSM_Praticas_Temas_Para_Saude pts
				   ON pt.id_praticas_temas_para_saude = pts.id
                WHERE id_atividade_coletiva  = ati.id AND pts.tipo = 'T'
				  FOR XML PATH(''), TYPE
		).value('.', 'nvarchar(max)'), 1, 1, '') AS LstTemasSaude,
        CAST(CASE ati.enviado WHEN 1 THEN 1 ELSE 0 END AS BIT) as Enviado
    from SIGSM_Atividade_Coletiva ati
	left join SetoresINEs sine on ati.codigo_equipe_ine_do_responsavel = sine.CodINE
    where ati.id_status = 2
GO

CREATE VIEW VW_FichaAtividadeColetiva_HeaderTransport AS
 select ati.id as Id,
	    ati.guid_esus as UuidFicha,
		ati.numero_cartao_sus_do_responsavel AS ProfissionalCNS,
		(SELECT TOP(1) pro.cbo FROM SIGSM_Atividade_Coletiva_Profissional_Saude pro WHERE pro.id_atividade_coletiva = ati.id and (pro.id_profissional_saude = ati.id_responsavel or pro.cbo is not null)) as CboCodigo_2002,
	    ati.codigo_cnes_unidade_do_responsavel as Cnes,
	    ati.codigo_equipe_ine_do_responsavel as Ine,
	    ati.data_de_atividade as DataAtendimento,
		(SELECT TOP(1) pro.codibge_munic_nasc FROM SIGSM_Atividade_Coletiva_Profissional_Saude pro WHERE pro.id_atividade_coletiva = ati.id and (pro.id_profissional_saude = ati.id_responsavel or pro.codibge_munic_nasc is not null)) as CodigoIbgeMunicipio
    from 
        SIGSM_Atividade_Coletiva ati left join SetoresINEs sine on ati.codigo_equipe_ine_do_responsavel = sine.CodINE
    where ati.id_status = 2
GO

CREATE VIEW VW_FichaAtividadeColetiva_ProfissionalCboRowItem AS
 select ati.guid_esus as UuidFicha,
        pro.id as Id,
		dpe.Numero as Cns,
		RTRIM(LTRIM(pro.cbo)) as CodigoCbo2002
	from SIGSM_Atividade_Coletiva_Profissional_Saude pro
		left outer join SIGSM_Atividade_Coletiva ati on pro.id_atividade_coletiva = ati.id
		left outer join ASSMED_Cadastro cad on pro.id_profissional_saude = cad.Codigo
		INNER JOIN ASSMED_CadastroDocPessoal dpe on dpe.NumContrato = cad.NumContrato AND dpe.Codigo = cad.Codigo AND dpe.CodTpDocP = 6
GO

CREATE VIEW VW_FichaAtividadeColetiva_ParticipanteRowItem AS
 select usu.id as Id,
		ati.guid_esus as UuidFicha,
        atu.numero_cartao_sus as CnsParticipante,
		usu.data_nascimento as DataNascimento,
        CAST(CASE usu.avaliacao_alterada WHEN 1 THEN 1 ELSE 0 END AS BIT) as AvaliacaoAlterada,
		usu.peso as Peso,
		usu.altura as Altura,
        CAST(CASE usu.tabagismo_cessou_habito_fumar WHEN 1 THEN 1 ELSE 0 END AS BIT) as CessouHabitoFumar, 
        CAST(CASE usu.tabagismo_abandonou_grupo WHEN 1 THEN 1 ELSE 0 END AS BIT) as AbandonouGrupo,
		sex.codigo AS Sexo
	from SIGSM_Atividade_Coletiva_Usuario usu
	    left outer join SIGSM_Atividade_Coletiva ati on usu.id_atividade_coletiva = ati.id
	    left outer join ASSMED_Cadastro cad on usu.id_identificacao_usuario = cad.Codigo
        inner join SIGSM_Atividade_Coletiva_Usuario atu on (ati.id = atu.id_atividade_coletiva)
		left join TP_Sexo sex on atu.sexo = sex.codigo
GO

ALTER VIEW VW_FichaAtividadeColetiva AS
 select ati.id as Id,
	    ati.guid_esus as UuidFicha,
	    ati.data_de_atividade as DtAtividadeColetiva,
	    ati.local_de_atividades as OutraLocalidade,
	    ati.numero_inep_escola_creche as Inep,
	    ati.numero_de_participantes as NumParticipantes,
	    ati.numero_de_avaliacoes_alteradas as NumAvaliacoesAlteradas,
	    isnull(ati.id_acao_estruturante_atividade, ati.id_acao_saude_atividade) as AtividadeTipo,
		STUFF((SELECT ',' + CAST(id_temas_reuniao AS NVARCHAR)
                 FROM SIGSM_Atividade_Coletiva_Temas_Reuniao
                WHERE id_atividade_coletiva  = ati.id
				  FOR XML PATH(''), TYPE
		).value('.', 'nvarchar(max)'), 1, 1, '') AS LstTemasParaReuniao,
		STUFF((SELECT  ',' + CAST(id_acao_saude_publico_alvo AS NVARCHAR)
                 FROM SIGSM_Atividade_Coletiva_Publico_Alvo
	            WHERE id_atividade_coletiva = ati.id
				  FOR XML PATH(''), TYPE
		).value('.', 'nvarchar(max)'), 1, 1, '') AS LstPublico,
        3 as TbCdsOrigem,
		ati.CodigoCnesUnidadeAtividade as CnesLocalAtividade,
		ati.procedimento,
		ati.turno,
		STUFF((SELECT ',' + CAST(pt.id_praticas_temas_para_saude AS NVARCHAR)
                 FROM SIGSM_Atividade_Coletiva_Praticas_Temas_Para_Saude pt
		   INNER JOIN SIGSM_Praticas_Temas_Para_Saude pts
				   ON pt.id_praticas_temas_para_saude = pts.id
                WHERE id_atividade_coletiva  = ati.id AND pts.tipo = 'P'
				  FOR XML PATH(''), TYPE
		).value('.', 'nvarchar(max)'), 1, 1, '') AS LstPraticasSaude,
		STUFF((SELECT ',' + CAST(pt.id_praticas_temas_para_saude AS NVARCHAR)
                 FROM SIGSM_Atividade_Coletiva_Praticas_Temas_Para_Saude pt
		   INNER JOIN SIGSM_Praticas_Temas_Para_Saude pts
				   ON pt.id_praticas_temas_para_saude = pts.id
                WHERE id_atividade_coletiva  = ati.id AND pts.tipo = 'T'
				  FOR XML PATH(''), TYPE
		).value('.', 'nvarchar(max)'), 1, 1, '') AS LstTemasSaude,
        CAST(CASE ati.enviado WHEN 1 THEN 1 ELSE 0 END AS BIT) as Enviado
    from SIGSM_Atividade_Coletiva ati
	left join SetoresINEs sine on ati.codigo_equipe_ine_do_responsavel = sine.CodINE
    where ati.id_status = 2
GO

DECLARE @objname NVARCHAR(255)
SELECT @objname = '['+DB_NAME()+'].[dbo].' + '[SIGSM_Avaliacao_Elegibilidade_Admissao_Profissiona]'

IF EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID(@objname))
	EXEC sp_rename @objname = @objname, @newname = 'SIGSM_Avaliacao_Elegibilidade_Admissao_Profissional'
GO

CREATE VIEW VW_FichaAvaliacaoElegibilidade AS
 SELECT ava.id as Id,
        ava.guid_esus as UuidFicha,
        3 as TpCdsOrigem,
        pac.numero_cartao_sus as CnsCidadao,
        pac.nome_completo as NomeCidadao,
        pac.nome_social as NomeSocialCidadao,
        pac.data_nascimento as DataNascimentoCidadao,
        sex.sigla AS SexoCidadao,
        pac.raca as RacaCorCidadao,
        pac.nome_completo_mae AS nomeMaeCidadao,
        CAST(CASE LEN(COALESCE(LTRIM(RTRIM(pac.nome_completo_mae)), '')) WHEN 0 THEN 1 ELSE 0 END AS BIT) DesconheceNomeMae,
        pro.codibge_munic_nasc as CodigoIbgeMunicipioNascimento,
        pac.nacionalidade AS nacionalidadeCidadao,
        pac.email as EmailCidadao,
        pac.nis as NumeroNisPisPasep,
        pac.origem as AtencaoDomiciliarProcedencia,
        pac.conclusao as AtencaoDomiciliarModalidade,
		dbo.FN_ParseCondicoesAvaliadas(pac.condicoes_avaliadas) AS LstCondicoesAvaliadas,
		pac.cid_1 as Cid10Principal,
        pac.cid_2 as Cid10SecundarioUm,
        pac.cid_3 as Cid10SecundarioDois,
        pac.conclusao_elegivel as ConclusaoDestinoElegivel,
        pac.conclusao_inelegivel as LstConclusaoDestinoInelegivel,
        pac.cuidador as CuidadorCidadao,
		pac.turno AS Turno,
		pac.nome_completo_pai AS NomePaiCidadao,
        CAST(CASE LEN(COALESCE(LTRIM(RTRIM(pac.nome_completo_pai)), '')) WHEN 0 THEN 1 ELSE 0 END AS BIT) DesconheceNomePai,
		pac.NAC_NAT_DATA AS DtNaturalizacao,
		pac.NAC_NAT_PORTARIA AS PortariaNaturalizacao,
		pac.NAC_EST_DATA AS DtEntradaBrasil,
		pac.NAC_EST_PAIS AS PaisNascimento,
		pac.ETNIA AS Etnia,
		pac.CNS_CUIDADOR AS CnsCuidador,
		ava.data_avaliacao AS DataAvaliacao,
        CAST(CASE ava.enviado WHEN 1 THEN 1 ELSE 0 END AS BIT)  as Enviado
    FROM SIGSM_Avaliacao_Elegibilidade_Admissao ava
            Left Outer Join SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente pac on ava.id = pac.id_avaliacao
            Left Outer Join SIGSM_Avaliacao_Elegibilidade_Admissao_Profissional pro on pro.id_avaliacao = ava.id
			left join TP_Sexo sex on pac.sexo = sex.sigla
    where (enviado = 0 or enviado is null)
GO

CREATE VIEW VW_FichaAvaliacaoElegibilidade_EnderecoLocalPermanencia AS
 SELECT ava.guid_esus as UuidFicha,
	    ava.id as Id,
        pac.bairro as Bairro,
        pac.cep as Cep,
        pro.codibge_munic_nasc as CodigoIbgeMunicipio,
        pac.complemento as Complemento,
        pac.nome_logradouro as NomeLogradouro,
        pac.numero as Numero,
        pac.uf as NumeroDneUf,
        pac.telefone_referencia as TelefoneContato,
        pac.telefone_residencial as TelefoneResidencia,
        pac.tipo_logradouro as TipoLogradouroNumeroDne,
		CAST(CASE LEN(COALESCE(RTRIM(LTRIM(pac.numero)), '')) WHEN 0 THEN 1 ELSE 0 END AS BIT)
			AS StSemNumero,
		pac.PontoReferencia AS PontoReferencia
    FROM SIGSM_Avaliacao_Elegibilidade_Admissao_Paciente pac
        Left Outer Join SIGSM_Avaliacao_Elegibilidade_Admissao_Profissional pro on pro.id_avaliacao = pac.id
        Left Outer Join SIGSM_Avaliacao_Elegibilidade_Admissao ava on pac.id_avaliacao = ava.id
    WHERE  ava.id_status = 2
GO

CREATE VIEW VW_FichaAvaliacaoElegibilidade_HeaderTransport AS
 SELECT ava.id as Id,
        ava.guid_esus as UuidFicha,
        dpe.Numero as ProfissionalCNS,
        rtrim(ltrim(pro.cbo)) as CboCodigo_2002, 
        ava.codigo_cnes_unidade as Cnes,
        ava.codigo_equipe_ine as Ine,
        ava.data_avaliacao as DataAtendimento,
        pro.codibge_munic_nasc as CodigoIbgeMunicipio
   from SIGSM_Avaliacao_Elegibilidade_Admissao ava
            left outer join ASSMED_Cadastro cad on ava.id = cad.Codigo
            left outer join SIGSM_Avaliacao_Elegibilidade_Admissao_Profissional pro on pro.id_avaliacao = ava.id
            INNER JOIN ASSMED_CadastroDocPessoal dpe on dpe.NumContrato = cad.NumContrato AND dpe.Codigo = cad.Codigo AND dpe.CodTpDocP = 6
GO

CREATE VIEW VW_FichaCadastroDomiciliarTerritorial AS
	 SELECT STUFF((
				SELECT ',' + CAST(ad.id_tp_animal AS NVARCHAR)
				  FROM api.AnimalNoDomicilio AS ad
				 WHERE ad.id_cadastro_domiciliar = cd.id
				   FOR XML PATH(''), TYPE
			).value('.', 'nvarchar(max)'), 1, 1, '') AS LstAnimaisNoDomicilio,
			cd.fichaAtualizada AS FichaAtualizada,
			cd.quantosAnimaisNoDomicilio AS QuantosAnimaisNoDomicilio,
			cd.stAnimaisNoDomicilio AS StAnimaisNoDomicilio,
			cd.statusTermoRecusa AS StatusTermoRecusa,
			3 AS TpCdsOrigem,
			cd.id AS Uuid,
			cd.uuidFichaOriginadora AS UuidFichaOriginadora,
			cd.tipoDeImovel AS TipoDeImovel,
			ult.dataAtendimento AS DataAtendimento
	   FROM api.CadastroDomiciliar AS cd
 INNER JOIN api.UnicaLotacaoTransport AS ult
		 ON cd.headerTransport = ult.id
 INNER JOIN api.OrigemVisita AS ov
		 ON ult.token = ov.token
	  WHERE ov.enviarParaThrift = 1
		AND ov.finalizado = 1
		AND ov.enviado = 0
GO

CREATE VIEW VW_FichaCadastroDomiciliarTerritorial_CondicaoMoradia AS
	 SELECT cd.id AS Uuid,
			cm.abastecimentoAgua AS AbastecimentoAgua,
			cm.areaProducaoRural AS AreaProducaoRural,
			cm.destinoLixo AS DestinoLixo,
			cm.formaEscoamentoBanheiro AS FormaEscoamentoBanheiro,
			cm.localizacao AS Localizacao,
			cm.materialPredominanteParedesExtDomicilio AS MaterialPredominanteParedesExtDomicilio,
			cm.nuComodos AS NuComodos,
			cm.nuMoradores AS NuMoradores,
			cm.situacaoMoradiaPosseTerra AS SituacaoMoradiaPosseTerra,
			cm.stDisponibilidadeEnergiaEletrica AS StDisponibilidadeEnergiaEletrica,
			cm.tipoAcessoDomicilio AS TipoAcessoDomicilio,
			cm.tipoDomicilio AS TipoDomicilio,
			cm.aguaConsumoDomicilio AS AguaConsumoDomicilio
	   FROM api.CondicaoMoradia AS cm
 INNER JOIN api.CadastroDomiciliar AS cd
		 ON cd.condicaoMoradia = cm.id
GO

CREATE VIEW VW_FichaCadastroDomiciliarTerritorial_EnderecoLocalPermanencia AS
	 SELECT cd.id AS Uuid,
			elp.bairro AS Bairro,
			elp.cep AS Cep,
			elp.codigoIbgeMunicipio AS CodigoIbgeMunicipio,
			elp.complemento AS Complemento,
			elp.nomeLogradouro AS NomeLogradouro,
			elp.numero AS Numero,
			elp.numeroDneUf AS NumeroDneUf,
			elp.telefoneContato AS TelefoneContato,
			elp.telefoneResidencia AS TelefoneResidencia,
			elp.tipoLogradouroNumeroDne AS TipoLogradouroNumeroDne,
			elp.stSemNumero AS StSemNumero,
			elp.pontoReferencia AS PontoReferencia,
			elp.microarea AS Microarea,
			elp.stForaArea AS StForaArea
	   FROM api.EnderecoLocalPermanencia AS elp
 INNER JOIN api.CadastroDomiciliar AS cd
		 ON cd.enderecoLocalPermanencia = elp.id
GO

CREATE VIEW VW_FichaCadastroDomiciliarTerritorial_FamiliaRow AS
	 SELECT cd.id AS Uuid,
			fr.dataNascimentoResponsavel AS DataNascimentoResponsavel,
			fr.numeroCnsResponsavel AS NumeroCnsResponsavel,
			fr.numeroMembrosFamilia AS NumeroMembrosFamilia,
			fr.numeroProntuario AS NumeroProntuario,
			fr.rendaFamiliar AS RendaFamiliar,
			fr.resideDesde AS ResideDesde,
			fr.stMudanca AS StMudanca
	   FROM api.FamiliaRow AS fr
 INNER JOIN api.Familias AS f
		 ON fr.id = f.id_familia_row
 INNER JOIN api.CadastroDomiciliar AS cd
		 ON cd.id = f.id_cadatro_domiciliar
GO

CREATE VIEW VW_FichaCadastroDomiciliarTerritorial_InstituicaoPermanencia AS
	 SELECT cd.id AS Uuid,
			ip.nomeInstituicaoPermanencia AS NomeInstituicaoPermanencia,
			ip.stOutrosProfissionaisVinculados AS StOutrosProfissionaisVinculados,
			ip.nomeResponsavelTecnico AS NomeResponsavelTecnico,
			ip.cnsResponsavelTecnico AS CnsResponsavelTecnico,
			ip.cargoInstituicao AS CargoInstituicao,
			ip.telefoneResponsavelTecnico AS TelefoneResponsavelTecnico
	   FROM api.InstituicaoPermanencia AS ip
 INNER JOIN api.CadastroDomiciliar AS cd
		 ON cd.instituicaoPermanencia = ip.id
GO

CREATE VIEW VW_FichaCadastroDomiciliarTerritorial_UnicaLotacaoHeader AS
	 SELECT cd.id AS Uuid,
			ult.profissionalCNS AS ProfissionalCNS,
			ult.cboCodigo_2002 AS CboCodigo_2002,
			ult.cnes AS Cnes,
			ult.ine AS Ine,
			ult.dataAtendimento AS DataAtendimento,
			ult.codigoIbgeMunicipio AS CodigoIbgeMunicipio
	   FROM api.CadastroDomiciliar AS cd
 INNER JOIN api.UnicaLotacaoTransport AS ult
		 ON cd.headerTransport = ult.id
 INNER JOIN api.OrigemVisita AS ov
		 ON ult.token = ov.token
	  WHERE ov.enviarParaThrift = 1
		AND ov.finalizado = 1
		AND ov.enviado = 0
GO

CREATE VIEW VW_FichaCadastroIndividual AS
	 SELECT ci.fichaAtualizada AS FichaAtualizada,
			ci.statusTermoRecusaCadastroIndividualAtencaoBasica AS StatusTermoRecusaCadastroIndividualAtencaoBasica,
			3 AS TpCdsOrigem,
			ci.id AS Uuid,
			ci.uuidFichaOriginadora AS UuidFichaOriginadora,
			ult.dataAtendimento AS DataAtendimento
	   FROM api.CadastroIndividual AS ci
 INNER JOIN api.UnicaLotacaoTransport AS ult
		 ON ci.headerTransport = ult.id
 INNER JOIN api.OrigemVisita AS ov
		 ON ult.token = ov.token
	  WHERE ov.enviarParaThrift = 1
		AND ov.finalizado = 1
		AND ov.enviado = 0
GO

CREATE VIEW VW_FichaCadastroIndividual_CondicoesDeSaude AS
	 SELECT ci.id AS Uuid,
			cds.descricaoCausaInternacaoEm12Meses AS DescricaoCausaInternacaoEm12Meses,
			cds.descricaoOutraCondicao1 AS DescricaoOutraCondicao1,
			cds.descricaoOutraCondicao2 AS DescricaoOutraCondicao2,
			cds.descricaoOutraCondicao3 AS DescricaoOutraCondicao3,
			cds.descricaoPlantasMedicinaisUsadas AS DescricaoPlantasMedicinaisUsadas,
			STUFF((SELECT ',' + CAST(dc.id_tp_doenca_cariaca AS NVARCHAR)
					 FROM api.DoencaCardiaca AS dc
					WHERE dc.id_condicao_de_saude = cds.id
					  FOR XML PATH(''), TYPE
			).value('.', 'nvarchar(max)'), 1, 1, '') AS LstDoencaCardiaca,
			STUFF((SELECT ',' + CAST(dc.id_tp_doenca_respiratoria AS NVARCHAR)
					 FROM api.DoencaRespiratoria AS dc
					WHERE dc.id_condicao_de_saude = cds.id
					  FOR XML PATH(''), TYPE
			).value('.', 'nvarchar(max)'), 1, 1, '') AS LstDoencaRespiratoria,
			STUFF((SELECT ',' + CAST(dc.id_tp_doenca_rins AS NVARCHAR)
					 FROM api.DoencaRins AS dc
					WHERE dc.id_condicao_de_saude = cds.id
					  FOR XML PATH(''), TYPE
			).value('.', 'nvarchar(max)'), 1, 1, '') AS LstDoencaRins,
			cds.maternidadeDeReferencia AS MaternidadeDeReferencia,
			cds.situacaoPeso AS SituacaoPeso,
			cds.statusEhDependenteAlcool AS StatusEhDependenteAlcool,
			cds.statusEhDependenteOutrasDrogas AS StatusEhDependenteOutrasDrogas,
			cds.statusEhFumante AS StatusEhFumante,
			cds.statusEhGestante AS StatusEhGestante,
			cds.statusEstaAcamado AS StatusEstaAcamado,
			cds.statusEstaDomiciliado AS StatusEstaDomiciliado,
			cds.statusTemDiabetes AS StatusTemDiabetes,
			cds.statusTemDoencaRespiratoria AS StatusTemDoencaRespiratoria,
			cds.statusTemHanseniase AS StatusTemHanseniase,
			cds.statusTemHipertensaoArterial AS StatusTemHipertensaoArterial,
			cds.statusTemTeveCancer AS StatusTemTeveCancer,
			cds.statusTemTeveDoencasRins AS StatusTemTeveDoencasRins,
			cds.statusTemTuberculose AS StatusTemTuberculose,
			cds.statusTeveAvcDerrame AS StatusTeveAvcDerrame,
			cds.statusTeveDoencaCardiaca AS StatusTeveDoencaCardiaca,
			cds.statusTeveInfarto AS StatusTeveInfarto,
			cds.statusTeveInternadoem12Meses AS StatusTeveInternadoem12Meses,
			cds.statusUsaOutrasPraticasIntegrativasOuComplementares AS StatusUsaOutrasPraticasIntegrativasOuComplementares,
			cds.statusUsaPlantasMedicinais AS StatusUsaPlantasMedicinais,
			cds.statusDiagnosticoMental AS StatusDiagnosticoMental
	   FROM api.CondicoesDeSaude AS cds
 INNER JOIN api.CadastroIndividual AS ci
		 ON ci.condicoesDeSaude = cds.id
GO

CREATE VIEW VW_FichaCadastroIndividual_EmSituacaoDeRua AS
	 SELECT ci.id AS Uuid,
			esdr.grauParentescoFamiliarFrequentado AS GrauParentescoFamiliarFrequentado,
			STUFF((SELECT ',' + CAST(hpsr.codigo_higiene_pessoal AS NVARCHAR)
					 FROM api.HigienePessoalSituacaoRua AS hpsr
					WHERE hpsr.id_em_situacao_de_rua = esdr.id
					  FOR XML PATH(''), TYPE
			).value('.', 'nvarchar(max)'), 1, 1, '') AS LstHigienePessoalSituacaoRua,
			STUFF((SELECT ',' + CAST(hpsr.id_tp_origem_alimento AS NVARCHAR)
					 FROM api.OrigemAlimentoSituacaoRua AS hpsr
					WHERE hpsr.id_em_situacao_rua = esdr.id
					  FOR XML PATH(''), TYPE
			).value('.', 'nvarchar(max)'), 1, 1, '') AS LstOrigemAlimentoSituacaoRua,
			esdr.outraInstituicaoQueAcompanha AS OutraInstituicaoQueAcompanha,
			esdr.quantidadeAlimentacoesAoDiaSituacaoRua AS QuantidadeAlimentacoesAoDiaSituacaoRua,
			esdr.statusAcompanhadoPorOutraInstituicao AS StatusAcompanhadoPorOutraInstituicao,
			esdr.statusPossuiReferenciaFamiliar AS StatusPossuiReferenciaFamiliar,
			esdr.statusRecebeBeneficio AS StatusRecebeBeneficio,
			esdr.statusSituacaoRua AS StatusSituacaoRua,
			esdr.statusTemAcessoHigienePessoalSituacaoRua AS StatusTemAcessoHigienePessoalSituacaoRua,
			esdr.statusVisitaFamiliarFrequentemente AS StatusVisitaFamiliarFrequentemente,
			esdr.tempoSituacaoRua AS TempoSituacaoRua
	   FROM api.EmSituacaoDeRua AS esdr
 INNER JOIN api.CadastroIndividual AS ci
		 ON ci.emSituacaoDeRua = esdr.id
GO

CREATE VIEW VW_FichaCadastroIndividual_IdentificacaoUsuarioCidadao AS
	 SELECT ci.id AS Uuid,
			iuc.nomeSocial AS NomeSocial,
			iuc.codigoIbgeMunicipioNascimento AS CodigoIbgeMunicipioNascimento,
			iuc.dataNascimentoCidadao AS DataNascimentoCidadao,
			iuc.desconheceNomeMae AS DesconheceNomeMae,
			iuc.emailCidadao AS EmailCidadao,
			iuc.nacionalidadeCidadao AS NacionalidadeCidadao,
			iuc.nomeCidadao AS NomeCidadao,
			iuc.nomeMaeCidadao AS NomeMaeCidadao,
			iuc.cnsCidadao AS CnsCidadao,
			iuc.cnsResponsavelFamiliar AS CnsResponsavelFamiliar,
			iuc.telefoneCelular AS TelefoneCelular,
			iuc.numeroNisPisPasep AS NumeroNisPisPasep,
			iuc.paisNascimento AS PaisNascimento,
			iuc.racaCorCidadao AS RacaCorCidadao,
			iuc.sexoCidadao AS SexoCidadao,
			iuc.statusEhResponsavel AS StatusEhResponsavel,
			iuc.etnia AS Etnia,
			iuc.nomePaiCidadao AS NomePaiCidadao,
			iuc.desconheceNomePai AS DesconheceNomePai,
			iuc.dtNaturalizacao AS DtNaturalizacao,
			iuc.portariaNaturalizacao AS PortariaNaturalizacao,
			iuc.dtEntradaBrasil AS DtEntradaBrasil,
			iuc.microarea AS MicroArea,
			iuc.stForaArea AS StForaArea
	   FROM api.IdentificacaoUsuarioCidadao AS iuc
 INNER JOIN api.CadastroIndividual AS ci
		 ON ci.identificacaoUsuarioCidadao = iuc.id
GO

CREATE VIEW VW_FichaCadastroIndividual_InformacoesSocioDemograficas AS
	 SELECT ci.id AS Uuid,
			STUFF((SELECT ',' + CAST(dc.id_tp_deficiencia_cidadao AS NVARCHAR)
					 FROM api.DeficienciasCidadao AS dc
					WHERE dc.id_informacoes_socio_demograficas = isd.id
					  FOR XML PATH(''), TYPE
			).value('.', 'nvarchar(max)'), 1, 1, '') AS LstDeficienciasCidadao,
			isd.grauInstrucaoCidadao AS GrauInstrucaoCidadao,
			isd.ocupacaoCodigoCbo2002 AS OcupacaoCodigoCbo2002,
			isd.orientacaoSexualCidadao AS OrientacaoSexualCidadao,
			isd.povoComunidadeTradicional AS PovoComunidadeTradicional,
			isd.relacaoParentescoCidadao AS RelacaoParentescoCidadao,
			isd.situacaoMercadoTrabalhoCidadao AS SituacaoMercadoTrabalhoCidadao,
			isd.statusDesejaInformarOrientacaoSexual AS StatusDesejaInformarOrientacaoSexual,
			isd.statusFrequentaBenzedeira AS StatusFrequentaBenzedeira,
			isd.statusFrequentaEscola AS StatusFrequentaEscola,
			isd.statusMembroPovoComunidadeTradicional AS StatusMembroPovoComunidadeTradicional,
			isd.statusParticipaGrupoComunitario AS StatusParticipaGrupoComunitario,
			isd.statusPossuiPlanoSaudePrivado AS StatusPossuiPlanoSaudePrivado,
			isd.statusTemAlgumaDeficiencia AS StatusTemAlgumaDeficiencia,
			isd.identidadeGeneroCidadao AS IdentidadeGeneroCidadao,
			isd.statusDesejaInformarIdentidadeGenero AS StatusDesejaInformarIdentidadeGenero,
			STUFF((SELECT ',' + CAST(rpc.id_tp_crianca AS NVARCHAR)
					 FROM api.ResponsavelPorCrianca AS rpc
					WHERE rpc.id_informacoes_sociodemograficas = isd.id
					  FOR XML PATH(''), TYPE
			).value('.', 'nvarchar(max)'), 1, 1, '') AS ResponsavelPorCrianca
	   FROM api.InformacoesSocioDemograficas AS isd
 INNER JOIN api.CadastroIndividual AS ci
		 ON ci.informacoesSocioDemograficas = isd.id
GO

CREATE VIEW VW_FichaCadastroIndividual_SaidaCidadaoCadastro AS
	 SELECT ci.id AS Uuid,
			scc.motivoSaidaCidadao AS MotivoSaidaCidadao,
			scc.dataObito AS DataObito,
			scc.numeroDO AS NumeroDO
	   FROM api.SaidaCidadaoCadastro AS scc
 INNER JOIN api.CadastroIndividual AS ci
		 ON scc.id = ci.saidaCidadaoCadastro
GO

CREATE VIEW VW_FichaCadastroIndividual_UnicaLotacaoHeader AS
	 SELECT ci.id AS Uuid,
			ult.profissionalCNS AS ProfissionalCNS,
			ult.cboCodigo_2002 AS CboCodigo_2002,
			ult.cnes AS Cnes,
			ult.ine AS Ine,
			ult.dataAtendimento AS DataAtendimento,
			ult.codigoIbgeMunicipio AS CodigoIbgeMunicipio
	   FROM api.CadastroIndividual AS ci
 INNER JOIN api.UnicaLotacaoTransport AS ult
		 ON ci.headerTransport = ult.id
 INNER JOIN api.OrigemVisita AS ov
		 ON ult.token = ov.token
	  WHERE ov.enviarParaThrift = 1
		AND ov.finalizado = 1
		AND ov.enviado = 0
GO

CREATE VIEW VW_FichaConsumoAlimentar AS
 SELECT con.id as Id,
        con.guid_esus as UuidFicha,
        con.numero_cartao_sus as CnsCidadao,
        con.nome_cidadao as IdentificacaoUsuario,
        con.data_nascimento as DataNascimento,
        sex.codigo AS Sexo,
        con.local_atendimento as LocalAtendimento,
        3 as TpCdsOrigem,
        CAST(CASE con.enviado WHEN 1 THEN 1 ELSE 0 END AS BIT) AS Enviado,
		con.data_marcadores AS DataAtendimento
    FROM SIGSM_Marcadores_Consumo_Alimentar AS con
LEFT JOIN TP_Sexo AS sex ON con.sexo = sex.sigla
    where 
        (enviado = 0 or enviado is null) 
GO

CREATE VIEW VW_FichaConsumoAlimentar_UnicaLotacaoHeader AS
	 SELECT con.id AS Id,
            con.guid_esus AS UuidFicha,
            dpe.Numero AS ProfissionalCNS,
            rtrim(ltrim(con.cbo)) AS CboCodigo_2002,
            con.codigo_cnes_unidade AS Cnes,
            con.codigo_equipe_ine AS Ine,
            con.data_marcadores AS DataAtendimento,
            cont.CodigoIbgeMunicipio AS CodigoIbgeMunicipio
       FROM SIGSM_Marcadores_Consumo_Alimentar AS con
  LEFT JOIN ASSMED_Cadastro AS cad
		 ON con.id_profissional_saude = cad.Codigo	
 INNER JOIN ASSMED_Contratos AS cont
		 ON cont.NumContrato = 22
 INNER JOIN ASSMED_CadastroDocPessoal AS dpe
		 ON dpe.NumContrato = cad.NumContrato 
		AND dpe.Codigo = cad.Codigo
		AND dpe.CodTpDocP = 6
GO

CREATE VIEW VW_FichaConsumoAlimentar_PerguntaQuestionarioCriancasMenoresSeisMeses AS
	 SELECT mca.id AS FichaId, [Pergunta], [RespostaUnicaEscolha]
	   FROM [SIGSM_Crianca_Menor_Seis_Meses] AS cmsm
 INNER JOIN SIGSM_Marcadores_Consumo_Alimentar AS mca
		 ON cmsm.id = mca.id_crianca_menor_seis_meses
CROSS APPLY (
	 VALUES (1, CASE 
				WHEN cmsm.[a_crianca_ontem_tomou_leite_do_peito] = 'Sim' THEN 1
				WHEN cmsm.[a_crianca_ontem_tomou_leite_do_peito] = 'Nao' THEN 2
				WHEN cmsm.[a_crianca_ontem_tomou_leite_do_peito] = 'Nao Sabe' THEN 3
				ELSE Null
			END),
			(3, CASE 
				WHEN [ontem_a_crianca_consumiu_mingau] = 'Sim' THEN 1
				WHEN [ontem_a_crianca_consumiu_mingau] = 'Nao' THEN 2
				WHEN [ontem_a_crianca_consumiu_mingau] = 'Nao Sabe' THEN 3
				ELSE Null
			END),
			(4, CASE 
				WHEN [ontem_a_crianca_consumiu_agua_cha] = 'Sim' THEN 1
				WHEN [ontem_a_crianca_consumiu_agua_cha] = 'Nao' THEN 2
				WHEN [ontem_a_crianca_consumiu_agua_cha] = 'Nao Sabe' THEN 3
				ELSE Null
			END),
			(5, CASE 
				WHEN [ontem_a_crianca_consumiu_leite_de_vaca] = 'Sim' THEN 1
				WHEN [ontem_a_crianca_consumiu_leite_de_vaca] = 'Nao' THEN 2
				WHEN [ontem_a_crianca_consumiu_leite_de_vaca] = 'Nao Sabe' THEN 3
				ELSE Null
			END),
			(6, CASE 
				WHEN [ontem_a_crianca_consumiu_formula_infantil] = 'Sim' THEN 1
				WHEN [ontem_a_crianca_consumiu_formula_infantil] = 'Nao' THEN 2
				WHEN [ontem_a_crianca_consumiu_formula_infantil] = 'Nao Sabe' THEN 3
				ELSE Null
			END),
			(7, CASE 
				WHEN [ontem_a_crianca_consumiu_suco_de_fruta] = 'Sim' THEN 1
				WHEN [ontem_a_crianca_consumiu_suco_de_fruta] = 'Nao' THEN 2
				WHEN [ontem_a_crianca_consumiu_suco_de_fruta] = 'Nao Sabe' THEN 3
				ELSE Null
			END),
			(8, CASE 
				WHEN [ontem_a_crianca_consumiu_fruta] = 'Sim' THEN 1
				WHEN [ontem_a_crianca_consumiu_fruta] = 'Nao' THEN 2
				WHEN [ontem_a_crianca_consumiu_fruta] = 'Nao Sabe' THEN 3
				ELSE Null
			END),
			(9, CASE 
				WHEN [ontem_a_crianca_consumiu_comida_de_sal] = 'Sim' THEN 1
				WHEN [ontem_a_crianca_consumiu_comida_de_sal] = 'Nao' THEN 2
				WHEN [ontem_a_crianca_consumiu_comida_de_sal] = 'Nao Sabe' THEN 3
				ELSE Null
			END),
			(10, CASE 
				WHEN [ontem_a_crianca_consumiu_outros_alimentos_bebidas] = 'Sim' THEN 1
				WHEN [ontem_a_crianca_consumiu_outros_alimentos_bebidas] = 'Nao' THEN 2
				WHEN [ontem_a_crianca_consumiu_outros_alimentos_bebidas] = 'Nao Sabe' THEN 3
				ELSE Null
			END)
			) AS a ([Pergunta], [RespostaUnicaEscolha])
GO

CREATE VIEW VW_FichaConsumoAlimentar_PerguntaQuestionarioCriancasDeSeisVinteTresMeses AS
	 SELECT mca.id AS FichaId, [Pergunta], [RespostaUnicaEscolha]
	   FROM [SIGSM_Crianca_Seis_a_23_Meses] AS cmsm
 INNER JOIN SIGSM_Marcadores_Consumo_Alimentar AS mca
		 ON cmsm.id = mca.id_crianca_menor_seis_meses
CROSS APPLY (
	 VALUES (21, CASE 
				WHEN upper([a_crianca_ontem_tomou_leite_do_peito]) = upper('Sim') THEN 1
				WHEN upper([a_crianca_ontem_tomou_leite_do_peito]) = upper('Nao') THEN 2
				WHEN upper([a_crianca_ontem_tomou_leite_do_peito]) = upper('Nao Sabe') THEN 3
				ELSE Null
            END),
            (22, CASE 
				WHEN upper([ontem_a_crianca_comeu_fruta_inteira_em_pedaco_amassada]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_comeu_fruta_inteira_em_pedaco_amassada]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_comeu_fruta_inteira_em_pedaco_amassada]) = upper('Nao Sabe') THEN 3
				ELSE Null
            END),
            (23, CASE 
				WHEN upper([quantas_vezes_ontem_a_crianca_comeu_fruta_inteira_em_pedaco_amassada]) = upper('Nao Sabe') THEN 3
				WHEN upper([quantas_vezes_ontem_a_crianca_comeu_fruta_inteira_em_pedaco_amassada]) = upper('1 vez') THEN 4
				WHEN upper([quantas_vezes_ontem_a_crianca_comeu_fruta_inteira_em_pedaco_amassada]) = upper('2 vezes') THEN 5
				WHEN upper([quantas_vezes_ontem_a_crianca_comeu_fruta_inteira_em_pedaco_amassada]) = upper('3 vezes ou mais') THEN 6
				ELSE Null
            END),
            (24, CASE 
				WHEN upper([ontem_a_crianca_comeu_comida_de_sal]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_comeu_comida_de_sal]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_comeu_comida_de_sal]) = upper('Nao Sabe') THEN 3
				ELSE Null
            END),
            (25, CASE 
				WHEN upper([quantas_vezes_ontem_a_crianca_comeu_comida_de_sal]) = upper('Nao Sabe') THEN 3
				WHEN upper([quantas_vezes_ontem_a_crianca_comeu_comida_de_sal]) = upper('1 vez') THEN 4
				WHEN upper([quantas_vezes_ontem_a_crianca_comeu_comida_de_sal]) = upper('2 vezes') THEN 5
				WHEN upper([quantas_vezes_ontem_a_crianca_comeu_comida_de_sal]) = upper('3 vezes ou mais') THEN 6
				ELSE Null
            END),
            (26, CASE 
				WHEN upper([comida_oferecida]) = upper('Nao Sabe') THEN 3
				WHEN upper([comida_oferecida]) = upper('Em pedacos') THEN 7
				WHEN upper([comida_oferecida]) = upper('Amassada') THEN 8
				WHEN upper([comida_oferecida]) = upper('Passada na peneira') THEN 9
				WHEN upper([comida_oferecida]) = upper('Liquidificada') THEN 10
				WHEN upper([comida_oferecida]) = upper('So o caldo') THEN 11
				ELSE Null
            END),
            (28, CASE 
				WHEN upper([ontem_a_crianca_consumiu_outro_leite_nao_de_peito]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_consumiu_outro_leite_nao_de_peito]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_consumiu_outro_leite_nao_de_peito]) = upper('Nao Sabe') THEN 3
				ELSE Null
            END),
            (29, CASE 
				WHEN upper([ontem_a_crianca_consumiu_mingau_com_leite]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_consumiu_mingau_com_leite]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_consumiu_mingau_com_leite]) = upper('Nao Sabe') THEN 3
				ELSE Null
            END),
            (30, CASE 
				WHEN upper([ontem_a_crianca_consumiu_iogurte]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_consumiu_iogurte]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_consumiu_iogurte]) = upper('Nao Sabe') THEN 3
				ELSE Null
            END),
            (31, CASE 
				WHEN upper([ontem_a_crianca_consumiu_legumes]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_consumiu_legumes]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_consumiu_legumes]) = upper('Nao Sabe') THEN 3
				ELSE Null
            END),
            (32, CASE 
				WHEN upper([ontem_a_crianca_consumiu_vegetal_ou_fruta_de_cor_alaranjada]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_consumiu_vegetal_ou_fruta_de_cor_alaranjada]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_consumiu_vegetal_ou_fruta_de_cor_alaranjada]) = upper('Nao Sabe') THEN 3
            ELSE Null
            END),
            (33, CASE 
				WHEN upper([ontem_a_crianca_consumiu_verdura_de_folha]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_consumiu_verdura_de_folha]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_consumiu_verdura_de_folha]) = upper('Nao Sabe') THEN 3
            ELSE Null
            END),
            (34, CASE 
				WHEN upper([ontem_a_crianca_consumiu_carne]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_consumiu_carne]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_consumiu_carne]) = upper('Nao Sabe') THEN 3
            ELSE Null
            END),
            (35, CASE 
				WHEN upper([ontem_a_crianca_consumiu_figado]) = upper('Nao Sabe') THEN 3
				WHEN upper([ontem_a_crianca_consumiu_figado]) = upper('1 vez') THEN 4
				WHEN upper([ontem_a_crianca_consumiu_figado]) = upper('2 vezes') THEN 5
            ELSE Null
            END),
            (36, CASE 
				WHEN upper([ontem_a_crianca_consumiu_feijao]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_consumiu_feijao]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_consumiu_feijao]) = upper('Nao Sabe') THEN 3
            ELSE Null
            END),
            (37, CASE 
				WHEN upper([ontem_a_crianca_consumiu_arroz_batata_inhame_aipim_farinha_macarrao_sem_ser_instataneo]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_consumiu_arroz_batata_inhame_aipim_farinha_macarrao_sem_ser_instataneo]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_consumiu_arroz_batata_inhame_aipim_farinha_macarrao_sem_ser_instataneo]) = upper('Nao Sabe') THEN 3
            ELSE Null
            END),
            (38, CASE 
				WHEN upper([ontem_a_crianca_consumiu_hamburguer_e_ou_embutidos]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_consumiu_hamburguer_e_ou_embutidos]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_consumiu_hamburguer_e_ou_embutidos]) = upper('Nao Sabe') THEN 3
            ELSE Null
            END),
            (39, CASE 
				WHEN upper([ontem_a_crianca_consumiu_bebidas_adocadas]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_consumiu_bebidas_adocadas]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_consumiu_bebidas_adocadas]) = upper('Nao Sabe') THEN 3
            ELSE Null
            END),
            (40, CASE 
				WHEN upper([ontem_a_crianca_consumiu_macarrao_instantaneo_salgadinhos_de_pacote_biscoitos_salgados]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_consumiu_macarrao_instantaneo_salgadinhos_de_pacote_biscoitos_salgados]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_consumiu_macarrao_instantaneo_salgadinhos_de_pacote_biscoitos_salgados]) = upper('Nao Sabe') THEN 3
            ELSE Null
            END),
            (41, CASE 
				WHEN upper([ontem_a_crianca_consumiu_biscoito_recheado_doces]) = upper('Sim') THEN 1
				WHEN upper([ontem_a_crianca_consumiu_biscoito_recheado_doces]) = upper('Nao') THEN 2
				WHEN upper([ontem_a_crianca_consumiu_biscoito_recheado_doces]) = upper('Nao Sabe') THEN 3
            ELSE Null
            END)
			) AS a ([Pergunta], [RespostaUnicaEscolha])
GO

DECLARE @objname NVARCHAR(255)
SELECT @objname = '['+DB_NAME()+'].[dbo].' + '[SIGSM_Crianca_Mais_2_Anos_Adolescente_Adulto_Gesta]'

IF EXISTS (SELECT 1 FROM sys.tables WHERE object_id = OBJECT_ID(@objname))
	EXEC sp_rename @objname = @objname, @newname = 'SIGSM_Crianca_Mais_2_Anos_Adolescente_Adulto_Gestante_Idoso'
GO

CREATE VIEW VW_FichaConsumoAlimentar_PerguntaQuestionarioCriancasComMaisDoisAnos AS
	 SELECT mca.id AS FichaId, [Pergunta], [RespostaUnicaEscolha]
	   FROM [SIGSM_Crianca_Mais_2_Anos_Adolescente_Adulto_Gestante_Idoso] AS cmsm
 INNER JOIN SIGSM_Marcadores_Consumo_Alimentar AS mca
		 ON cmsm.id = mca.id_crianca_menor_seis_meses
CROSS APPLY (
	 VALUES (11, CASE 
				WHEN Upper([refeicao_assistindo_tv_mexendo_no_pc_e_ou_celular]) = Upper('Sim') THEN '1'
				WHEN Upper([refeicao_assistindo_tv_mexendo_no_pc_e_ou_celular]) = Upper('Nao') THEN '2'
				WHEN Upper([refeicao_assistindo_tv_mexendo_no_pc_e_ou_celular]) = Upper('Nao Sabe') THEN '3'
            ELSE Null
            END),
            (12, replace(replace(replace(replace(replace(replace(replace(		
                Upper([quais_reficoes_voce_faz]), 'CAFÉ DA MANHÃ', '12'), 
                upper('Lanche da Manhã'), '13'), 
                upper('Almoço'), '14'),
                upper('Lanche da Tarde'), '15'),
                upper('Jantar'), '16'),
                upper('Ceia'), '17'), ' ', '')),
            (14, CASE 
				WHEN Upper([ontem_voce_consumiu_feijao]) = Upper('Sim') THEN '1'
				WHEN Upper([ontem_voce_consumiu_feijao]) = Upper('Nao') THEN '2'
				WHEN Upper([ontem_voce_consumiu_feijao]) = Upper('Nao Sabe') THEN '3'
            ELSE Null
            END),
            (15, CASE 
				WHEN Upper([ontem_voce_consumiu_frutas_frescas]) = Upper('Sim') THEN '1'
				WHEN Upper([ontem_voce_consumiu_frutas_frescas]) = Upper('Nao') THEN '2'
				WHEN Upper([ontem_voce_consumiu_frutas_frescas]) = Upper('Nao Sabe') THEN '3'
            ELSE Null
            END),
            (16, CASE 
				WHEN Upper([ontem_voce_consumiu_verduras_e_ou_legumes]) = Upper('Sim') THEN '1'
				WHEN Upper([ontem_voce_consumiu_verduras_e_ou_legumes]) = Upper('Nao') THEN '2'
				WHEN Upper([ontem_voce_consumiu_verduras_e_ou_legumes]) = Upper('Nao Sabe') THEN '3'
            ELSE Null
            END),
            (17, CASE 
				WHEN Upper([ontem_voce_consumiu_hamburguer_e_ou_embutidos]) = Upper('Sim') THEN '1'
				WHEN Upper([ontem_voce_consumiu_hamburguer_e_ou_embutidos]) = Upper('Nao') THEN '2'
				WHEN Upper([ontem_voce_consumiu_hamburguer_e_ou_embutidos]) = Upper('Nao Sabe') THEN '3'
            ELSE Null
            END),
            (18, CASE 
				WHEN Upper([ontem_voce_consumiu_bebidas_adocadas]) = Upper('Sim') THEN '1'
				WHEN Upper([ontem_voce_consumiu_bebidas_adocadas]) = Upper('Nao') THEN '2'
				WHEN Upper([ontem_voce_consumiu_bebidas_adocadas]) = Upper('Nao Sabe') THEN '3'
            ELSE Null
            END),
            (19, CASE 
				WHEN Upper([ontem_voce_consumiu_macarrao_instantaneo_salgadinhos_de_pacote_biscoitos_salgados]) = Upper('Sim') THEN '1'
				WHEN Upper([ontem_voce_consumiu_macarrao_instantaneo_salgadinhos_de_pacote_biscoitos_salgados]) = Upper('Nao') THEN '2'
				WHEN Upper([ontem_voce_consumiu_macarrao_instantaneo_salgadinhos_de_pacote_biscoitos_salgados]) = Upper('Nao Sabe') THEN '3'
            ELSE Null
            END),
            (20, CASE 
				WHEN Upper([ontem_voce_consumiu_biscoito_recheado_doces]) = Upper('Sim') THEN '1'
				WHEN Upper([ontem_voce_consumiu_biscoito_recheado_doces]) = Upper('Nao') THEN '2'
				WHEN Upper([ontem_voce_consumiu_biscoito_recheado_doces]) = Upper('Nao Sabe') THEN '3'
            ELSE Null
            END)
			) AS a ([Pergunta], [RespostaUnicaEscolha])
GO

CREATE TABLE SIGSM_ComplementarZika_TesteOlhinho (
	id int primary key,
	descricao varchar(60) not null
)
GO

INSERT INTO SIGSM_ComplementarZika_TesteOlhinho VALUES
	(1, 'Presente bilateral'),
	(2, 'Duvidoso ou ausente')
GO

CREATE TABLE SIGSM_ComplementarZika_ExameFundoOlho (
	id int primary key,
	descricao varchar(60) not null
)
GO

INSERT INTO SIGSM_ComplementarZika_ExameFundoOlho VALUES
	(3, 'Normal'),
	(4, 'Alterado')
GO

CREATE TABLE SIGSM_ComplementarZika_TesteOrelhinha (
	id int primary key,
	descricao varchar(60) not null
)
GO

INSERT INTO SIGSM_ComplementarZika_TesteOrelhinha VALUES
	(5, 'Passou'),
	(6, 'Falhou')
GO

CREATE TABLE SIGSM_ComplementarZika_USTransfontanela (
	id int primary key,
	descricao varchar(60) not null
)
GO

INSERT INTO SIGSM_ComplementarZika_USTransfontanela VALUES
	(7, 'Normal'),
	(8, 'Sugestivo de infecção congênita'),
	(9, 'Outras alterações'),
	(10, 'Indeterminado')
GO

CREATE TABLE SIGSM_ComplementarZika_TomografiaComputadorizada (
	id int primary key,
	descricao varchar(60) not null
)
GO

INSERT INTO SIGSM_ComplementarZika_TomografiaComputadorizada VALUES
	(11, 'Normal'),
	(12, 'Sugestivo de infecção congênita'),
	(13, 'Outras alterações'),
	(14, 'Indeterminado')
GO

CREATE TABLE SIGSM_ComplementarZika_RessonanciaMagnetica (
	id int primary key,
	descricao varchar(60) not null
)
GO

INSERT INTO SIGSM_ComplementarZika_RessonanciaMagnetica VALUES
	(15, 'Normal'),
	(16, 'Sugestivo de infecção congênita'),
	(17, 'Outras alterações'),
	(18, 'Indeterminado')
GO

CREATE TABLE SIGSM_ComplementarZika (
	id bigint not null primary key identity,
	profissionalCns char(15) not null,
	cboCodigo_2002 char(6) not null,
	cnes char(7) not null,
	ine char(10) null,
	codigoIbgeMunicipio char(7) not null,
	dataAtendimento date not null,
	uuidFicha uniqueidentifier not null default newid(),
	turno int not null,
	cnsCidadao char(15) not null,
	cnsResponsavelFamiliar char(15) null,
	dataRealizacaoTesteOlhinho date null,
	coResultadoTesteOlhinho int null,
	dataRealizacaoExameFundoOlho date null,
	coResultadoExameFundoOlho int null,
	dataRealizacaoTesteOrelhinha date null,
	coResultadoTesteOrelhinha int null,
	dataRealizacaoUSTransfontanela date null,
	coResultadoUSTransfontanela int null,
	dataRealizacaoTomografiaComputadorizada date null,
	coResultadoTomografiaComputadorizada int null,
	dataRealizacaoRessonanciaMagnetica date null,
	coResultadoRessonanciaMagnetica int null,

	digitado_por int not null,
	data_cadastro datetime not null default getdate(),
	conferido_por int null,
	id_profissional_saude decimal(18, 0) null,
	num_contrato_profissional int null,
	enviado bit not null default 0,
	finalizado bit not null default 0,
	numero_folha bigint null,
	 CONSTRAINT FK_ComplementarZika_ASSMED_Cadastro
	FOREIGN KEY (num_contrato_profissional, id_profissional_saude) REFERENCES ASSMED_Cadastro(NumContrato, Codigo),
	 CONSTRAINT FK_ComplementarZika_Turno
	FOREIGN KEY (turno) REFERENCES SIGSM_Turno(id),
	 CONSTRAINT FK_ComplementarZika_TesteOlhinho
	FOREIGN KEY (coResultadoTesteOlhinho) REFERENCES SIGSM_ComplementarZika_TesteOlhinho(id),
	 CONSTRAINT FK_ComplementarZika_ExameFundoOlho
	FOREIGN KEY (coResultadoExameFundoOlho) REFERENCES SIGSM_ComplementarZika_ExameFundoOlho(id),
	 CONSTRAINT FK_ComplementarZika_TesteOrelhinha
	FOREIGN KEY (coResultadoTesteOrelhinha) REFERENCES SIGSM_ComplementarZika_TesteOrelhinha(id),
	 CONSTRAINT FK_ComplementarZika_USTransfontanela
	FOREIGN KEY (coResultadoUSTransfontanela) REFERENCES SIGSM_ComplementarZika_USTransfontanela(id),
	 CONSTRAINT FK_ComplementarZika_TomografiaComputadorizada
	FOREIGN KEY (coResultadoTomografiaComputadorizada) REFERENCES SIGSM_ComplementarZika_TomografiaComputadorizada(id),
	 CONSTRAINT FK_ComplementarZika_RessonanciaMagnetica
	FOREIGN KEY (coResultadoRessonanciaMagnetica) REFERENCES SIGSM_ComplementarZika_RessonanciaMagnetica(id)
)
GO

CREATE VIEW VW_FichaComplementar AS
	 SELECT c.uuidFicha AS UuidFicha,
			3 AS TpCdsOrigem,
			c.turno AS Turno,
			c.cnsCidadao AS CnsCidadao,
			c.cnsResponsavelFamiliar AS CnsResponsavelFamiliar,
			c.dataRealizacaoTesteOlhinho AS DataRealizacaoTesteOlhinho,
			c.coResultadoTesteOlhinho AS CoResultadoTesteOlhinho,
			c.dataRealizacaoExameFundoOlho AS DataRealizacaoExameFundoOlho,
			c.coResultadoExameFundoOlho AS CoResultadoExameFundoOlho,
			c.dataRealizacaoTesteOrelhinha AS DataRealizacaoTesteOrelhinha,
			c.coResultadoTesteOrelhinha AS CoResultadoTesteOrelhinha,
			c.dataRealizacaoUSTransfontanela AS DataRealizacaoUSTransfontanela,
			c.coResultadoUSTransfontanela AS CoResultadoUSTransfontanela,
			c.dataRealizacaoTomografiaComputadorizada AS DataRealizacaoTomografiaComputadorizada,
			c.coResultadoTomografiaComputadorizada AS CoResultadoTomografiaComputadorizada,
			c.dataRealizacaoRessonanciaMagnetica AS DataRealizacaoRessonanciaMagnetica,
			c.coResultadoRessonanciaMagnetica AS CoResultadoRessonanciaMagnetica,
			c.dataAtendimento AS DataAtendimento
	   FROM SIGSM_ComplementarZika AS c
	  WHERE c.enviado = 0 AND c.finalizado = 1
GO

CREATE VIEW VW_FichaComplementar_UnicaLotacaoHeader AS
	 SELECT c.uuidFicha AS UuidFicha,
			c.profissionalCns AS ProfissionalCns,
			c.cnes AS Cnes,
			c.codigoIbgeMunicipio AS CodigoIbgeMunicipio,
			c.ine AS Ine,
			c.cboCodigo_2002 AS CboCodigo_2002,
			c.dataAtendimento AS DataAtendimento
	   FROM SIGSM_ComplementarZika AS c
	  WHERE c.enviado = 0 AND c.finalizado = 1
GO

CREATE VIEW VW_FichaProcedimentoMaster AS
	 select guid_esus as UuidFicha,
            3 as TpCdsOrigem,
            afericao_pa as NumTotalAfericaoPa,
            glicemia_capilar as NumTotalGlicemiaCapilar,
            afericao_temperatura as NumTotalAfericaoTemperatura,
            medicao_altura as NumTotalMedicaoAltura,
            curativo_simples as NumTotalCurativoSimples,
            medicao_peso as NumTotalMedicaoPeso,
            coleta_material as NumTotalColetaMaterialParaExameLaboratorial,
            CAST(CASE enviado WHEN 1 THEN 1 ELSE 0 END AS BIT) as Enviado,
			data_procedimento as DataAtendimento
    from SIGSM_Procedimento
    where (enviado = 0 or enviado is null) and id_status = 2
GO

CREATE VIEW VW_FichaProcedimentoChild AS
	 select pro.guid_esus as UuidFicha,
            usu.numero_prontuario AS NumProntuario,
            usu.Numero_Cartao_Sus as CnsCidadao,
            usu.data_nascimento as DtNascimento,
            sex.codigo AS Sexo,
            usu.local_atendimento as LocalAtendimento,
            tur.id AS Turno,
            CAST(CASE
				WHEN usu.escuta_inicial_orientacao is null or usu.escuta_inicial_orientacao = 0 THEN 0
				ELSE 1
            END AS BIT) as StatusEscutaInicialOrientacao,
			STUFF((
			  select ',' + codigoab
				from sigsm_procedimento_usuario u   left join sigsm_procedimento pro        on ( u.id_procedimento = pro.id )
									                left join assmed_cadastro cad           on ( u.id_identificacao_usuario = cad.codigo )
									                left join assmed_cadastrodocpessoal dpe on ( dpe.numcontrato = cad.numcontrato ) 
									                                                        and ( dpe.codigo = cad.codigo ) 
																		                    and ( dpe.codtpdocp = 6 )
			cross apply (
				VALUES
                    (case u.acupuntura_com_insercao_de_agulhas WHEN 1 THEN 'ABPG001' --'ACUPUNTURA COM INSERÇÃO DE AGULHAS'
                    end),
					(case u.administracao_de_vitamina_a WHEN 1 THEN 'ABPG002' --'ADMINISTRAÇÃO DE VITAMINA A'
                    end),
					(case u.cateterismo_vesical_de_alivio WHEN 1 THEN 'ABPG003' --'CATETERISMO VESICAL DE ALIVIO'
                    end),
	                (case u.cauterizacao_quimica_de_pequenas_lesoes WHEN 1 THEN 'ABPG004' --'CAUTERIZAÇÃO QUIMICA DE PEQUENAS LESÕES'
                    end),
	                (case u.cirurgia_de_unha_cantoplastia WHEN 1 THEN 'ABPG005' --'CIRURGIA DE UNHA (CANTOPLASTIA)'
                    end),
	                (case u.cuidado_de_estomas WHEN 1 THEN 'ABPG006' --'CUIDADO DE ESTOMAS'
                    end),
	                (case u.curativo_especial WHEN 1 THEN 'ABPG007' --'CURATIVO ESPECIAL'
                    end),
	                (case u.drenagem_de_abscesso WHEN 1 THEN 'ABPG008' --'DRENAGEM DE ABSCESSO'
                    end),
	                (case u.eletrocardiograma WHEN 1 THEN 'ABEX004' --'ELETROCARDIOGRAMA'
                    end),
	                (case u.coleta_de_citopatologico_de_colo_uterino WHEN 1 THEN 'ABPG010' --'COLETA DE CITOPATOLOGICO DE COLO UTERINO'
                    end),
	                (case u.exame_do_pe_diabetico WHEN 1 THEN 'ABPG011' --'EXAME DO PÉ DIABÉTICO'
                    end),
	                (case u.exerese_biopsia_puncao_de_tumores_superficiais_de_pele WHEN 1 THEN 'ABPG012' --'EXÉRESE / BIÓPSIA / PUNÇÃO DE TUMORES SUPERFICIAIS DE PELE'
                    end),
	                (case u.fundoscopia_exame_de_fundo_de_olho WHEN 1 THEN 'ABPG013' --'FUNDOSCOPIA (EXAME DE FUNDO DE OLHO)'
                    end),
	                (case u.infiltracao_em_cavidade_sinovial WHEN 1 THEN 'ABPG014' --'INFILTRACÃO EM CAVIDADE SINOVIAL'
                    end),
	                (case u.remocao_de_corpo_estranho_da_cavidade_auditiva_e_nasal WHEN 1 THEN 'ABPG015' --'REMOÇÃO DE CORPO ESTRANHO DA CAVIDADE AUDITIVA E NASAL' -- Não fazia parte do select da MartTech
                    end),
    	            (case u.remocao_de_corpo_estranho_subcutaneo WHEN 1 THEN 'ABPG016'--'REMOÇÃO DE CORPO ESTRANHO SUBCUTÂNEO'
                    end),
	                (case u.retirada_de_cerume WHEN 1 THEN 'ABPG017' --'RETIRADA DE CERUME'
                    end),
	                (case u.retirada_de_pontos_de_cirurgias WHEN 1 THEN 'ABPG018' --'RETIRADA DE PONTOS DE CIRURGIAS'
                    end),
	                (case u.sutura_simples WHEN 1 THEN 'ABPG019' --'SUTURA SIMPLES'
                    end),
	                (case u.triagem_oftalmologica WHEN 1 THEN 'ABPG020' --'TRIAGEM OFTALMOLÓGICA'
                    end),
	                (case u.tamponamento_de_epistaxe WHEN 1 THEN 'ABPG021' --'TAMPONAMENTO DE EPISTAXE'
                    end),
	                (case u.teste_rapido_de_gravides WHEN 1 THEN 'ABPG022' -- 'TESTE RÁPIDO DE GRAVIDES'
                    end),
	                (case u.teste_rapido_dosagem_de_proteinuria WHEN 1 THEN 'ABPG040' --'TESTE RÁPIDO DOSAGEM DE PROTEINURIA'
                    end),
	                (case u.teste_rapido_para_hiv WHEN 1 THEN 'ABPG024' -- 'TESTE RÁPIDO PARA HIV'
                    end),
	                (case u.teste_rapido_para_hepatite_c WHEN 1 THEN 'ABPG025' --'TESTE RÁPIDO PARA HEPATITE C'
                    end),
	                (case u.teste_rapido_para_sifilis WHEN 1 THEN 'ABPG026' --'TESTE RÁPIDO PARA SÍFILIS'
                    end),
	                (case u.admin_de_medicamentos_oral WHEN 1 THEN 'ABPG027' --'ADMINISTRAÇÃO DE MEDICAMENTOS VIA ORAL'
                    end),
	                (case u.admin_de_medicamentos_intramuscular WHEN 1 THEN 'ABPG028' --'ADMINISTRAÇÃO DE MEDICAMENTOS VIA INTRAMUSCULAR'
                    end),
	                (case u.admin_de_medicamentos_endovenosa WHEN 1 THEN 'ABPG029' --'ADMINISTRAÇÃO DE MEDICAMENTOS VIA ENDOVENOSA'
                    end),
	                (case u.admin_de_medicamentos_inalacao_nebulizacao WHEN 1 THEN 'ABPG030' --'ADMINISTRAÇÃO DE MEDICAMENTOS VIA INALAÇÃO / NEBULIZAÇÃO'
                    end),
	                (case u.admin_de_medicamentos_topica WHEN 1 THEN 'ABPG031' --'ADMINISTRAÇÃO DE MEDICAMENTOS VIA TÓPICA'
                    end),
	                (case u.admin_de_medicamentos_penicilina_para_tratamento_de_sifilis WHEN 1 THEN 'ABPG032' --'ADMINISTRAÇÃO DE PENICILINA PARA TRATAMENTO DE SÍFILIS'
                    end)
				) c (codigoab)
                where u.id = usu.id
				FOR XML PATH(''), TYPE
			).value('.', 'nvarchar(max)'), 1, 1, '') AS LstProcedimentos,
			STUFF((select distinct ',' + rtrim(ltrim(tproc.CodProcTab))
                     from SIGSM_Procedimento sigsmProc left join SIGSM_Procedimento_Sigtap sigtap on (sigsmProc.id = sigtap.id_procedimento)
                                                       left join AS_Procedimentos procedimento    on (procedimento.CodProc = sigtap.CodProc )
								                       left join AS_TabelaProc tproc              on (procedimento.CodProc = tproc.CodProc )
                    where sigsmProc.guid_esus = pro.guid_esus and sigsmProc.id_status = 2
					FOR XML PATH(''), TYPE
			).value('.', 'nvarchar(max)'), 1, 1, '') AS LstOutrosSiaProcedimentos
    from SIGSM_Procedimento_Usuario usu left join SIGSM_Procedimento pro        on ( usu.id_procedimento = pro.id )
                                        left join ASSMED_Cadastro cad           on ( usu.id_identificacao_usuario = cad.Codigo )
                                        left join ASSMED_CadastroDocPessoal dpe on ( dpe.NumContrato = cad.NumContrato ) 
                                                                                and ( dpe.Codigo = cad.Codigo ) 
                                                                                and ( dpe.CodTpDocP = 6 )	
										left join TP_Sexo sex					on usu.sexo = sex.sigla
										left join SIGSM_Turno tur				on usu.turno = tur.sigla
    where pro.id_status = 2
GO

CREATE VIEW VW_FichaProcedimento_UnicaLotacaoHeader AS
	 select pro.guid_esus as UuidFicha,
	        dpe.Numero as ProfissionalCNS,
	        rtrim(ltrim(pro.cbo)) as CboCodigo_2002,
	        pro.codigo_cnes_unidade as Cnes,
	        sti.Numero as Ine,
	        pro.data_procedimento as DataAtendimento,
	        --pro.codibge_munic_nasc as CodigoIbgeMunicipio
	        COALESCE(pro.codibge_munic_nasc, cont.CodigoIbgeMunicipio) as CodigoIbgeMunicipio
        from SIGSM_Procedimento pro
	   inner join ASSMED_Contratos AS cont ON cont.NumContrato = 22
        left join ASSMED_Cadastro cad           on ( pro.id_profissional_saude = cad.Codigo )
        left join ASSMED_CadastroDocPessoal dpe on ( dpe.NumContrato = cad.NumContrato ) 
                                                and ( dpe.Codigo = cad.Codigo ) 
									            and ( dpe.CodTpDocP = 6 )
        left join SetoresINEs sti               on ( pro.codigo_equipe_ine = sti.CodINE )
 
        where pro.id_status = 2
GO

CREATE TABLE SIGSM_Versao (
	Major int NOT NULL,
	Minor int NOT NULL,
	Revision int NOT NULL,
	Ativo bit NOT NULL,
	primary key (Major, Minor, Revision)
)
GO

INSERT INTO SIGSM_Versao VALUES
	(2, 0, 0, 0),
	(2, 1, 0, 1)
GO

CREATE TABLE SIGSM_DadoInstalacao (
	uuidInstalacao uniqueidentifier not null,
	origem bit not null,
	contraChave varchar(120) not null,
	cpfOuCnpj varchar(15) not null,
	nomeOuRazaoSocial varchar(255) not null,
	fone varchar(11) not null,
	email varchar(255) not null,
	nomeBancoDados varchar(50) not null,
	versaoSistema varchar(8) not null,
	primary key(uuidInstalacao, origem)
)
GO

INSERT INTO SIGSM_DadoInstalacao VALUES
	('b9f2d27c-b21a-4fb8-90f2-20ddd8e01df6', 1, 'SIGSM - 1.0', '83111056856', 'Softpark Informatica Ltda', '1133333333', 'atendimento@softpark.com.br', 'SIGSM', '1.1'),
	('b9f2d27c-b21a-4fb8-90f2-20ddd8e01df6', 0, 'SIGSM - 1.0', '83111056856', 'Softpark Informatica Ltda', '1133333333', 'atendimento@softpark.com.br', 'SIGSM', '1.1')
GO

CREATE VIEW VW_FichaVisitaDomiciliarTerritorialMaster AS
	 SELECT ficha.uuidFicha AS UuidFicha,
			3 AS TpCdsOrigem,
			header.dataAtendimento AS DataAtendimento
	   FROM api.UnicaLotacaoTransport AS header
 INNER JOIN api.FichaVisitaDomiciliarMaster AS ficha
		 ON header.id = ficha.headerTransport
 INNER JOIN api.OrigemVisita AS origem
		 ON header.token = origem.token
	  WHERE origem.enviado = 0
		AND origem.finalizado = 1
		AND origem.enviarParaThrift = 1
GO

CREATE VIEW VW_FichaVisitaDomiciliarTerritorialChild AS
	 SELECT ficha.uuidFicha AS UuidFicha,
			child.turno AS Turno,
			child.numProntuario AS NumProntuario,
			child.cnsCidadao AS CnsCidadao,
			child.dtNascimento AS DtNascimento,
			child.sexo AS Sexo,
			child.statusVisitaCompartilhadaOutroProfissional AS StatusVisitaCompartilhadaOutroProfissional,
			STUFF((SELECT ',' + CAST(motivo.codigo AS NVARCHAR)
					 FROM api.FichaVisitaDomiciliarChild_MotivoVisita AS motivo
					WHERE motivo.childId = child.childId
					  FOR XML PATH(''), TYPE
			).value('.', 'nvarchar(max)'), 1, 1, '') AS LstMotivosVisita,
			child.desfecho AS Desfecho,
			child.microarea AS MicroArea,
			child.stForaArea AS StForaArea,
			child.tipoDeImovel AS TipoDeImovel,
			child.pesoAcompanhamentoNutricional AS PesoAcompanhamentoNutricional,
			child.alturaAcompanhamentoNutricional AS AlturaAcompanhamentoNutricional,
			header.dataAtendimento AS DataAtendimento
	   FROM api.FichaVisitaDomiciliarChild AS child
 INNER JOIN api.FichaVisitaDomiciliarMaster AS ficha
		 ON child.uuidFicha = ficha.uuidFicha
 INNER JOIN api.UnicaLotacaoTransport AS header
		 ON ficha.headerTransport = header.id
GO

CREATE VIEW VW_FichaVisitaDomiciliarTerritorial_UnicaLotacaoHeader AS
	 SELECT ficha.uuidFicha AS UuidFicha,
			header.cboCodigo_2002 AS CboCodigo_2002,
			header.cnes AS Cnes,
			header.codigoIbgeMunicipio AS CodigoIbgeMunicipio,
			header.dataAtendimento AS DataAtendimento,
			header.ine AS Ine,
			header.profissionalCNS AS ProfissionalCNS
	   FROM api.UnicaLotacaoTransport AS header
 INNER JOIN api.FichaVisitaDomiciliarMaster AS ficha
		 ON header.id = ficha.headerTransport
GO

UPDATE Nacionalidade SET DesNacao = 'AZERBAIDJÃO' WHERE CodNacao = 138;
UPDATE Nacionalidade SET DesNacao = 'ANTIGUA E BARBUDA' WHERE CodNacao = 28;
UPDATE Nacionalidade SET DesNacao = 'ANDORRA' WHERE CodNacao = 94;
UPDATE Nacionalidade SET DesNacao = 'BAREIN' WHERE CodNacao = 243;
UPDATE Nacionalidade SET DesNacao = 'BERMUDA' WHERE CodNacao = 83;
UPDATE Nacionalidade SET DesNacao = 'BUTÃO' WHERE CodNacao = 246;
UPDATE Nacionalidade SET DesNacao = 'CAZAQUISTÃO' WHERE CodNacao = 143;
UPDATE Nacionalidade SET DesNacao = 'TAIWAN' WHERE CodNacao = 249;
UPDATE Nacionalidade SET DesNacao = 'BAHAMAS' WHERE CodNacao = 40;
UPDATE Nacionalidade SET DesNacao = 'DOMINICA' WHERE CodNacao = 54;
UPDATE Nacionalidade SET DesNacao = 'CORÉIA DO SUL' WHERE CodNacao = 43;
UPDATE Nacionalidade SET DesNacao = 'VATICANO' WHERE CodNacao = 129;
UPDATE Nacionalidade SET DesNacao = 'ESTADOS UNIDOS DA AMÉRICA' WHERE CodNacao = 36;
UPDATE Nacionalidade SET DesNacao = 'GRÃ-BRETANHA' WHERE CodNacao = 32;
UPDATE Nacionalidade SET DesNacao = 'GROENLÂNDIA' WHERE CodNacao = 84;
UPDATE Nacionalidade SET DesNacao = 'IUGOSLÁVIA' WHERE CodNacao = 114;
UPDATE Nacionalidade SET DesNacao = 'MALAUÍ' WHERE CodNacao = 204;
UPDATE Nacionalidade SET DesNacao = 'MOLDÁVIA' WHERE CodNacao = 158;
UPDATE Nacionalidade SET DesNacao = 'MONTSERRAT' WHERE CodNacao = 70;
UPDATE Nacionalidade SET DesNacao = 'EGITO' WHERE CodNacao = 189;
UPDATE Nacionalidade SET DesNacao = 'ÁFRICA DO SUL' WHERE CodNacao = 173;
UPDATE Nacionalidade SET DesNacao = 'BELARUS' WHERE CodNacao = 140;
UPDATE Nacionalidade SET DesNacao = 'MACEDÔNIA' WHERE CodNacao = 133;
UPDATE Nacionalidade SET DesNacao = 'EL SALVADOR' WHERE CodNacao = 56;
UPDATE Nacionalidade SET DesNacao = 'MALTA' WHERE CodNacao = 120;
UPDATE Nacionalidade SET DesNacao = 'GABÃO' WHERE CodNacao = 191;
UPDATE Nacionalidade SET DesNacao = 'HAITI' WHERE CodNacao = 62;
UPDATE Nacionalidade SET DesNacao = 'NIGER' WHERE CodNacao = 212;
UPDATE Nacionalidade SET DesNacao = 'GUIANA' WHERE CodNacao = 88;
UPDATE Nacionalidade SET DesNacao = 'ZIMBÁBUE' WHERE CodNacao = 220;
UPDATE Nacionalidade SET DesNacao = 'ZIMBÁBUE' WHERE CodNacao = 239;
UPDATE Nacionalidade SET DesNacao = 'SAHARA OCIDENTAL' WHERE CodNacao = 222;
UPDATE Nacionalidade SET DesNacao = 'SÃO CRISTÓVÃO E NÉVIS' WHERE CodNacao = 78;
UPDATE Nacionalidade SET DesNacao = 'SÃO VICENTE E GRANADINAS' WHERE CodNacao = 79;
UPDATE Nacionalidade SET DesNacao = 'SEICHELES' WHERE CodNacao = 225;
UPDATE Nacionalidade SET DesNacao = 'SÃO PIERRE E MIQUELON' WHERE CodNacao = 86;
UPDATE Nacionalidade SET DesNacao = 'ILHAS SVALBARD E JAN MAYEN' WHERE CodNacao = 127;
UPDATE Nacionalidade SET DesNacao = 'ILHAS SANTA HELENA' WHERE CodNacao = 223;
UPDATE Nacionalidade SET DesNacao = 'TURCOMÊNIA' WHERE CodNacao = 163;
UPDATE Nacionalidade SET DesNacao = 'UZBEQUISTÃO' WHERE CodNacao = 168;
UPDATE Nacionalidade SET DesNacao = 'COMORES' WHERE CodNacao = 184;
UPDATE Nacionalidade SET DesNacao = 'ESTADO DA PALESTINA' WHERE CodNacao = 194;
UPDATE Nacionalidade SET DesNacao = 'GUADALUPE' WHERE CodNacao = 60;
UPDATE Nacionalidade SET DesNacao = 'TURQUIA' WHERE CodNacao = 80;
UPDATE Nacionalidade SET DesNacao = 'ILHAS VIRGENS' WHERE CodNacao = 82;
UPDATE Nacionalidade SET DesNacao = 'SAN MARINO' WHERE CodNacao = 125;
UPDATE Nacionalidade SET DesNacao = 'ILHAS VIRGENS' WHERE CodNacao = 82;
GO

UPDATE Nacionalidade SET codigo = b.codigo
from Nacionalidade a
INNER JOIN Paises b
ON REPLACE(dbo.fnTiraAcento(LTRIM(RTRIM(a.DesNacao))), '-', ' ') COLLATE Latin1_General_CI_AI =
   REPLACE(dbo.fnTiraAcento(LTRIM(RTRIM(b.nome))), '-', ' ') COLLATE Latin1_General_CI_AI
WHERE a.codigo IS NULL
GO

UPDATE Nacionalidade SET codigo = 96 WHERE CodNacao = 63;
UPDATE Nacionalidade SET codigo = 115 WHERE CodNacao = 219;
UPDATE Nacionalidade SET codigo = 128 WHERE CodNacao = 111;
UPDATE Nacionalidade SET codigo = 180 WHERE CodNacao = 73;
GO

DELETE FROM Nacionalidade WHERE CodNacao IN (157, 203);
GO

ALTER TABLE [api].[IdentificacaoUsuarioCidadao]
	   DROP CONSTRAINT FK_IdentificacaoUsuarioCidadao_Paises
GO

DROP TABLE [dbo].[Paises]
GO

CREATE VIEW VW_MenuSistema AS
     SELECT _pi.id_menu_pai AS id_pai_indireto,
            m.id_menu_pai AS id_pai_direto,
            m.id_menu,
            m.link,
            m.sublink,
            m.ordem,
            m.id_sistema,
            m.icone,
            m.descricao
       FROM menu AS m
  LEFT JOIN menu AS _pi
         ON m.id_menu_pai = _pi.id_menu
      WHERE m.ativo = 1
GO

CREATE VIEW VW_SystemLanguage AS
     SELECT *
       FROM SYS.SYSLANGUAGES
      WHERE NAME = @@LANGUAGE
GO

ALTER TABLE dbo.ASSMED_Cadastro
		ADD IdFicha uniqueidentifier REFERENCES api.IdentificacaoUsuarioCidadao (id)
GO

ALTER TABLE dbo.ASSMED_Endereco
		ADD IdFicha uniqueidentifier REFERENCES api.EnderecoLocalPermanencia (id)
GO

ALTER TABLE dbo.ASSMED_Cadastro
        ADD idAuto bigint identity
GO

CREATE VIEW dbo.VW_ConsultaCadastrosIndividuais AS
	 SELECT x.Nome COLLATE Latin1_General_CI_AI AS NomeCidadao,
			pf.DtNasc AS DataNascimento,
			pf.NomeMae COLLATE Latin1_General_CI_AI AS NomeMae,
			cn.Numero COLLATE Latin1_General_CI_AI AS CnsCidadao,
			ci.NomeCidade COLLATE Latin1_General_CI_AI AS MunicipioNascimento,
			x.Codigo,
			l.idCadastroIndividual AS IdFicha
	   FROM dbo.ASSMED_Cadastro AS x
  LEFT JOIN api.VW_ultimo_cadastroIndividual AS l
		 ON x.Codigo = l.Codigo
  LEFT JOIN dbo.ASSMED_PesFisica AS pf
		 ON x.Codigo = pf.Codigo
  LEFT JOIN dbo.Cidade AS ci
		 ON pf.MUNICIPIONASC = ci.CodCidade
  LEFT JOIN dbo.ASSMED_CadastroDocPessoal AS cn
		 ON x.Codigo = cn.Codigo
		AND cn.CodTpDocP = 6
		AND cn.Numero IS NOT NULL
		AND LEN(LTRIM(RTRIM(cn.Numero))) > 0
	  WHERE x.Nome IS NOT NULL
		AND LEN(LTRIM(RTRIM(x.Nome))) > 0
		AND x.Nome NOT LIKE '%*%'
   GROUP BY x.Nome,
			pf.DtNasc,
			pf.NomeMae,
			cn.Numero,
			ci.NomeCidade,
			x.Codigo,
			l.idCadastroIndividual
GO

ALTER TABLE ASSMED_CadTelefones
		ADD IDTelefone BIGINT NOT NULL PRIMARY KEY IDENTITY
GO

ALTER TABLE api.IdentificacaoUsuarioCidadao
		ADD Codigo decimal(18, 0)
GO

ALTER TABLE api.IdentificacaoUsuarioCidadao
		ADD CONSTRAINT FK_Identificacao_Cadastro
			FOREIGN KEY (num_contrato, Codigo)
			REFERENCES dbo.ASSMED_Cadastro (NumContrato, Codigo)
GO

CREATE NONCLUSTERED INDEX [IX_ASSMED_Cadastro] ON [dbo].[ASSMED_Cadastro]
(
	[Codigo] ASC,
	[Nome] ASC,
	[NomeSocial] ASC,
	[IdFicha] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ASSMED_Cadastro_Doc_Pessoal] ON [dbo].[ASSMED_CadastroDocPessoal]
(
	[Numero] ASC,
	[CodTpDocP] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Iden_UsuCid] ON [api].[IdentificacaoUsuarioCidadao]
(
	[Id] ASC,
	[Codigo] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_CadInd] ON [api].[CadastroIndividual]
(
	[id] ASC,
	[identificacaoUsuarioCidadao] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ASSMED_PesFisica] ON [dbo].[ASSMED_PesFisica]
(
	[Codigo] ASC,
	[NomeMae] ASC,
	[DtNasc] ASC,
	[MUNICIPIONASC] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Cidade] ON [dbo].[Cidade]
(
	CodCidade ASC,
	NomeCidade ASC
)
GO

CREATE PROCEDURE [dbo].[PR_ConsultaCadastroIndividuais] (@search NVARCHAR(MAX), @skip INT, @take INT, @orderCol INT, @orderDirection INT, @total INT OUT, @totalFiltered INT OUT) AS
BEGIN
	SET NOCOUNT ON;
	SET LANGUAGE brazilian;

    declare @isNum BIT,
			@isGuid BIT,
			@isDate BIT,
			@isCns BIT,
			@len INT;
	declare @sDate DATETIME,
			@sCNS CHAR(15),
			@sCodigo DECIMAL(18, 0),
			@sGuid UNIQUEIDENTIFIER;
	declare @cads TABLE (
				Nome NVARCHAR(512),
				DtNasc NVARCHAR(512),
				NomeMae NVARCHAR(512),
				Cns NVARCHAR(512),
				NomeCidade NVARCHAR(512),
				Codigo NVARCHAR(512)
			);
	
	DECLARE @skips int = @skip + 1;
	DECLARE @takes int = @skip + @take;
	DECLARE @column NVARCHAR(50);

	 SELECT @len = LEN(@search);

	 SELECT @isNum = ISNUMERIC(@search),
			@isGuid = (CASE WHEN @search LIKE REPLACE('00000000-0000-0000-0000-000000000000', '0', '[0-9a-fA-F]') THEN 1 ELSE 0 END),
			@isDate = ISDATE(@search),
			@isCns = (CASE WHEN @len = 15 AND ISNUMERIC(@search) = 1 AND SUBSTRING(@search, 1, 1) IN ('1', '2', '7', '8', '9') THEN 1 ELSE 0 END);
			
	 SELECT @totalFiltered = COUNT(Codigo) FROM ASSMED_Cadastro WHERE Nome IS NOT NULL AND LEN(RTRIM(LTRIM(Nome))) > 0 AND Nome NOT LIKE '%*%';
	 SELECT @total = (CASE WHEN @totalFiltered > 100 THEN 100 ELSE @total END);

	 SELECT @sDate = (CASE @isDate WHEN 1 THEN @search ELSE NULL END),
			@sCns = (CASE @isCns WHEN 1 THEN @search ELSE NULL END),
			@sGuid = (CASE @isGuid WHEN 1 THEN @search ELSE NULL END),
			@sCodigo = (CASE WHEN (@isCns = 0 AND @isNum = 1) THEN CAST(@search AS decimal(18, 0)) ELSE NULL END);

	IF @orderDirection = 0
	BEGIN
	 SELECT DISTINCT * FROM (
	 SELECT TOP 100 PERCENT
			Nome,
			DtNasc,
			NomeMae,
			Cns,
			NomeCidade,
			Codigo
	   FROM (
	 SELECT ac.Nome COLLATE Latin1_General_CI_AI AS [Nome],
			CONVERT(CHAR, CAST(pf.DtNasc AS DATE), 103) AS DtNasc,
			pf.NomeMae COLLATE Latin1_General_CI_AI AS [NomeMae],
			SUBSTRING(LTRIM(RTRIM(cns.Numero)), 1, 15) COLLATE Latin1_General_CI_AI AS Cns,
			cid.NomeCidade COLLATE Latin1_General_CI_AI AS [NomeCidade],
			CAST(CAST(ac.Codigo AS INT) AS NVARCHAR(MAX)) AS Codigo,
			ROW_NUMBER() OVER (ORDER BY
			(CASE @orderCol WHEN 0 THEN ac.Nome COLLATE Latin1_General_CI_AI
					WHEN 1 THEN CONVERT(CHAR, pf.DtNasc, 126)
					WHEN 2 THEN pf.NomeMae COLLATE Latin1_General_CI_AI
					WHEN 3 THEN cns.Numero COLLATE Latin1_General_CI_AI
					WHEN 4 THEN cid.NomeCidade COLLATE Latin1_General_CI_AI
					ELSE CAST(CAST(ac.Codigo AS BIGINT) AS NVARCHAR(30)) END) ASC, ac.Nome COLLATE Latin1_General_CI_AI ASC) AS _row
	   FROM ASSMED_Cadastro AS ac
  LEFT JOIN api.VW_ultimo_cadastroIndividual AS vci
		 ON ac.Codigo = vci.Codigo
  LEFT JOIN ASSMED_PesFisica AS pf
		 ON pf.Codigo = ac.Codigo
  LEFT JOIN api.CadastroIndividual AS ci
		 ON vci.idCadastroIndividual = ci.id
  LEFT JOIN ASSMED_CadastroDocPessoal AS cns
		 ON ac.Codigo = cns.Codigo
		AND cns.CodTpDocP = 6
  LEFT JOIN Cidade as cid
		 ON pf.MUNICIPIONASC = cid.CodCidade
	  WHERE ac.Nome IS NOT NULL
		AND LEN(RTRIM(LTRIM(ac.Nome COLLATE Latin1_General_CI_AI))) > 0
		AND ac.Nome COLLATE Latin1_General_CI_AI NOT LIKE '%*%'
		AND ((@len = 0 OR @search IS NULL) OR
			(@isCns = 1 AND cns.Numero COLLATE Latin1_General_CI_AI = @sCNS COLLATE Latin1_General_CI_AI) OR
			(@isGuid = 1 AND ci.id = @sGuid) OR
			(@isCns = 0 AND @isNum = 1 AND ac.Codigo = @sCodigo) OR
			(@isDate = 1 AND pf.DtNasc = @sDate) OR
			(@isCns = 1 AND cns.Numero COLLATE Latin1_General_CI_AI = @sCNS COLLATE Latin1_General_CI_AI) OR
			(@isCns = 0 AND @isGuid = 0 AND @isDate = 0 AND (
				cid.NomeCidade COLLATE Latin1_General_CI_AI LIKE '%' + @search + '%' OR
				ac.Nome COLLATE Latin1_General_CI_AI LIKE '%' + @search + '%' OR
				pf.NomeMae COLLATE Latin1_General_CI_AI LIKE '%' + @search + '%'
			)))
		  ) AS _
	  WHERE _row BETWEEN @skips AND @takes
   ORDER BY _row) AS a;
	END
	ELSE
	BEGIN
	 SELECT DISTINCT * FROM (
	 SELECT TOP 100 PERCENT
			Nome,
			DtNasc,
			NomeMae,
			Cns,
			NomeCidade,
			Codigo
	   FROM (
	 SELECT ac.Nome COLLATE Latin1_General_CI_AI AS [Nome],
			CONVERT(CHAR, CAST(pf.DtNasc AS DATE), 103) AS DtNasc,
			pf.NomeMae COLLATE Latin1_General_CI_AI AS [NomeMae],
			SUBSTRING(LTRIM(RTRIM(cns.Numero)), 1, 15) COLLATE Latin1_General_CI_AI AS Cns,
			cid.NomeCidade COLLATE Latin1_General_CI_AI AS [NomeCidade],
			CAST(CAST(ac.Codigo AS INT) AS NVARCHAR(MAX)) AS Codigo,
			ROW_NUMBER() OVER (ORDER BY
			(CASE @orderCol WHEN 0 THEN ac.Nome COLLATE Latin1_General_CI_AI
					WHEN 1 THEN CONVERT(CHAR, pf.DtNasc, 126)
					WHEN 2 THEN pf.NomeMae COLLATE Latin1_General_CI_AI
					WHEN 3 THEN cns.Numero COLLATE Latin1_General_CI_AI
					WHEN 4 THEN cid.NomeCidade COLLATE Latin1_General_CI_AI
					ELSE CAST(CAST(ac.Codigo AS BIGINT) AS NVARCHAR(30)) END) DESC, ac.Nome COLLATE Latin1_General_CI_AI DESC) AS _row
	   FROM ASSMED_Cadastro AS ac
  LEFT JOIN api.VW_ultimo_cadastroIndividual AS vci
		 ON ac.Codigo = vci.Codigo
  LEFT JOIN ASSMED_PesFisica AS pf
		 ON pf.Codigo = ac.Codigo
  LEFT JOIN api.CadastroIndividual AS ci
		 ON vci.idCadastroIndividual = ci.id
  LEFT JOIN ASSMED_CadastroDocPessoal AS cns
		 ON ac.Codigo = cns.Codigo
		AND cns.CodTpDocP = 6
  LEFT JOIN Cidade as cid
		 ON pf.MUNICIPIONASC = cid.CodCidade
	  WHERE ac.Nome IS NOT NULL
		AND LEN(RTRIM(LTRIM(ac.Nome COLLATE Latin1_General_CI_AI))) > 0
		AND ac.Nome COLLATE Latin1_General_CI_AI NOT LIKE '%*%'
		AND ((@len = 0 OR @search IS NULL) OR
			(@isCns = 1 AND cns.Numero COLLATE Latin1_General_CI_AI = @sCNS COLLATE Latin1_General_CI_AI) OR
			(@isGuid = 1 AND ci.id = @sGuid) OR
			(@isCns = 0 AND @isNum = 1 AND ac.Codigo = @sCodigo) OR
			(@isDate = 1 AND pf.DtNasc = @sDate) OR
			(@isCns = 1 AND cns.Numero COLLATE Latin1_General_CI_AI = @sCNS COLLATE Latin1_General_CI_AI) OR
			(@isCns = 0 AND @isGuid = 0 AND @isDate = 0 AND (
				cid.NomeCidade COLLATE Latin1_General_CI_AI LIKE '%' + @search + '%' OR
				ac.Nome COLLATE Latin1_General_CI_AI LIKE '%' + @search + '%' OR
				pf.NomeMae COLLATE Latin1_General_CI_AI LIKE '%' + @search + '%'
			)))
		  ) AS _
	  WHERE _row BETWEEN @skips AND @takes
   ORDER BY _row) AS a;
	END
END
GO


CREATE NONCLUSTERED INDEX [IX_CadDom] ON [api].[CadastroDomiciliar]
(
	[id] ASC,
	[uuidFichaOriginadora] ASC,
	[enderecoLocalPermanencia] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_EndLocPer] ON [api].[EnderecoLocalPermanencia]
(
	[id] ASC,
	[bairro] ASC,
	[cep] ASC,
	[codigoIbgeMunicipio] ASC,
	[complemento] ASC,
	[nomeLogradouro] ASC,
	[numero] ASC,
	[telefoneResidencia] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_SetorPar] ON [dbo].[AS_SetoresPar]
(
	[CodSetor] ASC,
	[Codigo] ASC,
	[CNES] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Setor] ON [dbo].[Setores]
(
	[CodSetor] ASC,
	[DesSetor] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_CredSetor] ON [dbo].[AS_CredenciadosVinc]
(
	[CodCred] ASC,
	[CNESLocal] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_ProfCBO] ON [dbo].[AS_ProfissoesTab]
(
	[CodProfTab] ASC,
	[DesProfTab] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_Ines] ON [dbo].[SetoresINEs]
(
	[CodINE] ASC,
	[CodSetor] ASC,
	[Numero] ASC,
	[Descricao] ASC
)
GO

CREATE NONCLUSTERED INDEX [IX_HeaderTrans] ON [api].[UnicaLotacaoTransport]
(
	[dataAtendimento] ASC,
	[profissionalCns] ASC,
	[cboCodigo_2002] ASC,
	[cnes] ASC,
	[ine] ASC,
	[codigoIbgeMunicipio] ASC
)
GO

ALTER TABLE [dbo].[ProAtendimentos]
    ADD [A_ID_Usuario_EI_CodCred] INT NULL,
        [A_ID_Usuario_AT_CodCred] INT NULL;
GO
CREATE TABLE [dbo].[ProAntecedentes] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [ID_PACIENTE] INT            NULL,
    [Cirurgias]   VARCHAR (1000) NULL,
    [Internacoes] VARCHAR (1000) NULL,
    [Alergias]    VARCHAR (1000) NULL,
    [OBS]         VARCHAR (1000) NULL,
    [HistMedico]  VARCHAR (8000) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);
GO
CREATE TABLE [dbo].[ProAntecedentes_Familiares] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [ID_PACIENTE] INT           NULL,
    [ID_CIAP]     NVARCHAR (10) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);
GO
CREATE TABLE [dbo].[ProAntecedentes_Pessoais] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [ID_PACIENTE] INT           NULL,
    [ID_CIAP]     NVARCHAR (10) NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);
GO
ALTER VIEW ProAtendimentos_Listagem(
	A_Status,
    A_ID_Status,
    A_ID,
    A_DataHora,
    A_ID_Setor,
    IDPaciente,
    Paciente,
    IDProfissional,
    Profissional,
    TpServicos,
    IDsTpServicos)
AS
SELECT
	'<i class="glyphicon glyphicon-user"'
	+ ' data-risco="' + isnull(convert(varchar(2),EI_RISCO),'') + '"'
	+ ' style="color:#' 
	+ case when EI_RISCO=0 then '428bca' when EI_RISCO=1 then '5cb85c' when EI_RISCO=2 then 'f0ad4e' when EI_RISCO=3 then 'd9534f' else 'aaa' end + ';"'
	+ ' title="' + case when EI_RISCO=0 then 'Sem Risco - ' when EI_RISCO=1 then 'Risco Baixo - ' when EI_RISCO=2 then 'Risco Médio - ' when EI_RISCO=3 then 'Risco Alto - ' else '' end + S.Descricao COLLATE Latin1_General_CI_AI + '"'
	+ '></i>',
	A.A_ID_Status,
	A.A_ID,
	A.A_DataHora,
	A_ID_Setor,
	C.codigo,
	'<span class="nomePaciente" data-valor="'+(C.nome collate latin1_general_CI_AI)+'"'
	+ ' data-idstatus="' + convert(varchar(2),S.ID) + '"'
	+ ' data-Corstatus="' + S.Cor COLLATE Latin1_General_CI_AI + '"'
	+ ' data-idUsuario="'
	+ case when A_ID_Usuario is not Null then convert(varchar(10),A_ID_Usuario) else '0' end
	+ '" data-nmUsuario="'
	+ case when u.Nome is not Null then u.Nome else '' end COLLATE Latin1_General_CI_AI
	+ '" data-escuta="'
	+ case when EI_DESFECHO is not Null then '1' else '0' end
	+ '" data-atendimento="'
	+ case when At_DESFECHO is not Null then '1' else '0' end
	+ '">'+(C.nome collate latin1_general_CI_AI)+'</span>',
	isnull(A.EI_ID_Credenciado,A.A_ID_Credenciado) as A_ID_Credenciado,
	C2.nome COLLATE Latin1_General_CI_AI as Profissional,
	isnull(
		(
			select PATSs.descricao COLLATE Latin1_General_CI_AI + ', '
			from ProAtendimentos_EI_TpServico PATS 
			left join ProAtendimentos_TpServicos PATSs on PATS.ID_TpServico=PATSs.ID
			where PATS.ID_Atendimento=A.A_ID order by PATSs.descricao COLLATE Latin1_General_CI_AI for xml path('')
		),(
			select PATSs.descricao COLLATE Latin1_General_CI_AI + ', '
			from ProAtendimentos_A_TpServico PATS 
			left join ProAtendimentos_TpServicos PATSs on PATS.ID_TpServico=PATSs.ID
			where PATS.ID_Atendimento=A.A_ID order by PATSs.descricao COLLATE Latin1_General_CI_AI for xml path('')
		)
	) as TpServicos,
	isnull(
		(
			select '|' + convert(varchar(5),PATS.ID_TpServico) + '|'
			from ProAtendimentos_EI_TpServico PATS 
			where PATS.ID_Atendimento=A.A_ID for xml path('')
		),(
			select '|' + convert(varchar(5),PATS.ID_TpServico) + '|'
			from ProAtendimentos_A_TpServico PATS 
			where PATS.ID_Atendimento=A.A_ID for xml path('')
		)
	) as IDsTpServicos
from
	ProAtendimentos A	
	left join ASSMED_CADASTRO C on A.A_ID_Paciente=C.codigo
	left join AS_Credenciados Cr on isnull(A.EI_ID_Credenciado,A.A_ID_Credenciado)=Cr.CodCred
	left join ASSMED_CADASTRO C2 on Cr.codigo=C2.codigo
	left join ProAtendimentos_Status S on A.A_ID_Status=S.ID
	left join assmed_usuario U on A.A_ID_Usuario=U.codusu

--GAMBIARRA PARA TRAZER O AGENDAMENTO DO ASSMED
union
select 
'<i class="glyphicon glyphicon-user" data-risco="0" style="color:#aaa;" title="Agendado"></i>'
,1,0,CONVERT(datetime,CONVERT(char(8), Ag_C.DtAgenda, 112)+' ' + Ag_C.HrAgenda COLLATE Latin1_General_CI_AI + ':00.000'),Ag_C.codsetor,Ag_C.codigo
,'<span class="nomePaciente" data-valor="'+Ag_C.nome COLLATE Latin1_General_CI_AI+'" data-idstatus="1" data-Corstatus="00b8ff" data-idUsuario="0" data-nmUsuario="" data-escuta="0" data-atendimento="0" data-paciente="' + convert(varchar(10),Ag_C.codigo) + '" data-medico="' + convert(varchar(10),Ag_C.CodCred) + '" data-dtsistema="' + convert(varchar(24),Ag_C.dtsistema,121) + '">'+Ag_C.nome COLLATE Latin1_General_CI_AI+'</span>'
,Ag_C.CodCred,Ag_C2.nome COLLATE Latin1_General_CI_AI,null,null
from AS_CredAgenda Ag_C 
	left join AS_Credenciados Ag_Cr on Ag_C.CodCred=Ag_Cr.CodCred
	left join ASSMED_CADASTRO Ag_C2 on Ag_Cr.codigo=Ag_C2.codigo
where convert(date,Ag_C.DtAgenda)=convert(date,getdate()) and SitAgenda COLLATE Latin1_General_CI_AI='A' --and CodSetor=52
union
select 
'<i class="glyphicon glyphicon-user" data-risco="0" style="color:#aaa;" title="Agendado"></i>'
,1,0,CONVERT(datetime,CONVERT(char(8), Ag_E.DtAgenda, 112)+' ' + Ag_E.HrAgenda COLLATE Latin1_General_CI_AI + ':00.000'),Ag_E.codsetor,Ag_E.codigo
,'<span class="nomePaciente" data-valor="'+Ag_E.nome COLLATE Latin1_General_CI_AI+'" data-idstatus="1" data-Corstatus="00b8ff" data-idUsuario="0" data-nmUsuario="" data-escuta="0" data-atendimento="0" data-paciente="' + convert(varchar(10),Ag_E.codigo) + '" data-especialidade="' + convert(varchar(10),Ag_E.CodEsp) + '" data-dtsistema="' + convert(varchar(24),Ag_E.dtsistema,121) + '">'+Ag_E.nome COLLATE Latin1_General_CI_AI+'</span>'
,Ag_E.CodEsp,Ag_Es.DesEsp COLLATE Latin1_General_CI_AI,null,null
from AS_EspAgenda Ag_E 
	left join AS_ESPECIALIDADES Ag_Es on Ag_E.CodEsp=Ag_Es.CodEsp
where convert(date,Ag_E.DtAgenda)=convert(date,getdate()) and SitAgenda='A' --and CodSetor=52
GO

CREATE TABLE SIGSM_ServicoSerializador_Fichas (
             Ficha NVARCHAR(32) NOT NULL PRIMARY KEY,
             Formato NVARCHAR(6) NOT NULL,
             Gerar BIT NOT NULL
)
GO

INSERT INTO SIGSM_ServicoSerializador_Fichas VALUES
            ('AtendimentoDomiciliar', 'XML', 0),
            ('AtendimentoIndividual', 'XML', 0),
            ('AtendimentoOdontologico', 'XML', 0),
            ('AtividadeColetiva', 'XML', 0),
            ('AvaliacaoElegibilidade', 'XML', 0),
            ('CadastroDomiciliar', 'XML', 1),
            ('CadastroIndividual', 'XML', 1),
            ('Complementar', 'XML', 0),
            ('ConsumoAlimentar', 'XML', 0),
            ('Procedimento', 'XML', 0),
            ('VisitaDomiciliar', 'XML', 1)
GO

CREATE TABLE SIGSM_ServicoSerializador_Config (
             Configuracao NVARCHAR(32) NOT NULL PRIMARY KEY,
             Valor NVARCHAR(512)
)
GO

INSERT INTO SIGSM_ServicoSerializador_Config VALUES
            ('sessionLocation', 'http://localhost/sigsm/v2/SessionVar.asp'),
            ('runningOn', 'http://localhost:8081'),
            ('ThriftFilesLocation', 'C:\inetpub\wwwroot\SIGSM\v2\files\ESUS\thrift'),
            ('XmlFilesLocation', 'C:\inetpub\wwwroot\SIGSM\v2\files\ESUS\xml'),
            ('TTL', '10000'),
            ('limiteHorasToken', '51'),
            ('authCryptAlg', 'SHA256'),
            ('ApiPath', 'http://localhost/sigsm/v2/ESUS/fichas')
GO

CREATE TABLE SIGSM_ServicoSerializador_Agenda (
             Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
             IdTransmissao UNIQUEIDENTIFIER NOT NULL,
             Executando BIT NOT NULL,
             ExecutadoEm DATETIME NULL
)
GO

CREATE TABLE [dbo].[SIGSM_Transmissao_StatusGeracao] (
             [Id]        INT           IDENTITY (1, 1) NOT NULL,
             [Descricao] NVARCHAR (60) NOT NULL,
             [Cor]       NVARCHAR (60) NULL,
             [Icone]     NVARCHAR (60) NULL,
             PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

INSERT INTO [dbo].[SIGSM_Transmissao_StatusGeracao] ([Descricao]) VALUES
            ('Agendado'),
            ('Em Execução'),
            ('Falha na Geração'),
            ('Validando'),
            ('Falha na Validação'),
            ('Finalizado'),
            ('Cancelado')
GO

CREATE TABLE [dbo].[SIGSM_Transmissao] (
             [Id]              UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
             [DataSolicitacao] DATETIME         DEFAULT (getdate()) NOT NULL,
             [CodUsu]          INT              NOT NULL,
             [DataLote]        DATETIME         NOT NULL,
             PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[SIGSM_Transmissao_Processos] (
             [Id]               UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
             [IdTransmissao]    UNIQUEIDENTIFIER NOT NULL,
             [IdStatusGeracao]  INT              NOT NULL,
             [InicioDoProcesso] DATETIME         NULL,
             [FimDoProcesso]    DATETIME         NULL,
             [ArquivoGerado]    NVARCHAR (255)   NULL,
             [TipoFicha]        NVARCHAR (100)   NULL,
             [SerializarEm]     NVARCHAR (10)    NOT NULL,
             PRIMARY KEY CLUSTERED ([Id] ASC),
             FOREIGN KEY ([IdStatusGeracao]) REFERENCES [dbo].[SIGSM_Transmissao_StatusGeracao] ([Id]),
             FOREIGN KEY ([IdTransmissao]) REFERENCES [dbo].[SIGSM_Transmissao] ([Id])
)
GO

CREATE TABLE [dbo].[SIGSM_Transmissao_Processos_Log] (
             [Id]         UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
             [IdProcesso] UNIQUEIDENTIFIER NOT NULL,
             [Erro]       NVARCHAR (MAX)   NOT NULL,
             [DataLog]    DATETIME         DEFAULT (getdate()) NOT NULL,
             PRIMARY KEY CLUSTERED ([Id] ASC),
             FOREIGN KEY ([IdProcesso]) REFERENCES [dbo].[SIGSM_Transmissao_Processos] ([Id])
)
GO

CREATE TABLE dbo.SIGSM_Migrations (
    [Version] NVARCHAR(20) NOT NULL PRIMARY KEY,
    [Date] DATETIME NOT NULL
)
GO

ALTER VIEW [api].[VW_Profissional] WITH SCHEMABINDING AS
		 SELECT ROW_NUMBER() OVER (ORDER BY (SELECT NULL))                  AS [id],
                RTRIM(LTRIM(doc.Numero)) COLLATE Latin1_General_CI_AI       AS [CNS],
                RTRIM(LTRIM(cad.Nome)) COLLATE Latin1_General_CI_AI         AS [Nome],
                RTRIM(LTRIM(cbo.CodProfTab)) COLLATE Latin1_General_CI_AI   AS [CBO],
                RTRIM(LTRIM(cbo.DesProfTab)) COLLATE Latin1_General_CI_AI   AS [Profissao],
                RTRIM(LTRIM(vinc.CNESLocal)) COLLATE Latin1_General_CI_AI   AS [CNES],
                RTRIM(LTRIM(setor.DesSetor)) COLLATE Latin1_General_CI_AI   AS [Unidade],
                RTRIM(LTRIM(ine.Numero)) COLLATE Latin1_General_CI_AI       AS [INE],
                RTRIM(LTRIM(ine.Descricao)) COLLATE Latin1_General_CI_AI    AS [Equipe],
				CAST(cad.Codigo AS INT) AS [CodUsu]
		   FROM [dbo].[ASSMED_Cadastro]					                    AS cad
	 INNER JOIN [dbo].[ASSMED_CadastroDocPessoal]		                    AS doc
			 ON cad.Codigo = doc.Codigo                 
	 INNER JOIN [dbo].[AS_CREDENCIADOS]					                    AS cred
			 ON cad.Codigo = cred.Codigo                    
	 INNER JOIN [dbo].[AS_CredenciadosVinc]				                    AS vinc
			 ON cred.CodCred = vinc.CodCred
     INNER JOIN [dbo].[AS_ProfissoesTab]                                    AS cbo
             ON RTRIM(LTRIM(vinc.CodProfTab)) COLLATE Latin1_General_CI_AI = RTRIM(LTRIM(cbo.CodProfTab))  COLLATE Latin1_General_CI_AI
     INNER JOIN [dbo].[AS_SetoresPar]                                       AS par
             ON RTRIM(LTRIM(vinc.CNESLocal)) COLLATE Latin1_General_CI_AI = RTRIM(LTRIM(par.CNES)) COLLATE Latin1_General_CI_AI
	 INNER JOIN [dbo].[Setores]                                             AS setor
             ON par.CodSetor = setor.CodSetor
            AND par.Codigo = setor.Codigo
      LEFT JOIN [dbo].[SetoresINEs]                                         AS ine
             ON setor.CodSetor = ine.CodSetor
            AND RTRIM(LTRIM(vinc.CodINE)) COLLATE Latin1_General_CI_AI = RTRIM(LTRIM(ine.CodINE)) COLLATE Latin1_General_CI_AI
	      WHERE doc.CodTpDocP = 6
		    AND cbo.CodProfissao IS NOT NULL
			AND LEN(LTRIM(RTRIM(COALESCE(doc.Numero COLLATE Latin1_General_CI_AI, '')))) > 0
			AND LEN(LTRIM(RTRIM(COALESCE(vinc.CNESLocal COLLATE Latin1_General_CI_AI, '')))) > 0
	   GROUP BY RTRIM(LTRIM(doc.Numero)) COLLATE Latin1_General_CI_AI,
                RTRIM(LTRIM(cad.Nome)) COLLATE Latin1_General_CI_AI,
                RTRIM(LTRIM(cbo.CodProfTab)) COLLATE Latin1_General_CI_AI,
                RTRIM(LTRIM(cbo.DesProfTab)) COLLATE Latin1_General_CI_AI,
                RTRIM(LTRIM(vinc.CNESLocal)) COLLATE Latin1_General_CI_AI,
                RTRIM(LTRIM(setor.DesSetor)) COLLATE Latin1_General_CI_AI,
                RTRIM(LTRIM(ine.Numero)) COLLATE Latin1_General_CI_AI,
                RTRIM(LTRIM(ine.Descricao)) COLLATE Latin1_General_CI_AI,
				cad.Codigo;
GO

INSERT INTO dbo.SIGSM_Migrations VALUES ('20171013181515', GETDATE())
GO

CREATE NONCLUSTERED INDEX [dbo_as_atendimento]
    ON [dbo].[AS_Atendimento]([Codigo] ASC, [AnoAtend] ASC, [NumAtend] ASC);
GO

CREATE NONCLUSTERED INDEX [index_dbo_as_atendimento]
    ON [dbo].[AS_Atendimento]([Codigo] ASC, [AnoAtend] ASC, [NumAtend] ASC);
GO

ALTER TABLE [dbo].[AS_EXAMES_LOCAGEND]
        ADD [TipoLocal] VARCHAR (10)  NULL;
GO

CREATE NONCLUSTERED INDEX [dbo.ASSMED_Cadastro_NMCODIDF]
    ON [dbo].[ASSMED_Cadastro]([Nome] ASC)
    INCLUDE([Codigo], [IdFicha]);
GO

CREATE NONCLUSTERED INDEX [dbo.ASSMED_Cadastro_nome]
    ON [dbo].[ASSMED_Cadastro]([Nome] ASC);
GO

CREATE NONCLUSTERED INDEX [idx_ASSMED_CadastroDocPessoal-Codigo-CodTpDocP-Numero]
    ON [dbo].[ASSMED_CadastroDocPessoal]([Codigo] ASC, [CodTpDocP] ASC)
    INCLUDE([Numero]);
GO

CREATE NONCLUSTERED INDEX [idx_ASSMED_CadastroDocPessoal-CodTpDocP-Codigo-Numero]
    ON [dbo].[ASSMED_CadastroDocPessoal]([CodTpDocP] ASC)
    INCLUDE([Codigo], [Numero]);
GO

ALTER VIEW [dbo].[VW_COMP_FAMILIAR](
    PK,
    id_composicao_familiar,
    codigo_pessoa,
    nome,
    dtnasc,
	id_grau_parentesco,
    grau_parentesco,
	num_contrato)
AS	
select
	a.codigo_pessoa as pk,
	a.id_composicao_familiar,
	a.codigo_pessoa,
	c.nome,
	convert(char(10),isnull(pf.dtnasc, pf2.dtnasc), 103) dtnasc,
	a.id_grau_parentesco,
	b.descricao as grau_parentesco,
	c.numcontrato
from
	composicao_familiar a 
	--inner join grau_parentesco b on b.id_grau_parentesco = a.id_grau_parentesco
	left join tp_relacao_parentesco b on a.id_grau_parentesco = b.codigo
	inner join assmed_cadastro c on c.codigo = a.codigo_pessoa
	left join assmed_pesfisica pf on pf.codigo = c.codigo
	left join assmed_cadastropf pf2 on pf2.codigo = c.codigo
GO

ALTER PROCEDURE [dbo].[SPF_CadastroSaudeAtualiza20]
           @NumContrato  INT = 0,
           @Codigo  DECIMAL = 0,
           @CPF  VARCHAR(14) = NULL,
           @CNS  VARCHAR(50) = NULL,
           @NumRG  VARCHAR(50) = NULL,
           @CodSetor  INT = 0,
           @NumProntuario  VARCHAR(15) = NULL,
           @CodOrgao  INT = 0,
           @UFOrgao  CHAR(2) = NULL,
           @DtEmissaoRG  CHAR(10) = NULL,
           @Nome  VARCHAR(100) = NULL,
           @DtNasc char(10) = NULL,
           @Sexo char(1) = NULL,
           @EstCivil char(1) = NULL,
           @NomePai varchar(100) = NULL,
           @NomeMae varchar(100) = NULL,
           @TpSangue char(3) = NULL,
           @Doador char(1) = NULL,           
           @DDDTelFixoRes  INT = 0,
           @NumTelFixoRes  CHAR(9) = NULL,
           @DDDTelFixoCom  INT = 0,
           @NumTelFixoCom  CHAR(9) = NULL,
           @DDDTelMovelPart  INT = 0,
           @NumTelMovelPart  CHAR(9) = NULL,
           @DDDTelMovelTrab  INT = 0,
           @NumTelMovelTrab  CHAR(9) = NULL,
           @ItemEnd  INT = 0,
           @CEP  CHAR(10) = NULL,
           @TipoEnd  CHAR(1) = NULL,
           @Corresp  CHAR(1) = NULL,
           @Logradouro  VARCHAR(100) = NULL,
           @Complemento  VARCHAR(50) = NULL,
           @Numero  CHAR(10) = NULL,
           @Bairro  VARCHAR(50) = NULL,
           @NomeCidade VARCHAR(80) = NULL,
           @CodCidade  INT = 0,
           @CodTpLogra  INT = 0,
           @UF  CHAR(2) = NULL,
           @NumIP VARCHAR(20) = NULL,
           @CodUsu INT = 0,
           @Peso REAL = 0,
           @Altura REAL = 0,
           @CodCross INT = 0
AS
  DECLARE @DtEmiRG datetime
  DECLARE @CodSitFam Int
  DECLARE @CodEscola Int
  DECLARE @CodCor Int
  DECLARE @CodNacao Int
  DECLARE @CodEtnia Int
  DECLARE @Deficiente char(1)
  DECLARE @CodDeficiencia Int
  DECLARE @DtNascD datetime
  DECLARE @CodTpDocCNS INT --=6
  DECLARE @CodTpDocrg INT --=1
  DECLARE @CodOrgaoDataSUS INT  --=9
  DECLARE @TIPOPESSOA CHAR(1)
  DECLARE @TIPOANTES CHAR(1)
  DECLARE @NomeAntes  VARCHAR(100)
  SELECT @TIPOPESSOA='C'

  IF EXISTS (SELECT 1 FROM ASSMED_PesFisica WHERE NumContrato=@NumContrato and Codigo=@Codigo)
  BEGIN
     SELECT @CodSitFam=CodSitFam,@CodEscola=CodEscola,@CodCor=CodCor,@CodNacao=CodNacao,@CodEtnia=CodEtnia,
            @deficiente=Deficiente,@CodDeficiencia=CodDeficiencia
       FROM ASSMED_PesFisica WHERE NumContrato=@NumContrato and Codigo=@Codigo
  END
  ELSE
  BEGIN
    IF EXISTS (SELECT 1 FROM ASSMED_cadastroPF WHERE NumContrato=@NumContrato and Codigo=@Codigo)
    BEGIN
       SELECT @CodSitFam=CodSitFam,@CodEscola=CodEscola,@CodCor=CodCor,@CodNacao=CodNacao,@CodEtnia=CodEtnia,
              @deficiente=Deficiente,@CodDeficiencia=CodDeficiencia
         FROM ASSMED_cadastroPF WHERE NumContrato=@NumContrato and Codigo=@Codigo
    END
  END   
  
  
BEGIN TRANSACTION	    
  SELECT @CodTpDocCNS=6
  SELECT @CodTpDocRG=1
  SELECT @CodOrgaoDataSUS=9
  SELECT @DtEmiRG=CONVERT(datetime,@DtEmissaoRG,103)
  SELECT @DtNascD=Convert(datetime,@DtNasc,103)
  SELECT @NomeAntes=Nome,@TIPOANTES=Tipo From ASSMED_Cadastro WHERE NumContrato=@NumContrato and Codigo=@Codigo
  
  IF @CPF > '' SELECT @TIPOPESSOA='F'
  
  IF @NumProntuario > ''
  BEGIN
     IF NOT EXISTS (SELECT 1 FROM ASSMED_CadastroSetor Where NumContrato=@NumContrato and Codigo=@Codigo and CodSetor=@CodSetor)
     BEGIN
        INSERT INTO ASSMED_CadastroSetor (NumContrato,Codigo,CodSetor,NumProntuario,CodUsu,NumIP,DtSistema)
        values (@NumContrato,@Codigo,@CodSetor,@NumProntuario,@CodUsu,@NumIP,GETDATE())
     END
     ELSE
     BEGIN
       IF @NumProntuario<>(SELECT ISNULL(NumProntuario,'') FROM ASSMED_CadastroSetor Where NumContrato=@NumContrato and Codigo=@Codigo and CodSetor=@CodSetor)
       BEGIN
          UPDATE ASSMED_CadastroSetor Set NumProntuario=@NumProntuario,CodUsu=@CodUsu,NumIP=@NumIP,DtSistema=GETDATE()
          Where NumContrato=@NumContrato and Codigo=@Codigo and CodSetor=@CodSetor                                          
       END
     END
  END
  IF EXISTS (SELECT 1 FROM ASSMED_CadastroDocPessoal WHERE Numcontrato=@NumContrato  AND Codigo=@Codigo  and CodTpDocP = @CodTpDocRG)
  BEGIN
     Update ASSMED_CadastroDocPessoal Set Numero=@NumRG,CodOrgao=@CodOrgao,UFOrgao=@UFOrgao,
                                          DtEmissao=@DtEmiRG,CodUsu=@CodUsu,NumIP=@NumIP,DtSistema=GETDATE()
     WHERE Numcontrato=@NumContrato  AND Codigo=@Codigo  and CodTpDocP = @CodTpDocRG
  END
  ELSE
  BEGIN
     INSERT INTO ASSMED_CadastroDocPessoal (NumContrato,Codigo,CodTpDocP,Numero,CodOrgao,UFOrgao,DtEmissao,CodUsu,NumIP,DtSistema)
     VALUES (@NumContrato,@Codigo,@CodTpDocRG,@NumRG,@CodOrgao,@UFOrgao,@DtEmiRG,@CodUsu,@NumIP,GETDATE())
  END 

  IF EXISTS (SELECT 1 FROM ASSMED_CadastroDocPessoal WHERE Numcontrato=@NumContrato  AND Codigo=@Codigo  and CodTpDocP = @CodTpDocCNS)
  BEGIN
     Update ASSMED_CadastroDocPessoal Set Numero=@CNS,CodOrgao=@CodOrgaoDataSUS,
                                          CodUsu=@CodUsu,NumIP=@NumIP,DtSistema=GETDATE()
     WHERE Numcontrato=@NumContrato  AND Codigo=@Codigo  and CodTpDocP = @CodTpDocCNS                                          
  END
  ELSE
  BEGIN
     INSERT INTO ASSMED_CadastroDocPessoal (NumContrato,Codigo,CodTpDocP,Numero,CodOrgao,UFOrgao,DtEmissao,CodUsu,NumIP,DtSistema)
     VALUES (@NumContrato,@Codigo,@CodTpDocCNS,@CNS,@CodOrgaoDataSUS,'',Null,@CodUsu,@NumIP,GETDATE())
  END 
  
  /*=== INI === INSERE/ATUALIZA TELEFONES ===*/
      IF NOT EXISTS (SELECT 1 FROM ASSMED_CadTelefones WHERE Numcontrato=@NumContrato AND Codigo=@Codigo and DDD = @DDDTelFixoCom and NumTel = @NumTelFixoCom and TipoTel='C')
      BEGIN
         IF @NumTelFixoCom > ''
         BEGIN
           INSERT INTO ASSMED_CadTelefones (NumContrato,Codigo,DDD,NumTel,TipoTel,CodUsu,NumIP,DtSistema)
           values (@NumContrato,@Codigo,@DDDTelFixoCom,@NumTelFixoCom,'C',@CodUsu,@NumIP,GETDATE())
         END  
      END
      else
      BEGIN
        UPDATE ASSMED_CadTelefones SET DtSistema=GETDATE()
        WHERE Numcontrato=@NumContrato AND Codigo=@Codigo and DDD=@DDDTelFixoCom and NumTel=@NumTelFixoCom and TipoTel='C'
      END 
      IF NOT EXISTS (SELECT 1 FROM ASSMED_CadTelefones WHERE Numcontrato=@NumContrato AND Codigo=@Codigo and DDD = @DDDTelFixoRes and NumTel = @NumTelFixoRes and TipoTel='R')
      BEGIN
         IF @NumTelFixoRes > ''
         BEGIN
           INSERT INTO ASSMED_CadTelefones (NumContrato,Codigo,DDD,NumTel,TipoTel,CodUsu,NumIP,DtSistema)
           values (@NumContrato,@Codigo,@DDDTelFixoRes,@NumTelFixoRes,'R',@CodUsu,@NumIP,GETDATE())
         END  
      END  
      else
      BEGIN
        UPDATE ASSMED_CadTelefones SET DtSistema=GETDATE()
        WHERE Numcontrato=@NumContrato AND Codigo=@Codigo and DDD=@DDDTelFixoRes and NumTel=@NumTelFixoRes and TipoTel='R'
      END 
      IF NOT EXISTS (SELECT 1 FROM ASSMED_CadTelefones WHERE Numcontrato=@NumContrato AND Codigo=@Codigo and DDD = @DDDTelMovelTrab and NumTel = @NumTelMovelTrab and TipoTel='T')
      BEGIN
         IF @NumTelMovelTrab > ''
         BEGIN
            INSERT INTO ASSMED_CadTelefones (NumContrato,Codigo,DDD,NumTel,TipoTel,CodUsu,NumIP,DtSistema)
            values (@NumContrato,@Codigo,@DDDTelMovelTrab,@NumTelMovelTrab,'T',@CodUsu,@NumIP,GETDATE())
         END   
      END  
      else
      BEGIN
        UPDATE ASSMED_CadTelefones SET DtSistema=GETDATE()
        WHERE Numcontrato=@NumContrato AND Codigo=@Codigo and DDD=@DDDTelMovelTrab and NumTel=@NumTelMovelTrab and TipoTel='T'
      END 
      IF NOT EXISTS (SELECT 1 FROM ASSMED_CadTelefones WHERE Numcontrato=@NumContrato AND Codigo=@Codigo and DDD=@DDDTelMovelPart and NumTel=@NumTelMovelPart and TipoTel='P')
      BEGIN
         IF @NumTelMovelPart > ''
         BEGIN
            INSERT INTO ASSMED_CadTelefones (NumContrato,Codigo,DDD,NumTel,TipoTel,CodUsu,NumIP,DtSistema)
            values (@NumContrato,@Codigo,@DDDTelMovelPart,@NumTelMovelPart,'P',@CodUsu,@NumIP,GETDATE())
         END   
      END  
      ELSE
      BEGIN
        UPDATE ASSMED_CadTelefones SET DtSistema=GETDATE()
        WHERE Numcontrato=@NumContrato AND Codigo=@Codigo and DDD=@DDDTelMovelPart and NumTel=@NumTelMovelPart and TipoTel='P'
      END 
  /*=== FIM === INSERE/ATUALIZA TELEFONES ===*/

  /*=== INI === INSERE/ATUALIZA ENDERECO ===*/
      IF EXISTS (SELECT 1 FROM ASSMED_Endereco WHERE Numcontrato=@NumContrato AND Codigo=@Codigo and ItemEnd=@ItemEnd )
      BEGIN
        UPDATE ASSMED_Endereco SET CEP=@CEP,TipoEnd=@TipoEnd,Corresp=@Corresp,Logradouro=@Logradouro,
                                   Bairro=@Bairro,Complemento=@Complemento,NomeCidade=@NomeCidade,
                                   CodCidade=@CodCidade,UF=@UF,Numero=@Numero,CodTpLogra=@CodTpLogra
        WHERE Numcontrato=@NumContrato AND Codigo=@Codigo and ItemEnd=@ItemEnd 
      END
      ELSE
      BEGIN
        Insert Into ASSMED_Endereco (NumContrato,Codigo,Latitude,Longitude,CodTpLogra,ItemEnd,CEP,TipoEnd
                                    ,Corresp,Logradouro,Complemento,Numero,Bairro,CodCidade,NomeCidade,UF)
        Values (@NumContrato,@Codigo,'','',@CodTpLogra,
               (Select ISNULL(Max(ItemEnd),0)+1 From ASSMED_Endereco WHERE NumContrato=@NumContrato AND Codigo=@Codigo)
               ,@CEP,@TipoEnd
               ,@Corresp,@Logradouro,@Complemento,@Numero,@Bairro,@CodCidade,@NomeCidade,@UF)                 
      END 
  /*=== FIM === INSERE/ATUALIZA ENDERECO ===*/
  
  IF @NomeAntes<>@Nome OR @TIPOANTES<>@TIPOPESSOA
  BEGIN
	  UPDATE ASSMED_Cadastro SET Nome=upper(@Nome),Tipo=@TIPOPESSOA
	                             ,CodUsu=@CodUsu
	                             ,NumIP=@NumIP
	                             ,DtSistema=GETDATE() 
	   where NumContrato=@NumContrato AND Codigo=@Codigo
  END	   
  IF @TIPOPESSOA='F' 
  BEGIN
     IF EXISTS (SELECT 1 FROM ASSMED_PesFisica WHERE NumContrato=@NumContrato and Codigo=@Codigo)
     BEGIN
        UPDATE ASSMED_PesFisica SET Sexo=@Sexo,DtNasc=@DtNascD
                                    ,EstCivil=@EstCivil
                                    ,NomeMae=@NomeMae
                                    ,NomePai=@NomePai
                                    ,TpSangue=@TpSangue
                                    ,Doador=@Doador
                                    ,CodSitFam=@CodSitFam,CodEscola=@CodEscola
                                    ,CodCor=@CodCor
                                    ,CodNacao=@CodNacao,CodEtnia=@CodEtnia
                                    ,Deficiente=@Deficiente,CodDeficiencia=@CodDeficiencia
                                    ,Peso=@Peso,Altura=@Altura
                                    ,CodCross=@CodCross
        where NumContrato=@NumContrato AND Codigo=@Codigo                                    
     END
     ELSE
     BEGIN
        INSERT INTO ASSMED_PesFisica (NumContrato,Codigo,CPF,Sexo,DtNasc,EstCivil,NomeMae,NomePai,TpSangue,Doador,
                                      CodSitFam,CodEscola,CodCor,CodNacao,CodEtnia,Deficiente,CodDeficiencia,Peso,Altura,CodCross)     
           VALUES (@NumContrato,@Codigo,@CPF,@Sexo,@DtNascD,@EstCivil,@NomeMae,@NomePai,@TpSangue,@Doador,
                                      @CodSitFam,@CodEscola,@CodCor,@CodNacao,@CodEtnia,@Deficiente,@CodDeficiencia,@Peso,@Altura,@CodCross)
        DELETE ASSMED_CadastroPF where NumContrato=@NumContrato AND Codigo=@Codigo
     END
  END
  ELSE
  BEGIN
     IF EXISTS (SELECT 1 FROM ASSMED_cadastroPF WHERE NumContrato=@NumContrato and Codigo=@Codigo)
     BEGIN
        UPDATE ASSMED_cadastroPF SET Sexo=@Sexo,DtNasc=@DtNascD
                                    ,EstCivil=@EstCivil
                                    ,NomeMae=@NomeMae
                                    ,NomePai=@NomePai
                                    ,TpSangue=@TpSangue
                                    ,Doador=@Doador
                                    ,CodSitFam=@CodSitFam,CodEscola=@CodEscola
                                    ,CodCor=@CodCor
                                    ,CodNacao=@CodNacao,CodEtnia=@CodEtnia
                                    ,Deficiente=@Deficiente,CodDeficiencia=@CodDeficiencia
                                    ,Peso=@Peso,Altura=@Altura
                                    ,CodCross=@CodCross
        where NumContrato=@NumContrato AND Codigo=@Codigo                                    
     END
     ELSE
     BEGIN
        INSERT INTO ASSMED_CadastroPF (NumContrato,Codigo,CPF,Sexo,DtNasc,EstCivil,NomeMae,NomePai,TpSangue,Doador,
                                      CodSitFam,CodEscola,CodCor,CodNacao,CodEtnia,Deficiente,CodDeficiencia,Peso,Altura,CodCross)     
           VALUES (@NumContrato,@Codigo,'',@Sexo,@DtNascD,@EstCivil,@NomeMae,@NomePai,@TpSangue,@Doador,
                                      @CodSitFam,@CodEscola,@CodCor,@CodNacao,@CodEtnia,@Deficiente,@CodDeficiencia,@Peso,@Altura,@CodCross)
        DELETE ASSMED_PesFisica where NumContrato=@NumContrato AND Codigo=@Codigo
     END  
  END
	  
	  
IF @@ERROR <> 0 ROLLBACK TRANSACTION ELSE COMMIT TRANSACTION
GO

CREATE SCHEMA [SignalR]
    AUTHORIZATION [dbo];
GO

CREATE TABLE [SignalR].[Messages_0] (
    [PayloadId]  BIGINT          NOT NULL,
    [Payload]    VARBINARY (MAX) NOT NULL,
    [InsertedOn] DATETIME        NOT NULL,
    PRIMARY KEY CLUSTERED ([PayloadId] ASC)
);
GO

CREATE TABLE [SignalR].[Messages_0_Id] (
    [PayloadId] BIGINT NOT NULL,
    PRIMARY KEY CLUSTERED ([PayloadId] ASC)
);
GO

CREATE TABLE [SignalR].[Schema] (
    [SchemaVersion] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([SchemaVersion] ASC)
);
GO

CREATE VIEW VW_COMPLEMENTAR AS	  
SELECT 
--Campos para exibição
	ficha.id AS PK,
	ficha.id AS ficha_ID,
	CONVERT(varchar(10), ficha.dataAtendimento, 103) DATA,
	id_profissional_saude as Profissional_ID,
	Profissional.nome as Profissional_nome,
	Digitador.CodUsu as Digitador_ID,
	Digitador.nome as Digitador_nome,
	finalizado,
	case when finalizado=1 then 'Finalizada' else 'Não Finalizada' end Status_Nome,
--Campos para filtagem
	ficha.cnes Unidade_CNES,
	setor.CodSetor Unidade_ID,
    uuidficha
FROM Sigsm_complementarZika ficha
left join assmed_cadastro Profissional on Profissional.codigo=ficha.id_profissional_saude
left join ASSMED_Usuario Digitador on ficha.digitado_por =Digitador.codusu
left join as_setorespar setor on ficha.cnes=setor.CNES collate Latin1_General_CI_AI
GO

CREATE VIEW dbo.VW_ConsultaCadastrosDomiciliares AS
	 SELECT x.Complemento COLLATE Latin1_General_CI_AI AS Complemento,
			c.NomeCidadao COLLATE Latin1_General_CI_AI AS Responsavel,
			x.Numero COLLATE Latin1_General_CI_AI AS Numero,
			x.Logradouro COLLATE Latin1_General_CI_AI AS Endereco,
			CAST(t.DDD AS NVARCHAR) + (t.NumTel COLLATE Latin1_General_CI_AI) AS Telefone,
			x.IdFicha,
			x.Codigo
	   FROM dbo.ASSMED_Endereco AS x
 INNER JOIN dbo.VW_ConsultaCadastrosIndividuais AS c
		 ON x.Codigo = c.Codigo
CROSS APPLY (
	 SELECT TOP 1 * FROM dbo.ASSMED_CadTelefones AS tel
	  WHERE x.Codigo = tel.Codigo
		AND tel.TipoTel = 'R'
		AND tel.DDD IS NOT NULL
		AND tel.NumTel IS NOT NULL) AS t
	  WHERE x.Logradouro IS NOT NULL
		AND LEN(LTRIM(RTRIM(x.Logradouro))) > 0
   GROUP BY x.Complemento,
			c.NomeCidadao,
			x.Numero,
			x.Logradouro,
			t.DDD,
			t.NumTel,
			x.IdFicha,
			x.Codigo
GO

delete from grupo_sistemas where ID_Sistema NOT IN (2, 99)
GO
