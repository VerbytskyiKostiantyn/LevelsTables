using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LevelsTables.Models.Tables
{
    public class Test
    {
        [Key]
        [Ignore]
        public int Id { get; set; }
        public int level { get; set; }
        public int value { get; set; }
    }
}
