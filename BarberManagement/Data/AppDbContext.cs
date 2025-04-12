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
    }
}
