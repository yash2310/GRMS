using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AMS.ApplicationCore.Entities.Security;

namespace AMS.ApplicationCore.Entities
{
    public class Department : BaseEntity
    {
        [Required(ErrorMessage = "Department Name Required")]
        public virtual string Name { get; set; }

		public virtual IList<Employee> Employees { get; set; }
	}
}
