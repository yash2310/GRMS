using AMS.ApplicationCore.Interfaces;
using AMS.Infrastructure.DatabaseHelper;
using NHibernate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AMS.ApplicationCore.Entities;
using AMS.ApplicationCore.Entities.Security;
using NHibernate.Util;

namespace AMS.Infrastructure.Repository
{
	public class GoalRepository<T> : IRepository<T>
	{
		private static ISession _session;

		public static bool AddItem(T item, int type)
		{
			ValidationContext context = new ValidationContext(item, null, null);
			List<ValidationResult> results = new List<ValidationResult>();
			bool valid = Validator.TryValidateObject(item, context, results, true);

			if (!valid)
			{
				throw new Exception(results.Count > 0 ? results[0].ToString() : "");
			}

			bool status = false;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					bool goalExist = false;

					List<EmployeeGoal> employeeGoal = null;
					ManagerialEmployeeGoal managerialGoal = null;
					if (type.Equals(1))
					{
						employeeGoal = item as List<EmployeeGoal>;

						goalExist = _session.CreateCriteria<EmployeeGoal>()
						.List<EmployeeGoal>()
						.Any(emp => emp.Goal.Trim().Equals(employeeGoal.FirstOrDefault().Goal.Trim()) && emp.Status.Id.Equals(1) && emp.Employee.Id.Equals(employeeGoal.FirstOrDefault().Employee.Id));// && emp.Reviewer.Equals(employeeGoal.Reviewer));
					}
					else if (type.Equals(2))
					{
						managerialGoal = item as ManagerialEmployeeGoal;
						goalExist = _session.CreateCriteria<ManagerialEmployeeGoal>()
						.List<ManagerialEmployeeGoal>()
						.Any(mgr => mgr.Goal.Trim().Equals(managerialGoal.Goal.Trim()) && mgr.Status.Id.Equals(1) && mgr.Employee.Id.Equals(managerialGoal.Employee.Id));
					}

					if (goalExist)
					{
						status = false;
						throw new Exception("goal");
					}

					using (var transaction = _session.BeginTransaction())
					{
						if (type.Equals(1))
						{
							foreach (EmployeeGoal egoal in employeeGoal)
								_session.Save(egoal);
						}
						else if (type.Equals(2))
						{
							_session.Save(managerialGoal);
						}

						transaction.Commit();
						status = true;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex.Message.Equals("goal"))
					throw new Exception("goal");

				status = false;
			}
			return status;
		}

