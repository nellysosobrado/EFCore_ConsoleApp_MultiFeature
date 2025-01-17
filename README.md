# **Project 1: Console App with EF Core**

## **Table of Contents**
1. [Project Description](#project-description)
2. [Technologies](#technologies)
3. [Features](#features)
4. [Installation](#installation)
5. [Architecture and Design Patterns](#architecture-and-design-patterns)
6. [Design Patterns](#design-patterns)
7. [Lessons Learned and Reflections](#lessons-learned-and-reflections)

---

## **Project Description**
This project was developed as part of the *Database Technology* course. The goal is to build a Console App that handles three different modules:
1. **ShapeApp** – Calculates area and perimeter for various geometric shapes.
2. **CalculatorApp** – A calculator that performs basic mathematical operations.
3. **RPSGameApp** – A rock-paper-scissors game against the computer, with results and statistics stored in a database.

The project uses **Entity Framework Core** with the **Code First approach** to manage the database.

---

## **Technologies**
The project is developed using the following technologies and libraries:
- **Visual Studio**
- **SQL Database**

## **Nuget packages**
- **Autofac** (Dependency Injection)
- **Spectre.Console** (For the user interface)
- **Fluent validation** (For the user interface)
- **Microsoft.EntityFrameworkCore.SqlServerr**
- **Microsoft.EntityFrameworkCore.Tools**
- **Microsoft.Extensions.Configuration.Json**





---

## **Features**
### **ShapeApp**
- Calculates area and perimeter for:
  - Rectangle
  - Triangle
  - Parallelogram
  - Rhombus
- CRUD functionality to manage saved calculations.
- Soft delete functionality to mark calculations as deleted without removing them from the database.
- All calculations are saved to the database with timestamps.

### **CalculatorApp**
- Supports basic mathematical operations:
  - Addition (+), Subtraction (-), Multiplication (*), Division (/)
  - Square root (√) and Modulus (%)
- CRUD functionality to manage saved calculations.
- Soft delete functionality to mark calculations as deleted without removing them from the database.
- Results are saved to the database with two decimal precision.

### **RPSGameApp**
- Play rock-paper-scissors against the computer.
- Saves game history, including:
  - Player choice, computer choice, result, and timestamp.
- Displays statistics, including average win rate.

---

## **Installation**
1. Clone this repository:
   ```bash
   git clone https://github.com/<your-repo-name>.git
   ```
5. Create the database:
   ```bash
   dotnet ef database update
   ```
 5. Set 'StartUp' Project as startup project:
   ```
   Right click 'Startup' Project. Click 'Set as startup project'
   ```
6. Run the application:
   ``` Visual Studio
   F5
   ```

---

## **Architecture and Design Patterns**
The project is structured with a clear architecture and design patterns to ensure the code is modular and maintainable. The solution consists of several projects, each with specific responsibilities:
- **Startup Project:** This is the entry point of the application where the program begins execution. It coordinates the initialization of other components, such as setting up dependency injection using Autofac and configuring database connections through the DAL. The Startup Project ensures seamless communication between ShapeApp, CalculatorApp, and RPSGameApp by referencing their services and managing their dependencies.
- **ClassLibrary Project:** This project contains reusable components, including interfaces, shared models, utilities, common enums with their extensions (e.g., enum extensions for menu management), and repositories. Repositories in this project are responsible for abstracting database queries, ensuring that the logic for accessing and manipulating data is centralized and reusable. The ClassLibrary Project serves as a foundation that other projects reference, ensuring consistency, modularity, and reusability across the solution.
-**DAL (Data Access Layer) Project:** Responsible for managing the database connection and handling all interactions with the database. It includes the DbContext, and configuration files like appsettings.json to store connection strings and other database-related settings. This ensures all database-related logic and configurations are centralized and easy to manage.
- **ShapeApp Project:** Focuses on geometric calculations, such as area and perimeter, for various shapes. It includes full CRUD functionality, allowing users to create new calculations, read saved data, update existing entries, and mark records as deleted using soft delete. The application uses the Factory Pattern to dynamically create geometric objects based on user input, ensuring a modular and extensible design. The CRUD operations are implemented through repositories to ensure separation of concerns and maintainability. All calculations, along with their timestamps, are stored in the database for future reference.
- **CalculatorApp Project:** Designed to perform basic mathematical operations such as addition, subtraction, multiplication, and division. This project includes CRUD functionality that allows users to:
- Create: Save new calculations with operator details and numerical inputs.
- Read: View previously saved calculations with results and timestamps.

- Update: Modify stored calculations if corrections are needed.
- Delete: Soft delete entries, marking them as inactive without permanent removal, preserving data integrity and enabling recovery if necessary.

The CRUD operations are managed through repositories to keep database logic organized and separate from the business logic. All calculations are saved in the database with two-decimal precision for results, ensuring accuracy and consistency in record-keeping.

- **RPSGameApp Project:** is a game where you play rock-paper-scissors against the computer. The game keeps track of what you and the computer choose, who wins, and when each game was played. It also shows you how often you win. The main focus is on playing the game and showing your stats. 

## **Design Patterns**

- Factory Pattern: Used in ShapeApp to dynamically create geometric objects, ensuring flexibility and extensibility.

- Repository Pattern: Abstracts database queries and organizes data access, making the code more modular and testable.

- Dependency Injection: Implemented using Autofac to manage dependencies between classes, improving modularity and testability.

- SOLID Principles: Followed throughout the project to ensure classes have clear, single responsibilities, making the codebase more maintainable and scalable.

---

## **Lessons Learned and Reflections**
- I learned how to use **design patterns** like Factory Pattern and Repository Pattern to create flexible and scalable code.
- Implementing **Entity Framework Core** and managing databases with the **Code First approach** gave me deeper insights into database technology.
- Structuring the project with clear folders and following **SOLID principles** made the codebase easier to manage and extend.
- I also gained experience in using **Git** and **branching**, which made the work more efficient.


