using System;
using System.Collections.Generic;
using EmploymentManagmentSystem.Services;
using EmploymentManagmentSystem.Models;
using EmploymentManagmentSystem.Helpers;
using EmploymentManagmentSystem.Common;
using EmploymentManagmentSystem.Events;

namespace EmploymentManagmentSystem
{
    class Program
    {
        static Company company;
        static bool skillEventSubscribed = true;

        static void Main(string[] args)
        {
            company = new Company();
            
            company.EmployeeOnboarded += OnEmployeeOnboarded;
            company.EmployeePromoted += OnEmployeePromoted;
            company.EmployeeSkillRegistered += OnEmployeeSkillRegistered;
            company.BudgetExceeded += OnBudgetExceeded;

            company.SeedData();
            bool exit = false;

            do
            {
                Console.Clear();
                ConsoleHelper.PrintMenuHeader("Employee Management System (Advanced + Bonus)");
                
                ConsoleHelper.PrintMenuOption("1", "Add New Department");
                ConsoleHelper.PrintMenuOption("2", "Add Employee to Onboarding");
                ConsoleHelper.PrintMenuOption("3", "Process Next Onboarding");
                ConsoleHelper.PrintMenuOption("4", "Process All Onboarding");
                ConsoleHelper.PrintMenuOption("5", "Add Team Member to Manager");
                ConsoleHelper.PrintMenuOption("6", "Register Skill for Employee");
                ConsoleHelper.PrintMenuOption("7", "Search Employee by ID (Generic)");
                ConsoleHelper.PrintMenuOption("8", "Search Department by ID (Generic)");
                ConsoleHelper.PrintMenuOption("9", "Search Employee by Name");
                ConsoleHelper.PrintMenuOption("10", "Display All Active Employees");
                ConsoleHelper.PrintMenuOption("11", "Display Employee Details");
                ConsoleHelper.PrintMenuOption("12", "Display Employees by Department");
                ConsoleHelper.PrintMenuOption("13", "Display Manager Team Members");
                ConsoleHelper.PrintMenuOption("14", "Display Manager Team Skills");
                ConsoleHelper.PrintMenuOption("15", "Display Unique Company Skills");
                ConsoleHelper.PrintMenuOption("16", "Calculate Average Salary");
                ConsoleHelper.PrintMenuOption("17", "Display Highest Salary");
                ConsoleHelper.PrintMenuOption("18", "Display Department Employee Count Report");
                ConsoleHelper.PrintMenuOption("19", "Display Specific Department Budget");
                ConsoleHelper.PrintMenuOption("20", "Display All Departments Budget");
                ConsoleHelper.PrintMenuOption("21", "Display Company Statistics");
                ConsoleHelper.PrintMenuOption("22", "Display Action History");
                ConsoleHelper.PrintMenuOption("23", "Display Command History");
                ConsoleHelper.PrintMenuOption("24", "Filter: Managers Only (Lambda)");
                ConsoleHelper.PrintMenuOption("25", "Filter: Salary > 10000 (Lambda)");
                ConsoleHelper.PrintMenuOption("26", "Filter: Top Performers (Rating >= 4)");
                ConsoleHelper.PrintMenuOption("27", "Sort: By Salary (Ascending)");
                ConsoleHelper.PrintMenuOption("28", "Sort: By Name (A-Z)");
                ConsoleHelper.PrintMenuOption("29", "Promote Employee to Manager");
                ConsoleHelper.PrintMenuOption("30", "Submit Leave Request");
                ConsoleHelper.PrintMenuOption("31", "Approve Next Leave Request");
                ConsoleHelper.PrintMenuOption("32", "Update Employee Rating");
                ConsoleHelper.PrintMenuOption("33", "Toggle Skill Event Subscription");
                ConsoleHelper.PrintMenuOption("0", "Exit");

                ConsoleHelper.PrintSeparator();
                string choice = Console.ReadLine();
                company.LogCommand($"Menu Option: {choice}");

                switch (choice)
                {
                    case "1":
                        string deptName = Validation.GetValidString("Enter department name: ");
                        string deptDesc = Validation.GetValidString("Enter department description: ");
                        PrintResult(company.AddDepartment(deptName, deptDesc));
                        ConsoleHelper.Pause();
                        break;

                    case "2":
                        string firstName = Validation.GetValidString("Enter first name: ");
                        string lastName = Validation.GetValidString("Enter last name: ");
                        string email = Validation.GetValidEmail("Enter email: ");
                        string phone = Validation.GetValidPhoneNumber("Enter phone number: ");
                        DateTime dob = Validation.GetValidDateOfBirth("Enter date of birth (e.g., 01/01/1990): ");
                        
                        company.DisplayAllDepartments();
                        int deptId = Validation.GetValidPositiveInt("Enter department ID: ");
                        
                        decimal salary = Validation.GetValidPositiveDecimal("Enter salary: ");
                        string skillsInput = Validation.GetValidString("Enter skills (comma-separated, e.g., C#, SQL): ");
                        List<string> skills = new List<string>();
                        foreach (string s in skillsInput.Split(','))
                        {
                            if (!string.IsNullOrWhiteSpace(s))
                                skills.Add(s.Trim());
                        }
                        
                        bool isManager = Validation.GetYesNoResponse("Is this employee a manager? (Y/N): ");
                        PrintResult(company.AddToOnboarding(firstName, lastName, email, phone, dob, deptId, salary, skills, isManager));
                        ConsoleHelper.Pause();
                        break;

                    case "3":
                        PrintResult(company.ProcessOnboarding());
                        ConsoleHelper.Pause();
                        break;

                    case "4":
                        company.ProcessAllOnboarding();
                        ConsoleHelper.Pause();
                        break;

                    case "5":
                        company.DisplayAllEmployees();
                        int mgrId = Validation.GetValidPositiveInt("Enter Manager ID: ");
                        int empId = Validation.GetValidPositiveInt("Enter Employee ID to add to team: ");
                        PrintResult(company.AddTeamMember(mgrId, empId));
                        ConsoleHelper.Pause();
                        break;

                    case "6":
                        company.DisplayAllEmployees();
                        int skillEmpId = Validation.GetValidPositiveInt("Enter Employee ID: ");
                        string newSkill = Validation.GetValidString("Enter skill to add: ");
                        PrintResult(company.RegisterSkill(skillEmpId, newSkill));
                        ConsoleHelper.Pause();
                        break;

                    case "7":
                        int searchId = Validation.GetValidPositiveInt("Enter Employee ID to search: ");
                        Employee foundById = company.FindById<Employee>(searchId);
                        if (foundById != null)
                        {
                            ConsoleHelper.PrintSuccess($"Found: {foundById.FirstName} {foundById.LastName} (ID: {foundById.Id})");
                        }
                        else
                        {
                            ConsoleHelper.PrintError("Employee not found.");
                        }
                        ConsoleHelper.Pause();
                        break;

                    case "8":
                        int deptSearchId = Validation.GetValidPositiveInt("Enter Department ID to search: ");
                        Department foundDept = company.FindById<Department>(deptSearchId);
                        if (foundDept != null)
                        {
                            ConsoleHelper.PrintSuccess($"Found Department: {foundDept.Name} (ID: {foundDept.Id})");
                        }
                        else
                        {
                            ConsoleHelper.PrintError("Department not found.");
                        }
                        ConsoleHelper.Pause();
                        break;

                    case "9":
                        string searchFirst = Validation.GetValidString("Enter first name: ");
                        string searchLast = Validation.GetValidString("Enter last name: ");
                        Employee foundByName = company.FindEmployeeByName(searchFirst, searchLast);
                        if (foundByName != null)
                        {
                            ConsoleHelper.PrintSuccess($"Found: {foundByName.FirstName} {foundByName.LastName} (ID: {foundByName.Id})");
                        }
                        else
                        {
                            ConsoleHelper.PrintError("Employee not found.");
                        }
                        ConsoleHelper.Pause();
                        break;

                    case "10":
                        company.DisplayAllEmployees();
                        ConsoleHelper.Pause();
                        break;

                    case "11":
                        company.DisplayAllEmployees();
                        int detailsId = Validation.GetValidPositiveInt("Enter Employee ID for details: ");
                        company.DisplayEmployeeDetails(detailsId);
                        ConsoleHelper.Pause();
                        break;

                    case "12":
                        company.DisplayAllDepartments();
                        int filterDeptId = Validation.GetValidPositiveInt("Enter Department ID to filter: ");
                        company.DisplayEmployeesByDepartment(filterDeptId);
                        ConsoleHelper.Pause();
                        break;

                    case "13":
                        company.DisplayAllEmployees();
                        int teamMgrId = Validation.GetValidPositiveInt("Enter Manager ID: ");
                        company.DisplayManagerTeam(teamMgrId);
                        ConsoleHelper.Pause();
                        break;

                    case "14":
                        company.DisplayAllEmployees();
                        int skillMgrId = Validation.GetValidPositiveInt("Enter Manager ID: ");
                        company.DisplayManagerTeamSkills(skillMgrId);
                        ConsoleHelper.Pause();
                        break;

                    case "15":
                        company.DisplayUniqueSkills();
                        ConsoleHelper.Pause();
                        break;

                    case "16":
                        company.CalculateAverageSalary();
                        ConsoleHelper.Pause();
                        break;

                    case "17":
                        company.DisplayHighestSalary();
                        ConsoleHelper.Pause();
                        break;

                    case "18":
                        company.DisplayDepartmentReport();
                        ConsoleHelper.Pause();
                        break;

                    case "19":
                        company.DisplayAllDepartments();
                        int budgetDeptId = Validation.GetValidPositiveInt("Enter Department ID for budget report: ");
                        company.DisplayDepartmentBudget(budgetDeptId);
                        ConsoleHelper.Pause();
                        break;

                    case "20":
                        company.DisplayAllDepartmentsBudget();
                        ConsoleHelper.Pause();
                        break;

                    case "21":
                        company.DisplayCompanyStatistics();
                        ConsoleHelper.Pause();
                        break;

                    case "22":
                        company.DisplayActionHistory();
                        ConsoleHelper.Pause();
                        break;

                    case "23":
                        company.DisplayCommandHistory();
                        ConsoleHelper.Pause();
                        break;

                    case "24":
                        ConsoleHelper.PrintHeader("Managers Only (Using Lambda)");
                        List<Employee> managers = company.FilterEmployees(e => e is Manager);
                        foreach (var m in managers)
                            Console.WriteLine($"- {m.FirstName} {m.LastName}");
                        ConsoleHelper.Pause();
                        break;

                    case "25":
                        ConsoleHelper.PrintHeader("Salary > 10000 (Using Lambda)");
                        List<Employee> highEarners = company.FilterEmployees(e => e.Salary > 10000);
                        foreach (var e in highEarners)
                            Console.WriteLine($"- {e.FirstName} {e.LastName} ({e.Salary:C})");
                        ConsoleHelper.Pause();
                        break;

                    case "26":
                        company.DisplayTopPerformers();
                        ConsoleHelper.Pause();
                        break;

                    case "27":
                        company.SortEmployees((a, b) => a.Salary.CompareTo(b.Salary));
                        ConsoleHelper.PrintSuccess("Employees sorted by Salary (Ascending)!");
                        company.DisplayAllEmployees();
                        ConsoleHelper.Pause();
                        break;

                    case "28":
                        company.SortEmployees((a, b) => a.FirstName.CompareTo(b.FirstName));
                        ConsoleHelper.PrintSuccess("Employees sorted by Name (A-Z)!");
                        company.DisplayAllEmployees();
                        ConsoleHelper.Pause();
                        break;

                    case "29":
                        company.DisplayAllEmployees();
                        int promoteId = Validation.GetValidPositiveInt("Enter Employee ID to promote: ");
                        PrintResult(company.PromoteToManager(promoteId));
                        ConsoleHelper.Pause();
                        break;

                    case "30":
                        company.DisplayAllEmployees();
                        int leaveEmpId = Validation.GetValidPositiveInt("Enter Employee ID: ");
                        DateTime startDate = Validation.GetValidDate("Enter start date (e.g., 01/01/2026): ");
                        DateTime endDate = Validation.GetValidDate("Enter end date (e.g., 05/01/2026): ");
                        string reason = Validation.GetValidString("Enter reason: ");
                        PrintResult(company.SubmitLeaveRequest(leaveEmpId, startDate, endDate, reason));
                        ConsoleHelper.Pause();
                        break;
                    case "31":
                        PrintResult(company.ApproveNextLeaveRequest());
                        ConsoleHelper.Pause();
                        break;

                    case "32":
                        company.DisplayAllEmployees();
                        int ratingEmpId = Validation.GetValidPositiveInt("Enter Employee ID: ");
                        int newRating = Validation.GetValidPositiveInt("Enter new rating (1-5): ");
                        company.UpdateEmployeeRating(ratingEmpId, newRating);
                        ConsoleHelper.Pause();
                        break;

                    case "33":
                        if (skillEventSubscribed)
                        {
                            company.EmployeeSkillRegistered -= OnEmployeeSkillRegistered;
                            skillEventSubscribed = false;
                            ConsoleHelper.PrintWarning("Unsubscribed from Skill Event!");
                        }
                        else
                        {
                            company.EmployeeSkillRegistered += OnEmployeeSkillRegistered;
                            skillEventSubscribed = true;
                            ConsoleHelper.PrintSuccess("Subscribed to Skill Event!");
                        }
                        ConsoleHelper.Pause();
                        break;

                    case "0":
                        exit = true;
                        ConsoleHelper.PrintSuccess("Thank you for using the Employee Management System. Goodbye!");
                        break;

                    default:
                        ConsoleHelper.PrintError("Invalid option. Please try again.");
                        ConsoleHelper.Pause();
                        break;
                }

            } while (!exit);
        }

