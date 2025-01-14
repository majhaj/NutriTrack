using Microsoft.EntityFrameworkCore;
using NutriTrack.Data;
using NutriTrackData.Entities;
using NutriTrackData.Models;
using System.Linq;

public class MealService : IMealService
{
    private readonly ApplicationDbContext _context;

    public MealService(ApplicationDbContext context)
    {
        _context = context;
    }

    // Pobieranie posiłków użytkownika
    public async Task<List<Meal>> GetMealsByUserAsync(string userId)
    {
        return await _context.Meals
                             .Where(m => m.UserId == userId)
                             .Include(m => m.MealProducts)
                             .ThenInclude(mp => mp.Product)
                             .ToListAsync();
    }

    // Pobieranie posiłku po jego ID
    public async Task<Meal> GetMealByIdAsync(int mealId)
    {
        return await _context.Meals
                             .Include(m => m.MealProducts)
                             .ThenInclude(mp => mp.Product)
                             .FirstOrDefaultAsync(m => m.Id == mealId);
    }

    public async Task SaveMealAsync(string userId, MealModel mealModel)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException("UserId cannot be null or empty.");
        }

        var meal = new Meal
        {
            Name = mealModel.Name,
            CategoryId = mealModel.CategoryId,
            UserId = userId,
            Time = DateTime.UtcNow
        };

        var totalCalories = 0;
        foreach (var productModel in mealModel.Products)
        {
            var product = await _context.Products.FindAsync(productModel.ProductId);
            if (product != null)
            {
                totalCalories += (int)(product.Calories * (productModel.Quantity / 100.0));
            }
        }

        meal.Calories = totalCalories;

        _context.Meals.Add(meal);
        await _context.SaveChangesAsync();

        foreach (var productModel in mealModel.Products)
        {
            var mealProduct = new MealProduct
            {
                MealId = meal.Id,
                ProductId = productModel.ProductId,
                Quantity = productModel.Quantity
            };

            _context.MealProducts.Add(mealProduct);
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteMealAsync(int id)
    {
        var meal = await _context.Meals.Include(m => m.MealProducts).FirstOrDefaultAsync(m => m.Id == id);
        if (meal == null) throw new Exception("Meal not found.");

        _context.MealProducts.RemoveRange(meal.MealProducts);
        _context.Meals.Remove(meal);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMealAsync(int id, MealModel mealModel)
    {
        var meal = await _context.Meals.Include(m => m.MealProducts).FirstOrDefaultAsync(m => m.Id == id);
        if (meal == null)
        {
            throw new Exception("Meal not found.");
        }

        meal.Name = mealModel.Name;
        meal.CategoryId = mealModel.CategoryId;

        var totalCalories = 0;
        foreach (var productModel in mealModel.Products)
        {
            var product = await _context.Products.FindAsync(productModel.ProductId);
            if (product != null)
            {
                totalCalories += (int)(product.Calories * (productModel.Quantity / 100.0));
            }
        }

        meal.Calories = totalCalories;

        _context.MealProducts.RemoveRange(meal.MealProducts);
        foreach (var productModel in mealModel.Products)
        {
            var mealProduct = new MealProduct
            {
                MealId = meal.Id,
                ProductId = productModel.ProductId,
                Quantity = productModel.Quantity
            };

            _context.MealProducts.Add(mealProduct);
        }

        await _context.SaveChangesAsync();
    }
}
