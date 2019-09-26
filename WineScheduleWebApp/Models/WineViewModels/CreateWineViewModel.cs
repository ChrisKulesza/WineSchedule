using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WineScheduleWebApp.HelperModels;

namespace WineScheduleWebApp.Models.WineViewModels
{
    public class CreateWineViewModel
    {
        public CreateWineViewModel()
        {
            IdCheckBoxes = new List<IdCheckBox>();
        }
        public List<IdCheckBox> IdCheckBoxes { get; set; }
        [Required]
        public Wine Wine { get; set; }
    }
}
