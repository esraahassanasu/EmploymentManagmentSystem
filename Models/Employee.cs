using System;
namespace EmploymentManagmentSystem.Models
{
public class Employee
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
    public  Employee()
    {
        this.Skills = new List<string>();
        this.Status = EmployeeStatus.Pending;
            
        
    }

    public  Employee(int id, string firstName, string lastName, string email, string phoneNumber, DateTime dateOfBirth, DateTime hireDate, int departmentId, decimal salary, List<string> skills)
    {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Email = email;
        this.PhoneNumber = phoneNumber;
        this.DateOfBirth = dateOfBirth;
        this.HireDate = hireDate;
        this.Status = EmployeeStatus.Pending;
        this.DepartmentId = departmentId;
        this.Salary = salary;
        this.Skills = skills?? new List<string>();
    }
}
}