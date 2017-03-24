var Api = { URL: "ajax/marcadoresConsumoAlimentar/ajax-marcador-consumo-alimentar.asp", getPaciente: "ajax/atendIndividual/getPaciente.asp" };
var intervaloMeses = null;
var mensagem = "";
var profissional = false;
var cidadao = false;
var detalhes = false;

$(document).ready(function () {
    jQuery('#btnCancelar').on('click', limparFormMarcadorConsumoAlimentarUsuario);
    jQuery('#modalProfSus').on('shown.bs.modal', function () {
        $('#txtModalBusca').focus();
    });
    getDadosMarcadorConsumoAlimentar();
    getProfissionais();
    getCidadao();
    getDetalhes();

    //VALIDAÇÕES DO DICIONARIO DE DADOS DO CDS
    $("#txtCartaoSUS").on('blur', function () {
        validacaoCampo('#txtCartaoSUS', 'CNS');
    });
    $("#txtDataNascimento").on('blur', function () {
        validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade');
    });

    $('#selCnesUni').on('change', CarregarEquipeIne);
    jQuery("#pnlDadosProfissional .panel-heading").on("click", getProfissionais);
    jQuery("#pnlDadosCidadao .panel-heading").on("click", getCidadao);
    jQuery("#pnlDadosDetalhes .panel-heading").on("click", getDetalhes);
    jQuery("#txtDataNascimento").on("change", function () {
        jQuery("#frmDetalhes").hide().find("input").prop("checked", "");
        validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade');
    });
    jQuery('#btnNovoCartaoSus').on('click', addSemCadastroMarcador);
    jQuery('#btnFinalizarMarcadorConsumoAlimentar').on('click', finalizar);
    jQuery("#frmDadosProfissional").validate({
        rules: {
            CodigoEquipeIne: {
                required: true
            },
            DataMarcadores: {
                required: true
            },
            CodigoCnesUnidade: {
                required: true
            }
        },
        messages: {
            CodigoEquipeIne: "Campo INE Obrigatório",
            DataMarcadores: "Data é obrigatória",
            CodigoCnesUnidade: "Campo Código CNES Unidade é obrigatório",
        },
        submitHandler: function (form) {
            if (verificarItensnaTableProfissionais()) {
                confirmarProfissional();
                profissional = true;
            }
        }
    });
    jQuery("#frmDadosCidadao").validate({
        rules: {
            LocalAtendimento: {
                required: true
            },            
            DataNascimento: {
                required: true
            },
            nomeCidadao: {
                required: true
            },
            /*numeroCartaoSusUsuario: {
                minlength: 15,
                maxlength: 15
            },*/
            Sexo: {
                required: true
            }
        },
        messages: {
            LocalAtendimento: "Campo Local de Atendimento é obrigatório",
            DataNascimento: "Data de Nascimento é obrigatória",
            nomeCidadao: "Campo Nome do Cidadão é obrigatório",
            //numeroCartaoSusUsuario: "Nº Cartão SUS incorreto.",
            Sexo: "Campo obrigatório",
        },
        submitHandler: function (form) {
            $('#frmDadosCidadao').find('.msgErro').html('');
            if ($('#txtNome').val().split(' ').length > 1) {
                if ($('#txtNome').val().length < 0 || $('#txtNome').val().length > 100) {
                    $('#txtNome').parents('div:first').find('.msgErro').html('O nome deve ter entre 5 e 100 caracteres');
                    return false;
                }
            } else {
                $('#txtNome').parents('div:first').find('.msgErro').html('O nome deve ter o mínimo Nome e Sobrenome');
                return false;
            }
            if (verificarItensnaTableCidadao()) {
                salvarDadosCidadao();
                cidadao = true;
                ocultaBlocos();
            }
            
        }
    });
    jQuery("#frmDetalhes").validate({
        submitHandler: function (form) {
            if (salvarDetalhes()) {
                detalhes = true;
            }
        }
    });
    jQuery("#frmFinalizar").validate({
        submitHandler: function (form) {
            jQuery.ajax({
                type: "POST",
                data: {
                    acao: "finalizar",
                    idMarcador: jQuery("#idMarcadorConsumoAlimentar").val(),
                    status: 2
                },
                datatype: "json",
                async: false,
                url: Api.URL
            }).done(function (data) {
                window.location.replace("marcadoresConsumoAlimentar.asp");
            })
        }
    })
    jQuery("#btnSalvarDetalhes").on("click", salvarDetalhes);
});

function finalizar(event) {
    event.preventDefault();
    jQuery("#frmDadosProfissional").submit();
    jQuery("#frmDadosCidadao").submit();
    jQuery("#frmDetalhes").submit();
    verificarItensnaTableCidadao();
    verificarItensnaTableProfissionais();
    if (profissional && cidadao && detalhes) {
        jQuery("#frmFinalizar").submit();
    }
}

function getDetalhes() {
    jQuery.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getDetalhesByIdMarcador', idMarcador: jQuery("#idMarcadorConsumoAlimentar").val() },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        var obj = JSON.parse(data);
        if (obj.ok) {

            if (data[0].DataMarcadores != null) {
                data[0].DataMarcadores = formatarDataParaExibicao(data[0].DataMarcadores);
            }

            setFormData(obj.resultado);

            jQuery("#checkDadosDetalhes").hide();
            jQuery('#dadosDetalhes').show();
        }
    });
}

