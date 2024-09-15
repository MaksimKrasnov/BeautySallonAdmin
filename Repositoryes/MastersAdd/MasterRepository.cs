using BeautySaloon.Models;
using BeautySallonAdmin.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeautySallonAdmin.Repositoryes.MastersAdd;

namespace BeautySallonAdmin.Repositories
{
    public class MasterRepository : IMasterRepository
    {
        private readonly ApplicationDbContext _context;

        public MasterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Master>> GetAllMastersAsync()
        {
            return await _context.Master.ToListAsync();
        }

        public async Task<Master> GetMasterByIdAsync(int? id)
        {
            return await _context.Master.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<string>> GetUniquePositionsAsync()
        {
            var masters = await _context.Master.ToListAsync();
            return masters.Select(m => m.Position).Distinct().ToList();
        }

        public async Task<List<Service>> GetServicesByMasterPositionAsync(string position)
        {
            return await _context.Master
                .Where(m => m.Position == position)
                .Include(m => m.MasterServices)
                    .ThenInclude(ms => ms.Service)
                .SelectMany(m => m.MasterServices.Select(ms => ms.Service))
                .Distinct()
                .ToListAsync();
        }

        public async Task AddMasterAsync(Master master)
        {
            _context.Add(master);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMasterAsync(Master master)
        {
            _context.Update(master);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMasterAsync(int id)
        {
            var master = await _context.Master.FindAsync(id);
            if (master != null)
            {
                _context.Master.Remove(master);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> MasterExistsAsync(int id)
        {
            return await _context.Master.AnyAsync(e => e.Id == id);
        }
    }
}
