
using BeautySaloon.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeautySaloon.Repositories
{
    public interface IRecordRepository
    {
        Task<List<Master>> GetAllMastersAsync();
        Task<List<Master>> GetMastersByServiceAsync(int serviceId);
        Task<List<Service>> GetAllServicesAsync();
        Task<List<Appointment>> GetAppointmentsByMasterAndDateAsync(int masterId, DateTime selectedDate);
        Task<bool> AppointmentExistsAsync(DateTime selectedDate, int masterId);
        Task DeleteAppointmentAsync(DateTime selectedDate, int masterId);
    }
}
