$(document).ready(function () {
    $('#listaAvaliacaoElegibilidadeAdmissao').dataTable({
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "ajax/dataTables/?xQ=listaAvaliacaoElegibilidadeAdmissao",
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
            if (permCad)
                $('.dataTables_filter').prepend('<button type="button" id="btnCadAvaliacaoElegelibidade" class="btn btn-success pull-right btn-datatable" onclick="cadAvaliacaoElegibilidadeAdmissao()">Novo Cadastro</button>');

            if (permAtu) {
                $("#btnCadAvaliacaoElegelibidade").prop('disabled', false);
                $("#listaAvaliacaoElegibilidadeAdmissao .btn-outline").click(function () {
                    editAvaliacaoElegibilidadeAdmissao($(this).data("id"));
                });
            } else {
                $('#listaAvaliacaoElegibilidadeAdmissao .permAtu center').hide();
            }
        }
    }).fnSetFilteringDelay(800);
});

function cadAvaliacaoElegibilidadeAdmissao() {
    $("#btnCadAvaliacaoElegelibidade").prop('disabled', true);
    jQuery.ajax({
        type: "POST",
        async: false,
        data: {
            acao: "create",
            data: dataAtualFormatada()
        },
        datatype: "json",
        url: "ajax/avaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp"
    })
    .done(function (data) {
        if (parseInt(data) > 0) {
            window.location.assign("avaliacaoElegibilidadeAdmissaoCad.asp");
        } else {
            modalAlerta('Atenção', 'Falha ao criar Avaliação!');
        }
    });
}

function editAvaliacaoElegibilidadeAdmissao(id) {
    jQuery.ajax({
        type: "POST",
        async: false,
        data: { acao: "transf", id: id },
        datatype: "json",
        url: "ajax/avaliacaoElegibilidadeAdmissao/ajax-avaliacao-elegibilidade-admissao.asp"
    })
    .done(function (data) {
        window.location.assign("avaliacaoElegibilidadeAdmissaoCad.asp");
    });
}