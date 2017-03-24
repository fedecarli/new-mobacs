var Api = { URL: "ajax/procedimento/ajax-procedimento.asp", getPaciente: "ajax/atendIndividual/getPaciente.asp" };
var Action = { List: "procedimento.asp" };

var profissionais = false;
var usuarios = false;
var procedimentos = false;
var exibirMsg = false;

jQuery(document).ready(function () {
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

    //Evento Click
    jQuery('#btnConfObs').on('click', executaSelectExames);
    jQuery('#btnCancelar').on('click', limparFormProcedimentoUsuario);
    jQuery('#btnFinalizarProcedimento').on('click', finalizar);

    $('#btnConfirmar').on('click', function (evt) {
        var verificacao = true;
        if (verificacao) { verificacao = validacaoCampo('#txtCartaoSUS', 'CNS'); };
        if (verificacao) { verificacao = validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade'); };
        if (verificacao) {
            $('#frmUsuario').submit();
        }

    });
    jQuery("#pnlProfissionais .panel-heading").on("click", getProfissionais);
    jQuery("#pnlProcedimentos .panel-heading").on("click", getProcedimentosConsolidados);
    $('#selCnesUni').on('change', CarregarEquipeIne);
    jQuery('#btnNovoCartaoSus').on('click', addSemCadastroProcedimento);
    jQuery('#ProcedimentosPequenasCirurgias input[type="checkbox"]').on("click", function () { jQuery('#msgErroPequenasCirurgias').text(''); jQuery('#msgErroPequenasCirurgias').parent().hide();})

    //Get dos Dados - Carregamento da Tela
    getProfissionais();
    getProcedimentosConsolidados();
    getDadosProcedimento();
    getPacientes();

    //Validate
    jQuery("#frmProfissional").validate({
        rules: {
            CodigoCnesUnidade: "CodCnesUnidadeSelecionado",
            CodigoEquipeIne: {
                required: true
            },
            DataProcedimento: {
                required: true
            },
            txtNomeSusProf: {
                required: true
            }            
        },
        messages: {
            CodigoEquipeIne: "Campo INE Obrigatório",
            DataProcedimento: "Campo Data é obrigatório",
            txtNomeSusProf: "Campo Profissional do SUS é obrigatório"
        },
        submitHandler: function (form) {
            if (verificarItensnaTableProfissionais()) {
                setProfissional();
                profissionais = true;
            }
        }
    });
    jQuery("#frmUsuario").validate({
        errorPlacement: function (error, element) {
            if (jQuery(element).parent("div").find("div.msgErro").length > 0) {
                error.appendTo(jQuery(element).parent("div").find("div.msgErro"));
            }
            else {
                error.appendTo(jQuery(element).parents("div.form-group").find("div.msgErro"));
            }
        },
        rules: {
            txtNomeSusUsu: {
                required: true
            },
            DataNascimento: {
                required: true
            },
            Sexo: {
                required: true
            },
            LocalAtendimento: {
                required: true
            },
            NumeroProntuario: {
                maxlength: 30,
                min:0
            }
        },
        messages: {
            txtNomeSusUsu: "Campo Número Cartão SUS é obrigatório",
            DataNascimento: "Data de Nascimento é obrigatória",
            Sexo: "Campo Sexo é obrigatório",
            LocalAtendimento: "Campo Local de Atendimento é obrigatório",
            NumeroProntuario: "Por favor, indique um numero maior que 0 e um número menor ou igual a 30 caracteres.",
            NumeroCartaoSus: "Nº Cartão SUS incorreto.",

        },
        submitHandler: function (form) {
            if (verificarItensnaTableUsuario()) {
                alteraPacienteProcedimento();
                usuarios = true;                
            }
        }
    });
    jQuery("#frmFinalizar").validate({
        rules: {
        },
        messages: {
        },
        submitHandler: function () {

        }
    });
    jQuery("#frmProcedimentosConsolidados").validate({        
        submitHandler: function () {
            if (validaProcedimentosConsolidados() && verificarItensnaTableProfissionais()) {
                setProcedimentosConsolidados();
                procedimentos = true;
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

function validaProcedimentosConsolidados() {
    var retorno = true;

    jQuery('#msgErroProcedimentosConsolidados').text('');
    var somaConsolidado = 0;

    jQuery('.procedimentosConsolidados').each(function (index, item){
        somaConsolidado = somaConsolidado + getValNumberById(item.id);
		if(getValNumberById(item.id)<0){
			jQuery('#msgErroProcedimentosConsolidados').parent().show();
			jQuery('#msgErroProcedimentosConsolidados').text('Os Procedimentos Consolidados não podem ser menores que 0.');
			jQuery('#' + item.id).focus();
			retorno = false;
		}
        setValDefaultById(item.id, '');
    })

    if (somaConsolidado == 0 && $('#listaProcedimento tbody tr').length==0) {
        jQuery('#msgErroProcedimentosConsolidados').parent().show();
        jQuery('#msgErroProcedimentosConsolidados').text('É necessário informar no mínimo um Procedimento Consolidado.');
        jQuery('.procedimentosConsolidados input[type="number"]').eq(0).focus();
        retorno = false;
    }

    return retorno;
}

function verificarItensnaTableProfissionais() {
    var retorno = true;

    $('#msgErroTableProf').text('');
    if ($('#listaProfissionaisProcedimento tbody tr').length == 0) {
        $('#msgErroTableProf').text('Insira no mínimo um Profissional.');
        $('#btnBuscarProf').focus();
        retorno = false;
    }

    return retorno;
}

function verificarItensnaTableUsuario() {
    var retorno = true;

    jQuery('#msgErroTableUsu').text('');
    jQuery('#msgErroTableSigtap').text('');

    if (jQuery('#listaSIGTAP tbody tr').length == 0 && !verificarItensPequenasCirurgias()) {
        jQuery('#msgErroTableSigtap').text('Insira no mínimo um SIGTAP.');
        jQuery('#btnBuscarSigtap').focus();
        jQuery('.IdentificacaoUsuario').show();
        retorno = false;
    }
    return retorno;
}

function verificarItensPequenasCirurgias() {
    var retorno = true;

    jQuery('#msgErroPequenasCirurgias').text('');    
    if (jQuery('#ProcedimentosPequenasCirurgias input[type="checkbox"]:checked').length == 0) {
        jQuery('#msgErroPequenasCirurgias').parent().show();
        jQuery('#msgErroPequenasCirurgias').text('É necessário informar no mínimo uma Pequena Cirurgia.');
        jQuery('#ProcedimentosPequenasCirurgias input[type="checkbox"]').eq(0).focus();
        retorno = false;
    }

    return retorno;
}

function finalizar() {
    var validado = false;
    var contemErros = false;
    jQuery("#frmProfissional").submit();
    jQuery("#frmProcedimentosConsolidados").submit();

    if (jQuery("#listaProcedimento tbody tr").length > 0) {
        //verificarItensnaTableUsuario();
        $.ajax({
            type: "POST",
            data: { acao: 'GetValidacao', id: jQuery("#idProcedimento").val() },
            datatype: "json",
            url: Api.URL
        })
        .done(function (data) {
            data = JSON.parse(data)
            if (data.status) {
                finalizaFicha();
            }
            else {
                modalAlerta("Erro", data.resultado);
            }
        });
    }
    else {
        if (profissionais && procedimentos) {
            finalizaFicha();
        }
    }
    function finalizaFicha() {
        $.ajax({
            type: "POST",
            data: { acao: 'SetFinalizaFicha', id: jQuery("#idProcedimento").val() },
            datatype: "json",
            url: Api.URL
        })
        .done(function () {
            modalAlerta("Confirmação", "Procedimento gravado com sucesso.");
            jQuery("#Procedimento").hide();
            //setInterval(window.location.replace(Action.List), 5000);
        })
    }
}

function executaSelectExames() {
    var seleciona = $('#selSIGTAP :selected').val();
    var textcombo = $('#selSIGTAP :selected').text();
    var idProcedimento = jQuery("#idProcedimento").val();
    var idProcedimentoUsuario = jQuery(".IdentificacaoUsuario").data("id");

    if (seleciona > 0) {
        var idExame = salvarExamesPacienteProcedimento(idProcedimento, seleciona);
        if (idExame > 0) {
            makeTableExamesPaciente(textcombo, idExame);
        } else {
            modalAlerta("Atenção", "Falha ao adicionar o SIGTAP do Paciente no Procedimento, tente novamente mais tarde.");
        }
    } else {
        modalAlerta("Atenção", "Selecione no mínimo um SIGTAP para confirmar.");
    }
}

function makeTableExamesPaciente(textcombo, idExame) {
    var strTable = '';
    strTable += '   <tr id="' + idExame + '" class="sel-cns" data-idexame="' + idExame + '">';
    strTable += '       <td class="text-capitalize">' + textcombo + '</td>';
    strTable += '       <td class="text-center"><button type="button" class="btn btn-danger" title="Remover"><span class="glyphicon glyphicon-remove"></span></button></td>';
    strTable += '   </tr>';
    jQuery('#listaSIGTAP tbody').append(strTable);
    jQuery('#msgErroTableSigtap').text('');
    removeGridExames();
    desabilitaBuscaExames();
    jQuery("#modalSigtap").modal("hide");
}

function removeGridExames() {
    $('#listaSIGTAP tbody .btn-danger').click(function () {
        var tr = $(this).closest("tr");
        removerExamesPacienteProcedimento($(tr).data("idexame"));
        $(tr).remove();
        desabilitaBuscaExames();
    });
}

function removerExamesPacienteProcedimento(idExame) {
    $.ajax({
        type: "POST",
        data: { acao: 'removeExamesPaciente', id: idExame },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
    });
}

function desabilitaBuscaExames() {
    if ($('#listaSIGTAP tbody tr').length > 5) {
        $('#btnConfObs').attr('disabled', 'disabled');
    } else {
        $('#btnConfObs').removeAttr('disabled');
    }
}

function salvarExamesPacienteProcedimento(idProcedimento, CodProc) {
    var blnRet = false;
    jQuery.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createExamesPaciente',
            idProcedimento: idProcedimento,
            idTipoProcedimento: null,
            CodProc: CodProc,
            id_Procedimento_Usuario: $('#usuarioAtivo').val()
        },
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

function cancelar() {
    if ($('#listaProcedimento tbody tr').length == 0) {
        window.location.assign("procedimento.asp");
    }
}

function busca_Paciente_Sus() {
    jQuery('#txtmodalBuscaCartaoSus').val('');
    jQuery('#txtmodalConfirmaCartaoSus').val('');
    limparFormProcedimentoUsuario();
    jQuery("#listaAtendimentoIndividual tbody tr ").removeClass("active");
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

function buscaSigtap() {

    jQuery(".modal-title").html("Busca de SIGTAP");

    jQuery("#tipoBusca").val('1');
    jQuery("#modalBusca").val('');
    jQuery("label[for=modalBusca]").html('Código *');
    jQuery("#modalBusca").attr('placeholder', 'Código');

    jQuery('#modalSigtap').find('input[type=text], select').each(function () {
        $(this).parents(".form-group").removeClass("has-error");
    });

    jQuery('#listaModalSigtap tbody').empty();
    jQuery('#listaModalSigtap tbody').append('<tr><th colspan="2" class="text-center">Efetue a busca acima</th></tr>');
    jQuery('#modalSigtap').modal('show');
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

function tpBuscaSigtap() {
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
            var idProcedimento = $("#idProcedimento").val();

            if (!jQuery("#" + $(this).data("idusr")).hasClass("sel-cns")) {
                var idPacienteProcedimento = criaPacienteProcedimento(idProcedimento, $(this).data("idusr"));
                if (idPacienteProcedimento != false) {
                    getPacientes();
                }
            } else {
                $('.msgErroDuplicado').text('O Profissional selecionado já foi inserido no Atendimento e não poderá ser inserido novamente.');
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
    //removeGridProfissionais();
    var rowProfissional = $("#listaProfissionaisProcedimento tbody tr.sel-cns");
    $("#idProcedimento").val(rowProfissional.data("idprocedimento"));
    $("#idProfissionalSaude").val($(event.currentTarget).data("idprof"));
    $("#cbo").val($(event.currentTarget).data("cbo"));
    makeTableProfissional(
        $(event.currentTarget).data("nome"),
        $(event.currentTarget).data("cns"),
        $(event.currentTarget).data("idprof"),
        rowProfissional.data("idProcedimento"),
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

                if ($("#listaProfissionaisProcedimento tbody tr.sel-cns").length > 0) {
                    alteraInfoProfissional(event);
                    $("#modalProfSus").modal("hide");
                } else {
                    if (!jQuery("#" + idProfissionalSaude).hasClass("sel-cns")) {
                        jQuery("#cbo").val(cbo);
                        jQuery("#idProfissionalSaude").val(idProfissionalSaude);
                        makeTableProfissional($(this).data("nome"), $(this).data("cns"), idProfissionalSaude, jQuery("#idProcedimento").val(), cbo);
                    } else {
                        $('.msgErroDuplicado').text('O Profissional selecionado já foi inserido na Visita Domiciliar e não poderá ser inserido novamente.');
                    }
                }
            }
        });
    }
    return false;
}

function salvaProfissionalProcedimento(idProcedimento, idProfissional, cbo) {
    //TODO: Verificar se será necessário o idAtendimento, idProfissional e o cbo
    
}

function removerProfissionalProcedimento(codigoCnesUnidade, codigoEquipeIne) {
    postAjax({ dadosProfissional: dadosProfissional, acao: "removeProfissional"});
}

function criaPacienteProcedimento(idProcedimento, idIdentificacaoUsuario) {
    var blnRet = false;

    jQuery.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createUsuarioProcedimento',
            idProcedimento: idProcedimento,
            idIdentificacaoUsuario: idIdentificacaoUsuario
        },
        url: "ajax/procedimento/ajax-procedimento.asp"
    })
    .done(function (data) {
        if (data > 0) {
            blnRet = parseInt(data);
        }
    });
    return blnRet;
}

function dadosUsuarioProcedimento() {
    var dados = getFormData("#frmUsuario");

    return JSON.stringify(dados);
}

function dadosProfissional() {
    var dados = getFormData("#frmProfissional");
    dados.CodigoCnesUnidade = $('#selCnesUni :selected').val();
    if (dados.DataProcedimento != 0) {
        dados.DataProcedimento = dados.DataProcedimento;
    }

    return JSON.stringify(dados);
}

function dadosProcedimentosConsolidados() {
    var dados = getFormData("#frmProcedimentosConsolidados");
    return JSON.stringify(dados);
}

function alteraPacienteProcedimento() {
    jQuery.ajax({
        type: "POST",
        data: {
            acao: 'editUsuarioProcedimento',
            dadosProcedimentoUsuario: dadosUsuarioProcedimento()
        },
        datatype: "json",
        url: Api.URL
    }).done(onSuccess);

    function onSuccess() {
        limparFormProcedimentoUsuario();
        jQuery('.IdentificacaoUsuario').hide();
        jQuery('#btnConfirmar').hide();
        jQuery('#btnCancelar').hide();
    }
}

function setProfissional() {
    postAjax({
        acao: 'setProfissional',
        dadosProfissional: dadosProfissional()
    });

    jQuery("#checkDadosProfissionais").show();
    jQuery('#dadosProfissionais').hide();
}

function setProcedimentosConsolidados() {
    postAjax({
        acao: 'setProcedimentosConsolidados',
        dadosProcedimentosConsolidados: dadosProcedimentosConsolidados()
    });

    jQuery("#checkDadosProcedimentos").show();
    jQuery('#dadosProcedimentos').hide();
}

function removerPacienteProcedimento(idPacienteProcedimento, idusr, tr) {
    return jQuery.ajax({
        type: "POST",
        data: { acao: 'removeUsuarioProcedimento', id: idPacienteProcedimento },
        url: Api.URL
    })
    .done(onSuccess)
    .fail(onError);

    function onSuccess() {
        if (jQuery('#listaProcedimento tbody tr').length == 1 || jQuery("#" + idusr).hasClass("active")) {
            limparFormProcedimentoUsuario();
        }

        if (jQuery('#listaProcedimento tbody tr').length == 2) {
            jQuery("#listaProcedimento tbody tr").removeClass("active");
        }

        jQuery(tr).remove();
        desabilitaBuscaPaciente();
    }

    function onError() {
        modalAlerta("Atenção", "Falha ao remover o Paciente, tente novamente mais tarde.");
    }
}

function limparFormProcedimentoUsuario() {
    jQuery('.IdentificacaoUsuario').hide();
    removerValidacoesParticipantes();
}

function makeTablePaciente(numero,nome, cns, idusr, idusrprocedimento) {
    var strTable = '';
    strTable += '   <tr id="' + idusr + '" class="sel-cns" data-nome="' + nome.toCapitalize() + '" data-cns="' + cns + '" data-idusr="' + idusr + '" data-idusrprocedimento="' + idusrprocedimento + '" >';
    strTable += '       <td class="text-center">' + numero + '</td>';
    strTable += '       <td class="text-capitalize">' + nome.toCapitalize() + '</td>';
    strTable += '       <td class="text-center">' + cns + '</td>';
    strTable += '       <td class="text-center"><button type="button" class="btn btn-danger" title="Remover"><span class="glyphicon glyphicon-remove"></span></button></td>';
    strTable += "       <td class='text-center'><button type='button' class='btn btn-primary preencherDadosPaciente' title='Preencher Ficha'><span class='glyphicon glyphicon-edit'></span></button></td>";
    strTable += '   </tr>';
    $('#listaProcedimento tbody').append(strTable);
    $('#msgErroTableUsu').text('');
    removeGridPacientes(idusrprocedimento);
    desabilitaBuscaPaciente();
    habilitaFichaPaciente();
    $('#modalProfSus').modal('hide');
}

function makeTableProfissional(nome, cns, idprof, idprocedimento, cbo) {
    var strTable = "";
    strTable += "   <tr id='" + idprof + "' class='sel-cns' data-nome='" + nome.toCapitalize() + "' data-cns='" + cns + "' data-idprof='" + idprof + "' data-idprocedimento='" + idprocedimento + "' data-cbo='" + cbo + "'>";
    strTable += "       <td class='text-capitalize'>" + nome.toCapitalize() + "</td>";
    strTable += "       <td class='text-center'>" + cns + "</td>";
    strTable += "       <td class='text-center'>" + cbo + "</td>";
    strTable += "       <td class='text-center'><button type='button' class='btn btn-success' title='Substituir'><span class='glyphicon glyphicon-refresh'></span></button></td>";
    strTable += "   </tr>";
    removeGridProfissionais();
    $("#listaProfissionaisProcedimento tbody").append(strTable);
    $('#msgErroTableProf').text('');    
    substituiProfSus();
    desabilitaBuscaProfissional();
    $("#modalProfSus").modal("hide");
}

function removeGridProfissionais() {
    //$('#listaProfissionaisProcedimento tbody .btn-danger').click(function () {
    var tr = $('#listaProfissionaisProcedimento tbody tr');
    var codigoCnesUnidade = $('#selCnesUni :selected').val();
    var codigoEquipeIne = ($('#selIneEquipe').val() == "") ? 0 : $('#selIneEquipe').val();
    $(tr).remove();
    //removerProfissionalProcedimento(codigoCnesUnidade, codigoEquipeIne, null);
    desabilitaBuscaProfissional();
 //   });
}

function desabilitaBuscaProfissional() {
    if ($('#listaProfissionaisProcedimento tbody tr').length > 0) {
        $('#btnBuscarProf').attr('disabled', 'disabled');
    } else {
        $('#btnBuscarProf').removeAttr('disabled');
    }
}

function removeGridPacientes(idusrprocedimento) {
    $('#listaProcedimento tbody .btn-danger').click(function () {
        var tr = $(this).closest("tr");
        removerPacienteProcedimento($(tr).data("idusrprocedimento"), $(tr).data("idusr"), tr);
    });
}

function desabilitaBuscaPaciente() {
    if ($('#listaProcedimento tbody tr').length > 22) {
        $('#btnBuscarUsu').attr('disabled', 'disabled');
        $('#btnAddSemCadastro').attr('disabled', 'disabled');
    } else {
        $('#btnAddSemCadastro').removeAttr('disabled');
        $('#btnBuscarUsu').removeAttr('disabled');
    }
}

function habilitaFichaPaciente() {
    jQuery("#listaProcedimento tbody .preencherDadosPaciente").on("click", function () {
        limparFormProcedimentoUsuario();
        $('#listaSIGTAP tbody').empty();
        limparCampos(".IdentificacaoUsuario");
        var idusrprocedimento = jQuery(this).closest("tr").data("idusrprocedimento");
        var idusr = jQuery(this).closest("tr").data("idusr");
        var id = jQuery('.IdentificacaoUsuario').data("id");

        jQuery("#usuarioAtivo").val(idusrprocedimento);
        jQuery("#idIdentificacaoUsuario").val(idusr);

        jQuery("#listaProcedimento tbody tr ").removeClass("active");

        if ($("#listaProcedimento tbody tr ").length > 1) {
            jQuery(this).closest("tr").addClass("active");
        }

        if (idusrprocedimento != id) {
            limparFormProcedimentoUsuario();
        }

        jQuery('.IdentificacaoUsuario').show().data("id", idusrprocedimento);
        getFichaPaciente(idusrprocedimento);
        getExamesPacienteProcedimentos();
        jQuery('#btnConfirmar').show();
        jQuery('#btnCancelar').show();
        jQuery('#btnFinalizarProcedimento').show();
    });
}

function getFichaPaciente(idUsrProcedimento) {
    jQuery.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getUsuarioProcedimentoById', idProcedimento: idUsrProcedimento },
        dataType: "json",
        url: Api.URL
    })
    .done(function (data) {
        setFormData(data[0]);
    });
}

function getProfissionais() {
    jQuery.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getProfissionalByIdProcedimento', idProcedimento: jQuery("#idProcedimento").val() },
        dataType: "json",
        url: Api.URL
    })
    .done(function (data) {
        if (data.length > 0) {
            jQuery("#listaProfissionaisProcedimento tbody").empty();


            setFormData(data[0]);

            jQuery.each(data, function (ResultadoItens, item) {
                makeTableProfissional(item.Nome, item.CNS, item.IdProfissionalSaude, item.IdProcedimento, item.Cbo);
            });
        }

        jQuery("#checkDadosProfissionais").hide();
        jQuery('#dadosProfissionais').show();
    });
}

