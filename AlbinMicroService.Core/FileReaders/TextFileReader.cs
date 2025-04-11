using System.Reflection;

namespace AlbinMicroService.Core.FileReaders
{
    public class TextFileReader
    {
        Assembly ExecutingAssembly { get; set; }

        public TextFileReader()
        {
            ExecutingAssembly = Assembly.GetExecutingAssembly();
        }

        // Reads an embedded Human Readbale (HTML/CSS/JS/Xml,Json etc..) file as a string
        public string ReadTextFile(string fileName)
        {
            // here we are using . for accessing the Embeded Resource
            string resourceName = $"AlbinMicroService.Core.Static.Contents.{fileName}";

            using Stream stream = ExecutingAssembly.GetManifestResourceStream(resourceName) ?? throw new FileNotFoundException($"File '{resourceName}' not found.");
            using StreamReader reader = new(stream);
            return reader.ReadToEnd(); // Returns file content as string
        }
    }
}
