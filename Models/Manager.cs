using System;
namespace EmploymentManagmentSystem.Models
{
    public class Manager : Employee
    {
        public List<Employee> TeamMembers { get; set; }
        
        public  Manager()
        {
            this.TeamMembers = new List<Employee>();
           
        }
        public Manager(int id, string firstName, string lastName, string email, 
            string phoneNumber, DateTime dateOfBirth, DateTime hireDate, 
            int departmentId, decimal salary)
            : base(id, firstName, lastName, email, phoneNumber, dateOfBirth, 
                   hireDate, departmentId, salary)
        {
            TeamMembers = new List<Employee>();
        }
                public List<string> GetTeamSkills()
        {
            HashSet<string> uniqueTeamSkills = new HashSet<string>();

            foreach (var member in TeamMembers)
            {
                if (member.Skills != null)
                {
                    foreach (var skill in member.Skills)
                    {
                        uniqueTeamSkills.Add(skill.ToLower());
                    }
                }
            }

            return new List<string>(uniqueTeamSkills);
        }

    }
}
