using System;

namespace EmploymentManagmentSystem.Events
{
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
