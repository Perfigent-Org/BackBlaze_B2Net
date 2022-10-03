using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace B2Net.Tests
{
    [TestClass]
    public class KeyTests : BaseTest
    {
        private B2Client Client = null;
        private string KeyName = $"B2NETTestingKey-{Path.GetRandomFileName().Replace(".", "").Substring(0, 6)}";

        [TestInitialize]
        public void Initialize()
        {
            Client = new B2Client(B2Client.Authorize(Options).Result);
        }

        [TestMethod]
        public void GetKeyListTest()
        {
            var list = Client.Keys.GetList().Result;
            Assert.AreNotEqual(0, list.Count);
        }

        [TestMethod]
        public void CreateKeyTest()
        {
            var name = KeyName;
            var key = Client.Keys.Create(name, new string[] { "listFiles" }).Result;

            //Clean up
            if (!string.IsNullOrEmpty(key.KeyName))
            {
                Client.Keys.Delete(key.ApplicationKeyId).Wait();
            }

            Assert.AreEqual(name, key.KeyName);
        }
    }
}
