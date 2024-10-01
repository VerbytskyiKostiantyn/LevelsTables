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
using System.Text.Json;
using LevelsTables.Models.View_Models;

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
        public async Task<IActionResult> Info(int id)
        {
            List<Calibration> values = await _db.Calibrations.Where(q => q.TankId == id).OrderBy(q => q.Level).ToListAsync();

            InfoVM infoVM = new InfoVM
            {
                Tank = _db.Tanks.FirstOrDefault(s => s.Id == id),
                Calibrations = values
            };

            return View(infoVM);
        }


        [HttpPost]
        public IActionResult Info(int id, IFormFile file)
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
                    HasHeaderRecord = true,
                };

                var csv = new CsvReader(reader, csvConfig);
                var records = csv.GetRecords<Calibration>();

                DateTime currentTime = DateTime.Now;

                foreach (var record in records)
                {
                    record.TankId = id;
                    record.timeOfUpload = currentTime;
                    // Save the record to the database
                    _db.Calibrations.Add(record);
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
