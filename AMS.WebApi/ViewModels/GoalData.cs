using System.Collections.Generic;

namespace AMS.WebApi.ViewModels
{
    public class GoalData
    {
        public List<OrganizationGoalData> OrganizationGoalList { get; set; } = new List<OrganizationGoalData>();
        public List<EmployeeGoalData> EmployeeGoalList { get; set; } = new List<EmployeeGoalData>();
        public List<ManagerialGoalData> ManagerialGoalList { get; set; } = new List<ManagerialGoalData>();
        public List<DesignationGoalData> DesignationGoalList { get; set; } = new List<DesignationGoalData>();
    }
}