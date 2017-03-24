
var xDraw;
var xBlock = true;
var xPnFamilia = false;

$(document).ready(function () {

    var Timer;
    var Intervalo = 1000;

    $('#txtBuscaFamilia').keyup(function () {
        clearTimeout(Timer);
        Timer = setTimeout(buscaRespFamilia, Intervalo);
    });

    $('#txtBuscaNome').keyup(function () {
        clearTimeout(Timer);
        Timer = setTimeout(buscaMunicipe, Intervalo);
    });

    if ($("#id").val().length > 0) {

        xBlock = false;

        var xId = $("#id").val();

        $.ajax({
            type: "POST",
            data: { domicilio: xId },
            datatype: "json",
            url: "ajax/domicilio/getDomicilio.asp"
        })
        .done(function (data) {
            if (data.status) {
                $.each(data.resultado, function (ResultadoItens, item) {

                    // Domicílio
                    $("#txtNomeSusProf").val(item.nome_prof);
                    $("#txtSusProf").val(item.sus_prof);
                    $("#txtNumSusProf").val(item.sus_prof);
                    $("#selCnesUni").val(item.cnes_uni);
                    $("#txtIneEquipe").val(item.ine_eqp);
                    $("#txtMicroarea").val(item.microarea);
                    $("#txtData").val(item.data_visita);
                    $("#txtCepEnd").val(item.cep);
                    $("#selectTipoEnd").val(item.id_tipo_end);
                    $("#txtLogradouro").val(item.logradouro);
                    $("#txtNumeroEnd").val(item.numero);
                    $("#txtBairroEnd").val(item.bairro);
                    $("#txtUfEnd").val(item.uf);
                    $("#txtMunicipioEnd").val(item.municipio);
                    $("#txtComplEnd").val(item.complemento);
                    $("#txtFoneResi").val(item.fone_resid);
                    $("#txtFoneRefe").val(item.fone_refe);

                    // Condições
                    $("#txtTpSitMoradia").val(item.id_tp_situacao_moradia);
                    $('input[name=rdTpSitMoradia]').each(function () { if ($(this).val() == item.id_tp_situacao_moradia) $(this).attr('checked', 'checked'); })

                    $("#txtTpLocalizacao").val(item.id_tp_localizacao);
                    $('input[name=rdTpLocalizacao]').each(function () { if ($(this).val() == item.id_tp_localizacao) $(this).attr('checked', 'checked'); })

                    $("#txtTpSitMoradiaRural").val(item.id_tp_situacao_moradia_rural);
                    $('input[name=rdTpSitMoradiaRural]').each(function () { if ($(this).val() == item.id_tp_situacao_moradia_rural) $(this).attr('checked', 'checked'); })

                    $("#txtTpDomicilio").val(item.id_tp_domicilio);
                    $('input[name=rdTpDomicilio]').each(function () { if ($(this).val() == item.id_tp_domicilio) $(this).attr('checked', 'checked'); })

                    $("#txtTpAcessoDomi").val(item.id_tp_acesso_domicilio);
                    $('input[name=rdTpAcessoDomi]').each(function () { if ($(this).val() == item.id_tp_acesso_domicilio) $(this).attr('checked', 'checked'); })

                    $("#txtTpMatParede").val(item.id_tp_construcao_domicilio);
                    $('input[name=rdTpMatParede]').each(function () { if ($(this).val() == item.id_tp_construcao_domicilio) $(this).attr('checked', 'checked'); })

                    $("#txtTpAbsAgua").val(item.id_tp_abastecimento_agua);
                    $('input[name=rdTpAbsAgua]').each(function () { if ($(this).val() == item.id_tp_abastecimento_agua) $(this).attr('checked', 'checked'); })

                    $("#txtTpTrtAgua").val(item.id_tp_tratamento_agua);
                    $('input[name=rdTpTrtAgua]').each(function () { if ($(this).val() == item.id_tp_tratamento_agua) $(this).attr('checked', 'checked'); })

                    $("#txtTpEsgoto").val(item.id_tp_escoamento_esgoto);
                    $('input[name=rdTpEsgoto]').each(function () { if ($(this).val() == item.id_tp_escoamento_esgoto) $(this).attr('checked', 'checked'); })

                    $("#txtTpLixo").val(item.id_tp_destino_lixo);
                    $('input[name=rdTpLixo]').each(function () { if ($(this).val() == item.id_tp_destino_lixo) $(this).attr('checked', 'checked'); })

                    $("#txtTemAnimais").val(item.animal);
                    $('input[name=rdTemAnimais]').each(function () { if ($(this).val() == item.animal) $(this).attr('checked', 'checked'); })
                    $("#txtQtdAnimais").val(item.qtd_animal);

                    $("#txtDispEnergia").val(item.energia);
                    $('input[name=rdDispEnergia]').each(function () { if ($(this).val() == item.energia) $(this).attr('checked', 'checked'); })

                    $("#txtNumMorador").val(item.qtd_morador);
                    $("#txtNumComodos").val(item.qtd_comodo);

                    $(".fone").each(function () {
                        var phone;
                        $(this).unmask();
                        phone = $(this).val().replace(/\D/g, '');
                        if (phone.length > 10) {
                            $(this).mask("(99) 99999-999?9");
                        } else {
                            $(this).mask("(99) 9999-9999?9");
                        }
                    });

                });

                $('#navPnFamilia').show();

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

        $.ajax({
            type: "POST",
            data: { domicilio: xId },
            datatype: "json",
            url: "ajax/domicilio/getDomicilioAnimal.asp"
        })
       .done(function (data) {
           if (data.status) {
               $.each(data.resultado, function (ResultadoItens, item) {

                   if (item.status == 1) {
                       $('#tpAnimal_' + item.idAnimal).attr('checked', 'checked');
                   }

               });
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

        buscaFamilias(xId);
        blockCampo(1);

    }

    $('#modalProfSus').on('shown.bs.modal', function () {
        $('#txtModalBusca').focus();
    });

    $('#modalEndereco').on('shown.bs.modal', function () {
        $('#txtModalLogradouro').focus();
    })

});

function cancelar() {
    window.location.assign("domicilio.asp");
}

function blockCampo(tp) {

    if (tp == 1) {
        $("#formGrupo").find("input[type=text], input[type=email], input[type=checkbox], input[type=radio], select, #btnBuscarProf, #btnBuscarCep, #pnFamilia button").each(function () {
            $(this).attr('disabled', 'disabled');
        });

        $("#formGrupo").find("#pnListaFamilia button.btn-eye, #pnListaFamilia button.back").each(function () {
            $(this).removeAttr('disabled');
        });

        $('#btnSalvar, #btnSalvarIndividual').hide();
        $('#btnEditar').show();
    }

    if (tp == 2) {
        $("#formGrupo").find("input[type=text], input[type=email], input[type=checkbox], input[type=radio], select, #btnBuscarProf, #btnBuscarCep, #pnFamilia button").each(function () {
            $(this).removeAttr('disabled');
        });

        setAnimal($('#txtTemAnimais').val());

        $('#txtNomeSusProf, #txtNumSusProf, #selectTipoEnd, #txtLogradouro, #txtBairroEnd, #txtUfEnd, #txtMunicipioEnd').attr('disabled', 'disabled');
        $('#btnEditar').hide();

        if (!xPnFamilia) $('#btnSalvar, #btnSalvarIndividual').show();
        else $('#btnSalvarIndividual').show();
        


        $("#cadIndividual").find("#fdsDeficiencia input[type=checkbox], #fdsCardiaco input[type=checkbox], #fdsRenal input[type=checkbox], #fdsRespira input[type=checkbox], #fdsHigiene input[type=checkbox], #rdTpGenero input[type=radio], #fdsSitRua input[type=radio], #txtIndRespFam, #txtIndParente, #txtIndComuniTradDesc, #txtIndMaternidade, #txtIndInterCausa, #txtIndPlantaDesc, #txtIndOtrInstDesc, #selIndGrauParente").each(function () {
            $(this).attr('disabled', 'disabled');
        });

        xBlock = true;
        buscaFamilias($("#id").val(), 1);
    }

    if (tp == 3) {
        $("#cadIndividual").find("input[type=text], input[type=email], input[type=checkbox], input[type=radio], select, #btnBuscarProf, #btnBuscarCep, #pnFamilia button").each(function () {
            $(this).attr('disabled', 'disabled');
        });
    }

}

function buscaFamilias(domicilio, blk) {

    $('#listaFamilia tbody').empty();
    $('#listaFamilia tbody').append('<tr><td colspan="10" class="text-center">Carregando...</td></tr>');

    $.ajax({
        type: "POST",
        data: { domicilio: domicilio },
        datatype: "json",
        url: "ajax/domicilio/getDomicilioFamilia.asp"
    })
    .done(function (data) {
        $('#listaFamilia tbody').empty();

        if (data.status) {
            $.each(data.resultado, function (ResultadoItens, item) {

                strTable = '<tr>';
                strTable += '<td>' + item.id_familia + '</td>';
                strTable += '<td>' + item.nome.toCapitalize() + '</td>';
                strTable += '<td>' + item.cns + '</td>';
                strTable += '<td class="text-center">' + item.dt_nasc + '</td>';
                strTable += '<td class="text-center">' + item.qtd_membro + '</td>';
                strTable += '<td class="text-center">' + item.dt_reside + '</td>';
                strTable += '<td class="text-center">' + item.desc_mudou + '</td>';

                if (!xBlock) {
                    strTable += '<td class="text-center"><button type="button" title="Vizualizar" class="btn btn-outline btn-success btn-xs btn-eye" onclick="buscaMembroFamilia(' + item.id_familia + ',1)" ><span class="glyphicon glyphicon-eye-open"></span></button>&nbsp;';
                    strTable += '<button type="button" title="Remover" class="btn btn-outline btn-success btn-xs" data-id="' + item.id_familia + '"><span class="glyphicon glyphicon-remove"></span></button></td>';
                }else{
                    if (item.mudou == 0) {
                        strTable += '<td class="text-center"><button type="button" title="Editar" class="btn btn-outline btn-success btn-xs" onclick="buscaMembroFamilia(' + item.id_familia + ')" ><span class="glyphicon glyphicon-pencil"></span></button>&nbsp;';
                        strTable += '<button type="button" title="Remover" class="btn btn-outline btn-success btn-xs" data-id="' + item.id_familia + '"><span class="glyphicon glyphicon-remove"></span></button></td>';
                    } else {
                        strTable += '<td class="text-center"><button type="button" title="Vizualizar" class="btn btn-outline btn-success btn-xs btn-eye" onclick="buscaMembroFamilia(' + item.id_familia + ',1)" ><span class="glyphicon glyphicon-eye-open"></span></button>&nbsp;';
                        strTable += '<button type="button" title="Motivo" class="btn btn-outline btn-success btn-xs btn-eye" data-id="' + item.id_familia + '"><span class="glyphicon glyphicon-warning-sign"></span></button></td>';
                    }
                }
                strTable += '</tr>';

                $('#listaFamilia tbody').append(strTable);

            });
        } else {

            if (data.sessao) {
                setTimeout(function () {
                    modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                    setTimeout(function () {
                        window.location.assign("login.asp");
                    }, 2000);
                }, 800);
            }

            strTable = '<tr><td colspan="8" class="text-center">Nenhuma família cadastrada</td></tr>';
            $('#listaFamilia tbody').append(strTable);

        }

        if (!blk) {
            blockCampo(1);
        }

    });

}

function buscaRespFamilia(tp) {

    var xBusca = $('#txtBuscaFamilia').val();
    xBusca = xBusca.toString();

    if (xBusca.length > 2) {
        $('#listaBuscaFamilia').slideDown("slow");

        xDraw = Math.floor((Math.random() * 100) + 1);

        $.ajax({
            type: "POST",
            data: { domicilio: 'domicilio', pessoa: xBusca, draw: xDraw },
            datatype: "json",
            url: "ajax/compFamiliar/getCompFamiliar.asp"
        })
        .done(function (data) {
            if (data.status && data.draw == xDraw) {
                $('#listaBuscaFamilia .list-group-item').remove();
                $('#listaBuscaFamilia').append('<span class="list-group-item list-group-item-success">Nomes Localizados</span>');

                var qtdBusca = data.resultado.length
                var count = 1

                $.each(data.resultado, function (ResultadoItens, item) {

                    if (item.domicilio) {
                        $('#listaBuscaFamilia').append('<a href="#nogo" class="list-group-item list-group-item-danger comDomicilio" data-id="' + item.id_composicao_familiar + '" data-nome="' + item.nome.toCapitalize() + '" data-qtd="' + item.qtd_membros + '" style="color: #a94442 !important;"><span class="badge">Munícipe já possui Domicilio - ' + item.id_domicilio + '</span> ' + item.nome.toCapitalize() + ' - (' + item.data_nasc + ')</a>');
                    } else {
                        $('#listaBuscaFamilia').append('<a href="#nogo" class="list-group-item semDomicilio" data-id="' + item.id_composicao_familiar + '" data-nome="' + item.nome.toCapitalize() + '" data-qtd="' + item.qtd_membros + '" ><span class="badge">' + item.id_pessoa + '</span> ' + item.nome.toCapitalize() + ' - (' + item.data_nasc + ')</a>');
                    }

                    if (count == qtdBusca) {
                        executaSelect();
                    }
                    count++;

                });

                $('#infoResultFamilia').show('slow');

                if (tp) {
                    locFamilia();
                }

            } else {

                if (data.sessao) {
                    setTimeout(function () {
                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                        setTimeout(function () {
                            window.location.assign("login.asp");
                        }, 2000);
                    }, 800);
                }

                $('#listaBuscaFamilia .list-group-item').remove();
                $('#listaBuscaFamilia').append('<li class="list-group-item" style="color: red;">Nenhuma Fámilia localizada.</li>')
            }
        })
    } else if (xBusca.length == 0) {
        $('#listaBuscaFamilia .list-group-item').remove();
        $('#listaBuscaFamilia').hide();
        $('#infoResultFamilia').hide();
        $('#listaBuscaFamilia').empty();
    } else if (xBusca.length > 0 && xBusca.length <= 2) {
        $('#listaBuscaFamilia .list-group-item').remove();
        $('#listaBuscaFamilia').append('<li class="list-group-item">Digite no mínimo 03 caracteres!</li>');
        $('#listaBuscaFamilia').slideDown("slow");
        $('#infoResultFamilia').hide();
    }

    //Function que habilita o click nos resultados da busca do Munícipe
    function executaSelect() {

        $('#listaBuscaFamilia .semDomicilio').click(function () {
            $(this).siblings().removeClass('list-group-item-success');
            $(this).siblings().slideUp();
            $(this).siblings().remove();
            $(this).addClass('list-group-item-success');
            $(this).css('border-radius', '5px');

            $("#modalAddFamilia").find(".has-error").each(function () {
                $(this).removeClass("has-error");
            });

            $("#modalAddFamilia").find("input[type=text]").each(function () {
                $(this).val('');
            });

            $('#modalAddFamilia').modal('show');

            $('#infoResultFamilia').slideUp();
            $('#txtBuscaFamilia').val("");
        });

    }

}

function carregarMunicipio() {

    $('#txtMunicipioNasc').empty();
    $('#txtMunicipioNasc').append('<option value="">Carregando...</option>');

    $.ajax({
        type: "POST",
        data: { muni: $('#selectUfNasc').val() },
        datatype: "json",
        url: "ajax/cep/getMunicipio.asp"
    })
        .done(function (data) {
            if (data.status) {
                $('#txtMunicipioNasc').empty();
                $('#txtMunicipioNasc').append('<option value="">Selecione</option>');

                $.each(data.resultado, function (ResultadoItens, item) {
                    $('#txtMunicipioNasc').append('<option value="' + item.toString() + '">' + item.toString() + '</option>');
                });
            } else {
                $('#txtMunicipioNasc').empty();
                $('#txtMunicipioNasc').append('<option value="">Selecione a UF</option>');
            }
        });

}

function busca_cep() {

    xCep = $('#txtCepEnd').val();
    xCep = xCep.replace(/_/g, "");
    xCep = xCep.replace("-", "");

    if (xCep.length == 8) {

        $('#txtCepEnd').attr('disabled', 'disabled');

        $("#selectTipoEnd").val("");
        $("#txtLogradouro").val("");
        $("#txtBairroEnd").val("");
        $("#txtUfEnd").val("");
        $("#txtMunicipioEnd").val("");

        if (xCep == "99999999") {

            $("label[for=txtCepEnd]").html('CEP *');
            $("#txtCepEnd").removeClass('obg');
            $("#txtCepEnd").addClass('obg');
            $("#txtCepEnd").removeAttr('disabled');

            $("#selectTipoEnd").val("081");
            $("#selectTipoEnd").attr('disabled', 'disabled');

            $("#txtLogradouro").val("SEM INFORMACAO");
            $("#txtLogradouro").attr('disabled', 'disabled');

            $('#txtNumeroEnd').val("0");
            $("#txtNumeroEnd").attr('disabled', 'disabled');

            $("#txtBairroEnd").val("SEM INFORMACAO");
            $("#txtBairroEnd").attr('disabled', 'disabled');

            $("#txtUfEnd").val("SI");
            $("#txtUfEnd").attr('disabled', 'disabled');

            $("#txtMunicipioEnd").val("SEM INFORMACAO");
            $("#txtMunicipioEnd").attr('disabled', 'disabled');

        } else if (xCep == "14900000") {

            $("label[for=txtCepEnd]").html('CEP *');
            $("#txtCepEnd").removeClass('obg');
            $("#txtCepEnd").addClass('obg');
            $("#txtCepEnd").removeAttr('disabled');

            $("#selectTipoEnd").val('');
            $("#selectTipoEnd").removeAttr('disabled');

            $("#txtLogradouro").val('');
            $("#txtLogradouro").removeAttr('disabled');

            $('#txtNumeroEnd').val('');
            $("#txtNumeroEnd").removeAttr('disabled');

            $("#txtBairroEnd").val('');
            $("#txtBairroEnd").removeAttr('disabled');

            $("#txtUfEnd").val('SP');
            $("#txtMunicipioEnd").val('ITAPOLIS');

        } else if (xCep == "11740000") {

            $("label[for=txtCepEnd]").html('CEP *');
            $("#txtCepEnd").removeClass('obg');
            $("#txtCepEnd").addClass('obg');
            $("#txtCepEnd").removeAttr('disabled');

            $("#selectTipoEnd").val('');
            $("#selectTipoEnd").removeAttr('disabled');

            $("#txtLogradouro").val('');
            $("#txtLogradouro").removeAttr('disabled');

            $('#txtNumeroEnd').val('');
            $("#txtNumeroEnd").removeAttr('disabled');

            $("#txtBairroEnd").val('');
            $("#txtBairroEnd").removeAttr('disabled');

            $("#txtUfEnd").val('SP');
            $("#txtMunicipioEnd").val('ITANHAEM');

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

                        $("label[for=txtCepEnd]").html('CEP *');
                        $("#txtCepEnd").removeClass('obg');
                        $("#txtCepEnd").addClass('obg');
                        $("#txtCepEnd").prop('disabled', '');

                        $("#selectTipoEnd").val(item.idTipo);
                        $("#selectTipoEnd").attr('disabled', 'disabled');

                        $("#txtLogradouro").val(item.logradouro);
                        $("#txtLogradouro").attr('disabled', 'disabled');

                        $("#txtBairroEnd").val(item.bairro);
                        $("#txtBairroEnd").attr('disabled', 'disabled');

                        $("#txtUfEnd").val(item.uf);
                        $("#txtUfEnd").attr('disabled', 'disabled');

                        $("#txtMunicipioEnd").val(item.municipio);
                        $("#txtMunicipioEnd").attr('disabled', 'disabled');

                        $('#txtNumeroEnd').focus();
                    });

                    $('#txtCepEnd').removeAttr('disabled');

                } else {

                    if (data.sessao) {

                        setTimeout(function () {
                            modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                            setTimeout(function () {
                                window.location.assign("login.asp");
                            }, 2000);
                        }, 800);

                    } else {

                        $('#txtCepEnd').val('');
                        $("#selectTipoEnd").val('');
                        $("#txtLogradouro").val('');
                        $("#txtNumeroEnd").val('');
                        $("#txtBairroEnd").val('');
                        $("#txtUfEnd").val('');
                        $("#txtMunicipioEnd").val('');

                        $('#modalEndereco').find('input[type=text], select').each(function () {
                            $(this).val('');
                            $(this).parents(".form-group").removeClass("has-error");
                        });

                        $('#listaModalEndereco tbody').empty();
                        $('#listaModalEndereco tbody').append('<tr><th colspan="6" class="text-center">Cep não encontrado! <br> Efetue a busca acima</th></tr>');
                        $('#modalEndereco').modal('show');

                        $('#txtCepEnd').removeAttr('disabled');
                    }
                }
            });
        }
    }

}

function busca_endereco() {

    $("#selectTipoEnd").val('');
    $("#txtCepEnd").val('');
    $("#txtLogradouro").val('');
    $("#txtNumeroEnd").val('');
    $("#txtBairroEnd").val('');
    $("#txtUfEnd").val('');
    $("#txtMunicipioEnd").val('');

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

                    var strTable = '<tr class="sel-cep" data-cep="' + item.cep + '" data-tipo="' + item.idTipo + '" data-logradouro="' + item.logradouro + '" data-bairro="' + item.bairro + '" data-municipio="' + item.municipio + '" data-uf="' + item.uf + '" >';
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

            $("label[for=txtCepEnd]").html('CEP *');
            $("#txtCepEnd").removeClass('obg');
            $("#txtCepEnd").addClass('obg');
            $("#txtCepEnd").val($(this).data('cep'));

            $("#selectTipoEnd").val($(this).data('tipo'));
            $("#selectTipoEnd").attr('disabled', 'disabled');

            $("#txtLogradouro").val($(this).data('logradouro'));
            $("#txtLogradouro").attr('disabled', 'disabled');

            $("#txtBairroEnd").val($(this).data('bairro'));
            $("#txtBairroEnd").attr('disabled', 'disabled');

            $("#txtUfEnd").val($(this).data('uf'));
            $("#txtUfEnd").attr('disabled', 'disabled');

            $("#txtMunicipioEnd").val($(this).data('municipio'));
            $("#txtMunicipioEnd").attr('disabled', 'disabled');

            $('#modalEndereco').modal('hide');

            $('#txtNumeroEnd').focus();
        });
    }

}

