var lang = jQuery("#lang").val();
//se for chrome
var is_chrome = ((navigator.userAgent.toLowerCase().indexOf('chrome') > -1) && (navigator.vendor.toLowerCase().indexOf("google") > -1));
/*
 * detect IE
 * returns version of IE or false, if browser is not Internet Explorer
 */
function detectIE() {
    var ua = window.navigator.userAgent;

    var msie = ua.indexOf('MSIE ');
    if (msie > 0) {
        // IE 10 or older => return version number
        return parseInt(ua.substring(msie + 5, ua.indexOf('.', msie)), 10);
    }

    var trident = ua.indexOf('Trident/');
    if (trident > 0) {
        // IE 11 => return version number
        var rv = ua.indexOf('rv:');
        return parseInt(ua.substring(rv + 3, ua.indexOf('.', rv)), 10);
    }

    var edge = ua.indexOf('Edge/');
    if (edge > 0) {
        // Edge (IE 12+) => return version number
        return parseInt(ua.substring(edge + 5, ua.indexOf('.', edge)), 10);
    }

    // other browser
    return false;
}
if ((detectIE() !== false && detectIE() >= 11) || is_chrome){
    //alert(navigator.userAgent.toLowerCase() + '/-/' + navigator.vendor.toLowerCase());
}else {
    $('body').html('');
    alert('Este sistema só pode ser acessado pelo Chrome');
    document.location = 'login.asp';
};
$(function () {

    // Capitalize
    String.prototype.toCapitalize = function () {
        return this.toLowerCase().replace(/^.|\s\S/g, function (a) { return a.toUpperCase(); });
    }

    // Ctrl+P
    document.onkeydown = function (e) {
        var code = e.keyCode || e.which;
        if (e.ctrlKey && (code == 80 || code == 112)) {
            e.preventDefault && e.preventDefault();
            alert('Está função está desabilitada!');
            return false;
        }
    }

    // Click dIreito do mouse
    function ClickDireito() {
        //alert('Está função está desabilitada!');
        return false;
    }
    //document.oncontextmenu = ClickDireito

    // Sessão
    setInterval(function () {
        $.ajax({
            type: "POST",
            datatype: "json",
            url: "../_inc/ajax/login/verificaSessao.asp"
        })
        .done(function (data) {
            if (!data.sessao) {
                modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                setTimeout(function () {
                    window.location.assign("../");
                }, 5000);
            }
        });
    }, 960000);

    // Dados do Prefeitura/Cliente
    //nomeCliente = '';
    //estadoCliente = '';
    //nomeInicialSistema = '';
    //logoInicialSistema = '';
    //
    //$.ajax({
    //    type: "POST",
    //    datatype: "json",
    //    url: "ajax/default.asp"
    //})
    //.done(function (data) {
    //    if (data.status) {
    //        nomeCliente = data.nomeCliente;
    //        estadoCliente = data.estadoCliente;
    //        nomeInicialSistema = data.nomeInicialSistema;
    //        logoInicialSistema = data.logoInicialSistema;
    //    }
    //});


    $(".cep").mask("99999-999");
    $(".data").mask("99/99/9999").css('width', '100px').bind('blur', function () { validacaoCampo('#' + $(this).attr('id'), 'DATA') });
    $(".dia").mask("99").bind('blur', function () { validacaoCampo('#' + $(this).attr('id'), 'DIA') });
    $(".mes").mask("99").bind('blur', function () { validacaoCampo('#' + $(this).attr('id'), 'MES') });
    $(".ano").mask("9999").bind('blur', function () { validacaoCampo('#' + $(this).attr('id'), 'ANO') });
    $(".porcentagem").bind('blur', function () { validacaoCampo('#' + $(this).attr('id'), 'PORCENTAGEM') });
    $(".hora").mask("99:99").css('width', '70px').bind('blur', function () { validacaoCampo('#' + $(this).attr('id'), 'TEMPO_HHMM') });
    $(".cpf").mask("999.999.999-99").bind('blur', function () { validacaoCampo('#' + $(this).attr('id'), 'CPF') });
    $(".cns").mask("999999999999999").bind('blur', function () { validacaoCampo('#' + $(this).attr('id'), 'CNS') });
    $(".cnpj").mask("99.999.999/9999-99").bind('blur', function () { validacaoCampo('#' + $(this).attr('id'), 'CNPJ') });
    $(".fone_fixo").mask("9999-9999");
    $(".ddd").mask("(99)");
    $(".real").maskMoney({ prefix: 'R$ ', allowNegative: true, thousands: '.', decimal: ',', affixesStay: false })
    $(".fone").mask("(99) 9999-9999?9");

    $('.fone').focusout(function () {
        var phone, element;
        $(this).unmask();
        phone = $(this).val().replace(/\D/g, '');
        if (phone.length > 10) {
            $(this).mask("(99) 99999-999?9");
        } else {
            $(this).mask("(99) 9999-9999?9");
        }
    });

    hoje = new Date();
    dt_min = "01/01/1900";
    dt_min2 = hoje.getDate() + "/" + (hoje.getMonth() + 1) + "/" + hoje.getFullYear();
    dt_max = hoje.getDate() + "/" + (hoje.getMonth() + 1) + "/" + hoje.getFullYear();

    $(".data").datepicker({
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true,
        showWeek: true,
        closeText: "Fechar",
        prevText: "&#x3C;Anterior",
        nextText: "Próximo&#x3E;",
        currentText: "Hoje",
        monthNames: ["Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
        monthNamesShort: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"],
        dayNames: ["Domingo", "Segunda-feira", "Terça-feira", "Quarta-feira", "Quinta-feira", "Sexta-feira", "Sábado"],
        dayNamesShort: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"],
        dayNamesMin: ["Dom", "Seg", "Ter", "Qua", "Qui", "Sex", "Sáb"],
        weekHeader: "Sm",
        dateFormat: "dd/mm/yy",
        firstDay: 0,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ""
    });
    $(".data.data_Maior18").datepicker("option", "maxDate", "-18Y");
    $(".data.data_AntesHoje").datepicker("option", "maxDate", new Date());
    $(".data.data_DepoisHoje").datepicker("option", "minDate", new Date());

    $(".hora").timepicker();
    //$(".datepicker").datepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    dateFormat: 'dd/mm/yy',
    //    dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
    //    dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
    //    dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
    //    monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
    //    monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
    //    nextText: 'Próximo',
    //    prevText: 'Anterior'
    //});
    //
    //$(".datepickerMaxDateAtual").datepicker({
    //    minDate: dt_min,
    //    maxDate: dt_max,
    //    changeMonth: true,
    //    changeYear: true,
    //    dateFormat: 'dd/mm/yy',
    //    dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
    //    dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
    //    dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
    //    monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
    //    monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
    //    nextText: 'Próximo',
    //    prevText: 'Anterior'
    //});
    //
    //$(".datepickerMaxDateAtualModal").datepicker({
    //    minDate: dt_min,
    //    maxDate: dt_max,
    //    dateFormat: 'dd/mm/yy',
    //    dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
    //    dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
    //    dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
    //    monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
    //    monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
    //    nextText: 'Próximo',
    //    prevText: 'Anterior'
    //});
    //
    //$(".datepickerMinDateAtual").datepicker({
    //    minDate: dt_min2,
    //    changeMonth: true,
    //    changeYear: true,
    //    dateFormat: 'dd/mm/yy',
    //    dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
    //    dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
    //    dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
    //    monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
    //    monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
    //    nextText: 'Próximo',
    //    prevText: 'Anterior'
    //});
    //
    //$(".datepickerMinDateAtualModal").datepicker({
    //    minDate: dt_min2,
    //    dateFormat: 'dd/mm/yy',
    //    dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
    //    dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
    //    dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
    //    monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
    //    monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
    //    nextText: 'Próximo',
    //    prevText: 'Anterior'
    //});
    //tinymce.init({
    //    selector: ".editor",
    //    height: 500,
    //    plugins: [
    //      "advlist autolink autosave link image lists charmap print preview hr anchor pagebreak spellchecker",
    //      "searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking",
    //      "table contextmenu directionality emoticons template textcolor paste fullpage textcolor colorpicker textpattern"
    //    ],
    //    toolbar1: "newdocument fullpage | bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | styleselect formatselect fontselect fontsizeselect",
    //    toolbar2: "cut copy paste | searchreplace | bullist numlist | outdent indent blockquote | undo redo | link unlink anchor image media code | insertdatetime preview | forecolor backcolor",
    //    toolbar3: "table | hr removeformat | subscript superscript | charmap emoticons | print fullscreen | ltr rtl | spellchecker | visualchars visualblocks nonbreaking template pagebreak restoredraft",
    //    menubar: false,
    //    toolbar_items_size: 'small',
    //    content_css: [
    //      '//fast.fonts.net/cssapi/e6dc9b99-64fe-4292-ad98-6974f93cd2a2.css',
    //      '//www.tinymce.com/css/codepen.min.css'
    //    ]
    //});


    tinymce.PluginManager.add('charactercount', function (editor) {
        var self = this;

        function update() {
            editor.theme.panel.find('#charactercount').text(['Caracteres: {0}', self.getCount()]);
        }

        editor.on('init', function () {
            var statusbar = editor.theme.panel && editor.theme.panel.find('#statusbar')[0];

            if (statusbar) {
                window.setTimeout(function () {
                    statusbar.insert({
                        type: 'label',
                        name: 'charactercount',
                        text: ['Caracteres: {0}', self.getCount()],
                        classes: 'charactercount',
                        disabled: editor.settings.readonly
                    }, 0);

                    editor.on('setcontent beforeaddundo', update);

                    editor.on('keyup', function (e) {
                        update();
                    });
                }, 0);
            }
        });

        self.getCount = function () {
            var tx = editor.getContent({ format: 'raw' });
            var decoded = decodeHtml(tx);
            var decodedStripped = decoded.replace(/(<([^>]+)>)/ig, "").trim();
            var tc = decodedStripped.length;
            return tc;
        };

        function decodeHtml(html) {
            var txt = document.createElement("textarea");
            txt.innerHTML = html;
            return txt.value;
        }
    });
    //o plugin abaixo ainda esta funcionando porempor ordens superioras ele esta com display none(hunf!)
    //C:\inetpub\wwwroot\SIGSM\v2\_inc\js\plugins\tinymce\skins\lightgray\skin.min.css
    tinymce.PluginManager.add('HTMLcount', function (editor) {
        var self = this;
        var maxlength = '';
        if ($('#' + self.constructor.arguments[0].id)) {
            if ($('#' + self.constructor.arguments[0].id).attr('maxlength')) {
                maxlength = $('#' + self.constructor.arguments[0].id).attr('maxlength');
            }
        }
        if (maxlength!='') {
            function update() {
                editor.theme.panel.find('#HTMLcount').text(['Capacidade: {0}', self.getCount() + '%']);
            }

            editor.on('init', function () {
                var statusbar = editor.theme.panel && editor.theme.panel.find('#statusbar')[0];

                if (statusbar) {
                    window.setTimeout(function () {
                        statusbar.insert({
                            type: 'label',
                            name: 'HTMLcount',
                            text: ['Capacidade: {0}%', self.getCount()],
                            classes: 'HTMLcount',
                            disabled: editor.settings.readonly
                        }, 0);

                        editor.on('setcontent beforeaddundo', update);

                        editor.on('keyup', function (e) {
                            update();
                        });
                    }, 0);
                }
            });
        }
        self.getCount = function () {
            var tx = (100 / maxlength * editor.getContent().length).toFixed(2);
            return tx;
        };
    });

    tinyMCE_Propriedades_Completas = {
        selector: "textarea.editor",
        height: 200,
        plugins: [
          "advlist autolink link lists charmap print preview hr spellchecker",
          "wordcount charactercount HTMLcount",
          "table contextmenu directionality textcolor paste fullpage textcolor colorpicker textpattern"
        ],
        toolbar1: "bold italic underline strikethrough | alignleft aligncenter alignright alignjustify | styleselect formatselect fontselect fontsizeselect",
        toolbar2: "bullist numlist | outdent indent blockquote | undo redo | link unlink | preview | forecolor backcolor",
        toolbar3: "table | hr removeformat | subscript superscript | charmap | print | ltr rtl",
        menubar: false,
        toolbar_items_size: 'small',
        browser_spellcheck: true,
        language: 'pt_BR',
        elementpath: false,
        setup: function (editor) {
            editor.on('keypress', function (e) {
                var HTMLcount = this.plugins["HTMLcount"].getCount();
                var maxlength = $('#' + tinyMCE.activeEditor.id).attr('maxlength');
                if (maxlength != '') {
                    if (parseFloat(HTMLcount) >= 100) {
                        modalAlerta('Atenção', 'Você estorou o limite de dados permitidos');
                        return false;
                    }
                };
            });
        },
        init_instance_callback: function (editor) {
            editor.on('blur', function (e) {
                tinyMCE.triggerSave();
                $('.textarea.editor,textarea.editor_simples').each(function () {
                    $(this).val(
                        $(this).val()
                        .replace(/\n/g, '')
                        .replace('<!DOCTYPE html><html><head></head><body>', '')
                        .replace('</body></html>', '')
                    )
                })
            });
        }
    }
    tinyMCE_Propriedades_Simples = tinyMCE_Propriedades_Completas;
    tinyMCE_Propriedades_Simples.selector = "textarea.editor_simples";
    tinyMCE_Propriedades_Simples.toolbar1 = "bold italic underline | alignjustify aligncenter | bullist numlist | removeformat";
    tinyMCE_Propriedades_Simples.toolbar2 = "";
    tinyMCE_Propriedades_Simples.toolbar3 = "";
    tinyMCE_Propriedades_Simples.height = 100;
    tinymce.init(tinyMCE_Propriedades_Completas);
    tinymce.init(tinyMCE_Propriedades_Simples);
    $('textarea.editor,textarea.editor_simples').bind("change paste keyup", function () {
        tinyMCE.get($(this).attr('id')).setContent($(this).val());
    });
    fieldsPreDefinidos();
    tpField_seletor('');
    dt_dia = hoje.getDate();
    dt_mes = (hoje.getMonth() + 1);
    dt_ano = (hoje.getFullYear());

    if (dt_dia < 10) {
        dt_dia = '0' + dt_dia;
    }

    if (dt_mes < 10) {
        dt_mes = '0' + dt_mes;
    }

    dt_max = dt_dia + "/" + dt_mes + "/" + hoje.getFullYear();
    dt_atual = dt_max;
    hr_atual = hoje.getHours() + ":" + hoje.getMinutes() + ":" + hoje.getSeconds();
    hr_atualHM = hoje.getHours() + ":" + hoje.getMinutes();

    dt_meses = ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];
    dt_meses_abre = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];
    
    // Saudação
    saudacao();
    //setInterval(function () {
    //    saudacao();
    //}, 1000);

});

