using AMS.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace AMS.WebApi.Controllers
{
    [RoutePrefix("api/DesignationGoals")]
    public class DesignationGoalsController : ApiController
    {
        ///<summary>
        ///To get Designation Goal by goal id
        ///</summary>
        [HttpGet]
        [Route("Goals/{goalId}/Goals")]
        public DesignationGoal GetGoalByGoalId(string goalId)
        {
            return new DesignationGoal
            {
                Designation = new Designation() { Name = "Sw Engineer" },

                Goal = "To Track Employee Records",
                Weightage = 30,
                Description = "To Track Employee Records",
                Cycle = new ReviewCycle() { Name = "Q1" },
                Status = null,

                CreatedOn = Convert.ToDateTime("20 Feb 2018"),
                CreatedBy = 1,
                UpdatedOn = Convert.ToDateTime("20 Feb 2018"),
                UpdatedBy = 2
            };
        }

        ///<summary>
        ///To get my Designation Goals
        ///</summary>
        [HttpGet]
        [Route("Employees/{empId}/Goals")]
        public List<DesignationGoal> GetGoalsByEmployeeId(int empId)
        {
            List<DesignationGoal> goalList = new List<DesignationGoal>();
            DesignationGoal goal = new DesignationGoal
            {
                Designation = new Designation() { Name = "Sw Engineer" },

                Goal = "To Track Employee Records",
                Weightage = 30,
                Description = "To Track Employee Records",
                Cycle = new ReviewCycle() { Name = "Q1" },
                Status = null,

                CreatedOn = Convert.ToDateTime("20 Feb 2018"),
                CreatedBy = 1,
                UpdatedOn = Convert.ToDateTime("20 Feb 2018"),
                UpdatedBy = 2
            };

            goalList.Add(goal);

            return goalList;
        }

        ///<summary>
        ///To get Designation Goals of my Team
        ///</summary>
        [HttpGet]
        [Route("Managers/{managerId}/Goals")]
        public List<DesignationGoal> GetTeamGoals(int managerId)
        {
            List<DesignationGoal> goalList = new List<DesignationGoal>();
            DesignationGoal goal = new DesignationGoal
            {
                Designation = new Designation() { Name = "Sw Engineer" },

                Goal = "To Track Employee Records",
                Weightage = 30,
                Description = "To Track Employee Records",
                Cycle = new ReviewCycle() { Name = "Q1" },
                Status = null,

                CreatedOn = Convert.ToDateTime("20 Feb 2018"),
                CreatedBy = 1,
                UpdatedOn = Convert.ToDateTime("20 Feb 2018"),
                UpdatedBy = 2
            };

            goalList.Add(goal);

            return goalList;
        }

        ///<summary>
        ///To add a Designation Goal
        ///</summary>
        [HttpPost]
        [Route("Add")]
        public string Create([FromBody]DesignationGoal indGoal)
        {
            return "Success";
        }

        ///<summary>
        ///To Update a Designation Goal
        ///</summary>
        [HttpPut]
        public string Update([FromBody]DesignationGoal indGoal)
        {
            return "Success";
        }

        ///<summary>
        ///To Delete a Designation Goal
        ///</summary>
        [HttpDelete]
        public string Delete(string goalId)
        {
            return "Success";
        }
    }
}