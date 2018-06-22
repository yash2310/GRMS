using System.Linq;

namespace AMS.WebApi.Controllers
{
	using AMS.ApplicationCore.Entities;
	#region Namespaces

	using AMS.ApplicationCore.Entities.Security;
	using AMS.Infrastructure.Repository;
	using AMS.WebApi.Models;
	using AMS.WebApi.ViewModels;
	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Web.Http;
	using System.Web.Http.Cors;

	#endregion

	//[Authorize(Roles = "Super Admin,Leadership Team,Manager")]
	[RoutePrefix("api/reportees")]
	public class ReporteesController : ApiController
	{
		///<summary>
		///To get all direct reportees
		///</summary>
		[HttpGet]
		[Route("employees/{empId}/reportees")]
		public List<ReporteeData> GetReporteesByEmpId(int empId)
		{
			//IHttpActionResult response;
			//var loginResponse = new LoginResponse { };
			try
			{
				List<ReporteeData> reporteeList = new List<ReporteeData>();

				List<Employee> reporteess = EmployeeRepository.GetDirectReportee(empId);

				foreach (var rep in reporteess)
				{
					ReporteeData reportee = new ReporteeData();
					int goalCount = rep.EmployeeGoals.Count + rep.ManagerialEmployeeGoals.Count;
					reportee.Name = rep.Name;
					reportee.EmployeeNo = rep.EmployeeNo;
					reportee.Designation = new Data { Id = rep.Designation.Id, Name = rep.Designation.Name };
					reportee.EmailId = rep.Email;
					reportee.GoalsCount = rep.EmployeeGoals.Count + rep.ManagerialEmployeeGoals.Count + rep.OrganizationGoals.Count + rep.DesignationGoals.Count; ;
					reportee.ReporteeId = rep.Id;

					List<Data> roleList = new List<Data>();
					foreach (var role in rep.Roles)
					{
						roleList.Add(new Data { Id = role.Id, Name = role.Name });
					}
					reportee.Role = roleList;


					if (goalCount > 0)
					{
						if (reportee.Role.AsEnumerable().Any(r => r.Name.Equals("Employee")) &&
							!reportee.Role.AsEnumerable().Any(r => r.Name.Equals("Manager")))
						{
							foreach (EmployeeGoal goal in rep.EmployeeGoals)
							{
								if (goal.Status.Id == 1)
									reportee.GoalStatus = "Draft";
								else if (goal.Status.Id == 2)
									reportee.GoalStatus = "Finalized";
								else
									reportee.GoalStatus = "Not Set";
								break;
							}
						}
						else if (reportee.Role.AsEnumerable().Any(r => r.Name.Equals("Manager")))
						{
							int empStatus = 0;
							int mgrStatus = 0;

							foreach (EmployeeGoal goal in rep.EmployeeGoals)
							{
								if (goal.Status.Id == 1)
									empStatus = 1;
								else if (goal.Status.Id == 2)
									empStatus = 2;
								else
									empStatus = 0;
								break;
							}

							foreach (ManagerialEmployeeGoal goal in rep.ManagerialEmployeeGoals)
							{
								if (goal.Status.Id == 1)
									mgrStatus = 1;
								else if (goal.Status.Id == 2)
									mgrStatus = 2;
								else
									mgrStatus = 0;
								break;
							}

							if (empStatus == 2 && mgrStatus == 2)
							{
								reportee.GoalStatus = "Finalized";
							}
							else if (empStatus == 0 && mgrStatus == 0)
							{
								reportee.GoalStatus = "Not Set";
							}
							else
							{
								reportee.GoalStatus = "Draft";
							}
						}
					}
					else
					{
						reportee.GoalStatus = "Not Set";
					}
					reporteeList.Add(reportee);
				}

				return reporteeList;
			}
			catch (Exception e)
			{
				return new List<ReporteeData>();
			}
		}

