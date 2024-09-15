using BeautySaloon.Controllers;
using BeautySaloon.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySaloon.Repositories
{
    public class WorkScheduleRepository : IWorkScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Master>> GetAllMastersAsync()
        {
            return await _context.Master.ToListAsync();
        }

        public async Task<List<WorkSchedule>> GetWorkSchedulesAsync(int masterId, DateTime startDate, DateTime endDate)
        {
            return await _context.WorkSchedules
                .Where(ws => ws.MasterId == masterId && ws.Date >= startDate && ws.Date <= endDate)
                .ToListAsync();
        }

        public async Task AddOrUpdateWorkScheduleAsync(int masterId, List<DateStatus> dates)
        {
            foreach (var dateStatus in dates)
            {
                DateTime date = DateTime.ParseExact(dateStatus.Date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                var existingWorkSchedule = await _context.WorkSchedules
                    .FirstOrDefaultAsync(ws => ws.MasterId == masterId && ws.Date == date);

                if (existingWorkSchedule != null)
                {
                    // Обновляем запись
                    existingWorkSchedule.IsWorkingDay = dateStatus.IsWorkingDay;
                    _context.WorkSchedules.Update(existingWorkSchedule);
                }
                else
                {
                    // Добавляем новую запись
                    var workSchedule = new WorkSchedule
                    {
                        MasterId = masterId,
                        Date = date,
                        IsWorkingDay = dateStatus.IsWorkingDay
                    };
                    _context.WorkSchedules.Add(workSchedule);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
