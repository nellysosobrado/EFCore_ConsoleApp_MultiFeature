# Project 1: .NET Console App

## Introduction
This project is mulit project, **console application** built using the **Entity Framework with Code First approach**. The goal of the project is to create an application that handles several features, using same sql database. :

- Geometric shape calculations project
- A calculator project
- A Rock, Paper, Scissors game project
- Classlibrary project
- Data acces Layer project

The application is designed with a focus on Object-Oriented Programming (OOP) and is built using modern software development principles such as **Dependency Injection**, **DRY**, and **Design Patterns**. 

---

## Features

### **Part 1: Shapes**
- Users can:
  - Input parameters for different shapes such as rectangles, triangles, parallelograms, and rhombuses.
  - Calculate and save the area and perimeter to the database.
  - Seed initial data for each shape.
  - Use CRUD operations to add, update, soft-delete, and display existing shapes.
- All shapes are saved to the database along with the date the calculation was performed.

#### Techniques and Design:
- **Error Handling**:
  - Invalid inputs are handled with **FluentValidation** and Spectre.Console.
  - Errors are caught using **try-catch** blocks.
- **Design Patterns**:
  - **Strategy Pattern** to handle different shapes and their calculation logic.
  - **Repository Pattern** for database access.

### **Part 2: Calculator**
- Features:
  - Support for arithmetic operations: addition, subtraction, multiplication, division, square root, and modulus.
  - Results are displayed with two decimal
  - Calculations are saved in a separate sql database table with inputs, operator, result,date and soft-delete (isdeleted)
  - CRUD operations are available for saved calculations.

#### Techniques and Design:
- **Error Handling**:
  - Validation of input numbers and operators using Spectre.Console and FluentValidation.
  -  Errors are caught using **try-catch** blocks.
- **Design Patterns**:
  - **Strategy Pattern** to manage different operations.
  - **Service Pattern** to separate business logic from the UI.

### **Part 3: Rock, Paper, Scissors**
- Features:
  - Play against the computer and save results (win, loss, draw) along with the date.
  - Calculate the average win percentage based on all past games.
  - View a list of previous games, including the player’s choice, the computer’s choice, the result, and the date.

#### Techniques and Design:
- **Design Patterns**:
  - **Service Pattern** to handle game logic.

---

## Technologies Used

### **Languages and Frameworks:**
- C#
- .NET Core
- Entity Framework Core

### **Database:**
- SQL Server: Used as the database solution for storing application data.
- Database management is handled via **Entity Framework Code First**.
- All three parts of the application (Shapes, Calculator, and Rock, Paper, Scissors) use the same SQL database but store data in separate tables to ensure modular and organized data handling.
- SQL Server: Used as the database solution for storing application data.
- Database management is handled via **Entity Framework Code First**.
- SQL Server
- Database management is handled via **Entity Framework Code First**.

### **NuGet Packages:**
- **Spectre.Console**: For elegant and clear console output.
- **FluentValidation**: For input validation.
- **Autofac**: For Dependency Injection.
- **Microsoft.Extensions.Hosting**: To manage the application lifecycle.
- **Microsoft.EntityFrameworkCore**: For database interactions.

---

## Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/<your-username>/console-app-project.git
   ```

2. **Open the project in Visual Studio:**
   - Navigate to the cloned folder and open the project file.

3. **Set the startup project:**
   - Right-click on the project in **Solution Explorer** and select **"Set as Startup Project"**.

4. **Build and run the application:**
   - Press **F5** or select **Run** from the menu.


