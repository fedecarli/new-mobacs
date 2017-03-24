window.ESUS.register('pre', function () {
    window.carregaINE = function (elSetor, idINE, val) {
        if ($(elSetor).val()) {
            CodSetor = $('option:selected', elSetor).data('idsetor');
            $.get('../assmed20/json/getINESetor.asp?CodSetor=' + CodSetor)
            .done(function (data) {
                if (data.status) {
                    $('#' + idINE).html('<option value="">SELECIONE UMA EQUIPE</option>');
                    $.each(data.resultado, function (ResultadoItens, item) {
                        $('#' + idINE).append('<option value="' + item.INE + '">' + item.Descricao + '</option>');
                    });
                    $('#' + idINE).val(val);
                    $('label[for=' + idINE + ']').html($('label[for=' + idINE + ']').html());
                    $('#' + idINE).addClass('obg');
                } else {
                    $('#' + idINE).html('<option value="">NÃO HÁ EQUIPES</option>');
                    $('label[for=' + idINE + ']').html($('label[for=' + idINE + ']').html());
                    $('#' + idINE).removeClass('obg');
                }
            });
        }
    };
});