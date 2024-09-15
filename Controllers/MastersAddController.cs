
using BeautySaloon.Models;
using BeautySallonAdmin.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeautySallonAdmin.Repositoryes.MastersAdd;

namespace BeautySallonAdmin.Controllers
{
    public class MastersAddController : Controller
    {
        private readonly IMasterRepository _masterRepository;

        public MastersAddController(IMasterRepository masterRepository)
        {
            _masterRepository = masterRepository;
        }

        // GET: MastersAdd
        public async Task<IActionResult> Index()
        {
            return View(await _masterRepository.GetAllMastersAsync());
        }

        // GET: MastersAdd/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var master = await _masterRepository.GetMasterByIdAsync(id);
            if (master == null)
            {
                return NotFound();
            }

            return View(master);
        }

        // GET: MastersAdd/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.UniquePositions = await _masterRepository.GetUniquePositionsAsync();
            return View();
        }

        // POST: MastersAdd/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Position")] Master master)
        {
            master.MasterServices = new List<MasterService>();
            var services = await _masterRepository.GetServicesByMasterPositionAsync(master.Position);
            AssignServicesToMaster(master, services);

            await _masterRepository.AddMasterAsync(master);
            return RedirectToAction(nameof(Index));
        }

        public void AssignServicesToMaster(Master master, List<Service> services)
        {
            foreach (var service in services)
            {
                if (!master.MasterServices.Any(ms => ms.ServiceId == service.Id))
                {
                    master.MasterServices.Add(new MasterService
                    {
                        MasterId = master.Id,
                        ServiceId = service.Id
                    });
                }
            }
        }

        // GET: MastersAdd/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var master = await _masterRepository.GetMasterByIdAsync(id);
            if (master == null)
            {
                return NotFound();
            }

            ViewBag.UniquePositions = await _masterRepository.GetUniquePositionsAsync();
            return View(master);
        }

        // POST: MastersAdd/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Position")] Master master)
        {
            if (id != master.Id)
            {
                return NotFound();
            }

            await _masterRepository.UpdateMasterAsync(master);
            return RedirectToAction(nameof(Index));
        }

        // GET: MastersAdd/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var master = await _masterRepository.GetMasterByIdAsync(id);
            if (master == null)
            {
                return NotFound();
            }

            return View(master);
        }

        // POST: MastersAdd/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _masterRepository.DeleteMasterAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

