using System.ComponentModel.DataAnnotations;

namespace AMS.ApplicationCore.Entities
{
	public class GoalStatus : BaseEntity
	{
		public virtual string Name { get; set; }
	}
}
