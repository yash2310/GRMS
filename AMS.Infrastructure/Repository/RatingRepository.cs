using AMS.ApplicationCore.Entities;
using AMS.ApplicationCore.Entities.Security;
using AMS.ApplicationCore.Interfaces;
using AMS.Infrastructure.DatabaseHelper;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static AMS.Infrastructure.Enumeration;

namespace AMS.Infrastructure.Repository
{
	public class RatingRepository<T> : IRepository<T>
	{
		private static ISession _session;

		public static ArrayList GetAllGoalsRating(EmployeeRating empRating)
		{
			List<RatingData> employeeGoalRating = new List<RatingData>();
			List<RatingData> designationGoalRating = new List<RatingData>();
			List<RatingData> organizationGoalRating = new List<RatingData>();

			ArrayList listRating = new ArrayList();
			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
                    if (empRating.UserType == null)
                    {
                        #region Employee Goal Rating

                        IQuery queryData = _session.CreateSQLQuery("Select rr.*, sr.Rate SelfRating, sr.Comment SelfComment from " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM EmployeeGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType ={(int)GoalType.Employee}  WHERE eg.Employee = {empRating.Ratee} and eg.Reviewer = {empRating.Rater} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as rr " +
                            "left join " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM EmployeeGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType ={(int)GoalType.Employee}  WHERE eg.Employee = {empRating.Ratee} and eg.Reviewer = {empRating.Ratee} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as sr on rr.Goal = sr.Goal");

                        var emplists = queryData.List();

                        foreach (object[] emplist in emplists)
                        {
                            employeeGoalRating.Add(new RatingData
                            {
                                Id = emplist[0] != null ? Convert.ToInt32(emplist[0]) : 0,
                                GoalId = Convert.ToInt32(emplist[1]),
                                Goal = Convert.ToString(emplist[2]),
                                GoalType = GoalType.Employee,
                                StartDate = Convert.ToDateTime(emplist[3]),
                                EndDate = Convert.ToDateTime(emplist[4]),
                                Status = Convert.ToString(emplist[5]),
                                Weightage = Convert.ToInt32(emplist[6]),
                                Rating = emplist[7] != null ? (float)emplist[7] : 0,
                                Comment = emplist[8] != null ? Convert.ToString(emplist[8]) : "",
                                SelfRating = emplist[9] != null ? (float)emplist[9] : 0,
                                SelfComment = emplist[10] != null ? Convert.ToString(emplist[10]) : ""
                            });
                        }

                        listRating.Add(employeeGoalRating);

                        #endregion

                        #region Designation Goal Rating

                        queryData = _session.CreateSQLQuery("Select rr.*, sr.Rate SelfRating, sr.Comment SelfComment from " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM DesignationGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType = {(int)GoalType.Designation} and rt.Rater = {empRating.Rater} and rt.Ratee = {empRating.Ratee} WHERE eg.Designation = {empRating.Designation} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as rr	" +
                            $"left join " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM DesignationGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType = {(int)GoalType.Designation} and rt.Rater = {empRating.Ratee} and rt.Ratee = {empRating.Ratee} WHERE eg.Designation = {empRating.Designation} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as sr on rr.Goal = sr.Goal");

                        var dglists = queryData.List();

                        foreach (object[] dglist in dglists)
                        {
                            designationGoalRating.Add(new RatingData
                            {
                                Id = dglist[0] != null ? Convert.ToInt32(dglist[0]) : 0,
                                GoalId = Convert.ToInt32(dglist[1]),
                                Goal = Convert.ToString(dglist[2]),
                                GoalType = GoalType.Designation,
                                StartDate = Convert.ToDateTime(dglist[3]),
                                EndDate = Convert.ToDateTime(dglist[4]),
                                Status = Convert.ToString(dglist[5]),
                                Weightage = Convert.ToInt32(dglist[6]),
                                Rating = dglist[7] != null ? (float)dglist[7] : 0,
                                Comment = dglist[8] != null ? Convert.ToString(dglist[8]) : "",
                                SelfRating = dglist[9] != null ? (float)dglist[9] : 0,
                                SelfComment = dglist[10] != null ? Convert.ToString(dglist[10]) : ""
                            });
                        }

                        listRating.Add(designationGoalRating);

                        #endregion

                        #region Organization Goal Rating

                        queryData = _session.CreateSQLQuery("Select rr.*, sr.Rate SelfRating, sr.Comment SelfComment from " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM OrganizationGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType = {(int)GoalType.Organization} and rt.Rater = {empRating.Rater} and rt.Ratee = {empRating.Ratee} WHERE eg.Organization = {empRating.Organization} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as rr	" +
                            "left join " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM OrganizationGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType = {(int)GoalType.Organization} and rt.Rater = {empRating.Ratee} and rt.Ratee = {empRating.Ratee} WHERE eg.Organization = {empRating.Organization} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as sr on rr.Goal = sr.Goal");


                        var orglists = queryData.List();

                        foreach (object[] orglist in orglists)
                        {
                            organizationGoalRating.Add(new RatingData
                            {
                                Id = orglist[0] != null ? Convert.ToInt32(orglist[0]) : 0,
                                GoalId = Convert.ToInt32(orglist[1]),
                                Goal = Convert.ToString(orglist[2]),
                                GoalType = GoalType.Organization,
                                StartDate = Convert.ToDateTime(orglist[3]),
                                EndDate = Convert.ToDateTime(orglist[4]),
                                Status = Convert.ToString(orglist[5]),
                                Weightage = Convert.ToInt32(orglist[6]),
                                Rating = orglist[7] != null ? (float)orglist[7] : 0,
                                Comment = orglist[8] != null ? Convert.ToString(orglist[8]) : "",
                                SelfRating = orglist[9] != null ? (float)orglist[9] : 0,
                                SelfComment = orglist[10] != null ? Convert.ToString(orglist[10]) : ""
                            });
                        }

                        listRating.Add(organizationGoalRating);

                        #endregion
                    }
                    else if (empRating.UserType == "Reportee")
                    {
                        #region Employee Goal Rating

                        IQuery queryData = _session.CreateSQLQuery("Select rr.*, sr.Rate SelfRating, sr.Comment SelfComment from " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM EmployeeGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType ={(int)GoalType.Employee}  WHERE eg.Employee = {empRating.Ratee} and eg.Reviewer = {empRating.Rater} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as rr " +
                            "left join " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM EmployeeGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType ={(int)GoalType.Employee}  WHERE eg.Employee = {empRating.Ratee} and eg.Reviewer = {empRating.Ratee} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as sr on rr.Goal = sr.Goal");

                        var emplists = queryData.List();

                        foreach (object[] emplist in emplists)
                        {
                            employeeGoalRating.Add(new RatingData
                            {
                                Id = emplist[0] != null ? Convert.ToInt32(emplist[0]) : 0,
                                GoalId = Convert.ToInt32(emplist[1]),
                                Goal = Convert.ToString(emplist[2]),
                                GoalType = GoalType.Employee,
                                StartDate = Convert.ToDateTime(emplist[3]),
                                EndDate = Convert.ToDateTime(emplist[4]),
                                Status = Convert.ToString(emplist[5]),
                                Weightage = Convert.ToInt32(emplist[6]),
                                Rating = emplist[7] != null ? (float)emplist[7] : 0,
                                Comment = emplist[8] != null ? Convert.ToString(emplist[8]) : "",
                                SelfRating = emplist[9] != null ? (float)emplist[9] : 0,
                                SelfComment = emplist[10] != null ? Convert.ToString(emplist[10]) : ""
                            });
                        }

                        listRating.Add(employeeGoalRating);

                        #endregion

                        #region Designation Goal Rating

                        queryData = _session.CreateSQLQuery("Select rr.*, sr.Rate SelfRating, sr.Comment SelfComment from " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM DesignationGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType = {(int)GoalType.Designation} and rt.Rater = {empRating.Rater} and rt.Ratee = {empRating.Ratee} WHERE eg.Designation = {empRating.Designation} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as rr	" +
                            $"left join " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM DesignationGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType = {(int)GoalType.Designation} and rt.Rater = {empRating.Ratee} and rt.Ratee = {empRating.Ratee} WHERE eg.Designation = {empRating.Designation} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as sr on rr.Goal = sr.Goal");

                        var dglists = queryData.List();

                        foreach (object[] dglist in dglists)
                        {
                            designationGoalRating.Add(new RatingData
                            {
                                Id = dglist[0] != null ? Convert.ToInt32(dglist[0]) : 0,
                                GoalId = Convert.ToInt32(dglist[1]),
                                Goal = Convert.ToString(dglist[2]),
                                GoalType = GoalType.Designation,
                                StartDate = Convert.ToDateTime(dglist[3]),
                                EndDate = Convert.ToDateTime(dglist[4]),
                                Status = Convert.ToString(dglist[5]),
                                Weightage = Convert.ToInt32(dglist[6]),
                                Rating = dglist[7] != null ? (float)dglist[7] : 0,
                                Comment = dglist[8] != null ? Convert.ToString(dglist[8]) : "",
                                SelfRating = dglist[9] != null ? (float)dglist[9] : 0,
                                SelfComment = dglist[10] != null ? Convert.ToString(dglist[10]) : ""
                            });
                        }

                        listRating.Add(designationGoalRating);
                        listRating.Add(new RatingData());

                        #endregion
                    }
                    else if (empRating.UserType == "Reviewee")
                    {
                        #region Employee Goal Rating

                        IQuery queryData = _session.CreateSQLQuery("Select rr.*, sr.Rate SelfRating, sr.Comment SelfComment from " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM EmployeeGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType ={(int)GoalType.Employee}  WHERE eg.Employee = {empRating.Ratee} and eg.Reviewer = {empRating.Rater} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as rr " +
                            "left join " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM EmployeeGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType ={(int)GoalType.Employee}  WHERE eg.Employee = {empRating.Ratee} and eg.Reviewer = {empRating.Ratee} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as sr on rr.Goal = sr.Goal");

                        var emplists = queryData.List();

                        foreach (object[] emplist in emplists)
                        {
                            employeeGoalRating.Add(new RatingData
                            {
                                Id = emplist[0] != null ? Convert.ToInt32(emplist[0]) : 0,
                                GoalId = Convert.ToInt32(emplist[1]),
                                Goal = Convert.ToString(emplist[2]),
                                GoalType = GoalType.Employee,
                                StartDate = Convert.ToDateTime(emplist[3]),
                                EndDate = Convert.ToDateTime(emplist[4]),
                                Status = Convert.ToString(emplist[5]),
                                Weightage = Convert.ToInt32(emplist[6]),
                                Rating = emplist[7] != null ? (float)emplist[7] : 0,
                                Comment = emplist[8] != null ? Convert.ToString(emplist[8]) : "",
                                SelfRating = emplist[9] != null ? (float)emplist[9] : 0,
                                SelfComment = emplist[10] != null ? Convert.ToString(emplist[10]) : ""
                            });
                        }

                        listRating.Add(employeeGoalRating);
                        listRating.Add(new RatingData());
                        listRating.Add(new RatingData());

                        #endregion
                    }
                    else
                    {
                        #region Organization Goal Rating

                        IQuery queryData = _session.CreateSQLQuery("Select rr.*, sr.Rate SelfRating, sr.Comment SelfComment from " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM OrganizationGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType = {(int)GoalType.Organization} and rt.Rater != {empRating.Ratee} and rt.Ratee = {empRating.Ratee} WHERE eg.Organization = {empRating.Organization} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as rr	" +
                            "left join " +
                            $"(SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM OrganizationGoal eg left Join Rating rt on eg.Id = rt.GoalId and rt.GoalType = {(int)GoalType.Organization} and rt.Rater = {empRating.Ratee} and rt.Ratee = {empRating.Ratee} WHERE eg.Organization = {empRating.Organization} and eg.ReviewCycle = {empRating.Cycle} and eg.GoalStatus = 2) as sr on rr.Goal = sr.Goal");


                        var orglists = queryData.List();

                        foreach (object[] orglist in orglists)
                        {
                            organizationGoalRating.Add(new RatingData
                            {
                                Id = orglist[0] != null ? Convert.ToInt32(orglist[0]) : 0,
                                GoalId = Convert.ToInt32(orglist[1]),
                                Goal = Convert.ToString(orglist[2]),
                                GoalType = GoalType.Organization,
                                StartDate = Convert.ToDateTime(orglist[3]),
                                EndDate = Convert.ToDateTime(orglist[4]),
                                Status = Convert.ToString(orglist[5]),
                                Weightage = Convert.ToInt32(orglist[6]),
                                Rating = orglist[7] != null ? (float)orglist[7] : 0,
                                Comment = orglist[8] != null ? Convert.ToString(orglist[8]) : "",
                                SelfRating = orglist[9] != null ? (float)orglist[9] : 0,
                                SelfComment = orglist[10] != null ? Convert.ToString(orglist[10]) : ""
                            });
                        }

                        listRating.Add(new RatingData());
                        listRating.Add(new RatingData());
                        listRating.Add(organizationGoalRating);

                        #endregion
                    }
				}
			}
			catch (Exception e)
			{
				listRating = null;
			}
			return listRating;
		}

        public static ArrayList GetCompleteRatingByEmpId(int empId)
        {
            List<RatingData> employeeGoalRating = new List<RatingData>();
            List<RatingData> managerGoalRating = new List<RatingData>();
            List<RatingData> designationGoalRating = new List<RatingData>();
            List<RatingData> organizationGoalRating = new List<RatingData>();

            ArrayList listRating = new ArrayList();
            try
            {
                using (_session = AmsDatabaseHelper.Create().Session)
                {
                    #region Employee Goal Rating

                    IQuery queryData = _session.CreateSQLQuery("select rt.Id,rt.GoalId,eg.Goal,rt.GoalType,eg.StartDate,eg.EndDate,eg.GoalStatus,eg.Weightage,rt.Rate,rt.Comment,emp.Name from Rating rt join EmployeeGoal eg on rt.GoalId = eg.Id join employee emp on rt.Rater = emp.Id where rt.GoalType=4 and rt.Ratee=" + empId + " and rt.Rater != " + empId + " order by GoalId asc");

                    var emplists = queryData.List();

                    foreach (object[] emplist in emplists)
                    {
                        employeeGoalRating.Add(new RatingData
                        {
                            Id = emplist[0] != null ? Convert.ToInt32(emplist[0]) : 0,
                            GoalId = Convert.ToInt32(emplist[1]),
                            Goal = Convert.ToString(emplist[2]),
                            GoalType = GoalType.Employee,
                            StartDate = Convert.ToDateTime(emplist[4]),
                            EndDate = Convert.ToDateTime(emplist[5]),
                            Status = Convert.ToString(emplist[6]),
                            Weightage = Convert.ToInt32(emplist[7]),
                            Rating = emplist[8] != null ? (float)emplist[8] : 0,
                            Comment = emplist[9] != null ? Convert.ToString(emplist[9]) : "",
                            Reviewer = emplist[10] != null ? Convert.ToString(emplist[10]) : ""
                        });
                    }

                    listRating.Add(employeeGoalRating);

                    #endregion

                    #region Managerial Employee Goal Rating

                    queryData = _session.CreateSQLQuery("select rt.Id,rt.GoalId,meg.Goal,rt.GoalType,meg.StartDate,meg.EndDate,meg.GoalStatus,meg.Weightage,rt.Rate,rt.Comment,emp.Name from Rating rt join ManagerialEmployeeGoal meg on rt.GoalId = meg.Id join employee emp on rt.Rater = emp.Id where rt.GoalType=3 and rt.Ratee= " + empId + " and rt.Rater != " + empId + " order by GoalId asc");

                    var mglists = queryData.List();

                    foreach (object[] mglist in mglists)
                    {
                        managerGoalRating.Add(new RatingData
                        {
                            Id = mglist[0] != null ? Convert.ToInt32(mglist[0]) : 0,
                            GoalId = Convert.ToInt32(mglist[1]),
                            Goal = Convert.ToString(mglist[2]),
                            GoalType = GoalType.Managerial,
                            StartDate = Convert.ToDateTime(mglist[4]),
                            EndDate = Convert.ToDateTime(mglist[5]),
                            Status = Convert.ToString(mglist[6]),
                            Weightage = Convert.ToInt32(mglist[7]),
                            Rating = mglist[8] != null ? (float)mglist[8] : 0,
                            Comment = mglist[9] != null ? Convert.ToString(mglist[9]) : "",
                            Reviewer = mglist[10] != null ? Convert.ToString(mglist[10]) : ""
                        });
                    }

                    listRating.Add(managerGoalRating);

                    #endregion

                    #region Designation Goal Rating

                    queryData = _session.CreateSQLQuery("select rt.Id,rt.GoalId,dg.Goal,rt.GoalType,dg.StartDate,dg.EndDate,dg.GoalStatus,dg.Weightage,rt.Rate,rt.Comment,emp.Name from Rating rt join DesignationGoal dg on rt.GoalId = dg.Id join employee emp on rt.Rater = emp.Id where rt.GoalType=2 and rt.Ratee= " + empId + " and rt.Rater != " + empId + " order by GoalId asc");

                    var dglists = queryData.List();

                    foreach (object[] dglist in dglists)
                    {
                        designationGoalRating.Add(new RatingData
                        {
                            Id = dglist[0] != null ? Convert.ToInt32(dglist[0]) : 0,
                            GoalId = Convert.ToInt32(dglist[1]),
                            Goal = Convert.ToString(dglist[2]),
                            GoalType = GoalType.Designation,
                            StartDate = Convert.ToDateTime(dglist[4]),
                            EndDate = Convert.ToDateTime(dglist[5]),
                            Status = Convert.ToString(dglist[6]),
                            Weightage = Convert.ToInt32(dglist[7]),
                            Rating = dglist[8] != null ? (float)dglist[8] : 0,
                            Comment = dglist[9] != null ? Convert.ToString(dglist[9]) : "",
                            Reviewer = dglist[10] != null ? Convert.ToString(dglist[10]) : ""
                        });
                    }

                    listRating.Add(designationGoalRating);

                    #endregion

                    #region Organization Goal Rating

                    queryData = _session.CreateSQLQuery("select rt.Id,rt.GoalId,og.Goal,rt.GoalType,og.StartDate,og.EndDate,og.GoalStatus,og.Weightage,rt.Rate,rt.Comment,emp.Name from Rating rt join OrganizationGoal og on rt.GoalId = og.Id join employee emp on rt.Rater = emp.Id where rt.GoalType=1 and rt.Ratee= " + empId + " and rt.Rater != " + empId + " order by GoalId asc");

                    var orglists = queryData.List();

                    foreach (object[] orglist in orglists)
                    {
                        organizationGoalRating.Add(new RatingData
                        {
                            Id = orglist[0] != null ? Convert.ToInt32(orglist[0]) : 0,
                            GoalId = Convert.ToInt32(orglist[1]),
                            Goal = Convert.ToString(orglist[2]),
                            GoalType = GoalType.Organization,
                            StartDate = Convert.ToDateTime(orglist[4]),
                            EndDate = Convert.ToDateTime(orglist[5]),
                            Status = Convert.ToString(orglist[6]),
                            Weightage = Convert.ToInt32(orglist[7]),
                            Rating = orglist[8] != null ? (float)orglist[8] : 0,
                            Comment = orglist[9] != null ? Convert.ToString(orglist[9]) : "",
                            Reviewer = orglist[10] != null ? Convert.ToString(orglist[10]) : ""
                        });
                    }

                    listRating.Add(organizationGoalRating);

                    #endregion
                }
            }
            catch (Exception e)
            {
                listRating = null;
            }
            return listRating;
        }

        public static double GetFinalRatingByEmpId(int empId,string userRole)
        {
            double finalRating = 0;
            double finalRatingEmp = 0;
            double finalRatingMgr = 0;
            double finalRatingDes = 0;
            double finalRatingOrg = 0;

            int count = 0;

            try
            {
                using (_session = AmsDatabaseHelper.Create().Session)
                {
                    //Employee Goal Final Rating
                    IQuery queryData = _session.CreateSQLQuery("select (sum(rt.Rate)/count(rt.GoalId))*eg.Weightage/100 as averageRating from Rating rt join EmployeeGoal eg on rt.GoalId = eg.Id where rt.GoalType = 4 and rt.Ratee = " + empId + " and rt.Rater != " + empId + " group by eg.Goal, eg.Weightage order by eg.Goal");
                    var empRating = queryData.List<double>();

                    if (empRating.Count > 0)
                    {
                        count++;
                        finalRatingEmp = empRating.Sum() * (userRole == UserRoles.Employee.ToString() ? Convert.ToInt32(EmployeeWeightage.EmployeeWeight) : Convert.ToInt32(ManagerWeightage.EmployeeWeight))  / 100;
                    }

                    //Managerial Employee Goal Rating
                    queryData = _session.CreateSQLQuery("select (sum(rt.Rate)/count(rt.GoalId))*meg.Weightage/100 as averageRating from Rating rt join ManagerialEmployeeGoal meg on rt.GoalId = meg.Id where rt.GoalType = 3 and rt.Ratee = " + empId + " and rt.Rater != " + empId + " group by meg.Goal, meg.Weightage order by meg.Goal");
                    var mgrRating = queryData.List<double>();

                    if (mgrRating.Count > 0)
                    {
                        count++;
                        finalRatingMgr = mgrRating.Sum() * (userRole == UserRoles.Employee.ToString() ? Convert.ToInt32(EmployeeWeightage.ManagerialWeight) : Convert.ToInt32(ManagerWeightage.ManagerialWeight)) / 100;
                    }

                    //Designation Goal Rating
                    queryData = _session.CreateSQLQuery("select (sum(rt.Rate)/count(rt.GoalId))*dg.Weightage/100 as averageRating from Rating rt join DesignationGoal dg on rt.GoalId = dg.Id where rt.GoalType = 2 and rt.Ratee = " + empId + " and rt.Rater != " + empId + " group by dg.Goal, dg.Weightage order by dg.Goal");
                    var desRating = queryData.List<double>();

                    if (desRating.Count > 0)
                    {
                        count++;
                        finalRatingDes = desRating.Sum() * (userRole == UserRoles.Employee.ToString() ? Convert.ToInt32(EmployeeWeightage.DesignationWeight) : Convert.ToInt32(ManagerWeightage.DesignationWeight)) / 100;
                    }

                    //Organization Goal Rating
                    queryData = _session.CreateSQLQuery("select (sum(rt.Rate)/count(rt.GoalId))*og.Weightage/100 as averageRating from Rating rt join OrganizationGoal og on rt.GoalId = og.Id where rt.GoalType = 1 and rt.Ratee = " + empId + " and rt.Rater != " + empId + " group by og.Goal, og.Weightage order by og.Goal");
                    var orgRating  = queryData.List<double>();

                    if (orgRating.Count > 0)
                    {
                        count++;
                        finalRatingOrg = orgRating.Sum() * (userRole == UserRoles.Employee.ToString() ? Convert.ToInt32(EmployeeWeightage.OrganizationWeight) : Convert.ToInt32(ManagerWeightage.OrganizationWeight)) / 100;
                    }

                    //if (count > 0)
                    //{
                    //finalRating = Math.Round(((finalRatingEmp + finalRatingMgr + finalRatingDes + finalRatingOrg )/ count), 1);
                    finalRating = Math.Round(((finalRatingEmp + finalRatingMgr + finalRatingDes + finalRatingOrg)), 1);
                    double numberPart = Math.Truncate(finalRating);
                        double decimalPart = finalRating - numberPart;

                        if (decimalPart > .5)
                            finalRating = numberPart + 1;
                        else if (decimalPart == .5)
                        { }
                        else
                            finalRating = numberPart;
                    //}
                    //else
                    //    finalRating = 0;
                }
            }
            catch (Exception e)
            {
                finalRating = 0;
            }

            return finalRating;
        }

        public static List<RatingData> GetManagerialGoalsRating(EmployeeRating empRating)
		{
			List<RatingData> managerialGoalRating = new List<RatingData>();

			try
			{
				using (_session = AmsDatabaseHelper.Create().Session)
				{
					#region Managerial Goal Rating

					bool employee = _session.CreateCriteria<Employee>()
						.List<Employee>()
						.Any(emp => emp.Id.Equals(empRating.Rater) && emp.ReportingManager.Id.Equals(empRating.Ratee));

					if (employee == true)
					{
						IQuery queryData = _session.CreateSQLQuery(
							$"SELECT rt.Id, eg.Id as GoalId, eg.Goal, eg.StartDate, eg.EndDate, eg.GoalStatus, eg.Weightage, rt.Rate, rt.Comment  FROM ManagerialEmployeeGoal eg left Join Rating rt on eg.Id=rt.GoalId and rt.GoalType={(int)GoalType.Managerial} and rt.Rater = {(int)empRating.Rater} WHERE eg.Employee={empRating.Ratee} and eg.ReviewCycle={empRating.Cycle} and eg.GoalStatus=2");
						var mglists = queryData.List();

						foreach (object[] mglist in mglists)
						{
							managerialGoalRating.Add(new RatingData
							{
								Id = mglist[0] != null ? Convert.ToInt32(mglist[0]) : 0,
								GoalId = Convert.ToInt32(mglist[1]),
								Goal = Convert.ToString(mglist[2]),
								GoalType = GoalType.Managerial,
								StartDate = Convert.ToDateTime(mglist[3]),
								EndDate = Convert.ToDateTime(mglist[4]),
								Status = Convert.ToString(mglist[5]),
								Weightage = Convert.ToInt32(mglist[6]),
								Rating = mglist[0] != null ? (float)mglist[7] : 0,
								Comment = mglist[0] != null ? Convert.ToString(mglist[8]) : ""
							});
						}
					}
					else
					{
						throw new Exception("Manager not belongs to the employee");
					}

					#endregion
				}
			}
			catch (Exception e)
			{
				managerialGoalRating = null;
			}
			return managerialGoalRating;
		}

		public static bool AddRating(T item)
		{
			bool status = false;
			try
			{
				Type type = typeof(T);

				using (_session = AmsDatabaseHelper.Create().Session)
				{
					ValidationContext context = new ValidationContext(item, null, null);
					List<ValidationResult> results = new List<ValidationResult>();
					bool valid = Validator.TryValidateObject(item, context, results, true);

					if (!valid)
					{
						throw new Exception(results.Count > 0 ? results[0].ToString() : "");
					}

					using (var transaction = _session.BeginTransaction())
					{
						Rating rating = item as Rating;

						_session.Save(rating);

						transaction.Commit();
						status = true;
					}
				}
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				status = false;
			}
			return status;
		}

		public static bool UpdateRating(T item)
		{
			bool status = false;
			try
			{
				Type type = typeof(T);

				using (_session = AmsDatabaseHelper.Create().Session)
				{
					ValidationContext context = new ValidationContext(item, null, null);
					List<ValidationResult> results = new List<ValidationResult>();
					bool valid = Validator.TryValidateObject(item, context, results, true);

					if (!valid)
					{
						throw new Exception(results.Count > 0 ? results[0].ToString() : "");
					}

					using (var transaction = _session.BeginTransaction())
					{
						Rating rating = item as Rating;

						_session.Update(rating);

						transaction.Commit();
						status = true;
					}
				}
			}
			catch (Exception ex)
			{
				var message = ex.Message;
				status = false;
			}
			return status;
		}

		//public static RatingData GoalRatingData(GoalRating goalRating)
		//{
		//	RatingData data = new RatingData();
		//	try
		//	{
		//		using (_session = AmsDatabaseHelper.Create().Session)
		//		{
		//			int goalId = 0;

		//			if (goalRating.GoalType.Equals((int)GoalType.Employee))
		//			{
		//				EmployeeGoal eg = _session.CreateCriteria<EmployeeGoal>()
		//				.List<EmployeeGoal>().FirstOrDefault(e => e.Id.Equals(goalRating.GoalId));

		//				if (eg != null)
		//				{
		//					var datagoalId = _session.CreateCriteria<EmployeeGoal>()
		//					.List<EmployeeGoal>().Where(e => e.Goal.Equals(eg.Goal) && e.Status.Id.Equals(2));
		//					goalId = datagoalId.FirstOrDefault(e => e.Employee.Id.Equals(goalRating.Ratee) && e.Reviewer.Id.Equals(goalRating.Rater)).Id;
		//				}
		//			}
		//			else
		//			{
		//				goalId = goalRating.GoalId;
		//			}

		//			Rating rating = _session.CreateCriteria<Rating>()
		//				.List<Rating>()
		//				.FirstOrDefault(rate => rate.GoalId.Equals(goalId) && rate.Ratee.Id.Equals(goalRating.Ratee) && (int)rate.GoalType == goalRating.GoalType && rate.Rater.Id.Equals(goalRating.Rater));

		//			if (rating != null)
		//			{
		//				data.Id = rating.Id;
		//				data.GoalId = rating.GoalId;
		//				data.GoalType = rating.GoalType;
		//				data.Rating = rating.Rate;
		//				data.Comment = rating.Comment;
		//			}
		//			else
		//			{
		//				data = null;
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		var message = ex.Message;
		//		data = null;
		//	}
		//	return data;
		//}
	}

	public class RatingData
	{
		public int Id { get; set; }
		public int GoalId { get; set; }
		public string Goal { get; set; }
		public GoalType GoalType { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string Status { get; set; }
		public int Weightage { get; set; }
		public float Rating { get; set; }
		public string Comment { get; set; }
		public float SelfRating { get; set; }
		public string SelfComment { get; set; }

        public string Reviewer { get; set; }
    }

	public class EmployeeRating
	{
		public int Rater { get; set; }
		public int Ratee { get; set; }
		public int Designation { get; set; }
		public int Organization { get; set; }
		public int Cycle { get; set; }
        public string UserType { get; set; }
	}

	public class GoalRating
	{
		public int Rater { get; set; }
		public int Ratee { get; set; }
		public int GoalId { get; set; }
		public int GoalType { get; set; }
	}
}