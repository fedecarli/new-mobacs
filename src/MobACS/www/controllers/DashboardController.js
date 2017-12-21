angular.module( 'mobacs')
.controller( 'DashboardController', function( $scope, $rootScope , $ionicPlatform ,$location, $ionicHistory, $ionicPopup, $state){

  $rootScope.flagCNSForReturnHomeVisit = null;

  $scope.exitApp = function(){
    ionic.Platform.exitApp();
  };

  $ionicPlatform.registerBackButtonAction(function (event) {
    event.preventDefault();
    if ($location.path() === "/app/dashboard/index") {
      $ionicPopup.confirm({
        title: '<strong>Sair!</strong>',
        template: 'Tem certeza que você deseja sair?'
      })
        .then(function(response){
          if(response)
            $scope.exitApp();
        })
    }
  }, 100);

  $scope.aboutInformation = function(){
    $ionicPopup.alert({
      title: '<strong>Sobre</strong>',
      template: 'O Aplicativo MobileACS foi desenvolvido pela Prefeitura Municipal de Santana de Parnaíba para uso exclusivo dos Agentes Comunitários de Saúde (ACS) através de smartphone e tablet.<br><br>'
        + 'Este aplicativo foi criado com a finalidade de automatizar os processos realizados pelos ACS de uma forma mais simples e totalmente digital, dispensando utilização de impressos e proporcionando maior flexibilidade e sustentabilidade. Logo, oferecer um melhor serviço aos munícipes.<br><br>'
        + 'Nesta primeira versão foram automatizados os processos referentes às fichas "Cadastro Individual", "Cadastro Domiciliar" e de "Visitas Domiciliares".<br><br>'
        + 'Secretaria Municipal de Tecnologia da Informação - SMTI<br>'
        + 'Agosto/2017<br><br>'
        + 'Versão '+ $rootScope.environmentApp.version
    })
  };

  $scope.aboutHomeAdress = function(){
    $ionicPopup.alert({
      title: '<strong>Informativo</strong>',
      template: 'Está função está descontinuada, para criar um domicílio, vá até a cadastro individual e insira um CNS válido e defina o cidadão como responsável familiar, automaticamente será criado um domicílio para este cidadão.'
    })
  };

});
