using System.Collections.Generic;

namespace AMS.WebApi.Controllers
{
	#region Namespaces

	using AMS.ApplicationCore.Entities;
	using AMS.ApplicationCore.Entities.Security;
	using AMS.Infrastructure.Repository;
	using AMS.WebApi.ViewModels;
	using System;
	using System.Net.Http;
	using System.Web.Http;
	using System.Web.Http.Cors;

    #endregion

    //[Authorize(Roles = "Leadership Team,Super Admin,Manager")]
    [RoutePrefix("api/employee/goals")]
	public class EmployeeGoalsController : ApiController
	{
		///<summary>
		///To get weightage for employee goals
		///</summary>
		[HttpGet]
		[Route("employee/{empId}/weightage")]
		public int GetWeightage(int empId)
		{
			if (Utilities.Common.GetSettingData().Goal.Equals(false))
				throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

			return GoalRepository<EmployeeGoal>.GetWeightage(empId, "Employee");
		}

		///<summary>
		///To finalize managerial goals
		///</summary>
		[HttpGet]
		[Route("{empId}/finalizegoal")]
		public bool finalizeGoalStatus(int empId)
		{
			if (Utilities.Common.GetSettingData().Goal.Equals(false))
				throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

			return GoalRepository<EmployeeGoal>.UpdateGoalsStatus(empId, "finalize");
		}

		///<summary>
		///To remove managerial goals
		///</summary>
		[HttpPost]
		[Route("remove")]
		public bool RemoveGoal([FromBody] EmployeeGoalData employeeGoal)
		{
			if (Utilities.Common.GetSettingData().Goal.Equals(false))
				throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

			EmployeeGoal empGoal = new EmployeeGoal();

			empGoal.Goal = employeeGoal.Title;
			empGoal.Weightage = employeeGoal.Weightage;
			empGoal.Description = employeeGoal.Description;
			empGoal.StartDate = Convert.ToDateTime(employeeGoal.StartDate);
			empGoal.EndDate = Convert.ToDateTime(employeeGoal.EndDate);
			empGoal.Cycle = MasterRepository.GetCycleById(employeeGoal.ReviewCycle);
			empGoal.Status = MasterRepository.GetGoalStatusById(1); // For Draft Mode 
			empGoal.Employee = EmployeeRepository.GetEmployeeByEmailId(employeeGoal.EmployeeEmail);
			empGoal.CreatedOn = DateTime.Now;
			empGoal.CreatedBy = 1;
			empGoal.UpdatedOn = DateTime.Now;
			empGoal.UpdatedBy = 1;

			return GoalRepository<EmployeeGoal>.RemoveGoal(empGoal);
		}

		///<summary>
		///To update managerial goals
		///</summary>
		[HttpPut]
		[Route("update")]
		public bool UpdateGoal([FromBody] EmployeeGoalData employeeGoal)
		{
			if (Utilities.Common.GetSettingData().Goal.Equals(false))
				throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

			try
			{
                EmployeeGoal empGoal = new EmployeeGoal();

                empGoal.Goal = employeeGoal.Title;
                empGoal.Weightage = employeeGoal.Weightage;
                empGoal.Description = employeeGoal.Description;
                empGoal.StartDate = Convert.ToDateTime(employeeGoal.StartDate);
                empGoal.EndDate = Convert.ToDateTime(employeeGoal.EndDate);
                empGoal.Cycle = MasterRepository.GetCycleById(employeeGoal.ReviewCycle);
                empGoal.Status = MasterRepository.GetGoalStatusById(1); // For Draft Mode 
                empGoal.Employee = EmployeeRepository.GetEmployeeByEmailId(employeeGoal.EmployeeEmail);
                empGoal.CreatedOn = DateTime.Now;
                empGoal.CreatedBy = 1;
                empGoal.UpdatedOn = DateTime.Now;
                empGoal.UpdatedBy = 1;

                List<int> reviewerList = new List<int>();
                foreach (var reviewer in employeeGoal.ReviewerList)
                {
                    reviewerList.Add(reviewer.Id);
                }

                return GoalRepository<EmployeeGoal>.UpdateGoal(empGoal, reviewerList);
            }
            catch (Exception ex)
            {
                return false;
            }
			
		}

		///<summary>
		///To add an Employee goal
		///</summary>
		[HttpPost]
		[Route("add")]
		public bool Create([FromBody] EmployeeGoalData employeeGoal)
		{
			if (Utilities.Common.GetSettingData().Goal.Equals(false))
				throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

			bool status = true;
			List<EmployeeGoal> employeeGoals = new List<EmployeeGoal>();

			if (employeeGoal.ReviewerList.Count <= 0)
			{
				return false;
			}

			foreach (var reviewer in employeeGoal.ReviewerList)
			{
				EmployeeGoal empGoal = new EmployeeGoal();
				empGoal.Goal = employeeGoal.Title;
				empGoal.Weightage = employeeGoal.Weightage;
				empGoal.Description = employeeGoal.Description;
				empGoal.StartDate = Convert.ToDateTime(employeeGoal.StartDate);
				empGoal.EndDate = Convert.ToDateTime(employeeGoal.EndDate);
				empGoal.Cycle = MasterRepository.GetCycleById(employeeGoal.ReviewCycle);
				empGoal.Status = MasterRepository.GetGoalStatusById(1); // For Draft Mode 
				empGoal.Employee = EmployeeRepository.GetEmployeeByEmailId(employeeGoal.EmployeeEmail);
				empGoal.CreatedOn = DateTime.Now;
				empGoal.CreatedBy = 1;
				empGoal.UpdatedOn = DateTime.Now;
				empGoal.UpdatedBy = 1;
				empGoal.Reviewer = EmployeeRepository.GetEmployeeById(reviewer.Id);

				employeeGoals.Add(empGoal);
			}

			try
			{
				status = GoalRepository<List<EmployeeGoal>>.AddItem(employeeGoals, 1);
			}
			catch (Exception ex)
			{
				if (ex.Message.Equals("goal"))
				{
					HttpResponseMessage message = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
					message.Content = new StringContent("Invalid Goal");
					throw new HttpResponseException(message);
				}
				status = false;
			}

			return status;
		}

	}
}