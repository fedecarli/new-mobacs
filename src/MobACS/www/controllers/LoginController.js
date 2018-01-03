angular.module('mobacs')
    .controller('LoginController', function ($scope, $rootScope, $state, UsersService, $http, $cordovaNetwork, $ionicPlatform, WebApiService, $ionicLoading, SecurityService) {
        var condition = null;

        var apiLoginDev = "http://192.168.2.183/sigsm/v2/ESUS/fichas/api/Login/ConsultarLogin/";
        var apiLoginProd = "http://192.168.2.183/sigsm/v2/ESUS/fichas/api/Login/ConsultarLogin/";

        $scope.login = {
            userLogin: '',
            password: '',
            // userLogin: '',
            // password: '',
            // version: '1.8.0',
            // description: "Produção",
            environment: $rootScope.environmentApp.environment,
            version: $rootScope.environmentApp.version,
            description: ($rootScope.environmentApp.environment == "dev") ? "Desenvolvimento" : "Produção"
                // environment: $rootScope.environmentApp.environment
        };

        var environment = {
            api: ($rootScope.environmentApp.environment == "dev") ? apiLoginDev : apiLoginProd
                // ,
                // log: ($rootScope.environmentApp.environment == "dev") ? developmentLog : productionLog,
                // version: $rootScope.version
        };

        $ionicPlatform.ready(function() {
            $scope.typeNetwork = $cordovaNetwork.getNetwork();
            $scope.isNetworknOnline = $cordovaNetwork.isOnline();
            $scope.isNetworkOffline = $cordovaNetwork.isOffline();
        });

        //UsersService.dropTable();
        UsersService.createTable();
        //Comentar para não ocorrer problemas
        //UsersService.addUserTeste();

        $scope.loginApplication = function() {
            $ionicLoading.show();
            
            if(typeof $scope.typeNetwork === 'undefined' && typeof $scope.isNetworknOnline === 'undefined') {
                $scope.typeNetwork = $cordovaNetwork.getNetwork();
                $scope.isNetworknOnline = $cordovaNetwork.isOnline();
                $scope.isNetworkOffline = $cordovaNetwork.isOffline();
            }

            if ($scope.typeNetwork == 'wifi' && $scope.isNetworknOnline == true) {
                var userReq = { login: $scope.login.userLogin, senha: SecurityService.encrypt($scope.login.password).toString() };
                $http.post(environment.api, JSON.stringify(userReq))
                    .success(function(data) {
                        var user = data[0];
                        if (user.Sucesso) {
                            $ionicLoading.hide();
                            dataUser = {
                                'nome': user.NomeUsuario,
                                'email': user.Email,
                                'userLogin': user.Login,
                                'profissionalCNS': user.CNS,
                                'cboCodigo_2002': user.CBO,
                                'cnes': user.CNES,
                                'ine': user.INE,
                                'codigoIbgeMunicipio': user.IBGE,
                                'senha': CryptoJS.SHA256($scope.login.password).toString()
                            }

                            condition = ' userLogin = "' + $scope.login.userLogin + '" AND senha = "' + CryptoJS.SHA256($scope.login.password).toString() + '"';

                            UsersService.getUser(condition).then(function(user) {
                                if (typeof user[0] !== "undefined") {
                                    if (dataUser.Login == user[0].Login) {
                                        $rootScope.userLogged = dataUser;
                                        $state.go('app.dashboard/index');
                                    } else {
                                        $scope.getDataLogin();
                                    }
                                } else {
                                    $scope.getDataLogin();
                                }
                            }).catch(function(err) {
                                alert("Problema na autenticação");
                            });

                        } else {
                            $ionicLoading.hide();
                            alert("Usuário ou senha inválido");
                        }
                    })
                    .error(function(data) {
                        $ionicLoading.hide();
                        alert("Verifique sua conexão com a internet! Contate o administrador caso sua internet não esteja apresentando problemas fora do aplicativo.");
                    });
            } else {
                condition = ' userLogin = "' + $scope.login.userLogin + '" AND senha = "' + CryptoJS.SHA256($scope.login.password).toString() + '"';

                UsersService.getUser(condition).then(function(user) {
                    if (typeof user[0] !== "undefined") {
                        $ionicLoading.hide();
                        $rootScope.userLogged = user[0];
                        $state.go('app.dashboard/index');
                    } else {
                        $ionicLoading.hide();
                        $scope.getDataLogin();
                    }
                }).catch(function(err) {
                    $ionicLoading.hide();
                    alert("Problema na autenticação");
                });
            }
        }

        $scope.getDataLogin = function() {

            UsersService.getAllUsers().then(function(response) {
                if (response.length > 0) {
                    var usersLocal = response;
                    for (var i = 0; i < usersLocal.length; i++) {
                        if (usersLocal[i].statusSincronizacao == 2) {
                            VisitaDomiciliarService.dropTable();
                            UsersService.dropTable();
                            $scope.saveUser();
                        } else {
                            alert("O usuário " + usersLocal[i].userLogin + " ainda não fez a sicronização dos dados");
                        }
                    }

                } else {
                    $scope.saveUser();
                }
            }).catch(function(err) {
                alert("Problema na autenticação");
            });

        }

        $scope.saveUser = function() {
            var userReq = { login: $scope.login.userLogin, senha: SecurityService.encrypt($scope.login.password).toString() };
            $http.post(environment.api, JSON.stringify(userReq))
                .success(function(data) {
                    var user = data[0];
                    if (user.Sucesso) {
                        dataUser = {
                            'nome': user.NomeUsuario,
                            'email': user.Email,
                            'userLogin': user.Login,
                            'profissionalCNS': user.CNS,
                            'cboCodigo_2002': user.CBO,
                            'cnes': user.CNES,
                            'ine': user.INE,
                            'codigoIbgeMunicipio': user.IBGE,
                            'senha': CryptoJS.SHA256($scope.login.password).toString(),
                            'statusSincronizacao': 1
                        }

                        //save user
                        UsersService.addUser(dataUser).then(function() {
                            $rootScope.userLogged = dataUser;
                            $state.go('app.dashboard/index');
                        }).catch(function(err) {
                            alert("Problema ao salvar o usuário!");
                        });

                    } else {
                        alert("Usuário ou senha inválido");
                    }
                })
                .error(function(data) {
                    alert("Verifique sua conexão com a internet!");
                });
        }

        /*
        PARA REALIZAR TESTE
        */

        // $scope.loginApplication();
    })