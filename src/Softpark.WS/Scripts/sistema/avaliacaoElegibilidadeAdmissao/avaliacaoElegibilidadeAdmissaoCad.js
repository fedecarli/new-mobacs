var Api = { URL: "ajax/AvaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp", getPaciente: "ajax/atendIndividual/getPaciente.asp" };

jQuery(document).ready(function () {
    //Mascara
    jQuery("#txtCep").mask("99999-999");
    jQuery("#txtTelefoneResidencial").mask("(99) 9999-9999?9");
    jQuery("#txtTelefoneReferencia").mask("(99) 99999-999?9");

    //Modal
    jQuery('#modalProfSus').on('shown.bs.modal', function () {
        jQuery('#txtModalBusca').focus();
    });
    //Evento Click
    var verificacao = true;
    $('#btnBuscarProf').on('click', busca_Prof_Sus);
    jQuery('#btnCancelar').on('click', limparFormAtendimentoUsuario);
    jQuery('#btnSalvar').on('click', function () {
        verificacao = true;
        $('.msgErro').html('');
        if (!verificarItensnaTable()) { verificacao = false; };
        if (!validacaoCampo('#txtCartaoSUS', 'CNS')) { verificacao = false; };
        if (!validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade')) { verificacao = false; };
        if (!validacaoCampo('[name="rdbSexo"]', 'Sexo')) { verificacao = false; };
        if (!validacaoCampoNIS('#txtNis')) { verificacao = false; };
        if (!validacaoCampo('#txtEmail', 'Email')) { verificacao = false; };
        if ($('[name=rdbElegivel]:checked').val()==1) {
            if (!validacaoCampoNomeCidadao('#txtNomeCompleto')) { verificacao = false; };
            if (!$('[name=rdbRaca]:checked').val()) {
                $('[name=rdbRaca]').parents('div:first').find('.msgErro').html('O Campo Raça é obrigatório');
                verificacao = false;
            }
            if ($('#chkNomeMaeDesconhecido').prop('checked') == false) {
                if (!validacaoCampoNomeCidadao('#txtNomeMae')) { verificacao = false; };
            }
            if (!$('[name=rdbNacionalidade]:checked').val()) {
                $('[name=rdbNacionalidade]').parents('div:first').find('.msgErro').html('O Campo Nacionalidade é obrigatório');
                verificacao = false;
            }
            if ($('#txtCep').val().length == 9) {
                if ($('#selTipoLogradouro').val() == '') {
                    $('#selTipoLogradouro').removeProp('disabled').parents('div:first').find('.msgErro').html('O Campo Tipo Logradouro é obrigatório');
                    verificacao = false;
                }
                if ($('#selUf').val() == '') {
                    $('#selUf').removeProp('disabled').parents('div:first').find('.msgErro').html('O Campo UF é obrigatório');
                    verificacao = false;
                }
                if ($('#selMunicipio').val() == '') {
                    $('#selMunicipio').removeProp('disabled').parents('div:first').find('.msgErro').html('O Campo Município é obrigatório');
                    verificacao = false;
                }
                if ($('#txtNomeLogradouro').val() == '') {
                    $('#txtNomeLogradouro').removeProp('disabled').parents('div:first').find('.msgErro').html('O Campo Nome Logradouro é obrigatório');
                    verificacao = false;
                }
                if ($('#txtBairro').val() == '') {
                    $('#txtBairro').removeProp('disabled').parents('div:first').find('.msgErro').html('O Campo Bairro é obrigatório');
                    verificacao = false;
                }
                if (!$('#chkSemNumero').checked) {
                    if ($('#txtNumero').val() == '') {
                        $('#txtNumero').removeProp('disabled').parents('div:first').find('.msgErro').html('O Campo Nº é obrigatório');
                        verificacao = false;
                    }
                }
            } else {
                $('#txtCep').parents('fieldset:first').next().html('Preencha o Campo CEP');
                verificacao = false;
            }
        }
        if ($('#selCid1').val() == '') {
            $('#selCid1').parents('div:first').find('.msgErro').html('O Campo CID (Principal) é obrigatório');
            verificacao = false;
        }
        if ($('#selCid1').val() == $('#selCid2').val() && $('#selCid2').val() != '') {
            $('#selCid2').parents('div:first').find('.msgErro').html('O Campo CID (secundário 1) não pode ser igual ao CID (Principal)');
            verificacao = false;
        }
        if ($('#selCid1').val() == $('#selCid3').val() && $('#selCid3').val() != '') {
            $('#selCid3').parents('div:first').find('.msgErro').html('O Campo CID (secundário 2) não pode ser igual ao CID (Principal)');
            verificacao = false;
        }
        if ($('#selCid2').val() == $('#selCid3').val() && $('#selCid3').val() != '') {
            $('#selCid3').parents('div:first').find('.msgErro').html('O Campo CID (secundário 2) não pode ser igual ao CID (secundário 1)');
            verificacao = false;
        }
        if (!$('[name=rdbConclusao]:checked').val()) {
            $('[name=rdbConclusao]').parents('div:first').find('.msgErro').html('O Campo Conclusão é obrigatório');
            verificacao = false;
        } else {
            if ($('[name=rdbConclusao]:checked').val() != 4) {
                if (!$('[name=rdbElegivel]:checked').val()) {
                    $('[name=rdbElegivel]').parents('div:first').find('.msgErro').html('O Campo Elegível é obrigatório');
                    verificacao = false;
                }
            } else {
                if (!$('[name="chkInelegivel[]"]:checked').length>0) {
                    $('[name="chkInelegivel[]"]').parents('div:first').find('.msgErro').html('O Campo Inelegível é obrigatório');
                    verificacao = false;
                }
            }
        }
        //$(campoSeletor).parent().find('.msgErro').html('O Campo Nº Cartão Sus / CNS está incompleto');
        //if (verificacao) {
            $('#fichaAvaliacaoElegitibilidade').submit();
        //}
    });
    //VALIDAÇÕES DO DICIONARIO DE DADOS DO CDS
    $("#txtCartaoSUS")      .on('blur', function () { validacaoCampo('#txtCartaoSUS', 'CNS'); });
    $("#txtDataNascimento") .on('blur', function () { validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade'); });
    $("#txtNomeCompleto").on('blur', function () { validacaoCampoNomeCidadao('#txtNomeCompleto'); });
    $("#txtNomeMae").on('blur', function () {
        if ($('#chkNomeMaeDesconhecido').prop('checked') == false) {
            validacaoCampoNomeCidadao('#txtNomeMae');
        }
    });
    $("#txtNis").on('blur', function () {
        validacaoCampoNIS('#txtNis');
    });
    $("#txtEmail")          .on('blur', function () { validacaoCampo('#txtEmail', 'Email'); });

    function validacaoCampoNIS(campoSeletor) {
        var validaCampoNIS = true;
        $(campoSeletor).parents('div:first').find('.msgErro').html('');
        if ($(campoSeletor).val()!='' && $(campoSeletor).val().length < 11) {
            $(campoSeletor).parents('div:first').find('.msgErro').html('O Campo NIS tem que ter 11 dígitos<br>');
            validaCampoNIS= false;
        }
        return validaCampoNIS;
    }

    function validacaoCampoNomeCidadao(campoSeletor) {
        var validaCampoNomeCidadaoVerificacao = true;
        //var campoSeletor = '#txtNomeCompleto';
        var valor = $(campoSeletor).val();

        $(campoSeletor).parents('div:first').find('.msgErro').html('');

        if (valor.length < 5) {
            $(campoSeletor).parents('div:first').find('.msgErro').html('O Campo Nome tem que ter no mínimo 5 dígitos<br>');
            validaCampoNomeCidadaoVerificacao = false;
        }
        if (valor.split(' ').length < 2) {
            if ($(campoSeletor).parents('div:first').find('.msgErro').html().length > 0) {
                $(campoSeletor).parents('div:first').find('.msgErro').append(' e ');
            } else {
                $(campoSeletor).parents('div:first').find('.msgErro').html('O Campo Nome tem que ');
            }
            $(campoSeletor).parents('div:first').find('.msgErro').append('ter no mínimo Nome e 1 Sobrenome');
            validaCampoNomeCidadaoVerificacao = false;
        }
        return validaCampoNomeCidadaoVerificacao;
    }

    $('#chkNomeMaeDesconhecido').on('change', function () {
        if (this.checked) {
            $("#txtNomeMae").val('').prop('disabled', 'disabled');
        } else {
            $("#txtNomeMae").removeProp('disabled');
        }
    });

    $('[name=rdbNacionalidade]').on('change', function () {
        if ($('[name=rdbNacionalidade]:checked').val()!=1) {
            $("#selMunicipioUFNascimento").val('');
            $("#btnBuscarUf").prop('disabled', 'disabled');
        } else {
            $("#btnBuscarUf").removeProp('disabled');
        }
    });
    
    jQuery('#btnNovoCartaoSus').on('click', addSemCadastroAvaliacao);

    $('#selCnesUni').on('change', CarregarEquipeIne);
    $('[name=rdbConclusao]').on('click', ElegivelOuInelegivel);
    $('[name=rdbElegivel]').on('click', ObrigaSeElegivel);
    //Get dos Dados - Carregamento da Tela
    getDadosAvaliacao();
    getProfissionais();
    getPacientes();

    jQuery("#txtNumero").on("blur", function () {
        if ($("#txtNumero").val() == ""){
            jQuery("#chkSemNumero").prop("checked", true);
        } else {
            jQuery("#chkSemNumero").prop("checked", false);
        }
    });
    //Evento Saída do Foco
    /*jQuery("#txtNomeMae").on("blur", function () {
        if ($("#txtNomeMae").val() == "")
        {
           jQuery("#chkNomeMaeDesconhecido").prop("checked", true);            
        }else
        {
            jQuery("#chkNomeMaeDesconhecido").prop("checked", false);
        }
    });*/

    //Validate
    jQuery("#fichaAvaliacaoElegitibilidade").validate({
        errorPlacement: function (error, element) {
            error.appendTo(jQuery(element).closest("div").find("div.msgErro"));
        },
        rules: {
            selCnesUni:         {required: true},
            selIneEquipe:       {required: true, min:0},
            txtData:            {required: true},
            txtCartaoSUS:       {required: true},
            //txtDataNascimento:  {required: true},
            //rdbSexo:            {required: true},
            selLocalAtend:      {required: true},
            //selCid1:            {required: true},
            //rdbConclusao:       {required: true}
        },
        messages: {
            selCnesUni:         "CNES é obrigatório ",
            selIneEquipe:       "INE é Obrigatório",
            txtData:            "Data é obrigatória ",
            txtCartaoSUS:       "Nº cartão SUS é obrigatório",
            //txtDataNascimento:  "Data de Nascimento é obrigatória",
            //rdbSexo:            "Sexo é obrigatório",
            selLocalAtend:      "Local de atendimento é obrigatório",
            //selCid1:            "CID é obrigatório",
            //rdbConclusao:       "Conclusão é obrigatório"
        },
        submitHandler: function (form) {
            if (verificacao) {
                alteraPacienteAtendimento();
            }
        }
    });

    $("#btnBuscaCID").on("click", CID);
});

function busca_cep() {

    xCep = $('#txtCep').val();
    xCep = xCep.replace(/_/g, "");
    xCep = xCep.replace("-", "");

    if (xCep.length == 8) {

        $('#txtCep').attr('disabled', 'disabled');

        $("#selTipoLogradouro").val("");
        $("#txtNomeLogradouro").val("");
        $("#txtBairro").val("");
        $("#selUf").val("");
        $("#selMunicipio").val("");

        if (xCep == "99999999") {

            $("label[for=txtCep]").html('CEP *');
            $("#txtCep").removeClass('obg');
            $("#txtCep").addClass('obg');
            $("#txtCep").removeAttr('disabled');

            $("#selTipoLogradouro").val("081");
            $("#selTipoLogradouro").attr('disabled', 'disabled');

            $("#txtNomeLogradouro").val("SEM INFORMACAO");
            $("#txtNomeLogradouro").attr('disabled', 'disabled');

            $('#txtNumero').val("0");
            $("#txtNumero").attr('disabled', 'disabled');

            $("#txtBairro").val("SEM INFORMACAO");
            $("#txtBairro").attr('disabled', 'disabled');

            $("#selUf").val("SI");
            $("#selUf").attr('disabled', 'disabled');
            $("#selMunicipio").removeAttr('disabled');

        } else if (xCep == "14900000") {

            $("label[for=txtCepEnd]").html('CEP *');
            $("#txtCepEnd").removeClass('obg');
            $("#txtCepEnd").addClass('obg');
            $("#txtCepEnd").removeAttr('disabled');

            $("#selTipoLogradouro").val('');
            $("#selTipoLogradouro").removeAttr('disabled');

            $("#txtNomeLogradouro").val('');
            $("#txtNomeLogradouro").removeAttr('disabled');

            $('#txtNumero').val('');
            $("#txtNumero").removeAttr('disabled');

            $("#txtBairro").val('');
            $("#txtBairro").removeAttr('disabled');

            $("#selUf").val('SP');
            $("#selMunicipio").removeAttr('disabled');

        } else if (xCep == "11740000") {

            $("label[for=txtCep]").html('CEP *');
            $("#txtCep").removeClass('obg');
            $("#txtCep").addClass('obg');
            $("#txtCep").removeAttr('disabled');

            $("#selTipoLogradouro").val('');
            $("#selTipoLogradouro").removeAttr('disabled');

            $("#txtNomeLogradouro").val('');
            $("#txtNomeLogradouro").removeAttr('disabled');

            $('#txtNumero').val('');
            $("#txtNumero").removeAttr('disabled');

            $("#txtBairro").val('');
            $("#txtBairro").removeAttr('disabled');

            $("#selUf").val('SP');
            $("#selMunicipio").removeAttr('disabled');

        } else {
            $.ajax({
                type: "POST",
                data: { cep: xCep },
                datatype: "json",
                url: "ajax/cep/default.asp"
            })
            .done(function (data) {
                if (data.status) {

                    $.each(data.resultado, function (ResultadoItens, item) {

                        $("label[for=txtCep]").html('CEP *');
                        $("#txtCep").removeClass('obg');
                        $("#txtCep").addClass('obg');
                        $("#txtCep").prop('disabled', '');

                        $("#selTipoLogradouro").val(item.idTipo);
                        $("#selTipoLogradouro").attr('disabled', 'disabled');

                        $("#txtNomeLogradouro").val(item.logradouro);
                        $("#txtNomeLogradouro").attr('disabled', 'disabled');

                        $("#txtBairro").val(item.bairro);
                        $("#txtBairro").attr('disabled', 'disabled');

                        $("#selUf").val(item.uf);
                        $("#selUf").attr('disabled', 'disabled');

                        $("#selMunicipio").val(item.idmunicipio);
                        $("#selMunicipio").attr('disabled', 'disabled');

                        $('#txtNumero').focus();
                    });

                    $('#txtCep').removeAttr('disabled');

                } else {

                    if (data.sessao) {

                        setTimeout(function () {
                            modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                            setTimeout(function () {
                                window.location.assign("login.asp");
                            }, 2000);
                        }, 800);

                    } else {

                        $('#txtCep').val('');
                        $("#selTipoLogradouro").val('');
                        $("#txtNomeLogradouro").val('');
                        $("#txtNumero").val('');
                        $("#txtBairro").val('');
                        $("#selUf").val('');
                        $("#selMunicipio").val('');

                        $('#modalEndereco').find('input[type=text], select').each(function () {
                            $(this).val('');
                            $(this).parents(".form-group").removeClass("has-error");
                        });

                        $('#listaModalEndereco tbody').empty();
                        $('#listaModalEndereco tbody').append('<tr><th colspan="6" class="text-center">Cep não encontrado! <br> Efetue a busca acima</th></tr>');
                        $('#modalEndereco').modal('show');

                        $('#txtCep').removeAttr('disabled');
                    }
                }
            });
        }
    }

}

function busca_endereco() {

    $("#selTipoLogradouro").val('');
    $("#txtCep").val('');
    $("#txtNomeLogradouro").val('');
    $("#txtNumero").val('');
    $("#txtBairro").val('');
    $("#selUf").val('');
    $("#selMunicipio").val('');

    $('#modalEndereco').find('input[type=text], select').each(function () {
        $(this).val('');
        $(this).parents(".form-group").removeClass("has-error");
    });

    $("#txtModalMunicipioEnd").val(nomeCliente);
    $("#txtModalUfEnd").val('SP');
    $('#listaModalEndereco tbody').empty();
    $('#listaModalEndereco tbody').append('<tr><th colspan="6" class="text-center">Efetue a busca acima</th></tr>');
    $('#modalEndereco').modal('show');

}

function busca_municipio() {

    $("#selMunicipioUFNascimento").val('');

    $('#modalMunicipio').find('input[type=text], select').each(function () {
        $(this).val('');
        $(this).parents(".form-group").removeClass("has-error");
    });

    $("#txtModalMunicipioUf").val(nomeCliente);
    $("#txtModalUf").val('SP');
    $('#listaModalMunicipio tbody').empty();
    $('#listaModalMunicipio tbody').append('<tr><th colspan="6" class="text-center">Efetue a busca acima</th></tr>');
    $('#modalMunicipio').modal('show');

}

function tpBuscaCID() {
    if ($("#selModalTpBuscaCID").val() == 1) {
        $("#txtModalBuscaCID").val('');
        $("label[for=txtModalBuscaCID]").html('Código *');
        $("#txtModalBuscaCID").attr('placeholder', 'Código');
    } else {
        $("#txtModalBuscaCID").val('');
        $("label[for=txtModalBuscaCID]").html('Descricao *');
        $("#txtModalBuscaCID").attr('placeholder', 'Descricao');
    }
}

function busca_CID(event) {
    $('.msgErroDuplicado').text('');
    jQuery(event.currentTarget).closest(".txtClicado").find("input[type='text']").val('');
    $("#btnBuscaCID").data("botao", event.currentTarget.id);

    $('#modalCID').find('input[type=text], select').each(function () {
        jQuery("#selModalTpBuscaCID").val('1');
        jQuery("#txtModalBuscaCID").val('');
        $(this).parents(".form-group").removeClass("has-error");
    });

    $("#txtModalBuscaCID").val(nomeCliente);    
    $('#listaModalCID tbody').empty();
    $('#listaModalCID tbody').append('<tr><th colspan="6" class="text-center">Efetue a busca acima</th></tr>');
    $('#modalCID').modal('show');
}

function CID(event) {
    var botaoClicado = $(event.currentTarget).data("botao");
    if (validaCampos($("#modalCID"), 2, "alertaSemModalCID")) {
        var xTipoBusca = $('#selModalTpBuscaCID :selected').val();
        var xDescricao = removerAcentos($('#txtModalBuscaCID').val().trim());
        $('#listaModalCID tbody').empty();
        $('#listaModalCID tbody').append('<tr><th colspan="6" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');

        $.ajax({
            type: "POST",
            data: { acao: 'getCIDById', descricao: xDescricao, tipoBusca: xTipoBusca },
            datatype: "json",
            url: "ajax/AvaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp"
        })
        .done(function (data) {
            var obj = JSON.parse(data);
            $('#listaModalCID tbody').empty();
            if (obj.status) {
                $.each(obj.resultado, function (ResultadoItens, item) {
                    var strTable = '<tr class="sel-cep"  data-codigo="' + item.codigo + '" data-descricao="' + item.descricao + '" >';
                    strTable += '<td class="text-center">' + item.codigo.toUpperCase() + '</td>';
                    strTable += '<td class="text-capitalize">' + item.descricao.toUpperCase() + '</td>';
                    strTable += '</tr>';
                    $('#listaModalCID tbody').append(strTable);
                });
                executaSelect(botaoClicado);
            } else {
                if (obj.sessao) {
                    setTimeout(function () {
                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                        setTimeout(function () {
                            window.location.assign("login.asp");
                        }, 2000);
                    }, 800);
                } else {

                    $('#listaModalCID tbody').append('<tr><th colspan="6" class="text-center">Nenhum resultado encontrado!</th></tr>');
                }
            }
        });
    }

    function executaSelect(botaoClicado) {
        $('#listaModalCID tbody .sel-cep').click(function () {            
            var valorPesquisaCID = $(this).data('codigo') + " - " + $(this).data('descricao');
            if (valorPesquisaCID != $("#selCid1").val() && valorPesquisaCID != $("#selCid2").val() && valorPesquisaCID != $("#selCid3").val())
            {
                var valorCID = jQuery("#" + botaoClicado).closest(".txtClicado").find("input[type='text']").val($(this).data('codigo') + " - " + $(this).data('descricao'));
                $(this).siblings().removeClass('list-group-item-success');
                jQuery(valorCID).attr('disabled', 'disabled');
                $('#modalCID').modal('hide');
            }else
            {
                $('.msgErroDuplicado').text('O CID selecionado já foi informado na Avaliação e nao poderá ser informado novamente.');
            }
        });
    }
}

function endereco() {

    if (validaCampos($("#modalEndereco"), 2, "alertaSemModalEndereco")) {
        var xEndereco = removerAcentos($('#txtModalLogradouro').val().trim());
        var xMunicipio = removerAcentos($('#txtModalMunicipioEnd').val().trim());
        var xUf = $('#txtModalUfEnd').val();

        $('#listaModalEndereco tbody').empty();
        $('#listaModalEndereco tbody').append('<tr><th colspan="6" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');

        $.ajax({
            type: "POST",
            data: { endereco: xEndereco, municipio: xMunicipio, uf: xUf },
            datatype: "json",
            url: "ajax/cep/default.asp"
        })
        .done(function (data) {

            $('#listaModalEndereco tbody').empty();

            if (data.status) {

                $.each(data.resultado, function (ResultadoItens, item) {

                    var strTable = '<tr class="sel-cep" data-cep="' + item.cep + '" data-tipo="' + item.idTipo + '" data-logradouro="' + item.logradouro + '" data-bairro="' + item.bairro + '" data-municipio="' + item.municipio +  '" data-idmunicipio="' + item.idmunicipio + '" data-uf="' + item.uf + '" >';
                    strTable += '<td class="text-center">' + item.cep + '</td>';
                    strTable += '<td class="text-capitalize">' + item.tipo.toLowerCase() + '</td>';
                    strTable += '<td class="text-capitalize">' + item.logradouro.toLowerCase() + '</td>';
                    strTable += '<td class="text-capitalize">' + item.bairro.toLowerCase() + '</td>';
                    strTable += '<td class="text-capitalize">' + item.municipio.toLowerCase() + '</td>';
                    strTable += '<td class="text-center">' + item.uf + '</td>';
                    strTable += '</tr>';

                    $('#listaModalEndereco tbody').append(strTable);
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

                    $('#listaModalEndereco tbody').append('<tr><th colspan="6" class="text-center">Nenhum resultado encontrado!</th></tr>');

                    $('#modalEndereco').modal('hide');
                    $('#modalSemEndereco').modal('show');
                }
            }
        });

    }

    //Function que habilita o click nos resultados da busca do Endereço
    function executaSelect() {
        $('#listaModalEndereco tbody .sel-cep').click(function () {
            //Remove o active dos outros
            $(this).siblings().removeClass('list-group-item-success');

            $("label[for=txtCep]").html('CEP *');
            $("#txtCep").removeClass('obg');
            $("#txtCep").addClass('obg');
            $("#txtCep").val($(this).data('cep'));

            $("#selTipoLogradouro").val($(this).data('tipo'));
            $("#selTipoLogradouro").attr('disabled', 'disabled');

            $("#txtNomeLogradouro").val($(this).data('logradouro'));
            $("#txtNomeLogradouro").attr('disabled', 'disabled');

            $("#txtBairro").val($(this).data('bairro'));
            $("#txtBairro").attr('disabled', 'disabled');

            $("#selUf").val($(this).data('uf'));
            $("#selUf").attr('disabled', 'disabled');

            $("#selMunicipio").val($(this).data('idmunicipio'));
            $("#selMunicipio").attr('disabled', 'disabled');

            $('#modalEndereco').modal('hide');

            $('#txtNumero').focus();
        });
    }

}

function municipio() {

    if (validaCampos($("#modalMunicipio"), 2, "alertaSemModalMunicipio")) {
        var xMunicipio = removerAcentos($('#txtModalMunicipioUf').val().trim());
        var xUf = $('#txtModalUf').val();

        $('#listaModalMunicipio tbody').empty();
        $('#listaModalMunicipio tbody').append('<tr><th colspan="6" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');

        $.ajax({
            type: "POST",
            data: { municipio: xMunicipio, uf: xUf },
            datatype: "json",
            url: "ajax/AvaliacaoElegibilidadeAdmissao/getMunicipioUf.asp"
        })
        .done(function (data) {
                      

            $('#listaModalMunicipio tbody').empty();

            if (data.status) {

                $.each(data.resultado, function (ResultadoItens, item) {

                    var strTable = '<tr class="sel-cep"  data-municipio="' + item.municipio + '" data-idmunicipio="' + item.idmunicipio + '" data-uf="' + item.uf + '" >';
                    strTable += '<td class="text-capitalize">' + item.municipio.toLowerCase() + '</td>';
                    strTable += '<td class="text-center">' + item.uf + '</td>';
                    strTable += '</tr>';

                    $('#listaModalMunicipio tbody').append(strTable);
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

                    $('#listaModalMunicipio tbody').append('<tr><th colspan="6" class="text-center">Nenhum resultado encontrado!</th></tr>');

                }
            }
        });

    }

    //Function que habilita o click nos resultados da busca do Endereço
    function executaSelect() {
        $('#listaModalMunicipio tbody .sel-cep').click(function () {
            $(this).siblings().removeClass('list-group-item-success');

            $("#selMunicipioUFNascimento").val($(this).data('municipio') + " - " + $(this).data('uf'));
            $("#selMunicipioUFNascimento").attr('disabled', 'disabled');

            $('#modalMunicipio').modal('hide');

            $('#txtEmail').focus();
        });
    }

}

function enderecoNovamente() {

    $('#modalSemEndereco').modal('hide');
    $('#modalEndereco').modal('show');

}

function addEndereco() {

    $("label[for=txtCep]").html('CEP');
    $("#txtCep").removeClass('obg');
    $("#txtCep").removeAttr('disabled');
    $("#txtCep").val('');

    $("#selTipoLogradouro").val('');
    $("#selTipoLogradouro").removeAttr('disabled');

    $("#txtLogradouro").val(removerAcentos($('#txtModalLogradouro').val().trim()).toUpperCase());

    $("#txtNumero").removeAttr('disabled');

    $("#txtBairro").val('');
    $("#txtBairro").removeAttr('disabled');

    $("#txtUf").val($('#txtModalUfEnd').val());

    $("#selMunicipio").val(removerAcentos($('#txtModalMunicipioEnd').text().trim()).toUpperCase());

    $('#modalSemEndereco').modal('hide');
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

function busca_Paciente_Sus() {
    jQuery('#txtmodalBuscaCartaoSus').val('');
    jQuery('#txtmodalConfirmaCartaoSus').val('');
    limparFormAtendimentoUsuario();
    jQuery('#btnFinalizarAtend').hide();
    jQuery('.IdentificacaoUsuario').hide();
    jQuery("#listaAtendimentoIndividual tbody tr ").removeClass("active");
    jQuery("#btnConfirmar").hide();
    jQuery("#btnCancelar").hide();

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

function busca_Prof_Sus(event) {
    if (event.currentTarget.id == "btnBuscarProf") {
        $('#modalProfSus').addClass('add');
    } else {
        $('#modalProfSus').removeClass('add');
    }
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
            var idAvaliacao = $("#idAvaliacaoElegibilidade").val();
            var idPacienteAvaliacao = criaPacienteAvaliacao(idAvaliacao, $(this).data("idusr"), $(this).data('cns'));
            if (idPacienteAvaliacao != false) {
                makeTablePaciente($(this).data('nome'), $(this).data('cns'), $(this).data('idusr'), idPacienteAvaliacao)
            } else {
                modalAlerta("Atenção", "Falha ao adicionar o Paciente na Avaliacao, tente novamente mais tarde.");
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
                var idAvaliacao = jQuery("#idAvaliacaoElegibilidade").val();
                var add = $('#modalProfSus').hasClass('add');
                if (add) {
                    if (!jQuery("#" + $(this).data("idprof")).hasClass("sel-cns")) {
                        var idProfissionalAvaliacao = salvaProfissionalAvaliacao(idAvaliacao, $(this).data("idprof"), $(this).data("cbo"));
                        if (idProfissionalAvaliacao != false) {
                            $("#listaProfissionaisAtendimento tbody tr button.btn-danger").show();
                            $("#listaProfissionaisAtendimento tbody tr button.btn-success").hide();
                            var unico = (linhas == 0);
                            makeTableProfissional($(this).data("nome"), $(this).data("cns"), $(this).data("idprof"), idProfissionalAvaliacao, $(this).data("cbo"), unico);
                        } else {
                            modalAlerta("Atenção", "Falha ao adicionar o Profissional na Avaliacao, tente novamente mais tarde.");
                        }
                    } else {
                        $('.msgErroDuplicado').text('O Profissional selecionado já foi inserido na Avaliacao e não poderá ser inserido novamente.');
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
    var idProfAvaliacao = rowProfissional.data("idprofavaliacao");
    if (substituirProfissional(idProfAvaliacao, $(event.currentTarget).data("idprof"), $(event.currentTarget).data("cbo"))) {
        makeTableProfissional(
            $(event.currentTarget).data("nome"),
            $(event.currentTarget).data("cns"),
            $(event.currentTarget).data("idprof"),
            idProfAvaliacao,
            $(event.currentTarget).data("cbo"),
            true
        );
        rowProfissional.remove();
    } else {
        $('.msgErroDuplicado').text('Ocorreu um erro ao substituir o profissional.');
    }
}

function salvaProfissionalAvaliacao(idAvaliacao, idProfissional, cbo) {
    var blnRet = false;
    alteraAvaliacao(1);
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createProfissional',
            idAvaliacaoElegibilidade: idAvaliacao,
            idProfissional: idProfissional,
            cbo: cbo
        },
        datatype: "json",
        url: "ajax/AvaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp"
    })
    .done(function (data) {
        if (data > 0) {
            blnRet = parseInt(data);
        }
    });
    return blnRet;
}

function substituirProfissional(idProfAvaliacao, idProfissional, cbo) {
    var blnRet = false;
    alteraAvaliacao(1);
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'substituirProfissional',
            idProfAvaliacao: idProfAvaliacao,
            idProfissional: idProfissional,
            cbo: cbo
        },
        datatype: "json",
        url: "ajax/AvaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp"
    })
    .success(function (data) {
        blnRet = true;
    });
    return blnRet;
}

function removerProfissionalAvaliacao(idProfissionalAvaliacao) {
    $.ajax({
        type: "POST",
        data: { acao: 'removeProfissional', id: idProfissionalAvaliacao },
        datatype: "json",
        url: "ajax/AvaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp"
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

function criaPacienteAvaliacao(idAvaliacaoElegibilidade, idPaciente, numeroSus) {
    var blnRet = false;    
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createPacienteAvaliacao',
            idAvaliacaoElegibilidade: idAvaliacaoElegibilidade,
            idPaciente: idPaciente,
            numeroCartaoSus: numeroSus
        },
        datatype: "json",
        url: "ajax/AvaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp"
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

function dadosAvaliacaoElegibilidadeAdmissaoPaciente() {
    var dados = {
        Id: parseInt(jQuery(".IdentificacaoUsuario").data("id")),
        DataNascimento: jQuery("#txtDataNascimento").val(),
        Sexo: jQuery("[name='rdbSexo']:checked").val(),
        Origem: jQuery("#selLocalAtend :selected").val(),
        //Origem: jQuery("[name='rdbOrigem']:checked").val(),
        Condicoes: concatenarCheckBox("chkCondicoes"),
        Cid1: jQuery('#selCid1').val(),
        Cid2: jQuery("#selCid2").val() == undefined ? "" : jQuery('#selCid2').val(),
        Cid3: jQuery("#selCid3").val() == undefined ? "" : jQuery('#selCid3').val(),
        Conclusao: jQuery("[name='rdbConclusao']:checked").val(),
        Elegivel: jQuery("[name='rdbElegivel']:checked").val() == undefined ? 0 : jQuery("[name='rdbElegivel']:checked").val(),
        Inelegivel: concatenarCheckBox("chkInelegivel"),
        NomeCompleto: jQuery("#txtNomeCompleto").val(),
        NomeSocial: jQuery("#txtNomeSocial").val(),
        Raca: jQuery("[name='rdbRaca']:checked").val() == undefined ? 0 : jQuery("[name='rdbRaca']:checked").val(),
        Nis: jQuery("#txtNis").val(),
        NomeMae: jQuery("#txtNomeMae").val(),
        Nacionalidade: jQuery("[name='rdbNacionalidade']:checked").val() == undefined ? 0 : jQuery("[name='rdbNacionalidade']:checked").val(),
        MunicipioUfNascimento: jQuery("#selMunicipioUFNascimento").val() == undefined ? "" : jQuery('#selMunicipioUFNascimento').val(),
        Email: jQuery("#txtEmail").val(),
        TipoLogradouro: jQuery("#selTipoLogradouro :selected").val() == undefined ? 0 : jQuery('#selTipoLogradouro :selected').val(),
        NomeLogradouro: jQuery("#txtNomeLogradouro").val(),
        Numero: (jQuery("#txtNumero").val() == "" ? 0 : jQuery("#txtNumero").val()),
        Complemento: jQuery("#txtComplemento").val(),
        Bairro: jQuery("#txtBairro").val(),
        Municipio: jQuery("#selMunicipio :selected").val() == undefined ? 0 : jQuery('#selMunicipio :selected').val(),
        Uf: jQuery("#selUf :selected").val() == undefined ? 0 : jQuery('#selUf :selected').val(),
        Cep: jQuery("#txtCep").val(),
        TelefoneResidencial: jQuery("#txtTelefoneResidencial").val(),
        TelefoneReferencia: jQuery("#txtTelefoneReferencia").val(),
        Cuidador: jQuery("[name='rdbCuidador']:checked").val() == undefined ? 0 : jQuery("[name='rdbCuidador']:checked").val()
    };
    return JSON.stringify(dados);
}

function dadosAvaliacaoElegibilidadeAdmissao() {
    var dados = {
        codigoCnesUnidade: $('#selCnesUni :selected').val(),
        codEquipe: ($('#selIneEquipe').val() == "") ? 0 : $('#selIneEquipe').val(),
        conferidoPor: "",
        dataAvaliacao: $('#txtData').val(),
        Id: jQuery("#idAvaliacaoElegibilidade").val()
    };
    return JSON.stringify(dados);
}

function alteraPacienteAtendimento() {
    
    $.ajax({
        type: "POST",
        data: {
            acao: 'editPacienteAvaliacao',
            dadosAvaliacaoElegibilidadeAdmissaoPaciente: dadosAvaliacaoElegibilidadeAdmissaoPaciente()
        },
        datatype: "json",
        url: "ajax/AvaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp"
    }).done(
        function () {
            alteraAvaliacao(2)
        }
    )

}

function alteraAvaliacao(status) {
    
    $.ajax({
        type: "POST",
        data: {
            acao: 'Edit',
            dadosAvaliacaoElegibilidadeAdmissao: dadosAvaliacaoElegibilidadeAdmissao(),
            status: status
        },
        datatype: "json",
        url: "ajax/AvaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp"
    }).done(function () {
        if (status == 2) {
            modalAlerta("Confirmação", "Avaliacao finalizada com sucesso!")
            setInterval(window.location.assign("avaliacaoElegibilidadeAdmissao.asp"), 5000);
        }

    });
}

function removerPacienteAvaliacao(idPacienteAtendimento, idusr, tr) {
    return jQuery.ajax({
        type: "POST",
        data: { acao: 'removePacienteAvaliacao', id: idPacienteAtendimento },
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

function limparFormAtendimentoUsuario() {
    $('.IdentificacaoUsuario').each(function (i) {
        $(this).find('input[type=checkbox]:checked').removeAttr('checked', false);
        $(this).find('input[type=text]').val('');
        $(this).find('input[type=radio]:checked').removeAttr('checked', false);        
    });
}

function makeTablePaciente(nome, cns, idusr, idusravaliacao) {
    var strTable = '';
    strTable += '   <tr class="sel-cns" data-nome="' + nome.toCapitalize() + '" data-cns="' + cns + '" data-idusr="' + idusr + '" data-idusravaliacao="' + idusravaliacao + '" >';
    strTable += '       <td class="text-capitalize">' + nome.toCapitalize() + '</td>';
    strTable += '       <td class="text-center">' + cns + '</td>';
    strTable += '       <td class="text-center"><button type="button" class="btn btn-danger" title="Remover"><span class="glyphicon glyphicon-remove"></span></button></td>';
    strTable += "       <td class='text-center'><button type='button' class='btn btn-primary preencherDadosPaciente' title='Preencher Ficha'><span class='glyphicon glyphicon-edit'></span></button></td>";
    strTable += '   </tr>';
    $('#listaAtendimentoIndividual tbody').append(strTable);
    removeGridPacientes();
    desabilitaBuscaPaciente();
    habilitaFichaPaciente();
    $('#modalProfSus').modal('hide');
}

function makeTableProfissional(nome, cns, idprof, idprofavaliacao, cbo, unico) {
    var strTable = "";
    strTable += "   <tr class='sel-cns' data-nome='" + nome.toCapitalize() + "' data-cns='" + cns + "' data-idprof='" + idprof + "' data-idprofavaliacao='" + idprofavaliacao + "' data-cbo='" + cbo + "'>";
    strTable += "       <td class='text-capitalize'>" + nome.toCapitalize() + "</td>";
    strTable += "       <td class='text-center'>" + cns + "</td>";
    strTable += "       <td class='text-center'>" + cbo + "</td>";
    var displayRemover = (unico) ? "none" : "block";
    var displaySubstituir = (unico) ? "block" : "none";
    strTable += "       <td class='text-center'><button style='display:" + displaySubstituir + "' type='button' class='btn btn-success' title='Substituir'><span class='glyphicon glyphicon-refresh'></span></button>";
    strTable += "       <button style='display:" + displayRemover + "' type='button' class='btn btn-danger' title='Remover'><span class='glyphicon glyphicon-remove'></span></button></td>";
    strTable += "   </tr>";
    jQuery("#listaProfissionaisAtendimento tbody").append(strTable);
    substituiProfSus();
    desabilitaBuscaProfissional();
    jQuery("#modalProfSus").modal("hide");
    jQuery("#msgErroTableProf").text("");
}

function desabilitaBuscaProfissional() {
    if ($('#listaProfissionaisAtendimento tbody tr').length > 0) {
        $('#btnBuscarProf').attr('disabled', 'disabled');
    } else {
        $('#btnBuscarProf').removeAttr('disabled');
    }
}

function removeGridPacientes() {
    $('#listaAtendimentoIndividual tbody .btn-danger').click(function () {
        var tr = $(this).closest("tr");
        removerPacienteAvaliacao($(tr).data("idusravaliacao"), $(tr).data("idusr"), tr);
    });
}

function desabilitaBuscaPaciente() {
    if ($('#listaAtendimentoIndividual tbody tr').length > 0) {
        $('#btnBuscarUsu').attr('disabled', 'disabled');
    } else {
        $('#btnBuscarUsu').removeAttr('disabled');
    }
}

function verificarItensnaTable(event) {
    var retorno = true;
    $('#msgErroTableUsu').text('');
    $('#msgErroTableProf').text('');
    if ($('#listaAtendimentoIndividual tbody tr').length == 0) {
        $('#msgErroTableUsu').text('Insira no mínimo um Paciente para realizar a Avaliacao.');
        $('#btnBuscarUsu').focus();
        retorno = false;
    }
    if ($('#listaProfissionaisAtendimento tbody tr').length == 0) {
        $('#msgErroTableProf').text('Insira no mínimo um Profissional para realizar a Avaliacao.');
        $('#btnBuscarProf').focus();
        retorno = false;
    }
    return retorno;

}

function habilitaFichaPaciente() {
    $("#listaAtendimentoIndividual tbody .preencherDadosPaciente").on("click", function () {
        var data = $(this).closest("tr").data("idusravaliacao");
        $("#listaAtendimentoIndividual tbody tr ").removeClass("active");
        if ($("#listaAtendimentoIndividual tbody tr ").length > 1) {
            jQuery(this).closest("tr").addClass("active");
        }
        if (data != $('.IdentificacaoUsuario').data("id")) {
            limparFormAtendimentoUsuario();
        }
        $('.IdentificacaoUsuario').show().data("id", data);
        getFichaPaciente(data);
        $('#btnSalvar').show();
        $('#btnCancelar').show();        
    });

}

function getFichaPaciente(idUsrAvaliacao) {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getUsuarioAvaliacaoById', idAvaliacaoElegibilidadePaciente: idUsrAvaliacao },
        datatype: "json",
        url: "ajax/AvaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            makeDadosPaciente(obj.resultado);
        }
    });
}

function makeDadosPaciente(item) {
    jQuery("#txtCartaoSUS").val(item.numero_cartao_sus);
    if (item.data_nascimento != null && item.data_nascimento != "01/01/1900" && item.data_nascimento != "1900-01-01") {
        $('#txtDataNascimento').val(item.data_nascimento);
    }

    if (item.sexo != null)
        jQuery("[name='rdbSexo'][value='" + item.sexo + "']").prop("checked", true);
    $('#selLocalAtend').val(item.origem);
    //jQuery("[name='rdbOrigem'][value='" + item.origem + "']").prop("checked", true);
    exibirCheckBox("chkCondicoes", item.condicoes_avaliadas);
    jQuery("#selCid1").val(item.cid_1);
    jQuery("#selCid2").val(item.cid_2 == null ? "" : item.cid_2);
    jQuery("#selCid3").val(item.cid_3 == null ? "" : item.cid_3);
    //jQuery("#selLocalAtend").val(item.local_atendimento);
    jQuery("[name='rdbConclusao'][value='" + item.conclusao + "']").prop("checked", true);
    jQuery("[name='rdbElegivel'][value='" + item.conclusao_elegivel + "']").prop("checked", true);
    exibirCheckBox("chkInelegivel", item.conclusao_inelegivel);
    jQuery("#txtNomeCompleto").val(item.nome_completo);
    jQuery("#txtNomeSocial").val(item.nome_social);
    jQuery("[name='rdbRaca'][value='" + item.raca + "']").prop("checked", true);
    jQuery("#txtNis").val(item.nis);
    if (item.nome_completo_mae == "") {
        jQuery("#chkNomeMaeDesconhecido").prop("checked", true);
    } else {
        jQuery("#txtNomeMae").val(item.nome_completo_mae);
    }
    jQuery("[name='rdbNacionalidade'][value='" + item.nacionalidade + "']").prop("checked", true);
    jQuery("#selMunicipioUFNascimento").val(item.municipio_uf_nascimento);
    jQuery("#txtEmail").val(item.email);
    jQuery("#selTipoLogradouro").val(item.tipo_logradouro);
    jQuery("#txtNomeLogradouro").val(item.nome_logradouro);
    if (item.numero == "0") {
        jQuery("#chkSemNumero").prop("checked", true);
    } else {
        jQuery("#txtNumero").val(item.numero);
    }
    jQuery("#txtComplemento").val(item.complemento);
    jQuery("#txtBairro").val(item.bairro);
    jQuery("#selMunicipio").val(item.municipio);
    jQuery("#selUf").val(item.uf);
    jQuery("#txtCep").val(item.cep);
    jQuery("#txtTelefoneResidencial").val(item.telefone_residencial);
    jQuery("#txtTelefoneReferencia").val(item.telefone_referencia);
    jQuery("[name='rdbCuidador'][value='" + item.cuidador + "']").prop("checked", true);
    jQuery("#idAvaliacaoElegibilidade").val(item.id_avaliacao);

    ElegivelOuInelegivel()
}

function getProfissionais() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getProfissionalByIdAvaliacao', idAvaliacaoElegibilidade: jQuery("#idAvaliacaoElegibilidade").val() },
        datatype: "json",
        url: "ajax/AvaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            var linhas = obj.resultado.length;
            $.each(obj.resultado, function (ResultadoItens, item) {
                if (linhas == 1 && ResultadoItens == 0)
                    makeTableProfissional(item.Nome, item.CNS, item.IdProf, item.IdProfAvaliacao, item.CBO, true);
                else
                    makeTableProfissional(item.Nome, item.CNS, item.IdProf, item.IdProfAvaliacao, item.CBO, false);

            });
        }
    });
}

function getPacientes() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getPacienteByIdAvaliacao', idAvaliacaoElegibilidade: jQuery("#idAvaliacaoElegibilidade").val() },
        datatype: "json",
        url: "ajax/AvaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data);
        if (obj.status) {
            $('#listaAtendimentoIndividual tbody').empty();
            $.each(obj.resultado, function (ResultadoItens, item) {
                makeTablePaciente(item.Nome, item.CNS, item.IdUsr, item.IdUsrAvaliacao);
            });
        }
    });
}

