
$(document).ready(function () {

    // Capitalize
    String.prototype.toCapitalize = function () {
        return this.toLowerCase().replace(/^.|\s\S/g, function (a) { return a.toUpperCase(); });
    }    

});

function modalAlerta(xTitulo, xTexto) {
    var modalInicio = "<div class='modal fade' id='alertaModal' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'><div class='modal-dialog'><div class='modal-content'><div class='modal-header'><button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button><h4 class='modal-title' id='myModalLabel'>" + xTitulo + "</h4></div><div class='modal-body'>";
    var modalFim = "</div><div class='modal-footer'><button type='button' class='btn btn-default' data-dismiss='modal'>Fechar</button></div></div></div></div>";

    $("#alerta").empty();
    $("#alerta").append(modalInicio + xTexto + modalFim);
    $("#alertaModal").modal("show");
}

/*//fOI CRIADO APENAS PARA FINGIR UM ACESSO POR BIOMETRIA
function carregaBiometria() {
    if ($('#txt_user').val() == 'robinson.ledesma' && $('#txt_pass').val() == '54321') {
        $('#modalBIOMETRIA').modal('show');

        var numShifts = 0;
        $(document).keydown(function (e) {
            if (e.keyCode == 16) {
                numShifts+=1;
            }
        });

        setTimeout(function () {
            $('#modalBIOMETRIA').modal('hide');
            if (numShifts>1) {
                carregasistema();
            } else {
                modalAlerta('Atenção', 'Biometria inválido!');
            }
        }, 5000);
    } else {
        carregasistema();
    }
}
*/
/*//fOI CRIADO PARA UM ACESSO MAIS SEGURO UTILIZANDO UM CARTÃO DE SENHAS, A CHAMADA DESSA FUNÇÃO SUBISTITUI A CHAMADA DA FUNÇÃO carregasistema NA LOGIN.ASP
function carregaTancode() {
    $.ajax({
        type: "POST",
        data: {
            user: $('#txt_user').val(),
            pass: $('#txt_pass').val(),
            NumRef: $('#modalTANCODE .modal-body #NumRef').html(),
            NumSenha: $('#modalTANCODE .modal-body #NumSenha').html(),
            SenhaTan: $('#modalTANCODE .modal-body #SenhaTan').val()
        },
        datatype: "json",
        url: "ajax/login/TanCode.asp"
    })
    .done(function (data) {
        $('#modalTANCODE').modal('hide');
        if (data.status) {
            if (data.resultado.NumSenha) {
                if (data.passo == '1') {
                    $('#modalTANCODE .modal-body #NumSenha').html(data.resultado.NumSenha);
                    $('#modalTANCODE .modal-body #NumRef').html(data.resultado.NumRef);
                    $('#modalTANCODE').modal('show');
                } else {
                    carregaBiometria();
                    $('#modalTANCODE .modal-body #NumSenha').html('');
                    $('#modalTANCODE .modal-body #NumRef').html('');
                    $('#modalTANCODE .modal-body #SenhaTan').val('');
                }
            } else {
                carregaBiometria();
                $('#modalTANCODE .modal-body #NumSenha').html('');
                $('#modalTANCODE .modal-body #NumRef').html('');
                $('#modalTANCODE .modal-body #SenhaTan').val('');
            }
        } else {
            $('#modalTANCODE .modal-body #NumSenha').html('');
            $('#modalTANCODE .modal-body #NumRef').html('');
            $('#modalTANCODE .modal-body #SenhaTan').val('');
            modalAlerta('Atenção', 'Login/Senha inválido!');
        }
    });
    return false;
}
*/
function carregasistema() {

    $("#divUnidade").hide();
    $("#selectSetor").empty();
    //$("#alertaModal").modal("hide");

    var user = $('#txt_user').val();
    var pass = $('#txt_pass').val();

    if (user != "") {

        $.ajax({
            type: "POST",
            data: { user: user, pass: pass},
            datatype: "json",
            url: "ajax/login/sistema.asp"
        })
        .done(function (data) {

            $("#selectSist").empty();

            if (data.status) {
                var cont = 1;
                $.each(data.resultado, function (ResultadoItens, item) {

                    txt = '<i class="fa ' + item.faIcon + ' fa-fw" style="font-size: 28px;"></i><br /> ' + item.descricao;
                    var st = '<button type="button" class="btn ' + item.btnIcon + '" style="margin: 0px 2.5px; width: 125px;" data-sist="' + item.codSistema + '" onclick="buscaSetor(' + item.codUsuario + ', ' + item.codSistema + ',' + data.numContrato + ', 0, this);">' + txt + '</button>';
                    
                    if (cont == 5) {
                        st = '<br><br>' + st;
                        cont = 0;
                    }

                    $("#selectSist").append(st);
                    cont++;
                });
                //O Robison pediu para quando tiver apenas 1 sistema passar direto
                if (data.qtdSistema > 1) {//MOSTRA A ESCOLHA DE SISTEMA SOMENTE SE TIVER MAIS DE 1
                //if (data.qtdSistema > 0) {//MOSTRA A ESCOLHA DE SISTEMA MESMO QUE TENHA APENAS 1

                    $('#modalSistema').modal('show');

                } else {

                    buscaSetor(data.resultado[0].codUsuario, data.resultado[0].codSistema, data.numContrato, 1);

                }


            } else {
                if (data.ativo) {
                    modalAlerta('Atenção', 'Usuário desativado!');
                } else {
                    modalAlerta('Atenção', 'Login/Senha inválido!');
                }
            }

        });
    }

    return false;
}

