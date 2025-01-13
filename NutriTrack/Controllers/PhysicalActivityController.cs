using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NutriTrack.Services;
using NutriTrackApp.Interfaces;
using NutriTrackData.Entities;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize]
public class PhysicalActivityController : Controller
{
    private readonly IPhysicalActivityService _activityService;
    private readonly UserManager<User> _userManager;

    public PhysicalActivityController(IPhysicalActivityService activityService, UserManager<User> userManager)
    {
        _activityService = activityService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized("User is not logged in.");
        }

        var activities = await _activityService.GetPhysicalActivityHistoryAsync(user.Id);
        return View(activities);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create([Bind("Name,CaloriesBurnedPerMinute,Duration")] PhysicalActivity activityModel)
    {
        if (ModelState.IsValid)
        {
            // Sprawdzenie, czy użytkownik jest zalogowany
            if (!User.Identity.IsAuthenticated)
            {
                // Zwrócenie błędu, jeśli użytkownik nie jest zalogowany
                return Unauthorized("User is not logged in.");
            }

            // Pobranie userId z Claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Sprawdzenie, czy userId jest poprawnie pobrane
            if (string.IsNullOrEmpty(userId))
            {
                // Logowanie błędu, jeśli userId jest puste
                Console.WriteLine("UserId is null or empty.");
                return Unauthorized("Unable to retrieve user ID.");
            }

            // Logowanie userId dla debugowania
            Console.WriteLine($"User ID: {userId}");

            // Sprawdzenie, czy użytkownik istnieje w bazie danych
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Jeśli użytkownik nie istnieje, przekierowanie na stronę logowania
                return RedirectToAction("Login", "Account");
            }

            // Zapis aktywności fizycznej
            var result = await _activityService.SavePhysicalActivityAsync(user, activityModel);

            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Error saving data.");
            }
        }

        return View(activityModel);
    }
}
