namespace AMS.Infrastructure
{
    public class Enumeration
    {
        public enum UserRoles
        {
            SuperAdmin,
            LeadershipTeam,
            Manager,
            Employee,
            HR
        };

        public enum EmployeeWeightage
        {
            EmployeeWeight = 90,
            ManagerialWeight = 0,
            DesignationWeight = 10,
            OrganizationWeight = 0
        }; 

        public enum ManagerWeightage
        {
            EmployeeWeight = 80,
            ManagerialWeight = 10,
            DesignationWeight = 10,
            OrganizationWeight = 0
        };
    }
}