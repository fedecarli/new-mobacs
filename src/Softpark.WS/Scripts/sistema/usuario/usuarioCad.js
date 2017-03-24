
$(document).ready(function () {

    var Timer;
    var Intervalo = 800;

    $('#txtLogin').keyup(function () {
        clearTimeout(Timer);
        Timer = setTimeout(buscaLogin, Intervalo);
    });

    if ($("#id").val().length > 0) {

        $('#btnRestauraSenha').show();

        if (parseInt($("#ativoUsuario").val()) == 0) {
            $('#btnDesativar').html('Reativar');
            $('#btnDesativar').removeClass('btn-danger');
            $('#btnDesativar').addClass('btn-primary');
            $('#btnDesativar').attr('onclick', 'confDesativarCadastro("A");');
            $('#btnExcluiUsuario').attr('onclick', 'desativarCadastro(1);');
        }

        var xId = $("#id").val();

        $.ajax({
            type: "POST",
            data: { usuario: xId },
            datatype: "json",
            url: "ajax/seguranca/getUsuario.asp"
        })
        .done(function (data) {
            if (data.status) {
                $.each(data.resultado, function (ResultadoItens, item) {

                    $('#loginOk').val('1');
                    $("#id").val(item.id);
                    $("#txtNome").val(item.nome.toCapitalize());
                    $("#txtLogin").val(item.login);
                    $("#txtCpf").val(item.cpf);
                    $("#txtFone").val(item.fone);
                    $("#txtEmail").val(item.email);
                    $("#txtCepEnd").val(item.cep);
                    $("#selectTipoEnd").val(item.tipo_end);
                    $("#txtLogradouro").val(item.logradouro);
                    $("#txtNumeroEnd").val(item.numero);
                    $("#txtBairroEnd").val(item.bairro);
                    $("#txtUfEnd").val(item.uf);
                    $("#txtMunicipioEnd").val(item.municipio);
                    $("#txtComplEnd").val(item.complemento);

                    $("#txtLogin").attr('disabled', 'disabled');
                    $("#txtLogin").removeClass('obg');
                    $("label[for=txtLogin]").html('Login');

                });
            }

            mascaras();
        });

        buscaGrupos();

    } else {
        $('#alertSenha').show();
        $('#btnDesativar').hide();
        $('#txtEmail').removeAttr('disabled');
    }

});

function cancelar() {
    window.location.assign("usuario.asp");
}

