using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using WebApplication.Infrastructure;
using WebApplication.Resources;

namespace WebApplication.Filters
{
    public class LinkRewritingFilter: IAsyncResultFilter
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public LinkRewritingFilter(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;

        }
        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var asObjectResult = context.Result as ObjectResult;
            bool shouldSkip = asObjectResult?.StatusCode >= 400
                              || asObjectResult?.Value == null
                              || asObjectResult?.Value as Resource == null;
            if (shouldSkip)
            {
                return next();
            }

            var rewriter = new LinkRewriter(_urlHelperFactory.GetUrlHelper(context));
            RewriteAllLinks(asObjectResult.Value, rewriter);
            return next();
        }

        private static void RewriteAllLinks(object model, LinkRewriter rewriter)
        {
            if (model == null) return;
//            var allProperties = model.GetType().GetTypeInfo().ToProperty().Where(p => p.CanRead).ToArray();
            var allProperties = model.GetType().GetTypeInfo();
//            var linkProperties = allProperties.Where(p => p.CanWrite && p.PropertyType == typeof(Link));
//            foreach (var linkProperty in linkProperties)
//            {
//                var rewritten = rewriter.Rewrite(linkProperty.GetValue(model) as Link);
//                if (rewritten == null) continue;
//                linkProperty.setValue(model, rewritten);
//            }
//
//            var arrayProperties = allProperties.Where(p => p.PropertyType.IsArray);
//            RewriteAllLink;
        }
    }
}