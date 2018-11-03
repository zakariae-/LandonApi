using System;
namespace LandonApi.Models
{
    public class Room : Resource
    {
        public string Name { get; set; }

        public decimal Rate { get; set; }
    }
}
