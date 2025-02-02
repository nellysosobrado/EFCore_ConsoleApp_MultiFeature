﻿using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassLibrary.Interface;

public interface IApplicationDbContext
{
    DbSet<Calculator> Calculations { get; set; }
    DbSet<Shape> Shapes { get; set; }
    DbSet<Game> Games { get; set; }
    int SaveChanges();
}
