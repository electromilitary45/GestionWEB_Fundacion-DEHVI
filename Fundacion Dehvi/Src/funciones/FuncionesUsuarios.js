
/*--------METODO PARA Obtener nombre -------*/
function consultaCedula() {
    let cedulaPersona = $("#cedulaFisica").val();
    let nombreCompleto = "";
    if (cedulaPersona.length > 0) {
        $.ajax({
            url: 'https://apis.gometa.org/cedulas/' + cedulaPersona,
            type: "GET",
            success: function (data) {
                $("#nombre").val(data.results[0].firstname1 + " " + data.results[0].firstname2);
                $("#apellido1").val(data.results[0].lastname1);
                $("#apellido2").val(data.results[0].lastname2);

                //nombreCompleto = data.firstname1 + " " + data.firstname2 + " " + data.lastname1 + " " + data.lastname2;
            }
        });
    } else {
        $("#nombre").val("");
        $("#primApellido").val("");
        $("#segApellido").val("");
    }
}


