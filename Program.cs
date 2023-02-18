using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MyProject
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

                string choice = Console.ReadLine();
                // Register
                if (choice == "1")
                {
                    Console.Write("Enter a username: ");
                    string username = Console.ReadLine();

                    Console.Write("Enter a password: ");
                    string password = Console.ReadLine();

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
                }

                // Login
                else if (choice == "2")
                {
                    Console.Write("Enter your username: ");
                    string username = Console.ReadLine();

                    Console.Write("Enter your password: ");
                    string password = Console.ReadLine();

                    string selectUserQuery = "SELECT * FROM users WHERE username = @username AND password = @password";
                    MySqlCommand selectUserCommand = new MySqlCommand(selectUserQuery, connection);
                    selectUserCommand.Parameters.AddWithValue("@username", username);
                    selectUserCommand.Parameters.AddWithValue("@password", password);

                    using (MySqlDataReader reader = selectUserCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Console.WriteLine("Login successful!");
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

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
