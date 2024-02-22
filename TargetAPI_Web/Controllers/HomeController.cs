using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using TargetAPI_Utility;
using TargetAPI_Web.Models;
using TargetAPI_Web.Models.Dto;
using TargetAPI_Web.Services.IServices;
namespace TargetAPI_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITargetAPIService targetAPIService;
        private readonly IMapper mapper;

        public HomeController(ITargetAPIService targetAPIService, IMapper mapper)
        {
            this.targetAPIService = targetAPIService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            List<TargetAPIDto> list = new();

            var response = await targetAPIService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<TargetAPIDto>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
    }
}
