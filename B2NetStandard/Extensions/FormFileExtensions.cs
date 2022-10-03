using System.IO;
using System.Threading.Tasks;

namespace B2NetStandard.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> ToBytes(this FileStream formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static async Task<MemoryStream> ToMemoryStream(this FileStream formFile)
        {
            var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream;
        }
    }
}
