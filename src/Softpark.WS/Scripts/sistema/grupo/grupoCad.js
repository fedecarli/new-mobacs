
$(document).ready(function () {

    var Timer;
    var Intervalo = 800;

    $('.fa-question').click(function () {
        $('#modalNivel').modal('show');
    })

    if ($("#id").val().length > 0) {

        if (parseInt($("#ativoGrupo").val()) == 0) {
            $('#btnDesativar').html('Reativar');
            $('#btnDesativar').removeClass('btn-danger');
            $('#btnDesativar').addClass('btn-primary');
            $('#btnDesativar').attr('onclick', 'confDesativarCadastro("A");');
            $('#btnExcluiUsuario').attr('onclick', 'desativarCadastro(1);');
        }

        setTimeout(function () {
            buscaMenuGrupo();
        }, 800);

    } else {
        $('#btnDesativar').hide();
    }

    $('#txtNome').keyup(function () {
        clearTimeout(Timer);
        Timer = setTimeout(buscaNome, Intervalo);
    });

});

function cancelar() {
    window.location.assign("grupo.asp");
}

function setSetor() {

    if ($('#selectSistema').val() != '') {
        $('#selectSetor').removeAttr('disabled');
        $('#listaMenu thead').hide();
        $('#divBtnSalvar, #divDados').slideDown();
        listaMenu();
    } else {
        $('#selectSetor').attr('disabled', 'disabled');
        $('#selectSetor').val('');
        $('#divBtnSalvar, #divDados').slideUp();
    }

}

function buscaNome() {

    var xNomeAtu = $('#txtNomeAtu').val().trim();
    var xBusca = $('#txtNome').val().trim();
    var xDraw = Math.floor((Math.random() * 100) + 1);

    xBusca = xBusca.toString();

    if (xBusca.length > 4) {
        if (removerAcentos(xBusca) != removerAcentos(xNomeAtu)) {
            $.ajax({
                type: "POST",
                data: { nome: xBusca, draw: xDraw },
                datatype: "json",
                url: "ajax/seguranca/getGrupoNome.asp"
            })
            .done(function (data) {
                if (data.status && data.draw == xDraw) {
                    $('#nomeOK').val('0');
                    $('#alertNome').hide();
                    $('#alertNome').removeClass('alert-warning');
                    $('#alertNome').removeClass('alert-danger');
                    $('#alertNome').removeClass('alert-success');
                    $('#alertNome').addClass('alert-danger');
                    $('#alertNome').html('<i class="fa fa-exclamation-triangle fa-fw"></i> Nome já cadastrado!');
                    $('#alertNome').slideDown('slow');

                    setTimeout(function () {
                        $('#alertNome').slideUp('slow');
                    }, 3500);

                } else {

                    if (data.sessao) {
                        setTimeout(function () {
                            modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                            setTimeout(function () {
                                window.location.assign("login.asp");
                            }, 2000);
                        }, 800);
                    }

                    $('#nomeOK').val('1');
                    $('#alertNome').hide();
                    $('#alertNome').removeClass('alert-warning');
                    $('#alertNome').removeClass('alert-danger');
                    $('#alertNome').removeClass('alert-success');
                    $('#alertNome').addClass('alert-success');
                    $('#alertNome').html('<i class="fa fa-check fa-fw"></i> Nome disponível!');
                    $('#alertNome').slideDown('slow');

                    setTimeout(function () {
                        $('#alertNome').slideUp('slow');
                    }, 3000);

                }

            })
        }
    } else if (xBusca.length > 0) {
        $('#nomeOK').val('0');
        $('#alertNome').hide();
        $('#alertNome').removeClass('alert-warning');
        $('#alertNome').removeClass('alert-danger');
        $('#alertNome').removeClass('alert-success');
        $('#alertNome').addClass('alert-warning');
        $('#alertNome').html('<i class="fa fa-info fa-fw"></i> Nome deve possuir ao menos 5 caracteres!');
        $('#alertNome').slideDown('slow');

        setTimeout(function () {
            $('#alertNome').slideUp('slow');
        }, 3500);
    } else {
        $('#nomeOK').val('0');
        $('#txtNome').val('');
    }
}

