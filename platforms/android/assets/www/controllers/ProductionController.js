angular.module('mobacs').controller('ProductionController',function($scope,$ionicHistory,$ionicPopup ,$location, $ionicPlatform, ProductionService, $state){

  $scope.goHome = function() {
    $ionicHistory.goBack();
  };

  $ionicPlatform.ready(function() {

    ProductionService.selectStatusProduction()
      .then(function(response){
        $scope.status = {
          people : response[0].dado,
          homeRegistration: response[1].dado,
          homeVisit : response[2].dado
        };
      })
      .catch(function(err){
        console.log(err);
      });

  });

  //BEGIN
  $ionicPlatform.registerBackButtonAction(function (event) {
    event.preventDefault();
    var location = $location.path();
    if(location === '/production'){
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

  $scope.goProduction = function(){
    $state.go('production',{reload: true});
  };

})
