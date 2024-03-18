$(document).ready(function () {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset-utf-8",
        dataType: "json",
        url: "https://marmatcondominioscr.azurewebsites.net/Dashboard/GetReporteBitacora",
        error: function () {
            swal({
                title: "Error!",
                text: "No se pudieron consultar los datos",
                icon: "error"
            });
        },
        success: function (data) {
            swal({
                title: "Cargado!",
                text: "Datos cargados satisfactoríamente",
                icon: "success"
            });
            GraficaBitacora(data);
        }
    })
})

function GraficaBitacora(data) {
    Highcharts.chart('Bitacora', {
        chart: {
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: true,
            type: 'pie'
        },
        title: {
            text: 'Transacciones en bitácora por usuario'
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
            name: 'Porcentaje de transacciones',
            colorByPoint: true,
            data: data
        }]
    });
}