function listaMenu() {

    xSistema = $('#selectSistema').val();

    $('#listaMenu tbody').empty();
    $('#listaMenu tbody').append('<tr><td colspan="10" class="text-center">Carregando...</td></tr>');

    $.ajax({
        type: "POST",
        data: { sistema: xSistema },
        datatype: "json",
        url: "ajax/seguranca/getMenu.asp"
    })
    .done(function (data) {
        $('#listaMenu thead').show();
        $('#listaMenu tbody').empty();

        if (data.status) {

            var strTable = ''
            strTable = '<tr><td>&nbsp;</td></tr>';
            $('#listaMenu tbody').append(strTable);

            $.each(data.resultado, function (ResultadoItens, item) {

                strTable =  '<tr class="menu">';
                strTable += '<td class="tb-bg"><span class="fa ' + item.icone + '" style="font-size: 16px;"></span>  ' + item.menu + '  <span class="fa fa-angle-down"></span></td>';
                strTable += '<td class="text-center tb-bg"><span class="fa fa-eye check-all" title="Vizualizar" onclick="checkAll(\'\',\'\',\'view\',' + item.id_menu + ')"> </span><input type="checkbox" name="ver" value="' + item.id_menu + '" class="menu' + item.id_menu + ' view"> </td>';
                strTable += '<td class="text-center tb-bg"><span class="fa fa-pencil check-all" title="Incluir" onclick="checkAll(\'\',\'\',\'add\',' + item.id_menu + ')">  </span><input type="checkbox" name="add" value="' + item.id_menu + '" class="menu' + item.id_menu + ' add">  </td>';
                strTable += '<td class="text-center tb-bg"><span class="fa fa-edit check-all" title="Editar"    onclick="checkAll(\'\',\'\',\'edt\',' + item.id_menu + ')">  </span><input type="checkbox" name="edt" value="' + item.id_menu + '" class="menu' + item.id_menu + ' edt">  </td>';
                strTable += '<td class="text-center tb-bg"><span class="fa fa-times check-all" title="Excluir"  onclick="checkAll(\'\',\'\',\'del\',' + item.id_menu + ')">  </span><input type="checkbox" name="del" value="' + item.id_menu + '" class="menu' + item.id_menu + ' del">  </td>';
                strTable += '<td class="text-center tb-bg"><span class="fa fa-print check-all" title="Imprimir" onclick="checkAll(\'\',\'\',\'prt\',' + item.id_menu + ')">  </span><input type="checkbox" name="prt" value="' + item.id_menu + '" class="menu' + item.id_menu + ' prt">  </td>';
                strTable += '</tr>';
                $('#listaMenu tbody').append(strTable);

                if (item.statusSubMenu) {

                    $.each(item.subMenu, function (ResultadoItens, item2) {

                        strTable = '<tr class="menu">';
                        if (item2.statusSubMenu) {
                            strTable += '<td class="tb-bg"><span style="margin-left: 35px;"></span>  ' + item2.menu + '  <span class="fa fa-angle-down"></span></td>';
                            strTable += '<td class="text-center tb-bg"><span class="fa fa-eye check-all" title="Vizualizar" onclick="checkAll(\'\',\'\',\'view\',' + item2.id_menu + ',' + item.id_menu + ')"> </span><input type="checkbox" name="ver" value="' + item2.id_menu + '" class="menu' + item2.id_menu + ' view' + item.id_menu + ' view" </td>';
                            strTable += '<td class="text-center tb-bg"><span class="fa fa-pencil check-all" title="Incluir" onclick="checkAll(\'\',\'\',\'add\',' + item2.id_menu + ',' + item.id_menu + ')">  </span><input type="checkbox" name="add" value="' + item2.id_menu + '" class="menu' + item2.id_menu + ' add' + item.id_menu + ' add"   </td>';
                            strTable += '<td class="text-center tb-bg"><span class="fa fa-edit check-all" title="Editar"    onclick="checkAll(\'\',\'\',\'edt\',' + item2.id_menu + ',' + item.id_menu + ')">  </span><input type="checkbox" name="edt" value="' + item2.id_menu + '" class="menu' + item2.id_menu + ' edt' + item.id_menu + ' edt"   </td>';
                            strTable += '<td class="text-center tb-bg"><span class="fa fa-times check-all" title="Excluir"  onclick="checkAll(\'\',\'\',\'del\',' + item2.id_menu + ',' + item.id_menu + ')">  </span><input type="checkbox" name="del" value="' + item2.id_menu + '" class="menu' + item2.id_menu + ' del' + item.id_menu + ' del"   </td>';
                            strTable += '<td class="text-center tb-bg"><span class="fa fa-print check-all" title="Imprimir" onclick="checkAll(\'\',\'\',\'prt\',' + item2.id_menu + ',' + item.id_menu + ')">  </span><input type="checkbox" name="prt" value="' + item2.id_menu + '" class="menu' + item2.id_menu + ' prt' + item.id_menu + ' prt"   </td>';
                        } else {
                            strTable += '<td><span class="fa fa-angle-right" style="margin-left: 35px;"></span>  ' + item2.menu + '</td>';
                            strTable += '<td class="text-center"><input type="checkbox" name="ver" value="' + item2.id_menu + '" class="menu' + item2.id_menu + ' view' + item.id_menu + ' view" onclick="checkPai(\'view\',' + item2.id_menu + ',' + item.id_menu + ')"></td>';
                            strTable += '<td class="text-center"><input type="checkbox" name="add" value="' + item2.id_menu + '" class="menu' + item2.id_menu + ' add' + item.id_menu + ' add" onclick="checkPai(\'add\',' + item2.id_menu + ',' + item.id_menu + ')"></td>';
                            strTable += '<td class="text-center"><input type="checkbox" name="edt" value="' + item2.id_menu + '" class="menu' + item2.id_menu + ' edt' + item.id_menu + ' edt" onclick="checkPai(\'edt\',' + item2.id_menu + ',' + item.id_menu + ')"></td>';
                            strTable += '<td class="text-center"><input type="checkbox" name="del" value="' + item2.id_menu + '" class="menu' + item2.id_menu + ' del' + item.id_menu + ' del" onclick="checkPai(\'del\',' + item2.id_menu + ',' + item.id_menu + ')"></td>';
                            strTable += '<td class="text-center"><input type="checkbox" name="prt" value="' + item2.id_menu + '" class="menu' + item2.id_menu + ' prt' + item.id_menu + ' prt" onclick="checkPai(\'prt\',' + item2.id_menu + ',' + item.id_menu + ')"></td>';
                        }
                        strTable += '</tr>';

                        $('#listaMenu tbody').append(strTable);

                        if (item2.statusSubMenu) {

                            $.each(item2.subMenu, function (ResultadoItens, item3) {

                                strTable = '<tr class="menu">';
                                strTable += '<td><span class="fa fa-angle-right" style="margin-left: 70px;"></span>  ' + item3.menu + '</td>';
                                strTable += '<td class="text-center"><input type="checkbox" name="ver" value="' + item3.id_menu + '" class="menu' + item3.id_menu + ' view' + item.id_menu + ' view' + item2.id_menu + ' view" onclick="checkPai(\'view\',' + item3.id_menu + ',' + item2.id_menu + ',' + item.id_menu + ')"></td>';
                                strTable += '<td class="text-center"><input type="checkbox" name="add" value="' + item3.id_menu + '" class="menu' + item3.id_menu + ' add' + item.id_menu + ' add' + item2.id_menu + ' add" onclick="checkPai(\'add\',' + item3.id_menu + ',' + item2.id_menu + ',' + item.id_menu + ')"></td>';
                                strTable += '<td class="text-center"><input type="checkbox" name="edt" value="' + item3.id_menu + '" class="menu' + item3.id_menu + ' edt' + item.id_menu + ' edt' + item2.id_menu + ' edt" onclick="checkPai(\'edt\',' + item3.id_menu + ',' + item2.id_menu + ',' + item.id_menu + ')"></td>';
                                strTable += '<td class="text-center"><input type="checkbox" name="del" value="' + item3.id_menu + '" class="menu' + item3.id_menu + ' del' + item.id_menu + ' del' + item2.id_menu + ' del" onclick="checkPai(\'del\',' + item3.id_menu + ',' + item2.id_menu + ',' + item.id_menu + ')"></td>';
                                strTable += '<td class="text-center"><input type="checkbox" name="prt" value="' + item3.id_menu + '" class="menu' + item3.id_menu + ' prt' + item.id_menu + ' prt' + item2.id_menu + ' prt" onclick="checkPai(\'prt\',' + item3.id_menu + ',' + item2.id_menu + ',' + item.id_menu + ')"></td>';
                                strTable += '</tr>';
                                $('#listaMenu tbody').append(strTable);

                            });

                        }

                    });
                }

                strTable = '<tr><td colspan="6">&nbsp;</td></tr>';
                $('#listaMenu tbody').append(strTable);
            });

        } else {

            $('#listaMenu thead').hide();
            $('#listaMenu tbody').append('<tr><td colspan="10" class="text-center">Nenhum cadastro encontrado</td></tr>');

            if (data.sessao) {
                setTimeout(function () {
                    modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                    setTimeout(function () {
                        window.location.assign("login.asp");
                    }, 2000);
                }, 800);
            }

            if (data.sistema) {
                setTimeout(function () {
                    modalAlerta("Sistema", "Sistema Inválido");
                }, 1500);
            }
        }

        $('#listaMenu').slideDown();

    })

}

