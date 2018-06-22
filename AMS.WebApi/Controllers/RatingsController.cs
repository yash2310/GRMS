using System.Collections;
using AMS.Infrastructure.Repository;
using AMS.WebApi.ViewModels;
using EmployeeRating = AMS.Infrastructure.Repository.EmployeeRating;

namespace AMS.WebApi.Controllers
{
    #region Namespaces

    using AMS.ApplicationCore.Entities;
    using AMS.ApplicationCore.Entities.Security;
    using System;
    using System.Collections.Generic;
    using System.Web.Http;

    #endregion

    //[Authorize]
    [RoutePrefix("api/ratings")]
    public class RatingsController : ApiController
    {
	    ///<summary>
	    ///Add Self/Team/Manager Rating
	    ///</summary>
	    [HttpPost]
	    [Route("add")]
	    public string Create([FromBody] ReporteeRatingData ratingData)
	    {
		    string result = "";
		    bool flag = false;

			try
			{
				if (Utilities.Common.GetSettingData().Rate.Equals(false))
					throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

				if (ratingData.GoalType.Equals((int)GoalType.Employee) && ratingData.TypeId.Equals(0) &&
					ratingData.RateId.Equals(0))
				{
					if (ratingData.Rater.Equals(ratingData.Ratee))
						flag = EmployeeRepository.GetEmployeeById(ratingData.Ratee).Id.Equals(ratingData.Rater);
					else
						flag = EmployeeRepository.GetEmployeeById(ratingData.Ratee) != null;
				}
				else if (ratingData.GoalType.Equals((int)GoalType.Managerial) && ratingData.TypeId.Equals(0) && ratingData.RateId.Equals(0))
				{
					flag = EmployeeRepository.GetEmployeeById(ratingData.Rater).ReportingManager.Id.Equals(ratingData.Ratee);
				}
				else if (ratingData.GoalType.Equals((int)GoalType.Designation) && ratingData.RateId.Equals(0))
				{
					flag = EmployeeRepository.GetEmployeeById(ratingData.Ratee).Designation.Id.Equals(ratingData.TypeId);
				}
				else if (ratingData.GoalType.Equals((int)GoalType.Organization) && ratingData.RateId.Equals(0))
				{
					flag = EmployeeRepository.GetEmployeeById(ratingData.Ratee).Organization.Id.Equals(ratingData.TypeId);
				}

				if (flag)
				{
					Rating rating = new Rating
					{
						Id = ratingData.RateId,
						GoalId = ratingData.GoalId,
						GoalType = (GoalType)ratingData.GoalType,
						Rate = ratingData.Rate,
						Comment = ratingData.Comment,
						Ratee = EmployeeRepository.GetEmployeeById(ratingData.Ratee),
						Rater = EmployeeRepository.GetEmployeeById(ratingData.Rater),
						CreatedBy = ratingData.Rater,
						CreatedOn = DateTime.Now
					};

					bool status = RatingRepository<Rating>.AddRating(rating);

					if (status)
					{
						result = "Success";
					}
					else
					{
						result = "Failed";
					}
				}
				else
				{
					result = "Failed";
				}
			}
			catch (Exception e)
			{
				result = "Failed";
			}
		    return result;
	    }

