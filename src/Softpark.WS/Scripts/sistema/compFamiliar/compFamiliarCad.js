
var xDraw;

$(document).ready(function () {

    var Timer;
    var Intervalo = 1000;

    $('#txtBuscaNome').keyup(function () {
        clearTimeout(Timer);
        Timer = setTimeout(buscaMunicipe, Intervalo);
    });

    if ($("#idPessoaGrupo").val().length > 0) {

        $("#tblCompFamiliar tbody").empty();

        $.ajax({
            type: "POST",
            data: { pessoa: $("#idPessoaGrupo").val() },
            datatype: "json",
            url: "ajax/compFamiliar/getCompFamiliar.asp"
        })
        .done(function (data) {
            if (data.status) {
                $.each(data.resultado, function (ResultadoItens, item) {

                    $("#idGrupo").val(item.id_composicao_familiar)

                    $.ajax({
                        type: "POST",
                        data: { idPessoa: item.id_pessoa },
                        datatype: "json",
                        url: "ajax/pessoa/getPessoa.asp"
                    })
                    .done(function (data2) {
                        if (data2.status) {
                            $.each(data2.resultado, function (ResultadoItens, pessoaItem) {

                                if (item.chefe) {
                                    tipo = '<div class="form-group" style="margin-bottom: 0;"><label for="cboTipo' + pessoaItem.id + '" style="display:none;">Tipo de ' + pessoaItem.nome.toCapitalize() + '</label><select name="cboTipo' + pessoaItem.id + '" id="cboTipo' + pessoaItem.id + '" data-id="' + pessoaItem.id + '" class="form-control obg cboTipo"><option value ="">Selecione</option><option value ="1" selected>Responsável</option><option value ="2">Dependente</option></select></div>';
                                } else {
                                    tipo = '<div class="form-group" style="margin-bottom: 0;"><label for="cboTipo' + pessoaItem.id + '" style="display:none;">Tipo de ' + pessoaItem.nome.toCapitalize() + '</label><select name="cboTipo' + pessoaItem.id + '" id="cboTipo' + pessoaItem.id + '" data-id="' + pessoaItem.id + '" class="form-control obg cboTipo"><option value ="">Selecione</option><option value ="1">Responsável</option><option value ="2" selected>Dependente</option></select></div>';
                                }

                                parentesco = '<div class="form-group" style="margin-bottom: 0;"><label for="cboParent' + pessoaItem.id + '" style="display:none;">Parentesco de ' + pessoaItem.nome.toCapitalize() + '</label><select name="cboParent' + pessoaItem.id + '" id="cboParent' + pessoaItem.id + '" data-id="' + pessoaItem.id + '" class="form-control obg cboParent"><option value ="">Selecione</option><option value ="1">Pessoa Resp.Unid.Fam</option><option value ="2">Conjugê/Companheiro</option><option value ="3">Filho(a)</option><option value ="4">Enteado(a)</option><option value ="5">Neto(a) bisneto(a)</option><option value ="6">Pai ou mãe</option><option value ="7">Sogro(a)</option><option value ="8">Irmão ou irmã</option><option value ="9">Genro ou nora</option><option value ="10">Outro parente</option><option value ="11">Não parente</option><option value ="12">Não informado</option></select></div>';

                                btn = '<center><button type="button" title="Excluir" class="btn btn-outline btn-success btn-xs" style="margin-top: 6px;" ><span class="glyphicon glyphicon-remove"></span></button></center>'

                                pessoaGrupo = '<tr><td style="padding-top: 12px;" class="text-center cmc">' + pessoaItem.id + '</td><td style="padding-top: 12px;" class="nome">' + pessoaItem.nome.toCapitalize() + '</td><td>' + tipo + '</td><td>' + parentesco + '</td><td>' + btn + '</td></tr>';

                                $("#tblCompFamiliar tbody").append(pessoaGrupo);

                                $('#compFamiliar').slideDown();

                                excluiPessoa();

                                $("#cboParent" + pessoaItem.id).val(item.id_parentesco);

                            });

                            blockCampo(1);

                        }
                    })

                });
            }
        });

        $('#btnSalvaGrupoFamilia, #btnCancelaGrupoFamilia').show()
    }
    
});

function blockCampo(tp) {

    if (tp == 1) {
        $("#tblCompFamiliar tbody").find("select, button").each(function () {
            $(this).attr('disabled', 'disabled');
        });
        $('#rowBusca').slideUp();
        $('#btnSalvaGrupoFamilia').hide();
        $('#btnEditar').show();
    }

    if (tp == 2) {
        $("#tblCompFamiliar tbody").find("select, button").each(function () {
            $(this).removeAttr('disabled');
        });

        $('#tblCompFamiliar .cboTipo option[value=1]:selected').parent('select').each(function () {
            $('#tblCompFamiliar').find('.cboTipo').each(function () {
                $('#cboParent' + $(this).data('id') + ' option[value=1]').attr('disabled', 'disabled');
            });

            $('#cboParent' + $(this).data('id') + ' option[value=1]').removeAttr('disabled');
            $('#cboParent' + $(this).data('id')).attr('disabled', 'disabled');
        })

        $('#btnEditar').hide();
        $('#btnSalvaGrupoFamilia').show();
        $('#rowBusca').slideDown();
    }

}

function cancelarGrupo() {
    window.location.assign("compFamiliar.asp");
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
                        data: {pessoa: item.id },
                        datatype : "json",
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

        $('#btnSalvaGrupoFamilia, #btnCancelaGrupoFamilia').show()
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

function cancelar() {
    $('#rowNovoMunicipe').slideUp('slow');
    $('#txtMunicipioNasc').empty();
    $('#txtMunicipioNasc').append('<option value="">Selecione a UF</option>');
    $("#fform")[0].reset();
    $('#btnNovoMunicipe').fadeIn();
    $('#txtBuscaNome').prop('disabled', false);
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
                $("#tblCompFamiliar tbody").find("tr").each(function () {

                    if ($(this).find(".cboTipo").val() == "1") {

                        var dados = new Object;

                        if ($("#idGrupo").val().length > 0) {
                            dados["idGrupo"] = $("#idGrupo").val();
                            dados["idPessoa"] = $(this).find(".cmc").html();
                            dados["idParent"] = $(this).find(".cboParent").val();
                            dados["tp"] = 2;
                        } else {
                            dados["idPessoa"] = $(this).find(".cmc").html();
                            dados["idParent"] = $(this).find(".cboParent").val();
                            dados["tp"] = 1;
                        }

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

                                if ($("#idGrupo").val().length > 0) {
                                    modalAlerta('Composição Familiar', 'Cadastro alterado com Sucesso!');
                                } else {
                                    modalAlerta('Composição Familiar', 'Cadastro efetuado com Sucesso!');
                                }

                                blockCampo(1);

                                setTimeout(function () {
                                    $("#alertaModal").modal("hide");
                                }, 2500)

                            } else {

                                if (data.sessao) {
                                    setTimeout(function () {
                                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                                        setTimeout(function () {
                                            window.location.assign("login.asp");
                                        }, 2000);
                                    }, 800);
                                }

                                if ($("#idGrupo").val().length > 0) {
                                    modalAlerta('Composição Familiar', 'Falha ao alterar Composição Familiar!');
                                } else {
                                    modalAlerta('Composição Familiar', 'Falha ao cadastrar Composição Familiar!');
                                }
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

function setRdValor(elem) {
    $('#txt' + $(elem).data('rd')).val($(elem).val());
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
                blockCampo(2);
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