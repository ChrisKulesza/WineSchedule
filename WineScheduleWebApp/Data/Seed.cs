using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineScheduleWebApp.Models;

namespace WineScheduleWebApp.Data
{
    public static class Seed
    {
        public static void SeedData(this ApplicationDbContext context)

        {
            if (context.AllMigrationsApplied())
            {
                List<Appellation> appellations = new List<Appellation>();
                if (!context.Appellation.Any())
                {
                    context.Appellation.AddRange(
                        new Appellation { Name = "St. Emilion Grand Cu" },
                        new Appellation { Name = "Medoc" },
                        new Appellation { Name = "AOC Bordeaux"},
                        new Appellation { Name = "Rioja" }
                        );
                }
                else
                {
                    appellations = context.Appellation.ToList();
                }

                List<Region> regions = new List<Region>();
                if (!context.Region.Any())
                {
                    context.Region.AddRange(
                        new Region { Name = "Bordeaux" },
                        new Region { Name = "Piemonte" },
                        new Region { Name = "Espagne" }
                        );
                }
                else
                {
                    regions = context.Region.ToList();
                }

                List<Category> categories = new List<Category>();
                if (!context.Category.Any())
                {
                    context.Category.AddRange(
                        new Category { Name = "Red", Identifier = 1 },
                        new Category { Name = "White", Identifier = 2 },
                        new Category { Name = "Rose", Identifier = 3 },
                        new Category { Name = "Sparkling", Identifier = 4 }
                        );
                }
                else
                {
                    categories = context.Category.ToList();
                }

                List<Dryness> drynesses = new List<Dryness>();
                if (!context.Dryness.Any())
                {
                    context.Dryness.AddRange(
                        new Dryness { Name = "Dry", Identifier = 1 },
                        new Dryness { Name = "Semidry", Identifier = 2 },
                        new Dryness { Name = "Sweet", Identifier = 3 }
                        );
                }
                else
                {
                    drynesses = context.Dryness.ToList();
                }

                List<Grape> grapes = new List<Grape>();
                if (!context.Grape.Any())
                {
                    context.Grape.AddRange(
                        new Grape { Name = "Chardonnay" },
                        new Grape { Name = "Riesling" },
                        new Grape { Name = "Mueller Thurgau" },
                        new Grape { Name = "Cabernet Sauvignon" }
                        );
                }
                else
                {
                    grapes = context.Grape.ToList();
                }
                context.SaveChanges();
            }
        }
    }
}
