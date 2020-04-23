﻿using Microsoft.EntityFrameworkCore;
using Project33.Models;
using Project33.Services;
namespace Project33.Data
{
    public class BooksContext : DbContext
    {
        public BooksContext(DbContextOptions<BooksContext> options) : base(options)
        {
        }

        public BooksContext()
        {
            Database.EnsureCreated();
        }
        public  DbSet<Books> Books { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=Books;Username=postgres;Password=1923148");
        }

    }
}