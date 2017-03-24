var table;

window.ESUS.register('page', function () {
    var mdProf = this.modal('profissional');
    $('#btnBuscarProf').on('click', function (e) {
        e.preventDefault();
        mdProf();
    });
});

$(document).ready(function () {
    
    table = $('#listaIndividuais').dataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "ajax/dataTables/?xQ=listaIndividuais",
            "type": "POST"
        },
        "language": {
            "sEmptyTable": "<center>Nenhum registro encontrado</center>",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "",
            //"sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoFiltered": "",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "<img src=\"img/ajax-loader-2.gif\" />",
            "sZeroRecords": "<center>Nenhum registro encontrado</center>",
            "sSearch": "Pesquisar ",
            "oPaginate": {
                "sNext": "Próximo",
                "sPrevious": "Anterior",
                "sFirst": "Primeiro",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            },
        },
        "aoColumnDefs": [{
            'bSortable': false,
            'aTargets': ["nao-ordenar"]
            },
            {
                "sClass": "nomeCap",
                "aTargets": [0,2,4]
            },
            {
                "sClass": "text-center",
                "aTargets": [1,2]
            }
        ],
        "order": [[0, "asc"]],

        // Draw Callback DataTables
        "fnDrawCallback": function () {

            $('.nomeCap').each(function () {
                var valor = $(this).html().toLowerCase();
                $(this).html(valor.toCapitalize());
            });

            $('.dataTables_filter button').remove();
            if (permCad) $('.dataTables_filter').prepend('<button type="button" class="btn btn-success pull-right btn-datatable" onclick="MostraFormIndividual(1)">Novo Cadastro</button>');

            if (permAtu) {
                $("#listaIndividuais .btn-outline").click(function () {
                    CarregaIndividual($(this).attr("data-id"));
                }).find('span').addClass('glyphicon-search');
            } else {
                $('#listaIndividuais .permAtu center').hide();
            }

        }
    }).fnSetFilteringDelay(800);

});

function MostraFormIndividual(tp) {
    limpaForm();
    if (tp==1) {
        $('#Busca').slideUp();
        $('#Formu').slideDown();
    }else{
        $('#Formu').slideUp();
        $('#Busca').slideDown();
    }
}

function CarregaIndividual(id) {
    MostraFormIndividual(1);
    buscaQuestionario(id);
}

function cancelar() {
    table.api().ajax.reload();
    MostraFormIndividual(0);
}

function limpaForm() {
    $("#Formu").find("input[type=text], input[type=hidden], input[type=email], input[type=checkbox], input[type=radio], select").each(function () {
        if ($(this).context.type == "checkbox" || $(this).context.type == "radio") {
            $(this).prop("checked", false)
        } else {
            if (!$(this).hasClass('total')) {
                $(this).val('');
            }
        }
    });
}

//Funções do Felipe
//function busca_Prof_Sus() {

//    $("#selModalTpBusca").val('1');
//    $("#txtModalBusca").val('');
//    $("label[for=txtModalBusca]").text('Nome');
//    $("#txtModalBusca").attr('placeholder', 'Número');

//    $('#modalProfSus').find('input[type=text], select').each(function () {
//        $(this).parents(".form-group").removeClass("has-error");
//    });

//    $('#listamodalProfSus tbody').empty();
//    $('#listamodalProfSus tbody').append('<tr><th colspan="3" class="text-center">Efetue a busca acima</th></tr>');
//    $('#modalProfSus').modal('show').find('#selModalTpBusca').val('2');

//    $("#selModalTpBusca").on('change', function () {
//        $("label[for=txtModalBusca]").text($(this.selectedOptions[0]).text());
//    });
//}

//function prof_Sus() {

//    if (validaCampos($("#modalProfSus"), 2, "alertaSemModalProfSus")) {
//        var xTbBusca = $('#selModalTpBusca').val();
//        var xBusca = removerAcentos($('#txtModalBusca').val().trim());

