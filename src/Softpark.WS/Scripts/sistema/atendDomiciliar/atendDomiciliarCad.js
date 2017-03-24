var Api = { URL: "ajax/atendDomiciliar/ajax-atendimento-Domiciliar.asp", getPaciente: "ajax/atendIndividual/getPaciente.asp" };
var idFicha = "#idAtendimentoDomiciliar";

jQuery(document).ready(function () {
    //Modal
    jQuery('#modalProfSus').on('shown.bs.modal', function () {
        $('#txtModalBusca').focus();
    });

    //Click
    //jQuery('#btnConfObs').on('click', executaSelectProcedimentos);
    jQuery('#btnCancelar').on('click', limparFormAtendimentoUsuario);
    jQuery('#btnConfirmar').on('click', function () {
        var verificacao = verificarItensnaTable();
        if (verificacao) { verificacao = validacaoCampo('#txtCartaoSUS', 'CNS'); };
        if (verificacao) { verificacao = validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade'); };
        if (verificacao) {
            $('#fichaAtendimentoDomiciliar').submit();
        }
    });
    jQuery('#btnFinalizarAtend').on('click', finalizar);
    jQuery('#btnNovoCartaoSus').on('click', addSemCadastro);

    jQuery('#btnBuscaProcedimento').on('click', getProcedimento);
    
    $('#selCnesUni').on('change', CarregarEquipeIne);
    //Get dos Dados - Carregamento da Tela
    getDadosAtendimento();
    getProfissionais();
    getPacientes();
    jQuery("#selCid option").each(function () {
        var aux = jQuery(this).val();
        jQuery(this).val(aux.trim());
    })
    //VALIDAÇÕES DO DICIONARIO DE DADOS DO CDS
    $("#txtCartaoSUS").on('blur', function () {
        validacaoCampo('#txtCartaoSUS', 'CNS');
    });
    $("#txtDataNascimento").on('blur', function () {
        validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade');
    });
    //Validate
    jQuery("#fichaAtendimentoDomiciliar").validate({
        errorPlacement: function (error, element) {
            if (jQuery(element).parent("div").find("div.msgErro").length > 0) {
                error.appendTo(jQuery(element).parent("div").find("div.msgErro"));
            }
            else {
                error.appendTo(jQuery(element).parents("div.form-group").find("div.msgErro"));
            }
        },
        rules: {
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
            rdbTurno: {
                required: true
            },
            rdbSexo: {
                required: true
            },
            rdbModalidade: {
                required: true
            },
            txtData: {
                required: true
            }
        },
        messages: {
            selLocalAtend: "Campo Local de Atendimento é obrigatório",
            selIneEquipe: "Campo INE Obrigatório",
            txtDataNascimento: "Campo Data de Nascimento é obrigatória",
            rdbtipoAtend: "Campo Tipo de Atendimento é obrigatório",
            rdbTurno: "Campo Turno é obrigatório",
            rdbSexo: "Campo obrigatório",
            rdbModalidade: "Campo Modalidade AD é obrigatório",
            txtData: "Campo Data é obrigatória"
        },
        submitHandler: function (form) {
            // form.submit();
            if (verificarItensnaTable()) {
                alteraPacienteAtendimento();
            }
        }
    });
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
    if ($('#listaAtendimentoDomiciliar tbody tr').length == 0) {
        window.location.assign("atendimentoDomiciliar.asp");
    }
}

function pacienteNovamente() {

    $('#modalSemCartaoSus').modal('hide');
    busca_Paciente_Sus();

}

function busca_Paciente_Sus() {
    jQuery('#txtmodalBuscaCartaoSus').val('');
    $('#txtmodalConfirmaCartaoSus').val('');
    limparFormAtendimentoUsuario();
    jQuery('#btnFinalizarAtend').hide();
    jQuery('.IdentificacaoUsuario').hide();
    jQuery("#listaAtendimentoDomiciliar tbody tr ").removeClass("active");
    jQuery("#btnConfirmar").hide();
    jQuery("#btnCancelar").hide();
    $('.msgErroDuplicado').text('');
    jQuery(".modal-title").html("Busca de Paciente");
    jQuery("#selModalTpBusca").val('1');
    jQuery("#txtModalBusca").val('');
    jQuery("label[for=txtModalBusca]").html('Nº SUS *');
    jQuery("#txtModalBusca").attr('placeholder', 'Número');
    jQuery('#modalProfSus').find('input[type=text], select').each(function () {
        $(this).parents(".form-group").removeClass("has-error");
    });
    jQuery('#listamodalProfSus tbody').empty();
    jQuery('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center">Efetue a busca acima</th></tr>');
    jQuery('#modalProfSus').modal('show');
}

function busca_Prof_Sus() {
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
            var idAtendimentoDomiciliar = $("#idAtendimentoDomiciliar").val();
            if (!jQuery("#" + $(this).data("idusr")).hasClass("sel-cns")) {
                var idPacienteAtendimento = criaPacienteAtendimento(idAtendimentoDomiciliar, $(this).data("idusr"), $(this).data('cns'), $(this).data('nome'));
                getPacientes();
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

function alteraInfoProfissional(event) {
    var rowProfissional = $("#listaProfissionaisAtendimento tbody tr.sel-cns");
    var idProfAtend = rowProfissional.data("idprofatend");
    if (substituirProfissional(idProfAtend, $(event.currentTarget).data("idprof"), $(event.currentTarget).data("cbo"))) {
        makeTableProfissional(
            $(event.currentTarget).data("nome"),
            $(event.currentTarget).data("cns"),
            $(event.currentTarget).data("idprof"),
            idProfAtend,
            $(event.currentTarget).data("cbo")
        );
    } else {
        $('.msgErroDuplicado').text('Ocorreu um erro ao substituir o profissional.');
    }
}

//function executaSelectProcedimentos() {
//    var seleciona = $('#selOutros :selected').val();
//    var textcombo = $('#selOutros :selected').text();
//    var idAtendimentoDomiciliar = jQuery("#idAtendimentoDomiciliar").val();
//    var idAtendimentoUsuario = jQuery(".IdentificacaoUsuario").data("id");
//    if (seleciona > 0)
//    {
//        var idExame = salvarProcedimentosPacienteAtendimento(idAtendimentoDomiciliar, idAtendimentoUsuario, textcombo);
//        if (idExame != false) {
//            makeTableProcedimentosPaciente(idAtendimentoDomiciliar, idAtendimentoUsuario, textcombo, idExame);
//        } else {
//            modalAlerta("Atenção", "Falha ao adicionar o Procedimento do Paciente no Atendimento, tente novamente mais tarde.");
//        }

//    } else {
//        modalAlerta("Atenção", "Selecione no mínimo um Procedimento do Paciente no Atendimento para confirmar.");
//    }
//}

function prof_Sus() {
    if (validaCampos($("#modalProfSus"), 2, "alertaSemModalProfSus")) {
        var xTbBusca = $('#selModalTpBusca').val();
        var xBusca = removerAcentos($('#txtModalBusca').val().trim());
        jQuery('#listamodalProfSus tbody').empty();
        jQuery('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');
        jQuery.ajax({
            type: "POST",
            data: { tpBusca: xTbBusca, busca: xBusca },
            dataType: "json",
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
                var idAtendimentoDomiciliar = jQuery("#idAtendimentoDomiciliar").val();
                if ($("#listaProfissionaisAtendimento tbody tr.sel-cns").length > 0) {
                    alteraInfoProfissional(event);
                    $("#modalProfSus").modal("hide");
                } else {
                    if (!jQuery("#" + $(this).data("idprof")).hasClass("sel-cns")) {
                        var idProfissionalAtendimento = salvaProfissionalAtendimento(idAtendimentoDomiciliar, $(this).data("idprof"), $(this).data("cbo"));
                        if (idProfissionalAtendimento != false) {
                            makeTableProfissional($(this).data("nome"), $(this).data("cns"), $(this).data("idprof"), idProfissionalAtendimento, $(this).data("cbo"));
                        }
                    } else {
                        $('.msgErroDuplicado').text('O Profissional selecionado já foi inserido na Atendimento e não poderá ser inserido novamente.');
                    }
                }
            }
        });
    }
    return false;
}

function salvaProfissionalAtendimento(idAtendimentoDomiciliar, idProfissional, cbo) {
    var blnRet = false;
    alteraAtendimento(1);
    jQuery.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createProfissional',
            idAtendimentoDomiciliar: idAtendimentoDomiciliar,
            idProfissionalSaude: idProfissional,
            cbo: cbo
        },
    url: Api.URL
    })
    .done(function (data) {
        if (data > 0) {
            blnRet = parseInt(data);
        }
    });
    return blnRet;
}

