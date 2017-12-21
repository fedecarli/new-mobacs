angular.module('mobacs')
    .factory('WebApiService', function($http, $log, $q, $rootScope) {

        var development = 'http://192.168.10.6:1003';
        var developmentLog = 'http://192.168.10.6:1000/api/LogMobile';
        var production = 'https://intranet.santanadeparnaiba.sp.gov.br/APIeSusMobile';
        var productionLog = 'https://intranet.santanadeparnaiba.sp.gov.br/APIeSusLogin/api/LogMobile';

        // var environment = {
        //   api: ($rootScope.environmentApp.environment == "dev") ? development : production,
        //   log: ($rootScope.environmentApp.environment == "dev") ? developmentLog : productionLog,
        //   version: $rootScope.environmentApp.version
        // };

        var environment = {
            api: 'http://192.168.2.131/sigsm/v2/ESUS/fichas',//($rootScope.environmentApp.environment == "dev") ? development : production,
            log: 'http://192.168.2.131/sigsm/v2/ESUS/fichas/api/LogMobile',//($rootScope.environmentApp.environment == "dev") ? developmentLog : productionLog,
            version: $rootScope.version
        };

        return {
            getToken: function(headerTransport) {
                var deferred = $q.defer();
                console.log(headerTransport);
                $http.post(environment.api + '/api/processos/enviar/cabecalho', JSON.stringify(headerTransport))
                    .success(function(data) {
                        console.log(data);
                        deferred.resolve({ token: data });
                    }).error(function(msg) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            },

            closeToken: function(token) {
                var deferred = $q.defer();
                $http.post(environment.api + '/api/processos/encerrar/' + token)
                    .success(function(data) {
                        deferred.resolve({ encerrado: data });
                    }).error(function(msg) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            },

            getUsers: function(token) {
                var deferred = $q.defer();
                console.log(token);
                $http.get(environment.api + '/api/dados/paciente/' + token)
                    .success(function(data) {
                        console.log(data); //////////////////////////////////////////////NOVO
                        deferred.resolve(data);
                    }).error(function(msg) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            },

            submitUser: function(user) {
                var deferred = $q.defer();
                $http.post(environment.api + '/api/processos/enviar/cadastro/individual', JSON.stringify(user))
                    .success(function(data) {
                        deferred.resolve({ status: data });
                    }).error(function(msg) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            },

            getAdress: function(token) {
                var deferred = $q.defer();
                $http.get(environment.api + '/api/dados/domicilio/' + token)
                    .success(function(data) {
                        deferred.resolve(data);
                    }).error(function(msg) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            },

            submitAdress: function(adress) {
                var deferred = $q.defer();
                $http.post(environment.api + '/api/processos/enviar/cadastro/domiciliar/', JSON.stringify(adress))
                    .success(function(data) {
                        deferred.resolve({ status: data });
                    }).error(function(msg) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            },

            submitHomeVisit: function(homevisit) {
                var deferred = $q.defer();
                $http.post(environment.api + '/api/processos/enviar/visita/child', JSON.stringify(homevisit))
                    .success(function(data) {
                        deferred.resolve({ status: data });
                    }).error(function(msg) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            },

            dataLog: function(data) {
                var deferred = $q.defer();
                $http.post(environment.log, "'" + JSON.stringify(data) + "'")
                    .success(function(data) {
                        deferred.resolve({ status: data });
                    }).error(function(msg) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            },

            submitAtomic: function(atomic) {
                console.log(atomic);
                // console.log(JSON.stringify(atomic));
                var deferred = $q.defer();
                $http.post(environment.api + '/api/processos/enviar/cadastro/atomico', JSON.stringify(atomic))
                    .success(function(data) {
                        deferred.resolve({ status: data });
                    }).error(function(msg) {
                        deferred.reject(msg);
                    });
                return deferred.promise;
            }
        };

    });