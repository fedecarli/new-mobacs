angular.module('mobacs').controller('ListAdressController',function($scope, FamilyService, $ionicPlatform, $location,$rootScope ,$state ,$ionicPopup , HomeRegistrationService){

  $scope.goToUrl = function ($url) {
    $state.go($url);
  };

  //BEGIN

  $ionicPlatform.registerBackButtonAction(function (event) {
    event.preventDefault();
    var location = $location.path();
    if(location === '/domicilios/index'){
      $state.go('app.dashboard/index');
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

  // $scope.directHome = function(){
  //   $rootScope.newHomeResgistration = null;
  //   $state.go('domicilios/tabs/add/add-adress', {reload:true});
  // };

  $scope.directHome = function(){
    $ionicPopup.alert({
      title: '<strong>Informativo</strong>',
      template: 'Está função está descontinuada, para criar um domicílio, vá até a cadastro individual, insira um CNS válido e defina o cidadão como responsável familiar, automaticamente será criado um domicílio para este cidadão.'
    })
  };


  $scope.listGetAllHomeRegistration = function(){
    HomeRegistrationService.getAllHomeRegistration()
      .then(function(response){
        $scope.listAllHomeRegistration = angular.fromJson(response);
        return FamilyService.getAllFamiliesWithDetails();
      })
      .then(function(response){
        $scope.AllFamilies = response;
        var contadorA = 0;
        var contadorB = 0;
        while(contadorB < $scope.listAllHomeRegistration.length){
          contadorA = 0;
          while(contadorA < $scope.AllFamilies.length){
            if($scope.listAllHomeRegistration[contadorB].id == $scope.AllFamilies[contadorA].cadastroDomiciliarId){
              $scope.listAllHomeRegistration[contadorB].familiaRow = $scope.AllFamilies[contadorA].nomeCidadao;
            }
            contadorA++;
          }
            $scope.listAllHomeRegistration[contadorB].dataAtendimento = parseInt($scope.listAllHomeRegistration[contadorB].dataAtendimento);
          contadorB++;
        }
      })
      .catch(function(error){
        alert('Ocorreu um erro na busca para a listagem de todos os domicílios!');
      })
  };

  $scope.listGetAllHomeRegistration();

  $scope.searchHome = function(search){

    if(search != undefined){
      if(search.length > 2){
        HomeRegistrationService.selectSearchHome(search)
          .then(function(response){
            $scope.listAllHomeRegistration = response;
          })
          .catch(function(err){
            console.log(err);
          })
      }else{
        $ionicPopup.alert({
          title: '<strong>Informativo!</strong>',
          template: 'Insira três ou mais caracteres para a busca!'
        })
      }
    }else{
      $ionicPopup.alert({
        title: '<strong>Informativo!</strong>',
        template: 'Digite ao menos três caracteres!'
      })
    }
  };

});
