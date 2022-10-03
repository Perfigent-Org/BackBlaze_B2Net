using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using B2NetStandard.Extensions;
using B2NetStandard.UploadFile;

namespace B2NetStandard.Tests
{
    [TestClass]
    public class FileUpload
    {

#if NETFULL
		private string FilePath => Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../");
#else
        private string FilePath => Path.Combine(System.AppContext.BaseDirectory, "../../../");
#endif

        private UploadFilesController uploadFilesController;

        [TestInitialize]
        public void Initialize()
        {
            uploadFilesController = new UploadFilesController("003d534cd2ca7df0000000009", "K003FdBAJnQ5nXl1jKjLnfoBxI7pgWc");
            uploadFilesController.AuthorizeAsync().Wait();
        }

        [TestMethod]
        public async Task UploadFile()
        {
            try
            {
                var fileName = "B2Test.txt";
                FileStream fileStream = File.OpenRead(Path.Combine(FilePath, fileName));
                var fileResponse = await uploadFilesController.Upload(fileStream, fileName, "7d2573c42c7dc25c7ae70d1f", "Test", default);
                Assert.IsTrue(fileResponse.IsSuccessful);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToAllString());
            }
        }
    }
}
