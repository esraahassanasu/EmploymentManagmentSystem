using System;
using System.Collections.Generic;

namespace EmploymentManagmentSystem.Models
{
    public class Employee : IHasId
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }
        public EmployeeStatus Status { get; set; }
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; }
        public List<string> Skills { get; set; }
        public int PerformanceRating { get; set; }

        public Employee(int id, string firstName, string lastName, string email, string phoneNumber, DateTime dateOfBirth, DateTime hireDate, int departmentId, decimal salary)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            DateOfBirth = dateOfBirth;
            HireDate = hireDate;
            Status = EmployeeStatus.Pending;
            DepartmentId = departmentId;
            Salary = salary;
            Skills = new List<string>();
            PerformanceRating = 3;
        }
    }
}