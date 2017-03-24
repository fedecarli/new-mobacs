var Api = { URL: "ajax/atendIndividual/ajax-atendimento-individual.asp", getPaciente: "ajax/atendIndividual/getPaciente.asp" };
var idFicha = "#idAtendimento";

$(document).ready(function () {
    //Modal
    $('#modalProfSus').on('shown.bs.modal', function () {
        $('#txtModalBusca').focus();
    });

    //Evento Saída do Foco
    $("#txtCartaoSUS").on('blur', function () {
        validacaoCampo('#txtCartaoSUS', 'CNS');
    });
    $("#txtDataNascimento").on('blur', function () {
        //validacaoCampo('txtDataNascimento,txtData', 'DataNasc-dataAtividade');
        //jQuery("#txtPeso").val(lPadDecimal(jQuery("#txtPeso").val(), String(parseInt(jQuery("#txtPeso").val())).length, 3));
    });
    $("#txtPeso").on('blur', function () {
        validacaoCampo('#txtPeso', 'Peso');
        //jQuery("#txtPeso").val(lPadDecimal(jQuery("#txtPeso").val(), String(parseInt(jQuery("#txtPeso").val())).length, 3));
    });
    $("#txtAltura").on('blur', function () {
        validacaoCampo('#txtAltura', 'Altura');
        //jQuery("#txtAltura").val(lpad(jQuery("#txtAltura").val()));
    });
    $("#txtDataDum").on('blur', function () { validaDUM(); });
    $("#txtIdadeGestacao").on('blur', function () { validaIdadeGestacional(); });
    $("#txtGestas").on('blur', function () { validaIdadeGestasPrevias(); });
    $("#txtPartos").on('blur', function () { validaIdadePartos(); });
    $('[name="rdbSexo"]').on('click', function () {
        if ($('[name="rdbSexo"][value="M"]:checked').length == 1) {
            $("#txtDataDum,#txtIdadeGestacao,#txtGestas,#txtPartos,[name='rdbGravidez']").prop('disabled', 'disabled');
        } else if ($('[name="rdbSexo"][value="F"]:checked').length == 1) {
            $("#txtDataDum,#txtIdadeGestacao,#txtGestas,#txtPartos,[name='rdbGravidez']").removeProp('disabled');
        }
    });
    $('#selCiap01').on('change', function () {
        $('#selCiap02 option').removeProp('disabled').filter('[value="' + $('#selCiap01 option:selected').val() + '"]').prop('disabled', 'disabled');
    });
    $('#selCiap02').on('change', function () {
        $('#selCiap01 option').removeProp('disabled').filter('[value="' + $('#selCiap02 option:selected').val() + '"]').prop('disabled', 'disabled');
    });
    //VALIDAÇÕES EXCLUSIVAS
    function validaDUM() {
        var retorno = true;
        $("#txtDataDum").parent().find('.msgErro').html('');
        if ($("#txtDataDum").val().trim() != '') {
            if (validacaoCampo('[name="rdbSexo"]', 'Sexo')) {
                if (validacaoCampo('#txtDataNascimento', 'DataNasc')) { 
                    if ($('#txtData').val().trim() != '') {
                        if ($('[name="rdbSexo"][value="F"]:checked').length == 1) {
                            var DataDUM = $("#txtDataDum").val().split('/');
                            var dDataDUM = new Date(DataDUM[2], DataDUM[1] - 1, DataDUM[0]);
                            var DataNasc = $("#txtDataNascimento").val().split('/');
                            var dDataNasc = new Date(DataNasc[2], DataNasc[1] - 1, DataNasc[0]);
                            var dataAtividade = $("#txtData").val().split('/');
                            var dDataAtividade = new Date(dataAtividade[2], dataAtividade[1] - 1, dataAtividade[0]);

                            if (dDataDUM.getTime() < dDataAtividade.getTime()) {
                                if (dDataDUM.getTime() < dDataNasc.getTime()) {
                                    retorno = false;
                                    $("#txtDataDum").parent().find('.msgErro').html('O Campo DUM não pode ser menor que o campo Data de Nascimento');
                                    goToByScroll($('#txtDataDum'), false);
                                }
                            } else {
                                retorno = false;
                                $("#txtDataDum").parent().find('.msgErro').html('O Campo DUM não pode ser maior que o campo data');
                                goToByScroll($('#txtDataDum'), false);
                            }
                        } else {
                            retorno = false;
                            $("#txtDataDum").parent().find('.msgErro').html('O Campo DUM não pode ser preenchido caso o sexo seja Masculino');
                            goToByScroll($('#txtDataDum'), false);
                        }
                    } else {
                        retorno = false;
                        $('#txtData').parent().find('.msgErro').html('O Campo Data é Obrigatório');
                        goToByScroll($('#txtData'), false);
                    }
                }
            }
        }
        return retorno;
    }

    function validaIdadeGestacional() {
        var retorno = true;
        $("#txtIdadeGestacao").parent().find('.msgErro').html('');
        if ($("#txtIdadeGestacao").val().trim() != '') {
            if (validacaoCampo('[name="rdbSexo"]', 'Sexo')) {
                if ($('[name="rdbSexo"][value="F"]:checked').length == 1) {
                    if ($("#txtIdadeGestacao").val().trim() < 1 || $("#txtIdadeGestacao").val().trim() > 42) {
                        retorno = false;
                        $("#txtIdadeGestacao").parent().find('.msgErro').html('O Campo Idade gestacional(semanas) não pode ser menor que 1, nem maior que 42');
                        goToByScroll($('#txtIdadeGestacao'), false);
                    }
                } else {
                    retorno = false;
                    $("#txtIdadeGestacao").parent().find('.msgErro').html('O Campo Idade gestacional(semanas) não pode ser preenchido caso o sexo seja Masculino');
                    goToByScroll($('#txtIdadeGestacao'), false);
                }
            } 
        }
        return retorno;
    }

    function validaIdadeGestasPrevias() {
        var retorno = true;
        $("#txtGestas").parent().find('.msgErro').html('');
        if ($("#txtGestas").val().trim() != '') {
            if (validacaoCampo('[name="rdbSexo"]', 'Sexo')) {
                if ($('[name="rdbSexo"][value="M"]:checked').length == 1) {
                    retorno = false;
                    $("#txtGestas").parent().find('.msgErro').html('O Campo Gestas Prévias não pode ser preenchido caso o sexo seja Masculino');
                    goToByScroll($('#txtGestas'), false);
                }
            }
        }
        return retorno;
    }

    function validaIdadePartos() {
        var retorno = true;
        $("#txtPartos").parent().find('.msgErro').html('');
        if ($("#txtPartos").val().trim() != '') {
            if (validacaoCampo('[name="rdbSexo"]', 'Sexo')) {
                if ($('[name="rdbSexo"][value="M"]:checked').length == 1) {
                    retorno = false;
                    $("#txtPartos").parent().find('.msgErro').html('O Campo Partos não pode ser preenchido caso o sexo seja Masculino');
                    goToByScroll($('#txtPartos'), false);
                }
            }
        }
        return retorno;
    }

    //Evento Click
    $('#btnConfObs').on('click', executaSelectExames);  
    $('#btnCancelar').on('click', limparFormAtendimentoUsuario);
    $('#btnFinalizarAtend').on('click', finalizar);
    $('#btnBuscarProf').on('click', busca_Prof_Sus);
    $('#btnNovoCartaoSus').on('click', addSemCadastro);
    jQuery('#CondicaoAvaliada input[type="checkbox"]').on("click", function () { jQuery('#msgErroCondicaoAvaliada').text(''); })
    jQuery('#CondicaoAvaliada select').on("change", function () { jQuery('#msgErroCondicaoAvaliada').text(''); })
    $('#btnConfirmar').on('click', function (evt) {
        var verificacao = verificarItensnaTable();
        if (verificacao) { verificacao = validacaoCampo('#txtCartaoSUS', 'CNS'); };
        if (verificacao) { verificacao = validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade'); };
        if (verificacao) { verificacao = validacaoCampo('#txtPeso', 'Peso'); };
        if (verificacao) { verificacao = validacaoCampo('#txtAltura', 'Altura'); };
        if (verificacao) { verificacao = validaDUM(); };
        if (verificacao) { verificacao = validaIdadeGestacional(); };
        if (verificacao) { verificacao = validaIdadeGestasPrevias(); };
        if (verificacao) { verificacao = validaIdadePartos(); };
        if (!verificacao) {
            evt.preventDefault();
        }
    });

    $('#selCnesUni').on('change', CarregarEquipeIne);

    jQuery('#btnBuscaProcedimento').on('click', getProcedimento);
    //Get dos Dados - Carregamento da Tela
    getDadosAtendimento();
    getProfissionais();
    getPacientes();
    jQuery("#selCid10 option").each(function () {
        var aux = jQuery(this).val();
        jQuery(this).val(aux.trim());
    })

    //Validate
    $("#fichaAtendimentoIndividual").validate({
        errorPlacement: function (error, element) {
            if (jQuery(element).parent("div").find("div.msgErro").length > 0) {
                error.appendTo(jQuery(element).parent("div").find("div.msgErro"));
            }
            else {
                error.appendTo(jQuery(element).parents("div.form-group").find("div.msgErro"));
            }
        },
        rules: {
            selCnesUni: "CodCnesUnidadeSelecionado",
            selIneEquipe: {
                required: true
            },
            txtData: {
                required: true
            },
            rdbSexo: {
                required: true
            },
            selLocalAtend: {
                required: true
            },
            rdbtipoAtend: {
                required: true
            },
            'chkCondulta[]': {
                required: true,
                minlength: 1
            }
        },
        messages: {
            selCnesUni: "Campo Código CNES Unidade é obrigatório",
            selIneEquipe: "Campo INE é obrigatório",
            txtData: "Data é obrigatória",
            rdbSexo: "Campo Sexo é obrigatório",
            selLocalAtend: "Campo Local de Atendimento é obrigatório",
            rdbtipoAtend: "Tipo de Atendimento é obrigatório",
            'chkCondAvaliada[]': "Selecione no mínimo uma opção",
            'chkCondulta[]': "Selecione no mínimo uma opção"
        },
        submitHandler: function (form) {
            if (verificarItensCondicoesAvaliada())
            {
                alteraPacienteAtendimento();
            }            
        }
    });

    //Validator
    jQuery.validator.addMethod("CodCnesUnidadeSelecionado", function (value) {
        if (value > 0) {
            return true;
        }
        else {
            return false;
        }
    }, 'Campo Código CNES Unidade é obrigatório');
});

function finalizar() {
    if (verificarItensnaTable()) {
        alteraAtendimento(2);
    }
}

function modalAlerta(xTitulo, xTexto) {
    var modalInicio = "<div class='modal fade' id='alertaModal' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'><div class='modal-dialog'><div class='modal-content'><div class='modal-header'><button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button><h4 class='modal-title' id='myModalLabel'>" + xTitulo + "</h4></div><div class='modal-body'>";
    var modalFim = "</div><div class='modal-footer'><button type='button' class='btn btn-default' data-dismiss='modal'>Fechar</button></div></div></div></div>";

    $("#alerta").empty();
    $("#alerta").append(modalInicio + xTexto + modalFim);
    $("#alertaModal").modal("show");
}

function verificarItensCondicoesAvaliada() {
    var retorno = true;
    var select = jQuery("#CondicaoAvaliada select").filter(function () {
        return this.value == '0' || this.value == null || this.value == '';
    });    
    var checkbox = jQuery('#CondicaoAvaliada input[type="checkbox"]:checked');
    jQuery('#msgErroCondicaoAvaliada').text('');
    if ((select.length == 3) && checkbox.length == 0) {
        jQuery('#msgErroCondicaoAvaliada').text('É necessário informar no mínimo uma Condição Avaliada.');
        jQuery('#CondicaoAvaliada input[type="checkbox"]').eq(0).focus();
        retorno = false;
    }
    return retorno;
}

function cancelar() {
    if ($('#listaAtendimentoIndividual tbody tr').length == 0) {
        window.location.assign("atendimentoIndividual.asp");
    }
}

function pacienteNovamente() {

    $('#modalSemCartaoSus').modal('hide');
    busca_Paciente_Sus();

}

function busca_Paciente_Sus() {
    $('#txtmodalBuscaCartaoSus').val('');
    $('#txtmodalConfirmaCartaoSus').val('');    
    limparFormAtendimentoUsuario();
    jQuery('#btnFinalizarAtend').hide();
    jQuery('.IdentificacaoUsuario').hide();
    jQuery("#listaAtendimentoIndividual tbody tr ").removeClass("active");
    jQuery("#btnConfirmar").hide();
    jQuery("#btnCancelar").hide();
    $('.msgErroDuplicado').text('');
    jQuery(".modal-title").html("Busca de Paciente");
    jQuery("#selModalTpBusca").val('1');
    jQuery("#txtModalBusca").val('');
    jQuery("label[for=txtModalBusca]").html('Nº SUS *');
    jQuery("#txtModalBusca").attr('placeholder', 'Número');
    jQuery('#modalProfSus').find('input[type=text], select').each(function () {
        jQuery(this).parents(".form-group").removeClass("has-error");
    });
    jQuery('#listamodalProfSus tbody').empty();
    jQuery('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center">Efetue a busca acima</th></tr>');
    jQuery('#modalProfSus').modal('show');
}

function busca_Prof_Sus(event) {
    if (event.currentTarget.id == "btnBuscarProf") {
        $('#modalProfSus').addClass('add');
    } else {
        $('#modalProfSus').removeClass('add');
    }
    $('.msgErroDuplicado').text('');
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
                    var strTable = '<tr class="sel-cns" data-nome="' + item.nome.toCapitalize() + '" data-cns="' + item.cns + '" data-idusr="' + item.IDUSR + '" >';
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
        $('#listamodalProfSus tbody .sel-cns').on('click', function (event) {
            $(this).siblings().removeClass('list-group-item-success');
            var idAtendimento = $("#idAtendimento").val();
            if (!jQuery("#" + $(this).data('idusr')).hasClass("sel-cns")) {
                var idPacienteAtendimento = criaPacienteAtendimento(idAtendimento, $(this).data("idusr"), $(this).data('cns'), $(this).data('nome'));
                if (idPacienteAtendimento != false) {
                    getPacientes();
                } else {
                    modalAlerta("Atenção", "Falha ao adicionar o Paciente no Atendimento, tente novamente mais tarde.");
                }
            } else {
                $('.msgErroDuplicado').text('O Paciente selecionado já foi inserido no Atendimento e não poderá ser inserido novamente.');
            }
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

function executaSelectExames() {
    var seleciona = $('#txtExame').val();
    var solicitado = $('#chkOutrosSiaSolicitados').is(":checked");
    var avaliado = $('#chkOutrosSiaAvaliados').is(":checked");
    var textcombo = $('#txtExame').val();
    var idAtendimento = jQuery("#idAtendimento").val();
    var idAtendimentoUsuario = jQuery(".IdentificacaoUsuario").data("id");
    var checadoSol = "";
    var checadoAval = "";
    if (solicitado == true)
    { checadoSol = "S"; } else { checadoSol = ""; }
    if (avaliado == true)
    { checadoAval = "A"; } else { checadoAval = ""; }
    if (seleciona != "") {
        if (checadoSol != '' || checadoAval != '') {
            if ($("#listaExames tr[data-exame='" + seleciona + "']").length == 0) {
                var idExame = salvarExamesPacienteAtendimento(idAtendimento, idAtendimentoUsuario, textcombo, checadoSol, checadoAval);
                if (idExame != false) {
                    $('#txtExame').val('Selecione');
                    $('#chkOutrosSiaSolicitados').prop("checked", false);
                    $('#chkOutrosSiaAvaliados').prop("checked", false);
                    makeTableExamesPaciente(idAtendimento, idAtendimentoUsuario, textcombo, checadoSol, checadoAval, idExame, seleciona);
                } else {
                    modalAlerta("Atenção", "Falha ao adicionar o Exame do Paciente no Atendimento, tente novamente mais tarde.");
                }
            } else {
                //modalAlerta("Atenção", "Selecione se o Exame foi Solicitado e/ou Avaliado.");
                $('#txtExame').val('Selecione');
                $('#chkOutrosSiaSolicitados').prop("checked", false);
                $('#chkOutrosSiaAvaliados').prop("checked", false);
                modalAlerta("Atenção", "Exame do Paciente já foi selecionado neste Atendimento.");
            }
        } else {
            modalAlerta("Atenção", "Selecione se o Exame foi Solicitado e/ou Avaliado.");
        }
    } else {
        modalAlerta("Atenção", "Selecione no mínimo um Exame para confirmar.");
    }

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
                    var strTable = '<tr class="sel-cns" data-nome="' + item.nome.toCapitalize() + '" data-cns="' + item.cns + '" data-idprof="' + item.IdProf.trim() + '" data-cbo="' + item.CBO + '" >';
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
            }else{
                //Remove o active dos outros
                $(this).siblings().removeClass("list-group-item-success");
                var linhas = $("#listaProfissionaisAtendimento tbody tr.sel-cns").length;
                var idAtendimento = jQuery("#idAtendimento").val();
                var add = $('#modalProfSus').hasClass('add');
                if (add) {
                    if (!jQuery("#" + $(this).data("idprof")).hasClass("sel-cns")) {
                        var idProfissionalAtendimento = salvaProfissionalAtendimento(idAtendimento, $(this).data("idprof"), $(this).data("cbo"));
                        if (idProfissionalAtendimento != false) {
                            $("#listaProfissionaisAtendimento tbody tr button.btn-danger").show();
                            $("#listaProfissionaisAtendimento tbody tr button.btn-success").hide();
                            var unico = (linhas == 0);
                            makeTableProfissional($(this).data("nome"), $(this).data("cns"), $(this).data("idprof"), idProfissionalAtendimento, $(this).data("cbo"), unico);
                        } else {
                            modalAlerta("Atenção", "Falha ao adicionar o Profissional no Atendimento, tente novamente mais tarde.");
                        }
                    } else {
                        $('.msgErroDuplicado').text('O Profissional selecionado já foi inserido no Atendimento e não poderá ser inserido novamente.');
                    }
                } else {
                    alteraInfoProfissional(event);
                }
            }
        });
    }
    return false;
}

