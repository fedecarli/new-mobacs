angular.module('mobacs')
  .factory('PeopleService', ['$q', 'SQLiteService', function($q, SQLiteService) {
    var startJQ = 0;

    return {
      createTable: function() {
        var query = 'CREATE TABLE IF NOT EXISTS Cadastro_Individual ( id integer primary key autoincrement not null, uuidFichaOriginadora text null, uuid text null,token text null, profissionalCNS text null, cboCodigo_2002 text null, cnes text null, ine text null, dataAtendimento text null, codigoIbgeMunicipio text null, descricaoCausaInternacaoEm12Meses text null, descricaoOutraCondicao1 text null, descricaoOutraCondicao2 text null, descricaoOutraCondicao3 text null, descricaoPlantasMedicinaisUsadas text null, Insuficiencia_cardiaca integer null, Outro_Doenca_Cardiaca integer null, Nao_Sabe_Doenca_Cardiaca integer null, Asma integer null, DPOC_Enfisema integer null, Outro_Doenca_Respiratoria integer null, Nao_Sabe_Doenca_Respiratoria integer null, Insuficiencia_renal integer null, Outro_Doenca_Rins integer null, Nao_Sabe_Doenca_Rins integer null, maternidadeDeReferencia text null, situacaoPeso integer null, statusEhDependenteAlcool integer null, statusEhDependenteOutrasDrogas integer null, statusEhFumante integer null, statusEhGestante integer null, statusEstaAcamado integer null, statusEstaDomiciliado integer null, statusTemDiabetes integer null, statusTemDoencaRespiratoria integer null, statusTemHanseniase integer null, statusTemHipertensaoArterial integer null, statusTemTeveCancer integer null, statusTemTeveDoencasRins integer null, statusTemTuberculose integer null, statusTeveAvcDerrame integer null, statusTeveDoencaCardiaca integer null, statusTeveInfarto integer null, statusTeveInternadoem12Meses integer null, statusUsaOutrasPraticasIntegrativasOuComplementares integer null, statusUsaPlantasMedicinais integer null, statusDiagnosticoMental integer null, grauParentescoFamiliarFrequentado text null, Banho integer null, Acesso_a_sanitario integer null, Higiene_bucal integer null, Outros_higienePessoalSituacaoRua integer null, Restaurante_popular integer null, Doacao_grupo_religioso integer null, Doacao_restaurante integer null, Doacao_popular integer null, Outros_origemAlimentoSituacaoRua integer null, outraInstituicaoQueAcompanha text null, quantidadeAlimentacoesAoDiaSituacaoRua integer null, statusAcompanhadoPorOutraInstituicao integer null, statusPossuiReferenciaFamiliar integer null, statusRecebeBeneficio integer null, statusSituacaoRua integer null, statusTemAcessoHigienePessoalSituacaoRua integer null, statusVisitaFamiliarFrequentemente integer null, tempoSituacaoRua integer integer null, fichaAtualizada integer null, nomeSocial text null, codigoIbgeMunicipioNascimento text null, dataNascimentoCidadao text null, desconheceNomeMae integer null, emailCidadao text null, nacionalidadeCidadao integer null, nomeCidadao text null, nomeMaeCidadao text null, cnsCidadao text null, cnsResponsavelFamiliar text null, telefoneCelular text null, numeroNisPisPasep text null, paisNascimento integer null, racaCorCidadao integer null, sexoCidadao integer null, statusEhResponsavel integer null, etnia integer null, nomePaiCidadao text null, desconheceNomePai integer null, dtNaturalizacao text null, portariaNaturalizacao text null, dtEntradaBrasil text null, microarea text null, stForaArea integer null, AuditivaDeficiencias integer null, VisualDeficiencias integer null, Intelectual_CognitivaDeficiencias integer null, FisicaDeficiencias integer null, OutraDeficiencias integer null, grauInstrucaoCidadao integer null, ocupacaoCodigoCbo2002 text null, orientacaoSexualCidadao integer null, povoComunidadeTradicional text null, relacaoParentescoCidadao integer null, situacaoMercadoTrabalhoCidadao integer null, statusDesejaInformarOrientacaoSexual integer null, statusFrequentaBenzedeira integer null, statusFrequentaEscola integer null, statusMembroPovoComunidadeTradicional integer null, statusParticipaGrupoComunitario integer null, statusPossuiPlanoSaudePrivado integer null, statusTemAlgumaDeficiencia integer null, identidadeGeneroCidadao integer null, statusDesejaInformarIdentidadeGenero integer null, AdultoResponsavelresponsavelPorCrianca integer null, OutrasCriancasresponsavelPorCrianca integer null, AdolescenteresponsavelPorCrianca integer null, SozinharesponsavelPorCrianca integer null, CrecheresponsavelPorCrianca integer null, OutroresponsavelPorCrianca integer null, statusTermoRecusaCadastroIndividualAtencaoBasica integer null, motivoSaidaCidadao integer null, dataObito text null, numeroDO text null, status text null, observacao text null, latitude text null, longitude text null, CPF text null, RG text null, ComplementoRG text null, EstadoCivil text null, beneficiarioBolsaFamilia integer null, Justificativa text null, DataRegistro text null)';
        return $q.when(SQLiteService.executeSql(query));
      },

      alterTable: function() {
        $q.when(SQLiteService.executeSql('ALTER TABLE Cadastro_Individual add column status text null;'));
        console.log("status");
        $q.when(SQLiteService.executeSql('ALTER TABLE Cadastro_Individual add column CPF text null;'));
        console.log("CPF");
        $q.when(SQLiteService.executeSql('ALTER TABLE Cadastro_Individual add column RG text null;'));
        console.log("RG");
        $q.when(SQLiteService.executeSql('ALTER TABLE Cadastro_Individual add column ComplementoRG text null;'));
        console.log("ComplementoRG");
        $q.when(SQLiteService.executeSql('ALTER TABLE Cadastro_Individual add column EstadoCivil text null;'));
        console.log("EstadoCivil");
        $q.when(SQLiteService.executeSql('ALTER TABLE Cadastro_Individual add column beneficiarioBolsaFamilia integer null;'));
        console.log("beneficiarioBolsaFamilia");
        $q.when(SQLiteService.executeSql('ALTER TABLE Cadastro_Individual add column Justificativa text null;'));
        console.log("Justificativa");
        $q.when(SQLiteService.executeSql('ALTER TABLE Cadastro_Individual add column DataRegistro text null;'));
        console.log("DataRegistro");
      },

      alterTable2: function() {
        var jSonQuery = {
          table: "Cadastro_Individual",
          fields: [{
              field: "uuid",
              query: "uuid text null"
            },
            {
              field: "CPF",
              query: "CPF text null"
            },
            {
              field: "RG",
              query: "RG text null"
            },
            {
              field: "ComplementoRG",
              query: "ComplementoRG text null"
            },
            {
              field: "EstadoCivil",
              query: "EstadoCivil text null"
            },
            {
              field: "beneficiarioBolsaFamilia",
              query: "beneficiarioBolsaFamilia integer null"
            },
            {
              field: "Justificativa",
              query: "Justificativa text null"
            },
            {
              field: "DataRegistro",
              query: "DataRegistro text null"
            }
          ]
        };

        //já rodou todos os campos e criou o que precisava
        if (startJQ >= jSonQuery.fields.length) return false;

        var query = 'PRAGMA table_info(' + jSonQuery.table + ')';
        $q.when(SQLiteService.getItems(query))
          .then(function(response) {

            alterTableFields = function(CamposTabela) {
              var find = false;
              //console.log(startJQ +" > "+ jSonQuery.fields.length +" = "+ (startJQ >= jSonQuery.fields.length));
              if (startJQ >= jSonQuery.fields.length) {
                return false;
              } else {

                for (var k = 0; k < CamposTabela.length; k++) {
                  //console.log(jSonQuery.fields[startJQ].field +" == "+ CamposTabela[k].name);
                  if (jSonQuery.fields[startJQ].field == CamposTabela[k].name) {
                    //console.log("já existe o campo "+ jSonQuery.fields[startJQ].field);
                    find = true;
                    break;
                  }
                }

                if (!find) {
                  //console.log("não encontrou o campo "+ jSonQuery.fields[startJQ].field);
                  $q.when(SQLiteService.executeSql('ALTER TABLE ' + jSonQuery.table + ' add column ' + jSonQuery.fields[startJQ].query))
                    .then(function(resp) {
                      //console.log('Criou a coluna '+ jSonQuery.fields[startJQ].query +' na tabela '+ jSonQuery.table);

                      startJQ++;
                      alterTableFields(CamposTabela);
                    })
                    .catch(function(err) {
                      //console.log('Erro ao criar a coluna '+ jSonQuery.fields[startJQ].query +' na tabela '+ jSonQuery.table);
                      console.log(err);

                      startJQ++;
                      alterTableFields(CamposTabela);
                    });
                } else {
                  startJQ++;
                  alterTableFields(CamposTabela);
                }
              }
            }

            alterTableFields(response);

          }).catch(function(err) {
            console.log(err);
          });
      },

      dropTable: function() {
        var query = 'DROP TABLE IF EXISTS Cadastro_Individual';
        return $q.when(SQLiteService.executeSql(query));
      },

      updatePeople: function(condition) {
        var query = 'UPDATE Cadastro_Individual SET ' + condition + '';
        return $q.when(SQLiteService.executeSql(query));
      },

      selectPeopleResponsible: function(cns) {
        var query = 'SELECT statusEhResponsavel FROM Cadastro_Individual WHERE cnsCidadao = ' + cns;
        return $q.when(SQLiteService.getItems(query));
      },

      selectTheAdressPeople: function(cns) {
        var query = 'SELECT * FROM FamiliaRow WHERE numeroCnsResponsavel = "' + cns + '"';
        return $q.when(SQLiteService.getItems(query));
      },

      selectThereAreChildrenForResponsible: function(cns) {
        var query = 'SELECT count(*) as NumberChilds FROM Cadastro_Individual WHERE cnsResponsavelFamiliar = ' + cns;
        return $q.when(SQLiteService.getItems(query));
      },

      selectSearchPeoples: function(search) {
        var query = 'SELECT id, nomeCidadao, cnsCidadao, dataAtendimento, status FROM Cadastro_Individual where nomeCidadao like "' + search + '%" order by ' +
          'case status ' +
          'when "Pendente de Edição" then 0 ' +
          'when "Em Edição" then 1 ' +
          'when "Aguardando Sincronismo" then 2 ' +
          'else 3 end, nomeCidadao';
        return $q.when(SQLiteService.getItems(query));
      },

      deletePeopleToAPI: function(condition) {
        var query = 'DELETE FROM Cadastro_Individual WHERE ' + condition;
        return $q.when(SQLiteService.getItems(query));
      },

      getPeople: function(condition) {
        // console.log(condition);
        var query = 'SELECT * FROM Cadastro_Individual WHERE ' + condition;
        return $q.when(SQLiteService.getItems(query));
      },

      getAllListPeople: function() {
        var query = 'SELECT id, nomeCidadao, cnsCidadao, cnsResponsavelFamiliar, dataAtendimento, status, statusEhResponsavel, statusTermoRecusaCadastroIndividualAtencaoBasica FROM Cadastro_Individual order by ' +
          'case status ' +
          'when "Pendente de Edição" then 0 ' +
          'when "Em Edição" then 1 ' +
          'when "Aguardando Sincronismo" then 2 ' +
          'else 3 end, nomeCidadao';
        return $q.when(SQLiteService.getItems(query));
      },

      addIdentificationPeople: function(dataInformation) {

        var query = 'INSERT INTO Cadastro_Individual (' +
          ' profissionalCNS ,' +
          ' cboCodigo_2002 ,' +
          ' cnes ,' +
          ' ine ,' +
          ' dataAtendimento,' +
          ' codigoIbgeMunicipio ,' +
          ' descricaoCausaInternacaoEm12Meses ,' +
          ' descricaoOutraCondicao1 ,' +
          ' descricaoOutraCondicao2 ,' +
          ' descricaoOutraCondicao3 ,' +
          ' descricaoPlantasMedicinaisUsadas ,' +
          ' Insuficiencia_cardiaca,' +
          ' Outro_Doenca_Cardiaca,' +
          ' Nao_Sabe_Doenca_Cardiaca ,' +
          ' Asma,' +
          ' DPOC_Enfisema ,' +
          ' Outro_Doenca_Respiratoria ,' +
          ' Nao_Sabe_Doenca_Respiratoria ,' +
          ' Insuficiencia_renal ,' +
          ' Outro_Doenca_Rins ,' +
          ' Nao_Sabe_Doenca_Rins ,' +
          ' maternidadeDeReferencia ,' +
          ' situacaoPeso ,' +
          ' statusEhDependenteAlcool ,' +
          ' statusEhDependenteOutrasDrogas ,' +
          ' statusEhFumante ,' +
          ' statusEhGestante ,' +
          ' statusEstaAcamado ,' +
          ' statusEstaDomiciliado ,' +
          ' statusTemDiabetes ,' +
          ' statusTemDoencaRespiratoria ,' +
          ' statusTemHanseniase ,' +
          ' statusTemHipertensaoArterial ,' +
          ' statusTemTeveCancer ,' +
          ' statusTemTeveDoencasRins ,' +
          ' statusTemTuberculose ,' +
          ' statusTeveAvcDerrame ,' +
          ' statusTeveDoencaCardiaca ,' +
          ' statusTeveInfarto ,' +
          ' statusTeveInternadoem12Meses ,' +
          ' statusUsaOutrasPraticasIntegrativasOuComplementares ,' +
          ' statusUsaPlantasMedicinais ,' +
          ' statusDiagnosticoMental ,' +
          ' grauParentescoFamiliarFrequentado ,' +
          ' Banho ,' +
          ' Acesso_a_sanitario ,' +
          ' Higiene_bucal ,' +
          ' Outros_higienePessoalSituacaoRua ,' +
          ' Restaurante_popular ,' +
          ' Doacao_grupo_religioso ,' +
          ' Doacao_restaurante ,' +
          ' Doacao_popular ,' +
          ' Outros_origemAlimentoSituacaoRua ,' +
          ' outraInstituicaoQueAcompanha ,' +
          ' quantidadeAlimentacoesAoDiaSituacaoRua ,' +
          ' statusAcompanhadoPorOutraInstituicao ,' +
          ' statusPossuiReferenciaFamiliar ,' +
          ' statusRecebeBeneficio ,' +
          ' statusSituacaoRua ,' +
          ' statusTemAcessoHigienePessoalSituacaoRua ,' +
          ' statusVisitaFamiliarFrequentemente ,' +
          ' tempoSituacaoRua ,' +
          ' fichaAtualizada ,' +
          ' nomeSocial ,' +
          ' codigoIbgeMunicipioNascimento ,' +
          ' dataNascimentoCidadao ,' +
          ' desconheceNomeMae ,' +
          ' emailCidadao ,' +
          ' nacionalidadeCidadao ,' +
          ' nomeCidadao ,' +
          ' nomeMaeCidadao ,' +
          ' cnsCidadao ,' +
          ' cnsResponsavelFamiliar ,' +
          ' telefoneCelular ,' +
          ' numeroNisPisPasep ,' +
          ' paisNascimento ,' +
          ' racaCorCidadao ,' +
          ' sexoCidadao ,' +
          ' statusEhResponsavel ,' +
          ' etnia ,' +
          ' nomePaiCidadao ,' +
          ' desconheceNomePai ,' +
          ' dtNaturalizacao,' +
          ' portariaNaturalizacao,' +
          ' dtEntradaBrasil,' +
          ' microarea ,' +
          ' stForaArea ,' +
          ' AuditivaDeficiencias ,' +
          ' VisualDeficiencias ,' +
          ' Intelectual_CognitivaDeficiencias ,' +
          ' FisicaDeficiencias ,' +
          ' OutraDeficiencias ,' +
          ' grauInstrucaoCidadao,' +
          ' ocupacaoCodigoCbo2002,' +
          ' orientacaoSexualCidadao,' +
          ' povoComunidadeTradicional,' +
          ' relacaoParentescoCidadao,' +
          ' situacaoMercadoTrabalhoCidadao,' +
          ' statusDesejaInformarOrientacaoSexual,' +
          ' statusFrequentaBenzedeira,' +
          ' statusFrequentaEscola,' +
          ' statusMembroPovoComunidadeTradicional,' +
          ' statusParticipaGrupoComunitario,' +
          ' statusPossuiPlanoSaudePrivado,' +
          ' statusTemAlgumaDeficiencia,' +
          ' identidadeGeneroCidadao,' +
          ' statusDesejaInformarIdentidadeGenero,' +
          ' AdultoResponsavelresponsavelPorCrianca  ,' +
          ' OutrasCriancasresponsavelPorCrianca  ,' +
          ' AdolescenteresponsavelPorCrianca  ,' +
          ' SozinharesponsavelPorCrianca  ,' +
          ' CrecheresponsavelPorCrianca  ,' +
          ' OutroresponsavelPorCrianca  ,' +
          ' statusTermoRecusaCadastroIndividualAtencaoBasica,' +
          ' motivoSaidaCidadao,' +
          ' dataObito,' +
          ' numeroDO, ' +
          ' status, ' +
          ' observacao, ' +
          ' uuidFichaOriginadora,' +
          ' uuid,' +
          ' token, ' +
          ' latitude, ' +
          ' longitude, ' +
          ' CPF, ' +
          ' RG, ' +
          ' ComplementoRG, ' +
          ' EstadoCivil, ' +
          ' beneficiarioBolsaFamilia, ' +
          ' Justificativa, ' +
          ' DataRegistro ' +
          ' ) VALUES ( ' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '? )';
        return $q.when(SQLiteService.executeSql(query, [
          dataInformation.profissionalCNS,
          dataInformation.cboCodigo_2002,
          dataInformation.cnes,
          dataInformation.ine,
          dataInformation.dataAtendimento,
          dataInformation.codigoIbgeMunicipio,
          dataInformation.descricaoCausaInternacaoEm12Meses,
          dataInformation.descricaoOutraCondicao1,
          dataInformation.descricaoOutraCondicao2,
          dataInformation.descricaoOutraCondicao3,
          dataInformation.descricaoPlantasMedicinaisUsadas,
          dataInformation.Insuficiencia_cardiaca,
          dataInformation.Outro_Doenca_Cardiaca,
          dataInformation.Nao_Sabe_Doenca_Cardiaca,
          dataInformation.Asma,
          dataInformation.DPOC_Enfisema,
          dataInformation.Outro_Doenca_Respiratoria,
          dataInformation.Nao_Sabe_Doenca_Respiratoria,
          dataInformation.Insuficiencia_renal,
          dataInformation.Outro_Doenca_Rins,
          dataInformation.Nao_Sabe_Doenca_Rins,
          dataInformation.maternidadeDeReferencia,
          dataInformation.situacaoPeso,
          dataInformation.statusEhDependenteAlcool,
          dataInformation.statusEhDependenteOutrasDrogas,
          dataInformation.statusEhFumante,
          dataInformation.statusEhGestante,
          dataInformation.statusEstaAcamado,
          dataInformation.statusEstaDomiciliado,
          dataInformation.statusTemDiabetes,
          dataInformation.statusTemDoencaRespiratoria,
          dataInformation.statusTemHanseniase,
          dataInformation.statusTemHipertensaoArterial,
          dataInformation.statusTemTeveCancer,
          dataInformation.statusTemTeveDoencasRins,
          dataInformation.statusTemTuberculose,
          dataInformation.statusTeveAvcDerrame,
          dataInformation.statusTeveDoencaCardiaca,
          dataInformation.statusTeveInfarto,
          dataInformation.statusTeveInternadoem12Meses,
          dataInformation.statusUsaOutrasPraticasIntegrativasOuComplementares,
          dataInformation.statusUsaPlantasMedicinais,
          dataInformation.statusDiagnosticoMental,
          dataInformation.grauParentescoFamiliarFrequentado,
          dataInformation.Banho,
          dataInformation.Acesso_a_sanitario,
          dataInformation.Higiene_bucal,
          dataInformation.Outros_higienePessoalSituacaoRua,
          dataInformation.Restaurante_popular,
          dataInformation.Doacao_grupo_religioso,
          dataInformation.Doacao_restaurante,
          dataInformation.Doacao_popular,
          dataInformation.Outros_origemAlimentoSituacaoRua,
          dataInformation.outraInstituicaoQueAcompanha,
          dataInformation.quantidadeAlimentacoesAoDiaSituacaoRua,
          dataInformation.statusAcompanhadoPorOutraInstituicao,
          dataInformation.statusPossuiReferenciaFamiliar,
          dataInformation.statusRecebeBeneficio,
          dataInformation.statusSituacaoRua,
          dataInformation.statusTemAcessoHigienePessoalSituacaoRua,
          dataInformation.statusVisitaFamiliarFrequentemente,
          dataInformation.tempoSituacaoRua,
          dataInformation.fichaAtualizada,
          dataInformation.nomeSocial,
          dataInformation.codigoIbgeMunicipioNascimento,
          dataInformation.dataNascimentoCidadao,
          dataInformation.desconheceNomeMae,
          dataInformation.emailCidadao,
          dataInformation.nacionalidadeCidadao,
          dataInformation.nomeCidadao,
          dataInformation.nomeMaeCidadao,
          dataInformation.cnsCidadao,
          dataInformation.cnsResponsavelFamiliar,
          dataInformation.telefoneCelular,
          dataInformation.numeroNisPisPasep,
          dataInformation.paisNascimento,
          dataInformation.racaCorCidadao,
          dataInformation.sexoCidadao,
          dataInformation.statusEhResponsavel,
          dataInformation.etnia,
          dataInformation.nomePaiCidadao,
          dataInformation.desconheceNomePai,
          dataInformation.dtNaturalizacao,
          dataInformation.portariaNaturalizacao,
          dataInformation.dtEntradaBrasil,
          dataInformation.microarea,
          dataInformation.stForaArea,
          dataInformation.AuditivaDeficiencias,
          dataInformation.VisualDeficiencias,
          dataInformation.Intelectual_CognitivaDeficiencias,
          dataInformation.FisicaDeficiencias,
          dataInformation.OutraDeficiencias,
          dataInformation.grauInstrucaoCidadao,
          dataInformation.ocupacaoCodigoCbo2002,
          dataInformation.orientacaoSexualCidadao,
          dataInformation.povoComunidadeTradicional,
          dataInformation.relacaoParentescoCidadao,
          dataInformation.situacaoMercadoTrabalhoCidadao,
          dataInformation.statusDesejaInformarOrientacaoSexual,
          dataInformation.statusFrequentaBenzedeira,
          dataInformation.statusFrequentaEscola,
          dataInformation.statusMembroPovoComunidadeTradicional,
          dataInformation.statusParticipaGrupoComunitario,
          dataInformation.statusPossuiPlanoSaudePrivado,
          dataInformation.statusTemAlgumaDeficiencia,
          dataInformation.identidadeGeneroCidadao,
          dataInformation.statusDesejaInformarIdentidadeGenero,
          dataInformation.AdultoResponsavelresponsavelPorCrianca,
          dataInformation.OutrasCriancasresponsavelPorCrianca,
          dataInformation.AdolescenteresponsavelPorCrianca,
          dataInformation.SozinharesponsavelPorCrianca,
          dataInformation.CrecheresponsavelPorCrianca,
          dataInformation.OutroresponsavelPorCrianca,
          dataInformation.statusTermoRecusaCadastroIndividualAtencaoBasica,
          dataInformation.motivoSaidaCidadao,
          dataInformation.dataObito,
          dataInformation.numeroDO,
          dataInformation.status,
          dataInformation.observacao,
          dataInformation.uuidFichaOriginadora,
          dataInformation.uuid,
          dataInformation.token,
          dataInformation.latitude,
          dataInformation.longitude,
          dataInformation.CPF,
          dataInformation.RG,
          dataInformation.ComplementoRG,
          dataInformation.EstadoCivil,
          dataInformation.beneficiarioBolsaFamilia,
          null, //dataInformation.Justificativa,
          dataInformation.DataRegistro
        ]));
      },

      insertRefusePeopleRegistration: function(dataInformation) {
        console.log(dataInformation);
        dataInformation.status = 'Aguardando Sincronismo';
        var query = 'INSERT INTO Cadastro_Individual (' +
          ' profissionalCNS, ' /* headerTransport */ +
          ' cboCodigo_2002, ' +
          ' cnes, ' +
          ' ine, ' +
          ' dataAtendimento, ' +
          ' codigoIbgeMunicipio, ' +
          ' DataRegistro, ' +
          ' codigoIbgeMunicipioNascimento, ' +
          ' statusTermoRecusaCadastroIndividualAtencaoBasica, ' +
          ' dataNascimentoCidadao, ' +
          ' nomeCidadao, ' +
          ' desconheceNomeMae, ' +
          ' nacionalidadeCidadao, ' +
          ' nomeMaeCidadao, ' +
          ' paisNascimento, ' +
          ' racaCorCidadao, ' +
          ' sexoCidadao, ' +
          ' etnia, ' +
          ' nomePaiCidadao, ' +
          ' desconheceNomePai, ' +
          ' dtNaturalizacao, ' +
          ' portariaNaturalizacao, ' +
          ' dtEntradaBrasil, ' +
          ' microarea, ' +
          ' stForaArea, ' +
          ' justificativa, ' +
          ' status ' +
          ' ) VALUES ( ' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?,' +
          '?)';
        return $q.when(SQLiteService.executeSql(query, [
          dataInformation.profissionalCNS,
          dataInformation.cboCodigo_2002,
          dataInformation.cnes,
          dataInformation.ine,
          dataInformation.dataAtendimento,
          dataInformation.codigoIbgeMunicipio,
          dataInformation.DataRegistro,
          dataInformation.codigoIbgeMunicipioNascimento,
          1,
          dataInformation.dataNascimentoCidadao,
          dataInformation.nomeCidadao,
          dataInformation.desconheceNomeMae,
          dataInformation.nacionalidadeCidadao,
          dataInformation.nomeMaeCidadao,
          dataInformation.paisNascimento,
          dataInformation.racaCorCidadao,
          dataInformation.sexoCidadao,
          dataInformation.etnia,
          dataInformation.nomePaiCidadao,
          dataInformation.desconheceNomePai,
          dataInformation.dtNaturalizacao,
          dataInformation.portariaNaturalizacao,
          dataInformation.dtEntradaBrasil,
          dataInformation.microarea,
          dataInformation.stForaArea,
          dataInformation.justificativa,
          dataInformation.status
          // 'Em Edição'
        ]));

      },

      updateInformationsPeople: function(dataInformation, condition) {
        var query = "UPDATE Cadastro_Individual set " +
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
        if (dataInformation.povoComunidadeTradicional != null)
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
          'OutraDeficiencias = ' + dataInformation.OutraDeficiencias + '' +
          ' WHERE ' + condition;

        return $q.when(SQLiteService.getItems(query));
      },

      updateEditInformationsPeople: function(dataInformation, geolocation, condition) {
        var query = "UPDATE Cadastro_Individual set " +
          ' dataAtendimento = ' + new Date().getTime() + ',' +
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
        if (dataInformation.microarea != null && dataInformation.microarea.length > 0) {
          query += ' microarea = "' + dataInformation.microarea + '",';
        } else {
          query += ' microarea = null,';
        }
        if (dataInformation.nomeCidadao != null && dataInformation.nomeCidadao.length > 0)
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
        if (dataInformation.numeroNisPisPasep != null)
          query += 'numeroNisPisPasep = "' + dataInformation.numeroNisPisPasep + '",';
        else
          query += 'numeroNisPisPasep = null,';
        if (dataInformation.nomeMaeCidadao != null)
          query += 'nomeMaeCidadao = "' + dataInformation.nomeMaeCidadao + '",';
        else
          query += 'nomeMaeCidadao = null,' +
          'desconheceNomeMae = ' + dataInformation.desconheceNomeMae + ',';
        if (dataInformation.nomePaiCidadao != null)
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
        if (dataInformation.telefoneCelular != null)
          query += 'telefoneCelular = "' + dataInformation.telefoneCelular + '",';
        else
          query += 'telefoneCelular = null,';
        if (dataInformation.emailCidadao != null)
          query += 'emailCidadao = "' + dataInformation.emailCidadao + '",';
        else
          query += 'emailCidadao = null,';
        if (dataInformation.observacao != null)
          query += 'observacao = "' + dataInformation.observacao + '",';
        else
          query += 'observacao = null,';
        query += ' status = "' + dataInformation.status + '",' +
          ' latitude = ' + geolocation.lat + ',' +
          ' longitude = ' + geolocation.long + '' +
          ' WHERE ' + condition;

        return $q.when(SQLiteService.getItems(query));
      },

      updateHelthPeople: function(dataInformation, condition) {
        var query = "UPDATE Cadastro_Individual set " +
          'statusEhGestante = ' + dataInformation.statusEhGestante + ',';
        if (dataInformation.maternidadeDeReferencia != null)
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
        if (dataInformation.descricaoCausaInternacaoEm12Meses != null)
          query += 'descricaoCausaInternacaoEm12Meses = "' + dataInformation.descricaoCausaInternacaoEm12Meses + '",';
        else
          query += 'descricaoCausaInternacaoEm12Meses = null,';
        query += 'statusDiagnosticoMental = ' + dataInformation.statusDiagnosticoMental + ',' +
          'statusEstaAcamado = ' + dataInformation.statusEstaAcamado + ',' +
          'statusEstaDomiciliado = ' + dataInformation.statusEstaDomiciliado + ',' +
          'statusUsaPlantasMedicinais = ' + dataInformation.statusUsaPlantasMedicinais + ',';
        if (dataInformation.descricaoPlantasMedicinaisUsadas != null)
          query += 'descricaoPlantasMedicinaisUsadas = "' + dataInformation.descricaoPlantasMedicinaisUsadas + '",';
        else
          query += 'descricaoPlantasMedicinaisUsadas = null,';
        query += 'statusUsaOutrasPraticasIntegrativasOuComplementares = ' + dataInformation.statusUsaOutrasPraticasIntegrativasOuComplementares + ',';
        if (dataInformation.descricaoOutraCondicao1 != null)
          query += 'descricaoOutraCondicao1 = "' + dataInformation.descricaoOutraCondicao1 + '",';
        else
          query += 'descricaoOutraCondicao1 = null,';
        if (dataInformation.descricaoOutraCondicao2 != null)
          query += 'descricaoOutraCondicao2 = "' + dataInformation.descricaoOutraCondicao2 + '",';
        else
          query += 'descricaoOutraCondicao2 = null,';
        if (dataInformation.descricaoOutraCondicao3 != null)
          query += 'descricaoOutraCondicao3 = "' + dataInformation.descricaoOutraCondicao3 + '"';
        else
          query += 'descricaoOutraCondicao3 = null';
        query += ' WHERE ' + condition;

        return $q.when(SQLiteService.getItems(query));
      },

      updateStreetPeople: function(dataInformation, condition) {
        var query = "UPDATE Cadastro_Individual set " +
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
        if (dataInformation.outraInstituicaoQueAcompanha != null) {
          query += 'outraInstituicaoQueAcompanha = "' + dataInformation.outraInstituicaoQueAcompanha + '",';
        } else {
          query += 'outraInstituicaoQueAcompanha = null,';
        }
        query += 'statusVisitaFamiliarFrequentemente = ' + dataInformation.statusVisitaFamiliarFrequentemente + ',';
        if (dataInformation.grauParentescoFamiliarFrequentado != null) {
          query += 'grauParentescoFamiliarFrequentado = "' + dataInformation.grauParentescoFamiliarFrequentado + '",';
        } else {
          query += 'grauParentescoFamiliarFrequentado = null,';
        }
        query += 'statusTemAcessoHigienePessoalSituacaoRua = ' + dataInformation.statusTemAcessoHigienePessoalSituacaoRua + ',' +
          'Banho = ' + dataInformation.Banho + ',' +
          'Acesso_a_sanitario = ' + dataInformation.Acesso_a_sanitario + ',' +
          'Higiene_bucal = ' + dataInformation.Higiene_bucal + ',' +
          'Outros_higienePessoalSituacaoRua = ' + dataInformation.Outros_higienePessoalSituacaoRua + '' +
          ' WHERE ' + condition;
        return $q.when(SQLiteService.getItems(query));
      },

      updateExitPeople: function(dataInformation, condition) {
        var query = "UPDATE Cadastro_Individual set " +
          'motivoSaidaCidadao = ' + dataInformation.motivoSaidaCidadao + ',';
        if (dataInformation.dataObito != null)
          query += 'dataObito = "' + dataInformation.dataObito + '",';
        else
          query += 'dataObito = null,';
        if (dataInformation.numeroDO != null)
          query += 'numeroDO = "' + dataInformation.numeroDO + '",';
        else
          query += 'numeroDO = null,';
        query += 'status = "' + dataInformation.status + '"' +
          ' WHERE ' + condition;
        return $q.when(SQLiteService.getItems(query));
      }
    };
  }]);
