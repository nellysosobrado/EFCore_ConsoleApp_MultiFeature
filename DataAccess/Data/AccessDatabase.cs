using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ClassLibrary.Data;

namespace DAL.Data;

public class AccessDatabase
{
    public ApplicationDbContext GetDbContext()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var config = builder.Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        var options = new DbContextOptionsBuilder<ApplicationDbContext>();
        options.UseSqlServer(connectionString);

        return new ApplicationDbContext(options.Options);
    }
}
