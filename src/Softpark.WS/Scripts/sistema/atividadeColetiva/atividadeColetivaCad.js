var Api = { URL: "ajax/atividadeColetiva/ajax-atividade-coletiva.asp", getPaciente: "ajax/atendIndividual/getPaciente.asp" };
var mensagem = "";
var atividade = false;
var profissionais = false;
var detalhesAtividade = false;
var responsavel = false;
var participantes = false;
var exibirMsg = false;

jQuery(function ($) {
    jQuery("#txtHoraInicio").mask("99:99");
    jQuery("#txtHoraFim").mask("99:99");
});

jQuery(document).ready(function () {
    //Modal
    jQuery('#modalProfSus').on('shown.bs.modal', function () {
        $('#txtModalBusca').focus();
    });
    jQuery('#modalResponsavel').on('shown.bs.modal', function () {
        jQuery('#txtMBusca').focus();
    });

    //Get dos Dados - Carregamento da Tela
    getDetalhesAtividade();
    getDadosAtividadeColetiva();
    getProfissionais();
    getPacientes();
    getResponsavel();
    getDadosAtividade();

    //VALIDAÇÕES DO DICIONARIO DE DADOS DO CDS
    $("#numeroCartaoSusPaciente").on('blur', function () {
        validaCNSobg();
    });
    $("#txtDataNascimento").on('blur', function () {
        $(this).parent().find('.msgErro').html('');
        if ($(this).val().replace(/_/g, '').replace(/\//g, '').length == 8) {
            validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade');
        }
    });
    jQuery("#txtPeso").on('blur', function () {
        validacaoCampo('#txtPeso', 'Peso');
    });
    jQuery("#txtAltura").on('blur', function () {
        validacaoCampo('#txtAltura', 'Altura');
    });
    $("#fdtPraticasTemasSaude input").on('click', function () {
        validaPNCT();
    });
    function validaCNSobg() {
        if ($('#numeroCartaoSusPaciente').val() == '') {
            $('#numeroCartaoSusPaciente').parent().find('.msgErro').html('O Campo Nº Cartão Sus / CNS é obrigatório');
            return false;
        }else{
            return validacaoCampo('#numeroCartaoSusPaciente', 'CNS');
        }
    }
    function validaPNCT() {
        if ($("#fdtPraticasTemasSaude input:checked").filter(function (index) { if ($(this).val() > 24 && $(this).val() < 29) { return true } else { return false } }).length > 0) {
            $('#chkCessou_O_Habito_De_Fumar,#chkAbandonou_o_grupo').removeProp('disabled');
        } else {
            $('#chkCessou_O_Habito_De_Fumar,#chkAbandonou_o_grupo').removeProp('checked').prop('disabled');
        }
    }
    //Evento Click
    jQuery('#btnCancelar').on('click', cancelar);
    jQuery('#btnConfirmar').on('click', function () {
        var verificacao = verificarItensnaTablePaciente();
        if (verificacao) { verificacao = validaCNSobg(); };
        //if (verificacao) { verificacao = validacaoCampo('#txtDataNascimento,#txtData', 'DataNasc-dataAtividade'); };
        if (verificacao) { verificacao = validacaoCampo('#txtPeso', 'Peso'); };
        if (verificacao) { verificacao = validacaoCampo('#txtAltura', 'Altura'); };
        if (verificacao) {
            $('#frmParticipantes').submit();
        }
    });
    jQuery("#pnlDadosAtividade .panel-heading").on("click", getDadosAtividade);
    jQuery("#pnlProfissionais .panel-heading").on("click", getProfissionais);
    jQuery("#pnlDetalhesAtividade .panel-heading").on("click", getDetalhesAtividade);
    jQuery("#pnlResponsavel .panel-heading").on("click", getResponsavel);
    jQuery("#pnlParticipantes .panel-heading").on("click", getPacientes);
    jQuery("#btnFinalizarAtividadeColetiva").on("click", finalizarAtividadeColetiva);
    jQuery('#btnBuscarProf').on('click', busca_Prof_Sus);
    jQuery('#btnNovoCartaoSus').on('click', addSemCadastroAtividade);

    jQuery("input[name='PraticasPNCTSessao1'], "
         + "input[name='PraticasPNCTSessao2'], "
         + "input[name='PraticasPNCTSessao3'], "
         + "input[name='PraticasPNCTSessao4']").on("change", habilitaPNCTUsuario);
    //$('#selCnesUniResponsavel').on('change', CarregarEquipeIneDoResponsavel);

    jQuery("input[name='AcaoEstruturanteAtividade'], input[name='AcaoSaudeAtividade']").on("change", function(event) {
        habilitaDesabilitaAtividade(event);
        habilitarTemasEPublico();
    });

    //Validate
    jQuery("#frmDadosAtividade").validate({
        errorElement: "div",
        errorPlacement: function (error, element) {
            if (jQuery(element).parent("div").find("div.msgErro").length > 0) {
                error.appendTo(jQuery(element).parent("div").find("div.msgErro"));
            }
            else {
                error.appendTo(jQuery(element).parents("div.form-group").find("div.msgErro"));
            }
        },
        rules: {
            DataDeAtividade: {
                required: true
            },
            NumeroInepEscolaCreche: {
                minlength: 8,
                maxlength: 8
            },
            ProgramacaoDeNumeroDeParticipantes: {
                min: 0,
                max:999
            },
            LocalDeAtividades: {
                maxlength: 250
            }
        },
        messages: {
            DataDeAtividade: "Data é obrigatória",
            NumeroInepEscolaCreche: "O campo Nº INEP (escola / creche) deve conter 8 dígitos.",
            ProgramacaoDeNumeroDeParticipantes: "Por favor, indique um número maior que 0 e menor que 999.",
            LocalDeAtividades: "O campo Local de Atividades tem no máx. 250 dígitos."
        },
        submitHandler: function (form) {
            var msgerro='';
            // form.submit();
            atividade = true;
            $('#frmDadosAtividade').find('.msgErro').html('');
            //debugger;
            var Data_de_Atividade = $('#txtData').val().split('/');
            Data_de_Atividade = Data_de_Atividade[2] + Data_de_Atividade[1] + Data_de_Atividade[0];
            Data_de_hoje = dt_ano + dt_mes + dt_dia;
            if (parseInt(Data_de_Atividade) > parseInt(Data_de_hoje)) {
                msgerro += "A Data de Atividade não pode ser maior do que hoje";
                $('#txtData').parent().find('.msgErro').append(msgerro);
                atividade = false;
            }
            if ($('#txtHoraInicio').val() != '') {
                msgerro = '';
                if ($('#txtHoraInicio').val().substring(0, 2) > 23 || $('#txtHoraInicio').val().substring(0, 2) < 0) {
                    msgerro += "Hora entre 0 e 23<br>";
                }
                if ($('#txtHoraInicio').val().substring(3, 5) > 60 || $('#txtHoraInicio').val().substring(3, 5) < 0) {
                    msgerro += "Minuto entre 0 e 60<br>";
                }
                if (msgerro!='') {
                    $('#txtHoraInicio').parent().find('.msgErro').append(msgerro);
                    atividade = false;
                }
            }
            if ($('#txtHoraFim').val() != '') {
                msgerro = '';
                if ($('#txtHoraFim').val().substring(0, 2) > 23 || $('#txtHoraFim').val().substring(0, 2) < 0) {
                    msgerro += "Hora entre 0 e 23<br>";
                }
                if ($('#txtHoraFim').val().substring(3, 5) > 60 || $('#txtHoraFim').val().substring(3, 5) < 0) {
                    msgerro += "Minuto entre 0 e 60<br>";
                }
                if (msgerro != '') {
                    $('#txtHoraFim').parent().find('.msgErro').append(msgerro);
                    atividade = false;
                }
            }
            if ($('#txtHoraInicio').val() != '' && $('#txtHoraFim').val() != '') {
                msgerro = '';
                if ($('#txtHoraInicio').val().replace(':', '') > $('#txtHoraFim').val().replace(':', '')) {
                    msgerro += "O campo Hora fim, não pode ser menor que o campo Hora Inicio";
                    $('#txtHoraInicio').parent().find('.msgErro').append(msgerro);
                    $('#txtHoraFim').parent().find('.msgErro').append(msgerro);
                }
            }
            if (atividade) {
                salvarDadosAtividade();
            }
        }
    });
    jQuery("#frmProfissionais").validate({
        errorElement: "div",
        errorPlacement: function (error, element) {
            if (jQuery(element).parent("div").find("div.msgErro").length > 0) {
                error.appendTo(jQuery(element).parent("div").find("div.msgErro"));
            }
            else {
                error.appendTo(jQuery(element).parents("div.form-group").find("div.msgErro"));
            }
        },
        rules: {
            
        },
        messages: {
            
        },
        submitHandler: function (form) {
            if (verificarItensnaTableProfissionais())
            {
                if (setProfissional(1, dadosProfissionais())) {
                    jQuery("#checkDadosProfissionais").show();
                    jQuery('#dadosProfissionais').hide();
                }

                profissionais = true;
            }
        }
    });
    jQuery("#frmDetalhesAtividade").validate({
        errorElement: "div",
        errorPlacement: function (error, element) {
            if (jQuery(element).parent("div").find("div.msgErro").length > 0) {
                error.appendTo(jQuery(element).parent("div").find("div.msgErro"));
            }
            else {
                error.appendTo(jQuery(element).parents("div.form-group").find("div.msgErro"));
            }
        },
        rules: {
            AcaoEstruturanteAtividade: "acoesDeSaude",
            TemasReuniaoQuestoesAdministrativasFuncionamento: "temasReuniao",
            PublicoAlvoComunidadeEmGeral: "publicoAlvo",
            PraticasAlimentacaoSaudavel: "praticasTemasSaude"
        },
        submitHandler: function (form) {
            salvarDetalhesDaAtividade();
            detalhesAtividade = true;
        }
    });
    jQuery("#frmFinalizar").validate({
        submitHandler: function () {
            mensagem = "";

            jQuery.ajax({
                type: "POST",
                async: false,
                data: {
                    acao: 'getPesoAlturaPreenchidoParticipantes',
                    idAtividadeColetiva: jQuery("#idAtividadeColetiva").val()
                },
                dataType: "json",
                url: Api.URL
            })
            .done(function (data) {
                if (data.status) {
                    if (jQuery("#chkAntropometria:checked").length > 0) {
                        if (JSLINQ(data.resultado).Any(function (item) { return item.Preenchido == false; })) {
                            //mensagem = 'Ao avaliar Antropometria é necessário informar o peso e a altura de todos os participantes';
                        }
                    }
                    else {
                        if (JSLINQ(data.resultado).Any(function (item) { return item.Preenchido == true; })) {
                            //mensagem = 'Há participantes com o peso e a altura informados, por favor selecione a opção "20 - Antropometria". Caso a atividade não avaliou a antropometria, por favor não informar o peso e a altura dos participantes';
                        }
                    }
                }
            });

            if (mensagem != "") {
                jQuery("#checkDetalhesAtividade").hide();
                jQuery('#detalhesAtividade').show();
                jQuery("#chkAntropometria").parents("div.form-group").find("span.msgErro").text("");
                jQuery("#chkAntropometria").parents("div.form-group").find("span.msgErro").text(mensagem);
            }
            else {
                jQuery("#chkAntropometria").parents("div.form-group").find("span.msgErro").text("");
                alteraAtividadeColetiva(2, dadosFinalizar());
                modalAlerta("Atividade Coletiva", "Ficha finalizada com sucesso.");
            }
        }
    });
    $('#frmResponsavel').submit(function (event) {
        if ($('#RespCNS option:selected').length == 0 || $('#RespCNS option:selected').val().length < 1) {
            modalAlerta("Atividade Coletiva", "Selecione um responsável");
            return false;
        }
        if ($('#selCnesUniResponsavel option:selected').length == 0 || $('#selCnesUniResponsavel option:selected').val().length < 1) {
            modalAlerta("Atividade Coletiva", "Selecione um CNES para o responsável");
            return false;
        }
        if ($('#selIneEquipeResponsavel option').length > 0) {
            if ($('#selIneEquipeResponsavel option').length==1 && $('#selIneEquipeResponsavel option:first').val().length > 0) {
                if ($('#selIneEquipeResponsavel option:selected').length == 0 || $('#selIneEquipeResponsavel option:selected').val().length < 1) {
                    modalAlerta("Atividade Coletiva", "Selecione um INE para o responsável");
                    return false;
                }
            }
        }
        if (parseInt($('#txtNumeroParticipantes').val()) < parseInt($('#txtNumeroAvaliacoesAlteradas').val())) {
            modalAlerta("Atividade Coletiva", "Não pode haver mais Avaliações alteradas do que Participantes");
            return false;
        }
        setResponsavel(1, dadosResponsavel());
        event.preventDefault();
    });
    /*jQuery("#frmResponsavel").validate({
        errorElement: "div",
        errorPlacement: function (error, element) {
            if (jQuery(element).parent("div").find("div.msgErro").length > 0) {
                error.appendTo(jQuery(element).parent("div").find("div.msgErro"));
            }
            else {
                error.appendTo(jQuery(element).parents("div.form-group").find("div.msgErro"));
            }
        },
        rules: {

            CodigoCnesUnidadeResponsavel: "CodCnesUnidadeSelecionado",
            CodigoEquipeIneResponsavel: {
                required: true
            },
            NumeroDeParticipantes: {
                min: 0,
                maxlength: 3
            },
            NumeroDeAvaliacoesAlteradas: {
                maxlength: 3
            }
        },
        messages: {
            CodigoEquipeIneResponsavel: "Campo Cód. equipe (INE) é obrigatório.",
            NumeroDeParticipantes: "O campo Nº de Participantes não pode ser menor que zero e deve ter no máx; 3 dígitos numéricos.",
            NumeroDeAvaliacoesAlteradas: "O campo Nº de avaliações alteradas tem no máx. 3 dígitos numéricos."
        },
        submitHandler: function (form) {
            if (verificarItensnaTableResponsavel()) {
                setResponsavel(1, dadosResponsavel());
            }
        }
    });*/
    /*jQuery("#frmParticipantes").validate({
        errorElement: "div",
        errorPlacement: function (error, element) {
            if (jQuery(element).parent("div").find("div.msgErro").length > 0) {
                error.appendTo(jQuery(element).parent("div").find("div.msgErro"));
            }
            else {
                error.appendTo(jQuery(element).parents("div.form-group").find("div.msgErro"));
            }
        },
        rules: {
            DataNascimento: {
                required: true
            },
            Peso: "avaliaAntropometriaPeso",
            Altura: "avaliaAntropometriaAltura",
            NumeroCartaoSus: {
                minlength: 15,
                maxlength: 15
            }
        },
        messages: {
            DataNascimento: "Campo Data de Nascimento é obrigatório",
            NumeroCartaoSus: "Nº Cartão SUS incorreto."
        },
        submitHandler: function (form) {
            jQuery("#notificacaoPacientes").text("");

            if (verificarItensnaTablePaciente()) {
                alteraPacienteAtividadeColetiva(dadosParticipantes());

                if (jQuery("#checkDadosParticipantes").prop("display") != "none") {
                    getPacientes();
                }

                if (!validaTableSeContemErros("#listaAtividadeColetiva tbody .preencherDadosPaciente",
                                               "#checkDadosParticipantes",
                                               '#dadosParticipantes',
                                                "#frmParticipantes",
                                                false,
                                                function () {
                                                    return jQuery("#txtDataNascimento").val() != "" && jQuery("#txtDataNascimento").val() != "01/01/1900";
                                                },
                                                function () {
                                                    jQuery("#notificacaoPacientes").text("Favor preencha uma Data de Nascimento valida.");
                                                })
                 ) {
                    jQuery("#checkDadosParticipantes").show();
                    jQuery('#dadosParticipantes').hide();
                    participantes = true;
                    modalAlerta("Confirmação", "Atividade Coletiva gravada com sucesso!");
                }
            }
        }
    });*/
    $("#frmParticipantes").submit(function (event) {
            jQuery("#notificacaoPacientes").text("");

            if (verificarItensnaTablePaciente()) {
                alteraPacienteAtividadeColetiva(dadosParticipantes());

                if (jQuery("#checkDadosParticipantes").prop("display") != "none") {
                    getPacientes();
                }
            }
            event.preventDefault();
        }
    );
    //Validator
    jQuery.validator.addMethod("acoesDeSaude", function (value) {
        return jQuery("input[name='AcaoEstruturanteAtividade']:checked, input[name='AcaoSaudeAtividade']:checked").length > 0;
    }, "É obrigatório selecionar uma Atividade");
    jQuery.validator.addMethod("temasReuniao", function (value) {
        if (jQuery("#ReuniaoDeEquipe:checked,"
            + "#ReuniaoComOutrasEquipesDeSaude:checked,"
            + "#ReuniaoIntersetorialConselhoLocalDeSaudeControleSocial:checked").length > 0) {
            return jQuery("#fdtTemasReuniao input:checked").length > 0;
        }
        else {
            return true;
        }
    }, 'Por favor selecione ao menos uma opção do bloco "Temas para Reunião"');
    jQuery.validator.addMethod("publicoAlvo", function (value) {
        if (jQuery("#EducacaoEmSaude:checked,"
            + "#AtendimentoEmGrupo:checked,"
            + "#AvaliacaoProcedimentoColetivo:checked, "
            + "#MobilizacaoSocial:checked").length > 0) {
            return jQuery("#fdtPublicoAlvo input:checked").length > 0;
        }
        else {
            return true;
        }
    }, 'Por favor selecione ao menos uma opção do bloco "Público Alvo"');
    jQuery.validator.addMethod("praticasTemasSaude", function (value) {
        var retorno = true;

        if (jQuery("#EducacaoEmSaude:checked,"
            + "#AtendimentoEmGrupo:checked,"
            + "#AvaliacaoProcedimentoColetivo:checked, "
            + "#MobilizacaoSocial:checked").length > 0) {
            if (jQuery("#fdtPraticasTemasSaude input:checked").length <= 0) {
                retorno = false;
            }
        }

        return retorno;
    }, 'Por favor selecione ao menos uma opção do bloco "Práticas / Temas para Saúde"');
    jQuery.validator.addMethod("avaliaAntropometriaPeso", function (value) {
        var retorno = false;

        if (jQuery("#chkAntropometria:checked").length > 0) {
            if ((jQuery("#txtPeso").val() == "") || (jQuery("#txtPeso").val() == "0")) {
                retorno = false;
            }
            else {
                retorno = true;
            }
        }
        else {
            retorno = true;
        }

        return retorno;
    }, 'É necessário informar o peso do participante');
    jQuery.validator.addMethod("avaliaAntropometriaAltura", function (value) {
        var retorno = false;
        if (jQuery("#chkAntropometria:checked").length > 0) {
            if ((jQuery("#txtAltura").val() == "") || (jQuery("#txtAltura").val() == "0")) {
                retorno = false;
            }
            else {
                retorno = true;
            }
        }
        else {
            retorno = true;
        }

        return retorno;
    }, 'É necessário informar a altura do participante');
    jQuery.validator.addMethod("CodCnesUnidadeSelecionado", function (value) {
        if (value > 0) {
            return true;
        }
        else {
            return false;
        }
    }, 'Campo Cód. CNES Unidade é obrigatório');
});

function finalizarAtividadeColetiva(event) {
    event.preventDefault();
    jQuery("#frmDadosAtividade").submit();
    jQuery("#frmProfissionais").submit();
    jQuery("#frmDetalhesAtividade").submit();
    jQuery("#frmResponsavel").submit();
    if (verificarItensnaTablePaciente()) {
        /*validaTableSeContemErros("#listaAtividadeColetiva tbody .preencherDadosPaciente",
                                 "#checkDadosParticipantes", 
                                 '#dadosParticipantes', 
                                 "#frmParticipantes", 
                                 false,
                                 function () {
                                     return jQuery("#txtDataNascimento").val() != "" && jQuery("#txtDataNascimento").val() != "01/01/1900";
                                 },
                                 function () {
                                     jQuery("#notificacaoPacientes").text("Favor preencha uma Data de Nascimento valida.");
                                 });*/
        
        if (atividade && profissionais && detalhesAtividade && responsavel && participantes) {
           
            $.ajax({
                type: "POST",
                data: { acao: 'GetValidacao', id: jQuery("#idAtividadeColetiva").val() },
                datatype: "json",
                url: Api.URL
            })
            .done(function (data) {
                data = JSON.parse(data)
                if (data.status) {
                    //jQuery("#frmFinalizar").submit();
                    modalAlerta("Confirmação", "Atendimento finalizado com sucesso!")
                    setInterval(function () { jQuery("#frmFinalizar").submit() }, 3000);
                }
                else {
                    modalAlerta("Erro", data.resultado);
                }
            });
        }
    }
}

function habilitaDesabilitaAtividade(event) {
    if (jQuery(event.currentTarget).prop("name") == "AcaoEstruturanteAtividade") {
        jQuery("input[name='AcaoSaudeAtividade']").prop("checked", false);
    }
    else {
        jQuery("input[name='AcaoEstruturanteAtividade']").prop("checked", false);
    }
}

function habilitarTemasEPublico() {
    if (jQuery("input[name='AcaoEstruturanteAtividade']:checked").length > 0) {
        jQuery("#fdtTemasReuniao input").prop("disabled", "");
        jQuery("#fdtPublicoAlvo input").prop("checked", false).prop("disabled", "disabled");
        jQuery("#fdtPraticasTemasSaude input").prop("checked", false).prop("disabled", "disabled");
        jQuery("#asteriscoTemasParaReuniao").show();
    }
    else if (jQuery("input[name='AcaoSaudeAtividade']:checked").length > 0) {
        jQuery("#fdtTemasReuniao input").prop("checked", false).prop("disabled", "disabled");
        jQuery("#fdtPublicoAlvo input").prop("disabled", "");
        jQuery("#fdtPraticasTemasSaude input").prop("disabled", "");
        jQuery("#asteriscoTemasParaReuniao").hide();
    }
    else {
        jQuery("#fdtTemasReuniao input").prop("disabled", "");
        jQuery("#fdtPublicoAlvo input").prop("disabled", "");
        jQuery("#fdtPraticasTemasSaude input").prop("disabled", "");
        jQuery("#asteriscoTemasParaReuniao").hide();
    }
}

function getDetalhesAtividade() {
    jQuery.when(
        //getAcaoEstruturanteAtividade
        processaDadosForm({
            acao: 'getAcaoEstruturanteAtividade',
            idAtividadeColetiva: jQuery("#idAtividadeColetiva").val()
        }),
        //getAcaoSaudeAtividade
        processaDadosForm({
            acao: 'getAcaoSaudeAtividade',
            idAtividadeColetiva: jQuery("#idAtividadeColetiva").val()
        }),
        //getAcaoEstruturanteTemasReuniao
        processaDadosForm({
            acao: 'getAcaoEstruturanteTemasReuniao',
            idAtividadeColetiva: jQuery("#idAtividadeColetiva").val()
        }),
        //getAcaoSaudePublicoAlvo
        processaDadosForm({
            acao: 'getAcaoSaudePublicoAlvo',
            idAtividadeColetiva: jQuery("#idAtividadeColetiva").val()
        }),
        //getPraticasTemasParaSaude
        processaDadosForm({
            acao: 'getPraticasTemasParaSaude',
            idAtividadeColetiva: jQuery("#idAtividadeColetiva").val()
        })
    ).then(onSuccess, onError);

    function onSuccess(){
        jQuery("#checkDetalhesAtividade").hide();
        jQuery('#detalhesAtividade').show();
        habilitarTemasEPublico();
    }

    function onError(){
        //Not  Implemented
    }
}

function salvarDetalhesDaAtividade() {
    var dadosForm = dadosDetalhesAtividade();

    //setAcaoEstruturanteAtividade
    jQuery.ajax({
        type: "POST",
        data: {
            acao: 'setAcaoEstruturanteAtividade',
            detalhesAtividade: dadosForm
        },
        datatype: "json",
        url: Api.URL
    })

    //setAcaoSaudeAtividade
    jQuery.ajax({
        type: "POST",
        data: {
            acao: 'setAcaoSaudeAtividade',
            detalhesAtividade: dadosForm
        },
        datatype: "json",
        url: Api.URL
    })

    //setAcaoEstruturanteTemasReuniao
    jQuery.ajax({
        type: "POST",
        data: {
            acao: 'setAcaoEstruturanteTemasReuniao',
            detalhesAtividade: dadosForm
        },
        datatype: "json",
        url: Api.URL
    })

    //setAcaoSaudePublicoAlvo
    jQuery.ajax({
        type: "POST",
        data: {
            acao: 'setAcaoSaudePublicoAlvo',
            detalhesAtividade: dadosForm
        },
        datatype: "json",
        url: Api.URL
    })

    //setPraticasTemasParaSaude
    jQuery.ajax({
        type: "POST",
        data: {
            acao: 'setPraticasTemasParaSaude',
            detalhesAtividade: dadosForm
        },
        datatype: "json",
        url: Api.URL
    })

    jQuery("#checkDetalhesAtividade").show();
    jQuery('#detalhesAtividade').hide();
}

function getDadosAtividade() {
    $.ajax({
        type: "POST",
        data: {
            acao: 'getDadosAtividade',
            idAtividadeColetiva: jQuery("#idAtividadeColetiva").val()
        },
        datatype: "json",
        url: Api.URL
    }).done(
        function (data) {
            var obj = JSON.parse(data);
            //if (obj.resultado[0].DataDeAtividade != null) {
            //    obj.resultado[0].DataDeAtividade = formatarDataParaExibicao(obj.resultado[0].DataDeAtividade);
            //}
            setFormData(obj.resultado[0]);
            jQuery("#checkDadosAtividade").hide();
            jQuery('#dadosAtividade').show();
        }
    )
}

function dadosAtividade() {
    var dados = getFormData("#frmDadosAtividade");

    if (dados.LocalDeAtividades != 0) {
        dados.LocalDeAtividades = dados.LocalDeAtividades;
    }

    if (dados.DataDeAtividade != 0) {
        dados.DataDeAtividade = dados.DataDeAtividade;
    }

    return JSON.stringify(dados);
}

function dadosProfissionais() {
    var dados = getFormData("#frmProfissionais");
    return JSON.stringify(dados);
}

function dadosResponsavel() {
    var dados = getFormData("#frmResponsavel");
    return JSON.stringify(dados);
}

function dadosFinalizar() {
    var dados = getFormData("#frmFinalizar");
    return JSON.stringify(dados);
}

function salvarDadosAtividade() {
    
    $.ajax({
        type: "POST",
        data: {
            acao: 'salvarDadosAtividade',
            dadosAtividade: dadosAtividade()
        },
        datatype: "json",
        url: Api.URL
    }).done(
        function () {
            jQuery("#checkDadosAtividade").show();
            jQuery('#dadosAtividade').hide();
        }
    )
}

function cancelar() {
    jQuery("#notificacaoPacientes").text("");
    limparFormAtividadeColetivaUsuario();
    jQuery('.IdentificacaoUsuario').hide();
}

function buscaResponsavelSus() {
    jQuery("#myModalLabel").empty();
    jQuery("#myModalLabel").append("Busca de Responsável");
    jQuery("#selModalTpBusca").val('1');
    jQuery("#txtModalBusca").val('');
    jQuery("label[for=txtModalBusca]").html('Nº SUS <span class="asterisco">*</span>');
    jQuery("#txtModalBusca").attr('placeholder', 'Número');
    jQuery('#modalProfSus').find('input[type=text], select').each(function () {
        $(this).parents(".form-group").removeClass("has-error");
    });
    jQuery('#listamodalResponsavel tbody').empty();
    jQuery('#listamodalResponsavel tbody').append('<tr><th colspan="2" class="text-center">Efetue a busca acima</th></tr>');
    jQuery('#modalResponsavel').modal('show');
}

function busca_Paciente_Sus() {
    jQuery('#txtmodalBuscaCartaoSus').val('');
    jQuery('#txtmodalConfirmaCartaoSus').val('');
    $('.msgErroDuplicado').text('');
    $(".modal-title").html("Busca de Participantes");
    $("#selModalTpBusca").val('1');
    $("#txtModalBusca").val('');
    $("label[for=txtModalBusca]").html('Nº SUS <span class="asterisco">*</span>');
    $("#txtModalBusca").attr('placeholder', 'Número');
    $('#modalProfSus').find('input[type=text], select').each(function () {
        $(this).parents(".form-group").removeClass("has-error");
    });
    $('#listamodalProfSus tbody').empty();
    $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center">Efetue a busca acima</th></tr>');
    $('#modalProfSus').modal('show').find('#selModalTpBusca').val('2');tpBuscaModalSus();
}

function busca_Prof_Sus(event) {
    if (event.currentTarget.id == "btnBuscarProf") {
        $('#modalProfSus').addClass('add');
    } else {
        $('#modalProfSus').removeClass('add');
    }
    $('.msgErroDuplicado').text('');
    $(".modal-title").html("Busca de Profissional");

    $("#selModalTpBusca").val('1');
    $("#txtModalBusca").val('');
    $("label[for=txtModalBusca]").html('Nº SUS <span class="asterisco">*</span>');
    $("#txtModalBusca").attr('placeholder', 'Número');

    $('#modalProfSus').find('input[type=text], select').each(function () {
        $(this).parents(".form-group").removeClass("has-error");
    });

    $('#listamodalProfSus tbody').empty();
    $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center">Efetue a busca acima</th></tr>');
    $('#modalProfSus').modal('show').find('#selModalTpBusca').val('2');tpBuscaModalSus();
}

function tpBuscaModalSus() {

    if ($("#selModalTpBusca").val() == 1) {
        $("#txtModalBusca").val('');
        $("label[for=txtModalBusca]").html('Nº SUS <span class="asterisco">*</span>');
        $("#txtModalBusca").attr('placeholder', 'Número');
    } else {
        $("#txtModalBusca").val('');
        $("label[for=txtModalBusca]").html('Nome <span class="asterisco">*</span>');
        $("#txtModalBusca").attr('placeholder', 'Nome');
    }

}

function tpBuscaModalResponsavel() {

    if ($("#selModalTipoBusca").val() == 1) {
        $("#txtMBusca").val('');
        $("label[for=txtModalBusca]").html('Nº SUS <span class="asterisco">*</span>');
        $("#txtMBusca").attr('placeholder', 'Número');
    } else {
        $("#txtMBusca").val('');
        $("label[for=txtModalBusca]").html('Nome <span class="asterisco">*</span>');
        $("#txtMBusca").attr('placeholder', 'Nome');
    }

}

function paciente_Sus() {
    if (validaCampos($("#modalProfSus"), 2, "alertaSemModalProfSus")) {
        var xTbBusca = $('#selModalTpBusca').val();
        var xBusca = removerAcentos($('#txtModalBusca').val().trim());
        $('#listamodalProfSus tbody').empty();
        $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');
        $.ajax({
            type: "POST",
            data: { tpBusca: xTbBusca, busca: xBusca },
            datatype: "json",
            url: "ajax/atendIndividual/getPaciente.asp"
        })
        .done(function (data) {
            $('#listamodalProfSus tbody').empty();
            if (data.status) {
                $.each(data.resultado, function (ResultadoItens, item) {
                    var strTable = '<tr class="sel-cns" data-nome="' + item.nome.toCapitalize() + '" data-cns="' + item.cns + '" data-idusr="' + item.IDUSR + '" >';
                    strTable += '<td class="text-capitalize">' + item.nome.toLowerCase() + '</td>';
                    strTable += '<td class="text-center">' + item.cns + '</td>';
                    strTable += '</tr>';
                    $('#listamodalProfSus tbody').append(strTable);
                });
                executaSelect();
            } else {
                if (data.sessao) {
                    setTimeout(function () {
                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                        setTimeout(function () {
                            window.location.assign("login.asp");
                        }, 2000);
                    }, 800);
                }
                $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center">Nenhum resultado encontrado!</th></tr>');
                $('#modalProfSus').modal('hide');
                $('#txtmodalBuscaCartaoSus').val(xBusca);
                $('#modalSemCartaoSus').modal('show');
                $('#modalSemCartaoSus .msgErro .col-md-12').text('');
            }
        });
    }

    function executaSelect() {
        $('#listamodalProfSus tbody .sel-cns').on('click',function (event) {
            $(this).siblings().removeClass('list-group-item-success');
            var idAtividadeColetiva = $("#idAtividadeColetiva").val();
            if (jQuery("#" + $(this).data("idusr")).val() != "" || $(this).data("idusr") == "") {
                if ($('#listaAtividadeColetiva tbody tr').length < 33) {
                    var idPacienteAtividadeColetiva = criaPacienteAtividadeColetiva(idAtividadeColetiva, $(this).data("idusr"));
                    if (idPacienteAtividadeColetiva != false) {
                        getPacientes();
                        cancelar();
                    } else {
                        $('.msgErroDuplicado').text("Falha ao adicionar o Usuário na Atividade Coletiva, tente novamente mais tarde.");
                    }
                } else {
                    $('.msgErroDuplicado').text("Falha ao adicionar o Usuário na Atividade Coletiva, O limite de 33 usuários ja foi atingido");
                }
            } else {
                $('.msgErroDuplicado').text('O Participante selecionado já foi inserido na Atividade Coletiva e não poderá ser inserido novamente.');
            }
        });
    }
    return false;
}

function responsavelSus() {
    if (validaCampos($("#modalResponsavel"), 2, "alertaSemModalResponsavel")) {
        var xTbBusca = $('#selModalTipoBusca').val();
        var xBusca = removerAcentos($('#txtMBusca').val().trim());
        $('#listamodalResponsavel tbody').empty();
        $('#listamodalResponsavel tbody').append('<tr><th colspan="2" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');
        $.ajax({
            type: "POST",
            data: { tpBusca: xTbBusca, busca: xBusca },
            datatype: "json",
            url: "ajax/atendIndividual/getProfissional.asp"
        })
        .done(function (data) {
            $('#listamodalResponsavel tbody').empty();
            if (data.status) {
                $.each(data.resultado, function (ResultadoItens, item) {
                    var strTable = '<tr class="sel-cns" data-nome="' + item.nome.toCapitalize() + '" data-cns="' + item.cns + '" data-idusr="' + item.IdProf + '" >';
                    strTable += '<td class="text-capitalize">' + item.nome.toLowerCase() + '</td>';
                    strTable += '<td class="text-center">' + item.cns + '</td>';
                    strTable += '</tr>';
                    $('#listamodalResponsavel tbody').append(strTable);
                });
                executaSelect();
            } else {
                if (data.sessao) {
                    setTimeout(function () {
                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                        setTimeout(function () {
                            window.location.assign("login.asp");
                        }, 2000);
                    }, 800);
                }
                $('#listamodalResponsavel tbody').append('<tr><th colspan="2" class="text-center">Nenhum resultado encontrado!</th></tr>');
            }
        });
    }

    function executaSelect() {
        $('#listamodalResponsavel tbody .sel-cns').click(function () {
            $(this).siblings().removeClass('list-group-item-success');
            makeTableResponsavel($(this).data('nome'), $(this).data('cns'), $(this).data('idusr'), null, 1)
        });
    }
    return false;
}

function chaveamentoModal(evt) {
    if ($(".modal-title").html() == "Busca de Profissional") {
        return prof_Sus();
    }
    else if ($(".modal-title").html() == "Busca de Responsável") {
        return prof_Sus();
    } else {
        return paciente_Sus();
    }
}

function prof_Sus() {
    if (validaCampos($("#modalProfSus"), 2, "alertaSemModalProfSus")) {
        var xTbBusca = $('#selModalTpBusca').val();
        var xBusca = removerAcentos($('#txtModalBusca').val().trim());
        $('#listamodalProfSus tbody').empty();
        $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center"><img src="img/ajax-loader-2.gif" /></th></tr>');
        $.ajax({
            type: "POST",
            data: { tpBusca: xTbBusca, busca: xBusca },
            datatype: "json",
            url: "ajax/atendIndividual/getProfissional.asp"
        })
        .done(function (data) {
            $('#listamodalProfSus tbody').empty();
            if (data.status) {
                $.each(data.resultado, function (ResultadoItens, item) {
                    var strTable = '<tr class="sel-cns" data-nome="' + item.nome.toCapitalize() + '" data-cns="' + item.cns + '" data-idprof="' + item.IdProf.trim() + '" data-cbo="' + item.CBO + '" >';
                    strTable += '<td class="text-capitalize">' + item.nome.toLowerCase() + '</td>';
                    strTable += '<td class="text-center">' + item.cns + '</td>';
                    strTable += '</tr>';
                    $('#listamodalProfSus tbody').append(strTable);
                });
                executaSelect();
            } else {
                if (data.sessao) {
                    setTimeout(function () {
                        modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                        setTimeout(function () {
                            window.location.assign("login.asp");
                        }, 2000);
                    }, 800);
                }
                $('#listamodalProfSus tbody').append('<tr><th colspan="2" class="text-center">Nenhum resultado encontrado!</th></tr>');
            }
        });
    }

    function executaSelect() {
        $("#listamodalProfSus tbody .sel-cns").on('click',function (event) {
            if ($(this).data('cns') == '') {
                $(this).parents('.modal').modal('hide');
                modalAlerta('Atenção', 'Este Profissional não pode ser selecionado, pois não tem CNS cadastrado');
            } else if ($(this).data('cbo') == '') {
                $(this).parents('.modal').modal('hide');
                modalAlerta('Atenção', 'Este Profissional não pode ser selecionado, pois não tem CBO cadastrado');
            } else {
                //Remove o active dos outros
                $(this).siblings().removeClass("list-group-item-success");
                var idAtividadeColetiva = jQuery("#idAtividadeColetiva").val();
                var linhas = $("#listaProfissionaisAtividadeColetiva tbody tr.sel-cns").length;
                var add = $('#modalProfSus').hasClass('add');
                if (add) {
                    if (jQuery("#" + $(this).data("idprof")).val() != "" || $(this).data("idprof") == "") {
                        var idProfissionalAtividadeColetiva = salvaProfissionalAtividadeColetiva(idAtividadeColetiva, $(this).data("idprof"), $(this).data("cbo"));
                        if (idProfissionalAtividadeColetiva != false) {
                            //makeTableProfissional($(this).data("nome"), $(this).data("cns"), $(this).data("idprof"), idProfissionalAtividadeColetiva, $(this).data("cbo"));
                            $("#listaProfissionaisAtividadeColetiva tbody tr button.btn-danger").show();
                            $("#listaProfissionaisAtividadeColetiva tbody tr button.btn-success").hide();
                            var unico = (linhas == 0);
                            makeTableProfissional($(this).data("nome"), $(this).data("cns"), $(this).data("idprof"), idProfissionalAtividadeColetiva, $(this).data("cbo"), unico);

                            AtualizaResps(0);
                        } else {
                            modalAlerta("Atenção", "Falha ao adicionar o Profissional na Atividade Coletiva, tente novamente mais tarde.");
                        }
                    } else {
                        $('.msgErroDuplicado').text('O Profissional selecionado já foi inserido na Atividade Coletiva e não poderá ser inserido novamente.');
                    }
                } else {
                    alteraInfoProfissional(event);
                }
            }
        });
    }
    return false;
}

function AtualizaResps(n) {
    var cnss = '';
    $('#listaProfissionaisAtividadeColetiva tr:not(:first)').each(function () {
        cnss = cnss + ',\'' + $(this).data('cns') + '\'';
    });
    var dados = new Object;
    switch (n) {
        case 2:
            dados['cnss'] = '\'' + $('#RespCNS option:selected').val() + '\'';
            dados['cnes'] = '\'' + $('#selCnesUniResponsavel option:selected').val() + '\'';
            break;
        case 1:
            dados['cnss'] = '\'' + $('#RespCNS option:selected').val() + '\'';
            break;
        default:
            dados['cnss'] = cnss.substring(1);
            break;
    }
    dados['tipo'] = n;
    dados['ID'] = '\'' + $('#frmDadosAtividade input[name=Id]').val() + '\'';
    $.ajax({
        type: "POST",
        data: dados,
        datatype: "json",
        url: "ajax/atividadeColetiva/getResponsavel.asp"//"ajax/atendIndividual/getResponsavel.asp"
    })
    .done(function (data) {
        switch (n) {
            case 2:
                $('#selIneEquipeResponsavel option').remove();
                break;
            case 1:
                $('#selCnesUniResponsavel option,#selIneEquipeResponsavel option').remove();
                break;
            default:
                $('#RespCNS option,#selCnesUniResponsavel option,#selIneEquipeResponsavel option').remove();
                break;
        }

        var strOptions = '';
        var strSelected = '';
        if (data.status) {
            $.each(data.resultado, function (ResultadoItens, item) {
                strSelected = '';
                switch (n) {
                    case 2:
                        if (item.INE != 0) {
                            if (item.CODINE == item.VALOR) { strSelected = ' selected="selected" '; }
                            strOptions += '<option value="' + item.CODINE + '"' + strSelected + '>' + item.INE + '</option>';
                        }
                        break;
                    case 1:
                        if (item.CNES == item.VALOR) { strSelected = ' selected="selected" '; }
                        strOptions += '<option value="' + item.CNES + '"' + strSelected + '>' + item.CNES + '</option>';
                        break;
                    default:
                        if (item.CNS == item.VALOR) { strSelected = ' selected="selected" '; }
                        strOptions += '<option value="' + item.CNS + '"' + strSelected + '>' + item.nome + '</option>';
                        break;
                }
            });
        } else {
            strOptions = '<option value="">Não há dados</option>';
        }
        switch (n) {
            case 2: $('#selIneEquipeResponsavel').append(strOptions); break;
            case 1: $('#selCnesUniResponsavel').append(strOptions); break;
            default: $('#RespCNS').append(strOptions); break;
        }
        switch (n) {
            case 2: break;
            case 1: AtualizaResps(2); break;
            default: AtualizaResps(1); break;
        }
    });
}

function alteraInfoProfissional(event) {
    var rowProfissional = $("#listaProfissionaisAtividadeColetiva tbody tr.sel-cns");
    var idProfAtividadeColetiva = rowProfissional.data("idprofatividadecoletiva");
    $("#frmProfAtvId").val($(event.currentTarget).data("idprof"));
    $("#cbo").val($(event.currentTarget).data("cbo"));
    if (substituirProfissional(idProfAtividadeColetiva, $(event.currentTarget).data("idprof"), $(event.currentTarget).data("cbo"))) {
    makeTableProfissional(
        $(event.currentTarget).data("nome"),
        $(event.currentTarget).data("cns"),
        $(event.currentTarget).data("idprof"),
        idProfAtividadeColetiva,
        $(event.currentTarget).data("cbo"),
        true
    );
    rowProfissional.remove();
    } else {
        $('.msgErroDuplicado').text('Ocorreu um erro ao substituir o profissional.');
    }
}

function salvaProfissionalAtividadeColetiva(idAtividadeColetiva, idProfissional, cbo) {
    var blnRet = false;
    //alteraAtividadeColetiva(1);
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createProfissional',
            idAtividadeColetiva: idAtividadeColetiva,
            idProfissionalSaude: idProfissional,
            cbo: cbo
        },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        if (data > 0) {
            blnRet = parseInt(data);
        }
    });
    return blnRet;
}