//Funções Array
Array.prototype.map = function(fnc) {
    var a = new Array(this.length);
    for (var i = 0; i < this.length; i++) {
        a[i] = fnc(this[i]);
    }
    return a;
}

Array.prototype.forEach = function (func, scope) {
    scope = scope || this;
    for (var i = 0, l = this.length; i < l; i++)
        func.call(scope, this[i], i, this);
}
//---

///<signature>
///<summary>Requisição Ajax</summary>
///<param name="dadosRequisicao" type="Object">Parâmetro "data" da requisição Ajax</param>
///<returns type="Object"/>
///</signature>

function postAjax(dadosRequisicao) {
    function OnSuccesDetalhes(data) {
        return data;
    }
    var ajaxOptions = {
        type: "POST",
        data: dadosRequisicao,
        dataType: "json",
        url: Api.URL
    };
    return jQuery
       .ajax(ajaxOptions)
       .done(OnSuccesDetalhes);
}

function postAjaxObject(dadosRequisicao) {
    function OnSuccesDetalhes(data) {
        return data;
    }
    var ajaxOptions = {
        type: "POST",
        data: dadosRequisicao,
        url: Api.URL
    };
    return jQuery
       .ajax(ajaxOptions)
       .done(OnSuccesDetalhes);
}

function processaDadosForm(dadosRequisicao) {
    function OnSuccesDetalhes(data) {
        setFormData(data.resultado[0]);
    }
    var ajaxOptions = {
        type: "POST",
        data: dadosRequisicao,
        dataType: "json",
        url: Api.URL
    };
    return jQuery
       .ajax(ajaxOptions)
       .done(OnSuccesDetalhes);
}

function Tecla(x) {
    var tecla = (window.event) ? event.keyCode : x.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}

function goToByScroll(seletor, force) {
//function goToByScroll(id, force) {
    // Scroll

    if (force) {
        moverScroll();
    } else {
        if ($(seletor).offset().top < $("body").scrollTop() || $(seletor).offset().top > $("body").scrollTop() + $(window).height()) {
            moverScroll();
        }
    }
    function moverScroll(){
        $('html,body').animate({
            scrollTop: $(seletor).offset().top-25
        },
        'slow');
    }
}

function validaTecla(validacao, evento) {
    // onKeyPress="return validaTecla('[0]{,}', event);"
    var tecla = (window.event) ? event.keyCode : evento.which;
    var retorno = false;
    //alert('TECLA: ' + tecla + validacao.indexOf('[A]') + '\n' + validacao.indexOf('[a]') + '\n' + validacao.indexOf('[0]') + '\n' + validacao.indexOf('[!]') + '\n' + validacao.indexOf('{'));
    if (tecla == 8) {
        retorno = true;
    }
    if (validacao.indexOf('[A]') > -1) {//alert('Maiusculo');//A-Z
        if ((tecla > 64 && tecla < 91)) { retorno = true };
    }
    if (validacao.indexOf('[a]') > -1) {//a-z
        if ((tecla > 96 && tecla < 123)) { retorno = true };
    }
    if (validacao.indexOf('[0]') > -1) {//0-9
        if ((tecla > 47 && tecla < 58)) { retorno = true };
    }
    if (validacao.indexOf('[!]') > -1) {
        if ((tecla > 31 && tecla < 48)
            || (tecla > 57 && tecla < 64)
            || (tecla > 90 && tecla < 97)
            || (tecla > 122 && tecla < 127)) { retorno = true };
    }
    if (validacao.indexOf('{')>0 && validacao.indexOf('}')>0) {//{#,@,!,A,1,-}
        if (validacao.indexOf('{') < validacao.indexOf('}')) {
            var caracteres = validacao.substr(validacao.indexOf('{'), validacao.indexOf('}'));
            caracteres = caracteres.split("");
            for (index = 0; index < caracteres.length; ++index) {
                if (caracteres[index].charCodeAt(0) == tecla) retorno = true;
            }
        }
    }
    //alert(retorno);
    return retorno;
}