function alteraInfoProfissional(event) {
    var rowProfissional = $("#listaProfissionaisAtendimento tbody tr.sel-cns");
    var idProfAtend = rowProfissional.data("idprofatend");
    if (substituirProfissional(idProfAtend, $(event.currentTarget).data("idprof"), $(event.currentTarget).data("cbo"))) {
        makeTableProfissional(
            $(event.currentTarget).data("nome"),
            $(event.currentTarget).data("cns"),
            $(event.currentTarget).data("idprof"),
            idProfAtend,
            $(event.currentTarget).data("cbo"),
            true
        );
        rowProfissional.remove();
    } else {
        $('.msgErroDuplicado').text('Ocorreu um erro ao substituir o profissional.');
    }
}

function salvaProfissionalAtendimento(idAtendimento, idProfissional, cbo) {
    var blnRet = false;
    alteraAtendimento(1);
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createProfissional',
            idAtendimentoIndividual: idAtendimento,
            idProfissionalSaude: idProfissional,
            cbo: cbo
        },
        datatype: "json",
    url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
    })
    .done(function (data) {
        if (data > 0) {
            blnRet = parseInt(data);
        }
    });
    return blnRet;
}

function salvarExamesPacienteAtendimento(idAtendimento, idPacienteAtendimento, Descricao, solicitado, avaliado) {
    var blnRet = false;
    jQuery.ajax({
        type: "POST",
        async: false,
        data: { acao: 'createExamesPaciente', idAtendimentoIndividual: idAtendimento, idAtendimentoUsuario: idPacienteAtendimento,Descricao : Descricao, Solicitado :solicitado, Avaliado :avaliado },
        dataType: "json",
        url: Api.URL
    })
    .done(function (data) {
        if (data > 0) {
            blnRet = parseInt(data);
        }
    });
    return blnRet;
}

