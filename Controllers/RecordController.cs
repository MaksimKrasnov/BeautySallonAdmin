
using BeautySaloon.Models;
using BeautySaloon.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySaloon.Controllers
{
    public class RecordController : Controller
    {
        private readonly IRecordRepository _recordRepository;

        public RecordController(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        public async Task<ActionResult> Index(DateTime selectedDate, int serviceId)
        {
            List<Master> masters = new List<Master>();

            if (serviceId == 0)
            {
                masters = await _recordRepository.GetAllMastersAsync();
            }
            else
            {
                masters = await _recordRepository.GetMastersByServiceAsync(serviceId);
            }

            List<Service> services = await _recordRepository.GetAllServicesAsync();

            if (selectedDate == DateTime.ParseExact("01.01.0001 0:00:00", "dd.MM.yyyy H:mm:ss", CultureInfo.InvariantCulture))
            {
                selectedDate = DateTime.Now;
            }

            // Генерируем список временных интервалов с 8:00 до 20:00
            Dictionary<Master, Dictionary<DateTime, string>> dict = new Dictionary<Master, Dictionary<DateTime, string>>();
            foreach (var master in masters)
            {
                var appointmentsOnSelectedDate = await _recordRepository.GetAppointmentsByMasterAndDateAsync(master.Id, selectedDate);
                var currentTime = selectedDate.Date.AddHours(8); // Начальное время записи

                Dictionary<DateTime, string> d = new Dictionary<DateTime, string>();
                while (currentTime.Hour < 20) // Пока не достигнем 20:00
                {
                    // Проверяем, занято ли текущее время для данного мастера
                    if (appointmentsOnSelectedDate.Any(a => a.DateTime.Hour == currentTime.Hour))
                    {
                        d.Add(currentTime, "Занято");
                    }
                    else
                    {
                        d.Add(currentTime, "Свободно");
                    }

                    // Переходим к следующему часу
                    currentTime = currentTime.AddHours(1);
                }
                dict.Add(master, d);
            }

            // Создаем модель представления
            var viewModel = new AdminViewModel
            {
                Services = services,
                Masters = masters,
                Appointments = dict,
                ServiceId = serviceId,
                Date = selectedDate
            };

            return View(viewModel);
        }

        public async Task<ActionResult> DeleteAppointment(DateTime selectedDate, int masterId)
        {
            if (await _recordRepository.AppointmentExistsAsync(selectedDate, masterId))
            {
                await _recordRepository.DeleteAppointmentAsync(selectedDate, masterId);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
