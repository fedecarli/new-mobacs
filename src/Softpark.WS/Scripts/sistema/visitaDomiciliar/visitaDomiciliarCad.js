var Api = { URL: "ajax/visitaDomiciliar/ajax-visita-domiciliar.asp", getPaciente: "ajax/atendIndividual/getPaciente.asp" };

jQuery(document).ready(function () {
    //Modal
    jQuery('#modalProfSus').on('shown.bs.modal', function () { jQuery('#txtModalBusca').focus(); });

    //VALIDAÇÕES DO DICIONARIO DE DADOS DO CDS
    $("#txtCartaoSUS").on('blur', function () {
        validacaoCampo('#txtCartaoSUS', 'CNS');
    });
    $("#txtDataNascimento").on('blur', function () {
        validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade');
    });
    $('[name=rdbDesfecho]').on('click', function () {
        validaDesfecho();
    });
    function validaDesfecho() {
        if ($('[name=rdbDesfecho]:checked').val() == 'Visita Realizada') {
            $('[name^=chkVisita]').removeProp('disabled');
        } else {
            $('[name^=chkVisita]').removeProp('checked').prop('disabled', 'disabled');
        }
    }
    //Evento Click
    //jQuery('#btnConfirmar').on('click', verificarItensnaTable);
    $('#btnConfirmar').on('click', function (evt) {
        var verificacao = verificarItensnaTable();
        if (verificacao) { verificacao = validacaoCampo('#txtCartaoSUS', 'CNS'); };
        if (verificacao) { verificacao = validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade'); };
        validaDesfecho();
        if (!verificacao) {
            evt.preventDefault();
        }
    });
    jQuery('#btnCancelar').on('click', limparFormAtendimentoUsuario);
    jQuery('#btnFinalizarAtend').on('click', finalizar);
    jQuery('#btnNovoCartaoSus').on('click', addSemCadastroVisitaDomiciliar);
    jQuery('[name=rdbDesfecho]').on('click', validaDesfecho);
    $('#selCnesUni').on('change', CarregarEquipeIne);
    //Get dos Dados - Carregamento da Tela
    getDadosVisitaDomiciliar();
    getPacientes();
    getProfissionais();

    //Validate
    requires();
});

function requires() {
    $("#fichaVisitaDomiciliar").validate({
        errorPlacement: function (error, element) {
            console.log(element.context.id);
            var elemento = '';
            if (element.context.id != '') {
                elemento = '#' + element.context.id;
            } else {
                elemento = '[name="' + element.context.name + '"]'
            }
            if (jQuery(elemento).closest("div").find("div.msgErro").length == 0) {
                jQuery(elemento).closest("div").append("<div class='msgErro'></div>");
                //error.appendTo(jQuery("#msgErro"));
            }
            error.appendTo(jQuery(elemento).closest("div").find("div.msgErro"));
        },
        rules: {
            selCnesUni: {
                required: true
            },
            txtDataNascimento: {
                required: true
            },
            rdbDesfecho: {
                required: true
            },
            rdbSexo: {
                required: true
            },
            txtData: {
                required: true
            },
            txtCartaoSUS: {
                minlength: 15,
                maxlength: 15
            }
        },
        messages: {
            selCnesUni: " - Campo CNES é obrigatório",
            txtDataNascimento: " - Data de Nascimento é obrigatória",
            rdbDesfecho: " - Tipo de Desfecho é obrigatório",
            rdbSexo: " - O campo Sexo é obrigatório",
            txtData: " - Data é obrigatória",
            txtCartaoSUS: "Nº Cartão SUS incorreto."
        },
        submitHandler: function (form) {
            if ($('[name=rdbDesfecho]:checked').val() == 'Visita Realizada' && $('[name^=chkVisita]:checked').length == 0) {
                $('[name^=chkVisita]').parents('fieldset:last').find("div.msgErro").remove();
                $('[name^=chkVisita]').parents('fieldset:last').append("<div class='msgErro'>Campo Motivo da Visita é obrigatório</div>");
                return false;
            }
            if (verificarItensnaTable()) {
                alteraPacienteVisita();
            }
        }
    });
}

function finalizar() {
    if ($('#selCnesUni').val() != '') {
        if ($('#txtData').val() != '') {
            if ($('#selIneEquipe option').length > 0) {
                if ($('#selIneEquipe').val() != '') {
                    if (verificarItensnaTable()) {
                        alteraAtendimento(2);
                    }
                } else {
                    modalAlerta("Erro", "O campo Cód. equipe (INE) é obrigatório");
                    return false;
                }
            } else {
                if (verificarItensnaTable()) {
                    alteraAtendimento(2);
                }
            }
        }
        else {
            modalAlerta("Erro", "O campo Data é obrigatório");
            return false;
        }
    }
    else {
        modalAlerta("Erro", "O campo Cód. CNES Unidade é obrigatório");
        return false;
    }
}

function chaveamentoModal(evt) {
    if ($(".modal-title").html() == "Busca de Profissional") {
        return prof_Sus();
    } else {
        return paciente_Sus();
    }
}

function busca_Paciente_Sus() {
    jQuery('#txtmodalBuscaCartaoSus').val('');
    jQuery('#txtmodalConfirmaCartaoSus').val('');
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
                } else {
                    $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center">Nenhum resultado encontrado!</th></tr>');
                    $('#modalProfSus').modal('hide');
                    $('#txtmodalBuscaCartaoSus').val(xBusca);
                    $('#modalSemCartaoSus').modal('show');
                    $('#modalSemCartaoSus .msgErro .col-md-12').text('');
                }
            }
        });
    }

    function executaSelect() {
        $('#listamodalProfSus tbody .sel-cns').on('click',function (event) {
            $(this).siblings().removeClass('list-group-item-success');
            var idVisitaDomiciliar = $("#idVisitaDomiciliar").val();
            if (!$("#" + $(this).data("idusr")).hasClass("sel-cns")) {""
                var idPacienteVisita = salvarPaciente(idVisitaDomiciliar, $(this).data("idusr"), $(this).data('cns'), $(this).data('nome'));
                if (idPacienteVisita != false) {
                    getPacientes();
                    //makeTablePaciente($(this).data('nome'), $(this).data('cns'), $(this).data('idusr'), idPacienteVisita)
                }
            } else {
                $('.msgErroDuplicado').text('O Paciente selecionado já foi inserido na Visita Domiciliar e não poderá ser inserido novamente.');
            }
        });
    }
    return false;
}

