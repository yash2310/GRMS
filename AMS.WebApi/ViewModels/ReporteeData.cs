using System.Collections.Generic;

namespace AMS.WebApi.ViewModels
{
	public class ReporteeData
	{
		public string Name { get; set; }
		public string EmployeeNo { get; set; }
		public Data Designation { get; set; }
		public string EmailId { get; set; }
		public int GoalsCount { get; set; }
		public string GoalStatus { get; set; }
		public List<Data> Role { get; set; }
		public int ReporteeId { get; set; }
	}

	public class RevieweeData
	{
		public List<ReporteeData> Reportee { get; set; }
		public List<ReporteeData> Reviewee { get; set; }
        public List<ReporteeData> Employee { get; set; }
    }
}