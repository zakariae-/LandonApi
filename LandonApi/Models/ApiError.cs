using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LandonApi.Models
{
    public class ApiError
    {
        public string Message { get; set; }

        public string Detail { get; set; }

        // Add Json.net attribute to the stacktrace property so that it disappears if it's null
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string StackTrace { get; set; }
    }
}
