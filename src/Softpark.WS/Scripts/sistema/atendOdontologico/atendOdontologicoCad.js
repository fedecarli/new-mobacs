var Api = { URL: "ajax/atendOdontologico/ajax-atendimento-odontologico.asp", getPaciente : "ajax/atendIndividual/getPaciente.asp" };
var idFicha = "#idAtendimentoOdontologico";

$(document).ready(function () {
    //Modal
    jQuery('#modalProfSus').on('shown.bs.modal', function () {
        $('#txtModalBusca').focus();
    });

    //VALIDAÇÕES DO DICIONARIO DE DADOS DO CDS
    $("#txtCartaoSUS").on('blur', function () {
        validacaoCampo('#txtCartaoSUS', 'CNS');
    });
    $("#txtDataNascimento").on('blur', function () {
        validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade');
    });

    function validaTiposConsulta() {
        var retorno = true;
        
        //validação 1 - É Requerido se o tipo de atendimento for consulta agendada.
        //validação 2 - Não deve ser preenchido se o tipo de atendimento for "Escuta Inicial / Orientação".
        //validação 3 - Se o tipo de atendimento for "Atendimento de urgência", a opção de "Consulta de retorno" não pode ser marcada.
        //validação 4 - Se for "Consulta no dia", o campo não é Requerido.
        //validação 5 - Aceita apenas um registro. 
        //AS 5 VALIDAÇÕES ESTÃO SENDO REGRADAS DURANTE O PREENCHIMENTO DA FICHA PELA FUNÇÃO: "validaTipoConsulta"
        validaTipoConsulta();
        return retorno;
    }
    //Click
    jQuery('#btnConfObs').on('click', executaSelectProcedimentos);
    jQuery('#btnCancelar').on('click', limparFormAtendimentoUsuario);
    jQuery('#btnConfirmar').on('click',function (evt) {
        var verificacao = verificarItensnaTable();
        if (verificacao) { verificacao = validacaoCampo('#txtCartaoSUS', 'CNS'); };
        if (verificacao) { verificacao = validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade'); };
        validaTiposConsulta();
        if (!verificacao) {
            evt.preventDefault();
        }
    });
    jQuery('#btnFinalizarAtend').on('click', finalizar);
    jQuery('#btnBuscarProf').on('click', busca_Prof_Sus);
    jQuery('#btnBuscaProcedimento').on('click', getProcedimento);
    jQuery('#btnNovoCartaoSus').on('click', addSemCadastro);

    jQuery('#selCnesUni').on('change', CarregarEquipeIne);
    //Get dos Dados - Carregamento da Tela
    getDadosAtendimento();
    getProfissionais();
    getPacientes();

    //Validate
    jQuery("#fichaAtendimentoOdontologico").validate({
        errorPlacement: function (error, element) {
            if (jQuery(element).closest("div.ValidaNumero").find("div.msgErro").length > 0) {
                error.appendTo(jQuery(element).closest("div.ValidaNumero").find("div.msgErro"));
                
            } else if (jQuery(element).parent("div").find("div.msgErro").length > 0) {
                error.appendTo(jQuery(element).parent("div").find("div.msgErro"));
            }
            else {
                error.appendTo(jQuery(element).parents("div.form-group").find("div.msgErro"));
            }
        },
        rules: {
            selCnesUni: "CodCnesUnidadeSelecionado",
            selLocalAtend: {
                required: true
            },
            selIneEquipe: {
                required: true
            },
            txtDataNascimento: {
                required: true
            },
            rdbtipoAtend: {
                required: true
            },
            rdbSexo: {
                required: true
            },
            //rdbtipoConsultazaaaaaaa: {
            //    required: true
            //},
            txtData: {
                required: true
            },
            txtOutrosSiaQtde: {
                min: 0,
                max:999
            },
            txtProntuario: {
                min: 0
            },
            txtCartaoSUS: {
                minlength: 15,
                maxlength: 15
            },
            acessoPolpaDentaria: {
                min: 0,
                max: 999
            },
            adaptacaoProteseDentaria: {
                min: 0,
                max: 999
            },
            aplicacaoCariostatico: {
                min: 0,
                max: 999
            },
            aplicacaoSelante: {
                min: 0,
                max: 999
            },
            aplicacaoTopicaFluor: {
                min: 0,
                max: 999
            },
            capeamentoPulpar: {
                min: 0,
                max: 999
            },
            cimentacaoProtese: {
                min: 0,
                max: 999
            },
            curativoDemora: {
                min: 0,
                max: 999
            },
            drenagemAbscesso: {
                min: 0,
                max: 999
            },
            evidenciacaodePlano: {
                min: 0,
                max: 999
            },
            exodontiaDeciduo: {
                min: 0,
                max: 999
            },
            exodontiaPermanente: {
                min: 0,
                max: 999
            },
            instalacaoProtese: {
                min: 0,
                max: 999
            },
            moldagemGengival: {
                min: 0,
                max: 999
            },
            orientacaoHigiene: {
                min: 0,
                max: 999
            },
            profilaxia: {
                min: 0,
                max: 999
            },
            pulpotomiaDentaria: {
                min: 0,
                max: 999
            },
            radiografiaPeriapcal: {
                min: 0,
                max: 999
            },
            raspagemAlisamentoPolimento: {
                min: 0,
                max: 999
            },
            raspagemAlisamentoSubgengivais: {
                min: 0,
                max: 999
            },
            restauracaoDeciduo: {
                min: 0,
                max: 999
            },
            restauracaoPermanenteAnterior: {
                min: 0,
                max: 999
            },
            restauracaoPermanentePosterior: {
                min: 0,
                max: 999
            },
            retiradaPontos: {
                min: 0,
                max: 999
            },
            selamentoProvisorio: {
                min: 0,
                max: 999
            },
            tratamentoAlveolite: {
                min: 0,
                max: 999
            },
            ulotomiaUlectomia: {
                min: 0,
                max: 999
            },
            'chkVigilancia[]': {
                required: true,
                minlength: 1
            },
            'chkConduta[]': {
                required: true,
                minlength: 1
            }
        },
        messages: {
            selCnesUni: "Campo Código CNES Unidade é obrigatório",
            selLocalAtend: "Campo Local de Atendimento é obrigatório",
            selIneEquipe: "Campo INE Obrigatório",
            txtDataNascimento: "Data de Nascimento é obrigatória",
            rdbtipoAtend: "Tipo de Atendimento é obrigatório",
            rdbSexo: "Campo obrigatório",
            //rdbtipoConsulta: "Campo Tipo de Consulta é obrigatório",
            txtData: "Data é obrigatória",
            txtProntuario: "Por favor, indique um número maior que 0",
            txtCartaoSUS: "Nº Cartão SUS incorreto.",
            txtOutrosSiaQtde: "Por favor, indique um número maior que 0 e menor que 999.",
            acessoPolpaDentaria: "Por favor, indique um número maior que 0 e menor que 999.",
            adaptacaoProteseDentaria: "Por favor, indique um número maior que 0 e menor que 999.",
            aplicacaoCariostatico: "Por favor, indique um número maior que 0 e menor que 999.",
            aplicacaoSelante: "Por favor, indique um número maior que 0 e menor que 999.",
            aplicacaoTopicaFluor: "Por favor, indique um número maior que 0 e menor que 999.",
            capeamentoPulpar: "Por favor, indique um número maior que 0 e menor que 999.",
            cimentacaoProtese: "Por favor, indique um número maior que 0 e menor que 999.",
            curativoDemora: "Por favor, indique um número maior que 0 e menor que 999.",
            drenagemAbscesso: "Por favor, indique um número maior que 0 e menor que 999.",
            evidenciacaodePlano: "Por favor, indique um número maior que 0 e menor que 999.",
            exodontiaDeciduo: "Por favor, indique um número maior que 0 e menor que 999.",
            exodontiaPermanente: "Por favor, indique um número maior que 0 e menor que 999.",
            instalacaoProtese: "Por favor, indique um número maior que 0 e menor que 999.",
            moldagemGengival: "Por favor, indique um número maior que 0 e menor que 999.",
            orientacaoHigiene: "Por favor, indique um número maior que 0 e menor que 999.",
            profilaxia: "Por favor, indique um número maior que 0 e menor que 999.",
            pulpotomiaDentaria: "Por favor, indique um número maior que 0 e menor que 999.",
            radiografiaPeriapcal: "Por favor, indique um número maior que 0 e menor que 999.",
            raspagemAlisamentoPolimento: "Por favor, indique um número maior que 0 e menor que 999.",
            raspagemAlisamentoSubgengivais: "Por favor, indique um número maior que 0 e menor que 999.",
            restauracaoDeciduo: "Por favor, indique um número maior que 0 e menor que 999.",
            restauracaoPermanenteAnterior: "Por favor, indique um número maior que 0 e menor que 999.",
            restauracaoPermanentePosterior: "Por favor, indique um número maior que 0 e menor que 999.",
            retiradaPontos: "Por favor, indique um número maior que 0 e menor que 999.",
            selamentoProvisorio: "Por favor, indique um número maior que 0 e menor que 999.",
            tratamentoAlveolite: "Por favor, indique um número maior que 0 e menor que 999.",
            ulotomiaUlectomia: "Por favor, indique um número maior que 0 e menor que 999.",
            'chkVigilancia[]': "Selecione no mínimo, uma opção",
            'chkConduta[]': "Selecione no mínimo, uma opção",
        },
        submitHandler: function (form) {
            // form.submit();
            if (verificarItensnaTable())
            {
                //if (validarTipoConsulta) {
                //    var validarTipoConsultaOk = false;
                //    $('input[name=rdbtipoConsulta]').each(function () {
                //        if ($(this).attr('checked') == 'checked') {
                //            validarTipoConsultaOk = true;
                //        } 
                //    })
                //    if (validarTipoConsultaOk == false) {
                //        alert('Campo Tipo de Consulta é obrigatório');
                //        return false;
                //    }
                //}
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

function cancelar() {
    if ($('#listaAtendimentoOdontologico tbody tr').length == 0) {
        window.location.assign("atendimentoOdontologico.asp");
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
    jQuery("#listaAtendimentoOdontologico tbody tr ").removeClass("active");
    jQuery("#btnConfirmar").hide();
    jQuery("#btnCancelar").hide();
    $('.msgErroDuplicado').text('');
    $(".modal-title").html("Busca de Paciente");
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
        $('#listamodalProfSus tbody .sel-cns').on('click',function (event) {
            $(this).siblings().removeClass('list-group-item-success');
            var idAtendimento = $("#idAtendimentoOdontologico").val();
            if (!jQuery("#" + $(this).data('idusr')).hasClass("sel-cns")) {
                var idPacienteAtendimento = criaPacienteAtendimento(idAtendimento, $(this).data("idusr"), $(this).data('cns'), $(this).data('nome'));
                if (idPacienteAtendimento != false) {
                    getPacientes();
                }
            }else
            {
                $('.msgErroDuplicado').text('O Paciente selecionado já foi inserido no Atendimento e não poderá ser inserido novamente.');
            }
        });
    }
    return false;
}

function chaveamentoModal(evt) {
    if ($("#myModalLabel").text() == "Busca de Profissional") {
        return prof_Sus();
    } else {
        return paciente_Sus();
    }
}

function executaSelectProcedimentos() {
    var qtde = $('#txtOutrosSiaQtde').val();
    var textcombo = $('#txtExame').val();
    var sigtap = $('#txtSigtap').val();
    var idAtendimento = jQuery("#idAtendimentoOdontologico").val();
    var idAtendimentoUsuario = jQuery(".IdentificacaoUsuario").data("id");
    if (textcombo != "" && qtde > 0)
    {
        if ($("#listaProcedimentos tr[data-exame='" + textcombo + "']").length == 0) {
            var idProcedimento = salvarProcedimentosPacienteAtendimento(idAtendimento, idAtendimentoUsuario, textcombo, sigtap, qtde);
            if (idProcedimento != false) {
                $('#txtExame').val('Selecione');
                $('#txtOutrosSiaQtde').val('');
                makeTableProcedimentosPaciente(idAtendimento, idAtendimentoUsuario, textcombo, qtde, idProcedimento);
            } else {
                modalAlerta("Atenção", "Falha ao adicionar o Procedimento do Paciente no Atendimento, tente novamente mais tarde.");
            }
        }
        else {
            $('#txtExame').val('Selecione');
            $('#txtOutrosSiaQtde').val('');
            modalAlerta("Atenção", "Procedimento do Paciente já foi selecionado neste Atendimento.");
        }

    }else
    {
        modalAlerta("Atenção", "Informe no mínimo uma Quantidade e um Procedimento para confirmar.");
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
            } else {
                //Remove o active dos outros
                $(this).siblings().removeClass("list-group-item-success");
                var linhas = $("#listaProfissionaisAtendimento tbody tr.sel-cns").length;
                var idAtendimento = $("#idAtendimentoOdontologico").val();
                var add = $('#modalProfSus').hasClass('add');
                if (add) {
                    if (!jQuery("#" + $(this).data("idprof")).hasClass("sel-cns")) {
                        var idProfissionalAtendimento = salvaProfissionalAtendimento(idAtendimento, $(this).data("idprof"), $(this).data("cbo"));
                        if (idProfissionalAtendimento != false) {
                            $("#listaProfissionaisAtendimento tbody tr button.btn-danger").show();
                            $("#listaProfissionaisAtendimento tbody tr button.btn-success").hide();
                            var unico = (linhas == 0);
                            makeTableProfissional($(this).data("nome"), $(this).data("cns"), $(this).data("idprof"), idProfissionalAtendimento, $(this).data("cbo"), unico);
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
            idAtendimentoOdontologico: idAtendimento,
            idProfissionalSaude: idProfissional,
            cbo: cbo
        },
        datatype: "json",
        url: "ajax/atendOdontologico/ajax-atendimento-odontologico.asp"
    })
    .done(function (data) {
        if (data > 0) {
            blnRet = parseInt(data);
        }
    });
    return blnRet;
}

function salvarProcedimentosPacienteAtendimento(idAtendimento, idPacienteAtendimento, Descricao, sigtap, qtde) {
    var blnRet = false;
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'createProcedimentosPaciente', idAtendimentoOdontologico: idAtendimento, idAtendimentoUsuario: idPacienteAtendimento, Descricao: Descricao, sigtap: sigtap, Qtde: qtde },
        datatype: "json",
        url: "ajax/atendOdontologico/ajax-atendimento-odontologico.asp"
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
        url: "ajax/atendOdontologico/ajax-atendimento-odontologico.asp"
    })
    .success(function (data) {
        blnRet = true;
    });
    return blnRet;
}

function removerProfissionalAtendimento(idProfissionalAtendimento, tr) {
    postAjaxObject({ acao: 'removeProfissional', id: idProfissionalAtendimento })
    .done(onSuccess)
    .fail(onError);

    function onSuccess() {
        jQuery(tr).remove();
    }

    function onError() {
        modalAlerta("Atenção", "Falha ao remover o Profissional, tente novamente mais tarde.");
    }
}

function criaPacienteAtendimento(idAtendimentoOdontologico, idIdentificacaoUsuario, numeroSus, nome) {
    var blnRet = false;
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createUsuarioAtendido',
            idAtendimentoOdontologico: idAtendimentoOdontologico,
            idIdentificacaoUsuario: idIdentificacaoUsuario,
            numeroCartaoSus: numeroSus
        },
        datatype: "json",
        url: "ajax/atendOdontologico/ajax-atendimento-odontologico.asp"
    })
    .done(function (data) {
        if (data > 0)
            blnRet = parseInt(data);
    });
    return blnRet;
}

function concatenarCheckBox(nameElemento) {
    var exame = "";
    var total = jQuery("[name='" + nameElemento + "[]']:checked").length - 1;
    jQuery("[name='" + nameElemento + "[]']:checked").each(function (i) {
        exame += jQuery(this).val();
        if (i < total)
            exame += ",";
    });
    return exame;
}

function exibirCheckBox(nameElemento, value) {
    if (value != "" && value != null) {
        var string = value.split(",");
        if (string.length > 0) {
            jQuery("[name='" + nameElemento + "[]']").each(function (i) {
                jQuery("[name='" + nameElemento + "[]'][value ='"+ string[i] +"']").prop("checked", true);
            });
        }
    }
}

function dadosUsuarioAtendido() {
    var dados = {
        Id: parseInt(jQuery(".IdentificacaoUsuario").data("id")),
        Turno: jQuery("[name='rdbTurno']:checked").val() == undefined ? null : jQuery("[name='rdbTurno']:checked").val(),
        NumeroProntuario: jQuery("#txtProntuario").val() == "" ? null : jQuery("#txtProntuario").val(),
        NumeroCartaoSUS: jQuery("#txtCartaoSUS").val() == "" ? null : jQuery("#txtCartaoSUS").val(),
        Nome: jQuery("#nome").val(),
        DataNascimento: jQuery("#txtDataNascimento").val(),
        Sexo: jQuery("[name='rdbSexo']:checked").val(),
        LocalAtendimento: jQuery("#selLocalAtend :selected").val() == undefined ? null : jQuery("#selLocalAtend :selected").val(),
        IdTipoAtendimento: jQuery("[name='rdbtipoAtend']:checked").val() == undefined ? null : jQuery("[name='rdbtipoAtend']:checked").val(),
        TipoConsulta: jQuery("[name='rdbtipoConsulta']:checked").val() == undefined ? null : jQuery("[name='rdbtipoConsulta']:checked").val(),
        Procedimentos: dadosProcedimentosUsuarioAtendido(),
        Conduta: concatenarCheckBox("chkConduta"),
        VigilanciaSaudeBucal: concatenarCheckBox("chkVigilancia"),
        EscovaDental: jQuery("#chkFornecimentoEscova").is(":checked") ? 1 : null,
        FioDental: jQuery("#chkFornecimentoCreme").is(":checked") ? 1 : null,
        CremeDental: jQuery("#chkFornecimentoFio").is(":checked") ? 1 : null,
        Gestante: jQuery("#chkgestante").is(":checked") ? 1 : null,
        PacienteNecessidadesEspeciais: jQuery("#chkpacientecomnecessidadesespeciais").is(":checked") ? 1 : null,
        IdAtendimentoOdontologicoUsuarioAtendido: jQuery("#idAtendimentoOdontologico").val(),
        //Tratar depois com data
        Numero: 0
    };
    return JSON.stringify(dados);
}

//Exibir dados serializados salvos no campo procedimentos
function makeDadosProcedimentos(procedimentos) {
        jQuery("[name='acessoPolpaDentaria']").val(procedimentos.acessoPolpaDentaria);
        jQuery("[name='adaptacaoProteseDentaria']").val(procedimentos.adaptacaoProteseDentaria);
        jQuery("[name='aplicacaoCariostatico']").val(procedimentos.aplicacaoCariostatico);
        jQuery("[name='aplicacaoSelante']").val(procedimentos.aplicacaoSelante);
        jQuery("[name='aplicacaoTopicaFluor']").val(procedimentos.aplicacaoTopicaFluor);
        jQuery("[name='capeamentoPulpar']").val(procedimentos.capeamentoPulpar);
        jQuery("[name='cimentacaoProtese']").val(procedimentos.cimentacaoProtese);
        jQuery("[name='curativoDemora']").val(procedimentos.curativoDemora);
        jQuery("[name='drenagemAbscesso']").val(procedimentos.drenagemAbscesso);
        jQuery("[name='evidenciacaodePlano']").val(procedimentos.evidenciacaodePlano);
        jQuery("[name='exodontiaDeciduo']").val(procedimentos.exodontiaDeciduo);
        jQuery("[name='exodontiaPermanente']").val(procedimentos.exodontiaPermanente);
        jQuery("[name='instalacaoProtese']").val(procedimentos.instalacaoProtese);
        jQuery("[name='moldagemGengival']").val(procedimentos.moldagemGengival);
        jQuery("[name='orientacaoHigiene']").val(procedimentos.orientacaoHigiene);
        jQuery("[name='profilaxia']").val(procedimentos.profilaxia);
        jQuery("[name='pulpotomiaDentaria']").val(procedimentos.pulpotomiaDentaria);
        jQuery("[name='radiografiaPeriapcal']").val(procedimentos.radiografiaPeriapcal);
        jQuery("[name='raspagemAlisamentoPolimento']").val(procedimentos.raspagemAlisamentoPolimento);
        jQuery("[name='raspagemAlisamentoSubgengivais']").val(procedimentos.raspagemAlisamentoSubgengivais);
        jQuery("[name='restauracaoDeciduo']").val(procedimentos.restauracaoDeciduo);
        jQuery("[name='restauracaoPermanenteAnterior']").val(procedimentos.restauracaoPermanenteAnterior);
        jQuery("[name='restauracaoPermanentePosterior']").val(procedimentos.restauracaoPermanentePosterior);
        jQuery("[name='retiradaPontos']").val(procedimentos.retiradaPontos);
        jQuery("[name='selamentoProvisorio']").val(procedimentos.selamentoProvisorio);
        jQuery("[name='tratamentoAlveolite']").val(procedimentos.tratamentoAlveolite);
        jQuery("[name='ulotomiaUlectomia']").val(procedimentos.ulotomiaUlectomia);
}

//Montar objeto para insercao no banco
function dadosProcedimentosUsuarioAtendido() {
    var dados = {
        acessoPolpaDentaria:jQuery("[name='acessoPolpaDentaria']").val(),
        adaptacaoProteseDentaria: jQuery("[name='adaptacaoProteseDentaria']").val(),
        aplicacaoCariostatico: jQuery("[name='aplicacaoCariostatico']").val(),
        aplicacaoSelante: jQuery("[name='aplicacaoSelante']").val(),
        aplicacaoTopicaFluor: jQuery("[name='aplicacaoTopicaFluor']").val(),
        capeamentoPulpar: jQuery("[name='capeamentoPulpar']").val(),
        cimentacaoProtese: jQuery("[name='cimentacaoProtese']").val(),
        curativoDemora: jQuery("[name='curativoDemora']").val(),
        drenagemAbscesso: jQuery("[name='drenagemAbscesso']").val(),
        evidenciacaodePlano: jQuery("[name='evidenciacaodePlano']").val(),
        exodontiaDeciduo: jQuery("[name='exodontiaDeciduo']").val(),
        exodontiaPermanente: jQuery("[name='exodontiaPermanente']").val(),
        instalacaoProtese: jQuery("[name='instalacaoProtese']").val(),
        moldagemGengival: jQuery("[name='moldagemGengival']").val(),
        orientacaoHigiene: jQuery("[name='orientacaoHigiene']").val(),
        profilaxia: jQuery("[name='profilaxia']").val(),
        pulpotomiaDentaria: jQuery("[name='pulpotomiaDentaria']").val(),
        radiografiaPeriapcal: jQuery("[name='radiografiaPeriapcal']").val(),
        raspagemAlisamentoPolimento: jQuery("[name='raspagemAlisamentoPolimento']").val(),
        raspagemAlisamentoSubgengivais: jQuery("[name='raspagemAlisamentoSubgengivais']").val(),
        restauracaoDeciduo: jQuery("[name='restauracaoDeciduo']").val(),
        restauracaoPermanenteAnterior: jQuery("[name='restauracaoPermanenteAnterior']").val(),
        restauracaoPermanentePosterior: jQuery("[name='restauracaoPermanentePosterior']").val(),
        retiradaPontos: jQuery("[name='retiradaPontos']").val(),
        selamentoProvisorio: jQuery("[name='selamentoProvisorio']").val(),
        tratamentoAlveolite: jQuery("[name='tratamentoAlveolite']").val(),
        ulotomiaUlectomia: jQuery("[name='ulotomiaUlectomia']").val(),
    };
    return JSON.stringify(dados);
}

function dadosAtendimentoOdontologico() {
    var dados = {
        codigoCnesUnidade: $('#selCnesUni :selected').val(),
        codEquipe: ($('#selIneEquipe').val() == "" || $('#selIneEquipe').val() == "0") ? null : $('#selIneEquipe').val(),
        conferidoPor: "",
        dataAtendimento: $('#txtData').val(),
        numeroFolha: null,
        Id: jQuery("#idAtendimentoOdontologico").val()
    };
    return JSON.stringify(dados);
}

function alteraPacienteAtendimento() {
    
    $.ajax({
        type: "POST",
        data: {
            acao: 'editUsuarioAtendido',
            dadosAtendimentoOdontologicoUsuarioAtendido: dadosUsuarioAtendido()
        },
        datatype: "json",
        url: "ajax/atendOdontologico/ajax-atendimento-odontologico.asp"
    }).done(
        function () {
            limparFormAtendimentoUsuario();
            $('.IdentificacaoUsuario').hide();
            $('#btnConfirmar').hide();
            $('#btnCancelar').hide();
            $('#btnFinalizarAtend').show();
            modalAlerta("Confirmação", "Atendimento salvo com sucesso!");
        }
    )

}

function alteraAtendimento(status) {
    
    $.ajax({
        type: "POST",
        data: {
            acao: 'Edit',
            dadosAtendimentoOdontologico: dadosAtendimentoOdontologico(),
            status: status
        },
        datatype: "json",
        url: "ajax/atendOdontologico/ajax-atendimento-odontologico.asp"
    }).done(function () {
        if (status == 2) {
            $.ajax({
                type: "POST",
                data: { acao: 'GetValidacao', id: jQuery("#idAtendimentoOdontologico").val() },
                datatype: "json",
                url: Api.URL
            })
            .done(function (data) {
                data = JSON.parse(data)
                if (data.status) {
                    modalAlerta("Confirmação", "Atendimento finalizado com sucesso!")
                    setInterval(function () { window.location = "atendimentoOdontologico.asp" }, 5000);
                }
                else {
                    modalAlerta("Erro", data.resultado);
                }
            });
        }
    });
}

function removerPacienteAtendimento(idPacienteAtendimento, idusr, tr) {
    return jQuery.ajax({
        type: "POST",
        data: { acao: 'removeUsuarioAtendido', id: idPacienteAtendimento },
        url: Api.URL
    })
    .done(onSuccess)
    .fail(onError);

    function onSuccess () {
        if (jQuery('#listaAtendimentoOdontologico tbody tr').length == 1 || jQuery("#" + idusr).hasClass("active")) {
            limparFormAtendimentoUsuario();
            jQuery('#btnFinalizarAtend').hide();
            jQuery('.IdentificacaoUsuario').hide();
            jQuery("#btnConfirmar").hide();
            jQuery("#btnCancelar").hide();
        }

        if (jQuery('#listaAtendimentoOdontologico tbody tr').length == 2) {
            jQuery("#listaAtendimentoOdontologico tbody tr").removeClass("active");
        }

        jQuery(tr).remove();
        desabilitaBuscaPaciente();
    }

    function onError() {
        modalAlerta("Atenção", "Falha ao remover o Paciente, tente novamente mais tarde.");
    }
}

function limparFormAtendimentoUsuario() {
    $('.IdentificacaoUsuario').each(function (i) {
        $(this).find('input[type=checkbox]:checked').removeAttr('checked', false);
        $(this).find('input[type=text]').val('');
        $(this).find('input[type=number]').val('');
        $(this).find('input[type=radio]:checked').removeAttr('checked', false);
        $('#listaProcedimentos tbody').empty();
    });
    removerValidacoesParticipantes();
}

function makeTablePaciente(Numero, nome, cns, idusr, idusratend) {
    
    var strTable = '';
    strTable += '   <tr id="' + idusr + '" class="sel-cns" data-nome="' + nome.toCapitalize() + '" data-cns="' + cns + '" data-idusr="' + idusr + '" data-idusratend="' + idusratend + '" >';
    strTable += '       <td class="text-center">' + Numero + '</td>';
    strTable += '       <td class="text-capitalize">' + nome.toCapitalize() + '</td>';
    strTable += '       <td class="text-center">' + cns + '</td>';
    strTable += '       <td class="text-center"><button type="button" class="btn btn-danger" title="Remover"><span class="glyphicon glyphicon-remove"></span></button></td>';
    strTable += "       <td class='text-center'><button type='button' class='btn btn-primary preencherDadosPaciente' title='Preencher Ficha'><span class='glyphicon glyphicon-edit'></span></button></td>";
    strTable += '   </tr>';
    $('#listaAtendimentoOdontologico tbody').append(strTable);
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

function makeTableProcedimentosPaciente(idAtendimento, idAtendimentoUsuario, textcombo, qtde, idExame) {
    var strTable = '';
    strTable += '   <tr data-exame="' + textcombo + '" class="sel-exame"  data-idexame="' + idExame + '" data-qtde="' + qtde + '">';
    strTable += '       <td class="text-center">' + qtde + '</td>';
    strTable += '       <td class="text-capitalize">' + textcombo + '</td>';    
    strTable += '       <td class="text-center"><button type="button" class="btn btn-danger" title="Remover"><span class="glyphicon glyphicon-remove"></span></button></td>';
    strTable += '   </tr>';
    $('#listaProcedimentos tbody').append(strTable);
    removeGridProcedimentos();
    desabilitaBuscaProcedimentos();
}

function removeGridProcedimentos() {
    $('#listaProcedimentos tbody .btn-danger').click(function () {
        var tr = $(this).closest("tr");
        removerProcedimentosPacienteAtendimento($(tr).data("idexame"));
        $(tr).remove();
        desabilitaBuscaProcedimentos();
    });
}

function removerProcedimentosPacienteAtendimento(idExame) {
    postAjax({ acao: 'removeProcedimentosPaciente', id: idExame });
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
    jQuery('#listaAtendimentoOdontologico tbody .btn-danger').click(function () {
        var tr = jQuery(this).closest("tr");
        removerPacienteAtendimento(jQuery(tr).data("idusratend"), jQuery(tr).data("idusr"), tr);
    });
}

function validaCns(cns) {
	if (s.matches("[1-2]\\d{10}00[0-1]\\d") || s.matches("[7-9]\\d{14}")) {
    return somaPonderada(s) % 11 == 0;
}
return false;
}

function somaPonderada(cns) {
    var cs = [cns.toCharArray()];
    var soma = 0;
    for (i = 0; i < cs.length; i++) {
        soma += Character.digit(cs[i], 10) * (15 - i);
    }
    return soma;
}

function desabilitaBuscaPaciente() {
    if ($('#listaAtendimentoOdontologico tbody tr').length > 12) {
        $('#btnBuscarUsu').attr('disabled', 'disabled');
        $('#btnAddSemCadastro').attr('disabled', 'disabled');
    } else {
        $('#btnAddSemCadastro').removeAttr('disabled');
        $('#btnBuscarUsu').removeAttr('disabled');
    }
}

function verificarItensnaTable(event) {
    var retorno = true;
    $('#msgErroTableUsu').text('');
    $('#msgErroTableProf').text('');
    if ($('#listaAtendimentoOdontologico tbody tr').length == 0) {
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

function desabilitaBuscaProcedimentos() {
    if ($('#listaProcedimentos tbody tr').length > 23) {
        $('#btnConfObs').attr('disabled', 'disabled');
    } else {
        $('#btnConfObs').removeAttr('disabled');
    }
}

function habilitaFichaPaciente() {
    $("#listaAtendimentoOdontologico tbody .preencherDadosPaciente").on("click", function () {
        var data = $(this).closest("tr").data("idusratend");
        $("#listaAtendimentoOdontologico tbody tr ").removeClass("active");
        if ($("#listaAtendimentoOdontologico tbody tr ").length > 1) {
            jQuery(this).closest("tr").addClass("active");
        }
        if (data != $('.IdentificacaoUsuario').data("id")) {
            limparFormAtendimentoUsuario();
        }
        $('.IdentificacaoUsuario').show().data("id", data);
        getFichaPaciente(data);
        getProcedimentosPaciente();
        $('#btnConfirmar').show();
        $('#btnCancelar').show();
        validaTipoConsulta();
    });
}

function getFichaPaciente(idUsrAtend) {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getUsuarioAtendidoById', idAtendimentoOdontologico: idUsrAtend },
        datatype: "json",
        url: "ajax/atendOdontologico/ajax-atendimento-odontologico.asp"
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

    if (item.data_nascimento != null) {
        if (item.data_nascimento.indexOf('-') > 0) {
            var dataNasc = item.data_nascimento.split("-");
            jQuery("#txtDataNascimento").val(dataNasc[2] + "/" + dataNasc[1] + "/" + dataNasc[0]);
        }
    }

    if (item.turno != null) {
        jQuery("[name='rdbTurno'][value='" + item.turno + "']").prop("checked", true);
    }

    if (item.sexo != null) {
        jQuery("[name='rdbSexo'][value='" + item.sexo + "']").prop("checked", true);
    }

    jQuery("#selLocalAtend").val(item.local_atendimento);
    jQuery("[name='rdbtipoAtend'][value='" + item.id_tipo_atendimento + "']").prop("checked", true);
    jQuery("[name='rdbtipoConsulta'][value='" + item.tipo_consulta + "']").prop("checked", true);
    if(item.procedimentos.length>0){
        var objProcedimentos = JSON.parse(item.procedimentos)
        if (objProcedimentos != null) {
            makeDadosProcedimentos(objProcedimentos);
        }
    }
    exibirCheckBox("chkVigilancia", item.vigilancia_saude_bucal);
    exibirCheckBox("chkConduta", item.conduta);
    if(item.escova_dental>0) $("#chkFornecimentoEscova").prop("checked", true);
    if(item.creme_dental>0) $("#chkFornecimentoCreme").prop("checked", true);
    if(item.fio_dental>0) $("#chkFornecimentoFio").prop("checked", true);
    if(item.gestante>0) $("#chkgestante").prop("checked", true);
    if(item.paciente_necessidades_especiais>0) $("#chkpacientecomnecessidadesespeciais").prop("checked", true);
    jQuery("#idAtendimentoOdontologico").val(item.id_atendimento_odontologico);
    $('.IdentificacaoUsuario').data("numero", item.numero);
}

function getProfissionais() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getProfissionalByIdAtendimento', idAtendimentoOdontologico: jQuery("#idAtendimentoOdontologico").val() },
        datatype: "json",
        url: "ajax/atendOdontologico/ajax-atendimento-odontologico.asp"
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
        data: { acao: 'getPacienteByIdAtendimento', idAtendimentoOdontologico: jQuery("#idAtendimentoOdontologico").val() },
        datatype: "json",
        url: "ajax/atendOdontologico/ajax-atendimento-odontologico.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            $('#listaAtendimentoOdontologico tbody').empty();
            $.each(obj.resultado, function (ResultadoItens, item) {
                makeTablePaciente(item.Numero, item.Nome, item.CNS, item.IdUsr, item.IdUsrAtend);
            });

        }
    });
}

function getProcedimentosPaciente() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getProcedimentosPacienteByIdAtendimento', idAtendimentoOdontologico: jQuery("#idAtendimentoOdontologico").val(), idAtendimentoUsuario: jQuery(".IdentificacaoUsuario").data("id") },
        datatype: "json",
        url: "ajax/atendOdontologico/ajax-atendimento-odontologico.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            $('#listaProcedimentos tbody').empty();
            $.each(obj.resultado, function (ResultadoItens, item) {
                makeTableProcedimentosPaciente(item.idAtendimentoOdontologico, item.idAtendimentoUsuario, item.Descricao, item.qtde, item.id);
            });
        }
    });
}

