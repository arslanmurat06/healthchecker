using System.Diagnostics;
using HealthChecker.Contracts.DTOs;
using HealthChecker.Contracts.Interfaces.Repositories;
using HealthChecker.Contracts.Interfaces.Services;
using HealthCheckerWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCheckerWeb.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly IJobService _jobService;

        public JobsController(ILogger<HomeController> logger, IJobService jobService)
        {
            _jobService = jobService;
        }


        public async Task<IActionResult> Index()
        {

            var userName = User.Identity.Name;

            var result = await _jobService.GetJobs(userName);
            if(result.Success)
                return View(result.Data);

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }


        //TODO: Model valid controlü yap

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobDTO job)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;

                var result = await _jobService.AddJob(job, userName);

                if (!result.Success)
                {
                    TempData["error"] = "Job could not be created";
                    return NotFound();
                }

                TempData["success"] = "Job created successfully";

                return RedirectToAction("Index");
            }

            return View();

        }


        public async Task<IActionResult> Edit(int id)
        {

           
             var userName = User.Identity.Name;

             var result = await _jobService.GetJob(id);

            if (!result.Success)
                return NotFound();
            
            return View(result.Data);

          
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobDTO job)
        {
           
                var userName = User.Identity.Name;

                var result = await _jobService.UpdateJob(job, userName);

            if (!result.Success)
            {
                TempData["error"] = "Job could not be updated";
                return NotFound();
            }

            TempData["success"] = "Job updated successfully";

            return RedirectToAction("Index");

        }


        public async Task<IActionResult> Delete(int id)
        {
            var userName = User.Identity.Name;

            var result = await _jobService.GetJob(id);

            if (!result.Success)
                return NotFound();

            return View(result.Data);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteJob(int  id)
        {
            
            var userName = User.Identity.Name;
         
            var result = await _jobService.DeleteJob(id, userName);

            if (!result.Success)
            {
                TempData["error"] = "Job could not be deleted";
                return NotFound();
            }


            TempData["success"] = "Job deleted successfully";

            return RedirectToAction("Index");

        }
    }
}