function validacaoCampo(campoSeletor, tipo) {
    var retorno = true;
    if ($(campoSeletor).parents('div:first').find('.msgErro').length==0) {
        $(campoSeletor).parents('div:first').append('<div class="msgErro"></div>');
    }
    $(campoSeletor).parents('div:first').find('.msgErro').html('');
    if (tipo == 'CNS') {
        if ($(campoSeletor).val().trim() != '') {
            if (retorno && $(campoSeletor).val().trim().length < 15) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' está incompleto');
            }
            if (retorno && !validateCNS($(campoSeletor).val().trim())) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' está incorreto');
            }
        }
    } else if (tipo == 'CPF') {
        if ($(campoSeletor).val().trim() != '') {
            if (retorno && $(campoSeletor).val().trim().length < 14) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' está incompleto');
            }
            if (retorno && !validateCPF($(campoSeletor).val().trim())) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' está incorreto');
            }
        }
    } else if (tipo == 'CNPJ') {
        if ($(campoSeletor).val().trim() != '') {
            if (retorno && $(campoSeletor).val().trim().length < 18) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' está incompleto');
            }
            if (retorno && !validateCNPJ($(campoSeletor).val().trim())) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' está incorreto');
            }
        }
    } else if (tipo == 'DATA') {
        if ($(campoSeletor).val().trim() != '') {
            if (retorno && $(campoSeletor).val().trim().length < 10) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' está incompleto');
            }
            if (retorno && verificaData2($(campoSeletor).val().trim())) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' está incorreto');
            }
        }
    } else if (tipo == 'DIA') {
        if ($(campoSeletor).val().trim() != '') {
            if (retorno && !isNaN($(campoSeletor).val().trim())) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' não pode conter letras');
            } else if (retorno && ($(campoSeletor).val().trim() < 1 || $(campoSeletor).val().trim()>31)) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' deve conter um valor entre 1 e 31');
            }
        }
    } else if (tipo == 'MES') {
        if ($(campoSeletor).val().trim() != '') {
            if (retorno && !isNaN($(campoSeletor).val().trim())) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' não pode conter letras');
            } else if (retorno && ($(campoSeletor).val().trim() < 1 || $(campoSeletor).val().trim() > 31)) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' deve conter um valor entre 1 e 12');
            }
        }
    } else if (tipo == 'ANO') {
        if ($(campoSeletor).val().trim() != '') {
            if (retorno && !isNaN($(campoSeletor).val().trim())) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' não pode conter letras');
            }
        }
    } else if (tipo == 'Sexo') {
        if ($(campoSeletor + ':checked').length < 1) {
            retorno = false;
            $(campoSeletor + ':first').parents('div:first').find('.msgErro').html('O Campo Sexo é Obrigatório');
            campoSeletor = campoSeletor + ':first';
        }
        //jQuery("[name='rdbSexo'][value='" + item.sexo + "']").prop("checked", true);
    } else if (tipo == 'DataNasc') {
        if ($(campoSeletor).val().trim() == '') {
            retorno = false;
            $(campoSeletor).parent().find('.msgErro').html('O Campo Data de Nascimento é Obrigatório');
        }

        var DataNasc = $(campoSeletor).val().split('/');
        var dDataNasc = new Date(DataNasc[2], DataNasc[1] - 1, DataNasc[0]);

        if (retorno && dDataNasc.getTime() > hoje.getTime()) {
            retorno = false;
            $(campoSeletor).parent().find('.msgErro').html('O Campo Data de Nascimento não pode ser maior que hoje');
        }

        //if (retorno) {
            //retorno = validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade');
        //}
        
    } else if (tipo == 'DataNasc-dataAtividade') {
        campoSeletor = campoSeletor.split(',');
        $(campoSeletor[0] + ',' + campoSeletor[1]).parent().find('.msgErro').html('');

        if (!validacaoCampo(campoSeletor[0], 'DataNasc')) {
            retorno = false;
        }

        if (retorno && $(campoSeletor[1]).val().trim() == '') {
            retorno = false;
            $(campoSeletor[0] + ',' + campoSeletor[1]).parent().find('.msgErro').html('O Campo Data é Obrigatório');
        }
        if (retorno){
            var DataNasc = $(campoSeletor[0]).val().split('/');
            var dataAtividade = $(campoSeletor[1]).val().split('/');
            var dDataNasc = new Date(DataNasc[2], DataNasc[1] - 1, DataNasc[0]);
            var dDataAtividade = new Date(dataAtividade[2], dataAtividade[1] - 1, dataAtividade[0]);
        
            if (retorno && dDataNasc.getTime() > dDataAtividade.getTime()) {
                retorno = false;
                $(campoSeletor[0] + ',' + campoSeletor[1]).parent().find('.msgErro').html('O Campo Data de Nascimento não pode ser maior que o campo data');
            }

            var dDataAtividadeLimite = new Date(dataAtividade[2], dataAtividade[1] - 1, dataAtividade[0]);
            dDataAtividadeLimite.setFullYear(dDataAtividadeLimite.getFullYear() - 130);

            if (retorno && dDataNasc.getTime() < dDataAtividadeLimite.getTime()) {
                retorno = false;
                $(campoSeletor[0] + ',' + campoSeletor[1]).parent().find('.msgErro').html('O Campo Data de Nascimento não pode ser menor que o campo data menos 130 anos');
            }
        }
        campoSeletor = campoSeletor[0]; 
    } else if (tipo == 'PORCENTAGEM') {
        if ($(campoSeletor).val().trim() != '') {
            if (retorno && isNaN($(campoSeletor).val().trim())) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' não pode conter letras');
            } else if (retorno && ($(campoSeletor).val().trim() < 1 || $(campoSeletor).val().trim()>100)) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Campo ' + $(campoSeletor).parents('div:first').find('label').html() + ' deve conter um valor entre 1 e 100');
            }
        }
    } else if (tipo == 'Peso') {
        if ($(campoSeletor).val().trim() != '') {
            //$(campoSeletor).val(String(Math.round(parseFloat($(campoSeletor).val().replace(',', '.')) * 1000) / 1000).replace('.', ','));
            if (retorno && parseFloat($(campoSeletor).val().replace(',', '.')) < 0.5) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Peso Mínimo deve ser 0,5 Kg');
            }
            if (retorno && parseFloat($(campoSeletor).val().replace(',', '.')) > 500) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('O Peso máximo deve ser 500 Kg');
            }
        }
    } else if (tipo == 'Altura') {
        if ($(campoSeletor).val().trim() != '') {
            //$(campoSeletor).val(String(Math.round(parseFloat($(campoSeletor).val().replace(',', '.')) * 100) / 100).replace('.', ','));
            if (retorno && parseFloat($(campoSeletor).val().replace(',', '.')) < 20) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('A Altura Mínima deve ser 20 cm');
            }
            if (retorno && parseFloat($(campoSeletor).val().replace(',', '.')) > 250) {
                retorno = false;
                $(campoSeletor).parent().find('.msgErro').html('A Altura máxima deve ser 250 cm');
            }
        }
    } else if (tipo == 'Email') {
        var email=$(campoSeletor).val().trim();
        if (retorno && email != '') {
            var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            retorno = regex.test(email);
            if (!retorno) { $(campoSeletor).parent().find('.msgErro').html('O email é inválido'); }
        }
}
    if (!retorno) goToByScroll(campoSeletor,false);
    return retorno;
}

function setConferidoPor(id) {
    return jQuery.ajax({
        type: "POST",
        async: false,
        data: {
            acao: "setConferidoPor", id: id
        },
        datatype: "json",
        url: Api.URL
    });
}
//---

//Conversões
function stringToBoolean(string) {
    if (string != "" && string != null && (typeof string) == "string") {
        switch (string.toLowerCase().trim()) {
            case "true": case "verdadeiro": return true;
            case "false": case "falso": case "0": case null: return false;
            default: return Boolean(string);
        }
    }
    else if ((typeof string) == "boolean") {
        return string;
    }
    else
    {
        return false;
    }
}
//---

//Formulários
function getFormData(id) {
    var data = jQuery(id)
        .serialize()
        .split("&")
        .map(function (x) { return x.split("=") });

    data = data.reduce(ArrArrToObject, {});

    var uncheckeds = {};
    jQuery(id)
        .find("input[type=checkbox]")
        .each(function (i, ele) {
            uncheckeds[jQuery(ele).attr("name")] = null;
        });
    data = jQuery.extend(uncheckeds, data);

    uncheckeds = {};
    jQuery(id)
        .find("input[type=radio]")
        .each(function (i, ele) {
            uncheckeds[jQuery(ele).attr("name")] = null;
        });
    data = jQuery.extend(uncheckeds, data);

    return data;

    function ArrArrToObject(object, value, indice) {
        if (value[1] == "on") {
            object[value[0]] = 1;
        }
        else
        {
            //object[value[0]] = object[value[0]] == null ? unescape(decodeURIComponent(value[1].replace(/\+/g, " "))) || 0 : object[value[0]] + ", " + unescape(decodeURIComponent(value[1].replace(/\+/g, " "))) || 0;
            object[value[0]] = object[value[0]] == null ? unescape(decodeURIComponent(value[1].replace(/\+/g, " "))) || null : object[value[0]] + ", " + unescape(decodeURIComponent(value[1].replace(/\+/g, " "))) || 0;
        }

        return object;
    }
}

function setFormData(objeto) {
    
    for (var key in objeto) {
        if (objeto.hasOwnProperty(key)) {
            ObjectToForm(key)
        }
    }
    function ObjectToForm(name) {
        var elemento = jQuery("[name='" + name + "']");

        if (elemento.hasClass("data")) {
            elemento.val(formatarDataParaExibicao(objeto[name]));
        }
        else
        {
            if (elemento.attr("cols") > 0 || elemento.attr("type") == "number" || elemento.attr("type") == "hidden" || elemento.attr("type") == "text" || elemento.hasClass("combobox")) {
                elemento.val(objeto[name]);
            }

            if (elemento.hasClass("checkByVal") && elemento.attr("type") == "checkbox") {
                if (objeto[name] != null) {
                    var valores = objeto[name].split(",");
                    for (var i = 0; i < valores.length; i++) {
                        var el = jQuery("[value='" + valores[i].trim() + "']");
                        el.prop("checked", true);
                    }
                }
            }else if (elemento.attr("type") == "checkbox") {
                if (objeto[name]) {
                    elemento.prop("checked", true);
                }
            }

            if (elemento.attr("type") == "radio") {
                if (objeto[name] != null) {
                    jQuery("[name='" + name + "'][value='" + objeto[name] + "']").prop("checked", true);
                }
            }
        }
    }
}
//---

