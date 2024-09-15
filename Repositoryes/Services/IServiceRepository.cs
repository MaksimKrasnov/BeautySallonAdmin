using BeautySaloon.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeautySaloon.Repositories
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetAllServicesAsync();
        Task<Service> GetServiceByIdAsync(int? id);
        Task<List<int>> GetMasterIdsByServiceCategoryAsync(int servicesId);
        Task CreateServiceAsync(Service service, List<int> masterIds);
        Task UpdateServiceAsync(Service service);
        Task DeleteServiceAsync(int id);
        Task<bool> ServiceExistsAsync(int id);
    }
}