function salvarDetalhes() {
    var dadosDetalhes = JSON.stringify(getFormData("#frmDetalhes"));
    $('#frmDetalhes').find('.msgErro').html('');
    salvarDetalhesValido = true;
    if (intervaloMeses != null && intervaloMeses >= 0)
    {
        if (intervaloMeses < 6) {
            CamposObrigatorios = 'Menor6MesesACriancaOntemTomouLeiteDoPeito';
            CamposObrigatorios += ',OntemACriancaConsumiuMingau';
            CamposObrigatorios += ',OntemACriancaConsumiuAguaCha';
            CamposObrigatorios += ',OntemACriancaConsumiuLeiteDeVaca';
            CamposObrigatorios += ',OntemACriancaConsumiuFormulaInfantil';
            CamposObrigatorios += ',OntemACriancaConsumiuSucoDeFruta';
            CamposObrigatorios += ',OntemACriancaConsumiuFruta';
            CamposObrigatorios += ',Menor6MesesOntemACriancaConsumiuComidaDeSal';
            CamposObrigatorios += ',OntemACriancaConsumiuOutrosAlimentosBebidas';
            CamposObrigatorios = CamposObrigatorios.split(',');
            $.each(CamposObrigatorios, function (ResultadoItens, item) {
                if ($('[name=' + item + ']:checked').length == 0) {
                    if ($('[name=' + item + ']:first').parents('div:first').find('.msgErro').length == 0) {
                        $('[name=' + item + ']:first').parents('div:first').append('<div class=\'msgErro\'></div>');
                    }
                    $('[name=' + item + ']:first').parents('div:first').find('.msgErro').html('Este Campo é Obrigatório');
                    salvarDetalhesValido = false;
                }
            });
            if(salvarDetalhesValido) salvarCriancaMenorSeisMeses(dadosDetalhes);
        }
        else if ((intervaloMeses >= 6) && (intervaloMeses <= 23)) {
            CamposObrigatorios = 'ACriancaOntemTomouLeiteDoPeito';
            CamposObrigatorios += ',OntemACriancaComeuFrutaInteiraEmPedacoAmassada';
            CamposObrigatorios += ',QuantasVezesOntemACriancaComeuFrutaInteiraEmPedacoAmassada';
            CamposObrigatorios += ',OntemACriancaConsumiuComidaDeSal';
            CamposObrigatorios += ',QuantasVezesOntemACriancaConsumiuComidaDeSal';
            CamposObrigatorios += ',ComidaOferecida';
            CamposObrigatorios += ',OntemACriancaConsumiuOutroLeiteNaoDePeito';
            CamposObrigatorios += ',OntemACriancaConsumiuMingauComLeite';
            CamposObrigatorios += ',OntemACriancaConsumiuIogurte';
            CamposObrigatorios += ',OntemACriancaConsumiuLegumes';
            CamposObrigatorios += ',OntemACriancaConsumiuVegetalOuFrutaDeCorAlaranjada';
            CamposObrigatorios += ',OntemACriancaConsumiuVerduraDeFolha';
            CamposObrigatorios += ',OntemACriancaConsumiuCarne';
            CamposObrigatorios += ',OntemACriancaConsumiuFigado';
            CamposObrigatorios += ',OntemACriancaConsumiuFeijao';
            CamposObrigatorios += ',OntemACriancaConsumiuArrozBatataInhameAipimFarinhaMacarraoSemSerInstataneo';
            CamposObrigatorios += ',OntemACriancaConsumiuHamburguerEOuEmbutidos';
            CamposObrigatorios += ',OntemACriancaConsumiuBebidasAdocadas';
            CamposObrigatorios += ',OntemACriancaConsumiuMacarraoInstantaneoSalgadinhosDePacoteBiscoitosSalgados';
            CamposObrigatorios += ',OntemACriancaConsumiuBiscoitoRecheadoDoces';
            CamposObrigatorios = CamposObrigatorios.split(',');
            $.each(CamposObrigatorios, function (ResultadoItens, item) {
                if ($('[name=' + item + ']:checked').length == 0) {
                    if ($('[name=' + item + ']:first').parents('div:first').find('.msgErro').length == 0) {
                        $('[name=' + item + ']:first').parents('div:first').append('<div class=\'msgErro\'></div>');
                    }
                    $('[name=' + item + ']:first').parents('div:first').find('.msgErro').html('Este Campo é Obrigatório');
                    salvarDetalhesValido = false;
                }
            });
            if (salvarDetalhesValido) salvarCriancaSeisA23MesesByIdMarcador(dadosDetalhes);
        }
        else if (intervaloMeses > 23) {
            CamposObrigatorios = 'RefeicaoAssistindoTvMexendoNoPcEOuCelular';
            CamposObrigatorios += ',QuaisRefeicoesVoceFaz';
            CamposObrigatorios += ',OntemVoceConsumiuFeijao';
            CamposObrigatorios += ',OntemVoceConsumiuFrutasFrescas';
            CamposObrigatorios += ',OntemVoceConsumiuVerdurasEOuLegumes';
            CamposObrigatorios += ',OntemVoceConsumiuHamburguerEOuEmbutidos';
            CamposObrigatorios += ',OntemVoceConsumiuBebidasAdocadas';
            CamposObrigatorios += ',OntemVoceConsumiuMacarraoInstantaneoSalgadinhosDePacoteBiscoitosSalgados';
            CamposObrigatorios += ',OntemVoceConsumiuBiscoitoRecheadoDoces';
            CamposObrigatorios = CamposObrigatorios.split(',');
            $.each(CamposObrigatorios, function (ResultadoItens, item) {
                if ($('[name=' + item + ']:checked').length == 0) {
                    if ($('[name=' + item + ']:first').parents('div:first').find('.msgErro').length == 0) {
                        $('[name=' + item + ']:first').parents('div:first').append('<div class=\'msgErro\'></div>');
                    }
                    $('[name=' + item + ']:first').parents('div:first').find('.msgErro').html('Este Campo é Obrigatório');
                    salvarDetalhesValido = false;
                }
            });
            if (salvarDetalhesValido) salvarCriancaMais2AnosAdolescenteAdultoGestanteIdoso(dadosDetalhes);
        }
    }
    return salvarDetalhesValido;
}