function buscaProcedimento() {
    jQuery(".msgErroDuplicado").text('');
    jQuery(".modal-title").html("Busca de Procedimento");

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
            url: "ajax/atendOdontologico/ajax-atendimento-odontologico.asp"
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
            var codigo = jQuery(this).data("codigo");
            if (!jQuery("#" + codigo).hasClass("sel-procedimento")) {
                $('#txtExame').val(nome);
                $('#txtSigtap').val(codigo);
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
        data: { acao: 'getById', id: jQuery("#idAtendimentoOdontologico").val() },
        datatype: "json",
        url: "ajax/atendOdontologico/ajax-atendimento-odontologico.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            if (obj.resultado.codigo_cnes_unidade > 0) {
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
    jQuery("#Procedimentos").find(".msgErro").empty();
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
                $("label[for='txtIneEquipe']").html('Cód. Equipe  <span class="asterisco"> *</span>').removeClass('removeasterisco');
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
    $('#listaProfissionaisAtendimento tbody .btn-success').on("click", busca_Prof_Sus);
}
var validarTipoConsulta = false;

function validaTipoConsulta() {
    var TipoAtendimento = $('input[name=rdbtipoAtend]:checked');
    $('input[name=rdbtipoConsulta]:first').parents('fieldset').find('legend .asterisco').hide();
    validarTipoConsulta = false;
    $('input[name=rdbtipoConsulta]').each(function () {
        $(this).removeAttr('disabled');
    });
    switch (TipoAtendimento.val()) {
        case '2'://Consulta agendada
            $('input[name=rdbtipoConsulta]:first').parents('fieldset').find('legend .asterisco').show();
            validarTipoConsulta = true;
            break;
        case '4'://Escuta inicial / Orientação
            $('input[name=rdbtipoConsulta]').each(function(){
                $(this).removeAttr('checked').attr('disabled','disabled');
            });
            break;
        case '5'://Consulta no dia

            break;
        case '6'://Atendimento de urgência
            $('input[name=rdbtipoConsulta][value=2]').each(function () {
                $(this).removeAttr('checked').attr('disabled', 'disabled');
            });
            break;
        default:

            break;
    }
    if ($('input[name=rdbtipoConsulta]:checked').length==0) {
        $('input[name=rdbtipoConsulta]:enabled:first').prop('checked', 'checked');
    }
}