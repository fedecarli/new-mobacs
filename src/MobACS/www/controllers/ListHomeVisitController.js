angular.module('mobacs')
.controller('ListHomeVisitController',function($scope,HomeVisitService){

  //LIST ALL VISIT
  $scope.listGetAllHomeVisit = function (){
    HomeVisitService.selectListAllHomeVisit()
      .then(function ( data ) {
        $scope.listHomeVisit = angular.fromJson(data);
      }).catch(function (err) {
      console.log(err)
    });
  };

  $scope.listGetAllHomeVisit();

});
