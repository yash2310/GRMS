using AMS.Infrastructure.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AMS.Test
{
	[TestFixture]
	public class EmployeesTest
	{
		[Test]
		public void AllReportees()
		{
            var reportees = EmployeeRepository.GetDirectReportee(1);
		}

		[Test]
		public void AllGoals()
		{
            var goals = EmployeeRepository.GetAllGoalsByUser(1);
		}

		[Test]
		public void AllReviewer()
		{
            var reviewers = EmployeeRepository.GetAllReviewerById(1);
            var goals = EmployeeRepository.GetAllGoalsByUser(1);
		}
	}
}
