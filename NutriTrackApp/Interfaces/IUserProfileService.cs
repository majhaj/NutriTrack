using NutriTrackData.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace NutriTrackApp.Interfaces
{
    public interface IUserProfileService
    {
        string GetBmiInterpretation(double bmi);

        Task<bool> SaveUserProfileAsync(User currentUser, User updatedUserProfile);

        Task<IQueryable<WeightHistory>> GetWeightHistoryAsync(string userId);
    }
}
