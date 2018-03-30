using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample
{
    public class AppDbContext : DbContext
    {
        public AppDbContext( DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }

    }
}
