angular.module('mobacs').controller('FamilyController', function($scope, $ionicPlatform, $ionicPopup,$location,$rootScope, $state, $stateParams, FamilyService, HomeRegistrationService){

  $scope.requiredInput = {
    numeroCnsResponsavel: true
  };

  $scope.disabledInput = {
    numeroCnsResponsavel: true,
    stMudanca : false
  };

  $scope.removeButton = typeof angular.fromJson($stateParams.id) != 'undefined' ? true : false;

  $scope.removeFamily = function(cns){

    var condition = 'cadastroDomiciliarId = (select cadastroDomiciliarId from FamiliaRow where id = '+angular.fromJson($stateParams.id)+')';
    FamilyService.getFamilies(condition)
      .then(function(response){
        if(response.length == 1){
          $ionicPopup.confirm({
            title: '<strong>Informativo</strong>',
            // subtitle: '',
            template: 'Ao "confirmar" está alteração a ficha será removida!',
            buttons: [
              { text: 'Não',
                type: 'button-stable'
              },
              {
                text: '<b>Confirmar</b>',
                type: 'button-positive',
                onTap: function() {
                  var deleteFamily = 'id = '+angular.fromJson($stateParams.id)+'';
                  var deleteHomeRegistration = 'id = '+response[0].cadastroDomiciliarId+'';
                  FamilyService.deleteFamilies(deleteFamily)
                    .then(function(response){
                      return HomeRegistrationService.deleteHomeAdress(deleteHomeRegistration);
                    })
                    .then(function(response){
                      $ionicPopup.alert({
                        title: 'Informativo!',
                        template: 'Você será redirecionado para a página inicial'
                      }).then(function(){
                        $state.go('app.dashboard/index');
                      })
                    })
                    .catch(function(err){
                        $ionicPopup.alert({
                          title: '<strong>Ocorreu um erro</strong>',
                          template: 'Erro: '+err+''
                        })
                    })
                }
              }
            ]
          })
        }else{
          var conditionDelete = 'numeroCnsResponsavel = '+cns+' and id = '+angular.fromJson($stateParams.id)+'';
          FamilyService.deleteFamilies(conditionDelete)
            .then(function(response){
              $state.go('domicilios/tabs/add/add-families',{id: $rootScope.newHomeResgistration.id}, {reload: true});
            })
            .catch(function(err){
                $ionicPopup.alert({
                  title: '<strong>Ocorreu um erro</strong>',
                  template: 'Erro: '+err+''
                })
            })
        }
      })
      .catch(function(err){
        $ionicPopup.alert({
          title: '<strong>Ocorreu um erro</strong>',
          template: 'Erro: '+err+''
        })
      })

  };

  //BEGIN
  $ionicPlatform.registerBackButtonAction(function (event) {
    event.preventDefault();
    var location = $location.path();
    if(location === '/domicilios/tabs/add/add-all-families'){
      $state.go('domicilios/tabs/add/add-families', {id:  $rootScope.newHomeResgistration.id});
    }else if(location.search('/domicilios/tabs/add/edit-all-families/') != -1){
      $state.go('domicilios/tabs/add/add-families', {id:  $rootScope.newHomeResgistration.id});
    }
  }, 100);

  $scope.validNoDataFamily = function(param){
    //é inválido
    if(!param)
      return true;
    else{

      param = param.toString();
      param = param.trim();

      while (param.search(/\s\s/) >= 0)
        param = param.replace(/\s\s/g, ' ');

      if(param.length == 0) return true;
    }

    return false;
  };

  $scope.valueInDataFamily = function(param){

    var elemento = null;
    var RegExp = null

    if(param == 'dataNascimentoResponsavel'){
      elemento = document.getElementById('dataNascimentoResponsavel');
      RegExp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
      if(!RegExp.test(elemento.value)){
        document.getElementById("dataNascimentoResponsavelError").style.display = "block"
      }else{
        document.getElementById("dataNascimentoResponsavelError").style.display = "none"
      }
    }else if(param == 'resideDesde'){
      RegExp = new RegExp('^(0[1-9]|1[012])/[12][0-9]{3}$');
      if(!RegExp.test(elemento.value)){
        document.getElementById("resideDesdeError").style.display = "block"
      }else{
        document.getElementById("resideDesdeError").style.display = "none"
      }
    }
  };

  //END

  $scope.familia = {};

  $scope.dataValidation = function(){

    $rootScope.newFamilyRegistration = $scope.dataEditFamily;

    $scope.familia = {
      numeroProntuario : $scope.dataEditFamily.numeroProntuario,
      numeroCnsResponsavel : $scope.dataEditFamily.numeroCnsResponsavel,
      dataNascimentoResponsavel: $scope.dataEditFamily.dataNascimentoResponsavel != null ? parseInt($scope.dataEditFamily.dataNascimentoResponsavel) : null,
      rendaFamiliar: $scope.dataEditFamily.rendaFamiliar == null ? null : $scope.dataEditFamily.rendaFamiliar.toString(),
      numeroMembrosFamilia: $scope.dataEditFamily.numeroMembrosFamilia,
      resideDesde: $scope.dataEditFamily.resideDesde,
      stMudanca: $scope.dataEditFamily.stMudanca
    }
  };

  $scope.searchData = function(id){

    if($rootScope.newHomeResgistration.tipoDeImovel != 1){
      $scope.disabledInput.stMudanca = true;
      $scope.familia.stMudanca = 1;
    }

    if(id != undefined){
      var condition = 'id = '+id;
      FamilyService.getFamilies(condition)
        .then(function(response){
          if(response[0] != undefined){
            $scope.dataEditFamily = response[0];
            $scope.dataValidation();
          }
        })
        .catch(function(err){
            $ionicPopup.alert({
              title: '<strong>Ocorreu um erro</strong>',
              template: 'Erro: '+err+''
            })
        })
      }
  };

  $scope.searchData(angular.fromJson($stateParams.id));

  $scope.directToEditFamily = function(id){
    $state.go('domicilios/tabs/add/edit-all-families', {id:id});
  };

  $scope.addFamilia = function(dataHome){

    var invalid = false;
    var message;

    if($scope.validNoDataFamily(dataHome.numeroProntuario)){
      dataHome.numeroProntuario = null;
    }

    if($scope.validNoDataFamily(dataHome.numeroCnsResponsavel)){
      dataHome.numeroCnsResponsavel = null;
      invalid = true;
      message = 'É necessário inserir o CNS do Responsável Familiar'
    }

    if($scope.validNoDataFamily(dataHome.dataNascimentoResponsavel)){
      dataHome.dataNascimentoResponsavel = null;
    }

    if($scope.validNoDataFamily(dataHome.numeroMembrosFamilia)){
      dataHome.numeroMembrosFamilia = 1;
    }else{
      if(dataHome.numeroMembrosFamilia == 0){
        invalid = true;
        message = 'Insira um valor maior que 0'
      }
    }

    if($scope.validNoDataFamily(dataHome.resideDesde)){
      dataHome.resideDesde = null;
    }


    if(invalid){

      $ionicPopup.alert({
        title: 'Informativo',
        template: message
      })

    }else{

      var idAdress = $rootScope.newHomeResgistration.id;

      if(angular.fromJson($stateParams.id) == undefined){

        FamilyService.insertFamily(dataHome,idAdress)
          .then(function(response){
            return HomeRegistrationService.updateGenericHomeAdress('nuMoradores = '+dataHome.numeroMembrosFamilia+'' ,'cadastroDomiciliarId = '+idAdress);
          })
          .then(function(response){
            $state.go('domicilios/tabs/add/add-families',{id: idAdress}, {reload: true});
          })
          .catch(function(error){
            $ionicPopup.alert({
              title: '<strong>Ocorreu um erro</strong>',
              template: 'Erro: '+err+''
            })
          })

      }else{

        var condition = ' id = '+angular.fromJson($stateParams.id);
        FamilyService.updateFamily(dataHome, condition)
          .then(function(response){
            return HomeRegistrationService.updateGenericHomeAdress('nuMoradores = '+ dataHome.numeroMembrosFamilia+'' ,' id = '+idAdress);
          })
          .then(function(response){
            $state.go('domicilios/tabs/add/add-families',{id: idAdress}, {reload: true});
          })
          .catch(function(err){
            $ionicPop({
              title: '<strong>Ocorreu um erro</strong>',
              template: 'Erro: '+err+''
            })
          })
      }
    }
  };
});