function salvarCriancaMenorSeisMeses(dadosDetalhes) {
    var existe = false;
    var dados = dadosDetalhes;

    //Já contém esse detalhe?
    jQuery.ajax({
        type: "POST",
        data: {
            acao: "existCriancaMenorSeisMesesByIdMarcador",
            idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
        },
        datatype: "json",
        async: false,
        url: Api.URL
    }).done(function (data) {
        data = JSON.parse(data);

        if (data.ok) {
            if (data.resultado.Existe) {
                jQuery.ajax({
                    type: "POST",
                    data: {
                        acao: "editCriancaMenorSeisMeses",
                        dadosDetalhes: dadosDetalhes
                    },
                    datatype: "json",
                    async: false,
                    url: Api.URL
                }).done(function () {
                    jQuery("#checkDadosDetalhes").show();
                    jQuery('#dadosDetalhes').hide();
                })
            }
            else {
                var id = 0;
                jQuery.ajax({
                    type: "POST",
                    data: {
                        acao: "addCriancaMenorSeisMeses",
                        dadosDetalhes: dadosDetalhes
                    },
                    datatype: "json",
                    async: false,
                    url: Api.URL
                }).done(function (data) {
                    id = parseInt(data);
                    jQuery("#idDetalhe").val(data);
                    jQuery("#checkDadosDetalhes").show();
                    jQuery('#dadosDetalhes').hide();
                })

                if (id > 0) {
                    jQuery.ajax({
                        type: "POST",
                        data: {
                            acao: "vincularCriancaMenorSeisMeses",
                            id: id,
                            idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
                        },
                        datatype: "json",
                        async: false,
                        url: Api.URL
                    })
                }
            }
            jQuery.ajax({
                type: "POST",
                data: {
                    acao: "removeCriancaSeisA23Meses",
                    idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
                },
                datatype: "json",
                async: false,
                url: Api.URL
            })
            jQuery.ajax({
                type: "POST",
                data: {
                    acao: "removeCriancaMais2AnosAdolescenteAdultoGestanteIdoso",
                    idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
                },
                datatype: "json",
                async: false,
                url: Api.URL
            })
        }
    })
}

function salvarCriancaSeisA23MesesByIdMarcador(dadosDetalhes) {
    var existe = false;

    //Já contém esse detalhe?
    jQuery.ajax({
        type: "POST",
        data: {
            acao: "existCriancaSeisA23MesesByIdMarcador",
            idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
        },
        datatype: "json",
        async: false,
        url: Api.URL
    }).done(function (data) {
        data = JSON.parse(data);

        if (data.ok) {
            if (data.resultado.Existe) {
                jQuery.ajax({
                    type: "POST",
                    data: {
                        acao: "editCriancaSeisA23MesesByIdMarcador",
                        dadosDetalhes: dadosDetalhes
                    },
                    datatype: "json",
                    async: false,
                    url: Api.URL
                }).done(function () {
                    jQuery("#checkDadosDetalhes").show();
                    jQuery('#dadosDetalhes').hide();
                })
            }
            else {
                var id = 0;

                jQuery.ajax({
                    type: "POST",
                    data: {
                        acao: "addCriancaSeisA23Meses",
                        dadosDetalhes: dadosDetalhes
                    },
                    datatype: "json",
                    async: false,
                    url: Api.URL
                }).done(function (data) {
                    id = parseInt(data);
                    jQuery("#idDetalhe").val(data);
                    jQuery("#checkDadosDetalhes").show();
                    jQuery('#dadosDetalhes').hide();
                })

                if (id > 0) {
                    jQuery.ajax({
                        type: "POST",
                        data: {
                            acao: "vincularCriancaSeisA23Meses",
                            id: id,
                            idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
                        },
                        datatype: "json",
                        async: false,
                        url: Api.URL
                    })
                }
            }
            jQuery.ajax({
                type: "POST",
                data: {
                    acao: "removeCriancaMenorSeisMeses",
                    idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
                },
                datatype: "json",
                async: false,
                url: Api.URL
            })
            jQuery.ajax({
                type: "POST",
                data: {
                    acao: "removeCriancaMais2AnosAdolescenteAdultoGestanteIdoso",
                    idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
                },
                datatype: "json",
                async: false,
                url: Api.URL
            })
        }
    })

    

    existe = false;

    //Existem outros?
    jQuery.ajax({
        type: "POST",
        data: {
            acao: "existCriancaMenorSeisMesesByIdMarcador",
            idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
        },
        datatype: "json",
        async: false,
        url: Api.URL
    }).done(function (data) {
        data = JSON.parse(data);

        if (data.ok) {
            existe = data.resultado.Existe;
        }
    })

    if (existe) {
        
    }

    existe = false;

    jQuery.ajax({
        type: "POST",
        data: {
            acao: "existCriancaMais2AnosAdolescenteAdultoGestanteIdosoByIdMarcador",
            idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
        },
        datatype: "json",
        async: false,
        url: Api.URL
    }).done(function (data) {
        data = JSON.parse(data);

        if (data.ok) {
            existe = data.resultado.Existe;
        }
    })

    if (existe) {
        
    }
}

function salvarCriancaMais2AnosAdolescenteAdultoGestanteIdoso(dadosDetalhes) {
    var existe = false;
    var dados = dadosDetalhes;

    //Já contém esse detalhe?
    jQuery.ajax({
        type: "POST",
        data: {
            acao: "existCriancaMais2AnosAdolescenteAdultoGestanteIdosoByIdMarcador",
            idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
        },
        datatype: "json",
        async: false,
        url: Api.URL
    }).done(function (data) {
        data = JSON.parse(data);

        if (data.ok) {
            if (data.resultado.Existe) {
                jQuery.ajax({
                    type: "POST",
                    data: {
                        acao: "editCriancaMais2AnosAdolescenteAdultoGestanteIdoso",
                        dadosDetalhes: dados
                    },
                    datatype: "json",
                    async: false,
                    url: Api.URL
                }).done(function () {
                    jQuery("#checkDadosDetalhes").show();
                    jQuery('#dadosDetalhes').hide();
                })
            } else {
                var id = 0;

                jQuery.ajax({
                    type: "POST",
                    data: {
                        acao: "addCriancaMais2AnosAdolescenteAdultoGestanteIdoso",
                        dadosDetalhes: dados
                    },
                    datatype: "json",
                    async: false,
                    url: Api.URL
                }).done(function (data) {
                    id = parseInt(data);
                    jQuery("#idDetalhe").val(data);
                    jQuery("#checkDadosDetalhes").show();
                    jQuery('#dadosDetalhes').hide();
                })

                if (id > 0) {
                    jQuery.ajax({
                        type: "POST",
                        data: {
                            acao: "vincularCriancaMais2AnosAdolescenteAdultoGestanteIdoso",
                            id: id,
                            idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
                        },
                        datatype: "json",
                        url: Api.URL
                    })
                }
            }
            jQuery.ajax({
                type: "POST",
                data: {
                    acao: "removeCriancaMenorSeisMeses",
                    idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
                },
                datatype: "json",
                async: false,
                url: Api.URL
            })
            jQuery.ajax({
                type: "POST",
                data: {
                    acao: "removeCriancaSeisA23Meses",
                    idMarcador: jQuery("#idMarcadorConsumoAlimentar").val()
                },
                datatype: "json",
                async: false,
                url: Api.URL
            })
        }
    })
}

