using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WineScheduleWebApp.Models
{
    public class Grape : BaseModel
    {
        public string Id { get; set; }
        public ICollection<WineGrape> WineGrapes { get; set; }
        [Required]
        public string Name { get; set; }

    }
}
