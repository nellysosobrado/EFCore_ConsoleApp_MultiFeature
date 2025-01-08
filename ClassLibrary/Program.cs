using System;
using ClassLibrary.Data;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Checking database...");

        var dbContext = DatabaseConfig.GetDbContext();

        dbContext.Database.EnsureCreated();

        Console.WriteLine("Creatwed and connected!");
    }
}
