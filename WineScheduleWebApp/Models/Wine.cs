using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WineScheduleWebApp.Models.WineViewModels;

namespace WineScheduleWebApp.Models
{
    public class Wine : BaseModel
    {
        public Wine()
        {
            WineGrapes = new List<WineGrape>();
        }
        // Primary Key
        public string Id { get; set; }
        // Foreign Keys
        public string AppellationId { get; set; }
        public virtual Appellation Appellation { get; set; }
        public string RegionId { get; set; }
        public virtual Region Region { get; set; }
        public string DrynessId { get; set; }
        public virtual Dryness Dryness { get; set; }
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public List<WineGrape> WineGrapes { get; set; }
        // Attributes
        [Required]
        public string Name { get; set; }
        public short Year { get; set; }
        public double Price { get; set; }
        public short Rating { get; set; }
        public short Stock { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
    }
}
