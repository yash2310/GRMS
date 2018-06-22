using System;
using System.ComponentModel.DataAnnotations;

namespace AMS.ApplicationCore.Entities
{
    public class Goals : BaseEntity
    {
        private int _weightage;

        [Required(ErrorMessage = "Goal Required")]
        public virtual string Goal { get; set; }

        [Required(ErrorMessage = "Weightage Required")]
        [Range(0, 100, ErrorMessage = "Weightage 0 to 100")]
        public virtual int Weightage
        {
            get => _weightage;
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new Exception("Invalid weightage");
                }
                _weightage = value;
            }
        }

        [Required(ErrorMessage = "Description Required")]
        [StringLength(500, ErrorMessage = "Description not more than 500 character")]
        public virtual string Description { get; set; } // Description for Goals

        [Required(ErrorMessage = "Review Cycle Required")]
        public virtual ReviewCycle Cycle { get; set; } // Review cycle

        [Required(ErrorMessage = "Goal Status Required")]
		public virtual GoalStatus Status { get; set; } // Goal Status

		[Required(ErrorMessage = "Start Date Required")]
	    public virtual DateTime StartDate { get; set; } // Goal start date

	    [Required(ErrorMessage = "End Date Required")]
	    public virtual DateTime EndDate { get; set; } // Goal end date
	}
}
