using System;
using System.Collections.Generic;
namespace EmploymentManagmentSystem.Models
{
    public delegate bool EmployeeFilter(Employee employee);
    public delegate int EmployeeComparer(Employee first, Employee second);
}