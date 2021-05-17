﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimulatedCloudSdk.Queue;
using TennisBookings.Merchandise.Api.External.Queue;

namespace TennisBookings.APITests.Fakes
{
    public class FakeCloudQueue : ICloudQueue
    {
        public List<SendRequest> Requests = new List<SendRequest>();

        public Task<SendResponse> SendAsync(SendRequest request)
        {
            Requests.Add(request);
            return Task.FromResult(new SendResponse { IsSuccess = true, StatusCode = 200 });
        }
    }
}