function enderecoNovamente() {

    $('#modalSemEndereco').modal('hide');
    $('#modalEndereco').modal('show');

}

function addEndereco() {

    $("label[for=txtCepEnd]").html('CEP');
    $("#txtCepEnd").removeClass('obg');
    $("#txtCepEnd").removeAttr('disabled');
    $("#txtCepEnd").val('');

    $("#selectTipoEnd").val('');
    $("#selectTipoEnd").removeAttr('disabled');

    $("#txtLogradouro").val(removerAcentos($('#txtModalLogradouro').val().trim()).toUpperCase());

    $("#txtNumeroEnd").removeAttr('disabled');

    $("#txtBairroEnd").val('');
    $("#txtBairroEnd").removeAttr('disabled');

    $("#txtUfEnd").val($('#txtModalUfEnd').val());

    $("#txtMunicipioEnd").val(removerAcentos($('#txtModalMunicipioEnd').val().trim()).toUpperCase());

    $('#modalSemEndereco').modal('hide');


}

function busca_Prof_Sus() {

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
            url: "ajax/domicilio/getProfissional.asp"
        })
        .done(function (data) {

            $('#listamodalProfSus tbody').empty();

            if (data.status) {

                $.each(data.resultado, function (ResultadoItens, item) {

                    var strTable = '<tr class="sel-cns" data-nome="' + item.nome.toCapitalize() + '" data-cns="' + item.cns + '" >';
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

    //Function que habilita o click nos resultados da busca do Endereço
    function executaSelect() {
        $('#listamodalProfSus tbody .sel-cns').click(function () {
            //Remove o active dos outros
            $(this).siblings().removeClass('list-group-item-success');

            $("#txtNumSusProf").val($(this).data('cns'));
            $("#txtSusProf").val($(this).data('cns'));
            $("#txtNomeSusProf").val($(this).data('nome')); 

            $('#modalProfSus').modal('hide');
        });
    }

    return false;

}

function setRdValor(elem) {
    $('#txt' + $(elem).data('rd')).val($(elem).val());
}

function setAnimal(tp) {

    if (tp == 1) {

        $("#cadDomicilio").find("input[type=checkbox]").each(function () {
            $(this).removeAttr('disabled');
        });
        $('#txtQtdAnimais').removeAttr('disabled');

    } else {

        $("#cadDomicilio").find("input[type=checkbox]").each(function () {
            $(this).attr('disabled', 'disabled');
            $(this).prop('checked', false);
        });
        $('#txtQtdAnimais').attr('disabled', 'disabled');
        $('#txtQtdAnimais').val('');
    }

}

function setPnFamilia(tp) {
    if (tp) {

        if (!xBlock) {
            xPnFamilia = true;
            $('#btnSalvar').hide();
        } else {
            if (salvaCadastroDomicilio(1)) {
                xPnFamilia = true;
                $('#btnSalvar').hide();
            } else {
                setTimeout(function () {
                    $('#cadDomicilio .nav.nav-tabs li').each(function () { $(this).removeClass('in active') });
                    $('#cadDomicilio .nav.nav-tabs li:first').each(function () { $(this).addClass('in active') });
                    $('#cadDomicilio .tab-pane').each(function () { $(this).removeClass('active in') });
                    $('#cadDomicilio .tab-pane:first').each(function () { $(this).addClass('active in') });
                }, 500);
            }
        }
    } else {
        xPnFamilia = false;
        if (!xBlock) $('#btnSalvar').hide();
        else $('#btnSalvar').show();
        cadFamilia(2);
    }

}

function cadFamilia(tp) {

    if (!tp) {
        cancelaAddFamilia();
        $('#pnListaFamilia').slideUp();
        $('#pnCadFamilia').slideDown();
    } else if(tp == 1) {
        $('#pnCadFamilia').slideUp();
        $('#pnNewCadFamilia').slideUp();
        $('#pnListaFamilia').slideDown();
        $('#txtBuscaFamilia').val('');
    } else {
        $('#tbListaMembroFamilia').hide();
        $('#listaFamilia').fadeIn();
        $('#btnVoltarFamilia').hide();
        $('#btnCancelar').show();
        $('#pnListaFamilia .panel-heading span').html('Familias');
        $('#pnListaFamilia .panel-heading button').show();
    }
}

function buscaMembroFamilia(familia, mudou, blk) {

    $.ajax({
        type: "POST",
        data: { grupo: familia },
        datatype: "json",
        url: "ajax/compFamiliar/getCompFamiliar.asp"
    })
    .done(function (data) {
        $('#tbListaMembroFamilia tbody').empty();

        if (data.status) {

            var nome = '';

            $.each(data.resultado, function (ResultadoItens, item) {

                if (item.chefe) {
                    nome = item.nome.toCapitalize();
                }

                strTable = '<tr>';
                strTable += '<td class="text-center">' + item.id_composicao_familiar + '</td>';
                strTable += '<td class="text-center">' + item.id_pessoa + '</td>';
                strTable += '<td>' + item.nome.toCapitalize() + '</td>';
                strTable += '<td class="text-center">' + item.data_nasc + '</td>';
                strTable += '<td class="text-center">' + item.parentesco + '</td>';
                
                if (!mudou) {
                    if (!xBlock) {
                        strTable += '<td class="text-center"><button type="button" title="visualizar Cadastro" class="btn btn-outline btn-success btn-xs btn-eye" data-id="' + item.id_pessoa + '" onclick="buscaQuestionario(' + item.id_pessoa + ')"><span class="glyphicon glyphicon-eye-open"></span></button></td>';
                    } else {
                        strTable += '<td class="text-center"><button type="button" title="Editar Cadastro" class="btn btn-outline btn-success btn-xs" data-id="' + item.id_pessoa + '" onclick="buscaQuestionario(' + item.id_pessoa + ',1)"><span class="glyphicon glyphicon-pencil"></span></button></td>';
                    }
                } else {
                    strTable += '<td class="text-center"><button type="button" title="visualizar Cadastro" class="btn btn-outline btn-success btn-xs btn-eye" data-id="' + item.id_pessoa + '" onclick="buscaQuestionario(' + item.id_pessoa + ')"><span class="glyphicon glyphicon-eye-open"></span></button></td>';
                }
                
                
                strTable += '</tr>';

                $('#tbListaMembroFamilia tbody').append(strTable);

            });

            $('#listaFamilia').hide();
            $('#tbListaMembroFamilia').fadeIn();

            $('#pnListaFamilia .panel-heading span').html('Composição Familiar de <b>' + nome + '</b>');
            $('#pnListaFamilia .panel-heading button').hide();

        } else {

            if (data.sessao) {
                setTimeout(function () {
                    modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                    setTimeout(function () {
                        window.location.assign("login.asp");
                    }, 2000);
                }, 800);
            }

            modalAlerta("Atenção", "Falha ao carregar família");

        }

        if (!xBlock) blockCampo(1);

        $('#btnVoltarFamilia').show();
        $('#btnCancelar').hide();

    });

}

function cancelaAddFamilia() {
    $('#listaBuscaFamilia').hide();
    $('#listaBuscaFamilia .list-group-item').remove();
}

function addFamilia() {

    if (validaCampos($("#modalAddFamilia"), 2, "alertaSemModalAddFamilia")) {

        xIdDomicilio = $("#id").val();
        xFamilia = $('#listaBuscaFamilia .semDomicilio').data('id');
        xQtdMembros = $('#listaBuscaFamilia .semDomicilio').data('qtd');
        xDtReside = '01/' + $('#txtModalFamMes').val() + '/' + $('#txtModalFamAno').val();

        var dados = new Object;
        dados['domicilio'] = xIdDomicilio;
        dados['familia'] = xFamilia;
        dados['membros'] = xQtdMembros;
        dados['dataResid'] = xDtReside;

        $.ajax({
            type: "POST",
            data: dados,
            datatype: "json",
            url: "ajax/domicilio/cadDomicilioFamilia.asp"
        })
        .done(function (data) {

            if (data.status) {

                $('#modalAddFamilia').modal('hide');

                modalAlerta('Cadastro de Família no Domicílio', 'Família adicionada com sucesso!');
                setTimeout(function () {
                    $('#alertaModal').modal('hide');
                }, 2000);

                $('#listaBuscaFamilia').hide();
                $('#listaBuscaFamilia .list-group-item').remove();

                buscaFamilias(xIdDomicilio,1);
                cadFamilia(1);

            } else {

                if (data.sessao) {
                    $('#modalAddFamilia').modal('hide');
                    setTimeout(function () {
                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                        setTimeout(function () {
                            window.location.assign("login.asp");
                        }, 2000);
                    }, 800);
                } else {
                    modalAlerta('Cadastro de Família no Domicílio', 'Falha ao adicionar família!');
                }
            }
        });

    }
}

function salvaCadastroDomicilio(tp) {

    var back = false;

    $('#btnSalvar').attr('disabled', 'disabled');

    $("#cadDomicilioSave1, #cadDomicilioSave2").find("input[type=text], input[type=email]").each(function () {
        $('#' + $(this).context.name).val($(this).context.value.trim());
    });

    if (validaCampos($("#cadDomicilioSave1, #cadDomicilioSave2"))) {

        xId = $('#id').val();

        var dados = new Object;

        $("#cadDomicilioSave1, #cadDomicilioSave2").find("input[type=text], input[type=hidden], input[type=email], input[type=checkbox], input[type=radio], select").each(function () {

            if ($(this).context.type == "checkbox") {
                if ($(this).is(':checked')) {
                    dados[$(this).context.name] = $(this).context.value + ',1';
                } else {
                    dados[$(this).context.name] = $(this).context.value + ',0';
                }
            } else if ($(this).context.type == "radio") {
                if ($(this).is(':checked')) {
                    dados[$(this).context.name] = $(this).context.value.trim();
                }
            } else {
                dados[$(this).context.name] = $(this).context.value.trim();
            }

        });

        dados['idDomicilio'] = xId;

        $.ajax({
            type: "POST",
            data: dados,
            datatype: "json",
            url: "ajax/domicilio/cadDomicilio.asp",
            async: false
        })
        .done(function (data) {

            if (data.status) {

                back = true;

                if (!tp) {
                    if ($("#id").val().length > 0) {
                        modalAlerta('Alteração de Domicílio', 'Alteração efetuada com sucesso!');
                    } else {
                        modalAlerta('Cadastro de Domicílio', 'Cadastro efetuado com sucesso!');
                        $("#id").val(data.domicilio);
                        $('.page-header').html('Alteração de Domicílio');
                    }

                    setTimeout(function () {
                        $('#alertaModal').modal('hide');
                    }, 2000);

                    $('#navPnFamilia').show();
                    buscaFamilias(xId);
                }

            } else {

                if (data.sessao) {
                    setTimeout(function () {
                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                        setTimeout(function () {
                            window.location.assign("login.asp");
                        }, 2000);
                    }, 800);
                }

                if (!tp) {
                    modalAlerta('Cadastro de Domicílio', 'Falha ao efutar cadastro!');
                }
            }
        });

    }

    $('#btnSalvar').removeAttr('disabled');

    return back;
}

function setMae() {
    if ($('#ckIndNomeMae').is(':checked')) {
        $('label[for=txtIndNomeMae]').html('Nome Completo Mãe');
        $('#txtIndNomeMae').attr('disabled', 'disabled');
        $('#txtIndNomeMae').removeClass('obg');
        $('#txtIndNomeMae').val('');
    } else {
        $('label[for=txtIndNomeMae]').html('Nome Completo Mãe *');
        $('#txtIndNomeMae').removeAttr('disabled');
        $('#txtIndNomeMae').addClass('obg');
    }
}

function setMunicipioUf() {

    if ($('#selIndNacional').val() != 10) {
        $('label[for=txtIndUf]').html('UF');
        $('#txtIndUf').removeClass('obg');
        $('#txtIndUf').attr('disabled', 'disabled');
        $('#txtIndUf').val('');

        $('label[for=txtIndMunicipio]').html('Município');
        $('#txtIndMunicipio').removeClass('obg');
        $('#txtIndMunicipio').attr('disabled', 'disabled');
        $('#txtIndMunicipio').val('');
    } else {
        $('label[for=txtIndUf]').html('UF *');
        $('#txtIndUf').addClass('obg');
        $('#txtIndUf').removeAttr('disabled');

        $('label[for=txtIndMunicipio]').html('Município *');
        $('#txtIndMunicipio').addClass('obg');
        $('#txtIndMunicipio').removeAttr('disabled');
    }

}

function setMunicipioUf2() {

    if ($('#selMuNacional').val() != 10) {
        $('label[for=txtMuUf]').html('UF');
        $('#txtMuUf').removeClass('obg');
        $('#txtMuUf').attr('disabled', 'disabled');
        $('#txtMuUf').val('');

        $('label[for=txtMuMunicipio]').html('Município');
        $('#txtMuMunicipio').removeClass('obg');
        $('#txtMuMunicipio').attr('disabled', 'disabled');
        $('#txtMuMunicipio').val('');
    } else {
        $('label[for=txtMuUf]').html('UF *');
        $('#txtMuUf').addClass('obg');
        $('#txtMuUf').removeAttr('disabled');
        $('#txtMuUf').val('');

        $('label[for=txtMuMunicipio]').html('Município *');
        $('#txtMuMunicipio').addClass('obg');
        $('#txtMuMunicipio').removeAttr('disabled');
        $('#txtMuMunicipio').val('');
    }

}

function setComuTrad(tp) {

    if (tp == 1) {
        $('label[for=txtIndComuniTradDesc]').html('Qual(is) Comunidade(s) ? *');
        $('#txtIndComuniTradDesc').removeAttr('disabled');
        $('#txtIndComuniTradDesc').addClass('obg');
    } else {
        $('label[for=txtIndComuniTradDesc]').html('Qual(is) Comunidade(s) ?');
        $('#txtIndComuniTradDesc').attr('disabled', 'disabled');
        $('#txtIndComuniTradDesc').removeClass('obg');
        $('#txtIndComuniTradDesc').val('');
    }

}

function setGenero(tp) {

    if (tp == 1) {
        $("#rdTpGenero").find("input[type=radio]").each(function () {
            $(this).removeAttr('disabled');
        });
        $('#txtIndGeneroTp').val('');
        $('#txtIndGeneroTp').addClass('obg');
    } else {
        $("#rdTpGenero").find("input[type=radio]").each(function () {
            $(this).attr('disabled', 'disabled');
            $(this).prop('checked', false);
        });
        $('#txtIndGeneroTp').val('');
        $('#txtIndGeneroTp').removeClass('obg');
    }

}

function setDeficiencia(tp) {

    if (tp == 1) {

        $("#fdsDeficiencia").find("input[type=checkbox]").each(function () {
            $(this).removeAttr('disabled');
        });
        $('#txtIndTemDeficiOk').addClass('obg');
        $('#txtIndTemDeficiOk').val('');
    } else if (tp == 2) {
        check = false;
        $("#fdsDeficiencia").find("input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                check = true;
            }
        });
        if (check) { $('#txtIndTemDeficiOk').val('1'); } else { $('#txtIndTemDeficiOk').val(''); }
    } else {

        $("#fdsDeficiencia").find("input[type=checkbox]").each(function () {
            $(this).attr('disabled', 'disabled');
            $(this).prop('checked', false);
        });
        $('#txtIndTemDeficiOk').removeClass('obg');
        $('#txtIndTemDeficiOk').val('');
    }

}

function setGestante(tp) {

    if (tp == 1) {
        $('label[for=txtIndMaternidade]').html('Maternidade ? *');
        $('#txtIndMaternidade').removeAttr('disabled');
        $('#txtIndMaternidade').addClass('obg');
    } else {
        $('label[for=txtIndMaternidade]').html('Maternidade ?');
        $('#txtIndMaternidade').attr('disabled', 'disabled');
        $('#txtIndMaternidade').removeClass('obg');
        $('#txtIndMaternidade').val('');
    }

}

function setCardiaco(tp) {

    if (tp == 1) {

        $("#fdsCardiaco").find("input[type=checkbox]").each(function () {
            $(this).removeAttr('disabled');
        });
        $('#txtIndCardiacoOk').addClass('obg');
        $('#txtIndCardiacoOk').val('');
    } else if (tp == 2) {
        check = false;
        $("#fdsCardiaco").find("input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                check = true;
            }
        });
        if (check) { $('#txtIndCardiacoOk').val('1'); } else { $('#txtIndCardiacoOk').val(''); }
    } else {

        $("#fdsCardiaco").find("input[type=checkbox]").each(function () {
            $(this).attr('disabled', 'disabled');
            $(this).prop('checked', false);
        });
        $('#txtIndCardiacoOk').removeClass('obg');
        $('#txtIndCardiacoOk').val('');
    }

}

