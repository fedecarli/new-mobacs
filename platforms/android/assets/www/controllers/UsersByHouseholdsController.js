angular.module('mobacs')
.controller('UsersByHouseholdsController',function($scope, $stateParams, PeopleService, $rootScope, $ionicPlatform, $location, $state, $ionicPopup, HomeVisitService, FamilyService){

  $scope.goToViewHomeAdress = function(){
    var location = $location.path();
    if(location.search('/usersbyhouseholds/') != -1){
      $state.go('view/adress', {id: $rootScope.newHomeResgistration.id});
    }else if(location === '/app/dashboard/index'){
      $ionicPopup.confirm({
        title: '<strong>Sair!</strong>',
        template: 'Tem certeza que você deseja sair?'
      })
        .then(function(response){
          if(response)
            ionic.Platform.exitApp();
        })
    }
  };

  //BEGIN
  $ionicPlatform.registerBackButtonAction(function (event) {
    event.preventDefault();
    var location = $location.path();
    if(location.search('/usersbyhouseholds/') != -1){
      $state.go('view/adress', {id: $rootScope.newHomeResgistration.id});
    }else if(location === '/app/dashboard/index'){
      $ionicPopup.confirm({
        title: '<strong>Sair!</strong>',
        template: 'Tem certeza que você deseja sair?'
      })
        .then(function(response){
          if(response)
            ionic.Platform.exitApp();
        })
    }
  }, 100);
  //END

  $scope.listUsers = function(userCns){
    PeopleService.getPeople('cnsCidadao = "'+userCns+'" or cnsResponsavelFamiliar = "'+userCns+'" order by statusEhResponsavel desc')
      .then(function(response){
        $scope.listUsersByHouseholds = response;
      })
      .catch(function(err){
        console.log(err);
      })
  };

  $scope.listUsers($stateParams.cns);

  $scope.createHomeVisitForUser = function(dataHomeVisit){

    if(dataHomeVisit.cnsCidadao != null){

      var cnsResponsavel = null;

      if(dataHomeVisit.statusEhResponsavel == 1){
        cnsResponsavel = dataHomeVisit.cnsCidadao;
        $scope.insertHomeVisitForUser(dataHomeVisit, cnsResponsavel);
      }else{
        PeopleService.getPeople('cnsCidadao = "'+dataHomeVisit.cnsCidadao+'"')
          .then(function(response){
            cnsResponsavel = response[0].cnsResponsavelFamiliar;
            $scope.insertHomeVisitForUser(dataHomeVisit, cnsResponsavel);
          })
          .catch(function(err){
            console.log(err);
          })
      }

    }else{
      $ionicPopup.alert({
        title: '<strong>Informativo</strong>',
        template: 'O usuário <strong>'+dataHomeVisit.nomeCidadao+'</strong> não possui CNS cadastrado, por favor insira o CNS e volte a criar a ficha de Visita'
      })
    }
  };

  $scope.insertHomeVisitForUser = function(dataHomeVisit,cnsResponsavel){

    var newDataToHomeVisit = {};

    FamilyService.getFamilies(' numeroCnsResponsavel = "'+cnsResponsavel+'"')
      .then(function(response){
        newDataToHomeVisit = response[0];
        return HomeVisitService.getHomeVisit('cnsCidadao = "'+dataHomeVisit.cnsCidadao+'"');
      })
      .then(function(response){
        if(response.length == 0){
          var dataToHomeVisit = {
            profissionalCNS: $rootScope.userLogged.profissionalCNS,
            cboCodigo_2002 : $rootScope.userLogged.cboCodigo_2002,
            cnes : $rootScope.userLogged.cnes,
            ine : $rootScope.userLogged.ine,
            dataAtendimento :  new Date(),
            codigoIbgeMunicipio : $rootScope.userLogged.codigoIbgeMunicipio,
            turno : 1,
            numProntuario : newDataToHomeVisit.numeroProntuario,
            cnsCidadao : dataHomeVisit.cnsCidadao,
            dtNascimento : dataHomeVisit.dataNascimentoCidadao,
            sexo : dataHomeVisit.sexoCidadao,
            statusVisitaCompartilhadaOutroProfissional : null,
            observacao : null,
            Cadastramento_Atualizacao : null,
            Visita_periodica : null,
            Consulta : null,
            Exame : null,
            Vacina : null,
            Condicionalidadesdobolsafamilia : null,
            Gestante : dataHomeVisit.statusEhGestante == 1 ? 5 : null,
            Puerpera : null,
            Recem_nascido : null,
            Crianca : null,
            PessoaDesnutricao : null,
            PessoaReabilitacaoDeficiencia : dataHomeVisit.statusTemAlgumaDeficiencia == 1 ? 10 : null,
            Hipertensao : dataHomeVisit.statusTemHipertensaoArterial == 1 ? 11 : null,
            Diabetes : dataHomeVisit.statusTemDiabetes == 1 ? 12 : null,
            Asma : dataHomeVisit.Asma == 30 ? 13 : null,
            DPOC_enfisema : dataHomeVisit.DPOC_Enfisema == 31 ? 14 : null,
            Cancer : dataHomeVisit.statusTemTeveCancer == 1 ? 15 : null,
            Outras_doencas_cronicas : null,
            Hanseniase : dataHomeVisit.statusTemHanseniase == 1 ? 17 : null,
            Tuberculose : dataHomeVisit.statusTemTuberculose == 1 ? 18 : null,
            Sintomaticos_Respiratorios : null,
            Tabagista : dataHomeVisit.statusEhFumante == 1 ? 33 : null,
            Domiciliados_Acamados : dataHomeVisit.statusEstaDomiciliado == 1 ? 19 : null,
            Condicoes_vulnerabilidade_social : null,
            Condicionalidades_bolsa_familia : null,
            Saude_mental : null,
            Usuario_alcool : dataHomeVisit.statusEhDependenteAlcool == 1 ? 23 : null,
            Usuario_outras_drogas : dataHomeVisit.statusEhDependenteOutrasDrogas == 1 ? 24 : null,
            Acao_educativa : null,
            Imovel_com_foco : null,
            Acao_mecanica : null,
            Tratamento_focal : null,
            Egresso_de_internacao : null,
            Convite_atividades_coletivas_campanha_saude : null,
            Orientacao_Prevencao : null,
            Outros : null,
            desfecho : null,
            microarea : dataHomeVisit.microarea,
            stForaArea : dataHomeVisit.stForaArea,
            tipoDeImovel : $rootScope.newHomeResgistration.tipoDeImovel,
            pesoAcompanhamentoNutricional : null,
            alturaAcompanhamentoNutricional : null,
            status : 'Em Edição'
          };

          HomeVisitService.addInformationsHomeVisit(dataToHomeVisit)
            .then(function(response){
              $rootScope.flagCNSForReturnHomeVisit = $stateParams.cns;
              $state.go('home-visit/edit/informations',{item: response.insertId});
              // $ionicPopup.alert({
              //   title: '<strong>Informativo</strong>',
              //   template: 'Foi criada uma Visita Domiciliar para o/a cidadão <strong>'+dataHomeVisit.nomeCidadao+'</strong>!'
              // })
            })
            .catch(function(err){
              console.log(err);
            })
        }else{
          $ionicPopup.alert({
            title: '<strong>Informativo</strong>',
            template: 'O/A cidadão <strong>'+dataHomeVisit.nomeCidadao+'</strong> já possui uma Visita Domiciliar atrelada.<br>Caso necessário transmita a visita e volte a criar a ficha novamente!'
          })
        }
      })
      .catch(function(err){
        console.log(err);
      })

  };

});
