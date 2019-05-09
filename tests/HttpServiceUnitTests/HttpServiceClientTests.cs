using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
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
        public void SerializeByteArray()
        {
            //arrange
            var srv = new HttpServiceClient(HttpClientFactory.CreateHttpClient());
            var str = "Hello World!";
            byte[] content = Encoding.ASCII.GetBytes(str);
            //act
            var result = srv.Serialize(content);
            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(ByteArrayContent), result.GetType());
        }

        [TestMethod]
        public void SerializeStream()
        {
            //arrange
            var srv = new HttpServiceClient(HttpClientFactory.CreateHttpClient());
            var str = "Hello World!";

            using (Stream content = new MemoryStream(Encoding.ASCII.GetBytes(str)))
            {
                //act
                var result = srv.Serialize(content);
                //assert
                Assert.IsNotNull(result);
                Assert.AreEqual(typeof(StreamContent), result.GetType());
            }
        }

        [TestMethod]
        public void SerializeIncompatibleContent()
        {
            //arrange
            var srv = new HttpServiceClient(HttpClientFactory.CreateHttpClient());
            var str = "Hello World!";

            //act
            var result = srv.Serialize(new object());
            //assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task GetResponseAsString_NotFoundResponse()
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
            var result = await srv.GetResponseAsStringAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task ProcessResponseAsStream_NotFoundResponse()
        {
            //arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var str = "[{'id':1,'value':'1'}]";

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
                    Content = new StringContent(str)
                })
                .Verifiable();

            var srv = new HttpServiceClient(new HttpClient(handlerMock.Object));
            //act
            var request = srv.CreateRequest(HttpMethod.Post, "https://mock/users");
            var result = await srv.GetResponseAsStreamAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }


        [TestMethod]
        public async Task GetResponseAsString_NoContentResponse()
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
            using (var request = srv.CreateRequest(HttpMethod.Post, "https://mock/users"))
            {
                var result = await srv.GetResponseAsStringAsync(request).ConfigureAwait(false);
                //assert
                Assert.IsNull(result);
            }
        }

        [TestMethod]
        public async Task GetResponseAsStream_NoContentResponse()
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
            using (var request = srv.CreateRequest(HttpMethod.Post, "https://mock/users"))
            {
                var result = await srv.GetResponseAsStreamAsync(request).ConfigureAwait(false);
                //assert
                Assert.IsNotNull(result);
                Assert.AreEqual(0, result.Length);
            }
        }


        [TestMethod]
        public async Task GetResponseAsString_NotModifiedResponse()
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
            var result = await srv.GetResponseAsStringAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetResponseAsStream_NotModifiedResponse()
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
            var result = await srv.GetResponseAsStreamAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }


        [TestMethod]
        public async Task GetResponseAsString_NullResponse()
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
            var result = await srv.GetResponseAsStringAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetResponseAsStream_NullResponse()
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
            var result = await srv.GetResponseAsStreamAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }


        [TestMethod]
        public async Task GetResponseAsString_EmptyResponse()
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
            var result = await srv.GetResponseAsStringAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsTrue(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public async Task GetResponseAsStream_EmptyResponse()
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
            var result = await srv.GetResponseAsStreamAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
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
            var result = await srv.GetResponseAsStringAsync(request).ConfigureAwait(false);
            //assert
            Assert.IsFalse(string.IsNullOrEmpty(result));
        }

        [TestMethod]
        public async Task GetResponseAsStreamAsync_OkResponse()
        {
            //arrange
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var str = "[{'id':1,'value':'1'}]";

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
                    Content = new StringContent(str)
                })
                .Verifiable();

            var srv = new HttpServiceClient(new HttpClient(handlerMock.Object));
            //act
            var request = srv.CreateRequest(HttpMethod.Post, "https://mock/users");

            using (var stream = await srv.GetResponseAsStreamAsync(request).ConfigureAwait(false))
            {
                StreamReader reader = new StreamReader(stream);
                string result = reader.ReadToEnd();
                //assert
                Assert.AreEqual(str, result);

            }
        }

    }
}
