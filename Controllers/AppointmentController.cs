//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using BeautySaloon.Models;
//using BeautySaloon.Email;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace BeautySaloon.Controllers
//{
//	public class AppointmentController : Controller
//	{
//		private readonly ApplicationDbContext db;

//		public AppointmentController(ApplicationDbContext context)
//		{
//			db = context;
//		}

//		// GET: Appointment
//		public async Task<IActionResult> Index(int id)
//		{
//			var masters = await db.MasterService
//				   .Include(ms => ms.Master)
//				   .Where(ms => ms.ServiceId == id)
//				   .Select(ms => ms.Master)
//				   .ToListAsync();

//			// Передаем список мастеров в представление
//			ViewBag.Masters = new SelectList(masters, "Id", "Name");
//			var service = await db.Service.FirstOrDefaultAsync(s => s.Id == id);

//			return View(service);

//		}

//		public async Task<IActionResult> GetTimeSlot(DateTime selectedDate, int masterId)
//		{

//			//  Получаем все записи на выбранный день
//			var appointmentsOnSelectedDate = await db.Appointment
//		  .Where(a => a.DateTime.Date == selectedDate.Date && a.MasterService.MasterId == masterId)
//		  .ToListAsync();

//			// Генерируем доступные временные интервалы с 08:00 до 20:00 для выбранного мастера
//			var availableTimeSlots = new List<DateTime>();
//			var currentTime = selectedDate.Date.AddHours(8); // Начальное время записи

//			while (currentTime.Hour < 20) // Пока не достигнем 20:00
//			{
//				// Проверяем, занято ли текущее время для данного мастера
//				if (!appointmentsOnSelectedDate.Any(a => a.DateTime.Hour == currentTime.Hour))
//				{
//					availableTimeSlots.Add(currentTime);
//				}

//				// Переходим к следующему часу
//				currentTime = currentTime.AddHours(1);
//			}

//			return PartialView("TimeSlotPartView", availableTimeSlots);
//		}

//		[HttpPost]
//		public async Task<IActionResult> Create(int serviceId, int masterId, string nameInput, string phoneInput, DateTime selectedDateTime)
//		{
//			if (selectedDateTime == DateTime.MinValue)
//			{
//				return BadRequest("Выберите корректную дату и время.");
//			}
//			if (nameInput != null && phoneInput != null)
//			{
//				try
//				{
//					var masterServiceId = await db.MasterService
//			.Where(ms => ms.ServiceId == serviceId && ms.MasterId == masterId)
//			.Select(ms => ms.Id)
//			.FirstOrDefaultAsync();
//					Appointment app = new Appointment { DateTime = selectedDateTime, MasterServiceId = masterServiceId, ClientName = nameInput, Phone = phoneInput };
//					db.Appointment.Add(app);
//					db.SaveChanges();
//				}
//				catch (Exception ex)
//				{
//					return BadRequest(ex.Message);
//				}
//				EmailService email = new EmailService();
//				email.SendMail("krasnovm020@gmail.com", "Запись ", $"Имя клиента: {nameInput} телефон: {phoneInput}  дата и время:{selectedDateTime}");

//				return Ok();
//			}
//			return BadRequest("Не заполнены поля");

//		}

//	}
//}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BeautySaloon.Models;
using System.Security.Claims;
using BeautySaloon.Repositoryes.AppointmentRep;
using System.Globalization;

namespace BeautySaloon.Controllers
{
    public class AppointmentController : Controller
	{
		private readonly ApplicationDbContext _db;


		private readonly IAppointmentRepository _appointmentRepository;

		public AppointmentController(IAppointmentRepository repository, ApplicationDbContext db)
		{
			_appointmentRepository = repository;
			_db = db;

		}

		// GET: Appointment
		public async Task<IActionResult> Index(int id)
		{
			var masters = await _appointmentRepository.GetMastersByServiceId(id);
			ViewBag.Masters = new SelectList(masters, "Id", "Name");
			var service = await _appointmentRepository.GetServiceById(id);

			var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
			var name = User.FindFirstValue(ClaimTypes.GivenName);
			var phoneNumber = User.FindFirstValue(ClaimTypes.MobilePhone);

			// Используйте значения по вашему усмотрению
			// Например, передайте их в представление
			ViewBag.UserId = userId;
			ViewBag.Name = name;
			ViewBag.PhoneNumber = phoneNumber;
			return View(service);
		}

		public async Task<IActionResult> GetTimeSlot(DateTime selectedDate, int masterId)
		{
			var availableTimeSlots = await _appointmentRepository.GetAvailableTimeSlots(selectedDate, masterId);
			return PartialView("TimeSlotPartView", availableTimeSlots);
		}

		[HttpPost]
		public async Task<IActionResult> Create(int serviceId, int masterId, string nameInput, string phoneInput, DateTime selectedDateTime)
		{
			var normalDate = ConvertTo24HourFormat(selectedDateTime.ToString());

			if (normalDate == DateTime.MinValue)
			{
				return BadRequest("Выберите корректную дату и время.");
			}
			int userId = 1;
			if(User?.Identity?.IsAuthenticated == true)
			{
				userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
			}
			if ((nameInput != null && phoneInput != null) )
			{
				try
				{
					await _appointmentRepository.CreateAppointment(serviceId, masterId, nameInput, phoneInput, userId, normalDate);
					return Ok();
				}
				catch (Exception ex)
				{
					return BadRequest(ex.Message);
				}
			}
			return BadRequest("Не заполнены поля");
		}
		public static DateTime ConvertTo24HourFormat(string dateInput)
		{
			DateTime dateTime;
			if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd hh:mm:ss tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
			{
				return dateTime;
			}
			else
			{
				return Convert.ToDateTime(dateInput);
			}
		}
	}
}
