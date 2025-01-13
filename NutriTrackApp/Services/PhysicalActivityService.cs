using NutriTrackData.Entities;
using NutriTrack.Data;
using Microsoft.EntityFrameworkCore;
using NutriTrackApp.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using NutriTrackData.Models;

namespace NutriTrack.Services
{
    public class PhysicalActivityService : IPhysicalActivityService
    {
        private readonly ApplicationDbContext _context;

        public PhysicalActivityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SavePhysicalActivityAsync(string userName, PhysicalActivityModel model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with username {userName} not found");
            }

            var activity = new PhysicalActivity
            {
                Name = model.Name,
                CaloriesBurnedPerMinute = model.CaloriesBurnedPerMinute,
                Duration = model.Duration,
                UserName = userName,
                User = user,
                Time = DateTime.UtcNow
            };

            _context.PhysicalActivities.Add(activity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<PhysicalActivity>> GetPhysicalActivityHistoryAsync(string userName)
        {
            var activities = await _context.PhysicalActivities
                .Where(pa => pa.UserName == userName)
                .OrderByDescending(pa => pa.Time)
                .ToListAsync();

            return activities;
        }

        public async Task<PhysicalActivity> GetPhysicalActivityByIdAsync(int id)
        {
            return await _context.PhysicalActivities
                .FirstOrDefaultAsync(pa => pa.Id == id);
        }

        public async Task<bool> UpdatePhysicalActivityAsync(int id, PhysicalActivityModel model)
        {
            var activity = await _context.PhysicalActivities.FirstOrDefaultAsync(pa => pa.Id == id);
            if (activity == null)
            {
                return false;
            }

            activity.Name = model.Name;
            activity.CaloriesBurnedPerMinute = model.CaloriesBurnedPerMinute;
            activity.Duration = model.Duration;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePhysicalActivityAsync(int id)
        {
            var activity = await _context.PhysicalActivities.FirstOrDefaultAsync(pa => pa.Id == id);
            if (activity == null)
            {
                return false;
            }

            _context.PhysicalActivities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
