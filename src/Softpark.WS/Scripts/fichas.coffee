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
        
assocs = []
assocsStorage = []

downs = []
downsStorage = []

updates = []
codes = []

window.beginRequest = (promise, options) ->
    criaLoading "page-wrapper"
    if options.url.toLowerCase().indexOf('marcacao/associacao') >= 0 && options.type.toLowerCase() is "post"
        d = {
            "X-Requested-With": "XMLHttpRequest",
            "associacoes": assocs
        }
        options.data = jQuery.param(d)

        promise
            .done (d) ->
                assocs = []
                assocsStorage = []
            .error () ->
                console.error arguments, 1

    if options.url.toLowerCase().indexOf('marcacao/download') >= 0 && options.type.toLowerCase() is "post"
        d = {
            "X-Requested-With": "XMLHttpRequest",
            "downloads": downs
        }
        options.data = jQuery.param(d)

        promise
            .done (d) ->
                downs = []
                downsStorage = []
            .error () ->
                console.error arguments, 1
            
    promise.complete ->
        destroiLoading "page-wrapper"

window.drawCallback = () ->
    $('.btn-create').remove()

    for draw in draws
        $(draw.el).append(draw.content)
    
    if updates.length > 0
        $('#updateZon').removeClass('disabled').removeAttr('disabled')
    else
        $('#updateZon').addClass('disabled').attr('disabled', true)

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

class ZoneamentoViewModel
    constructor: (microArea, codigo) ->
        @MicroArea = microArea
        @Codigo = codigo

$(document).on 'click', '#updateZon', (e) ->
    e.preventDefault()
    e.stopPropagation()
    criaLoading('page-wrapper')
    $('#updateZon').addClass('disabled').attr('disabled', true)
    $.post window.location.href + '/Edit', { zoneamentos: updates }
    .always () ->
        destroiLoading('page-wrapper')
    .done (d) ->
        if d is true
            updates = []
            codes = []
            oTable.fnDraw()
        else
            console.error arguments, 0

        if updates.length > 0
            $('#updateZon').removeClass('disabled').removeAttr('disabled')
        else
            $('#updateZon').addClass('disabled').attr('disabled', true)
    .error () ->
        console.error arguments, 1

$(document).on 'change', '.microArea', (e) ->
    e.preventDefault()
    e.stopPropagation()
    codigo = +(this.id)
    ma = $(this).val()

    if not ma
        if 0 <= codes.indexOf(codigo)
            delete updates[codes.indexOf(codigo)]
            delete codes[codes.indexOf(codigo)]
            updates = updates.filter (x) -> !!x
            codes = codes.filter (x) -> not isNaN(x)
    else
        if 0 <= codes.indexOf(codigo)
            updates[codes.indexOf(codigo)].MicroArea = ma
        else
            codes.push(codigo)
            updates.push(new ZoneamentoViewModel(ma, codigo))

    if updates.length > 0
        $('#updateZon').removeClass('disabled').removeAttr('disabled')
    else
        $('#updateZon').addClass('disabled').attr('disabled', true)

window.renderMaSelection = (d, s, i) ->
    codigo = i.Codigo
    tpl = $($('#tplMaSelection').html().replace('{codigo}', codigo))

    setTimeout () ->
        if 0 <= codes.indexOf(codigo)
            u = updates[codes.indexOf(codigo)]
            d = u.MicroArea
        
        if d
            $('#' + codigo).val(d)
    , 100

    tpl[0].outerHTML

class Assoc
    constructor: () ->
        @Codigo = 0
        @Relacionar = false
        
class Download
    constructor: () ->
        @Vinculo = 0
        @Baixar = false
        
reduce = (x, z) ->
    (typeof x.Relacionar is 'undefined' ? x : x.Relacionar) + z.Relacionar

$(document).on 'change', '.chk_associar', (e) ->
    e.preventDefault()
    e.stopPropagation()

    if assocs.length is 0 && assocsStorage.length > 0
        assocsStorage = []

    index = assocsStorage.indexOf $(this).data('codigo')
    
    if index < 0
        index = assocs.length
        assoc = new Assoc
        assoc.Codigo = $(this).data('codigo')
        assocs.push assoc
        assocsStorage.push $(this).data('codigo')

    assoc = assocs[index]
    assoc.Relacionar = this.checked

    $('#btn_update_assoc').attr('disabled', true).addClass('disabled')

    if assocs.length > 0
        $('#btn_update_assoc').removeAttr('disabled').removeClass('disabled')

window.renderVincSelection = (d, s, i) ->
    index = assocsStorage.indexOf $(this).data('codigo')
    
    if index >= 0
        assoc = assocs[index]
        
        if assoc.Relacionar is false
            d = d.replace('checked', '')
        else if d.indexOf('checked') < 0
            d = d.replace('input ', 'input checked ')

    $('#btn_update_assoc').attr('disabled', true).addClass('disabled')
    
    if assocs.length > 0
        $('#btn_update_assoc').removeAttr('disabled').removeClass('disabled')

    d

window.renderDownSelection = (d, s, i) ->
    index = downsStorage.indexOf $(this).data('vinculo')
    
    if index >= 0
        down = downs[index]
        
        if down.Baixar is false
            d = d.replace('checked', '')
        else if d.indexOf('checked') < 0
            d = d.replace('input ', 'input checked ')

    $('#btn_update_download').attr('disabled', true).addClass('disabled')
    
    if downs.length > 0
        $('#btn_update_download').removeAttr('disabled').removeClass('disabled')

    d

$(document).on 'change', '.chk_download', (e) ->
    e.preventDefault()
    e.stopPropagation()

    if downs.length is 0 && downsStorage.length > 0
        downsStorage = []

    index = downsStorage.indexOf $(this).data('vinculo')
    
    if index < 0
        index = downs.length
        down = new Download
        down.Vinculo = $(this).data('vinculo')
        downs.push down
        downsStorage.push $(this).data('vinculo')

    down = downs[index]
    down.Baixar = this.checked

    $('#btn_update_download').attr('disabled', true).addClass('disabled')

    if downs.length > 0
        $('#btn_update_download').removeAttr('disabled').removeClass('disabled')
