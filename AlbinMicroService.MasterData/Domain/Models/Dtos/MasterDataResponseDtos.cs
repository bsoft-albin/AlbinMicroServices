namespace AlbinMicroService.MasterData.Domain.Models.Dtos
{
    public class CountryResponse
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string? DialCode { get; set; }
        public string Name { get; set; } = null!;
    }
}
