function graficoBarra(datos, labels, graphInfo) {

    var chart = new CanvasJS.Chart(graphInfo.element, {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: graphInfo.title
        },
        exportEnabled: true,
        axisY: {
            title: "Reportes",
            interval: 1
        },
        data: [{
            type: "column",
            yValueFormatString: "#,###\"\"",
            dataPoints: datos.map(function (dato, indice) {
                return {
                    label: labels.Data[indice],
                    y: dato
                }
            })
        }]
    });
    chart.render();
}


document.addEventListener('DOMContentLoaded', function () {

    graficoBarra(respuestasAgradable, opciones, { element: "chartAgradable", title: "Opiniones estéticas del sitio web", axisX: "" })
    graficoBarra(respuestasNavegar, opciones, { element: "chartNavegar", title: "Opiniones de navegar en el sitio web", axisX: "" })
    graficoBarra(respuestasComprar, opciones, { element: "chartComprar", title: "Opiniones de comprar en el sitio web", axisX: "" })
    graficoBarra(respuestasPrecios, opciones, { element: "chartPrecios", title: "Opiniones de los precios del sitio web", axisX: "" })
    graficoBarra(respuestasSatisfecho, opciones, { element: "chartSatisfecho", title: "Opiniones de satisfacción en el sitio web", axisX: "" })
})