//Tratamento de Datas
function dataAtualFormatada() {
    var data = new Date();
    var dia = data.getDate();
    if (dia.toString().length == 1)
        dia = "0" + dia;
    var mes = data.getMonth() + 1;
    if (mes.toString().length == 1)
        mes = "0" + mes;
    var ano = data.getFullYear();

    //if (lang.trim() == "Português (Brasil)") {
        return dia + "/" + mes + "/" + ano;
    //}
    //else {
    //    return mes + "/" + dia + "/" + ano;
    //}
}

function formatarDataParaInsercao(data) {
    if (data != null) {
        var arrayData = data.split("-");
        if (arrayData.length > 1) {
            if (lang.trim() == "Português (Brasil)") {
                return arrayData[2] + "/" + arrayData[1] + "/" + arrayData[0];
            }
            else {
                return arrayData[1] + "/" + arrayData[2] + "/" + arrayData[0];
            }
        }
        else
        {
            arrayData = data.split("/");
            if (arrayData.length > 1) {
                if (lang.trim() == "Português (Brasil)") {
                    return arrayData[0] + "/" + arrayData[1] + "/" + arrayData[2];
                }
                else {
                    return arrayData[1] + "/" + arrayData[0] + "/" + arrayData[2];
                }
            }
            else {
                return data;
            }
        }
    }
}
function formatarDataParaExibicao(data) {
    if (data != null) {
        var arrayData = data.split("-");
        if (arrayData.length > 1) {
            //if (lang.trim() == "Português (Brasil)") {
            //    return arrayData[2] + "/" + arrayData[1] + "/" + arrayData[0];
            //}
            //else {
                //return arrayData[1] + "/" + arrayData[2] + "/" + arrayData[0];
                return arrayData[2] + "/" + arrayData[1] + "/" + arrayData[0];
            //}
        }
        else {
            arrayData = data.split("/");
            if (arrayData.length > 1) {
                if (lang.trim() == "Português (Brasil)") {
                    return arrayData[0] + "/" + arrayData[1] + "/" + arrayData[2];
                }
                else {
                    return arrayData[1] + "/" + arrayData[0] + "/" + arrayData[2];
                }
            }
            else {
                return data;
            }
        }
    }
}

function calculaMesesEntreDatas(dataInicial, dataFinal) {
    var resultado = null;

    //formatação
    if (dataInicial != null) {
        if (dataInicial.indexOf("-") >= 0) {
            dataInicial = formatarDataParaExibicao(dataInicial);
        }
    }

    if (dataFinal != null) {
        if (dataInicial.indexOf("-") >= 0) {
            dataInicial = formatarDataParaExibicao(dataInicial);
        }
    }
    //--

    if ((dataInicial.indexOf("/") >= 0) && (dataFinal.indexOf("/") >= 0)) {
        dataInicial = dataInicial.split("/");
        dataFinal = dataFinal.split("/");

        var data1 = new Date(dataInicial[2] + "/" + dataInicial[1] + "/" + dataInicial[0]);
        var data2 = new Date(dataFinal[2] + "/" + dataFinal[1] + "/" + dataFinal[0]);

        resultado = dateDiferencaEmDias(data1, data2);

        var meses = 0;
        var mes = parseInt(dataInicial[1]);
        var ano = parseInt(dataInicial[2]);

        while (resultado > 0 && resultado >= daysInMonth(mes, ano)) {
            var condicao = daysInMonth(mes, ano);
            var mes = mes + 1;
            if (mes == 13) {
                mes = 1;
                ano = ano + 1;
            }
            var meses = meses + 1;
            resultado = resultado - condicao;
        }
    }

    return meses;
}

function daysInMonth(month, year) {
    return new Date(year, month, 0).getDate();
}

function dateDiferencaEmDias(a, b) {
    // Descartando timezone e horário de verão
    var utc1 = Date.UTC(a.getFullYear(), a.getMonth(), a.getDate());
    var utc2 = Date.UTC(b.getFullYear(), b.getMonth(), b.getDate());

    return Math.floor((utc2 - utc1) / (1000 * 60 * 60 * 24));
}
//---

//Input
function getValNumberById(id) {
    return jQuery("#" + id).val() == "" ? 0 : jQuery("#" + id).val();
}

function setValDefaultById(id, val) {
    jQuery("#" + id).val() == "" ? jQuery("#" + id).val(val) : true;
}

function lpad(num, size) {
    var s = num + "";
    while (s.length < size) s = "0" + s;
    return s;
}

function rpad(num, size) {
    var s = num + "";
    while (s.length < size) s = s + "0";
    return s;
}

function lPadDecimal(v, a, d) {
    arrayV = v.split(',');

    va = arrayV[0];

    if (arrayV.length > 1)
    {
        vd = arrayV[1];
    }
    else {
        vd = "00";
    }

    va = va.replace(/\D/, "");
    va = va.replace(/^[0]+/, "");

    if (va == "") {
        return "0,00";
    }

    v = va + "," + rpad(vd, 2);

    return v;
}


//---

//Buscas
function salvarSemCadastro(cns)
{
    var xTbBusca = 2;
    var xBusca = "Sem Cadastro";
    $.ajax({
        type: "POST",
        data: { tpBusca: xTbBusca, busca: xBusca },
        datatype: "json",
        url: Api.getPaciente
    })
    .done(function (data) {
        if (data.status) {
            var idAtendimento = $(idFicha).val();
            $.each(data.resultado, function (ResultadoItens, item) {
                if (cns == "") {
                    cns = item.cns
                }
                var idPacienteAtendimento = criaPacienteAtendimento(idAtendimento, item.IDUSR, cns, "Sem Cadastro");
                if (idPacienteAtendimento != false) {
                    //makeTablePaciente(item.nome, cns, item.IDUSR, idPacienteAtendimento);
                    getPacientes();
                }                
            });
            $('#modalSemCartaoSus').modal('hide');
            $('#txtmodalBuscaCartaoSus').val('');
            $('#txtmodalBuscaCartaoSus').val('');
        } else {
            if (data.sessao) {
                setTimeout(function () {
                    modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                    setTimeout(function () {
                        window.location.assign("../");
                    }, 2000);
                }, 800);
            }
        }
    });
}

function addSemCadastro() {
    if ($('#txtmodalBuscaCartaoSus').val() != "")
    {
        if ($('#txtmodalBuscaCartaoSus').val() == $('#txtmodalConfirmaCartaoSus').val())
        {            
            salvarSemCadastro($('#txtmodalConfirmaCartaoSus').val());
        }
        else
        {
            $('#modalSemCartaoSus .msgErro .col-md-12').text('O cartão do SUS não confere com o digitado anteriormente.Favor digite novamente.');
        }
    }
    else
    {
        $('#txtmodalBuscaCartaoSus').val('');
        $('#txtmodalConfirmaCartaoSus').val('');
        salvarSemCadastro($('#txtmodalConfirmaCartaoSus').val());
    }

}

//---
function limparCampos(seletor) {
    $(seletor).each(function (i) {
        $(this).find('input[type=checkbox]:checked').removeAttr('checked', false);
        $(this).find('input[type=text]').val('');
        $(this).find('input[type=number]').val('');
        $(this).find('input[type=radio]:checked').removeAttr('checked', false);
    });
}

function saudacao() {

    dtAtu = new Date();
    dia = dtAtu.getDate();
    if (dia < 10) dia = '0' + dia;
    mes = (dtAtu.getMonth() + 1);
    if (mes < 10) mes = '0' + mes;
    hr = dtAtu.getHours();
    if (hr < 10) hr = '0' + hr;
    mn = dtAtu.getMinutes();
    if (mn < 10) mn = '0' + mn;

    saud = 'Bom dia, ';
    if (hr > 12) saud = 'Boa tarde, ';
    if (hr > 18) saud = 'Boa noite, ';

    txtSaudacao = ', ' + dia + " de " + dt_meses[dtAtu.getMonth()] + " de " + dtAtu.getFullYear() + ' - ' + hr + ':' + mn;
    //$('#spSaudacao').html(saudacao);
    $('#spSaudacaoHr').html(txtSaudacao);

}

