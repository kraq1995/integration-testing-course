using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TennisBookings.APITests.Fakes;
using TennisBookings.APITests.Models;
using TennisBookings.Merchandise.Api;
using TennisBookings.Merchandise.Api.Data.Dto;
using TennisBookings.Merchandise.Api.External.Database;

namespace TennisBookings.APITests.Controllers
{
    public class StockControllerTests
    {
        private HttpClient _client;
        private WebApplicationFactory<Startup> _factory;
        public StockControllerTests()
        {

        }

        [OneTimeSetUp]
        public void ClassInit()
        {
            var factory = new WebApplicationFactory<Startup>();
            factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/stock/");
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Test]
        public async Task GetStockTotal_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("total");

            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetStockTotal_ReturnsExpectedJsonContentString()
        {
            var response = await _client.GetStringAsync("total");

            Assert.AreEqual("{\"stockItemTotal\":1870}", response);
        }

        [Test]
        public async Task GetStockTotal_ReturnsExpectedJsonContentType()
        {
            var response = await _client.GetAsync("total");

            Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Test]
        public async Task GetStockTotal_ReturnsExpectedJson()
        {
            var model = await _client.GetFromJsonAsync<ExpectedStockTotalOutputModel>("total");

            Assert.NotNull(model);
            Assert.IsTrue(model.StockItemTotal > 0);
        }

        [Test]
        public async Task GetStockTotal_ReturnsExpectedStockQty()
        {
            var cloudDb = new FakeCloudDatabase(new[] 
            {
                new ProductDto { StockCount = 200 },
                new ProductDto { StockCount = 800 }
            });

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<ICloudDatabase>(cloudDb);
                });
            }).CreateClient();

            var model = await client.GetFromJsonAsync<ExpectedStockTotalOutputModel>("total");

            Assert.AreEqual(1000, model.StockItemTotal);
        }


        [OneTimeTearDown]
        public void ClassCleanup()
        {
            _client.Dispose();
            _factory.Dispose();
        }

    }
}
