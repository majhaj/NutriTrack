using Microsoft.AspNetCore.Mvc;
using NutriTrack.Data;
using Microsoft.EntityFrameworkCore;
using NutriTrackData.Entities;
using NutriTrackData.Models;
using System.Security.Claims;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace NutriTrack.Controllers
{
    [Authorize]
    public class MealController : Controller
    {
        private readonly IMealService _mealService;
        private readonly ApplicationDbContext _context;

        public MealController(IMealService mealService, ApplicationDbContext context)
        {
            _mealService = mealService;
            _context = context;
        }

        public async Task<IActionResult> Index(string? name, int? FilterCategory, int? calories)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = _context.Meals
                                .Include(m => m.Category)
                                .Where(m => m.UserId == userId) // Filtrowanie według użytkownika
                                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(m => EF.Functions.Like(m.Name, $"%{name}%"));
            }

            if (FilterCategory.HasValue)
            {
                query = query.Where(m => m.CategoryId == FilterCategory.Value);
            }

            if (calories.HasValue)
            {
                query = query.Where(m => m.Calories <= calories.Value);
            }

            var meals = await query.ToListAsync();

            ViewData["FilterName"] = name;
            ViewData["FilterCategory"] = FilterCategory;
            ViewData["FilterCalories"] = calories;

            ViewBag.Categories = await _context.MealCategories.ToListAsync();

            return View(meals);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.MealCategories.ToListAsync();
            ViewBag.AvailableProducts = await _context.Products.ToListAsync();

            return View(new MealModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MealModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _mealService.SaveMealAsync(userId, model);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _context.MealCategories.ToListAsync();
            ViewBag.AvailableProducts = await _context.Products.ToListAsync();

            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"Error in '{key}': {error.ErrorMessage}");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _mealService.DeleteMealAsync(id);
                TempData["SuccessMessage"] = "Meal deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting meal: {ex.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var meal = await _mealService.GetMealByIdAsync(id);

            if (meal == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.MealCategories.ToListAsync();
            ViewBag.AvailableProducts = await _context.Products.ToListAsync();

            var model = new MealModel
            {
                Name = meal.Name,
                CategoryId = meal.CategoryId,
                Products = meal.MealProducts.Select(mp => new ProductQuantityModel
                {
                    ProductId = mp.ProductId,
                    Quantity = mp.Quantity
                }).ToList()
            };

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            ViewBag.AvailableProductsJson = JsonConvert.SerializeObject(ViewBag.AvailableProducts, settings);

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MealModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _mealService.UpdateMealAsync(id, model);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _context.MealCategories.ToListAsync();
            ViewBag.AvailableProducts = await _context.Products.ToListAsync();

            return View(model);
        }
    }
}