//        $('#listamodalProfSus tbody').empty();
//        $('#listamodalProfSus tbody').append('<tr><th colspan="3" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');

//        $.ajax({
//            type: "POST",
//            data: { tpBusca: xTbBusca, busca: xBusca },
//            datatype: "json",
//            url: "ajax/domicilio/getProfissional.asp"
//        })
//        .done(function (data) {

//            $('#listamodalProfSus tbody').empty();

//            if (data.status) {

//                $.each(data.resultado, function (ResultadoItens, item) {

//                    var strTable = '<tr class="sel-cns" data-nome="' + item.nome.toCapitalize() + '" data-cns="' + item.cns + '" data-cbo="' +
//                        item.cbo + '" data-cbo_nome="' + item.cbo_nome.toCapitalize()+ '">';
//                    strTable += '<td class="text-capitalize">' + item.nome.toLowerCase() + '</td>';
//                    strTable += '<td class="text-center cns">' + item.cns + '</td>';
//                    strTable += '<td class="text-capitalize">' + item.cbo_nome.toLowerCase() + '</td>';
//                    strTable += '</tr>';

//                    $('#listamodalProfSus tbody').append(strTable);
//                });

//                executaSelect();

//            } else {

//                if (data.sessao) {
//                    setTimeout(function () {
//                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
//                        setTimeout(function () {
//                            window.location.assign("login.asp");
//                        }, 2000);
//                    }, 800);
//                }

//                $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center">Nenhum resultado encontrado!</th></tr>');
//            }
//        });

//    }

//    //Function que habilita o click nos resultados da busca do Endereço
//    function executaSelect() {
//        $('#listamodalProfSus tbody .sel-cns').click(function () {
//            if ($(this).find('.cns').html() == '') {
//                modalAlerta("Falta de informações", "Este Funcionário não tem CNS cadastrado");
//            } else {
//                //Remove o active dos outros
//                $(this).siblings().removeClass('list-group-item-success');

//                $("#txt_profissionalCNS").val($(this).data('cns')).attr('title',$(this).data('cns'));
//                $("#txtSusProf").val($(this).data('cns'));
//                $("#txtNomeSusProf").val($(this).data('nome')).attr('title', $(this).data('nome'));
//                $("#txt_cboCodigo_2002").val($(this).data('cbo'));
//                $("#txt_cboCodigo_2002Nome").val($(this).data('cbo_nome')).attr('title', $(this).data('cbo_nome'));

//                $('#modalProfSus').modal('hide');
//            }
//        });
//    }

//    return false;