        private static void OnEmployeeOnboarded(object sender, EmployeeEventArgs e)
        {
            ConsoleHelper.PrintWarning($"[EVENT] Welcome aboard, {e.Employee.FirstName}! (Onboarded at {e.EventTime:HH:mm:ss})");
        }

        private static void OnEmployeePromoted(object sender, EmployeeEventArgs e)
        {
            ConsoleHelper.PrintWarning($"[EVENT] Congratulations to {e.Employee.FirstName} on the promotion to Manager! (At {e.EventTime:HH:mm:ss})");
        }

        private static void OnEmployeeSkillRegistered(object sender, EmployeeEventArgs e)
        {
            ConsoleHelper.PrintWarning($"[EVENT] New skill registered: {e.ExtraInfo} by {e.Employee.FirstName}!");
        }

        private static void OnBudgetExceeded(object sender, BudgetAlertEventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n🚨 [BUDGET ALERT] Department '{e.DepartmentName}' exceeded budget!");
            Console.WriteLine($"   Current: {e.CurrentBudget:C} | Limit: {e.BudgetLimit:C} | Over by: {e.CurrentBudget - e.BudgetLimit:C}");
            Console.ResetColor();
        }

        private static void PrintResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
                ConsoleHelper.PrintSuccess(result.Message);
            else
                ConsoleHelper.PrintError(result.Message);
        }
    }
}