        ///<summary>
		///To get all reportees in downline
		///</summary>
		[HttpGet]
        [Route("employees/{empId}/downline")]
        public List<ReporteeData> GetDownlineReporteesByEmpId(int empId)
        {
            //IHttpActionResult response;
            //var loginResponse = new LoginResponse { };
            try
            {
                List<ReporteeData> reporteeList = new List<ReporteeData>();

                List<Employee> reporteess = EmployeeRepository.GetDownlineReportee(empId);

                foreach (var rep in reporteess)
                {
                    ReporteeData reportee = new ReporteeData();
                    int goalCount = rep.EmployeeGoals.Count + rep.ManagerialEmployeeGoals.Count;
                    reportee.Name = rep.Name;
                    reportee.EmployeeNo = rep.EmployeeNo;
                    reportee.Designation = new Data { Id = rep.Designation.Id, Name = rep.Designation.Name };
                    reportee.EmailId = rep.Email;
                    reportee.GoalsCount = rep.EmployeeGoals.Count + rep.ManagerialEmployeeGoals.Count + rep.OrganizationGoals.Count + rep.DesignationGoals.Count; ;
                    reportee.ReporteeId = rep.Id;

                    List<Data> roleList = new List<Data>();
                    foreach (var role in rep.Roles)
                    {
                        roleList.Add(new Data { Id = role.Id, Name = role.Name });
                    }
                    reportee.Role = roleList;

                    if (goalCount > 0)
                    {
                        if (reportee.Role.AsEnumerable().Any(r => r.Name.Equals("Employee")) &&
                            !reportee.Role.AsEnumerable().Any(r => r.Name.Equals("Manager")))
                        {
                            foreach (EmployeeGoal goal in rep.EmployeeGoals)
                            {
                                if (goal.Status.Id == 1)
                                    reportee.GoalStatus = "Draft";
                                else if (goal.Status.Id == 2)
                                    reportee.GoalStatus = "Finalized";
                                else
                                    reportee.GoalStatus = "Not Set";
                                break;
                            }
                        }
                        else if (reportee.Role.AsEnumerable().Any(r => r.Name.Equals("Manager")))
                        {
                            int empStatus = 0;
                            int mgrStatus = 0;

                            foreach (EmployeeGoal goal in rep.EmployeeGoals)
                            {
                                if (goal.Status.Id == 1)
                                    empStatus = 1;
                                else if (goal.Status.Id == 2)
                                    empStatus = 2;
                                else
                                    empStatus = 0;
                                break;
                            }

                            foreach (ManagerialEmployeeGoal goal in rep.ManagerialEmployeeGoals)
                            {
                                if (goal.Status.Id == 1)
                                    mgrStatus = 1;
                                else if (goal.Status.Id == 2)
                                    mgrStatus = 2;
                                else
                                    mgrStatus = 0;
                                break;
                            }

                            if (empStatus == 2 && mgrStatus == 2)
                            {
                                reportee.GoalStatus = "Finalized";
                            }
                            else if (empStatus == 0 && mgrStatus == 0)
                            {
                                reportee.GoalStatus = "Not Set";
                            }
                            else
                            {
                                reportee.GoalStatus = "Draft";
                            }
                        }
                    }
                    else
                    {
                        reportee.GoalStatus = "Not Set";
                    }
                    reporteeList.Add(reportee);
                }

                return reporteeList;
            }
            catch (Exception e)
            {
                return new List<ReporteeData>();
            }
        }

        ///<summary>
		///To get all reviewees by employee id and employee role
		///</summary>
		[HttpGet]
		[Route("employees/{empId}/{empRole}/reviewee")]
		public RevieweeData GetRevieweeById(int empId,string empRole)
		{
			try
			{
				if (Utilities.Common.GetSettingData().Rate.Equals(false) || Utilities.Common.GetSettingData().Review.Equals(false))
					throw new HttpResponseException(System.Net.HttpStatusCode.BadRequest);

				RevieweeData reviewee = new RevieweeData();

				List<ReporteeData> reporteeData = new List<ReporteeData>();
				List<ReporteeData> revieweeData = new List<ReporteeData>();
                List<ReporteeData> employeeData = new List<ReporteeData>();

                List<Employee> reviewees = EmployeeRepository.GetAllReviewee(empId);

                foreach (var rep in reviewees)
				{
					ReporteeData reportee = new ReporteeData();

					reportee.Name = rep.Name;
					reportee.EmployeeNo = rep.EmployeeNo;
					reportee.Designation = new Data { Id = rep.Designation.Id, Name = rep.Designation.Name };
					reportee.EmailId = rep.Email;
					reportee.GoalsCount = 0;
					reportee.ReporteeId = rep.Id;

					List<Data> roleList = new List<Data>();
					foreach (var role in rep.Roles)
					{
						roleList.Add(new Data { Id = role.Id, Name = role.Name });
					}
					reportee.Role = roleList;

					if (rep.ReportingManager.Id.Equals(empId))
					{
						reporteeData.Add(reportee);
					}
					else
					{
						revieweeData.Add(reportee);
					}
				}

                if (empRole == "Leadership Team" || empRole == "HR" || empRole == "Super Admin")
                {
                    List<Employee> allEmployeeList = EmployeeRepository.GetAllEmployees();

                    foreach (var emp in allEmployeeList)
                    {
                        ReporteeData employee = new ReporteeData();

                        employee.Name = emp.Name;
                        employee.EmployeeNo = emp.EmployeeNo;
                        employee.Designation = new Data { Id = emp.Designation.Id, Name = emp.Designation.Name };
                        employee.EmailId = emp.Email;
                        employee.ReporteeId = emp.Id;

                        employeeData.Add(employee);
                    }
                }

				reviewee.Reportee = reporteeData;
				reviewee.Reviewee = revieweeData;
                reviewee.Employee = employeeData;

                return reviewee;
			}
			catch (Exception e)
			{
				return new RevieweeData();
			}
		}

