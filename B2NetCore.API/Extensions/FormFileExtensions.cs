using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace B2NetCore.API.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<byte[]> ToBytes(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static async Task<MemoryStream> ToMemoryStream(this IFormFile formFile)
        {
            var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream;
        }
    }
}