function substituirProfissional(idProfAtend, idProfissional, cbo) {
    var blnRet = false;
    alteraAtendimento(1);
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'substituirProfissional',
            idProfAtend: idProfAtend,
            idProfissionalSaude: idProfissional,
            cbo: cbo
        },
        datatype: "json",
        url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
    })
    .success(function (data) {
        blnRet = true;
    });
    return blnRet;
}

function removerProfissionalAtendimento(idProfissionalAtendimento, tr) {
        $.ajax({
            type: "POST",
            data: { acao: 'removeProfissional', id: idProfissionalAtendimento },
            datatype: "json",
            url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
        })
        .done(onSuccess)
        .fail(onError);;

        function onSuccess() {
            $(tr).remove();
        }
        function onError() {
            modalAlerta("Atenção", "Falha ao remover o Profissional, tente novamente mais tarde.");
        }
}

function criaPacienteAtendimento(idAtendimentoIndividual, idIdentificacaoUsuario, numeroSus, nome) {
    var blnRet = false;
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createUsuarioAtendido',
            idAtendimentoIndividual: idAtendimentoIndividual,
            idIdentificacaoUsuario: idIdentificacaoUsuario,                            //valor padrão
            numeroCartaoSus: numeroSus,
            nome: nome
        },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        if (data > 0)
            blnRet = parseInt(data);
    });
    return blnRet;
}

function concatenarAvaliadosSolicitados(nameElemento) {
    var exame = "";
    var total = jQuery("[name='" + nameElemento + "[]']:checked").length - 1;
    jQuery("[name='" + nameElemento + "[]']:checked").each(function (i) {
        exame += jQuery(this).val();
        if (i < total)
            exame += ",";
    });
    return exame;
}

function exibirAvaliadosSolicitados(nameElemento,value) {
    if (value != "" && value != null) {
        var string = value.split(",");
        if (string.length > 0) {
            if (string[0] == "S")
                jQuery("[name='" + nameElemento + "[]'][value ='S']").prop("checked", true);
            if (string[0] == "A")
                jQuery("[name='" + nameElemento + "[]'][value ='A']").prop("checked", true);
            if (string.length > 1) {
                if (string[1] == "A")
                    jQuery("[name='" + nameElemento + "[]'][value ='A']").prop("checked", true);
            }
        }
    }
}

