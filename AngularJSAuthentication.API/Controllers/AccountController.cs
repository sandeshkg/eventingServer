using AngularJSAuthentication.API.Models;
using AngularJSAuthentication.API.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo = null;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public AccountController()
        {
            _repo = new AuthRepository();
        }


        [AllowAnonymous]
        [Route("Login")]
        [HttpGet]
        public async Task<IHttpActionResult> Login(string username, string password)
        {
            WebRequest request = WebRequest.Create(new Uri(String.Format("{0}token", Constants.BaseAddress)));
            request.Method = "POST";

            string postString = String.Format("username={0}&amp;password={1}&amp;grant_type=password", HttpUtility.HtmlEncode(username), HttpUtility.HtmlEncode(password));
            byte[] bytes = Encoding.UTF8.GetBytes(postString);
            using (Stream requestStream = await request.GetRequestStreamAsync())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)(await request.GetResponseAsync());
                string json;
                using (Stream responseStream = httpResponse.GetResponseStream())
                {
                    json = new StreamReader(responseStream).ReadToEnd();
                }
                TokenResponseModel tokenResponse = (TokenResponseModel)JsonConvert.DeserializeObject(json);
                return Ok(tokenResponse.AccessToken);
            }
            catch (Exception ex)
            {
                throw new SecurityException("Bad credentials", ex);
            }
        }

        

        

       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        
    }

    class TokenResponseModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("userName")]
        public string Username { get; set; }

        [JsonProperty(".issued")]
        public string IssuedAt { get; set; }

        [JsonProperty(".expires")]
        public string ExpiresAt { get; set; }
    }
    static class Constants
    {
        /// <summary>
        /// Update this to your own website server address
        /// </summary>
        public const string BaseAddress = "http://localhost:26264/";
    }
}
