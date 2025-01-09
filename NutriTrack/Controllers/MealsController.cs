using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NutriTrack.Data;
using NutriTrackData.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NutriTrack.Controllers
{
    [Authorize]
    public class MealsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MealsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var meals = await _context.Meals
                .Include(m => m.Category)
                .Include(m => m.MealProducts)
                .ThenInclude(mp => mp.Product)
                .Where(m => m.UserId == userId)
                .ToListAsync();

            return View(meals);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.MealCategories.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Meal meal, List<int> productIds, List<double> quantities)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.MealCategories.ToListAsync();
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(meal);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            meal.UserId = userId;
            meal.Time = DateTime.UtcNow;

            _context.Meals.Add(meal);
            await _context.SaveChangesAsync();

            for (int i = 0; i < productIds.Count; i++)
            {
                var mealProduct = new MealProduct
                {
                    MealId = meal.Id,
                    ProductId = productIds[i],
                    Quantity = quantities[i]
                };
                _context.MealProducts.Add(mealProduct);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var meal = await _context.Meals
                .Include(m => m.MealProducts)
                .ThenInclude(mp => mp.Product)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (meal == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.MealCategories.ToListAsync();
            ViewBag.Products = await _context.Products.ToListAsync();

            return View(meal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Meal meal, List<int> productIds, List<double> quantities)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != meal.Id || meal.UserId != userId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.MealCategories.ToListAsync();
                ViewBag.Products = await _context.Products.ToListAsync();
                return View(meal);
            }

            var existingMeal = await _context.Meals
                .Include(m => m.MealProducts)
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);

            if (existingMeal == null)
            {
                return NotFound();
            }

            existingMeal.Name = meal.Name;
            existingMeal.Calories = meal.Calories;
            existingMeal.CategoryId = meal.CategoryId;

            _context.MealProducts.RemoveRange(existingMeal.MealProducts);

            for (int i = 0; i < productIds.Count; i++)
            {
                var mealProduct = new MealProduct
                {
                    MealId = meal.Id,
                    ProductId = productIds[i],
                    Quantity = quantities[i]
                };
                _context.MealProducts.Add(mealProduct);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        }
    }
