using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WineScheduleWebApp.Models
{
    public class Grape : BaseModel
    {
        public Grape()
        {
            WineGrapes = new List<WineGrape>();
        }
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public List<WineGrape> WineGrapes { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