function getProcedimentosConsolidados() {
    jQuery.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getProcedimentosConsolidados', idProcedimento: jQuery("#idProcedimento").val() },
        dataType: "json",
        url: Api.URL
    })
    .done(function (data) {
        if (data.length > 0) {

            setFormData(data[0]);

        }

        jQuery("#checkDadosProcedimentos").hide();
        jQuery('#dadosProcedimentos').show();
    });
}

function getPacientes() {
    jQuery.ajax({
        type: "POST",
        data: { acao: 'getUsuarioByIdProcedimento', idProcedimento: jQuery("#idProcedimento").val() },
        dataType: "json",
        url: Api.URL
    })
    .done(function (data) {
        if (data.status) {
            jQuery('#listaProcedimento tbody').empty();
            jQuery.each(data.resultado, function (ResultadoItens, item) {
                makeTablePaciente(item.Numero,item.Nome, item.CNS, item.IdUsr, item.IdUsrProcedimento);
            });
        }
    });
}

function getDadosProcedimento() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getById', id: jQuery("#idProcedimento").val() },
        dataType: "json",
        url: Api.URL
    })
    .done(function (data) {
        if (data.status) {

            if (data.resultado.codigo_cnes_unidade > 0) {
                $('#selCnesUni').val(data.resultado.codigo_cnes_unidade).prop('disabled', 'disabled');
            }
            makeEquipeIne(data.resultado.codigo_cnes_unidade, data.resultado.codigo_equipe_ine);
            var dataProcedimento = formatarDataParaExibicao(data.resultado.data_procedimento);
            $('#txtData').val(dataProcedimento);
        }
    });
}

