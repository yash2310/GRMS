using System.ComponentModel.DataAnnotations;

namespace AMS.ApplicationCore.Entities
{
    public class DesignationGoal : Goals
    {
        [Required(ErrorMessage = "Designation Required")]
        public virtual Designation Designation { get; set; } // Designation Entity
    }
}
