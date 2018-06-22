using System;
using System.ComponentModel.DataAnnotations;
using AMS.ApplicationCore.Entities.Security;

namespace AMS.ApplicationCore.Entities
{
    public class Rating : BaseEntity
	{
		private float _rate;

		[Required(ErrorMessage = "Rater Id Required")]
		public virtual Employee Rater { get; set; } // Employee Entity for Rater

		[Required(ErrorMessage = "Ratee Id Required")]
		public virtual Employee Ratee { get; set; } // Employee Entity for Ratee

		[Required(ErrorMessage = "Goal Id Required")]
        public virtual int GoalId { get; set; } // Id of Goals of any type

		[Required(ErrorMessage = "Goal Id Required")]
		public virtual GoalType GoalType { get; set; } // Id of Goals of any type

		[Required(ErrorMessage = "Rate Required")]
        [Range(0, 5, ErrorMessage = "Rate 0 to 5")]
        public virtual float Rate
		{
			get => _rate;
			set
			{
				if (value < 0 || value > 5)
				{
					throw new Exception("Invalid rate");
				}
				_rate = value;
			}
		}

		[Required(ErrorMessage = "Comment Required")]
        public virtual string Comment { get; set; }
    }

	public enum GoalType
	{
		Organization = 1,
		Designation = 2,
		Managerial = 3,
		Employee = 4
	}
}