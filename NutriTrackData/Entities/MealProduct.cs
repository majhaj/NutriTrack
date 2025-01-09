namespace NutriTrackData.Entities
{
    public class MealProduct
    {
        public int MealId { get; set; }
        public Meal Meal { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public double Quantity { get; set; }
    }
}