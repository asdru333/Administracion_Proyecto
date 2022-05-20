function graficoBarra(datos, labels, graphInfo) {

    var chart = new CanvasJS.Chart(graphInfo.element, {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: graphInfo.title
        },
        axisY: {
            title: "Cantidad",
            interval: 1
        },
        data: [{
            type: "column",
            yValueFormatString: "#",
            dataPoints: datos.map(function (dato, indice) {
                return {
                    label: labels[indice],
                    y: dato
                }
            })
        }]
    });
    chart.render();
}

function graficoLinea(datos, labels, graphInfo) {

    var chart = new CanvasJS.Chart(graphInfo.element, {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: graphInfo.title
        },
        axisY: {
            title: "Cantidad",
            interval: 1
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

var seleccion = document.getElementById('opcion')

document.addEventListener('DOMContentLoaded', function () {
    graficoBarra(listaNumIdiomas, listaIdiomas, { element: "chart", title: "Idiomas", axisX: "" })
})

seleccion.addEventListener('change', function (event) {

    var opcion = event.target.value

    if (opcion == "linea") {
        graficoLinea(listaNumIdiomas, listaIdiomas, { element: "chart", title: "Idiomas", axisX: "" })

    } else if (opcion == "barra") {
        graficoBarra(listaNumIdiomas, listaIdiomas, { element: "chart", title: "Idiomas", axisX: "Días" })
    }
})
