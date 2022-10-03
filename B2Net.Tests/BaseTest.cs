using B2.Net.Tests;
using B2Net.Models;

namespace B2Net.Tests {
	public class BaseTest {
		public B2Options Options { get; set; }

		// TODO Change these to valid keys to run tests
		protected string applicationKey = "0030023545b9e6247568c5852d8adf5aa735a8ee35";
		protected string applicationKeyId = "d534cd2ca7df";

		protected string restrictedApplicationKey = "K0019m9qz095omc+WsnREy5mWsxNmtQ";
		protected string restrictedApplicationKeyId = "00151189a8b4c7a000000000d";

		public BaseTest() {
			Options = new B2Options() {
				KeyId = TestConstants.KeyId,
				ApplicationKey = TestConstants.ApplicationKey
			};
		}
	}
}
