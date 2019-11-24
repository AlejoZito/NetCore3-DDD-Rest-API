using Microsoft.EntityFrameworkCore;
using NetCore3_api.Domain.Models.Aggregates.Event;
using NetCore3_api.Domain.Models.Aggregates.User;
using NetCore3_api.Infrastructure.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore3_api.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Charge> Charges { get; set; }
        public DbSet<ChargeCategory> ChargeCategories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(
            //    @"Server=(localdb)\mssqllocaldb;Database=Blogging;Integrated Security=True");
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/ef/ef6/modeling/code-first/fluent/types-and-properties
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new ChargeConfiguration());

            Seed(modelBuilder);
        }

        private static void Seed(ModelBuilder modelBuilder)
        {
            ChargeCategory marketPlaceCategory = new ChargeCategory() { Id = 1, Name = "MarketPlace" };
            ChargeCategory serviciosCategory = new ChargeCategory() { Id = 2, Name = "Servicios" };
            ChargeCategory externoCategory = new ChargeCategory() { Id = 3, Name = "Externo" };
            modelBuilder.Entity<ChargeCategory>().HasData(marketPlaceCategory);
            modelBuilder.Entity<ChargeCategory>().HasData(serviciosCategory);
            modelBuilder.Entity<ChargeCategory>().HasData(externoCategory);

            modelBuilder.Entity<EventType>().HasData(new { Id = 1L, Name = "Clasificado", CategoryId = marketPlaceCategory.Id });
            modelBuilder.Entity<EventType>().HasData(new { Id = 2L, Name = "Venta", CategoryId = marketPlaceCategory.Id });
            modelBuilder.Entity<EventType>().HasData(new { Id = 3L, Name = "Envío", CategoryId = marketPlaceCategory.Id });
            modelBuilder.Entity<EventType>().HasData(new { Id = 4L, Name = "Crédito", CategoryId = serviciosCategory.Id });
            modelBuilder.Entity<EventType>().HasData(new { Id = 5L, Name = "Fidelidad", CategoryId = serviciosCategory.Id });
            modelBuilder.Entity<EventType>().HasData(new { Id = 6L, Name = "Publicidad", CategoryId = serviciosCategory.Id });
            modelBuilder.Entity<EventType>().HasData(new { Id = 7L, Name = "MercadoPago", CategoryId = externoCategory.Id });
            modelBuilder.Entity<EventType>().HasData(new { Id = 8L, Name = "MercadoShop", CategoryId = externoCategory.Id });

            modelBuilder.Entity<User>().HasData(new User() { Id = 1, Username = "Hermeto Pascoal" });
            modelBuilder.Entity<User>().HasData(new User() { Id = 2, Username = "Leon Montana" });
        }

        public override int SaveChanges()
        {
            //ToDo: Domain event handling

            return base.SaveChanges();
        }
    }
}

