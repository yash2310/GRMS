using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMS.ApplicationCore.Entities;
using AMS.ApplicationCore.Entities.Security;
using AMS.Infrastructure.Repository;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AMS.Test
{
	[TestFixture]
	public class GoalTest
	{
		[Test]
		public void InsertGoal()
		{
			try
			{
				EmployeeGoal goal = new EmployeeGoal
				{
					Goal = "dasdas",
					Weightage = 20,
					Description = "dasdsad",
					StartDate = DateTime.Now,
					EndDate = DateTime.Now,
					Reviewer = EmployeeRepository.GetEmployeeById(21),
					Employee = EmployeeRepository.GetEmployeeById(68),
					Cycle = MasterRepository.GetCurrentCycle(),
					Status = MasterRepository.GetGoalStatusById(1),
					CreatedOn = DateTime.Now,
					CreatedBy = 1
				};

				//GoalRepository<EmployeeGoal>.AddItem(goal);
			}
			catch (Exception e)
			{
			}
		}

		[Test]
		public void GoalWeightage()
		{
			int weightage = GoalRepository<Employee>.GetWeightage(1, "Employee");
			weightage = GoalRepository<Employee>.GetWeightage(1, "Managerial");
			weightage = GoalRepository<Employee>.GetWeightage(1, "Organizational");
			weightage = GoalRepository<Employee>.GetWeightage(1, "Designation");
		}

		[Test]
		public void UpdateGoalStatus()
		{
			GoalRepository<EmployeeGoal>.UpdateGoalsStatus(1,
				"finalize"); // email = email id of reportee and statusType = finalize/reviewed
			GoalRepository<ManagerialEmployeeGoal>.UpdateGoalsStatus(1,
				"finalize"); // email = email id of reportee and statusType = finalize/reviewed
		}

		[Test]
		public void RemoveGoal()
		{
			EmployeeGoal employeeGoal = new EmployeeGoal();
			GoalRepository<EmployeeGoal>
				.RemoveGoal(employeeGoal); // email = email id of reportee and statusType = finalize/reviewed

			ManagerialEmployeeGoal managerialGoal = new ManagerialEmployeeGoal();
			GoalRepository<ManagerialEmployeeGoal>
				.RemoveGoal(managerialGoal); // email = email id of reportee and statusType = finalize/reviewed
		}

		[Test]
		public void UpdateGoal()
		{
			EmployeeGoal employeeGoal = new EmployeeGoal();

			//2	dasda	23	dasda	2017-02-02 00:00:00.0000000	2017-02-02 00:00:00.0000000	1	1	68	NULL	2017-02-02 00:00:00.0000000	1	NULL	NULL
			//employeeGoal.Id = 1;
			employeeGoal.Goal = "dasda";
			employeeGoal.Weightage = 25;
			employeeGoal.Description = "adsfgh";
			employeeGoal.StartDate = Convert.ToDateTime("2017-02-02");
			employeeGoal.EndDate = Convert.ToDateTime("2017-02-02");
			employeeGoal.Cycle = MasterRepository.GetCurrentCycle();
			employeeGoal.Status = MasterRepository.GetGoalStatusById(1);
			employeeGoal.Employee = EmployeeRepository.GetEmployeeById(68);
			employeeGoal.CreatedBy = EmployeeRepository.GetEmployeeById(68).Id;
			employeeGoal.CreatedOn = Convert.ToDateTime("2017-02-02");
			employeeGoal.UpdatedBy = EmployeeRepository.GetEmployeeById(68).Id;
			employeeGoal.UpdatedOn = Convert.ToDateTime("2017-02-02");

			List<int> reviewerList = new List<int>();
			reviewerList.Add(1);
			reviewerList.Add(2);
			reviewerList.Add(3);

			bool statusEmp = GoalRepository<EmployeeGoal>
				.UpdateGoal(employeeGoal, reviewerList); // email = email id of reportee and statusType = finalize/reviewed

			ManagerialEmployeeGoal managerialGoal = new ManagerialEmployeeGoal();
			managerialGoal.Id = 4;
			managerialGoal.Goal = "asdfghjytre";
			managerialGoal.Weightage = 27;
			managerialGoal.Description = "adsfgh";
			managerialGoal.StartDate = Convert.ToDateTime("2017-02-02");
			managerialGoal.EndDate = Convert.ToDateTime("2017-02-02");
			managerialGoal.Cycle = MasterRepository.GetCurrentCycle();
			managerialGoal.Status = MasterRepository.GetGoalStatusById(1);
			managerialGoal.Employee = EmployeeRepository.GetEmployeeById(68);
			managerialGoal.UpdatedBy = EmployeeRepository.GetEmployeeById(68).Id;
			managerialGoal.UpdatedOn = Convert.ToDateTime("2017-02-02");

			bool statusMgr = GoalRepository<ManagerialEmployeeGoal>
				.UpdateGoal(managerialGoal, null); // email = email id of reportee and statusType = finalize/reviewed
		}
	}
}