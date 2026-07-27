using System;
using System.Text.RegularExpressions;

namespace EmployementManagementSystem.Helpers
{
    
    public static class Validation
    {
       
        
        //Numeric Validation
        public static int GetValidInt(string prompt)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                
                if (int.TryParse(input, out result))
                {
                    return result;
                }        
                PrintError("Invalid input. Please enter a valid integer number.");
            }
        }
        
       
        public static int GetValidPositiveInt(string prompt)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                
                if (int.TryParse(input, out result) && result > 0)
                {
                    return result;
                }
                
                PrintError("Invalid input. Please enter a positive integer greater than 0.");
            }
        }
        
        
        public static int GetValidIntInRange(string prompt, int min, int max)
        {
            int result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                
                if (int.TryParse(input, out result) && result >= min && result <= max)
                {
                    return result;
                }
                
                PrintError($"Invalid input. Please enter a number between {min} and {max}.");
            }
        }

        public static decimal GetValidDecimal(string prompt)
        {
            decimal result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                
                if (decimal.TryParse(input, out result))
                {
                    return result;
                }
                
                PrintError("Invalid input. Please enter a valid decimal number.");
            }
        }
        

        public static decimal GetValidPositiveDecimal(string prompt)
        {
            decimal result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                
                if (decimal.TryParse(input, out result) && result >= 0)
                {
                    return result;
                }
                
                PrintError("Invalid input. Please enter a non-negative decimal number.");
            }
        }


        //String Validation
        
        
        public static string GetValidString(string prompt)
        {
            string result;
            while (true)
            {
                Console.Write(prompt);
                result = Console.ReadLine();
                
                if (!string.IsNullOrWhiteSpace(result))
                {
                    return result.Trim();
                }
                
                PrintError("Invalid input. Please enter a non-empty text.");
            }
        }
        
        
        public static string GetValidStringWithMinLength(string prompt, int minLength)
        {
            string result;
            while (true)
            {
                Console.Write(prompt);
                result = Console.ReadLine();
                
                if (!string.IsNullOrWhiteSpace(result) && result.Trim().Length >= minLength)
                {
                    return result.Trim();
                }
                
                PrintError($"Invalid input. Please enter at least {minLength} characters.");
            }
        }
        
        
        public static string GetValidEmail(string prompt)
        {
            string result;
            while (true)
            {
                Console.Write(prompt);
                result = Console.ReadLine();
                
                if (!string.IsNullOrWhiteSpace(result) && IsValidEmail(result.Trim()))
                {
                    return result.Trim();
                }
                
                PrintError("Invalid email format. Please enter a valid email address (e.g., user@example.com).");
            }
        }
        
        
        public static string GetValidPhoneNumber(string prompt)
        {
            string result;
            while (true)
            {
                Console.Write(prompt);
                result = Console.ReadLine();
                
                if (!string.IsNullOrWhiteSpace(result) && IsValidPhoneNumber(result.Trim()))
                {
                    return result.Trim();
                }
                
                PrintError("Invalid phone number format. Please enter a valid phone number (digits only, 10-15 characters).");
            }
        }
        
      

       //Date Validation
        
       
        public static DateTime GetValidDate(string prompt)
        {
            DateTime result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                
                if (DateTime.TryParse(input, out result))
                {
                    return result;
                }
                
                PrintError("Invalid date format. Please enter a valid date (e.g., 01/01/2000 or 2000-01-01).");
            }
        }
        
        
        public static DateTime GetValidPastDate(string prompt)
        {
            DateTime result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                
                if (DateTime.TryParse(input, out result) && result < DateTime.Now)
                {
                    return result;
                }
                
                PrintError("Invalid date. Please enter a date in the past.");
            }
        }
        
        
        public static DateTime GetValidDateOfBirth(string prompt)
        {
            DateTime result;
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                
                if (DateTime.TryParse(input, out result))
                {
                    int age = DateTime.Now.Year - result.Year;
                    if (result.Date > DateTime.Now.AddYears(-age).Date) age--;
                    
                    if (age >= 18 && age <= 100)
                    {
                        return result;
                    }
                }
                
                PrintError("Invalid date of birth. Employee must be between 18 and 100 years old.");
            }
        }
        
        

        //Validation Helpers
        
       
        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            
            try
            {
                
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }
        
        
        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;
            
            
            string digitsOnly = Regex.Replace(phoneNumber, @"[^\d]", "");
            
            return digitsOnly.Length >= 10 && digitsOnly.Length <= 15;
        }
        
        
        public static bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;
            
            string pattern = @"^[a-zA-Z\s]+$";
            return Regex.IsMatch(name.Trim(), pattern);
        }
        
       
        //menuvalidation
        public static string GetValidMenuChoice(string prompt, params string[] validChoices)
        {
            string result;
            while (true)
            {
                Console.Write(prompt);
                result = Console.ReadLine()?.Trim();
                
                if (!string.IsNullOrEmpty(result))
                {
                    foreach (string choice in validChoices)
                    {
                        if (result == choice)
                        {
                            return result;
                        }
                    }
                }
                
                PrintError($"Invalid choice. Please enter one of: {string.Join(", ", validChoices)}");
            }
        }
        
        
        public static bool GetYesNoResponse(string prompt)
        {
            string result;
            while (true)
            {
                Console.Write(prompt);
                result = Console.ReadLine()?.Trim().ToUpper();
                
                if (result == "Y" || result == "YES")
                {
                    return true;
                }
                else if (result == "N" || result == "NO")
                {
                    return false;
                }
                
                PrintError("Invalid input. Please enter Y (Yes) or N (No).");
            }
        }
        

        private static void PrintError(string message)
        {
            Console.WriteLine(message);
        }
    }
}