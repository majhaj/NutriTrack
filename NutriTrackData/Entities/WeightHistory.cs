using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriTrackData.Entities
{
    public class WeightHistory
    {
        [Key]
        public int Id { get; set; }
        public double Weight { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
