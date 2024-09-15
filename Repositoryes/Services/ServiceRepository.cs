using BeautySaloon.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySaloon.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Service>> GetAllServicesAsync()
        {
            return await _context.Service.Include(s => s.Services).ToListAsync();
        }

        public async Task<Service> GetServiceByIdAsync(int? id)
        {
            return await _context.Service
                .Include(s => s.Services)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<int>> GetMasterIdsByServiceCategoryAsync(int servicesId)
        {
            return await _context.MasterService
                .Include(ms => ms.Master)
                .Include(ms => ms.Service)
                .ThenInclude(s => s.Services)
                .Where(ms => ms.Service.Services.Id == servicesId)
                .Select(ms => ms.MasterId)
                .Distinct()
                .ToListAsync();
        }

        public async Task CreateServiceAsync(Service service, List<int> masterIds)
        {
            _context.Service.Add(service);
            await _context.SaveChangesAsync();

            foreach (var masterId in masterIds)
            {
                var masterService = new MasterService
                {
                    MasterId = masterId,
                    ServiceId = service.Id
                };
                _context.MasterService.Add(masterService);
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateServiceAsync(Service service)
        {
            _context.Service.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteServiceAsync(int id)
        {
            var service = await _context.Service.FindAsync(id);
            if (service != null)
            {
                _context.Service.Remove(service);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ServiceExistsAsync(int id)
        {
            return await _context.Service.AnyAsync(e => e.Id == id);
        }
    }
}
