using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication.Filters
{
    public class RequireHttpsOrCloseAttribute : RequireHttpsAttribute
    {
        protected override void HandleNonHttpsRequest(AuthorizationFilterContext filterContext)
        {
            base.HandleNonHttpsRequest(filterContext);
            filterContext.Result = new StatusCodeResult(400);
        }
    }
}