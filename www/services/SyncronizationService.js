angular.module('mobacs')
    .factory('SyncronizationService', ['$q', 'SQLiteService', function($q, SQLiteService) {

        return {

            deleteHomeWithoutCitizenSyncronization: function() {
                var query = '' +
                    ' delete from cadastroDomiciliarTerritorial where id in(select cd.id from FamiliaRow as fr inner join cadastroDomiciliarTerritorial as cd' +
                    ' on fr.cadastroDomiciliarId = cd.id' +
                    ' left join Cadastro_Individual as ci' +
                    ' on fr.numeroCnsResponsavel = ci.cnsCidadao' +
                    ' where ci.id is null or fr.numeroCnsResponsavel is null)';
                return $q.when(SQLiteService.executeSql(query));
            },

            deleteFamilyWithoutHomeSyncronization: function() {
                var query = '' +
                    ' delete from FamiliaRow where id in (select fr.id from FamiliaRow as fr left join cadastroDomiciliarTerritorial as cd' +
                    ' on fr.cadastroDomiciliarId = cd.id' +
                    ' where cd.id is null)';
                return $q.when(SQLiteService.executeSql(query));
            },

            deleteHomeWithoutFamily: function() {
                var query = '' +
                    ' delete from cadastroDomiciliarTerritorial where id in (select cd.id from cadastroDomiciliarTerritorial as cd left join FamiliaRow as fr' +
                    ' on fr.cadastroDomiciliarId = cd.id' +
                    ' where fr.id is null)';
                return $q.when(SQLiteService.executeSql(query));
            },

            deleteFichaAntiga: function(uuidOriginadora) {
                var query = 'DELETE from Cadastro_Individual WHERE uuid = "' + uuidOriginadora + '"';
                return $q.when(SQLiteService.executeSql(query));
            },

            deleteFichaAntigaDomicilio: function(uuidOriginadora) {
                var query = 'DELETE from cadastroDomiciliarTerritorial WHERE uuid = "' + uuidOriginadora + '"';
                return $q.when(SQLiteService.executeSql(query));
            },

            selectStatusSyncronization: function() {
                var query = 'SELECT dado from ( ' +
                    'SELECT count(*) as dado FROM Cadastro_Individual WHERE status in ("Aguardando Sincronismo","Sincronizada com o servidor") union all' +
                    ' SELECT count(*) as dado FROM Cadastro_Individual union all ' +
                    ' SELECT count(*) as dado FROM cadastroDomiciliarTerritorial WHERE status in ("Aguardando Sincronismo","Sincronizada com o servidor") union all' +
                    ' SELECT count(*) as dado FROM cadastroDomiciliarTerritorial union all ' +
                    ' SELECT count(*) as dado FROM Visita_Domiciliar WHERE status in ("Aguardando Sincronismo") union all' +
                    ' SELECT count(*) as dado FROM Visita_Domiciliar' +
                    ') as Numbers';
                return $q.when(SQLiteService.getItems(query));
            },

            selectNumberPeoplesAPI: function() {
                var query = 'SELECT count(*) as numberAllPeople FROM Cadastro_Individual where status = "Aguardando Sincronismo"';
                return $q.when(SQLiteService.getItems(query));
            },

            selectPeoplesToAPI: function() {
                var query = 'SELECT * FROM Cadastro_Individual where status IN ("Aguardando Sincronismo", "Sincronizada com o servidor")'; //Usada para enviar os usuários
                return $q.when(SQLiteService.getItems(query));
            },

            selectRecuseRegistersToAPI: function() {
                var query = 'SELECT distinct * FROM Cadastro_Individual ' +
                    'WHERE EXISTS (SELECT Justificativa FROM Cadastro_Individual) ' +
                    'and statusTermoRecusaCadastroIndividualAtencaoBasica = 1 ' +
                    'and status IN ("Aguardando Sincronismo", "Sincronizada com o servidor")';
                return $q.when(SQLiteService.getItems(query));
            },
            //adicionar status no where

            selectValidRegistersToAPI: function() {
                var query = 'SELECT distinct f.numeroCnsResponsavel FROM cadastroDomiciliarTerritorial as d inner join FamiliaRow as f ' +
                    'on d.id = f.cadastroDomiciliarId inner join Cadastro_Individual as i on i.cnsCidadao = f.numeroCnsResponsavel ' +
                    'left join Cadastro_Individual ci on f.numeroCnsResponsavel = ci.cnsResponsavelFamiliar and ci.status not in ("Aguardando Sincronismo", "Sincronizada com o servidor") ' +
                    'where (d.status IN ("Aguardando Sincronismo", "Sincronizada com o servidor") and i.statusEhResponsavel = 1 and i.status IN ("Aguardando Sincronismo", "Sincronizada com o servidor") ' +
                    'and ci.id is null) or i.statusTermoRecusaCadastroIndividualAtencaoBasica = 1';
                return $q.when(SQLiteService.getItems(query));
            },

            selectPeopleDependentsAtomic: function(cns) {
                var query = 'SELECT * FROM Cadastro_Individual ' +
                    'where cnsResponsavelFamiliar = "' + cns + '" and status IN ("Aguardando Sincronismo", "Sincronizada com o servidor")';
                return $q.when(SQLiteService.getItems(query));
            },

            selectHomeResponsibleAtomic: function(cns) {
                var query = 'SELECT d.* FROM cadastroDomiciliarTerritorial as d inner join FamiliaRow as f ' +
                    'on d.id = f.cadastroDomiciliarId ' +
                    'where d.status IN ("Aguardando Sincronismo", "Sincronizada com o servidor") and f.numeroCnsResponsavel = "' + cns + '"';
                return $q.when(SQLiteService.getItems(query));
            },

            selectNumberHomeAPI: function() {
                var query = 'SELECT COUNT(*) as numberAllHome FROM cadastroDomiciliarTerritorial WHERE status = "Aguardando Sincronismo"';
                return $q.when(SQLiteService.getItems(query));
            },

            selectNumberAllHome: function() {
                var query = 'SELECT COUNT(*) as numberAllHome FROM cadastroDomiciliarTerritorial';
                return $q.when(SQLiteService.getItems(query));
            },

            selectAdressToAPI: function() {
                var query = 'SELECT * FROM cadastroDomiciliarTerritorial where status = "Aguardando Sincronismo"'; // Select endereços para envios
                return $q.when(SQLiteService.getItems(query));
            },

            selectAllRecordsIndividual: function(param) {
                var query = 'SELECT * FROM Cadastro_Individual';
                return $q.when(SQLiteService.getItems(query));
            },

            selectAllRecordsDomiciliar: function(param) {
                var query = 'SELECT * FROM cadastroDomiciliarTerritorial';
                return $q.when(SQLiteService.getItems(query));
            },

            selectCheckIfPeopleExist: function(param) {
                var query = 'SELECT * FROM Cadastro_Individual where uuid = "' + param + '"';
                return $q.when(SQLiteService.getItems(query));
            },

            selectCheckIfAdressExist: function(param) {
                var query = 'SELECT * FROM cadastroDomiciliarTerritorial where uuid = "' + param + '"';
                return $q.when(SQLiteService.getItems(query));
            },

            deleteAdressToAPI: function(condition) {
                var query = 'DELETE from cadastroDomiciliarTerritorial where ' + condition; //Delete Adress to API
                return $q.when(SQLiteService.getItems(query));
            },

            selectFamiliesToAPI: function(condition) {
                var query = 'SELECT * FROM FamiliaRow WHERE ' + condition + '';
                return $q.when(SQLiteService.getItems(query));
            },

            deleteFamiliesToAPI: function(condition) {
                var query = 'DELETE from FamiliaRow where ' + condition; //Delete Adress to API
                return $q.when(SQLiteService.getItems(query));
            },

            selectNumberHomeVisitAPI: function() {
                var query = 'SELECT COUNT(*) as numberAllHomeVisit FROM Visita_Domiciliar WHERE status = "Aguardando Sincronismo"';
                return $q.when(SQLiteService.getItems(query));
            },

            selectHomeVisitToAPI: function() {
                var query = 'SELECT * FROM Visita_Domiciliar where status = "Aguardando Sincronismo"';
                return $q.when(SQLiteService.getItems(query));
            },

            updateRegisterAllPeople: function(dataInformation, param) {
                var query = "UPDATE Cadastro_Individual set " +
                    'dataAtendimento = ' + new Date().getTime() + ',' +
                    'DataRegistro = ' + new Date().getTime() + ',' +
                    'cnsCidadao = ' + dataInformation.cnsCidadao + ',' +
                    'statusEhResponsavel = ' + dataInformation.statusEhResponsavel + ',' +
                    'cnsResponsavelFamiliar = ' + dataInformation.cnsResponsavelFamiliar + ',' +
                    'beneficiarioBolsaFamilia = ' + dataInformation.beneficiarioBolsaFamilia + ',' +
                    'stForaArea = ' + dataInformation.stForaArea + ',';
                if (dataInformation.CPF != null && dataInformation.CPF.length > 0) {
                    query += ' CPF = "' + dataInformation.CPF + '",';
                } else {
                    query += ' CPF = null,';
                }
                if (dataInformation.RG != null && dataInformation.RG.length > 0) {
                    query += ' RG = "' + dataInformation.RG + '",';
                } else {
                    query += ' RG = null,';
                }
                if (dataInformation.ComplementoRG != null && dataInformation.ComplementoRG.length > 0) {
                    query += ' ComplementoRG = "' + dataInformation.ComplementoRG + '",';
                } else {
                    query += ' ComplementoRG = null,';
                }
                if (dataInformation.EstadoCivil != null && dataInformation.EstadoCivil.length > 0) {
                    query += ' EstadoCivil = "' + dataInformation.EstadoCivil + '",';
                } else {
                    query += ' EstadoCivil = null,';
                }
                if (dataInformation.microarea != null) {
                    query += ' microarea = "' + dataInformation.microarea + '",';
                } else {
                    query += ' microarea = null,';
                }
                if (dataInformation.nomeCidadao != null)
                    query += 'nomeCidadao = "' + dataInformation.nomeCidadao + '",';
                else
                    query += 'nomeCidadao = null,';
                if (dataInformation.nomeSocial != null && dataInformation.nomeSocial.length > 0)
                    query += 'nomeSocial = "' + dataInformation.nomeSocial + '",';
                else
                    query += 'nomeSocial = null,';
                if (dataInformation.dataNascimentoCidadao != null)
                    query += 'dataNascimentoCidadao = "' + dataInformation.dataNascimentoCidadao + '",';
                else
                    query += 'dataNascimentoCidadao = null,';
                query += 'sexoCidadao = ' + dataInformation.sexoCidadao + ',' +
                    'racaCorCidadao = ' + dataInformation.racaCorCidadao + ',';
                if (dataInformation.etnia != null) {
                    query += 'etnia  = "' + dataInformation.etnia + '",';
                } else {
                    query += 'etnia = null,';
                }
                if (dataInformation.numeroNisPisPasep != null && dataInformation.numeroNisPisPasep.length > 0)
                    query += 'numeroNisPisPasep = "' + dataInformation.numeroNisPisPasep + '",';
                else
                    query += 'numeroNisPisPasep = null,';
                if (dataInformation.nomeMaeCidadao != null && dataInformation.nomeMaeCidadao.length > 0)
                    query += 'nomeMaeCidadao = "' + dataInformation.nomeMaeCidadao + '",';
                else
                    query += 'nomeMaeCidadao = null,' +
                    'desconheceNomeMae = ' + dataInformation.desconheceNomeMae + ',';
                if (dataInformation.nomePaiCidadao != null && dataInformation.nomePaiCidadao.length > 0)
                    query += 'nomePaiCidadao = "' + dataInformation.nomePaiCidadao + '",';
                else
                    query += 'nomePaiCidadao = null,';
                query += 'desconheceNomePai = ' + dataInformation.desconheceNomePai + ',' +
                    'nacionalidadeCidadao = ' + dataInformation.nacionalidadeCidadao + ',' +
                    'paisNascimento = ' + dataInformation.paisNascimento + ',';
                if (dataInformation.dtNaturalizacao != null)
                    query += 'dtNaturalizacao = "' + dataInformation.dtNaturalizacao + '",';
                else
                    query += 'dtNaturalizacao = null,';
                if (dataInformation.portariaNaturalizacao != null)
                    query += 'portariaNaturalizacao = "' + dataInformation.portariaNaturalizacao + '",';
                else
                    query += 'portariaNaturalizacao = null,';
                if (dataInformation.codigoIbgeMunicipioNascimento != null)
                    query += 'codigoIbgeMunicipioNascimento = "' + dataInformation.codigoIbgeMunicipioNascimento + '",';
                else
                    query += 'codigoIbgeMunicipioNascimento = null,';
                if (dataInformation.dtEntradaBrasil != null)
                    query += 'dtEntradaBrasil = "' + dataInformation.dtEntradaBrasil + '",';
                else
                    query += 'dtEntradaBrasil = null,';
                if (dataInformation.telefoneCelular != null && dataInformation.telefoneCelular.length > 0)
                    query += 'telefoneCelular = "' + dataInformation.telefoneCelular + '",';
                else
                    query += 'telefoneCelular = null,';
                if (dataInformation.emailCidadao != null && dataInformation.emailCidadao.length > 0)
                    query += 'emailCidadao = "' + dataInformation.emailCidadao + '",';
                else
                    query += 'emailCidadao = null,';
                if (dataInformation.observacao != null && dataInformation.observacao.length > 0)
                    query += 'observacao = "' + dataInformation.observacao + '",';
                else
                    query += 'observacao = null,' +
                    'statusEhGestante = ' + dataInformation.statusEhGestante + ',';
                if (dataInformation.maternidadeDeReferencia != null && dataInformation.maternidadeDeReferencia.length > 0)
                    query += 'maternidadeDeReferencia = "' + dataInformation.maternidadeDeReferencia + '",';
                else
                    query += 'maternidadeDeReferencia = null,';
                query += 'situacaoPeso = ' + dataInformation.situacaoPeso + ',' +
                    'statusEhFumante = ' + dataInformation.statusEhFumante + ',' +
                    'statusEhDependenteAlcool = ' + dataInformation.statusEhDependenteAlcool + ',' +
                    'statusEhDependenteOutrasDrogas = ' + dataInformation.statusEhDependenteOutrasDrogas + ',' +
                    'statusTemHipertensaoArterial = ' + dataInformation.statusTemHipertensaoArterial + ',' +
                    'statusTemDiabetes = ' + dataInformation.statusTemDiabetes + ',' +
                    'statusTeveAvcDerrame = ' + dataInformation.statusTeveAvcDerrame + ',' +
                    'statusTeveInfarto = ' + dataInformation.statusTeveInfarto + ',' +
                    'statusTeveDoencaCardiaca = ' + dataInformation.statusTeveDoencaCardiaca + ',' +
                    'Insuficiencia_cardiaca = ' + dataInformation.Insuficiencia_cardiaca + ',' +
                    'Outro_Doenca_Cardiaca = ' + dataInformation.Outro_Doenca_Cardiaca + ',' +
                    'Nao_Sabe_Doenca_Cardiaca = ' + dataInformation.Nao_Sabe_Doenca_Cardiaca + ',' +
                    'statusTemTeveDoencasRins = ' + dataInformation.statusTemTeveDoencasRins + ',' +
                    'Insuficiencia_renal = ' + dataInformation.Insuficiencia_renal + ',' +
                    'Outro_Doenca_Rins = ' + dataInformation.Outro_Doenca_Rins + ',' +
                    'Nao_Sabe_Doenca_Rins = ' + dataInformation.Nao_Sabe_Doenca_Rins + ',' +
                    'statusTemDoencaRespiratoria = ' + dataInformation.statusTemDoencaRespiratoria + ',' +
                    'Asma = ' + dataInformation.Asma + ',' +
                    'DPOC_Enfisema = ' + dataInformation.DPOC_Enfisema + ',' +
                    'Outro_Doenca_Respiratoria = ' + dataInformation.Outro_Doenca_Respiratoria + ',' +
                    'Nao_Sabe_Doenca_Respiratoria = ' + dataInformation.Nao_Sabe_Doenca_Respiratoria + ',' +
                    'statusTemHanseniase = ' + dataInformation.statusTemHanseniase + ',' +
                    'statusTemTuberculose = ' + dataInformation.statusTemTuberculose + ',' +
                    'statusTemTeveCancer = ' + dataInformation.statusTemTeveCancer + ',' +
                    'statusTeveInternadoem12Meses = ' + dataInformation.statusTeveInternadoem12Meses + ',';
                if (dataInformation.descricaoCausaInternacaoEm12Meses != null && dataInformation.descricaoCausaInternacaoEm12Meses.length > 0)
                    query += 'descricaoCausaInternacaoEm12Meses = "' + dataInformation.descricaoCausaInternacaoEm12Meses + '",';
                else
                    query += 'descricaoCausaInternacaoEm12Meses = null,';
                query += 'statusDiagnosticoMental = ' + dataInformation.statusDiagnosticoMental + ',' +
                    'statusEstaAcamado = ' + dataInformation.statusEstaAcamado + ',' +
                    'statusEstaDomiciliado = ' + dataInformation.statusEstaDomiciliado + ',' +
                    'statusUsaPlantasMedicinais = ' + dataInformation.statusUsaPlantasMedicinais + ',';
                if (dataInformation.descricaoPlantasMedicinaisUsadas != null && dataInformation.descricaoPlantasMedicinaisUsadas.length > 0)
                    query += 'descricaoPlantasMedicinaisUsadas = "' + dataInformation.descricaoPlantasMedicinaisUsadas + '",';
                else
                    query += 'descricaoPlantasMedicinaisUsadas = null,';
                query += 'statusUsaOutrasPraticasIntegrativasOuComplementares = ' + dataInformation.statusUsaOutrasPraticasIntegrativasOuComplementares + ',';
                if (dataInformation.descricaoOutraCondicao1 != null && dataInformation.descricaoOutraCondicao1.length > 0)
                    query += 'descricaoOutraCondicao1 = "' + dataInformation.descricaoOutraCondicao1 + '",';
                else
                    query += 'descricaoOutraCondicao1 = null,';
                if (dataInformation.descricaoOutraCondicao2 != null && dataInformation.descricaoOutraCondicao2.length > 0)
                    query += 'descricaoOutraCondicao2 = "' + dataInformation.descricaoOutraCondicao2 + '",';
                else
                    query += 'descricaoOutraCondicao2 = null,';
                if (dataInformation.descricaoOutraCondicao3 != null && dataInformation.descricaoOutraCondicao3.length > 0)
                    query += 'descricaoOutraCondicao3 = "' + dataInformation.descricaoOutraCondicao3 + '",';
                else
                    query += 'descricaoOutraCondicao3 = null,' +
                    'relacaoParentescoCidadao = ' + dataInformation.relacaoParentescoCidadao + ',' +
                    'statusFrequentaEscola = ' + dataInformation.statusFrequentaEscola + ',' +
                    'ocupacaoCodigoCbo2002 = ' + dataInformation.ocupacaoCodigoCbo2002 + ',' +
                    'grauInstrucaoCidadao = ' + dataInformation.grauInstrucaoCidadao + ',' +
                    'situacaoMercadoTrabalhoCidadao = ' + dataInformation.situacaoMercadoTrabalhoCidadao + ',' +
                    'AdultoResponsavelresponsavelPorCrianca = ' + dataInformation.AdultoResponsavelresponsavelPorCrianca + ',' +
                    'OutrasCriancasresponsavelPorCrianca = ' + dataInformation.OutrasCriancasresponsavelPorCrianca + ',' +
                    'AdolescenteresponsavelPorCrianca = ' + dataInformation.AdolescenteresponsavelPorCrianca + ',' +
                    'SozinharesponsavelPorCrianca = ' + dataInformation.SozinharesponsavelPorCrianca + ',' +
                    'CrecheresponsavelPorCrianca = ' + dataInformation.CrecheresponsavelPorCrianca + ',' +
                    'OutroresponsavelPorCrianca = ' + dataInformation.OutroresponsavelPorCrianca + ',' +
                    'statusFrequentaBenzedeira = ' + dataInformation.statusFrequentaBenzedeira + ',' +
                    'statusParticipaGrupoComunitario = ' + dataInformation.statusParticipaGrupoComunitario + ',' +
                    'statusPossuiPlanoSaudePrivado = ' + dataInformation.statusPossuiPlanoSaudePrivado + ',' +
                    'statusMembroPovoComunidadeTradicional = ' + dataInformation.statusMembroPovoComunidadeTradicional + ',';
                if (dataInformation.povoComunidadeTradicional != null && dataInformation.povoComunidadeTradicional.length > 0)
                    query += 'povoComunidadeTradicional = "' + dataInformation.povoComunidadeTradicional + '",';
                else
                    query += 'povoComunidadeTradicional = null,';
                query += 'statusDesejaInformarOrientacaoSexual = ' + dataInformation.statusDesejaInformarOrientacaoSexual + ',' +
                    'orientacaoSexualCidadao = ' + dataInformation.orientacaoSexualCidadao + ',' +
                    'statusDesejaInformarIdentidadeGenero = ' + dataInformation.statusDesejaInformarIdentidadeGenero + ',' +
                    'identidadeGeneroCidadao = ' + dataInformation.identidadeGeneroCidadao + ',' +
                    'statusTemAlgumaDeficiencia = ' + dataInformation.statusTemAlgumaDeficiencia + ',' +
                    'AuditivaDeficiencias = ' + dataInformation.AuditivaDeficiencias + ',' +
                    'VisualDeficiencias = ' + dataInformation.VisualDeficiencias + ',' +
                    'Intelectual_CognitivaDeficiencias = ' + dataInformation.Intelectual_CognitivaDeficiencias + ',' +
                    'FisicaDeficiencias = ' + dataInformation.FisicaDeficiencias + ',' +
                    'OutraDeficiencias = ' + dataInformation.OutraDeficiencias + ',' +
                    'statusSituacaoRua = ' + dataInformation.statusSituacaoRua + ',' +
                    'tempoSituacaoRua = ' + dataInformation.tempoSituacaoRua + ',' +
                    'statusRecebeBeneficio = ' + dataInformation.statusRecebeBeneficio + ',' +
                    'statusPossuiReferenciaFamiliar = ' + dataInformation.statusPossuiReferenciaFamiliar + ',' +
                    'quantidadeAlimentacoesAoDiaSituacaoRua = ' + dataInformation.quantidadeAlimentacoesAoDiaSituacaoRua + ',' +
                    'Restaurante_popular = ' + dataInformation.Restaurante_popular + ',' +
                    'Doacao_grupo_religioso = ' + dataInformation.Doacao_grupo_religioso + ',' +
                    'Doacao_restaurante = ' + dataInformation.Doacao_restaurante + ',' +
                    'Doacao_popular = ' + dataInformation.Doacao_popular + ',' +
                    'Outros_origemAlimentoSituacaoRua = ' + dataInformation.Outros_origemAlimentoSituacaoRua + ',' +
                    'statusAcompanhadoPorOutraInstituicao = ' + dataInformation.statusAcompanhadoPorOutraInstituicao + ',';
                if (dataInformation.outraInstituicaoQueAcompanha != null && dataInformation.outraInstituicaoQueAcompanha.length > 0) {
                    query += 'outraInstituicaoQueAcompanha = "' + dataInformation.outraInstituicaoQueAcompanha + '",';
                } else {
                    query += 'outraInstituicaoQueAcompanha = null,';
                }
                query += 'statusVisitaFamiliarFrequentemente = ' + dataInformation.statusVisitaFamiliarFrequentemente + ',';
                if (dataInformation.grauParentescoFamiliarFrequentado != null && dataInformation.grauParentescoFamiliarFrequentado.length > 0) {
                    query += 'grauParentescoFamiliarFrequentado = "' + dataInformation.grauParentescoFamiliarFrequentado + '",';
                } else {
                    query += 'grauParentescoFamiliarFrequentado = null,';
                }
                query += 'statusTemAcessoHigienePessoalSituacaoRua = ' + dataInformation.statusTemAcessoHigienePessoalSituacaoRua + ',' +
                    'Banho = ' + dataInformation.Banho + ',' +
                    'Acesso_a_sanitario = ' + dataInformation.Acesso_a_sanitario + ',' +
                    'Higiene_bucal = ' + dataInformation.Higiene_bucal + ',' +
                    'Outros_higienePessoalSituacaoRua = ' + dataInformation.Outros_higienePessoalSituacaoRua + ',' +
                    'motivoSaidaCidadao = ' + dataInformation.motivoSaidaCidadao + ',';
                if (dataInformation.dataObito != null && dataInformation.dataObito.length > 0)
                    query += 'dataObito = "' + dataInformation.dataObito + '",';
                else
                    query += 'dataObito = null,';
                if (dataInformation.numeroDO != null && dataInformation.numeroDO.length > 0)
                    query += 'numeroDO = "' + dataInformation.numeroDO + '",';
                else
                    query += 'numeroDO = null,';
                query += 'status = "' + dataInformation.status + '"' +
                    ' WHERE uuid = "' + param + '"'; //UUID !!!!!!!!!!!!!!!!!!!!!!!!
                console.log(query);
                return $q.when(SQLiteService.getItems(query));
            },

            updateRegisterAllAdress: function(dataInformation, param) {
                console.log(dataInformation.uuid);
                console.log(dataInformation.uuidFichaOriginadora);
                var query = "UPDATE cadastroDomiciliarTerritorial set " +
                    // 'dataAtendimento = ' + new Date().getTime() + ','; +
                    'DataRegistro="' + new Date().getTime() + '",'
                if (dataInformation.bairro != null && dataInformation.bairro.length > 0) {
                    query += 'bairro="' + dataInformation.bairro + '",';
                } else {
                    query += 'bairro=null,';
                }
                if (dataInformation.cep != null && dataInformation.cep.length > 0) {
                    query += 'cep="' + dataInformation.cep + '",';
                } else {
                    query += 'cep=null,';
                }
                if (dataInformation.codigoIbgeMunicipio != null && dataInformation.codigoIbgeMunicipio.length > 0) {
                    query += 'codigoIbgeMunicipio="' + dataInformation.codigoIbgeMunicipio + '",';
                } else {
                    query += 'codigoIbgeMunicipio=null,';
                }
                if (dataInformation.complemento != null && dataInformation.complemento.length > 0) {
                    query += 'complemento="' + dataInformation.complemento + '",';
                } else {
                    query += 'complemento=null,';
                }
                if (dataInformation.nomeLogradouro != null && dataInformation.nomeLogradouro.length > 0) {
                    query += 'nomeLogradouro="' + dataInformation.nomeLogradouro + '",';
                } else {
                    query += 'nomeLogradouro=null,';
                }
                if (dataInformation.numero != null && dataInformation.numero.length > 0) {
                    query += 'numero="' + dataInformation.numero + '",';
                } else {
                    query += 'numero=null,';
                }
                if (dataInformation.numeroDneUf != null && dataInformation.numeroDneUf.length > 0) {
                    query += 'numeroDneUf="' + dataInformation.numeroDneUf + '",';
                } else {
                    query += 'numeroDneUf=null,';
                }
                if (dataInformation.telefoneContato != null && dataInformation.telefoneContato.length > 0) {
                    query += 'telefoneContato="' + dataInformation.telefoneContato + '",';
                } else {
                    query += 'telefoneContato=null,';
                }
                if (dataInformation.telefoneResidencia != null && dataInformation.telefoneResidencia.length > 0) {
                    query += 'telefoneResidencia="' + dataInformation.telefoneResidencia + '",';
                } else {
                    query += 'telefoneResidencia=null,';
                }
                if (dataInformation.telefoneResidencia != null && dataInformation.telefoneResidencia.length > 0) {
                    query += 'telefoneResidencia="' + dataInformation.telefoneResidencia + '",';
                } else {
                    query += 'telefoneResidencia=null,';
                }
                if (dataInformation.tipoLogradouroNumeroDne != null && dataInformation.tipoLogradouroNumeroDne.length > 0) {
                    query += 'tipoLogradouroNumeroDne="' + dataInformation.tipoLogradouroNumeroDne + '",';
                } else {
                    query += 'tipoLogradouroNumeroDne=null,';
                }
                query += 'stSemNumero=' + dataInformation.stSemNumero + ',';
                if (dataInformation.pontoReferencia != null && dataInformation.pontoReferencia.length > 0) {
                    query += 'pontoReferencia="' + dataInformation.pontoReferencia + '",';
                } else {
                    query += 'pontoReferencia=null,';
                }
                if (dataInformation.microarea != null && dataInformation.microarea.length > 0) {
                    query += 'microarea="' + dataInformation.microarea + '",';
                } else {
                    query += 'microarea=null,';
                }
                query += 'stForaArea=' + dataInformation.stForaArea + ',' +
                    'tipoDeImovel=' + dataInformation.tipoDeImovel + ',';
                if (dataInformation.observacao != null && dataInformation.observacao.length > 0) {
                    query += 'observacao="' + dataInformation.observacao + '",';
                } else {
                    query += 'observacao=null,';
                }
                query += 'situacaoMoradiaPosseTerra=' + dataInformation.situacaoMoradiaPosseTerra + ',' +
                    'localizacao=' + dataInformation.localizacao + ',' +
                    'tipoDomicilio=' + dataInformation.tipoDomicilio + ',';
                if (dataInformation.nuMoradores != null && dataInformation.nuMoradores.length > 0) {
                    query += 'nuMoradores=' + dataInformation.nuMoradores + ',';
                } else {
                    query += 'nuMoradores=null,';
                }
                if (dataInformation.nuComodos != null && dataInformation.nuComodos.length > 0) {
                    query += 'nuComodos=' + dataInformation.nuComodos + ',';
                } else {
                    query += 'nuComodos=null,';
                }
                query += 'areaProducaoRural=' + dataInformation.areaProducaoRural + ',' +
                    'tipoAcessoDomicilio=' + dataInformation.tipoAcessoDomicilio + ',' +
                    'stDisponibilidadeEnergiaEletrica=' + dataInformation.stDisponibilidadeEnergiaEletrica + ',' +
                    'materialPredominanteParedesExtDomicilio=' + dataInformation.materialPredominanteParedesExtDomicilio + ',' +
                    'abastecimentoAgua=' + dataInformation.abastecimentoAgua + ',' +
                    'formaEscoamentoBanheiro=' + dataInformation.formaEscoamentoBanheiro + ',' +
                    'aguaConsumoDomicilio=' + dataInformation.aguaConsumoDomicilio + ',' +
                    'destinoLixo=' + dataInformation.destinoLixo + ',' +
                    'stAnimaisNoDomicilio=' + dataInformation.stAnimaisNoDomicilio + ',' +
                    'Gato=' + dataInformation.Gato + ',' +
                    'Cachorro=' + dataInformation.Cachorro + ',' +
                    'Passaro=' + dataInformation.Passaro + ',' +
                    'Outros_AnimalNoDomicilio=' + dataInformation.Outros_AnimalNoDomicilio + ',';
                if (dataInformation.quantosAnimaisNoDomicilio != null && dataInformation.quantosAnimaisNoDomicilio.length > 0) {
                    query += 'quantosAnimaisNoDomicilio=' + dataInformation.quantosAnimaisNoDomicilio + ',';
                } else {
                    query += 'quantosAnimaisNoDomicilio=null,';
                }
                if (dataInformation.nomeInstituicaoPermanencia != null && dataInformation.nomeInstituicaoPermanencia.length > 0) {
                    query += 'nomeInstituicaoPermanencia="' + dataInformation.nomeInstituicaoPermanencia + '",';
                } else {
                    query += 'nomeInstituicaoPermanencia=null,';
                }
                query += 'stOutrosProfissionaisVinculados=' + dataInformation.stOutrosProfissionaisVinculados + ',';
                if (dataInformation.nomeResponsavelTecnico != null && dataInformation.nomeResponsavelTecnico.length > 0) {
                    query += 'nomeResponsavelTecnico="' + dataInformation.nomeResponsavelTecnico + '",';
                } else {
                    query += 'nomeResponsavelTecnico=null,';
                }
                if (dataInformation.cnsResponsavelTecnico != null && dataInformation.cnsResponsavelTecnico.length > 0) {
                    query += 'cnsResponsavelTecnico="' + dataInformation.cnsResponsavelTecnico + '",';
                } else {
                    query += 'cnsResponsavelTecnico=null,';
                }
                if (dataInformation.cargoInstituicao != null && dataInformation.cargoInstituicao.length > 0) {
                    query += 'cargoInstituicao="' + dataInformation.cargoInstituicao + '",';
                } else {
                    query += 'cargoInstituicao=null,';
                }
                if (dataInformation.telefoneResponsavelTecnico != null && dataInformation.telefoneResponsavelTecnico.length > 0) {
                    query += 'telefoneResponsavelTecnico="' + dataInformation.telefoneResponsavelTecnico + '",';
                } else {
                    query += 'telefoneResponsavelTecnico=null,';
                }
                if (dataInformation.uuid != null && dataInformation.uuid.length > 0) {
                    query += 'uuid="' + dataInformation.uuid + '",';
                } else {
                    query += 'uuid=null,';
                }
                if (dataInformation.uuidFichaOriginadora != null && dataInformation.uuidFichaOriginadora.length > 0) {
                    query += 'uuidFichaOriginadora="' + dataInformation.uuidFichaOriginadora + '",';
                } else {
                    query += 'uuidFichaOriginadora=null,';
                }
                query += 'status="' + dataInformation.status + '"' +
                    ' where uuid="' + param + '"';
                console.log(query);
                return $q.when(SQLiteService.executeSql(query));
            }

        }
    }]);