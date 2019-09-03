using System;
using System.Collections.Generic;
using System.Linq;
using Liaison.AspNetCore.Core;

namespace Liaison.AspNetCore
{
    internal class RouteDictionary 
        : Dictionary<HttpMethods, IList<Tuple<RouteItem, IUriParser>>>
    {
        public bool AddRoute(RouteItem route, IUriParser uriParser)
        {
            var hasMethod = this.TryGetValue(route.Method, out var matches);

            if ( !hasMethod ||  matches == null  )
            {
                // would let fall through normally
                // but the next check gets the value out then executes the any
                // so easy to just ignore doing that
                this[route.Method] = new List<Tuple<RouteItem, IUriParser>> {new Tuple<RouteItem, IUriParser>( route, uriParser )};
                return true;
            }

            var alreadyExists = this[ route.Method ].Any( x => x.Item1.Route == route.Route );
            if ( alreadyExists )
            {
                return false;
            }

            this[route.Method].Add( new Tuple<RouteItem, IUriParser>( route, uriParser ) );
            return true;
        }

        public (bool, RouteItem, IUriParser) GetRoute(
            string method,
            string uri )
        {
            var isMethod = Enum.TryParse<HttpMethods>( method, out var convertedMethod );

            return !isMethod 
                       ? (false, null, null) 
                       : this.GetRoute( convertedMethod, uri );
        }

        public (bool, RouteItem, IUriParser) GetRoute(
            HttpMethods method,
            string uri )
        {
            var hasMethod = this.TryGetValue(method, out var matches);

            if ( !hasMethod || matches == null )
            {
                return (false, null, null );
            }

            var (routeItem, uriParser) = matches.FirstOrDefault(x => x.Item2?.IsMatch( uri ) ?? false) 
                                      ?? new Tuple<RouteItem, IUriParser>( null, null );

            return ( routeItem != null && uriParser != null, routeItem, uriParser );
        }
    }
}