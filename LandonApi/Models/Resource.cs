using System;
using Newtonsoft.Json;

namespace LandonApi.Models
{
    public abstract class Resource : Link
    {
        [JsonIgnore]
        public Link Self { get; set; }
    }
}
