using Levels.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace AlisonicLevels.Models
{
    public class Calibration
    {
        [Key]
        public int Id { get; set; }
        public int TankId {  get; set; }
        public Tank Tank { get; set; }
        public decimal Level { get; set; }
        public decimal Volume { get; set; }
        public decimal modificator { get; set; }
        public decimal ratio { get; set; }
    }
}
