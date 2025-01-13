using NutriTrackData.Entities;
using NutriTrack.Data;
using NutriTrack.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using NutriTrackData.Models;

public class PhysicalActivityServiceTests
{
    private readonly PhysicalActivityService _service;
    private readonly ApplicationDbContext _context;

    public PhysicalActivityServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb_PhysicalActivities")
            .Options;

        _context = new ApplicationDbContext(options);
        _service = new PhysicalActivityService(_context);
    }

    private void ClearDatabase()
    {
        _context.Users.RemoveRange(_context.Users);
        _context.PhysicalActivities.RemoveRange(_context.PhysicalActivities);
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetPhysicalActivityHistoryAsync_ReturnsEmpty_WhenUserHasNoActivities()
    {
        ClearDatabase();
        var user = new User { Id = "user1", Name = "Test User" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _service.GetPhysicalActivityHistoryAsync(user.Id);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPhysicalActivityHistoryAsync_ReturnsEmpty_WhenUserDoesNotExist()
    {
        ClearDatabase();
        var nonExistentUserId = "nonexistent";

        var result = await _service.GetPhysicalActivityHistoryAsync(nonExistentUserId);

        Assert.Empty(result);
    }

    [Fact]
    public async Task SavePhysicalActivityAsync_ThrowsException_WhenUserDoesNotExist()
    {
        ClearDatabase();
        var nonExistentUserName = "nonexistentUser";
        var model = new PhysicalActivityModel
        {
            Name = "Running",
            Duration = 30,
            CaloriesBurnedPerMinute = 10
        };

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        {
            await _service.SavePhysicalActivityAsync(nonExistentUserName, model);
        });

        Assert.Equal($"User with username {nonExistentUserName} not found", exception.Message);
    }

    [Fact]
    public async Task GetPhysicalActivityHistoryAsync_ReturnsActivities_WhenUserHasActivities()
    {
        ClearDatabase();
        var user = new User { UserName = "user1", Name = "Test User" };
        _context.Users.Add(user);

        var activities = new List<PhysicalActivity>
    {
        new PhysicalActivity { Name = "Running", Duration = 30, UserName = "user1", Time = DateTime.UtcNow.AddMinutes(-10) },
        new PhysicalActivity { Name = "Swimming", Duration = 45, UserName = "user1", Time = DateTime.UtcNow }
    };

        _context.PhysicalActivities.AddRange(activities);
        await _context.SaveChangesAsync();

        var result = await _service.GetPhysicalActivityHistoryAsync(user.UserName);

        Assert.Equal(2, result.Count);
        Assert.Equal("Swimming", result.First().Name);
        Assert.Equal(user.UserName, result.First().UserName);
    }

    [Fact]
    public async Task DeletePhysicalActivityAsync_DeletesActivity_WhenActivityExists()
    {
        ClearDatabase();
        var user = new User { UserName = "user1", Name = "Test User" };
        _context.Users.Add(user);

        var activity = new PhysicalActivity
        {
            Name = "Running",
            Duration = 30,
            UserName = user.UserName,
            User = user,
            Time = DateTime.UtcNow.AddMinutes(-10)
        };

        _context.PhysicalActivities.Add(activity);
        await _context.SaveChangesAsync();

        var existingActivity = await _context.PhysicalActivities.FirstOrDefaultAsync(a => a.Id == activity.Id);
        Assert.NotNull(existingActivity);

        var result = await _service.DeletePhysicalActivityAsync(activity.Id);

        Assert.True(result);
        var deletedActivity = await _context.PhysicalActivities.FindAsync(activity.Id);
        Assert.Null(deletedActivity);

        var activityAfterDeletion = await _context.PhysicalActivities.FindAsync(activity.Id);
        Assert.Null(activityAfterDeletion);
    }
}
