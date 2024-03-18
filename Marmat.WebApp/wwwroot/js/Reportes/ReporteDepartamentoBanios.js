﻿$(document).ready(function () {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset-utf-8",
        dataType: "json",
        url: "https://marmatcondominioscr.azurewebsites.net/Dashboard/ObtenerReporteDepartamentoBanios",
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
            GraficaDepartamentoBanios(data);
        }
    })
})

function GraficaDepartamentoBanios(data) {
    Highcharts.chart('BarrasDepartamentoBanios', {
        chart: {
            type: 'column'
        },
        title: {
            text: 'Cantidad de Baños'
        },
        subtitle: {
            text: ''
        },
        xAxis: {
            type: 'category',
            labels: {
                rotation: -60,
                style: {
                    fontSize: '12px',
                    fontFamily: 'Verdana, sans-serif'
                }
            }
        },
        yAxis: {
            min: 0,
            title: {
                text: 'Cantidad'
            }
        },
        legend: {
            enabled: true
        },
        tooltip: {
            pointFormat: 'Cantidad Baños: <b>{point.y:.1f} unit</b>'
        },
        series: [{
            name: 'Baños',
            data: data,
            dataLabels: {
                enabled: true,
                rotation: -90,
                color: '#FFFFFF',
                align: 'right',
                format: '{point.y:.1f}', // one decimal
                y: 10, // 10 pixels down from the top
                style: {
                    fontSize: '12px',
                    fontFamily: 'Verdana, sans-serif'
                }
            }
        }]
    });
}