function checkAll(hd, val, tp, nm, p1) {

    if (tp == 'view' && val == 2) {
        checkAll('fa-pencil', 2, 'add');
        checkAll('fa-edit', 2, 'edt');
        checkAll('fa-times', 2, 'del');
        checkAll('fa-print', 2, 'prt');
    }

    if (tp == 'add' && val == 1) {
        checkAll('fa-eye', 1, 'view');
    }

    if (tp == 'add' && val == 2) {
        checkAll('fa-edit', 2, 'edt');
        checkAll('fa-times', 2, 'del');
    }

    if (tp == 'edt' && val == 1) {
        checkAll('fa-eye', 1, 'view');
        checkAll('fa-pencil', 1, 'add');
    }

    if (tp == 'edt' && val == 2) {
        checkAll('fa-times', 2, 'del');
    }

    if (tp == 'del' && val == 1) {
        checkAll('fa-eye', 1, 'view');
        checkAll('fa-pencil', 1, 'add');
        checkAll('fa-edit', 1, 'edt');
    }

    if (tp == 'prt' && val == 1) {
        checkAll('fa-eye', 1, 'view');
    }

    if (val == 1) {
        $("#listaMenu tbody").find("." + tp).each(function () {
            $(this).prop('checked', true);
        });
        $('#listaMenu thead .' + hd).attr('onclick', 'checkAll("' + hd + '",2,"' + tp + '");');
    }

    if (val == 2) {
        $("#listaMenu tbody").find("." + tp).each(function () {
            $(this).prop('checked', false);
        });
        $('#listaMenu thead .' + hd).attr('onclick', 'checkAll("' + hd + '",1,"' + tp + '");');
    }

    if (nm) {
        var tem = false;
        $("#listaMenu tbody").find('.' + tp + nm + ', .menu' + nm + '.' + tp).each(function () {
            if ($(this).is(':checked')) {
                tem = true;
            }
        });

        if (tem) {
            $("#listaMenu tbody").find('.' + tp + nm + ', .menu' + nm + '.' + tp).each(function () {
                $(this).prop('checked', false);
            });
        } else {
            $("#listaMenu tbody").find('.' + tp + nm + ', .menu' + nm + '.' + tp).each(function () {
                $(this).prop('checked', true);
            });
        }
    }

    if (p1) {

        var filho = [];
        var cont = 0;

        $("#listaMenu tbody").find("." + tp + p1 + '.' + tp).each(function () {
            if ($(this).is(':checked')) {
                filho[cont] = $(this).val();
                cont++;
            }
        });

        var tem = false;
        $("#listaMenu tbody").find("." + tp + p1 + ', .menu' + p1 + '.' + tp).each(function () {
            if ($(this).val() != $("#listaMenu tbody .menu" + nm + '.' + tp).val() && $(this).val() != $("#listaMenu tbody .menu" + p1 + '.' + tp).val()) {

                if (inArray(filho, $(this).val())) {
                    tem = true;
                }
            }
        });

        if (!tem) {
            $("#listaMenu tbody").find(".menu" + p1 + '.' + tp).each(function () {

                if ($("#listaMenu tbody .menu" + nm + '.' + tp).is(':checked')) {
                    $(this).prop('checked', true);
                } else {
                    $(this).prop('checked', false);
                }

            });
        } else {
            $("#listaMenu tbody").find(".menu" + p1 + '.' + tp).each(function () {
                $(this).prop('checked', true);
            });
        }

    }

}

