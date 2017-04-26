ALTER VIEW [api].[VW_Enderecos] AS
	 SELECT CEP,
			Logradouro,
			Bairro,
			(CASE LEN(Complemento) WHEN 0 THEN NULL ELSE Complemento END) AS Complemento,
			(CASE LEN(Numero) WHEN 0 THEN NULL ELSE Numero END) AS Numero,
			CodTpLogra,
			CodCidade,
			Codigo
	   FROM (
	 SELECT STUFF(STUFF([CEP], PATINDEX('%[^0-9]%', [CEP]), 1, ''), PATINDEX('%[^0-9]%', STUFF([CEP], PATINDEX('%[^0-9]%', [CEP]), 1, '')), 1, '') COLLATE Latin1_General_CI_AI AS CEP
			,LTRIM(RTRIM([Logradouro])) COLLATE Latin1_General_CI_AI AS Logradouro
			,(CASE WHEN PATINDEX('%[-]%', [Bairro]) > 0 THEN SUBSTRING(LTRIM(RTRIM(COALESCE([Bairro], ''))), 1, PATINDEX('%[-]%', [Bairro]) - 1) ELSE Bairro END) COLLATE Latin1_General_CI_AI AS Bairro
			,COALESCE(RTRIM(LTRIM([Complemento])), '') COLLATE Latin1_General_CI_AI AS Complemento
			,COALESCE(RTRIM(LTRIM([Numero])), '') COLLATE Latin1_General_CI_AI AS Numero
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
		AND LEN((CASE WHEN PATINDEX('%[-]%', [Bairro]) > 0 THEN SUBSTRING(LTRIM(RTRIM(COALESCE([Bairro], ''))), 1, PATINDEX('%[-]%', [Bairro]) - 1) ELSE Bairro END)) > 0
			) AS ceps;
GO

ALTER VIEW [api].[VW_enderecoLocalPermanencia] AS
	 SELECT DISTINCT
			model.id,
			_log.bairro,
			_log.cep,
			_log.CodIbge AS codigoIbgeMunicipio,
			_log.complemento,
			_log.Logradouro AS nomeLogradouro,
			(CASE WHEN _log.numero IS NULL OR _log.numero = '0' THEN NULL ELSE _log.numero END) AS numero,
			_log.DNE AS numeroDneUf,
			_log.telCon AS telefoneContato,
			_log.telRes AS telelefoneResidencia,
			_log.tipoLogradouroNumeroDne,
			(CASE WHEN _log.numero IS NULL OR _log.numero = '0' THEN 1 ELSE 0 END) AS stSemNumero,
			model.pontoReferencia,
			model.microarea,
			(CASE WHEN model.microarea IS NULL THEN 1 ELSE 0 END) AS stForaArea
	   FROM (
			 SELECT e.CEP COLLATE Latin1_General_CI_AI AS CEP,
					e.Logradouro COLLATE Latin1_General_CI_AI AS Logradouro,
					e.Bairro COLLATE Latin1_General_CI_AI AS Bairro,
					e.Complemento COLLATE Latin1_General_CI_AI AS Complemento,
					e.Numero COLLATE Latin1_General_CI_AI AS Numero,
					logr.CO_TIPO_LOGRADOURO COLLATE Latin1_General_CI_AI AS tipoLogradouroNumeroDne,
					c.CodIbge COLLATE Latin1_General_CI_AI AS CodIbge,
					u.DNE COLLATE Latin1_General_CI_AI AS DNE,
					(CONVERT(NVARCHAR, _telRes.DDD) + _telRes.NumTel) COLLATE Latin1_General_CI_AI AS telRes,
					(CONVERT(NVARCHAR, _telCon.DDD) + _telCon.NumTel) COLLATE Latin1_General_CI_AI AS telCon
			   FROM [api].[VW_Enderecos] AS e
		 INNER JOIN [dbo].[Cidade] AS c ON e.CodCidade = c.CodCidade
		 INNER JOIN [dbo].[UF] AS u ON c.UF = u.UF
		 INNER JOIN [dbo].[TB_MS_TIPO_LOGRADOURO] AS logr
				 ON logr.CO_TIPO_LOGRADOURO COLLATE Latin1_General_CI_AI = CONVERT(NVARCHAR(5), e.CodTpLogra) COLLATE Latin1_General_CI_AI
		 INNER JOIN [dbo].[ASSMED_Cadastro] AS cad
				 ON e.Codigo = cad.Codigo
		OUTER APPLY (
						 SELECT TOP 1 *
						   FROM [dbo].[ASSMED_CadTelefones] AS t1
						  WHERE t1.Codigo = cad.Codigo
							AND t1.TipoTel = 'R'
					   ORDER BY DtSistema DESC
					) AS _telRes
		OUTER APPLY (
						 SELECT TOP 1 *
						   FROM [dbo].[ASSMED_CadTelefones] AS t2
						  WHERE t2.Codigo = cad.Codigo
							AND t2.TipoTel = 'P'
					   ORDER BY DtSistema DESC
					) AS _telCon
		   GROUP BY e.CEP,
					e.Logradouro,
					e.Bairro,
					e.Complemento,
					e.Numero,
					logr.CO_TIPO_LOGRADOURO,
					c.CodIbge,
					u.DNE,
					_telRes.DDD,
					_telRes.NumTel,
					_telCon.DDD,
					_telCon.NumTel)  AS _log
  LEFT JOIN [api].[EnderecoLocalPermanencia] AS model
		 ON _log.Bairro = model.bairro
		AND _log.CEP = model.cep
		AND _log.CodIbge = model.codigoIbgeMunicipio
		AND _log.Logradouro = model.nomeLogradouro
		AND _log.Numero = model.numero;
GO

