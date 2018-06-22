using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.ApplicationCore.Entities
{
	public class ReviewCycle : BaseEntity
	{
		[Required(ErrorMessage = "Cycle Name Required")]
		public virtual string Name { get; set; }

		[Required(ErrorMessage = "Valid From Required")]
		public virtual int StartMonth { get; set; }

		[Required(ErrorMessage = "Valid To Required")]
		public virtual int EndMonth { get; set; }

		[Required(ErrorMessage = "Status Required")]
		public virtual bool Status { get; set; }

		public virtual IList<OrganizationGoal> OrganizationGoals { get; set; }
		public virtual IList<DesignationGoal> DesignationGoals { get; set; }
		public virtual IList<ManagerialEmployeeGoal> ManagerialEmployeeGoals { get; set; }
		public virtual IList<EmployeeGoal> EmployeeGoals { get; set; }
	}
}
