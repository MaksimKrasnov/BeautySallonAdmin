using BeautySaloon.Models;

namespace BeautySaloon.Repositoryes.AppointmentRep
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Master>> GetMastersByServiceId(int serviceId);
        Task<Service> GetServiceById(int serviceId);

        Task<IEnumerable<DateTime>> GetAvailableTimeSlots(DateTime selectedDate, int masterId);
        //Task<bool> CreateAppointment(int serviceId, int masterId, string nameInput, string phoneInput, DateTime selectedDateTime);
        Task<bool> CreateAppointment(int serviceId, int masterId, string nameInput, string phoneInput, int userId, DateTime selectedDateTime);
    }
}