function salvarProcedimentosPacienteAtendimento(idAtendimentoDomiciliar, idPacienteAtendimento, Descricao, SIGTAP) {
    var blnRet = false;
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createProcedimentosPaciente',
            idAtendimentoDomiciliar: idAtendimentoDomiciliar,
            idAtendimentoUsuario: idPacienteAtendimento,
            Descricao: Descricao,
            SIGTAP: SIGTAP
        },
        url: Api.URL
    })
    .done(function (data) {
        if (data > 0) {
            blnRet = parseInt(data);
        }
    });
    return blnRet;
}

function removerProfissionalAtendimento(idProfissionalAtendimento) {
    postAjaxObject({ acao: 'removeProfissional', id: idProfissionalAtendimento });
}

function criaPacienteAtendimento(idAtendimentoDomiciliar, idIdentificacaoUsuario, numeroSus, nome) {
    var blnRet = false;
    //alert(numeroSus)
    jQuery.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createUsuarioAtendido',
            idAtendimentoDomiciliar: idAtendimentoDomiciliar,
            idIdentificacaoUsuario: idIdentificacaoUsuario,
            numeroCartaoSus: numeroSus
        },
        url: Api.URL
    })
    .done(function (data) {
        if (data > 0)
            blnRet = parseInt(data);
    });
    return blnRet;
}

