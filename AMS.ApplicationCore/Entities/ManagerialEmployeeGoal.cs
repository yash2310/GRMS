using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AMS.ApplicationCore.Entities.Security;

namespace AMS.ApplicationCore.Entities
{
    public class ManagerialEmployeeGoal : Goals
    {
        [Required(ErrorMessage = "Employee Required")]
        public virtual Employee Employee { get; set; } // Employee Entity

		//[Required(ErrorMessage = "Reviewer Required")]
		public virtual Employee Reviewer { get; set; } // List Employee Entity for Reviewer
    }
}
