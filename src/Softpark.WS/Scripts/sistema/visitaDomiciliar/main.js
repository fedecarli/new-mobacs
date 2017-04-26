$(document).ready(function () {
    $('#_listaVisitaDomiciliar').dataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "odata/listaVisitaDomiciliar",
            "type": "POST"
        },
        "language": {
            "sEmptyTable": "<center>Nenhum registro encontrado</center>",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "",
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
                "sClass": "text-center",
                "aTargets": [0, 1]
            },
            {
                "sClass": "nomeCap",
                "aTargets": [2, 3]
            },
            {
                "sClass": "permAtusdf",
                "aTargets": [5]
            },
            {
                "sClass": "text-center Stt",
                "aTargets": [4]
            }
        ],
        "order": [[0, "desc"]],
        "fnDrawCallback": function () {
            $('.Stt:contains(Não Finalizada)').parents('tr').find('td').css('color', 'red');
            $('.nomeCap').each(function () {
                var valor = $(this).html().toLowerCase();
                $(this).html(valor.toCapitalize());
            });

            $('.dataTables_filter button').remove();
            if (permCad) $('.dataTables_filter').prepend('<button type="button" id="btnCadVisitaDomiciliar" class="btn btn-success pull-right btn-datatable"  onclick="cadVisitaDomiciliar()">Novo Cadastro</button>');

            if (permAtu) {
                $("#btnCadVisitaDomiciliar").prop('disabled', false);
                $("#listaVisitaDomiciliar .btn-outline").click(function () {
                    $("#visitaDomiciliar").val($(this).attr("data-id"));
                    $("#listaVisitaDomiciliar").submit();
                    editVisitaDomiciliar($(this).data("id"));
                });
            } else {
                $('#listaVisitaDomiciliar .permAtu center').hide();
            }
        }
    }).fnSetFilteringDelay(800);
});

function cadVisitaDomiciliar() {
    $("#btnCadVisitaDomiciliar").prop('disabled', true);
    jQuery.ajax({
        type: "POST",
        async: false,
        data: {
            acao: "create",
            digitadoPor: "",
            dataVisita: dataAtualFormatada(),
            numeroFolha: 0,
            codigoCnesUnidade: 0,
            dataAtendimento: dataAtualFormatada()
        },
        datatype: "json",
        url: "ajax/visitaDomiciliar/ajax-visita-domiciliar.asp"
    })
    .done(function (data) {
        var id = parseInt(data);
        if (id > 0) {
            window.location.assign("visitaDomiciliarCad.asp");
        }
        else {
            modalAlerta('Atenção', 'Falha ao criar visita!');
        }
    });
}

function editVisitaDomiciliar(id) {
    jQuery.ajax({
        type: "POST",
        async: false,
        data: {
            acao: "transf", id: id
        },
        datatype: "json",
        url: "ajax/visitaDomiciliar/ajax-visita-domiciliar.asp"
    })
    .done(function (data) {
        window.location.assign("visitaDomiciliarCad.asp");
    })
    .error(function (data, data2, data3){
        var teste = "";
    });
}