function dadosUsuarioAtendido() {
    var turnos = "";
    jQuery("[name='chkTurno']:checked").each(function (i) {
        turnos = turnos == "" ? turnos + jQuery(this).val() : turnos + "," + jQuery(this).val();
    });
    var dados = {
        Id : parseInt(jQuery(".IdentificacaoUsuario").data("id")),
        Turno: turnos == "" ?  null : turnos,
        NumeroProntuario: jQuery("#txtProntuario").val() == "" ? null : jQuery("#txtProntuario").val(),
        NumeroCartaoSUS: jQuery("#txtCartaoSUS").val() == "" ? null : jQuery("#txtCartaoSUS").val(),
        Nome: jQuery("#nome").val(),
        DataNascimento: jQuery("#txtDataNascimento").val(),
        Sexo: jQuery("[name='rdbSexo']:checked").val(),
        LocalAtendimento: jQuery("#selLocalAtend :selected").val() == undefined ? null : jQuery("#selLocalAtend :selected").val(),
        IdTipoAtendimento: jQuery("[name='rdbtipoAtend']:checked").val() == undefined ? null : jQuery("[name='rdbtipoAtend']:checked").val(),
        Peso: jQuery("#txtPeso").val() == "" ? null : jQuery("#txtPeso").val().replace(",", "."),
        Altura: jQuery("#txtAltura").val() == "" ? null : jQuery("#txtAltura").val(),
        VacinacaoEmDia: jQuery("[name='rdbVacinacaoEmDia']:checked").val() == undefined ?  null : jQuery("[name='rdbVacinacaoEmDia']:checked").val(),
        CriancaAleitamentoMaterno: jQuery("#selAleitamento :selected").val() == undefined ? null : jQuery("#selAleitamento :selected").val(),
        GestanteDum: jQuery("#txtDataDum").val() == "" ?  null : jQuery("#txtDataDum").val(),
        GestanteGravidezPlanejada: jQuery("[name='rdbGravidez']:checked").val() == undefined ?  null : jQuery("[name='rdbGravidez']:checked").val(),
        GestanteIdadeGestacional: jQuery("#txtIdadeGestacao").val() == "" ? null : jQuery("#txtIdadeGestacao").val(),
        GestanteGestasPrevias: jQuery("#txtGestas").val() == "" ? null : jQuery("#txtGestas").val(),
        GestantePartos: jQuery("#txtPartos").val() == "" ? null : jQuery("#txtPartos").val(),
        ModalidadeAd: jQuery("[name='rdbModalidade']:checked").val() == undefined ?  null : jQuery("[name='rdbModalidade']:checked").val(),
        Asma: jQuery("#chkasma").is(":checked") ? 1 : null,
        Desnutricao: jQuery("#chkdesnutricao").is(":checked") ? 1 : null,
        Diabetes: jQuery("#chkdiabetes").is(":checked") ? 1 : null,
        Dpoc: jQuery("#chkdpoc").is(":checked") ? 1 : null,
        Hipertensao: jQuery("#chkhipertensao").is(":checked") ? 1 : null,
        Obesidade: jQuery("#chkobesidade").is(":checked") ? 1 : null,
        PreNatal: jQuery("#chkpreNatal").is(":checked") ? 1 : null,
        Puericultura: jQuery("#chkpuericultura").is(":checked") ? 1 : null,
        Puerperio: jQuery("#chkpuerperio").is(":checked") ? 1 : null,
        SaudeSexualReprodutiva: jQuery("#chksaudeSexualReprodutiva").is(":checked") ? 1 : null,
        Tabagismo: jQuery("#chktabagismo").is(":checked") ? 1 : null,
        UsuarioAlcool: jQuery("#chkusuarioAlcool").is(":checked") ? 1 : null,
        UsuarioOutrasDrogas: jQuery("#chkusuarioOutrasDrogas").is(":checked") ? 1 : null,
        SaudeMental: jQuery("#chksaudeMental").is(":checked") ? 1 : null,
        Reabilitacao: jQuery("#chkreabilitacao").is(":checked") ? 1: null,
        Tuberculose: jQuery("#chktuberculose").is(":checked") ? 1 : null,
        Hanseniase: jQuery("#chkhanseniase").is(":checked") ? 1 : null,
        Dengue: jQuery("#chkdengue").is(":checked") ? 1 : null,
        Dst: jQuery("#chkdst").is(":checked") ? 1 : null,
        CancerColoUtero: jQuery("#chkcancerColoUtero").is(":checked") ? 1 : null,
        CancerMama: jQuery("#chkcancerMama").is(":checked") ? 1 : null,
        RiscoCardiovascular: jQuery("#chkriscoCardiovascular").is(":checked") ? 1 : null,
        Ciap2_01: jQuery("#selCiap01 :selected").val() == undefined ? null : jQuery("#selCiap01 :selected").val(),
        Ciap2_02: jQuery("#selCiap02 :selected").val() == undefined ? null : jQuery("#selCiap02 :selected").val(),
        Cid10_01: jQuery("#selCid10 :selected").val() == undefined ? null : jQuery("#selCid10 :selected").val(),
        ColestorolTotal: concatenarAvaliadosSolicitados("chkcolesterolTotal"),
        Creatinina: concatenarAvaliadosSolicitados("chkcreatinina"),
        EasEqu: concatenarAvaliadosSolicitados("chkeasEqu"),
        Eletrocardiograma: concatenarAvaliadosSolicitados("chkeletrocardiograma"),
        EletroforeseDeHemoglobina: concatenarAvaliadosSolicitados("chkeletroforeseDeHemoglobina"),
        Espirometria: concatenarAvaliadosSolicitados("chkespirometria"),
        ExameDeEscarro: concatenarAvaliadosSolicitados("chkexamedeescarro"),
        Glicemia: concatenarAvaliadosSolicitados("chkglicemia"),
        Hdl: concatenarAvaliadosSolicitados("chkhdl"),
        HemoglobinaGlicada: concatenarAvaliadosSolicitados("chkhemoglobinaGlicada"),
        Hemograma: concatenarAvaliadosSolicitados("chkhemograma"),
        Ldl: concatenarAvaliadosSolicitados("chkldl"),
        RetinografiaFundoDeOlhoComOftalmologista: concatenarAvaliadosSolicitados("chkretinografiaFundoDeOlhoComOftalmologista"),
        SorologiaDeSiflisVdrl: concatenarAvaliadosSolicitados("chksorologiaDeSiflisVdrl"),
        SorologiaParaDengue: concatenarAvaliadosSolicitados("chksorologiaParaDengue"),
        SorologiaParaHiv: concatenarAvaliadosSolicitados("chksorologiaParaHiv"),
        TesteIndiretoDeAntiglobinaHumanaTia: concatenarAvaliadosSolicitados("chktesteIndiretoDeAntiglobinaHumanaTia"),
        TesteDaOrelhinha: concatenarAvaliadosSolicitados("chktesteDaOrelhinha"),
        TesteDeGravidez: concatenarAvaliadosSolicitados("chktesteDeGravidez"),
        TesteDoOlhinho: concatenarAvaliadosSolicitados("chktesteDoOlhinho"),
        TesteDoPezinho: concatenarAvaliadosSolicitados("chktesteDoPezinho"),
        UltrassonografiaObstetrica: concatenarAvaliadosSolicitados("chkultrassonografiaObstetrica"),
        Urocultura: concatenarAvaliadosSolicitados("chkurocultura"),
        Pic: jQuery("#selPic :selected").val() == undefined ? null : jQuery("#selPic :selected").val(),
        FicouEmObservacao: jQuery("[name='rdbObs']:checked").val() == undefined ?  null : jQuery("[name='rdbObs']:checked").val(),
        AvaliacaoDiagnostico: jQuery("#chkavaliacaoDiagnostico").is(":checked") ? 1 : null,
        ProcedimentosClinicosTerapeuticos: jQuery("#chkrdbprocedimentosClinicosTerapeuticos").is(":checked") ? 1 : null,
        PrescricaoTerapeutica: jQuery("#chkrdbprescricaoTerapeutica").is(":checked") ? 1 : null,
        RetornoParaConsultaAgendada: jQuery("#chkretornoParaConsultaAgendada").is(":checked") ? 1 : null,
        RetornoParaCuidadoContinuadoProgramado: jQuery("#chkretornoParaCuidadoContinuadoProgramado").is(":checked") ? 1 : null,
        AgendamentoParaGrupos: jQuery("#chkagendamentoParaGrupos").is(":checked") ? 1 : null,
        AgendamentoParaNasf: jQuery("#chkagendamentoParaNasf").is(":checked") ? 1 : null,
        AltaDoEpisodio: jQuery("#chkaltaDoEpisodio").is(":checked") ? 1 : null,
        EncaminhamentoInternoNoDia: jQuery("#chkencaminhamentoInternoNoDia").is(":checked") ? 1 : null,
        EncaminhamentoParaServicoEspecializado: jQuery("#chkencaminhamentoParaServicoEspecializado").is(":checked") ? 1 : null,
        EncaminhamentoParaCaps: jQuery("#chkencaminhamentoParaCaps").is(":checked") ? 1 : null,
        EncaminhamentoParaInternacaoHospitalar: jQuery("#chkencaminhamentoParaInternacaoHospitalar").is(":checked") ? 1 : null,
        EncaminhamentoParaUrgencia: jQuery("#chkencaminhamentoParaUrgencia").is(":checked") ? 1: null,
        EncaminhamentoParaServicoDeAtencaoDomiciliar: jQuery("#chkencaminhamentoParaServicoDeAtencaoDomiciliar").is(":checked") ? 1 : null,
        EncaminhamentoIntersetorial: jQuery("#chkencaminhamentoIntersetorial").is(":checked") ? 1 : null,
        IdAtendimentoIndividualUsuarioAtendido: jQuery("#idAtendimento").val(),
        Condutas: null,
        //Tratar depois com data
        Numero: null
    };

    dados = preencheCondutas(dados);

    return JSON.stringify(dados);
}

