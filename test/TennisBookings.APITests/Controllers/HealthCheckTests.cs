using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using TennisBookings.Merchandise.Api;

namespace TennisBookings.APITests
{
    [TestFixture]
    public class HealthCheckTests
    {
        private WebApplicationFactory<Startup> _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void ClassInit()
        {
            _factory = new WebApplicationFactory<Startup>();
            _client = _factory.CreateDefaultClient();
        }

        [SetUp]
        public void TestInit()
        {

        }

        [Test]
        public async Task HealthCheck_ReturnsOk()
        {
            var response = await _client.GetAsync("/healthcheck");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TearDown]
        public void TestCleanUp()
        {

        }

        [OneTimeTearDown]
        public void ClassCleanup()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
