using NutriTrackData.Entities;
using NutriTrackData.Models;

public interface IMealService
{
    Task<List<Meal>> GetMealsByUserAsync(string userId);
    Task<Meal> GetMealByIdAsync(int mealId);
    Task SaveMealAsync(string userId, MealModel mealModel);
    Task DeleteMealAsync(int id);
    Task UpdateMealAsync(int id, MealModel mealModel);
}
