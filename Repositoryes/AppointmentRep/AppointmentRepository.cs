//using BeautySaloon.Email;
using BeautySaloon.Models;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Repositoryes.AppointmentRep
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly ApplicationDbContext _db;

        public AppointmentRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Master>> GetMastersByServiceId(int serviceId)
        {
            return await _db.MasterService
                .Include(ms => ms.Master)
                .Where(ms => ms.ServiceId == serviceId)
                .Select(ms => ms.Master)
                .ToListAsync();
        }
        public async Task<Service> GetServiceById(int serviceId)
        {
            return await _db.Service.FirstOrDefaultAsync(s => s.Id == serviceId);
        }
        public async Task<IEnumerable<DateTime>> GetAvailableTimeSlots(DateTime selectedDate, int masterId)
        {
            //var appointmentsOnSelectedDate = await _db.Appointment
            //	.Where(a => a.DateTime.Date == selectedDate.Date && a.MasterService.MasterId == masterId)
            //	.ToListAsync();
            var appointmentsOnSelectedDate = await _db.Appointment
          .Include(a => a.MasterService)
          .Where(a => a.DateTime.Date == selectedDate.Date && a.MasterService != null && a.MasterService.MasterId == masterId)
          .ToListAsync();


            var availableTimeSlots = new List<DateTime>();
            var currentTime = selectedDate.Date.AddHours(8);

            while (currentTime.Hour < 20)
            {
                if (DateTime.Now.Date == currentTime.Date)
                {
                    if (!appointmentsOnSelectedDate.Any(a => a.DateTime.Hour == currentTime.Hour) && currentTime.Hour > DateTime.Now.Hour)
                    {
                        availableTimeSlots.Add(currentTime);
                    }
                }
                else
				{
					if (!appointmentsOnSelectedDate.Any(a => a.DateTime.Hour == currentTime.Hour))
					{
						availableTimeSlots.Add(currentTime);
					}
				}

                currentTime = currentTime.AddHours(1);
            }

            return availableTimeSlots;
        }

        //public async Task<bool> CreateAppointment(int serviceId, int masterId, string nameInput, string phoneInput, DateTime selectedDateTime)
        //{
        //	try
        //	{
        //		var masterServiceId = await _db.MasterService
        //			.Where(ms => ms.ServiceId == serviceId && ms.MasterId == masterId)
        //			.Select(ms => ms.Id)
        //			.FirstOrDefaultAsync();

        //		Appointment appointment = new Appointment { DateTime = selectedDateTime, MasterServiceId = masterServiceId, ClientName = nameInput, Phone = phoneInput };

        //		_db.Appointment.Add(appointment);
        //		await _db.SaveChangesAsync();

        //		EmailService email = new EmailService();
        //		await email.SendMail("krasnovm020@gmail.com", "Запись ", $"Имя клиента: {nameInput} телефон: {phoneInput}  дата и время:{selectedDateTime}");

        //		return true;
        //	}
        //	catch (Exception ex)
        //	{
        //		throw ex;

        //	}
        //}
        public async Task<bool> CreateAppointment(int serviceId, int masterId, string nameInput, string phoneInput, int userId, DateTime selectedDateTime)
        {
            try
            {
                var masterServiceId = await _db.MasterService
                    .Where(ms => ms.ServiceId == serviceId && ms.MasterId == masterId)
                    .Select(ms => ms.Id)
                    .FirstOrDefaultAsync();

                Appointment appointment = new Appointment { DateTime = selectedDateTime, MasterServiceId = masterServiceId, ClientName = nameInput, Phone = phoneInput, UserId = userId };

                _db.Appointment.Add(appointment);
                await _db.SaveChangesAsync();

              //  EmailService email = new EmailService();
               // await email.SendMail("krasnovm020@gmail.com", "Запись ", $"Имя клиента: {nameInput} телефон: {phoneInput}  дата и время:{selectedDateTime}");

                return true;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}