function preencheCondutas(dados) {
    dados.Condutas = new Array(12);

    if (dados.RetornoParaConsultaAgendada) {
        dados.Condutas[0] = 1;
    }

    if (dados.RetornoParaCuidadoContinuadoProgramado) {
        dados.Condutas[1] = 2;
    }

    if (dados.AgendamentoParaGrupos) {
        dados.Condutas[2] = 12;
    }

    if (dados.AgendamentoParaNasf) {
        dados.Condutas[3] = 3;
    }

    if (dados.AltaDoEpisodio) {
        dados.Condutas[4] = 9;
    }

    if (dados.EncaminhamentoInternoNoDia) {
        dados.Condutas[5] = 11;
    }

    if (dados.EncaminhamentoParaServicoEspecializado) {
        dados.Condutas[6] = 4;
    }

    if (dados.EncaminhamentoParaCaps) {
        dados.Condutas[7] = 5;
    }

    if (dados.EncaminhamentoParaInternacaoHospitalar) {
        dados.Condutas[8] = 6;
    }

    if (dados.EncaminhamentoParaUrgencia) {
        dados.Condutas[9] = 7;
    }

    if (dados.EncaminhamentoParaServicoDeAtencaoDomiciliar) {
        dados.Condutas[10] = 8;
    }

    if (dados.EncaminhamentoIntersetorial) {
        dados.Condutas[11] = 10;
    }

    dados.Condutas = JSLINQ(dados.Condutas)
                        .Where(function (item) { return item != null; }).items;

    return dados;
}

function dadosAtendimentoIndividual() {
    var dados = {
        codigoCnesUnidade : $('#selCnesUni :selected').val(),
        codEquipe: ($('#selIneEquipe').val() == "" || $('#selIneEquipe').val() == "0") ? null : $('#selIneEquipe').val(),
        conferidoPor :"",
        dataAtendimento: $('#txtData').val(),
        numeroFolha: null,
        Id: jQuery("#idAtendimento").val()
    };
    return JSON.stringify(dados);
}

function alteraPacienteAtendimento() {
    
    jQuery.ajax({
        type: "POST",
        data: {
            acao: 'editUsuarioAtendido',
            dadosAtendimentoIndividualUsuarioAtendido: dadosUsuarioAtendido()
        },
        datatype: "json",
        url: Api.URL
    }).done(onSuccess);

    function onSuccess() {
        limparFormAtendimentoUsuario();
        $("#dadosUsuario").appendTo("#identificacaoUsuarioOculta");
        $("#dadosAvaliacao").appendTo("#identificacaoUsuarioOculta");
        $('#btnConfirmar, #btnCancelar').css("display", "none");
        $('#btnFinalizarAtend').show();
        modalAlerta("Confirmação", "Atendimento salvo com sucesso!");
    }

}

function alteraAtendimento(status) {
    
    $.ajax({
        type: "POST",
        data: {
            acao: 'Edit',
            dadosAtendimentoIndividual: dadosAtendimentoIndividual(),
            status: status
        },
        datatype: "json",
        url: Api.URL
    }).done(function () {
        if(status == 2)
        {
            $.ajax({
                type: "POST",
                data: { acao: 'GetValidacao', id: jQuery("#idAtendimento").val() },
                datatype: "json",
                url: Api.URL
            })
            .done(function (data) {
                data = JSON.parse(data)
                if (data.status) {
                    modalAlerta("Confirmação", "Atendimento finalizado com sucesso!")
                    setInterval(function () { window.location = "atendimentoIndividual.asp" }, 5000);
                }
                else {
                    modalAlerta("Erro", data.resultado);
                }
            });
        }
    });
}

function removerPacienteAtendimento(idPacienteAtendimento) {
    $.ajax({
        type: "POST",
        data: { acao: 'removeUsuarioAtendido', id: idPacienteAtendimento },
        datatype: "json",
        url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
    })
    .done(function (data) {
        if ($('#listaAtendimentoIndividual tbody tr').length == 0) {
            limparFormAtendimentoUsuario();
            $('#btnFinalizarAtend').hide();
            $('.IdentificacaoUsuario').hide();
        }
        else if ($('#listaAtendimentoIndividual tbody tr').length == 1)
        {
            $("#listaAtendimentoIndividual tbody tr ").removeClass("active");
        }
    });
}

function limparFormAtendimentoUsuario() {
    $('.IdentificacaoUsuario').each(function (i) {
        $(this).find('input[type=checkbox]:checked').removeAttr('checked', false);
        $(this).find('input[type=text]').val('');
        $(this).find('input[type=number]').val('');
        $(this).find('input[type=radio]:checked').removeAttr('checked', false);
        $('#listaExames tbody').empty();
    });
    removerValidacoesParticipantes();
}

function makeTablePaciente(numero, nome, cns, idusr, idusratend) {
    var strTable = '';
    strTable += '   <tr id="' + idusr + '" class="sel-cns" data-nome="' + nome.toCapitalize() + '" data-cns="' + cns + '" data-idusr="' + idusr + '" data-idusratend="' + idusratend + '" >';
    strTable += '       <td class="text-center">' + numero + '</td>';
    strTable += '       <td class="text-capitalize">' + nome.toCapitalize() + '</td>';
    strTable += '       <td class="text-center">' + cns + '</td>';
    strTable += '       <td class="text-center"><button type="button" class="btn btn-danger" title="Remover"><span class="glyphicon glyphicon-remove"></span></button></td>';
    strTable += "       <td class='text-center'><button type='button' class='btn btn-primary preencherDadosPaciente' title='Preencher Ficha'><span class='glyphicon glyphicon-edit'></span></button></td>";
    strTable += '   </tr>';
    $('#listaAtendimentoIndividual tbody').append(strTable);
    $('#msgErroTableUsu').text('');
    removeGridPacientes();
    desabilitaBuscaPaciente();
    habilitaFichaPaciente();
    $('#modalProfSus').modal('hide');
}

