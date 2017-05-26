function loadTest(size) {
    var rootURL = location.href.substring(0, location.href.indexOf("/swagger"));

    var data = {"profissionalCNS":"708900743318910","cboCodigo_2002":"515105","cnes":3916308,"ine":"0000349488","dataAtendimento":1495542534,"codigoIbgeMunicipio":"3547304"};

    for(var i = 0; i < size; i++) {
        setTimeout(function() {
            $.ajax({url: rootURL + "/enviar/cabecalho", data: data, dataType: "json", type: "post" }).done(function(token){
                console.info(arguments);
                $.post(rootURL + "/encerrar/" + token);
            }).fail(function(){console.error(arguments)});
        }, size - i);
    }
}