using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AMS.ApplicationCore.Entities;

namespace AMS.ApplicationCore.Entities.Security
{
    public class Role : BaseEntity
    {
        [Required(ErrorMessage = "Name Required")]
        public virtual string Name { get; set; }

        public virtual IList<Employee> Employees { get; set; }
    }
}
