using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NutriTrackData.Entities
{
    public class Meal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Calories { get; set; }

        public DateTime Time { get; set; } = DateTime.UtcNow;


        [ForeignKey("CategoryId")]
        public MealCategory Category { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey("UserId")]
        [Required]
        public User User { get; set; }
        [Required]
        public string UserId { get; set; }

        public List<MealProduct> MealProducts = new List<MealProduct>();
    }
}
