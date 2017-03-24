window.ESUS.register('modal', function (fnSelect) {
    function open() {
        $("#txtModalBusca").val('');

        $('#modalProfSus').find('input[type=text], select').each(function () {
            $(this).parents(".form-group").removeClass("has-error");
        });

        $('#listamodalProfSus tbody').empty();
        $('#listamodalProfSus tbody').append('<tr><th colspan="3" class="text-center">Efetue a busca acima</th></tr>');
        $('#modalProfSus').modal('show').find('#selModalTpBusca').val('2');
    }

    function search() {
        if (validaCampos($("#modalProfSus"), 2, "alertaSemModalProfSus")) {
            var xTbBusca = $('#selModalTpBusca').val();
            var xBusca = removerAcentos($('#txtModalBusca').val().trim());

            $('#listamodalProfSus tbody').empty();
            $('#listamodalProfSus tbody').append('<tr><th colspan="3" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');

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

                        var strTable = '<tr class="sel-cns" data-nome="' + item.nome.toCapitalize() + '" data-cns="' + item.cns + '" data-cbo="' +
                            item.cbo + '" data-cbo_nome="' + item.cbo_nome.toCapitalize() + '">';
                        strTable += '<td class="text-capitalize">' + item.nome.toLowerCase() + '</td>';
                        strTable += '<td class="text-center cns">' + item.cns + '</td>';
                        strTable += '<td class="text-capitalize">' + item.cbo_nome.toLowerCase() + '</td>';
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

                    $('#listamodalProfSus tbody').append('<tr><th colspan="3" class="text-center">Nenhum resultado encontrado!</th></tr>');
                }
            });

        }

        //Function que habilita o click nos resultados da busca do Endereço
        function executaSelect() {
            $('#listamodalProfSus tbody .sel-cns').click(function () {
                if ($(this).find('.cns').html() == '') {
                    modalAlerta("Falta de informações", "Este Funcionário não tem CNS cadastrado");
                } else {
                    //Remove o active dos outros
                    $(this).siblings().removeClass('list-group-item-success');

                    $("#txt_profissionalCNS").val($(this).data('cns')).attr('title', $(this).data('cns'));
                    $("#txtSusProf").val($(this).data('cns'));
                    $("#txtNomeSusProf").val($(this).data('nome')).attr('title', $(this).data('nome'));
                    $("#txt_cboCodigo_2002").val($(this).data('cbo'));
                    $("#txt_cboCodigo_2002Nome").val($(this).data('cbo_nome')).attr('title', $(this).data('cbo_nome'));

                    $('#modalProfSus').modal('hide');
                }
            });
        }
    }

    open();

    $("#selModalTpBusca").unbind('change').on('change', function () {
        $("label[for=txtModalBusca]").text($(this.selectedOptions[0]).text())
        .attr('placeholder', $(this.selectedOptions[0]).text());
    });

    $('#btnBuscaProfSus').unbind('click').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();

        search();
    });
}, 'profissional');