function getDadosProfissional() {
    var dados = {};
    dados = getFormData("#frmDadosProfissional");
    dados.Id = $('#idMarcadorConsumoAlimentar').val();
    dados.CodigoCnesUnidade = $('#selCnesUni :selected').val();

    if (dados.DataMarcadores != 0) {
        dados.DataMarcadores = formatarDataParaExibicao(dados.DataMarcadores);
    }

    return JSON.stringify(dados);
}

function getDadosCidadao() {
    var dados = {};
    dados = getFormData("#frmDadosCidadao");
    return JSON.stringify(dados);
}

function confirmarProfissional() {
    jQuery.ajax({
        type: "POST",
        data: {
            acao: 'setProfissional',
            dadosProfissional: getDadosProfissional,
            status: 1
        },
        datatype: "json",
        url: Api.URL
    }).done(function () {
        jQuery("#dadosProfissional").hide();
        jQuery("#checkDadosProfissional").show();
    });
}

function salvarDadosCidadao() {
    jQuery.ajax({
        type: "POST",
        data: {
            acao: 'setCidadao',
            dadosCidadao: getDadosCidadao,
            status: 1
        },
        datatype: "json",
        url: Api.URL
    }).done(function () {
        jQuery("#dadosCidadao").hide();
        jQuery("#checkDadosCidadao").show();
    });
}

function ocultaBlocos() {
    intervaloMeses = calculaMesesEntreDatas(jQuery("#txtDataNascimento").val(), jQuery("#txtData").val());

    if (intervaloMeses < 6) {
        jQuery("#frmDetalhes").show();
        jQuery("#menoresSeisMeses").show();
        jQuery("#SeisAVinteETresMeses input").prop("checked", "");
        jQuery("#SeisAVinteETresMeses").hide();
        jQuery("#maisDeDoisAnos input").prop("checked", "");
        jQuery("#maisDeDoisAnos").hide();
    }
    else if ((intervaloMeses >= 6) && (intervaloMeses <= 23)) {
        jQuery("#frmDetalhes").show();
        jQuery("#SeisAVinteETresMeses").show();
        jQuery("#menoresSeisMeses input").prop("checked", "");
        jQuery("#menoresSeisMeses").hide();
        jQuery("#maisDeDoisAnos input").prop("checked", "");
        jQuery("#maisDeDoisAnos").hide();
    }
    else if (intervaloMeses > 23) {
        jQuery("#frmDetalhes").show();
        jQuery("#maisDeDoisAnos").show();
        jQuery("#menoresSeisMeses input").prop("checked", "");
        jQuery("#menoresSeisMeses").hide();
        jQuery("#SeisAVinteETresMeses input").prop("checked", "");
        jQuery("#SeisAVinteETresMeses").hide();
    }
}

function cancelar() {
    if ($('#listaMarcadorConsumoAlimentar tbody tr').length == 0) {
        window.location.assign("marcadorConsumoAlimentar.asp");
    }
}

function busca_Paciente_Sus() {
    jQuery('#txtmodalBuscaCartaoSus').val('');
    jQuery('#txtmodalConfirmaCartaoSus').val('');
    $(".modal-title").html("Busca de Cidadão");
    $("#selModalTpBusca").val('1');
    $("#txtModalBusca").val('');
    $("label[for=txtModalBusca]").html('Nº SUS *');
    $("#txtModalBusca").attr('placeholder', 'Número');
    $('#modalProfSus').find('input[type=text], select').each(function () {
        $(this).parents(".form-group").removeClass("has-error");
    });
    $('#listamodalProfSus tbody').empty();
    $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center">Efetue a busca acima</th></tr>');
    $('#modalProfSus').modal('show').find('#selModalTpBusca').val('2');tpBuscaModalSus();
}

function busca_Prof_Sus() {
    
    $(".modal-title").html("Busca de Profissional");

    $("#selModalTpBusca").val('1');
    $("#txtModalBusca").val('');
    $("label[for=txtModalBusca]").html('Nº SUS *');
    $("#txtModalBusca").attr('placeholder', 'Número');

    $('#modalProfSus').find('input[type=text], select').each(function () {
        $(this).parents(".form-group").removeClass("has-error");
    });

    $('#listamodalProfSus tbody').empty();
    $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center">Efetue a busca acima</th></tr>');
    $('#modalProfSus').modal('show').find('#selModalTpBusca').val('2');tpBuscaModalSus();
}

function tpBuscaModalSus() {

    if ($("#selModalTpBusca").val() == 1) {
        $("#txtModalBusca").val('');
        $("label[for=txtModalBusca]").html('Nº SUS *');
        $("#txtModalBusca").attr('placeholder', 'Número');
    } else {
        $("#txtModalBusca").val('');
        $("label[for=txtModalBusca]").html('Nome *');
        $("#txtModalBusca").attr('placeholder', 'Nome');
    }

}

