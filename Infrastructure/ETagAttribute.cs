using System;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication.Filters;

namespace WebApplication.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ETagAttribute : Attribute, IFilterFactory
    {
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            return new ETagHeaderFilter();
        }

        public bool IsReusable => true;
    }
}