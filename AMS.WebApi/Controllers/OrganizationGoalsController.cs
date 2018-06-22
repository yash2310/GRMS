using AMS.ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace AMS.WebApi.Controllers
{
    [RoutePrefix("api/OrganizationGoals")]
    public class OrganizationGoalsController : ApiController
    {
        ///<summary>
        ///To get Organization Goal by goal id
        ///</summary>
        [HttpGet]
        [Route("Goals/{goalId}/Goals")]
        public OrganizationGoal GetGoalByGoalId(string goalId)
        {
            return new OrganizationGoal
            {
                Organization = new Organization() { Name = "Bluepi" },

                Goal = "To Fill Youtrack",
                Weightage = 30,
                Description = "To Fill Youtrack on weekly basis",
                Cycle = new ReviewCycle() { Name = "Q1" },
                Status = null,

                CreatedOn = Convert.ToDateTime("20 Feb 2018"),
                CreatedBy = 1,
                UpdatedOn = Convert.ToDateTime("20 Feb 2018"),
                UpdatedBy = 2
            };
        }

        ///<summary>
        ///To get all Organization Goals
        ///</summary>
        [HttpGet]
        [Route("Goals")]
        public List<OrganizationGoal> GetAllGoals()
        {
            List<OrganizationGoal> goalList = new List<OrganizationGoal>();
            OrganizationGoal goal =  new OrganizationGoal
            {
                Organization = new Organization() { Name = "Bluepi" },

                Goal = "To Fill Youtrack",
                Weightage = 30,
                Description = "To Fill Youtrack on weekly basis",
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
        ///To get Team Organization Goals
        ///</summary>
        [HttpGet]
        [Route("Managers/{managerId}/Goals")]
        public List<OrganizationGoal> Get(int managerId)
        {
            List<OrganizationGoal> goalList = new List<OrganizationGoal>();
            OrganizationGoal goal = new OrganizationGoal
            {
                Organization = new Organization() { Name = "Bluepi" },

                Goal = "To Fill Youtrack",
                Weightage = 30,
                Description = "To Fill Youtrack on weekly basis",
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
        ///To get my Organization Goals
        ///</summary>
        [HttpGet]
        [Route("Roles/{roleId}/Goals")]
        public List<OrganizationGoal> GetGoalsByRole(int roleId)
        {
            List<OrganizationGoal> goalList = new List<OrganizationGoal>();
            OrganizationGoal goal = new OrganizationGoal
            {
                Organization = new Organization() { Name = "Bluepi" },

                Goal = "To Fill Youtrack",
                Weightage = 30,
                Description = "To Fill Youtrack on weekly basis",
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
        ///To add an organization Goal
        ///</summary>
        [HttpPost]
        public string Create([FromBody]OrganizationGoal orgGoal)
        {
            return "Success";
        }

        ///<summary>
        ///To Update an Organization Goal
        ///</summary>
        [HttpPut]
        public string Update([FromBody]OrganizationGoal orgGoal)
        {
            return "Success";
        }

        ///<summary>
        ///To Delete an Organization Goal
        ///</summary>
        [HttpDelete]
        public string Delete(string id)
        {
            return "Success";
        }
    }
}