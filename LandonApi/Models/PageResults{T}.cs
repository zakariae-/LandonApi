using System;
using System.Collections.Generic;

namespace LandonApi.Models
{
    public class PageResults<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalSize { get; set; }
    }
}
