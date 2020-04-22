﻿﻿using Microsoft.EntityFrameworkCore;
using Project33.Models;

 namespace Project33.Data
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=WebAppUsers;Username=postgres;Password=85ilasin85");
        }
    }
}