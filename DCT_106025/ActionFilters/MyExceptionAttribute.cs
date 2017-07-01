using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DCT_106025.ActionFilters
{
    public class MyExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext ex)
        {
            ex.Response = ex.Request.CreateResponse(HttpStatusCode.InternalServerError, new
            {
                Message = ex.Exception.Message,
                Error = ex.Exception
            });

            base.OnException(ex);
        }
    }
}