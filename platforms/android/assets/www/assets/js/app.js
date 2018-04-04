// MobACS App
angular.module('mobacs', ['ionic', 'ngCordova', 'ionic-datepicker'])

.run(function($ionicPlatform, SQLiteService, GlobalService, $rootScope, $ionicPopup, UsersService, PeopleService, HomeVisitService, HomeRegistrationService, FamilyService, $cordovaGeolocation) {
    $ionicPlatform.ready(function() {
        if (window.StatusBar) {
            StatusBar.styleDefault();
        }

        if (window.cordova && window.cordova.plugins.Keyboard) {
            cordova.plugins.Keyboard.hideKeyboardAccessoryBar(true);
            cordova.plugins.Keyboard.disableScroll(true);

        }
        /*
                    function onSuccess(position) {
                        alert('Latitude: ' + position.coords.latitude + '\n' +
                            'Longitude: ' + position.coords.longitude + '\n');
                    };

                    function onError(error) {
                        alert('code: ' + error.code + '\n' +
                            'message: ' + error.message + '\n');
                    }

                    navigator.geolocation.getCurrentPosition(onSuccess, onError);
        */
        if (window.StatusBar) {
            StatusBar.styleDefault();
        }
    });

    // cordova.plugins.diagnostic.isLocationEnabled(function(enabled){
    //     if(!enabled){
    //       $ionicPopup.alert({
    //         title: 'GPS Não habilitado',
    //         template: 'Por favor, habilite o GPS para usar a aplicativo!'
    //       }).then(function () {
    //         ionic.Platform.exitApp();
    //       });
    //     }
    //   },
    //   function(error){
    //     console.error("The following error occurred:"+error);
    //   });

    GlobalService.createTableCepSantanaParnaiba();
    GlobalService.createTableTipoDeLogradouro();
    GlobalService.createTableOcupacaoCodigoCbo2002();
    GlobalService.createTableMunicipios();
    GlobalService.createTablePaises();
    GlobalService.createTableEtnia();
    GlobalService.createTableAllMunicipios();

    PeopleService.createTable();
    PeopleService.alterTable();
    HomeVisitService.createTable();
    HomeVisitService.alterTable();
    HomeRegistrationService.createTable();
    HomeRegistrationService.alterTable();
    FamilyService.createTableFamilies();

    //BEGIN

    GlobalService.dropTableMunicipios();

    GlobalService.selectAllMunicipio()
        .then(function(response) {
            if (response.length == 0) {
                return GlobalService.insertAllMunicipios1();
            } else {
                return GlobalService.selectAllMunicipio();
            }
        })
        .then(function(response) {
            if (response.insertId > 1) {
                return GlobalService.insertAllMunicipios2();
            } else {
                return response;
            }
        })
        .then(function(response) {
            if (response.insertId > 1) {
                return GlobalService.selectAllMunicipio();
            } else {
                return response;
            }
        })
        .then(function(response) {
            $rootScope.todosCounties = response;
        })
        .catch(function(err) {
            console.log(err);
        });

    //END

    GlobalService.selectNumberAllCepSantanaParnaiba()
        .then(function(response) {
            var rowsCepSantanaParnaiba = response[0];
            if (rowsCepSantanaParnaiba.AllCepSantanaParnaiba == 0) {
                return GlobalService.insertCepSantanaParnaiba();
            } else {
                return GlobalService.selectAllCepSantanaParnaiba();
            }
        })
        .then(function(response) {
            if (!response.length) {
                return GlobalService.selectAllCepSantanaParnaiba();
            } else {
                return response;
            }
        })
        .then(function(response) {
            $rootScope.thisAllSantanaParnaiba = response;
        })
        .catch(function(error) {
            console.log(error);
        });

    GlobalService.selectNumberAllTipoDeLogradouro()
        .then(function(response) {
            var rowsTipoDeLogradouro = response[0];
            if (rowsTipoDeLogradouro.AllTipoDeLogradouro == 0) {
                return GlobalService.insertTipoDeLogradouro();
            } else {
                return GlobalService.selectAllTipoDeLogradouro();
            }
        })
        .then(function(response) {
            if (!response.length) {
                return GlobalService.selectAllTipoDeLogradouro();
            } else {
                return response;
            }
        })
        .then(function(response) {
            $rootScope.todosTiposLogradouros = response;
        })
        .catch(function(err) {
            console.log(err);
        });

    GlobalService.selectNumberAllOcupacaoCodigoCbo2002()
        .then(function(response) {
            var rowsOcupacaoCodigoCbo2002 = response[0];
            if (rowsOcupacaoCodigoCbo2002.AllOcupacaoCodigoCbo2002 == 0) {
                return GlobalService.insertOcupacaoCodigoCbo2002();
            } else {
                return GlobalService.selectAllOcupacaoCodigoCbo2002();
            }
        })
        .then(function(response) {
            if (!response.length) {
                return GlobalService.selectAllOcupacaoCodigoCbo2002();
            } else {
                return response;
            }
        })
        .then(function(response) {
            $rootScope.todasOcupations = response;
        })
        .catch(function(err) {
            console.log(err);
        });

    // GlobalService.selectNumberAllMunicipios()
    //   .then(function(response){
    //     var rowsMunicipios = response[0];
    //     if(rowsMunicipios.AllMunicipios == 0){
    //       return GlobalService.insertMunicipios1();
    //     }else{
    //       return GlobalService.selectAllMunicipios();
    //     }
    //   })
    //   .then(function(response){
    //     if(response.insertId == 2785) {
    //       return GlobalService.insertMunicipios2();
    //     }else{
    //       return response;
    //     }
    //   })
    //   .then(function(response){
    //     if(response.insertId == 5570) {
    //       return GlobalService.selectAllMunicipios();
    //     }else{
    //       return response;
    //     }
    //   })
    //   .then(function(response){
    //     $rootScope.todosCounties = response;
    //   })
    //   .catch(function(err){
    //     console.log(err);
    //   });

    GlobalService.selectNumberAllPaises()
        .then(function(response) {
            var rowsPaises = response[0];
            if (rowsPaises.AllPaises == 0) {
                return GlobalService.insertPaises();
            } else {
                return GlobalService.selectAllPaises();
            }
        })
        .then(function(response) {
            if (!response.length) {
                return GlobalService.selectAllPaises();
            } else {
                return response;
            }
        })
        .then(function(response) {
            $rootScope.todosCountries = response;
        })
        .catch(function(err) {
            console.log(err);
        });

    GlobalService.selectNumberAllEtnia()
        .then(function(response) {
            var rowsEtnia = response[0];
            if (rowsEtnia.AllEtnia == 0) {
                return GlobalService.insertEtnia();
            } else {
                return GlobalService.selectAllEtnia();
            }
        })
        .then(function(response) {
            if (!response.length) {
                return GlobalService.selectAllEtnia();
            } else {
                return response;
            }
        })
        .then(function(response) {
            $rootScope.todasEtnias = response;
        })
        .catch(function(err) {
            console.log(err);
        });
    $rootScope.environmentApp = {
        environment: "dev", //dev - prod,
        version: "1.8.25"
    };

    //SQLiteService.preloadDataBase( true ); //true for debugging

});