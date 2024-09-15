
using BeautySaloon.Models;
using BeautySaloon.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySaloon.Controllers
{
    public class WorkScheduleController : Controller
    {
        private readonly IWorkScheduleRepository _workScheduleRepository;

        public WorkScheduleController(IWorkScheduleRepository workScheduleRepository)
        {
            _workScheduleRepository = workScheduleRepository;
        }

        public async Task<IActionResult> Index()
        {
            var masters = await _workScheduleRepository.GetAllMastersAsync();
            var model = new WorkScheduleViewModel
            {
                Masters = masters
            };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetDates(int masterId, DateTime startDate, DateTime endDate)
        {
            var dates = GetDatesInRange(startDate, endDate);
            var workSchedules = await _workScheduleRepository.GetWorkSchedulesAsync(masterId, startDate, endDate);

            var model = new WorkScheduleViewModel
            {
                MasterId = masterId,
                Dates = dates.Select(date => new DateStatus
                {
                    Date = date,
                    IsWorkingDay = workSchedules.Any(ws => ws.Date == DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture) && ws.IsWorkingDay)
                }).ToList()
            };

            return PartialView("DatesTablePartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkSchedule(int masterId, List<DateStatus> dates)
        {
            await _workScheduleRepository.AddOrUpdateWorkScheduleAsync(masterId, dates);
            return RedirectToAction(nameof(Index));
        }

        private List<string> GetDatesInRange(DateTime startDate, DateTime endDate)
        {
            var dates = new List<string>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dates.Add(date.ToString("yyyy-MM-dd"));
            }
            return dates;
        }

    }
    public class DateStatus
    {
        public string Date { get; set; }
        public bool IsWorkingDay { get; set; }
    }

    public class WorkScheduleViewModel
    {
        public int MasterId { get; set; }
        public List<Master> Masters { get; set; }
        public List<DateStatus> Dates { get; set; }
    }
}

