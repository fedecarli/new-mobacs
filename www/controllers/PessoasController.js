// angular.module('mobacs')
angular.module('mobacs')
    .controller('PessoasController', function($scope, FamilyService, HomeRegistrationService, $ionicPlatform, $location, $ionicViewSwitcher, $rootScope, $state, $ionicHistory, HelpService, PeopleService, ionicDatePicker, $stateParams, GlobalService, SystemService, $ionicPopup, $cordovaGeolocation, $ionicLoading) {
        // $scope.dataPeople = {
        //   statusEhResponsavel: 1,
        //   statusTermoRecusaCadastroIndividualAtencaoBasica: null
        // };
        $rootScope.citizenType = {
            responsavel: null,
            nascimento: null,
            estadoCivil: null,
            nacionalidade: null,
            relacaoComResp: null,
            situacaoNoMerc: null
        };


        //Dados Auxiliares para o cep
        $scope.todosSantanaParnaiba = $rootScope.thisAllSantanaParnaiba;
        // $scope.allCounties = $rootScope.todosCounties;
        $scope.allTiposLogradouros = $rootScope.todosTiposLogradouros;
        //Fim de dados Auxiliares para o cep


        $scope.allEtnias = $rootScope.todasEtnias;
        $scope.allCountries = $rootScope.todosCountries;
        $scope.allCounties = $rootScope.todosCounties;
        $scope.allOcupations = $rootScope.todasOcupations;
        //Fim dados auxiliares
        // $scope.dataPeople.statusEhResponsavel == 1;
        // console.log($scope.dataPeople);


        $scope.voltar = function() {
            $ionicHistory.goBack();
        };

        $scope.goToViewPeople = function() {
            var location = $location.path();
            if (location === '/pessoas/tabs/add/add-identification') {
                $state.go('app.dashboard/index', {
                    reload: true
                });
            } else if (location.search('/pessoas/tabs/add/add-information') != -1) {
                $state.go('pessoas/tabs/edit-identification', {
                    item: angular.fromJson($stateParams.item)
                });
            } else if (location.search('/pessoas/tabs/add/add-refused') != -1) {
                console.log(location.search('/pessoas/tabs/add/add-refused'));
                $state.go('pessoas/tabs/edit-identification', {
                    item: angular.fromJson($stateParams.item)
                });
            } else if (location.search('/pessoas/tabs/view/') != -1) {
                $state.go('pessoas/index');
            } else if (location.search('/pessoas/tabs/add/edit-identification/') != -1) {
                $state.go('view/people', {
                    item: angular.fromJson($stateParams.item)
                });
            } else if (location.search('/pessoas/tabs/add/add-helth/') != -1) {
                $state.go('pessoas/tabs/add-information', {
                    item: angular.fromJson($stateParams.item)
                });
            } else if (location.search('/pessoas/tabs/add/add-street/') != -1) {
                $state.go('pessoas/tabs/add-helth', {
                    item: angular.fromJson($stateParams.item)
                });
            } else if (location.search('/pessoas/tabs/add/add-exit/') != -1) {
                $state.go('pessoas/tabs/street', {
                    item: angular.fromJson($stateParams.item)
                });
            }
        };

        $scope.direcionaPessoas = function() {
            $rootScope.newPeople = null;
            console.log("direciona pessoas");
            $state.go('pessoas/tabs/add-identification');
        };

        $ionicPlatform.registerBackButtonAction(function(event) {
            event.preventDefault();
            var location = $location.path();
            if (location === '/pessoas/tabs/add/add-identification') {
                console.log(location);
                $state.go('app.dashboard/index', {
                    reload: true
                });
            } else if (location === '/pessoas/tabs/add/add-refused') {
                console.log(location);
                $ionicViewSwitcher.nextDirection('back');
                $state.go('pessoas/tabs/edit-identification', {
                    reload: true
                });
            } else if (location.search('/pessoas/tabs/add/add-information') != -1) {
                $state.go('pessoas/tabs/edit-identification', {
                    item: angular.fromJson($stateParams.item)
                });
            } else if (location.search('/pessoas/tabs/view/') != -1) {
                $state.go('pessoas/index');
            } else if (location.search('/pessoas/tabs/add/edit-identification/') != -1) {
                $state.go('view/people', {
                    item: angular.fromJson($stateParams.item)
                });
            } else if (location.search('/pessoas/tabs/add/add-helth/') != -1) {
                $state.go('pessoas/tabs/add-information', {
                    item: angular.fromJson($stateParams.item)
                });
            } else if (location.search('/pessoas/tabs/add/add-street/') != -1) {
                $state.go('pessoas/tabs/add-helth', {
                    item: angular.fromJson($stateParams.item)
                });
            } else if (location.search('/pessoas/tabs/add/add-exit/') != -1) {
                $state.go('pessoas/tabs/street', {
                    item: angular.fromJson($stateParams.item)
                });
            } else if (location === '/app/dashboard/index') {
                $ionicPopup.confirm({
                        title: '<strong>Sair!</strong>',
                        template: 'Tem certeza que você deseja sair?'
                    })
                    .then(function(response) {
                        if (response)
                            ionic.Platform.exitApp();
                    });
            }

        }, 100);

        //BEGIN

        $scope.peopleGPSIsItEnabled = function() {


            $scope.peoplePosition = {
                lat: null,
                long: null
            };
            /*
                        var posOptions = {
                            timeout: 10000,
                            enableHighAccuracy: true
                        };

                        $cordovaGeolocation.getCurrentPosition(posOptions)
                            .then(function(position) {
                                console.log(position);
                                $scope.peoplePosition = {
                                    lat: position.coords.latitude,
                                    long: position.coords.longitude
                                };
                            })
                            .catch(function(err) {
                                console.error(err);
                            });

                        
                        cordova.plugins.diagnostic.isLocationEnabled(function(enabled) {
                                if (enabled) {

                                    var posOptions = {
                                        timeout: 10000,
                                        enableHighAccuracy: true
                                    };

                                    $cordovaGeolocation.getCurrentPosition(posOptions)
                                        .then(function(position) {
                                            console.log(position);
                                            $scope.peoplePosition = {
                                                lat: position.coords.latitude,
                                                long: position.coords.longitude
                                            };
                                            var latitud = $scope.peoplePosition.lat;
                                        })
                                        .catch(function(err) {
                                            console.error(err);
                                        });
                                    console.log(latitud);


                                    // var posOptions = { timeout: 10000, enableHighAccuracy: false };
                                    // $cordovaGeolocation.getCurrentPosition(posOptions)
                                    //   .then(function(position) {
                                    //     $scope.peoplePosition = {
                                    //       lat: position.coords.latitude,
                                    //       long: position.coords.longitude
                                    //     };
                                    // $scope.peoplePosition = {
                                    //   lat: null,
                                    //   long: null
                                    // };

                                    // }, function(err) {
                                    //   console.log(err);
                                    // });
                                    // })
                                    // .catch(function(err) {
                                    //   console.log(err);
                                    // })
            /*
                                } else {
                                    $ionicPopup.alert({
                                            title: '<strong>Informativo</strong>',
                                            template: 'Por favor, habilite o GPS para utilizar o aplicativo!'
                                        })
                                        .then(function() {
                                            $scope.peopleGPSIsItEnabled();
                                        });
                                }
                            },
                            function(error) {
                                console.error("The following error occurred:" + error);
                            });
                            */
        };

        $scope.peopleGPSIsItEnabled();

        $scope.valueInData = function(param) {

            var elemento = null;

            if (param == 'dataNascimentoCidadao') {
                elemento = document.getElementById('dataNascimentoCidadao');
                var RegExp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
                if (!RegExp.test(elemento.value)) {
                    document.getElementById("dataNascimentoCidadaoError").style.display = "block";
                } else {
                    document.getElementById("dataNascimentoCidadaoError").style.display = "none";
                }
            } else if (param == 'dtNaturalizacao') {
                elemento = document.getElementById('dtNaturalizacao');
                var RegExp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
                if (!RegExp.test(elemento.value)) {
                    document.getElementById("dtNaturalizacaoError").style.display = "block";
                } else {
                    document.getElementById("dtNaturalizacaoError").style.display = "none";
                }
            } else if (param == 'dtEntradaBrasil') {
                elemento = document.getElementById('dtEntradaBrasil');
                var RegExp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
                if (!RegExp.test(elemento.value)) {
                    document.getElementById("dtEntradaBrasilError").style.display = "block";
                } else {
                    document.getElementById("dtEntradaBrasilError").style.display = "none";
                }
            } else if (param == 'dataObito') {
                elemento = document.getElementById('dataObito');
                var RegExp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
                if (!RegExp.test(elemento.value)) {
                    document.getElementById("dataObitoError").style.display = "block";
                } else {
                    document.getElementById("dataObitoError").style.display = "none";
                }
            }

        };

        $scope.actualizeOrSaveCNS = function(cns, param) {

            if (param == 'update') {
                PeopleService.updatePeople('cnsCidadao = "' + cns + '" where cnsCidadao = "' + $rootScope.newPeople.cnsCidadao + '"')
                    .then(function(response) {
                        return FamilyService.updateCNS($rootScope.newPeople.cnsCidadao, cns);
                    })
                    .then(function(response) {
                        var updateChilds = 'cnsResponsavelFamiliar = "' + cns + '" where cnsResponsavelFamiliar = "' + $rootScope.newPeople.cnsCidadao + '"';
                        return PeopleService.updatePeople(updateChilds);
                    })
                    .then(function(response) {
                        $ionicPopup.alert({
                            title: '<strong>Informativo</strong>',
                            template: 'CNS atualizado em todos os registros com sucesso'
                        });
                    })
                    .catch(function(err) {
                        console.log(err);
                    });
            } else {
                $scope.dataPeople.cnsCidadao = cns;
            }

        };

        //END

        $scope.validNoDataPeople = function(param) {
            //é inválido
            if (!param)
                return true;
            else {

                param = param.toString();
                param = param.trim();

                while (param.search(/\s\s/) >= 0)
                    param = param.replace(/\s\s/g, ' ');

                if (param.length == 0) return true;
            }

            return false;
        };

        $scope.autocompleteText = function(param, string) {

            if (param == 'codigoIbgeMunicipioNascimento') {
                if (string != null && string.length > 2) {
                    $scope.hideListCountie = false;
                    var allCountie = $scope.allCounties;
                    var outputCountie = [];
                    var regularExpression = new RegExp('^' + string.toUpperCase());
                    angular.forEach(allCountie, function(countie, key) {
                        // if(countie.nome.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                        //   if(regularExpression.test(countie.nome)){
                        if (countie.nomeSemAcentuacao.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            if (regularExpression.test(countie.nomeSemAcentuacao)) {
                                outputCountie.push(countie);
                            }
                        }
                    });
                    $scope.filterCountie = outputCountie;
                } else {
                    $scope.hideListCountie = true;
                }
            } else if (param == 'etnia') {
                if (string != null && string.length > 2) {
                    $scope.hideListEtnia = false;
                    var allEtnia = $scope.allEtnias;
                    var outputEtnia = [];
                    angular.forEach(allEtnia, function(oneEtnia, key) {
                        if (oneEtnia.descricao.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            outputEtnia.push(oneEtnia);
                        }
                    });
                    $scope.filterEtnia = outputEtnia;
                } else {
                    $scope.hideListEtnia = true;
                }
            } else if (param == 'paisNascimento') {
                if (string != null && string.length > 2) {
                    $scope.hideListPaisNascimento = false;
                    var allPaisNascimento = $scope.allCountries;
                    var outputPaisNascimento = [];
                    angular.forEach(allPaisNascimento, function(onePaisNascimento, key) {
                        if (onePaisNascimento.nome.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            outputPaisNascimento.push(onePaisNascimento);
                        }
                    });
                    $scope.filterPaisNascimento = outputPaisNascimento;
                } else {
                    $scope.hideListPaisNascimento = true;
                }
            } else if (param == 'ocupacaoCodigoCbo2002') {
                if (string != null && string.length > 2) {
                    $scope.hideListOcupation = false;
                    var allOcupacaoCodigoCbo2002 = $scope.allOcupations;
                    var outputOcupation = [];
                    angular.forEach(allOcupacaoCodigoCbo2002, function(oneOcupation, key) {
                        if (oneOcupation.nome.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            outputOcupation.push(oneOcupation);
                        }
                    });
                    $scope.filterOcupation = outputOcupation;
                } else {
                    $scope.hideListOcupation = true;
                }
            }
        };

        $scope.removeTheUsersAdress = function(cnsResponsavel) {

            PeopleService.selectTheAdressPeople(cnsResponsavel)
                .then(function(response) {
                    if (response.length > 0) {
                        $ionicPopup.confirm({
                            title: '<strong>Responsável Familiar!</strong>',
                            subTitle: 'Você deseja confirmar está alteração?',
                            buttons: [{
                                    text: 'Não',
                                    type: 'button-positive',
                                    onTap: function() {
                                        $scope.disabledInput.cnsResponsavelFamiliar = true;
                                        $scope.requiredInput.cnsResponsavelFamiliar = false;
                                        $scope.dataPeople.statusEhResponsavel = 1;
                                    }

                                },
                                {
                                    text: '<b>Sim</b>',
                                    type: 'button-positive',
                                    onTap: function() {
                                        var conditionOne = null;
                                        var conditionTwo = null;
                                        conditionOne = 'id = ' + response[0].id + '';
                                        conditionTwo = 'id = ' + response[0].cadastroDomiciliarId + '';
                                        FamilyService.deleteFamilies(conditionOne)
                                            .then(function(response) {
                                                return HomeRegistrationService.deleteHomeAdress(conditionTwo);
                                            })
                                            .then(function(response) {
                                                console.log('Removeu um cadastro domiciliar/família');
                                            })
                                            .catch(function(err) {
                                                console.log(err);
                                            });
                                    }
                                }
                            ]
                        });
                    }
                })
                .catch(function(err) {
                    console.log(err);
                });

        };

        $scope.alterInputCns = function(cns) {

            var updateCNS = null;

            if (typeof angular.fromJson($stateParams.item) != 'undefined' && typeof cns != 'undefined') {

                if ($scope.validNoDataPeople(cns)) {
                    if (($rootScope.newPeople.cnsCidadao != null) && ($rootScope.newPeople.statusEhResponsavel == 1)) {
                        updateCNS = false;
                    }
                } else if (cns.length == 15 && ($rootScope.newPeople.cnsCidadao != cns) && ($rootScope.newPeople.statusEhResponsavel == 1)) {
                    updateCNS = true;
                }
            } else if (typeof angular.fromJson($stateParams.item) == 'undefined') {
                if ($scope.validNoDataPeople(cns)) {
                    updateCNS = null;
                } else {
                    updateCNS = true;
                }
            }

            if (updateCNS) {
                PeopleService.getPeople('cnsCidadao = "' + cns + '"')
                    .then(function(response) {
                        if (response.length == 0) {
                            $scope.actualizeOrSaveCNS(cns, typeof angular.fromJson($stateParams.item) == 'undefined' ? 'save' : 'update');
                        } else {
                            $ionicPopup.alert({
                                title: '<strong>Informativo</strong>',
                                template: 'O CNS digitado já está atrelado a outro cidadão. Verifique se você digitou o CNS correto ou<br>' +
                                    'atualize os dados do cidadão <strong>' + response[0].nomeCidadao + '</strong>'
                            });
                            $scope.dataPeople.cnsCidadao = null;
                        }
                    })
                    .catch(function(err) {
                        console.log(err);
                    });
            } else if (updateCNS == false) { // Validação necessário para o quando o usuário cadastra o Cidadão do tablet
                $ionicPopup.alert({
                    title: '<strong>Informativo</strong>',
                    template: 'O campo "CNS do Cidadão" não pode ser vazio, outros registros podem ser dependentes deste cidadão.<br>' +
                        'Insira um CNS válido ou verifique a necessidade da alteração do mesmo!'
                });
                $scope.dataPeople.cnsCidadao = $rootScope.newPeople.cnsCidadao;
            }

        };

        $scope.treatmentForText = function(nome, param) {

            var response = {
                valid: true
            };
            var mensagem = null;

            if (param == 'codigoIbgeMunicipioNascimento')
                mensagem = 'Insira o nome do município';
            else if (param == 'etnia')
                mensagem = 'Insira uma etnia';
            else if (param == 'paisNascimento')
                mensagem = 'Insira o país de nascimento';

            if (nome == null) {
                if (param != 'ocupacaoCodigoCbo2002') {
                    response = {
                        valid: false,
                        mensagem: mensagem
                    };
                    return response;
                } else {
                    response.valor = null;
                    return response;
                }

            }
            nome = nome.trim();

            // while (nome.search(/\s\s/) >= 0)
            //   nome = nome.replace(/\s\s/g, ' ');

            if (nome.length == 0) {
                if (param != 'ocupacaoCodigoCbo2002') {
                    response = {
                        valid: false,
                        mensagem: mensagem
                    };
                    return response;
                } else {
                    response.valor = null;
                    return response;
                }

            }
            nome = nome.toUpperCase(nome);

            if (param == 'codigoIbgeMunicipioNascimento') {
                var allCountie = $scope.allCounties;
                var outputCountie = [];
                angular.forEach(allCountie, function(countie, key) {
                    if (countie.nome.toLowerCase() === nome.toLowerCase()) {
                        outputCountie.push(countie);
                    }
                });

                if (outputCountie.length == 0) {
                    response = {
                        valid: false,
                        mensagem: 'O nome do município é inválido, por favor selecione uma das sugetões apresentadas ao digitar o texto!'
                    };
                    return response;
                } else {
                    response.valor = outputCountie[0].codigoIBGE;
                }

            } else if (param == 'etnia') {

                var allEtnia = $scope.allEtnias;
                var outputEtnia = [];
                angular.forEach(allEtnia, function(oneEtnia, key) {
                    if (oneEtnia.descricao.toLowerCase() === nome.toLowerCase()) {
                        outputEtnia.push(oneEtnia);
                    }
                });

                if (outputEtnia.length == 0) {
                    response = {
                        valid: false,
                        mensagem: 'O nome da etnia é inválido, por favor selecione uma das sugetões apresentadas ao digitar o texto!'
                    };
                    return response;
                } else {
                    response.valor = outputEtnia[0].codigo;
                }

            } else if (param == 'paisNascimento') {

                var allPaisNascimento = $scope.allCountries;
                var outputPaisNascimento = [];
                angular.forEach(allPaisNascimento, function(onePaisNascimento, key) {
                    if (onePaisNascimento.nome.toLowerCase() === nome.toLowerCase()) {
                        outputPaisNascimento.push(onePaisNascimento);
                    }
                });

                if (outputPaisNascimento.length == 0) {
                    response = {
                        valid: false,
                        mensagem: 'O nome do país é inválido, por favor selecione uma das sugetões apresentadas ao digitar o texto!'
                    };
                    return response;
                } else {
                    response.valor = outputPaisNascimento[0].codigo;
                }
            } else if (param == 'ocupacaoCodigoCbo2002') {

                var allOcupacaoCodigoCbo2002 = $scope.allOcupations;
                var outputOcupation = [];
                angular.forEach(allOcupacaoCodigoCbo2002, function(oneOcupation, key) {
                    if (oneOcupation.nome.toLowerCase() === nome.toLowerCase()) {
                        outputOcupation.push(oneOcupation);
                    }
                });

                if (outputOcupation.length == 0) {
                    response = {
                        valid: false,
                        mensagem: 'O nome da ocupação é inválida, por favor selecione uma das sugetões apresentadas ao digitar o texto!'
                    };
                    return response;
                } else {
                    response.valor = outputOcupation[0].codigo;
                }
            }

            return response;

        };

        $scope.checkCNSOfTheResponsible = function(cns) {

            if (cns != undefined) {
                PeopleService.selectPeopleResponsible(cns)
                    .then(function(response) {
                        if (response.length != 0) {
                            if (response[0].statusEhResponsavel != 1) {
                                $ionicPopup.alert({
                                    title: '<strong>Informativo</strong>',
                                    template: 'O CNS informado não é um reponsável familiar!'
                                });
                            }
                        } else {
                            $scope.dataPeople.cnsResponsavelFamiliar = null;
                            $ionicPopup.alert({
                                title: '<strong>Informativo</strong>',
                                template: 'Responsável não cadastrado! <br> Para este vínculo, é necessário que o CNS digitado seja responsável familiar!'
                            });
                        }
                    })
                    .catch(function(err) {
                        console.log(err);
                    });
            }
        };

        $scope.searchEtnia = function(search) {
            if (search != undefined && search != null) {
                if (search.length > 2) {
                    HelpService.selectEtnias(search)
                        .then(function(response) {
                            $scope.hideListEtnia = false;
                            $scope.filterEtnia = response;
                        })
                        .catch(function(err) {
                            console.log(err);
                        });
                } else {
                    $scope.hideListEtnia = true;
                }
            }
        };

        $scope.searchMunicipio = function(search) {
            console.log(search);
            if (search != undefined && search != null) {
                if (search.length > 2) {
                    HelpService.selectMunicipos(search)
                        .then(function(response) {
                            console.log(response);
                            $scope.hideListCountie = false;
                            $scope.filterCountie = response;
                        })
                        .catch(function(err) {
                            console.log(err);
                        });
                } else {
                    $scope.filterCountie = null;
                    $scope.hideListCountie = true;
                }
            }
        };

        $scope.searchPaises = function(search) {
            if (search != undefined && search != null) {
                if (search.length > 2) {
                    HelpService.selectPaises(search)
                        .then(function(response) {
                            $scope.hideListPaisNascimento = false;
                            $scope.filterPaisNascimento = response;
                        })
                        .catch(function(err) {
                            console.log(err);
                        });
                } else {
                    $scope.hideListPaisNascimento = true;
                }
            }
        };

        $scope.searchOcupation = function(search) {
            if (search != undefined && search != null) {
                if (search.length > 2) {
                    HelpService.selectOcupacao(search)
                        .then(function(response) {
                            $scope.hideListOcupation = false;
                            $scope.filterOcupation = response;
                        })
                        .catch(function(err) {
                            console.log(err);
                        });
                } else {
                    $scope.filterOcupation = null;
                    $scope.hideListOcupation = true;
                    $ionicPopup.alert({
                        title: '<strong>Informativo</strong>',
                        template: 'Insira ao menos duas letras para a busca!'
                    });
                }
            }
        };

        $scope.viewToEditPeople = function() {
            if ($rootScope.newPeople.statusTermoRecusaCadastroIndividualAtencaoBasica == 1) {
                $ionicPopup.alert({
                    title: '<strong>Informativo</strong>',
                    template: 'Não é possível editar um cadastro que foi recusado'
                });
            } else {
                $state.go('pessoas/tabs/edit-identification', {
                    item: angular.fromJson($stateParams.item)
                });
            }
        };

        $scope.clearObservation = function() {
            $scope.dataPeople.observacao = null;
        };

        $scope.formatName = function(param, Name) {
            if (Name != undefined) {
                Name = Name.replace(/[^a-zA-ZéúíóáÉÚÍÓÁèùìòàçÇÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄ\-\ \s]+$/, "");
                Name = Name.toUpperCase();

                if (param == 'nomeCidadao') {
                    $scope.dataPeople.nomeCidadao = Name;
                } else if (param == 'nomeSocial') {
                    $scope.dataPeople.nomeSocial = Name;
                } else if (param == 'nomeMaeCidadao') {
                    $scope.dataPeople.nomeMaeCidadao = Name;
                } else if (param == 'nomePaiCidadao') {
                    $scope.dataPeople.nomePaiCidadao = Name;
                }
            } else {
                return null;
            }
        };

        $scope.refusePeopleRegistration = function() {
            $rootScope.newPeople = null;
            $state.go('pessoas/tabs/add-refused');
        };

        $scope.directPeople = function() {
            $rootScope.newPeople = null;
            $state.go('pessoas/tabs/add-identification');
        };

        $scope.searchAdressInformation = function(string) {

            if (typeof string !== 'undefined') {
                var allAdress = $scope.todosSantanaParnaiba;
                var saida = [];
                string = string.replace('-', '');
                angular.forEach(allAdress, function(adress, key) {
                    if (adress.cep.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                        saida.push(adress);
                    }
                });
                if (saida.length > 0) {
                    $scope.dataPeople.codigoIbgeMunicipio = 'SANTANA DE PARNAÍBA';
                    $scope.dataPeople.numeroDneUf = '26';
                    $scope.dataPeople.tipoLogradouroNumeroDne = saida[0].tipoDeLogradouro.toString();
                    $scope.dataPeople.nomeLogradouro = saida[0].nomeLogradouro.toString();
                    $scope.dataPeople.bairro = saida[0].nomeBairro.toString();
                } else {
                    $scope.dataPeople.codigoIbgeMunicipio = null;
                    $scope.dataPeople.numeroDneUf = null;
                    $scope.dataPeople.tipoLogradouroNumeroDne = null;
                    $scope.dataPeople.nomeLogradouro = null;
                    $scope.dataPeople.bairro = null;
                    $scope.dataPeople.cep = '';
                    $ionicPopup.alert({
                        title: 'CEP Inválido',
                        template: 'Apenas são aceitos CEPs válidos de Santana de Parnaíba'
                    });
                }
            }
        };
        $scope.addRefusedRegistration = function(dataPeople) {

            var dataInformation;

            console.log(dataPeople);
            console.log(JSON.stringify(dataPeople));
            if (typeof dataPeople != 'undefined') {
                dataInformation = {
                    profissionalCNS: $rootScope.userLogged.profissionalCNS,
                    cboCodigo_2002: $rootScope.userLogged.cboCodigo_2002,
                    cnes: $rootScope.userLogged.cnes,
                    ine: $rootScope.userLogged.ine,
                    dataAtendimento: new Date().getTime(),
                    codigoIbgeMunicipio: $rootScope.userLogged.codigoIbgeMunicipio,
                    DataRegistro: new Date().getTime(),
                    codigoIbgeMunicipioNascimento: typeof dataPeople.codigoIbgeMunicipioNascimento !== 'undefined' ? dataPeople.codigoIbgeMunicipioNascimento : null,
                    statusTermoRecusaCadastroIndividualAtencaoBasica: 1,
                    dataNascimentoCidadao: typeof dataPeople.dataNascimentoCidadao !== 'undefined' ? dataPeople.dataNascimentoCidadao : null,
                    nomeCidadao: typeof dataPeople.nomeCidadao !== 'undefined' ? dataPeople.nomeCidadao : null,
                    desconheceNomeMae: typeof dataPeople.desconheceNomeMae !== 'undefined' ? dataPeople.desconheceNomeMae : null,
                    nacionalidadeCidadao: typeof dataPeople.nacionalidadeCidadao !== 'undefined' ? dataPeople.nacionalidadeCidadao : null,
                    nomeMaeCidadao: typeof dataPeople.nomeMaeCidadao !== 'undefined' ? dataPeople.nomeMaeCidadao : null,
                    paisNascimento: typeof dataPeople.paisNascimento !== 'undefined' ? dataPeople.paisNascimento : null,
                    racaCorCidadao: typeof dataPeople.racaCorCidadao !== 'undefined' ? dataPeople.racaCorCidadao : null,
                    sexoCidadao: typeof dataPeople.sexoCidadao !== 'undefined' ? dataPeople.sexoCidadao : null,
                    etnia: typeof dataPeople.etnia !== 'undefined' ? dataPeople.etnia : null,
                    nomePaiCidadao: typeof dataPeople.nomePaiCidadao !== 'undefined' ? dataPeople.nomePaiCidadao : null,
                    desconheceNomePai: typeof dataPeople.desconheceNomePai !== 'undefined' ? dataPeople.desconheceNomePai : null,
                    dtNaturalizacao: typeof dataPeople.dtNaturalizacao !== 'undefined' ? dataPeople.dtNaturalizacao : null,
                    portariaNaturalizacao: typeof dataPeople.portariaNaturalizacao !== 'undefined' ? dataPeople.portariaNaturalizacao : null,
                    dtEntradaBrasil: typeof dataPeople.dtEntradaBrasil !== 'undefined' ? dataPeople.dtEntradaBrasil : null,
                    microarea: typeof dataPeople.microarea !== 'undefined' ? dataPeople.microarea : null,
                    stForaArea: typeof dataPeople.stForaArea !== 'undefined' ? dataPeople.stForaArea : null,
                    justificativa: typeof dataPeople.justificativa !== 'undefined' ? dataPeople.justificativa : null,
                    status: 'Aguardando Sincronismo'
                };
                console.log(dataInformation);
                console.log(JSON.stringify(dataInformation));

            }

            var mensagem = null;
            var invalid = false;

            if (dataInformation.stForaArea != 1) {
                if ($scope.validNoDataPeople(dataInformation.microarea)) {
                    invalid = true;
                    mensagem = 'Insira a Microarea!';
                }
            }

            if ($scope.validNoDataPeople(dataInformation.dataNascimentoCidadao)) {
                document.getElementById("dataNascimentoCidadaoError").style.display = "block";
                invalid = true;
                mensagem = 'A data de nascimento é obrigatória no formato dd/MM/yyyy!';
            } else {
                var elemento = document.getElementById('dataNascimentoCidadao');
                var RegExp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
                if (!RegExp.test(elemento.value)) {
                    document.getElementById("dataNascimentoCidadaoError").style.display = "block";
                    invalid = true;
                    mensagem = 'Insira a data no formato indicado dd/MM/yyyy!';
                }

            }

            if ($scope.validNoDataPeople(dataInformation.sexoCidadao)) {
                invalid = true;
                mensagem = 'O sexo do cidadão é obrigatório!';
            }

            if (dataInformation.racaCorCidadao == null) {
                invalid = true;
                mensagem = 'A Raça/Cor do cidadão é obrigatório!';
            }

            if (dataInformation.racaCorCidadao == 5) {
                var codigoEtnia = $scope.treatmentForText(dataInformation.etnia, 'etnia');

                if (!codigoEtnia.valid) {
                    invalid = true;
                    mensagem = codigoEtnia.mensagem;
                } else {
                    dataInformation.etnia = codigoEtnia.valor;
                }
            }

            if (dataInformation.nacionalidadeCidadao == null) {
                invalid = true;
                mensagem = 'A Nacionalidade do cidadão é obrigatória';
            }

            if (dataInformation.nacionalidadeCidadao == 1) {
                var codigoIbge = $scope.treatmentForText(dataInformation.codigoIbgeMunicipioNascimento, 'codigoIbgeMunicipioNascimento');

                if (!codigoIbge.valid) {
                    invalid = true;
                    mensagem = codigoIbge.mensagem;
                } else {
                    dataInformation.paisNascimento = 31;
                    dataInformation.codigoIbgeMunicipioNascimento = codigoIbge.valor;
                }
            }

            if (dataInformation.nacionalidadeCidadao == 2) {

                if ($scope.validNoDataPeople(dataInformation.portariaNaturalizacao)) {
                    invalid = true;
                    mensagem = 'Para este tipo de nacionalidade, a portaria de naturalização é obrigatória!';
                }

                if ($scope.validNoDataPeople(dataInformation.dataNascimentoCidadao)) {
                    invalid = true;
                    mensagem = 'A data de nascimento é obrigatória no formato dd/MM/yyyy';

                } else {
                    if ($scope.validNoDataPeople(dataInformation.dtNaturalizacao)) {
                        invalid = true;
                        mensagem = 'Para este tipo de nacionalidade, a data de naturalização é obrigatória no formato dd/MM/yyyy';
                    } else {
                        if ($scope.validTwoDate(dataInformation.dataNascimentoCidadao, dataInformation.dtNaturalizacao)) {
                            invalid = true;
                            mensagem = 'Data de Naturalização não pode ser antes da data de Nascimento';
                        }
                    }
                }
            }

            if (dataInformation.nacionalidadeCidadao == 3) {
                var codigoPais = $scope.treatmentForText(dataInformation.paisNascimento, 'paisNascimento');

                if (!codigoPais.valid) {
                    invalid = true;
                    mensagem = codigoPais.mensagem;
                } else {
                    dataInformation.paisNascimento = codigoPais.valor;
                }

                if ($scope.validNoDataPeople(dataInformation.dataNascimentoCidadao)) {
                    invalid = true;
                    mensagem = 'A data de nascimento é obrigatória no formato dd/MM/yyyy';

                } else {
                    if ($scope.validNoDataPeople(dataInformation.dtEntradaBrasil)) {
                        invalid = true;
                        mensagem = 'Para este tipo de  nacionalidade, a data de entrada no Brasil é obrigatória no formato dd/MM/yyyy';
                    } else {
                        if ($scope.validTwoDate(dataInformation.dataNascimentoCidadao, dataInformation.dtEntradaBrasil)) {
                            invalid = true;
                            mensagem = 'Data de entrada no Brasil não pode ser antes da data de Nascimento';
                        }
                    }
                }

                if (dataInformation.paisNascimento == '31') {
                    invalid = true;
                    mensagem = 'Para está nacionalidade, o País de Nascimento não pode ser o BRASIL';
                    document.getElementById('paisNascimento').value = null;
                }
            }

            var nome, motherName, fatherName;

            nome = $scope.validName(dataInformation.nomeCidadao, 'Cidadão');
            if (nome.valid == false) {
                invalid = true;
                mensagem = nome.mensagem;
            }

            if (dataInformation.desconheceNomeMae != 1) {
                motherName = $scope.validName(dataInformation.nomeMaeCidadao, 'Mãe');
                if (motherName.valid == false) {
                    invalid = true;
                    mensagem = motherName.mensagem;
                }
            }

            if (dataInformation.desconheceNomePai != 1) {
                fatherName = $scope.validName(dataInformation.nomePaiCidadao, 'Pai');
                if (fatherName.valid == false) {
                    invalid = true;
                    mensagem = fatherName.mensagem;
                }
            }

            if ($scope.validNoDataPeople(dataInformation.justificativa)) {
                invalid = true;
                mensagem = 'Campo Justificativa é obrigatório!';
            }

            console.log(dataInformation);
            console.log(JSON.stringify(dataInformation));

            if (invalid) {
                $ionicPopup.alert({

                    title: 'Ocorreu um erro',
                    template: mensagem
                });

            } else {
                console.log(dataInformation);
                console.log(JSON.stringify(dataInformation));
                PeopleService.insertRefusePeopleRegistration(dataInformation).then(function(response) {
                    $rootScope.newPeople = null;
                    $ionicHistory.clearCache();
                    $state.go('app.dashboard/index', {
                        reload: true
                    });
                }).catch(function(error) {
                    console.log(error);
                });
            }

        };

        /*$scope.addRefusedRegistration_old = function(dataPeople) {
                var dataInformation;

                var mensagem = null;
                var invalid = false;

                var dataInformation = {
                        profissionalCNS: $rootScope.userLogged.profissionalCNS,
                        cboCodigo_2002: $rootScope.userLogged.cboCodigo_2002,
                        cnes: $rootScope.userLogged.cnes,
                        ine: $rootScope.userLogged.ine,
                        dataAtendimento: new Date().getTime(),
                        codigoIbgeMunicipio: $rootScope.userLogged.codigoIbgeMunicipio,
                        DataRegistro: new Date().getTime(),
                        nomeSocial: typeof dataPeople.nomeSocial !== 'undefined' ? dataPeople.nomeSocial : null,
                        codigoIbgeMunicipioNascimento: typeof dataPeople.codigoIbgeMunicipioNascimento !== 'undefined' ? dataPeople.codigoIbgeMunicipioNascimento : null,
                        dataNascimentoCidadao: typeof dataPeople.dataNascimentoCidadao !== 'undefined' ? dataPeople.dataNascimentoCidadao : null,
                        desconheceNomeMae: typeof dataPeople.desconheceNomeMae !== 'undefined' ? dataPeople.desconheceNomeMae : null,
                        nacionalidadeCidadao: typeof dataPeople.nacionalidadeCidadao !== 'undefined' ? dataPeople.nacionalidadeCidadao : null,
                        nomeCidadao: typeof dataPeople.nomeCidadao !== 'undefined' ? dataPeople.nomeCidadao : null,
                        nomeMaeCidadao: typeof dataPeople.nomeMaeCidadao !== 'undefined' ? dataPeople.nomeMaeCidadao : null,
                        paisNascimento: typeof dataPeople.paisNascimento !== 'undefined' ? dataPeople.paisNascimento : null,
                        racaCorCidadao: typeof dataPeople.racaCorCidadao !== 'undefined' ? dataPeople.racaCorCidadao : null,
                        EstadoCivil: typeof dataPeople.EstadoCivil !== 'undefined' ? dataPeople.EstadoCivil : null,
                        sexoCidadao: typeof dataPeople.sexoCidadao !== 'undefined' ? dataPeople.sexoCidadao : null,
                        etnia: typeof dataPeople.etnia !== 'undefined' ? dataPeople.etnia : null,
                        nomePaiCidadao: typeof dataPeople.nomePaiCidadao !== 'undefined' ? dataPeople.nomePaiCidadao : null,
                        desconheceNomePai: typeof dataPeople.desconheceNomePai !== 'undefined' ? dataPeople.desconheceNomePai : null,
                        dtNaturalizacao: typeof dataPeople.dtNaturalizacao !== 'undefined' ? dataPeople.dtNaturalizacao : null,
                        portariaNaturalizacao: typeof dataPeople.portariaNaturalizacao !== 'undefined' ? dataPeople.portariaNaturalizacao : null,
                        dtEntradaBrasil: typeof dataPeople.dtEntradaBrasil !== 'undefined' ? dataPeople.dtEntradaBrasil : null,
                        microarea: typeof dataPeople.microarea !== 'undefined' ? dataPeople.microarea : null,
                        stForaArea: typeof dataPeople.stForaArea !== 'undefined' ? dataPeople.stForaArea : null,
                        status: 'Aguardando Sincronismo'
                };

                if (dataInformation.stForaArea != 1) {
                        if ($scope.validNoDataPeople(dataInformation.microarea)) {
                                invalid = true;
                                mensagem = 'Insira a Microarea!';
                        }
                }

                if ($scope.validNoDataPeople(dataInformation.nomeSocial)) {
                        dataInformation.nomeSocial = null;
                }

                if ($scope.validNoDataPeople(dataInformation.dataNascimentoCidadao)) {
                        document.getElementById("dataNascimentoCidadaoError").style.display = "block";
                        invalid = true;
                        mensagem = 'A data de nascimento é obrigatória no formato dd/MM/yyyy!';
                } else {
                        var elemento = document.getElementById('dataNascimentoCidadao');
                        var RegExp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
                        if (!RegExp.test(elemento.value)) {
                                document.getElementById("dataNascimentoCidadaoError").style.display = "block";
                                invalid = true;
                                mensagem = 'Insira a data no formato indicado dd/MM/yyyy!';
                        }

                }

                if ($scope.validNoDataPeople(dataInformation.sexoCidadao)) {
                        invalid = true;
                        mensagem = 'O sexo do cidadão é obrigatório!';
                }

                if (dataInformation.racaCorCidadao == null) {
                        invalid = true;
                        mensagem = 'A Raça/Cor do cidadão é obrigatório!';
                }

                if (dataInformation.racaCorCidadao == 5) {
                        var codigoEtnia = $scope.treatmentForText(dataInformation.etnia, 'etnia');

                        if (!codigoEtnia.valid) {
                                invalid = true;
                                mensagem = codigoEtnia.mensagem;
                        } else {
                                dataInformation.etnia = codigoEtnia.valor;
                        }
                }

                if (dataInformation.nacionalidadeCidadao == null) {
                        invalid = true;
                        mensagem = 'A Nacionalidade do cidadão é obrigatória';
                }

                if (dataInformation.nacionalidadeCidadao == 1) {
                        var codigoIbge = $scope.treatmentForText(dataInformation.codigoIbgeMunicipioNascimento, 'codigoIbgeMunicipioNascimento');

                        if (!codigoIbge.valid) {
                                invalid = true;
                                mensagem = codigoIbge.mensagem;
                        } else {
                                dataInformation.paisNascimento = 31;
                                dataInformation.codigoIbgeMunicipioNascimento = codigoIbge.valor;
                        }
                }

                if (dataInformation.nacionalidadeCidadao == 2) {

                        if ($scope.validNoDataPeople(dataInformation.portariaNaturalizacao)) {
                                invalid = true;
                                mensagem = 'Para este tipo de nacionalidade, a portaria de naturalização é obrigatória!';
                        }

                        if ($scope.validNoDataPeople(dataInformation.dataNascimentoCidadao)) {
                                invalid = true;
                                mensagem = 'A data de nascimento é obrigatória no formato dd/MM/yyyy';

                        } else {
                                if ($scope.validNoDataPeople(dataInformation.dtNaturalizacao)) {
                                        invalid = true;
                                        mensagem = 'Para este tipo de nacionalidade, a data de naturalização é obrigatória no formato dd/MM/yyyy';
                                } else {
                                        if ($scope.validTwoDate(dataInformation.dataNascimentoCidadao, dataInformation.dtNaturalizacao)) {
                                                invalid = true;
                                                mensagem = 'Data de Naturalização não pode ser antes da data de Nascimento';
                                        }
                                }
                        }
                }

                if (dataInformation.nacionalidadeCidadao == 3) {
                        var codigoPais = $scope.treatmentForText(dataInformation.paisNascimento, 'paisNascimento');

                        if (!codigoPais.valid) {
                                invalid = true;
                                mensagem = codigoPais.mensagem;
                        } else {
                                dataInformation.paisNascimento = codigoPais.valor;
                        }

                        if ($scope.validNoDataPeople(dataInformation.dataNascimentoCidadao)) {
                                invalid = true;
                                mensagem = 'A data de nascimento é obrigatória no formato dd/MM/yyyy';

                        } else {
                                if ($scope.validNoDataPeople(dataInformation.dtEntradaBrasil)) {
                                        invalid = true;
                                        mensagem = 'Para este tipo de  nacionalidade, a data de entrada no Brasil é obrigatória no formato dd/MM/yyyy';
                                } else {
                                        if ($scope.validTwoDate(dataInformation.dataNascimentoCidadao, dataInformation.dtEntradaBrasil)) {
                                                invalid = true;
                                                mensagem = 'Data de entrada no Brasil não pode ser antes da data de Nascimento';
                                        }
                                }
                        }

                        if (dataInformation.paisNascimento == '31') {
                                invalid = true;
                                mensagem = 'Para está nacionalidade, o País de Nascimento não pode ser o BRASIL';
                                document.getElementById('paisNascimento').value = null;
                        }
                }

                var nome, motherName, fatherName;

                nome = $scope.validName(dataInformation.nomeCidadao, 'Cidadão');
                if (nome.valid == false) {
                        invalid = true;
                        mensagem = nome.mensagem;
                }

                if (dataInformation.desconheceNomeMae != 1) {
                        motherName = $scope.validName(dataInformation.nomeMaeCidadao, 'Mãe');
                        if (motherName.valid == false) {
                                invalid = true;
                                mensagem = motherName.mensagem;
                        }
                }

                if (dataInformation.desconheceNomePai != 1) {
                        fatherName = $scope.validName(dataInformation.nomePaiCidadao, 'Pai');
                        if (fatherName.valid == false) {
                                invalid = true;
                                mensagem = fatherName.mensagem;
                        }
                }

                if (invalid) {
                        $ionicPopup.alert({
                                title: 'Ocorreu um erro',
                                template: mensagem
                        })

                }

                if ($scope.validNoDataPeople(dataPeople.Justificativa)) {
                        $ionicPopup.alert({
                                title: '<strong>Atenção</strong>',
                                template: 'O campo Justificativa é obrigatório.'
                        }).then(function() {
                                document.getElementById('Justificativa').focus();
                        });

                        return false;
                }

                if ($scope.validNoDataPeople(dataPeople.nomeCidadao)) {
                        dataPeople.nomeCidadao = null;
                }

                if ($scope.validNoDataPeople(dataPeople.dataNascimentoCidadao)) {
                        dataPeople.dataNascimentoCidadao = null;
                }
                console.log(dataInformation);

                PeopleService.insertRefusePeopleRegistration(dataInformation).then(function(response) {
                        $rootScope.newPeople = null;
                        $ionicHistory.clearCache();
                        $state.go('app.dashboard/index', { reload: true });
                }).catch(function(error) {
                        console.log(error);
                })
        }*/

        /*$scope.refusePeopleRegistration_old = function() {
                $ionicPopup.confirm({
                        // template:
                        template: '<div class="row"><div class="col col-100"><div class="item item-divider">JUSTIFICATIVA</div><label class="item item-input item-stacked-label"><input type="text" name="Justificativa" ng-model="dataPeople.Justificativa"></label></div></div>', //ng-model="data.wifi"
                        title: 'Recusa do Cadastro!',
                        subTitle: 'Será gerado um número identificador sobre a recusa deste cadastro!',
                        buttons: [
                                { text: 'Voltar' },
                                {
                                        text: '<b>Confirmar</b>',
                                        type: 'button-positive',
                                        onTap: function() {
                                                if ($scope.validNoDataPeople($scope.dataPeople.Justificativa)) {
                                                        return false;
                                                }
                                                var dataInformation = {
                                                        profissionalCNS: $rootScope.userLogged.profissionalCNS,
                                                        cboCodigo_2002: $rootScope.userLogged.cboCodigo_2002,
                                                        cnes: $rootScope.userLogged.cnes,
                                                        ine: $rootScope.userLogged.ine,
                                                        dataAtendimento: new Date().getTime(),
                                                        codigoIbgeMunicipioHeader: $rootScope.userLogged.codigoIbgeMunicipio,
                                                        statusTermoRecusa: 1,
                                                        DataRegistro: new Date().getTime(),
                                                        status: 'Aguardando Sincronismo'
                                                };

                                                PeopleService.insertRefusePeopleRegistration(dataInformation).then(function(response) {
                                                        $rootScope.newPeople = null;
                                                        $ionicHistory.clearCache();
                                                        $state.go('app.dashboard/index', { reload: true });
                                                }).catch(function(error) {
                                                        console.log(error);
                                                })
                                        }
                                }
                        ]
                })
        };*/

        /*$scope.convertToSaveData = function(param, string){
                if(string != null){
                        if(string.length > 2){
                                if(param == 'codigoIbgeMunicipioNascimento'){
                                        var allCountie = $scope.allCounties;
                                        var outputCountie = [];
                                        angular.forEach(allCountie,function(countie,key){
                                                //if(countie.nome.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                                if(countie.nome.toLowerCase() === string.toLowerCase()) {
                                                                outputCountie.push(countie);
                                                }
                                        });
                                        return outputCountie[0].codigoIBGE;
                                }else if(param == 'etnia'){
                                        var allEtnia = $scope.allEtnias;
                                        var outputEtnia = [];
                                        angular.forEach(allEtnia,function(oneEtnia,key){
                                                if(oneEtnia.descricao.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                                        outputEtnia.push(oneEtnia);
                                                }
                                        });
                                        return outputEtnia[0].codigo;
                                }else if(param == 'paisNascimento'){
                                        var allPaisNascimento = $scope.allCountries;
                                        var outputPaisNascimento = [];
                                        angular.forEach(allPaisNascimento,function(onePaisNascimento,key){
                                                if(onePaisNascimento.nome.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                                        outputPaisNascimento.push(onePaisNascimento);
                                                }
                                        });
                                        return outputPaisNascimento[0].codigo;
                                }else if(param == 'ocupacaoCodigoCbo2002'){
                                        var allOcupacaoCodigoCbo2002 = $scope.allOcupations;
                                        var outputOcupation = [];
                                        angular.forEach(allOcupacaoCodigoCbo2002,function(oneOcupation,key){
                                                if(oneOcupation.nome.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                                        outputOcupation.push(oneOcupation)
                                                }
                                        });
                                        return outputOcupation[0].codigo;
                                }
                        }else{
                                return null;
                        }
                }else{
                        return null;
                }
        }*/

        $scope.fillTextbox = function(param, string) {

            if (param == 'codigoIbgeMunicipioNascimento') {
                $scope.dataPeople.codigoIbgeMunicipioNascimento = string.nome;
                $scope.hideListCountie = true;
            } else if (param == 'etnia') {
                $scope.dataPeople.etnia = string.descricao;
                $scope.hideListEtnia = true;
            } else if (param == 'paisNascimento') {
                $scope.dataPeople.paisNascimento = string.nome;
                $scope.hideListPaisNascimento = true;

                if (document.getElementById('nacionalidadeCidadao').value == 3 && $scope.dataPeople.paisNascimento == 'BRASIL') {
                    $ionicPopup.alert({
                        title: '<strong>Informativo</strong>',
                        template: 'Para está nacionalidade, o País de Nascimento não pode ser o BRASIL'
                    }).then(function() {
                        document.getElementById('paisNascimento').value = null;
                    });

                }
            } else if (param == 'ocupacaoCodigoCbo2002') {
                $scope.dataPeople.ocupacaoCodigoCbo2002 = string.nome;
                $scope.hideListOcupation = true;
            }
        };

        $scope.searchDataName = function(param, string) {

            if (string != null) {
                string = string.toString();
                if (param == 'codigoIbgeMunicipioNascimento') {
                    var allCountie = $scope.allCounties;
                    var outputCountie = [];
                    angular.forEach(allCountie, function(countie, key) {
                        if (countie.codigoIBGE.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            outputCountie.push(countie);
                        }
                    });
                    return outputCountie[0].nome;
                } else if (param == 'etnia') {
                    var allEtnia = $scope.allEtnias;
                    var outputEtnia = [];
                    angular.forEach(allEtnia, function(etnia, key) {
                        if (etnia.codigo.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            outputEtnia.push(etnia);
                        }
                    });
                    return outputEtnia[0].descricao;
                } else if (param == 'paisNascimento') {
                    var allPaisNascimento = $scope.allCountries;
                    var outputPaisNascimento = [];
                    angular.forEach(allPaisNascimento, function(pais, key) {
                        if (pais.codigo.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            outputPaisNascimento.push(pais);
                        }
                    });
                    return outputPaisNascimento[0].nome;
                } else if (param == 'ocupacaoCodigoCbo2002') {
                    var allOcupacaoCodigo = $scope.allOcupations;
                    var outputOcupacao = [];
                    if (string.length > 6) {
                        string = string.substring(0, 6);
                    }
                    angular.forEach(allOcupacaoCodigo, function(ocupacao, key) {
                        if (ocupacao.codigo.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            outputOcupacao.push(ocupacao);
                        }
                    });
                    return outputOcupacao[0].nome;
                } else if (param == 'codigoIbgeMunicipio') {
                    var allCountie = $scope.allCounties;
                    var outputCountie = [];
                    angular.forEach(allCountie, function(countie, key) {
                        if (countie.codigoIBGE.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            outputCountie.push(countie);
                        }
                    });
                    return outputCountie[0].nome;
                } else if (param == 'tipoLogradouroNumeroDne') {
                    var allLogradouros = $scope.allTiposLogradouros;
                    var outputTipoDeLogradouro = [];
                    angular.forEach(allLogradouros, function(logradouro, key) {
                        if (logradouro.NumeroDNE.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            outputTipoDeLogradouro.push(logradouro);
                        }
                    });
                    return outputTipoDeLogradouro[0].descricao;
                }
            } else {
                return null;
            }
        };

        $scope.dataPeople = {
            statusTermoRecusaCadastroIndividualAtencaoBasica: null
        };

        $scope.validDate = function(param, date) {

            if (date != 'undefined' && date != null) {
                if ($rootScope.newPeople != null || $rootScope.newPeople != 'undefined') {
                    if (date < $scope.dataPeople.dataNascimentoCidadao) {
                        if (param == 'dtNaturalizacao') {
                            $scope.dataPeople.dtNaturalizacao = '';
                        } else if (param == 'dtEntradaBrasil') {
                            $scope.dataPeople.dtEntradaBrasil = '';
                        } else if (param == 'dataObito') {
                            $scope.dataPeople.dataObito = '';
                        }
                        $ionicPopup.alert({
                            title: 'Data Inválida',
                            template: 'A data não pode ser anterior a data de nascimento!'
                        });
                    }
                }
            }
        };

        $scope.dataValidation = function() {
            $rootScope.newPeople = $scope.dataEditPeople;
            $scope.dataPeople = {
                id: $scope.dataEditPeople.id,
                cnsCidadao: $scope.dataEditPeople.cnsCidadao === null ? null : $scope.dataEditPeople.cnsCidadao.toString(),
                statusEhResponsavel: $scope.dataEditPeople.statusEhResponsavel,
                cnsResponsavelFamiliar: $scope.dataEditPeople.cnsResponsavelFamiliar == null ? null : $scope.dataEditPeople.cnsResponsavelFamiliar.toString(),
                CPF: $scope.dataEditPeople.CPF == null ? null : $scope.dataEditPeople.CPF.toString(),
                RG: $scope.dataEditPeople.RG == null ? null : $scope.dataEditPeople.RG.toString(),
                ComplementoRG: $scope.dataEditPeople.ComplementoRG == null ? null : $scope.dataEditPeople.ComplementoRG.toString(),
                stForaArea: $scope.dataEditPeople.stForaArea,
                microarea: $scope.dataEditPeople.microarea === null ? null : $scope.dataEditPeople.microarea.toString(),
                nomeCidadao: $scope.dataEditPeople.nomeCidadao,
                nomeSocial: $scope.dataEditPeople.nomeSocial,
                dataNascimentoCidadao: $scope.dataEditPeople.dataNascimentoCidadao === null ? null : parseInt($scope.dataEditPeople.dataNascimentoCidadao),
                sexoCidadao: $scope.dataEditPeople.sexoCidadao === null ? null : $scope.dataEditPeople.sexoCidadao.toString(),
                EstadoCivil: $scope.dataEditPeople.EstadoCivil,
                racaCorCidadao: $scope.dataEditPeople.racaCorCidadao === null ? null : $scope.dataEditPeople.racaCorCidadao.toString(),
                etnia: $scope.dataEditPeople.etnia == null ? null : $scope.dataEditPeople.etnia.toString(),
                beneficiarioBolsaFamilia: $scope.dataEditPeople.beneficiarioBolsaFamilia,
                numeroNisPisPasep: $scope.dataEditPeople.numeroNisPisPasep == null ? null : $scope.dataEditPeople.numeroNisPisPasep.toString(),
                nomeMaeCidadao: $scope.dataEditPeople.nomeMaeCidadao,
                desconheceNomeMae: $scope.dataEditPeople.desconheceNomeMae,
                nomePaiCidadao: $scope.dataEditPeople.nomePaiCidadao,
                desconheceNomePai: $scope.dataEditPeople.desconheceNomePai,
                nacionalidadeCidadao: $scope.dataEditPeople.nacionalidadeCidadao === null ? null : $scope.dataEditPeople.nacionalidadeCidadao.toString(),
                paisNascimento: $scope.dataEditPeople.paisNascimento == null ? null : $scope.dataEditPeople.paisNascimento.toString(),
                dtNaturalizacao: $scope.dataEditPeople.dtNaturalizacao == null ? null : parseInt($scope.dataEditPeople.dtNaturalizacao),
                portariaNaturalizacao: $scope.dataEditPeople.portariaNaturalizacao,
                codigoIbgeMunicipioNascimento: $scope.dataEditPeople.codigoIbgeMunicipioNascimento == null ? null : $scope.dataEditPeople.codigoIbgeMunicipioNascimento.toString(),
                dtEntradaBrasil: $scope.dataEditPeople.dtEntradaBrasil == null ? null : parseInt($scope.dataEditPeople.dtEntradaBrasil),
                telefoneCelular: $scope.dataEditPeople.telefoneCelular, //Tratar com regex
                emailCidadao: $scope.dataEditPeople.emailCidadao == null ? null : $scope.dataEditPeople.emailCidadao.toString(),
                relacaoParentescoCidadao: $scope.dataEditPeople.relacaoParentescoCidadao == null ? null : $scope.dataEditPeople.relacaoParentescoCidadao.toString(),
                ocupacaoCodigoCbo2002: $scope.dataEditPeople.ocupacaoCodigoCbo2002 == null ? null : $scope.dataEditPeople.ocupacaoCodigoCbo2002, //Verificar
                statusFrequentaEscola: $scope.dataEditPeople.statusFrequentaEscola === null ? null : $scope.dataEditPeople.statusFrequentaEscola.toString(),
                grauInstrucaoCidadao: $scope.dataEditPeople.grauInstrucaoCidadao == null ? null : $scope.dataEditPeople.grauInstrucaoCidadao.toString(),
                situacaoMercadoTrabalhoCidadao: $scope.dataEditPeople.situacaoMercadoTrabalhoCidadao == null ? null : $scope.dataEditPeople.situacaoMercadoTrabalhoCidadao.toString(),
                AdultoResponsavelresponsavelPorCrianca: $scope.dataEditPeople.AdultoResponsavelresponsavelPorCrianca,
                OutrasCriancasresponsavelPorCrianca: $scope.dataEditPeople.OutrasCriancasresponsavelPorCrianca,
                AdolescenteresponsavelPorCrianca: $scope.dataEditPeople.AdolescenteresponsavelPorCrianca,
                SozinharesponsavelPorCrianca: $scope.dataEditPeople.SozinharesponsavelPorCrianca,
                CrecheresponsavelPorCrianca: $scope.dataEditPeople.CrecheresponsavelPorCrianca,
                OutroresponsavelPorCrianca: $scope.dataEditPeople.OutroresponsavelPorCrianca,
                statusFrequentaBenzedeira: $scope.dataEditPeople.statusFrequentaBenzedeira,
                statusParticipaGrupoComunitario: $scope.dataEditPeople.statusParticipaGrupoComunitario,
                statusPossuiPlanoSaudePrivado: $scope.dataEditPeople.statusPossuiPlanoSaudePrivado,
                statusMembroPovoComunidadeTradicional: $scope.dataEditPeople.statusMembroPovoComunidadeTradicional,
                povoComunidadeTradicional: $scope.dataEditPeople.povoComunidadeTradicional == null ? null : $scope.dataEditPeople.povoComunidadeTradicional.toString(),
                statusDesejaInformarOrientacaoSexual: $scope.dataEditPeople.statusDesejaInformarOrientacaoSexual,
                orientacaoSexualCidadao: $scope.dataEditPeople.orientacaoSexualCidadao == null ? null : $scope.dataEditPeople.orientacaoSexualCidadao.toString(),
                statusDesejaInformarIdentidadeGenero: $scope.dataEditPeople.statusDesejaInformarIdentidadeGenero,
                identidadeGeneroCidadao: $scope.dataEditPeople.identidadeGeneroCidadao == null ? null : $scope.dataEditPeople.identidadeGeneroCidadao.toString(),
                statusTemAlgumaDeficiencia: $scope.dataEditPeople.statusTemAlgumaDeficiencia,
                AuditivaDeficiencias: $scope.dataEditPeople.AuditivaDeficiencias,
                VisualDeficiencias: $scope.dataEditPeople.VisualDeficiencias,
                Intelectual_CognitivaDeficiencias: $scope.dataEditPeople.Intelectual_CognitivaDeficiencias,
                FisicaDeficiencias: $scope.dataEditPeople.FisicaDeficiencias,
                OutraDeficiencias: $scope.dataEditPeople.OutraDeficiencias,
                statusEhGestante: $scope.dataEditPeople.statusEhGestante,
                maternidadeDeReferencia: $scope.dataEditPeople.maternidadeDeReferencia == null ? null : $scope.dataEditPeople.maternidadeDeReferencia.toString(),
                situacaoPeso: $scope.dataEditPeople.situacaoPeso == null ? null : $scope.dataEditPeople.situacaoPeso.toString(),
                statusEhFumante: $scope.dataEditPeople.statusEhFumante,
                statusEhDependenteAlcool: $scope.dataEditPeople.statusEhDependenteAlcool,
                statusEhDependenteOutrasDrogas: $scope.dataEditPeople.statusEhDependenteOutrasDrogas,
                statusTemHipertensaoArterial: $scope.dataEditPeople.statusTemHipertensaoArterial,
                statusTemDiabetes: $scope.dataEditPeople.statusTemDiabetes,
                statusTeveAvcDerrame: $scope.dataEditPeople.statusTeveAvcDerrame,
                statusTeveInfarto: $scope.dataEditPeople.statusTeveInfarto,
                statusTeveDoencaCardiaca: $scope.dataEditPeople.statusTeveDoencaCardiaca,
                Insuficiencia_cardiaca: $scope.dataEditPeople.Insuficiencia_cardiaca,
                Outro_Doenca_Cardiaca: $scope.dataEditPeople.Outro_Doenca_Cardiaca,
                Nao_Sabe_Doenca_Cardiaca: $scope.dataEditPeople.Nao_Sabe_Doenca_Cardiaca,
                statusTemTeveDoencasRins: $scope.dataEditPeople.statusTemTeveDoencasRins,
                Insuficiencia_renal: $scope.dataEditPeople.Insuficiencia_renal,
                Outro_Doenca_Rins: $scope.dataEditPeople.Outro_Doenca_Rins,
                Nao_Sabe_Doenca_Rins: $scope.dataEditPeople.Nao_Sabe_Doenca_Rins,
                statusTemDoencaRespiratoria: $scope.dataEditPeople.statusTemDoencaRespiratoria,
                Asma: $scope.dataEditPeople.Asma,
                DPOC_Enfisema: $scope.dataEditPeople.DPOC_Enfisema,
                Outro_Doenca_Respiratoria: $scope.dataEditPeople.Outro_Doenca_Respiratoria,
                Nao_Sabe_Doenca_Respiratoria: $scope.dataEditPeople.Nao_Sabe_Doenca_Respiratoria,
                statusTemHanseniase: $scope.dataEditPeople.statusTemHanseniase,
                statusTemTuberculose: $scope.dataEditPeople.statusTemTuberculose,
                statusTemTeveCancer: $scope.dataEditPeople.statusTemTeveCancer,
                statusTeveInternadoem12Meses: $scope.dataEditPeople.statusTeveInternadoem12Meses,
                descricaoCausaInternacaoEm12Meses: $scope.dataEditPeople.descricaoCausaInternacaoEm12Meses == null ? null : $scope.dataEditPeople.descricaoCausaInternacaoEm12Meses.toString(),
                statusDiagnosticoMental: $scope.dataEditPeople.statusDiagnosticoMental,
                statusEstaAcamado: $scope.dataEditPeople.statusEstaAcamado,
                statusEstaDomiciliado: $scope.dataEditPeople.statusEstaDomiciliado,
                statusUsaPlantasMedicinais: $scope.dataEditPeople.statusUsaPlantasMedicinais,
                descricaoPlantasMedicinaisUsadas: $scope.dataEditPeople.descricaoPlantasMedicinaisUsadas == null ? null : $scope.dataEditPeople.descricaoPlantasMedicinaisUsadas.toString(),
                statusUsaOutrasPraticasIntegrativasOuComplementares: $scope.dataEditPeople.statusUsaOutrasPraticasIntegrativasOuComplementares,
                descricaoOutraCondicao1: $scope.dataEditPeople.descricaoOutraCondicao1 == null ? null : $scope.dataEditPeople.descricaoOutraCondicao1.toString(),
                descricaoOutraCondicao2: $scope.dataEditPeople.descricaoOutraCondicao2 == null ? null : $scope.dataEditPeople.descricaoOutraCondicao2.toString(),
                descricaoOutraCondicao3: $scope.dataEditPeople.descricaoOutraCondicao3 == null ? null : $scope.dataEditPeople.descricaoOutraCondicao3.toString(),
                statusSituacaoRua: $scope.dataEditPeople.statusSituacaoRua,
                tempoSituacaoRua: $scope.dataEditPeople.tempoSituacaoRua == null ? null : $scope.dataEditPeople.tempoSituacaoRua.toString(),
                statusRecebeBeneficio: $scope.dataEditPeople.statusRecebeBeneficio,
                statusPossuiReferenciaFamiliar: $scope.dataEditPeople.statusPossuiReferenciaFamiliar,
                quantidadeAlimentacoesAoDiaSituacaoRua: $scope.dataEditPeople.quantidadeAlimentacoesAoDiaSituacaoRua == null ? null : $scope.dataEditPeople.quantidadeAlimentacoesAoDiaSituacaoRua.toString(),
                Restaurante_popular: $scope.dataEditPeople.Restaurante_popular,
                Doacao_grupo_religioso: $scope.dataEditPeople.Doacao_grupo_religioso,
                Doacao_restaurante: $scope.dataEditPeople.Doacao_restaurante,
                Doacao_popular: $scope.dataEditPeople.Doacao_popular,
                Outros_origemAlimentoSituacaoRua: $scope.dataEditPeople.Outros_origemAlimentoSituacaoRua === null ? null : $scope.dataEditPeople.Outros_origemAlimentoSituacaoRua.toString(),
                statusAcompanhadoPorOutraInstituicao: $scope.dataEditPeople.statusAcompanhadoPorOutraInstituicao,
                outraInstituicaoQueAcompanha: $scope.dataEditPeople.outraInstituicaoQueAcompanha == null ? null : $scope.dataEditPeople.outraInstituicaoQueAcompanha.toString(),
                statusVisitaFamiliarFrequentemente: $scope.dataEditPeople.statusVisitaFamiliarFrequentemente,
                grauParentescoFamiliarFrequentado: $scope.dataEditPeople.grauParentescoFamiliarFrequentado == null ? null : $scope.dataEditPeople.grauParentescoFamiliarFrequentado.toString(),
                statusTemAcessoHigienePessoalSituacaoRua: $scope.dataEditPeople.statusTemAcessoHigienePessoalSituacaoRua,
                Banho: $scope.dataEditPeople.Banho,
                Acesso_a_sanitario: $scope.dataEditPeople.Acesso_a_sanitario,
                Higiene_bucal: $scope.dataEditPeople.Higiene_bucal,
                Outros_higienePessoalSituacaoRua: $scope.dataEditPeople.Outros_higienePessoalSituacaoRua,
                motivoSaidaCidadao: $scope.dataEditPeople.motivoSaidaCidadao == null ? null : $scope.dataEditPeople.motivoSaidaCidadao.toString(),
                dataObito: $scope.dataEditPeople.dataObito == null ? null : parseInt($scope.dataEditPeople.dataObito),
                numeroDO: $scope.dataEditPeople.numeroDO == null ? null : $scope.dataEditPeople.numeroDO.toString(),
                statusTermoRecusaCadastroIndividualAtencaoBasica: $scope.dataEditPeople.statusTermoRecusaCadastroIndividualAtencaoBasica == null ? null : $scope.dataEditPeople.statusTermoRecusaCadastroIndividualAtencaoBasica,
                observacao: $scope.dataEditPeople.observacao
            };

            $scope.dataPeople.etnia = $scope.searchDataName('etnia', $scope.dataPeople.etnia);
            $scope.dataPeople.paisNascimento = $scope.searchDataName('paisNascimento', $scope.dataPeople.paisNascimento);
            $scope.dataPeople.codigoIbgeMunicipioNascimento = $scope.searchDataName('codigoIbgeMunicipioNascimento', $scope.dataPeople.codigoIbgeMunicipioNascimento);
            $scope.dataPeople.ocupacaoCodigoCbo2002 = $scope.searchDataName('ocupacaoCodigoCbo2002', $scope.dataPeople.ocupacaoCodigoCbo2002);
        };

        $scope.validationForm = function(param, dado) {

            if (param == 'statusEhResponsavel') {
                if (dado == 1) {
                    $scope.disabledInput.cnsResponsavelFamiliar = true;
                    $scope.dataPeople.cnsResponsavelFamiliar = null;
                    $scope.requiredInput.cnsResponsavelFamiliar = false;
                } else {
                    var cns = typeof $scope.dataPeople.cnsCidadao === 'undefined' ? null : $scope.dataPeople.cnsCidadao;

                    PeopleService.selectThereAreChildrenForResponsible(cns)
                        .then(function(response) {
                            if (response[0].NumberChilds == 0) {
                                $scope.disabledInput.cnsResponsavelFamiliar = false;
                                $scope.requiredInput.cnsResponsavelFamiliar = true;
                                $scope.removeTheUsersAdress(cns);
                            } else {
                                $scope.dataPeople.statusEhResponsavel = 1;
                                $ionicPopup.alert({
                                    title: '<strong>Informativo</strong>',
                                    template: 'Não é possível modificar o status pois existem registros que estão vinculados a este responsável'
                                });
                            }
                        })
                        .catch(function(err) {
                            console.log(err);
                        });
                }
            } else if (param == 'beneficiarioBolsaFamilia') {
                if (dado == 1) {
                    $scope.requiredInput.numeroNisPisPasep = true;
                } else {
                    $scope.requiredInput.numeroNisPisPasep = false;
                }
            } else if (param == 'stForaArea') {
                if (dado == 1) {
                    $scope.disabledInput.microarea = true;
                    $scope.requiredInput.microarea = false;
                    $scope.dataPeople.microarea = null;
                } else {
                    $scope.disabledInput.microarea = false;
                    $scope.requiredInput.microarea = true;
                }
            } else if (param == 'racaCorCidadao') {
                if (dado == 5) {
                    $scope.disabledInput.etnia = false;
                    $scope.requiredInput.etnia = true;
                } else {
                    $scope.disabledInput.etnia = true;
                    $scope.requiredInput.etnia = false;
                    $scope.dataPeople.etnia = null;
                }
            } else if (param == 'desconheceNomeMae') {
                if (dado == 1) {
                    $scope.disabledInput.nomeMaeCidadao = true;
                    $scope.dataPeople.nomeMaeCidadao = null;
                    $scope.requiredInput.nomeMaeCidadao = false;
                } else {
                    $scope.disabledInput.nomeMaeCidadao = false;
                    $scope.requiredInput.nomeMaeCidadao = true;
                }
            } else if (param == 'desconheceNomePai') {
                if (dado == 1) {
                    $scope.disabledInput.nomePaiCidadao = true;
                    $scope.dataPeople.nomePaiCidadao = null;
                    $scope.requiredInput.nomePaiCidadao = false;
                } else {
                    $scope.disabledInput.nomePaiCidadao = false;
                    $scope.requiredInput.nomePaiCidadao = true;
                }
            } else if (param == 'nacionalidadeCidadao') {
                if (dado == 1) {
                    $scope.dataPeople.paisNascimento = 'BRASIL';
                    $scope.disabledInput.paisNascimento = true;
                    $scope.requiredInput.paisNascimento = true;
                    $scope.disabledInput.portariaNaturalizacao = true;
                    $scope.requiredInput.portariaNaturalizacao = false;
                    $scope.dataPeople.portariaNaturalizacao = null;
                    $scope.disabledInput.dtNaturalizacao = true;
                    $scope.requiredInput.dtNaturalizacao = false;
                    $scope.dataPeople.dtNaturalizacao = null;
                    $scope.disabledInput.codigoIbgeMunicipioNascimento = false;
                    // $scope.requiredInput.codigoIbgeMunicipioNascimento = true;
                    $scope.disabledInput.dtEntradaBrasil = true;
                    $scope.requiredInput.dtEntradaBrasil = false;
                    $scope.dataPeople.dtEntradaBrasil = null;
                } else if (dado == 2) {
                    $scope.disabledInput.dtNaturalizacao = false;
                    $scope.requiredInput.dtNaturalizacao = true;
                    $scope.disabledInput.portariaNaturalizacao = false;
                    $scope.requiredInput.portariaNaturalizacao = true;
                    $scope.requiredInput.paisNascimento = false;
                    $scope.disabledInput.paisNascimento = true;
                    $scope.dataPeople.paisNascimento = null;
                    $scope.disabledInput.codigoIbgeMunicipioNascimento = true;
                    $scope.requiredInput.codigoIbgeMunicipioNascimento = false;
                    $scope.dataPeople.codigoIbgeMunicipioNascimento = null;
                    $scope.disabledInput.dtEntradaBrasil = true;
                    $scope.requiredInput.dtEntradaBrasil = false;
                    $scope.dataPeople.dtEntradaBrasil = null;

                } else if (dado == 3) {
                    $scope.dataPeople.paisNascimento = null;
                    $scope.requiredInput.paisNascimento = true;
                    $scope.disabledInput.paisNascimento = false;
                    $scope.disabledInput.dtNaturalizacao = true;
                    $scope.requiredInput.dtNaturalizacao = false;
                    $scope.dataPeople.dtNaturalizacao = null;
                    $scope.disabledInput.portariaNaturalizacao = true;
                    $scope.requiredInput.portariaNaturalizacao = false;
                    $scope.dataPeople.portariaNaturalizacao = null;
                    $scope.disabledInput.codigoIbgeMunicipioNascimento = true;
                    $scope.requiredInput.codigoIbgeMunicipioNascimento = false;
                    $scope.dataPeople.codigoIbgeMunicipioNascimento = null;
                    $scope.disabledInput.dtEntradaBrasil = false;
                    $scope.requiredInput.dtEntradaBrasil = true;
                }
            } else if (param == 'statusMembroPovoComunidadeTradicional') {
                if (dado == 1) {
                    $scope.disabledInput.povoComunidadeTradicional = false;
                } else {
                    $scope.disabledInput.povoComunidadeTradicional = true;
                    $scope.dataPeople.povoComunidadeTradicional = null;
                }
            } else if (param == 'statusDesejaInformarOrientacaoSexual') {
                if (dado == 1) {
                    $scope.disabledInput.orientacaoSexualCidadao = false;
                } else {
                    $scope.disabledInput.orientacaoSexualCidadao = true;
                    $scope.dataPeople.orientacaoSexualCidadao = null;
                }
            } else if (param == 'statusDesejaInformarIdentidadeGenero') {
                if (dado == 1) {
                    $scope.disabledInput.identidadeGeneroCidadao = false;
                } else {
                    $scope.disabledInput.identidadeGeneroCidadao = true;
                    $scope.dataPeople.identidadeGeneroCidadao = null;
                }
            } else if (param == 'statusTemAlgumaDeficiencia') {
                if (dado == 1) {
                    $scope.disabledInput.AuditivaDeficiencias = false;
                    $scope.disabledInput.VisualDeficiencias = false;
                    $scope.disabledInput.Intelectual_CognitivaDeficiencias = false;
                    $scope.disabledInput.FisicaDeficiencias = false;
                    $scope.disabledInput.OutraDeficiencias = false;
                    $scope.requiredInput.AuditivaDeficiencias = true;
                    $scope.requiredInput.VisualDeficiencias = true;
                    $scope.requiredInput.Intelectual_CognitivaDeficiencias = true;
                    $scope.requiredInput.FisicaDeficiencias = true;
                    $scope.requiredInput.OutraDeficiencias = true;
                } else {
                    $scope.disabledInput.AuditivaDeficiencias = true;
                    $scope.disabledInput.VisualDeficiencias = true;
                    $scope.disabledInput.Intelectual_CognitivaDeficiencias = true;
                    $scope.disabledInput.FisicaDeficiencias = true;
                    $scope.disabledInput.OutraDeficiencias = true;
                    $scope.requiredInput.AuditivaDeficiencias = false;
                    $scope.requiredInput.VisualDeficiencias = false;
                    $scope.requiredInput.Intelectual_CognitivaDeficiencias = false;
                    $scope.requiredInput.FisicaDeficiencias = false;
                    $scope.requiredInput.OutraDeficiencias = false;
                    $scope.dataPeople.AuditivaDeficiencias = null;
                    $scope.dataPeople.VisualDeficiencias = null;
                    $scope.dataPeople.Intelectual_CognitivaDeficiencias = null;
                    $scope.dataPeople.FisicaDeficiencias = null;
                    $scope.dataPeople.OutraDeficiencias = null;
                }
            } else if (param == 'statusEhGestante') {
                if (dado == 1) {
                    $scope.disabledInput.maternidadeDeReferencia = false;
                } else {
                    $scope.disabledInput.maternidadeDeReferencia = true;
                    $scope.dataPeople.maternidadeDeReferencia = null;
                }
            } else if (param == 'statusTeveDoencaCardiaca') {
                if (dado == 1) {
                    $scope.disabledInput.Insuficiencia_cardiaca = false;
                    $scope.requiredInput.Insuficiencia_cardiaca = true;
                    $scope.disabledInput.Outro_Doenca_Cardiaca = false;
                    $scope.requiredInput.Outro_Doenca_Cardiaca = true;
                    $scope.disabledInput.Nao_Sabe_Doenca_Cardiaca = false;
                    $scope.requiredInput.Nao_Sabe_Doenca_Cardiaca = true;
                } else {
                    $scope.disabledInput.Insuficiencia_cardiaca = true;
                    $scope.requiredInput.Insuficiencia_cardiaca = false;
                    $scope.dataPeople.Insuficiencia_cardiaca = null;
                    $scope.disabledInput.Outro_Doenca_Cardiaca = true;
                    $scope.requiredInput.Outro_Doenca_Cardiaca = false;
                    $scope.dataPeople.Outro_Doenca_Cardiaca = null;
                    $scope.disabledInput.Nao_Sabe_Doenca_Cardiaca = true;
                    $scope.requiredInput.Nao_Sabe_Doenca_Cardiaca = false;
                    $scope.dataPeople.Nao_Sabe_Doenca_Cardiaca = null;
                }
            } else if (param == 'statusTemTeveDoencasRins') {
                if (dado == 1) {
                    $scope.disabledInput.Insuficiencia_renal = false;
                    $scope.requiredInput.Insuficiencia_renal = true;
                    $scope.disabledInput.Outro_Doenca_Rins = false;
                    $scope.requiredInput.Outro_Doenca_Rins = true;
                    $scope.disabledInput.Nao_Sabe_Doenca_Rins = false;
                    $scope.requiredInput.Nao_Sabe_Doenca_Rins = true;
                } else {
                    $scope.disabledInput.Insuficiencia_renal = true;
                    $scope.requiredInput.Insuficiencia_renal = false;
                    $scope.dataPeople.Insuficiencia_renal = null;
                    $scope.disabledInput.Outro_Doenca_Rins = true;
                    $scope.requiredInput.Outro_Doenca_Rins = false;
                    $scope.dataPeople.Outro_Doenca_Rins = null;
                    $scope.disabledInput.Nao_Sabe_Doenca_Rins = true;
                    $scope.requiredInput.Nao_Sabe_Doenca_Rins = false;
                    $scope.dataPeople.Nao_Sabe_Doenca_Rins = null;
                }
            } else if (param == 'statusTemDoencaRespiratoria') {
                if (dado == 1) {
                    $scope.disabledInput.Asma = false;
                    $scope.requiredInput.Asma = true;
                    $scope.disabledInput.DPOC_Enfisema = false;
                    $scope.requiredInput.DPOC_Enfisema = true;
                    $scope.disabledInput.Outro_Doenca_Respiratoria = false;
                    $scope.requiredInput.Outro_Doenca_Respiratoria = true;
                    $scope.disabledInput.Nao_Sabe_Doenca_Respiratoria = false;
                    $scope.requiredInput.Nao_Sabe_Doenca_Respiratoria = true;
                } else {
                    $scope.disabledInput.Asma = true;
                    $scope.requiredInput.Asma = false;
                    $scope.dataPeople.Asma = null;
                    $scope.disabledInput.DPOC_Enfisema = true;
                    $scope.requiredInput.DPOC_Enfisema = false;
                    $scope.dataPeople.DPOC_Enfisema = null;
                    $scope.disabledInput.Outro_Doenca_Respiratoria = true;
                    $scope.requiredInput.Outro_Doenca_Respiratoria = false;
                    $scope.dataPeople.Outro_Doenca_Respiratoria = null;
                    $scope.disabledInput.Nao_Sabe_Doenca_Respiratoria = true;
                    $scope.requiredInput.Nao_Sabe_Doenca_Respiratoria = false;
                    $scope.dataPeople.Nao_Sabe_Doenca_Respiratoria = null;

                }
            } else if (param == 'statusTeveInternadoem12Meses') {
                if (dado == 1) {
                    $scope.requiredInput.descricaoCausaInternacaoEm12Meses = true;
                    $scope.disabledInput.descricaoCausaInternacaoEm12Meses = false;
                } else {
                    $scope.requiredInput.descricaoCausaInternacaoEm12Meses = false;
                    $scope.disabledInput.descricaoCausaInternacaoEm12Meses = true;
                    $scope.dataPeople.descricaoCausaInternacaoEm12Meses = null;
                }
            } else if (param == 'statusUsaPlantasMedicinais') {
                if (dado == 1) {
                    $scope.disabledInput.descricaoPlantasMedicinaisUsadas = false;
                } else {
                    $scope.disabledInput.descricaoPlantasMedicinaisUsadas = true;
                    $scope.dataPeople.descricaoPlantasMedicinaisUsadas = null;
                }
            } else if (param == 'statusSituacaoRua') {
                if (dado == 1) {
                    $scope.disabledInput.tempoSituacaoRua = false;
                    $scope.disabledInput.statusRecebeBeneficio = false;
                    $scope.disabledInput.statusPossuiReferenciaFamiliar = false;
                    $scope.disabledInput.quantidadeAlimentacoesAoDiaSituacaoRua = false;
                    $scope.disabledInput.Restaurante_popular = false;
                    $scope.disabledInput.Doacao_grupo_religioso = false;
                    $scope.disabledInput.Doacao_restaurante = false;
                    $scope.disabledInput.Doacao_popular = false;
                    $scope.disabledInput.Outros_origemAlimentoSituacaoRua = false;
                    $scope.requiredInput.Restaurante_popular = true;
                    $scope.requiredInput.Doacao_grupo_religioso = true;
                    $scope.requiredInput.Doacao_restaurante = true;
                    $scope.requiredInput.Doacao_popular = true;
                    $scope.requiredInput.Outros_origemAlimentoSituacaoRua = false;
                    $scope.disabledInput.statusAcompanhadoPorOutraInstituicao = false;
                    $scope.disabledInput.statusVisitaFamiliarFrequentemente = false;
                    $scope.disabledInput.statusTemAcessoHigienePessoalSituacaoRua = false;
                } else {
                    $scope.disabledInput.tempoSituacaoRua = true;
                    $scope.dataPeople.tempoSituacaoRua = null;
                    $scope.disabledInput.statusRecebeBeneficio = true;
                    $scope.dataPeople.statusRecebeBeneficio = null;
                    $scope.disabledInput.statusPossuiReferenciaFamiliar = true;
                    $scope.dataPeople.statusPossuiReferenciaFamiliar = null;
                    $scope.disabledInput.quantidadeAlimentacoesAoDiaSituacaoRua = true;
                    $scope.dataPeople.quantidadeAlimentacoesAoDiaSituacaoRua = null;
                    $scope.disabledInput.Restaurante_popular = true;
                    $scope.disabledInput.Doacao_grupo_religioso = true;
                    $scope.disabledInput.Doacao_restaurante = true;
                    $scope.disabledInput.Doacao_popular = true;
                    $scope.disabledInput.Outros_origemAlimentoSituacaoRua = true;
                    $scope.requiredInput.Restaurante_popular = false;
                    $scope.requiredInput.Doacao_grupo_religioso = false;
                    $scope.requiredInput.Doacao_restaurante = false;
                    $scope.requiredInput.Doacao_popular = false;
                    $scope.requiredInput.Outros_origemAlimentoSituacaoRua = false;
                    $scope.dataPeople.Restaurante_popular = null;
                    $scope.dataPeople.Doacao_grupo_religioso = null;
                    $scope.dataPeople.Doacao_restaurante = null;
                    $scope.dataPeople.Doacao_popular = null;
                    $scope.dataPeople.Outros_origemAlimentoSituacaoRua = null;
                    $scope.disabledInput.statusAcompanhadoPorOutraInstituicao = true;
                    $scope.dataPeople.statusAcompanhadoPorOutraInstituicao = null;
                    $scope.disabledInput.statusVisitaFamiliarFrequentemente = true;
                    $scope.dataPeople.statusVisitaFamiliarFrequentemente = null;
                    $scope.disabledInput.grauParentescoFamiliarFrequentado = true;
                    $scope.requiredInput.grauParentescoFamiliarFrequentado = false;
                    $scope.dataPeople.grauParentescoFamiliarFrequentado = null;
                    $scope.disabledInput.statusTemAcessoHigienePessoalSituacaoRua = true;
                    $scope.dataPeople.statusTemAcessoHigienePessoalSituacaoRua = null;
                    $scope.disabledInput.outraInstituicaoQueAcompanha = true;
                    $scope.dataPeople.outraInstituicaoQueAcompanha = null;
                    $scope.disabledInput.Banho = true;
                    $scope.disabledInput.Acesso_a_sanitario = true;
                    $scope.disabledInput.Higiene_bucal = true;
                    $scope.disabledInput.Outros_higienePessoalSituacaoRua = true;
                    $scope.requiredInput.Banho = false;
                    $scope.requiredInput.Acesso_a_sanitario = false;
                    $scope.requiredInput.Higiene_bucal = false;
                    $scope.requiredInput.Outros_higienePessoalSituacaoRua = false;
                    $scope.dataPeople.Banho = null;
                    $scope.dataPeople.Acesso_a_sanitario = null;
                    $scope.dataPeople.Higiene_bucal = null;
                    $scope.dataPeople.Outros_higienePessoalSituacaoRua = null;
                }
            } else if (param == 'statusAcompanhadoPorOutraInstituicao') {
                if (dado == 1) {
                    $scope.disabledInput.outraInstituicaoQueAcompanha = false;
                } else {
                    $scope.disabledInput.outraInstituicaoQueAcompanha = true;
                    $scope.dataPeople.outraInstituicaoQueAcompanha = null;
                }
            } else if (param == 'statusVisitaFamiliarFrequentemente') {
                if (dado == 1) {
                    $scope.disabledInput.grauParentescoFamiliarFrequentado = false;
                    $scope.requiredInput.grauParentescoFamiliarFrequentado = true;
                } else {
                    $scope.disabledInput.grauParentescoFamiliarFrequentado = true;
                    $scope.requiredInput.grauParentescoFamiliarFrequentado = false;
                    $scope.dataPeople.grauParentescoFamiliarFrequentado = null;
                }
            } else if (param == 'statusTemAcessoHigienePessoalSituacaoRua') {
                if (dado == 1) {
                    $scope.disabledInput.Banho = false;
                    $scope.disabledInput.Acesso_a_sanitario = false;
                    $scope.disabledInput.Higiene_bucal = false;
                    $scope.disabledInput.Outros_higienePessoalSituacaoRua = false;
                    $scope.requiredInput.Banho = true;
                    $scope.requiredInput.Acesso_a_sanitario = true;
                    $scope.requiredInput.Higiene_bucal = true;
                    $scope.requiredInput.Outros_higienePessoalSituacaoRua = true;
                } else {
                    $scope.disabledInput.Banho = true;
                    $scope.disabledInput.Acesso_a_sanitario = true;
                    $scope.disabledInput.Higiene_bucal = true;
                    $scope.disabledInput.Outros_higienePessoalSituacaoRua = true;
                    $scope.requiredInput.Banho = false;
                    $scope.requiredInput.Acesso_a_sanitario = false;
                    $scope.requiredInput.Higiene_bucal = false;
                    $scope.requiredInput.Outros_higienePessoalSituacaoRua = false;
                    $scope.dataPeople.Banho = null;
                    $scope.dataPeople.Acesso_a_sanitario = null;
                    $scope.dataPeople.Higiene_bucal = null;
                    $scope.dataPeople.Outros_higienePessoalSituacaoRua = null;
                }
            } else if (param == 'motivoSaidaCidadao') {
                if (dado == 135) {
                    $scope.disabledInput.dataObito = false;
                    $scope.requiredInput.dataObito = true;
                    $scope.disabledInput.numeroDO = false;
                } else {
                    $scope.disabledInput.dataObito = true;
                    $scope.requiredInput.dataObito = false;
                    $scope.dataPeople.dataObito = null;
                    $scope.disabledInput.numeroDO = true;
                    $scope.dataPeople.numeroDO = null;
                }
            }

        };

        $scope.validationDataDictionary = function(objeto) {

            $scope.requiredInput = {
                // Primeira tela
                microarea: true,
                nomeCidadao: true,
                dataNascimentoCidadao: true,
                sexoCidadao: true,
                racaCorCidadao: true,
                numeroNisPisPasep: false,
                etnia: false,
                nomeMaeCidadao: true,
                nomePaiCidadao: true,
                nacionalidadeCidadao: true,
                paisNascimento: false,
                dtNaturalizacao: false,
                portariaNaturalizacao: false,
                codigoIbgeMunicipioNascimento: false,
                dtEntradaBrasil: false,

                // Segunda tela
                relacaoParentescoCidadao: false,
                statusFrequentaEscola: true,
                statusTemAlgumaDeficiencia: true,
                AuditivaDeficiencias: false,
                VisualDeficiencias: false,
                Intelectual_CognitivaDeficiencias: false,
                FisicaDeficiencias: false,
                OutraDeficiencias: false,

                // Terceira tela
                Insuficiencia_cardiaca: false,
                Outro_Doenca_Cardiaca: false,
                Nao_Sabe_Doenca_Cardiaca: false,
                Insuficiencia_renal: false,
                Outro_Doenca_Rins: false,
                Nao_Sabe_Doenca_: false,
                Asma: false,
                DPOC_Enfisema: false,
                Outro_Doenca_Respiratoria: false,
                Nao_Sabe_Doenca_Respiratoria: false,
                descricaoCausaInternacaoEm12Meses: false,

                // Quarta tela
                statusSituacaoRua: true,
                Restaurante_popular: false,
                Doacao_grupo_religioso: false,
                Doacao_restaurante: false,
                Doacao_popular: false,
                Outros_origemAlimentoSituacaoRua: false,
                Banho: false,
                Acesso_a_sanitario: false,
                Higiene_bucal: false,
                Outros_higienePessoalSituacaoRua: false,

                //Quinta Tela
                dataObito: false

            };

            $scope.disabledInput = {
                // Primeira tela
                cnsResponsavelFamiliar: false,
                microarea: false,
                etnia: true,
                nomeMaeCidadao: false,
                nomePaiCidadao: false,
                paisNascimento: true,
                dtNaturalizacao: true,
                portariaNaturalizacao: true,
                codigoIbgeMunicipioNascimento: true,
                dtEntradaBrasil: true,

                // Segunda tela
                relacaoParentescoCidadao: false,
                AdultoResponsavelresponsavelPorCrianca: false,
                OutrasCriancasresponsavelPorCrianca: false,
                AdolescenteresponsavelPorCrianca: false,
                SozinharesponsavelPorCrianca: false,
                CrecheresponsavelPorCrianca: false,
                OutroresponsavelPorCrianca: false,
                povoComunidadeTradicional: true,
                identidadeGeneroCidadao: true,
                orientacaoSexualCidadao: true,
                AuditivaDeficiencias: true,
                VisualDeficiencias: true,
                Intelectual_CognitivaDeficiencias: true,
                FisicaDeficiencias: true,
                OutraDeficiencias: true,

                // Terceira tela
                statusEhGestante: false,
                maternidadeDeReferencia: true,
                Insuficiencia_cardiaca: true,
                Outro_Doenca_Cardiaca: true,
                Nao_Sabe_Doenca_Cardiaca: true,
                Insuficiencia_renal: true,
                Outro_Doenca_Rins: true,
                Nao_Sabe_Doenca_Rins: true,
                Asma: true,
                DPOC_Enfisema: true,
                Outro_Doenca_Respiratoria: true,
                Nao_Sabe_Doenca_Respiratoria: true,
                descricaoCausaInternacaoEm12Meses: true,
                descricaoPlantasMedicinaisUsadas: true,

                // Quarta tela
                tempoSituacaoRua: true,
                statusRecebeBeneficio: true,
                statusPossuiReferenciaFamiliar: true,
                quantidadeAlimentacoesAoDiaSituacaoRua: true,
                Restaurante_popular: true,
                Doacao_grupo_religioso: true,
                Doacao_restaurante: true,
                Doacao_popular: true,
                Outros_origemAlimentoSituacaoRua: true,
                statusAcompanhadoPorOutraInstituicao: true,
                outraInstituicaoQueAcompanha: true,
                statusVisitaFamiliarFrequentemente: true,
                grauParentescoFamiliarFrequentado: true,
                statusTemAcessoHigienePessoalSituacaoRua: true,
                Banho: true,
                Acesso_a_sanitario: true,
                Higiene_bucal: true,
                Outros_higienePessoalSituacaoRua: true,

                //Quinta Tela
                dataObito: true,
                numeroDO: true
            };

            if (objeto.statusEhResponsavel == 1) {
                $scope.disabledInput.cnsResponsavelFamiliar = true;
                $scope.requiredInput.cnsResponsavelFamiliar = false;
                $scope.requiredInput.relacaoParentescoCidadao = false;
                $scope.disabledInput.relacaoParentescoCidadao = true;
                $scope.dataPeople.relacaoParentescoCidadao = null;
                $scope.disabledInput.AdultoResponsavelresponsavelPorCrianca = true;
                $scope.disabledInput.OutrasCriancasresponsavelPorCrianca = true;
                $scope.disabledInput.AdolescenteresponsavelPorCrianca = true;
                $scope.disabledInput.SozinharesponsavelPorCrianca = true;
                $scope.disabledInput.CrecheresponsavelPorCrianca = true;
                $scope.disabledInput.OutroresponsavelPorCrianca = true;
            } else {
                $scope.requiredInput.relacaoParentescoCidadao = false;
                $scope.disabledInput.relacaoParentescoCidadao = false;
                $scope.disabledInput.cnsResponsavelFamiliar = false;
                $scope.requiredInput.cnsResponsavelFamiliar = true;
            }

            if (objeto.stForaArea == 1) {
                $scope.requiredInput.microarea = false;
                $scope.disabledInput.microarea = true;
            }

            if (objeto.racaCorCidadao == 5) {
                $scope.requiredInput.etnia = true;
                $scope.disabledInput.etnia = false;
            }

            if (objeto.desconheceNomeMae == 1) {
                $scope.requiredInput.nomeMaeCidadao = false;
                $scope.disabledInput.nomeMaeCidadao = true;
            }
            if (objeto.desconheceNomePai == 1) {
                $scope.requiredInput.nomePaiCidadao = false;
                $scope.disabledInput.nomePaiCidadao = true;
            }

            if (objeto.nacionalidadeCidadao == 3) {
                $scope.requiredInput.paisNascimento = true;
                $scope.disabledInput.paisNascimento = false;
                $scope.requiredInput.dtEntradaBrasil = true;
                $scope.disabledInput.dtEntradaBrasil = false;
            } else if (objeto.nacionalidadeCidadao == 1) {
                $scope.requiredInput.paisNascimento = false;
                $scope.disabledInput.paisNascimento = true;
                $scope.requiredInput.codigoIbgeMunicipioNascimento = true;
                $scope.disabledInput.codigoIbgeMunicipioNascimento = false;
            } else if (objeto.nacionalidadeCidadao == 2) {
                $scope.requiredInput.dtNaturalizacao = true;
                $scope.disabledInput.dtNaturalizacao = false;
                $scope.requiredInput.portariaNaturalizacao = true;
                $scope.disabledInput.portariaNaturalizacao = false;
            }

            if (SystemService.isTenYears(objeto.dataNascimentoCidadao)) {
                $scope.disabledInput.AdultoResponsavelresponsavelPorCrianca = true;
                $scope.disabledInput.OutrasCriancasresponsavelPorCrianca = true;
                $scope.disabledInput.AdolescenteresponsavelPorCrianca = true;
                $scope.disabledInput.SozinharesponsavelPorCrianca = true;
                $scope.disabledInput.CrecheresponsavelPorCrianca = true;
                $scope.disabledInput.OutroresponsavelPorCrianca = true;
                $scope.dataPeople.AdultoResponsavelresponsavelPorCrianca = null;
                $scope.dataPeople.OutrasCriancasresponsavelPorCrianca = null;
                $scope.dataPeople.AdolescenteresponsavelPorCrianca = null;
                $scope.dataPeople.SozinharesponsavelPorCrianca = null;
                $scope.dataPeople.CrecheresponsavelPorCrianca = null;
                $scope.dataPeople.OutroresponsavelPorCrianca = null;
            }

            if (objeto.statusMembroPovoComunidadeTradicional == 1) {
                $scope.disabledInput.povoComunidadeTradicional = false;
            }

            if (objeto.statusDesejaInformarOrientacaoSexual == 1) {
                $scope.disabledInput.orientacaoSexualCidadao = false;
            }

            if (objeto.statusDesejaInformarIdentidadeGenero == 1) {
                $scope.disabledInput.identidadeGeneroCidadao = false;
            }

            if (objeto.statusTemAlgumaDeficiencia == 1) {
                $scope.requiredInput.AuditivaDeficiencias = true;
                $scope.requiredInput.VisualDeficiencias = true;
                $scope.requiredInput.Intelectual_CognitivaDeficiencias = true;
                $scope.requiredInput.FisicaDeficiencias = true;
                $scope.requiredInput.OutraDeficiencias = true;
                $scope.disabledInput.AuditivaDeficiencias = false;
                $scope.disabledInput.VisualDeficiencias = false;
                $scope.disabledInput.Intelectual_CognitivaDeficiencias = false;
                $scope.disabledInput.FisicaDeficiencias = false;
                $scope.disabledInput.OutraDeficiencias = false;
            }

            if (objeto.sexoCidadao != 1) {
                $scope.disabledInput.statusEhGestante = true;
            } else if (SystemService.isNineSixty(objeto.dataNascimentoCidadao)) {
                $scope.disabledInput.statusEhGestante = true;
            }

            if (objeto.statusEhGestante == 1) {
                $scope.disabledInput.maternidadeDeReferencia = false;
            }

            if (objeto.statusTeveDoencaCardiaca == 1) {
                $scope.disabledInput.Insuficiencia_cardiaca = false;
                $scope.requiredInput.Insuficiencia_cardiaca = true;
                $scope.disabledInput.Outro_Doenca_Cardiaca = false;
                $scope.requiredInput.Outro_Doenca_Cardiaca = true;
                $scope.disabledInput.Nao_Sabe_Doenca_Cardiaca = false;
                $scope.requiredInput.Nao_Sabe_Doenca_Cardiaca = true;
            }

            if (objeto.statusTemTeveDoencasRins == 1) {
                $scope.disabledInput.Insuficiencia_renal = false;
                $scope.requiredInput.Insuficiencia_renal = true;
                $scope.disabledInput.Outro_Doenca_Rins = false;
                $scope.requiredInput.Outro_Doenca_Rins = true;
                $scope.disabledInput.Nao_Sabe_Doenca_Rins = false;
                $scope.requiredInput.Nao_Sabe_Doenca_Rins = true;
            }

            if (objeto.statusTemDoencaRespiratoria == 1) {
                $scope.disabledInput.Asma = false;
                $scope.requiredInput.Asma = true;
                $scope.disabledInput.DPOC_Enfisema = false;
                $scope.requiredInput.DPOC_Enfisema = true;
                $scope.disabledInput.Outro_Doenca_Respiratoria = false;
                $scope.requiredInput.Outro_Doenca_Respiratoria = true;
                $scope.disabledInput.Nao_Sabe_Doenca_Respiratoria = false;
                $scope.requiredInput.Nao_Sabe_Doenca_Respiratoria = true;
            }

            if (objeto.statusTeveInternadoem12Meses == 1) {
                $scope.requiredInput.descricaoCausaInternacaoEm12Meses = true;
                $scope.disabledInput.descricaoCausaInternacaoEm12Meses = false;
            }

            if (objeto.statusUsaPlantasMedicinais == 1) {
                $scope.disabledInput.descricaoPlantasMedicinaisUsadas = false;
            }

            if (objeto.statusSituacaoRua == 1) {
                $scope.disabledInput.tempoSituacaoRua = false;
                $scope.disabledInput.statusRecebeBeneficio = false;
                $scope.disabledInput.statusPossuiReferenciaFamiliar = false;
                $scope.disabledInput.quantidadeAlimentacoesAoDiaSituacaoRua = false;
                $scope.disabledInput.Restaurante_popular = false;
                $scope.disabledInput.Doacao_grupo_religioso = false;
                $scope.disabledInput.Doacao_restaurante = false;
                $scope.disabledInput.Doacao_popular = false;
                $scope.disabledInput.Outros_origemAlimentoSituacaoRua = false;
                $scope.requiredInput.Restaurante_popular = true;
                $scope.requiredInput.Doacao_grupo_religioso = true;
                $scope.requiredInput.Doacao_restaurante = true;
                $scope.requiredInput.Doacao_popular = true;
                $scope.requiredInput.Outros_origemAlimentoSituacaoRua = false;
                $scope.disabledInput.statusAcompanhadoPorOutraInstituicao = false;
                $scope.disabledInput.statusVisitaFamiliarFrequentemente = false;
                $scope.disabledInput.statusTemAcessoHigienePessoalSituacaoRua = false;
            }

            if (objeto.statusAcompanhadoPorOutraInstituicao == 1) {
                $scope.disabledInput.outraInstituicaoQueAcompanha = false;
            }
            if (objeto.statusVisitaFamiliarFrequentemente == 1) {
                $scope.disabledInput.grauParentescoFamiliarFrequentado = false;
                $scope.requiredInput.grauParentescoFamiliarFrequentado = true;
            }

            if (objeto.statusTemAcessoHigienePessoalSituacaoRua == 1) {
                $scope.disabledInput.Banho = false;
                $scope.disabledInput.Acesso_a_sanitario = false;
                $scope.disabledInput.Higiene_bucal = false;
                $scope.disabledInput.Outros_higienePessoalSituacaoRua = false;
                $scope.requiredInput.Banho = true;
                $scope.requiredInput.Acesso_a_sanitario = true;
                $scope.requiredInput.Higiene_bucal = true;
                $scope.requiredInput.Outros_higienePessoalSituacaoRua = true;
            }

            if (objeto.motivoSaidaCidadao == 135) {
                $scope.disabledInput.dataObito = false;
                $scope.requiredInput.dataObito = true;
                $scope.disabledInput.numeroDO = false;
            }
        };

        $scope.searchData = function(data) {

            if (data != undefined) {
                var condition = 'id = ' + data;
                // console.log(condition);
                PeopleService.getPeople(condition).then(function(people) {
                    if (people[0] !== "undefined") {
                        $scope.dataEditPeople = people[0];
                        $scope.dataValidation();
                        $scope.validationDataDictionary($scope.dataEditPeople);
                    } else {
                        alert("Não foi possível encontrar o registro");
                    }
                }).catch(function(err) {
                    alert("Problema na busca do cadastro");
                });
            } else {
                $scope.requiredInput = {
                    cnsResponsavelFamiliar: true,
                    microarea: true,
                    nomeCidadao: true,
                    dataNascimentoCidadao: true,
                    sexoCidadao: true,
                    racaCorCidadao: true,
                    numeroNisPisPasep: false,
                    etnia: false,
                    nomeMaeCidadao: true,
                    nomePaiCidadao: true,
                    nacionalidadeCidadao: true,
                    paisNascimento: false,
                    dtNaturalizacao: false,
                    portariaNaturalizacao: false,
                    codigoIbgeMunicipioNascimento: false,
                    dtEntradaBrasil: false
                };

                $scope.disabledInput = {
                    cnsResponsavelFamiliar: false,
                    microarea: false,
                    etnia: true,
                    nomeMaeCidadao: false,
                    nomePaiCidadao: false,
                    paisNascimento: true,
                    dtNaturalizacao: true,
                    portariaNaturalizacao: true,
                    codigoIbgeMunicipioNascimento: true,
                    dtEntradaBrasil: true
                };
            }
        };

        $scope.searchData(angular.fromJson($stateParams.item));

        $scope.validTwoDate = function(dateOne, dateTwo) {

            var isDateOne = new Date(dateOne);
            isDateOne = isDateOne.getFullYear();

            var isDateTwo = new Date(dateTwo);
            isDateTwo = isDateTwo.getFullYear();

            console.log(isDateOne);
            console.log(isDateTwo);

            if (isDateOne > isDateTwo) {
                console.log('true');
                return true;
            } else if (isDateOne == isDateTwo) {
                console.log(dateOne);
                console.log(dateTwo);
                if (dateOne > dateTwo) {
                    console.log('true');
                    return true;
                } else {
                    console.log('false');
                    return false;
                }
            } else {
                console.log('false');
                return false;
            }

        };

        $scope.validName = function(nome, param) {

            var response = {};

            if (!nome) {
                response = {
                    valid: false,
                    mensagem: 'O campo "Nome ' + param + '" é obrigatório.'
                };
                return response;
            } else {
                nome = nome.trim();
                if (nome.length < 3) {
                    //regra 1
                    response = {
                        valid: false,
                        mensagem: 'O campo "Nome ' + param + '" precisa ter ao menos 3 caracteres.'
                    };
                    return response;
                }
            }
            //regra [3]
            while (nome.search(/\s\s/) >= 0)
                nome = nome.replace(/\s\s/g, ' ');

            //regra 4
            nome = nome.toUpperCase(nome);

            //regra 6
            if (!(/^[A-Za-záàâãéèêíïóôõöúçñÁÀÂÃÉÈÊÍÏÓÔÕÖÚÇÑ ']+$/).test(nome)) {
                response = {
                    valid: false,
                    mensagem: 'O campo "Nome ' + param + '" possui caracter inválido. São aceitos caracters de a - Z, espaço, com acento e apóstrofo.'
                };
                return response;
            }

            var nomeArr = nome.split(' ');

            //regra 2
            //console.log(nomeArr.length)
            if (nomeArr.length <= 1) {
                response = {
                    valid: false,
                    mensagem: 'O campo "Nome ' + param + '" deve ser composto por 2 termos ou mais.'
                };
                return response;
            }
            var aux = 0;
            for (i = 0; i < nomeArr.length; i++) {
                if (nomeArr[i].length == 1) {
                    if (!(i > 0 && (nomeArr[i] === 'E' || nomeArr[i] === 'Y'))) {
                        //regra 5/7
                        response = {
                            valid: false,
                            mensagem: 'No campo "Nome ' + param + '" : Apenas os termos "E" e/ou "Y" são aceitos com apenas 1 caracter'
                        };
                        return response;
                    }
                } else if (nomeArr[i].length == 2) {
                    //regra 8
                    aux++;
                }
            }

            //regra 8
            if (nomeArr.length == 2 && aux == nomeArr.length) {
                response = {
                    valid: false,
                    mensagem: 'O campo "Nome ' + param + '" possui apenas 2 termos com 2 caracteres cada.'
                };
            }

            return response = {
                valid: true
            };
        };

        var inserido = false;

        $scope.addIdentification = function(dataPeople) { //receberObjeto

            var dataInformation;

            // if (inserido) {
            //   console.log(inserido);
            //   return;
            // }


            if (typeof dataPeople != 'undefined') {
                dataInformation = {
                    profissionalCNS: $rootScope.userLogged.profissionalCNS,
                    cboCodigo_2002: $rootScope.userLogged.cboCodigo_2002,
                    cnes: $rootScope.userLogged.cnes,
                    ine: $rootScope.userLogged.ine,
                    dataAtendimento: new Date().getTime(),
                    codigoIbgeMunicipio: $rootScope.userLogged.codigoIbgeMunicipio,
                    DataRegistro: new Date().getTime(),
                    descricaoCausaInternacaoEm12Meses: null,
                    descricaoOutraCondicao1: null,
                    descricaoOutraCondicao2: null,
                    descricaoOutraCondicao3: null,
                    descricaoPlantasMedicinaisUsadas: null,
                    Insuficiencia_cardiaca: null,
                    Outro_Doenca_Cardiaca: null,
                    Nao_Sabe_Doenca_Cardiaca: null,
                    Asma: null,
                    DPOC_Enfisema: null,
                    Outro_Doenca_Respiratoria: null,
                    Nao_Sabe_Doenca_Respiratoria: null,
                    Insuficiencia_renal: null,
                    Outro_Doenca_Rins: null,
                    Nao_Sabe_Doenca_Rins: null,
                    maternidadeDeReferencia: null,
                    situacaoPeso: null,
                    statusEhDependenteAlcool: null,
                    statusEhDependenteOutrasDrogas: null,
                    statusEhFumante: null,
                    statusEhGestante: null,
                    statusEstaAcamado: null,
                    statusEstaDomiciliado: null,
                    statusTemDiabetes: null,
                    statusTemDoencaRespiratoria: null,
                    statusTemHanseniase: null,
                    statusTemHipertensaoArterial: null,
                    statusTemTeveCancer: null,
                    statusTemTeveDoencasRins: null,
                    statusTemTuberculose: null,
                    statusTeveAvcDerrame: null,
                    statusTeveDoencaCardiaca: null,
                    statusTeveInfarto: null,
                    statusTeveInternadoem12Meses: null,
                    statusUsaOutrasPraticasIntegrativasOuComplementares: null,
                    statusUsaPlantasMedicinais: null,
                    statusDiagnosticoMental: null,
                    grauParentescoFamiliarFrequentado: null,
                    Banho: null,
                    Acesso_a_sanitario: null,
                    Higiene_bucal: null,
                    Outros_higienePessoalSituacaoRua: null,
                    Restaurante_popular: null,
                    Doacao_grupo_religioso: null,
                    Doacao_restaurante: null,
                    Doacao_popular: null,
                    Outros_origemAlimentoSituacaoRua: null,
                    outraInstituicaoQueAcompanha: null,
                    quantidadeAlimentacoesAoDiaSituacaoRua: null,
                    statusAcompanhadoPorOutraInstituicao: null,
                    statusPossuiReferenciaFamiliar: null,
                    statusRecebeBeneficio: null,
                    statusSituacaoRua: null,
                    statusTemAcessoHigienePessoalSituacaoRua: null,
                    statusVisitaFamiliarFrequentemente: null,
                    tempoSituacaoRua: null,
                    fichaAtualizada: 0,
                    nomeSocial: typeof dataPeople.nomeSocial !== 'undefined' ? dataPeople.nomeSocial : null,
                    // codigoIbgeMunicipioNascimento: typeof dataPeople.codigoIbgeMunicipioNascimento !== 'undefined' ? $scope.convertToSaveData('codigoIbgeMunicipioNascimento',dataPeople.codigoIbgeMunicipioNascimento) : null,
                    codigoIbgeMunicipioNascimento: typeof dataPeople.codigoIbgeMunicipioNascimento !== 'undefined' ? dataPeople.codigoIbgeMunicipioNascimento : null,
                    dataNascimentoCidadao: typeof dataPeople.dataNascimentoCidadao !== 'undefined' ? dataPeople.dataNascimentoCidadao : null,
                    desconheceNomeMae: typeof dataPeople.desconheceNomeMae !== 'undefined' ? dataPeople.desconheceNomeMae : null,
                    emailCidadao: typeof dataPeople.emailCidadao !== 'undefined' ? dataPeople.emailCidadao : null,
                    nacionalidadeCidadao: typeof dataPeople.nacionalidadeCidadao !== 'undefined' ? dataPeople.nacionalidadeCidadao : null,
                    nomeCidadao: typeof dataPeople.nomeCidadao !== 'undefined' ? dataPeople.nomeCidadao : null,
                    nomeMaeCidadao: typeof dataPeople.nomeMaeCidadao !== 'undefined' ? dataPeople.nomeMaeCidadao : null,
                    cnsCidadao: typeof dataPeople.cnsCidadao !== 'undefined' ? dataPeople.cnsCidadao : null,
                    cnsResponsavelFamiliar: typeof dataPeople.cnsResponsavelFamiliar !== 'undefined' ? dataPeople.cnsResponsavelFamiliar : null,
                    CPF: typeof dataPeople.CPF !== 'undefined' ? dataPeople.CPF : null,
                    RG: typeof dataPeople.RG !== 'undefined' ? dataPeople.RG : null,
                    ComplementoRG: typeof dataPeople.ComplementoRG !== 'undefined' ? dataPeople.ComplementoRG : null,
                    telefoneCelular: typeof dataPeople.telefoneCelular !== 'undefined' ? dataPeople.telefoneCelular : null,
                    beneficiarioBolsaFamilia: typeof dataPeople.beneficiarioBolsaFamilia !== "undefined" ? dataPeople.beneficiarioBolsaFamilia : null,
                    numeroNisPisPasep: typeof dataPeople.numeroNisPisPasep !== 'undefined' ? dataPeople.numeroNisPisPasep : null,
                    // paisNascimento: typeof dataPeople.paisNascimento !== 'undefined' ? $scope.convertToSaveData('paisNascimento',dataPeople.paisNascimento) : null,
                    paisNascimento: typeof dataPeople.paisNascimento !== 'undefined' ? dataPeople.paisNascimento : null,
                    racaCorCidadao: typeof dataPeople.racaCorCidadao !== 'undefined' ? dataPeople.racaCorCidadao : null,
                    EstadoCivil: typeof dataPeople.EstadoCivil !== 'undefined' ? dataPeople.EstadoCivil : null,
                    sexoCidadao: typeof dataPeople.sexoCidadao !== 'undefined' ? dataPeople.sexoCidadao : null,
                    statusEhResponsavel: typeof dataPeople.statusEhResponsavel !== 'undefined' ? dataPeople.statusEhResponsavel : null,
                    // etnia: typeof dataPeople.etnia !== 'undefined' ? $scope.convertToSaveData('etnia',dataPeople.etnia) : null,
                    etnia: typeof dataPeople.etnia !== 'undefined' ? dataPeople.etnia : null,
                    nomePaiCidadao: typeof dataPeople.nomePaiCidadao !== 'undefined' ? dataPeople.nomePaiCidadao : null,
                    desconheceNomePai: typeof dataPeople.desconheceNomePai !== 'undefined' ? dataPeople.desconheceNomePai : null,
                    dtNaturalizacao: typeof dataPeople.dtNaturalizacao !== 'undefined' ? dataPeople.dtNaturalizacao : null,
                    portariaNaturalizacao: typeof dataPeople.portariaNaturalizacao !== 'undefined' ? dataPeople.portariaNaturalizacao : null,
                    dtEntradaBrasil: typeof dataPeople.dtEntradaBrasil !== 'undefined' ? dataPeople.dtEntradaBrasil : null,
                    microarea: typeof dataPeople.microarea !== 'undefined' ? dataPeople.microarea : null,
                    stForaArea: typeof dataPeople.stForaArea !== 'undefined' ? dataPeople.stForaArea : null,
                    observacao: typeof dataPeople.observacao !== 'undefined' ? dataPeople.observacao : null,
                    AuditivaDeficiencias: null,
                    VisualDeficiencias: null,
                    Intelectual_CognitivaDeficiencias: null,
                    FisicaDeficiencias: null,
                    OutraDeficiencias: null,
                    grauInstrucaoCidadao: null,
                    ocupacaoCodigoCbo2002: null,
                    orientacaoSexualCidadao: null,
                    povoComunidadeTradicional: null,
                    relacaoParentescoCidadao: null,
                    situacaoMercadoTrabalhoCidadao: null,
                    statusDesejaInformarOrientacaoSexual: null,
                    statusFrequentaBenzedeira: null,
                    statusFrequentaEscola: null,
                    statusMembroPovoComunidadeTradicional: null,
                    statusParticipaGrupoComunitario: null,
                    statusPossuiPlanoSaudePrivado: null,
                    statusTemAlgumaDeficiencia: 0,
                    identidadeGeneroCidadao: null,
                    statusDesejaInformarIdentidadeGenero: null,
                    AdultoResponsavelresponsavelPorCrianca: null,
                    OutrasCriancasresponsavelPorCrianca: null,
                    AdolescenteresponsavelPorCrianca: null,
                    SozinharesponsavelPorCrianca: null,
                    CrecheresponsavelPorCrianca: null,
                    OutroresponsavelPorCrianca: null,
                    statusTermoRecusaCadastroIndividualAtencaoBasica: null,
                    motivoSaidaCidadao: null,
                    dataObito: null,
                    numeroDO: null,
                    status: 'Em Edição',
                    uuidFichaOriginadora: null,
                    uuid: null,
                    token: null,
                    latitude: $scope.peoplePosition.lat,
                    longitude: $scope.peoplePosition.long
                };


            }

            var mensagem = null;
            var invalid = false;

            /*if (dataInformation.cnsCidadao = dataInformation.cnsResponsavelFamiliar) {
                invalid = true;
                mensagem = 'O campo CNS Cidadão não pode ser igual ao campo CNS Responsável';

            }*/

            if ($scope.validNoDataPeople(dataInformation.cnsCidadao)) {
                dataInformation.cnsCidadao = null;
            }

            if (dataInformation.statusEhResponsavel != 1) {
                if (dataInformation.cnsCidadao == dataInformation.cnsResponsavelFamiliar) {
                    invalid = true;
                    mensagem = 'O CNS do responsável familiar deve ser diferente do CNS do cidadão.';
                }
                if ($scope.validNoDataPeople(dataInformation.cnsResponsavelFamiliar)) {
                    invalid = true;
                    mensagem = 'Insira o CNS do responsável familiar!';
                }
            }

            if ($scope.validNoDataPeople(dataInformation.ComplementoRG)) {
                dataInformation.ComplementoRG = null;
            }

            if (dataInformation.stForaArea != 1) {
                if ($scope.validNoDataPeople(dataInformation.microarea)) {
                    invalid = true;
                    mensagem = 'Insira a Microarea!';
                }
            }

            if ($scope.validNoDataPeople(dataInformation.nomeSocial)) {
                dataInformation.nomeSocial = null;
            }

            if ($scope.validNoDataPeople(dataInformation.dataNascimentoCidadao)) {
                document.getElementById("dataNascimentoCidadaoError").style.display = "block";
                invalid = true;
                mensagem = 'A data de nascimento é obrigatória no formato dd/MM/yyyy!';
            } else {
                var elemento = document.getElementById('dataNascimentoCidadao');
                var RegExp = /^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$/;
                if (!RegExp.test(elemento.value)) {
                    document.getElementById("dataNascimentoCidadaoError").style.display = "block";
                    invalid = true;
                    mensagem = 'Insira a data no formato indicado dd/MM/yyyy!';
                }

            }

            if ($scope.validNoDataPeople(dataInformation.sexoCidadao)) {
                invalid = true;
                mensagem = 'O sexo do cidadão é obrigatório!';
            }

            if (dataInformation.racaCorCidadao == null) {
                invalid = true;
                mensagem = 'A Raça/Cor do cidadão é obrigatório!';
            }

            if (dataInformation.racaCorCidadao == 5) {
                var codigoEtnia = $scope.treatmentForText(dataInformation.etnia, 'etnia');

                if (!codigoEtnia.valid) {
                    invalid = true;
                    mensagem = codigoEtnia.mensagem;
                } else {
                    dataInformation.etnia = codigoEtnia.valor;
                }
            }

            if ($scope.validNoDataPeople(dataInformation.numeroNisPisPasep)) {
                if (dataInformation.beneficiarioBolsaFamilia == 1) {
                    invalid = true;
                    mensagem = 'O NIS(PIS/PASEP) é obrigatório para Beneficiários do Bolsa Família';
                } else {
                    dataInformation.numeroNisPisPasep = null;
                }
            } else {
                if (dataInformation.numeroNisPisPasep.length != 11) {
                    invalid = true;
                    mensagem = 'O NIS(PIS/PASEP) do cidadão deve ter 11 digitos';
                }
            }

            if (dataInformation.nacionalidadeCidadao == null) {
                invalid = true;
                mensagem = 'A Nacionalidade do cidadão é obrigatória';
            }

            if (dataInformation.nacionalidadeCidadao == 1) {
                var codigoIbge = $scope.treatmentForText(dataInformation.codigoIbgeMunicipioNascimento, 'codigoIbgeMunicipioNascimento');

                if (!codigoIbge.valid) {
                    invalid = true;
                    mensagem = codigoIbge.mensagem;
                } else {
                    dataInformation.paisNascimento = 31;
                    dataInformation.codigoIbgeMunicipioNascimento = codigoIbge.valor;
                }
            }

            if (dataInformation.nacionalidadeCidadao == 2) {

                if ($scope.validNoDataPeople(dataInformation.portariaNaturalizacao)) {
                    invalid = true;
                    mensagem = 'Para este tipo de nacionalidade, a portaria de naturalização é obrigatória!';
                }

                if ($scope.validNoDataPeople(dataInformation.dataNascimentoCidadao)) {
                    invalid = true;
                    mensagem = 'A data de nascimento é obrigatória no formato dd/MM/yyyy';

                } else {
                    if ($scope.validNoDataPeople(dataInformation.dtNaturalizacao)) {
                        invalid = true;
                        mensagem = 'Para este tipo de nacionalidade, a data de naturalização é obrigatória no formato dd/MM/yyyy';
                    } else {
                        if ($scope.validTwoDate(dataInformation.dataNascimentoCidadao, dataInformation.dtNaturalizacao)) {
                            invalid = true;
                            mensagem = 'Data de Naturalização não pode ser antes da data de Nascimento';
                        }
                    }
                }
            }

            if (dataInformation.nacionalidadeCidadao == 3) {
                var codigoPais = $scope.treatmentForText(dataInformation.paisNascimento, 'paisNascimento');

                if (!codigoPais.valid) {
                    invalid = true;
                    mensagem = codigoPais.mensagem;
                } else {
                    dataInformation.paisNascimento = codigoPais.valor;
                }

                if ($scope.validNoDataPeople(dataInformation.dataNascimentoCidadao)) {
                    invalid = true;
                    mensagem = 'A data de nascimento é obrigatória no formato dd/MM/yyyy';

                } else {
                    if ($scope.validNoDataPeople(dataInformation.dtEntradaBrasil)) {
                        invalid = true;
                        mensagem = 'Para este tipo de  nacionalidade, a data de entrada no Brasil é obrigatória no formato dd/MM/yyyy';
                    } else {
                        if ($scope.validTwoDate(dataInformation.dataNascimentoCidadao, dataInformation.dtEntradaBrasil)) {
                            invalid = true;
                            mensagem = 'Data de entrada no Brasil não pode ser antes da data de Nascimento';
                        }
                    }
                }

                if (dataInformation.paisNascimento == '31') {
                    invalid = true;
                    mensagem = 'Para está nacionalidade, o País de Nascimento não pode ser o BRASIL';
                    document.getElementById('paisNascimento').value = null;
                }
            }

            var nome, motherName, fatherName;

            nome = $scope.validName(dataInformation.nomeCidadao, 'Cidadão');
            if (nome.valid == false) {
                invalid = true;
                mensagem = nome.mensagem;
            }

            if (dataInformation.desconheceNomeMae != 1) {
                motherName = $scope.validName(dataInformation.nomeMaeCidadao, 'Mãe');
                if (motherName.valid == false) {
                    invalid = true;
                    mensagem = motherName.mensagem;
                }
            }

            if (dataInformation.desconheceNomePai != 1) {
                fatherName = $scope.validName(dataInformation.nomePaiCidadao, 'Pai');
                if (fatherName.valid == false) {
                    invalid = true;
                    mensagem = fatherName.mensagem;
                }
            }

            // var x = dataInformation.telefoneCelular;
            var x = document.getElementsByName('telefoneCelular')[0].value;
            if (x.length > 0 && x.length != 14) {
                invalid = true;
                mensagem = 'O campo Celular deve conter DDD com dois dígitos + telefone celular com 9 dígitos.';
                console.log(x.length);
            }
            if (x.length == 0) {
                dataInformation.telefoneCelular = null;
            }

            // if ($scope.validNoDataPeople(dataInformation.telefoneCelular)) {
            //   dataInformation.telefoneCelular = null;
            // }


            if ($scope.validNoDataPeople(dataInformation.emailCidadao)) {
                dataInformation.emailCidadao = null;
            }

            if ($scope.validNoDataPeople(dataInformation.observacao)) {
                dataInformation.observacao = null;
            }

            if (invalid) {
                $ionicPopup.alert({
                    title: 'Ocorreu um erro',
                    template: mensagem
                });

            } else {

                var insertFamily = false;
                if (dataInformation.statusEhResponsavel == 1 && dataInformation.cnsCidadao != null) {
                    insertFamily = true;
                }

                if ($rootScope.newPeople == null) {

                    if (dataInformation.observacao != null) {
                        dataInformation.status = 'Pendente de Edição';
                    }

                    var jaExiste = false;
                    /*
                    EVITAR CADASTRO DUPLICADO
                    */
                    var idRegister = null;
                    console.log(dataInformation);
                    console.log(idRegister);
                    // PeopleService.getPeople('cnsCidadao = "' + dataInformation.cnsCidadao + '"')
                    //   .then(function(response) {
                    //     if (response.length) {
                    //       jaExiste = true;
                    //       console.log(jaExiste);
                    //     } else {
                    //       jaExiste = false;
                    //       console.log(jaExiste);
                    //     }
                    //   })
                    //   .catch(function(err) {
                    //     console.log(err);
                    //   })

                    // if (jaExiste == false) {
                    //   console.log(jaExiste);
                    PeopleService.getPeople('cnsCidadao = "' + dataInformation.cnsCidadao + '"')
                        .then(function(response) {
                            if (response.length) {
                                console.log(response);
                                $ionicPopup.alert({
                                    title: '<strong>Cadastro Duplicado</strong>',
                                    template: 'Já existe um registro com este CNS'
                                });
                            } else {
                                console.log(response);
                                PeopleService.addIdentificationPeople(dataInformation)
                                    .then(function(data) {
                                        if (data.insertId > 0) {
                                            idRegister = 'id =' + data.insertId;

                                            if (insertFamily) {

                                                var controlData = {
                                                    profissionalCNS: dataInformation.profissionalCNS,
                                                    cboCodigo_2002: dataInformation.cboCodigo_2002,
                                                    cnes: dataInformation.cnes,
                                                    ine: dataInformation.ine,
                                                    dataAtendimento: dataInformation.dataAtendimento,
                                                    DataRegistro: dataInformation.DataRegistro,
                                                    codigoIbgeMunicipioHeader: dataInformation.codigoIbgeMunicipioHeader,
                                                    observacao: null,
                                                    bairro: null,
                                                    /* enderecoLocalPermanencia */
                                                    cep: null,
                                                    codigoIbgeMunicipio: null,
                                                    complemento: null,
                                                    nomeLogradouro: null,
                                                    numero: null,
                                                    numeroDneUf: null,
                                                    telefoneContato: null,
                                                    telelefoneResidencia: null,
                                                    tipoLogradouroNumeroDne: null,
                                                    stSemNumero: null,
                                                    tipoDeImovel: null,
                                                    pontoReferencia: null,
                                                    microarea: null,
                                                    stForaArea: null,
                                                    abastecimentoAgua: null,
                                                    /* CondicaoMoradia */
                                                    areaProducaoRural: null,
                                                    destinoLixo: null,
                                                    formaEscoamentoBanheiro: null,
                                                    localizacao: null,
                                                    materialPredominanteParedesExtDomicilio: null,
                                                    nuComodos: null,
                                                    nuMoradores: null,
                                                    situacaoMoradiaPosseTerra: null,
                                                    stDisponibilidadeEnergiaEletrica: null,
                                                    tipoAcessoDomicilio: null,
                                                    tipoDomicilio: null,
                                                    aguaConsumoDomicilio: null,
                                                    Gato: null,
                                                    /* animaisNoDomicilio*/
                                                    Cachorro: null,
                                                    Passaro: null,
                                                    Outros_AnimalNoDomicilio: null,
                                                    quantosAnimaisNoDomicilio: null,
                                                    stAnimaisNoDomicilio: null,
                                                    statusTermoRecusa: null,
                                                    /* statusTermoRecusa*/
                                                    nomeInstituicaoPermanencia: null,
                                                    /* InstituicaoPermanencia*/
                                                    stOutrosProfissionaisVinculados: null,
                                                    nomeResponsavelTecnico: null,
                                                    cnsResponsavelTecnico: null,
                                                    cargoInstituicao: null,
                                                    telefoneResponsavelTecnico: null,
                                                    status: 'Pendente de Edição',
                                                    /*status ficha*/
                                                    fichaAtualizada: 0,
                                                    uuid: null,
                                                    token: null
                                                };

                                                return HomeRegistrationService.insertAdressHomeRegistration(controlData);
                                            } else {
                                                return data = null;
                                            }
                                        }
                                    })
                                    .then(function(response) {
                                        if (response != null && response.insertId > 0) {

                                            var controlFamily = {
                                                dataNascimentoResponsavel: dataInformation.dataNascimentoCidadao,
                                                numeroCnsResponsavel: dataInformation.cnsCidadao,
                                                numeroMembrosFamilia: null,
                                                numeroProntuario: null,
                                                rendaFamiliar: null,
                                                resideDesde: null,
                                                stMudanca: null
                                            };

                                            return FamilyService.insertFamily(controlFamily, response.insertId);
                                        } else {
                                            return response = null;
                                        }
                                    })
                                    .then(function(response) {
                                        return PeopleService.getPeople(idRegister);
                                    })
                                    .then(function(people) {
                                        if (people[0] !== "undefined") {
                                            $rootScope.newPeople = people[0];
                                            $state.go('pessoas/tabs/add-information', {
                                                item: $rootScope.newPeople.id
                                            });
                                        } else {
                                            alert("Não foi possível salvar");
                                        }
                                    })
                                    .catch(function(error) {
                                        alert('Problema na execução');
                                    });
                            }
                        });
                } else {

                    if (dataInformation.observacao != null) {
                        dataInformation.status = 'Pendente de Edição';
                    }

                    var condition = 'id =' + $rootScope.newPeople.id;

                    PeopleService.updateEditInformationsPeople(dataInformation, $scope.peoplePosition, condition)
                        .then(function(response) {
                            if (dataInformation.statusEhResponsavel == 1) {
                                return HomeRegistrationService.getHomeRegistration('id = (select cadastroDomiciliarId from FamiliaRow where numeroCnsResponsavel = "' + dataInformation.cnsCidadao + '")');
                            } else {
                                return null;
                            }
                        })
                        .then(function(response) {

                            if (response != null && response.length == 0 && dataInformation.cnsCidadao != null) {

                                var controlData = {
                                    profissionalCNS: dataInformation.profissionalCNS,
                                    cboCodigo_2002: dataInformation.cboCodigo_2002,
                                    cnes: dataInformation.cnes,
                                    ine: dataInformation.ine,
                                    dataAtendimento: dataInformation.dataAtendimento,
                                    DataRegistro: dataInformation.DataRegistro,
                                    codigoIbgeMunicipioHeader: dataInformation.codigoIbgeMunicipioHeader,
                                    observacao: null,
                                    bairro: null,
                                    /* enderecoLocalPermanencia */
                                    cep: null,
                                    codigoIbgeMunicipio: null,
                                    complemento: null,
                                    nomeLogradouro: null,
                                    numero: null,
                                    numeroDneUf: null,
                                    telefoneContato: null,
                                    telelefoneResidencia: null,
                                    tipoLogradouroNumeroDne: null,
                                    stSemNumero: null,
                                    tipoDeImovel: null,
                                    pontoReferencia: null,
                                    microarea: null,
                                    stForaArea: null,
                                    abastecimentoAgua: null,
                                    /* CondicaoMoradia */
                                    areaProducaoRural: null,
                                    destinoLixo: null,
                                    formaEscoamentoBanheiro: null,
                                    localizacao: null,
                                    materialPredominanteParedesExtDomicilio: null,
                                    nuComodos: null,
                                    nuMoradores: null,
                                    situacaoMoradiaPosseTerra: null,
                                    stDisponibilidadeEnergiaEletrica: null,
                                    tipoAcessoDomicilio: null,
                                    tipoDomicilio: null,
                                    aguaConsumoDomicilio: null,
                                    Gato: null,
                                    /* animaisNoDomicilio*/
                                    Cachorro: null,
                                    Passaro: null,
                                    Outros_AnimalNoDomicilio: null,
                                    quantosAnimaisNoDomicilio: null,
                                    stAnimaisNoDomicilio: null,
                                    statusTermoRecusa: null,
                                    /* statusTermoRecusa*/
                                    nomeInstituicaoPermanencia: null,
                                    /* InstituicaoPermanencia*/
                                    stOutrosProfissionaisVinculados: null,
                                    nomeResponsavelTecnico: null,
                                    cnsResponsavelTecnico: null,
                                    cargoInstituicao: null,
                                    telefoneResponsavelTecnico: null,
                                    status: 'Pendente de Edição',
                                    /*status ficha*/
                                    fichaAtualizada: 0,
                                    uuid: null,
                                    token: null
                                };

                                return HomeRegistrationService.insertAdressHomeRegistration(controlData);
                            } else {
                                return null;
                            }

                        })
                        .then(function(response) {
                            if (response != null && response.insertId > 0) {

                                var controlFamily = {
                                    dataNascimentoResponsavel: dataInformation.dataNascimentoCidadao,
                                    numeroCnsResponsavel: dataInformation.cnsCidadao,
                                    numeroMembrosFamilia: null,
                                    numeroProntuario: null,
                                    rendaFamiliar: null,
                                    resideDesde: null,
                                    stMudanca: null
                                };

                                return FamilyService.insertFamily(controlFamily, response.insertId);
                            } else {
                                return response = null;
                            }
                        })
                        .then(function(response) {
                            return PeopleService.getPeople('id = ' + $rootScope.newPeople.id);
                        })
                        .then(function(people) {
                            if (people[0] !== "undefined") {
                                $rootScope.newPeople = people[0];
                                $rootScope.citizenType = {
                                    responsavel: $scope.dataPeople.statusEhResponsavel,
                                    nascimento: $scope.dataPeople.dataNascimentoCidadao,
                                    estadoCivil: $scope.dataPeople.EstadoCivil,
                                    nacionalidade: $scope.dataPeople.nacionalidadeCidadao
                                }
                                console.log(people);
                                console.log($rootScope.citizenType);
                                console.log($rootScope.newPeople);
                                $state.go('pessoas/tabs/add-information', {
                                    item: $rootScope.newPeople.id
                                });
                            } else {
                                alert("Não foi possível salvar");
                            }
                        })
                        .catch(function(error) {
                            console.log(error);
                            alert('Ocorreu um erro ao atualizar este cadastro');
                        });
                }
            }

        };

        $scope.updateInformations = function(dataPeople, param) {

            var dataInformation;

            if (typeof dataPeople != 'undefined') {
                dataInformation = {
                    relacaoParentescoCidadao: typeof dataPeople.relacaoParentescoCidadao !== 'undefined' ? dataPeople.relacaoParentescoCidadao : null,
                    statusFrequentaEscola: typeof dataPeople.statusFrequentaEscola !== 'undefined' ? dataPeople.statusFrequentaEscola : null,
                    // ocupacaoCodigoCbo2002: typeof dataPeople.ocupacaoCodigoCbo2002 !== 'undefined' ? $scope.convertToSaveData('ocupacaoCodigoCbo2002',dataPeople.ocupacaoCodigoCbo2002) : null,
                    ocupacaoCodigoCbo2002: typeof dataPeople.ocupacaoCodigoCbo2002 !== 'undefined' ? dataPeople.ocupacaoCodigoCbo2002 : null,
                    grauInstrucaoCidadao: typeof dataPeople.grauInstrucaoCidadao !== 'undefined' ? dataPeople.grauInstrucaoCidadao : null,
                    situacaoMercadoTrabalhoCidadao: typeof dataPeople.situacaoMercadoTrabalhoCidadao !== 'undefined' ? dataPeople.situacaoMercadoTrabalhoCidadao : null,
                    AdultoResponsavelresponsavelPorCrianca: typeof dataPeople.AdultoResponsavelresponsavelPorCrianca !== 'undefined' ? dataPeople.AdultoResponsavelresponsavelPorCrianca : null,
                    OutrasCriancasresponsavelPorCrianca: typeof dataPeople.OutrasCriancasresponsavelPorCrianca !== 'undefined' ? dataPeople.OutrasCriancasresponsavelPorCrianca : null,
                    AdolescenteresponsavelPorCrianca: typeof dataPeople.AdolescenteresponsavelPorCrianca !== 'undefined' ? dataPeople.AdolescenteresponsavelPorCrianca : null,
                    SozinharesponsavelPorCrianca: typeof dataPeople.SozinharesponsavelPorCrianca !== 'undefined' ? dataPeople.SozinharesponsavelPorCrianca : null,
                    CrecheresponsavelPorCrianca: typeof dataPeople.CrecheresponsavelPorCrianca !== 'undefined' ? dataPeople.CrecheresponsavelPorCrianca : null,
                    OutroresponsavelPorCrianca: typeof dataPeople.OutroresponsavelPorCrianca !== 'undefined' ? dataPeople.OutroresponsavelPorCrianca : null,
                    statusFrequentaBenzedeira: typeof dataPeople.statusFrequentaBenzedeira !== 'undefined' ? dataPeople.statusFrequentaBenzedeira : null,
                    statusParticipaGrupoComunitario: typeof dataPeople.statusParticipaGrupoComunitario !== 'undefined' ? dataPeople.statusParticipaGrupoComunitario : null,
                    statusPossuiPlanoSaudePrivado: typeof dataPeople.statusPossuiPlanoSaudePrivado !== 'undefined' ? dataPeople.statusPossuiPlanoSaudePrivado : null,
                    statusMembroPovoComunidadeTradicional: typeof dataPeople.statusMembroPovoComunidadeTradicional !== 'undefined' ? dataPeople.statusMembroPovoComunidadeTradicional : null,
                    povoComunidadeTradicional: typeof dataPeople.povoComunidadeTradicional !== 'undefined' ? dataPeople.povoComunidadeTradicional : null,
                    statusDesejaInformarOrientacaoSexual: typeof dataPeople.statusDesejaInformarOrientacaoSexual !== 'undefined' ? dataPeople.statusDesejaInformarOrientacaoSexual : null,
                    orientacaoSexualCidadao: typeof dataPeople.orientacaoSexualCidadao !== 'undefined' ? dataPeople.orientacaoSexualCidadao : null,
                    statusDesejaInformarIdentidadeGenero: typeof dataPeople.statusDesejaInformarIdentidadeGenero !== 'undefined' ? dataPeople.statusDesejaInformarIdentidadeGenero : null,
                    identidadeGeneroCidadao: typeof dataPeople.identidadeGeneroCidadao !== 'undefined' ? dataPeople.identidadeGeneroCidadao : null,
                    statusTemAlgumaDeficiencia: typeof dataPeople.statusTemAlgumaDeficiencia !== 'undefined' ? dataPeople.statusTemAlgumaDeficiencia : null,
                    AuditivaDeficiencias: typeof dataPeople.AuditivaDeficiencias !== 'undefined' ? dataPeople.AuditivaDeficiencias : null,
                    VisualDeficiencias: typeof dataPeople.VisualDeficiencias !== 'undefined' ? dataPeople.VisualDeficiencias : null,
                    Intelectual_CognitivaDeficiencias: typeof dataPeople.Intelectual_CognitivaDeficiencias !== 'undefined' ? dataPeople.Intelectual_CognitivaDeficiencias : null,
                    FisicaDeficiencias: typeof dataPeople.FisicaDeficiencias !== 'undefined' ? dataPeople.FisicaDeficiencias : null,
                    OutraDeficiencias: typeof dataPeople.OutraDeficiencias !== 'undefined' ? dataPeople.OutraDeficiencias : null
                };
            } else {
                dataInformation = $rootScope.newPeople;
            }

            //Validation
            var invalid = false;
            var message = null;

            var codigoCbo = $scope.treatmentForText(dataInformation.ocupacaoCodigoCbo2002, 'ocupacaoCodigoCbo2002');

            if (!codigoCbo.valid) {
                invalid = true;
                message = codigoCbo.mensagem;
            } else {
                dataInformation.ocupacaoCodigoCbo2002 = codigoCbo.valor;
            }

            if ($scope.validNoDataPeople(dataInformation.povoComunidadeTradicional)) {
                dataInformation.povoComunidadeTradicional = null;
            }

            var allDeficiences = [];
            var qtdDeficiencias = 0;

            if (dataInformation.statusTemAlgumaDeficiencia == 1) {

                allDeficiences.push(dataInformation.AuditivaDeficiencias);
                allDeficiences.push(dataInformation.VisualDeficiencias);
                allDeficiences.push(dataInformation.Intelectual_CognitivaDeficiencias);
                allDeficiences.push(dataInformation.FisicaDeficiencias);
                allDeficiences.push(dataInformation.OutraDeficiencias);

                var i = 0;
                for (i = 0; i < allDeficiences.length; i++) {
                    if (allDeficiences[i] != null) {
                        qtdDeficiencias++;
                    }
                }
            }

            if (dataInformation.statusTemAlgumaDeficiencia == 1 && qtdDeficiencias == 0) {
                invalid = true;
                message = 'Selecione ao menos uma deficiência';
            }

            if (invalid) {
                $ionicPopup.alert({
                    title: 'Ocorreu um erro!',
                    template: message
                });
            } else {
                var condition = "id = " + $rootScope.newPeople.id;

                PeopleService.updateInformationsPeople(dataInformation, condition).then(function(response) {

                    PeopleService.getPeople(condition).then(function(people) {
                        if (people[0] !== "undefined") {
                            $rootScope.newPeople = people[0];
                            $rootScope.citizenType = {
                                relacaoComResp: dataPeople.relacaoParentescoCidadao,
                                situacaoNoMerc: dataPeople.situacaoMercadoTrabalhoCidadao
                            }
                            if (param == 'return') {
                                $state.go('pessoas/tabs/edit-identification', {
                                    item: angular.fromJson($stateParams.item)
                                });
                            } else {
                                $state.go('pessoas/tabs/add-helth', {
                                    item: angular.fromJson($stateParams.item)
                                });
                            }

                        } else {
                            alert("Não foi possível salvar");
                        }
                    }).catch(function(err) {
                        alert("Problema na busca da visita");
                    });

                }).catch(function(error) {
                    alert("Não foi possível atualizar as informações");
                });
            }
            //Validation
        };

        $scope.updateHealth = function(dataPeople, param) {

            var dataInformation;

            if (typeof dataPeople != 'undefined') {
                dataInformation = {
                    statusEhGestante: typeof dataPeople.statusEhGestante !== 'undefined' ? dataPeople.statusEhGestante : null,
                    maternidadeDeReferencia: typeof dataPeople.maternidadeDeReferencia !== 'undefined' ? dataPeople.maternidadeDeReferencia : null,
                    situacaoPeso: typeof dataPeople.situacaoPeso !== 'undefined' ? dataPeople.situacaoPeso : null,
                    statusEhFumante: typeof dataPeople.statusEhFumante !== 'undefined' ? dataPeople.statusEhFumante : null,
                    statusEhDependenteAlcool: typeof dataPeople.statusEhDependenteAlcool !== 'undefined' ? dataPeople.statusEhDependenteAlcool : null,
                    statusEhDependenteOutrasDrogas: typeof dataPeople.statusEhDependenteOutrasDrogas !== 'undefined' ? dataPeople.statusEhDependenteOutrasDrogas : null,
                    statusTemHipertensaoArterial: typeof dataPeople.statusTemHipertensaoArterial !== 'undefined' ? dataPeople.statusTemHipertensaoArterial : null,
                    statusTemDiabetes: typeof dataPeople.statusTemDiabetes !== 'undefined' ? dataPeople.statusTemDiabetes : null,
                    statusTeveAvcDerrame: typeof dataPeople.statusTeveAvcDerrame !== 'undefined' ? dataPeople.statusTeveAvcDerrame : null,
                    statusTeveInfarto: typeof dataPeople.statusTeveInfarto !== 'undefined' ? dataPeople.statusTeveInfarto : null,
                    statusTeveDoencaCardiaca: typeof dataPeople.statusTeveDoencaCardiaca !== 'undefined' ? dataPeople.statusTeveDoencaCardiaca : null,
                    Insuficiencia_cardiaca: typeof dataPeople.Insuficiencia_cardiaca !== 'undefined' ? dataPeople.Insuficiencia_cardiaca : null,
                    Outro_Doenca_Cardiaca: typeof dataPeople.Outro_Doenca_Cardiaca !== 'undefined' ? dataPeople.Outro_Doenca_Cardiaca : null,
                    Nao_Sabe_Doenca_Cardiaca: typeof dataPeople.Nao_Sabe_Doenca_Cardiaca !== 'undefined' ? dataPeople.Nao_Sabe_Doenca_Cardiaca : null,
                    statusTemTeveDoencasRins: typeof dataPeople.statusTemTeveDoencasRins !== 'undefined' ? dataPeople.statusTemTeveDoencasRins : null,
                    Insuficiencia_renal: typeof dataPeople.Insuficiencia_renal !== 'undefined' ? dataPeople.Insuficiencia_renal : null,
                    Outro_Doenca_Rins: typeof dataPeople.Outro_Doenca_Rins !== 'undefined' ? dataPeople.Outro_Doenca_Rins : null,
                    Nao_Sabe_Doenca_Rins: typeof dataPeople.Nao_Sabe_Doenca_Rins !== 'undefined' ? dataPeople.Nao_Sabe_Doenca_Rins : null,
                    statusTemDoencaRespiratoria: typeof dataPeople.statusTemDoencaRespiratoria !== 'undefined' ? dataPeople.statusTemDoencaRespiratoria : null,
                    Asma: typeof dataPeople.Asma !== 'undefined' ? dataPeople.Asma : null,
                    DPOC_Enfisema: typeof dataPeople.DPOC_Enfisema !== 'undefined' ? dataPeople.DPOC_Enfisema : null,
                    Outro_Doenca_Respiratoria: typeof dataPeople.Outro_Doenca_Respiratoria !== 'undefined' ? dataPeople.Outro_Doenca_Respiratoria : null,
                    Nao_Sabe_Doenca_Respiratoria: typeof dataPeople.Nao_Sabe_Doenca_Respiratoria !== 'undefined' ? dataPeople.Nao_Sabe_Doenca_Respiratoria : null,
                    statusTemHanseniase: typeof dataPeople.statusTemHanseniase !== 'undefined' ? dataPeople.statusTemHanseniase : null,
                    statusTemTuberculose: typeof dataPeople.statusTemTuberculose !== 'undefined' ? dataPeople.statusTemTuberculose : null,
                    statusTemTeveCancer: typeof dataPeople.statusTemTeveCancer !== 'undefined' ? dataPeople.statusTemTeveCancer : null,
                    statusTeveInternadoem12Meses: typeof dataPeople.statusTeveInternadoem12Meses !== 'undefined' ? dataPeople.statusTeveInternadoem12Meses : null,
                    descricaoCausaInternacaoEm12Meses: typeof dataPeople.descricaoCausaInternacaoEm12Meses !== 'undefined' ? dataPeople.descricaoCausaInternacaoEm12Meses : null,
                    statusDiagnosticoMental: typeof dataPeople.statusDiagnosticoMental !== 'undefined' ? dataPeople.statusDiagnosticoMental : null,
                    statusEstaAcamado: typeof dataPeople.statusEstaAcamado !== 'undefined' ? dataPeople.statusEstaAcamado : null,
                    statusEstaDomiciliado: typeof dataPeople.statusEstaDomiciliado !== 'undefined' ? dataPeople.statusEstaDomiciliado : null,
                    statusUsaPlantasMedicinais: typeof dataPeople.statusUsaPlantasMedicinais !== 'undefined' ? dataPeople.statusUsaPlantasMedicinais : null,
                    descricaoPlantasMedicinaisUsadas: typeof dataPeople.descricaoPlantasMedicinaisUsadas !== 'undefined' ? dataPeople.descricaoPlantasMedicinaisUsadas : null,
                    statusUsaOutrasPraticasIntegrativasOuComplementares: typeof dataPeople.statusUsaOutrasPraticasIntegrativasOuComplementares !== 'undefined' ? dataPeople.statusUsaOutrasPraticasIntegrativasOuComplementares : null,
                    descricaoOutraCondicao1: typeof dataPeople.descricaoOutraCondicao1 !== 'undefined' ? dataPeople.descricaoOutraCondicao1 : null,
                    descricaoOutraCondicao2: typeof dataPeople.descricaoOutraCondicao2 !== 'undefined' ? dataPeople.descricaoOutraCondicao2 : null,
                    descricaoOutraCondicao3: typeof dataPeople.descricaoOutraCondicao3 !== 'undefined' ? dataPeople.descricaoOutraCondicao3 : null
                };
            } else {
                dataInformation = $rootScope.newPeople;
            }

            if ($scope.validNoDataPeople(dataInformation.maternidadeDeReferencia)) {
                dataInformation.maternidadeDeReferencia = null;
            }

            var title = 'Informativo';
            var mensagem;
            var invalid = false;

            var allCardiac = [];
            var allKidney = [];
            var allRespiratory = [];
            var qtdCardiac = 0;
            var qtdKidney = 0;
            var qtdRespiratory = 0;
            var i = 0;

            allCardiac.push(dataInformation.Insuficiencia_cardiaca);
            allCardiac.push(dataInformation.Outro_Doenca_Cardiaca);
            allCardiac.push(dataInformation.Nao_Sabe_Doenca_Cardiaca);

            allKidney.push(dataInformation.Insuficiencia_renal);
            allKidney.push(dataInformation.Outro_Doenca_Rins);
            allKidney.push(dataInformation.Nao_Sabe_Doenca_Rins);

            allRespiratory.push(dataInformation.Asma);
            allRespiratory.push(dataInformation.DPOC_Enfisema);
            allRespiratory.push(dataInformation.Outro_Doenca_Respiratoria);
            allRespiratory.push(dataInformation.Nao_Sabe_Doenca_Respiratoria);

            if (dataInformation.statusTeveDoencaCardiaca == 1) {
                for (i = 0; i < allCardiac.length; i++) {
                    if (allCardiac[i] != null) {
                        qtdCardiac++;
                    }
                }
            }
            if (dataInformation.statusTemTeveDoencasRins == 1) {
                for (i = 0; i < allKidney.length; i++) {
                    if (allKidney[i] != null) {
                        qtdKidney++;
                    }
                }
            }
            if (dataInformation.statusTemDoencaRespiratoria == 1) {
                for (i = 0; i < allRespiratory.length; i++) {
                    if (allRespiratory[i] != null) {
                        qtdRespiratory++;
                    }
                }
            }

            if (dataInformation.statusTeveDoencaCardiaca == 1 && qtdCardiac == 0) {
                invalid = true;
                mensagem = 'Selecione ao menos uma doença cardíaca';
            }
            if (dataInformation.statusTemTeveDoencasRins == 1 && qtdKidney == 0) {
                invalid = true;
                mensagem = 'Selecione ao menos uma doença dos rins';
            }
            if (dataInformation.statusTemDoencaRespiratoria == 1 && qtdRespiratory == 0) {
                invalid = true;
                mensagem = 'Selecione ao menos uma doença respiratória';
            }

            if (dataInformation.statusTeveInternadoem12Meses == 1) {
                if ($scope.validNoDataPeople(dataInformation.descricaoCausaInternacaoEm12Meses)) {
                    invalid = true;
                    mensagem = 'Insira a descrição da causa de internação';
                }
            }

            if ($scope.validNoDataPeople(dataInformation.descricaoPlantasMedicinaisUsadas)) {
                dataInformation.descricaoPlantasMedicinaisUsadas = null;
            }

            if ($scope.validNoDataPeople(dataInformation.descricaoOutraCondicao1)) {
                dataInformation.descricaoOutraCondicao1 = null;
            }

            if ($scope.validNoDataPeople(dataInformation.descricaoOutraCondicao2)) {
                dataInformation.descricaoOutraCondicao2 = null;
            }

            if ($scope.validNoDataPeople(dataInformation.descricaoOutraCondicao3)) {
                dataInformation.descricaoOutraCondicao3 = null;
            }

            if (invalid) {
                $ionicPopup.alert({
                    title: title,
                    template: mensagem
                });
            } else {
                var condition = "id = " + $rootScope.newPeople.id;
                PeopleService.updateHelthPeople(dataInformation, condition).then(function(response) {
                    PeopleService.getPeople(condition).then(function(people) {
                        if (people[0] !== "undefined") {
                            $rootScope.newPeople = people[0];
                            if (param == 'return') {
                                $state.go('pessoas/tabs/add-information', {
                                    item: angular.fromJson($stateParams.item)
                                });
                            } else {
                                $state.go('pessoas/tabs/street', {
                                    item: angular.fromJson($stateParams.item)
                                });
                            }
                        } else {
                            alert("Não foi possível salvar");
                        }
                    }).catch(function(err) {
                        alert("Problema na busca da visita");
                    });

                }).catch(function(error) {
                    alert("Não foi possível atualizar as informações");
                });
            }

        };

        $scope.updateStreet = function(dataPeople, param) {

            var dataInformation;

            if (typeof dataPeople != 'undefined') {
                dataInformation = {
                    statusSituacaoRua: typeof dataPeople.statusSituacaoRua !== 'undefined' ? dataPeople.statusSituacaoRua : null,
                    tempoSituacaoRua: typeof dataPeople.tempoSituacaoRua !== 'undefined' ? dataPeople.tempoSituacaoRua : null,
                    statusRecebeBeneficio: typeof dataPeople.statusRecebeBeneficio !== 'undefined' ? dataPeople.statusRecebeBeneficio : null,
                    statusPossuiReferenciaFamiliar: typeof dataPeople.statusPossuiReferenciaFamiliar !== 'undefined' ? dataPeople.statusPossuiReferenciaFamiliar : null,
                    quantidadeAlimentacoesAoDiaSituacaoRua: typeof dataPeople.quantidadeAlimentacoesAoDiaSituacaoRua !== 'undefined' ? dataPeople.quantidadeAlimentacoesAoDiaSituacaoRua : null,
                    Restaurante_popular: typeof dataPeople.Restaurante_popular !== 'undefined' ? dataPeople.Restaurante_popular : null,
                    Doacao_grupo_religioso: typeof dataPeople.Doacao_grupo_religioso !== 'undefined' ? dataPeople.Doacao_grupo_religioso : null,
                    Doacao_restaurante: typeof dataPeople.Doacao_restaurante !== 'undefined' ? dataPeople.Doacao_restaurante : null,
                    Doacao_popular: typeof dataPeople.Doacao_popular !== 'undefined' ? dataPeople.Doacao_popular : null,
                    Outros_origemAlimentoSituacaoRua: typeof dataPeople.Outros_origemAlimentoSituacaoRua !== 'undefined' ? dataPeople.Outros_origemAlimentoSituacaoRua : null,
                    statusAcompanhadoPorOutraInstituicao: typeof dataPeople.statusAcompanhadoPorOutraInstituicao !== 'undefined' ? dataPeople.statusAcompanhadoPorOutraInstituicao : null,
                    outraInstituicaoQueAcompanha: typeof dataPeople.outraInstituicaoQueAcompanha !== 'undefined' ? dataPeople.outraInstituicaoQueAcompanha : null,
                    statusVisitaFamiliarFrequentemente: typeof dataPeople.statusVisitaFamiliarFrequentemente !== 'undefined' ? dataPeople.statusVisitaFamiliarFrequentemente : null,
                    grauParentescoFamiliarFrequentado: typeof dataPeople.grauParentescoFamiliarFrequentado !== 'undefined' ? dataPeople.grauParentescoFamiliarFrequentado : null,
                    statusTemAcessoHigienePessoalSituacaoRua: typeof dataPeople.statusTemAcessoHigienePessoalSituacaoRua !== 'undefined' ? dataPeople.statusTemAcessoHigienePessoalSituacaoRua : null,
                    Banho: typeof dataPeople.Banho !== 'undefined' ? dataPeople.Banho : null,
                    Acesso_a_sanitario: typeof dataPeople.Acesso_a_sanitario !== 'undefined' ? dataPeople.Acesso_a_sanitario : null,
                    Higiene_bucal: typeof dataPeople.Higiene_bucal !== 'undefined' ? dataPeople.Higiene_bucal : null,
                    Outros_higienePessoalSituacaoRua: typeof dataPeople.Outros_higienePessoalSituacaoRua !== 'undefined' ? dataPeople.Outros_higienePessoalSituacaoRua : null
                };
            } else {
                dataInformation = $rootScope.newPeople;
            }

            var message;
            var invalid = false;

            if ($scope.validNoDataPeople(dataInformation.outraInstituicaoQueAcompanha)) {
                dataInformation.outraInstituicaoQueAcompanha = null;
            }

            if (dataInformation.statusVisitaFamiliarFrequentemente == 1) {
                if ($scope.validNoDataPeople(dataInformation.grauParentescoFamiliarFrequentado)) {
                    invalid = true;
                    message = 'Insira o grau de parentesco do familiar frequentado';
                }
            }

            var allStreet = [];
            var qtdStreet = 0;
            var i = 0;

            allStreet.push(dataInformation.Banho);
            allStreet.push(dataInformation.Acesso_a_sanitario);
            allStreet.push(dataInformation.Higiene_bucal);
            allStreet.push(dataInformation.Outros_higienePessoalSituacaoRua);

            for (i = 0; i < allStreet.length; i++) {
                if (allStreet[i] != null) {
                    qtdStreet++;
                }
            }

            if (dataInformation.statusTemAcessoHigienePessoalSituacaoRua == 1 && qtdStreet == 0) {
                invalid = true;
                message = 'Selecione ao menos um item sobre higiene pessoal';
            }

            if (invalid) {
                $ionicPopup.alert({
                    title: 'Informativo!',
                    template: message
                });
            } else {
                var condition = "id = " + $rootScope.newPeople.id;

                PeopleService.updateStreetPeople(dataInformation, condition).then(function(response) {

                    PeopleService.getPeople(condition).then(function(people) {
                        if (people[0] !== "undefined") {
                            $rootScope.newPeople = people[0];
                            if (param == 'return') {
                                $state.go('pessoas/tabs/add-helth', {
                                    item: angular.fromJson($stateParams.item)
                                });
                            } else {
                                $state.go('pessoas/tabs/add-exit', {
                                    item: angular.fromJson($stateParams.item)
                                });
                            }
                        } else {
                            alert("Não foi possível salvar");
                        }
                    }).catch(function(err) {
                        alert("Problema na busca da visita");
                    });

                }).catch(function(error) {
                    alert("Não foi possível atualizar as informações");
                });
            }
        };

        var typeCitizen;

        $scope.updateExit = function(dataPeople) {

            var dataInformation;

            if (typeof dataPeople !== 'undefined') {
                dataInformation = {
                    motivoSaidaCidadao: typeof dataPeople.motivoSaidaCidadao !== 'undefined' ? dataPeople.motivoSaidaCidadao : null,
                    dataObito: typeof dataPeople.dataObito !== 'undefined' ? dataPeople.dataObito : null,
                    numeroDO: typeof dataPeople.numeroDO !== 'undefined' ? dataPeople.numeroDO : null
                };
            } else {
                dataInformation = $rootScope.newPeople;
            }

            var invalid = false;
            var message;

            if ($rootScope.newPeople.observacao != null ||
                ($rootScope.newPeople.cnsCidadao == null && $rootScope.newPeople.statusEhResponsavel == 1) ||
                ($rootScope.newPeople.statusEhResponsavel != 1 && ($rootScope.newPeople.cnsResponsavelFamiliar == null || $rootScope.newPeople.cnsCidadao == null))) {
                dataInformation.status = 'Pendente de Edição';
            } else {
                dataInformation.status = 'Aguardando Sincronismo';
            }

            if (dataInformation.motivoSaidaCidadao == 135) {

                if ($scope.validNoDataPeople(dataInformation.dataObito)) {
                    invalid = true;
                    message = 'Insira a data de Óbito no formato dd/MM/yyyy';
                } else if (dataInformation.dataObito < $rootScope.newPeople.dataNascimentoCidadao) {
                    invalid = true;
                    message = 'A data de óbito não pode ser antes da data de nascimento.';
                }

                if ($scope.validNoDataPeople(dataInformation.numeroDO)) {
                    invalid = true;
                    message = 'Insira o número de D.O.';
                }

            }

            if (invalid) {
                $ionicPopup.alert({
                    title: 'Informativo',
                    template: message
                });

            } else {

                var condition = "id = " + $rootScope.newPeople.id;

                PeopleService.updateExitPeople(dataInformation, condition).then(function(response) {
                    $rootScope.newPeople = null;
                    $state.go('app.dashboard/index');
                }).catch(function(error) {
                    alert("Não foi possível atualizar as informações");
                });

            }
        };

        $scope.addAttach = function(dataPeople) {};

        $scope.dataPeople.statusEhResponsavel = 1;
        $scope.dataPeople.desconheceNomeMae = null;
        $scope.dataPeople.desconheceNomePai = null;
        $scope.dataPeople.stForaArea = null;
        // console.log($scope.dataPeople);
    });
// );