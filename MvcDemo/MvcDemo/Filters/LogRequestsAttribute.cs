using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace MvcDemo.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LogRequestsAttribute : ActionFilterAttribute
    {
        TextWriter output;

        public LogRequestsAttribute(string fileName = "reqests.log")
        {
            output = new StreamWriter(fileName, true);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            if (request == null || output == null)
            {
                return;
            }

            //output.WriteLine(filterContext.HttpContext.Request.Headers);

            output.WriteLine("{0} {1}", request.HttpMethod, request.RawUrl);
            foreach(string key in request.Headers.AllKeys)
            {
                output.Write("\t{0}=", key);
                string[] values = request.Headers.GetValues(key);
                foreach(string value in values)
                {
                    output.Write("{0};", value);
                }
                output.WriteLine();
            }
            output.Flush();
        }
    }
}