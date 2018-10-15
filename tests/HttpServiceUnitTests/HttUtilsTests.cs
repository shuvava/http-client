using Microsoft.VisualStudio.TestTools.UnitTesting;

using HttpService;


namespace HttpServiceUnitTests
{
    [TestClass]
    public class HttUtilsTests
    {
        [TestMethod]
        public void JoinUrls_WithOutTrailSlash()
        {
            //arrange
            const string baseUrl = "http://localhost";
            const string relativeUrl = "api/users";
            //act
            var result = HttpUtils.JoinUrls(baseUrl, relativeUrl);
            //assert
            Assert.AreEqual($"{baseUrl}/{relativeUrl}", result);
        }
    }
}
