using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WineScheduleWebApp.Models
{
    public class Wine : BaseModel
    {
        public Wine()
        {
            WineGrapes = new HashSet<WineGrape>();
        }
        public string RegionId { get; set; }
        public virtual Region Region { get; set; }
        public string AppellationId { get; set; }
        public virtual Appellation Appellation { get; set; }
        public string Id { get; set; }
        public ICollection<WineGrape> WineGrapes { get; set; }
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
