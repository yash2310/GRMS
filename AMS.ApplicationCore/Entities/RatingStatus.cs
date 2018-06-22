using System.ComponentModel.DataAnnotations;

namespace AMS.ApplicationCore.Entities
{
	public class RatingStatus : BaseEntity
	{
		public virtual string Name { get; set; }
	}
}
