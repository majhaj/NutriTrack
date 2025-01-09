using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NutriTrack.Services;
using NutriTrackApp.Interfaces;
using NutriTrackData.Entities;
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
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _activityService.SavePhysicalActivityAsync(user, activityModel);

            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Saving data error.");
            }
        }

        return View(activityModel);

    }
}
