function main() {
    mostrarTablaExtranjeros()
    mostrarTablaProductosConFiltros()
    mostrarTablaParesDeProductos()
}

const gridExtranjeros = new gridjs.Grid()
const gridParesDeProductos = new gridjs.Grid()
const gridProductosPorFiltro = new gridjs.Grid()

function mostrarTablaExtranjeros() {
    const columnas = [
        { id: "Nombre",   name: "Nombre" },
        { id: "Pais",     name: "Pais" },
        { id: "Precio", name: "Precio(₡)" },
        { id: "Cantidad", name: "Cantidad" },
        { id: "Ingresos", name: "Ingresos Generados(₡)" }
    ]
    const contenedor = document.getElementById("tablaDatosExtranjeros")
    mostrarTabla(gridExtranjeros, columnas, contenedor, actualizarTablaDatosExtranjeros)
}

function mostrarTablaParesDeProductos() {
    const columnas = [
        { id: "Producto",      name: "Producto" },
        { id: "CompradoCon",   name: "Comprado con" },
        { id: "CantidadVeces", name: "Cantidad de veces" },
        { id: "Ingresos", name: "Ingresos generados(₡)" },
    ]
    const contenedor = document.getElementById("tablaParesDeProductos")
    mostrarTabla(gridParesDeProductos, columnas, contenedor, actualizarTablaParesDeProductos)
}

function mostrarTablaProductosConFiltros() {
    const columnas = [
        { id: "Nombre", name: "Nombre" },
        { id: "Precio", name: "Precio(₡)" },
        { id: "Cantidad", name: "Cantidad" },
        { id: "Ingresos", name: "Ingresos Generados(₡)" }
    ]
    const contenedor = document.getElementById("tablaProductosConFiltro")
    mostrarTabla(gridProductosPorFiltro, columnas, contenedor, actualizarTablaProductosConFiltro)
}