function substituirProfissional(idProfAtividadeColetiva, idProfissional, cbo) {
    var blnRet = false;
    //alteraAvaliacao(1);
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'substituirProfissional',
            idProfAtividadeColetiva: idProfAtividadeColetiva,
            idProfissionalSaude: idProfissional,
            cbo: cbo
        },
        datatype: "json",
        url: Api.URL
    })
    .success(function (data) {
        blnRet = true;
    });
    return blnRet;
}

function removerProfissionalAtividadeColetiva(idProfissionalAtividadeColetiva, tr) {
    $.ajax({
        type: "POST",
        data: { acao: 'removeProfissional', id: idProfissionalAtividadeColetiva },
        datatype: "json",
        url: Api.URL
    })
    .done(onSuccess)
    .fail(onError);

    function onSuccess() {
        jQuery(tr).remove();
        AtualizaResps(0);
}

    function onError() {
        modalAlerta("Atenção", "Falha ao remover o Profissional, tente novamente mais tarde.");
    }
}

function criaPacienteAtividadeColetiva(idAtividadeColetiva, idIdentificacaoUsuario) {
    var blnRet = false;

    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'createUsuarioAtividadeColetiva',
            idAtividadeColetiva: idAtividadeColetiva,
            idIdentificacaoUsuario: idIdentificacaoUsuario
        },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        if (data > 0)
            blnRet = parseInt(data);
    });
    return blnRet;
}

