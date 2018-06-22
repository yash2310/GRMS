using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.ApplicationCore.Entities.Security;

namespace AMS.ApplicationCore.Entities
{
	public class RatingLog : BaseEntity
	{
		public virtual Employee Employee { get; set; }
		public virtual Employee ReportingManager { get; set; }
		public virtual Designation Designation { get; set; }
		public virtual ReviewCycle ReviewCycle { get; set; }
		public virtual float Rating { get; set; }
		public virtual string Feedback { get; set; }
	}
}