function paciente_Sus() {
    if (validaCampos($("#modalProfSus"), 2, "alertaSemModalProfSus")) {
        var xTbBusca = $('#selModalTpBusca').val();
        var xBusca = removerAcentos($('#txtModalBusca').val().trim());
        $('#listamodalProfSus tbody').empty();
        $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');
        $.ajax({
            type: "POST",
            data: { tpBusca: xTbBusca, busca: xBusca },
            datatype: "json",
            url: "ajax/atendIndividual/getPaciente.asp"
        })
        .done(function (data) {
            $('#listamodalProfSus tbody').empty();
            if (data.status) {
                $.each(data.resultado, function (ResultadoItens, item) {
                    var strTable = '<tr class="sel-cns" data-nome="' + item.nome.toCapitalize() + '" data-cns="' + item.cns + '" data-idusr="' + item.IDUSR + '"data-numcontrato="' + item.numContratoIdCidadao + '" >';
                    strTable += '<td class="text-capitalize">' + item.nome.toLowerCase() + '</td>';
                    strTable += '<td class="text-center">' + item.cns + '</td>';
                    strTable += '</tr>';
                    $('#listamodalProfSus tbody').append(strTable);
                });
                executaSelect();
            } else {
                if (data.sessao) {
                    setTimeout(function () {
                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                        setTimeout(function () {
                            window.location.assign("login.asp");
                        }, 2000);
                    }, 800);
                }
                $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center">Nenhum resultado encontrado!</th></tr>');
                $('#modalProfSus').modal('hide');
                $('#txtmodalBuscaCartaoSus').val(xBusca);
                $('#modalSemCartaoSus').modal('show');
                $('#modalSemCartaoSus .msgErro .col-md-12').text('');
            }
        });
    }

    function executaSelect() {
        $('#listamodalProfSus tbody .sel-cns').on('click',function (event) {
            $(this).siblings().removeClass('list-group-item-success');
            var dataNascimento = jQuery("#txtDataNascimento").val();
            var sexo = jQuery("[name='Sexo']:checked").val();
            var localAtendimento = jQuery("#selLocalAtend :selected").val();            
            var idMarcadorConsumoAlimentar = $("#idMarcadorConsumoAlimentar").val();
            makeTablePaciente($(this).data('nome'), $(this).data('cns'), $(this).data('idusr'));
            $('#txtCartaoSUS').val($(this).data('cns'));
            $('#txtNome').val($(this).data('nome'));
            $('.dadosCidadao').show('slow');
        });
    }
    return false;
}

function chaveamentoModal(evt) {
    if ($(".modal-title").html() == "Busca de Profissional") {
        return prof_Sus();
    } else {
        return paciente_Sus();
    }
}

function alteraInfoProfissional(event) {
    //removeGridProfissionais();
    var rowProfissional = $("#listaProfissionaisMarcadorConsumoAlimentar tbody tr.sel-cns");
    $("#idprofmarcadorconsumoalimentar").val(rowProfissional.data("idprofmarcadorconsumoalimentar"));
    $("#idProfissionalSaude").val($(event.currentTarget).data("idprof"));
    $("#cbo").val($(event.currentTarget).data("cbo"));
    makeTableProfissional(
        $(event.currentTarget).data("nome"),
        $(event.currentTarget).data("cns"),
        $(event.currentTarget).data("idprof"),
        rowProfissional.data("idprofmarcadorconsumoalimentar"),
        $(event.currentTarget).data("cbo"));
}

function prof_Sus() {
    if (validaCampos($("#modalProfSus"), 2, "alertaSemModalProfSus")) {
        var xTbBusca = $('#selModalTpBusca').val();
        var xBusca = removerAcentos($('#txtModalBusca').val().trim());
        $('#listamodalProfSus tbody').empty();
        $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');
        $.ajax({
            type: "POST",
            data: { tpBusca: xTbBusca, busca: xBusca },
            datatype: "json",
            url: "ajax/atendIndividual/getProfissional.asp"
        })
        .done(function (data) {
            $('#listamodalProfSus tbody').empty();
            if (data.status) {
                $.each(data.resultado, function (ResultadoItens, item) {
                    var strTable = '<tr class="sel-cns" data-nome="' + item.nome.toCapitalize() + '" data-cns="' + item.cns + '" data-idprof="' + item.IdProf.trim() + '" data-cbo="' + item.CBO + '" data-numcontrato="' + item.numContrato + '" >';
                    strTable += '<td class="text-capitalize">' + item.nome.toLowerCase() + '</td>';
                    strTable += '<td class="text-center">' + item.cns + '</td>';
                    strTable += '</tr>';
                    $('#listamodalProfSus tbody').append(strTable);
                });
                executaSelect();
            } else {
                if (data.sessao) {
                    setTimeout(function () {
                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                        setTimeout(function () {
                            window.location.assign("login.asp");
                        }, 2000);
                    }, 800);
                }
                $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center">Nenhum resultado encontrado!</th></tr>');
            }
        });
    }

    function executaSelect() {
        $("#listamodalProfSus tbody .sel-cns").on('click',function (event) {
            if ($(this).data('cns') == '') {
                $(this).parents('.modal').modal('hide');
                modalAlerta('Atenção', 'Este Profissional não pode ser selecionado, pois não tem CNS cadastrado');
            } else if ($(this).data('cbo') == '') {
                $(this).parents('.modal').modal('hide');
                modalAlerta('Atenção', 'Este Profissional não pode ser selecionado, pois não tem CBO cadastrado');
            } else {
                //Remove o active dos outros
                $(this).siblings().removeClass("list-group-item-success");
                var idProfissionalSaude = $(this).data("idprof");
                var codigoCnesUnidade = $('#selCnesUni :selected').val();
                var codigoEquipeIne = ($('#selIneEquipe').val() == "") ? 0 : $('#selIneEquipe').val();
                var cbo = $(this).data("cbo");
                var numContrato = $(this).data("numcontrato");
                if ($("#listaProfissionaisMarcadorConsumoAlimentar tbody tr.sel-cns").length > 0) {
                    alteraInfoProfissional(event);
                    $("#modalProfSus").modal("hide");
                }
                else {
                    alteraMarcadorConsumoAlimentar(1, idProfissionalSaude, codigoCnesUnidade, codigoEquipeIne, cbo, numContrato);
                    makeTableProfissional($(this).data("nome"), $(this).data("cns"), $(this).data("idprof"), jQuery("#idMarcadorConsumoAlimentar").val(), $(this).data("cbo"));
                }
            }
        });
    }
    return false;
}

