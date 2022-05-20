function graficoLinea(datos, labels, graphInfo) {

    var chart = new CanvasJS.Chart(graphInfo.element, {
        animationEnabled: true,
        theme: "light2",
        zoomEnabled: true,
        title: {
            text: graphInfo.title
        },
        axisX: {
            title: "Fechas"
        },
        axisY: {
            title: "Cantidad vendida"
        },
        data: [{
            type: "line",
            indexLabelFontSize: 16,
            dataPoints: datos.map(function (dato, indice) {
                return {
                    y: dato,
                    label: labels[indice]
                }
            })
        }]
    });
    chart.render();
}

document.addEventListener('DOMContentLoaded', function () {
    graficoLinea(cantidad, fecha, { element: "chart", title: "Ventas", axisX: "" })
})