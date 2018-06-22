using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMS.Infrastructure.Repository;

namespace AMS.WebApi.ViewModels
{
	public class EmployeeRating
	{
		public int Rater { get; set; }
		public int Ratee { get; set; }
		public int Designation { get; set; }
		public int Organization { get; set; }
		public int Cycle { get; set; }
	}

	public class EmployeeGoalRating
	{
		public List<RatingData> EmployeeGoalsRating { get; set; }
		public List<RatingData> DesignationGoalsRating { get; set; }
		public List<RatingData> OrganizationGoalsRating { get; set; }
	}

    public class OverallRatingData
    {
        public List<RatingData> EmployeeGoalsRating { get; set; }
        public List<RatingData> ManagerGoalsRating { get; set; }
        public List<RatingData> DesignationGoalsRating { get; set; }
        public List<RatingData> OrganizationGoalsRating { get; set; }
    }

    public class ReporteeRatingData
	{
		public int Rater { get; set; }
		public int Ratee { get; set; }
		public int TypeId { get; set; }
		public int RateId { get; set; }
		public int GoalId { get; set; }
		public int GoalType { get; set; }
		public float Rate { get; set; }
		public string Comment { get; set; }
	}


}