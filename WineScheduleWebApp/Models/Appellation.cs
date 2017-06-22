using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WineScheduleWebApp.Models
{
    public class Appellation : BaseModel
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