function setRenal(tp) {

    if (tp == 1) {

        $("#fdsRenal").find("input[type=checkbox]").each(function () {
            $(this).removeAttr('disabled');
        });
        $('#txtIndRenalOk').addClass('obg');
        $('#txtIndRenalOk').val('');
    } else if (tp == 2) {
        check = false;
        $("#fdsRenal").find("input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                check = true;
            }
        });
        if (check) { $('#txtIndRenalOk').val('1'); } else { $('#txtIndRenalOk').val(''); }
    } else {

        $("#fdsRenal").find("input[type=checkbox]").each(function () {
            $(this).attr('disabled', 'disabled');
            $(this).prop('checked', false);
        });
        $('#txtIndRenalOk').removeClass('obg');
        $('#txtIndRenalOk').val('');
    }

}

function setRespiratorio(tp) {

    if (tp == 1) {

        $("#fdsRespira").find("input[type=checkbox]").each(function () {
            $(this).removeAttr('disabled');
        });
        $('#txtIndRespiraOk').addClass('obg');
        $('#txtIndRespiraOk').val('');
    } else if (tp == 2) {
        check = false;
        $("#fdsRespira").find("input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                check = true;
            }
        });
        if (check) { $('#txtIndRespiraOk').val('1'); } else { $('#txtIndRespiraOk').val(''); }
    } else {

        $("#fdsRespira").find("input[type=checkbox]").each(function () {
            $(this).attr('disabled', 'disabled');
            $(this).prop('checked', false);
        });
        $('#txtIndRespiraOk').removeClass('obg');
        $('#txtIndRespiraOk').val('');
    }

}

