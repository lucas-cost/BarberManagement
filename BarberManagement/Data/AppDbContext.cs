using BarberManagement.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace BarberManagement.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        private static string DbPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "barber.db");

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed com usuário admin
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Email = "admin@xavier.com",
                Senha = "1234",
                IsAdmin = true
            });
        }
    }
}
