
function cadDomicilio() {
    window.location.assign("domicilioCad.asp")
}

var table;

$(document).ready(function () {
    
    table = $('#listaDomicilio').dataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "ajax/dataTables/?xQ=listaDomicilio",
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
                "aTargets": [0,1,3]
            },
            {
                "sClass": "text-center",
                "aTargets": [2,4]
            },
            {
                "sClass": "permAtu",
                "aTargets": [5]
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
            if (permCad) $('.dataTables_filter').prepend('<button type="button" class="btn btn-success pull-right btn-datatable" onclick="cadDomicilio()">Novo Cadastro</button>');

            if (permAtu) {
                $("#listaDomicilio .btn-outline").click(function () {
                    $("#domicilio").val($(this).attr("data-id"))
                    $("#editDomicilio").submit()
                });
            } else {
                $('#listaDomicilio .permAtu center').hide();
            }

        }
    }).fnSetFilteringDelay(800);

});








