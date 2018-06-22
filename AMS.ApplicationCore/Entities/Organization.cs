using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AMS.ApplicationCore.Entities.Security;

namespace AMS.ApplicationCore.Entities
{
    public class Organization : BaseEntity
    {
        [Required(ErrorMessage = "Organization Name Required")]
        public virtual string Name { get; set; }

        public virtual IList<OrganizationGoal> OrganizationGoals { get; set; }

        public virtual IList<Employee> Employees { get; set; }
    }
}
