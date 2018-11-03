using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LandonApi.Models
{
    /**
     * Simple model of a link, the Ion specification describes
     *   - First is the href, which is an absolute uri to the resource, or route
     *   - Second is the method, or HTTP verb that should be used to interact with the resource
     *   - Third is the relation type which can be self, collection, form ...
     * {
     *  "href": "https://xxxx",
     *  "method": "GET",
     *  "rel": ["collection"]
     * }
     **/
    public class Link
    {
        public const string GetMethod = "GET";

        public const string PropertyName = "rel";

        public static Link To(string routeName, object routeValues = null)
        => new Link
        {
            RouteName = routeName,
            RouteValues = routeValues,
            Method = GetMethod,
            Relations = null
        };

        [JsonProperty(Order = -4)]
        public string Href { get; set; }

        [JsonProperty(Order = -3, NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(GetMethod)]
        public string Method { get; set; }

        [JsonProperty(Order = -2, PropertyName = PropertyName, NullValueHandling = NullValueHandling.Ignore)]
        public string[] Relations { get; set; }

        // Store the route name before being rewritten
        [JsonIgnore]
        public string RouteName { get; set; }

        // Store the route name before being rewritten
        [JsonIgnore]
        public object RouteValues { get; set; }
    }
}
