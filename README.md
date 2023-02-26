
# BMI calculator in C# console app

This is a BMI calculator program written in C# that stores user information and BMI calculation results in a MySQL database. The program starts by asking the user to either register or login. If the user chooses to register, they are prompted to enter a username and password, which are then stored in the database. If the user chooses to login, they are prompted to enter their username and password, and the program checks the database to see if the combination exists. If the user's credentials are valid, they are taken to the BMI calculator, where they can enter their height and weight and get their BMI and weight status.




## Features

* Auth system: Users can register and login

* BMI Calculation: Users can calculate their BMI based on their height and weight.

* Measurement System Selection: Users can select either the imperial or metric system for height and weight input.

* Weight Status Indication: After the BMI is calculated, the application indicates whether the user is underweight, normal weight, overweight, or obese based on their BMI.

* MySQL Database Integration: The application uses a MySQL database to store user information, including username and password.

* Command-Line Interface: The application has a command-line interface where users interact with the application.
