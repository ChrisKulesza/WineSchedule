using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WineScheduleWebApp.Models
{
    public class BaseModel
    {
        public string ApplicationUserId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime LastModifiedDate { get; set; }
        public void MarkUpdate()
        {
            this.LastModifiedDate = DateTime.Now;
        }
    }
}
