
-- Criação de procedure para cadastro individual
CREATE PROCEDURE PR_INT_CadastroIndividual (
	@token UNIQUEIDENTIFIER,
	@fichaAtualizada BIT = 0,
	@informacoesSocioDemograficasId UNIQUEIDENTIFIER = NULL,
	@statusTermoRecusaCadastroIndividualAtencaoBasica BIT = 0,
	@uuidFichaOriginadora NCHAR(44) = NULL,

	-- CondicoesDeSaude
	@descricaoCausaInternacaoEm12Meses nvarchar(200) = NULL,
	@descricaoOutraCondicao1 nvarchar(200) = NULL,
	@descricaoOutraCondicao2 nvarchar(200) = NULL,
	@descricaoOutraCondicao3 nvarchar(200) = NULL,
	@descricaoPlantasMedicinaisUsadas nvarchar(200) = NULL,
	@maternidadeDeReferencia nvarchar(200) = NULL,
	@situacaoPeso int = NULL,
	@statusEhDependenteAlcool bit,
	@statusEhDependenteOutrasDrogas bit,
	@statusEhFumante bit,
	@statusEhGestante bit,
	@statusEstaAcamado bit,
	@statusEstaDomiciliado bit,
	@statusTemDiabetes bit,
	@statusTemDoencaRespiratoria bit,
	@statusTemHanseniase bit,
	@statusTemHipertensaoArterial bit,
	@statusTemTeveCancer bit,
	@statusTemTeveDoencasRins bit,
	@statusTemTuberculose bit,
	@statusTeveAvcDerrame bit,
	@statusTeveDoencaCardiaca bit,
	@statusTeveInfarto bit,
	@statusTeveInternadoem12Meses bit,
	@statusUsaOutrasPraticasIntegrativasOuComplementares bit,
	@statusUsaPlantasMedicinais bit,
	@statusDiagnosticoMental bit,
	
	-- EmSituacaoDeRua
	@grauParentescoFamiliarFrequentado nvarchar(200) = NULL,
	@outraInstituicaoQueAcompanha nvarchar(200) = NULL,
	@quantidadeAlimentacoesAoDiaSituacaoRua int = NULL,
	@statusAcompanhadoPorOutraInstituicao bit,
	@statusPossuiReferenciaFamiliar bit,
	@statusRecebeBeneficio bit,
	@statusSituacaoRua bit,
	@statusTemAcessoHigienePessoalSituacaoRua bit,
	@statusVisitaFamiliarFrequentemente bit,
	@tempoSituacaoRua int = NULL,

	-- IdentificacaoUsuarioCidadao
	@nomeSocial nvarchar(140) = NULL,
	@codigoIbgeMunicipioNascimento nchar(14) = NULL,
	@dataNascimentoCidadao int,
	@desconheceNomeMae bit,
	@emailCidadao nvarchar(200) = NULL,
	@nacionalidadeCidadao int,
	@nomeCidadao nvarchar(140),
	@nomeMaeCidadao nvarchar(140) = NULL,
	@cnsCidadao nchar(30) = NULL,
	@cnsResponsavelFamiliar nchar(30) = NULL,
	@telefoneCelular nvarchar(22) = NULL,
	@numeroNisPisPasep nchar(22) = NULL,
	@paisNascimento int = NULL,
	@racaCorCidadao int,
	@sexoCidadao int,
	@statusEhResponsavel bit,
	@etnia int = NULL,
	@num_contrato int = NULL,
	@nomePaiCidadao nvarchar(140) = NULL,
	@desconheceNomePai bit,
	@dtNaturalizacao int = NULL,
	@portariaNaturalizacao nvarchar(32) = NULL,
	@dtEntradaBrasil int = NULL,
	@microarea nchar(4) = NULL,
	@stForaArea bit,

	-- SaidaCidadaoCadastro
	@motivoSaidaCidadao int = NULL,
	@dataObito int = NULL,
	@numeroDO nchar(18) = NULL
) AS
BEGIN
BEGIN TRANSACTION
BEGIN TRY
	/* @TODO - Implementar o cadastro de pessoa e individual e os validadores */

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
