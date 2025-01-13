using NutriTrackData.Entities;
using NutriTrack.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using NutriTrackApp.Interfaces;

namespace NutriTrack.Services
{
    public class PhysicalActivityService : IPhysicalActivityService
    {
        private readonly ApplicationDbContext _context;

        public PhysicalActivityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SavePhysicalActivityAsync(User currentUser, PhysicalActivity activity)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == currentUser.Id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with id {currentUser.Id} not found");
            }

            activity.UserId = existingUser.Id;
            activity.User = existingUser;
            activity.Time = DateTime.UtcNow;

            Console.WriteLine($"Saving activity for user {existingUser.UserName}: {activity.Name}, {activity.CaloriesBurnedPerMinute} calories/min, {activity.Duration} minutes");

            _context.PhysicalActivities.Add(activity);
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<List<PhysicalActivity>> GetPhysicalActivityHistoryAsync(string userId)
        {
            var activities = await _context.PhysicalActivities
                .Where(pa => pa.UserId == userId)
                .OrderByDescending(pa => pa.Time)
                .ToListAsync();

            return activities;
        }

    }
}
