﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TennisBookings.APITests.Fakes;
using TennisBookings.Merchandise.Api.External.Database;

namespace TennisBookings.APITests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where
        TStartup : class
    {
        public FakeCloudDatabase FakeCloudDatabase { get; }

        public CustomWebApplicationFactory()
        {
            FakeCloudDatabase = FakeCloudDatabase.WithDefaultProducts();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<ICloudDatabase>(FakeCloudDatabase);
            });

            base.ConfigureWebHost(builder);
        }
    }
}