function pacienteNovamente() {
    $('#modalSemCartaoSus').modal('hide');
    busca_Paciente_Sus();
}

function novoCartaoSus() {
    var idVisitaDomiciliar = jQuery("#idVisitaDomiciliar").val();
    var idPacienteVisita = salvarPaciente(idVisitaDomiciliar, "", "");
    if (idPacienteVisita != false) {
        makeTablePacienteSemCartaoSus(idPacienteVisita);
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
        $("#listamodalProfSus tbody .sel-cns").click(function (event) {
            if ($(this).data('cns') == '') {
                $(this).parents('.modal').modal('hide');
                modalAlerta('Atenção', 'Este Profissional não pode ser selecionado, pois não tem CNS cadastrado');
            } else if ($(this).data('cbo') == '') {
                $(this).parents('.modal').modal('hide');
                modalAlerta('Atenção', 'Este Profissional não pode ser selecionado, pois não tem CBO cadastrado');
            } else {
                $(this).siblings().removeClass("list-group-item-success");
                var idVisitaDomiciliar = jQuery("#idVisitaDomiciliar").val();
                if ($("#listaProfissionaisAtendimento tbody tr.sel-cns").length > 0) {
                    alteraInfoProfissional(event);
                    $("#modalProfSus").modal("hide");
                } else {
                    if (!jQuery("#" + $(this).data("idprof")).hasClass("sel-cns")) {
                        var idProfissionalVisita = salvarProfissional(idVisitaDomiciliar, $(this).data("idprof"), $(this).data("cbo"));
                        if (idProfissionalVisita != false) {
                            makeTableProfissional($(this).data("nome"), $(this).data("cns"), $(this).data("idprof"), idProfissionalVisita, $(this).data("cbo"));
                        }
                    } else {
                        $('.msgErroDuplicado').text('O Profissional selecionado já foi inserido na Visita Domiciliar e não poderá ser inserido novamente.');
                    }
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
            $(event.currentTarget).data("cbo")
        );
    } else {
        $('.msgErroDuplicado').text('Ocorreu um erro ao substituir o profissional.');
    }
}

function salvarPaciente(idVisitaDomiciliar, idPaciente) {
    var blnRet = false;
    var numero = $("#listaAtendimentoIndividual tbody tr").length + 1;
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createPaciente',
            idVisitaDomiciliar: idVisitaDomiciliar,
            idPaciente: idPaciente
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

function salvarProfissional(idVisitaDomiciliar, idProfissional, cbo) {
    var blnRet = false;
    alteraAtendimento(1);
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createProfissional',
            idVisitaDomiciliar: idVisitaDomiciliar,
            idProfissional: idProfissional,
            cbo: cbo
        },
        datatype: "json",
        url: "ajax/visitaDomiciliar/ajax-visita-domiciliar.asp"
    })
    .done(function (data) {
        if (data > 0) {
            blnRet = parseInt(data);
        }
    });
    return blnRet;
}

function substituirProfissional(idProfVisita, idProfissional, cbo) {
    var blnRet = false;
    alteraAtendimento(1);
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'substituirProfissional',
            idProfVisita: idProfVisita,
            idProfissional: idProfissional,
            cbo: cbo
        },
        datatype: "json",
        url: "ajax/visitaDomiciliar/ajax-visita-domiciliar.asp"
    })
    .success(function (data) {
        blnRet = true;
    });
    return blnRet;
}

