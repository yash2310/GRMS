using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMS.WebApi.Utilities
{
    public class Enumeration
    {
        enum UserRoles
        {
            SuperAdmin,
            LeadershipTeam,
            Manager,
            Employee,
            HR
        };

        enum EmployeeWeightage
        {
            EmployeeWeight = 90,
            ManagerialWeight = 0,
            DesignationWeight = 10,
            OrganizationWeight = 0
        };

        enum ManagerWeightage
        {
            EmployeeWeight = 80,
            ManagerialWeight = 10,
            DesignationWeight = 10,
            OrganizationWeight = 0
        };
    }
}