function validaCampos(form, tipo, codModal) {

    var aux = true;
    var mess = "";

    $(form).find(".has-error").each(function () {
        $(this).removeClass("has-error");
    });

    $(form).find(".obg").each(function () {
        var valor = '' + $(this).val();
        if (!valor.trim()) {
            campo = '' + $("label[for=" + $(this).attr("id") + "]").html();
            campo = campo.replace(/\*/g, "");
            mess = mess + "<h5>Preencha o campo <strong>" + campo + "</strong></h5>";
            $(this).parents(".form-group:first").addClass("has-error");
            aux = false;
        }
    });

    if (aux) {
        if ($('#obg2').val() == '1') {
            $(form).find(".obg2").each(function () {
                if (!$(this).val().trim()) {
                    campo = $("label[for=" + $(this).attr("id") + "]").html();
                    campo = campo.replace(/\*/g, "");
                    mess = mess + "<h5>Preencha o campo <strong>" + campo + "</strong></h5>";
                    $(this).parents(".form-group:first").addClass("has-error");
                    aux = false;
                }
            });
        }
    }

    if (aux) {
        $(form).find(".data").each(function () {
            if ($(this).val() != "") {
                ano = $(this).val().substring(6, 10);
                
                if (verificaData2($(this).val())) {
                    campo = $("label[for=" + $(this).attr("id") + "]").html();
                    campo = campo.replace(/\*/g, "");
                    mess = mess + "<h5>Campo <strong>" + campo + " está inválido!</strong></h5>";
                    $(this).parents(".form-group:first").addClass("has-error");
                    aux = false;
                } else {
                    if (ano < 1900 || ano > hoje.getFullYear()) {
                        campo = $("label[for=" + $(this).attr("id") + "]").html();
                        campo = campo.replace(/\*/g, "");
                        mess = mess + "<h5>O Ano do campo <strong>" + campo + " deve estar entre 1900 e " + hoje.getFullYear() + "</strong></h5>";
                        $(this).parents(".form-group:first").addClass("has-error");
                        aux = false;
                    }
                }
            }
        });
    }

    if (aux) {
        $(form).find(".mes").each(function () {
            if ($(this).val() != "") {
                if ($(this).val() < 1 || $(this).val() > 12) {
                    campo = $("label[for=" + $(this).attr("id") + "]").html();
                    campo = campo.replace(/\*/g, "");
                    mess = mess + "<h5>O campo <strong>" + campo + "</strong> está inválido!</h5>";
                    $(this).parents(".form-group:first").addClass("has-error");
                    aux = false;
                }
            }
        });
    }

    if (aux) {
        $(form).find(".ano").each(function () {
            if ($(this).val() != "") {
                if ($(this).val() < 1900 || $(this).val() > hoje.getFullYear()) {
                    campo = $("label[for=" + $(this).attr("id") + "]").html();
                    campo = campo.replace(/\*/g, "");
                    mess = mess + "<h5>O Ano do campo <strong>" + campo + " deve estar entre 1900 e " + hoje.getFullYear() + "</strong></h5>";
                    $(this).parents(".form-group:first").addClass("has-error");
                    aux = false;
                }
            }
        });
    }

    if (aux) {
        $(form).find(".datepickerMaxDateAtual").each(function () {
            if ($(this).val() != "") {
                if (!verificaData2($(this).val())) {
                    if (!comparaData($(this).val(), dt_max)) {
                        campo = $("label[for=" + $(this).attr("id") + "]").html();
                        campo = campo.replace(/\*/g, "");
                        mess = mess + "<h5>A data do campo <strong>" + campo + "</strong> deve ser menor ou igual a data atual!</h5>";
                        $(this).parents(".form-group:first").addClass("has-error");
                        aux = false;
                    }
                }
            }
        });
    }

    if (aux) {
        $(form).find(".datepickerMinDateAtual").each(function () {
            if ($(this).val() != "") {
                if (!verificaData2($(this).val())) {
                    if (!comparaData($(this).val(), dt_atual, 1)) {
                        campo = $("label[for=" + $(this).attr("id") + "]").html();
                        campo = campo.replace(/\*/g, "");
                        mess = mess + "<h5>A data do campo <strong>" + campo + "</strong> deve ser maior ou igual a data atual!</h5>";
                        $(this).parents(".form-group:first").addClass("has-error");
                        aux = false;
                    }
                }
            }
        });
    }

    if (aux) {
        $(form).find(".hora").each(function () {
            if ($(this).val() != "") {
                if (!verificaHora($(this).val())) {
                    campo = $("label[for=" + $(this).attr("id") + "]").html();
                    campo = campo.replace(/\*/g, "");
                    mess = mess + "<h5>Campo <strong>" + campo + " está inválido!</strong></h5>";
                    $(this).parents(".form-group:first").addClass("has-error");
                    aux = false;
                }
            }
        });
    }

    if (aux) {
        $(form).find(".email").each(function () {
            if (!validateEmail($(this).val().trim())) {
                campo = $("label[for=" + $(this).attr("id") + "]").html();
                campo = campo.replace(/\*/g, "");
                mess = mess + "<h5>Campo <strong>" + campo + " está inválido!</strong></h5>";
                $(this).parents(".form-group:first").addClass("has-error");
                aux = false;
            }
        });
    }

    if (aux) {
        $(form).find(".cpf").each(function () {
            if (!validateCPF($(this).val().trim())) {
                campo = $("label[for=" + $(this).attr("id") + "]").html();
                campo = campo.replace(/\*/g, "");
                mess = mess + "<h5>Campo <strong>" + campo + " está inválido!</strong></h5>";
                $(this).parents(".form-group:first").addClass("has-error");
                aux = false;
            }
        });
    }

    if (aux) {
        $(form).find(".cnpj").each(function () {
            if (!validateCNPJ($(this).val().trim())) {
                campo = $("label[for=" + $(this).attr("id") + "]").html();
                campo = campo.replace(/\*/g, "");
                mess = mess + "<h5>Campo <strong>" + campo + " está inválido!</strong></h5>";
                $(this).parents(".form-group:first").addClass("has-error");
                aux = false;
            }
        });
    }

    if (!aux) {
        if (tipo == 1) {
            $('#alertaSemModal').html(mess);
            $('#alertaSemModal').slideDown();
            setTimeout(function () {
                $('#alertaSemModal').slideUp();
            }, 5000);
        } else if(tipo == 2) {
            $('#' + codModal).html(mess);
            $('#' + codModal).slideDown();
            setTimeout(function () {
                $('#'+codModal).slideUp();
            }, 5000);
        } else {
            modalAlerta("Atenção", mess);
        }
        return false;
    } else {
        return true;
    }

}

function SomenteNumero(e, caracter) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) {
            return true;
        }
        else {
            var tem = false;
            if (caracter) {
                var array = caracter.split(",");
                $.each(array, function (key, item) {
                    if (tecla == item) tem = true;
                })
            }
            if (tem) return true;
            else return false;
        }
    }
}

function removerAcentos(newStringComAcento) {
    var string = newStringComAcento;
    var mapaAcentosHex = {
        a: /[\xE0-\xE6]/g,
        A: /[\xC0-\xC6]/g,
        e: /[\xE8-\xEB]/g,
        E: /[\xC8-\xCB]/g,
        i: /[\xEC-\xEF]/g,
        I: /[\xCC-\xCF]/g,
        o: /[\xF2-\xF6]/g,
        O: /[\xD2-\xD6]/g,
        u: /[\xF9-\xFC]/g,
        U: /[\xD9-\xDC]/g,
        c: /\xE7/g,
        C: /\xC7/g,
        n: /\xF1/g,
        N: /\xD1/g
    };

    for (var letra in mapaAcentosHex) {
        var expressaoRegular = mapaAcentosHex[letra];
        string = string.replace(expressaoRegular, letra);
    }

    return string;
}

function verificaData(dia, mes, ano) {
    // verifica o dia valido para cada mes 
    if ((dia < 01) || (dia < 01 || dia > 30) && (mes == 04 || mes == 06 || mes == 09 || mes == 11) || dia > 31) {
        return true;
    }

        // verifica se o mes e valido 
    else if (mes < 01 || mes > 12) {
        return true;
    }

        // verifica se e ano bissexto 
    else if (mes == 2 && (dia < 01 || dia > 29 || (dia > 28 && (parseInt(ano / 4) != ano / 4)))) {
        return true;
    }
    else {
        return false;
    }
}

function verificaData2(data) {
    var dia, mes, ano

    if (data == '') {
        return false;
    }

    dia = data.substring(0, 2)
    mes = data.substring(3, 5)
    ano = data.substring(6, 10)

    // verifica o dia valido para cada mes 
    if ((dia < 01) || (dia < 01 || dia > 30) && (mes == 04 || mes == 06 || mes == 09 || mes == 11) || dia > 31) {
        return true;
    }

        // verifica se o mes e valido 
    else if (mes < 01 || mes > 12) {
        return true;
    }

        // verifica se e ano bissexto 
    else if (mes == 2 && (dia < 01 || dia > 29 || (dia > 28 && (parseInt(ano / 4) != ano / 4)))) {
        return true;
    }
    else {
        return false;
    }
}

function comparaData(dtInicial, dtFinal, tipo) {
    var dtini = dtInicial;
    var dtfim = dtFinal;

    if ((dtini == '') && (dtfim == '')) {
        alert('Complete os Campos.');
        campos.inicial.focus();
        return false;
    }

    datInicio = new Date(dtini.substring(6, 10),
						 dtini.substring(3, 5),
						 dtini.substring(0, 2));
    datInicio.setMonth(datInicio.getMonth() - 1);


    datFim = new Date(dtfim.substring(6, 10),
					  dtfim.substring(3, 5),
					  dtfim.substring(0, 2));

    datFim.setMonth(datFim.getMonth() - 1);

    if (tipo) {
        if (datInicio >= datFim) {
            return true;
        } else {
            return false;
        }
    }
    else {
        if (datInicio <= datFim) {
            return true;
        } else {
            return false;
        }
    }
}

function verificaHora(hora) {
    if (hora.length == 5) {
        hrs = (hora.substring(0, 2));
        min = (hora.substring(3, 5));
        aux = true;

        if ((hrs < 00) || (hrs > 23) || (min < 00) || (min > 59)) {
            aux = false;
        }

        return aux;
    } else {
        return true;
    }
}

function comparaHora(horaInicial, horaFinal) {
    if (horaInicial.length > 0 && horaFinal.length > 0) {
        horaIni = horaInicial.split(':');
        horaFim = horaFinal.split(':');

        hIni = parseInt(horaIni[0], 10);
        hFim = parseInt(horaFim[0], 10);
        if (hIni != hFim)
            return hIni < hFim;

        mIni = parseInt(horaIni[1], 10);
        mFim = parseInt(horaFim[1], 10);
        if (mIni != mFim)
            return mIni < mFim;
    } else {
        return true;
    }
}

function inArray(a, obj) {
    var i = a.length;
    while (i--) {
        if (a[i] === obj) {
            return true;
        }
    }
    return false;
}

function alerter(title, message) {
    $('#bmodal').focus();
    if ($('#bmodal').length == 0) {
        $('body').after('<div id="bmodal"></div>');
    }
    $('#bmodal').attr('title', title);
    $('#bmodal').html(message);
    $('#bmodal').dialog({
        modal: true,
        buttons: {
            "Ok": function () { $(this).dialog("close") }
        }
    });
    $('#bmodal button').focus();
}

