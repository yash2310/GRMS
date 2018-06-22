using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AMS.ApplicationCore.Entities.Security;

namespace AMS.ApplicationCore.Entities
{
    public class Designation : BaseEntity
    {
        [Required(ErrorMessage = "Designation Name Required")]
        public virtual string Name { get; set; }

		public virtual IList<DesignationGoal> DesignationGoals { get; set; }

		public virtual IList<Employee> Employees { get; set; }
    }
}
