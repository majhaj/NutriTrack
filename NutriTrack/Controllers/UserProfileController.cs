using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NutriTrackData.Entities;
using NutriTrackApp.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace NutriTrack.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly UserManager<User> _userManager;

        public UserProfileController(IUserProfileService userProfileService,
                                        UserManager<User> userManager)
        {
            _userProfileService = userProfileService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }

        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User userModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var result = await _userProfileService.SaveUserProfileAsync(user, userModel);

                if (result)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Saving data error.");
                }
            }

            return View(userModel);
        }

        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var weightHistory = await _userProfileService.GetWeightHistoryAsync(user.Id);
            return View(weightHistory);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