function substituirPacienteAtendimento(idIdentificacaoUsuario, numeroSus) {
    var blnRet = false;
    var numero = jQuery("#listaAtendimentoDomiciliar tbody tr").length + 1;
    var nasc = new Date();
    nasc = nasc.toLocaleDateString().replace(/[^ -~]/g, '');
    jQuery.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'substituirPaciente',
            idIdentificacaoUsuario: idIdentificacaoUsuario,
            numeroCartaoSus: numeroSus,
            dataNascimento: nasc,
            numero: numero,
            Id: parseInt(jQuery(".IdentificacaoUsuario").data("id"))

            //valor padrão data atual da maquina do usuario
        },
        url: Api.URL
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
                jQuery("[name='" + nameElemento + "[]'][value ='" + string[i] + "']").prop("checked", true);
            });
        }
    }
}

function dadosUsuarioAtendido() {
    var dados = {
        Id : parseInt(jQuery(".IdentificacaoUsuario").data("id")),
        Turno: jQuery("[name='rdbTurno']:checked").val(),        
        DataNascimento: jQuery("#txtDataNascimento").val(),
        NumeroCartaoSUS: jQuery("#txtCartaoSUS").val() == "" ? null : jQuery("#txtCartaoSUS").val(),
        Nome: jQuery("#nome").val(),
        Sexo: jQuery("[name='rdbSexo']:checked").val(),
        LocalAtendimento: jQuery("#selLocalAtend :selected").val() == undefined ? null : jQuery("#selLocalAtend :selected").val(),
        IdTipoAtendimento: jQuery("[name='rdbtipoAtend']:checked").val() == undefined ? null : jQuery("[name='rdbtipoAtend']:checked").val(),
        ModalidadeAd: jQuery("[name='rdbModalidade']:checked").val(),
        CondutaMotivoSaida: jQuery("[name='rdbConduta']:checked").val() == undefined ? null : jQuery("[name='rdbConduta']:checked").val(),
        Ciap: jQuery("#selCiap :selected").val() == undefined ? null : jQuery("#selCiap :selected").val(),
        Cid: jQuery("#selCid :selected").val() == undefined ? null : jQuery("#selCid :selected").val(),
        Condicoes: concatenarCheckBox("chkCondAvaliada"),
        Procedimentos: concatenarCheckBox("chkProcedimentos"),
        PosObito: jQuery("#chkposObito").is(":checked") ? 1 : null,
        IdAtendimentoDomiciliarUsuarioAtendido: jQuery("#idAtendimentoDomiciliar").val(),
        //Tratar depois com data
        Numero: 0
    };
    return JSON.stringify(dados);
}

function dadosAtendimentoDomiciliar() {
    var dados = {
        codigoCnesUnidade : $('#selCnesUni :selected').val(),
        codEquipe: ($('#selIneEquipe').val() == "") ? 0 : $('#selIneEquipe').val(),
        conferidoPor :"",
        dataAtendimento: $('#txtData').val(),
        numeroFolha: 0,
        Id: jQuery("#idAtendimentoDomiciliar").val()
    };
    return JSON.stringify(dados);
}