function removerProfissionalMarcadorConsumoAlimentar(codigoCnesUnidade, codigoEquipeIne) {
    alteraMarcadorConsumoAlimentar(1, null, codigoCnesUnidade, codigoEquipeIne, "", null);
}

function removerPacienteMarcadorConsumoAlimentar(codigoCnesUnidade, codigoEquipeIne) {
    alteraPacienteMarcadorConsumoAlimentar(1, null, codigoCnesUnidade, codigoEquipeIne, "", null, id);
}

function dadosUsuarioMarcadorConsumoAlimentar() {
    var dados = getFormData("#fichaMarcadorConsumoAlimentar");

    return JSON.stringify(dados);
}

function dadosMarcadorConsumoAlimentar(status, idProfissionalSaude, codigoCnesUnidade, codigoEquipeIne, cbo, numContrato) {
    var dados = {
        data: dataAtualFormatada(),
        numeroFolha: 0,
        idProfissionalSaude: idProfissionalSaude,
        numContratoIdProfissionalSaude: numContrato,
        codigoCnesUnidade: codigoCnesUnidade,
        idCriancaMenorSeisMeses: null,
        idCriancaSeisA23Meses: null,
        idCriancaMais2AnosAdolescenteAdultoGestanteIdoso: null,
        codigoEquipeIne: codigoEquipeIne,
        dataMarcadores: $('#txtData').val(),
        numeroCartaoSusUsuario: $('#txtCartaoSUS').val(),
        nomeCidadao: $('#txtNome').val(),
        dataNascimento: null,
        sexo: null,
        localAtendimento: null,
        idStatus: status,
        cbo: cbo,
        Id: jQuery("#idMarcadorConsumoAlimentar").val()
    };
    return JSON.stringify(dados);
}

function dadosMarcadorCidadao(idMarcadorConsumoAlimentar, cns, idusr, nome, dataNascimento, sexo, localAtendimento) {
    var dados = {
        numeroCartaoSusUsuario: cns,
        nomeCidadao: nome,
        dataNascimento: dataNascimento,
        sexo: sexo,
        localAtendimento: localAtendimento,
        Id: idMarcadorConsumoAlimentar,
        IdCidadao: idusr
    };
    return JSON.stringify(dados);
}

function alteraPacienteMarcadorConsumoAlimentar() {
    
    $.ajax({
        type: "POST",
        data: {
            acao: 'editUsuarioMarcadorConsumoAlimentar',
            dadosMarcadorConsumoAlimentar: dadosUsuarioMarcadorConsumoAlimentar()
        },
        datatype: "json",
        url: Api.URL
    }).done(
        function () {
            if ($('#listaMarcadorConsumoAlimentar tbody tr').length == 0) {
                limparFormMarcadorConsumoAlimentarUsuario();
                $('.dadosCidadao').hide();
            }
        }
    )

}

function alteraMarcadorConsumoAlimentar(status, idProfissionalSaude, codigoCnesUnidade, codigoEquipeIne, cbo, numContrato) {
    
    $.ajax({
        type: "POST",
        data: {
            acao: 'Edit',
            dadosMarcadorConsumoAlimentar: dadosMarcadorConsumoAlimentar(status, idProfissionalSaude, codigoCnesUnidade, codigoEquipeIne, cbo, numContrato),
            status: status
        },
        datatype: "json",
        url: Api.URL
    }).done(function () {
        if (status == 2) {
            modalAlerta("Confirmação", "Marcadores de Consumo Alimentar finalizado com sucesso!");
            window.location.assign("marcadorConsumoAlimentar.asp");
        }
    });
}

function alteraMarcadorPaciente(idMarcadorConsumoAlimentar,idusr, cns, nome,dataNascimento,sexo,localAtendimento) {
    
    $.ajax({
        type: "POST",
        data: {
            acao: 'setCidadao',
            dadosMarcadorCidadao: dadosMarcadorCidadao(idMarcadorConsumoAlimentar,idusr, cns, nome, dataNascimento, sexo, localAtendimento)
        },
        datatype: "json",
        url: Api.URL
    }).done(function () {
    });
}

function removerPacienteMarcadorConsumoAlimentar(idPacienteMarcadorConsumoAlimentar) {
    jQuery.ajax({
        type: "POST",
        data: { acao: 'editUsuarioMarcadorConsumoAlimentar', dadosMarcadorConsumoAlimentarUsuario: getDadosCidadao },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        limparFormMarcadorConsumoAlimentarUsuario();
        $('.dadosCidadao,#frmDetalhes').hide('slow');
    });
}

function limparFormMarcadorConsumoAlimentarUsuario() {
    $('.IdentificacaoUsuario').each(function (i) {
        $(this).find('input[type=checkbox]:checked').removeAttr('checked', false);
        $(this).find('input[type=text]').val('');
        $(this).find('input[type=number]').val('');
        $(this).find('input[type=radio]:checked').removeAttr('checked', false);
        $('#listaExames tbody').empty();
    });
}

function makeTablePaciente(nome, cns, idusr, idusrmarcadorconsumoalimentar) {
    jQuery("#numeroCartaoSusUsuario").val(cns);
    jQuery("#idCidadao").val(idusr);
    jQuery("#nome").val(nome.toCapitalize());

    var strTable = '';
    strTable += '   <tr class="sel-cns" data-nome="' + nome.toCapitalize() + '" data-cns="' + cns + '" data-idusr="' + idusr  + "' data-idusrmarcadorconsumoalimentar='" + idusrmarcadorconsumoalimentar +  '" >';
    strTable += '       <td class="text-capitalize">' + nome.toCapitalize() + '</td>';
    strTable += '       <td class="text-center">' + cns + '</td>';
    strTable += '       <td class="text-center"><button type="button" class="btn btn-danger" title="Remover"><span class="glyphicon glyphicon-remove"></span></button></td>';    
    strTable += '   </tr>';
    $('#listaMarcadorConsumoAlimentar tbody').append(strTable);
    $('#msgErroTableCid').text('');
    removeGridPacientes();
    desabilitaBuscaPaciente();
    jQuery('#modalProfSus').modal('hide');
}

