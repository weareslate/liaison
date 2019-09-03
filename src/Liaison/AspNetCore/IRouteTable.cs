using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Liaison.AspNetCore
{
    public interface IRouteTable
    {
        RouteItem AddRoute< TRequest >(
            HttpMethods method,
            string route,
            RequestDelegate pipeline,
            string dataObjectKey,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null );

        RouteItem GetRequest(
            string requestUri,
            string method);

        (bool, RouteItem) IsValidRoute(
            string requestUri,
            string method );

        IDictionary<string, string> ExtrapolateTemplateValues(
            string requestUri,
            RouteItem routeItem );
    }
}