namespace AlbinMicroService.Core.Utilities
{
    public class ValidatorTemplate
    {
        public bool IsValidated { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
