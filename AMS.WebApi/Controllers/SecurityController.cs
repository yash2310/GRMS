using System.Text;
using AMS.WebApi.Authenticate;

namespace AMS.WebApi.Controllers
{
	#region Namespaces

	using AMS.ApplicationCore.Entities.Security;
	using AMS.ApplicationCore.Entities;
	using AMS.ApplicationCore.Utilities;
	using AMS.ApplicationCore.Utilities.Encryption;
	using AMS.Infrastructure.Repository;
	using AMS.WebApi.Models;
	using AMS.WebApi.ViewModels;
	using System;
	using System.Collections.Generic;
	using System.IdentityModel.Tokens.Jwt;
	using System.Net;
	using System.Net.Http;
	using System.Security.Claims;
	using System.Threading;
	using System.Web.Http;
	using System.Linq;

	#endregion

	[RoutePrefix("api/security")]
	public class SecurityController : ApiController
	{
		[HttpPost]
		[Route("register")]
		public IHttpActionResult Registration(NewEmployee newEmployee)
		{
			try
			{
				if (AccountRepository.Registration(newEmployee))
				{
					return Json(new {Status = HttpStatusCode.OK, Message = "Success"});
				}
				else
				{
					return Json(new {Status = HttpStatusCode.NotFound, Message = "Failed"});
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return Json(new {Status = HttpStatusCode.NotFound, Message = "Failed"});
			}
		}

		[HttpGet]
		[Route("login")]
		public IHttpActionResult Login(string email, string pwd)
		{
			try
			{
				Employee employee = AccountRepository.Login(email, pwd);
				if (employee != null)
				{
					EmployeeData data = new EmployeeData();

					data.Id = employee.Id;
					data.Name = employee.Name;
					data.EmailId = employee.Email;
					data.EmployeeNo = employee.EmployeeNo;
					data.ContactNo = employee.ContactNo;
					data.ImageUrl = employee.ImageUrl;

					if (employee.ReportingManager != null)
					{
						data.Manager = new Data {Id = employee.ReportingManager.Id, Name = employee.ReportingManager.Name};
					}

					if (employee.Department != null)
					{
						data.Department = new Data {Id = employee.Department.Id, Name = employee.Department.Name};
					}

					if (employee.Designation != null)
					{
						data.Designation = new Data {Id = employee.Designation.Id, Name = employee.Designation.Name};
					}

					if (employee.Organization != null)
					{
						data.Organization = new Data {Id = employee.Organization.Id, Name = employee.Organization.Name};
					}

					if (employee.Roles != null && employee.Roles.Count > 0)
					{
						data.Roles = new Data {Id = employee.Roles.First().Id, Name = employee.Roles.First().Name};
					}

					var cycle = MasterRepository.GetCurrentCycle();
					if (cycle != null)
					{
						data.ReviewCycle = new Data {Id = cycle.Id, Name = cycle.Name};
					}

					return Json(new
					{
						Status = HttpStatusCode.OK,
						Message = "Success",
						Employee = data,
						Token = Convert.ToBase64String(Encoding.UTF8.GetBytes(email + "|" + pwd + "|" + DateTime.Now))
					});
				}
				else
				{
					return Json(new {Status = HttpStatusCode.NotFound, Message = "Failed"});
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return Json(new {Status = HttpStatusCode.NotFound, Message = "Failed"});
			}
		}

		[HttpGet]
		[BasicAuthentication]
		[Route("reset")]
		public IHttpActionResult Reset(int id, string pwd)
		{
			try
			{
				bool status = AccountRepository.ResetPassword(id, pwd);
				if (status == true)
				{
					return Json(new {Status = HttpStatusCode.OK, Message = "Success"});
				}
				else
				{
					return Json(new {Status = HttpStatusCode.NotFound, Message = "Failed"});
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return Json(new {Status = HttpStatusCode.NotFound, Message = "Failed"});
			}
		}

		[HttpGet]
		[Route("forget")]
		public IHttpActionResult Forget(string email)
		{
			try
			{
				bool status = AccountRepository.ForgetPassword(email);
				if (status == true)
				{
					return Json(new {Status = HttpStatusCode.OK, Message = "Success"});
				}
				else
				{
					return Json(new {Status = HttpStatusCode.NotFound, Message = "Failed"});
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return Json(new {Status = HttpStatusCode.NotFound, Message = "Failed"});
			}
		}

		[HttpGet]
		[Route("role")]
		public IHttpActionResult Roles()
		{
			try
			{
				List<Data> roles = MasterRepository.GetAllRole().Select(rl => new Data {Id = rl.Id, Name = rl.Name}).ToList();
				if (roles.Count >= 0)
				{
					return Json(new {Status = HttpStatusCode.OK, Message = "Success", Roles = roles});
				}
				else
				{
					return Json(new {Status = HttpStatusCode.NotFound, Message = "Failed"});
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				return Json(new {Status = HttpStatusCode.NotFound, Message = "Failed"});
			}
		}

		[HttpGet]
		[Route("setting")]
		public SettingData GetSetting()
		{
			try
			{
				return Utilities.Common.GetSettingData();
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}