# Employee Management System

A console-based C# application for managing employees, departments, onboarding workflows, manager teams, and company reports.

## Overview

This project is a simple employee management console app that helps simulate common HR and company administration tasks. Users can create departments, onboard employees, assign managers, track skills, review reports, and manage recent actions.

## Features

- Add and view departments
- Add employees to the onboarding queue
- Process onboarding for one employee or all pending employees
- Assign employees to manager teams
- Register and review employee skills
- Search employees by ID or name
- Display employee details and department-based reports
- View company statistics and action history
- Undo the last action

## Project Structure

- Models/: contains the core domain models such as Employee, Manager, Department, and EmployeeStatus
- Services/: contains the Company service that implements the main business logic
- Program.cs: entry point and interactive menu
- Helpers.cs: console helpers and validation utilities

## Technologies

- C#
- .NET 9
- Console application

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

When the app starts, you will see a menu with options for managing departments, onboarding employees, viewing reports, and handling company operations. Follow the prompts to interact with the system.

## Example Workflow

1. Create one or more departments.
2. Add employees to onboarding.
3. Process onboarding to activate employees.
4. Assign team members to managers and register skills.
5. Review salary, budget, and company statistics reports.

## License

This project is provided as a learning or demonstration application.