function validateCPF(c) {

    if (c == '') {
        return true;
    }

    var i;
    s = c.replace(/\.|-|\//gi, ''); // elimina .(ponto), -(hifem) e /(barra)
    var c = s.substr(0, 9);
    var dv = s.substr(9, 2);
    var d1 = 0;
    var v = false;

    for (i = 0; i < 9; i++) {
        d1 += c.charAt(i) * (10 - i);
    }
    if (d1 == 0) {
        v = true;
        return false;
    }
    d1 = 11 - (d1 % 11);
    if (d1 > 9) d1 = 0;
    if (dv.charAt(0) != d1) {
        v = true;
        return false;
    }

    d1 *= 2;
    for (i = 0; i < 9; i++) {
        d1 += c.charAt(i) * (11 - i);
    }
    d1 = 11 - (d1 % 11);
    if (d1 > 9) d1 = 0;
    if (dv.charAt(1) != d1) {
        v = true;
        return false;
    }
    if (!v) {
        return true;
    }
}

function validateCNPJ(cnpj) {

    if (cnpj == '') {
        return true;
    }

    cnpj = cnpj.replace(/[^\d]+/g, '');

    if (cnpj == '') return false;

    if (cnpj.length != 14)
        return false;

    // Elimina CNPJs invalidos conhecidos
    if (cnpj == "00000000000000" ||
        cnpj == "11111111111111" ||
        cnpj == "22222222222222" ||
        cnpj == "33333333333333" ||
        cnpj == "44444444444444" ||
        cnpj == "55555555555555" ||
        cnpj == "66666666666666" ||
        cnpj == "77777777777777" ||
        cnpj == "88888888888888" ||
        cnpj == "99999999999999")
        return false;

    // Valida DVs
    tamanho = cnpj.length - 2
    numeros = cnpj.substring(0, tamanho);
    digitos = cnpj.substring(tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(0))
        return false;

    tamanho = tamanho + 1;
    numeros = cnpj.substring(0, tamanho);
    soma = 0;
    pos = tamanho - 7;
    for (i = tamanho; i >= 1; i--) {
        soma += numeros.charAt(tamanho - i) * pos--;
        if (pos < 2)
            pos = 9;
    }
    resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
    if (resultado != digitos.charAt(1))
        return false;

    return true;

}

function validateEmail(email) {

    if (email == '') {
        return true;
    }

    er = /^[a-zA-Z0-9][a-zA-Z0-9\._-]+@([a-zA-Z0-9\._-]+\.)[a-zA-Z-0-9]{2}/;

    if (er.exec(email)) {
        return true;
    } else {
        return false;
    }
}

function montarDataTables(idTabela, ordem, tipoOrdem) {
    $('#' + idTabela).dataTable({
        "language": {
            "sEmptyTable": "<center>Nenhum registro encontrado</center>",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
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
        }],
        "order": [[ordem, "" + tipoOrdem]]
    });
}

function formatReal(valor) {
    var inteiro = null, decimal = null, c = null, j = null;
    var aux = new Array();
    valor = "" + valor;
    c = valor.indexOf(".", 0);
    //encontrou o ponto na string
    if (c > 0) {
        //separa as partes em inteiro e decimal
        inteiro = valor.substring(0, c);
        decimal = valor.substring(c + 1, valor.length);
    } else {
        inteiro = valor;
    }

    //pega a parte inteiro de 3 em 3 partes
    for (j = inteiro.length, c = 0; j > 0; j -= 3, c++) {
        aux[c] = inteiro.substring(j - 3, j);
    }

    //percorre a string acrescentando os pontos
    inteiro = "";
    for (c = aux.length - 1; c >= 0; c--) {
        inteiro += aux[c] + '.';
    }
    //retirando o ultimo ponto e finalizando a parte inteiro

    inteiro = inteiro.substring(0, inteiro.length - 1);

    decimal = parseInt(decimal);
    if (isNaN(decimal)) {
        decimal = "00";
    } else {
        decimal = "" + decimal;
        if (decimal.length === 1) {
            decimal = decimal + "0";
        }
    }


    valor = inteiro + "," + decimal;


    return valor;

}

function allReplace(string, token, newtoken) {
    while (string.indexOf(token) != -1) {
        string = string.replace(token, newtoken);
    }
    return string;
}

function alteraSenha() {

    $('#modalAlterarSenha').find(".has-error").each(function () {
        $(this).removeClass("has-error");
    });

    $("#modalAlterarSenha").find("input[type=password]").each(function () {
        $('#' + $(this).context.name).val('');
    });

    $('#modalAlterarSenha').modal('show');
}

function salvaAlteraSenha() {

    $('#modalAlterarSenha').find(".has-error").each(function () {
        $(this).removeClass("has-error");
    });

    var xSenhaAtual = $('#senhaAtual').val();
    var xNovaSenha = $('#novaSenha').val();
    var xNovaSenha2 = $('#novaSenha2').val();

    if (validaCampos($("#modalAlterarSenha"), 2, "alertaSemModalAltSenha")) {

        if (xNovaSenha == xNovaSenha2) {
            
            $.ajax({
                type: "POST",
                data: { senhaAtual: xSenhaAtual, novaSenha: xNovaSenha },
                datatype: "json",
                url: "ajax/seguranca/setSenha.asp"
            })
            .done(function (data) {
                if (data.status) {
                    $('#modalAlterarSenha').modal('hide');
                    modalAlerta('Alteração de Senha', 'Senha alterada com sucesso!');
                    setTimeout(function () {
                        $('#alertaModal').modal('hide');
                    }, 1500);
                } else {
                    if (data.sessao) {
                        $('#page-wrapper').css("opacity", "1");
                        $('.imgCarregando').hide();
                        setTimeout(function () {

                            modalAlerta("Sessão Expirada", "Sua sessão expirou!");

                            setTimeout(function () {
                                window.location.assign("../");
                            }, 2000);
                        }, 800);
                    }

                    if (data.senhaAtual) {
                        $('#senhaAtual').parents(".form-group").addClass("has-error");

                        $('#alertaSemModalAltSenha').html('<h5>Senha atual inválida!</h5>');
                        $('#alertaSemModalAltSenha').slideDown();
                        setTimeout(function () {
                            $('#alertaSemModalAltSenha').slideUp();
                        }, 5000);
                    } else {
                        $('#modalAlterarSenha').modal('hide');
                        modalAlerta('Alteração de Senha', 'Falha ao alterar senha!');
                    }
                }
            });

        } else {
            $('#novaSenha').parents(".form-group").addClass("has-error");
            $('#novaSenha2').parents(".form-group").addClass("has-error");

            $('#alertaSemModalAltSenha').html('<h5>As senhas não conferem!</h5>');
            $('#alertaSemModalAltSenha').slideDown();
            setTimeout(function () {
                $('#alertaSemModalAltSenha').slideUp();
            }, 5000);
        }

    }

}

function criaLoading(alvoID) {
    boxCarregando = '<div class="div-carregando" id="div-carregando-' + alvoID + '">';
    boxCarregando +=    '<div class="div-carregando-fundo"></div>';
    boxCarregando +=    '<div class="div-carregando-conteudo">';
    boxCarregando += '<img src="img/ajax-loader-2.gif" /><br>';
    boxCarregando +=        '<span><b>Carregando...</b></span>';
    boxCarregando +=    '</div>';
    boxCarregando += '</div>';

    if (!$('#div-carregando-' + alvoID).length) {
        $('body').append(boxCarregando);
    }

    $('#div-carregando-' + alvoID).css({
        display: 'block',
        width: $('#' + alvoID).css("width"),
        height: $('#' + alvoID).css("height"),
        top: $('#' + alvoID).offset().top,
        left: $('#' + alvoID).offset().left
    })
}

function destroiLoading(alvoID) {
    $('#div-carregando-' + alvoID).remove();
}