  //      ///<summary>
		/////To get all reportees hierarchy
		/////</summary>
		//[HttpGet]
  //      [Route("employees/{empId}/reportees")]
  //      public List<ReporteeData> GetReporteesByEmailId(int empId)
  //      {
  //          //IHttpActionResult response;
  //          //var loginResponse = new LoginResponse { };
  //          try
  //          {
  //              List<ReporteeData> reporteeList = new List<ReporteeData>();

  //              List<Employee> reporteess = EmployeeRepository.GetAllReportee(empId);

  //              foreach (var rep in reporteess)
  //              {
  //                  ReporteeData reportee = new ReporteeData();
  //                  int goalCount = rep.EmployeeGoals.Count + rep.ManagerialEmployeeGoals.Count;
  //                  reportee.Name = rep.Name;
  //                  reportee.EmployeeNo = rep.EmployeeNo;
  //                  reportee.Designation = new Data { Id = rep.Designation.Id, Name = rep.Designation.Name };
  //                  reportee.EmailId = rep.Email;
  //                  reportee.GoalsCount = rep.EmployeeGoals.Count + rep.ManagerialEmployeeGoals.Count + rep.OrganizationGoals.Count + rep.DesignationGoals.Count; ;
  //                  reportee.ReporteeId = rep.Id;

  //                  List<Data> roleList = new List<Data>();
  //                  foreach (var role in rep.Roles)
  //                  {
  //                      roleList.Add(new Data { Id = role.Id, Name = role.Name });
  //                  }
  //                  reportee.Role = roleList;


  //                  if (goalCount > 0)
  //                  {
  //                      if (reportee.Role.AsEnumerable().Any(r => r.Name.Equals("Employee")) &&
  //                          !reportee.Role.AsEnumerable().Any(r => r.Name.Equals("Manager")))
  //                      {
  //                          foreach (EmployeeGoal goal in rep.EmployeeGoals)
  //                          {
  //                              if (goal.Status.Id == 1)
  //                                  reportee.GoalStatus = "Draft";
  //                              else if (goal.Status.Id == 2)
  //                                  reportee.GoalStatus = "Finalized";
  //                              else
  //                                  reportee.GoalStatus = "Not Set";
  //                              break;
  //                          }
  //                      }
  //                      else if (reportee.Role.AsEnumerable().Any(r => r.Name.Equals("Manager")))
  //                      {
  //                          int empStatus = 0;
  //                          int mgrStatus = 0;

  //                          foreach (EmployeeGoal goal in rep.EmployeeGoals)
  //                          {
  //                              if (goal.Status.Id == 1)
  //                                  empStatus = 1;
  //                              else if (goal.Status.Id == 2)
  //                                  empStatus = 2;
  //                              else
  //                                  empStatus = 0;
  //                              break;
  //                          }

  //                          foreach (ManagerialEmployeeGoal goal in rep.ManagerialEmployeeGoals)
  //                          {
  //                              if (goal.Status.Id == 1)
  //                                  mgrStatus = 1;
  //                              else if (goal.Status.Id == 2)
  //                                  mgrStatus = 2;
  //                              else
  //                                  mgrStatus = 0;
  //                              break;
  //                          }

  //                          if (empStatus == 2 && mgrStatus == 2)
  //                          {
  //                              reportee.GoalStatus = "Finalized";
  //                          }
  //                          else if (empStatus == 0 && mgrStatus == 0)
  //                          {
  //                              reportee.GoalStatus = "Not Set";
  //                          }
  //                          else
  //                          {
  //                              reportee.GoalStatus = "Draft";
  //                          }
  //                      }
  //                  }
  //                  else
  //                  {
  //                      reportee.GoalStatus = "Not Set";
  //                  }
  //                  reporteeList.Add(reportee);
  //              }

  //              return reporteeList;
  //          }
  //          catch (Exception e)
  //          {
  //              return new List<ReporteeData>();
  //          }
  //      }

    }
}