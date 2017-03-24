$(function () {
    $.get($('#unicaLotacaoHeader').data('apiurl') + '/AjaxTemplates/Cabecalho', function (template) {
        $("#unicaLotacaoHeader").replaceWith(template);
    }, "text/html");
});
