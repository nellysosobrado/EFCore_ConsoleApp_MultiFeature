using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary;

public interface IApplicationDbContext
{
    DbSet<Calculator> Calculations { get; set; }
    int SaveChanges();
}
