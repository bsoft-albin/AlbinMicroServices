using AlbinMicroService.Core.FileReaders;
using AlbinMicroService.Kernel.Interfaces;

namespace AlbinMicroService.Kernel.Concretes
{
    public class KernelMeths : IKernelMeths
    {
        public string GetTextFileContents(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentNullException($"{fileName} Cannot be Null or Empty");
            }

            return new TextFileReader().ReadTextFile(fileName);
        }
    }
}