function dadosParticipantes() {
    var dados = {};
    dados = getFormData("#frmParticipantes");
    console.log(dados)
    dados.Altura = parseFloat(String(dados.Altura).replace(",", "."));
    dados.Peso = parseFloat(String(dados.Peso).replace(",", "."));
    console.log(dados)
    return JSON.stringify(dados);
}

function dadosDetalhesAtividade() {
    var dados = {};
    dados = getFormData("#frmDetalhesAtividade");
    return JSON.stringify(dados);
}

function alteraPacienteAtividadeColetiva(dados) {
    
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'editUsuarioAtividadeColetiva',
            dadosAtividadeColetivaUsuario: dados
        },
        datatype: "json",
        url: Api.URL
    }).done(
        function () {
            limparFormAtividadeColetivaUsuario();
            $('.IdentificacaoUsuario').hide();
        }
    );
}

function alteraAtividadeColetiva(status, dados) {
    var retorno = false;
    
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'Edit',
            dadosAtividadeColetiva: dados,
            status: status
        },
        datatype: "json",
        url: Api.URL
    }).done(function () {
        if (status == 2) {
            window.location.assign("atividadeColetiva.asp");
        }

        retorno = true;
    });

    return retorno;
}

function setProfissional(status, dados) {
    var retorno = false;
    
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'setProfissional',
            dadosAtividadeColetiva: dados,
            status: status
        },
        datatype: "json",
        url: Api.URL
    }).done(function () {
        retorno = true;
    });

    return retorno;
}

