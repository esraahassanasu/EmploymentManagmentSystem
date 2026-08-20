using System;
using System.Collections.Generic;
namespace EmploymentManagmentSystem.Models
{
    public class LeaveRequest
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public bool IsApproved { get; set; }

        public LeaveRequest(int employeeId, string employeeName, DateTime startDate, DateTime endDate, string reason)
        {
            EmployeeId = employeeId;
            EmployeeName = employeeName;
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
            IsApproved = false;
        }
    }
}