function makeTableProfissional(nome, cns, idprof, idprofmarcadorconsumoalimentar, cbo) {
    jQuery("#idProfissionalSaude").val(idprof);
    jQuery("#cbo").val(cbo);

    var strTable = "";
    strTable += "   <tr class='sel-cns' data-nome='" + nome.toCapitalize() + "' data-cns='" + cns + "' data-idprof='" + idprof + "' data-idprofmarcadorconsumoalimentar='" + idprofmarcadorconsumoalimentar + "' data-cbo='" + cbo + "'>";
    strTable += "       <td class='text-capitalize'>" + nome.toCapitalize() + "</td>";
    strTable += "       <td class='text-center'>" + cns + "</td>";
    strTable += "       <td class='text-center'>" + cbo + "</td>";
    strTable += "       <td class='text-center'><button type='button' class='btn btn-success' title='Substituir'><span class='glyphicon glyphicon-refresh'></span></button></td>";
    strTable += "   </tr>";
    removeGridProfissionais();
    $("#listaProfissionaisMarcadorConsumoAlimentar tbody").append(strTable);
    $('#msgErroTableProf').text('');    
    substituiProfSus();
    desabilitaBuscaProfissional();
    $("#modalProfSus").modal("hide");
}

function removeGridProfissionais() {
    var tr = $('#listaProfissionaisMarcadorConsumoAlimentar tbody tr');
    var codigoCnesUnidade = $('#selCnesUni :selected').val();
    var codigoEquipeIne = ($('#selIneEquipe').val() == "") ? 0 : $('#selIneEquipe').val();
    $(tr).remove();
    desabilitaBuscaProfissional();    
}

function desabilitaBuscaProfissional() {
    if ($('#listaProfissionaisMarcadorConsumoAlimentar tbody tr').length > 0) {
        $('#btnBuscarProf').attr('disabled', 'disabled');
    } else {
        $('#btnBuscarProf').removeAttr('disabled');
    }
}

function removeGridPacientes() {
    $('#listaMarcadorConsumoAlimentar tbody .btn-danger').click(function () {
        var tr = $(this).closest("tr");
        removerPacienteMarcadorConsumoAlimentar($(tr).data("idusrmarcadorconsumoalimentar"));
        $(tr).remove();
        desabilitaBuscaPaciente();
    });
}

function verificarItensnaTableCidadao(event) {
    var retorno = true;
    $('#msgErroTableCid').text('');
    if ($('#listaMarcadorConsumoAlimentar tbody tr').length == 0) {
        $('#msgErroTableCid').text('Insira no mínimo um Cidadao para realizar o Marcador de Consumo Alimentar.');
        $('#btnBuscarUsu').focus();
        retorno = false;
    }
    return retorno;               
}

function verificarItensnaTableProfissionais(event) {
    var retorno = true;
    $('#msgErroTableProf').text('');
    if ($('#listaProfissionaisMarcadorConsumoAlimentar tbody tr').length == 0) {
        $('#msgErroTableProf').text('Insira no mínimo um Profissional para realizar o Marcador de Consumo Alimentar.');
        $('#btnBuscarProf').focus();
        retorno = false;
    }

    return retorno;
       

}

function desabilitaBuscaPaciente() {
    if ($('#listaMarcadorConsumoAlimentar tbody tr').length > 0) {
        $('#btnBuscarUsu').attr('disabled', 'disabled');
        $('#btnAddSemCadastro').attr('disabled', 'disabled');
    } else {
        $('#btnAddSemCadastro').removeAttr('disabled');
        $('#btnBuscarUsu').removeAttr('disabled');
    }
}

function habilitaFichaPaciente() {
    $("#listaMarcadorConsumoAlimentar tbody .preencherDadosPaciente").on("click", function () {
        var data = $(this).closest("tr").data("idusrmarcadorconsumoalimentar");
        $("#listaMarcadorConsumoAlimentar tbody tr ").removeClass("active");
        jQuery(this).closest("tr").addClass("active");
        if (data != $('.IdentificacaoUsuario').data("id")) {
            limparFormMarcadorConsumoAlimentarUsuario();
        }
        $('.IdentificacaoUsuario').show().data("id", data);
        getFichaPaciente(data);
        //$('#btnConfirmar').show();
        //$('#btnCancelar').show();
        $('#btnFinalizarMarcadorConsumoAlimentar').show();
    });

}

function getFichaPaciente(idUsrMarcadorConsumoAlimentar) {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getUsuarioMarcadorConsumoAlimentarById', idMarcadorConsumoAlimentar: idUsrMarcadorConsumoAlimentar },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        var obj = JSON.parse(data)[0];
        makeDadosPaciente(obj);
    });
}

function makeDadosPaciente(item) {
    setFormData(item);
}

function getProfissionais() {
    jQuery.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getProfissionalByIdMarcadorConsumoAlimentar', idMarcadorConsumoAlimentar: jQuery("#idMarcadorConsumoAlimentar").val() },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        var obj = JSON.parse(data);
        if (obj.status) {
            jQuery("#listaProfissionaisMarcadorConsumoAlimentar tbody").empty();
            jQuery.each(obj.resultado, function (ResultadoItens, item) {
                makeTableProfissional(item.Nome, item.CNS, item.IdProf, item.IdProfAtend, item.CBO);
            });

            jQuery("#checkDadosProfissional").hide();
            jQuery('#dadosProfissional').show();
        }
    });
}

