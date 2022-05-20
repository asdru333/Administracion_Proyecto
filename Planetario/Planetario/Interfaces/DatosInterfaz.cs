using System.Collections.Generic;
using System.Web.Mvc;

namespace Planetario.Interfaces
{
    public interface DatosInterfaz
    {
        string ObtenerRutaDocumento(string nombreDocumento);

        List<string> ObtenerTodosLosTopicos();

        List<string> ObtenerTopicosPorCategoria(string categoria);

        List<SelectListItem> CrearSelectList(List<string> listaDeStrings);

        List<SelectListItem> SelectListPaises();

        List<SelectListItem> SelectListCategorias();

        List<SelectListItem> SelectListGeneros();

        List<SelectListItem> SelectListComplejidades();

        List<SelectListItem> SelectListPublicos();

        List<SelectListItem> SelectListTiposDeActividad();

        List<SelectListItem> SelectListNivelesEducativos();

        List<SelectListItem> SelectListDiasDeLaSemana();

        List<SelectListItem> SelectListTodosLosTopicos();

    }
}