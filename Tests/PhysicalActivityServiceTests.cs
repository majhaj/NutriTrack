using NutriTrackData.Entities;
using NutriTrack.Data;
using NutriTrack.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

public class PhysicalActivityServiceTests
{
    private readonly PhysicalActivityService _service;
    private readonly ApplicationDbContext _context;

    public PhysicalActivityServiceTests()
    {
        // Użycie in-memory database dla testów
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb_PhysicalActivities")
            .Options;

        _context = new ApplicationDbContext(options);
        _service = new PhysicalActivityService(_context);
    }

    // Upewnij się, że baza danych jest czyszczona przed każdym testem
    private void ClearDatabase()
    {
        _context.Users.RemoveRange(_context.Users);
        _context.PhysicalActivities.RemoveRange(_context.PhysicalActivities);
        _context.SaveChanges();
    }

    /*[Fact]
    public async Task SavePhysicalActivityAsync_SavesActivity_WhenUserExists()
    {
        // Arrange
        ClearDatabase();  // Czyszczenie bazy przed testem
        var user = new User { Id = "user1", Name = "Test User" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var activity = new PhysicalActivity { Name = "Running", Duration = 30 };

        // Act
        var result = await _service.SavePhysicalActivityAsync(user, activity);

        // Assert
        Assert.True(result);
        Assert.Single(_context.PhysicalActivities);
        Assert.Equal("Running", _context.PhysicalActivities.First().Name);
    }*/

/*    [Fact]
    public async Task SavePhysicalActivityAsync_ThrowsException_WhenUserDoesNotExist()
    {
        // Arrange
        ClearDatabase();  // Czyszczenie bazy przed testem
        var nonExistentUser = new User { Id = "nonexistent", Name = "Nonexistent User" };
        var activity = new PhysicalActivity { Name = "Running", Duration = 30 };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        {
            await _service.SavePhysicalActivityAsync(nonExistentUser, activity);
        });
    }*/

    /*[Fact]
    public async Task GetPhysicalActivityHistoryAsync_ReturnsActivities_WhenUserHasActivities()
    {
        // Arrange
        ClearDatabase();  // Czyszczenie bazy przed testem
        var user = new User { Id = "user1", Name = "Test User" };
        _context.Users.Add(user);

        var activities = new List<PhysicalActivity>
        {
            new PhysicalActivity { Name = "Running", Duration = 30, UserId = "user1", Time = DateTime.UtcNow.AddMinutes(-10) },
            new PhysicalActivity { Name = "Swimming", Duration = 45, UserId = "user1", Time = DateTime.UtcNow }
        };

        _context.PhysicalActivities.AddRange(activities);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetPhysicalActivityHistoryAsync(user.Id);

        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal("Swimming", result.First().Name); // Oczekiwane posortowanie malejąco
    }*/

    [Fact]
    public async Task GetPhysicalActivityHistoryAsync_ReturnsEmpty_WhenUserHasNoActivities()
    {
        // Arrange
        ClearDatabase();  // Czyszczenie bazy przed testem
        var user = new User { Id = "user1", Name = "Test User" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetPhysicalActivityHistoryAsync(user.Id);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPhysicalActivityHistoryAsync_ReturnsEmpty_WhenUserDoesNotExist()
    {
        // Arrange
        ClearDatabase();  // Czyszczenie bazy przed testem
        var nonExistentUserId = "nonexistent";

        // Act
        var result = await _service.GetPhysicalActivityHistoryAsync(nonExistentUserId);

        // Assert
        Assert.Empty(result);
    }
}
