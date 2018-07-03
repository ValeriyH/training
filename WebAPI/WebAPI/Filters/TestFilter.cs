using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebAPI.Filters
{
    public class TestFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var test = actionContext;
            //if (actionContext.ActionArguments["shift"] != 0)
            //{
            //    actionContext.ActionArguments["id"] = actionContext.ActionArguments["shift"];
            //}
        }
    }
}