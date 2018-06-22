using AMS.Infrastructure.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AMS.Test
{
	[TestFixture]
	public class MasterTest
	{
		[Test]
		public void CycleTest()
		{
			var data = MasterRepository.GetCurrentCycle();
		}
	}
}
