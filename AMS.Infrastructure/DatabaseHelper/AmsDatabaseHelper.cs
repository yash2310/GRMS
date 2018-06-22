using AMS.ApplicationCore.Entities;
using AMS.ApplicationCore.Entities.Security;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;

namespace AMS.Infrastructure.DatabaseHelper
{
	internal class AmsDatabaseHelper
	{
		private AmsDatabaseHelper()
		{
			Configuration configuration;

			#region Database Configuration

			var directoryPath = AppContext.BaseDirectory;
			configuration = new Configuration().Configure(directoryPath + @"bin\DatabaseHelper\hibernate.cfg.xml");

			#endregion

			#region Entity Mapping

			configuration.AddFile(directoryPath + @"bin\EntityMapping\Role.hbm.xml");
			configuration.AddFile(directoryPath + @"bin\EntityMapping\Department.hbm.xml");
			configuration.AddFile(directoryPath + @"bin\EntityMapping\Designation.hbm.xml");
			configuration.AddFile(directoryPath + @"bin\EntityMapping\GoalStatus.hbm.xml");
			configuration.AddFile(directoryPath + @"bin\EntityMapping\RatingStatus.hbm.xml");
			configuration.AddFile(directoryPath + @"bin\EntityMapping\Setting.hbm.xml");
			configuration.AddFile(directoryPath + @"bin\EntityMapping\Organization.hbm.xml");
			configuration.AddFile(directoryPath + @"bin\EntityMapping\ReviewCycle.hbm.xml");

			configuration.AddFile(directoryPath + @"bin\EntityMapping\Employee.hbm.xml");
			configuration.AddFile(directoryPath + @"bin\EntityMapping\OrganizationGoal.hbm.xml");
			configuration.AddFile(directoryPath + @"bin\EntityMapping\DesignationGoal.hbm.xml");
			configuration.AddFile(directoryPath + @"bin\EntityMapping\ManagerialEmployeeGoal.hbm.xml");
			configuration.AddFile(directoryPath + @"bin\EntityMapping\EmployeeGoal.hbm.xml");
			configuration.AddFile(directoryPath + @"bin\EntityMapping\Rating.hbm.xml");

			configuration.AddFile(directoryPath + @"bin\EntityMapping\NotificationLog.hbm.xml");
			configuration.AddFile(directoryPath + @"bin\EntityMapping\RatingLog.hbm.xml");

			#endregion

			#region Envers Configuration For Audit

			var enversConf = new NHibernate.Envers.Configuration.Fluent.FluentConfiguration();

			// RatingLog and NotificationLog Entities are used for logging
			enversConf.Audit<Department>()
				.Exclude("CreatedOn")
				.Exclude("CreatedBy")
				.Exclude("Employees");
			enversConf.Audit<Designation>()
				.Exclude("CreatedOn")
				.Exclude("CreatedBy")
				.Exclude("DesignationGoals")
				.Exclude("Employees");
			enversConf.Audit<Organization>()
				.Exclude("CreatedOn")
				.Exclude("CreatedBy")
				.Exclude("OrganizationGoals")
				.Exclude("Employees");
			enversConf.Audit<GoalStatus>()
				.Exclude("CreatedOn")
				.Exclude("CreatedBy");
			enversConf.Audit<ReviewCycle>()
				.Exclude("CreatedOn")
				.Exclude("CreatedBy")
				.Exclude("OrganizationGoals")
				.Exclude("DesignationGoals")
				.Exclude("ManagerialEmployeeGoals")
				.Exclude("EmployeeGoals");
			enversConf.Audit<RatingStatus>()
				.Exclude("CreatedOn")
				.Exclude("CreatedBy");
			//enversConf.Audit<Setting>()
			//	.Exclude("CreatedOn")
			//	.Exclude("CreatedBy");
			enversConf.Audit<Role>()
				.Exclude("CreatedOn")
				.Exclude("CreatedBy");

			enversConf.Audit<Employee>()
				.Exclude("CreatedOn")
				.Exclude("CreatedBy")
				.Exclude("Roles")
				.Exclude("OrganizationGoals")
				.Exclude("DesignationGoals")
				.Exclude("ManagerialEmployeeGoals")
				.Exclude("EmployeeGoals");
			enversConf.Audit<DesignationGoal>()
				.Exclude("CreatedOn")
				.Exclude("CreatedBy");
			enversConf.Audit<OrganizationGoal>()
				.Exclude("CreatedOn")
				.Exclude("CreatedBy");
			enversConf.Audit<EmployeeGoal>()
				.Exclude("CreatedOn")
				.Exclude("CreatedBy");
			enversConf.Audit<ManagerialEmployeeGoal>()
				.Exclude("CreatedOn")
				.Exclude("CreatedBy");
			enversConf.Audit<Rating>()
				.Exclude("CreatedOn")
				.Exclude("CreatedBy");

			configuration.IntegrateWithEnvers(enversConf);

			#endregion

			_sessionFactory = configuration.BuildSessionFactory();

			#region Create/Update Database Schema

			new SchemaUpdate(configuration).Execute(false, true);

			#endregion
		}

		public static AmsDatabaseHelper Create()
		{
			if (_nHibHelper == null)
			{
				try
				{
					_nHibHelper = new AmsDatabaseHelper();
				}
				catch (Exception)
				{
					_nHibHelper = null;
				}
			}

			return _nHibHelper;
		}

		public ISession Session
		{
			get { return _sessionFactory.OpenSession(); }
		}

		private readonly ISessionFactory _sessionFactory;
		static AmsDatabaseHelper _nHibHelper;
	}
}