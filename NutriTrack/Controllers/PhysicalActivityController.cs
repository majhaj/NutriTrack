using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NutriTrackApp.Interfaces;
using NutriTrackData.Entities;
using NutriTrackData.Models;
using System.Security.Claims;

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

    public async Task<IActionResult> Index(string activityName, int? minDuration, int? maxDuration)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized("User is not logged in.");
        }

        var activities = await _activityService.GetPhysicalActivityHistoryAsync(user.Id);

        if (!string.IsNullOrEmpty(activityName))
        {
            activities = activities.Where(a => a.Name.Contains(activityName, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        if (minDuration.HasValue)
        {
            activities = activities.Where(a => a.Duration >= minDuration.Value).ToList();
        }
        if (maxDuration.HasValue)
        {
            activities = activities.Where(a => a.Duration <= maxDuration.Value).ToList();
        }

        return View(activities);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PhysicalActivityModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userName = User.Identity.Name;

        if (string.IsNullOrEmpty(userName))
        {
            return Unauthorized("Unable to retrieve user name.");
        }

        var result = await _activityService.SavePhysicalActivityAsync(userName, model);

        if (result)
        {
            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("", "Error saving data.");
        }

        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var activity = await _activityService.GetPhysicalActivityByIdAsync(id);
        if (activity == null)
        {
            return NotFound();
        }

        var model = new PhysicalActivityModel
        {
            Name = activity.Name,
            CaloriesBurnedPerMinute = activity.CaloriesBurnedPerMinute,
            Duration = activity.Duration
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, PhysicalActivityModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _activityService.UpdatePhysicalActivityAsync(id, model);

        if (result)
        {
            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("", "Error updating data.");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _activityService.DeletePhysicalActivityAsync(id);
        if (!result)
        {
            ModelState.AddModelError("", "Error deleting activity.");
        }

        return RedirectToAction("Index");
    }

}
