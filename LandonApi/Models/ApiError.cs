using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace LandonApi.Models
{
    public class ApiError
    {
        public string Message { get; set; }

        public string Detail { get; set; }

        public ApiError()
        {

        }

        public ApiError(ModelStateDictionary modelState)
        {
            Message = "Invalid parameters";
            Detail = modelState
                .FirstOrDefault(x => x.Value.Errors.Any()).Value.Errors
                .FirstOrDefault().ErrorMessage;
        }

        // Add Json.net attribute to the stacktrace property so that it disappears if it's null
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string StackTrace { get; set; }
    }
}
