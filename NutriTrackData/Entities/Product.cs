using System.ComponentModel.DataAnnotations;

namespace NutriTrackData.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Calories is required")]
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }

        public List<MealProduct> MealProducts { get; set; } = new List<MealProduct>();

    }
}