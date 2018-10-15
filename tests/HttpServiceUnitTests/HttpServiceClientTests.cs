using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Moq.Protected;

using HttpService;


namespace HttpServiceUnitTests
{
    [TestClass]
    public class HttpServiceClientTests
    {
        [TestMethod]
        public void SerializeNull()
        {
            //arrange
            var srv = new HttpServiceClient(HttpClientFactory.CreateHttpClient());
            //act
            var result = srv.Serialize(null);
            //assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task ProcessResponseAsync_NotFoundResponse()
        {
            //arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Content = new StringContent("[{'id':1,'value':'1'}]")
                })
                .Verifiable();

            var srv = new HttpServiceClient(new HttpClient(handlerMock.Object));
            //act
            var request = srv.CreateRequest(HttpMethod.Post, "https://mock/users");
            var result = await srv.SendAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task ProcessResponseAsync_NoContentResponse()
        {
            //arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent,
                    Content = new StringContent("[{'id':1,'value':'1'}]")
                })
                .Verifiable();

            var srv = new HttpServiceClient(new HttpClient(handlerMock.Object));
            //act
            var request = srv.CreateRequest(HttpMethod.Post, "https://mock/users");
            var result = await srv.SendAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task ProcessResponseAsync_NotModifiedResponse()
        {
            //arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotModified,
                    Content = new StringContent("[{'id':1,'value':'1'}]")
                })
                .Verifiable();

            var srv = new HttpServiceClient(new HttpClient(handlerMock.Object));
            //act
            var request = srv.CreateRequest(HttpMethod.Post, "https://mock/users");
            var result = await srv.SendAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task ProcessResponseAsync_NullResponse()
        {
            //arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new ByteArrayContent(new byte[0])
                })
                .Verifiable();

            var srv = new HttpServiceClient(new HttpClient(handlerMock.Object));
            //act
            var request = srv.CreateRequest(HttpMethod.Post, "https://mock/users");
            var result = await srv.SendAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task ProcessResponseAsync_EmptyResponse()
        {
            //arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                })
                .Verifiable();

            var srv = new HttpServiceClient(new HttpClient(handlerMock.Object));
            //act
            var request = srv.CreateRequest(HttpMethod.Post, "https://mock/users");
            var result = await srv.SendAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsTrue(string.IsNullOrEmpty(result));
        }


        [TestMethod]
        public async Task ProcessResponseAsync_OkResponse()
        {
            //arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                // Setup the PROTECTED method to mock
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                // prepare the expected response of the mocked http call
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("[{'id':1,'value':'1'}]")
                })
                .Verifiable();

            var srv = new HttpServiceClient(new HttpClient(handlerMock.Object));
            //act
            var request = srv.CreateRequest(HttpMethod.Post, "https://mock/users");
            var result = await srv.SendAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }
    }
}