		public static bool RemoveGoal(T item)
		{
			bool result = false;
			try
			{
				Type type = typeof(T);
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					using (var transaction = _session.BeginTransaction())
					{
						if (type.Name.Equals("EmployeeGoal"))
						{
							EmployeeGoal employeeGoal = item as EmployeeGoal;

							if (employeeGoal != null)
							{
								var empGoal = _session.CreateCriteria<Employee>().List<Employee>()
									.First(goal => goal.Email.Equals(employeeGoal.Employee.Email))
									.EmployeeGoals
									.Where(goal =>
										goal.Goal.Equals(employeeGoal.Goal) &&
										goal.Cycle.Id.Equals(employeeGoal.Cycle.Id) &&
										goal.Status.Id.Equals(employeeGoal.Status.Id));

								foreach (var goal in empGoal)
								{
									_session.Delete(goal);
								}
							}
						}
						else if (type.Name.Equals("ManagerialEmployeeGoal"))
						{
							ManagerialEmployeeGoal managerialGoal = item as ManagerialEmployeeGoal;

							if (managerialGoal != null)
							{
								var mngrGoal = _session.Get<ManagerialEmployeeGoal>(managerialGoal.Id);
								_session.Delete(mngrGoal);
							}
						}
						transaction.Commit();
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				result = false;
			}
			return result;
		}

		public static bool UpdateGoal(T item, List<int> reviewers)
		{
			bool result = false;
			try
			{
				Type type = typeof(T);
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					using (var transaction = _session.BeginTransaction())
					{
						if (type.Name.Equals("EmployeeGoal"))
						{
							EmployeeGoal employeeGoal = item as EmployeeGoal;

							if (employeeGoal != null)
							{

								if (reviewers.Count <= 0)
									return false;

								#region Delete Goal

								var empGoal = _session.CreateCriteria<Employee>().List<Employee>()
									.First(goal => goal.Email.Equals(employeeGoal.Employee.Email))
									.EmployeeGoals
									.Where(goal =>
										goal.Goal.Equals(employeeGoal.Goal) &&
										goal.Cycle.Id.Equals(employeeGoal.Cycle.Id) &&
										goal.Status.Id.Equals(employeeGoal.Status.Id));

								foreach (EmployeeGoal goal in empGoal)
									_session.Delete(goal);

								#endregion

								#region Insert Goal for update

								foreach (var reviewer in reviewers)
								{
									employeeGoal.Reviewer = EmployeeRepository.GetEmployeeById(reviewer);

									ValidationContext context = new ValidationContext(employeeGoal, null, null);
									List<ValidationResult> results = new List<ValidationResult>();
									bool valid = Validator.TryValidateObject(employeeGoal, context, results, true);

									if (!valid)
										throw new Exception(results.Count > 0 ? results[0].ToString() : "");

									_session.Save(employeeGoal);
									_session.Flush();
									_session.Clear();
								}

								#endregion
							}
						}
						else if (type.Name.Equals("ManagerialEmployeeGoal"))
						{
							ManagerialEmployeeGoal managerialGoal = item as ManagerialEmployeeGoal;

							if (managerialGoal != null)
							{
								#region Delete Goal

								var mngrGoal = _session.CreateCriteria<Employee>().List<Employee>()
									.First(goal => goal.Email.Equals(managerialGoal.Employee.Email))
									.ManagerialEmployeeGoals
									.First(goal =>
										goal.Id.Equals(managerialGoal.Id));
								_session.Delete(mngrGoal);

								#endregion

								#region Insert Goal for update

								ValidationContext context = new ValidationContext(managerialGoal, null, null);
								List<ValidationResult> results = new List<ValidationResult>();
								bool valid = Validator.TryValidateObject(managerialGoal, context, results, true);

								if (!valid)
									throw new Exception(results.Count > 0 ? results[0].ToString() : "");

								_session.Save(managerialGoal);

								#endregion
							}
						}

						transaction.Commit();
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				result = false;
			}
			return result;
		}

		public static bool UpdateGoalsStatus(int empId, string statusType)
		{
			bool result = false;
			int status = 0;

			try
			{
				Type type = typeof(T);

				if (statusType.ToLower().Equals("finalize"))
				{
					status = 2;
				}
				else if (statusType.ToLower().Equals("reviewed"))
				{
					status = 3;
				}

				using (_session = AmsDatabaseHelper.Create().Session)
				{
					using (var transaction = _session.BeginTransaction())
					{
						Employee employee = _session.CreateCriteria<Employee>()
							.List<Employee>()
							.FirstOrDefault(emp => emp.Id.Equals(empId));

						if (employee != null && type.Name.Equals("EmployeeGoal"))
						{
							NHibernateUtil.Initialize(employee.EmployeeGoals);

							if (employee.EmployeeGoals.Count > 0)
							{
								List<EmployeeGoal> employeeGoals = employee.EmployeeGoals
									.Where(goal => goal.Status.Equals(_session.Get<GoalStatus>(1))).ToList();

								foreach (var employeeGoal in employeeGoals)
								{
									employeeGoal.Status = _session.Get<GoalStatus>(status);
									_session.Update(employeeGoal);
								}
							}
						}
						else if (employee != null && type.Name.Equals("ManagerialEmployeeGoal"))
						{
							NHibernateUtil.Initialize(employee.ManagerialEmployeeGoals);

							List<ManagerialEmployeeGoal> managerialGoals = employee.ManagerialEmployeeGoals
								.Where(goal => goal.Status.Equals(_session.Get<GoalStatus>(1))).ToList();

							foreach (var managerialGoal in managerialGoals)
							{
								managerialGoal.Status = _session.Get<GoalStatus>(status);
								_session.Update(managerialGoal);
							}
						}
						else if (employee != null && type.Name.Equals("OrganizationGoal"))
						{
							//OrganizationGoal organizationGoal = item as OrganizationGoal;
							//if (organizationGoal != null)
							//{
							//	organizationGoal.Status = _session.Get<GoalStatus>(status);
							//	_session.Update(organizationGoal);
							//}
						}
						else if (employee != null && type.Name.Equals("DesignationGoal"))
						{
							//DesignationGoal designationGoal = item as DesignationGoal;
							//if (designationGoal != null)
							//{
							//	designationGoal.Status = _session.Get<GoalStatus>(status);
							//	_session.Update(designationGoal);
							//}
						}
						transaction.Commit();
						result = true;
					}
				}
			}
			catch (Exception e)
			{
				var message = e.Message;
				result = false;
			}
			return result;
		}

		public static int GetWeightage(int empId, string goalType)
		{
			int totalWeight = 0;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					if (goalType.Trim().ToLower().Equals("employee"))
					{
						var firstOrDefault = _session.CreateCriteria<Employee>()
							.List<Employee>()
							.FirstOrDefault(emp => emp.Id.Equals(empId));

						totalWeight = firstOrDefault != null
							? Convert.ToInt32(firstOrDefault.EmployeeGoals
								.Where(goal => goal.Status.Id.Equals(1))
								.Select(goals => new {Goal = goals.Goal, Weight = goals.Weightage})
								.GroupBy(goals => goals.Goal)
								.Select(goals => goals.Average(g => g.Weight))
								.Sum())
							: 0;
					}
					else if (goalType.Trim().ToLower().Equals("managerial"))
					{
						var firstOrDefault = _session.CreateCriteria<Employee>()
							.List<Employee>()
							.FirstOrDefault(emp => emp.Id.Equals(empId));

						totalWeight = firstOrDefault != null
							? Convert.ToInt32(firstOrDefault.ManagerialEmployeeGoals
								.Where(goal => goal.Status.Id.Equals(1))
								.Sum(goals => goals.Weightage))
							: 0;
					}
					else if (goalType.Trim().ToLower().Equals("organizational"))
					{
						var firstOrDefault = _session.CreateCriteria<Organization>()
							.List<Organization>()
							.FirstOrDefault(emp => emp.Id.Equals(empId));

						totalWeight = firstOrDefault != null
							? Convert.ToInt32(firstOrDefault.OrganizationGoals
								.Where(goal => goal.Status.Id.Equals(1))
								.Select(goals => new {Goal = goals.Goal, Weight = goals.Weightage})
								.GroupBy(goals => goals.Goal)
								.Select(goals => goals.Average(g => g.Weight))
								.Sum())
							: 0;
					}
					else if (goalType.Trim().ToLower().Equals("designation"))
					{
						var firstOrDefault = _session.CreateCriteria<Designation>()
							.List<Designation>()
							.FirstOrDefault(emp => emp.Id.Equals(empId));

						totalWeight = firstOrDefault != null
							? Convert.ToInt32(firstOrDefault.DesignationGoals
								.Where(goal => goal.Status.Id.Equals(1))
								.Select(goals => new {Goal = goals.Goal, Weight = goals.Weightage})
								.GroupBy(goals => goals.Goal)
								.Select(goals => goals.Average(g => g.Weight))
								.Sum())
							: 0;
					}
				}
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}

			return totalWeight;
		}
	}
}