function makeTableProfissional(nome, cns, idprof, idprofatend, cbo, unico) {
    var strTable = "";
    strTable += "   <tr id='" + idprof + "' class='sel-cns' data-nome='" + nome.toCapitalize() + "' data-cns='" + cns + "' data-idprof='" + idprof + "' data-idprofatend='" + idprofatend + "' data-cbo='" + cbo + "'>";
    strTable += "       <td class='text-capitalize'>" + nome.toCapitalize() + "</td>";
    strTable += "       <td class='text-center'>" + cns + "</td>";
    strTable += "       <td class='text-center'>" + cbo + "</td>";
    var displayRemover = (unico) ? "none" : "block" ;
    var displaySubstituir = (unico) ? "block" : "none";
    strTable += "       <td class='text-center'><button style='display:" + displaySubstituir + "' type='button' class='btn btn-success' title='Substituir'><span class='glyphicon glyphicon-refresh'></span></button>";
    strTable += "       <button style='display:" + displayRemover + "' type='button' class='btn btn-danger' title='Remover'><span class='glyphicon glyphicon-remove'></span></button></td>";
    strTable += "   </tr>";
    $("#listaProfissionaisAtendimento tbody").append(strTable);
    $('#msgErroTableProf').text('');
    substituiProfSus();
    removeGridProfissionais();
    desabilitaBuscaProfissional();
    $("#modalProfSus").modal("hide");
}

function makeTableExamesPaciente(idAtendimento, idAtendimentoUsuario, textcombo, checadoSol, checadoAval, idExame, seleciona) {
    if (checadoSol == null) { checadoSol = ''; }
    if (checadoAval == null) { checadoAval = ''; }
    var strTable = '';
    strTable += '   <tr data-exame="' + seleciona + '" class="sel-exame" data-idexame="' + idExame + '" data-solicitado="' + checadoSol + '" data-avaliado="' + checadoAval + '">';
    strTable += '       <td class="text-capitalize">' + textcombo + '</td>';
    strTable += '       <td class="text-center">' + checadoSol + '</td>';
    strTable += '       <td class="text-center">' + checadoAval + '</td>';
    strTable += '       <td class="text-center"><button type="button" class="btn btn-danger" title="Remover"><span class="glyphicon glyphicon-remove"></span></button></td>';
    strTable += '   </tr>';
    $('#listaExames tbody').append(strTable);
    removeGridExames();
    desabilitaBuscaExames();
}

function removeGridExames() {
    $('#listaExames tbody .btn-danger').click(function () {
        var tr = $(this).closest("tr");
        removerExamesPacienteAtendimento($(tr).data("idexame"));
        $(tr).remove();
        desabilitaBuscaExames();
    });
}

function removerExamesPacienteAtendimento(idExame) {
    $.ajax({
        type: "POST",
        data: { acao: 'removeExamesPaciente', id: idExame },
        datatype: "json",
        url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
    })
    .done(function (data) {
    });
}

function removeGridProfissionais() {
    $('#listaProfissionaisAtendimento tbody .btn-danger').click(function () {
        var rows = $("#listaProfissionaisAtendimento tbody tr").length;
        var tr = $(this).closest("tr");
        if (rows > 1) {
            removerProfissionalAtendimento($(tr).data("idprofatend"), tr);
            desabilitaBuscaProfissional();
            if (rows == 2) {
                $("#listaProfissionaisAtendimento tbody tr button.btn-danger").hide();
                $("#listaProfissionaisAtendimento tbody tr button.btn-success").show();
            }
        }
    });
}

function desabilitaBuscaProfissional() {
    if ($('#listaProfissionaisAtendimento tbody tr').length > 2) {
        $('#btnBuscarProf').attr('disabled', 'disabled');
    } else {
        $('#btnBuscarProf').removeAttr('disabled');
    }
}

function removeGridPacientes() {
    $('#listaAtendimentoIndividual tbody .btn-danger').click(function () {
        var tr = $(this).closest("tr");
        removerPacienteAtendimento($(tr).data("idusratend"));
        $(tr).remove();
        desabilitaBuscaPaciente();
    });
}

function desabilitaBuscaPaciente() {
    if ($('#listaAtendimentoIndividual tbody tr').length > 12) {
        $('#btnBuscarUsu').attr('disabled', 'disabled');
        $('#btnAddSemCadastro').attr('disabled', 'disabled');
    } else {
        $('#btnAddSemCadastro').removeAttr('disabled');
        $('#btnBuscarUsu').removeAttr('disabled');
    }
}

function verificarItensnaTable(event)
{
    var retorno = true;
    $('#msgErroTableUsu').text('');
    $('#msgErroTableProf').text('');
    if ($('#listaAtendimentoIndividual tbody tr').length == 0) {
        $('#msgErroTableUsu').text('Insira no mínimo um Paciente para realizar o Atendimento.');
        $('#btnBuscarUsu').focus();
        retorno = false;
    }
    if ($('#listaProfissionaisAtendimento tbody tr').length == 0) {
        $('#msgErroTableProf').text('Insira no mínimo um Profissional para realizar o Atendimento.');
        $('#btnBuscarProf').focus();
        retorno = false;
    }
    return retorno;
   
}

function desabilitaBuscaExames() {
    if ($('#listaExames tbody tr').length >= 3) {
        $('#btnConfObs').attr('disabled', 'disabled');
    } else {
        $('#btnConfObs').removeAttr('disabled');
    }
}

function habilitaFichaPaciente() {
    $("#listaAtendimentoIndividual tbody .preencherDadosPaciente").on("click", function () {
        var data = $(this).closest("tr").data("idusratend");
        $("#listaAtendimentoIndividual tbody tr ").removeClass("active");

        if ($('.IdentificacaoUsuario').parent("#identificacaoUsuarioOculta").length > 0) {
            $("#dadosUsuario").appendTo("#Usuario");
            $("#dadosAvaliacao").appendTo("#UsuarioAvaliacao");
        }
        else {
            $('.IdentificacaoUsuario').show();
        }

        if ($("#listaAtendimentoIndividual tbody tr ").length > 1) {
            jQuery(this).closest("tr").addClass("active");
        }

        if (data != $('.IdentificacaoUsuario').data("id")) {
            limparFormAtendimentoUsuario();
        }

        $('.IdentificacaoUsuario').data("id", data);
        getFichaPaciente(data);
        getExamesPaciente();
        $('#btnConfirmar').show();
        $('#btnCancelar').show();
    });
    
}

function getFichaPaciente(idUsrAtend) {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getUsuarioAtendidoById', idAtendimentoIndividual: idUsrAtend },
        datatype: "json",
        url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data)[0];
        makeDadosPaciente(obj);
    });
}