function validateCNS(cns) {
    //Rotina de validação de Números que iniciam com “1” ou “2”

    /*
        *Chamar a rotina em um evento do form. Exemplo:
            <input name="cns" type="text" title="cns" onChange="validaCNS(this.value)" value="cns" size="15" maxlength="15" />
    */
    var nInicial = Number(cns.substring(0, 1));
    var retorno = false;
    if (nInicial == 1 || nInicial == 2) {
        retorno = validaCNS(cns);
    } else if (nInicial == 7 || nInicial == 8 || nInicial == 9) {
        retorno = ValidaCNS_PROV(cns);
    }

    return retorno;

    // Validação CNS
    function validaCNS(vlrCNS) {
        // Formulário que contem o campo CNS
        var soma = new Number;
        var resto = new Number;
        var dv = new Number;
        var pis = new String;
        var resultado = new String;
        var tamCNS = vlrCNS.length;
        if ((tamCNS) != 15) {
            //alert("Numero de CNS invalido");
            return false;
        }
        pis = vlrCNS.substring(0,11);
        soma = (((Number(pis.substring(0,1))) * 15) +   
	            ((Number(pis.substring(1,2))) * 14) +
		        ((Number(pis.substring(2,3))) * 13) +
		        ((Number(pis.substring(3,4))) * 12) +
                ((Number(pis.substring(4,5))) * 11) +
                ((Number(pis.substring(5,6))) * 10) +
                ((Number(pis.substring(6,7))) * 9) +
                ((Number(pis.substring(7,8))) * 8) +
                ((Number(pis.substring(8,9))) * 7) +
                ((Number(pis.substring(9,10))) * 6) +
                ((Number(pis.substring(10,11))) * 5));
        resto = soma % 11;
        dv = 11 - resto;
        if (dv == 11) {
            dv = 0;
        }
        if (dv == 10) {
            soma = (((Number(pis.substring(0,1))) * 15) +   
	                ((Number(pis.substring(1,2))) * 14) +
		    	    ((Number(pis.substring(2,3))) * 13) +
		    	    ((Number(pis.substring(3,4))) * 12) +
            	    ((Number(pis.substring(4,5))) * 11) +
            	    ((Number(pis.substring(5,6))) * 10) +
            	    ((Number(pis.substring(6,7))) * 9) +
            	    ((Number(pis.substring(7,8))) * 8) +
            	    ((Number(pis.substring(8,9))) * 7) +
            	    ((Number(pis.substring(9,10))) * 6) +
            	    ((Number(pis.substring(10,11))) * 5) + 2);
            resto = soma % 11;
            dv = 11 - resto;
            resultado = pis + "001" + String(dv);
        } else { 
            resultado = pis + "000" + String(dv);
        }
        if (vlrCNS != resultado) {
            //alert("Numero de CNS invalido");
            return false;
        } else {
            //alert("Numero de CNS válido");
            return true;
        }
    }
    //Rotina de validação de Números que iniciam com “7”, “8” ou “9”

    function ValidaCNS_PROV(Obj)
    {
        var pis;
        var resto;
        var dv;
        var soma;
        var resultado;
        var result;
        result = 0;

        pis = Obj.substring(0,15);

        if (pis == "")
        {
            return false
        }
	    
        if ( (Obj.substring(0,1) != "7")  && (Obj.substring(0,1) != "8") && (Obj.substring(0,1) != "9") )
        {
            //alert("Atenção! Número Provisório inválido!");
            return false
        }
 
        soma = (   (parseInt(pis.substring( 0, 1),10)) * 15)
                + ((parseInt(pis.substring( 1, 2),10)) * 14)
                + ((parseInt(pis.substring( 2, 3),10)) * 13)
                + ((parseInt(pis.substring( 3, 4),10)) * 12)
                + ((parseInt(pis.substring( 4, 5),10)) * 11)
                + ((parseInt(pis.substring( 5, 6),10)) * 10)
                + ((parseInt(pis.substring( 6, 7),10)) * 9)
                + ((parseInt(pis.substring( 7, 8),10)) * 8)
                + ((parseInt(pis.substring( 8, 9),10)) * 7)
                + ((parseInt(pis.substring( 9,10),10)) * 6)
                + ((parseInt(pis.substring(10,11),10)) * 5)
                + ((parseInt(pis.substring(11,12),10)) * 4)
                + ((parseInt(pis.substring(12,13),10)) * 3)
                + ((parseInt(pis.substring(13,14),10)) * 2)
                + ((parseInt(pis.substring(14,15),10)) * 1);

        resto = soma % 11;
	
        if (resto == 0)
        {
            return true;
        }
        else
        {
            //alert("Atenção! Número Provisório inválido!");
            return false;  
        }   
    }
/*
    Observações:

    1)	Não existe máscara para o CNS nem para o Número Provisório. O número que aparece no cartão de forma separada (898  0000  0004  3208) deverá ser digitado sem as separações.
    2)	O 16º número que aparece no Cartão é o número da via do cartão, não é deverá ser digitado.
*/
}

function validaTableSeContemErros(seletor, checkDados, dados, form, blnSubmit, validates, notificacao) {
    var contemErros = false;

    jQuery(seletor).each(function (linha, elemento) {
        if (!contemErros) {
            participantes = false;
            jQuery(checkDados).hide();

            jQuery(dados).show(function () {
                jQuery(seletor)[linha].click();
            });

            if (blnSubmit) {
                jQuery(form).submit();
            } else {
                if (verificarItensnaTablePaciente() && validates())
                {
                    participantes = true;
                }
                else
                {
                    participantes = false;
                    notificacao();
                }
            }

            if (!participantes) {
                contemErros = true;
            }
        }
    });

    return contemErros;
}

function pegarValores(seletor) {
    var dados = new Object;
    $(seletor).find("input[type=text], input[type=hidden], select, textarea").each(function () {
        if ($(this).context.type == "radio" && $(this).is(':checked')) {
            dados[$(this).context.name] = $(this).context.value;
        } else { dados[$(this).context.name] = $(this).context.value.trim(); }
    })
    $(seletor).find("input[type=radio]:checked").each(function () {
        var valor = '';
        if ($(this).context.value != 'on') { valor = $(this).context.value; }
        else if ($(this).context.id != '') { valor = $(this).context.id; }
        else valor = 'on';
        dados[$(this).context.name] = valor;
    })
    $(seletor).find("input[type=checkbox]:checked").each(function () {
        var valor = '';
        if ($(this).context.value != 'on') { valor = $(this).context.value; }
        else if ($(this).context.id != '') { valor = $(this).context.id; }
        else valor = 'on';

        if (dados[$(this).context.name] != '' && dados[$(this).context.name] != undefined) { dados[$(this).context.name] += ',' + valor; }
        else { dados[$(this).context.name] = valor; }
    });
    return dados;
}

function busca_cep(id_cep, id_Uf, id_Municipio, id_Bairro, id_Logradouro, id_TipoEnd, id_Numero) {
    xCep = $("#" + id_cep).val();
    xCep = xCep.replace(/_/g, "");
    xCep = xCep.replace("-", "");
    if (xCep.length == 8) {
        $("#" + id_cep).attr("disabled", "disabled");
        $("#" + id_TipoEnd + ",#" + id_Logradouro + ",#" + id_Bairro + ",#" + id_Uf + ",#" + id_Municipio).val("");
        if (xCep == "99999999") {/*Endereço não informado*/
            $("#" + id_TipoEnd + ",#" + id_Logradouro + ",#" + id_Bairro + ",#" + id_Uf + ",#" + id_Municipio).attr("disabled", "disabled");
            $("label[for=" + id_cep + "]").html("CEP *");
            $("#" + id_cep).removeClass("obg").addClass("obg").removeAttr("disabled");
            $("#" + id_TipoEnd).val("081");
            $("#" + id_Logradouro + ",#" + id_Bairro + ",#" + id_Municipio).val("SEM INFORMACAO");
            $("#" + id_Numero).val("0").attr('disabled', 'disabled');
            $("#" + id_Uf).val("SI");
        } else if (xCep == "14900000" || xCep == "11740000") {/*um Cep para a cidade*/
            $("#" + id_TipoEnd + ",#" + id_Logradouro + ",#" + id_Bairro + ",#" + id_Numero).val("").removeAttr("disabled");
            $("label[for=" + id_cep + "]").html("CEP *");
            $("#" + id_cep).removeClass("obg").addClass("obg").removeAttr("disabled");
            $("#" + id_Uf).val("SP");
            if (xCep == "14900000") $("#" + id_Municipio).val("ITAPOLIS");
            if (xCep == "11740000") $("#" + id_Municipio).val("ITANHAEM");
        } else {
            $.ajax({
                type: "POST",
                data: { cep: xCep },
                datatype: "json",
                url: "../_inc/ajax/cep/default.asp"
            })
            .done(function (data) {
                if (data.status) {
                    $.each(data.resultado, function (ResultadoItens, item) {
                        //$("#" + id_TipoEnd + ",#" + id_Logradouro + ",#" + id_Bairro + ",#" + id_Uf + ",#" + id_Municipio).attr("disabled", "disabled");
                        $("label[for=" + id_cep + "]").html("CEP *");
                        $("#" + id_cep).removeClass("obg").addClass("obg").removeAttr("disabled");
                        $("#" + id_TipoEnd).val(item.idTipo).keyup();
                        $("#" + id_Logradouro).val(item.logradouro).keyup();
                        $("#" + id_Bairro).val(item.bairro).keyup();
                        $("#" + id_Municipio).val(item.municipio).keyup();//trazer o municipio com o estado
                        $("#" + id_Uf).val(item.uf).keyup();
                        $("#" + id_Numero).val("").removeAttr("disabled").focus();
                    });
                } else {
                    if (data.sessao) {
                        setTimeout(function () {
                            modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                            setTimeout(function () {
                                window.location.assign("../");
                            }, 2000);
                        }, 800);
                    } else {
                        $("#" + id_cep + ",#" + id_TipoEnd + ",#" + id_Logradouro + ",#" + id_Bairro + ",#" + id_Uf + ",#" + id_Municipio + ",#" + id_Numero)
                            .val("").removeAttr("disabled").each(function () {
                                $(this).val('');
                                $(this).parents(".form-group").removeClass("has-error");
                            });
                        $("#modalEndereco").find("#End_id_cep,#End_id_TipoEnd,#End_id_Logradouro,#End_id_Bairro,#End_id_Municipio,#End_id_Uf").val('');
                        $("#modalEndereco #End_id_cep").val(id_cep);
                        $("#modalEndereco #End_id_TipoEnd").val(id_TipoEnd);
                        $("#modalEndereco #End_id_Logradouro").val(id_Logradouro);
                        $("#modalEndereco #End_id_Bairro").val(id_Bairro);
                        $("#modalEndereco #End_id_Municipio").val(id_Municipio);
                        $("#modalEndereco #End_id_Uf").val(id_Uf);
                        $("#modalEndereco_Lista tbody").html('<tr><th colspan="6" class="text-center">Cep não encontrado! <br> Efetue a busca acima</th></tr>');
                        $("#modalEndereco").modal('show');
                    }
                }
            });
        }
    }
}

