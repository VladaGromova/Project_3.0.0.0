﻿﻿using Microsoft.EntityFrameworkCore;
using Project33.Services.Models;

 namespace Project33.Data
{
    public class UserContext : DbContext
    {
        
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }
        
        public UserContext()
        {
            Database.EnsureCreated();
        }
        
        public DbSet<User> Users { get; set; }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=WebAppUsers;Username=postgres;Password=1923148");
        }
    }
}