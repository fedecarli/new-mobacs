angular.module('mobacs').controller('ListPeopleController', function($scope, $ionicPlatform, $location, $rootScope, $state, $ionicPopup, PeopleService) {

  $scope.goToUrl = function($url) {
    $state.go($url);
  };

  //BEGIN
  $ionicPlatform.registerBackButtonAction(function(event) {
    event.preventDefault();
    var location = $location.path();
    if (location === '/pessoas/index') {
      $state.go('app.dashboard/index');
    } else if (location === '/app/dashboard/index') {
      $ionicPopup.confirm({
          title: '<strong>Sair!</strong>',
          template: 'Tem certeza que você deseja sair?'
        })
        .then(function(response) {
          if (response)
            ionic.Platform.exitApp();
        })
    }
  }, 100);
  //END

  $scope.directPeople = function() {
    $rootScope.newPeople = null;
    $state.go('pessoas/tabs/add-identification');
  };

  $scope.listGetAllPeople = function() {
    PeopleService.getAllListPeople().then(function(response) {
      $scope.listPeoples = angular.fromJson(response);
      var cont = 0;
      while (cont < $scope.listPeoples.length) {
        $scope.listPeoples[cont].dataAtendimento = parseInt($scope.listPeoples[cont].dataAtendimento);
        cont++;
      }
      // console.log($scope.listPeoples);
    }).catch(function(error) {
      console.log(error);
    })
  };

  $scope.listGetAllPeople();

  $scope.searchPeople = function(search) {

    if (search != undefined) {
      if (search.length > 2) {
        PeopleService.selectSearchPeoples(search)
          .then(function(response) {
            $scope.listPeoples = response;
          })
          .catch(function(err) {
            console.log(err);
          })
      } else {
        $ionicPopup.alert({
          title: '<strong>Informativo!</strong>',
          template: 'Insira três ou mais caracteres para a busca!'
        })
      }
    } else {
      $ionicPopup.alert({
        title: '<strong>Informativo!</strong>',
        template: 'Digite ao menos três caracteres!'
      })
    }
  };

  $scope.directToRoute = function(id, param) {

    if (param == 'view') {
      $state.go('view/people', { item: id });
    }

  };

});
