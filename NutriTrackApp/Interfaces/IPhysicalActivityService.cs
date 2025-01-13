using NutriTrackData.Entities;
using NutriTrackData.Models;
using System.Threading.Tasks;

namespace NutriTrackApp.Interfaces
{
    public interface IPhysicalActivityService
    {
        Task<bool> SavePhysicalActivityAsync(string userName, PhysicalActivityModel model);
        Task<List<PhysicalActivity>> GetPhysicalActivityHistoryAsync(string userId);
        Task<PhysicalActivity> GetPhysicalActivityByIdAsync(int id);
        Task<bool> UpdatePhysicalActivityAsync(int id, PhysicalActivityModel model);
        Task<bool> DeletePhysicalActivityAsync(int id);
    }
}
