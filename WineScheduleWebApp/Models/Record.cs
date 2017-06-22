using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WineScheduleWebApp.Models
{
    public class Record : BaseModel
    {
        public string Id { get; set; }
        [Required]
        public string WineId { get; set; }
        public virtual Wine Wine { get; set; }

        public double Price { get; set; }
        [Required]
        public short Quantity { get; set; }
        
    }
}
