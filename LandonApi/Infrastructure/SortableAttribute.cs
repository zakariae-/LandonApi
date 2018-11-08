using System;
namespace LandonApi.Infrastructure
{
    // This target is a property and that we can't have more than one
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SortableAttribute : Attribute
    {
    }
}
