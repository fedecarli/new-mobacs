var Api = { URL: "ajax/marcadoresConsumoAlimentar/ajax-marcador-consumo-alimentar.asp" };

function modalAlerta(xTitulo, xTexto) {
    var modalInicio = "<div class='modal fade' id='alertaModal' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'><div class='modal-dialog'><div class='modal-content'><div class='modal-header'><button type='button' class='close' data-dismiss='modal' aria-hidden='true'>&times;</button><h4 class='modal-title' id='myModalLabel'>" + xTitulo + "</h4></div><div class='modal-body'>";
    var modalFim = "</div><div class='modal-footer'><button type='button' class='btn btn-default' data-dismiss='modal'>Fechar</button></div></div></div></div>";

    $("#alerta").empty();
    $("#alerta").append(modalInicio + xTexto + modalFim);
    $("#alertaModal").modal("show");
}

function cadMarcadorConsumoAlimentar() {
    $("#btnCadMarcadorConsumoAlimentar").prop('disabled', true);
    jQuery.ajax({
        type: "POST",
        data: {
            acao: "create",
            digitadoPor: "",
            data: dataAtualFormatada()
        },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        var id = parseInt(data);
        if (id > 0) {
            window.location.assign("marcadoresConsumoAlimentarCad.asp");
        }
        else {
            modalAlerta('Atenção', 'Falha ao criar marcador de consumo alimentar!');
        }
    });
}

function editMarcadorConsumoAlimentar(id) {
    setConferidoPor(id)
        .then(function () {
            jQuery.ajax({
                type: "POST",
                data: {
                    acao: "transf", id: id
                },
                datatype: "json",
                url: Api.URL
            })
            .done(onSuccess);
        });

    function onSuccess(data, textStatus, jqXHR) {
        window.location.assign("marcadoresConsumoAlimentarCad.asp");
    }
}

$(document).ready(function () {
    $('#listaMarcadorConsumoAlimentar').dataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "ajax/dataTables/?xQ=listaMarcadorConsumoAlimentar",
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
            if (permCad) $('.dataTables_filter').prepend('<button type="button" id="btnCadMarcadorConsumoAlimentar" class="btn btn-success pull-right btn-datatable" onclick="cadMarcadorConsumoAlimentar()">Novo Cadastro</button>');

            if (permAtu) {
                $("#btnCadMarcadorConsumoAlimentar").prop('disabled', false);
                $("#listaMarcadorConsumoAlimentar .btn-outline").click(function () {
                    $("#marcadorConsumoAlimentar").val($(this).attr("data-id"));
                    $("#listaMarcadorConsumoAlimentar").submit();
                    editMarcadorConsumoAlimentar($(this).data("id"));
                });
            } else {
                $('#listaMarcadorConsumoAlimentar .permAtu center').hide();
            }
        }
    }).fnSetFilteringDelay(800);
});