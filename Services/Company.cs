using System;
using System.Collections.Generic;
using EmploymentManagmentSystem.Models;

namespace EmploymentManagmentSystem.Services
{
    public class Company
    {  
        #region Constants
        private const string ERROR_EMPLOYEE_NOT_FOUND = "Employee not found!";
        private const string ERROR_DEPARTMENT_NOT_FOUND = "Department does not exist!";
        private const string ERROR_NOT_A_MANAGER = "Selected employee is not a manager!";
        private const string ERROR_ALREADY_IN_TEAM = "Employee is already in the team!";
        private const string ERROR_QUEUE_EMPTY = "Onboarding queue is empty!";
        private const string ERROR_NO_EMPLOYEES = "No employees registered yet.";
        private const string ERROR_NO_SKILLS = "No skills registered yet.";
        private const string ERROR_NO_ACTIONS = "No actions recorded yet.";
        private const string ERROR_UNDO_NOT_AVAILABLE = "No actions to undo!";
        #endregion

        #region Properties
        public List<Employee> ActiveEmployees { get; private set; }
        public Dictionary<int, Department> Departments { get; private set; }
        public Queue<Employee> OnboardingQueue { get; private set; }
        public Stack<string> ActionHistory { get; private set; }
        public HashSet<string> UniqueSkills { get; private set; }
        
        private Dictionary<int, Employee> _employeeLookup;
        private Stack<Action> _undoStack;
        
        private int _nextEmployeeId;
        private int _nextDepartmentId;

        public Company()
        {
            ActiveEmployees = new List<Employee>();
            Departments = new Dictionary<int, Department>();
            OnboardingQueue = new Queue<Employee>();
            ActionHistory = new Stack<string>();
            UniqueSkills = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _employeeLookup = new Dictionary<int, Employee>();
            _undoStack = new Stack<Action>();
            
            _nextEmployeeId = 1;
            _nextDepartmentId = 1;
        }
        #endregion

        #region Methods
        private void LogAction(string actionDescription)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ActionHistory.Push($"{timestamp} - {actionDescription}");
        }

        private bool TryGetEmployee(int id, out Employee employee)
        {
            return _employeeLookup.TryGetValue(id, out employee);
        }

        private bool DepartmentExists(int departmentId)
        {
            return Departments.ContainsKey(departmentId);
        }

        private string NormalizeSkill(string skill)
        {
            return skill.Trim().ToLower();
        }

        private string GetDepartmentName(int departmentId)
        {
            if (Departments.TryGetValue(departmentId, out Department dept))
            {
                return dept.Name;
            }
            return "Unknown Department";
        }

        public void AddDepartment(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ConsoleHelper.PrintError("Department name cannot be empty!");
                return;
            }

            int id = _nextDepartmentId++;
            var department = new Department(id, name, description);
            Departments.Add(id, department);
            
            LogAction($"Added new department: {name} (Id: {id})");
            
            _undoStack.Push(() => {
                Departments.Remove(id);
                _nextDepartmentId--;
            });
            