function makeDadosPaciente(item) {    
    jQuery("#txtProntuario").val(item.numero_prontuario);
    jQuery("#txtCartaoSUS").val(item.numero_cartao_sus);
    jQuery("#nome").val(item.nome);

    if (item.data_nascimento != '') {
        var dataNasc = item.data_nascimento.split("-");    
        jQuery("#txtDataNascimento").val(dataNasc[2] + "/" + dataNasc[1] + "/" + dataNasc[0]);
    }

    if (item.turno != null) {
        var turnos = item.turno.split(',');

        jQuery(turnos).each(function (i) {
            jQuery("[name='chkTurno'][value='" + this + "']").prop("checked", true);
        });
    }

    if (item.sexo != null) {
        jQuery("[name='rdbSexo'][value='" + item.sexo + "']").prop("checked", true);
    }

    jQuery("#selLocalAtend").val(item.local_atendimento);
    jQuery("[name='rdbtipoAtend'][value='" + item.id_tipo_atendimento + "']").prop("checked", true);
    jQuery("#txtPeso").val(String(item.peso).replace(",", "."));
    jQuery("#txtAltura").val(String(item.altura).replace(",", "."));
    jQuery("[name='rdbVacinacaoEmDia'][value='" + item.vacinacao_em_dia + "']").prop("checked", true);
    jQuery("#selAleitamento").val(item.crianca_aleitamento_materno);

    if (item.gestante_dum != null) {
        if (item.gestante_dum.indexOf('-') > 0) {
            var dataDum = item.gestante_dum.split("-");
            jQuery("#txtDataDum").val(dataDum[2] + "/" + dataDum[1] + "/" + dataDum[0]);
        }
    }

    jQuery("[name='rdbGravidez'][value='" + item.gestante_gravidez_planejada + "']").prop("checked", true);
    jQuery("#txtIdadeGestacao").val(item.gestante_idade_gestacional);
    jQuery("#txtGestas").val(item.gestante_gestas_previas);
    jQuery("#txtPartos").val(item.gestante_partos);
    jQuery("[name='rdbModalidade'][value='" + item.modalidade_ad + "']").prop("checked", true);
    if (item.asma                    ) { $("#chkasma").prop("checked", true) };
    if (item.desnutricao             ) { $("#chkdesnutricao").prop("checked", true) };
    if (item.diabetes                ) { $("#chkdiabetes").prop("checked", true) };
    if (item.dpoc                    ) { $("#chkdpoc").prop("checked", true) };
    if (item.hipertensao_arterial    ) { $("#chkhipertensao").prop("checked", true) };
    if (item.obesidade               ) { $("#chkobesidade").prop("checked", true) };
    if (item.pre_natal               ) { $("#chkpreNatal").prop("checked", true) };
    if (item.puericultura            ) { $("#chkpuericultura").prop("checked", true) };
    if (item.puerperio               ) { $("#chkpuerperio").prop("checked", true) };
    if (item.saude_sexual_reprodutiva) { $("#chksaudeSexualReprodutiva").prop("checked", true) };
    if (item.tabagismo               ) { $("#chktabagismo").prop("checked", true) };
    if (item.usuario_alcool          ) { $("#chkusuarioAlcool").prop("checked", true) };
    if (item.usuario_outras_drogas   ) { $("#chkusuarioOutrasDrogas").prop("checked", true) };
    if (item.saude_mental            ) { $("#chksaudeMental").prop("checked", true) };
    if (item.reabilitacao            ) { $("#chkreabilitacao").prop("checked", true) };
    if (item.tuberculose             ) { $("#chktuberculose").prop("checked", true) };
    if (item.hanseniase              ) { $("#chkhanseniase").prop("checked", true) };
    if (item.dengue                  ) { $("#chkdengue").prop("checked", true) };
    if (item.dst                     ) { $("#chkdst").prop("checked", true) };
    if (item.cancer_colo_utero       ) { $("#chkcancerColoUtero").prop("checked", true) };
    if (item.cancer_mama             ) { $("#chkcancerMama").prop("checked", true) };
    if (item.risco_cardiovascular    ) { $("#chkriscoCardiovascular").prop("checked", true) };
    jQuery("#selCiap01").val(item.ciap2_01);
    jQuery("#selCiap02").val(item.ciap2_02);
    jQuery("#selCid10").val(item.cid10_01);
    exibirAvaliadosSolicitados("chkcolesterolTotal", item.colesterol_total);
    exibirAvaliadosSolicitados("chkcreatinina", item.creatinina);
    exibirAvaliadosSolicitados("chkeasEqu", item.eas_equ);
    exibirAvaliadosSolicitados("chkeletrocardiograma", item.eletrocardiograma);
    exibirAvaliadosSolicitados("chkeletroforeseDeHemoglobina", item.eletroforese_de_hemoglobina);
    exibirAvaliadosSolicitados("chkespirometria", item.espirometria);
    exibirAvaliadosSolicitados("chkexamedeescarro", item.exame_de_escarro);
    exibirAvaliadosSolicitados("chkglicemia", item.glicemia);
    exibirAvaliadosSolicitados("chkhdl", item.hdl);
    exibirAvaliadosSolicitados("chkhemoglobinaGlicada", item.hemoglobina_glicada);
    exibirAvaliadosSolicitados("chkhemograma", item.hemograma);
    exibirAvaliadosSolicitados("chkldl", item.ldl);
    exibirAvaliadosSolicitados("chkretinografiaFundoDeOlhoComOftalmologista", item.retinografia_fundo_de_olho_com_oftalmologista);
    exibirAvaliadosSolicitados("chksorologiaDeSiflisVdrl", item.sorologia_de_siflis_vdrl);
    exibirAvaliadosSolicitados("chksorologiaParaHiv", item.sorologia_para_hiv);
    exibirAvaliadosSolicitados("chksorologiaParaDengue", item.sorologia_para_dengue);
    exibirAvaliadosSolicitados("chktesteIndiretoDeAntiglobinaHumanaTia", item.teste_indireto_de_antiglobulina_humana_tia);
    exibirAvaliadosSolicitados("chktesteDaOrelhinha", item.teste_da_orelhinha);
    exibirAvaliadosSolicitados("chktesteDeGravidez", item.teste_de_gravidez);
    exibirAvaliadosSolicitados("chktesteDoOlhinho", item.teste_do_olhinho);
    exibirAvaliadosSolicitados("chktesteDoPezinho", item.teste_do_pezinho);
    exibirAvaliadosSolicitados("chkultrassonografiaObstetrica", item.ultrassonografia_obstetrica);
    exibirAvaliadosSolicitados("chkurocultura", item.urocultura);
    jQuery("#selPic").val(item.pic);
    jQuery("[name='rdbObs'][value='" + item.ficou_em_observacao + "']").prop("checked", true);
    if(item.avaliacao_diagnostico){$("#chkavaliacaoDiagnostico").prop("checked", true) };
    if(item.procedimentos_clinicos_terapeuticos){$("#chkrdbprocedimentosClinicosTerapeuticos").prop("checked", true) };
    if(item.prescricao_terapeutica){$("#chkrdbprescricaoTerapeutica").prop("checked", true) };
    if(item.retorno_para_consulta_agendada){$("#chkretornoParaConsultaAgendada").prop("checked", true) };
    if(item.retorno_para_cuidado_continuado_programado){$("#chkretornoParaCuidadoContinuadoProgramado").prop("checked", true) };
    if(item.agendamento_para_grupos){$("#chkagendamentoParaGrupos").prop("checked", true) };
    if(item.agendamento_para_nasf){$("#chkagendamentoParaNasf").prop("checked", true) };
    if(item.alta_do_episodio){$("#chkaltaDoEpisodio").prop("checked", true) };
    if(item.encaminhamento_interno_no_dia){$("#chkencaminhamentoInternoNoDia").prop("checked", true) };
    if(item.encaminhamento_para_servico_especializado){$("#chkencaminhamentoParaServicoEspecializado").prop("checked", true) };
    if(item.encaminhamento_para_caps){$("#chkencaminhamentoParaCaps").prop("checked", true) };
    if(item.encaminhamento_para_internacao_hospitalar){$("#chkencaminhamentoParaInternacaoHospitalar").prop("checked", true) };
    if(item.encaminhamento_para_urgencia){$("#chkencaminhamentoParaUrgencia").prop("checked", true) };
    if(item.encaminhamento_para_servico_de_atencao_domiciliar){$("#chkencaminhamentoParaServicoDeAtencaoDomiciliar").prop("checked", true) };
    if(item.encaminhamento_intersetorial){$("#chkencaminhamentoIntersetorial").prop("checked", true) };
    jQuery("#idAtendimento").val(item.id_atendimento_individual);
    $('.IdentificacaoUsuario').data("numero", item.numero);
}

