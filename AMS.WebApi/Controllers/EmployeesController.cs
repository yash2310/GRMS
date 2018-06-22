namespace AMS.WebApi.Controllers
{
    #region Namespaces

    using AMS.ApplicationCore.Entities;
    using AMS.ApplicationCore.Entities.Security;
    using AMS.Infrastructure.Repository;
    using AMS.WebApi.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    #endregion

    //[Authorize]
    [RoutePrefix("api/employees")]
    public class EmployeesController : ApiController
    {
        ///<summary>
        ///To get all Employees
        ///</summary>
        [HttpGet]
        public List<Employee> GetAll()
        {
            List<Employee> employeeList = new List<Employee>();

            IList<Role> roleList = new List<Role>();
            Role role = new Role();
            roleList.Add(role);

            string empName = "Manoj";

            Employee emp = new Employee
            {
                Name = empName,
                Email = empName + "@gmail.com",
                ContactNo = 9015368897,
                Password = "abc@123",
                ReportingManager = new Employee() { Name = "Anshu" },
                Designation = new Designation() { Name = "Manager" },
                Department = new Department() { Name = "Networking" },
                Organization = new Organization() { Name = "Bluepi" },
                Roles = roleList,
                ManagerialEmployeeGoals = null,
                EmployeeGoals = null,

                CreatedOn = Convert.ToDateTime("20 Feb 2018"),
                CreatedBy = 1,
                UpdatedOn = Convert.ToDateTime("20 Feb 2018"),
                UpdatedBy = 2
            };

            employeeList.Add(emp);

            return employeeList;
        }

        ///<summary>
        ///To get an employee by Name
        ///</summary>
        [HttpGet]
        [Route("employees/{name}")]
        public Employee GetByName(string name)
        {
            IList<Role> roleList = new List<Role>();
            Role role = new Role();
            roleList.Add(role);

            string empName = "Manoj";

            return new Employee
            {
                Name = empName,
                Email = empName + "@gmail.com",
                ContactNo = 9015368897,
                Password = "abc@123",
                ReportingManager = new Employee() { Name = "Anshu" },
                Designation = new Designation() { Name = "Manager" },
                Department = new Department() { Name = "Networking" },
                Organization = new Organization() { Name = "Bluepi" },
                Roles = roleList,
                ManagerialEmployeeGoals = null,
                EmployeeGoals = null,

                CreatedOn = Convert.ToDateTime("20 Feb 2018"),
                CreatedBy = 1,
                UpdatedOn = Convert.ToDateTime("20 Feb 2018"),
                UpdatedBy = 2
            };
        }

        ///<summary>
        ///To get all Goals by employee email id
        ///</summary>
        [HttpGet]
        [Route("employees/{empId}/goals")]
        public GoalData GetGoalsByEmailId(int empId)
        {
            GoalData goalsData = new GoalData();
            var allGoalsList = EmployeeRepository.GetAllGoalsByUser(empId);

            if (allGoalsList != null)
            {
                foreach (var goal in (List<OrganizationGoal>)allGoalsList[0])
                {
                    OrganizationGoalData orgData = new OrganizationGoalData();
                    orgData.Id = goal.Id;
                    orgData.Title = goal.Goal;
                    orgData.Type = "Organization";
                    orgData.StartDate = goal.StartDate.ToShortDateString();
                    orgData.EndDate = goal.EndDate.ToShortDateString();
                    orgData.Description = goal.Description;
                    orgData.Weightage = goal.Weightage;

                    goalsData.OrganizationGoalList.Add(orgData);
                }

                List<EmployeeGoal> goals = (List<EmployeeGoal>)allGoalsList[1];
                var groupedGoalsList = goals.GroupBy(u => u.Goal).Select(grp => grp.ToList()).ToList();

                foreach (var goal in groupedGoalsList)
                {
                    EmployeeGoalData empData = new EmployeeGoalData();
                    List<ReviewerData> reviewDataList = new List<ReviewerData>();

                    EmployeeGoal currGoal = goal[0];
                    empData.Id = currGoal.Id;
                    empData.Title = currGoal.Goal;
                    empData.Type = "Employee";
                    empData.StartDate = currGoal.StartDate.ToShortDateString();
                    empData.EndDate = currGoal.EndDate.ToShortDateString();
                    empData.Description = currGoal.Description;
                    empData.Weightage = currGoal.Weightage;
	                empData.Status = currGoal.Status.Id;

					//foreach (var g in goal.Select(x=>x.Reviewer != ))
                        foreach (var g in goal)
                            reviewDataList.Add(new ReviewerData() { Id = g.Reviewer.Id, Name = g.Reviewer.Name,EmailId=g.Reviewer.Email,EmployeeNo=g.Reviewer.EmployeeNo });

                    empData.ReviewerList = reviewDataList;

                    goalsData.EmployeeGoalList.Add(empData);
                }

                foreach (var goal in (List<ManagerialEmployeeGoal>)allGoalsList[2])
                {
                    ManagerialGoalData managerData = new ManagerialGoalData();
                    managerData.Id = goal.Id;
                    managerData.Title = goal.Goal;
                    managerData.Type = "Managerial";
                    managerData.StartDate = goal.StartDate.ToShortDateString();
                    managerData.EndDate = goal.EndDate.ToShortDateString();
                    managerData.Description = goal.Description;
                    managerData.Weightage = goal.Weightage;
	                managerData.Status = goal.Status.Id;

					goalsData.ManagerialGoalList.Add(managerData);
                }

                foreach (var goal in (List<DesignationGoal>)allGoalsList[3])
                {
                    DesignationGoalData desData = new DesignationGoalData();
                    desData.Id = goal.Id;
                    desData.Title = goal.Goal;
                    desData.Type = "Designation";
                    desData.StartDate = goal.StartDate.ToShortDateString();
                    desData.EndDate = goal.EndDate.ToShortDateString();
                    desData.Description = goal.Description;
                    desData.Weightage = goal.Weightage;

                    goalsData.DesignationGoalList.Add(desData);
                }
            }

            return goalsData;
        }

        ///<summary>
        ///To get all reviewers by email id
        ///</summary>
        [HttpGet]
        [Route("employees/{empId}/reviewers")]
        public List<ReviewerData> GetReviewersByEmployeeId(int empId)
		{
			if (Utilities.Common.GetSettingData().Goal.Equals(false))
				throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

			List<ReviewerData> reviewersList = new List<ReviewerData>();

            var reviewers =  EmployeeRepository.GetAllReviewerById(empId);

            if (reviewers != null)
            {
                foreach (var item in reviewers)
                {
                    ReviewerData reviewer = new ReviewerData();
	                reviewer.Id = item.Id;
                    reviewer.Name = item.Name;
                    reviewer.EmailId = item.Email;
                    reviewer.EmployeeNo = item.EmployeeNo;

                    reviewersList.Add(reviewer);
                }
            }

            return reviewersList;
        }
    }
}