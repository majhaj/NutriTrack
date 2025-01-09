using NutriTrackData.Entities;
using NutriTrack.Data;
using Microsoft.EntityFrameworkCore;
using NutriTrackApp.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NutriTrack.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDbContext _context;

        public UserProfileService(ApplicationDbContext context)
        {
            _context = context;
        }

        public string GetBmiInterpretation(double bmi)
        {
            if (bmi < 18.5)
                return "Underweight";
            else if (bmi >= 18.5 && bmi <= 24.9)
                return "Normal weight";
            else if (bmi >= 25 && bmi <= 29.9)
                return "Overweight";
            else
                return "Obesity";
        }

        public async Task<bool> SaveUserProfileAsync(User currentUser, User updatedUserProfile)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == currentUser.Id);

            if (existingUser == null)
            {
                return false;
            }

            existingUser.Name = updatedUserProfile.Name;
            existingUser.DateOfBirth = updatedUserProfile.DateOfBirth;
            existingUser.Weight = updatedUserProfile.Weight;
            existingUser.Height = updatedUserProfile.Height;

            _context.Users.Update(existingUser);

            var weightHistory = new WeightHistory
            {
                UserId = existingUser.Id,
                Weight = existingUser.Weight,
                Date = DateTime.Now
            };
            _context.WeightHistories.Add(weightHistory);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IQueryable<WeightHistory>> GetWeightHistoryAsync(string userId)
        {
            var weightHistory = _context.WeightHistories
                .Where(wh => wh.UserId == userId)
                .OrderByDescending(wh => wh.Date);

            return await Task.FromResult(weightHistory);
        }
    }
}
