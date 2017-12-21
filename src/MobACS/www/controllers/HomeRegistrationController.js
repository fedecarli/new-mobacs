angular.module('mobacs').controller('HomeRegistrationController', function($scope, $ionicPlatform, $location, GlobalService, $state, HomeRegistrationService, $rootScope, $stateParams, $ionicHistory, $ionicPopup, FamilyService, $cordovaGeolocation) {

    //Dados Auxiliares para o cep
    $scope.todosSantanaParnaiba = $rootScope.thisAllSantanaParnaiba;
    $scope.allCounties = $rootScope.todosCounties;
    $scope.allTiposLogradouros = $rootScope.todosTiposLogradouros;
    //Fim de dados Auxiliares para o cep
    $scope.familia = null;

    $scope.direcionaHomeRegistration = function() {
        $rootScope.newHomeResgistration = null;
        $state.go('domicilios/tabs/add/add-adress', { reload: true });
    }

    $scope.goHomeRegistarationHistory = function() {
        $ionicHistory.goBack();
    };

    $scope.goToViewHomeRegistration = function() {
        var location = $location.path();
        if (location === '/domicilios/tabs/add/add-adress') {
            $state.go('app.dashboard/index', { reload: true });
        } else if (location.search('/domicilios/tabs/add/add-livingconditions/') != -1) {
            $state.go('domicilios/tabs/add/edit-adress', { id: angular.fromJson($stateParams.id) });
        } else if (location.search('/domicilios/tabs/add/add-animals/') != -1) {
            $state.go('domicilios/tabs/add/add-livingconditions', { id: angular.fromJson($stateParams.id) });
        } else if (location.search('/domicilios/tabs/add/add-families/') != -1) {
            $state.go('domicilios/tabs/add/add-animals', { id: angular.fromJson($stateParams.id) });
        } else if (location.search('/domicilios/tabs/add/add-institution/') != -1) {
            $state.go('domicilios/tabs/add/add-families', { id: angular.fromJson($stateParams.id) });
        } else if (location.search('/domicilios/tabs/view/') != -1) {
            $state.go('domicilios/index');
        } else if (location.search('/domicilios/tabs/add/edit-adress/') != -1) {
            $state.go('view/adress', { id: angular.fromJson($stateParams.id) });
        }
    };

    //BEGIN TEST
    $ionicPlatform.registerBackButtonAction(function(event) {
        event.preventDefault();
        var location = $location.path();
        if (location === '/domicilios/tabs/add/add-adress') {
            $state.go('app.dashboard/index', { reload: true });
        } else if (location.search('/domicilios/tabs/add/add-livingconditions/') != -1) {
            $state.go('domicilios/tabs/add/edit-adress', { id: angular.fromJson($stateParams.id) });
        } else if (location.search('/domicilios/tabs/add/add-animals/') != -1) {
            $state.go('domicilios/tabs/add/add-livingconditions', { id: angular.fromJson($stateParams.id) });
        } else if (location.search('/domicilios/tabs/add/add-families/') != -1) {
            $state.go('domicilios/tabs/add/add-animals', { id: angular.fromJson($stateParams.id) });
        } else if (location.search('/domicilios/tabs/add/add-institution/') != -1) {
            $state.go('domicilios/tabs/add/add-families', { id: angular.fromJson($stateParams.id) });
        } else if (location.search('/domicilios/tabs/view/') != -1) {
            $state.go('domicilios/index');
        } else if (location.search('/domicilios/tabs/add/edit-adress/') != -1) {
            $state.go('view/adress', { id: angular.fromJson($stateParams.id) });
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

    $scope.goToHomeVisit = function(familia) {
        $state.go('usersbyhouseholds', { cns: familia[0].numeroCnsResponsavel });
    };

    $scope.homeAdressGPSIsItEnabled = function() {

        cordova.plugins.diagnostic.isLocationEnabled(function(enabled) {
                if (enabled) {

                    // var posOptions = {timeout: 10000, enableHighAccuracy: false};
                    // $cordovaGeolocation
                    //   .getCurrentPosition(posOptions)
                    //   .then(function (position) {
                    //     $scope.homeAdressPosition = {
                    //       lat : position.coords.latitude,
                    //       long : position.coords.longitude
                    //     };
                    $scope.homeAdressPosition = {
                        lat: null,
                        long: null
                    };

                    // }, function(err) {
                    //   console.log(err);
                    // });

                } else {
                    $ionicPopup.alert({
                            title: '<strong>Informativo</strong>',
                            template: 'Por favor, habilite o GPS para utilizar o aplicativo!'
                        })
                        .then(function() {
                            $scope.homeAdressGPSIsItEnabled();
                        })
                }
            },
            function(error) {
                console.error("The following error occurred:" + error);
            });
    };

    $scope.homeAdressGPSIsItEnabled();

    //END TEST
    $scope.validNoDataHomeRegistration = function(param) {
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

    $scope.formatProfessionalName = function(param, Name) {
        if (Name != undefined) {
            Name = Name.replace(/[^a-zA-ZéúíóáÉÚÍÓÁèùìòàçÇÈÙÌÒÀõãñÕÃÑêûîôâÊÛÎÔÂëÿüïöäËYÜÏÖÄ\-\ \s]+$/, "");
            Name = Name.toUpperCase();

            if (param == 'nomeResponsavelTecnico') {
                $scope.dataHomeRegistration.nomeResponsavelTecnico = Name;
            }
        } else {
            return null;
        }
    };

    $scope.validNameInstitution = function(nome, param) {
        //nome = "cr adte";
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

        return response = { valid: true };
    };

    $scope.viewToEditHome = function() {
        if ($rootScope.newHomeResgistration.statusTermoRecusa == 1) {
            $ionicPopup.alert({
                title: '<strong>Informativo</strong>',
                template: 'Não é possível editar um cadastro que foi recusado'
            })
        } else {
            $state.go('domicilios/tabs/add/edit-adress', { id: angular.fromJson($stateParams.id) });
        }
    };

    $scope.limparObservacao = function() {
        $scope.dataHomeRegistration.observacao = null;
    }

    $scope.convertToSaveData = function(param, string) {

        if (string != null) {
            if (string.length > 2) {
                if (param == 'codigoIbgeMunicipio') {
                    var allCountie = $scope.allCounties;
                    var outputCountie = [];
                    angular.forEach(allCountie, function(countie, key) {
                        if (countie.nome.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            outputCountie.push(countie);
                        }
                    });
                    return outputCountie[0].codigoIBGE;
                } else if (param == 'tipoLogradouroNumeroDne') {
                    var allLogradouros = $scope.allTiposLogradouros;
                    var outputTipoDeLogradouro = [];
                    angular.forEach(allLogradouros, function(logradouro, key) {
                        if (logradouro.descricao.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            outputTipoDeLogradouro.push(logradouro);
                        }
                    });
                    return outputTipoDeLogradouro[0].NumeroDNE;
                }
            } else {
                return null;
            }
        } else {
            return null;
        }
    };

    $scope.searchDataName = function(param, string) {

        if (string != null) {
            if (param == 'codigoIbgeMunicipio') {
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
                $scope.dataHomeRegistration.codigoIbgeMunicipio = 'SANTANA DE PARNAÍBA';
                $scope.dataHomeRegistration.numeroDneUf = '26';
                $scope.dataHomeRegistration.tipoLogradouroNumeroDne = saida[0].tipoDeLogradouro.toString();
                $scope.dataHomeRegistration.nomeLogradouro = saida[0].nomeLogradouro.toString();
                $scope.dataHomeRegistration.bairro = saida[0].nomeBairro.toString();
            } else {
                $scope.dataHomeRegistration.codigoIbgeMunicipio = null;
                $scope.dataHomeRegistration.numeroDneUf = null;
                $scope.dataHomeRegistration.tipoLogradouroNumeroDne = null;
                $scope.dataHomeRegistration.nomeLogradouro = null;
                $scope.dataHomeRegistration.bairro = null;
                $scope.dataHomeRegistration.cep = '';
                $ionicPopup.alert({
                    title: 'CEP Inválido',
                    template: 'Apenas são aceitos CEPs válidos de Santana de Parnaíba'
                });
            }
        }
    };

    $scope.disabledFamiliesForm = true;
    $scope.dataHomeRegistration = {};

    $scope.dataValidation = function() {

        $rootScope.newHomeResgistration = $scope.dataEditHomeRegistration;
        $scope.dataHomeRegistration = {
            id: $scope.dataEditHomeRegistration.id,
            bairro: $scope.dataEditHomeRegistration.bairro,
            cep: $scope.dataEditHomeRegistration.cep,
            codigoIbgeMunicipio: $scope.dataEditHomeRegistration.codigoIbgeMunicipio == null ? null : $scope.dataEditHomeRegistration.codigoIbgeMunicipio.toString(),
            complemento: $scope.dataEditHomeRegistration.complemento,
            nomeLogradouro: $scope.dataEditHomeRegistration.nomeLogradouro,
            numero: $scope.dataEditHomeRegistration.numero === null ? null : $scope.dataEditHomeRegistration.numero.toString(),
            numeroDneUf: $scope.dataEditHomeRegistration.numeroDneUf === null ? null : $scope.dataEditHomeRegistration.numeroDneUf.toString(),
            telefoneContato: $scope.dataEditHomeRegistration.telefoneContato,
            telefoneResidencia: $scope.dataEditHomeRegistration.telefoneResidencia,
            tipoLogradouroNumeroDne: $scope.dataEditHomeRegistration.tipoLogradouroNumeroDne,
            stSemNumero: $scope.dataEditHomeRegistration.stSemNumero,
            pontoReferencia: $scope.dataEditHomeRegistration.pontoReferencia,
            microarea: $scope.dataEditHomeRegistration.microarea === null ? null : $scope.dataEditHomeRegistration.microarea.toString(),
            stForaArea: $scope.dataEditHomeRegistration.stForaArea,
            tipoDeImovel: $scope.dataEditHomeRegistration.tipoDeImovel === null ? null : $scope.dataEditHomeRegistration.tipoDeImovel.toString(),
            observacao: $scope.dataEditHomeRegistration.observacao,
            abastecimentoAgua: $scope.dataEditHomeRegistration.abastecimentoAgua === null ? null : $scope.dataEditHomeRegistration.abastecimentoAgua.toString(),
            areaProducaoRural: $scope.dataEditHomeRegistration.areaProducaoRural === null ? null : $scope.dataEditHomeRegistration.areaProducaoRural.toString(),
            destinoLixo: $scope.dataEditHomeRegistration.destinoLixo == null ? null : $scope.dataEditHomeRegistration.destinoLixo.toString(),
            formaEscoamentoBanheiro: $scope.dataEditHomeRegistration.formaEscoamentoBanheiro == null ? null : $scope.dataEditHomeRegistration.formaEscoamentoBanheiro.toString(),
            localizacao: $scope.dataEditHomeRegistration.localizacao === null ? null : $scope.dataEditHomeRegistration.localizacao.toString(),
            materialPredominanteParedesExtDomicilio: $scope.dataEditHomeRegistration.materialPredominanteParedesExtDomicilio == null ? null : $scope.dataEditHomeRegistration.materialPredominanteParedesExtDomicilio.toString(),
            nuComodos: $scope.dataEditHomeRegistration.nuComodos === null ? null : $scope.dataEditHomeRegistration.nuComodos.toString(),
            nuMoradores: $scope.dataEditHomeRegistration.nuMoradores === null ? null : $scope.dataEditHomeRegistration.nuMoradores.toString(),
            situacaoMoradiaPosseTerra: $scope.dataEditHomeRegistration.situacaoMoradiaPosseTerra === null ? null : $scope.dataEditHomeRegistration.situacaoMoradiaPosseTerra.toString(),
            stDisponibilidadeEnergiaEletrica: $scope.dataEditHomeRegistration.stDisponibilidadeEnergiaEletrica,
            tipoAcessoDomicilio: $scope.dataEditHomeRegistration.tipoAcessoDomicilio === null ? null : $scope.dataEditHomeRegistration.tipoAcessoDomicilio.toString(),
            tipoDomicilio: $scope.dataEditHomeRegistration.tipoDomicilio === null ? null : $scope.dataEditHomeRegistration.tipoDomicilio.toString(),
            aguaConsumoDomicilio: $scope.dataEditHomeRegistration.aguaConsumoDomicilio == null ? null : $scope.dataEditHomeRegistration.aguaConsumoDomicilio.toString(),
            Gato: $scope.dataEditHomeRegistration.Gato,
            Cachorro: $scope.dataEditHomeRegistration.Cachorro,
            Passaro: $scope.dataEditHomeRegistration.Passaro,
            Outros_AnimalNoDomicilio: $scope.dataEditHomeRegistration.Outros_AnimalNoDomicilio,
            quantosAnimaisNoDomicilio: $scope.dataEditHomeRegistration.quantosAnimaisNoDomicilio,
            stAnimaisNoDomicilio: $scope.dataEditHomeRegistration.stAnimaisNoDomicilio,
            statusTermoRecusa: $scope.dataEditHomeRegistration.statusTermoRecusa,
            nomeInstituicaoPermanencia: $scope.dataEditHomeRegistration.nomeInstituicaoPermanencia,
            stOutrosProfissionaisVinculados: $scope.dataEditHomeRegistration.stOutrosProfissionaisVinculados,
            nomeResponsavelTecnico: $scope.dataEditHomeRegistration.nomeResponsavelTecnico,
            cnsResponsavelTecnico: $scope.dataEditHomeRegistration.cnsResponsavelTecnico,
            cargoInstituicao: $scope.dataEditHomeRegistration.cargoInstituicao,
            telefoneResponsavelTecnico: $scope.dataEditHomeRegistration.telefoneResponsavelTecnico,
            //Teste
            dataNascimentoResponsavel: $scope.dataEditHomeRegistration.dataNascimentoResponsavel,
            numeroCnsResponsavel: $scope.dataEditHomeRegistration.numeroCnsResponsavel,
            numeroMembrosFamilia: $scope.dataEditHomeRegistration.numeroMembrosFamilia,
            numeroProntuario: $scope.dataEditHomeRegistration.numeroProntuario,
            rendaFamiliar: $scope.dataEditHomeRegistration.rendaFamiliar,
            resideDesde: $scope.dataEditHomeRegistration.resideDesde,
            stMudanca: $scope.dataEditHomeRegistration.stMudanca
        }

        $scope.dataHomeRegistration.codigoIbgeMunicipio = $scope.searchDataName('codigoIbgeMunicipio', $scope.dataHomeRegistration.codigoIbgeMunicipio);
        $scope.dataHomeRegistration.tipoLogradouroNumeroDne = $scope.searchDataName('tipoLogradouroNumeroDne', $scope.dataHomeRegistration.tipoLogradouroNumeroDne);
    }

    $scope.validationHomeRegistrationForm = function(param, dado) {

        if (param == 'stSemNumero') {
            if (dado == 1) {
                $scope.disabledInput.numero = true;
                $scope.requiredInput.numero = false;
                $scope.dataHomeRegistration.numero = null;
            } else {
                $scope.disabledInput.numero = false;
                $scope.requiredInput.numero = true;
            }
        } else if (param == 'stForaArea') {
            if (dado == 1) {
                $scope.disabledInput.microarea = true;
                $scope.requiredInput.microarea = false;
                $scope.dataHomeRegistration.microarea = null;
            } else {
                $scope.disabledInput.microarea = false;
                $scope.requiredInput.microarea = true;
            }
        } else if (param == 'localizacao') {
            if (dado == 83) {
                $scope.disabledInput.areaProducaoRural = true;
                $scope.requiredInput.areaProducaoRural = false;
                $scope.dataHomeRegistration.areaProducaoRural = null;
            } else if (dado != 83) {
                if ($scope.dataEditHomeRegistration.tipoDeImovel == 1) {
                    $scope.disabledInput.areaProducaoRural = false;
                    $scope.requiredInput.areaProducaoRural = true;
                } else {
                    $scope.disabledInput.areaProducaoRural = false;
                    $scope.requiredInput.areaProducaoRural = true;
                }
            }
        } else if (param == 'stAnimaisNoDomicilio') {
            if (dado == 1) {
                $scope.disabledInput.Gato = false;
                $scope.disabledInput.Cachorro = false;
                $scope.disabledInput.Passaro = false;
                $scope.disabledInput.Outros_AnimalNoDomicilio = false;
                $scope.disabledInput.quantosAnimaisNoDomicilio = false;
                $scope.requiredInput.Gato = true;
                $scope.requiredInput.Cachorro = true;
                $scope.requiredInput.Passaro = true;
                $scope.requiredInput.Outros_AnimalNoDomicilio = true;
                $scope.requiredInput.quantosAnimaisNoDomicilio = true;
                //$scope.dataHomeRegistration.quantosAnimaisNoDomicilio = 1;
            } else {
                $scope.disabledInput.Gato = true;
                $scope.disabledInput.Cachorro = true;
                $scope.disabledInput.Passaro = true;
                $scope.disabledInput.Outros_AnimalNoDomicilio = true;
                $scope.disabledInput.quantosAnimaisNoDomicilio = true;
                $scope.requiredInput.Gato = false;
                $scope.requiredInput.Cachorro = false;
                $scope.requiredInput.Passaro = false;
                $scope.requiredInput.Outros_AnimalNoDomicilio = false;
                $scope.requiredInput.quantosAnimaisNoDomicilio = false;
                $scope.dataHomeRegistration.Gato = null;
                $scope.dataHomeRegistration.Cachorro = null;
                $scope.dataHomeRegistration.Passaro = null;
                $scope.dataHomeRegistration.Outros_AnimalNoDomicilio = null;
                $scope.dataHomeRegistration.quantosAnimaisNoDomicilio = 0;
            }
        }

    }

    $scope.validationHomeRegistrationDataDictionary = function(objeto) {

        $scope.requiredInput = {
            //Primeira Tela
            cep: true,
            codigoIbgeMunicipio: true,
            numeroDneUf: true,
            bairro: true,
            tipoLogradouroNumeroDne: true,
            nomeLogradouro: true,
            numero: true,
            microarea: true,
            tipoDeImovel: true,

            //Segunda Tela
            situacaoMoradiaPosseTerra: true,
            localizacao: true,
            tipoDomicilio: false,
            areaProducaoRural: true,

            //Terceira Tela
            stAnimaisNoDomicilio: false,
            Gato: false,
            Cachorro: false,
            Passaro: false,
            Outros_AnimalNoDomicilio: false,
            quantosAnimaisNoDomicilio: false,

            //Quarta Tela
            numeroCnsResponsavel: true,

            //Quinta Tela
            nomeResponsavelTecnico: true

        };

        $scope.disabledInput = {
            //Primeira Tela
            codigoIbgeMunicipio: true,
            numeroDneUf: true,
            bairro: true,
            nomeLogradouro: true,

            numero: false,
            microarea: false,

            //Segunda Tela
            situacaoMoradiaPosseTerra: false,
            localizacao: false,
            tipoDomicilio: false,
            nuComodos: false,
            areaProducaoRural: false,
            tipoAcessoDomicilio: false,
            stDisponibilidadeEnergiaEletrica: false,
            materialPredominanteParedesExtDomicilio: false,
            abastecimentoAgua: false,
            formaEscoamentoBanheiro: false,
            aguaConsumoDomicilio: false,
            destinoLixo: false,

            //Terceira Tela
            stAnimaisNoDomicilio: true,
            Gato: true,
            Cachorro: true,
            Passaro: true,
            Outros_AnimalNoDomicilio: true,
            quantosAnimaisNoDomicilio: true,

            //Quarta Tela
            stMudanca: true,

            //Quinta Tela
            nomeInstituicaoPermanencia: true,
            stOutrosProfissionaisVinculados: true,
            nomeResponsavelTecnico: true,
            cnsResponsavelTecnico: true,
            cargoInstituicao: true,
            telefoneResponsavelTecnico: true

        };

        $scope.disabledFamiliesForm = true;

        if (objeto.stSemNumero == 1) {
            $scope.disabledInput.numero = true;
            $scope.requiredInput.numero = false;
        }

        if (objeto.stForaArea == 1) {
            $scope.disabledInput.microarea = true;
            $scope.requiredInput.microarea = false;
        }

        if (objeto.tipoDeImovel == 2 || objeto.tipoDeImovel == 3 || objeto.tipoDeImovel == 4 || objeto.tipoDeImovel == 5 || objeto.tipoDeImovel == 6 || objeto.tipoDeImovel == 12 || objeto.tipoDeImovel == 99) {
            $scope.disabledInput.situacaoMoradiaPosseTerra = true;
            $scope.disabledInput.localizacao = false;
            $scope.disabledInput.tipoDomicilio = false;
            $scope.disabledInput.nuMoradores = false;
            $scope.disabledInput.nuComodos = false;
            $scope.disabledInput.areaProducaoRural = true;
            $scope.disabledInput.tipoAcessoDomicilio = true;
            $scope.disabledInput.stDisponibilidadeEnergiaEletrica = true;
            $scope.disabledInput.materialPredominanteParedesExtDomicilio = true;
            $scope.disabledInput.abastecimentoAgua = true;
            $scope.disabledInput.formaEscoamentoBanheiro = true;
            $scope.disabledInput.aguaConsumoDomicilio = true;
            $scope.disabledInput.destinoLixo = true;
            $scope.requiredInput.situacaoMoradiaPosseTerra = false;
            $scope.requiredInput.localizacao = false;
            $scope.requiredInput.tipoDomicilio = true;
            $scope.requiredInput.nuMoradores = false;
            $scope.requiredInput.nuComodos = false;
            $scope.requiredInput.areaProducaoRural = false;
            $scope.requiredInput.tipoAcessoDomicilio = false;
            $scope.dataHomeRegistration.situacaoMoradiaPosseTerra = null;
            // $scope.dataHomeRegistration.localizacao = null;
            // $scope.dataHomeRegistration.tipoDomicilio = null;
            // $scope.dataHomeRegistration.nuMoradores = null;
            // $scope.dataHomeRegistration.nuComodos = null;
            // $scope.dataHomeRegistration.areaProducaoRural = null;
            $scope.dataHomeRegistration.tipoAcessoDomicilio = null;
            $scope.dataHomeRegistration.stDisponibilidadeEnergiaEletrica = null;
            $scope.dataHomeRegistration.materialPredominanteParedesExtDomicilio = null;
            $scope.dataHomeRegistration.abastecimentoAgua = null;
            $scope.dataHomeRegistration.formaEscoamentoBanheiro = null;
            $scope.dataHomeRegistration.aguaConsumoDomicilio = null;
            $scope.dataHomeRegistration.destinoLixo = null;

            $scope.disabledInput.nomeInstituicaoPermanencia = true;
            $scope.disabledInput.stOutrosProfissionaisVinculados = true;
            $scope.disabledInput.nomeResponsavelTecnico = true;
            $scope.disabledInput.cnsResponsavelTecnico = true;
            $scope.disabledInput.cargoInstituicao = true;
            $scope.disabledInput.telefoneResponsavelTecnico = true;

            $scope.requiredInput.nomeResponsavelTecnico = false;

        } else if (objeto.tipoDeImovel == 7 || objeto.tipoDeImovel == 8 || objeto.tipoDeImovel == 9 || objeto.tipoDeImovel == 10 || objeto.tipoDeImovel == 11) {
            $scope.disabledInput.situacaoMoradiaPosseTerra = true;
            $scope.requiredInput.situacaoMoradiaPosseTerra = false;
            $scope.dataHomeRegistration.situacaoMoradiaPosseTerra = null;
            $scope.disabledInput.tipoDomicilio = true;
            $scope.requiredInput.tipoDomicilio = false;
            $scope.dataHomeRegistration.tipoDomicilio = null;
            $scope.disabledInput.nuComodos = true;
            $scope.dataHomeRegistration.nuComodos = null;
            $scope.disabledInput.areaProducaoRural = true;
            $scope.requiredInput.areaProducaoRural = false;
            $scope.dataHomeRegistration.areaProducaoRural = null;
            $scope.disabledInput.tipoAcessoDomicilio = true;
            $scope.requiredInput.tipoAcessoDomicilio = false;
            $scope.dataHomeRegistration.tipoAcessoDomicilio = null;
            $scope.disabledInput.materialPredominanteParedesExtDomicilio = true;
            $scope.dataHomeRegistration.materialPredominanteParedesExtDomicilio = null;

            //Quinta Tela
            $scope.disabledInput.nomeInstituicaoPermanencia = false;
            $scope.disabledInput.stOutrosProfissionaisVinculados = false;
            $scope.disabledInput.nomeResponsavelTecnico = false;
            $scope.disabledInput.cnsResponsavelTecnico = false;
            $scope.disabledInput.cargoInstituicao = false;
            $scope.disabledInput.telefoneResponsavelTecnico = false;

        } else if (objeto.tipoDeImovel == 1) {
            $scope.disabledInput.stAnimaisNoDomicilio = false;
            $scope.requiredInput.stAnimaisNoDomicilio = true;
            if (objeto.stAnimaisNoDomicilio == 1) {
                $scope.disabledInput.Gato = false;
                $scope.disabledInput.Cachorro = false;
                $scope.disabledInput.Passaro = false;
                $scope.disabledInput.Outros_AnimalNoDomicilio = false;
                $scope.disabledInput.quantosAnimaisNoDomicilio = false;
                $scope.requiredInput.Gato = true;
                $scope.requiredInput.Cachorro = true;
                $scope.requiredInput.Passaro = true;
                $scope.requiredInput.Outros_AnimalNoDomicilio = true;
                $scope.requiredInput.quantosAnimaisNoDomicilio = true;
            }

            $scope.disabledInput.nomeInstituicaoPermanencia = true;
            $scope.disabledInput.stOutrosProfissionaisVinculados = true;
            $scope.disabledInput.nomeResponsavelTecnico = true;
            $scope.disabledInput.cnsResponsavelTecnico = true;
            $scope.disabledInput.cargoInstituicao = true;
            $scope.disabledInput.telefoneResponsavelTecnico = true;

            $scope.requiredInput.nomeResponsavelTecnico = false;
        }

        if (objeto.localizacao == 83) {
            $scope.disabledInput.areaProducaoRural = true;
            $scope.requiredInput.areaProducaoRural = false;
            $scope.dataHomeRegistration.areaProducaoRural = null;
        } else {
            $scope.disabledInput.areaProducaoRural = false;
            $scope.requiredInput.areaProducaoRural = true;
        }

        if (objeto.tipoDeImovel == 1) {
            $scope.disabledInput.stMudanca = false;
        }

    }

    $scope.allFamilies = function(data) {

        if (data != undefined) {
            var condition = 'cadastroDomiciliarId = ' + data + '';
            HomeRegistrationService.getFamilies(condition).then(function(response) {
                $scope.familiaRow = angular.fromJson(response);
                if ($scope.familiaRow.length > 0) {
                    $scope.disabledFamiliesForm = false;
                } else {
                    $scope.disabledFamiliesForm = true;
                }

            }).catch(function(error) {
                console.log(error);
            })
        }
    }

    $scope.allFamilies(angular.fromJson($stateParams.id));

    $scope.searchData = function(data) {
        if (data != undefined) {
            var condition = 'id = ' + data;
            HomeRegistrationService.getHomeRegistration(condition).then(function(response) {
                $scope.dataEditHomeRegistration = response[0];
                $scope.dataValidation();
                $scope.validationHomeRegistrationDataDictionary($scope.dataEditHomeRegistration);
            }).catch(function(error) {
                console.log(error);
            })
        } else {
            $scope.requiredInput = {
                cep: true,
                codigoIbgeMunicipio: true,
                numeroDneUf: true,
                bairro: true,
                tipoLogradouroNumeroDne: true,
                nomeLogradouro: true,
                numero: true,
                microarea: true,
                tipoDeImovel: true,
            }

            $scope.disabledInput = {
                codigoIbgeMunicipio: true,
                numeroDneUf: true,
                bairro: true,
                tipoLogradouroNumeroDne: true,
                nomeLogradouro: true
            }

        }
    }

    $scope.searchData(angular.fromJson($stateParams.id));

    $scope.addAdress = function(dataHome) {
        var dataInformation;



        var str = document.getElementsByName('numero')[0].value;
        var expReg = /^[0-9]+$/;
        var result = str.match(expReg);

        console.log(result);

        if (typeof dataHome != 'undefined') {
            dataInformation = {
                profissionalCNS: $rootScope.userLogged.profissionalCNS,
                cboCodigo_2002: $rootScope.userLogged.cboCodigo_2002,
                cnes: $rootScope.userLogged.cnes,
                ine: $rootScope.userLogged.ine,
                dataAtendimento: new Date().getTime(),
                codigoIbgeMunicipioHeader: $rootScope.userLogged.codigoIbgeMunicipio,

                /* enderecoLocalPermanencia */
                bairro: typeof dataHome.bairro !== 'undefined' ? dataHome.bairro : null,
                cep: typeof dataHome.cep !== 'undefined' ? dataHome.cep : null,
                codigoIbgeMunicipio: typeof dataHome.codigoIbgeMunicipio !== 'undefined' ? $scope.convertToSaveData('codigoIbgeMunicipio', dataHome.codigoIbgeMunicipio) : null,
                complemento: typeof dataHome.complemento !== 'undefined' ? dataHome.complemento : null,
                nomeLogradouro: typeof dataHome.nomeLogradouro !== 'undefined' ? dataHome.nomeLogradouro : null,
                numero: typeof dataHome.numero !== 'undefined' ? dataHome.numero : null,
                numeroDneUf: typeof dataHome.numeroDneUf !== 'undefined' ? dataHome.numeroDneUf : null,
                telefoneContato: typeof dataHome.telefoneContato !== 'undefined' ? dataHome.telefoneContato : null,
                //telelefoneResidencia: typeof dataHome.telelefoneResidencia !== 'undefined' ? dataHome.telelefoneResidencia : null,
                telefoneResidencia: typeof dataHome.telefoneResidencia !== 'undefined' ? dataHome.telefoneResidencia : null,
                tipoLogradouroNumeroDne: typeof dataHome.tipoLogradouroNumeroDne !== 'undefined' ? $scope.convertToSaveData('tipoLogradouroNumeroDne', dataHome.tipoLogradouroNumeroDne) : null,
                stSemNumero: typeof dataHome.stSemNumero !== 'undefined' ? dataHome.stSemNumero : null,
                pontoReferencia: typeof dataHome.pontoReferencia !== 'undefined' ? dataHome.pontoReferencia : null,
                microarea: typeof dataHome.microarea !== 'undefined' ? dataHome.microarea : null,
                stForaArea: typeof dataHome.stForaArea !== 'undefined' ? dataHome.stForaArea : null,
                tipoDeImovel: typeof dataHome.tipoDeImovel !== 'undefined' ? dataHome.tipoDeImovel : null,
                observacao: typeof dataHome.observacao !== 'undefined' ? dataHome.observacao : null,

                /* CondicaoMoradia */
                abastecimentoAgua: null,
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

                /* animaisNoDomicilio*/
                Gato: null,
                Cachorro: null,
                Passaro: null,
                Outros_AnimalNoDomicilio: null,
                quantosAnimaisNoDomicilio: null,
                stAnimaisNoDomicilio: null,

                /* familias */
                dataNascimentoResponsavel: null,
                numeroCnsResponsavel: null,
                numeroMembrosFamilia: null,
                numeroProntuario: null,
                rendaFamiliar: null,
                resideDesde: null,
                stMudanca: null,

                /* statusTermoRecusa*/
                statusTermoRecusa: null,

                /* InstituicaoPermanencia*/
                nomeInstituicaoPermanencia: null,
                stOutrosProfissionaisVinculados: null,
                nomeResponsavelTecnico: null,
                cnsResponsavelTecnico: null,
                cargoInstituicao: null,
                telefoneResponsavelTecnico: null,

                /*status ficha*/
                status: 'Em Edição',

                //Dados internos
                fichaAtualizada: 0,
                uuid: null,
                uuidFichaOriginadora: null,
                token: null,
                latitude: $scope.homeAdressPosition.lat,
                longitude: $scope.homeAdressPosition.long,
                DataRegistro: new Date().getTime()
            };

            if (dataInformation.observacao != null) {
                dataInformation.status = 'Pendente de Edição';
            }
        }

        var invalid = false;
        var message;

        if ($scope.validNoDataHomeRegistration(dataInformation.cep)) {
            invalid = true;
            message = 'O campo CEP é obrigatório'
        }

        if (dataInformation.cep > 9) {
            invalid = true;
            message = 'O campo CEP não está preenchido corretamente'
        }

        if (dataInformation.stSemNumero != 1) {
            if (result == null) {
                invalid = true;
                message = 'O campo número da casa só aceita números.'
            }
            if ($scope.validNoDataHomeRegistration(dataInformation.numero)) {
                invalid = true;
                message = 'O campo número é obrigatório.'
            }
        }

        if ($scope.validNoDataHomeRegistration(dataInformation.complemento)) {
            console.log(dataInformation.complemento);
            dataInformation.complemento = null;
        }

        if (dataInformation.complemento > 30) {
            console.log(dataInformation.complemento);
            invalid = true;
            message = 'O campo Complemento deve ter no máximo 30 caracteres'
        }

        if ($scope.validNoDataHomeRegistration(dataInformation.pontoReferencia)) {
            dataInformation.pontoReferencia = null;
        }

        if (dataInformation.stForaArea != 1) {
            if ($scope.validNoDataHomeRegistration(dataInformation.microarea)) {
                invalid = true;
                message = 'O campo microárea é obrigatório'
            }
        }

        if (dataInformation.tipoDeImovel == null) {
            invalid = true;
            message = 'O tipo de domicílio é obrigatório'
        }

        if (dataInformation.telefoneResidencia != null) {
            var telResidencia = dataInformation.telefoneResidencia;
            if (telResidencia.length > 0 && telResidencia.length != 13) {
                invalid = true;
                message = 'O número da Residência deve conter DDD com dois dígitos + número do telefone fixo com 8 dígitos.';
                console.log('dataInformation.telefoneResidencia = ' + dataInformation.telefoneResidencia);
                console.log('telResidencia = ' + telResidencia + ' - ' + 'telResidencia.length = ' + telResidencia.length);
            }
        }
        // if ($scope.validNoDataHomeRegistration(dataInformation.telefoneResidencia)) {
        //   dataInformation.telefoneResidencia = null;
        // }
        if (dataInformation.telefoneContato != null) {
            var telContato = dataInformation.telefoneContato;
            console.log('telContato = ' + telContato + ' - ' + 'telContato.length = ' + telContato.length);
            if (telContato.length > 0 && telContato.length < 13) {
                invalid = true;
                message = 'O número de Contato deve conter DDD com dois dígitos + número do telefone fixo ou telefone móvel com 8 e 9 dígitos, respectivamente.';
                console.log(dataInformation.telefoneContato);
                console.log(telContato + ' ' + telContato.length);
            }
        }
        // if ($scope.validNoDataHomeRegistration(dataInformation.telefoneContato)) {
        //   dataInformation.telefoneContato = null;
        // }

        if ($scope.validNoDataHomeRegistration(dataInformation.observacao)) {
            dataInformation.observacao = null;
        }

        if (invalid) {

            $ionicPopup.alert({
                title: '<strong>Informativo</strong>',
                template: message
            })
        } else {
            console.log(dataInformation);
            if (angular.fromJson($stateParams.id) == undefined && $rootScope.newHomeResgistration == null) {
                HomeRegistrationService.insertAdressHomeRegistration(dataInformation).then(function(data) {
                    if (data.insertId > 0) {
                        var condition = 'id=' + data.insertId;
                        HomeRegistrationService.getHomeRegistration(condition).then(function(response) {
                            if (response[0] !== "undefined") {
                                $rootScope.newHomeResgistration = response[0];
                                $state.go('domicilios/tabs/add/add-livingconditions', { id: $rootScope.newHomeResgistration.id });
                            } else {
                                alert("Não foi possível salvar")
                            }
                        }).catch(function(error) {
                            console.log('Ocorreu um erro na busca da visita');
                        })
                    }
                }).catch(function(error) {
                    console.log(error);
                })
            } else if (angular.fromJson($stateParams.id) != undefined || $rootScope.newHomeResgistration != null) {

                var condition;

                if (angular.fromJson($stateParams.id) != undefined) {
                    condition = 'id=' + angular.fromJson($stateParams.id);
                } else {
                    condition = 'id=' + $rootScope.newHomeResgistration.id;
                }

                HomeRegistrationService.updateAdress(dataInformation, $scope.homeAdressPosition, condition).then(function(response) {
                    HomeRegistrationService.getHomeRegistration(condition).then(function(response) {
                        if (response[0] !== "undefined") {
                            $rootScope.newHomeResgistration = response[0];
                            $state.go('domicilios/tabs/add/add-livingconditions', { id: $rootScope.newHomeResgistration.id });
                        } else {
                            alert("Não foi possível salvar")
                        }
                    }).catch(function(error) {
                        console.log('Ocorreu um erro na busca da visita');
                    })

                }).catch(function(error) {
                    console.log(error);
                })
            }
        }
    };

    $scope.refuseHomeRegistration = function() {

        var dataInformation = {
            profissionalCNS: $rootScope.userLogged.profissionalCNS,
            cboCodigo_2002: $rootScope.userLogged.cboCodigo_2002,
            cnes: $rootScope.userLogged.cnes,
            ine: $rootScope.userLogged.ine,
            dataAtendimento: new Date().getTime(),
            codigoIbgeMunicipioHeader: $rootScope.userLogged.codigoIbgeMunicipio,
            Justificativa: null, //preencher
            DataRegistro: new Date().getTime(),
            statusTermoRecusa: 1,
            status: 'Aguardando Sincronismo'
        };

        $ionicPopup.confirm({
            // template:
            title: 'Recusa do Cadastro!',
            subTitle: 'Será gerado um número identificador sobre a recusa deste cadastro!',
            buttons: [
                { text: 'Voltar' },
                {
                    text: '<b>Confirmar</b>',
                    type: 'button-positive',
                    onTap: function() {

                        if (angular.fromJson($stateParams.id) != undefined) {

                            var condition = 'id = ' + $rootScope.newHomeResgistration.id;

                            HomeRegistrationService.updateRefuse(dataInformation, condition).then(function(response) {
                                $scope.familiaRow = null;
                                $rootScope.newHomeResgistration = null;
                                $ionicHistory.clearCache();
                                $state.go('app.dashboard/index');
                            }).catch(function(error) {
                                console.log('Não foi possível recusar a visita');
                            })

                        } else {
                            HomeRegistrationService.insertRefuseHomeRegistration(dataInformation).then(function(response) {
                                $scope.familiaRow = null;
                                $rootScope.newHomeResgistration = null;
                                $ionicHistory.clearCache();
                                $state.go('app.dashboard/index', { reload: true });
                            }).catch(function(error) {
                                console.log(error);
                            })
                        }

                    }
                }
            ]
        })
    }

    $scope.addLivingConditions = function(dataHome, param) {

        var dataInformation;

        if (dataHome != 'undefined') {
            dataInformation = {
                situacaoMoradiaPosseTerra: typeof dataHome.situacaoMoradiaPosseTerra === 'undefined' ? null : dataHome.situacaoMoradiaPosseTerra,
                localizacao: typeof dataHome.localizacao === 'undefined' ? null : dataHome.localizacao,
                tipoDomicilio: typeof dataHome.tipoDomicilio === 'undefined' ? null : dataHome.tipoDomicilio,
                nuMoradores: typeof dataHome.nuMoradores === 'undefined' ? null : dataHome.nuMoradores,
                nuComodos: typeof dataHome.nuComodos === 'undefined' ? null : dataHome.nuComodos,
                areaProducaoRural: typeof dataHome.areaProducaoRural === 'undefined' ? null : dataHome.areaProducaoRural,
                tipoAcessoDomicilio: typeof dataHome.tipoAcessoDomicilio === 'undefined' ? null : dataHome.tipoAcessoDomicilio,
                stDisponibilidadeEnergiaEletrica: typeof dataHome.stDisponibilidadeEnergiaEletrica === 'undefined' ? null : dataHome.stDisponibilidadeEnergiaEletrica,
                materialPredominanteParedesExtDomicilio: typeof dataHome.materialPredominanteParedesExtDomicilio === 'undefined' ? null : dataHome.materialPredominanteParedesExtDomicilio,
                abastecimentoAgua: typeof dataHome.abastecimentoAgua === 'undefined' ? null : dataHome.abastecimentoAgua,
                formaEscoamentoBanheiro: typeof dataHome.formaEscoamentoBanheiro === 'undefined' ? null : dataHome.formaEscoamentoBanheiro,
                aguaConsumoDomicilio: typeof dataHome.aguaConsumoDomicilio === 'undefined' ? null : dataHome.aguaConsumoDomicilio,
                destinoLixo: typeof dataHome.destinoLixo === 'undefined' ? null : dataHome.destinoLixo
            }
        }

        var invalid = false;
        var message;


        if ($rootScope.newHomeResgistration.tipoDeImovel == 1 && dataInformation.situacaoMoradiaPosseTerra == null) {
            invalid = true;
            message = 'O campo situação moradia posse de terra é obrigatório';
        }

        if (dataInformation.localizacao == null) {
            invalid = true;
            message = 'O campo localização é obrigatório';
        }

        if ($rootScope.newHomeResgistration.tipoDeImovel == 1 ||
            $rootScope.newHomeResgistration.tipoDeImovel == 2 ||
            $rootScope.newHomeResgistration.tipoDeImovel == 3 ||
            $rootScope.newHomeResgistration.tipoDeImovel == 4 ||
            $rootScope.newHomeResgistration.tipoDeImovel == 5 ||
            $rootScope.newHomeResgistration.tipoDeImovel == 6 ||
            $rootScope.newHomeResgistration.tipoDeImovel == 12 ||
            $rootScope.newHomeResgistration.tipoDeImovel == 99
        ) {


            if (dataInformation.tipoDomicilio == null) {
                invalid = true;
                message = 'O campo tipo de domicílio é obrigatório';
            }

            if (dataInformation.localizacao == 84 && dataInformation.areaProducaoRural == null) {
                invalid = true;
                message = 'O campo tipo área de produção rural é obrigatório';
            }
        }

        if ($scope.validNoDataHomeRegistration(dataInformation.nuMoradores)) {
            invalid = true;
            message = 'O campo número de moradores deve ter valor mínimo 1';
        } else {
            if (dataInformation.nuMoradores < 1) {
                invalid = true;
                message = 'O campo número de moradores deve ter valor mínimo 1';
            }
        }

        if ($scope.validNoDataHomeRegistration(dataInformation.nuComodos)) {
            dataInformation.nuComodos = null;
        }

        if (invalid) {

            $ionicPopup.alert({
                title: '<strong>Informativo</strong>',
                template: message
            })

        } else {

            var condition;

            condition = ' id = ' + $rootScope.newHomeResgistration.id + '';

            HomeRegistrationService.updateLivingConditions(dataInformation, condition)
                .then(function(response) {
                    return FamilyService.getFamilies(' cadastroDomiciliarId = ' + $rootScope.newHomeResgistration.id + ' ');
                })
                .then(function(response) {
                    if (response.length > 0) {
                        FamilyService.updateGenericFamiliaRow(' numeroMembrosFamilia = ' + dataInformation.nuMoradores + ' ', ' cadastroDomiciliarId = ' + $rootScope.newHomeResgistration.id + ' ');
                    }

                    HomeRegistrationService.getHomeRegistration(condition).then(function(response) {
                        if (typeof response[0] !== "undefined") {
                            $rootScope.newHomeResgistration = response[0];
                            if (param == 'return') {
                                $state.go('domicilios/tabs/add/edit-adress', { id: $stateParams.id });
                            } else {
                                $state.go('domicilios/tabs/add/add-animals', { id: $stateParams.id });
                            }

                        } else {
                            alert("Não foi recuperar o retorno do registro")
                        }
                    }).catch(function(error) {
                        console.log('Ocorreu um erro no retorno dos dados');
                    })

                })
                .catch(function(error) {
                    console.log('Ocorreu um erro ao tentar atualizar o registro');
                })

        }
    };

    $scope.addAnimals = function(dataHome, param) {

        var dataInformation;

        if (dataHome != 'undefined') {
            dataInformation = {
                stAnimaisNoDomicilio: typeof dataHome.stAnimaisNoDomicilio === 'undefined' ? null : dataHome.stAnimaisNoDomicilio,
                Gato: typeof dataHome.Gato === 'undefined' ? null : dataHome.Gato,
                Cachorro: typeof dataHome.Cachorro === 'undefined' ? null : dataHome.Cachorro,
                Passaro: typeof dataHome.Passaro === 'undefined' ? null : dataHome.Passaro,
                Outros_AnimalNoDomicilio: typeof dataHome.Outros_AnimalNoDomicilio === 'undefined' ? null : dataHome.Outros_AnimalNoDomicilio,
                quantosAnimaisNoDomicilio: typeof dataHome.quantosAnimaisNoDomicilio === 'undefined' ? null : dataHome.quantosAnimaisNoDomicilio
            }

            if (dataInformation.quantosAnimaisNoDomicilio == 0)
                dataInformation.quantosAnimaisNoDomicilio = null;
        }

        var allAnimals = [];
        allAnimals.push(dataInformation.Gato);
        allAnimals.push(dataInformation.Cachorro);
        allAnimals.push(dataInformation.Passaro);
        allAnimals.push(dataInformation.Outros_AnimalNoDomicilio);

        var pos;
        var tam = 0;
        var qtdAnimais = dataInformation.quantosAnimaisNoDomicilio;

        for (pos = 0; pos <= allAnimals.length - 1; pos++) {
            if (allAnimals[pos] != null) {
                tam++;
            }
        }

        if (dataInformation.stAnimaisNoDomicilio == 1 && tam == 0 ||
            dataInformation.stAnimaisNoDomicilio == 1 && qtdAnimais < tam
        ) {

            $scope.dataHomeRegistration.quantosAnimaisNoDomicilio = 0;

            $ionicPopup.alert({
                title: 'Ocorreu um erro!',
                template: 'Selecione um valor de acordo com os animais selecionados!'
            })

        } else {

            if (dataInformation.quantosAnimaisNoDomicilio != null) {
                dataInformation.quantosAnimaisNoDomicilio = dataInformation.quantosAnimaisNoDomicilio.toString();
            }

            var condition;
            condition = ' id = ' + $rootScope.newHomeResgistration.id + '';

            HomeRegistrationService.updateAnimals(dataInformation, condition).then(function(response) {

                HomeRegistrationService.getHomeRegistration(condition).then(function(response) {
                    if (typeof response[0] !== "undefined") {
                        $rootScope.newHomeResgistration = response[0];
                        if (param == 'return') {
                            $state.go('domicilios/tabs/add/add-livingconditions', { id: angular.fromJson($stateParams.id) });
                        } else {
                            $state.go('domicilios/tabs/add/add-families', { id: angular.fromJson($stateParams.id) });
                        }
                    } else {
                        alert("Não foi recuperar o retorno do registro")
                    }
                }).catch(function(error) {
                    console.log('Ocorreu um erro no retorno dos dados');
                })

            }).catch(function(error) {
                console.log('Ocorreu um erro ao tentar atualizar o registro');
            })

        }

    }

    $scope.deleteFamilie = function(id) {

        var condition = 'id = ' + id;

        HomeRegistrationService.deleteFamilies(condition).then(function(response) {
            $state.reload();
        }).catch(function(error) {
            console.log(error);
        })

    };

    $scope.addFamilyRow = function(dataInformation, param) {

        if (param == 'return') {
            $state.go('domicilios/tabs/add/add-animals', { id: angular.fromJson($stateParams.id) });
        } else {
            $state.go('domicilios/tabs/add/add-institution', { id: angular.fromJson($stateParams.id) });
        }

    }

    $scope.addInstitution = function(dataHome) {

        var dataInformation;

        if (dataHome != 'undefined') {

            dataInformation = {
                nomeInstituicaoPermanencia: typeof dataHome.nomeInstituicaoPermanencia === 'undefined' ? null : dataHome.nomeInstituicaoPermanencia,
                stOutrosProfissionaisVinculados: typeof dataHome.stOutrosProfissionaisVinculados === 'undefined' ? null : dataHome.stOutrosProfissionaisVinculados,
                nomeResponsavelTecnico: typeof dataHome.nomeResponsavelTecnico === 'undefined' ? null : dataHome.nomeResponsavelTecnico,
                cnsResponsavelTecnico: typeof dataHome.cnsResponsavelTecnico === 'undefined' ? null : dataHome.cnsResponsavelTecnico,
                cargoInstituicao: typeof dataHome.cargoInstituicao === 'undefined' ? null : dataHome.cargoInstituicao,
                telefoneResponsavelTecnico: typeof dataHome.telefoneResponsavelTecnico === 'undefined' ? null : dataHome.telefoneResponsavelTecnico
            }
        }

        var invalid = false;
        var message;

        if ($rootScope.newHomeResgistration.tipoDeImovel == 7 ||
            $rootScope.newHomeResgistration.tipoDeImovel == 8 ||
            $rootScope.newHomeResgistration.tipoDeImovel == 9 ||
            $rootScope.newHomeResgistration.tipoDeImovel == 10 ||
            $rootScope.newHomeResgistration.tipoDeImovel == 11
        ) {

            if ($scope.validNoDataHomeRegistration(dataHome.nomeInstituicaoPermanencia)) {
                dataHome.nomeInstituicaoPermanencia = null;
            }

            if ($scope.validNoDataHomeRegistration(dataHome.cnsResponsavelTecnico)) {
                dataHome.cnsResponsavelTecnico = null;
            }

            if ($scope.validNoDataHomeRegistration(dataHome.cargoInstituicao)) {
                dataHome.cargoInstituicao = null;
            }

            if ($scope.validNoDataHomeRegistration(dataHome.telefoneResponsavelTecnico)) {
                dataHome.telefoneResponsavelTecnico = null;
            }

            var nomeResponsavelTecnico;
            nomeResponsavelTecnico = $scope.validNameInstitution(dataHome.nomeResponsavelTecnico, 'Responsável Técnico');
            if (nomeResponsavelTecnico.valid == false) {
                invalid = true;
                message = nomeResponsavelTecnico.mensagem;
            }
        }

        if (invalid) {

            $ionicPopup.alert({
                title: 'Informativo',
                template: message
            })

        } else {

            if ($rootScope.newHomeResgistration.observacao != null) {
                dataInformation.status = 'Pendente de Edição';
            } else {
                dataInformation.status = 'Aguardando Sincronismo';
            }

            var condition;

            condition = ' id = ' + angular.fromJson($stateParams.id) + '';

            HomeRegistrationService.updateInstituition(dataInformation, condition).then(function() {

                $rootScope.newHomeResgistration = null;
                $ionicHistory.clearCache();
                $state.go('app.dashboard/index');

            }).catch(function(error) {
                console.log(error);
                console.log('Ocorreu um erro ao atualizar a Instituição');
            })

        }
    }

});