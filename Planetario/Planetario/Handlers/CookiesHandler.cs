using System.Web;
using Planetario.Interfaces;

namespace Planetario.Handlers
{
    public class CookiesHandler: CookiesInterfaz
    {
        public bool SesionIniciada()
        {
            return (HttpContext.Current.Request.IsAuthenticated);
        }
        public string CorreoUsuario()
        {
            return (HttpContext.Current.User.Identity.Name);
        }
    }
}