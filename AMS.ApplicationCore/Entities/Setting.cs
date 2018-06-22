using System;

namespace AMS.ApplicationCore.Entities
{
	public class Setting : BaseEntity
	{
		public virtual string Name { get; set; }
		public virtual DateTime StartDate { get; set; }
		public virtual DateTime EndDate { get; set; }
	}
}