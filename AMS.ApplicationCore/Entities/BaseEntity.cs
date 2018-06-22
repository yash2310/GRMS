using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.ApplicationCore.Entities
{
    public class BaseEntity
    {
        [Required]
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Date Required")]
        public virtual DateTime CreatedOn { get; set; }

        [Required(ErrorMessage = "Employee Id Required")]
        public virtual int CreatedBy { get; set; }

        public virtual DateTime? UpdatedOn { get; set; }

        public virtual int? UpdatedBy { get; set; }
    }
}
