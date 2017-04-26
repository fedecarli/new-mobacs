ALTER VIEW [dbo].[VW_INDIVIDUAIS](
    PK,
    ID,NOME,DTNASC,NOMEMAE,CNS,MUNNASC,DOCUMENTOS)
AS
select
	a.codigo,
	a.codigo,
	a.nome COLLATE Latin1_General_CI_AI as nome,
	convert(char(10), isnull(pf.dtnasc, pf2.dtnasc), 103) COLLATE Latin1_General_CI_AI as dt_nasc,
	isnull(pf.nomemae, pf2.nomemae) COLLATE Latin1_General_CI_AI as nomemae,
	e.numero COLLATE Latin1_General_CI_AI as cns,
	DI.municipio COLLATE Latin1_General_CI_AI AS municipio,
	(
		select atdp.DesTpDocP + ': ' + acdp.Numero + '|br|'
		from assmed_cadastrodocpessoal acdp 
		left join assmed_tipodocpessoal atdp on acdp.CodTpDocP=atdp.CodTpDocP
		where Codigo=a.codigo and acdp.Numero<>'' and acdp.Numero is not null for xml path('')
	) COLLATE Latin1_General_CI_AI as documentos
from assmed_cadastro a
	left join assmed_pesfisica pf on pf.codigo = a.codigo
	left join assmed_cadastropf pf2 on pf2.codigo = a.codigo
	left join assmed_cadastrodocpessoal e on e.codigo = a.codigo and e.codtpdocp = 6
	left join Domicilio_Individual DI on a.codigo = DI.id_pessoa
where a.nome<>'' and a.nome is not null