function alteraPacienteAtendimento() {
   // 
        jQuery.ajax({
            type: "POST",
            data: {
                acao: 'editUsuarioAtendido',
                dadosAtendimentoDomiciliarUsuarioAtendido: dadosUsuarioAtendido()
            },
            url: "ajax/atendDomiciliar/ajax-atendimento-Domiciliar.asp"
        }).done(
            function () {
                limparFormAtendimentoUsuario();
                //$('.IdentificacaoUsuario').hide();
                $("#dadosUsuario").appendTo("#identificacaoUsuarioOculta");
                $("#dadosAvaliacao").appendTo("#identificacaoUsuarioOculta");

                $('#btnConfirmar').hide();
                $('#btnCancelar').hide();
                $('#btnFinalizarAtend').show();
                modalAlerta("Confirmação", "Atendimento salvo com sucesso!");
            }
        )
}

function alteraAtendimento(status) {
    jQuery.ajax({
        type: "POST",
        data: {
            acao: 'Edit',
            dadosAtendimentoDomiciliar: dadosAtendimentoDomiciliar(),
            status: status
        },
        url: Api.URL
    }).done(function () {
        if(status == 2)
        {
            $.ajax({
                type: "POST",
                data: { acao: 'GetValidacao', id: jQuery("#idAtendimentoDomiciliar").val() },
                datatype: "json",
                url: Api.URL
            })
            .done(function (data) {
                data = JSON.parse(data)
                if (data.status) {
                    modalAlerta("Confirmação", "Atendimento finalizado com sucesso!")
                    setInterval(function () { window.location = "atendimentoDomiciliar.asp" }, 5000);
                }
                else {
                    modalAlerta("Erro", data.resultado);
                }
            });
        }
    });
}

function removerPacienteAtendimento(idPacienteAtendimento, idusr, tr) {
    postAjaxObject({ acao: 'removeUsuarioAtendido', id: idPacienteAtendimento })
    .done(onSuccess)
    .fail(onError);

    function onSuccess() {
        if (jQuery('#listaAtendimentoDomiciliar tbody tr').length == 1 || jQuery("#" + idusr).hasClass("active")) {
            limparFormAtendimentoUsuario();
            jQuery('#btnFinalizarAtend').hide();
            jQuery('.IdentificacaoUsuario').hide();
            jQuery("#btnConfirmar").hide();
            jQuery("#btnCancelar").hide();
        }

        if (jQuery('#listaAtendimentoDomiciliar tbody tr').length == 2) {
            jQuery("#listaAtendimentoDomiciliar tbody tr").removeClass("active");
        }

        jQuery(tr).remove();
        desabilitaBuscaPaciente();
    }

    function onError() {
        modalAlerta("Atenção", "Falha ao remover o Paciente, tente novamente mais tarde.");
    }
}

function limparFormAtendimentoUsuario() {
    jQuery('.IdentificacaoUsuario').each(function (i) {
        jQuery(this).find("input[type='checkbox']:checked").removeAttr('checked', false);
        jQuery(this).find("input[type='text']").val('');
        jQuery(this).find("input[type='radio']:checked").removeAttr('checked', false);
        jQuery('#listaProcedimentos tbody').empty();
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
    jQuery('#listaAtendimentoDomiciliar tbody').append(strTable);
    jQuery('#msgErroTableUsu').text('');
    removeGridPacientes();
    desabilitaBuscaPaciente();
    habilitaFichaPaciente();
    jQuery('#modalProfSus').modal('hide');
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
        url: "ajax/atendDomiciliar/ajax-atendimento-domiciliar.asp"
    })
    .success(function (data) {
        blnRet = true;
    });
    return blnRet;
}

function makeTableProfissional(nome, cns, idprof, idprofatend, cbo) {
    var strTable = "";
    strTable += "   <tr id='" + idprof + "' class='sel-cns' data-nome='" + nome.toCapitalize() + "' data-cns='" + cns + "' data-idprof='" + idprof + "' data-idprofatend='" + idprofatend + "' data-cbo='" + cbo + "'>";
    strTable += "       <td class='text-capitalize'>" + nome.toCapitalize() + "</td>";
    strTable += "       <td class='text-center'>" + cns + "</td>";
    strTable += "       <td class='text-center'>" + cbo + "</td>";
    strTable += "       <td class='text-center'><button type='button' class='btn btn-success' title='Substituir'><span class='glyphicon glyphicon-refresh'></span></button></td>";
    strTable += "   </tr>";
    removeGridProfissionais();
    jQuery("#listaProfissionaisAtendimento tbody").append(strTable);
    jQuery('#msgErroTableProf').text('');
    substituiProfSus();
    desabilitaBuscaProfissional();
    jQuery("#modalProfSus").modal("hide");
}

