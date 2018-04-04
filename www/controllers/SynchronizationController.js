angular.module('mobacs').controller('SynchronizationController', function($scope, $http, $location, $ionicHistory, $cordovaNetwork, $ionicPlatform, $ionicPopup, $state, $rootScope, $ionicLoading, PeopleService, HomeRegistrationService, HomeVisitService, FamilyService, WebApiService, SyncronizationService) {

    $scope.initLoading = function() {
        $ionicLoading.show({
            template: 'Transmissão em andamento...',
            duration: 300000
        }).then(function() {});
    };

    $scope.endLoading = function() {
        $ionicLoading.hide()
            .then(function() {});
    };

    $scope.goBackHistory = function() {
        $ionicHistory.goBack();
    };

    $ionicPlatform.ready(function() {

        $scope.typeNetwork = $cordovaNetwork.getNetwork();
        $scope.isNetworknOnline = $cordovaNetwork.isOnline();
        $scope.isNetworkOffline = $cordovaNetwork.isOffline();

        SyncronizationService.selectStatusSyncronization()
            .then(function(response) {
                $scope.status = {
                    peopleSyncronization: response[0].dado,
                    peopleAll: response[1].dado,
                    homeSyncronization: response[2].dado,
                    homeAll: response[3].dado,
                    homeVisitSyncronization: response[4].dado,
                    homeVisitAll: response[5].dado
                };
                return FamilyService.getAllFamilies();
            })
            .catch(function(err) {
                console.log(err);
            });

    });

    //BEGIN
    $ionicPlatform.registerBackButtonAction(function(event) {
        event.preventDefault();
        var location = $location.path();
        if (location === '/synchronization') {
            $state.go('app.dashboard/index');
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

    $scope.removeNullFromArray = function(array) {

        var cont = 0;
        var returnArray = [];

        while (cont < array.length) {
            if (array[cont] != null) returnArray.push(array[cont]);
            cont++;
        }

        return returnArray;
    };

    $scope.validValueInArray = function(arr, val) {
        if (!arr) return false;

        if (arr.indexOf(val) > -1) {
            return true;
        } else {
            return false;
        }
    };
    //END

    $scope.convertToEpoch = function(data, param) {

        var addTimeZone = param;

        // var retornoData = data;

        if (data == null) {
            return null;
        } else {
            data = parseInt(data);
            var localISOTime = null;
            if (addTimeZone) {
                localISOTime = (new Date(data)).toISOString().slice(0, -1);
                localISOTime = '' + localISOTime.substring(0, 12) + '0' + localISOTime.substring(13, 23) + 'Z';
            } else {
                var tzoffset = (new Date()).getTimezoneOffset() * 60000; //offset in milliseconds
                localISOTime = (new Date(data - tzoffset)).toISOString().slice(0, -1);
                localISOTime = '' + localISOTime + 'Z';
            }
            // data = data.toString();
            // data.substring(0,10);
            // data = data /1000;
            return localISOTime;
        }
    };

    $scope.convertEpochToMiliseconds = function(data) {

        if (data == null) {
            return null;
        } else {
            var d = new Date(data.toString());
            data = d.setTime(d.getTime() + d.getTimezoneOffset() * 60000);
            // data = data*1000;
            return data;
        }
    };

    $scope.numberToBoolean = function(nro) {
        if (nro == null) {
            return false;
        } else if (nro == 1) {
            return true;
        } else if (nro == 0) {
            return false;
        }
    };

    $scope.convertCep = function(dado, tipo) {

        var cep = null;

        if (tipo == 'envio') {
            cep = dado.replace('-', '');
            return cep;
        } else {
            return cep;
        }

    };

    $scope.numberLogradouroFormatForJson = function(dado) {

        if (dado == null) {
            return null;
        } else {
            dado = dado.toString();
            if (dado.length == 1) {
                return '00' + dado + '';
            } else if (dado.length == 2) {
                return '0' + dado + '';
            } else {
                return dado;
            }
        }
    };

    $scope.numberUFFormatForJson = function(dado) {

        if (dado == null) {
            return null;
        } else {
            dado = dado.toString();
            if (dado.length == 1) {
                return '0' + dado + '';
            } else {
                return dado;
            }
        }
    };

    $scope.numberFormatForTable = function(dado) {
        if (dado == null) {
            return null;
        } else {
            if (dado.length == 3) {
                if (dado.indexOf("00") == 0) {
                    dado = dado.replace("0", "");
                    dado = dado.replace("0", "");
                    return dado;
                } else if (dado.indexOf("0") == 0) {
                    dado = dado.replace("0", "");
                    return dado;
                } else {
                    return dado;
                }
            } else if (dado.length == 2) {
                if (dado.indexOf("0") == 0) {
                    dado = dado.replace("0", "");
                    return dado;
                } else {
                    return dado;
                }
            } else {
                return dado;
            }
        }
    };

    $scope.cepFormatForTable = function(dado) {
        var partOne = dado.substring(0, 5);
        var partTwo = dado.substring(5, 8);
        var cep = partOne + '-' + partTwo;
        return cep;
    };

    $scope.formatPhoneNumber = function(dado) {

        if (dado == null) {
            return null;
        } else {
            dado = dado.replace("(", "");
            dado = dado.replace(")", "");
            dado = dado.replace("-", "");
            if (dado.length < 11) {
                return null;
            } else {
                return dado;
            }
        }
    };

    $scope.formatStaticPhoneNumber = function(dado) {

        if (dado == null) {
            return null;
        } else {
            dado = dado.replace("(", "");
            dado = dado.replace(")", "");
            dado = dado.replace("-", "");
            if (dado.length < 10) {
                return null;
            } else {
                return dado;
            }
        }
    };

    $scope.formatHomeNumber = function(dado) {

        var numberHome = null;

        if (dado == null) {
            return null;
        } else {
            numberHome = dado.replace(/( )+/g, '');
            return numberHome;
        }
    };

    $scope.phoneFormatForTable = function(dado) {
        // console.log(dado);
        if (dado == null) {
            // console.log(dado + ' ' + 'RETURN NULL');
            return null;
        } else {
            if (dado.length == 10) {
                dado = '(' + dado.substring(0, 2) + ')' + dado.substring(2, 6) + '-' + dado.substring(6, 10);
                // console.log(dado + ' ' + 'dado.length == 10');
                return dado;
            } else if (dado.length >= 11) {
                dado = '(' + dado.substring(0, 2) + ')' + dado.substring(2, 7) + '-' + dado.substring(7, 11);
                // console.log(dado + ' ' + 'dado.length >= 11');
                return dado;
            } else if (dado.length < 10) {
                // console.log(dado + ' ' + 'dado.length < 10');
                return dado;
            }
        }
    };

    $scope.convertToDouble = function(data) {

        if (data != null) {

            data = data.toString();
            data = data.replace(',', '.');

            return data;
        } else {
            return null;
        }

    };

    $scope.headerTransport = function(data) {

        var headerTransport = {
            "profissionalCNS": data.profissionalCNS,
            "cboCodigo_2002": data.cboCodigo_2002,
            "cnes": data.cnes,
            "ine": data.ine,
            "dataAtendimento": null,
            "codigoIbgeMunicipio": data.codigoIbgeMunicipio
        };

        if (data === $rootScope.userLogged) {
            headerTransport.dataAtendimento = $scope.convertToEpoch(new Date().getTime(), false);
        } else {
            headerTransport.dataAtendimento = $scope.convertToEpoch(data.dataAtendimento, false);
        }

        return headerTransport;
    };

    $scope.jsonToUser = function(register) {

        var jsonUser = {
            //Dados ACS
            profissionalCNS: $rootScope.userLogged.profissionalCNS,
            cboCodigo_2002: $rootScope.userLogged.cboCodigo_2002,
            cnes: $rootScope.userLogged.cnes,
            ine: $rootScope.userLogged.ine,
            dataAtendimento: new Date().getTime(),
            codigoIbgeMunicipio: $rootScope.userLogged.codigoIbgeMunicipio,

            cnsCidadao: register.identificacaoUsuarioCidadao.cnsCidadao,
            cnsResponsavelFamiliar: register.identificacaoUsuarioCidadao.cnsResponsavelFamiliar,
            codigoIbgeMunicipioNascimento: register.identificacaoUsuarioCidadao.codigoIbgeMunicipioNascimento,
            dataNascimentoCidadao: $scope.convertEpochToMiliseconds(register.identificacaoUsuarioCidadao.dataNascimentoCidadao),
            desconheceNomeMae: Number(register.identificacaoUsuarioCidadao.desconheceNomeMae),
            desconheceNomePai: Number(register.identificacaoUsuarioCidadao.desconheceNomePai),
            dtEntradaBrasil: $scope.convertEpochToMiliseconds(register.identificacaoUsuarioCidadao.dtEntradaBrasil),
            dtNaturalizacao: $scope.convertEpochToMiliseconds(register.identificacaoUsuarioCidadao.dtNaturalizacao),
            emailCidadao: register.identificacaoUsuarioCidadao.emailCidadao,
            etnia: register.identificacaoUsuarioCidadao.etnia,
            microarea: register.identificacaoUsuarioCidadao.microarea,
            nacionalidadeCidadao: register.identificacaoUsuarioCidadao.nacionalidadeCidadao,
            nomeCidadao: register.identificacaoUsuarioCidadao.nomeCidadao,
            nomeMaeCidadao: register.identificacaoUsuarioCidadao.nomeMaeCidadao,
            nomePaiCidadao: register.identificacaoUsuarioCidadao.nomePaiCidadao,
            nomeSocial: register.identificacaoUsuarioCidadao.nomeSocial,
            numeroNisPisPasep: register.identificacaoUsuarioCidadao.numeroNisPisPasep,
            paisNascimento: register.identificacaoUsuarioCidadao.paisNascimento,
            portariaNaturalizacao: register.identificacaoUsuarioCidadao.portariaNaturalizacao,
            racaCorCidadao: register.identificacaoUsuarioCidadao.racaCorCidadao,
            sexoCidadao: register.identificacaoUsuarioCidadao.sexoCidadao,
            stForaArea: Number(register.identificacaoUsuarioCidadao.stForaArea),
            statusEhResponsavel: Number(register.identificacaoUsuarioCidadao.statusEhResponsavel),
            telefoneCelular: $scope.phoneFormatForTable(register.identificacaoUsuarioCidadao.telefoneCelular),
            CPF: register.identificacaoUsuarioCidadao.CPF,
            RG: register.identificacaoUsuarioCidadao.RG,
            ComplementoRG: register.identificacaoUsuarioCidadao.ComplementoRG,
            beneficiarioBolsaFamilia: Number(register.identificacaoUsuarioCidadao.beneficiarioBolsaFamilia),
            EstadoCivil: register.identificacaoUsuarioCidadao.EstadoCivil,

            //Informações Sócio Demográficas
            relacaoParentescoCidadao: register.informacoesSocioDemograficas.relacaoParentescoCidadao,
            statusFrequentaEscola: Number(register.informacoesSocioDemograficas.statusFrequentaEscola),
            ocupacaoCodigoCbo2002: register.informacoesSocioDemograficas.ocupacaoCodigoCbo2002,
            grauInstrucaoCidadao: register.informacoesSocioDemograficas.grauInstrucaoCidadao,
            situacaoMercadoTrabalhoCidadao: register.informacoesSocioDemograficas.situacaoMercadoTrabalhoCidadao,
            AdultoResponsavelresponsavelPorCrianca: null,
            OutrasCriancasresponsavelPorCrianca: null,
            AdolescenteresponsavelPorCrianca: null,
            SozinharesponsavelPorCrianca: null,
            CrecheresponsavelPorCrianca: null,
            OutroresponsavelPorCrianca: null,
            statusFrequentaBenzedeira: Number(register.informacoesSocioDemograficas.statusFrequentaBenzedeira),
            statusParticipaGrupoComunitario: Number(register.informacoesSocioDemograficas.statusParticipaGrupoComunitario),
            statusPossuiPlanoSaudePrivado: Number(register.informacoesSocioDemograficas.statusPossuiPlanoSaudePrivado),
            statusMembroPovoComunidadeTradicional: Number(register.informacoesSocioDemograficas.statusMembroPovoComunidadeTradicional),
            povoComunidadeTradicional: register.informacoesSocioDemograficas.povoComunidadeTradicional,
            statusDesejaInformarOrientacaoSexual: Number(register.informacoesSocioDemograficas.statusDesejaInformarOrientacaoSexual),
            orientacaoSexualCidadao: register.informacoesSocioDemograficas.orientacaoSexualCidadao,
            statusDesejaInformarIdentidadeGenero: Number(register.informacoesSocioDemograficas.statusDesejaInformarIdentidadeGenero),
            identidadeGeneroCidadao: register.informacoesSocioDemograficas.identidadeGeneroCidadao,
            statusTemAlgumaDeficiencia: Number(register.informacoesSocioDemograficas.statusTemAlgumaDeficiencia),
            AuditivaDeficiencias: null, //Criar vinculos
            VisualDeficiencias: null,
            Intelectual_CognitivaDeficiencias: null,
            FisicaDeficiencias: null,
            OutraDeficiencias: null,

            //Condições de Saúde
            statusEhGestante: Number(register.condicoesDeSaude.statusEhGestante),
            maternidadeDeReferencia: register.condicoesDeSaude.maternidadeDeReferencia,
            situacaoPeso: register.condicoesDeSaude.situacaoPeso,
            statusEhFumante: Number(register.condicoesDeSaude.statusEhFumante),
            statusEhDependenteAlcool: Number(register.condicoesDeSaude.statusEhDependenteAlcool),
            statusEhDependenteOutrasDrogas: Number(register.condicoesDeSaude.statusEhDependenteOutrasDrogas),
            statusTemHipertensaoArterial: Number(register.condicoesDeSaude.statusTemHipertensaoArterial),
            statusTemDiabetes: Number(register.condicoesDeSaude.statusTemDiabetes),
            statusTeveAvcDerrame: Number(register.condicoesDeSaude.statusTeveAvcDerrame),
            statusTeveInfarto: Number(register.condicoesDeSaude.statusTeveInfarto),
            statusTeveDoencaCardiaca: Number(register.condicoesDeSaude.statusTeveDoencaCardiaca),
            Insuficiencia_cardiaca: null, //Criar vinculos
            Outro_Doenca_Cardiaca: null,
            Nao_Sabe_Doenca_Cardiaca: null,
            statusTemTeveDoencasRins: Number(register.condicoesDeSaude.statusTemTeveDoencasRins),
            Insuficiencia_renal: null, //Criar vinculos
            Outro_Doenca_Rins: null,
            Nao_Sabe_Doenca_Rins: null,
            statusTemDoencaRespiratoria: Number(register.condicoesDeSaude.statusTemDoencaRespiratoria),
            Asma: null, //Criar vinculos
            DPOC_Enfisema: null,
            Outro_Doenca_Respiratoria: null,
            Nao_Sabe_Doenca_Respiratoria: null,
            statusTemHanseniase: Number(register.condicoesDeSaude.statusTemHanseniase),
            statusTemTuberculose: Number(register.condicoesDeSaude.statusTemTuberculose),
            statusTemTeveCancer: Number(register.condicoesDeSaude.statusTemTeveCancer),
            statusTeveInternadoem12Meses: Number(register.condicoesDeSaude.statusTeveInternadoem12Meses),
            descricaoCausaInternacaoEm12Meses: register.condicoesDeSaude.descricaoCausaInternacaoEm12Meses,
            statusDiagnosticoMental: Number(register.condicoesDeSaude.statusDiagnosticoMental),
            statusEstaAcamado: Number(register.condicoesDeSaude.statusEstaAcamado),
            statusEstaDomiciliado: Number(register.condicoesDeSaude.statusEstaDomiciliado),
            statusUsaPlantasMedicinais: Number(register.condicoesDeSaude.statusUsaPlantasMedicinais),
            descricaoPlantasMedicinaisUsadas: register.condicoesDeSaude.descricaoPlantasMedicinaisUsadas,
            statusUsaOutrasPraticasIntegrativasOuComplementares: Number(register.condicoesDeSaude.statusUsaOutrasPraticasIntegrativasOuComplementares),
            descricaoOutraCondicao1: register.condicoesDeSaude.descricaoOutraCondicao1,
            descricaoOutraCondicao2: register.condicoesDeSaude.descricaoOutraCondicao2,
            descricaoOutraCondicao3: register.condicoesDeSaude.descricaoOutraCondicao3,

            //Em situação de rua
            statusSituacaoRua: Number(register.emSituacaoDeRua.statusSituacaoRua),
            tempoSituacaoRua: register.emSituacaoDeRua.tempoSituacaoRua,
            statusRecebeBeneficio: Number(register.emSituacaoDeRua.statusRecebeBeneficio),
            statusPossuiReferenciaFamiliar: Number(register.emSituacaoDeRua.statusPossuiReferenciaFamiliar),
            quantidadeAlimentacoesAoDiaSituacaoRua: register.emSituacaoDeRua.quantidadeAlimentacoesAoDiaSituacaoRua,
            Restaurante_popular: null, //Criar vinculos
            Doacao_grupo_religioso: null,
            Doacao_restaurante: null,
            Doacao_popular: null,
            Outros_origemAlimentoSituacaoRua: null,
            statusAcompanhadoPorOutraInstituicao: Number(register.emSituacaoDeRua.statusAcompanhadoPorOutraInstituicao),
            outraInstituicaoQueAcompanha: register.emSituacaoDeRua.outraInstituicaoQueAcompanha,
            statusVisitaFamiliarFrequentemente: Number(register.emSituacaoDeRua.statusVisitaFamiliarFrequentemente),
            grauParentescoFamiliarFrequentado: register.emSituacaoDeRua.grauParentescoFamiliarFrequentado,
            statusTemAcessoHigienePessoalSituacaoRua: Number(register.emSituacaoDeRua.statusTemAcessoHigienePessoalSituacaoRua),
            Banho: null, //Criar vinculos
            Acesso_a_sanitario: null,
            Higiene_bucal: null,
            Outros_higienePessoalSituacaoRua: null,

            // Saída Cidadão do Cadastro
            motivoSaidaCidadao: register.saidaCidadaoCadastro.motivoSaidaCidadao,
            dataObito: $scope.convertEpochToMiliseconds(register.saidaCidadaoCadastro.dataObito),
            numeroDO: register.saidaCidadaoCadastro.numeroDO,

            //Recusa do cadastro
            statusTermoRecusaCadastroIndividualAtencaoBasica: Number(register.statusTermoRecusaCadastroIndividualAtencaoBasica),

            //Ficha atualizada sempre deve vir true
            fichaAtualizada: 1,

            //Status ficha
            status: 'Sincronizada com o servidor',

            //UUID FICHA ORIGINADORA
            uuidFichaOriginadora: register.uuidFichaOriginadora,
            uuid: register.uuid,
            token: register.token,
            observacao: null,
            latitude: null,
            longitude: null,
            Justificativa: register.Justificativa,
            DataRegistro: new Date().getTime()
        };

        if (jsonUser.numeroDO == '999999999') {
            jsonUser.numeroDO = null;
            jsonUser.status = 'Pendente de Edição';
        }

        var tamanho = 0;
        var contador = 0;

        if (register.informacoesSocioDemograficas.responsavelPorCrianca != undefined) {
            tamanho = register.informacoesSocioDemograficas.responsavelPorCrianca.length;
            if (tamanho > 0) {
                for (contador = 0; contador < tamanho; contador++) {
                    if (register.informacoesSocioDemograficas.responsavelPorCrianca[contador] == 1) {
                        jsonUser.AdultoResponsavelresponsavelPorCrianca = 1;
                    }
                    if (register.informacoesSocioDemograficas.responsavelPorCrianca[contador] == 2) {
                        jsonUser.OutrasCriancasresponsavelPorCrianca = 2;
                    }
                    if (register.informacoesSocioDemograficas.responsavelPorCrianca[contador] == 133) {
                        jsonUser.AdolescenteresponsavelPorCrianca = 133;
                    }
                    if (register.informacoesSocioDemograficas.responsavelPorCrianca[contador] == 3) {
                        jsonUser.SozinharesponsavelPorCrianca = 3;
                    }
                    if (register.informacoesSocioDemograficas.responsavelPorCrianca[contador] == 134) {
                        jsonUser.CrecheresponsavelPorCrianca = 134;
                    }
                    if (register.informacoesSocioDemograficas.responsavelPorCrianca[contador] == 4) {
                        jsonUser.OutroresponsavelPorCrianca = 4;
                    }
                }
            }
        }

        tamanho = 0;
        contador = 0;

        if (register.informacoesSocioDemograficas.deficienciasCidadao != undefined) {
            tamanho = register.informacoesSocioDemograficas.deficienciasCidadao.length;
            if (tamanho > 0) {
                for (contador = 0; contador < tamanho; contador++) {
                    if (register.informacoesSocioDemograficas.deficienciasCidadao[contador] == 12) {
                        jsonUser.AuditivaDeficiencias = 12;
                    }
                    if (register.informacoesSocioDemograficas.deficienciasCidadao[contador] == 13) {
                        jsonUser.VisualDeficiencias = 13;
                    }
                    if (register.informacoesSocioDemograficas.deficienciasCidadao[contador] == 14) {
                        jsonUser.Intelectual_CognitivaDeficiencias = 14;
                    }
                    if (register.informacoesSocioDemograficas.deficienciasCidadao[contador] == 15) {
                        jsonUser.FisicaDeficiencias = 15;
                    }
                    if (register.informacoesSocioDemograficas.deficienciasCidadao[contador] == 16) {
                        jsonUser.OutraDeficiencias = 16;
                    }
                }
            }
        }

        if (register.condicoesDeSaude.doencaCardiaca != undefined) {
            tamanho = 0;
            contador = 0;
            tamanho = register.condicoesDeSaude.doencaCardiaca.length;
            if (tamanho > 0) {
                for (contador = 0; contador < tamanho; contador++) {
                    if (register.condicoesDeSaude.doencaCardiaca[contador] == 24) {
                        jsonUser.Insuficiencia_cardiaca = 24;
                    }
                    if (register.condicoesDeSaude.doencaCardiaca[contador] == 25) {
                        jsonUser.Outro_Doenca_Cardiaca = 25;
                    }
                    if (register.condicoesDeSaude.doencaCardiaca[contador] == 26) {
                        jsonUser.Nao_Sabe_Doenca_Cardiaca = 26;
                    }
                }
            }
        }

        if (register.condicoesDeSaude.doencaRins != undefined) {
            tamanho = 0;
            contador = 0;
            tamanho = register.condicoesDeSaude.doencaRins.length;
            if (tamanho > 0) {
                for (contador = 0; contador < tamanho; contador++) {
                    if (register.condicoesDeSaude.doencaRins[contador] == 27) {
                        jsonUser.Insuficiencia_renal = 27;
                    }
                    if (register.condicoesDeSaude.doencaRins[contador] == 28) {
                        jsonUser.Outro_Doenca_Rins = 28;
                    }
                    if (register.condicoesDeSaude.doencaRins[contador] == 29) {
                        jsonUser.Nao_Sabe_Doenca_Rins = 29;
                    }
                }
            }
        }

        if (register.condicoesDeSaude.doencaRespiratoria != undefined) {
            tamanho = 0;
            contador = 0;
            tamanho = register.condicoesDeSaude.doencaRespiratoria.length;
            if (tamanho > 0) {
                for (contador = 0; contador < tamanho; contador++) {
                    if (register.condicoesDeSaude.doencaRespiratoria[contador] == 30) {
                        jsonUser.Asma = 30;
                    }
                    if (register.condicoesDeSaude.doencaRespiratoria[contador] == 31) {
                        jsonUser.DPOC_Enfisema = 31;
                    }
                    if (register.condicoesDeSaude.doencaRespiratoria[contador] == 32) {
                        jsonUser.Outro_Doenca_Respiratoria = 32;
                    }
                    if (register.condicoesDeSaude.doencaRespiratoria[contador] == 33) {
                        jsonUser.Nao_Sabe_Doenca_Respiratoria = 33;
                    }
                }
            }
        }

        if (register.emSituacaoDeRua.origemAlimentoSituacaoRua != undefined) {
            tamanho = 0;
            contador = 0;
            tamanho = register.emSituacaoDeRua.origemAlimentoSituacaoRua.length;
            if (tamanho > 0) {
                for (contador = 0; contador < tamanho; contador++) {
                    if (register.emSituacaoDeRua.origemAlimentoSituacaoRua[contador] == 37) {
                        jsonUser.Restaurante_popular = 37;
                    }
                    if (register.emSituacaoDeRua.origemAlimentoSituacaoRua[contador] == 38) {
                        jsonUser.Doacao_grupo_religioso = 38;
                    }
                    if (register.emSituacaoDeRua.origemAlimentoSituacaoRua[contador] == 39) {
                        jsonUser.Doacao_restaurante = 39;
                    }
                    if (register.emSituacaoDeRua.origemAlimentoSituacaoRua[contador] == 40) {
                        jsonUser.Doacao_popular = 40;
                    }
                    if (register.emSituacaoDeRua.origemAlimentoSituacaoRua[contador] == 41) {
                        jsonUser.Outros_origemAlimentoSituacaoRua = 41;
                    }
                }
            }
        }

        if (register.emSituacaoDeRua.higienePessoalSituacaoRua != undefined) {
            tamanho = 0;
            contador = 0;
            tamanho = register.emSituacaoDeRua.higienePessoalSituacaoRua.length;
            if (tamanho > 0) {
                for (contador = 0; contador < tamanho; contador++) {
                    if (register.emSituacaoDeRua.higienePessoalSituacaoRua[contador] == 42) {
                        jsonUser.Banho = 42;
                    }
                    if (register.emSituacaoDeRua.higienePessoalSituacaoRua[contador] == 43) {
                        jsonUser.Acesso_a_sanitario = 43;
                    }
                    if (register.emSituacaoDeRua.higienePessoalSituacaoRua[contador] == 44) {
                        jsonUser.Higiene_bucal = 44;
                    }
                    if (register.emSituacaoDeRua.higienePessoalSituacaoRua[contador] == 45) {
                        jsonUser.Outros_higienePessoalSituacaoRua = 45;
                    }
                }
            }
        }

        return jsonUser;
    };

    $scope.jsonToAdress = function(register) {
        console.log(register);

        var jsonAdress = {
            profissionalCNS: $rootScope.userLogged.profissionalCNS,
            cboCodigo_2002: $rootScope.userLogged.cboCodigo_2002,
            cnes: $rootScope.userLogged.cnes,
            ine: $rootScope.userLogged.ine,
            dataAtendimento: new Date().getTime(),
            codigoIbgeMunicipioHeader: $rootScope.userLogged.codigoIbgeMunicipio,

            /* enderecoLocalPermanencia */
            bairro: register.enderecoLocalPermanencia.bairro,
            cep: $scope.cepFormatForTable(register.enderecoLocalPermanencia.cep),
            codigoIbgeMunicipio: register.enderecoLocalPermanencia.codigoIbgeMunicipio,
            complemento: register.enderecoLocalPermanencia.complemento,
            nomeLogradouro: register.enderecoLocalPermanencia.nomeLogradouro,
            numero: $scope.formatHomeNumber(register.enderecoLocalPermanencia.numero),
            numeroDneUf: $scope.numberFormatForTable(register.enderecoLocalPermanencia.numeroDneUf),
            telefoneContato: $scope.phoneFormatForTable(register.enderecoLocalPermanencia.telefoneContato),
            telefoneResidencia: $scope.phoneFormatForTable(register.enderecoLocalPermanencia.telefoneResidencia),
            tipoLogradouroNumeroDne: $scope.numberFormatForTable(register.enderecoLocalPermanencia.tipoLogradouroNumeroDne),
            stSemNumero: Number(register.enderecoLocalPermanencia.stSemNumero),
            pontoReferencia: register.enderecoLocalPermanencia.pontoReferencia,
            microarea: register.enderecoLocalPermanencia.microarea,
            stForaArea: Number(register.enderecoLocalPermanencia.stForaArea),
            tipoDeImovel: register.tipoDeImovel,

            /* CondicaoMoradia */
            abastecimentoAgua: register.condicaoMoradia.abastecimentoAgua,
            areaProducaoRural: register.condicaoMoradia.areaProducaoRural,
            destinoLixo: register.condicaoMoradia.destinoLixo,
            formaEscoamentoBanheiro: register.condicaoMoradia.formaEscoamentoBanheiro,
            localizacao: register.condicaoMoradia.localizacao,
            materialPredominanteParedesExtDomicilio: register.condicaoMoradia.materialPredominanteParedesExtDomicilio,
            nuComodos: register.condicaoMoradia.nuComodos,
            nuMoradores: register.condicaoMoradia.nuMoradores,
            situacaoMoradiaPosseTerra: register.condicaoMoradia.situacaoMoradiaPosseTerra,
            stDisponibilidadeEnergiaEletrica: Number(register.condicaoMoradia.stDisponibilidadeEnergiaEletrica),
            tipoAcessoDomicilio: register.condicaoMoradia.tipoAcessoDomicilio,
            tipoDomicilio: register.condicaoMoradia.tipoDomicilio,
            aguaConsumoDomicilio: register.condicaoMoradia.aguaConsumoDomicilio,

            /* animaisNoDomicilio*/
            Gato: null,
            Cachorro: null,
            Passaro: null,
            Outros_AnimalNoDomicilio: null,
            quantosAnimaisNoDomicilio: register.quantosAnimaisNoDomicilio,
            stAnimaisNoDomicilio: Number(register.stAnimaisNoDomicilio),

            /* familias */
            dataNascimentoResponsavel: null,
            numeroCnsResponsavel: null,
            numeroMembrosFamilia: null,
            numeroProntuario: null,
            rendaFamiliar: null,
            resideDesde: null,
            stMudanca: null,
            familiaRow: register.familiaRow,

            /* statusTermoRecusa*/
            statusTermoRecusa: Number(register.statusTermoRecusa),

            /* InstituicaoPermanencia*/
            nomeInstituicaoPermanencia: register.instituicaoPermanencia.nomeInstituicaoPermanencia,
            stOutrosProfissionaisVinculados: Number(register.instituicaoPermanencia.stOutrosProfissionaisVinculados),
            nomeResponsavelTecnico: register.instituicaoPermanencia.nomeResponsavelTecnico,
            cnsResponsavelTecnico: register.instituicaoPermanencia.cnsResponsavelTecnico,
            cargoInstituicao: register.instituicaoPermanencia.cargoInstituicao,
            telefoneResponsavelTecnico: $scope.phoneFormatForTable(register.instituicaoPermanencia.telefoneResponsavelTecnico),

            /*status ficha*/
            status: 'Pendente de Edição',

            /*Dados internos*/
            observacao: null,
            fichaAtualizada: 1,
            uuid: register.uuid,
            uuidFichaOriginadora: register.uuidFichaOriginadora,
            token: register.token,
            latitude: null,
            longitude: null,
            Justificativa: null,
            DataRegistro: new Date().getTime()
        };

        if (register.uuidFichaOriginadora != null) {
            jsonAdress.status = 'Sincronizada com o servidor';
        }

        var tamanho = 0;
        var contador = 0;

        if (register.animalNoDomicilio != undefined) {
            tamanho = register.animalNoDomicilio.length;
            if (tamanho > 0) {
                for (contador = 0; contador < tamanho; contador++) {
                    if (register.animalNoDomicilio[contador] == 128) {
                        jsonAdress.Gato = 128;
                    }
                    if (register.animalNoDomicilio[contador] == 129) {
                        jsonAdress.Cachorro = 129;
                    }
                    if (register.animalNoDomicilio[contador] == 130) {
                        jsonAdress.Passaro = 130;
                    }
                    if (register.animalNoDomicilio[contador] == 132) {
                        jsonAdress.Outros_AnimalNoDomicilio = 132;
                    }
                }
            }
        }

        console.log(jsonAdress);
        return jsonAdress;
    };

    $scope.jsonToFamily = function(register) {
        var jsonFamily = {
            dataNascimentoResponsavel: $scope.convertEpochToMiliseconds(register.dataNascimentoResponsavel),
            numeroCnsResponsavel: register.numeroCnsResponsavel,
            numeroMembrosFamilia: register.numeroMembrosFamilia,
            numeroProntuario: register.numeroProntuario,
            rendaFamiliar: register.rendaFamiliar,
            resideDesde: $scope.convertEpochToMiliseconds(register.resideDesde),
            stMudanca: Number(register.stMudanca)
        };

        return jsonFamily;
    };

    $scope.userToJson = function(mobileAllUsers) {

        var doencaCardiaca = [];
        doencaCardiaca.push(mobileAllUsers.Insuficiencia_cardiaca);
        doencaCardiaca.push(mobileAllUsers.Outro_Doenca_Cardiaca);
        doencaCardiaca.push(mobileAllUsers.Nao_Sabe_Doenca_Cardiaca);

        var doencaRespiratoria = [];
        doencaRespiratoria.push(mobileAllUsers.Asma);
        doencaRespiratoria.push(mobileAllUsers.DPOC_Enfisema);
        doencaRespiratoria.push(mobileAllUsers.Outro_Doenca_Respiratoria);
        doencaRespiratoria.push(mobileAllUsers.Nao_Sabe_Doenca_Respiratoria);

        var doencaRins = [];
        doencaRins.push(mobileAllUsers.Insuficiencia_renal);
        doencaRins.push(mobileAllUsers.Outro_Doenca_Rins);
        doencaRins.push(mobileAllUsers.Nao_Sabe_Doenca_Rins);

        var higienePessoalSituacaoRua = [];
        higienePessoalSituacaoRua.push(mobileAllUsers.Banho);
        higienePessoalSituacaoRua.push(mobileAllUsers.Acesso_a_sanitario);
        higienePessoalSituacaoRua.push(mobileAllUsers.Higiene_bucal);
        higienePessoalSituacaoRua.push(mobileAllUsers.Outros_higienePessoalSituacaoRua);

        var origemAlimentoSituacaoRua = [];
        origemAlimentoSituacaoRua.push(mobileAllUsers.Restaurante_popular);
        origemAlimentoSituacaoRua.push(mobileAllUsers.Doacao_grupo_religioso);
        origemAlimentoSituacaoRua.push(mobileAllUsers.Doacao_restaurante);
        origemAlimentoSituacaoRua.push(mobileAllUsers.Doacao_popular);
        origemAlimentoSituacaoRua.push(mobileAllUsers.Outros_origemAlimentoSituacaoRua);

        var deficienciasCidadao = [];
        deficienciasCidadao.push(mobileAllUsers.AuditivaDeficiencias);
        deficienciasCidadao.push(mobileAllUsers.VisualDeficiencias);
        deficienciasCidadao.push(mobileAllUsers.Intelectual_CognitivaDeficiencias);
        deficienciasCidadao.push(mobileAllUsers.FisicaDeficiencias);
        deficienciasCidadao.push(mobileAllUsers.OutraDeficiencias);

        var responsavelPorCrianca = [];
        responsavelPorCrianca.push(mobileAllUsers.AdultoResponsavelresponsavelPorCrianca);
        responsavelPorCrianca.push(mobileAllUsers.OutrasCriancasresponsavelPorCrianca);
        responsavelPorCrianca.push(mobileAllUsers.AdolescenteresponsavelPorCrianca);
        responsavelPorCrianca.push(mobileAllUsers.SozinharesponsavelPorCrianca);
        responsavelPorCrianca.push(mobileAllUsers.CrecheresponsavelPorCrianca);
        responsavelPorCrianca.push(mobileAllUsers.OutroresponsavelPorCrianca);

        //caso não venha um Estado Civil Válido
        if (!$scope.validValueInArray(['C', 'D', 'P', 'S', 'U', 'V', 'I'], mobileAllUsers.EstadoCivil)) {
            mobileAllUsers.EstadoCivil = 'I'; //seta o estado Inválido padrão
        }

        var userJson = {
            "condicoesDeSaude": {
                "descricaoCausaInternacaoEm12Meses": mobileAllUsers.descricaoCausaInternacaoEm12Meses,
                "descricaoOutraCondicao1": mobileAllUsers.descricaoOutraCondicao1,
                "descricaoOutraCondicao2": mobileAllUsers.descricaoOutraCondicao2,
                "descricaoOutraCondicao3": mobileAllUsers.descricaoOutraCondicao3,
                "descricaoPlantasMedicinaisUsadas": mobileAllUsers.descricaoPlantasMedicinaisUsadas,
                "maternidadeDeReferencia": mobileAllUsers.maternidadeDeReferencia,
                "situacaoPeso": mobileAllUsers.situacaoPeso,
                "statusEhDependenteAlcool": $scope.numberToBoolean(mobileAllUsers.statusEhDependenteAlcool),
                "statusEhDependenteOutrasDrogas": $scope.numberToBoolean(mobileAllUsers.statusEhDependenteOutrasDrogas),
                "statusEhFumante": $scope.numberToBoolean(mobileAllUsers.statusEhFumante),
                "statusEhGestante": $scope.numberToBoolean(mobileAllUsers.statusEhGestante),
                "statusEstaAcamado": $scope.numberToBoolean(mobileAllUsers.statusEstaAcamado),
                "statusEstaDomiciliado": $scope.numberToBoolean(mobileAllUsers.statusEstaDomiciliado),
                "statusTemDiabetes": $scope.numberToBoolean(mobileAllUsers.statusTemDiabetes),
                "statusTemDoencaRespiratoria": $scope.numberToBoolean(mobileAllUsers.statusTemDoencaRespiratoria),
                "statusTemHanseniase": $scope.numberToBoolean(mobileAllUsers.statusTemHanseniase),
                "statusTemHipertensaoArterial": $scope.numberToBoolean(mobileAllUsers.statusTemHipertensaoArterial),
                "statusTemTeveCancer": $scope.numberToBoolean(mobileAllUsers.statusTemTeveCancer),
                "statusTemTeveDoencasRins": $scope.numberToBoolean(mobileAllUsers.statusTemTeveDoencasRins),
                "statusTemTuberculose": $scope.numberToBoolean(mobileAllUsers.statusTemTuberculose),
                "statusTeveAvcDerrame": $scope.numberToBoolean(mobileAllUsers.statusTeveAvcDerrame),
                "statusTeveDoencaCardiaca": $scope.numberToBoolean(mobileAllUsers.statusTeveDoencaCardiaca),
                "statusTeveInfarto": $scope.numberToBoolean(mobileAllUsers.statusTeveInfarto),
                "statusTeveInternadoem12Meses": $scope.numberToBoolean(mobileAllUsers.statusTeveInternadoem12Meses),
                "statusUsaOutrasPraticasIntegrativasOuComplementares": $scope.numberToBoolean(mobileAllUsers.statusUsaOutrasPraticasIntegrativasOuComplementares),
                "statusUsaPlantasMedicinais": $scope.numberToBoolean(mobileAllUsers.statusUsaPlantasMedicinais),
                "statusDiagnosticoMental": $scope.numberToBoolean(mobileAllUsers.statusDiagnosticoMental),
                "doencaCardiaca": $scope.removeNullFromArray(doencaCardiaca),
                "doencaRespiratoria": $scope.removeNullFromArray(doencaRespiratoria),
                "doencaRins": $scope.removeNullFromArray(doencaRins)
            },
            "emSituacaoDeRua": {
                "grauParentescoFamiliarFrequentado": mobileAllUsers.grauParentescoFamiliarFrequentado,
                "outraInstituicaoQueAcompanha": mobileAllUsers.outraInstituicaoQueAcompanha,
                "quantidadeAlimentacoesAoDiaSituacaoRua": mobileAllUsers.quantidadeAlimentacoesAoDiaSituacaoRua,
                "statusAcompanhadoPorOutraInstituicao": $scope.numberToBoolean(mobileAllUsers.statusAcompanhadoPorOutraInstituicao),
                "statusPossuiReferenciaFamiliar": $scope.numberToBoolean(mobileAllUsers.statusPossuiReferenciaFamiliar),
                "statusRecebeBeneficio": $scope.numberToBoolean(mobileAllUsers.statusRecebeBeneficio),
                "statusSituacaoRua": $scope.numberToBoolean(mobileAllUsers.statusSituacaoRua),
                "statusTemAcessoHigienePessoalSituacaoRua": $scope.numberToBoolean(mobileAllUsers.statusTemAcessoHigienePessoalSituacaoRua),
                "statusVisitaFamiliarFrequentemente": $scope.numberToBoolean(mobileAllUsers.statusVisitaFamiliarFrequentemente),
                "tempoSituacaoRua": mobileAllUsers.tempoSituacaoRua,
                "higienePessoalSituacaoRua": $scope.removeNullFromArray(higienePessoalSituacaoRua),
                "origemAlimentoSituacaoRua": $scope.removeNullFromArray(origemAlimentoSituacaoRua)
            },
            "fichaAtualizada": $scope.numberToBoolean(mobileAllUsers.fichaAtualizada),
            "identificacaoUsuarioCidadao": {
                "nomeSocial": mobileAllUsers.nomeSocial,
                "codigoIbgeMunicipioNascimento": mobileAllUsers.codigoIbgeMunicipioNascimento,
                "dataNascimentoCidadao": $scope.convertToEpoch(mobileAllUsers.dataNascimentoCidadao, true),
                "desconheceNomeMae": $scope.numberToBoolean(mobileAllUsers.desconheceNomeMae),
                "emailCidadao": mobileAllUsers.emailCidadao,
                "nacionalidadeCidadao": mobileAllUsers.nacionalidadeCidadao,
                "nomeCidadao": mobileAllUsers.nomeCidadao,
                "nomeMaeCidadao": mobileAllUsers.nomeMaeCidadao,
                "cnsCidadao": mobileAllUsers.cnsCidadao,
                "cnsResponsavelFamiliar": mobileAllUsers.cnsResponsavelFamiliar,
                "CPF": mobileAllUsers.CPF,
                "RG": mobileAllUsers.RG,
                "ComplementoRG": mobileAllUsers.ComplementoRG,
                "telefoneCelular": $scope.formatPhoneNumber(mobileAllUsers.telefoneCelular),
                "beneficiarioBolsaFamilia": mobileAllUsers.beneficiarioBolsaFamilia,
                "numeroNisPisPasep": mobileAllUsers.numeroNisPisPasep,
                "paisNascimento": mobileAllUsers.paisNascimento,
                "racaCorCidadao": mobileAllUsers.racaCorCidadao,
                "EstadoCivil": mobileAllUsers.EstadoCivil,
                "sexoCidadao": mobileAllUsers.sexoCidadao,
                "statusEhResponsavel": $scope.numberToBoolean(mobileAllUsers.statusEhResponsavel),
                "etnia": mobileAllUsers.etnia,
                "nomePaiCidadao": mobileAllUsers.nomePaiCidadao,
                "desconheceNomePai": $scope.numberToBoolean(mobileAllUsers.desconheceNomePai),
                "dtNaturalizacao": $scope.convertToEpoch(mobileAllUsers.dtNaturalizacao, true),
                "portariaNaturalizacao": mobileAllUsers.portariaNaturalizacao,
                "dtEntradaBrasil": $scope.convertToEpoch(mobileAllUsers.dtEntradaBrasil, true),
                "microarea": mobileAllUsers.microarea,
                "stForaArea": $scope.numberToBoolean(mobileAllUsers.stForaArea)
            },
            "informacoesSocioDemograficas": {
                "grauInstrucaoCidadao": mobileAllUsers.grauInstrucaoCidadao,
                "ocupacaoCodigoCbo2002": mobileAllUsers.ocupacaoCodigoCbo2002,
                "orientacaoSexualCidadao": mobileAllUsers.orientacaoSexualCidadao,
                "povoComunidadeTradicional": mobileAllUsers.povoComunidadeTradicional,
                "relacaoParentescoCidadao": mobileAllUsers.relacaoParentescoCidadao,
                "situacaoMercadoTrabalhoCidadao": mobileAllUsers.situacaoMercadoTrabalhoCidadao,
                "statusDesejaInformarOrientacaoSexual": $scope.numberToBoolean(mobileAllUsers.statusDesejaInformarOrientacaoSexual),
                "statusFrequentaBenzedeira": $scope.numberToBoolean(mobileAllUsers.statusFrequentaBenzedeira),
                "statusFrequentaEscola": $scope.numberToBoolean(mobileAllUsers.statusFrequentaEscola),
                "statusMembroPovoComunidadeTradicional": $scope.numberToBoolean(mobileAllUsers.statusMembroPovoComunidadeTradicional),
                "statusParticipaGrupoComunitario": $scope.numberToBoolean(mobileAllUsers.statusParticipaGrupoComunitario),
                "statusPossuiPlanoSaudePrivado": $scope.numberToBoolean(mobileAllUsers.statusPossuiPlanoSaudePrivado),
                "statusTemAlgumaDeficiencia": $scope.numberToBoolean(mobileAllUsers.statusTemAlgumaDeficiencia),
                "identidadeGeneroCidadao": mobileAllUsers.identidadeGeneroCidadao,
                "statusDesejaInformarIdentidadeGenero": $scope.numberToBoolean(mobileAllUsers.statusDesejaInformarIdentidadeGenero),
                "deficienciasCidadao": $scope.removeNullFromArray(deficienciasCidadao),
                "responsavelPorCrianca": $scope.removeNullFromArray(responsavelPorCrianca)
            },
            "statusTermoRecusaCadastroIndividualAtencaoBasica": $scope.numberToBoolean(mobileAllUsers.statusTermoRecusaCadastroIndividualAtencaoBasica),
            "uuidFichaOriginadora": mobileAllUsers.uuid,
            "saidaCidadaoCadastro": {
                "motivoSaidaCidadao": mobileAllUsers.motivoSaidaCidadao,
                "dataObito": $scope.convertToEpoch(mobileAllUsers.dataObito, true),
                "numeroDO": mobileAllUsers.numeroDO
            },
            "latitude": mobileAllUsers.latitude,
            "longitude": mobileAllUsers.longitude,
            "Justificativa": mobileAllUsers.Justificativa,
            "DataRegistro": $scope.convertToEpoch(mobileAllUsers.DataRegistro, false)
        };

        if (userJson.saidaCidadaoCadastro.motivoSaidaCidadao == null &&
            userJson.saidaCidadaoCadastro.dataObito == null &&
            userJson.saidaCidadaoCadastro.numeroDO == null) {
            userJson.saidaCidadaoCadastro = null;
        }

        return userJson;
    };

    $scope.adressToJson = function(mobileAllAdress) {
        console.log(mobileAllAdress);

        var animalNoDomicilio = [];
        animalNoDomicilio.push(mobileAllAdress.Gato);
        animalNoDomicilio.push(mobileAllAdress.Cachorro);
        animalNoDomicilio.push(mobileAllAdress.Passaro);
        animalNoDomicilio.push(mobileAllAdress.Outros_AnimalNoDomicilio);

        var jsonAdress = {
            "condicaoMoradia": {
                "abastecimentoAgua": mobileAllAdress.abastecimentoAgua,
                "areaProducaoRural": mobileAllAdress.areaProducaoRural,
                "destinoLixo": mobileAllAdress.destinoLixo,
                "formaEscoamentoBanheiro": mobileAllAdress.formaEscoamentoBanheiro,
                "localizacao": mobileAllAdress.localizacao,
                "materialPredominanteParedesExtDomicilio": mobileAllAdress.materialPredominanteParedesExtDomicilio,
                "nuComodos": mobileAllAdress.nuComodos,
                "nuMoradores": mobileAllAdress.nuMoradores,
                "situacaoMoradiaPosseTerra": mobileAllAdress.situacaoMoradiaPosseTerra,
                "stDisponibilidadeEnergiaEletrica": $scope.numberToBoolean(mobileAllAdress.stDisponibilidadeEnergiaEletrica),
                "tipoAcessoDomicilio": mobileAllAdress.tipoAcessoDomicilio,
                "tipoDomicilio": mobileAllAdress.tipoDomicilio,
                "aguaConsumoDomicilio": mobileAllAdress.aguaConsumoDomicilio
            },
            "enderecoLocalPermanencia": {
                "bairro": mobileAllAdress.bairro,
                "cep": $scope.convertCep(mobileAllAdress.cep, 'envio'),
                "codigoIbgeMunicipio": mobileAllAdress.codigoIbgeMunicipio,
                "complemento": mobileAllAdress.complemento,
                "nomeLogradouro": mobileAllAdress.nomeLogradouro,
                "numero": mobileAllAdress.numero,
                "numeroDneUf": $scope.numberUFFormatForJson(mobileAllAdress.numeroDneUf),
                "telefoneContato": $scope.formatStaticPhoneNumber(mobileAllAdress.telefoneContato),
                "telefoneResidencia": $scope.formatStaticPhoneNumber(mobileAllAdress.telefoneResidencia),
                "tipoLogradouroNumeroDne": $scope.numberLogradouroFormatForJson(mobileAllAdress.tipoLogradouroNumeroDne),
                "stSemNumero": $scope.numberToBoolean(mobileAllAdress.stSemNumero),
                "pontoReferencia": mobileAllAdress.pontoReferencia,
                "microarea": mobileAllAdress.microarea,
                "stForaArea": $scope.numberToBoolean(mobileAllAdress.stForaArea)
            },
            "fichaAtualizada": $scope.numberToBoolean(mobileAllAdress.fichaAtualizada),
            "quantosAnimaisNoDomicilio": mobileAllAdress.quantosAnimaisNoDomicilio,
            "stAnimaisNoDomicilio": $scope.numberToBoolean(mobileAllAdress.stAnimaisNoDomicilio),
            "statusTermoRecusa": $scope.numberToBoolean(mobileAllAdress.statusTermoRecusa),
            "uuidFichaOriginadora": mobileAllAdress.uuid,
            "tipoDeImovel": mobileAllAdress.tipoDeImovel,
            "instituicaoPermanencia": {
                "nomeInstituicaoPermanencia": mobileAllAdress.nomeInstituicaoPermanencia,
                "stOutrosProfissionaisVinculados": $scope.numberToBoolean(mobileAllAdress.stOutrosProfissionaisVinculados),
                "nomeResponsavelTecnico": mobileAllAdress.nomeResponsavelTecnico,
                "cnsResponsavelTecnico": mobileAllAdress.cnsResponsavelTecnico,
                "cargoInstituicao": mobileAllAdress.cargoInstituicao,
                "telefoneResponsavelTecnico": $scope.formatPhoneNumber(mobileAllAdress.telefoneResponsavelTecnico)
            },
            "animalNoDomicilio": $scope.removeNullFromArray(animalNoDomicilio),
            "familiaRow": [],
            "latitude": mobileAllAdress.latitude,
            "longitude": mobileAllAdress.longitude,
            "Justificativa": mobileAllAdress.Justificativa,
            "DataRegistro": $scope.convertToEpoch(mobileAllAdress.DataRegistro, true)
        };

        if (jsonAdress.instituicaoPermanencia.nomeInstituicaoPermanencia == null &&
            jsonAdress.instituicaoPermanencia.stOutrosProfissionaisVinculados == false &&
            jsonAdress.instituicaoPermanencia.nomeResponsavelTecnico == null &&
            jsonAdress.instituicaoPermanencia.cnsResponsavelTecnico == null &&
            jsonAdress.instituicaoPermanencia.cargoInstituicao == null &&
            jsonAdress.instituicaoPermanencia.telefoneResponsavelTecnico == null) {

            jsonAdress.instituicaoPermanencia = null;
        }

        console.log(jsonAdress);
        return jsonAdress;
    };

    $scope.familyToJson = function(mobileAllFamilies) {
        var jsonFamily = {
            "dataNascimentoResponsavel": $scope.convertToEpoch(mobileAllFamilies.dataNascimentoResponsavel, true),
            "numeroCnsResponsavel": mobileAllFamilies.numeroCnsResponsavel,
            "numeroMembrosFamilia": mobileAllFamilies.numeroMembrosFamilia,
            "numeroProntuario": mobileAllFamilies.numeroProntuario,
            "rendaFamiliar": mobileAllFamilies.rendaFamiliar,
            "resideDesde": $scope.convertToEpoch(mobileAllFamilies.resideDesde, true),
            "stMudanca": $scope.numberToBoolean(mobileAllFamilies.stMudanca)
        };

        return jsonFamily;
    };

    $scope.homeVisitToJson = function(mobileHomeVisit, token) {

        var motivosVisita = [];
        motivosVisita.push(mobileHomeVisit.Cadastramento_Atualizacao);
        motivosVisita.push(mobileHomeVisit.Visita_periodica);
        motivosVisita.push(mobileHomeVisit.Consulta);
        motivosVisita.push(mobileHomeVisit.Exame);
        motivosVisita.push(mobileHomeVisit.Vacina);
        motivosVisita.push(mobileHomeVisit.Condicionalidadesdobolsafamilia);
        motivosVisita.push(mobileHomeVisit.Gestante);
        motivosVisita.push(mobileHomeVisit.Puerpera);
        motivosVisita.push(mobileHomeVisit.Recem_nascido);
        motivosVisita.push(mobileHomeVisit.Crianca);
        motivosVisita.push(mobileHomeVisit.PessoaDesnutricao);
        motivosVisita.push(mobileHomeVisit.PessoaReabilitacaoDeficiencia);
        motivosVisita.push(mobileHomeVisit.Hipertensao);
        motivosVisita.push(mobileHomeVisit.Diabetes);
        motivosVisita.push(mobileHomeVisit.Asma);
        motivosVisita.push(mobileHomeVisit.DPOC_enfisema);
        motivosVisita.push(mobileHomeVisit.Cancer);
        motivosVisita.push(mobileHomeVisit.Outras_doencas_cronicas);
        motivosVisita.push(mobileHomeVisit.Hanseniase);
        motivosVisita.push(mobileHomeVisit.Tuberculose);
        motivosVisita.push(mobileHomeVisit.Sintomaticos_Respiratorios);
        motivosVisita.push(mobileHomeVisit.Tabagista);
        motivosVisita.push(mobileHomeVisit.Domiciliados_Acamados);
        motivosVisita.push(mobileHomeVisit.Condicoes_vulnerabilidade_social);
        motivosVisita.push(mobileHomeVisit.Condicionalidades_bolsa_familia);
        motivosVisita.push(mobileHomeVisit.Saude_mental);
        motivosVisita.push(mobileHomeVisit.Usuario_alcool);
        motivosVisita.push(mobileHomeVisit.Usuario_outras_drogas);
        motivosVisita.push(mobileHomeVisit.Acao_educativa);
        motivosVisita.push(mobileHomeVisit.Imovel_com_foco);
        motivosVisita.push(mobileHomeVisit.Acao_mecanica);
        motivosVisita.push(mobileHomeVisit.Tratamento_focal);
        motivosVisita.push(mobileHomeVisit.Egresso_de_internacao);
        motivosVisita.push(mobileHomeVisit.Convite_atividades_coletivas_campanha_saude);
        motivosVisita.push(mobileHomeVisit.Orientacao_Prevencao);
        motivosVisita.push(mobileHomeVisit.Outros);

        var jsonHomeVisit = {
            "token": token,
            "turno": mobileHomeVisit.turno,
            "numProntuario": mobileHomeVisit.numProntuario,
            "cnsCidadao": mobileHomeVisit.cnsCidadao,
            "dtNascimento": $scope.convertToEpoch(mobileHomeVisit.dtNascimento, true),
            "sexo": mobileHomeVisit.sexo,
            "motivosVisita": $scope.removeNullFromArray(motivosVisita),
            "desfecho": mobileHomeVisit.desfecho,
            "microarea": mobileHomeVisit.microarea,
            "stForaArea": $scope.numberToBoolean(mobileHomeVisit.stForaArea),
            "tipoDeImovel": mobileHomeVisit.tipoDeImovel,
            "pesoAcompanhamentoNutricional": $scope.convertToDouble(mobileHomeVisit.pesoAcompanhamentoNutricional),
            "alturaAcompanhamentoNutricional": $scope.convertToDouble(mobileHomeVisit.alturaAcompanhamentoNutricional),
            "statusVisitaCompartilhadaOutroProfissional": $scope.numberToBoolean(mobileHomeVisit.statusVisitaCompartilhadaOutroProfissional),
            "latitude": mobileHomeVisit.latitude,
            "longitude": mobileHomeVisit.longitude,
            "Justificativa": mobileHomeVisit.Justificativa,
            "DataRegistro": $scope.convertToEpoch(mobileHomeVisit.DataRegistro, true)
        };

        return jsonHomeVisit;
    };

    var indiceUser = 0;
    var indiceUserToMobile = 0;
    var indiceAdress = 0;
    var indiceAdressToMobile = 0;
    var indiceFamily = 0;
    var indiceFamilyToMobile = 0;
    var indiceHomeVisit = 0;
    var familyRow = [];
    var indiceSyncronization = 0;

    $scope.submitFamilies = function(allRegister) {

        var family = allRegister[indiceFamily];

        familyRow.push($scope.familyToJson(family));

        if (indiceFamily < allRegister.length - 1) {
            indiceFamily++;
            return $scope.submitFamilies(allRegister);
        } else {
            indiceFamily = 0;
        }
        return familyRow;
    };

    $scope.insertUsersAPIToMobile = function(allUsers) {

        if (indiceUserToMobile < allUsers.length) {
            var register = allUsers[indiceUserToMobile];
            console.log(register);

            SyncronizationService.selectCheckIfPeopleExist(register.uuidFichaOriginadora) //teste -> uuid
                .then(function(response) {
                    console.log(response);
                    if (response.length == 0) {
                        console.log('VAZIO - não tem uuidFichaOriginadora(download) = uuid(device)');
                        PeopleService.addIdentificationPeople($scope.jsonToUser(register))
                            .then(function(response) {
                                if (indiceUserToMobile < allUsers.length - 1) {
                                    indiceUserToMobile++;
                                    $scope.insertUsersAPIToMobile(allUsers);
                                } else {
                                    indiceUserToMobile = 0;
                                    $scope.getAllSyncronizationAdress();
                                }
                            })
                            .catch(function(err) {
                                WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Insert de Usuários no SQLITE', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                                console.log(err);
                            });
                    } else {
                        // return SyncronizationService.updateRegisterAllPeople($scope.jsonToUser(register),register.uuid);
                        console.log('EXISTE - tem uuidFichaOriginadora(download) = uuid(device)');
                        $scope.endLoading();
                        // $ionicPopup.confirm({
                        //     title: '<strong>Informativo</strong>',
                        //     template: 'Você deseja sobrescrever o cadastro do(a) ' + register.identificacaoUsuarioCidadao.nomeCidadao + ' ?',
                        //     buttons: [{
                        //             text: 'Não',
                        //             type: 'button-positive',
                        //             onTap: function() {
                        //                 //return null;
                        //                 if (indiceUserToMobile < allUsers.length) {
                        //                     indiceUserToMobile++;
                        //                     $scope.insertUsersAPIToMobile(allUsers);
                        //                 } else {
                        //                     indiceUserToMobile = 0;
                        //                     $scope.getAllSyncronizationAdress();
                        //                 }
                        //             }
                        //         },
                        //         {
                        //             text: '<b>Sim</b>',
                        //             type: 'button-positive',
                        //             onTap: function() {
                        ////return SyncronizationService.updateRegisterAllPeople($scope.jsonToUser(register),register.uuid);
                        // console.log(indiceUserToMobile);
                        // console.log(allUsers);
                        // console.log(register);
                        // console.log('apertei sim');

                        // SyncronizationService.deleteFichaAntiga(register.uuidFichaOriginadora)
                        //     .then(function(response) {

                        //         PeopleService.addIdentificationPeople($scope.jsonToUser(register))
                        //             .then(function(response) {
                        //                 if (indiceUserToMobile < allUsers.length - 1) {
                        //                     indiceUserToMobile++;
                        //                     $scope.insertUsersAPIToMobile(allUsers);
                        //                 } else {
                        //                     indiceUserToMobile = 0;
                        //                     $scope.getAllSyncronizationAdress();
                        //                 }
                        //             })
                        //             .catch(function(err) {
                        //                 console.log(err);
                        //             })
                        //     })
                        //     .catch(function(err) {
                        //         WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Insert de Usuários no SQLITE', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                        //         console.log(err);
                        //     })
                        /*
                          // SyncronizationService.updateRegisterAllPeople($scope.jsonToUser(register), register.uuidFichaOriginadora)
                          //     .then(function(response) {
                          //         if (indiceUserToMobile < allUsers.length) {
                          //             console.log('array(allUsers) maior que indiceUserToMobile');
                          //             indiceUserToMobile++;
                          //             $scope.insertUsersAPIToMobile(allUsers);
                          //         } else {
                          //             console.log('array(allUsers) MENOR que indiceUserToMobile');
                          //             indiceUserToMobile = 0;
                          //             $scope.getAllSyncronizationAdress();
                          //         }
                          //     })*/
                        //                 .catch(function(err) {
                        //                     WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Sobreescrita de individuo', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                        //                     console.log(err);
                        //                 })
                        //             }
                        //         }
                        //     ]
                        // });
                        $ionicPopup.confirm({
                            title: '<strong>Informativo</strong>',
                            template: 'O cadastro ' + register.identificacaoUsuarioCidadao.nomeCidadao + ' será sobreescrito!',
                            buttons: [
                                // {
                                //       text: 'Não',
                                //       type: 'button-positive',
                                //       onTap: function() {
                                //           if (indiceUserToMobile < allUsers.length) {
                                //               indiceUserToMobile++;
                                //               $scope.insertUsersAPIToMobile(allUsers);
                                //           } else {
                                //               indiceUserToMobile = 0;
                                //               $scope.getAllSyncronizationAdress();
                                //           }
                                //       }
                                //   },
                                {
                                    text: '<b>Ok</b>',
                                    type: 'button-positive',
                                    onTap: function() {
                                        //return SyncronizationService.updateRegisterAllPeople($scope.jsonToUser(register),register.uuid);
                                        console.log(indiceUserToMobile);
                                        console.log(allUsers);
                                        console.log(register);
                                        console.log('apertei sim');

                                        SyncronizationService.deleteFichaAntiga(register.uuidFichaOriginadora)
                                            .then(function(response) {

                                                PeopleService.addIdentificationPeople($scope.jsonToUser(register))
                                                    .then(function(response) {
                                                        if (indiceUserToMobile < allUsers.length - 1) {
                                                            indiceUserToMobile++;
                                                            $scope.insertUsersAPIToMobile(allUsers);
                                                        } else {
                                                            indiceUserToMobile = 0;
                                                            $scope.getAllSyncronizationAdress();
                                                        }
                                                    })
                                                    .catch(function(err) {
                                                        console.log(err);
                                                    });
                                            })
                                            .catch(function(err) {
                                                WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Insert de Usuários no SQLITE', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                                                console.log(err);
                                            })

                                        // SyncronizationService.updateRegisterAllPeople($scope.jsonToUser(register), register.uuidFichaOriginadora)
                                        //     .then(function(response) {
                                        //         if (indiceUserToMobile < allUsers.length) {
                                        //             console.log('array(allUsers) maior que indiceUserToMobile');
                                        //             indiceUserToMobile++;
                                        //             $scope.insertUsersAPIToMobile(allUsers);
                                        //         } else {
                                        //             console.log('array(allUsers) MENOR que indiceUserToMobile');
                                        //             indiceUserToMobile = 0;
                                        //             $scope.getAllSyncronizationAdress();
                                        //         }
                                        //     })
                                        .catch(function(err) {
                                            WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Sobreescrita de individuo', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                                            console.log(err);
                                        });
                                    }
                                }
                            ]
                        });
                    }
                })
                /*.then(function(response){
                  $scope.initLoading();
                  if(indiceUserToMobile < allUsers.length){
                    indiceUserToMobile++;
                    $scope.insertUsersAPIToMobile(allUsers);
                  }else {
                    indiceUserToMobile = 0;
                    $scope.endLoading();
                  }
                })*/
                .catch(function(err) {
                    WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Verificar se Indivíduo já existe no tablet', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                    console.log(err);
                });
        } else {
            $scope.getAllSyncronizationAdress();
        }
    };

    $scope.getAllSyncronizationUsers = function() {
        $scope.initLoading();
        var token = null;

        WebApiService.getToken($scope.headerTransport($rootScope.userLogged))
            .then(function(response) {
                token = response.token;
                console.log(token);
                return WebApiService.getUsers(token);
            })
            .then(function(response) {
                WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Download Usuários', token), $scope.headerTransport($rootScope.userLogged), response, [], []));
                $scope.insertUsersAPIToMobile(response);
                return WebApiService.closeToken(token);
            })
            .then(function(response) {
                $scope.endLoading();
                $ionicPopup.alert({
                    title: '<strong>Informativo </strong>',
                    template: 'Cadastros Individuais Recebidos'
                });
            })
            .catch(function(error) {
                $scope.endLoading();
                WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Processo de Download de Usuários', error), $scope.headerTransport($rootScope.userLogged), [], [], []));
                $ionicPopup.alert({
                    title: '<strong>Erro: Registros Individuais</strong>',
                    template: error
                }).then(function() {
                    $state.go('app.dashboard/index', { reload: true });
                });
                console.log(error);
                console.log($rootScope.userLogged);
                console.log($scope.headerTransport($rootScope.userLogged));
            });

    };

    $scope.insertFamilyAPIToMobile = function(allFamily, idHomeRegistration, allAdress) {

        var register = allFamily[indiceFamilyToMobile];

        // var condition = 'cadastroDomiciliarId = '+idHomeRegistration+'';
        // FamilyService.getFamilies(condition)
        //   .then(function(response){
        //     if(response.length == 0){
        FamilyService.insertFamily($scope.jsonToFamily(register), idHomeRegistration)
            .then(function(response) {
                // }else{
                //   indiceAdressToMobile++;
                //   indiceFamilyToMobile = 0;
                //   $scope.insertAdressAPIToMobile(allAdress);
                // }
                // })
                // .then(function(response){
                //   if(indiceFamilyToMobile < allFamily.length){
                //     indiceFamilyToMobile++;
                //     $scope.insertFamilyAPIToMobile(allFamily, idHomeRegistration, allAdress);
                //   }else {
                indiceAdressToMobile++;
                indiceFamilyToMobile = 0;
                $scope.insertAdressAPIToMobile(allAdress);
                // }
            })
            .catch(function(err) {
                WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Inserir Família no SQLITE', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                console.log(err);
            });
    };

    $scope.insertAdressAPIToMobile = function(allAdress) {
        console.log(allAdress);
        console.log(indiceAdressToMobile);
        if (indiceAdressToMobile < allAdress.length) {
            var register = allAdress[indiceAdressToMobile];
            var idHomeRegistration = null;

            console.log(register);
            SyncronizationService.selectCheckIfAdressExist(register.uuidFichaOriginadora)
                .then(function(response) {
                    if (response.length == 0) {
                        HomeRegistrationService.insertAdressHomeRegistration($scope.jsonToAdress(register))
                            .then(function(response) {
                                var param;
                                param = response.insertId;
                                $scope.insertFamilyAPIToMobile(register.familiaRow, param, allAdress);
                            })
                            .catch(function(err) {
                                WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Insert de Endereços no SQLITE', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                                console.log(err);
                            });
                    } else {
                        $scope.endLoading();
                        // idHomeRegistration = response[0].id;
                        // return SyncronizationService.updateRegisterAllAdress($scope.jsonToAdress(register), register.uuid);
                        $ionicPopup.confirm({
                            title: '<strong>Informativo</strong>',
                            template: 'O domicílio ' + register.enderecoLocalPermanencia.nomeLogradouro + ', ' + register.enderecoLocalPermanencia.numero + ' será sobreescrito!',
                            buttons: [
                                // {
                                //       text: 'Não',
                                //       type: 'button-positive',
                                //       onTap: function() {
                                //           indiceAdressToMobile++; // NEW
                                //           indiceFamilyToMobile = 0; // NEW
                                //           $scope.insertAdressAPIToMobile(allAdress); // NEW
                                //       }
                                //   },
                                {
                                    // text: '<b>Sim</b>',
                                    text: '<b>Ok</b>',
                                    type: 'button-positive',
                                    onTap: function() {
                                        idHomeRegistration = response[0].id;
                                        console.log(response);
                                        console.log(register);
                                        SyncronizationService.updateRegisterAllAdress($scope.jsonToAdress(register), register.uuidFichaOriginadora)
                                            .then(function(response) {
                                                FamilyService.deleteFamilies('cadastroDomiciliarId = ' + idHomeRegistration + '')
                                                    .then(function(response) {
                                                        $scope.insertFamilyAPIToMobile(register.familiaRow, idHomeRegistration, allAdress);
                                                    })
                                                    .catch(function(err) {
                                                        $scope.endLoading();
                                                        WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Sobreescrever Família', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                                                        $ionicPopup.alert({
                                                            title: '<strong>Erro: Registro de Famílias</strong>',
                                                            template: 'Ocorreu um erro ao tentar sobreescrever as informações da família situada no endereço ' +
                                                                register.enderecoLocalPermanencia.nomeLogradouro + ', ' +
                                                                register.enderecoLocalPermanencia.numero + ''
                                                        });
                                                        console.log(err);
                                                    });
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
                .catch(function(error) {
                    WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro', error), $scope.headerTransport($rootScope.userLogged), [], [], []));
                    console.log(error);
                    $scope.endLoading();
                });
        } else {
            $scope.endLoading();
            $ionicPopup.alert({
                title: '<strong>Informativo</strong>',
                template: 'Encerrada a conexão com o servidor, verifique os dados no tablet'
            }).then(function() {
                $state.go('app.dashboard/index', { reload: true });
            });
        }
    };

    $scope.getAllSyncronizationAdress = function() {
        $scope.initLoading();
        var token = null;

        WebApiService.getToken($scope.headerTransport($rootScope.userLogged))
            .then(function(response) {
                token = response.token;
                return WebApiService.getAdress(token);
            })
            .then(function(response) {
                WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Download Endereços', token), $scope.headerTransport($rootScope.userLogged), [], response, []));
                $scope.insertAdressAPIToMobile(response);
                return WebApiService.closeToken(token);
            })
            .then(function(response) {})
            .catch(function(err) {
                $scope.endLoading();
                WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Processo de Download de Endereços', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                var message = null;
                if (typeof err.ExceptionMessage != 'undefined') {
                    message = err.ExceptionMessage;
                } else {
                    message = err;
                }
                $ionicPopup.alert({
                    title: '<strong>Erro: Registros Domiciliares</strong>',
                    template: message
                });
                console.log(err);
            });

    };

    $scope.submitAllSyncronizationUsers = function(allRegister) {
        $scope.initLoading();
        var user = allRegister[indiceUser];
        var token = null;

        WebApiService.getToken($scope.headerTransport(user))
            .then(function(response) {
                token = response.token;
                var userJson = $scope.userToJson(user, token);
                return WebApiService.submitUser(userJson);
            })
            .then(function(response) {
                var condition = 'id = ' + user.id;
                return PeopleService.deletePeopleToAPI(condition);
            })
            .then(function(response) {
                return WebApiService.closeToken(token);
            })
            .then(function(response) {
                if (indiceUser < allRegister.length - 1) {
                    indiceUser++;
                    return $scope.submitAllSyncronizationUsers(allRegister);
                } else {
                    indiceUser = 0;
                    $scope.submitRegisterToAPI();
                }
            })
            .catch(function(err) {
                $scope.endLoading();
                WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Processo de Envio de Usuários', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                var message = null;
                console.log(err);
                if (typeof err.ExceptionMessage != 'undefined') {
                    message = err.ExceptionMessage;
                } else {
                    message = '' + user.nomeCidadao + ' <br> ' + err + '';
                }

                console.log(message);
                console.log(user.nomeCidadao);
                console.error(err);

                $ionicPopup.alert({
                    title: '<strong>Ocorreu um erro no Servidor</strong>',
                    template: message
                }).then(function() {
                    $state.go('app.dashboard/index', { reload: true });
                });
                console.log(err);
            });
    };

    $scope.submitAllSyncronizationAdress = function(allRegister) {
        $scope.initLoading();
        var adress = allRegister[indiceAdress];
        var token = null;
        var condition = null;
        var adressJson = null;

        WebApiService.getToken($scope.headerTransport(adress))
            .then(function(response) {
                token = response.token;
                adressJson = $scope.adressToJson(adress, token);
                condition = 'cadastroDomiciliarId = ' + adress.id;
                return SyncronizationService.selectFamiliesToAPI(condition);
            })
            .then(function(response) {
                adressJson.familiaRow = $scope.submitFamilies(response);
                return WebApiService.submitAdress(adressJson);
            })
            .then(function(response) {
                var condition = 'id = ' + adress.id;
                return SyncronizationService.deleteAdressToAPI(condition);
            })
            .then(function(response) {
                condition = 'cadastroDomiciliarId = ' + adress.id;
                return SyncronizationService.deleteFamiliesToAPI(condition);
            })
            .then(function(response) {
                return WebApiService.closeToken(token);
            })
            .then(function(response) {
                if (indiceAdress < allRegister.length - 1) {
                    indiceAdress++;
                    return $scope.submitAllSyncronizationAdress(allRegister);
                } else {
                    $scope.endLoading();
                    indiceAdress = 0;
                    $ionicPopup.alert({
                        title: 'Transmissão',
                        template: 'Dados transmitidos com sucesso'
                    }).then(function() {
                        $state.go('app.dashboard/index', { reload: true });
                    });
                }
            })
            .catch(function(err) {
                $scope.endLoading();
                WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Processo de Envio de Endereços', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                var message = null;
                console.log(err);
                if (typeof err.ExceptionMessage != 'undefined') {
                    message += err.ExceptionMessage;
                } else {
                    message = '' + adress.nomeLogradouro + ',' + adress.numero + ' <br> ' + err + '';
                }

                console.log(message);
                console.log(adress.nomeLogradouro);
                console.log(adress.numero);
                console.error(err);

                $ionicPopup.alert({
                    title: '<strong>Ocorreu um erro no Servidor</strong>',
                    template: message
                }).then(function() {
                    $state.go('app.dashboard/index', { reload: true });
                });
            });
    };

    $scope.submitAllSyncronizationHomeVisit = function(allRegister) {
        $scope.initLoading();
        var homeVisit = allRegister[indiceHomeVisit];
        var token = null;

        WebApiService.getToken($scope.headerTransport(homeVisit))
            .then(function(response) {
                token = response.token;
                var homeVisitJson = $scope.homeVisitToJson(homeVisit, token);
                WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Upload Visitas Domiciliares', token), $scope.headerTransport($rootScope.userLogged), [], [], homeVisitJson));
                return WebApiService.submitHomeVisit(homeVisitJson);
            })
            .then(function(response) {
                var condition = 'id = ' + homeVisit.id;
                return HomeVisitService.deleteHomeVisitToAPI(condition);
            })
            .then(function(response) {
                return WebApiService.closeToken(token);
            })
            .then(function(response) {
                if (indiceHomeVisit < allRegister.length - 1) {
                    indiceHomeVisit++;
                    return $scope.submitAllSyncronizationHomeVisit(allRegister);
                } else {
                    indiceHomeVisit = 0;
                    $scope.submitHomeVisitRegisterAPI();
                }
            })
            .catch(function(err) {
                $scope.endLoading();
                WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Processo de Envio de Visitas Domiciliares', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                var message = null;
                if (typeof err.ExceptionMessage != 'undefined') {
                    message = err.ExceptionMessage;
                } else {
                    message = err;
                }
                $ionicPopup.alert({
                    title: '<strong>Erro no servidor</strong>',
                    template: message
                }).then(function() {
                    $state.go('app.dashboard/index', { reload: true });
                });
            });

    };

    $scope.submitRegisterToAPI = function() {

        if ($scope.isNetworkOffline) {

            $ionicPopup.alert({
                title: 'Falha na conexão',
                template: 'Verifique a sua conexão com a internet'
            });

        } else {
            var numberPeopleForSyncronization = 0;
            var numberHomeForSyncronization = 0;
            // var numberVisitForSyncronization = 0;

            SyncronizationService.selectNumberPeoplesAPI()
                .then(function(response) {
                    numberPeopleForSyncronization = response[0].numberAllPeople;
                    return SyncronizationService.selectNumberHomeAPI();
                })
                .then(function(response) {
                    numberHomeForSyncronization = response[0].numberAllHome;
                    // return SyncronizationService.selectNumberHomeVisitAPI();
                })
                // .then(function(response){
                //   numberVisitForSyncronization = response[0].numberAllHomeVisit;
                // })
                .then(function() {
                    if (numberPeopleForSyncronization > 0) {
                        return SyncronizationService.selectPeoplesToAPI();
                    } else if (numberHomeForSyncronization > 0) {
                        return SyncronizationService.selectAdressToAPI();
                    }
                    // }else if(numberVisitForSyncronization > 0){
                    //   return SyncronizationService.selectHomeVisitToAPI();
                    // }
                })
                .then(function(response) {
                    if (numberPeopleForSyncronization > 0) {
                        $scope.submitAllSyncronizationUsers(response);
                        indiceSyncronization++;
                    } else if (numberHomeForSyncronization > 0) {
                        $scope.submitAllSyncronizationAdress(response);
                        indiceSyncronization++;
                        // }else if(numberVisitForSyncronization > 0){
                        //   $scope.submitAllSyncronizationHomeVisit(response);
                        //   indiceSyncronization++;
                        // }else if(numberPeopleForSyncronization == 0 && numberHomeForSyncronization == 0 && numberVisitForSyncronization == 0){
                    } else if (numberPeopleForSyncronization == 0 && numberHomeForSyncronization == 0) {

                        var infoTitle = null;
                        var infoMessage = null;

                        if (indiceSyncronization == 0) {
                            infoTitle = 'Informativo';
                            infoMessage = 'Não existem fichas para serem transmitidas';
                            indiceSyncronization = 0;
                        } else {
                            infoTitle = 'Transmissão';
                            infoMessage = 'Fichas transmitidas com sucesso para o servidor';
                            indiceSyncronization = 0;
                        }
                        $scope.endLoading();
                        $ionicPopup.alert({
                            title: '<strong>' + infoTitle + '</strong>',
                            template: infoMessage
                        }).then(function() {
                            $state.go('app.dashboard/index', { reload: true });
                        });
                    }
                })
                .catch(function(err) {
                    WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Submeter Registros Individuais e Domiciliares para a API', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                    var message = null;
                    if (typeof err.ExceptionMessage != 'undefined') {
                        message = err.ExceptionMessage;
                    } else {
                        message = err;
                    }
                    $scope.endLoading();
                    $ionicPopup.alert({
                        title: '<strong>Ocorreu um erro</strong>',
                        template: message
                    }).then(function() {
                        $state.go('app.dashboard/index', { reload: true });
                    });
                    console.log(err);
                });
        }

    };

    $scope.downloadRegisterAPI = function() {

        if ($scope.isNetworkOffline) {

            $ionicPopup.alert({
                title: 'Falha na conexão',
                template: 'Verifique a sua conexão com a internet'
            });

        } else {
            $scope.getAllSyncronizationUsers();
        }

    };

    $scope.submitHomeVisitRegisterAPI = function() {

        if ($scope.isNetworkOffline) {

            $ionicPopup.alert({
                title: 'Falha na conexão',
                template: 'Verifique a sua conexão com a internet'
            });

        } else {

            // var numberPeopleForSyncronization = 0;
            // var numberHomeForSyncronization = 0;
            var numberVisitForSyncronization = 0;

            SyncronizationService.selectNumberHomeVisitAPI()
                // .then(function(response){
                //   numberPeopleForSyncronization = response[0].numberAllPeople;
                //   return SyncronizationService.selectNumberHomeAPI();
                // })
                // .then(function(response){
                //   numberHomeForSyncronization = response[0].numberAllHome;
                //   return SyncronizationService.selectNumberHomeVisitAPI();
                // })
                .then(function(response) {
                    numberVisitForSyncronization = response[0].numberAllHomeVisit;
                })
                .then(function() {
                    // if(numberPeopleForSyncronization > 0){
                    //   return SyncronizationService.selectPeoplesToAPI();
                    // }else if(numberHomeForSyncronization > 0){
                    //   return SyncronizationService.selectAdressToAPI();
                    // }else if(numberVisitForSyncronization > 0){
                    if (numberVisitForSyncronization > 0) {
                        return SyncronizationService.selectHomeVisitToAPI();
                    }
                })
                .then(function(response) {
                    // if(numberPeopleForSyncronization > 0){
                    //   $scope.submitAllSyncronizationUsers(response);
                    //   indiceSyncronization++;
                    // }else if(numberHomeForSyncronization > 0){
                    //   $scope.submitAllSyncronizationAdress(response);
                    //   indiceSyncronization++;
                    // }else if(numberVisitForSyncronization > 0){
                    if (numberVisitForSyncronization > 0) {
                        $scope.submitAllSyncronizationHomeVisit(response);
                        indiceSyncronization++;
                        // }else if(numberPeopleForSyncronization == 0 && numberHomeForSyncronization == 0 && numberVisitForSyncronization == 0){
                    } else if (numberVisitForSyncronization == 0) {

                        var infoTitle = null;
                        var infoMessage = null;

                        if (indiceSyncronization == 0) {
                            infoTitle = 'Informativo';
                            infoMessage = 'Não existem fichas para serem transmitidas';
                            indiceSyncronization = 0;
                        } else {
                            $scope.endLoading();
                            infoTitle = 'Transmissão';
                            infoMessage = 'Fichas transmitidas com sucesso para o servidor';
                            indiceSyncronization = 0;
                        }
                        $ionicPopup.alert({
                            title: '<strong>' + infoTitle + '</strong>',
                            template: infoMessage
                        }).then(function() {
                            $state.go('app.dashboard/index', { reload: true });
                        });
                    }
                })
                .catch(function(err) {
                    $scope.endLoading();
                    WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Submeter Visitas Domiciliares', err), $scope.headerTransport($rootScope.userLogged), [], [], []));

                    var message = null;

                    if (typeof err.ExceptionMessage != 'undefined') {
                        message = err.ExceptionMessage;
                    } else {
                        message = err;
                    }

                    $ionicPopup.alert({
                        title: '<strong>Erro no servidor</strong>',
                        template: message
                    }).then(function() {
                        $state.go('app.dashboard/index', { reload: true });
                    });
                    console.log(err);
                });
        }

    };
    /*
  $scope.createRefusedAtomic = function(response) {
    listPeopleInformation.push($scope.userToJson(response[0]));
    console.log(listPeopleInformation);////////////////////////
    WebApiService.submitAtomic($scope.dataToAtomicJson($scope.headerTransport($rootScope.userLogged), listPeopleInformation, [], []))
      .then(function(resposta) {
        console.log(resposta);///////////////////////////
        if (resposta.status) {
          var condicao = 'EXISTS (SELECT Justificativa FROM Cadastro_Individual) ' +
            'and statusTermoRecusaCadastroIndividualAtencaoBasica = 1';
          PeopleService.getPeople(condicao)
            .then(function(recusados) {
              console.log(recusados);///////////////////////////////
              $scope.deleteRecusedRegisterAtomic(recusados);
            })
            .catch(function(err) {
              console.log(err);
            })
          // $scope.deleteRecusedRegisterAtomic(deletePeoples);
          console.log("sucesso");////////////////////////////////
        }
      })
      .catch(function(err) {
        $scope.endLoading();
        WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Envio do Atômico', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
        console.error(err);
        $ionicPopup.alert({
            title: '<strong>Erro: Ocorreu um erro no servidor</strong>',
            template: err
          })
          .then(function() {
            $state.go('app.dashboard/index', { reload: true });
          })
      })
  }
*/
    $scope.atomicSendToAPI = function() {

        if ($scope.isNetworkOffline) {

            $ionicPopup.alert({
                title: 'Falha na conexão',
                template: 'Verifique a sua conexão com a internet'
            });

        } else {

            $scope.initLoading();
            SyncronizationService.selectValidRegistersToAPI()
                .then(function(response) {
                    console.log(response);
                    if (response.length > 0) {
                        $scope.createStructureAtomic(response);
                        console.log(response.length);
                    } else {
                        $scope.endLoading();

                        $ionicPopup.alert({
                            title: '<strong>Informativo</strong>',
                            template: 'As composições familiares não estão com o status válidos para a transmissão ou o tablet não tem registros válidos.'
                        });
                    }
                })
                .then(function(response) {
                    SyncronizationService.selectRecuseRegistersToAPI()
                        .then(function(resposta) {
                            console.log(resposta);
                            console.log(resposta.length);
                            if (resposta.length > 0) {
                                $scope.createStructureAtomic(resposta);
                                console.log(resposta.length);
                                $scope.endLoading();
                            } else {
                                $scope.endLoading();
                                $ionicPopup.alert({
                                    title: '<strong>Informativo</strong>',
                                    template: 'O tablet não tem registros recusados.'
                                });
                            }
                        })
                        .catch(function(err) {
                            $scope.endLoading();
                            WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Verificação de dados para início de envio Atômico', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                            console.log(err);
                        });
                })
                .catch(function(err) {
                    $scope.endLoading();
                    WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Verificação de dados para início de envio Atômico', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                    console.log(err);
                });

        }

    };

    $scope.dataToAtomicJson = function(headerTransport, peopleInformation, homeRegistrationInformations, homeVisitRegistration) {
        console.log(peopleInformation);
        var atomic = [{
            "cabecalho": headerTransport,
            "visitas": homeVisitRegistration,
            "domicilios": homeRegistrationInformations,
            "individuos": peopleInformation
        }];

        return atomic;
    };

    $scope.createStructureAtomic = function(array) {

        $scope.selectDataToAtomic(array);

    };

    var indiceRegistroAtomico = 0;
    var indiceDependentsInformation = 0;
    var listPeopleInformation = [];
    var listHomeRegistrationInformations = [];
    var listHomeVisitRegistration = [];

    //Delete data
    var deletePeoples = [];
    var indiceDeleteAtomic = 0;

    $scope.listAllDependents = function(array, allCNS) {

        if (indiceDependentsInformation < array.length) {
            var register = array[indiceDependentsInformation];
            listPeopleInformation.push($scope.userToJson(register));
            indiceDependentsInformation++;
            $scope.listAllDependents(array, allCNS);
        } else {
            indiceDependentsInformation = 0;
            indiceRegistroAtomico++;
            $scope.selectDataToAtomic(allCNS);
        }

    };

    var contA = 0;

    $scope.selectDataToAtomic = function(allCNS) {
        console.log(allCNS);
        console.log(indiceRegistroAtomico);

        if (indiceRegistroAtomico < allCNS.length) {
            var register = allCNS[indiceRegistroAtomico];
            deletePeoples = allCNS;
            var cnsResponsavel = null;
            var homeResponsavel = null;

            console.log(register);
            if (register.Justificativa == null) {
                PeopleService.getPeople('cnsCidadao = "' + register.numeroCnsResponsavel + '"')
                    .then(function(response) {
                        cnsResponsavel = response[0].cnsCidadao;
                        listPeopleInformation.push($scope.userToJson(response[0]));
                        return SyncronizationService.selectHomeResponsibleAtomic(cnsResponsavel);
                    })
                    .then(function(response) {
                        homeResponsavel = response;
                        listHomeRegistrationInformations.push($scope.adressToJson(homeResponsavel[0]));
                        return FamilyService.getFamilies('cadastroDomiciliarId = ' + homeResponsavel[0].id + '');
                    })
                    .then(function(response) {
                        var contB = 0;
                        if (homeResponsavel.length > 0) {
                            while (contB < response.length) {
                                if (homeResponsavel[0].id == response[contB].cadastroDomiciliarId) {
                                    listHomeRegistrationInformations[contA].familiaRow.push($scope.familyToJson(response[contB]));
                                }
                                contB++;
                            }
                            contA++;
                        }

                        return SyncronizationService.selectPeopleDependentsAtomic(cnsResponsavel);
                    })
                    .then(function(response) {
                        $scope.listAllDependents(response, allCNS);
                    })
                    .catch(function(err) {
                        WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Montagem de JSON para o Atômico', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                        console.log(err);
                    });
            } else {
                SyncronizationService.selectRecuseRegistersToAPI()
                    .then(function(resposta) {
                        console.log(resposta);
                        console.log(resposta.length);
                        if (resposta.length > 0) {
                            var indiceDeRecusados = 0;
                            for (indiceDeRecusados = 0; indiceDeRecusados < resposta.length; indiceDeRecusados++) {
                                console.log(listPeopleInformation);
                                listPeopleInformation.push($scope.userToJson(resposta[indiceDeRecusados]));
                                console.log(listPeopleInformation);
                            }
                            $scope.endLoading();
                            // $ionicPopup.alert({
                            //   title: '<strong>Informativo</strong>',
                            //   template: 'Os dados foram enviados com sucesso'
                            // });
                        } else {
                            $scope.endLoading();
                            $ionicPopup.alert({
                                title: '<strong>Informativo</strong>',
                                template: 'O tablet não tem registros recusados.'
                            });
                        }
                    })
                    .catch(function(err) {
                        $scope.endLoading();
                        WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Verificação de dados para início de envio Atômico', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                        console.log(err);
                    });
                console.log(listPeopleInformation);
            }
            console.log(listPeopleInformation);
        } else {
            indiceRegistroAtomico = 0;

            console.log(listPeopleInformation);

            WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Upload Atômico', 'Não há'), $scope.headerTransport($rootScope.userLogged), listPeopleInformation, listHomeRegistrationInformations, []));
            console.log(listPeopleInformation);
            WebApiService.submitAtomic($scope.dataToAtomicJson($scope.headerTransport($rootScope.userLogged), listPeopleInformation, listHomeRegistrationInformations, []))
                .then(function(response) {
                    console.log(listPeopleInformation);
                    if (response.status) {
                        var condicao = 'EXISTS (SELECT Justificativa FROM Cadastro_Individual) ' +
                            'and statusTermoRecusaCadastroIndividualAtencaoBasica = 1';
                        PeopleService.getPeople(condicao)
                            .then(function(recusados) {
                                console.log(recusados); ///////////////////////////////
                                $scope.deleteRecusedRegisterAtomic(recusados);
                            })
                            .catch(function(err) {
                                console.log(err);
                            });
                        $scope.deleteRegisterAtomic(deletePeoples);
                    }
                })
                .catch(function(err) {
                    $scope.endLoading();
                    WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro: Envio do Atômico', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                    console.error(err);
                    var msgErro = JSON.stringify(err);
                    $ionicPopup.alert({
                            title: '<strong>Erro: Ocorreu um erro no servidor</strong>',
                            template: msgErro
                        })
                        .then(function() {
                            $state.go('app.dashboard/index', { reload: true });
                        });
                });
        }
    };

    var indiceDeleteRecused = 0;
    $scope.deleteRecusedRegisterAtomic = function(registrosRecusados) {
        console.log(registrosRecusados);
        console.log(indiceDeleteRecused);

        if (indiceDeleteRecused < registrosRecusados.length) {
            var registerRecused = registrosRecusados[indiceDeleteRecused];

            PeopleService.deletePeopleToAPI('Justificativa = "' + registerRecused.Justificativa + '" and nomeCidadao = "' + registerRecused.nomeCidadao + '"')
                .then(function(response) {
                    console.log(response);
                    indiceDeleteRecused++;
                    $scope.deleteRecusedRegisterAtomic(registrosRecusados);
                })
                .catch(function(err) {
                    WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro ao deletar Registros no Tablet', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                    console.log(err);
                });
        } else {
            $scope.endLoading();
            $ionicPopup.alert({
                    title: '<strong>Informativo</strong>',
                    template: 'Registros Recusados transmitidos com sucesso!'
                })
                .then(function() {
                    $state.go('app.dashboard/index');
                });

        }
    };

    indiceDeleteAtomic = 0;
    $scope.deleteRegisterAtomic = function(listCNS) {
        // console.log(indiceDeleteAtomic);
        // console.log(listCNS);


        if (indiceDeleteAtomic < listCNS.length) {
            var register = listCNS[indiceDeleteAtomic];
            var idHome = null;
            PeopleService.deletePeopleToAPI('cnsCidadao = "' + register.numeroCnsResponsavel + '" or cnsResponsavelFamiliar = "' + register.numeroCnsResponsavel + '"')
                .then(function(response) {
                    return FamilyService.getFamilies('numeroCnsResponsavel = "' + register.numeroCnsResponsavel + '"');
                })
                .then(function(response) {
                    idHome = response[0].cadastroDomiciliarId;
                    return FamilyService.deleteFamilies('numeroCnsResponsavel = "' + register.numeroCnsResponsavel + '"');
                })
                .then(function(response) {
                    return HomeRegistrationService.deleteHomeAdress('id = ' + idHome + '');
                })
                .then(function(response) {
                    indiceDeleteAtomic++;
                    $scope.deleteRegisterAtomic(listCNS);
                })
                .catch(function(err) {
                    WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro ao deletar Registros no Tablet', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                    console.log(err);
                });
        } else {
            $scope.endLoading();
            $ionicLoading.show();
            SyncronizationService.deleteHomeWithoutCitizenSyncronization()
                .then(function(response) {
                    return SyncronizationService.deleteFamilyWithoutHomeSyncronization();
                })
                .then(function(response) {
                    return SyncronizationService.deleteHomeWithoutFamily();
                })
                .then(function(response) {
                    $ionicPopup.alert({
                            title: '<strong>Informativo</strong>',
                            template: 'Registros transmitidos com sucesso!'
                        })
                        .then(function() {
                            $ionicLoading.hide();
                        })
                        .then(function() {
                            $state.go('app.dashboard/index');
                        });
                })
                .catch(function(err) {
                    WebApiService.dataLog($scope.formatDataLog($scope.formatDescription('Erro ao deletar registros orfãos no tablet', err), $scope.headerTransport($rootScope.userLogged), [], [], []));
                    console.log(err);
                    $ionicLoading.hide();
                });
        }
    };

    $scope.selectAllData = function() {
        SyncronizationService.selectAllRecordsIndividual()
            .then(function(response) {
                console.log(response);
            });
    };

    $scope.selectAllDataDomicilio = function() {
        SyncronizationService.selectAllRecordsDomiciliar()
            .then(function(response) {
                console.log(response);
            });
    };


    //BEGIN TEST

    $scope.formatDescription = function(param, token) {
        var description = {
            "TipoChamada": param,
            "Token": token,
            "ProfissionalCNS": $rootScope.userLogged.profissionalCNS,
            "NomeACS": $rootScope.userLogged.nome
        };

        return description;
    };

    $scope.formatDataLog = function(description, headerTransport, peopleInformation, homeRegistrationInformations, homeVisitRegistration) {
        var atomic = [{
            "Descricao": description,
            "cabecalho": headerTransport,
            "visitas": homeVisitRegistration,
            "domicilios": homeRegistrationInformations,
            "individuos": peopleInformation
        }];
        return atomic;
    };
    //END TEST

});