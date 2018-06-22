using AMS.ApplicationCore.Entities;
using AMS.ApplicationCore.Entities.Security;
using AMS.ApplicationCore.Interfaces;
using AMS.Infrastructure.DatabaseHelper;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Infrastructure.Repository
{
	public class EmployeeRepository : IRepository<Employee>
	{
		private static ISession _session;

		#region Get employee by email id

		public static Employee GetEmployeeByEmailId(string email)
		{
			Employee employee = null;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					employee = _session.CreateCriteria<Employee>()
						.List<Employee>()
						.FirstOrDefault(emp => emp.Email.Equals(email));
				}
			}
			catch (Exception ex)
			{
				var errMessage = ex.Message;
				employee = null;
			}
			return employee;
		}

		#endregion

		#region Get employee by id

		public static Employee GetEmployeeById(int Id)
		{
			Employee employee = null;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					employee = _session.CreateCriteria<Employee>()
						.List<Employee>()
						.FirstOrDefault(emp => emp.Id.Equals(Id));
				}
			}
			catch (Exception ex)
			{
				var errMessage = ex.Message;
				employee = null;
			}
			return employee;
		}

		#endregion

		#region Get all reviewers for an employee

		public static List<Employee> GetAllReviewerById(int empId)
		{
			List<Employee> employeeList = new List<Employee>();
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					List<Employee> dataList = _session.CreateCriteria<Employee>()
						.List<Employee>().ToList();

					Employee employee = dataList.FirstOrDefault(emp => emp.Id.Equals(empId));
					if (employee != null)
					{
						NHibernateUtil.Initialize(employee.Roles);
						var employeeRole = employee.Roles.OrderBy(role => role.Id).FirstOrDefault();

						List<Role> roles = GetAllRoles();

						if (roles != null && employeeRole != null)
						{
							int[] roleArray = roles?.Where(rl => rl.Id <= employeeRole.Id).Select(r => r.Id).ToArray();

							foreach (var empl in dataList)
							{
								NHibernateUtil.Initialize(empl.Roles);
								var roleId = empl.Roles.OrderBy(r => r.Id).FirstOrDefault();

								if (roleId != null && roleArray.Contains(Convert.ToInt32(roleId.Id)) && empl.Id != employee.Id)
								{
									employeeList.Add(empl);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				var errMessage = ex.Message;
				employeeList = null;
			}
			return employeeList;
		}

		#endregion

		#region Get all direct reportees of an employee

		public static List<Employee> GetDirectReportee(int empId)
		{
			List<Employee> employeeList = new List<Employee>();
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					Employee reportingManager = _session.CreateCriteria<Employee>()
						.List<Employee>()
						.FirstOrDefault(emp => emp.Id.Equals(empId));

					if (reportingManager != null)
					{
						var employees = _session.CreateCriteria<Employee>()
							.List<Employee>()
							.Where(emp => emp.ReportingManager == reportingManager).ToList();

                        if (employees.Count > 0)
						{
							foreach (var employee in employees)
							{
                                if (employee.Roles.Where(x=>x.Name == "Leadership Team" ).Count() > 0)
                                    continue;

                                NHibernateUtil.Initialize(employee.Roles);
                                NHibernateUtil.Initialize(employee.Designation);
								NHibernateUtil.Initialize(employee.Organization.OrganizationGoals);
								NHibernateUtil.Initialize(employee.Designation.DesignationGoals);
								NHibernateUtil.Initialize(employee.EmployeeGoals);
								NHibernateUtil.Initialize(employee.ManagerialEmployeeGoals);


                                employee.EmployeeGoals = employee.EmployeeGoals.Where(g => g.Status.Id.Equals(1) || g.Status.Id.Equals(2))
                                    .GroupBy(p => p.Goal).Select(g => g.First()).ToList();

                                employee.ManagerialEmployeeGoals = employee.ManagerialEmployeeGoals.Where(g => g.Status.Id.Equals(1) || g.Status.Id.Equals(2))
                                    .GroupBy(p => p.Goal).Select(g => g.First()).ToList();

                                employee.DesignationGoals = employee.Designation.DesignationGoals.Where(g => g.Status.Id.Equals(1) || g.Status.Id.Equals(2))
									.ToList();
								employee.OrganizationGoals = employee.Organization.OrganizationGoals.Where(g => g.Status.Id.Equals(1) || g.Status.Id.Equals(2))
									.ToList();

								employeeList.Add(employee);
							}
						}
					}
				}

				//_session.Dispose();
			}
			catch (Exception)
			{
				employeeList = null;
				//_session.Dispose();
			}
			return employeeList;
		}

        #endregion

        #region Get all downline reportees of an employee

        static List<Employee> reporteeList;

        public static List<Employee> GetDownlineReportee(int empId)
        {
            try
            {
                reporteeList = new List<Employee>();
                using (_session = AmsDatabaseHelper.Create().Session)
                {
                    Employee reportingManager = _session.CreateCriteria<Employee>()
                        .List<Employee>()
                        .FirstOrDefault(emp => emp.Id.Equals(empId));

                    if (reportingManager != null)
                    {
                        var employees = _session.CreateCriteria<Employee>()
                            .List<Employee>()
                            .Where(emp => emp.ReportingManager == reportingManager).ToList();

                        if (employees.Count > 0)
                        {
                            foreach (var employee in employees)
                            {
                                if (employee.Roles.Where(x => x.Name == "Leadership Team").Count() > 0)
                                    continue;

                                Employee emp = GetEmployeeData(employee);
                                reporteeList.Add(emp);

                                Level1Employees(employee);
                            }
                        }
                        //foreach (var employee in employees)
                        //{
                        //    GetDownlineReportee(employee.Id);
                        //}
                    }
                }

                //_session.Dispose();
            }
            catch (Exception)
            {
                reporteeList = null;
                //_session.Dispose();
            }
            return reporteeList;
        }

        public static List<Employee> Level1Employees(Employee employee)
        {
            try
            {
                using (_session = AmsDatabaseHelper.Create().Session)
                {
                    Employee reportingManager = _session.CreateCriteria<Employee>()
                        .List<Employee>()
                        .FirstOrDefault(emp => emp.Id.Equals(employee.Id));

                    var employees = _session.CreateCriteria<Employee>()
                        .List<Employee>()
                        .Where(emp => emp.ReportingManager == reportingManager).ToList();

                    if(employees.Count <= 0)
                    {
                        return reporteeList;
                    }
                    else
                    {
                        foreach (var empl in employees)
                        {
                            Employee emp = GetEmployeeData(empl);
                            reporteeList.Add(emp);

                            Level1Employees(emp);
                        }
                        return reporteeList;
                    }
                }

                //_session.Dispose();
            }
            catch (Exception)
            {
                reporteeList = null;
                //_session.Dispose();
            }
            return reporteeList;
        }

        public static Employee GetEmployeeData(Employee employee)
        {
            NHibernateUtil.Initialize(employee.Roles);
            NHibernateUtil.Initialize(employee.Designation);
            NHibernateUtil.Initialize(employee.Organization.OrganizationGoals);
            NHibernateUtil.Initialize(employee.Designation.DesignationGoals);
            NHibernateUtil.Initialize(employee.EmployeeGoals);
            NHibernateUtil.Initialize(employee.ManagerialEmployeeGoals);

            employee.EmployeeGoals = employee.EmployeeGoals.Where(g => g.Status.Id.Equals(1) || g.Status.Id.Equals(2))
                .GroupBy(p => p.Goal).Select(g => g.First()).ToList();

            employee.ManagerialEmployeeGoals = employee.ManagerialEmployeeGoals.Where(g => g.Status.Id.Equals(1) || g.Status.Id.Equals(2))
                .GroupBy(p => p.Goal).Select(g => g.First()).ToList();

            employee.DesignationGoals = employee.Designation.DesignationGoals.Where(g => g.Status.Id.Equals(1) || g.Status.Id.Equals(2))
                .ToList();
            employee.OrganizationGoals = employee.Organization.OrganizationGoals.Where(g => g.Status.Id.Equals(1) || g.Status.Id.Equals(2))
                .ToList();

            return employee;
        }

        #endregion

        #region Get all goals of an employee

        public static ArrayList GetAllGoalsByUser(int empId)
		{
			ArrayList list = new ArrayList();

			List<OrganizationGoal> organizationGoals = new List<OrganizationGoal>();
			List<EmployeeGoal> employeeGoals = new List<EmployeeGoal>();
			List<ManagerialEmployeeGoal> managerialGoals = new List<ManagerialEmployeeGoal>();
			List<DesignationGoal> designationGoals = new List<DesignationGoal>();

			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					var data = _session.CreateCriteria<Employee>()
						.List<Employee>()
						.FirstOrDefault(emp => emp.Id.Equals(empId));

					if (data != null)
					{
						foreach (var organizationGoal in data.Organization.OrganizationGoals.OrderByDescending(x => x.Id).ToList())
						{
							NHibernateUtil.Initialize(organizationGoal.Cycle);
							organizationGoals.Add(organizationGoal);
						}

						foreach (var employeeGoal in data.EmployeeGoals.Where(e => e.Reviewer.Id != empId).OrderByDescending(x => x.Id).ToList())
						{
							NHibernateUtil.Initialize(employeeGoal.Cycle);
							employeeGoals.Add(employeeGoal);
						}

						foreach (var managerialGoal in data.ManagerialEmployeeGoals.OrderByDescending(x => x.Id).ToList())
						{
							NHibernateUtil.Initialize(managerialGoal.Cycle);
							managerialGoals.Add(managerialGoal);
						}

						foreach (var designationGoal in data.Designation.DesignationGoals.OrderByDescending(x => x.Id).ToList())
						{
							NHibernateUtil.Initialize(designationGoal.Cycle);
							designationGoals.Add(designationGoal);
						}

						list.Add(organizationGoals);
						list.Add(employeeGoals);
						list.Add(managerialGoals);
						list.Add(designationGoals);
					}
				}
			}
			catch (Exception ex)
			{
				list = null;
				_session.Close();
			}
			return list;
		}

		#endregion

		#region Get all roles

		public static List<Role> GetAllRoles()
		{
			List<Role> roles = new List<Role>();
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					roles = _session.CreateCriteria<Role>()
						.List<Role>().ToList();
				}
			}
			catch (Exception ex)
			{
				roles = null;
			}
			return roles;
		}

		#endregion

		#region Get all reviewee

		public static List<Employee> GetAllReviewee(int empId)
		{
			List<Employee> employeeList = new List<Employee>();
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					Employee reviewer = _session.CreateCriteria<Employee>()
						.List<Employee>()
						.FirstOrDefault(emp => emp.Id.Equals(empId));

					if (reviewer != null)
					{
//                        var employees = new List<Employee>();
                        var employees = _session.CreateCriteria<EmployeeGoal>()
                            .List<EmployeeGoal>()
                            .Where(emp => emp.Reviewer.Id == reviewer.Id && emp.Status.Id.Equals(2) && emp.Employee.Id != reviewer.Id).Select(r => r.Employee).Distinct().ToList();



                        var employees1 = _session.CreateCriteria<Employee>()
                            .List<Employee>()
                            .ToList();

                        foreach (var employee in employees1)
                        {
                            NHibernateUtil.Initialize(employee.ReportingManager);

                            if (employee.ReportingManager != null)
                            {
                                if (employee.ReportingManager.Id == reviewer.Id)
                                    employees.Add(employee);
                            }
                        }

                        if (employees.Distinct().ToList().Count > 0)
						{
							foreach (var employee in employees.Distinct().ToList())
							{
								NHibernateUtil.Initialize(employee.ReportingManager);
								NHibernateUtil.Initialize(employee.Roles);
								NHibernateUtil.Initialize(employee.Designation);
								NHibernateUtil.Initialize(employee.Organization);
								NHibernateUtil.Initialize(employee.EmployeeGoals);

								employee.EmployeeGoals = employee.EmployeeGoals.Where(g => g.Status.Id.Equals(1) || g.Status.Id.Equals(2))
									.GroupBy(p => p.Goal).Select(g => g.First()).ToList();
								
								employeeList.Add(employee);
							}
						}
					}
				}
			}
			catch (Exception e)
			{
				employeeList = null;
			}
			return employeeList;
		}

        #endregion

        #region Get employee by id

        public static List<Employee> GetAllEmployees()
        {
            List<Employee> employee = null;
            try
            {
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					employee = _session.CreateCriteria<Employee>()
					.List<Employee>().ToList();
					employee.ForEach(emp => NHibernateUtil.Initialize(emp.Roles));
					employee.ForEach(emp => NHibernateUtil.Initialize(emp.Designation));

					employee = employee.Where(emp => emp.Roles.Any(r => r.Id.Equals(4) || r.Id.Equals(3))).ToList();
				}
            }
            catch (Exception ex)
            {
                var errMessage = ex.Message;
                employee = null;
            }
            return employee;
        }

        #endregion
    }
}