function checkPai(tp, nm, p1, p2) {

    if (p2) {
        var tem = false;

        $("#listaMenu tbody").find("." + tp + p2 + ', .menu' + p2 + '.' + tp).each(function () {
            if ($(this).val() != $("#listaMenu tbody .menu" + nm + '.' + tp).val() && $(this).val() != $("#listaMenu tbody .menu" + p2 + '.' + tp).val() && $(this).val() != $("#listaMenu tbody .menu" + p1 + '.' + tp).val()) {
                if ($(this).is(':checked')) {
                    tem = true;
                }
            }
        });

        if (!tem) {
            $("#listaMenu tbody").find(".menu" + p2 + '.' + tp).each(function () {
                if ($("#listaMenu tbody .menu" + nm + '.' + tp).is(':checked')) {
                    $(this).prop('checked', true);
                } else {
                    $(this).prop('checked', false);
                }
            });
        }
    }

    if (p1) {

        var tem = false;
        $("#listaMenu tbody").find("." + tp + p1 + ', .menu' + p1 + '.' + tp).each(function () {
            if ($(this).val() != $("#listaMenu tbody .menu" + nm + '.' + tp).val() && $(this).val() != $("#listaMenu tbody .menu" + p1 + '.' + tp).val()) {
                if ($(this).is(':checked')) {
                    tem = true;
                }
            }
        });

        if (!tem) {
            $("#listaMenu tbody").find(".menu" + p1 + '.' + tp).each(function () {
                if ($("#listaMenu tbody .menu" + nm + '.' + tp).is(':checked')) {
                    $(this).prop('checked', true);
                } else {
                    $(this).prop('checked', false);
                }
            });
        }
    }


}