	    ///<summary>
	    ///Update Self/Team/Manager Rating
	    ///</summary>
	    [HttpPost]
	    [Route("update")]
	    public string Update([FromBody] ReporteeRatingData ratingData)
	    {
		    string result = "";
		    bool flag = false;

		    try
			{
				if (Utilities.Common.GetSettingData().Rate.Equals(false))
					throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

				if (ratingData.GoalType.Equals((int)GoalType.Employee) && ratingData.TypeId.Equals(0) &&
					!ratingData.RateId.Equals(0))
				{
					if (ratingData.Rater.Equals(ratingData.Ratee))
						flag = EmployeeRepository.GetEmployeeById(ratingData.Ratee).Id.Equals(ratingData.Rater);
					else
						flag = EmployeeRepository.GetEmployeeById(ratingData.Ratee) != null;
				}
				else if (ratingData.GoalType.Equals((int)GoalType.Managerial) && ratingData.TypeId.Equals(0) &&
						 !ratingData.RateId.Equals(0))
				{
					flag = EmployeeRepository.GetEmployeeById(ratingData.Rater).ReportingManager.Id.Equals(ratingData.Ratee);
				}
				else if (ratingData.GoalType.Equals((int)GoalType.Designation) && !ratingData.RateId.Equals(0))
				{
					flag = EmployeeRepository.GetEmployeeById(ratingData.Ratee).Designation.Id.Equals(ratingData.TypeId);
				}
				else if (ratingData.GoalType.Equals((int)GoalType.Organization) && !ratingData.RateId.Equals(0))
				{
					flag = EmployeeRepository.GetEmployeeById(ratingData.Ratee).Organization.Id.Equals(ratingData.TypeId);
				}
				else if (ratingData.GoalType.Equals((int)GoalType.Employee) && ratingData.TypeId.Equals(0) &&
						 !ratingData.RateId.Equals(0))
				{
					flag = EmployeeRepository.GetEmployeeById(ratingData.Ratee).Id.Equals(ratingData.Rater);
				}

			    if (flag)
			    {
				    Rating rating = new Rating
				    {
					    Id = ratingData.RateId,
					    GoalId = ratingData.GoalId,
					    GoalType = (GoalType) ratingData.GoalType,
					    Rate = ratingData.Rate,
					    Comment = ratingData.Comment,
						Ratee = EmployeeRepository.GetEmployeeById(ratingData.Ratee),
						Rater = EmployeeRepository.GetEmployeeById(ratingData.Rater),
						UpdatedBy = ratingData.Rater,
					    UpdatedOn = DateTime.Now
				    };

				    bool status = RatingRepository<Rating>.UpdateRating(rating);

				    if (status)
				    {
					    result = "Success";
				    }
				    else
				    {
					    result = "Failed";
				    }
			    }
			    else
			    {
				    result = "Failed";
			    }
		    }
		    catch (Exception e)
		    {
			    result = "Failed";
		    }
		    return result;
	    }

	    ///<summary>
	    ///To Get Employee rating by Rater and Ratee
	    ///</summary>
	    [HttpPost]
	    [Route("employee")]
	    public EmployeeGoalRating GetRatingByEmpId([FromBody] EmployeeRating employeeRating)
        {
		    try
		    {
			    EmployeeGoalRating employeeGoalRating = new EmployeeGoalRating();
			    ArrayList listGoalsRating = new ArrayList();
			    listGoalsRating = RatingRepository<EmployeeRating>.GetAllGoalsRating(employeeRating);

			    if (listGoalsRating != null && listGoalsRating.Count > 0)
			    {
				    employeeGoalRating.EmployeeGoalsRating = listGoalsRating[0] as List<RatingData>;
				    employeeGoalRating.DesignationGoalsRating = listGoalsRating[1] as List<RatingData>;
				    employeeGoalRating.OrganizationGoalsRating = listGoalsRating[2] as List<RatingData>;
			    }
			    return employeeGoalRating;
		    }
		    catch (Exception e)
		    {
			    return null;
		    }
		}

