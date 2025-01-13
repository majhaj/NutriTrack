using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriTrackData.Models
{
    public class PhysicalActivityModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Calories burned per minute is required")]
        public int CaloriesBurnedPerMinute { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        public int Duration { get; set; }
    }
}
