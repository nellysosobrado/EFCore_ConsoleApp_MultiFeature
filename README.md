# **Project 1**

## **Table of Contents**
1. [Project Description](#project-description)
2. [Technologies](#technologies)
3. [Installation](#installation)
4. [features](#features)
5. [Design Patterns](#design-patterns)
6. [Lessons Learned and Reflections](#lessons-learned-and-reflections)
7. [Flowchart](#Flowchart)

---

## **Project Description**
This multi-project console application is built using Entity Framework Core with the Code First approach to manage the database. The goal is to create an application that handles several features using the same SQL database.
1. **ShapeApp Project** – Calculates area and perimeter for various geometric shapes.
2. **CalculatorApp Project** – A calculator that performs basic mathematical operations.
3. **RPSGameApp Project** – A rock-paper-scissors game against the computer, with results and statistics stored in a database.
4. **ClassLibrary Project**– Contains shared classes and reusable code used across multiple applications, such as utility functions and entity models.
5. **Startup Project** – The entry point for the application; responsible for initializing and coordinating other projects.
6. **DAL (Data Access Layer) Project** – Manages database interactions, such as CRUD operations, and handles communication with the database using Entity Framework Core.


---

## **Technologies**
The project is developed using the following technologies and libraries:
- **Visual Studio**
- **SQL Database**

## **Nuget packages**
- **Autofac** 
- **Spectre.Console** 
- **Fluent validation** 
- **Microsoft.EntityFrameworkCore.SqlServerr**
- **Microsoft.EntityFrameworkCore.Tools**
- **Microsoft.Extensions.Configuration.Design**

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

## **Features**
The project is structured with a clear architecture and design patterns to ensure the code is modular and maintainable. The solution consists of several projects, each with specific responsibilities:
- **Startup Project:** This is where the program starts. It sets up other parts, like connecting to the database and making sure ShapeApp, CalculatorApp, and RPSGameApp work together smoothly.
- **ClassLibrary Project:** This part has reusable pieces like interfaces, models, utilities, enums, and repositories. It helps other parts of the project work the same way and makes the code easy to reuse.
- **DAL Project:** This part handles the database connection and all the database work. It includes settings and configurations to keep everything organized and easy to manage.
- **ShapeApp Project:** This part calculates area and perimeter for shapes. It lets users create, read, update, and delete calculations, using soft delete to mark deleted records without permanently removing them. It uses a pattern to make shapes based on user input and stores all calculations with timestamps and deletion dates in the database.
- **CalculatorApp Project:** This part does basic math like adding, subtracting, multiplying, dividing, finding the square root, and modulus. It lets users save, view, update, and delete calculations. All calculations are saved in the database with two-decimal precision to keep them accurate.
- **RPSGameApp Project:** This is a game where you play rock-paper-scissors against the computer. It keeps track of your choices, the computer's choices, who wins, and when you played. It also shows how often you win. The main focus is on playing the game and showing your stats (Win/Losses/Draws/AverageWins)

## **Design Patterns**
In my project, I utilized several design patterns to ensure the code is modular, maintainable, and scalable.

- **Factory Pattern:** I implemented the Factory Pattern in the ShapeApp to dynamically create geometric objects based on user input. This approach provides flexibility and extensibility, allowing the application to easily accommodate new shapes in the future.

- **Repository Pattern:** The Repository Pattern was used to abstract database queries and organize data access. By centralizing data access logic, this pattern makes the code more modular and testable, ensuring that changes to the database structure have minimal impact on the rest of the application.

- **Dependency Injection:** I used Autofac for dependency injection to manage dependencies between classes. This pattern improves modularity and testability by allowing classes to be easily swapped out or modified without affecting other parts of the application.

- **SOLID Principles:** Throughout the project, I adhered to SOLID principles to ensure that each class has a clear, single responsibility. This approach makes the codebase more maintainable and scalable, as it promotes separation of concerns and reduces the likelihood of introducing bugs when making changes.

---

## **Lessons Learned and Reflections**
- I learned how to use **design patterns** like Factory Pattern and Repository Pattern to create flexible and scalable code.
- Implementing **Entity Framework Core** and managing databases with the **Code First approach** gave me deeper insights into database technology.
- Structuring the project with clear folders and following **SOLID principles** made the codebase easier to manage and extend.
- I also gained experience in using **Git** and **branching**, which made the work more efficient.
  
---

## **Flowchart**

![Proejct1 drawio](https://github.com/user-attachments/assets/d3fc13f3-5f85-4869-aa14-ac3ca2e9e613)


