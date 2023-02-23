using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace mmmmmm
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connection string for MySQL database
            string connectionString = "server=localhost;database=bmi_calculator;uid=root;password=7824;";

            // Create a new MySqlConnection object and open the connection
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Prompt the user to select an option
                Console.WriteLine("Welcome to the BMI Calculator!");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.Write("Enter your choice (1 or 2): ");

                string choice = "";
                while (choice != "1" && choice != "2")
                {
                    
                    choice = Console.ReadLine();

                    if (choice != "1" && choice != "2")
                    {
                        Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                    }
                }

                // Register
                if (choice == "1")
                {
                    Console.Write("Enter a username: ");
                    string username = Console.ReadLine();
                    while (string.IsNullOrEmpty(username))
                    {
                        Console.WriteLine("Username cannot be empty.");
                        Console.Write("Enter a username: ");
                        username = Console.ReadLine();
                    }

                    Console.Write("Enter a password: ");
                    string password = Console.ReadLine();
                    while (string.IsNullOrEmpty(password))
                    {
                        Console.WriteLine("Password cannot be empty.");
                        Console.Write("Enter a password: ");
                        password = Console.ReadLine();
                    }

                    // Check if the username already exists in the database
                    string checkUserQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
                    MySqlCommand checkUserCommand = new MySqlCommand(checkUserQuery, connection);
                    checkUserCommand.Parameters.AddWithValue("@username", username);
                    int userCount = Convert.ToInt32(checkUserCommand.ExecuteScalar());

                    if (userCount > 0)
                    {
                        Console.WriteLine("Username already exists. Please choose a different username.");
                        Console.Write("Enter a username: ");
                        username = Console.ReadLine();
                    }

                    string insertUserQuery = "INSERT INTO users (username, password) VALUES (@username, @password)";
                    MySqlCommand insertUserCommand = new MySqlCommand(insertUserQuery, connection);
                    insertUserCommand.Parameters.AddWithValue("@username", username);
                    insertUserCommand.Parameters.AddWithValue("@password", password);
                    try
                    {
                        insertUserCommand.ExecuteNonQuery();
                        Console.WriteLine("Registration successful!");
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                    // Open the BMI calculator
                    BMICalculator(username, connection);
                }

                // Login
                else if (choice == "2")
                {
                    Console.Write("Enter your username: ");
                    string username = Console.ReadLine();
                    while (string.IsNullOrEmpty(username))
                    {
                        Console.WriteLine("Username cannot be empty.");
                        Console.Write("Enter your username: ");
                        username = Console.ReadLine();
                    }

                    Console.Write("Enter your password: ");
                    string password = Console.ReadLine();
                    while (string.IsNullOrEmpty(password))
                    {
                        Console.WriteLine("Password cannot be empty.");
                        Console.Write("Enter your password: ");
                        password = Console.ReadLine();
                    }

                    string selectUserQuery = "SELECT * FROM users WHERE username = @username AND password = @password";
                    MySqlCommand selectUserCommand = new MySqlCommand(selectUserQuery, connection);
                    selectUserCommand.Parameters.AddWithValue("@username", username);
                    selectUserCommand.Parameters.AddWithValue("@password", password);

                    using (MySqlDataReader reader = selectUserCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("Login successful!");

                            reader.Close();

                            // Open the BMI calculator
                            BMICalculator(username, connection);
                        }
                        else
                        {
                            Console.WriteLine("Incorrect username or password.");
                        }
                    }
                }
                // Invalid choice
                else
                {
                    Console.WriteLine("Invalid choice.");
                }
            }


            static void BMICalculator(string username, MySqlConnection connection)
            {
                Console.WriteLine($"Welcome to the BMI Calculator, {username}!");

                Console.WriteLine("Please select your measurement system: ");
                Console.WriteLine("1. Imperial");
                Console.WriteLine("2. Metric");

                string choice = "";
                while (choice != "1" && choice != "2")
                {

                    choice = Console.ReadLine();

                    if (choice != "1" && choice != "2")
                    {
                        Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                    }
                }
                // imperial
                if (choice == "1")
                {
                    Console.WriteLine("Enter your height in feet and inches.");
                    Console.Write("Feet: ");
                    int feet = int.Parse(Console.ReadLine());
                    Console.Write("Inches: ");
                    int inches = int.Parse(Console.ReadLine());

                    Console.WriteLine("Enter your weight in pounds.");
                    Console.Write("Pounds: ");
                    int pounds = int.Parse(Console.ReadLine());

                    // Calculate the BMI
                    double heightInInches = feet * 12 + inches;
                    double bmi = (pounds * 703) / (heightInInches * heightInInches);

                    // Display the BMI and weight status
                    Console.WriteLine($"Your BMI is {bmi:F2}.");
                    if (bmi < 18.5)
                    {
                        Console.WriteLine("You are underweight.");
                    }
                    else if (bmi >= 18.5 && bmi < 25)
                    {
                        Console.WriteLine("You are normal weight.");
                    }
                    else if (bmi >= 25 && bmi < 30)
                    {
                        Console.WriteLine("You are overweight.");
                    }
                    else
                    {
                        Console.WriteLine("You are obese.");
                    }

                    // Get the user ID based on the username
                    string getUserIdQuery = "SELECT id FROM users WHERE username = @username";
                    MySqlCommand getUserIdCommand = new MySqlCommand(getUserIdQuery, connection);
                    getUserIdCommand.Parameters.AddWithValue("@username", username);
                    int userId = 0;
                    try
                    {
                        object result = getUserIdCommand.ExecuteScalar();
                        if (result != null)
                        {
                            userId = Convert.ToInt32(result);
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return;
                    }

                    // Save the BMI data to the database
                    Console.WriteLine("Saving BMI data to database...");
                    string insertBmiQuery = "INSERT INTO imperial (user_id, height_ft, height_in, weight_lbs, bmi) " +
                                            "VALUES (@user_id, @height_ft, @height_in, @weight_lbs, @bmi)";
                    MySqlCommand insertBmiCommand = new MySqlCommand(insertBmiQuery, connection);
                    insertBmiCommand.Parameters.AddWithValue("@user_id", userId);
                    insertBmiCommand.Parameters.AddWithValue("@height_ft", feet);
                    insertBmiCommand.Parameters.AddWithValue("@height_in", inches);
                    insertBmiCommand.Parameters.AddWithValue("@weight_lbs", pounds);
                    insertBmiCommand.Parameters.AddWithValue("@bmi", bmi);
                    try
                    {
                        insertBmiCommand.ExecuteNonQuery();
                        Console.WriteLine("BMI data saved successfully!");
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                }
                //login
                else if (choice == "2")
                {
                    Console.WriteLine("Enter your height in centimeters.");
                    Console.Write("Centimeters: ");
                    double heightCm = double.Parse(Console.ReadLine()) / 100.0;

                    Console.WriteLine("Enter your weight in kilograms.");
                    Console.Write("Kilograms: ");
                    double weightKg = double.Parse(Console.ReadLine());

                    // Calculate the BMI
                    double bmi = weightKg / (heightCm * heightCm);

                    // Display the BMI and weight status
                    Console.WriteLine($"Your BMI is {bmi:F2}.");
                    if (bmi < 18.5)
                    {
                        Console.WriteLine("You are underweight.");
                    }
                    else if (bmi >= 18.5 && bmi < 25)
                    {
                        Console.WriteLine("You are normal weight.");
                    }
                    else if (bmi >= 25 && bmi < 30)
                    {
                        Console.WriteLine("You are overweight.");
                    }
                    else
                    {
                        Console.WriteLine("You are obese.");
                    }

                    // Get the user ID based on the username
                    string getUserIdQuery = "SELECT id FROM users WHERE username = @username";
                    MySqlCommand getUserIdCommand = new MySqlCommand(getUserIdQuery, connection);
                    getUserIdCommand.Parameters.AddWithValue("@username", username);
                    int userId = 0;
                    try
                    {
                        object result = getUserIdCommand.ExecuteScalar();
                        if (result != null)
                        {
                            userId = Convert.ToInt32(result);
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return;
                    }
                    // Save the BMI data to the database
                    Console.WriteLine("Saving BMI data to database...");
                    string insertBmiQuery = "INSERT INTO metric (user_id, height_cm, weight_kg, bmi) " +
                                            "VALUES (@user_id, @height_cm, @weight_kg, @bmi)";
                    MySqlCommand insertBmiCommand = new MySqlCommand(insertBmiQuery, connection);
                    insertBmiCommand.Parameters.AddWithValue("@user_id", userId);
                    insertBmiCommand.Parameters.AddWithValue("@height_cm", heightCm);
                    insertBmiCommand.Parameters.AddWithValue("@weight_kg", weightKg);
                    insertBmiCommand.Parameters.AddWithValue("@bmi", bmi);
                    try
                    {
                        insertBmiCommand.ExecuteNonQuery();
                        Console.WriteLine("BMI data saved successfully!");
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }

                }

            }
        }
    }
}