function getCidadao() {
    jQuery.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getUsuarioByIdMarcador', idMarcador: jQuery("#idMarcadorConsumoAlimentar").val() },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        var obj = JSON.parse(data);
        if (obj.status) {
            jQuery('#listaMarcadorConsumoAlimentar tbody').empty();
            setFormData(obj.resultado[0]);
            ocultaBlocos();
            jQuery.each(obj.resultado, function (ResultadoItens, item) {
                makeTablePaciente(item.Nome, item.CNS, item.IdCidadao, item.IdPacienteMarcador);
                $('#txtCartaoSUS').val(item.CNS);
                $('#txtNome').val(item.Nome);
                $('.dadosCidadao').show('slow');
            });

            jQuery("#checkDadosCidadao").hide();
            jQuery('#dadosCidadao').show();
        }
    });
}

function getDadosMarcadorConsumoAlimentar() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getById', id: jQuery("#idMarcadorConsumoAlimentar").val() },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        var obj = JSON.parse(data);
        if (obj.status) {

            if (obj.resultado.codigo_cnes_unidade > 0) {
                $('#selCnesUni').val(obj.resultado.codigo_cnes_unidade).prop('disabled', 'disabled');
            }
            makeEquipeIne(obj.resultado.codigo_cnes_unidade, obj.resultado.codigo_equipe_ine);
            if (obj.resultado.data_marcadores != null) {
                var dataMarcadorConsumoAlimentar = obj.resultado.data_marcadores.split("-");
                $('#txtData').val(dataMarcadorConsumoAlimentar[2] + "/" + dataMarcadorConsumoAlimentar[1] + "/" + dataMarcadorConsumoAlimentar[0]);
            }
        }
    });
}

function CarregarEquipeIne() {

    var CodCnes = $('#selCnesUni :selected').val();
    if (parseInt(CodCnes) > 0) {
        $.ajax({
            type: "POST",
            async: false,
            data: { acao: 'getEquipeIne', codCnes: CodCnes },
            datatype: "json",
            url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
        })
        .done(function (data) {
            var obj = JSON.parse(data)
            if (obj.status) {
                var options = "";
                options = '<option value="">SELECIONE UMA EQUIPE</option>'
                $.each(obj.resultado, function (ResultadoItens, item) {
                    options += '<option value="' + item.CodINE + '">' + item.Numero + ' - ' + item.Descricao + '</option>';
                });
                $("#selIneEquipe").html(options);
                $("label[for='txtIneEquipe']").html('Cód. equipe (INE)  <span class="asterisco"> *</span>').removeClass('removeasterisco');
            }
            else {
                $('#selIneEquipe').html('<option value="0">NÃO HÁ EQUIPES</option>');
                $("label[for='txtIneEquipe'] .asterisco").remove();
                $("label[for='txtIneEquipe']").addClass('removeasterisco');
            }
        });
    }
}

function makeEquipeIne(CodCnes, CodINE) {

    if (parseInt(CodCnes) > 0) {
        $.ajax({
            type: "POST",
            async: false,
            data: { acao: 'getEquipeIne', codCnes: CodCnes },
            datatype: "json",
            url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
        })
        .done(function (data) {
            var obj = JSON.parse(data)
            if (obj.status) {
                var options = "";
                options = '<option value="">SELECIONE UMA EQUIPE</option>'
                $.each(obj.resultado, function (ResultadoItens, item) {
                    options += '<option value="' + item.CodINE + '">' + item.Numero + ' - ' + item.Descricao + '</option>';
                });
                $("#selIneEquipe").html(options);
                $("label[for='txtIneEquipe']").html('Cód. equipe (INE) <span class="asterisco"> *</span>').removeClass('removeasterisco');
                if (CodINE <= 23 && CodINE > 0) { $("#selIneEquipe").val(CodINE) };

            }
            else {
                $('#selIneEquipe').html('<option value="0">NÃO HÁ EQUIPES</option>');
                $("label[for='txtIneEquipe'] .asterisco").remove();
                $("label[for='txtIneEquipe']").addClass('removeasterisco');
            }
        });
    }
}

function substituiProfSus() {
    $('#listaProfissionaisMarcadorConsumoAlimentar tbody .btn-success').on("click", function () {
        busca_Prof_Sus();
    });
}

function pacienteNovamente() {

    $('#modalSemCartaoSus').modal('hide');
    busca_Paciente_Sus();

}

function salvarSemCadastroMarcador(cns) {
    var xTbBusca = 2;
    var xBusca = "Sem Cadastro";
    $.ajax({
        type: "POST",
        data: { tpBusca: xTbBusca, busca: xBusca },
        datatype: "json",
        url: Api.getPaciente
    })
    .done(function (data) {
        if (data.status) {
            var idFicha = $("#frmUsuarioIdProcedimento").val();
            $.each(data.resultado, function (ResultadoItens, item) {
                var idFicha = $("#frmCidadaoIdMarcadorConsumoAlimentar").val();
                if (cns == "") {
                    cns = item.cns
                }
                makeTablePaciente(item.nome, cns, item.IDUSR);
                $('#txtCartaoSUS').val(cns);
                $('#txtNome').val(item.nome);
                $('.dadosCidadao').show('slow');
            });
            $('#modalSemCartaoSus').modal('hide');
            $('#txtmodalBuscaCartaoSus').val('');
            $('#txtmodalConfirmaCartaoSus').val('');
        } else {
            if (data.sessao) {
                setTimeout(function () {
                    modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                    setTimeout(function () {
                        window.location.assign("login.asp");
                    }, 2000);
                }, 800);
            }
        }
    });

}

function addSemCadastroMarcador() {
    if ($('#txtmodalBuscaCartaoSus').val() != "") {
        if ($('#txtmodalBuscaCartaoSus').val() == $('#txtmodalConfirmaCartaoSus').val()) {
            salvarSemCadastroMarcador($('#txtmodalConfirmaCartaoSus').val());
        }
        else {
            $('#modalSemCartaoSus .msgErro .col-md-12').text('O cartão do SUS não confere com o digitado anteriormente.Favor digite novamente.');
        }
    }
    else {
        $('#txtmodalBuscaCartaoSus').val('');
        $('#txtmodalConfirmaCartaoSus').val('');
        salvarSemCadastroMarcador($('#txtmodalConfirmaCartaoSus').val());
    }

}
