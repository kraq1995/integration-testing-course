using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using TennisBookings.APITests.Fakes;
using TennisBookings.APITests.Helpers;
using TennisBookings.APITests.Models;
using TennisBookings.Merchandise.Api;
using TennisBookings.Merchandise.Api.Data;
using TennisBookings.Merchandise.Api.External.Queue;

namespace TennisBookings.APITests.Controllers
{
    public class ProductsControllerTests
    {
        private HttpClient _client;
        private CustomWebApplicationFactory<Startup> _factory;

        public ProductsControllerTests()
        {

        }

        [OneTimeSetUp]
        public void ClassInit()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
            _factory.ClientOptions.BaseAddress = new Uri("http://localhost/api/products/");
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task GetAll_ReturnsExpectedArrayOfProducts()
        {
            var products = await _client.GetFromJsonAsync<ExpectedProductModel[]>("");
            Assert.NotNull(products);
            Assert.AreEqual(_factory.FakeCloudDatabase.Products.Count, products.Count());
        }

        [Test]
        public async Task Get_ReturnsExpectedProduct()
        {
            var firstProduct = _factory.FakeCloudDatabase.Products.First();

            var product = await _client.GetFromJsonAsync<ExpectedProductModel>($"{firstProduct.Id}");

            Assert.NotNull(product);
            Assert.AreEqual(firstProduct.Name, product.Name);
        }

        [Test]
        public async Task Post_WithoutName_ReturnsBadRequest()
        {
            var productInputModel = GetValidProductInputModel().CloneWith(m => m.Name = null);

            var response = await _client.PostAsJsonAsync("", productInputModel, JsonSerializerHelper.DefaultDeserializationOptions);

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Post_WithoutName_ReturnsExpectedProblemDetails()
        {
            var productInputModel = GetValidProductInputModel().CloneWith(m => m.Name = null);

            var response = await _client.PostAsJsonAsync("", productInputModel, JsonSerializerHelper.DefaultDeserializationOptions);

            var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();

            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Test]
        public async Task Post_WithExistingProductId_ReturnsConflict_WithExpectedLocation()
        {
            var id = _factory.FakeCloudDatabase.Products.First().Id;
            var content = GetValidProductJsonContent(id);

            var response = await _client.PostAsync("", content);

            Assert.AreEqual(HttpStatusCode.Conflict, response.StatusCode);
            Assert.AreEqual($"http://localhost/api/products/{id}", response.Headers.Location.ToString().ToLower());
        }

        [Test]
        public async Task Post_AfterPostingValidProduct_CanBeRetrieved()
        {
            var id = Guid.NewGuid();
            var content = GetValidProductJsonContent(id);

            var response = await _client.PostAsync("", content);

            response.EnsureSuccessStatusCode();

            var getResponse = await _client.GetAsync(response.Headers.Location.ToString());

            getResponse.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task Post_WithValidProduct_SendsQueueMessage()
        {
            var cloudQueue = new FakeCloudQueue();
            var client = _factory.WithWebHostBuilder(builder =>
           {
               builder.ConfigureTestServices(services =>
               {
                   services.AddSingleton<ICloudQueue>(cloudQueue);
               });
           }).CreateClient();

            var content = GetValidProductJsonContent(Guid.NewGuid());

            var response = await client.PostAsync("", content);

            Assert.IsTrue(cloudQueue.Requests.Count.Equals(1));
        }

        [Test]
        public async Task Post_WithValidProduct_ReturnsCreatedResult()
        {
            var id = Guid.NewGuid();
            var content = GetValidProductJsonContent(id);

            var response = await _client.PostAsync("", content);

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual($"http://localhost/api/products/{id}", response.Headers.Location.ToString().ToLower());
        }

        private static JsonContent GetValidProductJsonContent(Guid? id = null)
        {
            return JsonContent.Create(GetValidProductInputModel(id));
        }

        private static TestProductInputModel GetValidProductInputModel(Guid? id = null)
        {
            return new TestProductInputModel { 
                Id = id is object ? id.Value.ToString() : Guid.NewGuid().ToString(),
                Name = "Some Product",
                Description = "This is a description",
                Category = new CategoryProvider().AllowedCategories().First(),
                InternalReference = "ABC123",
                Price = 4.00m
            };

        }

        [OneTimeTearDown]
        public void TearDown()
        {

        }
    }
}
