using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TargetAPI_Utility;
using TargetAPI_Web.Models;
using TargetAPI_Web.Models.Dto;
using TargetAPI_Web.Services.IServices;

namespace TargetAPI_Web.Controllers
{
    public class TargetAPIController : Controller
    {
        private readonly ITargetAPIService targetAPIService;
        private readonly IMapper mapper;

        public TargetAPIController(ITargetAPIService targetAPIService, IMapper mapper)
        {
            this.targetAPIService = targetAPIService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> IndexTargetAPI()
        {
            List<TargetAPIDto> list = new();

            var response = await targetAPIService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<TargetAPIDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> CreateTargetAPI()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTargetAPI(TargetAPICreateDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await targetAPIService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "TargetAPI created successfully";
                    return RedirectToAction(nameof(IndexTargetAPI));
                }
            }
            TempData["error"] = "Error encountered";
            return View(model);
        }

        public async Task<IActionResult> UpdateTargetAPI(int id)
        {
            var response = await targetAPIService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TargetAPIDto model = JsonConvert.DeserializeObject<TargetAPIDto>(Convert.ToString(response.Result));
                return View(mapper.Map<TargetAPIUpdateDto>(model));
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateTargetAPI(TargetAPIUpdateDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await targetAPIService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionToken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "TargetAPI updated successfully";
                    return RedirectToAction(nameof(IndexTargetAPI));
                }
            }
            TempData["error"] = "Error encountered";
            return View(model);
        }

        public async Task<IActionResult> DeleteTargetAPI(int id)
        {
            var response = await targetAPIService.GetAsync<APIResponse>(id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TargetAPIDto model = JsonConvert.DeserializeObject<TargetAPIDto>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTargetAPI(TargetAPIDto model)
        {
            var response = await targetAPIService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "TargetAPI deleted successfully";
                return RedirectToAction(nameof(IndexTargetAPI));
            }
            TempData["error"] = "Error encountered";
            return View(model);
        }
    }
}
