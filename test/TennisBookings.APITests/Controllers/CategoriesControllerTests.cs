using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using TennisBookings.APITests.Helpers;
using TennisBookings.APITests.Models;
using TennisBookings.Merchandise.Api;

namespace TennisBookings.APITests
{
    public class CategoriesControllerTests
    {
        private WebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        public CategoriesControllerTests()
        {
        }

        [OneTimeSetUp]
        public void ClassInit()
        {
            _factory = new WebApplicationFactory<Startup>();
            _client = _factory.CreateDefaultClient(new Uri("http://localhost/api/categories"));
        }

        [Test]
        public async Task GetAll_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("");

            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetAll_ReturnsExpectedJson()
        {
            var expected = new List<string> { "Accessories", "Bags", "Balls", "Clothing", "Rackets" };
            var responseStream = await _client.GetStreamAsync("");

            var model = await JsonSerializer.DeserializeAsync<ExpectedCategoriesModel>(responseStream, JsonSerializerHelper.DefaultDeserializationOptions);

            Assert.NotNull(model?.AllowedCategories);
            Assert.AreEqual(expected.OrderBy(s => s), model.AllowedCategories.OrderBy(s => s));

        }

        [Test]
        public async Task GetAll_ReturnsExpectedResponse()
        {
            var expected = new List<string> { "Accessories", "Bags", "Balls", "Clothing", "Rackets" };

            var model = await _client.GetFromJsonAsync<ExpectedCategoriesModel>("");

            Assert.NotNull(model?.AllowedCategories);
            Assert.AreEqual(expected.OrderBy(s => s), model.AllowedCategories.OrderBy(s => s));
        }

        [Test]
        public async Task GetAll_SetsExpectedCacheControlHeader()
        {
            var response = await _client.GetAsync("");
            var header = response.Headers.CacheControl;

            Assert.True(header.MaxAge.HasValue);
            Assert.AreEqual(TimeSpan.FromMinutes(5), header.MaxAge);
            Assert.True(header.Public);
        }
    }
}
