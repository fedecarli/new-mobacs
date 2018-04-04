angular.module('mobacs')
    .controller('HomeVisitController', function($scope, $ionicPlatform, $location, $rootScope, $state, $ionicHistory, HomeVisitService, ionicDatePicker, $stateParams, $ionicPopup, $cordovaGeolocation, $ionicLoading) {

        $scope.direcionaHomeVisit = function() {
            $rootScope.newHomeVisit = null;
            $state.go('home-visit/add/informations', { reload: true });
        };

        $scope.goToViewHomeVisit = function() {
            var location = $location.path();
            if (location === '/home-visit/add/informations') {
                $state.go('app.dashboard/index', { reload: true });
            } else if (location.search('/home-visit/edit/informations/') != -1) {
                if (typeof $rootScope.flagCNSForReturnHomeVisit != 'undefined' && $rootScope.flagCNSForReturnHomeVisit != null) {
                    $state.go('usersbyhouseholds', { cns: angular.fromJson($rootScope.flagCNSForReturnHomeVisit) });
                } else {
                    $state.go('view/home-visit', { item: angular.fromJson($stateParams.item) });
                }
            } else if (location.search('/home-visit/index') != -1) {
                $state.go('app.dashboard/index', { reload: true });
            } else if (location.search('/home-visit/view/') != -1) {
                $state.go('home-visit/index', { reload: true });
            }
        };

        $scope.goHomeVisitHistory = function() {
            $ionicHistory.goBack();
        };

        $scope.dataHomeVisit = {};

        //BEGIN TEST

        $ionicPlatform.ready(function() {

            $scope.homeVisitPosition = {
                lat: null,
                long: null
            };

            // cordova.plugins.diagnostic.isLocationEnabled(function(enabled){
            //     if(enabled){
            //       var thisLocation = $location.path();
            //       if(thisLocation === '/home-visit/add/informations' || thisLocation.search('/home-visit/edit/informations/') != -1){
            //
            //         $ionicLoading.show({
            //           template: '<ion-spinner icon="bubbles"></ion-spinner><br/>Buscando Localização!'
            //         });
            //
            //         // var posOptions = {timeout: 10000, enableHighAccuracy: false};
            //         $cordovaGeolocation
            //           // .getCurrentPosition(posOptions)
            //           .getCurrentPosition()
            //           .then(function (position) {
            //             $ionicLoading.hide();
            //             $scope.homeVisitPosition = {
            //               lat : position.coords.latitude,
            //               long : position.coords.longitude
            //             };
            // $scope.homeVisitPosition = {
            //   lat : null,
            //   long : null
            // };
            //   console.log(position);
            //   console.log($scope.homeVisitPosition);
            // }, function(err) {
            //   $ionicLoading.hide();
            //   console.log(err);
            // });

            // var watchOptions = {
            //   timeout : 3000,
            //   enableHighAccuracy: false // may cause errors if true
            // };

            // var watch = $cordovaGeolocation.watchPosition(watchOptions);
            // watch.then(
            //   null,
            //   function(err) {
            //     // error
            //     console.log(err);
            //   },
            //   function(position) {
            //     $scope.homeVisitPosition = {
            //       lat : position.coords.latitude,
            //       long : position.coords.longitude
            //     };
            //   });

            //         }
            //       }else{
            //         $ionicPopup.alert({
            //           title: '<strong>Informativo</strong>',
            //           template: 'Por favor, habilite o GPS para utilizar o aplicativo!'
            //         })
            //           .then(function(){
            //             $scope.homeVisitGPSIsItEnabled();
            //           })
            //       }
            //     },
            //     function(error){
            //       console.error("The following error occurred:"+error);
            //     });
            //
        });

        $ionicPlatform.registerBackButtonAction(function(event) {
            event.preventDefault();
            var location = $location.path();
            if (location === '/home-visit/add/informations') {
                $state.go('app.dashboard/index', { reload: true });
            } else if (location.search('/home-visit/index') != -1) {
                $state.go('app.dashboard/index', { reload: true });
            } else if (location.search('/home-visit/view/') != -1) {
                $state.go('home-visit/index', { reload: true });
            } else if (location.search('/home-visit/edit/informations/') != -1) {
                if (typeof $rootScope.flagCNSForReturnHomeVisit != 'undefined' && $rootScope.flagCNSForReturnHomeVisit != null) {
                    $state.go('usersbyhouseholds', { cns: angular.fromJson($rootScope.flagCNSForReturnHomeVisit) });
                } else {
                    $state.go('view/home-visit', { item: angular.fromJson($stateParams.item) });
                }
            } else if (location.search('/home-visit/edit/informations/') != -1) {
                $state.go('view/home-visit', { item: angular.fromJson($stateParams.item) });
            } else if (location.search('/home-visit/add/motive-visit/') != -1) {
                $state.go('home-visit/edit/informations', { item: angular.fromJson($stateParams.item) });
            } else if (location.search('/home-visit/add/anthropometry/') != -1) {
                $state.go('home-visit/add/motive-visit', { item: angular.fromJson($stateParams.item) });
            } else if (location.search('/home-visit/add/outcome/') != -1) {
                $state.go('home-visit/add/anthropometry', { item: angular.fromJson($stateParams.item) });
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

        $scope.homeVisitGPSIsItEnabled = function() {

            cordova.plugins.diagnostic.isLocationEnabled(function(enabled) {
                    if (enabled) {

                        var posOptions = { timeout: 10000, enableHighAccuracy: false };
                        $cordovaGeolocation
                            .getCurrentPosition(posOptions)
                            .then(function(position) {
                                $scope.homeVisitPosition = {
                                    lat: position.coords.latitude,
                                    long: position.coords.longitude
                                };
                                // $scope.homeVisitPosition = {
                                //   lat : null,
                                //   long : null
                                // };
                            }, function(err) {
                                console.log(err);
                            })
                            .catch(function(err) {
                                console.log(err);
                            });

                    } else {
                        $ionicPopup.alert({
                                title: '<strong>Informativo</strong>',
                                template: 'Por favor, habilite o GPS para utilizar o aplicativo!'
                            })
                            .then(function() {
                                $scope.homeVisitGPSIsItEnabled();
                            })
                    }
                },
                function(error) {
                    console.error("The following error occurred:" + error);
                });
        };

        // $scope.homeVisitGPSIsItEnabled();

        //END TEST

        $scope.validValueInArray = function(arr, val) {

            if (arr.indexOf(val) > -1) {
                return true;
            } else {
                return false;
            }
            // function checkVal(tpIm){
            //   return tpIm == val;
            // }

            //o tipo de imóvel é um dos mencionados
            // if(arr.findIndex(checkVal) >= 0) {
            //   return true;
            // }else{
            //   return false;
            // }
        };

        $scope.validNoDataHomeVisit = function(param) {
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

        $scope.returnToInit = function() {
            $state.go('home-visit/edit/informations', { item: angular.fromJson($stateParams.item) });
        };

        $scope.viewToEditHomeVisit = function() {
            if ($rootScope.newHomeVisit.desfecho == 3 || $rootScope.newHomeVisit.desfecho == 2) {
                $ionicPopup.alert({
                    title: '<strong>Informativo</strong>',
                    template: 'Não é possível editar um cadastro que foi recusado/Ausente'
                })
            } else {
                $state.go('home-visit/edit/informations', { item: angular.fromJson($stateParams.item) });
            }
        };

        $scope.clearObservacao = function() {
            $scope.dataHomeVisit.observacao = null;
        };

        $scope.clearJustify = function() {
            $scope.dataHomeVisit.Justificativa = null;
        };

        $scope.validationHomeRegistrationDataDictionary = function(objeto) {

            $scope.requiredInput = {
                turno: true,
                microarea: true,
                stForaArea: false,
                tipoDeImovel: true,
                dtNascimento: false,
                sexo: false,
                Justificativa: false
            };

            $scope.disabledInput = {
                microarea: false,
                numProntuario: true,
                cnsCidadao: true,
                dtNascimento: true,
                sexo: true,

                //Segunda tela
                Cadastramento_Atualizacao: true,
                Visita_periodica: true,
                Consulta: true,
                Exame: true,
                Vacina: true,
                Condicionalidadesdobolsafamilia: true,
                Gestante: true,
                Puerpera: true,
                Recem_nascido: true,
                Crianca: true,
                PessoaDesnutricao: true,
                PessoaReabilitacaoDeficiencia: true,
                Hipertensao: true,
                Diabetes: true,
                Asma: true,
                DPOC_enfisema: true,
                Cancer: true,
                Outras_doencas_cronicas: true,
                Hanseniase: true,
                Tuberculose: true,
                Sintomaticos_Respiratorios: true,
                Tabagista: true,
                Domiciliados_Acamados: true,
                Condicoes_vulnerabilidade_social: true,
                Condicionalidades_bolsa_familia: true,
                Saude_mental: true,
                Usuario_alcool: true,
                Usuario_outras_drogas: true,
                Acao_educativa: true,
                Imovel_com_foco: true,
                Acao_mecanica: true,
                Tratamento_focal: true,
                Egresso_de_internacao: true,
                Convite_atividades_coletivas_campanha_saude: true,
                Orientacao_Prevencao: true,
                Outros: true,
                //Terceira Tela
                pesoAcompanhamentoNutricional: true,
                alturaAcompanhamentoNutricional: true
            };

            if (objeto.stForaArea == 1) {
                $scope.dataHomeVisit.microarea = null;
                $scope.requiredInput.microarea = false;
                $scope.disabledInput.microarea = true;
            } else {
                $scope.requiredInput.microarea = true;
                $scope.disabledInput.microarea = false;
            }

            if (objeto.desfecho != 1) {
                $scope.requiredInput.Justificativa = true;
            }

            if (objeto.tipoDeImovel == 2 || objeto.tipoDeImovel == 3 || objeto.tipoDeImovel == 4 || objeto.tipoDeImovel == 5 || objeto.tipoDeImovel == 6 || objeto.tipoDeImovel == 12) {
                //Primeira Tela
                $scope.dataHomeVisit.numProntuario = null;
                $scope.disabledInput.numProntuario = true;
                $scope.dataHomeVisit.cnsCidadao = null;
                $scope.disabledInput.cnsCidadao = true;
                $scope.dataHomeVisit.dtNascimento = null;
                $scope.requiredInput.dtNascimento = false;
                $scope.disabledInput.dtNascimento = true;
                $scope.dataHomeVisit.sexo = null;
                $scope.requiredInput.sexo = false;
                $scope.disabledInput.sexo = true;
                //Segunda Tela
                $scope.disabledInput.Cadastramento_Atualizacao = false;
                $scope.disabledInput.Acao_educativa = false;
                $scope.disabledInput.Imovel_com_foco = false;
                $scope.disabledInput.Acao_mecanica = false;
                $scope.disabledInput.Tratamento_focal = false;
                $scope.disabledInput.Convite_atividades_coletivas_campanha_saude = false;
                $scope.disabledInput.Orientacao_Prevencao = false;
                $scope.disabledInput.Outros = false;

                //IS NULL
                $scope.dataHomeVisit.Visita_periodica = null;
                $scope.dataHomeVisit.Consulta = null;
                $scope.dataHomeVisit.Exame = null;
                $scope.dataHomeVisit.Vacina = null;
                $scope.dataHomeVisit.Condicionalidadesdobolsafamilia = null;
                $scope.dataHomeVisit.Gestante = null;
                $scope.dataHomeVisit.Puerpera = null;
                $scope.dataHomeVisit.Recem_nascido = null;
                $scope.dataHomeVisit.Crianca = null;
                $scope.dataHomeVisit.PessoaDesnutricao = null;
                $scope.dataHomeVisit.PessoaReabilitacaoDeficiencia = null;
                $scope.dataHomeVisit.Hipertensao = null;
                $scope.dataHomeVisit.Diabetes = null;
                $scope.dataHomeVisit.Asma = null;
                $scope.dataHomeVisit.DPOC_enfisema = null;
                $scope.dataHomeVisit.Cancer = null;
                $scope.dataHomeVisit.Outras_doencas_cronicas = null;
                $scope.dataHomeVisit.Hanseniase = null;
                $scope.dataHomeVisit.Tuberculose = null;
                $scope.dataHomeVisit.Sintomaticos_Respiratorios = null;
                $scope.dataHomeVisit.Tabagista = null;
                $scope.dataHomeVisit.Domiciliados_Acamados = null;
                $scope.dataHomeVisit.Condicoes_vulnerabilidade_social = null;
                $scope.dataHomeVisit.Condicionalidades_bolsa_familia = null;
                $scope.dataHomeVisit.Saude_mental = null;
                $scope.dataHomeVisit.Usuario_alcool = null;
                $scope.dataHomeVisit.Usuario_outras_drogas = null;
                $scope.dataHomeVisit.Egresso_de_internacao = null;
            } else {
                $scope.disabledInput.numProntuario = false;
                $scope.disabledInput.cnsCidadao = false;
                $scope.requiredInput.dtNascimento = true;
                $scope.disabledInput.dtNascimento = false;
                $scope.requiredInput.sexo = true;
                $scope.disabledInput.sexo = false;
                //Segunda Tela
                $scope.disabledInput.Cadastramento_Atualizacao = false;
                $scope.disabledInput.Visita_periodica = false;
                $scope.disabledInput.Consulta = false;
                $scope.disabledInput.Exame = false;
                $scope.disabledInput.Vacina = false;
                $scope.disabledInput.Condicionalidadesdobolsafamilia = false;
                $scope.disabledInput.Gestante = false;
                $scope.disabledInput.Puerpera = false;
                $scope.disabledInput.Recem_nascido = false;
                $scope.disabledInput.Crianca = false;
                $scope.disabledInput.PessoaDesnutricao = false;
                $scope.disabledInput.PessoaReabilitacaoDeficiencia = false;
                $scope.disabledInput.Hipertensao = false;
                $scope.disabledInput.Diabetes = false;
                $scope.disabledInput.Asma = false;
                $scope.disabledInput.DPOC_enfisema = false;
                $scope.disabledInput.Cancer = false;
                $scope.disabledInput.Outras_doencas_cronicas = false;
                $scope.disabledInput.Hanseniase = false;
                $scope.disabledInput.Tuberculose = false;
                $scope.disabledInput.Sintomaticos_Respiratorios = false;
                $scope.disabledInput.Tabagista = false;
                $scope.disabledInput.Domiciliados_Acamados = false;
                $scope.disabledInput.Condicoes_vulnerabilidade_social = false;
                $scope.disabledInput.Condicionalidades_bolsa_familia = false;
                $scope.disabledInput.Saude_mental = false;
                $scope.disabledInput.Usuario_alcool = false;
                $scope.disabledInput.Usuario_outras_drogas = false;
                $scope.disabledInput.Acao_educativa = false;
                $scope.disabledInput.Imovel_com_foco = false;
                $scope.disabledInput.Acao_mecanica = false;
                $scope.disabledInput.Tratamento_focal = false;
                $scope.disabledInput.Egresso_de_internacao = false;
                $scope.disabledInput.Convite_atividades_coletivas_campanha_saude = false;
                $scope.disabledInput.Orientacao_Prevencao = false;
                $scope.disabledInput.Outros = false;
                //Terceira Tela
                $scope.disabledInput.pesoAcompanhamentoNutricional = false;
                $scope.disabledInput.alturaAcompanhamentoNutricional = false;
            }
        };

        $scope.validateForm = function(param, dado) {

            if (param == 'stForaArea') {
                if (dado == 1) {
                    $scope.dataHomeVisit.microarea = null;
                    $scope.requiredInput.microarea = false;
                    $scope.disabledInput.microarea = true;
                } else {
                    $scope.requiredInput.microarea = true;
                    $scope.disabledInput.microarea = false;
                }
            } else if (param == "desfecho") {
                if (dado != 1) {
                    $scope.requiredInput.Justificativa = true;
                }
            } else if (param == 'tipoDeImovel') {
                if (dado == null || dado == 2 || dado == 3 || dado == 4 || dado == 5 || dado == 6 || dado == 12) {
                    $scope.dataHomeVisit.numProntuario = null;
                    $scope.disabledInput.numProntuario = true;
                    $scope.dataHomeVisit.cnsCidadao = null;
                    $scope.disabledInput.cnsCidadao = true;
                    $scope.dataHomeVisit.dtNascimento = null;
                    $scope.requiredInput.dtNascimento = false;
                    $scope.disabledInput.dtNascimento = true;
                    $scope.dataHomeVisit.sexo = null;
                    $scope.requiredInput.sexo = false;
                    $scope.disabledInput.sexo = true;
                } else {
                    $scope.disabledInput.numProntuario = false;
                    $scope.disabledInput.cnsCidadao = false;
                    $scope.requiredInput.dtNascimento = true;
                    $scope.disabledInput.dtNascimento = false;
                    $scope.requiredInput.sexo = true;
                    $scope.disabledInput.sexo = false;
                }
            }
        };

        $scope.searchData = function(data) {
            if (data != undefined) {
                var condition = 'id = ' + data;
                HomeVisitService.getHomeVisit(condition).then(function(homeVisit) {
                    if (typeof homeVisit[0] !== "undefined") {
                        $scope.dataEditHomeVisit = homeVisit[0];
                        $scope.dataValidation();
                        $scope.validationHomeRegistrationDataDictionary($scope.dataEditHomeVisit);
                    } else {
                        alert("Não foi possível encontrar a visita")
                    }
                }).catch(function(err) {
                    alert("Problema na busca da visita");
                });
            } else {
                $scope.requiredInput = {
                    turno: true,
                    microarea: true,
                    stForaArea: false,
                    tipoDeImovel: true,
                    dtNascimento: false,
                    sexo: false,
                    Justificativa: false
                }

                $scope.disabledInput = {}
            }
        };

        $scope.dataValidation = function() {
            $rootScope.newHomeVisit = $scope.dataEditHomeVisit;
            $scope.dataHomeVisit = {
                id: $scope.dataEditHomeVisit.id,
                turno: $scope.dataEditHomeVisit.turno.toString(),
                stForaArea: $scope.dataEditHomeVisit.stForaArea == null ? null : $scope.dataEditHomeVisit.stForaArea,
                microarea: $scope.dataEditHomeVisit.microarea == null ? null : $scope.dataEditHomeVisit.microarea.toString(),
                tipoDeImovel: $scope.dataEditHomeVisit.tipoDeImovel === null ? null : $scope.dataEditHomeVisit.tipoDeImovel.toString(),
                numProntuario: $scope.dataEditHomeVisit.numProntuario === null ? null : $scope.dataEditHomeVisit.numProntuario.toString(),
                cnsCidadao: $scope.dataEditHomeVisit.cnsCidadao === null ? null : $scope.dataEditHomeVisit.cnsCidadao.toString(),
                dtNascimento: $scope.dataEditHomeVisit.dtNascimento == null ? null : parseInt($scope.dataEditHomeVisit.dtNascimento),
                sexo: $scope.dataEditHomeVisit.sexo === null ? null : $scope.dataEditHomeVisit.sexo.toString(),
                statusVisitaCompartilhadaOutroProfissional: $scope.dataEditHomeVisit.statusVisitaCompartilhadaOutroProfissional,
                observacao: $scope.dataEditHomeVisit.observacao,
                /*Motive Visit*/
                Cadastramento_Atualizacao: $scope.dataEditHomeVisit.Cadastramento_Atualizacao,
                Visita_periodica: $scope.dataEditHomeVisit.Visita_periodica,
                Consulta: $scope.dataEditHomeVisit.Consulta,
                Exame: $scope.dataEditHomeVisit.Exame,
                Vacina: $scope.dataEditHomeVisit.Vacina,
                Condicionalidadesdobolsafamilia: $scope.dataEditHomeVisit.Condicionalidadesdobolsafamilia,
                Gestante: $scope.dataEditHomeVisit.Gestante,
                Puerpera: $scope.dataEditHomeVisit.Puerpera,
                Recem_nascido: $scope.dataEditHomeVisit.Recem_nascido,
                Crianca: $scope.dataEditHomeVisit.Crianca,
                PessoaDesnutricao: $scope.dataEditHomeVisit.PessoaDesnutricao,
                PessoaReabilitacaoDeficiencia: $scope.dataEditHomeVisit.PessoaReabilitacaoDeficiencia,
                Hipertensao: $scope.dataEditHomeVisit.Hipertensao,
                Diabetes: $scope.dataEditHomeVisit.Diabetes,
                Asma: $scope.dataEditHomeVisit.Asma,
                DPOC_enfisema: $scope.dataEditHomeVisit.DPOC_enfisema,
                Cancer: $scope.dataEditHomeVisit.Cancer,
                Outras_doencas_cronicas: $scope.dataEditHomeVisit.Outras_doencas_cronicas,
                Hanseniase: $scope.dataEditHomeVisit.Hanseniase,
                Tuberculose: $scope.dataEditHomeVisit.Tuberculose,
                Sintomaticos_Respiratorios: $scope.dataEditHomeVisit.Sintomaticos_Respiratorios,
                Tabagista: $scope.dataEditHomeVisit.Tabagista,
                Domiciliados_Acamados: $scope.dataEditHomeVisit.Domiciliados_Acamados,
                Condicoes_vulnerabilidade_social: $scope.dataEditHomeVisit.Condicoes_vulnerabilidade_social,
                Condicionalidades_bolsa_familia: $scope.dataEditHomeVisit.Condicionalidades_bolsa_familia,
                Saude_mental: $scope.dataEditHomeVisit.Saude_mental,
                Usuario_alcool: $scope.dataEditHomeVisit.Usuario_alcool,
                Usuario_outras_drogas: $scope.dataEditHomeVisit.Usuario_outras_drogas,
                Acao_educativa: $scope.dataEditHomeVisit.Acao_educativa,
                Imovel_com_foco: $scope.dataEditHomeVisit.Imovel_com_foco,
                Acao_mecanica: $scope.dataEditHomeVisit.Acao_mecanica,
                Tratamento_focal: $scope.dataEditHomeVisit.Tratamento_focal,
                Egresso_de_internacao: $scope.dataEditHomeVisit.Egresso_de_internacao,
                Convite_atividades_coletivas_campanha_saude: $scope.dataEditHomeVisit.Convite_atividades_coletivas_campanha_saude,
                Orientacao_Prevencao: $scope.dataEditHomeVisit.Orientacao_Prevencao,
                Outros: $scope.dataEditHomeVisit.Outros,
                /*Anthropometry*/
                pesoAcompanhamentoNutricional: $scope.dataEditHomeVisit.pesoAcompanhamentoNutricional != null ? $scope.dataEditHomeVisit.pesoAcompanhamentoNutricional.toString() : null,
                alturaAcompanhamentoNutricional: $scope.dataEditHomeVisit.alturaAcompanhamentoNutricional != null ? $scope.dataEditHomeVisit.alturaAcompanhamentoNutricional.toString() : null,
                /*Outcome*/
                desfecho: $scope.dataEditHomeVisit.desfecho == null ? null : $scope.dataEditHomeVisit.desfecho.toString(),
                Justificativa: $scope.dataEditHomeVisit.Justificativa
            }

        };

        $scope.searchData(angular.fromJson($stateParams.item));

        // INSERT
        $scope.addInformations = function(dataHomeVisit) {

            var dataInformation;

            if (typeof dataHomeVisit != 'undefined') {

                dataInformation = {
                    profissionalCNS: $rootScope.userLogged.profissionalCNS,
                    cboCodigo_2002: $rootScope.userLogged.cboCodigo_2002,
                    cnes: $rootScope.userLogged.cnes,
                    ine: $rootScope.userLogged.ine,
                    dataAtendimento: new Date(),
                    codigoIbgeMunicipio: $rootScope.userLogged.codigoIbgeMunicipio,
                    turno: typeof dataHomeVisit.turno === 'undefined' ? null : dataHomeVisit.turno,
                    numProntuario: typeof dataHomeVisit.numProntuario === 'undefined' ? null : dataHomeVisit.numProntuario,
                    cnsCidadao: typeof dataHomeVisit.cnsCidadao === 'undefined' ? null : dataHomeVisit.cnsCidadao,
                    dtNascimento: typeof dataHomeVisit.dtNascimento === 'undefined' ? null : dataHomeVisit.dtNascimento,
                    sexo: typeof dataHomeVisit.sexo === 'undefined' ? null : dataHomeVisit.sexo,
                    statusVisitaCompartilhadaOutroProfissional: null,
                    observacao: typeof dataHomeVisit.observacao === 'undefined' ? null : dataHomeVisit.observacao,
                    Cadastramento_Atualizacao: null,
                    Visita_periodica: null,
                    Consulta: null,
                    Exame: null,
                    Vacina: null,
                    Condicionalidadesdobolsafamilia: null,
                    Gestante: null,
                    Puerpera: null,
                    Recem_nascido: null,
                    Crianca: null,
                    PessoaDesnutricao: null,
                    PessoaReabilitacaoDeficiencia: null,
                    Hipertensao: null,
                    Diabetes: null,
                    Asma: null,
                    DPOC_enfisema: null,
                    Cancer: null,
                    Outras_doencas_cronicas: null,
                    Hanseniase: null,
                    Tuberculose: null,
                    Sintomaticos_Respiratorios: null,
                    Tabagista: null,
                    Domiciliados_Acamados: null,
                    Condicoes_vulnerabilidade_social: null,
                    Condicionalidades_bolsa_familia: null,
                    Saude_mental: null,
                    Usuario_alcool: null,
                    Usuario_outras_drogas: null,
                    Acao_educativa: null,
                    Imovel_com_foco: null,
                    Acao_mecanica: null,
                    Tratamento_focal: null,
                    Egresso_de_internacao: null,
                    Convite_atividades_coletivas_campanha_saude: null,
                    Orientacao_Prevencao: null,
                    Outros: null,
                    desfecho: null,
                    microarea: typeof dataHomeVisit.microarea === 'undefined' ? null : dataHomeVisit.microarea,
                    stForaArea: typeof dataHomeVisit.stForaArea === 'undefined' ? null : dataHomeVisit.stForaArea,
                    tipoDeImovel: typeof dataHomeVisit.tipoDeImovel === 'undefined' ? null : dataHomeVisit.tipoDeImovel,
                    pesoAcompanhamentoNutricional: null,
                    alturaAcompanhamentoNutricional: null,
                    status: 'Em Edição',
                    latitude: $scope.homeVisitPosition.lat,
                    longitude: $scope.homeVisitPosition.long,
                    Justificativa: typeof dataHomeVisit.Justificativa === 'undefined' ? null : dataHomeVisit.Justificativa
                };

                if (dataInformation.observacao != null) {
                    dataInformation.status = 'Pendente de Edição';
                }
            }

            var invalid = false,
                mensagem = null;

            //turno
            if ($scope.validNoDataHomeVisit(dataInformation.turno)) {
                invalid = true;
                mensagem = 'O turno é obrigatório';
            }

            //microarea
            if (dataInformation.stForaArea != 1 && $scope.validNoDataHomeVisit(dataInformation.microarea)) {
                invalid = true;
                mensagem = 'A microárea é obrigatória';
            }

            //tipo de imóvel
            if ($scope.validNoDataHomeVisit(dataInformation.tipoDeImovel)) {
                invalid = true;
                mensagem = 'O Tipo de Imóvel é obrigatório';
            }

            //numProntuario
            if ($scope.validNoDataHomeVisit(dataInformation.numProntuario)) {
                dataInformation.numProntuario = null;
            }
            // else{
            //   if(dataInformation.numProntuario.search(/^[A-Za-z0-9]+$/) >= -1){
            //     invalid = true;
            //     mensagem = 'O Número do Prontuário deve conter apenas letras e/ou números';
            //   }
            // }

            //dtNascimento
            if ($scope.validNoDataHomeVisit(dataInformation.dtNascimento)) {
                if ($scope.validValueInArray(['1', '7', '8', '9', '10', '11', '99'], dataInformation.tipoDeImovel)) {
                    invalid = true;
                    mensagem = 'A data de nascimento é obrigatória';
                }

                /*
                 Campo motivosVisita possuir qualquer opção dos grupos #BUSCA_ATIVA ou #ACOMPANHAMENTO inserida;
                 Campo motivosVisita possuir pelo menos uma das opções 25 Egresso de internação ou 31 Orientação / Prevenção inseridas;
                 Pelo menos um dos campos pesoAcompanhamentoNutricional e alturaAcompanhamentoNutricional for preenchido
                 */
            }

            //sexo
            if ($scope.validNoDataHomeVisit(dataInformation.sexo)) {
                if ($scope.validValueInArray(['1', '7', '8', '9', '10', '11', '99'], dataInformation.tipoDeImovel)) {
                    invalid = true;
                    mensagem = 'O sexo é obrigatório';
                }

                /*
                 Campo motivosVisita possuir qualquer opção dos grupos #BUSCA_ATIVA ou #ACOMPANHAMENTO inserida;
                 Campo motivosVisita possuir pelo menos uma das opções 25 Egresso de internação ou 31 Orientação / Prevenção inseridas;
                 Pelo menos um dos campos pesoAcompanhamentoNutricional e alturaAcompanhamentoNutricional estiver preenchido.
                 */
            }

            if ($scope.validNoDataHomeVisit(dataInformation.numProntuario)) {
                dataInformation.numProntuario = null;
            }

            if ($scope.validNoDataHomeVisit(dataInformation.observacao)) {
                dataInformation.observacao = null;
            }

            if (invalid) {
                $ionicPopup.alert({
                    title: '<strong>Informativo</strong>',
                    template: mensagem
                })
            } else {

                if (angular.fromJson($stateParams.item) == undefined && $rootScope.newHomeResgistration == null) {
                    HomeVisitService.addInformationsHomeVisit(dataInformation).then(function(data) {
                        if (data.insertId > 0) {
                            var condition = 'id=' + data.insertId;
                            HomeVisitService.getHomeVisit(condition).then(function(homeVisit) {
                                if (typeof homeVisit[0] !== "undefined") {
                                    $rootScope.newHomeVisit = homeVisit[0];
                                    $state.go('home-visit/add/motive-visit', { item: $rootScope.newHomeVisit.id });
                                } else {
                                    alert("Não foi possível salvar")
                                }
                            }).catch(function(err) {
                                alert("Problema na busca da visita");
                            });
                        } else {
                            alert("Problema ao salvar");
                        }
                    }).catch(function(err) {
                        alert("Problema na execução");
                    });
                } else if (angular.fromJson($stateParams.item) != undefined || $rootScope.newHomeResgistration != null) {

                    var condition;

                    if (angular.fromJson($stateParams.item) != undefined) {
                        condition = 'id=' + angular.fromJson($stateParams.item);
                    } else {
                        condition = 'id=' + $rootScope.newHomeVisit.id;
                    }

                    HomeVisitService.updateEditHomeVisit(dataInformation, $scope.homeVisitPosition, condition)
                        .then(function(result) {
                            HomeVisitService.getHomeVisit(condition).then(function(homeVisit) {
                                if (typeof homeVisit[0] !== "undefined") {
                                    $rootScope.newHomeVisit = homeVisit[0];
                                    $state.go('home-visit/add/motive-visit', { item: $rootScope.newHomeVisit.id });
                                } else {
                                    alert("Não foi possível salvar")
                                }
                            }).catch(function(err) {
                                alert("Problema na busca da visita");
                            });
                        }).catch(function(err) {
                            console.log(err);
                            alert("Problema ao atualizar as informações");
                        });
                }
            }

        };

        // UPDATE MOTIVE-VISIT
        $scope.updateMotiveVisit = function(dataHomeVisit, param) {

            var dataInformation;

            if (typeof dataHomeVisit != 'undefined') {
                dataInformation = {
                    Cadastramento_Atualizacao: typeof dataHomeVisit.Cadastramento_Atualizacao === 'undefined' ? null : dataHomeVisit.Cadastramento_Atualizacao,
                    Visita_periodica: typeof dataHomeVisit.Visita_periodica === 'undefined' ? null : dataHomeVisit.Visita_periodica,
                    Consulta: typeof dataHomeVisit.Consulta === 'undefined' ? null : dataHomeVisit.Consulta,
                    Exame: typeof dataHomeVisit.Exame === 'undefined' ? null : dataHomeVisit.Exame,
                    Vacina: typeof dataHomeVisit.Vacina === 'undefined' ? null : dataHomeVisit.Vacina,
                    Condicionalidadesdobolsafamilia: typeof dataHomeVisit.Condicionalidadesdobolsafamilia === 'undefined' ? null : dataHomeVisit.Condicionalidadesdobolsafamilia,
                    Gestante: typeof dataHomeVisit.Gestante === 'undefined' ? null : dataHomeVisit.Gestante,
                    Puerpera: typeof dataHomeVisit.Puerpera === 'undefined' ? null : dataHomeVisit.Puerpera,
                    Recem_nascido: typeof dataHomeVisit.Recem_nascido === 'undefined' ? null : dataHomeVisit.Recem_nascido,
                    Crianca: typeof dataHomeVisit.Crianca === 'undefined' ? null : dataHomeVisit.Crianca,
                    PessoaDesnutricao: typeof dataHomeVisit.PessoaDesnutricao === 'undefined' ? null : dataHomeVisit.PessoaDesnutricao,
                    PessoaReabilitacaoDeficiencia: typeof dataHomeVisit.PessoaReabilitacaoDeficiencia === 'undefined' ? null : dataHomeVisit.PessoaReabilitacaoDeficiencia,
                    Hipertensao: typeof dataHomeVisit.Hipertensao === 'undefined' ? null : dataHomeVisit.Hipertensao,
                    Diabetes: typeof dataHomeVisit.Diabetes === 'undefined' ? null : dataHomeVisit.Diabetes,
                    Asma: typeof dataHomeVisit.Asma === 'undefined' ? null : dataHomeVisit.Asma,
                    DPOC_enfisema: typeof dataHomeVisit.DPOC_enfisema === 'undefined' ? null : dataHomeVisit.DPOC_enfisema,
                    Cancer: typeof dataHomeVisit.Cancer === 'undefined' ? null : dataHomeVisit.Cancer,
                    Outras_doencas_cronicas: typeof dataHomeVisit.Outras_doencas_cronicas === 'undefined' ? null : dataHomeVisit.Outras_doencas_cronicas,
                    Hanseniase: typeof dataHomeVisit.Hanseniase === 'undefined' ? null : dataHomeVisit.Hanseniase,
                    Tuberculose: typeof dataHomeVisit.Tuberculose === 'undefined' ? null : dataHomeVisit.Tuberculose,
                    Sintomaticos_Respiratorios: typeof dataHomeVisit.Sintomaticos_Respiratorios === 'undefined' ? null : dataHomeVisit.Sintomaticos_Respiratorios,
                    Tabagista: typeof dataHomeVisit.Tabagista === 'undefined' ? null : dataHomeVisit.Tabagista,
                    Domiciliados_Acamados: typeof dataHomeVisit.Domiciliados_Acamados === 'undefined' ? null : dataHomeVisit.Domiciliados_Acamados,
                    Condicoes_vulnerabilidade_social: typeof dataHomeVisit.Condicoes_vulnerabilidade_social === 'undefined' ? null : dataHomeVisit.Condicoes_vulnerabilidade_social,
                    Condicionalidades_bolsa_familia: typeof dataHomeVisit.Condicionalidades_bolsa_familia === 'undefined' ? null : dataHomeVisit.Condicionalidades_bolsa_familia,
                    Saude_mental: typeof dataHomeVisit.Saude_mental === 'undefined' ? null : dataHomeVisit.Saude_mental,
                    Usuario_alcool: typeof dataHomeVisit.Usuario_alcool === 'undefined' ? null : dataHomeVisit.Usuario_alcool,
                    Usuario_outras_drogas: typeof dataHomeVisit.Usuario_outras_drogas === 'undefined' ? null : dataHomeVisit.Usuario_outras_drogas,
                    Acao_educativa: typeof dataHomeVisit.Acao_educativa === 'undefined' ? null : dataHomeVisit.Acao_educativa,
                    Imovel_com_foco: typeof dataHomeVisit.Imovel_com_foco === 'undefined' ? null : dataHomeVisit.Imovel_com_foco,
                    Acao_mecanica: typeof dataHomeVisit.Acao_mecanica === 'undefined' ? null : dataHomeVisit.Acao_mecanica,
                    Tratamento_focal: typeof dataHomeVisit.Tratamento_focal === 'undefined' ? null : dataHomeVisit.Tratamento_focal,
                    Egresso_de_internacao: typeof dataHomeVisit.Egresso_de_internacao === 'undefined' ? null : dataHomeVisit.Egresso_de_internacao,
                    Convite_atividades_coletivas_campanha_saude: typeof dataHomeVisit.Convite_atividades_coletivas_campanha_saude === 'undefined' ? null : dataHomeVisit.Convite_atividades_coletivas_campanha_saude,
                    Orientacao_Prevencao: typeof dataHomeVisit.Orientacao_Prevencao === 'undefined' ? null : dataHomeVisit.Orientacao_Prevencao,
                    Outros: typeof dataHomeVisit.Outros === 'undefined' ? null : dataHomeVisit.Outros
                }
            }

            var allMotiveVisit = [];
            allMotiveVisit.push(dataInformation.Cadastramento_Atualizacao);
            allMotiveVisit.push(dataInformation.Visita_periodica);
            allMotiveVisit.push(dataInformation.Consulta);
            allMotiveVisit.push(dataInformation.Exame);
            allMotiveVisit.push(dataInformation.Vacina);
            allMotiveVisit.push(dataInformation.Condicionalidadesdobolsafamilia);
            allMotiveVisit.push(dataInformation.Gestante);
            allMotiveVisit.push(dataInformation.Puerpera);
            allMotiveVisit.push(dataInformation.Recem_nascido);
            allMotiveVisit.push(dataInformation.Crianca);
            allMotiveVisit.push(dataInformation.PessoaDesnutricao);
            allMotiveVisit.push(dataInformation.PessoaReabilitacaoDeficiencia);
            allMotiveVisit.push(dataInformation.Hipertensao);
            allMotiveVisit.push(dataInformation.Diabetes);
            allMotiveVisit.push(dataInformation.Asma);
            allMotiveVisit.push(dataInformation.DPOC_enfisema);
            allMotiveVisit.push(dataInformation.Cancer);
            allMotiveVisit.push(dataInformation.Outras_doencas_cronicas);
            allMotiveVisit.push(dataInformation.Hanseniase);
            allMotiveVisit.push(dataInformation.Tuberculose);
            allMotiveVisit.push(dataInformation.Sintomaticos_Respiratorios);
            allMotiveVisit.push(dataInformation.Tabagista);
            allMotiveVisit.push(dataInformation.Domiciliados_Acamados);
            allMotiveVisit.push(dataInformation.Condicoes_vulnerabilidade_social);
            allMotiveVisit.push(dataInformation.Condicionalidades_bolsa_familia);
            allMotiveVisit.push(dataInformation.Saude_mental);
            allMotiveVisit.push(dataInformation.Usuario_alcool);
            allMotiveVisit.push(dataInformation.Usuario_outras_drogas);
            allMotiveVisit.push(dataInformation.Acao_educativa);
            allMotiveVisit.push(dataInformation.Imovel_com_foco);
            allMotiveVisit.push(dataInformation.Acao_mecanica);
            allMotiveVisit.push(dataInformation.Tratamento_focal);
            allMotiveVisit.push(dataInformation.Egresso_de_internacao);
            allMotiveVisit.push(dataInformation.Convite_atividades_coletivas_campanha_saude);
            allMotiveVisit.push(dataInformation.Orientacao_Prevencao);
            allMotiveVisit.push(dataInformation.Outros);

            var cont = 0;
            var i = 0;
            for (i = 0; i < allMotiveVisit.length; i++) {
                if (allMotiveVisit[i] != null) {
                    cont++;
                }
            }

            if (cont == 0) {
                $ionicPopup.alert({
                    title: 'Ocorreu um erro!',
                    template: 'É necessário selecionar ao menos um motivo para a vista!'
                })
            } else {
                var condition = ' id = ' + $rootScope.newHomeVisit.id;

                HomeVisitService.updateMotiveVisitHomeVisit(dataInformation, condition).then(function(result) {
                    HomeVisitService.getHomeVisit(condition).then(function(homeVisit) {
                        if (typeof homeVisit[0] !== "undefined") {
                            $rootScope.newHomeVisit = homeVisit[0];
                            if (param == 'return') {
                                $state.go('home-visit/edit/informations', { item: angular.fromJson($stateParams.item) });
                            } else {
                                $state.go('home-visit/add/anthropometry', { item: angular.fromJson($stateParams.item) });
                            }

                        } else {
                            alert("Não foi possível salvar")
                        }
                    }).catch(function(err) {
                        alert("Problema na busca da visita");
                    });

                }).catch(function(err) {
                    console.log(err);
                    alert("Problema ao atualizar o motivo da visita");
                });
            }
        }

        //UPDATE ANTHROPOMETRY
        $scope.updateAnthropometry = function(dataHomeVisit, param) {
            var dataInformation;

            if (typeof dataHomeVisit != 'undefined') {
                dataInformation = {
                    pesoAcompanhamentoNutricional: typeof dataHomeVisit.pesoAcompanhamentoNutricional === 'undefined' ? null : dataHomeVisit.pesoAcompanhamentoNutricional,
                    alturaAcompanhamentoNutricional: typeof dataHomeVisit.alturaAcompanhamentoNutricional === 'undefined' ? null : dataHomeVisit.alturaAcompanhamentoNutricional
                }
            } else {
                dataInformation = {
                    pesoAcompanhamentoNutricional: null,
                    alturaAcompanhamentoNutricional: null
                }
            }

            var invalid = false,
                mensagem = null;

            if (!$scope.validNoDataHomeVisit(dataInformation.pesoAcompanhamentoNutricional)) {
                var pesoAcompNutri = parseFloat(dataInformation.pesoAcompanhamentoNutricional.replace(',', '.'));

                if (pesoAcompNutri < 0.5 || pesoAcompNutri > 500) {
                    invalid = true;
                    mensagem = 'O peso deve estar entre 0,5 e 500 kg.';
                }
            }

            //alturaAcompanhamentoNutricional
            if (!$scope.validNoDataHomeVisit(dataInformation.alturaAcompanhamentoNutricional)) {
                var altAcompNutri = parseFloat(dataInformation.alturaAcompanhamentoNutricional.replace(',', '.'));
                if (altAcompNutri < 20 || altAcompNutri > 250) {
                    invalid = true;
                    mensagem = 'A altura deve estar entre 20 e 250 cm.';
                }
            }

            if (invalid) {
                $ionicPopup.alert({
                    title: '<strong>Informativo</strong>',
                    template: mensagem
                })
            } else {

                var condition = 'id = ' + $rootScope.newHomeVisit.id;

                HomeVisitService.updateAnthropometryHomeVisit(dataInformation, condition).then(function(result) {
                    HomeVisitService.getHomeVisit(condition).then(function(homeVisit) {
                        if (typeof homeVisit[0] !== "undefined") {
                            $rootScope.newHomeVisit = homeVisit[0];
                            if (param == 'return') {
                                $state.go('home-visit/add/motive-visit', { item: angular.fromJson($stateParams.item) });
                            } else {
                                $state.go('home-visit/add/outcome', { item: angular.fromJson($stateParams.item) });
                            }
                        } else {
                            alert("Não foi possível salvar")
                        }
                    }).catch(function(err) {
                        alert("Problema na busca da visita");
                    });
                }).catch(function(err) {
                    alert("Problema ao atualizar a Antropometria");
                });

            }
        };

        //UPDATE OUTCOME
        $scope.updateOutcome = function(dataHomeVisit) {

            var dataInformation;

            if (typeof dataHomeVisit != 'undefined') {
                dataInformation = {
                    desfecho: typeof dataHomeVisit.desfecho === 'undefined' ? null : dataHomeVisit.desfecho,
                    Justificativa: typeof dataHomeVisit.Justificativa === 'undefined' || dataHomeVisit.Justificativa == null ? null : dataHomeVisit.Justificativa.trim(),
                    status: 'Aguardando Sincronismo'
                }
            }

            if ($rootScope.newHomeVisit.observacao != null) {
                dataInformation.status = 'Pendente de Edição';
            }

            if (dataInformation.desfecho == 2 || dataInformation.desfecho == 3) {
                if ($scope.validNoDataHomeVisit(dataInformation.Justificativa)) {
                    $scope.dataHomeVisit.Justificativa = null;
                    $ionicPopup.alert({
                        title: '<strong>Informativo</strong>',
                        template: "A Justificativa é obrigatória para o desfecho informado!"
                    });
                    return false;
                }

                dataInformation.status = 'Aguardando Sincronismo';
            }

            var condition = ' id = ' + $rootScope.newHomeVisit.id;

            HomeVisitService.updateOutcomeHomeVisit(dataInformation, condition).then(function(result) {
                $rootScope.newHomeVisit = null;
                $ionicHistory.clearCache();
                if ($rootScope.flagCNSForReturnHomeVisit != null) {
                    $state.go('usersbyhouseholds', { cns: $rootScope.flagCNSForReturnHomeVisit });
                } else {
                    $state.go('app.dashboard/index');
                }
            }).catch(function(error) {
                alert("Problema ao atualizar o Desfecho da visita");
            });
        };

    });