using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace myNote.Api.Filters
{
    public class ArgumentExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is ArgumentException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
            }
        }
    }
}