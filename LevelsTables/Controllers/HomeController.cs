using AlisonicLevels.Models;
using Levels.Models;
using LevelsTables.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;
using System.Linq;
using LevelsTables.Models.Tables;

namespace LevelsTables.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LevelsDbContext _db;

        public HomeController(ILogger<HomeController> logger, LevelsDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Info(int id)
        {

            var Tank = _db.Tanks.FirstOrDefault(s => s.Id == id);

            return View(Tank);
        }


        [HttpPost]
        public IActionResult Info(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";", // or "," depending on your CSV file
                    HasHeaderRecord = false,
                };

                var csv = new CsvReader(reader, csvConfig);
                var records = csv.GetRecords<Test>();

                foreach (var record in records)
                {
                    // Save the record to the database
                    _db.Tests.Add(record);
                }

                _db.SaveChanges();

                return Ok("CSV file uploaded successfully.");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
