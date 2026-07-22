using System;
using System.Collections.Generic;
using EmploymentManagmentSystem.Services;
using EmploymentManagmentSystem.Models;

namespace EmploymentManagmentSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Company company = new Company();
            company.SeedData();

            bool exit = false;

            do
            {
                Console.Clear();
                ConsoleHelper.PrintMenuHeader("Employee Management System");
                
                ConsoleHelper.PrintMenuOption("1", "Add New Department");
                ConsoleHelper.PrintMenuOption("2", "Add Employee to Onboarding");
                ConsoleHelper.PrintMenuOption("3", "Process Next Onboarding");
                ConsoleHelper.PrintMenuOption("4", "Process All Onboarding");
                ConsoleHelper.PrintMenuOption("5", "Add Team Member to Manager");
                ConsoleHelper.PrintMenuOption("6", "Register Skill for Employee");
                ConsoleHelper.PrintMenuOption("7", "Search Employee by ID");
                ConsoleHelper.PrintMenuOption("8", "Search Employee by Name");
                ConsoleHelper.PrintMenuOption("9", "Display All Active Employees");
                ConsoleHelper.PrintMenuOption("10", "Display Employee Details");
                ConsoleHelper.PrintMenuOption("11", "Display Employees by Department");
                ConsoleHelper.PrintMenuOption("12", "Display Manager Team Members");
                ConsoleHelper.PrintMenuOption("13", "Display Manager Team Skills");
                ConsoleHelper.PrintMenuOption("14", "Display Unique Company Skills");
                ConsoleHelper.PrintMenuOption("15", "Calculate Average Salary");
                ConsoleHelper.PrintMenuOption("16", "Display Highest Salary");
                ConsoleHelper.PrintMenuOption("17", "Display Department Employee Count Report");
                ConsoleHelper.PrintMenuOption("18", "Display Specific Department Budget");
                ConsoleHelper.PrintMenuOption("19", "Display All Departments Budget");
                ConsoleHelper.PrintMenuOption("20", "Display Company Statistics");
                ConsoleHelper.PrintMenuOption("21", "Display Action History");
                ConsoleHelper.PrintMenuOption("22", "Undo Last Action");
                ConsoleHelper.PrintMenuOption("0", "Exit");

                ConsoleHelper.PrintSeparator();
                string choice = Validation.GetValidMenuChoice("Select an option: ", 
                    "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", 
                    "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22");

                switch (choice)
                {
                    case "1":
                        string deptName = Validation.GetValidString("Enter department name: ");
                        string deptDesc = Validation.GetValidString("Enter department description: ");
                        company.AddDepartment(deptName, deptDesc);
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
                        company.AddToOnboarding(firstName, lastName, email, phone, dob, deptId, salary, skills, isManager);
                        ConsoleHelper.Pause();
                        break;

                    case "3":
                        company.ProcessOnboarding();
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
                        company.AddTeamMember(mgrId, empId);
                        ConsoleHelper.Pause();
                        break;

                    case "6":
                        company.DisplayAllEmployees();
                        int skillEmpId = Validation.GetValidPositiveInt("Enter Employee ID: ");
                        string newSkill = Validation.GetValidString("Enter skill to add: ");
                        company.RegisterSkill(skillEmpId, newSkill);
                        ConsoleHelper.Pause();
                        break;

                    case "7":
                        int searchId = Validation.GetValidPositiveInt("Enter Employee ID to search: ");
                        Employee foundById = company.FindEmployeeById(searchId);
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

                    case "9":
                        company.DisplayAllEmployees();
                        ConsoleHelper.Pause();
                        break;

                    case "10":
                        company.DisplayAllEmployees();
                        int detailsId = Validation.GetValidPositiveInt("Enter Employee ID for details: ");
                        company.DisplayEmployeeDetails(detailsId);
                        ConsoleHelper.Pause();
                        break;

                    case "11":
                        company.DisplayAllDepartments();
                        int filterDeptId = Validation.GetValidPositiveInt("Enter Department ID to filter: ");
                        company.DisplayEmployeesByDepartment(filterDeptId);
                        ConsoleHelper.Pause();
                        break;

                    case "12":
                        company.DisplayAllEmployees();
                        int teamMgrId = Validation.GetValidPositiveInt("Enter Manager ID: ");
                        company.DisplayManagerTeam(teamMgrId);
                        ConsoleHelper.Pause();
                        break;

                    case "13":
                        company.DisplayAllEmployees();
                        int skillMgrId = Validation.GetValidPositiveInt("Enter Manager ID: ");
                        company.DisplayManagerTeamSkills(skillMgrId);
                        ConsoleHelper.Pause();
                        break;

                    case "14":
                        company.DisplayUniqueSkills();
                        ConsoleHelper.Pause();
                        break;

                    case "15":
                        company.CalculateAverageSalary();
                        ConsoleHelper.Pause();
                        break;

                    case "16":
                        company.DisplayHighestSalary();
                        ConsoleHelper.Pause();
                        break;

                    case "17":
                        company.DisplayDepartmentReport();
                        ConsoleHelper.Pause();
                        break;

                    case "18":
                        company.DisplayAllDepartments();
                        int budgetDeptId = Validation.GetValidPositiveInt("Enter Department ID for budget report: ");
                        company.DisplayDepartmentBudget(budgetDeptId);
                        ConsoleHelper.Pause();
                        break;

                    case "19":
                        company.DisplayAllDepartmentsBudget();
                        ConsoleHelper.Pause();
                        break;

                    case "20":
                        company.DisplayCompanyStatistics();
                        ConsoleHelper.Pause();
                        break;

                    case "21":
                        company.DisplayActionHistory();
                        ConsoleHelper.Pause();
                        break;

                    case "22":
                        bool confirmUndo = Validation.GetYesNoResponse("Are you sure you want to undo the last action? (Y/N): ");
                        if (confirmUndo)
                        {
                            company.UndoLastAction();
                        }
                        else
                        {
                            ConsoleHelper.PrintInfo("Undo cancelled.");
                        }
                        ConsoleHelper.Pause();
                        break;

                    case "0":
                        exit = true;
                        ConsoleHelper.PrintSuccess("Thank you for using the Employee Management System. Goodbye!");
                        break;
                }

            } while (!exit);
        }
    }
}