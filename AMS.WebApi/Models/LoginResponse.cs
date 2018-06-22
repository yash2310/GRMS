using System.Net.Http;

namespace AMS.WebApi.Models
{
    public class LoginResponse
    {
        public LoginResponse()
        {

            this.token = "";
            this.responseMsg = new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.Unauthorized };
        }

        public string token { get; set; }
        public HttpResponseMessage responseMsg { get; set; }
    }
}