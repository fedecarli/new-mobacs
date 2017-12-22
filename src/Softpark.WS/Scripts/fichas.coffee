# CoffeeScript
class Routes
    constructor: ->
        @history = []
        @history.push window.location.href
        @current = 0
        that = @
        window.onpopstate = (e) ->
            that.pop e

    navigate: (url) ->
        if not url || url is undefined || url is null
            throw new Error "Invalid Url Navigation"

        url = url.split('?')[0]

        current = @history[@current]

        if current != url
            window.history.pushState {
                previous: {
                    index: @current,
                    url: current
                },
                url: url
                }, '', url
            @current = @history.push(url) - 1
        return
    pop: (e) -> window.location.reload()

window.renderHtmlCell = (d) -> d

window.routes = new Routes

window.beginRequest = (promise) ->
    criaLoading "page-wrapper"
    promise.complete ->
        destroiLoading "page-wrapper"

window.drawCallback = () ->
    $('.dataTables_filter button, .btn-create').remove()

    for draw in draws
        $(draw.el).append(draw.content)

jQuery.ajaxPrefilter (options, originalOptions, xhr) ->
    if options.url is 'ajax/default.asp'
        options.url = originalOptions.url = "#{appPath}../#{options.url}"

    if options.url is 'ajax/login/verificaSessao.asp' || options.url is '../_inc/ajax/login/verificaSessao.asp'
        options.url = originalOptions.url = "#{appPath}../#{options.url}"
        xhr
        .done (data) ->
            if not data.sessao
                modalAlerta "Sessão Expirada", "Sua sessão expirou!"
                setTimeout(() ->
                    window.location.assign "#{appPath}../login.asp"
                , 5000)

$ ->
    selectedSetor = null

    prop = false

    window.ddlChange = ->
        if this.value is ''
            return

        txt = $(this).text()
        d = txt.split(' / ')
        ma = d[0].trim()
        cnes = d[1].split(' - ')[0].trim()
        nome = d[1].trim()

        $(this).text(ma).data({ setor: { cnes: cnes, nome: nome } })

        prop = true

        if this.selected && not selectedSetor
            selectedSetor = cnes
            $('[name=unidadeCred]').val(cnes)
        else if selectedSetor != cnes
            $(this).attr('disabled', true)

        $('[name=unidadeCred]').trigger('change')

        prop = false
        return

    $('select#idMicroAreaUnidade option, select#ItemVinc option').each ddlChange

    $(document).on 'change', '[name=unidadeCred]', ->
        set = $(this).val()

        if set != selectedSetor && not prop
            $('select#idMicroAreaUnidade, select#ItemVinc').val('')

        selectedSetor = set

        $('select#idMicroAreaUnidade option, select#ItemVinc option').each ->
            setor = $(this).data('setor')
            if not setor
                return
            cnes = setor.cnes
            if selectedSetor is cnes
                $(this).removeAttr('disabled')
            else
                $(this).attr('disabled', true)
    return

loadings = []

window.criaLoading = (alvoID) ->
    if 0 is $('#' + alvoID).length then return

    out = 0 > loadings.indexOf alvoID
    if out then loadings.push(alvoID)

    boxCarregando = '<div class="div-carregando" id="div-carregando-' + alvoID + '">'
    boxCarregando +=    '<div class="div-carregando-fundo"></div>'
    boxCarregando +=    '<div class="div-carregando-conteudo">'
    boxCarregando += '<img src="' + appPath + '../img/ajax-loader-2.gif" /><br>'
    boxCarregando +=        '<span><b>Carregando...</b></span>'
    boxCarregando +=    '</div>'
    boxCarregando += '</div>'

    if $('#div-carregando-' + alvoID).length is 0
        $('body').append(boxCarregando)

    $('#div-carregando-' + alvoID).css({
        display: 'block',
        width: $('#' + alvoID).css("width"),
        height: $('#' + alvoID).css("height"),
        top: $('#' + alvoID).offset().top,
        left: $('#' + alvoID).offset().left
    })
    return

window.destroiLoading = (alvoID) ->
    a = $.extend [], [], loadings
    b = a.splice(a.indexOf(alvoID))
    b.shift()
    a = [].concat(a, b)
    loadings = a
    $('#div-carregando-' + alvoID).remove()

$(window).on 'resize', ->
    for alvoID in loadings
        $('#div-carregando-' + alvoID).css({
            width: 0,
            height: 0,
            top: $('#' + alvoID).offset().top,
            left: $('#' + alvoID).offset().left
        }).css({
            width: $('#' + alvoID).css("width"),
            height: $('#' + alvoID).css("height"),
            top: $('#' + alvoID).offset().top,
            left: $('#' + alvoID).offset().left
        })