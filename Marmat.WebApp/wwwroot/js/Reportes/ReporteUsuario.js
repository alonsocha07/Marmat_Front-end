$(document).ready(function () {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset-utf-8",
        dataType: "json",
        url: "https://marmatcondominioscr.azurewebsites.net/Dashboard/ObtenerReporteUsuario",
        error: function () {
            swal({
                title: "Error!",
                text: "No se pudo consultar los datos",
                icon: "error"
            });
        },
        success: function (data) {
            swal({
                title: "Cargado!",
                text: "Datos cargados satisfactoríamente",
                icon: "success"
            });
            GraficaUsuarioRol(data);
        }
    })
})

function GraficaUsuarioRol(data) {
    Highcharts.chart('Usuario', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: true,
            type: 'pie'
        },
        title: {
            text: 'Cantidad de usuarios registrados según su rol'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true
                },
                showInLegend: true
            }
        },
        series: [{
            name: 'Cantidad de usuarios registrados por ID de su rol',
            colorByPoint: true,
            data: data
        }]
    });
}