function buscaSetor(usuario, sistema, contrato, qtdSist, btn) {

    if (qtdSist == 1) {

    }

    var busca = true;
    $('#selectSist').find('.ativo').each(function () {
        if ($(this).data('sist') == $(btn).data('sist')) {
            busca = false;
        }
    });

    if (busca) {
        $("#selectSetor").empty();
        $("#divUnidade").slideUp();

        $('#selectSist').find('.btn').each(function () {
            $(this).removeClass('inactive');
            $(this).removeClass('ativo');
            $(this).addClass('ativo');
        });

        if (qtdSist == 0) {
            $(btn).removeClass('ativo');
            $('#selectSist').find('.ativo').each(function () {
                $(this).addClass('inactive');
                $(this).removeClass('ativo');
            });
            $(btn).addClass('ativo');
        }

        $.ajax({
            type: "POST",
            data: { codUser: usuario, numContrato: contrato },
            datatype: "json",
            url: "ajax/login/setor.asp"
        })
        .done(function (data) {

            $("#selectSetor").attr('data-sist', sistema);
            $("#selectSetor").attr('data-cont', contrato);

            $("#selectSetor").empty();

            if (data.status) {

                $("#selectSetor").append('<option value = "">Selecione a Unidade</option>');

                $.each(data.resultado, function (ResultadoItens, item) {
                    var st = '';
                    var resumido = '';
                    //if (item.setorres.toCapitalize()!='') {
                    //   resumido = item.setorres.toCapitalize() + ' - '
                    //}
                    st = '<option value = "' + item.codSetor + '">' + resumido + item.setor.toCapitalize() + '</option>';

                    $("#selectSetor").append(st);

                });

            } else {
                $("#selectSetor").append('<option value = "">Nenhuma Unidade Cadastrada</option>');
            }

            $("#divUnidade").slideDown();

            //O Robison pediu para quando tiver apenas 1 setor passar direto
            if (data.qtdSetor > 1) {//MOSTRA A ESCOLHA DE SETOR SOMENTE SE TIVER MAIS DE 1
            //if (data.qtdSetor > 0) {//MOSTRA A ESCOLHA DE SETOR MESMO QUE TENHA APENAS 1
            
                $('#modalSistema').modal('show');
            } else {
                $("#selectSetor").val(data.resultado[0].codSetor);
                validaSenhaAdm();
            }

        });
    }
}

