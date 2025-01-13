using NutriTrackData.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace NutriTrackApp.Interfaces
{
    public interface IPhysicalActivityService
    {
        Task<bool> SavePhysicalActivityAsync(User currentUser, PhysicalActivity activity);
        Task<List<PhysicalActivity>> GetPhysicalActivityHistoryAsync(string userId);
    }
}
