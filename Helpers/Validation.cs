using System;
using System.Text.RegularExpressions;

namespace EmploymentManagmentSystem.Helpers
{
    public static class ConsoleHelper
    {
        public static void PrintHeader(string text) 
        { 
         Console.ForegroundColor = ConsoleColor.Cyan;
         Console.WriteLine($"\n=== {text} ==="); 
         Console.ResetColor(); 
        }
        public static void PrintSuccess(string text) 
        { 
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✓ {text}");
            Console.ResetColor(); 
        }
        public static void PrintError(string text) 
        {
             Console.ForegroundColor = ConsoleColor.Red;
             Console.WriteLine($"✗ {text}");
             Console.ResetColor(); 
        }
        public static void PrintWarning(string text) 
        { 
            Console.ForegroundColor = ConsoleColor.Yellow; 
            Console.WriteLine($"⚠ {text}"); 
            Console.ResetColor(); 
        }
        public static void PrintInfo(string text) 
        { 
            Console.ForegroundColor = ConsoleColor.White; 
            Console.WriteLine($"ℹ {text}"); 
            Console.ResetColor(); 
        }
        public static void Pause() 
        { 
            Console.WriteLine("\nPress any key to continue..."); 
            Console.ReadKey(); 
        }
        public static void PrintMenuOption(string number, string description) 
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{number}. ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(description);
            Console.ResetColor();
        }
        public static void PrintMenuHeader(string title) 
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n=== {title} ===");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Please select an option:");
            Console.ResetColor();
        }
        public static void PrintSeparator() 
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('-', 60));
            Console.ResetColor();
        }
    }

    public static class Validation
    {
        public static int GetValidPositiveInt(string prompt) 
        { 
            int result; 
            while (true) 
            { 
                Console.Write(prompt); 
                if (int.TryParse(Console.ReadLine(), out result) && result > 0) 
                return result; 
                ConsoleHelper.PrintError("Invalid input. Please enter a positive integer.");
            } 
        }
        public static decimal GetValidPositiveDecimal(string prompt) 
        { 
            decimal result; 
            while (true) 
            { 
                Console.Write(prompt); 
                if (decimal.TryParse(Console.ReadLine(), out result) && result >= 0) 
                    return result; 
                ConsoleHelper.PrintError("Invalid input. Please enter a non-negative decimal."); 
            } 
        }
        public static string GetValidString(string prompt) 
        { 
            string result; 
            while (true) 
            { 
                Console.Write(prompt); 
                result = Console.ReadLine(); 
                if (!string.IsNullOrWhiteSpace(result)) 
                    return result.Trim(); 
                ConsoleHelper.PrintError("Invalid input. Please enter a non-empty text."); 
            } 
        }
        public static string GetValidEmail(string prompt) 
        { 
            string result; 
            while (true) 
            { 
                Console.Write(prompt); 
                result = Console.ReadLine(); 
                if (!string.IsNullOrWhiteSpace(result) && Regex.IsMatch(result.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)) 
                    return result.Trim(); 
                ConsoleHelper.PrintError("Invalid email format."); 
            } 
        }
        public static string GetValidPhoneNumber(string prompt) 
        { 
            string result; 
            while (true) 
            { 
                Console.Write(prompt); 
                result = Console.ReadLine(); 
                string digits = Regex.Replace(result.Trim(), @"[^\d]", ""); 
                if (digits.Length >= 10 && digits.Length <= 15) 
                    return result.Trim(); 
                ConsoleHelper.PrintError("Invalid phone number (10-15 digits)."); 
            } 
        }
        public static DateTime GetValidDateOfBirth(string prompt) 
        { 
            DateTime result; 
            while (true) 
            { 
                Console.Write(prompt); 
                if (DateTime.TryParse(Console.ReadLine(), out result)) 
                { 
                    int age = DateTime.Now.Year - result.Year; 
                    if (result.Date > DateTime.Now.AddYears(-age).Date) age--; 
                    if (age >= 18 && age <= 100) return result; 
                } 
                ConsoleHelper.PrintError("Invalid date of birth (18-100 years)."); 
            } 
        }
             public static DateTime GetValidDate(string prompt)
        {
            DateTime result;
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParse(Console.ReadLine(), out result))
                {
                    return result;
                }
                ConsoleHelper.PrintError("Invalid date format. Please enter a valid date (e.g., 01/01/2026).");
            }
        }
        public static bool GetYesNoResponse(string prompt) 
        { 
            string result; 
            while (true) 
            { 
                Console.Write(prompt); 
                result = Console.ReadLine()?.Trim().ToUpper(); 
                if (result == "Y" || result == "YES") return true; 
                if (result == "N" || result == "NO") return false; 
                ConsoleHelper.PrintError("Please enter Y or N."); 
            } 
        }
    }
}