using System.ComponentModel.DataAnnotations;

namespace AcerProProject1.Models.Dto
{
    public class TargetAPIDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public int MonitoringInterval { get;  set; }
    }
}
