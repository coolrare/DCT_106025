using DCT_106025.ActionFilters;
using System.Web;
using System.Web.Mvc;

namespace DCT_106025
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new MyExceptionAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
