using System.Collections.Generic;

namespace AMS.WebApi.ViewModels
{
    public class ReviewerData
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
        public virtual string EmailId { get; set; }
        public virtual string EmployeeNo { get; set; }
    }
}