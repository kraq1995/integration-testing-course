using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TennisBookings.APITests.Helpers
{
    public static class JsonSerializerHelper
    {
        public static JsonSerializerOptions DefaultSerializationOptions => new JsonSerializerOptions
        {
            IgnoreNullValues = false
        };

        public static JsonSerializerOptions DefaultDeserializationOptions => new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }
}
