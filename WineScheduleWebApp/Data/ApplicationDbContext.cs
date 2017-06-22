using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WineScheduleWebApp.Models;

namespace WineScheduleWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<WineGrape>().HasKey(wg => new { wg.WineId, wg.GrapeId });
        }

        public DbSet<WineScheduleWebApp.Models.ApplicationUser> ApplicationUser { get; set; }

        public DbSet<WineScheduleWebApp.Models.Region> Region { get; set; }
        public DbSet<WineScheduleWebApp.Models.Grape> Grape { get; set; }
        public DbSet<WineScheduleWebApp.Models.Record> Record { get; set; }
        public DbSet<WineScheduleWebApp.Models.Wine> Wine { get; set; }
        public DbSet<WineScheduleWebApp.Models.WineGrape> WineGrape { get; set; }
        public DbSet<WineScheduleWebApp.Models.Appellation> Appellation { get; set; }
    }
}
