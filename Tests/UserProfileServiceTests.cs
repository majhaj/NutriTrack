using Microsoft.EntityFrameworkCore;
using NutriTrack.Data;
using NutriTrack.Services;
using NutriTrackData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class InMemoryDbContext : ApplicationDbContext
{
    public InMemoryDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}

public class UserProfileServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly UserProfileService _service;

    public UserProfileServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new InMemoryDbContext(options);

        _context.Users.Add(new User { Id = "user1", Name = "Test User", Weight = 70, Height = 175, DateOfBirth = new DateTime(1990, 1, 1) });
        _context.SaveChanges();

        _service = new UserProfileService(_context);
    }

    [Fact]
    public void GetBmiInterpretation_ReturnsUnderweight_WhenBmiIsUnder18_5()
    {
        double bmi = 17.0;

        var result = _service.GetBmiInterpretation(bmi);

        Assert.Equal("Underweight", result);
    }

    [Fact]
    public void GetBmiInterpretation_ReturnsNormalWeight_WhenBmiIsBetween18_5And24_9()
    {
        double bmi = 22.0;

        var result = _service.GetBmiInterpretation(bmi);

        Assert.Equal("Normal weight", result);
    }

    [Fact]
    public void GetBmiInterpretation_ReturnsOverweight_WhenBmiIsBetween25And29_9()
    {
        double bmi = 27.0;

        var result = _service.GetBmiInterpretation(bmi);

        Assert.Equal("Overweight", result);
    }

    [Fact]
    public void GetBmiInterpretation_ReturnsObesity_WhenBmiIsOver30()
    {
        double bmi = 32.0;

        var result = _service.GetBmiInterpretation(bmi);

        Assert.Equal("Obesity", result);
    }

    [Fact]
    public async Task SaveUserProfileAsync_AddsWeightHistoryEntry_WhenWeightIsUpdated()
    {
        var currentUser = new User { Id = "user1", Name = "Test User", Weight = 70 };
        var updatedUserProfile = new User { Id = "user1", Name = "Test User", Weight = 80 };

        var result = await _service.SaveUserProfileAsync(currentUser, updatedUserProfile);

        Assert.True(result);

        var weightHistories = _context.WeightHistories.Where(wh => wh.UserId == "user1").ToList();
        Assert.Single(weightHistories);
        Assert.Equal(80, weightHistories.First().Weight);
    }

    [Fact]
    public async Task SaveUserProfileAsync_ReturnsFalse_WhenUserDoesNotExist()
    {
        var currentUser = new User { Id = "nonexistent_user", Name = "Nonexistent User", Weight = 70 };
        var updatedUserProfile = new User { Id = "nonexistent_user", Name = "Nonexistent User", Weight = 80 };

        var result = await _service.SaveUserProfileAsync(currentUser, updatedUserProfile);

        Assert.False(result);
    }

    [Fact]
    public async Task GetWeightHistoryAsync_ReturnsWeightHistory_ForGivenUserId()
    {
        var userId = "user1";
        _context.WeightHistories.Add(new WeightHistory { UserId = userId, Weight = 70, Date = DateTime.Now });
        _context.SaveChanges();

        var result = await _service.GetWeightHistoryAsync(userId);

        Assert.NotNull(result);
        Assert.Single(result);
    }
}
