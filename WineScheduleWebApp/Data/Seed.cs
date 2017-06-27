using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WineScheduleWebApp.Models;

namespace WineScheduleWebApp.Data
{
    public static class Seed
    {
        // Not in use
        public static void CreateDefaultData(this ApplicationDbContext context)
        {
            if (context.AllMigrationsApplied())
            {
            }

        }
        public static void CreateSeedData(this ApplicationDbContext context, string userId)
        {
            if (context.AllMigrationsApplied())
            {
                List<Category> categories = new List<Category>();
                context.Category.AddRange(
                    new Category { ApplicationUserId = userId, Name = "Red", Identifier = 1 },
                    new Category { ApplicationUserId = userId, Name = "White", Identifier = 2 },
                    new Category { ApplicationUserId = userId, Name = "Rose", Identifier = 3 },
                    new Category { ApplicationUserId = userId, Name = "Sparkling", Identifier = 4 }
                    );

                List<Dryness> drynesses = new List<Dryness>();
                context.Dryness.AddRange(
                    new Dryness { ApplicationUserId = userId, Name = "Dry", Identifier = 1 },
                    new Dryness { ApplicationUserId = userId, Name = "Semidry", Identifier = 2 },
                    new Dryness { ApplicationUserId = userId, Name = "Sweet", Identifier = 3 }
                    );

                List<Region> regions = new List<Region>();
                context.Region.AddRange(
                    new Region { ApplicationUserId = userId, Name = "Bordeaux" },
                    new Region { ApplicationUserId = userId, Name = "Piemonte" },
                    new Region { ApplicationUserId = userId, Name = "Espagne" }
                    );

                int i = context.SaveChanges();
                if(i > 0)
                {
                    var newRegions = context.Region
                        .Where(c => c.ApplicationUserId == userId)
                        .ToList();
                    List<Appellation> appellations = new List<Appellation>();
                    foreach (var region in newRegions)
                    {
                        switch (region.Name)
                        {
                            case "Bordeaux":
                                context.Appellation.AddRange(
                                    new Appellation { ApplicationUserId = userId, Name = "St. Emilion Grand Cu", RegionId = region.Id },
                                    new Appellation { ApplicationUserId = userId, Name = "Haut Medoc Cru Bourg.", RegionId = region.Id },
                                    new Appellation { ApplicationUserId = userId, Name = "Margaux", RegionId = region.Id },
                                    new Appellation { ApplicationUserId = userId, Name = "St. Estephe", RegionId = region.Id },
                                    new Appellation { ApplicationUserId = userId, Name = "Medoc Cru Bourg", RegionId = region.Id },
                                    new Appellation { ApplicationUserId = userId, Name = "Medoc", RegionId = region.Id },
                                    new Appellation { ApplicationUserId = userId, Name = "AOC Bordeaux", RegionId = region.Id }
                                );
                                break;
                            case "Piemonte":
                                context.Appellation.Add(new Appellation { ApplicationUserId = userId, Name = "DOCG", RegionId = region.Id });
                                break;
                            case "Espagne":
                                context.Appellation.Add(new Appellation { ApplicationUserId = userId, Name = "Rioja", RegionId = region.Id });
                                break;
                            default:
                                break;
                        }
                    }

                    var newCategories = context.Category
                        .Where(c => c.ApplicationUserId == userId)
                        .ToList();

                    List<Grape> grapes = new List<Grape>();
                    foreach (var category in newCategories)
                    {
                        switch (category.Name)
                        {
                            case "Red":
                                context.Grape.AddRange(
                                    new Grape { ApplicationUserId = userId, Name = "Carbernet Franc", CategoryId = category.Id },
                                    new Grape { ApplicationUserId = userId, Name = "Cabernet Sauvignon", CategoryId = category.Id },
                                    new Grape { ApplicationUserId = userId, Name = "Grenache", CategoryId = category.Id },
                                    new Grape { ApplicationUserId = userId, Name = "Pinot Noir", CategoryId = category.Id },
                                    new Grape { ApplicationUserId = userId, Name = "Carignan", CategoryId = category.Id },
                                    new Grape { ApplicationUserId = userId, Name = "Gamay", CategoryId = category.Id },
                                    new Grape { ApplicationUserId = userId, Name = "Mourvedre", CategoryId = category.Id },
                                    new Grape { ApplicationUserId = userId, Name = "Syrah", CategoryId = category.Id },
                                    new Grape { ApplicationUserId = userId, Name = "Merlot", CategoryId = category.Id }
                                );
                                break;
                            case "White":
                                //context.Grape.AddRange(
                                //    new Grape { ApplicationUserId = userId, Name = "Chardonnay" },
                                //    new Grape { ApplicationUserId = userId, Name = "Riesling" },
                                //    new Grape { ApplicationUserId = userId, Name = "Mueller Thurgau" },
                                //    new Grape { ApplicationUserId = userId, Name = "Cabernet Sauvignon" }
                                //);
                                break;
                            case "Rose":
                                break;
                            case "Sparkling":
                                break;
                            default:
                                break;
                        }
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
