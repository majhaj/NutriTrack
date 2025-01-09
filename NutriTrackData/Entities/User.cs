using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace NutriTrackData.Entities
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; } = "";

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Range(1, 500, ErrorMessage = "Weight must be between 1 and 500 kg.")]
        public float Weight { get; set; }

        [Required]
        [Range(50, 250, ErrorMessage = "Height must be between 50 and 250 cm.")]
        public float Height { get; set; }

        public double BMI { get; set; }

        public List<Meal> Meals { get; set; } = new List<Meal>();
        public List<PhysicalActivity> PhysicalActivities { get; set; } = new List<PhysicalActivity>();
        public List<WeightHistory> WeightHistory { get; set; } = new List<WeightHistory>();

        public void CalculateBMI()
        {
            BMI = Weight / ((Height / 100) * (Height / 100));
        }
    }
}