function makeTablePacienteSemCartaoSus(idusratend) {
    var strTable = '';
    strTable += '   <tr class="sel-cns" data-nome="" data-cns="" data-idusr="" data-idusratend="' + idusratend + '" >';
    strTable += '       <td class="text-capitalize"></td>';
    strTable += '       <td class="text-center"></td>';
    strTable += '       <td class="text-center"><button type="button" class="btn btn-danger" title="Remover"><span class="glyphicon glyphicon-remove"></span></button></td>';
    strTable += "       <td class='text-center'><button type='button' class='btn btn-primary preencherDadosPaciente' title='Preencher Ficha'><span class='glyphicon glyphicon-edit'></span></button></td>";
    strTable += '   </tr>';
    $('#listaAtendimentoIndividual tbody').append(strTable);
    $('#msgErroTableUsu').text('');
    removeGridPacientes();
    desabilitaBuscaPaciente();
    habilitaFichaPaciente();
    $('#modalSemCartaoSus').modal('hide');
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

function makeTableProfissional(nome, cns, idprof, idprofatend, cbo) {
    var strTable = "";
    strTable += "   <tr id='" + idprof + "' class='sel-cns' data-nome='" + nome.toCapitalize() + "' data-cns='" + cns + "' data-idprof='" + idprof + "' data-idprofatend='" + idprofatend + "' data-cbo='" + cbo + "'>";
    strTable += "       <td class='text-capitalize'>" + nome.toCapitalize() + "</td>";
    strTable += "       <td class='text-center'>" + cns + "</td>";
    strTable += "       <td class='text-center'>" + cbo + "</td>";
    strTable += "       <td class='text-center'><button type='button' class='btn btn-success' title='Substituir'><span class='glyphicon glyphicon-refresh'></span></button></td>";
    strTable += "   </tr>";
    removeGridProfissionais();
    $("#listaProfissionaisAtendimento tbody").append(strTable);
    $('#msgErroTableProf').text('');
    substituiProfSus();
    desabilitaBuscaProfissional();
    $("#modalProfSus").modal("hide");
}

function makeDadosPaciente(item) {
    $.each(item[0], function (key, value) {
        stringToBoolean(value) ? $("#chk" + key.toCapitalize()).prop("checked", true) : "";
    });

    if (item[0].turno != null)
        $("[name='rdbTurno'][value='" + item[0].turno + "']").prop("checked", true);

    if (item[0].sexo != null)
        $("[name='rdbSexo'][value='" + item[0].sexo + "']").prop("checked", true);

    $("#txtCartaoSUS").val(item[0].numero_cartao_sus);
    $("#txtProntuario").val(item[0].numero_prontuario);
    $("#txtNome").val(item[0].nome);
    //if (item[0].data_nascimento != null) {
    $("#txtDataNascimento").val(formatarDataParaExibicao(item[0].data_nascimento));
    //}
    if (item[0].visita_realizada) { $("[name='rdbDesfecho'][value='Visita Realizada']").prop("checked", true) };
    if (item[0].visita_recusada) { $("[name='rdbDesfecho'][value='Visita Recusada']").prop("checked", true); $('[name^=chkVisita]').prop('disabled', 'disabled') };
    if (item[0].ausente) {$("[name='rdbDesfecho'][value='Ausente']").prop("checked", true); $('[name^=chkVisita]').prop('disabled','disabled')}
}

function removeGridPacientes() {
    $('#listaAtendimentoIndividual tbody .btn-danger').click(function () {
        var tr = $(this).closest("tr");
        removerPacienteAtendimento($(tr).data("idusratend"), $(tr).data("idusr"), tr);
    });
}

function removerPacienteAtendimento(idPacienteVisita, idusr, tr) {
    return jQuery.ajax({
        type: "POST",
        data: { acao: 'removePaciente', id: idPacienteVisita },
        url: Api.URL
    })
    .done(onSuccess)
    .fail(onError);

    function onSuccess() {
        if (jQuery('#listaAtendimentoIndividual tbody tr').length == 1 || jQuery("#" + idusr).hasClass("active")) {
            limparFormAtendimentoUsuario();
            jQuery('#btnFinalizarAtend').hide();
            jQuery('.IdentificacaoUsuario').hide();
            jQuery("#btnConfirmar").hide();
            jQuery("#btnCancelar").hide();
        }

        if (jQuery('#listaAtendimentoIndividual tbody tr').length == 2) {
            jQuery("#listaAtendimentoIndividual tbody tr").removeClass("active");
        }

        jQuery(tr).remove();
        desabilitaBuscaPaciente();
    }

    function onError() {
        modalAlerta("Atenção", "Falha ao remover o Paciente, tente novamente mais tarde.");
    }
}

function removeGridProfissionais() {
    //$('#listaProfissionaisAtendimento tbody .btn-danger').on("click", function () {
        //var rows = $("#listaProfissionaisAtendimento tbody tr").length;
        //if (rows > 1) {
            var tr = $("#listaProfissionaisAtendimento tbody tr.sel-cns");
            //if (rows == 2) {
            //    $("#listaProfissionaisAtendimento tbody tr button.btn-danger")
            //        .after('<button type="button" class="btn btn-success" title="Substituir"><span class="glyphicon glyphicon-refresh"></span></button>');
            //    $("#listaProfissionaisAtendimento tbody tr button.btn-danger").remove();
            //}
            //removerProfissionalAtendimento($(tr).data("idprofatend"));
            $(tr).remove();
            desabilitaBuscaProfissional();
        //}
    //});
}

function removerProfissionalAtendimento(idProfissionalVisita) {
    $.ajax({
        type: "POST",
        data: { acao: 'removeProfissional', id: idProfissionalVisita },
        datatype: "json",
        url: "ajax/visitaDomiciliar/ajax-visita-domiciliar.asp"
    })
    .done(function (data) {
    });
}

function desabilitaBuscaPaciente() {
    if ($('#listaAtendimentoIndividual tbody tr').length > 22) {
        $('#btnBuscarUsu').attr('disabled', 'disabled');
        $('#btnAddSemCadastro').attr('disabled', 'disabled');
    } else {
        $('#btnAddSemCadastro').removeAttr('disabled');
        $('#btnBuscarUsu').removeAttr('disabled');
    }
}

function verificarItensnaTable() {
    var retorno = true;
    $('#msgErroTableUsu').text('');
    $('#msgErroTableProf').text('');
    if ($('#listaAtendimentoIndividual tbody tr').length == 0) {
        $('#msgErroTableUsu').text('Insira no mínimo um Paciente para realizar a Visita Domiciliar.');
        $('#btnBuscarUsu').focus();
        retorno = false;
    }
    if ($('#listaProfissionaisAtendimento tbody tr').length == 0) {
        $('#msgErroTableProf').text('Insira no mínimo um Profissional para realizar a Visita Domiciliar.');
        $('#btnBuscarProf').focus();
        retorno = false;
    }
    return retorno;

}

function desabilitaBuscaProfissional() {
    if ($('#listaProfissionaisAtendimento tbody tr').length > 0) {
        $('#btnBuscarProf').attr('disabled', 'disabled');
    } else {
        $('#btnBuscarProf').removeAttr('disabled');
    }
}

function habilitaFichaPaciente() {
    $("#listaAtendimentoIndividual tbody .preencherDadosPaciente").on("click", function () {
        var data = $(this).closest("tr").data("idusratend");
        $("#listaAtendimentoIndividual tbody tr ").removeClass("active");
        if ($("#listaAtendimentoIndividual tbody tr ").length > 1) {
            jQuery(this).closest("tr").addClass("active");
        }
        limparFormAtendimentoUsuario();
        $('.IdentificacaoUsuario').show().data("id", data);
        getFichaPaciente(data);
        $('#btnConfirmar').show();
        $('#btnCancelar').show();
        
    });
}

function getPacientes() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getPacientesById', idVisitaDomiciliar: jQuery("#idVisitaDomiciliar").val() },
        datatype: "json",
        url: "ajax/visitaDomiciliar/ajax-visita-domiciliar.asp"
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

function getProfissionais() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getProfissionaisById', idVisitaDomiciliar: jQuery("#idVisitaDomiciliar").val() },
        datatype: "json",
        url: "ajax/visitaDomiciliar/ajax-visita-domiciliar.asp"
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

function limparFormAtendimentoUsuario() {
    $('.IdentificacaoUsuario').each(function (i) {
        $(this).find('input[type=checkbox]:checked').removeAttr('checked', false);
        $(this).find('input[type=radio]:checked').removeAttr('checked', false);
    });
    removerValidacoesParticipantes();
}

function dadosVisitaDomiciliar() {
    var dados = {
        codigoCnesUnidade: $('#selCnesUni :selected').val(),
        codEquipe: ($('#selIneEquipe').val() == "") ? null : $('#selIneEquipe').val(),
        dataVisita: $('#txtData').val(),
        Id: jQuery("#idVisitaDomiciliar").val()
    };
    return JSON.stringify(dados);
}

function alteraAtendimento(status) {
    
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'Edit',
            dadosVisitaDomiciliar: dadosVisitaDomiciliar(),
            status: status
        },
        datatype: "json",
        url: "ajax/visitaDomiciliar/ajax-visita-domiciliar.asp"
    }).done(function () {
        if (status == 2) {
            $.ajax({
                type: "POST",
                data: { acao: 'GetValidacao', id: jQuery("#idVisitaDomiciliar").val() },
                datatype: "json",
                url: Api.URL
            })
            .done(function (data) {
                data = JSON.parse(data)
                if (data.status) {
                    modalAlerta("Confirmação", "Visita Domiciliar finalizada com sucesso!")
                    setInterval(function () { window.location = "visitaDomiciliar.asp" }, 5000);
                }
                else {
                    modalAlerta("Erro", data.resultado);
                }
            });
        }
    });
}

