var geocoder;
var map;
var locations = [];

$(document).ready(function () {

    var options = {
        zoom: 12,
        center: new google.maps.LatLng(-24.1817994, -46.785033399999975),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("mapa"), options);

    geocoder = new google.maps.Geocoder();

    var ini = 0;
    var fim = 50;
    while (ini < 3000) {

        setTimeout(function () {
            carregaMunicipe2(ini, fim)
        }, 1500);

        ini = fim
        fim += 50;
    }

    //console.log(fim)

});

function carregarNoMapa(endereco, titulo) {
    var title = 'Título';
    if (titulo) title = titulo;
    geocoder.geocode({ 'address': endereco + ', Brasil', 'region': 'BR' }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[0]) {
                var latitude = results[0].geometry.location.lat();
                var longitude = results[0].geometry.location.lng();

                //console.log(latitude + ', ' + longitude);

                new google.maps.Marker({
                    position: new google.maps.LatLng(latitude, longitude),
                    icon: "img/marcador_google.png",
                    title: title,
                    map: map
                });
            }
        }
    })
}

function getCoordenadas(endereco, titulo) {

    var title = 'Título';
    if (titulo) title = titulo;
    geocoder.geocode({ 'address': endereco + ', Brasil', 'region': 'BR' }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[0]) {
                var latitude = results[0].geometry.location.lat();
                var longitude = results[0].geometry.location.lng();

                var loc = [title, latitude, longitude];

                locations.push(loc);
            }
        }
    })

}

function addMap() {

    var infowindow = new google.maps.InfoWindow();

    var marker, i;

    for (i = 0; i < locations.length; i++) {
        marker = new google.maps.Marker({
            position: new google.maps.LatLng(locations[i][1], locations[i][2]),
            title: locations[i][0],
            map: map
        });

        /*google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                infowindow.setContent(locations[i][0]);
                infowindow.open(map, marker);
            }
        })(marker, i));*/
    }

}

function carregaMunicipe() {

    $.ajax({
        type: "POST",
        datatype: "json",
        url: "ajax/mapa/mapa_municipe.asp"
    })
    .done(function (data) {

        if (data.status) {

            $.each(data.resultado, function (ResultadoItens, item) {
                if (item.logradouro != '') {

                    var nome = item.codigo + ' - ' + item.nome;
                    var endereco = '';

                    if (item.logradouro != '') endereco += item.logradouro;
                    if ($.isNumeric(item.numero)) endereco += ', ' + item.numero;
                    if (item.bairro != '') endereco += ', ' + item.bairro;
                    if (item.cidade != '') endereco += ', ' + item.cidade;

                    carregarNoMapa(endereco, nome);
                }

            });

        }

    });
}

function carregaMunicipe2(ini, fim) {

    //console.log('Foi');

    $.ajax({
        type: "POST",
        data: {inicio: ini, fim: fim},
        datatype: "json",
        url: "ajax/mapa/mapa_municipe.asp"
    })
    .done(function (data) {
        if (data.status) {
            $.each(data.resultado, function (ResultadoItens, item) {
                if (item.logradouro != '') {

                    var nome = item.codigo + ' - ' + item.nome;
                    var endereco = '';

                    if (item.logradouro != '') endereco += item.logradouro;
                    if ($.isNumeric(item.numero)) endereco += ',' + item.numero;
                    if (item.bairro != '') endereco += ',' + item.bairro;
                    if (item.cidade != '') endereco += ',' + item.cidade;

                    endereco += ',BRASIL';

                    //carregarNoMapa(endereco, nome);

                    //getCoordenadas(endereco, nome);

                    $.ajax({
                        type: "GET",
                        datatype: "json",
                        url: "http://maps.google.com/maps/api/geocode/json?address=" + endereco + "&sensor=false"
                    })
                    .done(function (data, status) {

                        //console.log(data.status + ' >> ' + endereco);

                        if (data.status == 'OK') {

                            var latitude = data.results[0].geometry.location.lat;
                            var longitude = data.results[0].geometry.location.lng;

                            ////console.log(latitude + ', ' + longitude);

                            var loc = [nome, latitude, longitude];

                            locations.push(loc);

                        }
                    });

                }
            });
        }

    });
}