function validaSenhaAdm() {

    var sistema = $("#selectSetor").attr('data-sist');
    var contrato = $("#selectSetor").attr('data-cont');
    var setor = $("#selectSetor").val();

    var user = $('#txt_user').val();
    var pass = $('#txt_pass').val();

    if (setor != '') {

        $('#modalSistema').modal('hide');
        $("#alertaModal").modal("hide");

        if (user != "" && pass != "" && sistema != "" && contrato != "") {

            var dados = new Object;
            dados["user"] = user;
            dados["pass"] = pass;
            dados["sistema"] = sistema;
            dados["setor"] = setor;
            dados["contrato"] = contrato;

            $.ajax({
                type: "POST",
                data: dados,
                datatype: "json",
                url: "ajax/login/verificaSenha.asp"
            })
            .done(function (data) {

                $("#alertaModal").modal("hide");

                if (data.status) {

                    modalAlerta('Login', 'Login efetuado com sucesso!');

                    setTimeout(function () {
                        if (sistema == 98 || sistema == 99) {
                            window.location.href = "index.asp";
                        } else {
                            window.location.href = "../assmed20/principal.asp";
                        }
                    }, 500);

                } else {
                    if (data.ativo) {
                        modalAlerta('Atenção', 'Usuário desativado!');
                    } else {
                        modalAlerta('Atenção', 'Unidade inválida!');
                    }
                }

            });
            
        } else {
            modalAlerta('Atenção', 'Unidade inválida!');
        }
    } else {

        $('#alertaSetor').html('Selecione a Unidade!');
        $('#alertaSetor').slideDown();

        setTimeout(function () {
            $('#alertaSetor').slideUp();
        }, 3000);

    }

}

function processaLogin() {

    if ($('#txt_user').val() != "" && $('#txt_pass').val() != "") {

        $("#btn_login").html('Processando Login...')
        $("#btn_login").attr("disabled", "disabled");

        var dados = new Object;
        dados["user"] = $('#txt_user').val();
        dados["pass"] = $('#txt_pass').val();

        $.ajax({
            type: "POST",
            data: dados,
            datatype: "json",
            url: "ajax/login/default.asp"
        })
        .done(function (data) {

            if (data.status) {

                $("#btn_login").html('Acesso autorizado!');
                $("#btn_login").removeClass("btn-primary");
                $("#btn_login").addClass("btn-success");

                setTimeout(function () {
                    window.location.href = "index.asp";
                }, 500);
            } else {

                if (data.userBloq) {
                    $('#messAlertaModal').html('Usuário não existe ou está bloqueado!');
                    $('#modalAlteras').modal('show');
                }

                if (data.bloq) {
                    $('#messAlertaModal').html('O usuário foi <b>bloqueado</b> por excesso de tentativas!<br><br>Entre em contato com o administrador.');
                    $('#modalAlteras').modal('show');
                }

                if (data.qtdLogin > 1 && data.qtdLogin < 5) {
                    var msg = '';
                    var tentativas = 5 - data.qtdLogin;

                    if (tentativas > 1) {
                        msg = 'Restam apenas ' + tentativas + ' tentativas!';
                    } else {
                        msg = 'Resta apenas ' + tentativas + ' tentativa!';
                    }

                    $('#messAlertaModal').html(msg);
                    $('#modalAlteras').modal('show');
                }

                $("#btn_login").removeClass("btn-primary");
                $("#btn_login").addClass("btn-danger");
                $("#btn_login").removeAttr("disabled");
                $("#btn_login").html('Login/Senha inválido!');

                $('#msg_erro').show('slow');

                $('#txt_user').focus();

                setTimeout(function () {
                    $("#btn_login").removeClass("btn-danger");
                    $("#btn_login").addClass("btn-primary");
                    $("#btn_login").html('<span class="glyphicon glyphicon-ok"></span>');
                }, 2000);

            }

        });

    } else {
        $("#btn_login").removeClass("btn-primary");
        $("#btn_login").addClass("btn-danger");
        $("#btn_login").removeAttr("disabled");
        $("#btn_login").html('Login/Senha inválido!');

        $('#msg_erro').show('slow');

        $('#txt_user').focus();

        setTimeout(function () {
            $("#btn_login").removeClass("btn-danger");
            $("#btn_login").addClass("btn-primary");
            $("#btn_login").html('<span class="glyphicon glyphicon-ok"></span>');
        }, 2000);
    }
    return false;
}

