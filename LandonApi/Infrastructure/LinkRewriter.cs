using System;
using LandonApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace LandonApi.Infrastructure
{
    /**
     * To generate an absolute URL, we need to access an ASP.NET core service called IUrlHelper
     * 
     */
    public class LinkRewriter
    {
        private readonly IUrlHelper _urlHelper;

        public LinkRewriter(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        public Link Rewrite(Link original)
        {
            if (original == null) return null;

            return new Link
            {
                Href = _urlHelper.Link(original.RouteName, original.RouteValues),
                Method = original.Method,
                Relations = original.Relations
            };
        }
    }
}
