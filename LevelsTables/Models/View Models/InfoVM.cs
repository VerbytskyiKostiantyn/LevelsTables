using AlisonicLevels.Models;
using Levels.Models;

namespace LevelsTables.Models.View_Models
{
    public class InfoVM
    {
        public Tank Tank { get; set; }
        public List<Calibration>? Calibrations { get; set; }
        public List<DateTime>? allUploadTimes { get; set; }
        public DateTime currentTableTime { get; set; }
    }
}
