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
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public async Task<IActionResult> Info(int id, string? date)
        {
            DateTime dateTime = new DateTime();
            if (date != null)
            {
                dateTime = DateTime.ParseExact(date, "o", CultureInfo.InvariantCulture);
            }


            var allUploadDates = _db.Calibrations
                .Where(t => t.TankId == id)
                .Select(t => t.timeOfUpload)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

            List<Calibration> values = new List<Calibration>();
            DateTime currentTableTime = new DateTime();

            if (allUploadDates.Any())
            {
                //going from info with date
                if (date != null)
                {
                    values = await _db.Calibrations.Where(q => q.TankId == id && q.timeOfUpload == dateTime).OrderBy(q => q.Level).ToListAsync();
                    currentTableTime = dateTime;
                }
                else //going from index
                {
                    values = await _db.Calibrations.Where(q => q.TankId == id && q.timeOfUpload == allUploadDates[0]).OrderBy(q => q.Level).ToListAsync();
                    currentTableTime = allUploadDates[0];
                }
            }

            InfoVM infoVM = new InfoVM
            {
                Tank = _db.Tanks.FirstOrDefault(s => s.Id == id),
                Calibrations = values,
                allUploadTimes = allUploadDates,
                currentTableTime = currentTableTime
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
                    Delimiter = ";",
                    HasHeaderRecord = true,
                };

                var csv = new CsvReader(reader, csvConfig);
                var records = csv.GetRecords<Calibration>();

                DateTime currentTime = DateTime.Now;

                //foreach (var record in records)
                //{
                //    record.TankId = id;
                //    record.timeOfUpload = currentTime;
                //    _db.Calibrations.Add(record);
                //}

                var finalRecords = records.Select(r =>
                {
                    r.TankId = id;
                    r.timeOfUpload = currentTime;
                    return r;
                });

                _db.Calibrations.AddRange(finalRecords);

                _db.SaveChanges();

                return RedirectToAction("Info", id);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id, string? date) {

            DateTime dateTime = DateTime.ParseExact(date, "o", CultureInfo.InvariantCulture);
            var values = _db.Calibrations.Where(q => q.TankId == id && q.timeOfUpload == dateTime);

            _db.Calibrations.RemoveRange(values);

            _db.SaveChanges();

            return Ok(id);
            
            //return RedirectToAction("Info", new {id = id});
        }
        [HttpPost]
        public IActionResult Update(IFormFile file)
        {
            //List<Calibration> calibration = JsonConvert.DeserializeObject<List<Calibration>>(calibrationJson);
            string jsonData = new StreamReader(file.OpenReadStream()).ReadToEnd();
            List<Calibration> calibrationList = JsonConvert.DeserializeObject<List<Calibration>>(jsonData);

            int id = calibrationList[0].TankId;

            foreach(Calibration calibration in calibrationList)
            {
                _db.Calibrations.Update(calibration);
            }

            _db.SaveChanges();

            return RedirectToAction("Info", new { id = id });
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