function getExamesPacienteProcedimentos() {
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'getExamesPacienteByIdProcedimento',
            idProcedimento: jQuery("#idProcedimento").val(),
            id_Procedimento_Usuario: $('#usuarioAtivo').val()
        },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            $('#listaSIGTAP tbody').empty();
            $.each(obj.resultado, function (ResultadoItens, item) {
                makeTableExamesPaciente(item.Descricao, item.id);
            });
        }
    });
}

function getSigtap() {
    var filtro = jQuery("#modalBusca").val();

    jQuery('#listaModalSigtap tbody').empty();
    jQuery('#listaModalSigtap tbody').append('<tr><th colspan="2" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');

    postAjax({ codigo: filtro, nome: filtro, acao: "getSigtap" })
    .done(function (data) {
        jQuery('#listaModalSigtap tbody').empty();
        jQuery.each(data, function (ResultadoItens, item) {
            var strTable = '<tr id="' + item.Codigo.toUpperCase() + '" class="sel-sigtap" data-codigo="' + item.Codigo.toUpperCase() + '" data-codproc="' + item.CodProc + '" data-nome="' + item.Nome.toUpperCase() + '" >';
            strTable += '<td class="text-capitalize">' + item.Codigo.toUpperCase() + '</td>';
            strTable += '<td class="text-capitalize">' + item.Nome.toUpperCase() + '</td>';
            strTable += '</tr>';
            jQuery('#listaModalSigtap tbody').append(strTable);
        });

        executaSelect();
    });

    function executaSelect() {
        jQuery("#listaModalSigtap tbody .sel-sigtap").click(function () {
            //Remove o active dos outros
            jQuery(this).siblings().removeClass("list-group-item-success");
            if ($('#listaSIGTAP .sel-cns').length >= 6) {
                jQuery("#modalSigtap").modal("hide");
                modalAlerta("Atenção", "Não pode haver mais de 6 procedimentos cadastrados");
            } else {
                var codigo = jQuery(this).data("codproc");
                var nome = jQuery(this).data("nome");
                var idProcedimento = jQuery("#idProcedimento").val();

                if (!jQuery("#" + codigo).hasClass("sel-cns")) {
                    var idExame = salvarExamesPacienteProcedimento(idProcedimento, codigo);
                    if (idExame > 0) {
                        makeTableExamesPaciente(nome.trim(), codigo);
                    }
                } else {
                    $('.msgErroDuplicado').text('O Paciente selecionado já foi inserido no Procedimento e não poderá ser inserido novamente.');
                }
            }
        });
    }
    return false;
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
    $('#listaProfissionaisProcedimento tbody .btn-success').on("click", function () {
        busca_Prof_Sus();
    });
}

function pacienteNovamente() {

    $('#modalSemCartaoSus').modal('hide');
    busca_Paciente_Sus();

}

function salvarSemCadastroProcedimento() {
    var idFicha = $("#frmUsuarioIdProcedimento").val();
    var id = criaPacienteProcedimento(idFicha, '21525961');
    if (id != false) {
        getPacientes();
    }
}

function addSemCadastroProcedimento() {
    if ($('#txtmodalBuscaCartaoSus').val() != "") {
        if ($('#txtmodalBuscaCartaoSus').val() == $('#txtmodalConfirmaCartaoSus').val()) {
            salvarSemCadastroProcedimento($('#txtmodalConfirmaCartaoSus').val());
        }
        else {
            $('#modalSemCartaoSus .msgErro .col-md-12').text('O cartão do SUS não confere com o digitado anteriormente.Favor digite novamente.');
        }
    }
    else {
        $('#txtmodalBuscaCartaoSus').val('');
        $('#txtmodalConfirmaCartaoSus').val('');
        salvarSemCadastroProcedimento($('#txtmodalConfirmaCartaoSus').val());
    }

}
