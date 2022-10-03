using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;
using B2NetStandard.Extensions;

namespace B2NetStandard.Tests
{
    [TestClass]
    public class FileTests
    {

#if NETFULL
		private string FilePath => Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../");
#else
        private string FilePath => Path.Combine(System.AppContext.BaseDirectory, "../../../");
#endif

        private FilesController filesController;

        [TestInitialize]
		public void Initialize()
		{
            filesController = new FilesController("003d534cd2ca7df0000000009", "K003FdBAJnQ5nXl1jKjLnfoBxI7pgWc");
            filesController.AuthorizeAsync().Wait();
        }

		[TestMethod]
        public async Task UploadFile()
        {
            try
            {
                var fileName = "B2Test.txt";
                FileStream fileStream = File.OpenRead(Path.Combine(FilePath, fileName));
                var fileResponse = await filesController.Upload(fileStream, fileName, "7d2573c42c7dc25c7ae70d1f", default);
                Assert.IsTrue(fileResponse.IsSuccessful);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToAllString());
            }
        }

        [TestMethod]
        public async Task DownloadFile()
        {
            try
            {
                var fileName = "B2Test.txt";
                FileStream fileStream = File.OpenRead(Path.Combine(FilePath, fileName));
                var fileData = await filesController.Upload(fileStream, fileName, "7d2573c42c7dc25c7ae70d1f", default);
                var fileResponse = await filesController.Download(fileData.FileId);
                Assert.IsTrue(fileResponse.IsSuccessful);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToAllString());
            }
        }
    }
}
