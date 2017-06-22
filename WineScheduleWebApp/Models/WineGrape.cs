using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WineScheduleWebApp.Models
{
    public class WineGrape : BaseModel
    {
        public string WineId { get; set; }
        public string GrapeId { get; set; }
        public virtual Wine Wine { get; set; }
        public virtual Grape Grape { get; set; }
    }
}
