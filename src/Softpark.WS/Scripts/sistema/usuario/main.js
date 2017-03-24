
function cadUsuario() {
    window.location.assign("usuarioCad.asp")
}

var table;

$(document).ready(function () {

    table = $('#listaUsuario').dataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "ajax/dataTables/?xQ=listaUsuario",
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
                "aTargets": [0]
            },
            {
                "sClass": "text-center",
                "aTargets": [3]
            },
            {
                "sClass": "permAtu",
                "aTargets": [4]
            }
        ],
        "order": [[1, "asc"]],

        // Draw Callback DataTables
        "fnDrawCallback": function () {

            $('.nomeCap').each(function () {
                var valor = $(this).html().toLowerCase();
                $(this).html(valor.toCapitalize());
            });

            $('.dataTables_filter button').remove();
            if (permCad) $('.dataTables_filter').prepend('<button type="button" class="btn btn-success pull-right btn-datatable" onclick="cadUsuario()">Novo Cadastro</button>');

            if (permAtu) {
                $("#listaUsuario .btn-outline").click(function () {
                    $("#usuario").val($(this).attr("data-id"))
                    $("#editUsuario").submit()
                });
            } else {
                $('#listaUsuario .permAtu center').hide();
            }
        }
    }).fnSetFilteringDelay(800);

});








