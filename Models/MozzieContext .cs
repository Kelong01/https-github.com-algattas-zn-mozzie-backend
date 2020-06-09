using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MozzieAiSystems.Models
{
    public class MozzieContext : DbContext
    {
        public MozzieContext(DbContextOptions<MozzieContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationFile> LocationFiles { get; set; }
        public DbSet<AlgattasCmsSession> AlgattasCmsSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<User>()
            //    .Property(b => b.Rating)
            //    .HasDefaultValue(3);

            modelBuilder.Entity<Location>().HasMany(p => p.Files).WithOne(p => p.Location)
                .HasForeignKey(p => p.LocationId);
        }
    }
}