function setResponsavel(status, dados) {
    $.ajax({
        type: "POST",
        async: false,
        data: {
            acao: 'setResponsavel',
            dadosAtividadeColetiva: dados,
            status: status
        },
        url: Api.URL
    }).done(function () {
        jQuery("#checkDadosResponsavel").show();
        jQuery('#dadosResponsavel').hide();

        responsavel = true;
    });
}

function removerPacienteAtividadeColetiva(idPacienteAtividadeColetiva) {
    $.ajax({
        type: "POST",
        data: { acao: 'removeUsuarioAtividadeColetiva', id: idPacienteAtividadeColetiva },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        if ($('#listaAtividadeColetiva tbody tr').length == 0) {
            limparFormAtividadeColetivaUsuario();
            $('.IdentificacaoUsuario').hide();
        }
    });
}

function limparFormAtividadeColetivaUsuario() {
    $('.IdentificacaoUsuario').each(function (i) {
        $(this).find('input[type=checkbox]:checked').removeAttr('checked', false);
        $(this).find('input[type=text]').val('');
        $(this).find('input[type=number]').val('');
        $(this).find('input[type=radio]:checked').removeAttr('checked', false);
    });
    removerValidacoesParticipantes();
}

function makeTableResponsavel(nome, cns, idusr, idusratividadecoletiva, numero) {
    var strTable = '';
    strTable += '   <tr id="' + idusr + '" class="sel-cns" data-nome="' + nome.toCapitalize() + '" data-cns="' + cns + '" data-idusr="' + idusr + '" data-idusratividadecoletiva="' + idusratividadecoletiva + '" data-numero="' + numero + '" >';
    strTable += '       <input type="hidden" id="idResponsavel" name="IdResponsavel" value="' + idusr + '" />';
    strTable += '       <input type="hidden" id="numContratoResponsavel" name="NumContratoResponsavel" value="0" />';
    strTable += '       <td class="text-capitalize">' + nome.toCapitalize() + '</td>';
    strTable += '       <td class="text-center">' + cns + '</td>';    
    strTable += '       <td class="text-center"><button type="button" class="btn btn-danger" title="Remover"><span class="glyphicon glyphicon-remove"></span></button></td>';
    strTable += '   </tr>';
    jQuery("#frmRespNumeroCartaoSusDoResponsavel").val(cns);

    if (jQuery("#" + idusr).val() != "") {
        $('#listaResponsavel tbody').append(strTable);
        $('#msgErroTableResp').text('');
        removeGridResponsavel();
        desabilitaBuscaResponsavel();
        $('#modalResponsavel').modal('hide');
    }
    else
    {
        $('#msgErroTableResp').text('Responsável já selecionado.');
    }
}

