using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Databases
{
    public class RealDatabase(DbContextOptions<RealDatabase> options) : DbContext(options)
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-7DUG0J0\\SQLEXPRESS01;Database=MyNewDatabase;Trusted_Connection=True;TrustServerCertificate=true;");
            }                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {                     
            base.OnModelCreating(modelBuilder);
            // Configure the primary key for the entities
            modelBuilder.Entity<Book>().HasKey(b => b.Id);
            modelBuilder.Entity<Author>().HasKey(a => a.Id);
            modelBuilder.Entity<User>().HasKey(u => u.Id);

            // Configure the one-to-many relationship
            modelBuilder.Entity<Book>()
            .HasOne(b => b.Author)     // Each Book has one Author
            .WithMany(a => a.Books)    // Each Author has many Books
            .HasForeignKey(b => b.AuthorId) // Foreign Key
            .OnDelete(DeleteBehavior.Cascade); // When Author is deleted, delete their Books
        }

    }
}
