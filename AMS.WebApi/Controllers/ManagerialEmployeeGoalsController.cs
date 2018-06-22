namespace AMS.WebApi.Controllers
{
    #region Namespaces

    using AMS.ApplicationCore.Entities;
    using AMS.Infrastructure.Repository;
    using AMS.WebApi.ViewModels;
    using System;
	using System.Net.Http;
	using System.Web.Http;
    using System.Web.Http.Cors;

    #endregion

    //[Authorize(Roles = "Leadership Team,Super Admin")]
    [RoutePrefix("api/manager/goals")]
    public class ManagerialEmployeeGoalsController : ApiController
    {
        ///<summary>
        ///To get weightage for managerial goals
        ///</summary>
        [HttpGet]
        [Route("manager/{empId}/weightage")]
        public int GetWeightage(int empId )
		{
			if (Utilities.Common.GetSettingData().Goal.Equals(false))
				throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

			return GoalRepository<ManagerialEmployeeGoal>.GetWeightage(empId, "Managerial");
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

			return GoalRepository<ManagerialEmployeeGoal>.UpdateGoalsStatus(empId, "finalize");
	    }

	    ///<summary>
	    ///To remove managerial goals
	    ///</summary>
		[HttpPost]
		[Route("remove")]
	    public bool RemoveGoal([FromBody]ManagerialGoalData managerialGoal)
		{
			if (Utilities.Common.GetSettingData().Goal.Equals(false))
				throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

			ManagerialEmployeeGoal managerEmpGoal = new ManagerialEmployeeGoal();

			managerEmpGoal.Id = Convert.ToInt32(managerialGoal.Id);
			managerEmpGoal.Goal = managerialGoal.Title;
			managerEmpGoal.Weightage = managerialGoal.Weightage;
			managerEmpGoal.Description = managerialGoal.Description;
			managerEmpGoal.StartDate = Convert.ToDateTime(managerialGoal.StartDate);
			managerEmpGoal.EndDate = Convert.ToDateTime(managerialGoal.EndDate);
			managerEmpGoal.Cycle = MasterRepository.GetCycleByName(managerialGoal.ReviewCycle);
			managerEmpGoal.Status = MasterRepository.GetGoalStatusById(1); // For Draft Mode 
			managerEmpGoal.Employee = EmployeeRepository.GetEmployeeById(managerialGoal.Id);
			managerEmpGoal.CreatedOn = DateTime.Now;
			managerEmpGoal.CreatedBy = 1;
			managerEmpGoal.UpdatedOn = DateTime.Now;
			managerEmpGoal.UpdatedBy = 1;

			return GoalRepository<ManagerialEmployeeGoal>.RemoveGoal(managerEmpGoal);
	    }

	    ///<summary>
	    ///To update managerial goals
	    ///</summary>
		[HttpPut]
	    [Route("update")]
	    public bool UpdateGoal([FromBody]ManagerialGoalData managerialGoal)
		{
			if (Utilities.Common.GetSettingData().Goal.Equals(false))
				throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

			ManagerialEmployeeGoal managerEmpGoal = new ManagerialEmployeeGoal();

		    managerEmpGoal.Id = Convert.ToInt32(managerialGoal.Id);
			managerEmpGoal.Goal = managerialGoal.Title;
		    managerEmpGoal.Weightage = managerialGoal.Weightage;
		    managerEmpGoal.Description = managerialGoal.Description;
		    managerEmpGoal.StartDate = Convert.ToDateTime(managerialGoal.StartDate);
		    managerEmpGoal.EndDate = Convert.ToDateTime(managerialGoal.EndDate);
		    managerEmpGoal.Cycle = MasterRepository.GetCycleById(managerialGoal.ReviewCycle);
		    managerEmpGoal.Status = MasterRepository.GetGoalStatusById(1); // For Draft Mode 
		    managerEmpGoal.Employee = EmployeeRepository.GetEmployeeByEmailId(managerialGoal.EmployeeEmail);
		    managerEmpGoal.CreatedOn = DateTime.Now;
		    managerEmpGoal.CreatedBy = 1;
		    managerEmpGoal.UpdatedOn = DateTime.Now;
		    managerEmpGoal.UpdatedBy = 1;

		    return GoalRepository<ManagerialEmployeeGoal>.UpdateGoal(managerEmpGoal, null);
	    }

		///<summary>
		///To add a Managerial Employee goal
		///</summary>
		[HttpPost]
        [Route("add")]
        public bool Create([FromBody]ManagerialGoalData managerGoal)
		{
			if (Utilities.Common.GetSettingData().Goal.Equals(false))
				throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

			bool status = true;
			ManagerialEmployeeGoal managerEmpGoal = new ManagerialEmployeeGoal();

            managerEmpGoal.Goal = managerGoal.Title;
            managerEmpGoal.Weightage = managerGoal.Weightage;
            managerEmpGoal.Description = managerGoal.Description;
            managerEmpGoal.StartDate = Convert.ToDateTime(managerGoal.StartDate);
            managerEmpGoal.EndDate = Convert.ToDateTime(managerGoal.EndDate);
            managerEmpGoal.Cycle = MasterRepository.GetCycleById(managerGoal.ReviewCycle);
            managerEmpGoal.Status = MasterRepository.GetGoalStatusById(1); // For Draft Mode 
            managerEmpGoal.Employee = EmployeeRepository.GetEmployeeByEmailId(managerGoal.EmployeeEmail);
            managerEmpGoal.CreatedOn = DateTime.Now;
            managerEmpGoal.CreatedBy = 1;
            managerEmpGoal.UpdatedOn = DateTime.Now;
            managerEmpGoal.UpdatedBy = 1;

			try
			{
				status = GoalRepository<ManagerialEmployeeGoal>.AddItem(managerEmpGoal, 2);
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