function makeTablePaciente(Numero, nome, cns, idusr, idusratividadecoletiva) {    
    var strTable = '';
    strTable += '   <tr id="' + idusr + '" class="sel-cns" data-nome="' + nome.toCapitalize() + '" data-cns="' + cns + '" data-idusr="' + idusr + '" data-idusratividadecoletiva="' + idusratividadecoletiva + '" data-numero="' + Numero + '" >';
    strTable += '       <td class="text-center">' + Numero + '</td>';
    strTable += '       <td class="text-capitalize">' + nome.toCapitalize() + '</td>';
    strTable += '       <td class="text-center">' + cns + '</td>';
    strTable += '       <td class="text-center"><button type="button" class="btn btn-danger" title="Remover"><span class="glyphicon glyphicon-remove"></span></button></td>';
    strTable += "       <td class='text-center'><button type='button' class='btn btn-primary preencherDadosPaciente' title='Preencher Ficha'><span class='glyphicon glyphicon-edit'></span></button></td>";
    strTable += '   </tr>';

    $('#listaAtividadeColetiva tbody').append(strTable);
    $('#msgErroTableUsu').text('');
    removeGridPacientes();
    habilitaFichaPaciente();
    $('#modalProfSus').modal('hide');
}

function makeTableProfissional(nome, cns, idprof, idprofatividadecoletiva, cbo, unico) {
    var strTable = "";
    strTable += "   <tr id='" + idprof + "' class='sel-cns' data-nome='" + nome.toCapitalize() + "' data-cns='" + cns + "' data-idprof='" + idprof + "' data-idprofatividadecoletiva='" + idprofatividadecoletiva + "' data-cbo='" + cbo + "'>";
    strTable += "       <td class='text-capitalize'>" + nome.toCapitalize() + "</td>";
    strTable += "       <td class='text-center'>" + cns + "</td>";
    strTable += "       <td class='text-center'>" + cbo + "</td>";
    var displayRemover = (unico) ? "none" : "block";
    var displaySubstituir = (unico) ? "block" : "none";
    strTable += "       <td class='text-center'><button style='display:" + displaySubstituir + "' type='button' class='btn btn-success' title='Substituir'><span class='glyphicon glyphicon-refresh'></span></button>";
    strTable += "       <button style='display:" + displayRemover + "' type='button' class='btn btn-danger' title='Remover'><span class='glyphicon glyphicon-remove'></span></button></td>";
    strTable += "   </tr>";

    $("#listaProfissionaisAtividadeColetiva tbody").append(strTable);
    $('#msgErroTableProf').text('');
    substituiProfSus();
    removeGridProfissionais();
    $("#modalProfSus").modal("hide");
}