function setInternado(tp) {

    if (tp == 1) {
        $('label[for=txtIndInterCausa]').html('Causa ? *');
        $('#txtIndInterCausa').removeAttr('disabled');
        $('#txtIndInterCausa').addClass('obg');
    } else {
        $('label[for=txtIndInterCausa]').html('Causa ?');
        $('#txtIndInterCausa').attr('disabled', 'disabled');
        $('#txtIndInterCausa').removeClass('obg');
        $('#txtIndInterCausa').val('');
    }

}

function setPlanta(tp) {

    if (tp == 1) {
        $('label[for=txtIndPlantaDesc]').html('Qual(is) Planta(s) ? *');
        $('#txtIndPlantaDesc').removeAttr('disabled');
        $('#txtIndPlantaDesc').addClass('obg');
    } else {
        $('label[for=txtIndPlantaDesc]').html('Qual(is) Planta(s) ?');
        $('#txtIndPlantaDesc').attr('disabled', 'disabled');
        $('#txtIndPlantaDesc').removeClass('obg');
        $('#txtIndPlantaDesc').val('');
    }

}

function setSitRua(tp) {

    if (tp == 1) {
        $("#fdsSitRua").find("input[type=radio]").each(function () {
            $(this).removeAttr('disabled');
        });
        $('#txtIndSitRuaTmp').val('');
    } else {
        $("#fdsSitRua").find("input[type=radio]").each(function () {
            $(this).attr('disabled', 'disabled');
            $(this).prop('checked', false);
        });
        $('#txtIndSitRuaTmp').val('');
    }

}

