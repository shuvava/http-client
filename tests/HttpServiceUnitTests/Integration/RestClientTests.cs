using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json;

using HttpService;
using HttpService.Abstractions.Exceptions;
using HttpService.Serializers;
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Local


namespace HttpServiceUnitTests.Integration
{
    [TestClass]
    [TestCategory("Integration")]
    public class RestClientTests
    {
        private readonly string _baseUrl = "https://reqres.in";


        [TestMethod]
        public async Task GetAsync_SuccessResponse()
        {
            //arrange
            var srv = RestClientFactory();
            var url = HttpUtils.JoinUrls(_baseUrl, "/api/users");
            //act
            var result = await srv.GetAsync<Response<User>>(url).ConfigureAwait(false);
            //assert
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public async Task GetAsync_NotFoundResponse()
        {
            //arrange
            var srv = RestClientFactory();
            var url = HttpUtils.JoinUrls(_baseUrl, "/api/users/23");
            //act
            var result = await srv.GetAsync<Response<User>>(url).ConfigureAwait(false);
            //assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task DeleteAsync_SuccessResponse()
        {
            //arrange
            var srv = RestClientFactory();
            var url = HttpUtils.JoinUrls(_baseUrl, "/api/users/2");
            //act
            var result = await srv.DeleteAsync<User>(url).ConfigureAwait(false);
            //assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public async Task PostAsync_SuccessResponse()
        {
            //arrange
            var srv = RestClientFactory();
            var url = HttpUtils.JoinUrls(_baseUrl, "/api/register");

            var model = new Register
            {
                Email = "test@test.test",
                Password = "1"
            };

            //act
            var result = await srv.PostAsync<Register, RegisterResponse>(url, model).ConfigureAwait(false);
            //assert
            Assert.IsFalse(string.IsNullOrEmpty(result.Token));
        }


        [TestMethod]
        public async Task PostAsync_UnSuccessResponse()
        {
            //arrange
            var srv = RestClientFactory();
            var url = HttpUtils.JoinUrls(_baseUrl, "/api/register");

            var model = new Register
            {
                Email = "test@test.test"
            };

            var testResult = false;

            //act
            try
            {
                await srv.PostAsync<Register, RegisterResponse>(url, model).ConfigureAwait(false);
            }
            catch (HttpException ex)
            {
                testResult = ex.StatusCode == HttpStatusCode.BadRequest;
            }

            //assert
            Assert.IsTrue(testResult);
        }


        public RestClient RestClientFactory()
        {
            var serializer = new JsonRestSerializer();

            return new RestClient(HttpClientFactory.CreateHttpClient(), serializer);
        }


        private class Register
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }


        private class RegisterResponse
        {
            public string Token { get; set; }
        }


        private class User
        {
            public int Id { get; set; }


            [JsonProperty("first_name")]
            public string FirstName { get; set; }


            [JsonProperty("last_name")]
            public string LastName { get; set; }


            public string Avatar { get; set; }
        }


        private class Response<T>
        {
            public int Page { get; set; }
            public int Total { get; set; }
            public IEnumerable<T> Data { get; set; }
        }
    }
}
