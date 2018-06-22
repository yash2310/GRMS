using AMS.ApplicationCore.Entities;
using AMS.ApplicationCore.Entities.Security;
using AMS.Infrastructure.DatabaseHelper;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AMS.Infrastructure.Repository
{
	public class MasterRepository
	{
		private static ISession _session;

		public List<ReviewCycle> GetAllCycle()
		{
			List<ReviewCycle> data = null;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
                    data = _session.CreateCriteria<ReviewCycle>()
                        .List<ReviewCycle>().ToList();
				}
			}
			catch (Exception)
			{
				data = null;
			}
			return data;
		}

        public static ReviewCycle GetCurrentCycle()
        {
            ReviewCycle data = null;
            try
            {
                using (_session = AmsDatabaseHelper.Create().Session)
                {
                    data = _session.CreateCriteria<ReviewCycle>()
                        .List<ReviewCycle>().FirstOrDefault(x => x.Status == true);
                }
            }
            catch (Exception)
            {
                data = null;
            }
            return data;
        }

		public static ReviewCycle GetCycleById(int Id)
		{
			ReviewCycle cycle;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					cycle = _session.Query<ReviewCycle>().FirstOrDefault(c => c.Id.Equals(Id));
				}
			}
			catch (Exception)
			{
				cycle = null;
			}
			return cycle;
		}

		public static ReviewCycle GetCycleByName(object reviewCycle)
		{
			ReviewCycle cycle;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					cycle = _session.Query<ReviewCycle>().FirstOrDefault(c => c.Name.Equals(reviewCycle));
				}
			}
			catch (Exception)
			{
				cycle = null;
			}
			return cycle;
		}

		public static IList<Organization> GetAllOrganization()
		{
			IList<Organization> organizations = null;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					organizations = _session.CreateCriteria<Organization>()
						.List<Organization>();
				}
			}
			catch (Exception)
			{
				organizations = null;
			}
			return organizations;
		}

		public static Organization GetOrganizationById(int id)
		{
			Organization organization;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					organization = _session.Query<Organization>().FirstOrDefault(c => c.Id.Equals(id));
				}
			}
			catch (Exception)
			{
				organization = null;
			}
			return organization;
		}

		public static IList<Department> GetAllDepartment()
		{
			IList<Department> departments = null;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					departments = _session.CreateCriteria<Department>()
						.List<Department>();
				}
			}
			catch (Exception)
			{
				departments = null;
			}
			return departments;
		}

		public static Department GetDepartmentById(int id)
		{
			Department department;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					department = _session.Query<Department>().FirstOrDefault(c => c.Id.Equals(id));
				}
			}
			catch (Exception)
			{
				department = null;
			}
			return department;
		}

		public static IList<Designation> GetAllDesignation()
		{
			IList<Designation> designations = null;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					designations = _session.CreateCriteria<Designation>()
						.List<Designation>();
				}
			}
			catch (Exception)
			{
				designations = null;
			}
			return designations;
		}

		public static Designation GetDesignationById(int id)
		{
			Designation designation;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					designation = _session.Query<Designation>().FirstOrDefault(c => c.Id.Equals(id));
				}
			}
			catch (Exception)
			{
				designation = null;
			}
			return designation;
		}

		public static IList<Role> GetAllRole()
		{
			IList<Role> roles = null;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					roles = _session.CreateCriteria<Role>()
						.List<Role>();
				}
			}
			catch (Exception)
			{
				roles = null;
			}
			return roles;
		}

		public static Role GetRoleById(int id)
		{
			Role role;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					role = _session.Query<Role>().FirstOrDefault(c => c.Id.Equals(id));
				}
			}
			catch (Exception)
			{
				role = null;
			}
			return role;
		}

		public static IList<GoalStatus> GetAllGoalStatus()
		{
			IList<GoalStatus> goalStatuses = null;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					goalStatuses = _session.CreateCriteria<GoalStatus>()
						.List<GoalStatus>();
				}
			}
			catch (Exception)
			{
				goalStatuses = null;
			}
			return goalStatuses;
		}

		public static GoalStatus GetGoalStatusById(int id)
		{
			GoalStatus goalStatuse;
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					goalStatuse = _session.Query<GoalStatus>().FirstOrDefault(c => c.Id.Equals(id));
				}
			}
			catch (Exception)
			{
				goalStatuse = null;
			}
			return goalStatuse;
		}
	}
}