function getDadosAvaliacao() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getById', id: jQuery("#idAvaliacaoElegibilidade").val() },
        datatype: "json",
        url: "ajax/AvaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp"
    })
    .done(function (data) {
        var obj = JSON.parse(data);
        if (obj.status) {

            if (obj.resultado.codigo_cnes_unidade > 0) {
                $('#selCnesUni').val(obj.resultado.codigo_cnes_unidade).prop('disabled', 'disabled');
            }
            makeEquipeIne(obj.resultado.codigo_cnes_unidade, obj.resultado.codigo_equipe_ine);
            if (obj.resultado.data_avaliacao != "01/01/1900" && obj.resultado.data_avaliacao != "1900-01-01") {
                var dataAvaliacao = obj.resultado.data_avaliacao.split("-");
                if (dataAvaliacao.length > 1) {
                    $('#txtData').val(dataAvaliacao[2] + "/" + dataAvaliacao[1] + "/" + dataAvaliacao[0]);
                } else {
                    $('#txtData').val(dataAvaliacao);
                }
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
    $('#listaProfissionaisAtendimento tbody .btn-success').on("click", busca_Prof_Sus);
}
function pacienteNovamente() {

    $('#modalSemCartaoSus').modal('hide');
    busca_Paciente_Sus();

}
function salvarSemCadastroAvaliacao(cns) {
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
            $.each(data.resultado, function (ResultadoItens, item) {
                var idAvaliacao = $("#idAvaliacaoElegibilidade").val();
                if (cns == "") {
                    cns = item.cns
                }                
                var idPacienteAvaliacao = criaPacienteAvaliacao(idAvaliacao, item.IDUSR, cns);
                if (idPacienteAvaliacao != false) {
                    makeTablePaciente(item.nome, cns, item.IDUSR, idPacienteAvaliacao)
                } else {
                    modalAlerta("Atenção", "Falha ao adicionar o Paciente na Avaliacao, tente novamente mais tarde.");
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
function addSemCadastroAvaliacao() {
    if ($('#txtmodalBuscaCartaoSus').val() != "") {
        if ($('#txtmodalBuscaCartaoSus').val() == $('#txtmodalConfirmaCartaoSus').val()) {
            salvarSemCadastroAvaliacao($('#txtmodalConfirmaCartaoSus').val());
        }
        else {
            $('#modalSemCartaoSus .msgErro .col-md-12').text('O cartão do SUS não confere com o digitado anteriormente.Favor digite novamente.');
        }
    }
    else {
        $('#txtmodalBuscaCartaoSus').val('');
        $('#txtmodalConfirmaCartaoSus').val('');
        salvarSemCadastroAvaliacao($('#txtmodalConfirmaCartaoSus').val());
    }

}

function ElegivelOuInelegivel() {
    $('[name=rdbElegivel]').removeAttr('disabled');
    $('[name="chkInelegivel[]"]').removeAttr('disabled');

    $('#fichaInelegivel legend .asterisco').remove();
    $('#fichaElegivel legend .asterisco').remove();
    
    if ($('[name=rdbConclusao]:checked').val()) {
        if ($('[name=rdbConclusao]:checked').val() == 4) {
            $('[name=rdbElegivel]').attr('disabled', 'disabled');
            $('#fichaInelegivel legend').append('<span class="asterisco"> *</span>');
        } else {
            $('[name="chkInelegivel[]"]').attr('disabled', 'disabled');
            $('#fichaElegivel legend').append('<span class="asterisco"> *</span>');
        }
    } else {
        $('[name=rdbElegivel],[name="chkInelegivel[]"]').attr('disabled', 'disabled');
    }
    ObrigaSeElegivel();
}
function ObrigaSeElegivel() {
    $('label[for=txtNomeCompleto] .asterisco').remove();
    $('legend:contains("Raça") .asterisco').remove();
    $('legend:contains("Nacionalidade") .asterisco').remove();
    $('legend:contains("Endereço / Local permanência") .asterisco').remove();

    if ($('[name=rdbElegivel]:checked').val() == 1) {
        $('label[for=txtNomeCompleto]').append('<span class="asterisco"> *</span>');
        $('legend:contains("Raça")').append('<span class="asterisco"> *</span>');
        $('legend:contains("Nacionalidade")').append('<span class="asterisco"> *</span>');
        $('legend:contains("Endereço / Local permanência")').append('<span class="asterisco"> *</span>');
    }
}