function getDadosVisitaDomiciliar() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getById', id: jQuery("#idVisitaDomiciliar").val() },
        datatype: "json",
        url: "ajax/visitaDomiciliar/ajax-visita-domiciliar.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {

            if (obj.resultado.codigo_cnes_unidade > 0) {
                $('#selCnesUni').val(obj.resultado.codigo_cnes_unidade).prop('disabled', 'disabled');
            }         
            makeEquipeIne(obj.resultado.codigo_cnes_unidade, obj.resultado.codigo_equipe_ine);
            if (obj.resultado.data_visita != "01/01/1900" && obj.resultado.data_visita != "1900-01-01") {
                var dataVisita = obj.resultado.data_visita.split("-");
                if (dataVisita.length > 1) {
                    $('#txtData').val(dataVisita[2] + "/" + dataVisita[1] + "/" + dataVisita[0]);
                } else {
                    $('#txtData').val(dataVisita);
                }
            }
        }
    });
}

function dadosPaciente() {
    var dados = '{ "campos":[';
    var chks = $('.IdentificacaoUsuario').find('input[type=checkbox]');
    chks.each(function (index, element) {
        var key = $(element).attr("id");
        key = key.replace("chk", "");
        dados += '{"key":"' + key + '","value":' + ($(element).is(":checked") ? 1 : null) + '},';
    });
    dados += '{"key":"sexo","value":"' + $("[name='rdbSexo']:checked").val() + '"},';
    dados += '{"key":"turno","value":' + ($("[name='rdbTurno']:checked").val() == undefined ? null : '"' + $("[name='rdbTurno']:checked").val() + '"') + '},';
    dados += '{"key":"data_nascimento","value":"' + $("#txtDataNascimento").val() + '"},';
    dados += '{"key":"numero_cartao_sus","value":"' + $("#txtCartaoSUS").val() + '"},'; 
    dados += '{"key":"numero_prontuario","value":"' + $("#txtProntuario").val() + '"},';
    dados += '{"key":"nome","value":"' + $("#txtNome").val() + '"},';
    var desfecho = $("[name='rdbDesfecho']:checked").val()
    dados += '{"key":"visita_realizada","value":' + (desfecho == "Visita Realizada" ? 1 : null) + '},';
    dados += '{"key":"visita_recusada","value":' + (desfecho == "Visita Recusada" ? 1 : null) + '},';
    dados += '{"key":"ausente","value":' + (desfecho == "Ausente" ? 1 : null) + '}';
    dados += ']}';
    return dados;
}

