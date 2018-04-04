angular.module('mobacs').factory('HomeRegistrationService', ['$q', 'SQLiteService', function($q, SQLiteService) {
  var startJQ = 0;

  return {
    selectSearchHome: function(search) {
      var query = 'SELECT id, nomeLogradouro, numero, bairro, dataAtendimento, status FROM cadastroDomiciliarTerritorial where nomeLogradouro like "%' + search + '%" order by ' +
        'case status ' +
        'when "Pendente de Edição" then 0 ' +
        'when "Em Edição" then 1 ' +
        'when "Aguardando Sincronismo" then 2 ' +
        'else 3 end, dataAtendimento';
      return $q.when(SQLiteService.getItems(query));
    },

    createTable: function() {
      var query = 'CREATE TABLE IF NOT EXISTS cadastroDomiciliarTerritorial ('
        /* ID para cada endereco */
        +
        'id integer primary key autoincrement not null,'

        /*Dados internos da aplicação*/
        +
        'fichaAtualizada text null,' +
        'uuid text null,' +
        'uuidFichaOriginadora text null,' +
        'token text null,' +
        'Justificativa text null,' +
        'DataRegistro text null,'

        /* headerTransport */
        +
        'profissionalCNS text null,' +
        'cboCodigo_2002 text null,' +
        'cnes text null,' +
        'ine text null,' +
        'dataAtendimento text null,' +
        'codigoIbgeMunicipioHeader text null,'

        /* enderecoLocalPermanencia */
        +
        'bairro text null,' +
        'cep text null,' +
        'codigoIbgeMunicipio text null,' +
        'complemento text null,' +
        'nomeLogradouro text null,' +
        'numero text null,' +
        'numeroDneUf text null,' +
        'telefoneContato text null,' +
        'telefoneResidencia text null,' +
        'tipoLogradouroNumeroDne text null,' +
        'stSemNumero integer null,' +
        'pontoReferencia text null,' +
        'microarea text null,' +
        'stForaArea integer null,' +
        'tipoDeImovel integer null,' +
        'observacao text null,'

        /* CondicaoMoradia */
        +
        'abastecimentoAgua integer null,' +
        'areaProducaoRural integer null,' +
        'destinoLixo integer null,' +
        'formaEscoamentoBanheiro integer null,' +
        'localizacao integer null,' +
        'materialPredominanteParedesExtDomicilio integer null,' +
        'nuComodos integer null,' +
        'nuMoradores integer null,' +
        'situacaoMoradiaPosseTerra integer null,' +
        'stDisponibilidadeEnergiaEletrica integer null,' +
        'tipoAcessoDomicilio integer null,' +
        'tipoDomicilio integer null,' +
        'aguaConsumoDomicilio integer null,'

        /* animaisNoDomicilio*/
        +
        'Gato integer null,' +
        'Cachorro integer null,' +
        'Passaro integer null,' +
        'Outros_AnimalNoDomicilio integer null,' +
        'quantosAnimaisNoDomicilio integer null,' +
        'stAnimaisNoDomicilio integer null,'

        /* statusTermoRecusa*/
        +
        'statusTermoRecusa integer null,'

        /* InstituicaoPermanencia*/
        +
        'nomeInstituicaoPermanencia text null,' +
        'stOutrosProfissionaisVinculados integer null,' +
        'nomeResponsavelTecnico text null,' +
        'cnsResponsavelTecnico text null,' +
        'cargoInstituicao text null,' +
        'telefoneResponsavelTecnico text null,'

        /*status ficha*/
        +
        'status text null,' +
        'latitude text null,' +
        'longitude text null )';

      return $q.when(SQLiteService.executeSql(query));
    },

    alterTable: function() {
      //$q.when(SQLiteService.executeSql('ALTER TABLE cadastroDomiciliarTerritorial add column telefoneResidencia text null;'));
      //console.log("telefoneResidencia");
      $q.when(SQLiteService.executeSql('ALTER TABLE cadastroDomiciliarTerritorial add column Justificativa text null;'));
      console.log("Justificativa");
      $q.when(SQLiteService.executeSql('ALTER TABLE cadastroDomiciliarTerritorial add column DataRegistro text null;'));
      console.log("DataRegistro");
    },
    //   var query = 'ALTER TABLE cadastroDomiciliarTerritorial add column uuidFichaOriginadora text null';
    //   return $q.when(SQLiteService.executeSql(query));
    // },

    alterTable2: function() {
      var jSonQuery = {
        table: "cadastroDomiciliarTerritorial",
        fields: [{
            field: "uuidFichaOriginadora",
            query: "uuidFichaOriginadora text null"
          },
          {
            field: "telefoneResidencia",
            query: "telefoneResidencia text null"
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
      var query = 'DROP TABLE IF EXISTS cadastroDomiciliarTerritorial';
      return $q.when(SQLiteService.executeSql(query));
    },

    getAllHomeRegistration: function() {
      var query = 'SELECT id, nomeLogradouro, numero, bairro, dataAtendimento, status FROM cadastroDomiciliarTerritorial order by ' +
        'case status ' +
        'when "Pendente de Edição" then 0 ' +
        'when "Em Edição" then 1 ' +
        'when "Aguardando Sincronismo" then 2 ' +
        'else 3 end, dataAtendimento';
      return $q.when(SQLiteService.getItems(query));
    },

    getHomeRegistration: function(condition) {
      var query = 'SELECT * FROM cadastroDomiciliarTerritorial WHERE ' + condition;
      return $q.when(SQLiteService.getItems(query));
    },

    selectAllHomeRegistration: function(condition) {
      var query = 'SELECT * FROM cadastroDomiciliarTerritorial';
      return $q.when(SQLiteService.getItems(query));
    },

    insertAdressHomeRegistration: function(dataInformation) {
      var query = 'INSERT INTO cadastroDomiciliarTerritorial (' +
        'profissionalCNS,' /* headerTransport */ +
        'cboCodigo_2002,' +
        'cnes,' +
        'ine,' +
        'dataAtendimento,' +
        'codigoIbgeMunicipioHeader,' +
        'observacao,' +
        'bairro,' /* enderecoLocalPermanencia */ +
        'cep,' +
        'codigoIbgeMunicipio,' +
        'complemento,' +
        'nomeLogradouro,' +
        'numero,' +
        'numeroDneUf,' +
        'telefoneContato,' +
        'telefoneResidencia,' +
        'tipoLogradouroNumeroDne,' +
        'stSemNumero,' +
        'tipoDeImovel,' +
        'pontoReferencia,' +
        'microarea,' +
        'stForaArea,' +
        'abastecimentoAgua,' /* CondicaoMoradia */ +
        'areaProducaoRural,' +
        'destinoLixo,' +
        'formaEscoamentoBanheiro,' +
        'localizacao,' +
        'materialPredominanteParedesExtDomicilio,' +
        'nuComodos,' +
        'nuMoradores ,' +
        'situacaoMoradiaPosseTerra,' +
        'stDisponibilidadeEnergiaEletrica,' +
        'tipoAcessoDomicilio,' +
        'tipoDomicilio,' +
        'aguaConsumoDomicilio,' +
        'Gato,' /* animaisNoDomicilio*/ +
        'Cachorro,' +
        'Passaro,' +
        'Outros_AnimalNoDomicilio,' +
        'quantosAnimaisNoDomicilio,' +
        'stAnimaisNoDomicilio,' +
        'statusTermoRecusa,' /* statusTermoRecusa*/ +
        'nomeInstituicaoPermanencia,' /* InstituicaoPermanencia*/ +
        'stOutrosProfissionaisVinculados,' +
        'nomeResponsavelTecnico,' +
        'cnsResponsavelTecnico,' +
        'cargoInstituicao,' +
        'telefoneResponsavelTecnico,' +
        'status,' /*status ficha*/ +
        'fichaAtualizada,' +
        'uuid,' +
        'uuidFichaOriginadora,' +
        'token,' +
        'latitude,' +
        'longitude,' +
        'DataRegistro' +
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
        '?)';
      return $q.when(SQLiteService.executeSql(query, [
        dataInformation.profissionalCNS,
        dataInformation.cboCodigo_2002,
        dataInformation.cnes,
        dataInformation.ine,
        dataInformation.dataAtendimento,
        dataInformation.codigoIbgeMunicipioHeader,
        dataInformation.observacao,
        dataInformation.bairro, /* enderecoLocalPermanencia */
        dataInformation.cep,
        dataInformation.codigoIbgeMunicipio,
        dataInformation.complemento,
        dataInformation.nomeLogradouro,
        dataInformation.numero,
        dataInformation.numeroDneUf,
        dataInformation.telefoneContato,
        dataInformation.telefoneResidencia,
        dataInformation.tipoLogradouroNumeroDne,
        dataInformation.stSemNumero,
        dataInformation.tipoDeImovel,
        dataInformation.pontoReferencia,
        dataInformation.microarea,
        dataInformation.stForaArea,
        dataInformation.abastecimentoAgua, /* CondicaoMoradia */
        dataInformation.areaProducaoRural,
        dataInformation.destinoLixo,
        dataInformation.formaEscoamentoBanheiro,
        dataInformation.localizacao,
        dataInformation.materialPredominanteParedesExtDomicilio,
        dataInformation.nuComodos,
        dataInformation.nuMoradores,
        dataInformation.situacaoMoradiaPosseTerra,
        dataInformation.stDisponibilidadeEnergiaEletrica,
        dataInformation.tipoAcessoDomicilio,
        dataInformation.tipoDomicilio,
        dataInformation.aguaConsumoDomicilio,
        dataInformation.Gato, /* animaisNoDomicilio*/
        dataInformation.Cachorro,
        dataInformation.Passaro,
        dataInformation.Outros_AnimalNoDomicilio,
        dataInformation.quantosAnimaisNoDomicilio,
        dataInformation.stAnimaisNoDomicilio,
        dataInformation.statusTermoRecusa, /* statusTermoRecusa*/
        dataInformation.nomeInstituicaoPermanencia, /* InstituicaoPermanencia*/
        dataInformation.stOutrosProfissionaisVinculados,
        dataInformation.nomeResponsavelTecnico,
        dataInformation.cnsResponsavelTecnico,
        dataInformation.cargoInstituicao,
        dataInformation.telefoneResponsavelTecnico,
        dataInformation.status, /*status ficha*/
        dataInformation.fichaAtualizada,
        dataInformation.uuid,
        dataInformation.uuidFichaOriginadora,
        dataInformation.token,
        dataInformation.latitude,
        dataInformation.longitude,
        new Date().getTime() //dataInformation.DataRegistro
      ]));
    },

    insertRefuseHomeRegistration: function(dataInformation) {
      var query = 'INSERT INTO cadastroDomiciliarTerritorial (' +
        'profissionalCNS,' /* headerTransport */ +
        'cboCodigo_2002,' +
        'cnes,' +
        'ine,' +
        'dataAtendimento,' +
        'codigoIbgeMunicipioHeader,' +
        'statusTermoRecusa,' +
        'Justificativa,' +
        'DataRegistro,' +
        'status' /*status ficha*/ +
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
        '?)';
      return $q.when(SQLiteService.executeSql(query, [
        dataInformation.profissionalCNS,
        dataInformation.cboCodigo_2002,
        dataInformation.cnes,
        dataInformation.ine,
        dataInformation.dataAtendimento,
        dataInformation.codigoIbgeMunicipioHeader,
        1,
        dataInformation.Justificativa,
        dataInformation.DataRegistro,
        'Aguardando Sincronismo'
      ]));

    },

    updateAdress: function(dataInformation, geolocation, condition) {
      var query = 'UPDATE cadastroDomiciliarTerritorial SET ' +
        'dataAtendimento = ' + new Date().getTime() + ',';
      if (dataInformation.bairro != null) {
        query += 'bairro = "' + dataInformation.bairro + '",';
      } else {
        query += 'bairro = null,';
      }
      if (dataInformation.cep != null) {
        query += 'cep = "' + dataInformation.cep + '",';
      } else {
        query += 'cep = null,';
      }
      if (dataInformation.codigoIbgeMunicipio != null) {
        query += 'codigoIbgeMunicipio = "' + dataInformation.codigoIbgeMunicipio + '",';
      } else {
        query += 'codigoIbgeMunicipio = null,';
      }
      if (dataInformation.complemento != null) {
        query += 'complemento = "' + dataInformation.complemento + '",';
      } else {
        query += 'complemento = null,';
      }
      if (dataInformation.nomeLogradouro != null) {
        query += 'nomeLogradouro = "' + dataInformation.nomeLogradouro + '",';
      } else {
        query += 'nomeLogradouro = null,';
      }
      if (dataInformation.numero != null) {
        query += 'numero = "' + dataInformation.numero + '",';
      } else {
        query += 'numero = null,';
      }
      if (dataInformation.numeroDneUf != null) {
        query += 'numeroDneUf = "' + dataInformation.numeroDneUf + '",';
      } else {
        query += 'numeroDneUf = null,';
      }
      if (dataInformation.telefoneContato != null) {
        query += 'telefoneContato = "' + dataInformation.telefoneContato + '",';
      } else {
        query += 'telefoneContato = null,';
      }
      if (dataInformation.telefoneResidencia != null) {
        query += 'telefoneResidencia = "' + dataInformation.telefoneResidencia + '",';
      } else {
        query += 'telefoneResidencia = null,';
      }
      if (dataInformation.tipoLogradouroNumeroDne != null) {
        query += 'tipoLogradouroNumeroDne = "' + dataInformation.tipoLogradouroNumeroDne + '",';
      } else {
        query += 'tipoLogradouroNumeroDne = null,';
      }
      query += 'stSemNumero = ' + dataInformation.stSemNumero + ',';
      if (dataInformation.pontoReferencia != null) {
        query += 'pontoReferencia = "' + dataInformation.pontoReferencia + '",';
      } else {
        query += 'pontoReferencia = null,';
      }
      if (dataInformation.microarea != null) {
        query += 'microarea = "' + dataInformation.microarea + '",';
      } else {
        query += 'microarea = null,';
      }
      query += 'stForaArea = ' + dataInformation.stForaArea + ',' +
        'tipoDeImovel = ' + dataInformation.tipoDeImovel + ',';
      if (dataInformation.observacao != null) {
        query += 'observacao = "' + dataInformation.observacao + '",';
      } else {
        query += 'observacao = null,';
      }
      query += 'status = "' + dataInformation.status + '",' +
        ' latitude = ' + geolocation.lat + ',' +
        ' longitude = ' + geolocation.long + ','
        //+ ' Justificativa = "'+ dataInformation.Justificativa+'",';
        +
        ' Justificativa = null,' +
        ' DataRegistro = ' + new Date().getTime() +
        ' WHERE ' + condition + ' ';
      return $q.when(SQLiteService.executeSql(query));
    },

    updateLivingConditions: function(dataInformation, condition) {
      var query = 'UPDATE cadastroDomiciliarTerritorial SET ' +
        ' situacaoMoradiaPosseTerra= ' + dataInformation.situacaoMoradiaPosseTerra + ',' +
        ' localizacao= ' + dataInformation.localizacao + ',' +
        ' tipoDomicilio= ' + dataInformation.tipoDomicilio + ',';
      if (dataInformation.nuMoradores != null) {
        query += ' nuMoradores= "' + dataInformation.nuMoradores + '",';
      } else {
        query += ' nuMoradores= null,';
      }
      if (dataInformation.nuComodos != null) {
        query += ' nuComodos= "' + dataInformation.nuComodos + '",';
      } else {
        query += ' nuComodos= null,';
      }
      query += ' areaProducaoRural= ' + dataInformation.areaProducaoRural + ',' +
        ' tipoAcessoDomicilio= ' + dataInformation.tipoAcessoDomicilio + ',' +
        ' stDisponibilidadeEnergiaEletrica= ' + dataInformation.stDisponibilidadeEnergiaEletrica + ',' +
        ' materialPredominanteParedesExtDomicilio= ' + dataInformation.materialPredominanteParedesExtDomicilio + ',' +
        ' abastecimentoAgua= ' + dataInformation.abastecimentoAgua + ',' +
        ' formaEscoamentoBanheiro= ' + dataInformation.formaEscoamentoBanheiro + ',' +
        ' aguaConsumoDomicilio= ' + dataInformation.aguaConsumoDomicilio + ',' +
        ' destinoLixo= ' + dataInformation.destinoLixo + '' +
        ' WHERE ' + condition + '';
      return $q.when(SQLiteService.getItems(query));
    },

    updateAnimals: function(dataInformation, condition) {

      var query = 'UPDATE cadastroDomiciliarTerritorial SET ' +
        'stAnimaisNoDomicilio = ' + dataInformation.stAnimaisNoDomicilio + ',' +
        'Gato = ' + dataInformation.Gato + ',' +
        'Cachorro = ' + dataInformation.Cachorro + ',' +
        'Passaro = ' + dataInformation.Passaro + ',' +
        'Outros_AnimalNoDomicilio = ' + dataInformation.Outros_AnimalNoDomicilio + ',';
      if (dataInformation.quantosAnimaisNoDomicilio != null) {
        query += 'quantosAnimaisNoDomicilio = "' + dataInformation.quantosAnimaisNoDomicilio + '"';
      } else {
        query += 'quantosAnimaisNoDomicilio = null';
      }
      query += ' WHERE ' + condition + '';

      return $q.when(SQLiteService.getItems(query));
    },

    updateRefuse: function(dataInformation, condition) {
      var query = 'UPDATE cadastroDomiciliarTerritorial SET' +
        ' abastecimentoAgua = null,' /* CondicaoMoradia */ +
        'areaProducaoRural = null,' +
        'destinoLixo = null,' +
        'formaEscoamentoBanheiro = null,' +
        'localizacao = null,' +
        'materialPredominanteParedesExtDomicilio = null,' +
        'nuComodos = null,' +
        'nuMoradores = null,' +
        'situacaoMoradiaPosseTerra = null,' +
        'stDisponibilidadeEnergiaEletrica = null,' +
        'tipoAcessoDomicilio = null,' +
        'tipoDomicilio = null,' +
        'aguaConsumoDomicilio = null,' +
        'Gato = null,' /* animaisNoDomicilio*/ +
        'Cachorro = null,' +
        'Passaro = null,' +
        'Outros_AnimalNoDomicilio = null,' +
        'quantosAnimaisNoDomicilio = null,' +
        'stAnimaisNoDomicilio = null,' +
        'statusTermoRecusa = ' + dataInformation.statusTermoRecusa + ',' +
        'Justificativa = ' + dataInformation.Justificativa + ',' +
        'status = "Aguardando Sincronismo"' +
        ' WHERE ' + condition + '';

      return $q.when(SQLiteService.getItems(query));
    },

    updateInstituition: function(dataInformation, condition) {
      var query = 'UPDATE cadastroDomiciliarTerritorial SET ';
      if (dataInformation.nomeInstituicaoPermanencia != null) {
        query += 'nomeInstituicaoPermanencia=' + dataInformation.nomeInstituicaoPermanencia + ',';
      } else {
        query += 'nomeInstituicaoPermanencia=null,';
      }
      query += 'stOutrosProfissionaisVinculados=' + dataInformation.stOutrosProfissionaisVinculados + ',';
      if (dataInformation.nomeResponsavelTecnico != null) {
        query += 'nomeResponsavelTecnico="' + dataInformation.nomeResponsavelTecnico + '",';
      } else {
        query += 'nomeResponsavelTecnico=null,';
      }
      if (dataInformation.cnsResponsavelTecnico != null) {
        query += 'cnsResponsavelTecnico=' + dataInformation.cnsResponsavelTecnico + ',';
      } else {
        query += 'cnsResponsavelTecnico=null,';
      }
      if (dataInformation.cargoInstituicao != null) {
        query += 'cargoInstituicao=' + dataInformation.cargoInstituicao + ',';
      } else {
        query += 'cargoInstituicao=null,';
      }
      if (dataInformation.telefoneResponsavelTecnico != null) {
        query += 'telefoneResponsavelTecnico=' + dataInformation.telefoneResponsavelTecnico + ',';
      } else {
        query += 'telefoneResponsavelTecnico=null,';
      }
      query += 'status = "' + dataInformation.status + '"' +
        ' WHERE ' + condition + '';

      return $q.when(SQLiteService.getItems(query));
    },

    updateGenericHomeAdress: function(data, condition) {
      var query = 'UPDATE cadastroDomiciliarTerritorial SET ' + data + ' WHERE ' + condition + ' ';
      return $q.when(SQLiteService.executeSql(query));
    },

    getFamilies: function(condition) {
      var query = 'SELECT * FROM FamiliaRow WHERE ' + condition + '';
      return $q.when(SQLiteService.getItems(query));
    },

    deleteHomeAdress: function(condition) {
      var query = 'DELETE from cadastroDomiciliarTerritorial WHERE ' + condition;
      return $q.when(SQLiteService.getItems(query));
    }
  }
}])
