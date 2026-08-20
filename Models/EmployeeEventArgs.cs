using System;
using System.Collections.Generic;
namespace EmploymentManagmentSystem.Models
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

    public class BudgetAlertEventArgs : EventArgs
    {
        public int DepartmentId { get; }
        public string DepartmentName { get; }
        public decimal CurrentBudget { get; }
        public decimal BudgetLimit { get; }
        public DateTime AlertTime { get; }

        public BudgetAlertEventArgs(int departmentId, string departmentName, decimal currentBudget, decimal budgetLimit)
        {
            DepartmentId = departmentId;
            DepartmentName = departmentName;
            CurrentBudget = currentBudget;
            BudgetLimit = budgetLimit;
            AlertTime = DateTime.Now;
        }
    }
}