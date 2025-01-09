using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriTrackData.Entities
{
    public class PhysicalActivity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Calories burned per minute is required")]
        public int CaloriesBurnedPerMinute { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        public double Duration { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
