using NutriTrackData.Entities;
using NutriTrackData.Models;
using NutriTrack.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using NutriTrack.Data;

public class MealServiceTests
{
    private readonly MealService _service;
    private readonly ApplicationDbContext _context;

    public MealServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb_Meals")
            .Options;

        _context = new ApplicationDbContext(options);
        _service = new MealService(_context);
    }

    private void ClearDatabase()
    {
        _context.Users.RemoveRange(_context.Users);
        _context.Meals.RemoveRange(_context.Meals);
        _context.MealProducts.RemoveRange(_context.MealProducts);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetMealsByUserAsync_ReturnsEmpty_WhenUserHasNoMeals()
    {
        ClearDatabase();
        var user = new User { Id = "user1", Name = "Test User" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _service.GetMealsByUserAsync(user.Id);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetMealsByUserAsync_ReturnsMeals_WhenUserHasMeals()
    {
        ClearDatabase();
        var user = new User { Id = "user1", Name = "Test User" };
        _context.Users.Add(user);

        var meals = new List<Meal>
        {
            new Meal { Name = "Breakfast", UserId = user.Id, Time = DateTime.UtcNow, Calories = 500 },
            new Meal { Name = "Lunch", UserId = user.Id, Time = DateTime.UtcNow, Calories = 700 }
        };

        _context.Meals.AddRange(meals);
        await _context.SaveChangesAsync();

        var result = await _service.GetMealsByUserAsync(user.Id);

        Assert.Equal(2, result.Count);
        Assert.Equal("Breakfast", result.First().Name);
    }

    [Fact]
    public async Task GetMealByIdAsync_ReturnsMeal_WhenMealExists()
    {
        ClearDatabase();
        var user = new User { Id = "user1", Name = "Test User" };
        _context.Users.Add(user);

        var meal = new Meal { Name = "Dinner", UserId = user.Id, Time = DateTime.UtcNow, Calories = 600 };
        _context.Meals.Add(meal);
        await _context.SaveChangesAsync();

        var result = await _service.GetMealByIdAsync(meal.Id);

        Assert.NotNull(result);
        Assert.Equal(meal.Name, result.Name);
    }

    [Fact]
    public async Task SaveMealAsync_SavesMeal_WhenValidDataProvided()
    {
        ClearDatabase();
        var user = new User { Id = "user1", Name = "Test User" };
        _context.Users.Add(user);

        var product = new Product { Id = 1, Name = "Apple", Calories = 52 };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var mealModel = new MealModel
        {
            Name = "Fruit Snack",
            CategoryId = 1,
            Products = new List<ProductQuantityModel>
            {
                new ProductQuantityModel { ProductId = 1, Quantity = 100 }
            }
        };

        await _service.SaveMealAsync(user.Id, mealModel);

        var meal = await _context.Meals.FirstOrDefaultAsync(m => m.UserId == user.Id && m.Name == "Fruit Snack");
        Assert.NotNull(meal);
        Assert.Equal(52, meal.Calories);
    }

    [Fact]
    public async Task DeleteMealAsync_DeletesMeal_WhenMealExists()
    {
        ClearDatabase();
        var user = new User { Id = "user1", Name = "Test User" };
        _context.Users.Add(user);

        var meal = new Meal { Name = "Dinner", UserId = user.Id, Time = DateTime.UtcNow, Calories = 600 };
        _context.Meals.Add(meal);
        await _context.SaveChangesAsync();

        var existingMeal = await _context.Meals.FirstOrDefaultAsync(m => m.Id == meal.Id);
        Assert.NotNull(existingMeal);

        await _service.DeleteMealAsync(meal.Id);

        var deletedMeal = await _context.Meals.FindAsync(meal.Id);
        Assert.Null(deletedMeal);
    }

    [Fact]
    public async Task UpdateMealAsync_ThrowsException_WhenMealNotFound()
    {
        ClearDatabase();
        var mealModel = new MealModel
        {
            Name = "Lunch",
            CategoryId = 1,
            Products = new List<ProductQuantityModel>
            {
                new ProductQuantityModel { ProductId = 1, Quantity = 100 }
            }
        };

        var exception = await Assert.ThrowsAsync<Exception>(async () =>
        {
            await _service.UpdateMealAsync(999, mealModel);
        });

        Assert.Equal("Meal not found.", exception.Message);
    }

    [Fact]
    public async Task UpdateMealAsync_UpdatesMeal_WhenValidDataProvided()
    {
        ClearDatabase();
        var user = new User { Id = "user1", Name = "Test User" };
        _context.Users.Add(user);

        var meal = new Meal { Name = "Dinner", UserId = user.Id, Time = DateTime.UtcNow, Calories = 600 };
        _context.Meals.Add(meal);
        await _context.SaveChangesAsync();

        var updatedMealModel = new MealModel
        {
            Name = "Updated Dinner",
            CategoryId = 1,
            Products = new List<ProductQuantityModel>
            {
                new ProductQuantityModel { ProductId = 1, Quantity = 150 }
            }
        };

        await _service.UpdateMealAsync(meal.Id, updatedMealModel);

        var updatedMeal = await _context.Meals.FirstOrDefaultAsync(m => m.Id == meal.Id);
        Assert.NotNull(updatedMeal);
        Assert.Equal("Updated Dinner", updatedMeal.Name);
    }
}