function makeTableProcedimentosPaciente(idAtendimentoDomiciliar, idAtendimentoUsuario, textcombo, idExame, SIGTAP) {
    var strTable = '';
    strTable += '   <tr class="sel-cns" data-idexame="' + idExame + '" data-SIGTAP="' + SIGTAP + '">';
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
    $.ajax({
        type: "POST",
        data: { acao: 'removeProcedimentosPaciente', id: idExame },
        datatype: "json",
        url: "ajax/atendDomiciliar/ajax-atendimento-Domiciliar.asp"
    })
    .done(function (data) {
    });
}

function removeGridProfissionais() {
  //  $('#listaProfissionaisAtendimento tbody .btn-danger').click(function () {
        var tr = $("#listaProfissionaisAtendimento tbody tr.sel-cns");
      //  removerProfissionalAtendimento($(tr).data("idprofatend"));
        $(tr).remove();
        desabilitaBuscaProfissional();
    //});
}

function desabilitaBuscaProfissional() {
    if ($('#listaProfissionaisAtendimento tbody tr').length > 0) {
        $('#btnBuscarProf').attr('disabled', 'disabled');
    } else {
        $('#btnBuscarProf').removeAttr('disabled');
    }
}

function removeGridPacientes() {
    $('#listaAtendimentoDomiciliar tbody .btn-danger').click(function () {
        var tr = $(this).closest("tr");
        removerPacienteAtendimento($(tr).data("idusratend"), $(tr).data("idusr"), tr);
    });
}

function desabilitaBuscaPaciente() {
    if ($('#listaAtendimentoDomiciliar tbody tr').length > 12) {
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
    if ($('#listaAtendimentoDomiciliar tbody tr').length == 0) {
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
    if ($('#listaProcedimentos tbody tr').length > 4) {
        $('#btnConfObs').attr('disabled', 'disabled');
    } else {
        $('#btnConfObs').removeAttr('disabled');
    }
}

function habilitaFichaPaciente() {
    $("#listaAtendimentoDomiciliar tbody .preencherDadosPaciente").on("click", function () {
        var data = $(this).closest("tr").data("idusratend");
        $("#listaAtendimentoDomiciliar tbody tr ").removeClass("active");

        if ($('.IdentificacaoUsuario').parent("#identificacaoUsuarioOculta").length > 0) {
            $("#dadosUsuario").appendTo("#Usuario");
            $("#dadosAvaliacao").appendTo("#UsuarioAvaliacao");
        }
        else {
            $('.IdentificacaoUsuario').show();
        }
        if ($("#listaAtendimentoDomiciliar tbody tr ").length > 1) {
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
    });
    
}

function getFichaPaciente(idUsrAtend) {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getUsuarioAtendidoById', idAtendimentoDomiciliar: idUsrAtend },
        datatype: "json",
        url: "ajax/atendDomiciliar/ajax-atendimento-Domiciliar.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data)[0];
        makeDadosPaciente(obj);
    });
}

function makeDadosPaciente(item) {       

    jQuery("#txtCartaoSUS").val(item.numero_cartao_sus);
    jQuery("#nome").val(item.nome);

    if (item.data_nascimento != '') {
        var dataNasc = item.data_nascimento.split("-");
        jQuery("#txtDataNascimento").val(dataNasc[2] + "/" + dataNasc[1] + "/" + dataNasc[0]);
    }
    jQuery("[name='rdbTurno'][value='" + item.turno + "']").prop("checked", true);
    jQuery("[name='rdbSexo'][value='" + item.sexo + "']").prop("checked", true);
    jQuery("#selLocalAtend").val(item.local_atendimento);
    jQuery("[name='rdbtipoAtend'][value='" + item.id_tipo_atendimento + "']").prop("checked", true);  
    jQuery("[name='rdbModalidade'][value='" + item.modalidade_ad + "']").prop("checked", true);
    jQuery("[name='rdbConduta'][value='" + item.conduta_motivo_saida + "']").prop("checked", true);
    exibirCheckBox("chkProcedimentos", item.procedimentos);
    exibirCheckBox("chkCondAvaliada", item.condicao);
    jQuery("#selCiap").val(item.ciap);
    jQuery("#selCid").val(item.cid);        
    stringToBoolean(item.acompanhamento_pos_obito) ? jQuery("#chkposObito").prop("checked", true) : "";
    jQuery("#idAtendimentoDomiciliar").val(item.id_atendimento_domiciliar);
    $('.IdentificacaoUsuario').data("numero", item.numero);
}

function getProfissionais() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getProfissionalByIdAtendimento', idAtendimentoDomiciliar: jQuery("#idAtendimentoDomiciliar").val() },
        datatype: "json",
        url: "ajax/atendDomiciliar/ajax-atendimento-Domiciliar.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            $.each(obj.resultado, function (ResultadoItens, item) {
                makeTableProfissional(item.Nome, item.CNS, item.IdProf, item.IdProfAtend, item.CBO);
            });
        }
    });
}

