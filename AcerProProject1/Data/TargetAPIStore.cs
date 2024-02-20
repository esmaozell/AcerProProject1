using AcerProProject1.Models.Dto;

namespace AcerProProject1.Data
{
    public static class TargetAPIStore
    {
        public static List<TargetAPIDto> targetAPIList = new List<TargetAPIDto>
        {
               new TargetAPIDto
                {
                    Id = 1,
                    Name = "Test",
                    Url = "Test",
                    MonitoringInterval = 10,
                }
            };
    }
    
}
