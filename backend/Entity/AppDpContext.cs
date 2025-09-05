<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using BasarStajApp.Entity;

namespace BasarStajApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<Feature> Features { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Feature>()
                .Property(f => f.Location)
                .HasColumnType("geometry"); // PostGIS tüm Geometry tiplerini destekler
        }

=======
﻿using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

using System.Security.Cryptography.X509Certificates;

namespace BasarStajApp.Entity
{
    public class AppDpContext :DbContext


    {
        public AppDpContext(DbContextOptions<AppDpContext> options): base(options) { }

        public DbSet<Point> Points { get; set; }
>>>>>>> dcc10a6 (son front end)
    }
}