//}

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
        $('label[for=txtIndUf]').html($('label[for=txtIndUf]').html().replace(' *', ''));
        $('#txtIndUf').removeClass('obg');
        $('#txtIndUf').attr('disabled', 'disabled');
        $('#txtIndUf').val('');

        $('label[for=txtIndMunicipio]').html($('label[for=txtIndMunicipio]').html().replace(' *',''));
        $('#txtIndMunicipio').removeClass('obg');
        $('#txtIndMunicipio').attr('disabled', 'disabled');
        $('#txtIndMunicipio').val('');

        $('#selIndNacional').removeAttr('disabled');
    } else {
        $('label[for=txtIndUf]').html($('label[for=txtIndUf]').html().replace(' *', '') + ' *');
        $('#txtIndUf').addClass('obg');
        $('#txtIndUf').removeAttr('disabled');

        $('label[for=txtIndMunicipio]').html($('label[for=txtIndMunicipio]').html().replace(' *', '') + ' *');
        $('#txtIndMunicipio').addClass('obg');
        $('#txtIndMunicipio').removeAttr('disabled');

        $('#selIndNacional').attr('disabled', 'disabled');
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
            $("#txtIndRespFam").val(item.resp_status);
            $('input[name=rdIndRespFam]').each(function () { if ($(this).val() == item.resp_status) $(this).prop("checked", true) });
            $("#txtDTsusRespFam,#txtDTsusRespFam2").val(item.resp_dt_nasc);
            $("#txtNsusRespFam,#txtNsusRespFam2").val(item.resp_cns);
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
            carregaMunicipio('txtIndUf', 'txtIndMunicipio', item.muninacao);
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

                //Dados do Profissional
                $('#txtNomeSusProf').val(item.respNome)
                $('#txtNumSusProf').val(item.num_sus_prof)
                $('#selCnesUni').val(item.num_cnes_uni)
                carregaINE('selCnesUni', 'txtIneEquipe', item.num_ine_eqp);
                //$('#txtIneEquipe').val(item.num_ine_eqp)
                $('#txtMicroarea').val(item.microarea)
                $('#txtData').val(item.data_visita)

                //Dados Pessoais
                $("#txtNacionalidade").val(item.nacionalidade);
                $('input[name=rdIndNaci]').each(function () { if ($(this).val() == item.nacionalidade) $(this).prop('checked', true); });


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

        /*if (!blk) { blockCampo(3); $('#btnSalvarIndividual').hide(); }
        else { blockCampo(2); $('#btnSalvarIndividual').show(); setMunicipioUf(); }
        */$('#btnSalvarIndividual').show(); setMunicipioUf();
        $("#cadDomicilio").slideUp();
        $("#Formu").slideDown();

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

function cancelarIndividual() {
    $("#idPessoa").val('');
    $("#Formu").slideUp();
    $("#cadDomicilio").slideDown();

    $("#Formu").find("input[type=text], input[type=hidden], input[type=email], input[type=checkbox], input[type=radio], select").each(function () {
        if ($(this).context.type == "checkbox" || $(this).context.type == "radio") {
            $(this).prop("checked", false)
        } else {
            if (!$(this).hasClass('total')) {
                $(this).val('');
            }
        }
    });

    //if (xBlock) blockCampo(2);

    $('#cadIndividual .nav.nav-tabs li').each(function () { $(this).removeClass('in active') });
    $('#cadIndividual .nav.nav-tabs li:first').each(function () { $(this).addClass('in active') });
    $('#cadIndividual .tab-pane').each(function () { $(this).removeClass('active in') });
    $('#cadIndividual .tab-pane:first').each(function () { $(this).addClass('active in') });

    setPnFamilia(1);

}

function salvaCadastroIndividual() {

    $('#btnSalvarIndividual').attr('disabled', 'disabled');

    $("#Formu").find("input[type=text], input[type=email]").each(function () {
        $('#' + $(this).context.name).val($(this).context.value.trim());
    });

    if (validaCampos($("#Formu"))) {
        //validar os cns's
        if ($('#txtIndSus').val() != '') {
            if (!validateCNS($('#txtIndSus').val())) { modalAlerta('Erro no CNS', 'Nº Cartão SUS incorreto'); $('#btnSalvarIndividual').removeAttr('disabled'); return false; }
        }
        if ($('#txtNsusRespFam').val() != '') {
            if (!validateCNS($('#txtNsusRespFam').val())) { modalAlerta('Erro no CNS', 'Nº Cartão SUS do Responsável incorreto'); $('#btnSalvarIndividual').removeAttr('disabled'); return false; }
        }
        var dados = new Object;

        $("#Formu").find("input[type=text], input[type=hidden], input[type=email], input[type=checkbox], input[type=radio], select").each(function () {

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

                //cancelarIndividual();
                cancelar();
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

function carregaMunicipio(idUf, idMunicipio, val) {
    $.ajax({
        type: "POST",
        data: { muni: $('#' + idUf).val() },
        datatype: "json",
        url: "ajax/cep/getMunicipio.asp"
    })
    .done(function (data) {
        if (data.status) {
            $('#' + idMunicipio).empty();
            $('#' + idMunicipio).append('<option value="">Selecione</option>');

            $.each(data.resultado, function (ResultadoItens, item) {
                $('#' + idMunicipio).append('<option value="' + item.toString() + '">' + item.toString() + '</option>');
            });
        } else {
            $('#' + idMunicipio).empty();
            $('#' + idMunicipio).append('<option value="">Selecione a UF</option>');
        }
        $('#' + idMunicipio).val(val);
    });
}

function blockCampo(tp) {

    if (tp == 1) {
        $("#formGrupo").find("input[type=text], input[type=email], input[type=checkbox], input[type=radio], select,.input-group-btn button,#btnNovoAdicionar,.btn-outline").each(function () {
            $(this).attr('disabled', 'disabled');
        });

        $("#formGrupo").find("#pnListaFamilia button.btn-eye, #pnListaFamilia button.back").each(function () {
            $(this).removeAttr('disabled');
        });

        $('#btnSalvar, #btnSalvarIndividual').hide();
        $('#btnEditar').show();
    }

    if (tp == 2) {
        $("#formGrupo").find("input[type=text], input[type=email], input[type=checkbox], input[type=radio], select,.input-group-btn button,#btnNovoAdicionar,.btn-outline").each(function () {
            $(this).removeAttr('disabled');
        });

        setAnimal($('#txtTemAnimais').val());

        $('#txtNomeSusProf, #txtNumSusProf, #selectTipoEnd, #txtLogradouro, #txtBairroEnd, #txtUfEnd, #txtMunicipioEnd').attr('disabled', 'disabled');
        $('#btnEditar').hide();

        //if (!xPnFamilia) $('#btnSalvar, #btnSalvarIndividual').show();
        //else $('#btnSalvarIndividual').show();



        $("#Formu").find("#fdsDeficiencia input[type=checkbox], #fdsCardiaco input[type=checkbox], #fdsRenal input[type=checkbox], #fdsRespira input[type=checkbox], #fdsHigiene input[type=checkbox], #rdTpGenero input[type=radio], #fdsSitRua input[type=radio], #txtIndRespFam, #txtIndParente, #txtIndComuniTradDesc, #txtIndMaternidade, #txtIndInterCausa, #txtIndPlantaDesc, #txtIndOtrInstDesc, #selIndGrauParente").each(function () {
            $(this).attr('disabled', 'disabled');
        });

        //xBlock = true;
        //buscaFamilias($("#id").val(), 1);
    }

    if (tp == 3) {
        $("#Formu").find("input[type=text], input[type=email], input[type=checkbox], input[type=radio], select,.input-group-btn button,#btnNovoAdicionar,.btn-outline").each(function () {
            $(this).attr('disabled', 'disabled');
        });
    }

}

function setRdValor(elem) {
    $('#txt' + $(elem).data('rd')).val($(elem).val());
}
//Funções do Felipe

//function carregaINE(idSetor, idINE, val) {
//    if ($(idSetor).val()) {
//        CodSetor = $('option:selected', idSetor).data('idsetor');
//        $.get('../assmed20/json/getINESetor.asp?CodSetor=' + CodSetor)
//        .done(function (data) {
//            if (data.status) {
//                $('#' + idINE).html('<option value="">SELECIONE UMA EQUIPE</option>');
//                $.each(data.resultado, function (ResultadoItens, item) {
//                    $('#' + idINE).append('<option value="' + item.INE + '">' + item.Descricao + '</option>');
//                });
//                $('#' + idINE).val(val);
//                $('label[for=' + idINE + ']').html($('label[for=' + idINE + ']').html());
//                $('#' + idINE).addClass('obg');
//            } else {
//                $('#' + idINE).html('<option value="">NÃO HÁ EQUIPES</option>');
//                $('label[for=' + idINE + ']').html($('label[for=' + idINE + ']').html());
//                $('#' + idINE).removeClass('obg');
//            }
//        });
//    }
//}






function busca_Municipe_Resp() {

    $("#selModalRespFamiliarTpBusca").val('1');
    $("#txtModalRespFamiliarBusca").val('');
    $("#txtModalRespFamiliarBusca").attr('placeholder', 'Número');

    $('#modalRespFamiliar').find('input[type=text], select').each(function () {
        $(this).parents(".form-group").removeClass("has-error");
    });

    $('#listamodalRespFamiliar tbody').empty();
    $('#listamodalRespFamiliar tbody').append('<tr><th colspan="2" class="text-center">Efetue a busca acima</th></tr>');
    $('#modalRespFamiliar').modal('show');
}

function tpBuscaModalRespFamiliar() {

    if ($("#selModalRespFamiliarTpBusca").val() == 1) {
        $("#txtModalRespFamiliarBusca").val('');
        $("#txtModalRespFamiliarBusca").attr('placeholder', 'Número');
    } else {
        $("#txtModalRespFamiliarBusca").val('');
        $("#txtModalRespFamiliarBusca").attr('placeholder', 'Nome');
    }

}

function RespFamiliar() {

    if (validaCampos($("#modalRespFamiliar"), 2, "alertaSemModalRespFamiliar")) {
        var xTbBusca = $('#selModalRespFamiliarTpBusca').val();
        var xBusca = removerAcentos($('#txtModalRespFamiliarBusca').val().trim());

        $('#listamodalRespFamiliar tbody').empty();
        $('#listamodalRespFamiliar tbody').append('<tr><th colspan="2" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');

        $.ajax({
            type: "POST",
            data: { pessoa: xBusca, resp: 1 },
            datatype: "json",
            url: "ajax/pessoa/getPessoa.asp"
        })
        .done(function (data) {

            $('#listamodalRespFamiliar tbody').empty();

            if (data.status) {

                $.each(data.resultado, function (ResultadoItens, item) {

                    var strTable = '<tr class="sel-cns" data-nome="' + item.nome.toCapitalize() + '" data-cns="' + item.cns + '" data-dtnasc="' + item.data_nasc + '" >';
                    strTable += '<td class="text-capitalize">' + item.nome.toLowerCase() + '</td>';
                    strTable += '<td class="text-center cns">' + item.cns + '</td>';
                    strTable += '</tr>';

                    $('#listamodalRespFamiliar tbody').append(strTable);
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

                $('#listamodalRespFamiliar tbody').append('<tr><th colspan="2" class="text-center">Nenhum resultado encontrado!</th></tr>');
            }
        });

    }

    //Function que habilita o click nos resultados da busca do Endereço
    function executaSelect() {
        $('#listamodalRespFamiliar tbody .sel-cns').click(function () {
            if ($(this).find('.cns').html() == '') {
                modalAlerta("Falta de informações", "Este Munícipe não tem CNS cadastrado");
            } else {
                //Remove o active dos outros
                $(this).siblings().removeClass('list-group-item-success');

                $("#txtNsusRespFam").val($(this).data('cns'));
                $("#txtDTsusRespFam").val($(this).data('dtnasc'));
                $('#modalRespFamiliar').modal('hide');
            }
        });
    }

    return false;

}
function AdicionarFamilia() {
    $('#pnListaFamilia').find(".form-group").removeClass("has-error");

    if ($('#txtNovoRespSUS').val() == '') {
        $('#txtNovoRespSUS').parents('.form-group').addClass("has-error");
        modalAlerta("Falta de informações", "Selecione um Munícipe com CNS");
        return false;
    }

    if (!validateCNS($('#txtNovoRespSUS').val())) {
        $('#txtNovoRespSUS').parents('.form-group').addClass("has-error");
        modalAlerta("Erro de informações", "Número CNS inválido");
        return false;
    }

    var jaExiste = false;
    $('#listaFamilia .RespSUS').each(function () {
        if ($(this).html() == $('#txtNovoRespSUS').val()) {
            jaExiste = true;
        }
    })

    if (jaExiste) {
        $('#txtNovoRespSUS').parents('.form-group').addClass("has-error");
        modalAlerta("Erro", "Este Responsável ja está neste Domicílio");
        return false;
    }


    if ($('#txtNovoDataNasc').val() != '') {
        var patt = new RegExp("^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[012])/[12][0-9]{3}$");
        var res = patt.test($('#txtNovoDataNasc').val());
        if (!res) {
            $('#txtNovoDataNasc').parents('.form-group').addClass("has-error");
            modalAlerta("Falta de informações", "Data de nascimento do responsável Inválida");
            return false;
        }
    }
    if ($('#txtResideMM').val() != '') {
        if ($('#txtResideMM').val() < 1 || $('#txtResideMM').val() > 12) {
            $('#txtResideMM').parents('.form-group').addClass("has-error");
            modalAlerta("Falta de informações", "Reside desde(mês) Inválido");
            return false;
        }

        var D = new Date();
        if ($('#txtResideAAAA').val() < 1900 || $('#txtResideAAAA').val() > D.getFullYear()) {
            $('#txtResideAAAA').parents('.form-group').addClass("has-error");
            modalAlerta("Falta de informações", "Reside desde(ano) Inválido");
            return false;
        }
    }

    var selNovoRendaVal = '';
    var selNovoRendatex = '';

    if ($('#selNovoRenda option:selected').val() != '') {
        selNovoRendaVal = $('#selNovoRenda option:selected').val();
        selNovoRendatex = $('#selNovoRenda option:selected').text();
    }
    if ($('#chkNovoMudou:checked').val()) {
        chkNovoMudou = 'Sim';
    } else {
        chkNovoMudou = 'Não';
    }
    $('#listaFamilia tbody').append(
        formataFamilias(
            $('#txtNovoPront').val(),
            $('#txtNovoRespSUS').val(),
            $('#txtNovoDataNasc').val(),
            selNovoRendaVal,
            selNovoRendatex,
            $('#txtMembros').val(),
            $('#txtResideMM').val(),
            $('#txtResideAAAA').val(),
            chkNovoMudou)
    );

}


function buscaDatabyCNS(cns) {
    if (!validateCNS(cns)) {
        $('#txtNovoRespSUS').parents('.form-group').addClass("has-error");
        modalAlerta("Erro de CNS", "Nº Cartão SUS inválido");
        return false;
    }else{
        modalAlerta('Erro no CNS', 'Nº Cartão SUS do Responsável incorreto'); 
        return false;
    }
    $.ajax({
        type: "POST",
        data: { pessoa: cns, resp: 1 },
        datatype: "json",
        url: "ajax/pessoa/getPessoa.asp"
    })
    .done(function (data) {
        if (data.status) {
            $.each(data.resultado, function (ResultadoItens, item) {
                $('#txtDTsusRespFam').val(item.data_nasc);
            })
        }
    })
}

function setResponsavel(n) {
    if (n == 1) {
        var cns = $('#txtIndSus');
        var cnsValor = cns.val();
        $('#txtNsusRespFam,#btnNovoRespSUS').val('').attr('disabled', 'disabled');

        $('#txtIndSus').change(function () {
            $('#txtNsusRespFam').val($('#txtIndSus').val());
        });
        $('#txtIndDtNasc').change(function () {
            $('#txtDTsusRespFam').val($('#txtIndDtNasc').val());
        });

        if (cnsValor.length == 15) {
            if (validateCNS(cnsValor)) {
                $('#txtNsusRespFam').val(cnsValor);
                $('#txtDTsusRespFam').val($('#txtIndDtNasc').val());
            } else {
                $('#txtIndSus').parents('.form-group').addClass("has-error");
                modalAlerta("Erro de CNS", "Nº Cartão SUS inválido");
                return false;
            }
        } else {
            modalAlerta("Erro de CNS", "Nº Cartão SUS incompleto");
            return false;
        }
    } else {
        $('#txtIndSus,#txtIndDtNasc').change('');
        $('#txtNsusRespFam,#btnNovoRespSUS').removeAttr('disabled');
        $('#txtNsusRespFam').val($('#txtNsusRespFam2').val());
        $('#txtDTsusRespFam').val($('#txtDTsusRespFam2').val());
    }
}