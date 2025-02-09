namespace AlbinMicroService.Users.Domain.DTOs
{
    public class BiodataDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public short Age { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
    }
}