function buscaMenuGrupo() {

    var xId = $("#id").val();

    $.ajax({
        type: "POST",
        data: { grupo: xId },
        datatype: "json",
        url: "ajax/seguranca/getGrupo.asp"
    })
    .done(function (data) {
        if (data.status) {

            $('#nomeOK').val('1');
            $('#selectSistema').val(data.id_sistema);
            $('#selectSetor').val(data.id_unidade);
            $("#txtNome").val(data.grupo);
            $("#txtNomeAtu").val(data.grupo);
            $("#selectNivel").val(data.nivel);

            setSetor();

            setTimeout(function () {
                $.each(data.resultado, function (ResultadoItens, item) {
                    if (item.view) {
                        $("#listaMenu tbody").find(".menu" + item.id_menu + ".view").each(function () {
                            $(this).prop('checked', true);
                        });
                    }

                    if (item.add) {
                        $("#listaMenu tbody").find(".menu" + item.id_menu + ".add").each(function () {
                            $(this).prop('checked', true);
                        });
                    }

                    if (item.edt) {
                        $("#listaMenu tbody").find(".menu" + item.id_menu + ".edt").each(function () {
                            $(this).prop('checked', true);
                        });
                    }

                    if (item.del) {
                        $("#listaMenu tbody").find(".menu" + item.id_menu + ".del").each(function () {
                            $(this).prop('checked', true);
                        });
                    }

                    if (item.prt) {
                        $("#listaMenu tbody").find(".menu" + item.id_menu + ".prt").each(function () {
                            $(this).prop('checked', true);
                        });
                    }
                });
            }, 800);

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

function confDesativarCadastro(tipo) {

    $('#modalExcluirGrupo #nomeGrupoExcluir').html('');

    var nome = $("#txtNome").val();
    nome = nome.toLowerCase();

    if (tipo == "A") {
        $('#modalExcluirGrupo #myModalLabel').html('Reativar Grupo');
        nome = 'O grupo de <b style="text-transform: capitalize;">' + nome + '</b> será reativado no sistema!';
    } else {
        $('#modalExcluirGrupo #myModalLabel').html('Desativar Grupo');
        nome = 'O grupo de <b style="text-transform: capitalize;">' + nome + '</b> será desativado do sistema!';
    }

    $('#modalExcluirGrupo #nomeGrupoExcluir').html(nome);

    $('#modalExcluirGrupo').modal('show');

}

function desativarCadastro(tipo) {

    var grupo = $('#id').val();

    $.ajax({
        type: "POST",
        data: { id: grupo, dsTipo: tipo, dst: true },
        datatype: "json",
        url: "ajax/seguranca/cadGrupo.asp"
    })
    .done(function (data) {

        $("#modalExcluirGrupo").modal("hide");

        if (data.status) {

            if (parseInt($("#ativoUsuario").val()) == 0) {
                modalAlerta('Reativar Grupo', 'Grupo reativado com Sucesso!');
                $("#ativoGrupo").val(1);
                $('#btnDesativar').html('Desativar');
                $('#btnDesativar').attr('onclick', 'confDesativarCadastro();');
                $('#btnExcluiGrupo').attr('onclick', 'desativarCadastro(1);');
                $('#btnDesativar').addClass('btn-danger');
                $('#btnDesativar').removeClass('btn-primary');
            } else {
                modalAlerta('Desativar Grupo', 'Grupo desativado com Sucesso!');
                $("#ativoGrupo").val(0);
                $('#btnDesativar').html('Reativar');
                $('#btnDesativar').attr('onclick', 'confDesativarCadastro("A");');
                $('#btnExcluiGrupo').attr('onclick', 'desativarCadastro(0);');
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

            if (parseInt($("#ativoGrupo").val()) == 0) {
                modalAlerta('Reativar Grupo', 'Erro ao reativar grupo!');
            } else {
                modalAlerta('Desativar Grupo', 'Erro ao desativar grupo!');
            }
        }
    });

}

function salvaCadastro() {

    $('#btnSalvar').attr('disabled', 'disabled');

    $("#formGrupo").find("input[type=text], input[type=email]").each(function () {
        $('#' + $(this).context.name).val($(this).context.value.trim());
    });

    if (validaCampos($("#formGrupo"))) {

        if ($('#nomeOK').val() == '1') {

            var dados = new Object;
            var count = 0;

            $("#formGrupo").find("input[type=text], input[type=hidden], select").each(function () {
                dados[$(this).context.name] = $(this).context.value.trim();
            });

            $("#formGrupo #listaMenu tbody").find("tr.menu").each(function () {

                count++;

                var id_menu = "";
                var menu = "";

                if ($(this).find(".view").is(':checked')) {
                    id_menu = $(this).find(".view").val();
                    menu += '1,';
                } else {
                    id_menu = $(this).find(".view").val();
                    menu += '0,';
                }

                if ($(this).find(".add").is(':checked')) {
                    id_menu = $(this).find(".add").val();
                    menu += '1,';
                } else {
                    id_menu = $(this).find(".add").val();
                    menu += '0,';
                }

                if ($(this).find(".edt").is(':checked')) {
                    id_menu = $(this).find(".edt").val();
                    menu += '1,';
                } else {
                    id_menu = $(this).find(".edt").val();
                    menu += '0,';
                }

                if ($(this).find(".del").is(':checked')) {
                    id_menu = $(this).find(".del").val();
                    menu += '1,';
                } else {
                    id_menu = $(this).find(".del").val();
                    menu += '0,';
                }

                if ($(this).find(".prt").is(':checked')) {
                    id_menu = $(this).find(".prt").val();
                    menu += '1';
                } else {
                    id_menu = $(this).find(".prt").val();
                    menu += '0';
                }

                dados[count] = id_menu + ',' + menu;

            });

            dados["count"] = count;

            $.ajax({
                type: "POST",
                data: dados,
                datatype: "json",
                url: "ajax/seguranca/cadGrupo.asp"
            })
            .done(function (data) {

                if (data.status) {

                    if ($("#id").val() != "") {
                        modalAlerta('Editar Grupo', 'Alteração efetuada com Sucesso!');
                    } else {
                        modalAlerta('Novo Grupo', 'Cadastro efetuado com Sucesso!');
                    }

                    setTimeout(function () {
                        window.location.assign("grupo.asp");
                    }, 1000);

                } else {
                    if (data.sessao) {

                        $('#page-wrapper').css("opacity", "1");
                        $('.imgCarregando').hide();

                        setTimeout(function () {
                            modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                            setTimeout(function () {
                                window.location.assign("login.asp");
                            }, 2000);
                        }, 800);
                    }

                    setTimeout(function () {
                        if ($("#id").val() != "") {
                            modalAlerta('Editar Grupo', 'Erro ao efetuar alteração!');
                        } else {
                            modalAlerta('Novo Grupo', 'Erro ao salvar novo cadastro!');
                        }
                    }, 1000);

                }
            });

        } else {
            modalAlerta('Novo Grupo', 'Nome do grupo está inválido!');
            buscaNome();
        }
    }

    $('#btnSalvar').removeAttr('disabled');
}