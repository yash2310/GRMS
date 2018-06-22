using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.ApplicationCore.Entities.Security;
using AMS.Infrastructure.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AMS.Test
{
	[TestFixture]
	public class AccountTest
	{
		[TestCase]
		public void LoginTest()
		{
			//Employee employee = AccountRepository.Login("yash@bluepi.in");
			//Console.WriteLine(employee != null ? true : false);
		}

		[TestCase]
		public void ResetPasswordTest()
		{
			//bool employee = AccountRepository.ResetPassword(68, "yash@bluepi");
			//Console.WriteLine(employee);
		}
	}
}
