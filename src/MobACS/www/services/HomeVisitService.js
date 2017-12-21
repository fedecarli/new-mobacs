angular.module('mobacs')
  .factory('HomeVisitService', ['$q', 'SQLiteService', function($q, SQLiteService) {
    return {

      deleteHomeVisitToAPI: function(condition) {
        var query = 'DELETE FROM Visita_Domiciliar where ' + condition;
        return $q.when(SQLiteService.executeSql(query));
      },

      getAllHomeVisit: function() {
        var query = 'SELECT * FROM Visita_Domiciliar order by id desc';
        return $q.when(SQLiteService.getItems(query));
      },

      selectListAllHomeVisit: function() {
        var query = 'SELECT vd.id, vd.cnsCidadao, vd.microarea, vd.dataAtendimento, vd.status, cd.nomeCidadao FROM ' +
          'Visita_Domiciliar as vd left join Cadastro_Individual as cd ' +
          'on vd.cnsCidadao = cd.cnsCidadao ' +
          'order by vd.id desc';
        return $q.when(SQLiteService.getItems(query));
      },

      getHomeVisit: function(condition) {
        var query = 'SELECT * FROM Visita_Domiciliar WHERE ' + condition;
        return $q.when(SQLiteService.getItems(query));
      },

      getNumberOfHomeVisit: function(condition) {
        var query = 'SELECT count(*) as NumberOfHomeVisit FROM Visita_Domiciliar WHERE status = "Aguardando Sincronismo"';
        return $q.when(SQLiteService.getItems(query));
      },

      addInformationsHomeVisit: function(dataInformation) {
        var query = 'INSERT INTO Visita_Domiciliar ( ' +
          'profissionalCNS,' +
          'cboCodigo_2002,' +
          'cnes,' +
          'ine,' +
          'dataAtendimento,' +
          'codigoIbgeMunicipio,' +
          'turno,' +
          'numProntuario,' +
          'cnsCidadao,' +
          'dtNascimento,' +
          'sexo,' +
          'statusVisitaCompartilhadaOutroProfissional,' +
          'observacao,' + //Campo para o ACS inserir informações
          'Cadastramento_Atualizacao,' +
          'Visita_periodica,' +
          'Consulta,' +
          'Exame,' +
          'Vacina,' +
          'Condicionalidadesdobolsafamilia,' +
          'Gestante,' +
          'Puerpera,' +
          'Recem_nascido,' +
          'Crianca,' +
          'PessoaDesnutricao,' +
          'PessoaReabilitacaoDeficiencia,' +
          'Hipertensao,' +
          'Diabetes,' +
          'Asma,' +
          'DPOC_enfisema,' +
          'Cancer,' +
          'Outras_doencas_cronicas,' +
          'Hanseniase,' +
          'Tuberculose,' +
          'Sintomaticos_Respiratorios,' +
          'Tabagista,' +
          'Domiciliados_Acamados,' +
          'Condicoes_vulnerabilidade_social,' +
          'Condicionalidades_bolsa_familia,' +
          'Saude_mental,' +
          'Usuario_alcool,' +
          'Usuario_outras_drogas,' +
          'Acao_educativa,' +
          'Imovel_com_foco,' +
          'Acao_mecanica,' +
          'Tratamento_focal,' +
          'Egresso_de_internacao,' +
          'Convite_atividades_coletivas_campanha_saude,' +
          'Orientacao_Prevencao,' +
          'Outros,' +
          'desfecho,' +
          'microarea,' +
          'stForaArea,' +
          'tipoDeImovel,' +
          'pesoAcompanhamentoNutricional,' +
          'alturaAcompanhamentoNutricional,' +
          'status,' +
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
          '?,' +
          '?,' +
          '?,' +
          '? )';
        return $q.when(SQLiteService.executeSql(query, [
          dataInformation.profissionalCNS,
          dataInformation.cboCodigo_2002,
          dataInformation.cnes,
          dataInformation.ine,
          dataInformation.dataAtendimento.getTime(),
          dataInformation.codigoIbgeMunicipio,
          dataInformation.turno,
          dataInformation.numProntuario,
          dataInformation.cnsCidadao,
          dataInformation.dtNascimento,
          dataInformation.sexo,
          dataInformation.statusVisitaCompartilhadaOutroProfissional,
          dataInformation.observacao,
          dataInformation.Cadastramento_Atualizacao,
          dataInformation.Visita_periodica,
          dataInformation.Consulta,
          dataInformation.Exame,
          dataInformation.Vacina,
          dataInformation.Condicionalidadesdobolsafamilia,
          dataInformation.Gestante,
          dataInformation.Puerpera,
          dataInformation.Recem_nascido,
          dataInformation.Crianca,
          dataInformation.PessoaDesnutricao,
          dataInformation.PessoaReabilitacaoDeficiencia,
          dataInformation.Hipertensao,
          dataInformation.Diabetes,
          dataInformation.Asma,
          dataInformation.DPOC_enfisema,
          dataInformation.Cancer,
          dataInformation.Outras_doencas_cronicas,
          dataInformation.Hanseniase,
          dataInformation.Tuberculose,
          dataInformation.Sintomaticos_Respiratorios,
          dataInformation.Tabagista,
          dataInformation.Domiciliados_Acamados,
          dataInformation.Condicoes_vulnerabilidade_social,
          dataInformation.Condicionalidades_bolsa_familia,
          dataInformation.Saude_mental,
          dataInformation.Usuario_alcool,
          dataInformation.Usuario_outras_drogas,
          dataInformation.Acao_educativa,
          dataInformation.Imovel_com_foco,
          dataInformation.Acao_mecanica,
          dataInformation.Tratamento_focal,
          dataInformation.Egresso_de_internacao,
          dataInformation.Convite_atividades_coletivas_campanha_saude,
          dataInformation.Orientacao_Prevencao,
          dataInformation.Outros,
          dataInformation.desfecho,
          dataInformation.microarea,
          dataInformation.stForaArea,
          dataInformation.tipoDeImovel,
          dataInformation.pesoAcompanhamentoNutricional,
          dataInformation.alturaAcompanhamentoNutricional,
          dataInformation.status,
          dataInformation.latitude,
          dataInformation.longitude,
          dataInformation.DataRegistro
        ]));
      },

      updateEditHomeVisit: function(dataHomeVisit, geolocation, condition) {
        var query = 'UPDATE Visita_Domiciliar SET' +
          ' dataAtendimento = ' + new Date().getTime() + ',' +
          ' DataRegistro = ' + new Date().getTime() + ',' +
          ' turno = ' + dataHomeVisit.turno + ',' +
          ' stForaArea = ' + dataHomeVisit.stForaArea + ',';
        if (dataHomeVisit.microarea != null) {
          query += ' microarea = "' + dataHomeVisit.microarea + '",';
        } else {
          query += ' microarea = null,';
        }
        query += ' tipoDeImovel = ' + dataHomeVisit.tipoDeImovel + ',';
        if (dataHomeVisit.numProntuario != null)
          query += ' numProntuario = "' + dataHomeVisit.numProntuario + '",';
        else
          query += ' numProntuario = null,';
        query += ' cnsCidadao = ' + dataHomeVisit.cnsCidadao + ',' +
          ' statusVisitaCompartilhadaOutroProfissional = ' + dataHomeVisit.statusVisitaCompartilhadaOutroProfissional + ',';
        if (dataHomeVisit.dtNascimento != null) {
          query += ' dtNascimento = "' + dataHomeVisit.dtNascimento + '",';
        } else {
          query += ' dtNascimento = null,';
        }
        query += ' sexo = ' + dataHomeVisit.sexo + ',';
        if (dataHomeVisit.observacao != null) {
          query += ' observacao = "' + dataHomeVisit.observacao + '",';
        } else {
          query += ' observacao = null,';
        }
        query += ' status = "' + dataHomeVisit.status + '",' +
          ' latitude = ' + geolocation.lat + ',' +
          ' longitude = ' + geolocation.long + '' +
          ' WHERE ' + condition;
        return $q.when(SQLiteService.executeSql(query));
      },

      updateMotiveVisitHomeVisit: function(dataHomeVisit, condition) {
        var query = 'UPDATE Visita_Domiciliar SET ' +
          'Cadastramento_Atualizacao  = ' + dataHomeVisit.Cadastramento_Atualizacao + ',' +
          ' Visita_periodica = ' + dataHomeVisit.Visita_periodica + ',' +
          ' Consulta = ' + dataHomeVisit.Consulta + ',' +
          ' Exame = ' + dataHomeVisit.Exame + ',' +
          ' Vacina = ' + dataHomeVisit.Vacina + ',' +
          ' Condicionalidadesdobolsafamilia = ' + dataHomeVisit.Condicionalidadesdobolsafamilia + ',' +
          ' Gestante = ' + dataHomeVisit.Gestante + ',' +
          ' Puerpera = ' + dataHomeVisit.Puerpera + ',' +
          ' Recem_nascido = ' + dataHomeVisit.Recem_nascido + ',' +
          ' Crianca = ' + dataHomeVisit.Crianca + ',' +
          ' PessoaDesnutricao = ' + dataHomeVisit.PessoaDesnutricao + ',' +
          ' PessoaReabilitacaoDeficiencia = ' + dataHomeVisit.PessoaReabilitacaoDeficiencia + ',' +
          ' Hipertensao = ' + dataHomeVisit.Hipertensao + ',' +
          ' Diabetes = ' + dataHomeVisit.Diabetes + ',' +
          ' Asma = ' + dataHomeVisit.Asma + ',' +
          ' DPOC_enfisema = ' + dataHomeVisit.DPOC_enfisema + ',' +
          ' Cancer = ' + dataHomeVisit.Cancer + ',' +
          ' Outras_doencas_cronicas = ' + dataHomeVisit.Outras_doencas_cronicas + ',' +
          ' Hanseniase = ' + dataHomeVisit.Hanseniase + ',' +
          ' Tuberculose = ' + dataHomeVisit.Tuberculose + ',' +
          ' Sintomaticos_Respiratorios = ' + dataHomeVisit.Sintomaticos_Respiratorios + ',' +
          ' Tabagista = ' + dataHomeVisit.Tabagista + ',' +
          ' Domiciliados_Acamados = ' + dataHomeVisit.Domiciliados_Acamados + ',' +
          ' Condicoes_vulnerabilidade_social = ' + dataHomeVisit.Condicoes_vulnerabilidade_social + ',' +
          ' Condicionalidades_bolsa_familia = ' + dataHomeVisit.Condicionalidades_bolsa_familia + ',' +
          ' Saude_mental = ' + dataHomeVisit.Saude_mental + ',' +
          ' Usuario_alcool = ' + dataHomeVisit.Usuario_alcool + ',' +
          ' Usuario_outras_drogas = ' + dataHomeVisit.Usuario_outras_drogas + ',' +
          ' Acao_educativa = ' + dataHomeVisit.Acao_educativa + ',' +
          ' Imovel_com_foco = ' + dataHomeVisit.Imovel_com_foco + ',' +
          ' Acao_mecanica = ' + dataHomeVisit.Acao_mecanica + ',' +
          ' Tratamento_focal = ' + dataHomeVisit.Tratamento_focal + ',' +
          ' Egresso_de_internacao = ' + dataHomeVisit.Egresso_de_internacao + ',' +
          ' Convite_atividades_coletivas_campanha_saude = ' + dataHomeVisit.Convite_atividades_coletivas_campanha_saude + ',' +
          ' Orientacao_Prevencao = ' + dataHomeVisit.Orientacao_Prevencao + ',' +
          ' Outros  = ' + dataHomeVisit.Outros +
          ' WHERE ' + condition;
        return $q.when(SQLiteService.getItems(query));
      },

      updateAnthropometryHomeVisit: function(dataHomeVisit, condition) {
        var query = 'UPDATE Visita_Domiciliar SET ';
        if (dataHomeVisit.pesoAcompanhamentoNutricional != null) {
          query += ' pesoAcompanhamentoNutricional = "' + dataHomeVisit.pesoAcompanhamentoNutricional + '",';
        } else {
          query += ' pesoAcompanhamentoNutricional = null,';
        }
        if (dataHomeVisit.alturaAcompanhamentoNutricional != null) {
          query += ' alturaAcompanhamentoNutricional = "' + dataHomeVisit.alturaAcompanhamentoNutricional + '"'
        } else {
          query += ' alturaAcompanhamentoNutricional = null'
        }
        query += ' WHERE ' + condition;
        return $q.when(SQLiteService.getItems(query));
      },

      updateOutcomeHomeVisit: function(dataHomevisit, condition) {
        var query = 'UPDATE Visita_Domiciliar SET ';
        if (dataHomevisit.desfecho != 1) {
          query += 'Cadastramento_Atualizacao = null,' +
            ' Visita_periodica = null,' +
            ' Consulta = null,' +
            ' Exame = null,' +
            ' Vacina = null,' +
            ' Condicionalidadesdobolsafamilia = null,' +
            ' Gestante = null,' +
            ' Puerpera = null,' +
            ' Recem_nascido = null,' +
            ' Crianca = null,' +
            ' PessoaDesnutricao = null,' +
            ' PessoaReabilitacaoDeficiencia = null,' +
            ' Hipertensao = null,' +
            ' Diabetes = null,' +
            ' Asma = null,' +
            ' DPOC_enfisema = null,' +
            ' Cancer = null,' +
            ' Outras_doencas_cronicas = null,' +
            ' Hanseniase = null,' +
            ' Tuberculose = null,' +
            ' Sintomaticos_Respiratorios = null,' +
            ' Tabagista = null,' +
            ' Domiciliados_Acamados = null,' +
            ' Condicoes_vulnerabilidade_social = null,' +
            ' Condicionalidades_bolsa_familia = null,' +
            ' Saude_mental = null,' +
            ' Usuario_alcool = null,' +
            ' Usuario_outras_drogas = null,' +
            ' Acao_educativa = null,' +
            ' Imovel_com_foco = null,' +
            ' Acao_mecanica = null,' +
            ' Tratamento_focal = null,' +
            ' Egresso_de_internacao = null,' +
            ' Convite_atividades_coletivas_campanha_saude = null,' +
            ' Orientacao_Prevencao = null,' +
            ' Outros = null,' +
            ' pesoAcompanhamentoNutricional = null,' +
            ' alturaAcompanhamentoNutricional = null,' +
            ' Justificativa = "' + dataHomevisit.Justificativa + '",';
        } else {
          query += 'Justificativa = null,';
        }
        query += 'desfecho = ' + dataHomevisit.desfecho + ',' +
          ' DataRegistro = ' + new Date().getTime() + ',' +
          ' status = "' + dataHomevisit.status + '"' +
          ' WHERE ' + condition;

        return $q.when(SQLiteService.getItems(query));
      },

      dropTable: function() {
        var query = 'DROP TABLE IF EXISTS Visita_Domiciliar';
        return $q.when(SQLiteService.executeSql(query));
      },

      createTable: function() {
        var query = 'CREATE TABLE IF NOT EXISTS Visita_Domiciliar (' +
          'id integer primary key autoincrement null, ' +
          /* headerTransport */
          'profissionalCNS text null, ' +
          'cboCodigo_2002 text null, ' +
          'cnes text null, ' +
          'ine text null, ' +
          'dataAtendimento text null, ' +
          'codigoIbgeMunicipio text null, ' +
          /* visita Domiciliar - Dados Cidadao */
          'turno integer null, ' +
          'numProntuario text null, ' +
          'cnsCidadao text null, ' +
          'dtNascimento text null, ' +
          'sexo text null, ' +
          'statusVisitaCompartilhadaOutroProfissional integer null, ' +
          'observacao text null, ' +
          /* visita Domiciliar - motivos Visita */
          'Cadastramento_Atualizacao integer null, ' +
          'Visita_periodica integer null, ' +
          'Consulta integer null, ' +
          'Exame integer null, ' +
          'Vacina integer null, ' +
          'Condicionalidadesdobolsafamilia integer null, ' +
          'Gestante integer null, ' +
          'Puerpera integer null, ' +
          'Recem_nascido integer null, ' +
          'Crianca integer null, ' +
          'PessoaDesnutricao integer null, ' +
          'PessoaReabilitacaoDeficiencia integer null, ' +
          'Hipertensao integer null, ' +
          'Diabetes integer null, ' +
          'Asma integer null, ' +
          'DPOC_enfisema integer null, ' +
          'Cancer integer null, ' +
          'Outras_doencas_cronicas integer null, ' +
          'Hanseniase integer null, ' +
          'Tuberculose integer null, ' +
          'Sintomaticos_Respiratorios integer null, ' +
          'Tabagista integer null, ' +
          'Domiciliados_Acamados integer null, ' +
          'Condicoes_vulnerabilidade_social integer null, ' +
          'Condicionalidades_bolsa_familia integer null, ' +
          'Saude_mental integer null, ' +
          'Usuario_alcool integer null, ' +
          'Usuario_outras_drogas integer null, ' +
          'Acao_educativa integer null, ' +
          'Imovel_com_foco integer null, ' +
          'Acao_mecanica integer null, ' +
          'Tratamento_focal integer null, ' +
          'Egresso_de_internacao integer null, ' +
          'Convite_atividades_coletivas_campanha_saude integer null, ' +
          'Orientacao_Prevencao integer null, ' +
          'Outros integer null, ' +
          /* visita Domiciliar - Mais Informacoes */
          'desfecho integer null, ' +
          'microarea text null, ' +
          'stForaArea integer null, ' +
          'tipoDeImovel integer null, ' +
          'pesoAcompanhamentoNutricional  text null, ' +
          'alturaAcompanhamentoNutricional text null,' +
          'status text null,' +
          'latitude text null,' +
          'longitude text null,' +
          'DataRegistro text null,' +
          'Justificativa text null )';
        return $q.when(SQLiteService.executeSql(query));
      },

      alterTable: function() {
        $q.when(SQLiteService.executeSql('ALTER TABLE Visita_Domiciliar add column Justificativa text null;'));
        console.log("Justificativa");
        $q.when(SQLiteService.executeSql('ALTER TABLE Visita_Domiciliar add column DataRegistro text null;'));
        console.log("DataRegistro");
      }
      //   var query = 'ALTER TABLE Visita_Domiciliar add column uuidFichaOriginadora text null';
      //   return $q.when(SQLiteService.executeSql(query));
      // }
    };
  }]);
