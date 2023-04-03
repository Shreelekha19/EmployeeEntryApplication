using EmployeeEntryApplication.Models;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace EmployeeEntryApplication.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            List<Employee> employees = new List<Employee>();

            using (var httpClient = new HttpClient())
            {
                using (
                    var response = await httpClient.GetAsync(
                        "https://localhost:7124/api/v1/employee"
                    )
                )
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };

                    employees = JsonConvert.DeserializeObject<List<Employee>>(
                        apiResponse,
                        settings
                    );
                }
            }

            return View(employees);
        }

        public ViewResult SaveEmployee()
        {
            return View("Employee");
        }

        [HttpPost]
        public async Task<ActionResult> SaveEmployee(Employee employee)
        {
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(
                    JsonConvert.SerializeObject(employee),
                    Encoding.UTF8,
                    "application/json"
                );
                string endpoint = "https://localhost:7124/api/v1/employee";

                var response = await client.PostAsync(endpoint, content);

                if (Response.StatusCode == 200)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View("Employee");
        }
    }
}
