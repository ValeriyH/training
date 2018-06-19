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
    //Block any requests if userid doesn't set
    class BlockHandler : DelegatingHandler
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
            if (string.IsNullOrEmpty(userid))
            {
                //Don't process request if there is no userid
                var message = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Hello! userid is not set!")
                };

                response = message;
            }
            else
            {
                //Setup principal and go on
                GenericIdentity indentity = new GenericIdentity(userid);
                String[] indentity_roles = { "Manager", "Teller" };
                GenericPrincipal principal = new GenericPrincipal(indentity, indentity_roles);
                SetPrincipal(principal);

                // Call the inner handler.
                response = await base.SendAsync(request, cancellationToken);
            }

             Debug.WriteLine("Process response");
            return response;
        }
    }
}
