using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AMS.ApplicationCore.Entities.Security
{
    public class Employee : BaseEntity
    {
        [Required(ErrorMessage = "Emplouee Name Required")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "Email Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email")]
        public virtual string Email { get; set; }

        [Required(ErrorMessage = "Employee Number Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Employee Number")]
        public virtual string EmployeeNo { get; set; }

        [Required(ErrorMessage = "ContactNo Required")]
        public virtual long ContactNo { get; set; }

        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password, ErrorMessage = "Invalid Password")]
        public virtual string Password { get; set; }

	    [Required(ErrorMessage = "Image URL Required")]
	    public virtual string ImageUrl { get; set; }

		[Required(ErrorMessage = "Reporting Manager Required")]
        public virtual Employee ReportingManager { get; set; }

        public virtual Designation Designation { get; set; } // Designation Entity

        public virtual Department Department { get; set; } // Department Entity

        public virtual Organization Organization { get; set; } // Organization Entity

        public virtual IList<Role> Roles { get; set; } // List Role Entity
		public virtual ICollection<OrganizationGoal> OrganizationGoals { get; set; }
		public virtual ICollection<DesignationGoal> DesignationGoals { get; set; }
		public virtual ICollection<ManagerialEmployeeGoal> ManagerialEmployeeGoals { get; set; }
		public virtual ICollection<EmployeeGoal> EmployeeGoals { get; set; }
	}

   
}