function getFichaPaciente(idPaciente) {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getPacienteById', idPaciente: idPaciente },
        datatype: "json",
        url: "ajax/visitaDomiciliar/ajax-visita-domiciliar.asp"
    })
   .done(function (data) {
       var obj = JSON.parse(data)
       if (obj.status) {
           makeDadosPaciente(obj.resultado);
       }
   });
}

function alteraPacienteVisita() {
    
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'editPaciente',
            dadosPaciente: dadosPaciente(),
            id: jQuery(".IdentificacaoUsuario").data("id")
        },
        datatype: "json",
        url: "ajax/visitaDomiciliar/ajax-visita-domiciliar.asp"
    }).done(
        function () {
            limparFormAtendimentoUsuario();
            $('.IdentificacaoUsuario').hide();
            $('#btnConfirmar').hide();
            $('#btnCancelar').hide();
            $('#btnFinalizarAtend').show();
            modalAlerta("Confirmação", "Visita Domiciliar salvo com sucesso!");
        }
    )
}

function removerValidacoesParticipantes() {
    jQuery("#Usuario").find(".msgErro").empty();
    jQuery("#Usuario").find("input[type='text']").removeClass("error");
    jQuery("#Usuario").find("select").removeClass("error");
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
                $('#selIneEquipe').html('<option value="">NÃO HÁ EQUIPES</option>');
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
                $('#selIneEquipe').html('<option value="">NÃO HÁ EQUIPES</option>');
                $("label[for='txtIneEquipe'] .asterisco").remove();
                $("label[for='txtIneEquipe']").addClass('removeasterisco');
            }
        });
    }
}

