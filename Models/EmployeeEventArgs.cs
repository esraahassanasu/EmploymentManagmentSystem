using System;

namespace EmploymentManagmentSystem.Models
{
    public class EmployeeEventArgs : EventArgs
    {
        public Employee Employee { get; }
        public DateTime EventTime { get; }

        public EmployeeEventArgs(Employee employee)
        {
            Employee = employee;
            EventTime = DateTime.Now;
        }
    }
}