function buscaLogin() {

    var xBusca = $('#txtLogin').val();
    var xDraw = Math.floor((Math.random() * 100) + 1);

    xBusca = xBusca.toString();

    if (xBusca.length > 4) {

        $.ajax({
            type: "POST",
            data: { login: xBusca, draw: xDraw },
            datatype: "json",
            url: "ajax/seguranca/getLogin.asp"
        })
        .done(function (data) {
            if (data.status && data.draw == xDraw) {
                $('#loginOk').val('0');
                $('#alertLogin').hide();
                $('#alertLogin').removeClass('alert-warning');
                $('#alertLogin').removeClass('alert-danger');
                $('#alertLogin').removeClass('alert-success');
                $('#alertLogin').addClass('alert-danger');
                $('#alertLogin').html('<i class="fa fa-exclamation-triangle fa-fw"></i> Login já cadastrado!');
                $('#alertLogin').slideDown('slow');
                $('#txtLogin').parents(".form-group").addClass("has-error");

                setTimeout(function () {
                    $('#alertLogin').slideUp('slow');
                }, 2500);

            } else {
                $('#loginOk').val('1');
                $('#alertLogin').hide();
                $('#alertLogin').removeClass('alert-warning');
                $('#alertLogin').removeClass('alert-danger');
                $('#alertLogin').removeClass('alert-success');
                $('#alertLogin').addClass('alert-success');
                $('#alertLogin').html('<i class="fa fa-check fa-fw"></i> Login disponível!');
                $('#alertLogin').slideDown('slow');
                $('#txtLogin').parents(".form-group").removeClass("has-error");

                setTimeout(function () {
                    $('#alertLogin').slideUp('slow');
                }, 2500);

            }

        })
    } else if (xBusca.length > 0) {
        $('#loginOk').val('0');
        $('#alertLogin').hide();
        $('#alertLogin').removeClass('alert-warning');
        $('#alertLogin').removeClass('alert-danger');
        $('#alertLogin').removeClass('alert-success');
        $('#alertLogin').addClass('alert-warning');
        $('#alertLogin').html('<i class="fa fa-info fa-fw"></i> Login deve possuir ao menos 5 caracteres!');
        $('#alertLogin').slideDown('slow');

        setTimeout(function () {
            $('#alertLogin').slideUp('slow');
        }, 2500);
    } else {
        $('#loginOk').val('0');
        $('#txtLogin').val('');
    }
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

function buscaGrupos() {

    var xId = $("#id").val();

    if (xId != '') {

        $.ajax({
            type: "POST",
            data: { usuario: xId },
            datatype: "json",
            url: "ajax/seguranca/getGrupoUsuario.asp"
        })
        .done(function (data) {
            if (data.status) {
                $('#listaGrupo tbody').empty();

                $.each(data.resultado, function (ResultadoItens, item) {

                    xSistema = item.sistema;
                    xIdSistema = item.id_sistema;
                    xSetor = item.setor;
                    xIdSetor = item.id_setor;
                    xGrupo = item.grupo;
                    xIdGrupo = item.id_grupo;

                    strTable = '<tr data-id="' + xIdGrupo + ',' + xIdSetor + '" data-sistema="' + xIdSistema + '">';
                    strTable += '<td>' + xGrupo + '</td>';
                    strTable += '<td>' + xSetor + '</td>';
                    strTable += '<td>' + xSistema + '</td>';
                    strTable += '<td class="text-center"><button type="button" title="Remover Grupo" class="btn btn-outline btn-success btn-xs" onclick="removeGrupo(this);"><span class="glyphicon glyphicon-remove"></span></button></td>';
                    strTable += '</tr>';

                    if ($('#listaGrupo tbody').find('td').length <= 1) {
                        $('#listaGrupo tbody').empty();
                    }

                    $('#listaGrupo tbody').append(strTable);

                    gpAtu = $('#txtGrupo').val();
                    if (gpAtu != '') {
                        gpAtu += '/' + xIdGrupo + ',' + xIdSetor + ',' + xIdSistema;
                    } else {
                        gpAtu = xIdGrupo + ',' + xIdSetor + ',' + xIdSistema;
                    }
                    $('#txtGrupo').val(gpAtu);
                });

            }
        });

    }

}

function setSistema() {
    if ($('#selectSistema').val() != '') {
        buscaSetor();
    } else {
        $('#selectSetor').attr('disabled', 'disabled');
        $('#selectSetor').empty();
        $('#selectSetor').append('<option value="">Selecione</option>');
    }
    $('#selectGrupo').attr('disabled', 'disabled');
    $('#selectGrupo').empty();
    $('#selectGrupo').append('<option value="">Selecione</option>');
}

function setSetor() {
    xSistema = $('#selectSistema').val();
    xSetor = $('#selectSetor').val();
    if (xSetor != '') {
        buscaGrupo(xSistema, xSetor);
    } else {
        $('#selectGrupo').attr('disabled', 'disabled');
        $('#selectGrupo').empty();
        $('#selectGrupo').append('<option value="">Selecione</option>');
    }
}

function buscaSetor() {

    $.ajax({
        type: "POST",
        datatype: "json",
        url: "ajax/seguranca/getSetor.asp"
    })
    .done(function (data) {
        $('#selectSetor').empty();
        if (data.status) {
            $('#selectSetor').append('<option value="">Selecione</option>');
            $.each(data.resultado, function (ResultadoItens, item) {
                $('#selectSetor').append('<option value="' + item.codSetor + '">' + item.setor.toCapitalize() + '</option>');
            })
            $('#selectSetor').removeAttr('disabled');
        } else {
            $('#selectSetor').append('<option value="">Nenhum cadastro encontrado</option>');
        }
    })

}

function buscaGrupo(sistema, setor) {

    $.ajax({
        type: "POST",
        data: { sistema: sistema, setor: setor },
        datatype: "json",
        url: "ajax/seguranca/getGrupoUsu.asp"
    })
    .done(function (data) {
        $('#selectGrupo').empty();
        if (data.status) {
            $('#selectGrupo').append('<option value="">Selecione</option>');
            $.each(data.resultado, function (ResultadoItens, item) {
                $('#selectGrupo').append('<option value="' + item.id_grupo + '">' + item.grupo.toCapitalize() + '</option>');
            })
            $('#selectGrupo').removeAttr('disabled');
        } else {
            $('#selectGrupo').append('<option value="">Nenhum cadastro encontrado</option>');
        }
    })

}

function addGrupo() {

    var aux = true;
    var mess = "";

    $("#formUsuario").find(".has-error").each(function () {
        $(this).removeClass("has-error");
    });

    $("#formUsuario").find(".obgGp").each(function () {
        if (!$(this).val().trim()) {
            campo = $("label[for=" + $(this).attr("id") + "]").html();
            campo = campo.replace(/\*/g, "");
            mess = mess + "<h5>Preencha o campo <strong>" + campo + "</strong></h5>";
            $(this).parents(".form-group").addClass("has-error");
            aux = false;
        }
    });

    if (!aux) {
        modalAlerta("Atenção", mess);
    } else {

        xSistema = $('#selectSistema option:selected').text();
        xIdSistema = $('#selectSistema').val();
        xSetor = $('#selectSetor option:selected').text();
        xIdSetor = $('#selectSetor').val();
        xGrupo = $('#selectGrupo option:selected').text();
        xIdGrupo = $('#selectGrupo').val();

        jaAdd = false;
        $('#listaGrupo tbody tr').each(function () {
            if ($(this).attr('data-id') == (xIdGrupo + ',' + xIdSetor)) {
                jaAdd = true;
            }
        })

        if (!jaAdd) {
            strTable = '<tr data-id="' + xIdGrupo + ',' + xIdSetor + '" data-sistema="' + xIdSistema + '">';
            strTable += '<td>' + xGrupo + '</td>';
            strTable += '<td>' + xSetor + '</td>';
            strTable += '<td>' + xSistema + '</td>';
            strTable += '<td class="text-center"><button type="button" title="Remover Grupo" class="btn btn-outline btn-success btn-xs" onclick="removeGrupo(this);"><span class="glyphicon glyphicon-remove"></span></button></td>';
            strTable += '</tr>';

            if ($('#listaGrupo tbody').find('td').length <= 1) {
                $('#listaGrupo tbody').empty();
            }

            $('#listaGrupo tbody').append(strTable);

            gpAtu = $('#txtGrupo').val();
            if (gpAtu != '') {
                gpAtu += '/' + xIdGrupo + ',' + xIdSetor + ',' + xIdSistema;
            } else {
                gpAtu = xIdGrupo + ',' + xIdSetor + ',' + xIdSistema;
            }
            $('#txtGrupo').val(gpAtu);

        } else {
            modalAlerta("Atenção", "O usuário já possui o grupo selecionado!");
        }
    }

}

function removeGrupo(tr) {

    gpAtu = $('#txtGrupo').val();
    gpRemove = $(tr).closest('tr').attr('data-id') + ',' + $(tr).closest('tr').attr('data-sistema');

    gpAtu = gpAtu.replace('/' + gpRemove, '');
    gpAtu = gpAtu.replace(gpRemove + '/', '');

    $('#txtGrupo').val(gpAtu);

    $(tr).closest('tr').remove();

    if ($('#listaGrupo tbody').find('td').length <= 1) {
        $('#listaGrupo tbody').append('<tr><td colspan="10" class="text-center">Nenhum grupo adicionado</td></tr>');
        $('#txtGrupo').val('');
    }

}

function restauraSenha() {

    $('#modalSenhaNome').html($('#txtNome').val());
    $('#modalRestauraSenha').modal('show');
}

function confRestauraSenha() {

    $('#btnSalvaRestauraSenha').attr('disabled', 'disabled');

    xUser = $('#id').val();

    $.ajax({
        type: "POST",
        data: { id: xUser },
        datatype: "json",
        url: "ajax/seguranca/setSenha.asp"
    })
    .done(function (data) {
        $('#modalRestauraSenha').modal('hide');

        if (data.status) {
            modalAlerta('Restauração de Senha', 'Senha restaurada para o padrão com sucesso!');
            setTimeout(function () {
                $('#alertaModal').modal('hide');
            }, 1500);
        } else {
            if (data.sessao) {
                setTimeout(function () {
                    modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                    setTimeout(function () {
                        window.location.assign("login.asp");
                    }, 2000);
                }, 800);
            }
            modalAlerta('Restauração de Senha', 'Falha ao restaurar senha!');
        }
    });

    $('#btnSalvaRestauraSenha').removeAttr('disabled');
}

function confDesativarCadastro(tipo) {

    $('#modalExcluirUsuario #nomeUsuarioExcluir').html('');

    var nome = $("#txtNome").val();
    nome = nome.toLowerCase();

    if (tipo == "A") {
        $('#modalExcluirUsuario #myModalLabel').html('Reativar Usuário');
        nome = '<b style="text-transform: capitalize;">' + nome + '</b> será reativada no sistema!';
    } else {
        $('#modalExcluirUsuario #myModalLabel').html('Desativar Usuário');
        nome = '<b style="text-transform: capitalize;">' + nome + '</b> será desativada do sistema!';
    }

    $('#modalExcluirUsuario #nomeUsuarioExcluir').html(nome);

    $('#modalExcluirUsuario').modal('show');

}

function desativarCadastro(tipo) {

    var usuario = $('#id').val();

    $.ajax({
        type: "POST",
        data: { id: usuario, dsTipo: tipo, dst: true },
        datatype: "json",
        url: "ajax/seguranca/cadUsuario.asp"
    })
    .done(function (data) {

        $("#modalExcluirUsuario").modal("hide");

        if (data.status) {

            if (parseInt($("#ativoUsuario").val()) == 0) {
                modalAlerta('Reativar Usuário', 'Usuário reativado com Sucesso!');
                $("#ativoUsuario").val(1);
                $('#btnDesativar').html('Desativar');
                $('#btnDesativar').attr('onclick', 'confDesativarCadastro();');
                $('#btnExcluiUsuario').attr('onclick', 'desativarCadastro(1);');
                $('#btnDesativar').addClass('btn-danger');
                $('#btnDesativar').removeClass('btn-primary');
            } else {
                modalAlerta('Desativar Usuário', 'Usuário desativado com Sucesso!');
                $("#ativoUsuario").val(0);
                $('#btnDesativar').html('Reativar');
                $('#btnDesativar').attr('onclick', 'confDesativarCadastro("A");');
                $('#btnExcluiUsuario').attr('onclick', 'desativarCadastro(0);');
                $('#btnDesativar').addClass('btn-primary');
                $('#btnDesativar').removeClass('btn-danger');
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

            if (parseInt($("#ativoUsuario").val()) == 0) {
                modalAlerta('Reativar Usuário', 'Erro ao reativar usuário!');
            } else {
                modalAlerta('Desativar Usuário', 'Erro ao desativar usuário!');
            }
        }
    });

}

function salvaCadastro() {
    //if($('#txtLogin').val().length<1){
    $('#txtLogin').val($('#txtEmail').val());
    //}
    $('#btnSalvar').attr('disabled', 'disabled');

    $("#formUsuario").find("input[type=text], input[type=email]").each(function () {
        $('#' + $(this).context.name).val($(this).context.value.trim());
    });

    if (validaCampos($("#formUsuario"))) {

        //if ($('#loginOk').val() == '1') {
        var dados = new Object;

        $("#formUsuario").find("input[type=text], input[type=hidden], input[type=checkbox], select").each(function () {

            if ($(this).context.type == "checkbox") {
                if ($(this).is(':checked')) {
                    dados[$(this).context.name] = '1';
                } else {
                    dados[$(this).context.name] = '0';
                }
            } else {
                dados[$(this).context.name] = $(this).context.value.trim();
            }

        });

        $.ajax({
            type: "POST",
            data: dados,
            datatype: "json",
            url: "ajax/seguranca/cadUsuario.asp"
        })
        .done(function (data) {

            if (data.status) {

                if ($("#id").val() != "") {
                    modalAlerta('Editar Usuário', 'Alteração efetuada com Sucesso!');
                } else {
                    modalAlerta('Novo Usuário', 'Cadastro efetuado com Sucesso!');
                }

                setTimeout(function () {
                    window.location.assign("usuario.asp");
                }, 1000);

            } else {
                if (data.sessao) {
                    setTimeout(function () {
                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                        setTimeout(function () {
                            window.location.assign("login.asp");
                        }, 2000);
                    }, 800);
                }

                setTimeout(function () {
                    if (data.motivo) {
                        modalAlerta('Atenção', data.motivo);
                    } else if ($("#id").val() != "") {
                        modalAlerta('Editar Usuário', 'Erro ao efetuar alteração!');
                    } else {
                        modalAlerta('Novo Usuário', 'Erro ao salvar novo cadastro!');
                    }
                }, 1000);

            }
        });

        //} else {
        //    modalAlerta('Novo Usuário', 'Login está inválido!');
        //    buscaLogin();
        //}
    }

    $('#btnSalvar').removeAttr('disabled');
}