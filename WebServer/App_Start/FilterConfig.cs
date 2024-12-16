using System.Web;
using System.Web.Mvc;
using WebServer.filter;

namespace WebServer
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new TkAuth());
        }
    }
}
