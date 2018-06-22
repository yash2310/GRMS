using AMS.ApplicationCore.Entities.Security;
using AMS.ApplicationCore.Entities;
using AMS.Infrastructure.DatabaseHelper;
using NHibernate;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AMS.Infrastructure.Repository
{
	public class AccountRepository
	{
		private static ISession _session;
		public static bool Authenticate(string email, string pwd)
		{
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					var data = _session.CreateCriteria<Employee>()
						.List<Employee>()
						.Any(emp => emp.Email.Equals(email) && emp.Password.Equals(pwd));

					return data;
				}
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static Employee Login(string email, string pwd)
		{
			Employee employee = null;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					var data = _session.CreateCriteria<Employee>()
						.List<Employee>()
						.FirstOrDefault(emp => emp.Email.Equals(email) && emp.Password.Equals(pwd));

					if (data != null)
					{
						if (data.Department != null)
						{
							NHibernateUtil.Initialize(data.Department);
						}
						if (data.Designation != null)
						{
							NHibernateUtil.Initialize(data.Designation);
						}
						if (data.Organization != null)
						{
							NHibernateUtil.Initialize(data.Organization);
						}
						if (data.Roles != null && data.Roles.Count > 0)
						{
							NHibernateUtil.Initialize(data.Roles);
						}
					}

					employee = data ?? null;
				}
			}
			catch (Exception)
			{
				employee = null;
			}
			return employee;
		}

		public static bool ResetPassword(int id, string password)
		{
			bool result = false;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					using (var transaction = _session.BeginTransaction())
					{
						var data = _session.CreateCriteria<Employee>()
							.List<Employee>()
							.FirstOrDefault(emp => emp.Id.Equals(id));

						if (data != null)
						{
							data.Password = password;
							_session.Update(data);
							transaction.Commit();
							result = true;
						}
					}
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public static bool ForgetPassword(string email)
		{
			if (email.Length > 5)
			{
				
			}
			return true;
		}

		public static bool Registration(NewEmployee nemp)
		{
			bool result = false;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					List<Role> roles = new List<Role>();
					Role role = MasterRepository.GetRoleById(nemp.Role);
					roles.Add(role);
					Employee employee = new Employee
					{
						Name = nemp.Name,
						Email = nemp.Email,
						EmployeeNo = nemp.EmpNo,
						ContactNo = nemp.Contact,
						Password = nemp.Password,
						Roles = roles
					};

					using (var transaction = _session.BeginTransaction())
					{
						_session.Save(employee);
						transaction.Commit();
						result = true;
					}
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public static List<Setting> AppSetting()
		{
			List<Setting> setting = null;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					setting = _session.CreateCriteria<Setting>()
						.List<Setting>().ToList();
				}
			}
			catch (Exception)
			{
				return null;
			}
			return setting;
		}
	}

	public class NewEmployee
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string EmpNo { get; set; }
		public long Contact { get; set; }
		public int Role { get; set; }
		public string Password { get; set; }
	}
}