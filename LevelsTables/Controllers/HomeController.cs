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
				TempData["Error"] = "Додайте CSV файл";
				return RedirectToAction("Info", id);
			}
			try
			{
				using (var reader = new StreamReader(file.OpenReadStream()))
				{
					var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
					{
						Delimiter = ";",
						HasHeaderRecord = true,
						PrepareHeaderForMatch = (header) => header.Header.ToLower()
					};


					var csv = new CsvReader(reader, csvConfig);
					var records = csv.GetRecords<Calibration>().ToList();

					DateTime currentTime = DateTime.Now;
					decimal previousVolume = -1;
					long stepOfLevels = (long)(records[1].Level - records[0].Level);



					foreach (var record in records)
					{
						record.TankId = id;
						record.timeOfUpload = currentTime;
						if (previousVolume == -1)
						{
							record.ratio = 0;
						}
						else
						{
							record.ratio = (record.Volume - previousVolume) / stepOfLevels;
						}

						_db.Calibrations.Add(record);

						previousVolume = record.Volume;
					}


					_db.Calibrations.AddRange(records);

					_db.SaveChanges();
					TempData["Success"] = "Дані успішно додані";
					return RedirectToAction("Info", id);
				}
			}
			catch
			{
				TempData["Error"] = "Додайте правильний CSV файл";
				return RedirectToAction("Info", id);
			}
		}

		[HttpPost]
		public IActionResult Delete(int id, string? date)
		{
			try
			{
				DateTime dateTime = DateTime.ParseExact(date, "o", CultureInfo.InvariantCulture);
				var values = _db.Calibrations.Where(q => q.TankId == id && q.timeOfUpload == dateTime);

				_db.Calibrations.RemoveRange(values);

				_db.SaveChanges();
				TempData["Success"] = "Дані успішно видалені";
				return Ok(id);
			}
			catch
			{
				TempData["Error"] = "Помилка під час видалення";
				return Ok(id);
			}
			//return RedirectToAction("Info", new {id = id});
		}
		[HttpPost]
		public IActionResult Update(IFormFile file)
		{
			string jsonData = new StreamReader(file.OpenReadStream()).ReadToEnd();
			List<Calibration> calibrationList = JsonConvert.DeserializeObject<List<Calibration>>(jsonData);

			int id = calibrationList[0].TankId;
			decimal previousVolume = -1;
			try
			{
				long stepOfLevels = (long)(calibrationList[1].Level - calibrationList[0].Level);

				foreach (Calibration calibration in calibrationList)
				{
					if (previousVolume == -1)
					{
						calibration.ratio = 0;
					}
					else
					{
						calibration.ratio = (calibration.Volume - previousVolume) / stepOfLevels;
					}
					_db.Calibrations.Update(calibration);
					previousVolume = calibration.Volume;
				}

				_db.SaveChanges();

				return RedirectToAction("Info", new { id = id });
			}
			catch
			{
				TempData["Error"] = "Введені дані не вірні";
				return RedirectToAction("Info", new { id = id });
			}
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
