using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UriParser = Liaison.AspNetCore.Core.UriParser;

namespace Liaison.AspNetCore
{
    public class RouteTable 
        : IRouteTable
    {
        private readonly ILogger logger;

        private readonly RouteDictionary routeDictionary;
        
        public RouteTable(
            ILoggerFactory loggerFactory )
        {
            this.logger = loggerFactory.CreateLogger( "RouteTable" );
            this.routeDictionary = new RouteDictionary();
        }

        public RouteItem AddRoute< TRequest >(
            HttpMethods method,
            string route,
            RequestDelegate pipeline,
            string dataObjectKey,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null )
        {
            var uriParser = new UriParser( route );

            var routeItem = new RouteItem
                            {
                                Route = route,
                                Method = method,
                                RequestParameters = requestParams,
                                RequestType = typeof(TRequest),
                                DataObjectKey = dataObjectKey,
                                Pipeline = pipeline,
                                WrapResponseDelegate = wrapDelegate,
                                HasRouteParameters = uriParser.HasRouteParameters,
                            };

            if ( this.routeDictionary
                     .AddRoute( routeItem, uriParser ) )
            {
                return routeItem;
            }

            var exception = new Exception( "Registering Duplicate Routes" );
            this.logger
                .LogError( exception,
                           "Trying to add duplicate route keys added, {Route}",
                           route );
            throw exception;

        }

        public RouteItem GetRequest(
            string requestUri,
            string method )
        {
            var (success, routeItem, uriParser) = this.routeDictionary.GetRoute( method, requestUri );
            
            return success ? routeItem : null;
        }

        public (bool, RouteItem) IsValidRoute(
            string requestUri,
            string method )
        {
            var (success, routeItem, uriParser) = this.routeDictionary.GetRoute( method, requestUri );
            
            return (success, routeItem);
        }

        public IDictionary<string, string> ExtrapolateTemplateValues(
            string requestUri,
            RouteItem routeItem )
        {
            var (success, foundRouteItem, uriParser) = this.routeDictionary.GetRoute( routeItem.Method, requestUri );

            if ( !success )
            {
                return new Dictionary<string, string>();
            }
            
            return uriParser.ExtrapolateTemplateValues( requestUri );
        }
    }
}
