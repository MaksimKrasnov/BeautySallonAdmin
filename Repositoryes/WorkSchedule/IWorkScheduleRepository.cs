using BeautySaloon.Controllers;
using BeautySaloon.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeautySaloon.Repositories
{
    public interface IWorkScheduleRepository
    {
        Task<List<Master>> GetAllMastersAsync();
        Task<List<WorkSchedule>> GetWorkSchedulesAsync(int masterId, DateTime startDate, DateTime endDate);
        Task AddOrUpdateWorkScheduleAsync(int masterId, List<DateStatus> dates);
    }
}
