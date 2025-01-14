using NutriTrackData.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutriTrackData.Models
{
    public class MealModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<ProductQuantityModel> Products { get; set; } = new List<ProductQuantityModel>();
    }


    public class ProductQuantityModel
        {
            public int ProductId { get; set; }
            public double Quantity { get; set; }
        }
    }
