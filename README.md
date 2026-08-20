# Employee Management System (Advanced + Bonus)

A console-based C# application for managing employees, departments, onboarding workflows, manager teams, performance ratings, leave requests, event handling, and comprehensive company reports.

## Overview

This project is an advanced employee management console application that simulates real-world HR and company administration tasks. Users can create departments, onboard employees, assign managers, track skills, promote employees, manage team members, review detailed reports, track action history, and subscribe to employee lifecycle events.

## Features

### Core Employee Management
- Add and view departments with descriptions
- Add employees to the onboarding queue with comprehensive details (name, email, phone, DOB, salary, skills, manager status)
- Process onboarding for one employee or all pending employees
- Search employees by ID or name
- Display employee details and department-based reports
- Promote employees to manager positions

### Manager & Team Management
- Assign employees to manager teams
- Manage manager team members
- Track and display team member skills
- View team-specific reports

### Skills Management
- Register and review employee skills
- Display unique company-wide skills
- Track team member skills aggregation
- Skills stored per employee with support for multiple competencies

### Reporting & Analytics
- Calculate average salary across company
- Display highest salary employee
- Department-based employee count report
- Specific department budget calculations
- All departments budget overview
- Company statistics (total employees, departments, budget)
- Action history tracking with timestamps

### Advanced Filtering
- Filter managers only (Lambda expression)
- Filter employees by salary threshold (Lambda expression)
- Filter top performers by rating (rating >= 4)
- Custom EmployeeFilter delegate for flexible querying

### Sorting
- Sort employees by salary (ascending)
- Sort employees by name (A-Z alphabetical)

### Event System
- **EmployeeOnboarded** event - Triggered when an employee completes onboarding
- **EmployeePromoted** event - Triggered when an employee is promoted to manager
- **EmployeeSkillRegistered** event - Triggered when a new skill is registered for an employee (toggleable)
- **BudgetExceeded** event - Triggered when department budget is exceeded
- Real-time event logging with timestamps
- Subscribe/unsubscribe from events at runtime

### Additional Features
- Action history stack for tracking system operations
- Command history logging for audit trail
- Comprehensive data validation for user inputs
- Seed data initialization for testing
- Console helper utilities for formatted output
- Employee rating system for performance tracking
- Leave request management (submit and approve)
- Generic search for any entity type (Employee, Department)
- Real-time event subscription management

## Project Structure

- **Models/**: Core domain models
  - `Employee.cs` - Base employee class
  - `Manager.cs` - Manager class inheriting from Employee with team management
  - `Department.cs` - Department information
  - `EmployeeStatus.cs` - Enum for employee status (Pending, Active)
  - `EmployeeEventArgs.cs` - Event arguments for employee events
  - `EmployeeFilter.cs` - Delegate for custom employee filtering
  - `Results.cs` - Operation result wrapper
  
- **Services/**: Business logic
  - `Company.cs` - Main service containing all business operations and event publishers
  
- **Helpers/**: Utility functions
  - `Validation.cs` - Input validation for emails, phone numbers, dates, and other fields
  - `ConsoleHelper.cs` - Console formatting and display utilities
  
- **Program.cs**: Entry point with interactive menu and event handlers

## Technologies

- C# 12
- .NET 9
- Console application
- Events and delegates
- Collections (List, Queue, Stack, HashSet, Dictionary)
- LINQ and Lambda expressions

## Prerequisites

- .NET SDK 9.0 or later

## Getting Started

1. Open the project folder.
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Run the application:
   ```bash
   dotnet run
   ```

## Usage

When the app starts, you will see an interactive menu with 33 options for managing the employee system:

- **Options 1-4**: Department and onboarding management
- **Options 5-6**: Team and skill management
- **Options 7-9**: Search employees (by ID with generics, department, name)
- **Options 10-14**: Employee viewing and filtering (all, details, by department, team, team skills)
- **Options 15-22**: Reports and analytics (skills, salaries, department, company stats, action history, command history)
- **Options 23-28**: Advanced filtering and sorting (managers, salary threshold, top performers, sort by salary, sort by name)
- **Options 29-33**: Employee lifecycle (promotion, leave requests, rating, event management)
- **Option 0**: Exit application

Follow the on-screen prompts to input data and select operations.

## Example Workflow

1. Create one or more departments (Option 1)
2. Add employees to onboarding with skills and manager status (Option 2)
3. Process onboarding to activate employees (Options 3-4)
4. Assign team members to managers (Option 5)
5. Register additional skills for employees (Option 6)
6. Search and view employee details using generic search (Options 7-11)
7. Update employee ratings for performance tracking (Option 32)
8. Generate department and budget reports (Options 15-20)
9. Review company statistics and action/command history (Options 21-22)
10. Apply filters to find managers, high earners, or top performers (Options 24-26)
11. Sort employees by salary or name for analysis (Options 27-28)
12. Submit and process leave requests (Options 30-31)
13. Promote qualified employees to managers (Option 29)
14. Toggle skill event notifications as needed (Option 33)

## Event Handling

The system publishes four key events:
- **EmployeeOnboarded**: Logged when an employee is successfully onboarded
- **EmployeePromoted**: Logged when an employee is promoted to manager
- **EmployeeSkillRegistered**: Logged when a new skill is added to an employee (toggleable via Option 33)
- **BudgetExceeded**: Logged when a department exceeds its budget threshold

Events are subscribed in Program.cs and logged to the console for real-time tracking. Users can dynamically toggle skill event subscriptions at runtime to control event notification behavior.

## License

This project is provided as a learning and demonstration application for advanced C# console programming concepts.
