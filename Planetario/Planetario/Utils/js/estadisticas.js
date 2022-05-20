function graficoBarra(datos, labels, graphInfo) {

    var chart = new CanvasJS.Chart(graphInfo.element, {
        animationEnabled: true,
        theme: "light2",
        title: {
            text: graphInfo.title
        },
        axisY: {
            title: "Participantes",
        },
        data: [{
            type: "column",
            yValueFormatString: "#,###\"\"",
            dataPoints: datos.map(function (dato, indice) {
                return {
                    label: labels[indice].Text,
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
        data: [{
            type: "line",
            indexLabelFontSize: 16,
            dataPoints: datos.map(function (dato, indice) {
                return {
                    y: dato,
                    label: labels[indice].Text
                }
            })
        }]
    });
    chart.render();

}

function recargarTopicos() {
    var opcionCategoria = document.getElementById("opcionCategoria").value;

    if (opcionCategoria === 'General') {
        var nuevosTopicos = ['Astrofotografia', 'Instrumentos', 'Pregunta Sencilla']
    }
    else if (opcionCategoria == "Astronomia") {
        var nuevosTopicos = ["Astronomia Observacional", "Astronomia Teorica", "Mecanica Celeste", "Astrofisica", "Astroquimica", "Astrobiologia"]
    }
    else if (opcionCategoria == "Cuerpos del sistema solar") {
        var nuevosTopicos = ["Planetas", "Satélites", "Cometas", "Asteroides"]
    }
    else if (opcionCategoria == "Objetos de Cielo Profundo") {
        var nuevosTopicos = ["Galaxias", "Estrellas", "Nebulosas", "Planetarias"]
    }
    else {
        var nuevosTopicos = ['Astrofotografia', 'Instrumentos', 'Pregunta Sencilla', "Astronomia Observacional", "Astronomia Teorica",
            "Mecanica Celeste", "Astrofisica", "Astroquimica", "Astrobiologia", "Planetas", "Satélites", "Cometas", "Asteroides",
            "Galaxias", "Estrellas", "Nebulosas", "Planetarias"]
    }
    
    let content = `<label for="opcionTopico">Topico</label>`

    content += `<select name="opcionTopico" id="opcionTopico">`
    content += `<option value="">Todos</option>`
    for (let counter = 0; counter < nuevosTopicos.length; ++counter)
    {
        content += `<option value= '${nuevosTopicos[counter]}'>${nuevosTopicos[counter]}</option>`
    }
    content += `</select>`
    let selectDetails = document.getElementById('dropdownTopico')
    selectDetails.innerHTML = content
}

var seleccion = document.getElementById('opcion')

document.addEventListener('DOMContentLoaded', function () {

    recargarTopicos();

    graficoBarra(participacionesPorDia, dias, { element: "chartFecha", title: "Participación según día", axisX: "" })
    graficoBarra(participacionesPorPublico, publicos, { element: "chartPublico", title: "Participación según público", axisX: "" })
    graficoBarra(participacionesPorComplejidad, complejidades, { element: "chartComplejidad", title: "Participación según complejidad", axisX: "" })
    graficoBarra(participacionesPorCategoria, categorias, { element: "chartCategoria", title: "Participación según categoría", axisX: "" })
    
    console.log(participacionesPorTopicoTodos);
    console.log(topicos);
    
    graficoBarra(participacionesPorTopicoTodos, topicos, { element: "chartTopico", title: "Participación según tópico", axisX: "" })
})

seleccion.addEventListener('change', function (event) {

    var opcion = event.target.value

    if (opcion == "linea") {
        graficoLinea(participacionesPorDia, dias, { element: "chartFecha", title: "Participación según día", axisX: "" })
        graficoLinea(participacionesPorPublico, publicos, { element: "chartPublico", title: "Participación según público", axisX: "" })
        graficoLinea(participacionesPorComplejidad, complejidades, { element: "chartComplejidad", title: "Participación según complejidad", axisX: "" })
        graficoLinea(participacionesPorCategoria, categorias, {element:"chartCategoria", title:"Participación según categoría",axisX:""})
        graficoLinea(participacionesPorTopicoTodos, topicos, { element: "chartTopico", title: "Participación según tópico", axisX: "" })

    } else if (opcion == "barra") {
        graficoBarra(participacionesPorDia, dias, { element: "chartFecha", title: "Participación según día", axisX: "Días" })
        graficoBarra(participacionesPorPublico, publicos, { element: "chartPublico", title: "Participación según público", axisX: "Público Dirigido" })
        graficoBarra(participacionesPorComplejidad, complejidades, { element: "chartComplejidad", title: "Participación según complejidad", axisX: "" })
        graficoBarra(participacionesPorCategoria, categorias, { element: "chartCategoria", title: "Participación según categoría", axisX: "" })
        graficoBarra(participacionesPorTopicoTodos, topicos, { element: "chartTopico", title: "Participación según tópico", axisX: "" })
    }
})

var seleccionCategoria = document.getElementById("opcionCategoria")

seleccionCategoria.addEventListener('change', function (event) {
    recargarTopicos();
})
