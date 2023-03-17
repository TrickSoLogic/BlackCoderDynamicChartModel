using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using BlackCoderDynamicChartModel.Models;
using System.Net.Http.Headers;

namespace BlackCoderDynamicChartModel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        HttpClient httpClient = new HttpClient();
        [HttpGet]
        public IActionResult Index()
        {
            string baseUrl = "https://localhost:7243/api/Values";
            List<SaleData> saleData = new List<SaleData>();

            var response = httpClient.GetAsync(baseUrl);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<SaleData>>();
                display.Wait();
                saleData = display.Result;
            }
            ViewBag.Data = JsonConvert.SerializeObject(saleData.Select(prop => new {label = prop.Month, y=prop.Sale}).ToList());
            return View(saleData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}