            ConsoleHelper.PrintSuccess($"Department '{name}' added successfully with Id: {id}");
        }

        public void DisplayAllDepartments()
        {
            ConsoleHelper.PrintHeader("All Departments");
            
            if (Departments.Count == 0)
            {
                Console.WriteLine("No departments registered yet.");
                return;
            }

            foreach (var dept in Departments.Values)
            {
                Console.WriteLine($"[{dept.Id}] {dept.Name}");
                if (!string.IsNullOrWhiteSpace(dept.Description))
                {
                    Console.WriteLine($"    Description: {dept.Description}");
                }
            }
        }

        public void DisplayDepartmentBudget(int departmentId)
        {
            if (!DepartmentExists(departmentId))
            {
                ConsoleHelper.PrintError(ERROR_DEPARTMENT_NOT_FOUND);
                return;
            }

            string deptName = GetDepartmentName(departmentId);
            ConsoleHelper.PrintHeader($"Budget Report: {deptName}");
            
            decimal totalBudget = 0;
            int employeeCount = 0;
            
            foreach (var emp in ActiveEmployees)
            {
                if (emp.DepartmentId == departmentId)
                {
                    totalBudget += emp.Salary;
                    employeeCount++;
                }
            }
            
            Console.WriteLine($"Department: {deptName}");
            Console.WriteLine($"Total Employees: {employeeCount}");
            Console.WriteLine($"Total Budget: {totalBudget:C}");
            Console.WriteLine($"Average Salary: {(employeeCount > 0 ? totalBudget / employeeCount : 0):C}");
        }

        public void DisplayAllDepartmentsBudget()
        {
            ConsoleHelper.PrintHeader("All Departments Budget Report");
            
            if (Departments.Count == 0)
            {
                Console.WriteLine("No departments registered yet.");
                return;
            }

            decimal companyTotalBudget = 0;
            
            foreach (var dept in Departments.Values)
            {
                decimal deptBudget = 0;
                int employeeCount = 0;
                
                foreach (var emp in ActiveEmployees)
                {
                    if (emp.DepartmentId == dept.Id)
                    {
                        deptBudget += emp.Salary;
                        employeeCount++;
                    }
                }
                
                companyTotalBudget += deptBudget;
                
                Console.WriteLine($"\n[{dept.Id}] {dept.Name}");
                Console.WriteLine($"  Employees: {employeeCount}");
                Console.WriteLine($"  Budget: {deptBudget:C}");
            }
            
            Console.WriteLine($"\n=================================");
            Console.WriteLine($"Total Company Budget: {companyTotalBudget:C}");
        }

        public void AddToOnboarding(string firstName, string lastName, string email, 
            string phoneNumber, DateTime dateOfBirth, int departmentId, decimal salary, 
            List<string> skills, bool isManager = false)
        {
            if (!DepartmentExists(departmentId))
            {
                ConsoleHelper.PrintError(ERROR_DEPARTMENT_NOT_FOUND);
                return;
            }

            if (salary < 0)
            {
                ConsoleHelper.PrintError("Salary cannot be negative!");
                return;
            }

            Employee newEmp;
            int id = _nextEmployeeId++;
            
            if (isManager)
            {
                newEmp = new Manager(id, firstName, lastName, email, phoneNumber, 
                    dateOfBirth, DateTime.Now, departmentId, salary, skills ?? new List<string>());
            }
            else
            {
                newEmp = new Employee(id, firstName, lastName, email, phoneNumber, 
                    dateOfBirth, DateTime.Now, departmentId, salary, skills ?? new List<string>());
            }
            
            OnboardingQueue.Enqueue(newEmp);
            _employeeLookup[id] = newEmp;
            
            LogAction($"Added {firstName} {lastName} to Onboarding queue (Id: {id})");
            
            _undoStack.Push(() => {
                _employeeLookup.Remove(id);
                _nextEmployeeId--;
            });
            
            ConsoleHelper.PrintSuccess($"{firstName} {lastName} added to Onboarding queue successfully.");
        }

        public void ProcessOnboarding()
        {
            if (OnboardingQueue.Count == 0)
            {
                ConsoleHelper.PrintError(ERROR_QUEUE_EMPTY);
                return;
            }

            Employee emp = OnboardingQueue.Dequeue();
            emp.Status = EmployeeStatus.Active;
            ActiveEmployees.Add(emp);
            
            LogAction($"Processed Onboarding for {emp.FirstName} {emp.LastName} (Id: {emp.Id})");
            ConsoleHelper.PrintSuccess($"{emp.FirstName} {emp.LastName} is now an active employee.");
        }

        public void ProcessAllOnboarding()
        {
            if (OnboardingQueue.Count == 0)
            {
                ConsoleHelper.PrintError(ERROR_QUEUE_EMPTY);
                return;
            }

            int count = OnboardingQueue.Count;
            while (OnboardingQueue.Count > 0)
            {
                ProcessOnboarding();
            }
            
            ConsoleHelper.PrintSuccess($"Successfully processed {count} employee(s) from Onboarding queue.");
        }

        public Employee FindEmployeeById(int id)
        {
            if (TryGetEmployee(id, out Employee employee))
            {
                return employee;
            }
            return null;
        }

        public Employee FindEmployeeByName(string firstName, string lastName)
        {
            foreach (var emp in ActiveEmployees)
            {
                if (emp.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase) && 
                    emp.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase))
                {
                    return emp;
                }
            }
            return null;
        }

        public void DisplayEmployeeDetails(int employeeId)
        {
            if (!TryGetEmployee(employeeId, out Employee emp))
            {
                ConsoleHelper.PrintError(ERROR_EMPLOYEE_NOT_FOUND);
                return;
            }

            ConsoleHelper.PrintHeader($"Employee Details: {emp.FirstName} {emp.LastName}");
            Console.WriteLine($"ID: {emp.Id}");
            Console.WriteLine($"Email: {emp.Email}");
            Console.WriteLine($"Phone: {emp.PhoneNumber}");
            Console.WriteLine($"Date of Birth: {emp.DateOfBirth:dd/MM/yyyy}");
            Console.WriteLine($"Hire Date: {emp.HireDate:dd/MM/yyyy}");
            Console.WriteLine($"Department: {GetDepartmentName(emp.DepartmentId)}");
            Console.WriteLine($"Salary: {emp.Salary:C}");
            Console.WriteLine($"Status: {emp.Status}");
            
            if (emp.Skills != null && emp.Skills.Count > 0)
            {
                Console.WriteLine($"Skills: {string.Join(", ", emp.Skills)}");
            }
            else
            {
                Console.WriteLine("Skills: None");
            }

            if (emp is Manager manager && manager.TeamMembers.Count > 0)
            {
                Console.WriteLine($"\nTeam Members ({manager.TeamMembers.Count}):");
                foreach (var member in manager.TeamMembers)
                {
                    Console.WriteLine($"  - {member.FirstName} {member.LastName} (Id: {member.Id})");
                }
            }
        }

        public void DisplayAllEmployees()
        {
            ConsoleHelper.PrintHeader("All Active Employees");
            
            if (ActiveEmployees.Count == 0)
            {
                Console.WriteLine(ERROR_NO_EMPLOYEES);
                return;
            }

            foreach (var emp in ActiveEmployees)
            {
                string role = emp is Manager ? "[Manager]" : "[Employee]";
                Console.WriteLine($"[{emp.Id}] {role} {emp.FirstName} {emp.LastName} - {GetDepartmentName(emp.DepartmentId)} - {emp.Salary:C}");
            }
        }

        public void AddTeamMember(int managerId, int employeeId)
        {
            if (!TryGetEmployee(managerId, out Employee managerEmp))
            {
                ConsoleHelper.PrintError("Manager not found!");
                return;
            }

            if (!TryGetEmployee(employeeId, out Employee teamMember))
            {
                ConsoleHelper.PrintError("Team member not found!");
                return;
            }

            if (!(managerEmp is Manager manager))
            {
                ConsoleHelper.PrintError(ERROR_NOT_A_MANAGER);
                return;
            }

            if (manager.TeamMembers.Contains(teamMember))
            {
                ConsoleHelper.PrintError(ERROR_ALREADY_IN_TEAM);
                return;
            }

            manager.TeamMembers.Add(teamMember);
            LogAction($"Added {teamMember.FirstName} {teamMember.LastName} to {manager.FirstName} {manager.LastName}'s team");
            
            _undoStack.Push(() => {
                manager.TeamMembers.Remove(teamMember);
            });
            
            ConsoleHelper.PrintSuccess("Team member added successfully.");
        }

        public void DisplayManagerTeamSkills(int managerId)
        {
            if (!TryGetEmployee(managerId, out Employee emp))
            {
                ConsoleHelper.PrintError(ERROR_EMPLOYEE_NOT_FOUND);
                return;
            }

            if (!(emp is Manager manager))
            {
                ConsoleHelper.PrintError(ERROR_NOT_A_MANAGER);
                return;
            }

            ConsoleHelper.PrintHeader($"Team Skills: {manager.FirstName} {manager.LastName}");
            List<string> skills = manager.GetTeamSkills();
            
            if (skills.Count == 0)
            {
                Console.WriteLine("No skills registered for the team yet.");
            }
            else
            {
                foreach (var skill in skills)
                {
                    Console.WriteLine($"- {skill}");
                }
            }
        }

        public void DisplayManagerTeam(int managerId)
        {
            if (!TryGetEmployee(managerId, out Employee emp))
            {
                ConsoleHelper.PrintError(ERROR_EMPLOYEE_NOT_FOUND);
                return;
            }

            if (!(emp is Manager manager))
            {
                ConsoleHelper.PrintError(ERROR_NOT_A_MANAGER);
                return;
            }

            ConsoleHelper.PrintHeader($"Team Members: {manager.FirstName} {manager.LastName}");
            
            if (manager.TeamMembers.Count == 0)
            {
                Console.WriteLine("No team members assigned yet.");
            }
            else
            {
                foreach (var member in manager.TeamMembers)
                {
                    Console.WriteLine($"- [{member.Id}] {member.FirstName} {member.LastName} - {GetDepartmentName(member.DepartmentId)}");
                }
                Console.WriteLine($"\nTotal Team Members: {manager.TeamMembers.Count}");
            }
        }

        public void RegisterSkill(int employeeId, string skill)
        {
            if (!TryGetEmployee(employeeId, out Employee emp))
            {
                ConsoleHelper.PrintError(ERROR_EMPLOYEE_NOT_FOUND);
                return;
            }

            if (string.IsNullOrWhiteSpace(skill))
            {
                ConsoleHelper.PrintError("Skill name cannot be empty!");
                return;
            }

            string normalizedSkill = NormalizeSkill(skill);
            
            if (!emp.Skills.Contains(normalizedSkill))
            {
                emp.Skills.Add(normalizedSkill);
            }
            
            UniqueSkills.Add(normalizedSkill);
            
            LogAction($"Registered skill '{normalizedSkill}' for {emp.FirstName} {emp.LastName}");
            ConsoleHelper.PrintSuccess($"Skill '{normalizedSkill}' registered successfully.");
        }

        public void DisplayUniqueSkills()
        {
            ConsoleHelper.PrintHeader("Unique Skills in Company");
            
            if (UniqueSkills.Count == 0)
            {
                Console.WriteLine(ERROR_NO_SKILLS);
                return;
            }

            int counter = 1;
            foreach (var skill in UniqueSkills)
            {
                Console.WriteLine($"{counter}. {skill}");
                counter++;
            }
            
            Console.WriteLine($"\nTotal Unique Skills: {UniqueSkills.Count}");
        }

        public void DisplayEmployeesByDepartment(int departmentId)
        {
            if (!DepartmentExists(departmentId))
            {
                ConsoleHelper.PrintError(ERROR_DEPARTMENT_NOT_FOUND);
                return;
            }

            string deptName = GetDepartmentName(departmentId);
            ConsoleHelper.PrintHeader($"Employees in Department: {deptName}");
            
            bool found = false;
            foreach (var emp in ActiveEmployees)
            {
                if (emp.DepartmentId == departmentId)
                {
                    string role = emp is Manager ? "[Manager]" : "[Employee]";
                    Console.WriteLine($"- {role} {emp.FirstName} {emp.LastName} (Id: {emp.Id}, Salary: {emp.Salary:C})");
                    found = true;
                }
            }
            
            if (!found)
            {
                Console.WriteLine("No employees found in this department.");
            }
        }

        public void CalculateAverageSalary()
        {
            ConsoleHelper.PrintHeader("Average Salary Report");
            
            if (ActiveEmployees.Count == 0)
            {
                Console.WriteLine("Average Salary: N/A (No employees)");
                return;
            }

            decimal total = 0;
            decimal minSalary = decimal.MaxValue;
            decimal maxSalary = decimal.MinValue;
            
            foreach (var emp in ActiveEmployees)
            {
                total += emp.Salary;
                if (emp.Salary < minSalary) minSalary = emp.Salary;
                if (emp.Salary > maxSalary) maxSalary = emp.Salary;
            }
            
            decimal average = total / ActiveEmployees.Count;
            
            Console.WriteLine($"Total Employees: {ActiveEmployees.Count}");
            Console.WriteLine($"Total Salary Budget: {total:C}");
            Console.WriteLine($"Average Salary: {average:C}");
            Console.WriteLine($"Minimum Salary: {minSalary:C}");
            Console.WriteLine($"Maximum Salary: {maxSalary:C}");
        }

        public void DisplayHighestSalary()
        {
            ConsoleHelper.PrintHeader("Highest Salary Report");
            
            if (ActiveEmployees.Count == 0)
            {
                Console.WriteLine("No employees registered yet.");
                return;
            }

            Employee highestPaidEmployee = null;
            decimal highestSalary = decimal.MinValue;
            
            foreach (var emp in ActiveEmployees)
            {
                if (emp.Salary > highestSalary)
                {
                    highestSalary = emp.Salary;
                    highestPaidEmployee = emp;
                }
            }
            
            if (highestPaidEmployee != null)
            {
                string role = highestPaidEmployee is Manager ? "Manager" : "Employee";
                Console.WriteLine($"Name: {highestPaidEmployee.FirstName} {highestPaidEmployee.LastName}");
                Console.WriteLine($"ID: {highestPaidEmployee.Id}");
                Console.WriteLine($"Role: {role}");
                Console.WriteLine($"Department: {GetDepartmentName(highestPaidEmployee.DepartmentId)}");
                Console.WriteLine($"Salary: {highestPaidEmployee.Salary:C}");
            }
        }

        public void DisplayDepartmentReport()
        {
            ConsoleHelper.PrintHeader("Department Employee Count Report");
            
            Dictionary<int, int> deptCounts = new Dictionary<int, int>();
            foreach (var dept in Departments.Values)
            {
                deptCounts[dept.Id] = 0;
            }

            foreach (var emp in ActiveEmployees)
            {
                if (deptCounts.ContainsKey(emp.DepartmentId))
                {
                    deptCounts[emp.DepartmentId]++;
                }
            }

            int totalEmployees = 0;
            foreach (var kvp in deptCounts)
            {
                string deptName = GetDepartmentName(kvp.Key);
                Console.WriteLine($"- {deptName}: {kvp.Value} employee(s)");
                totalEmployees += kvp.Value;
            }
            
            Console.WriteLine($"\nTotal Active Employees: {totalEmployees}");
        }

        public void DisplayCompanyStatistics()
        {
            ConsoleHelper.PrintHeader("Company Statistics");
            
            Console.WriteLine("\n General Statistics:");
            Console.WriteLine($"  Total Departments: {Departments.Count}");
            Console.WriteLine($"  Total Active Employees: {ActiveEmployees.Count}");
            Console.WriteLine($"  Employees in Onboarding: {OnboardingQueue.Count}");
            Console.WriteLine($"  Total Unique Skills: {UniqueSkills.Count}");
            Console.WriteLine($"  Total Actions Recorded: {ActionHistory.Count}");
            
            int managerCount = 0;
            int employeeCount = 0;
            foreach (var emp in ActiveEmployees)
            {
                if (emp is Manager)
                    managerCount++;
                else
                    employeeCount++;
            }
            
            Console.WriteLine("\n Employee Breakdown:");
            Console.WriteLine($"  Managers: {managerCount}");
            Console.WriteLine($"  Regular Employees: {employeeCount}");
            
            if (ActiveEmployees.Count > 0)
            {
                decimal totalSalary = 0;
                decimal minSalary = decimal.MaxValue;
                decimal maxSalary = decimal.MinValue;
                
                foreach (var emp in ActiveEmployees)
                {
                    totalSalary += emp.Salary;
                    if (emp.Salary < minSalary) minSalary = emp.Salary;
                    if (emp.Salary > maxSalary) maxSalary = emp.Salary;
                }
                
                decimal averageSalary = totalSalary / ActiveEmployees.Count;
                
                Console.WriteLine("\n Salary Statistics:");
                Console.WriteLine($"  Total Budget: {totalSalary:C}");
                Console.WriteLine($"  Average Salary: {averageSalary:C}");
                Console.WriteLine($"  Minimum Salary: {minSalary:C}");
                Console.WriteLine($"  Maximum Salary: {maxSalary:C}");
            }
            
            Console.WriteLine("\n Department Statistics:");
            foreach (var dept in Departments.Values)
            {
                int deptEmployeeCount = 0;
                decimal deptBudget = 0;
                
                foreach (var emp in ActiveEmployees)
                {
                    if (emp.DepartmentId == dept.Id)
                    {
                        deptEmployeeCount++;
                        deptBudget += emp.Salary;
                    }
                }
                
                Console.WriteLine($"  {dept.Name}: {deptEmployeeCount} employees, Budget: {deptBudget:C}");
            }
        }

        public void DisplayActionHistory()
        {
            ConsoleHelper.PrintHeader("Action History (Newest First)");
            
            if (ActionHistory.Count == 0)
            {
                Console.WriteLine(ERROR_NO_ACTIONS);
                return;
            }

            int counter = 1;
            foreach (var action in ActionHistory)
            {
                Console.WriteLine($"{counter}. {action}");
                counter++;
            }
            
            Console.WriteLine($"\nTotal Actions: {ActionHistory.Count}");
        }

        public void UndoLastAction()
        {
            if (_undoStack.Count == 0)
            {
                ConsoleHelper.PrintError(ERROR_UNDO_NOT_AVAILABLE);
                return;
            }

            Action undoAction = _undoStack.Pop();
            undoAction.Invoke();
            
            LogAction("Undid last action");
            ConsoleHelper.PrintSuccess("Last action undone successfully.");
        }
        #endregion

        #region Seed Data
        public void SeedData()
        {
            AddDepartment("IT", "Information Technology Department");
            AddDepartment("HR", "Human Resources Department");
            AddDepartment("Finance", "Financial Management Department");
            AddDepartment("Marketing", "Marketing and Sales Department");
            
            List<string> managerSkills = new List<string> { "C#", "ASP.NET", "SQL Server", "Leadership" };
            AddToOnboarding("Ahmed", "Ali", "ahmed.ali@company.com", "0101234567", 
                new DateTime(1990, 5, 15), 1, 15000, managerSkills, isManager: true);
            
            List<string> emp1Skills = new List<string> { "Communication", "Recruitment", "Training" };
            AddToOnboarding("Mona", "Said", "mona.said@company.com", "0109876543", 
                new DateTime(1992, 8, 20), 2, 8000, emp1Skills);
            
            List<string> emp2Skills = new List<string> { "Accounting", "Excel", "Financial Analysis" };
            AddToOnboarding("Omar", "Hassan", "omar.hassan@company.com", "0105555555", 
                new DateTime(1988, 3, 10), 3, 9000, emp2Skills);
            
            List<string> emp3Skills = new List<string> { "C#", "JavaScript", "React" };
            AddToOnboarding("Sara", "Mohamed", "sara.mohamed@company.com", "0107777777", 
                new DateTime(1995, 11, 25), 1, 7500, emp3Skills);
            
            ProcessAllOnboarding();

            AddTeamMember(1, 2);
            AddTeamMember(1, 4);

            RegisterSkill(1, "Project Management");
            RegisterSkill(2, "HR Management");
            RegisterSkill(3, "Budget Planning");
            RegisterSkill(4, "c#");
            RegisterSkill(1, "leadership");
            
            ConsoleHelper.PrintSuccess("Seed data loaded successfully!");
        }
        #endregion
    }
}