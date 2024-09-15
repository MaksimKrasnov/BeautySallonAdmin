
using BeautySaloon.Models;
using BeautySaloon.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeautySallonAdmin.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceRepository _serviceRepository;

        public ServicesController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            var services = await _serviceRepository.GetAllServicesAsync();
            return View(services);
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceRepository.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            ViewData["ServicesName"] = new SelectList(_serviceRepository.GetAllServicesAsync().Result, "Id", "Name");
            return View();
        }

        // POST: Services/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,ServicesId")] Service service)
        {
            var masterIds = await _serviceRepository.GetMasterIdsByServiceCategoryAsync(service.ServicesId);
            await _serviceRepository.CreateServiceAsync(service, masterIds);

            ViewData["ServicesName"] = new SelectList(await _serviceRepository.GetAllServicesAsync(), "Id", "Name", service.ServicesId);
            return RedirectToAction("Index", "Services");
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceRepository.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["ServicesName"] = new SelectList(await _serviceRepository.GetAllServicesAsync(), "Id", "Name", service.ServicesId);

            return View(service);
        }

        // POST: Services/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,ServicesId")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            try
            {
                await _serviceRepository.UpdateServiceAsync(service);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _serviceRepository.ServiceExistsAsync(service.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            ViewData["ServicesId"] = new SelectList(await _serviceRepository.GetAllServicesAsync(), "Id", "Name", service.ServicesId);
            return RedirectToAction("Index", "Services");
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceRepository.GetServiceByIdAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceRepository.DeleteServiceAsync(id);
            return RedirectToAction("Index", "Services");
        }
    }
}
