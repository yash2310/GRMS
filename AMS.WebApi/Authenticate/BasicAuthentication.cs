using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using AMS.Infrastructure.Repository;

namespace AMS.WebApi.Authenticate
{
	public class BasicAuthentication : AuthorizationFilterAttribute
	{
		public override void OnAuthorization(HttpActionContext actionContext)
		{
			HttpResponseMessage message = new HttpResponseMessage();
			if (actionContext.Request.Headers.Authorization == null)
			{
				message.StatusCode = HttpStatusCode.Unauthorized;
				message.ReasonPhrase = "Invalid Request";

				actionContext.Response = message;
			}
			else
			{
				string text =
					Encoding.UTF8.GetString(Convert.FromBase64String(actionContext.Request.Headers.Authorization.Parameter));

				string uname = text.Split('|')[0];
				string pwd = text.Split('|')[1];
				DateTime datetime = Convert.ToDateTime(text.Split('|')[2]);

				if ((DateTime.Now - datetime).Hours > 4)
				{
					message.StatusCode = HttpStatusCode.Unauthorized;
					message.ReasonPhrase = "Token Expired";

					actionContext.Response = message;
				}
				if (!AccountRepository.Authenticate(uname, pwd))
				{
					message.StatusCode = HttpStatusCode.Unauthorized;
					message.ReasonPhrase = "Unauthorized User";

					actionContext.Response = message;
				}
			}
			base.OnAuthorization(actionContext);
		}
	}
}