using System.ComponentModel.DataAnnotations;

namespace NutriTrackData.Entities
{
    public class MealCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public List<Meal> Meals { get; set; } 
    }
}
