using System.ComponentModel.DataAnnotations;

namespace AMS.ApplicationCore.Entities
{
    public class OrganizationGoal : Goals
    {
        [Required(ErrorMessage = "Organization Required")]
        public virtual Organization Organization { get; set; } // Organization Entity
    }
}