        ///<summary>
	    ///To all ratings for an Employee
	    ///</summary>
	    [HttpGet]
        [Route("employee/{empId}/ratings")]
        public OverallRatingData GetAllRatingByEmpId(int empId)
        {
            try
            {
                OverallRatingData overallRating = new OverallRatingData();
                ArrayList listGoalsRating = new ArrayList();
                listGoalsRating = RatingRepository<EmployeeRating>.GetCompleteRatingByEmpId(empId);

                if (listGoalsRating != null && listGoalsRating.Count > 0)
                {
                    overallRating.EmployeeGoalsRating = listGoalsRating[0] as List<RatingData>;
                    overallRating.ManagerGoalsRating = listGoalsRating[1] as List<RatingData>;
                    overallRating.DesignationGoalsRating = listGoalsRating[2] as List<RatingData>;
                    overallRating.OrganizationGoalsRating = listGoalsRating[3] as List<RatingData>;
                }
                return overallRating;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        ///<summary>
	    ///To all ratings for an Employee
	    ///</summary>
	    [HttpGet]
        [Route("employee/{empId}/{userRole}/finalrating")]
        public double GetFinalRatingByEmpId(int empId, string userRole)
        {
            double finalRating = 0;
            try
            {
                finalRating = RatingRepository<EmployeeRating>.GetFinalRatingByEmpId(empId, userRole);
                return finalRating;
            }
            catch (Exception e)
            {
                finalRating = 0;
                return finalRating;
            }
        }

        ///<summary>
        ///To Get Manager rating
        ///</summary>
        [HttpPost]
	    [Route("manager")]
	    public List<RatingData> GetManagerialRating([FromBody] EmployeeRating employeeRating)
	    {
		    try
		    {
			    List<RatingData> listGoalsRating = new List<RatingData>();
			    listGoalsRating = RatingRepository<EmployeeRating>.GetManagerialGoalsRating(employeeRating);
				
			    return listGoalsRating;
		    }
		    catch (Exception e)
		    {
			    return null;
		    }
		}

        ///<summary>
        ///To Get Self Goal Rating Data
        ///</summary>
        //[HttpPost]
        //[Route("goalrating")]
        //public RatingData GetSelfGoalRating([FromBody] GoalRating goalRating)
        //{
        //	try
        //	{
        //		return RatingRepository<GoalRating>.GoalRatingData(goalRating);
        //	}
        //	catch (Exception e)
        //	{
        //		return null;
        //	}
        //}

        ///<summary>
        ///To Get Immediate Team Rating
        ///</summary>
        //[HttpGet]
        //[Route("getimmediateteam/{empId}")]
        //public IList<Rating> GetImmediateTeamRating(int empId)
        //{
        //    IList<Rating> ratingList = new List<Rating>();
        //    Rating rating = new Rating()
        //    {
        //        Rater = new Employee() { Name = "Anshu" },
        //        Ratee = new Employee() { Name = "Manoj" },
        //        GoalId = 1,
        //        Rate = 80,
        //        Comment = "Delivered the AMS on time",

        //        CreatedOn = Convert.ToDateTime("20 Feb 2018"),
        //        CreatedBy = 1,
        //        UpdatedOn = Convert.ToDateTime("20 Feb 2018"),
        //        UpdatedBy = 2
        //    };

        //    ratingList.Add(rating);

        //    return ratingList;
        //}

        ///<summary>
        ///To Get Complete Team Rating
        ///</summary>
        //[HttpGet]
        //[Route("GetTeam/{empId}")]
        //public IList<Rating> GetCompleteTeamRating(int empId)
        //{
        //    IList<Rating> ratingList = new List<Rating>();
        //    Rating rating = new Rating()
        //    {
        //        Rater = new Employee() { Name = "Anshu" },
        //        Ratee = new Employee() { Name = "Manoj" },
        //        GoalId = 1,
        //        Rate = 80,
        //        Comment = "Delivered the AMS on time",

        //        CreatedOn = Convert.ToDateTime("20 Feb 2018"),
        //        CreatedBy = 1,
        //        UpdatedOn = Convert.ToDateTime("20 Feb 2018"),
        //        UpdatedBy = 2
        //    };

        //    ratingList.Add(rating);

        //    return ratingList;
        //}

        ///<summary>
        ///To Get All Employees Rating
        ///</summary>
        //[HttpGet]
        //[Route("GetAll/")]
        //public IList<Rating> GetAllRating()
        //{
        //    IList<Rating> ratingList = new List<Rating>();
        //    Rating rating = new Rating()
        //    {
        //        Rater = new Employee() { Name = "Anshu" },
        //        Ratee = new Employee() { Name = "Manoj" },
        //        GoalId = 1,
        //        Rate = 80,
        //        Comment = "Delivered the AMS on time",

        //        CreatedOn = Convert.ToDateTime("20 Feb 2018"),
        //        CreatedBy = 1,
        //        UpdatedOn = Convert.ToDateTime("20 Feb 2018"),
        //        UpdatedBy = 2
        //    };

        //    ratingList.Add(rating);

        //    return ratingList;
        //}

        ///<summary>
        ///To Get Rating By EmployeeId
        ///</summary>
        [HttpGet]
        [Route("employees/{empId}/ratings")]
        public IList<Rating> GetAllRatingByEmployeeId(int empId)
        {
            IList<Rating> ratingList = new List<Rating>();
            Rating rating = new Rating()
            {
                Rater = new Employee() { Name = "Anshu" },
                Ratee = new Employee() { Name = "Manoj" },
                GoalId = 1,
                Rate = 80,
                Comment = "Delivered the AMS on time",

                CreatedOn = Convert.ToDateTime("20 Feb 2018"),
                CreatedBy = 1,
                UpdatedOn = Convert.ToDateTime("20 Feb 2018"),
                UpdatedBy = 2
            };

            ratingList.Add(rating);

            return ratingList;
        }
    }
}