function getPacientes() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getPacienteByIdAtendimento', idAtendimentoDomiciliar: jQuery("#idAtendimentoDomiciliar").val() },
        datatype: "json",
        url: "ajax/atendDomiciliar/ajax-atendimento-Domiciliar.asp"
    })
    .done(function (data) {
         var obj = JSON.parse(data)
         if (obj.status) {
             $('#listaAtendimentoDomiciliar tbody').empty();
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
        data: { acao: 'getProcedimentosPacienteByIdAtendimento', idAtendimentoDomiciliar: jQuery("#idAtendimentoDomiciliar").val(), idAtendimentoUsuario: jQuery(".IdentificacaoUsuario").data("id") },
        datatype: "json",
        url: "ajax/atendDomiciliar/ajax-atendimento-Domiciliar.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            $('#listaProcedimentos tbody').empty();
            $.each(obj.resultado, function (ResultadoItens, item) {
                makeTableProcedimentosPaciente(item.idAtendimentoDomiciliar, item.idAtendimentoUsuario, item.Descricao, item.id, item.SIGTAP);
            });
        }
    });
}

function buscaProcedimento() {
    jQuery(".msgErroDuplicado").text('');
    jQuery(".modal-title").html("Busca de SIGTAP");

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
            url: "ajax/atendDomiciliar/ajax-atendimento-Domiciliar.asp"
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
                executaSelectProcedimentos();
            } else {
                $('#listaModalProcedimento tbody').append('<tr><th colspan="2" class="text-center">Nenhum resultado encontrado!</th></tr>');
            }
        });
    }
    function executaSelectProcedimentos() {
        jQuery("#listaModalProcedimento tbody .sel-procedimento").click(function () {
            //Remove o active dos outros
            jQuery(this).siblings().removeClass("list-group-item-success");
            var idAtendimentoDomiciliar = jQuery("#idAtendimentoDomiciliar").val();
            var idAtendimentoUsuario = jQuery(".IdentificacaoUsuario").data("id");
            var codigo = jQuery(this).data("codproc");
            var nome = jQuery(this).data("nome");
            var SIGTAP = jQuery(this).data("codigo").trim();
            
            if ($('#listaProcedimentos tbody tr').length < 4) {
                if ($('#listaProcedimentos tbody tr').filter('[data-SIGTAP="' + SIGTAP + '"]').length == 0) {
                    var idExame = salvarProcedimentosPacienteAtendimento(idAtendimentoDomiciliar, idAtendimentoUsuario, nome, SIGTAP);
                    if (idExame != false) {
                        $('#modalSigtap').modal('hide');
                        makeTableProcedimentosPaciente(idAtendimentoDomiciliar, idAtendimentoUsuario, nome, codigo);
                    } else {
                        modalAlerta("Atenção", "Falha ao adicionar o Procedimento do Paciente no Atendimento, tente novamente mais tarde.");
                    }
                } else {
                    $('.msgErroDuplicado').text('O Sigtap selecionado já foi inserido e não poderá ser inserido novamente.');
                }
            } else {
                $('.msgErroDuplicado').text('Não é possivel selecionar mais de 4 Procedimentos.');
            }
        });
    }
    return false;
}

function getDadosAtendimento() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getById', id: jQuery("#idAtendimentoDomiciliar").val() },
        datatype: "json",
        url: "ajax/atendDomiciliar/ajax-atendimento-Domiciliar.asp"
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
    jQuery("#Avaliacao").find(".msgErro").empty();
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
    $('#listaProfissionaisAtendimento tbody .btn-success').on("click", function () {
        busca_Prof_Sus();
    });
}

