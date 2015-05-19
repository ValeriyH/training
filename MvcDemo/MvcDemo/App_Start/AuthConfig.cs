using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using Microsoft.Web.WebPages.OAuth;
using MvcDemo.Models;
using DotNetOpenAuth.AspNet;

namespace MvcDemo
{
    public class GoogleOAuthClient : IAuthenticationClient
    {
        public string appId = null;
        public string appSecret = null;
        public string redirectUri = null;
        public string state = null;

        class AccessToken
        {
            public string access_token = null;
            public string id_token = null; //user id token
            public string expires_in = null;
            public string token_type = null;
            public string refresh_token = null;
        }

        AccessToken token;

        class UserData
        {
            public string id = null;
            public string nickname = null;
            public string birthday = null;
            public string gender = null;
            public string displayName = null;
            public Object[] emails = null;
            public Object name = null;
        }

        public GoogleOAuthClient(string appId, string appSecret)
        {
            this.appId = appId;
            this.appSecret = appSecret;
        }

        string IAuthenticationClient.ProviderName 
        {
            get { return "google-oauth"; }
        }

        void IAuthenticationClient.RequestAuthentication(HttpContextBase context, Uri returnUrl)
        {
            redirectUri = returnUrl.GetLeftPart(UriPartial.Path);
            state = context.Server.UrlEncode(returnUrl.Query);

            var address = String.Format(
                "https://accounts.google.com/o/oauth2/auth?client_id={0}&response_type=code&scope=openid%20email&redirect_uri={1}&state={2}",
                this.appId,
                redirectUri,
                state
            );
 
            context.Response.Redirect(address, false);
        }

        AuthenticationResult IAuthenticationClient.VerifyAuthentication(HttpContextBase context)
        {
            try
            {
                if (context.Request["error"] != null)
                {
                    throw new Exception(context.Request["error"]);
                }

                string code = context.Request["code"];

                var response = ValidateCode(code);
                token = DeserializeJson<AccessToken>(response);

                UserData userInfo = GetUserProfile();

                return new AuthenticationResult(
                    true, (this as IAuthenticationClient).ProviderName, userInfo.id,
                    userInfo.displayName,
                    new Dictionary<string, string>());

            }
            catch (Exception ex)
            {
                return new AuthenticationResult(ex);
            }
        }

        private UserData GetUserProfile()
        {
            //string request = String.Format("https://www.googleapis.com/plus/v1/people/{0}?access_token={1}", token.id_token, token.access_token);
            string request = String.Format("https://www.googleapis.com/plus/v1/people/me?access_token={0}", token.access_token);
            string response = GetRequst(request);
            return DeserializeJson<UserData>(response);
        }

        public string ValidateCode(string code)
        {
            var request = WebRequest.Create("https://accounts.google.com/o/oauth2/token") as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            System.Collections.Specialized.NameValueCollection outgoingQueryString = HttpUtility.ParseQueryString(String.Empty);
            outgoingQueryString.Add("code", code);
            outgoingQueryString.Add("client_id", appId);
            outgoingQueryString.Add("client_secret", appSecret);
            outgoingQueryString.Add("redirect_uri", redirectUri);
            outgoingQueryString.Add("grant_type", "authorization_code");

            string postdata = outgoingQueryString.ToString();
            byte[] byteArray = Encoding.UTF8.GetBytes(postdata);
            request.ContentLength = byteArray.Length;

            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            string responseString = "";
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    responseString = reader.ReadToEnd();
                    reader.Close();
                    response.GetResponseStream().Close();
                    response.Close();
                }
            }

            return responseString;
        }

        public static T DeserializeJson<T>(string input)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            return serializer.Deserialize<T>(input);
        }
    
        public static string GetRequst(string address)
        {
            WebRequest request = WebRequest.Create(address);
            // If required by the server, set the credentials.
            //request.Credentials = CredentialCache.DefaultCredentials;

            //Add "Authorization" to header. It is required if not added to URL
            //Authorization: Bearer 1/fFBGRNJru1FQd44AzqT3Zg
            //request.Headers.Add("Authorization", String.Format("Bearer {0}", token.access_token));

            // Get the response.
            WebResponse response = request.GetResponse();
            // Get the stream containing content returned by the server.
            System.IO.Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Clean up the streams and the response.
            reader.Close();
            response.Close();
            return responseFromServer;
        }
    }

    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "1508771539375139",
                appSecret: "acbd9bcb6c1bbc36a1d08c5ed7546863");

            OAuthWebSecurity.RegisterGoogleClient();

            OAuthWebSecurity.RegisterClient(new GoogleOAuthClient(
                appId: "1039159049302-hm207nvje6ot6su0ugttbnqcmiv170qs.apps.googleusercontent.com",
                appSecret: "1vM0RKNXqeYQlcCNwu3-foS7"),
                displayName: "Google OAuth",
                extraData: null);
        }
    }
}
