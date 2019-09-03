using System.Threading.Tasks;
using Liaison.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Liaison
{
    internal class MapMediatRRequestMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IApplicationBuilder app;

        public MapMediatRRequestMiddleware(
            RequestDelegate next,
            IApplicationBuilder app)
        {
            this.next = next;
            this.app = app;
        }

        public Task Invoke(
            HttpContext context )
        {
            // this avoids the issue where the Invoke method is used in AppBuilderExtensions
            var routeTable = context.RequestServices
                                    .GetRequiredService<IRouteTable>();

            var (isValid, routeItem) = routeTable.IsValidRoute( context.Request.Path,
                                                   context.Request.Method );

            return !isValid 
                       ? this.next( context ) 
                       : routeItem.Pipeline( context );
        }
    }
}