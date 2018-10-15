using Microsoft.VisualStudio.TestTools.UnitTesting;

using HttpService.Serializers;


namespace HttpServiceUnitTests
{
    [TestClass]
    public class JsonRestSerializerTests
    {
        [TestMethod]
        public void DeserializeToStringType()
        {
            //arrange
            var srv = new JsonRestSerializer();
            //atc
            var result = (string) srv.Deserialize("fff", typeof(string));
            //assert
            Assert.AreEqual("fff", result);
        }
    }
}