function getProfissionais() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getProfissionalByIdAtendimento', idAtendimentoIndividual: jQuery("#idAtendimento").val() },
        datatype: "json",
        url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            var linhas = obj.resultado.length;
            $.each(obj.resultado, function (ResultadoItens, item) {
                if (linhas == 1 && ResultadoItens == 0)
                    makeTableProfissional(item.Nome, item.CNS, item.IdProf, item.IdProfAtend, item.CBO, true);
                else 
                    makeTableProfissional(item.Nome, item.CNS, item.IdProf, item.IdProfAtend, item.CBO, false);
            });
        }
    });
}

function getPacientes() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getPacienteByIdAtendimento', idAtendimentoIndividual: jQuery("#idAtendimento").val() },
        datatype: "json",
        url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
    })
    .done(function (data) {
         var obj = JSON.parse(data)
         if (obj.status) {
             $('#listaAtendimentoIndividual tbody').empty();
             $.each(obj.resultado, function (ResultadoItens, item) {
                 makeTablePaciente(item.Numero, item.Nome, item.CNS, item.IdUsr, item.IdUsrAtend);
             });
        }
    });
}

function getExamesPaciente() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getExamesPacienteByIdAtendimento', idAtendimentoIndividual: jQuery("#idAtendimento").val(), idAtendimentoUsuario: jQuery(".IdentificacaoUsuario").data("id") },
        datatype: "json",
        url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            $('#listaExames tbody').empty();
            $.each(obj.resultado, function (ResultadoItens, item) {
                makeTableExamesPaciente(item.idAtendimentoIndividual,item.idAtendimentoUsuario,  item.Descricao, item.solicitado, item.avaliado,item.id, item.Descricao);
            });
        }
    });
}

function buscaProcedimento() {
    jQuery(".msgErroDuplicado").text('');
    jQuery(".modal-title").html("Busca de Exame");

    jQuery("#tipoBusca").val('1');
    jQuery("#modalBusca").val('');
    jQuery("label[for=modalBusca]").html('Código *');
    jQuery("#modalBusca").attr('placeholder', 'Código');

    jQuery('#modalSigtap').find('input[type=text], select').each(function () {
        $(this).parents(".form-group").removeClass("has-error");
    });

    jQuery('#listaModalProcedimento tbody').empty();
    jQuery('#listaModalProcedimento tbody').append('<tr><th colspan="2" class="text-center">Efetue a busca acima</th></tr>');
    jQuery('#modalSigtap').modal('show');
}

function tpBuscaProcedimento() {
    if ($("#tipoBusca").val() == 1) {
        $("#modalBusca").val('');
        $("label[for=modalBusca]").html('Código *');
        $("#modalBusca").attr('placeholder', 'Código');
    } else {
        $("#modalBusca").val('');
        $("label[for=modalBusca]").html('Nome *');
        $("#modalBusca").attr('placeholder', 'Nome');
    }
}

function getProcedimento() {
    if (validaCampos($("#modalSigtap"), 2, "alertaSemModalProcedimento")) {
        var filtro = jQuery("#modalBusca").val();

        jQuery('#listaModalProcedimento tbody').empty();
        jQuery('#listaModalProcedimento tbody').append('<tr><th colspan="2" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');

        jQuery.ajax({
            type: "POST",
            async: false,
            data: {
                acao: 'getSigtap',
                codigo: filtro,
                nome: filtro
            },
            url: "ajax/procedimento/ajax-procedimento.asp"
        })
        .done(function (data) {
            var obj = JSON.parse(data);
            jQuery('#listaModalProcedimento tbody').empty();
            if (obj.length > 0) {
                jQuery.each(obj, function (ResultadoItens, item) {
                    var strTable = '<tr id="' + item.Codigo.toUpperCase() + '" class="sel-procedimento" data-codigo="' + item.Codigo.toUpperCase() + '" data-codproc="' + item.CodProc + '" data-nome="' + item.Nome.toUpperCase() + '" >';
                    strTable += '<td class="text-center">' + item.Codigo.toUpperCase() + '</td>';
                    strTable += '<td class="text-capitalize">' + item.Nome.toUpperCase() + '</td>';
                    strTable += '</tr>';
                    jQuery('#listaModalProcedimento tbody').append(strTable);
                });
                executaSelect();
            } else {
                $('#listaModalProcedimento tbody').append('<tr><th colspan="2" class="text-center">Nenhum resultado encontrado!</th></tr>');
            }
        });
    }
    function executaSelect() {
        jQuery("#listaModalProcedimento tbody .sel-procedimento").click(function () {
            //Remove o active dos outros
            jQuery(this).siblings().removeClass("list-group-item-success");
            var codigo = jQuery(this).data("codproc");
            var nome = jQuery(this).data("nome");
            if (!jQuery("#" + codigo).hasClass("sel-procedimento")) {
                $('#txtExame').val(nome);
                $('#modalSigtap').modal('hide');
            } else {
                $('.msgErroDuplicado').text('O Exame selecionado já foi inserido e não poderá ser inserido novamente.');
            }
        });
    }
    return false;
}

function getDadosAtendimento() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getById', id: jQuery("#idAtendimento").val() },
        datatype: "json",
        url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            if (obj.resultado.codigo_cnes_unidade> 0) {
                $('#selCnesUni').val(obj.resultado.codigo_cnes_unidade).prop('disabled', 'disabled');
            }
            makeEquipeIne(obj.resultado.codigo_cnes_unidade, obj.resultado.codigo_equipe_ine);
            var dataAtend = obj.resultado.data_atendimento.split("-");
            $('#txtData').val(dataAtend[2] + "/" + dataAtend[1] + "/" + dataAtend[0]);
        }
    });
}

function removerValidacoesParticipantes() {
    jQuery("#Usuario").find(".msgErro").empty();
    jQuery("#Usuario").find("input[type='text']").removeClass("error");
    jQuery("#Usuario").find("select").removeClass("error");
    jQuery("#Avaliacao").find(".msgErro").empty();
}

function CarregarEquipeIne() {

    var CodCnes = $('#selCnesUni :selected').val();
    if (parseInt(CodCnes) > 0)
    {
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
                    options += '<option value="' + item.CodINE + '">' + item.Numero +' - ' + item.Descricao + '</option>';
                });                                
                $("#selIneEquipe").html(options);
                $("label[for='txtIneEquipe']").html('Cód. equipe (INE) <span class="asterisco"> *</span>').removeClass('removeasterisco');
            }
            else
            {                
                $('#selIneEquipe').html('<option value="0">NÃO HÁ EQUIPES</option>');                
                $("label[for='txtIneEquipe'] .asterisco").remove();
                $("label[for='txtIneEquipe']").addClass('removeasterisco');
            }
        });
    }
}

function makeEquipeIne(CodCnes,CodINE) {

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
                if (CodINE <= 23 && CodINE > 0){ $("#selIneEquipe").val(CodINE)};                
                    
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
    $('#listaProfissionaisAtendimento tbody .btn-success').on("click", busca_Prof_Sus);
}