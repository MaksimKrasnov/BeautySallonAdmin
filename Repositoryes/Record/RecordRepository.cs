using BeautySaloon.Models;
using BeautySallonAdmin.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySaloon.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public RecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Master>> GetAllMastersAsync()
        {
            return await _context.Master.ToListAsync();
        }

        public async Task<List<Master>> GetMastersByServiceAsync(int serviceId)
        {
            return await _context.MasterService
                .Where(ms => ms.ServiceId == serviceId)
                .Select(ms => ms.Master)
                .ToListAsync();
        }

        public async Task<List<Service>> GetAllServicesAsync()
        {
            return await _context.Service.ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByMasterAndDateAsync(int masterId, DateTime selectedDate)
        {
            return await _context.Appointment
                .Where(a => a.DateTime.Date == selectedDate.Date && a.MasterService.MasterId == masterId)
                .ToListAsync();
        }

        public async Task<bool> AppointmentExistsAsync(DateTime selectedDate, int masterId)
        {
            return await _context.Appointment
                .AnyAsync(a => a.DateTime == selectedDate && a.MasterService.MasterId == masterId);
        }

        public async Task DeleteAppointmentAsync(DateTime selectedDate, int masterId)
        {
            var appointmentToDelete = await _context.Appointment
                .FirstOrDefaultAsync(a => a.DateTime == selectedDate && a.MasterService.MasterId == masterId);

            if (appointmentToDelete != null)
            {
                _context.Appointment.Remove(appointmentToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