function fieldsPreDefinidos() {
    var fields = '';
    //define quais campos serão pesquisados
    if ($('.field_SetoresPermitidos').length > 0) fields += ',field_SetoresPermitidos';
    if ($('.field_RacaCor').length > 0) fields += ',field_RacaCor';
    //executa as pesquisas
    if (fields!='') {
        $.ajax({
            type: "POST",
            data: { fields: fields.substr(1) },
            datatype: "json",
            url: "../_inc/ajax/CamposPreDefinidos/default.asp"
        })
        .done(function (data) {
            $.each(data, function (Campo, Opcoes) {
                if ($('.' + Campo + ' option[value=""]').length==0) {
                    $('.' + Campo).append('<option value="">Selecione</option>');
                } 
                $.each(Opcoes, function (indice, Opcao) {
                    var selecionado = (Opcao.PADRAO==1) ? ' selected="selected" ' : '';
                    $('.' + Campo).append('<option value="' + Opcao.ID + '"' + selecionado + '>' + Opcao.DESCRICAO + '</option>');
                });
            });
        })
    }
}

function tpField_seletor(alvo) {
    /*
    Exemplo: <input type="text" class="form-control tpField_seletor field_Credenciados obg" name="A_ID_CREDENCIADO" id="A_ID_CREDENCIADO"/>
    */
    if (alvo!='') {alvo += ' '}
    $(alvo + '.tpField_seletor').each(function () {
        var fields = '';
        //define quais campos serão pesquisados
        if ($(this).hasClass('field_Credenciados')) fields = 'field_Credenciados';
        if ($(this).hasClass('field_Pacientes')) fields = 'field_Pacientes';
        if ($(this).hasClass('field_CID')) fields = 'field_CID';
        if ($(this).hasClass('field_Paises')) fields = 'field_Paises';
        if ($(this).hasClass('field_Municipios')) fields = 'field_Municipios';
        if ($(this).hasClass('field_TipoLogradouro')) fields = 'field_TipoLogradouro';
        if ($(this).hasClass('field_Etnia')) fields = 'field_Etnia';
        if ($(this).hasClass('field_Ocupacao')) fields = 'field_Ocupacao';
        if ($(this).hasClass('field_Escolaridade')) fields = 'field_Escolaridade';
        if ($(this).hasClass('field_CIAP2')) fields = 'field_CIAP2';
        if ($(this).hasClass('field_SIGTAP')) fields = 'field_SIGTAP';
        if ($(this).hasClass('field_EXAME')) fields = 'field_EXAME';
        
        //executa as pesquisas
        if (fields != '') {
            var Timer;
            var Campo = $(this).attr('id');
            //INUTILIDADE - inicio +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            $(this)
                .after('<span class="input-group-addon" onclick="$(this).prev().keyup()">&#8681;</span>')
                .parents('.form-group:first')
                .addClass('input-group')
                .find('label')
                .insertBefore($(this).parent());
            //INUTILIDADE - fim ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            $(this).before($(this).clone().hide());
            $(this).attr({ 'id': Campo + '_DESCRICAO', 'name': Campo + '_DESCRICAO', 'autocomplete': 'off' }).removeClass('obg');
            $(this).bind('keyup', function () {
                clearTimeout(Timer);
                Timer = setTimeout(function () { tpField_seletorNovo(Campo + '_DESCRICAO', fields) }, 800);
            });
            if ($('#' + Campo + '_DESCRICAO').val()!='') {
                tpField_seletorNovo(Campo + '_DESCRICAO', fields);
            }
        }
    })
}
function tpField_seletorNovo(elemento, fields) {
    $('.tpField_seletorContainer').remove();
    //Cria Container
    $('#' + elemento).parent().append('  <div class="tpField_seletorContainer">' +
                                '      <div class="tpField_seletorContainerBuscando"><img src="../_inc/img/ajax-loader-2.gif"></div>' +
                                '      <div class="tpField_seletorContainerVazio">Nenhum Registro encontrado</div>' +
                                '      <div class="tpField_seletorContainerConteudo">' +
                                '          <div class="item">texto1</div>' +
                                '          <div class="item">texto2</div>' +
                                '          <div class="item">texto3</div>' +
                                '      </div>' +
                                '      <div class="tpField_seletorContainerRodape">' +
                                '          <span class="tpField_seletorContainerRodapeMin"></span> à <span class="tpField_seletorContainerRodapeMax"></span> de <span class="tpField_seletorContainerRodapeTotal"></span>' +
                                '          <div class="tpField_seletorContainerControles">' +
                                '              <div class="tpField_seletorContainerBtnAnte">&#8678;</div>' +
                                '              <div class="tpField_seletorContainerBtnProx">&#8680;</div>' +
                                '          </div>' +
                                '      </div>' +
                                '  </div>');
    $('.tpField_seletorContainer').css({
        'background-color': 'white',
        'border': '1px rgb(102, 175, 233) solid',
        'box-shadow': '0 0 5px #4286f4',
        'position': 'absolute',
        'top': '100%',
        'left': '0px',
        'width': '100%',
        'z-index': '1500'
    });
    //Para Fechar caso clique fora
    $(document).mouseup(function (e) {
        var container = $('.tpField_seletorContainer,#' + elemento);
        if (!container.is(e.target) // if the target of the click isn't the container...
            && container.has(e.target).length === 0) // ... nor a descendant of the container
        {
            $('.tpField_seletorContainerRemover,.tpField_seletorContainer').remove();
            if ($('#' + elemento.replace('_DESCRICAO', '')).val()=='') {
                $('#' + elemento).val('');
            } 
        }
    });
    $('.tpField_seletorContainerBuscando,.tpField_seletorContainerVazio').css('text-align', 'center');
    $('.tpField_seletorContainerConteudo').css('overflow-x','hidden');
    $('.tpField_seletorContainerRodape').css({
        'border': '1px rgb(102, 175, 233) solid',
        'background-color': 'lightgreen'
    });
    $('.tpField_seletorContainerControles').css('float', 'right');
    $('.tpField_seletorContainerBtnAnte,.tpField_seletorContainerBtnProx').css({
        'display': 'inline-block',
        'cursor': 'pointer'
    });
    tpField_seletorBusca(elemento, fields,0);
}
function tpField_seletorBusca(elemento, field, RI) {
    $('.tpField_seletorContainerBuscando').show();
    $('.tpField_seletorContainerVazio,.tpField_seletorContainerConteudo,.tpField_seletorContainerRodape').hide();
    $.ajax({
        type: "POST",
        data: {
            busca: $('#' + elemento).val(),
            fields: field,
            RI: RI
        },
        datatype: "json",
        url: "../_inc/ajax/CamposPreDefinidos/default.asp"
    })
    .done(function (data) {
        $('.tpField_seletorContainerConteudo').html('');
        $('#' + elemento.replace('_DESCRICAO', '')).val('');
        if (data.resultados.length > 1) {
            $('.tpField_seletorContainerConteudo,.tpField_seletorContainerRodape').show();
            $('.tpField_seletorContainerVazio,.tpField_seletorContainerBuscando').hide();
            //paginação-Ini
            $('.tpField_seletorContainerRodapeMin').html(data.resultados[0].ROW_NUMBER);
            $('.tpField_seletorContainerBtnAnte').unbind('click');
            if ((parseInt(data.resultados[0].ROW_NUMBER) - 16) >= 0) {
                $('.tpField_seletorContainerBtnAnte').click(function () { tpField_seletorBusca(elemento, field, parseInt(data.resultados[0].ROW_NUMBER) - 16) });
            }
            $('.tpField_seletorContainerBtnProx').unbind('click');
            if ((parseInt(data.resultados[0].ROW_NUMBER) + 14) < parseInt(data.resultados[0].TOTAL)) {
                $('.tpField_seletorContainerRodapeMax').html(parseInt(data.resultados[0].ROW_NUMBER) + 4);
                $('.tpField_seletorContainerBtnProx').click(function () { tpField_seletorBusca(elemento, field, parseInt(data.resultados[0].ROW_NUMBER) + 14) });
            } else {
                $('.tpField_seletorContainerRodapeMax').html(data.resultados[0].TOTAL);
            }
            $('.tpField_seletorContainerRodapeTotal').html(data.resultados[0].TOTAL);
            //paginação-Fim
            $.each(data.resultados, function (indice, resultado) {
                var novalinha = '<div class="item" value="' + resultado.CODIGO + '" title="' + resultado.NOME + '">' + resultado.NOME + '</div>';
                if (resultado.PADRAO == '1') novalinha = novalinha.replace('class="item"', 'class="item padrao"');
                $('.tpField_seletorContainerConteudo').append(novalinha);
            })
            $('.tpField_seletorContainerConteudo .item').hover(function () { $(this).css('background-color', '#befebe') }, function () { $(this).css('background-color', '#ffffff') });
            $('.tpField_seletorContainerConteudo .item').click(function () { $('#' + $('#' + elemento).attr('id').replace('_DESCRICAO', '')).val($(this).attr('value')).change().blur(); $('#' + elemento).val($(this).html()).change().blur(); $('.tpField_seletorContainerRemover,.tpField_seletorContainer').remove(); });
            $('.tpField_seletorContainerConteudo .item').css({ 'white-space': 'nowrap', 'font-size': 'smaller', 'cursor': 'pointer' });
            //AUTOSELECT caso tenha apenas 1
            //if ($('.tpField_seletorContainerConteudo .item').length == 1) {
            //    $('.tpField_seletorContainerConteudo .item').click();
            //}
        } else if (data.resultados.length == 1) {//AUTOSELECT caso tenha apenas 1
            $('#' + elemento.replace('_DESCRICAO', '')).val(data.resultados[0].CODIGO).change().blur();
            $('#' + elemento).val(data.resultados[0].NOME).change().blur();
            $('.tpField_seletorContainerRemover,.tpField_seletorContainer').remove();
        } else {
            $('.tpField_seletorContainerVazio').show();
            $('.tpField_seletorContainerBuscando,.tpField_seletorContainerConteudo,.tpField_seletorContainerRodape').hide();
        }
    });
}

function zesq(n) {
    n = n.toString();
    if (n.length == 1) { n = '0' + n }
    return n;
}
   