function setInstituicao(tp) {

    if (tp == 1) {
        $('label[for=txtIndOtrInstDesc]').html('Qual(is) Intituição(ões) ? *');
        $('#txtIndOtrInstDesc').removeAttr('disabled');
        $('#txtIndOtrInstDesc').addClass('obg');
    } else {
        $('label[for=txtIndOtrInstDesc]').html('Qual(is) Intituição(ões) ?');
        $('#txtIndOtrInstDesc').attr('disabled', 'disabled');
        $('#txtIndOtrInstDesc').removeClass('obg');
        $('#txtIndOtrInstDesc').val('');
    }

}

function setVisitaFamiliar(tp) {

    if (tp == 1) {
        $('label[for=selIndGrauParente]').html('Grau de Parentesco *');
        $('#selIndGrauParente').removeAttr('disabled');
        $('#selIndGrauParente').addClass('obg');
    } else {
        $('label[for=selIndGrauParente]').html('Grau de Parentesco');
        $('#selIndGrauParente').attr('disabled', 'disabled');
        $('#selIndGrauParente').removeClass('obg');
        $('#selIndGrauParente').val('');
    }

}

function setHigiene(tp) {

    if (tp == 1) {

        $("#fdsHigiene").find("input[type=checkbox]").each(function () {
            $(this).removeAttr('disabled');
        });
        $('#txtIndTemHigieneOk').addClass('obg');
        $('#txtIndTemHigieneOk').val('');
    } else if (tp == 2) {
        check = false;
        $("#fdsHigiene").find("input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                check = true;
            }
        });
        if (check) { $('#txtIndTemHigieneOk').val('1'); } else { $('#txtIndTemHigieneOk').val(''); }
    } else {

        $("#fdsHigiene").find("input[type=checkbox]").each(function () {
            $(this).attr('disabled', 'disabled');
            $(this).prop('checked', false);
        });
        $('#txtIndTemHigieneOk').removeClass('obg');
        $('#txtIndTemHigieneOk').val('');
    }

}

