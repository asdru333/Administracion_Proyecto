function main() {
    mostrarTablaRanking()
}

const gridRanking = new gridjs.Grid()

function mostrarTablaRanking() {
    const columnas = [
        { id: "Nombre",   name: "Nombre" },
        { id: "Precio", name: "Precio(₡)" },
        { id: "fechaIngreso", name: "Fecha de ingreso" },
        { id: "fechaUltimaVenta", name: "Fecha de última venta" },
    ]
    const contenedor = document.getElementById("tablaProductosConFiltro")
    mostrarTabla(gridRanking, columnas, contenedor, actualizarTablaPorRanking)
}