using CsvHelper.Configuration.Attributes;
using Levels.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AlisonicLevels.Models
{
    public class Calibration
    {
        [Key]
        [Ignore]
        public int Id { get; set; }
        [Ignore]
        public int TankId {  get; set; }
        [Ignore]
        [JsonIgnore]
        public Tank Tank { get; set; }
        public decimal Level { get; set; }
        public decimal Volume { get; set; }
        public decimal? modificator { get; set; }
        public decimal ratio { get; set; }
        [Ignore]
        public DateTime timeOfUpload { get; set; }
    }
}