function removeGridProfissionais() {
    $('#listaProfissionaisAtividadeColetiva tbody .btn-danger').click(function () {
        var rows = $("#listaProfissionaisAtividadeColetiva tbody tr").length;
        var tr = $(this).closest("tr");
        if (rows > 1) {
            removerProfissionalAtividadeColetiva($(tr).data("idprofatividadecoletiva"), tr);
            //jQuery(tr).remove();
            desabilitaBuscaProfissional();
            if (rows == 2) {
                $("#listaProfissionaisAtividadeColetiva tbody tr button.btn-danger").hide();
                $("#listaProfissionaisAtividadeColetiva tbody tr button.btn-success").show();
            }
        }
    });
}

function desabilitaBuscaProfissional() {
    if ($('#listaProfissionaisAtividadeColetiva tbody tr').length > 5) {
        $('#btnBuscarProf').attr('disabled', 'disabled');
    } else {
        $('#btnBuscarProf').removeAttr('disabled');
    }
}

function removeGridResponsavel() {
    $('#listaResponsavel tbody .btn-danger').click(function () {
        var tr = $(this).closest("tr");
        jQuery("#numeroCartaoSusDoResponsavel").val("null");
        $(tr).remove();
        desabilitaBuscaResponsavel();
    });
}

function removeGridPacientes() {
    $('#listaAtividadeColetiva tbody .btn-danger').click(function () {
        var tr = $(this).closest("tr");
        removerPacienteAtividadeColetiva($(tr).data("idusratividadecoletiva"));
        $(tr).remove();
    });
}

