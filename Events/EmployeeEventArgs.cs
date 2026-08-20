using System;
using EmploymentManagmentSystem.Models;

namespace EmploymentManagmentSystem.Events
{
    public class EmployeeEventArgs : EventArgs
    {
        public Employee Employee { get; }
        public DateTime EventTime { get; }
        public string ExtraInfo { get; }

        public EmployeeEventArgs(Employee employee, string extraInfo = "")
        {
            Employee = employee;
            EventTime = DateTime.Now;
            ExtraInfo = extraInfo;
        }
    }
}
