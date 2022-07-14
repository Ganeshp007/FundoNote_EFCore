using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Services.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Services
{
    public class FundoContext : DbContext
    {
        public FundoContext(DbContextOptions options): base(options)
        {

        }
        public DbSet<User> Users { get; set; }
    }
}
