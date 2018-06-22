using System;
using System.ComponentModel.DataAnnotations;
using AMS.ApplicationCore.Entities.Security;

namespace AMS.ApplicationCore.Entities
{
    public class EmployeeGoal : Goals
    {
        [Required(ErrorMessage = "Employee Required")]
        public virtual Employee Employee { get; set; } // Employee Entity

        [Required(ErrorMessage = "Reviewer Required")]
        public virtual Employee Reviewer { get; set; } // Employee Entity for Reviewer
    }
}
