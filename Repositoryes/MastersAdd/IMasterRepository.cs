using BeautySaloon.Models;

namespace BeautySallonAdmin.Repositoryes.MastersAdd
{
    public interface IMasterRepository
    {
        Task<List<Master>> GetAllMastersAsync();
        Task<Master> GetMasterByIdAsync(int? id);
        Task<List<string>> GetUniquePositionsAsync();
        Task<List<Service>> GetServicesByMasterPositionAsync(string position);
        Task AddMasterAsync(Master master);
        Task UpdateMasterAsync(Master master);
        Task DeleteMasterAsync(int id);
        Task<bool> MasterExistsAsync(int id);
    }
}
