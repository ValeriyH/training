using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace WebAPISelfHost.Handlers
{
    //Authentication base on userid. Don't block the request if no userid.
    //But Controller/Action with [Authorize] will not be processed
    class AuthenticationHandler : DelegatingHandler
    {
        private void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

        protected async override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response;

            Debug.WriteLine("Process request");

            var queryStrings = request.GetQueryNameValuePairs();
            //if (queryStrings == null)
            //    return null

            string userid = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, "userid", true) == 0).Value;
            if (!string.IsNullOrEmpty(userid))
            {
                GenericIdentity indentity = new GenericIdentity(userid);
                String[] indentity_roles = { "Manager", "Teller" };
                GenericPrincipal principal = new GenericPrincipal(indentity, indentity_roles);
                SetPrincipal(principal);
            }
            // Call the inner handler.
            response = await base.SendAsync(request, cancellationToken);

            Debug.WriteLine("Process response");
            return response;
        }
    }
}
