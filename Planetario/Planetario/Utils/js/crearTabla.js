const lenguajeTabla = {
    search: { placeholder: "🔍 Busqueda..." },
    sort: {
        sortAsc: "Ordenar la columna en orden ascendente",
        sortDesc: "Ordenar la columna en orden descendente"
    },
    pagination: {
        previous: "Anterior",
        next: "Siguiente",
        navigate: function (e, r) { return "Página " + e + " de " + r },
        page: function (e) { return "Página " + e },
        showing: "Mostrando ", of: "de", to: "a", results: "resultados totales"
    },
    loading: "Cargando...",
    noRecordsFound: "Ningún resultado encontrado",
    error: "Se produjo un error al recuperar datos"
};

function mostrarTabla(grid,columnas, contenedor, llenarTabla_callback) {
    grid.updateConfig({
        columns: columnas,
        pagination: { limit: 5 },
        language: lenguajeTabla,
        sort: true,
        data: [],
        search: { enabled: true },
        resizable: true,
    }).render(contenedor);
    llenarTabla_callback()
}

function actualizarDatosTabla(grid,baseURL, diccionariParametros, callbakMapeo) {
    let urlDatos = baseURL + "?"

    for (let parametro in diccionariParametros) {
        urlDatos += parametro + "=" + diccionariParametros[parametro]
        urlDatos += "&"
    }
    
    grid.updateConfig({
        server: {
            url: urlDatos,
            then: callbakMapeo
        }
    }).forceRender();
}