function verificarItensnaTablePaciente(event) {
    var retorno = true;

    $('#msgErroTableUsu').text('');


    if ($('#listaAtividadeColetiva tbody tr').length == 0) {
        $('#msgErroTableUsu').text($('#msgErroTableUsu').text() + 'Insira no mínimo um Participante para realizar a Atividade Coletiva.');
        $('#btnBuscarUsu').focus();
        retorno = false;
    }   
    participantes = retorno;
    return retorno;
}

function verificarItensnaTableResponsavel() {
    var retorno = true;
    $('#msgErroTableResp').text('');
    if ($('#listaResponsavel tbody tr').length == 0) {
        $('#msgErroTableResp').text('Insira no mínimo um Responsável para realizar a Atividade Coletiva.');
        $('#btnBuscarResponsavel').focus();
        retorno = false;
    }
    
    return retorno;
}

function verificarItensnaTableProfissionais() {
    var retorno = true;

    $('#msgErroTableProf').text('');
    if ($('#listaProfissionaisAtividadeColetiva tbody tr').length == 0) {
        $('#msgErroTableProf').text('Insira no mínimo um Profissional para realizar a Atividade Coletiva.');
        $('#btnBuscarProf').focus();
        retorno = false;
    }
    
    return retorno;
}

function desabilitaBuscaResponsavel() {
    if ($('#listaResponsavel tbody tr').length > 0) {
        $('#btnBuscarResponsavel').attr('disabled', 'disabled');
    } else {
        $('#btnBuscarResponsavel').removeAttr('disabled');
    }
}

function desabilitaBuscaPaciente() {
    if ($('#listaAtividadeColetiva tbody tr').length > 12) {
        $('#btnBuscarUsu').attr('disabled', 'disabled');
    } else {
        $('#btnBuscarUsu').removeAttr('disabled');
    }
}

function habilitaFichaPaciente() {
    $("#listaAtividadeColetiva tbody .preencherDadosPaciente").on("click", function () {
        jQuery("#frmPartNumeroCartaoSus").val($(this).closest("tr").data("cns"));
        jQuery("#frmPartId").val($(this).closest("tr").data("idusratividadecoletiva"));
        jQuery("#frmPartIdIdentificacaoUsuario").val($(this).closest("tr").data("idusr"));
        var data = $(this).closest("tr").data("idusratividadecoletiva");
        $("#listaAtividadeColetiva tbody tr ").removeClass("active");
        jQuery(this).closest("tr").addClass("active");
        if (data != $('.IdentificacaoUsuario').data("id")) {
            limparFormAtividadeColetivaUsuario();
        }
        $('.IdentificacaoUsuario').show().data("id", data);
        getFichaPaciente(data);
    });
}

function habilitaFichaPacienteSelecionado(filtro) {
    $(filtro).on("click", function () {
        jQuery("#frmPartNumeroCartaoSus").val($(this).closest("tr").data("cns"));
        jQuery("#frmPartId").val($(this).closest("tr").data("idusratividadecoletiva"));
        jQuery("#frmPartIdIdentificacaoUsuario").val($(this).closest("tr").data("idusr"));
        var data = $(this).closest("tr").data("idusratividadecoletiva");
        $("#listaAtividadeColetiva tbody tr ").removeClass("active");
        jQuery(this).closest("tr").addClass("active");
        if (data != $('.IdentificacaoUsuario').data("id")) {
            limparFormAtividadeColetivaUsuario();
        }
        $('.IdentificacaoUsuario').show().data("id", data);
        getFichaPaciente(data);
    });
}

function getFichaPaciente(idUsrAtividadeColetiva) {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getUsuarioAtividadeColetivaById', idAtividadeColetiva: idUsrAtividadeColetiva },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        var paciente = JSON.parse(data)[0]
        //if (paciente.Altura != null && paciente.Altura > 0) {
            //paciente.Altura = String(parseFloat(paciente.Altura.replace(",", ".")));
        //}
        if (paciente.NumeroCartaoSus != null) {
            paciente.NumeroCartaoSus = paciente.NumeroCartaoSus.trim();
        }
        makeDadosPaciente(paciente);
        habilitaPNCTUsuario();
    });
}

function getFichaResponsavel(idAtividadeColetiva) {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getResponsavelAtividadeColetivaById', idAtividadeColetiva: idAtividadeColetiva },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            var responsavel = obj.resultado[0];
            $('#selCnesUniResponsavel').val(responsavel.CodigoCnesUnidadeResponsavel);
            makeEquipeIneDoResponsavel(responsavel.CodigoCnesUnidadeResponsavel, responsavel.CodigoEquipeIneResponsavel);
            setFormData(responsavel);
        }
    });
}

function makeDadosPaciente(item) {
    setFormData(item);
}

function getProfissionais() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getProfissionalByIdAtividadeColetiva', idAtividadeColetiva: jQuery("#idAtividadeColetiva").val() },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            $("#listaProfissionaisAtividadeColetiva tbody").empty();
            var linhas = obj.resultado.length;
            $.each(obj.resultado, function (ResultadoItens, item) {
                var unico = (linhas == 1 && ResultadoItens == 0);
                makeTableProfissional(item.Nome, item.CNS, item.IdProf, item.IdProfAtividadeColetiva, item.CBO, unico);
            });
            AtualizaResps(0);
        }
        jQuery("#checkDadosProfissionais").hide();
        jQuery('#dadosProfissionais').show();
    });
}

function getPacientes() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getUsuarioByIdAtividadeColetiva', idAtividadeColetiva: jQuery("#idAtividadeColetiva").val() },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            $('#listaAtividadeColetiva tbody').empty();
            $.each(obj.resultado, function (ResultadoItens, item) {
                makeTablePaciente(item.Numero, item.Nome, item.CNS, item.IdUsr, item.IdUsrAtividadeColetiva);
            });
        }

        jQuery("#checkDadosParticipantes").hide();
        jQuery('#dadosParticipantes').show();
    });
}