function salvarSemCadastroVisitaDomiciliar(cns) {
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
            var idVisitaDomiciliar = $("#idVisitaDomiciliar").val();
            $.each(data.resultado, function (ResultadoItens, item) {
                if (cns == "") {
                    cns = item.cns
                }
                var idPacienteVisita = salvarPaciente(idVisitaDomiciliar, item.IDUSR, cns, "Sem Cadastro");
                if (idPacienteVisita != false) {

                    getPacientes();
                    //makeTablePaciente(item.nome, cns, item.IDUSR, idPacienteVisita)
                }
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

function addSemCadastroVisitaDomiciliar() {
    if ($('#txtmodalBuscaCartaoSus').val() != "") {
        if ($('#txtmodalBuscaCartaoSus').val() == $('#txtmodalConfirmaCartaoSus').val()) {
            salvarSemCadastroVisitaDomiciliar($('#txtmodalConfirmaCartaoSus').val());
        }
        else {
            $('#modalSemCartaoSus .msgErro .col-md-12').text('O cartão do SUS não confere com o digitado anteriormente.Favor digite novamente.');
        }
    }
    else {
        $('#txtmodalBuscaCartaoSus').val('');
        $('#txtmodalConfirmaCartaoSus').val('');
        salvarSemCadastroVisitaDomiciliar($('#txtmodalConfirmaCartaoSus').val());
    }

}

function substituiProfSus() {
    $('#listaProfissionaisAtendimento tbody .btn-success').on("click", function () {
        busca_Prof_Sus();
    });
}