function buscaQuestionario(membro, blk) {

    domicilio = $('#id').val();

    $.ajax({
        type: "POST",
        data: { pessoa: membro },
        datatype: "json",
        url: "ajax/pessoa/getPessoa.asp"
    })
    .done(function (data) {
        $.each(data.resultado, function (ResultadoItens, item) {

            //Dados Pessoais
            $("#txtIndNome").val(item.nome.toCapitalize());
            $("#txtIndNomeSoci").val(item.nome_social.toCapitalize());
            $("#txtIndRespFam").val(item.resp_fam.toCapitalize());
            $("#txtIndDtNasc").val(item.data_nasc);

            $("#txtIndSexo").val(item.sexo);
            $('input[name=rdIndSexo]').each(function () { if ($(this).val() == item.sexo) $(this).prop("checked", true) });

            $("#txtIndSus").val(item.cns);
            $("#txtIndNis").val(item.nis);

            $("#txtIndRaca").val(item.codcor);
            $('input[name=rdIndRaca]').each(function () { if ($(this).val() == item.codcor) $(this).prop('checked', true); });

            $("#txtIndNomeMae").val(item.nome_mae.toCapitalize());
            if (item.nome_mae == '') {
                $('#ckIndNomeMae').attr('checked', 'checked');
                $("#txtIndNomeMae").attr('disabled', 'disabled')
                $("#txtIndNomeMae").val('');
            }

            $("#selIndNacional").val(item.codnacao);
            $("#txtIndUf").val(item.codufnacao);
            $("#txtIndMunicipio").val(item.muninacao);
            $("#txtIndEmail").val(item.email);
            $("#txtIndFone").val(item.foner);
            $("#txtIndCel").val(item.fonep);

        });

        $(".fone").each(function () {
            var phone;
            $(this).unmask();
            phone = $(this).val().replace(/\D/g, '');
            if (phone.length > 10) {
                $(this).mask("(99) 99999-999?9");
            } else {
                $(this).mask("(99) 9999-9999?9");
            }
        });

    });

    $.ajax({
        type: "POST",
        data: { domicilio: domicilio, pessoa: membro },
        datatype: "json",
        url: "ajax/domicilio/getDomicilioInd.asp"
    })
    .done(function (data) {

        $('#idPessoa').val(membro);

        if (data.status) {

            $.each(data.resultado, function (ResultadoItens, item) {

                buscaQuestionarioAnx(item.id_domicilio_ind, "deficiencia", "fdsDeficiencia");
                buscaQuestionarioAnx(item.id_domicilio_ind, "cardiaco", "fdsCardiaco");
                buscaQuestionarioAnx(item.id_domicilio_ind, "renal", "fdsRenal");
                buscaQuestionarioAnx(item.id_domicilio_ind, "repiratorio", "fdsRespira");
                buscaQuestionarioAnx(item.id_domicilio_ind, "higiene", "fdsHigiene");
                buscaQuestionarioAnx(item.id_domicilio_ind, "alimentacao", "fdsAlimenta");

                //Informações Sociodemográficas
                $("#txtIndParente").val(item.grau_parentesco);
                $("#txtIndOcupacao").val(item.ocupacao);
                $("#txtIndSitMercado").val(item.id_tp_sit_mercado);

                $("#txtIndEscola").val(item.escola);
                $('input[name=rdIndEscola]').each(function () { if ($(this).val() == item.escola) $(this).prop('checked', true); });

                $("#txtIndCurso").val(item.tp_curso);

                $("#txtIndCrianca").val(item.id_tp_crianca);
                $('input[name=rdIndCrianca]').each(function () { if ($(this).val() == item.id_tp_crianca) $(this).prop('checked', true); });

                $("#txtIndCuidaTrad").val(item.cuidador_trad);
                $('input[name=rdIndCuidaTrad]').each(function () { if ($(this).val() == item.cuidador_trad) $(this).prop('checked', true); });

                $("#txtIndGrpComuni").val(item.grp_comunitario);
                $('input[name=rdIndGrpComuni]').each(function () { if ($(this).val() == item.grp_comunitario) $(this).prop('checked', true); });

                $("#txtIndPlSaude").val(item.pln_saude);
                $('input[name=rdIndPlSaude]').each(function () { if ($(this).val() == item.pln_saude) $(this).prop('checked', true); });

                $("#txtIndComuniTrad").val(item.comunidade_trad);
                $('input[name=rdIndComuniTrad]').each(function () { if ($(this).val() == item.comunidade_trad) $(this).prop('checked', true); });
                $("#txtIndComuniTradDesc").val(item.comuni_desc);
                setComuTrad(item.comunidade_trad);

                $("#txtIndGenero").val(item.genero);
                setGenero(item.genero);
                $('input[name=rdIndGenero]').each(function () { if ($(this).val() == item.genero) $(this).prop('checked', true); });

                $("#txtIndGeneroTp").val(item.id_tp_sexo_genero);
                $('input[name=rdIndGeneroTp]').each(function () {
                    if ($(this).val() == item.id_tp_sexo_genero) $(this).prop('checked', true);
                    if (item.genero == 1) $(this).removeAttr('disabled');
                });

                $("#txtIndTemDefici").val(item.deficiencia);
                $('input[name=rdIndTemDefici]').each(function () { if ($(this).val() == item.deficiencia) $(this).prop('checked', true); });
                setDeficiencia(item.deficiencia);

                $("#txtIndSaidaCad").val(item.id_tp_saida_cadastro);
                $('input[name=rdIndSaidaCad]').each(function () { if ($(this).val() == item.id_tp_saida_cadastro) $(this).prop('checked', true); });

                //Condições / Situações de Saúde Gerais
                $("#txtIndGestante").val(item.gestante);
                $('input[name=rdIndGestante]').each(function () { if ($(this).val() == item.gestante) $(this).prop('checked', true); });
                $("#txtIndMaternidade").val(item.maternidade);
                setGestante(item.gestante);

                $("#txtIndPeso").val(item.peso);
                $('input[name=rdIndPeso]').each(function () { if ($(this).val() == item.peso) $(this).prop('checked', true); });

                $("#txtIndFumante").val(item.fumante);
                $('input[name=rdIndFumante]').each(function () { if ($(this).val() == item.fumante) $(this).prop('checked', true); });

                $("#txtIndAlcool").val(item.alcool);
                $('input[name=rdIndAlcool]').each(function () { if ($(this).val() == item.alcool) $(this).prop('checked', true); });

                $("#txtIndDrogas").val(item.drogas);
                $('input[name=rdIndDrogas]').each(function () { if ($(this).val() == item.drogas) $(this).prop('checked', true); });

                $("#txtIndHipert").val(item.hipertensao);
                $('input[name=rdIndHipert]').each(function () { if ($(this).val() == item.hipertensao) $(this).prop('checked', true); });

                $("#txtIndDiabete").val(item.diabetes);
                $('input[name=rdIndDiabete]').each(function () { if ($(this).val() == item.diabetes) $(this).prop('checked', true); });

                $("#txtIndAvc").val(item.avc);
                $('input[name=rdIndAvc]').each(function () { if ($(this).val() == item.avc) $(this).prop('checked', true); });

                $("#txtIndInfarto").val(item.infarto);
                $('input[name=rdIndInfarto]').each(function () { if ($(this).val() == item.infarto) $(this).prop('checked', true); });

                $("#txtIndCardiaco").val(item.cardiaco);
                $('input[name=rdIndCardiaco]').each(function () { if ($(this).val() == item.cardiaco) $(this).prop('checked', true); });
                setCardiaco(item.cardiaco);

                $("#txtIndRenal").val(item.renal);
                $('input[name=rdIndRenal]').each(function () { if ($(this).val() == item.renal) $(this).prop('checked', true); });
                setRenal(item.renal);

                $("#txtIndRespira").val(item.respiratorio);
                $('input[name=rdIndRespira]').each(function () { if ($(this).val() == item.respiratorio) $(this).prop('checked', true); });
                setRespiratorio(item.respiratorio);

                $("#txtIndHanse").val(item.hanseniase);
                $('input[name=rdIndHanse]').each(function () { if ($(this).val() == item.hanseniase) $(this).prop('checked', true); });

                $("#txtIndTuber").val(item.tuberculose);
                $('input[name=rdIndTuber]').each(function () { if ($(this).val() == item.tuberculose) $(this).prop('checked', true); });

                $("#txtIndInternado").val(item.interna_12m);
                $('input[name=rdIndInternado]').each(function () { if ($(this).val() == item.interna_12m) $(this).prop('checked', true); });
                $("#txtIndInterCausa").val(item.causa_inter_12m);
                setInternado(item.interna_12m);

                $("#txtIndCancer").val(item.cancer);
                $('input[name=rdIndCancer]').each(function () { if ($(this).val() == item.cancer) $(this).prop('checked', true); });

                $("#txtIndAcamado").val(item.acamado);
                $('input[name=rdIndAcamado]').each(function () { if ($(this).val() == item.acamado) $(this).prop('checked', true); });

                $("#txtIndDomiciliado").val(item.domiciliado);
                $('input[name=rdIndDomiciliado]').each(function () { if ($(this).val() == item.domiciliado) $(this).prop('checked', true); });

                $("#txtIndIntegrativo").val(item.prat_integrativa);
                $('input[name=rdIndIntegrativo]').each(function () { if ($(this).val() == item.prat_integrativa) $(this).prop('checked', true); });

                $("#txtIndPsiqui").val(item.psiquiatra);
                $('input[name=rdIndPsiqui]').each(function () { if ($(this).val() == item.psiquiatra) $(this).prop('checked', true); });

                $("#txtIndPlanta").val(item.planta_medic);
                $('input[name=rdIndPlanta]').each(function () { if ($(this).val() == item.planta_medic) $(this).prop('checked', true); });
                $("#txtIndPlantaDesc").val(item.planta_desc);
                setPlanta(item.planta_medic);

                $("#txtIndOutSa1").val(item.outra1);
                $("#txtIndOutSa2").val(item.outra2);
                $("#txtIndOutSa3").val(item.outra3);

                //Cidadão em Situação de Rua
                $("#txtIndSitRua").val(item.sit_rua);
                $('input[name=rdIndSitRua]').each(function () { if ($(this).val() == item.sit_rua) $(this).prop('checked', true); });
                setSitRua(item.sit_rua);

                $("#txtIndSitRuaTmp").val(item.id_tp_sit_rua);
                $('input[name=rdIndSitRuaTmp]').each(function () {
                    if ($(this).val() == item.id_tp_sit_rua) $(this).prop('checked', true);
                });

                $("#txtIndBeneficio").val(item.beneficio);
                $('input[name=rdIndBeneficio]').each(function () { if ($(this).val() == item.beneficio) $(this).prop('checked', true); });

                $("#txtIndRefFamilia").val(item.ref_familia);
                $('input[name=rdIndRefFamilia]').each(function () { if ($(this).val() == item.ref_familia) $(this).prop('checked', true); });

                $("#txtIndQtdAlimenta").val(item.qtd_alimenta);
                $('input[name=rdIndQtdAlimenta]').each(function () { if ($(this).val() == item.qtd_alimenta) $(this).prop('checked', true); });

                $("#txtIndOtrInst").val(item.outra_institu);
                $('input[name=rdIndOtrInst]').each(function () { if ($(this).val() == item.outra_institu) $(this).prop('checked', true); });
                $("#txtIndOtrInstDesc").val(item.outra_inst_desc);
                setInstituicao(item.outra_institu);

                $("#txtIndTemHigiene").val(item.higiene);
                $('input[name=rdIndTemHigiene]').each(function () { if ($(this).val() == item.higiene) $(this).prop('checked', true); });
                setHigiene(item.higiene);

                $("#txtIndVistFami").val(item.vista_familiar);
                $('input[name=rdIndVistFami]').each(function () { if ($(this).val() == item.vista_familiar) $(this).prop('checked', true); });
                $("#selIndGrauParente").val(item.id_grau_parentesco_vista);
                setVisitaFamiliar(item.vista_familiar);

            });


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

        if (!blk) { blockCampo(3); $('#btnSalvarIndividual').hide(); }
        else { blockCampo(2); $('#btnSalvarIndividual').show(); setMunicipioUf();}

        $("#cadDomicilio").slideUp();
        $("#cadIndividual").slideDown();

    });

}

function buscaQuestionarioAnx(domicilioInd, tipo, element) {

    $.ajax({
        type: "POST",
        data: { domicilioInd: domicilioInd, tipo: tipo },
        datatype: "json",
        url: "ajax/domicilio/getDomicilioIndAnx.asp"
    })
    .done(function (data) {
        if (data.status) {
            $.each(data.resultado, function (ResultadoItens, item) {
                $('#' + element + ' input[type=checkbox]').each(function () {
                    if (item.status == 1) {
                        if ($(this).val() == item.id) $(this).prop('checked', true);
                    }
                });
            });
        }

        setDeficiencia(2);
        setCardiaco(2);
        setRenal(2);
        setRespiratorio(2);
        setHigiene(2);

    });

}

function cancelarIndividual(){
    $("#idPessoa").val('');
    $("#cadIndividual").slideUp();
    $("#cadDomicilio").slideDown();

    $("#cadIndividual").find("input[type=text], input[type=hidden], input[type=email], input[type=checkbox], input[type=radio], select").each(function () {
        if ($(this).context.type == "checkbox" || $(this).context.type == "radio") {
            $(this).prop("checked",false)
        } else {
            if (!$(this).hasClass('total')) {
                $(this).val('');
            }
        }
    });

    if (xBlock) blockCampo(2);

    $('#cadIndividual .nav.nav-tabs li').each(function () { $(this).removeClass('in active') });
    $('#cadIndividual .nav.nav-tabs li:first').each(function () { $(this).addClass('in active') });
    $('#cadIndividual .tab-pane').each(function () { $(this).removeClass('active in') });
    $('#cadIndividual .tab-pane:first').each(function () { $(this).addClass('active in') });

    setPnFamilia(1);

}

function salvaCadastroIndividual() {

    $('#btnSalvarIndividual').attr('disabled', 'disabled');

    $("#cadIndividual").find("input[type=text], input[type=email]").each(function () {
        $('#' + $(this).context.name).val($(this).context.value.trim());
    });

    if (validaCampos($("#cadIndividual"))) {

        var dados = new Object;

        $("#cadIndividual").find("input[type=text], input[type=hidden], input[type=email], input[type=checkbox], input[type=radio], select").each(function () {

            if ($(this).context.type == "checkbox") {
                if ($(this).is(':checked')) {
                    dados[$(this).context.name] = $(this).context.value + ',1';
                } else {
                    dados[$(this).context.name] = $(this).context.value + ',0';
                }
            } else if ($(this).context.type == "radio") {
                if ($(this).is(':checked')) {
                    dados[$(this).context.name] = $(this).context.value.trim();
                }
            } else {
                dados[$(this).context.name] = $(this).context.value.trim();
            }

        });

        dados['idDomicilio'] = $('#id').val();
        dados['idPessoa'] = $('#idPessoa').val();

        $.ajax({
            type: "POST",
            data: dados,
            datatype: "json",
            url: "ajax/domicilio/cadDomicilioInd.asp"
        })
        .done(function (data) {

            if (data.status) {

                modalAlerta('Alteração de Questionário', 'Alteração efetuada com sucesso!');

                setTimeout(function () {
                    $('#alertaModal').modal('hide');
                }, 2000);

                cancelarIndividual();

            } else {

                if (data.sessao) {
                    setTimeout(function () {
                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                        setTimeout(function () {
                            window.location.assign("login.asp");
                        }, 2000);
                    }, 800);
                }

                modalAlerta('Alteração de Questionário', 'Falha ao efutar cadastro!');

            }
        });

    }

    $('#btnSalvarIndividual').removeAttr('disabled');
}

function novaFamilia(tp) {

    if (!tp) {
        $('#txtBuscaNome').val('');
        $("#tblCompFamiliar tbody").empty();
        $('#btnSalvaGrupoFamilia, #btnCancelaGrupoFamilia').hide();
        $('#pnCadFamilia').slideUp();
        $('#pnNewCadFamilia').slideDown();
    } else {
        $('#pnNewCadFamilia').slideUp();
        $('#pnCadFamilia').slideDown();
    }
}

function buscaMunicipe() {

    var xBusca = $('#txtBuscaNome').val();
    xBusca = xBusca.toString();

    if (xBusca.length > 2) {
        $('#listaBuscaNome').slideDown("slow");
        $('#btnGrupoFamilia').hide();

        xDraw = Math.floor((Math.random() * 100) + 1);

        $.ajax({
            type: "POST",
            data: {
                pessoa: xBusca, draw: xDraw
            },
            datatype: "json",
            url: "ajax/pessoa/getPessoa.asp"
        })
        .done(function (data) {
            if (data.status && data.draw == xDraw) {
                $('#listaBuscaNome .list-group-item').remove();
                $('#listaBuscaNome').append('<span class="list-group-item list-group-item-success">Nomes Localizados</span>');

                var qtdBusca = data.resultado.length
                var count = 1

                $.each(data.resultado, function (ResultadoItens, item) {
                    var grupo = false;

                    $.ajax({
                        type: "POST",
                        data: { pessoa: item.id },
                        datatype: "json",
                        url: "ajax/compFamiliar/getCompFamiliar.asp"
                    })
                    .done(function (data2) {
                        if (data2.status) {
                            grupo = true;
                            idGrupo = data2.resultado[0].id_composicao_familiar
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

                        if (grupo) {
                            $('#listaBuscaNome').append('<a href="#nogo" class="list-group-item list-group-item-danger comGrupo" data-id="' + item.id + '" data-nome="' + item.nome.toCapitalize() + '" style="color: #a94442 !important;"><span class="badge">Munícipe já possui Grupo Familiar - ' + idGrupo + '</span> ' + item.nome.toCapitalize() + ' - (' + item.data_nasc + ')</a>');
                        } else {
                            $('#listaBuscaNome').append('<a href="#nogo" class="list-group-item semGrupo" data-id="' + item.id + '" data-nome="' + item.nome.toCapitalize() + '" ><span class="badge">' + item.id + '</span> ' + item.nome.toCapitalize() + ' - (' + item.data_nasc + ')</a>');
                        }

                        if (count == qtdBusca) {
                            executaSelect();
                        }
                        count++;
                    });

                });

                $('#infoResult').show('slow');

            } else {

                if (data.sessao) {
                    setTimeout(function () {
                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                        setTimeout(function () {
                            window.location.assign("login.asp");
                        }, 2000);
                    }, 800);
                }

                $('#listaBuscaNome .list-group-item').remove();
                $('#listaBuscaNome').append('<li class="list-group-item" style="color: red;">Nenhum Munícipe localizado.</li>')
            }
        })
    } else if (xBusca.length == 0) {
        $('#listaBuscaNome .list-group-item').remove();
        $('#listaBuscaNome').hide();
        $('#infoResult').hide();
        $('#listaBuscaNome').empty();
    } else if (xBusca.length > 0 && xBusca.length <= 2) {
        $('#listaBuscaNome .list-group-item').remove();
        $('#listaBuscaNome').append('<li class="list-group-item">Digite no mínimo 03 caracteres!</li>');
        $('#listaBuscaNome').slideDown("slow");
        $('#infoResult').hide();
    }

    //Function que habilita o click nos resultados da busca do Munícipe
    function executaSelect() {

        $('#listaBuscaNome .semGrupo').click(function () {
            $('#compFamiliar').slideDown();

            tipo = '<div class="form-group" style="margin-bottom: 0;"><label for="cboTipo' + $(this).attr("data-id") + '" style="display:none;">Tipo de ' + $(this).attr("data-nome") + '</label><select name="cboTipo' + $(this).attr("data-id") + '" id="cboTipo' + $(this).attr("data-id") + '" data-id="' + $(this).attr("data-id") + '" class="form-control obg cboTipo"><option value ="">Selecione</option><option value ="1">Responsável</option><option value ="2">Dependente</option></select></div>';

            parentesco = '<div class="form-group" style="margin-bottom: 0;"><label for="cboParent' + $(this).attr("data-id") + '" style="display:none;">Parentesco de ' + $(this).attr("data-nome") + '</label><select name="cboParent' + $(this).attr("data-id") + '" id="cboParent' + $(this).attr("data-id") + '" data-id="' + $(this).attr("data-id") + '" class="form-control obg cboParent"><option value ="">Selecione</option><option value ="1">Pessoa Resp.Unid.Fam</option><option value ="2">Conjugê/Companheiro</option><option value ="3">Filho(a)</option><option value ="4">Enteado(a)</option><option value ="5">Neto(a) bisneto(a)</option><option value ="6">Pai ou mãe</option><option value ="7">Sogro(a)</option><option value ="8">Irmão ou irmã</option><option value ="9">Genro ou nora</option><option value ="10">Outro parente</option><option value ="11">Não parente</option><option value ="12">Não informado</option></select></div>';

            btn = '<center><button type="button" title="Excluir" class="btn btn-outline btn-success btn-xs" style="margin-top: 6px;" ><span class="glyphicon glyphicon-remove"></span></button></center>'

            pessoaGrupo = '<tr><td style="padding-top: 12px;" class="text-center cmc">' + $(this).attr("data-id") + '</td><td style="padding-top: 12px;" class="nome">' + $(this).attr("data-nome").toCapitalize() + '</td><td>' + tipo + '</td><td>' + parentesco + '</td><td>' + btn + '</td></tr>';

            $("#tblCompFamiliar tbody").append(pessoaGrupo);

            excluiPessoa();

            blockCampo(2);

            $('#listaBuscaNome .list-group-item').remove();

        });

        $('#listaBuscaNome .comGrupo').click(function () {

        });

        $('#btnSalvaGrupoFamilia, #btnCancelaGrupoFamilia').show();
    }

}

function excluiPessoa() {

    $('#txtBuscaNome').val("");
    $('#infoResult, #listaBuscaNome').slideUp();

    $('#txtBuscaNome').val("");
    $('#infoResult, #listaBuscaNome').slideUp();

    $("#tblCompFamiliar .btn-outline").click(function (event) {

        $('#idPessoaExcluir').val('');
        $('#modalExcluirPessoa #nomePessoaExcluir').html('');

        if ($("#idPessoaGrupo").val().length > 0 && $("#tblCompFamiliar tbody").find("tr").length == 1) {

            $('#alertaSemModalExcluiPessoa').html('A Composição Familiar deve possuir ao menos um mebro.');
            $('#alertaSemModalExcluiPessoa').slideDown('slow');

            setTimeout(function () {
                $('#alertaSemModalExcluiPessoa').slideUp();
            }, 3000);

        } else {
            var cmc = $(this).closest("tr").find(".cmc").html();
            var nome = $(this).closest("tr").find(".nome").html();
            nome = nome.toLowerCase() + ' (' + cmc + ')';

            $('#idPessoaExcluir').val(cmc);
            $('#modalExcluirPessoa #nomePessoaExcluir').html(nome);

            $('#modalExcluirPessoa').modal('show');
        }

    });

    $('#tblCompFamiliar .cboTipo').change(function () {
        if ($(this).val() == "1") {
            $('#tblCompFamiliar').find('.cboTipo').each(function () {
                $(this).val("2");
                if ($('#cboParent' + $(this).data('id')).val() == 1) {
                    $('#cboParent' + $(this).data('id')).val('');
                }
                $('#cboParent' + $(this).data('id')).removeAttr('disabled');
                $('#cboParent' + $(this).data('id') + ' option[value=1]').attr('disabled', 'disabled');
            });

            $(this).val("1");
            $('#cboParent' + $(this).data('id') + ' option[value=1]').removeAttr('disabled');
            $('#cboParent' + $(this).data('id')).attr('disabled', 'disabled');
            $('#cboParent' + $(this).data('id')).val(1);
        }
    })
}

function ConfExcluirPessoa() {

    $('#compFamiliar').find('.cmc').each(function () {
        if ($(this).html() == $('#idPessoaExcluir').val()) {
            $(this).closest("tr").remove();
        }
    });

    $('#modalExcluirPessoa').modal('hide');
    $('#idPessoaExcluir').val('');

}

function salvarGrupo() {

    $('#btnSalvaGrupoFamilia').attr('disabled', 'disabled');

    if ($("#tblCompFamiliar tbody").find("tr").length > 0) {

        if (validaCampos($("#tblCompFamiliar"))) {

            var resp = false;
            var resp2 = false;

            $("#tblCompFamiliar tbody").find(".cboTipo").each(function () {
                if ($(this).val() == "1") {
                    if (resp) {
                        resp2 = true;
                    }
                    resp = true;
                }
            });

            if (resp && !resp2) {

                var codResp = '';

                $("#tblCompFamiliar tbody").find("tr").each(function () {

                    if ($(this).find(".cboTipo").val() == "1") {

                        var dados = new Object;

                        codResp = $(this).find(".cmc").html();

                        dados["idPessoa"] = codResp;
                        dados["idParent"] = $(this).find(".cboParent").val();
                        dados["tp"] = 1;

                        $.ajax({
                            type: "POST",
                            data: dados,
                            datatype: "json",
                            url: "ajax/compFamiliar/cadCompFamiliar.asp"
                        })
                        .done(function (data) {
                            if (data.status) {

                                $("#tblCompFamiliar tbody").find("tr").each(function () {
                                    if ($(this).find(".cboTipo").val() == "2") {
                                        var dados2 = new Object;

                                        dados2["idGrupo"] = data.id_grupo;
                                        dados2["idPessoa"] = $(this).find(".cmc").html();
                                        dados2["idParent"] = $(this).find(".cboParent").val();
                                        dados2["tp"] = 3;

                                        $.ajax({
                                            type: "POST",
                                            data: dados2,
                                            datatype: "json",
                                            url: "ajax/compFamiliar/cadCompFamiliar.asp"
                                        })
                                        .done(function (data) {
                                            if (!data.status) {
                                                if (data.sessao) {
                                                    setTimeout(function () {
                                                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                                                        setTimeout(function () {
                                                            window.location.assign("login.asp");
                                                        }, 2000);
                                                    }, 800);
                                                }
                                            }
                                        })
                                    }
                                });

                                modalAlerta('Composição Familiar', 'Cadastro efetuado com Sucesso!');

                                setTimeout(function () {
                                    $("#alertaModal").modal("hide");

                                    $('#txtBuscaFamilia').val(codResp);
                                    buscaRespFamilia(1);
                                    novaFamilia(1);

                                }, 1000)

                            } else {

                                if (data.sessao) {
                                    setTimeout(function () {
                                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                                        setTimeout(function () {
                                            window.location.assign("login.asp");
                                        }, 2000);
                                    }, 800);
                                }

                                modalAlerta('Composição Familiar', 'Falha ao cadastrar Composição Familiar!');
                            }
                        });

                    }
                });
            } else {
                if (resp2) {
                    modalAlerta('Composição Familiar', 'Selecione apenas um responsável!');
                } else {
                    modalAlerta('Composição Familiar', 'Selecione um responsável!');
                }
            }

        }

    } else {
        modalAlerta('Composição Familiar', 'A Composição deve possuir ao menos um membro!');
    }

    $('#btnSalvaGrupoFamilia').removeAttr('disabled');

}

function novoMunicipe(tp) {

    if (tp) {
        $('#cadMunicipe').slideUp();
        $('#rowBusca').slideDown();
        $("#cadMunicipe").find("input[type=text], input[type=hidden], input[type=email], input[type=checkbox], input[type=radio], select").each(function () {
            if ($(this).context.type == "checkbox") {
                $(this).removeAttr('checked');
            } else if ($(this).context.type == "radio") {
                $(this).removeAttr('checked');
            } else {
                $(this).val('');
            }
        })
        setMuMae();
    } else {
        $('#rowBusca').slideUp();
        $('#cadMunicipe').slideDown();
    }

}

function setMuMae() {
    if ($('#ckMuNomeMae').is(':checked')) {
        $('label[for=txtMuNomeMae]').html('Nome Completo Mãe');
        $('#txtMuNomeMae').attr('disabled', 'disabled');
        $('#txtMuNomeMae').removeClass('obg');
        $('#txtMuNomeMae').val('');
    } else {
        $('label[for=txtMuNomeMae]').html('Nome Completo Mãe *');
        $('#txtMuNomeMae').removeAttr('disabled');
        $('#txtMuNomeMae').addClass('obg');
    }
}

function salvarMunicipe() {

    $('#btnSalvaMunicipe').attr('disabled');

    if (validaCampos($("#cadMunicipe"))) {

        var dados = new Object;

        $("#cadMunicipe").find("input[type=text], input[type=hidden], input[type=email], input[type=checkbox], input[type=radio], select").each(function () {

            if ($(this).context.type == "checkbox") {
                if ($(this).is(':checked')) {
                    dados[$(this).context.name] = $(this).context.value + ',1';
                } else {
                    dados[$(this).context.name] = $(this).context.value + ',0';
                }
            } else if ($(this).context.type == "radio") {
                if ($(this).is(':checked')) {
                    dados[$(this).context.name] = $(this).context.value.trim();
                }
            } else {
                dados[$(this).context.name] = $(this).context.value.trim();
            }

        });

        $.ajax({
            type: "POST",
            data: dados,
            datatype: "json",
            url: "ajax/pessoa/cadPessoa.asp"
        })
        .done(function (data) {
            if (data.status) {

                tipo = '<div class="form-group" style="margin-bottom: 0;"><label for="cboTipo' + data.codigo + '" style="display:none;">Tipo de ' + data.nome.toCapitalize() + '</label><select name="cboTipo' + data.codigo + '" id="cboTipo' + data.codigo + '" data-id="' + data.codigo + '" class="form-control obg cboTipo"><option value ="">Selecione</option><option value ="1">Responsável</option><option value ="2">Dependente</option></select></div>';

                parentesco = '<div class="form-group" style="margin-bottom: 0;"><label for="cboParent' + data.codigo + '" style="display:none;">Parentesco de ' + data.nome + '</label><select name="cboParent' + data.codigo + '" id="cboParent' + data.codigo + '" data-id="' + data.codigo + '" class="form-control obg cboParent"><option value ="">Selecione</option><option value ="1">Pessoa Resp.Unid.Fam</option><option value ="2">Conjugê/Companheiro</option><option value ="3">Filho(a)</option><option value ="4">Enteado(a)</option><option value ="5">Neto(a) bisneto(a)</option><option value ="6">Pai ou mãe</option><option value ="7">Sogro(a)</option><option value ="8">Irmão ou irmã</option><option value ="9">Genro ou nora</option><option value ="10">Outro parente</option><option value ="11">Não parente</option><option value ="12">Não informado</option></select></div>';

                btn = '<center><button type="button" title="Excluir" class="btn btn-outline btn-success btn-xs" style="margin-top: 6px;" ><span class="glyphicon glyphicon-remove"></span></button></center>'

                pessoaGrupo = '<tr><td style="padding-top: 12px;" class="text-center cmc">' + data.codigo + '</td><td style="padding-top: 12px;" class="nome">' + data.nome.toCapitalize() + '</td><td>' + tipo + '</td><td>' + parentesco + '</td><td>' + btn + '</td></tr>';

                $("#tblCompFamiliar tbody").append(pessoaGrupo);

                excluiPessoa();
                novoMunicipe(1);

            } else {

                if (data.sessao) {
                    setTimeout(function () {
                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                        setTimeout(function () {
                            window.location.assign("login.asp");
                        }, 2000);
                    }, 800);
                }

                modalAlerta("Cadastro de Munícipe", "Erro ao cadastrar munícipe!");

            }
        })

    }

    $('#btnSalvaMunicipe').removeAttr('disabled');

}

function locFamilia() {
    $('#listaBuscaFamilia .semDomicilio').siblings().removeClass('list-group-item-success');
    $('#listaBuscaFamilia .semDomicilio').siblings().slideUp();
    $('#listaBuscaFamilia .semDomicilio').siblings().remove();
    $('#listaBuscaFamilia .semDomicilio').addClass('list-group-item-success');
    $('#listaBuscaFamilia .semDomicilio').css('border-radius', '5px');

    $("#modalAddFamilia").find(".has-error").each(function () {
        $(this).removeClass("has-error");
    });

    $("#modalAddFamilia").find("input[type=text]").each(function () {
        $(this).val('');
    });

    $('#modalAddFamilia').modal('show');

    $('#infoResultFamilia').slideUp();
    $('#txtBuscaFamilia').val("");
}