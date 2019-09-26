using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WineScheduleWebApp.Models
{
    public class Region : BaseModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Country { get; set; }
    }
}