function getResponsavel() {
    jQuery.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getResponsavelByIdAtividadeColetiva', idAtividadeColetiva: jQuery("#idAtividadeColetiva").val() },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {
            $('#listaResponsavel tbody').empty();
            var item = obj.resultado[0];
            //makeTableResponsavel(item.Nome, item.CNS, item.IdResponsavel, item.IdAtividadeColetiva, 1);
            getFichaResponsavel(item.IdAtividadeColetiva);
        }

        jQuery("#checkDadosResponsavel").hide();
        jQuery('#dadosResponsavel').show();
    });
}

function getDadosAtividadeColetiva() {
    $.ajax({
        type: "POST",
        async: false,
        data: { acao: 'getById', id: jQuery("#idAtividadeColetiva").val() },
        datatype: "json",
        url: Api.URL
    })
    .done(function (data) {
        var obj = JSON.parse(data)
        if (obj.status) {

            if (obj.resultado.codigo_cnes_unidade > 0) {
                $('#selCnesUni').val(obj.resultado.codigo_cnes_unidade).prop('disabled', 'disabled');
            }
            $('#txtIneEquipe').val(obj.resultado.codigo_equipe_ine);

            if (obj.resultado.data_de_atividade != null) {
                var dataAtividadeColetiva = obj.resultado.data_de_atividade.split("-");
                $('#txtData').val(dataAtividadeColetiva[2] + "/" + dataAtividadeColetiva[1] + "/" + dataAtividadeColetiva[0]);
            }
        }
    });
}

//Regras Não Obrigatórias
//Hoje não é obrigatório, mas se ficar obrigatório será necessário chamar essa função:
//"Marcar se o usuário cessou o hábito de fumar e/ou se abandonou o grupo, 
//se a atividade ocorreu no âmbito do Programa Nacional de Controle do Tabagismo. 
//Em caso afirmativo, é preciso registrar também, no bloco de práticas/temas para saúde, 
//os itens de 25 a 28, de acordo com cada situação." - Pág. 75 - Manual CDS (Versão 2.0)

function getHasPNCTUsuario() {
    var retorno = {
        any: false,
        all: false
    };
    var def = new jQuery.Deferred();
    postAjaxObject({
        idAtividadeColetiva: jQuery("#idAtividadeColetiva").val(),
        acao: "getHasPNCTUsuario"
    }).done(onSuccess).fail(onError);

    function onSuccess(listPNCT) {
        retorno.any = JSLINQ(listPNCT).Any(function (item) { return item.PNCT == true });
        retorno.all = !JSLINQ(listPNCT).Any(function (item) { return item.PNCT == false });
        def.resolve(retorno);
    }

    function onError(data){
        def.reject("Falha na requisição.");
    }

    return def.promise();
}

function removerValidacoesParticipantes(event) {
    jQuery("#Usuario").find(".msgErro").empty();
    jQuery("#Usuario").find("input[type='text'], input[type='number']").removeClass("error");
    jQuery("#Usuario").find("select").removeClass("error");
}

function habilitaPNCTUsuario() {
    if (jQuery("input[name='PraticasPNCTSessao1']:checked, "
         + "input[name='PraticasPNCTSessao2']:checked, "
         + "input[name='PraticasPNCTSessao3']:checked, "
         + "input[name='PraticasPNCTSessao4']:checked").length > 0) {
        jQuery("input[name='TabagismoCessouHabitoFumar'], input[name='TabagismoAbandonouGrupo']").prop("disabled", "");
    }
    else {
        jQuery("input[name='TabagismoCessouHabitoFumar'], input[name='TabagismoAbandonouGrupo']").prop("disabled", "disabled");
    }
}

function CarregarEquipeIneDoResponsavel() {
    var CodCnes = $('#selCnesUniResponsavel :selected').val();
    if (parseInt(CodCnes) > 0) {
        $.ajax({
            type: "POST",
            async: false,
            data: { acao: 'getEquipeIne', codCnes: CodCnes },
            datatype: "json",
            url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
        })
        .done(function (data) {
            var obj = JSON.parse(data);
            $("#selIneEquipeResponsavel").prop("disabled", false);
            if (obj.status) {
                var options = "";
                options = '<option value="">SELECIONE UMA EQUIPE</option>'
                $.each(obj.resultado, function (ResultadoItens, item) {
                    options += '<option value="' + item.CodINE + '">' + item.Numero + ' - ' + item.Descricao + '</option>';
                });
                $("#selIneEquipeResponsavel").html(options);
                $("label[for='txtIneEquipeDoResponsavel']").html('Cód. equipe (INE)  <span class="asterisco"> *</span>').removeClass('removeasterisco');
            }
            else {
                $('#selIneEquipeResponsavel').html('<option value="">NÃO HÁ EQUIPES</option>');
                $("label[for='txtIneEquipeDoResponsavel'] .asterisco").remove();
                $("label[for='txtIneEquipeDoResponsavel']").addClass('removeasterisco');
            }
        });
    }
    else {
        $("#selIneEquipeResponsavel").empty();
        $("#selIneEquipeResponsavel").html('<option value="0" selected="selected">Selecione a unidade</option>');
        //$("#selIneEquipeResponsavel").prop("disabled", true);
    }
}

function makeEquipeIneDoResponsavel(CodCnes, CodINE) {
    if (parseInt(CodCnes) > 0) {
        $.ajax({
            type: "POST",
            async: false,
            data: { acao: 'getEquipeIne', codCnes: CodCnes },
            datatype: "json",
            url: "ajax/atendIndividual/ajax-atendimento-individual.asp"
        })
        .done(function (data) {
            var obj = JSON.parse(data);
            $("#selIneEquipeResponsavel").prop("disabled", false);
            if (obj.status) {
                var options = "";
                options = '<option value="">SELECIONE UMA EQUIPE</option>'
                $.each(obj.resultado, function (ResultadoItens, item) {
                    options += '<option value="' + item.CodINE + '">' + item.Numero + ' - ' + item.Descricao + '</option>';
                });
                $("#selIneEquipeResponsavel").html(options);
                $("label[for='txtIneEquipeDoResponsavel']").html('Cód. equipe (INE) <span class="asterisco"> *</span>').removeClass('removeasterisco');
                if (CodINE <= 23 && CodINE > 0) { $("#selIneEquipeResponsavel").val(CodINE) };

            }
            else {
                $('#selIneEquipeResponsavel').html('<option value="">NÃO HÁ EQUIPES</option>');
                $("label[for='txtIneEquipeDoResponsavel'] .asterisco").remove();
                $("label[for='txtIneEquipeDoResponsavel']").addClass('removeasterisco');
            }
        });
    }
    else {
        $("#selIneEquipeResponsavel").empty();
        //$("#selIneEquipeResponsavel").prop("disabled", true);
    }
}

function substituiProfSus() {
    $('#listaProfissionaisAtividadeColetiva tbody .btn-success').on("click", busca_Prof_Sus);
}

function pacienteNovamente() {

    $('#modalSemCartaoSus').modal('hide');
    busca_Paciente_Sus();

}

function salvarSemCadastroAtividade(cns) {
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
            $.each(data.resultado, function (ResultadoItens, item) {
                var idAtividadeColetiva = $("#idAtividadeColetiva").val();
                if (cns == "") {
                    cns = item.cns
                }
                var idPacienteAtividadeColetiva = criaPacienteAtividadeColetiva(idAtividadeColetiva, '21525961');
                if (idPacienteAtividadeColetiva != false) {
                    getPacientes();
                } else {
                    modalAlerta("Atenção", "Falha ao adicionar o Paciente na Atividade Coletiva, tente novamente mais tarde.");
                }
            });
            $('#modalSemCartaoSus').modal('hide');
            $('#txtmodalBuscaCartaoSus').val('');
            $('#txtmodalConfirmaCartaoSus').val('');
        } else {
            if (data.sessao) {
                setTimeout(function () {
                    modalAlerta("Sessão Expirada", "Sua sessão expirou!");
                    setTimeout(function () {
                        window.location.assign("login.asp");
                    }, 2000);
                }, 800);
            }
        }
    });

}

function addSemCadastroAtividade() {
    if ($('#txtmodalBuscaCartaoSus').val() != "") {
        if ($('#txtmodalBuscaCartaoSus').val() == $('#txtmodalConfirmaCartaoSus').val()) {
            salvarSemCadastroAtividade($('#txtmodalConfirmaCartaoSus').val());
        }
        else {
            $('#modalSemCartaoSus .msgErro .col-md-12').text('O cartão do SUS não confere com o digitado anteriormente.Favor digite novamente.');
        }
    }
    else {
        $('#txtmodalBuscaCartaoSus').val('');
        $('#txtmodalConfirmaCartaoSus').val('');
        salvarSemCadastroAtividade($('#txtmodalConfirmaCartaoSus').val());
    }

}