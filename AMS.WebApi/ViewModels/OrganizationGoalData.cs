using System.Collections.Generic;

namespace AMS.WebApi.ViewModels
{
    public class OrganizationGoalData
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
        public int Weightage { get; set; }
        public string EmployeeEmail { get; set; }
        public string ReviewCycle { get; set; }
    }
}