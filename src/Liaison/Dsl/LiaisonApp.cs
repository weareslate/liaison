using System;
using System.Collections.Generic;
using Liaison.AspNetCore;
using Microsoft.AspNetCore.Builder;

namespace Liaison.Dsl
{
    internal class LiaisonApp : ILiaisonApp
    {
        private readonly IRouteTable routeTable;
        private readonly IApplicationBuilder app;

        public LiaisonApp(
            IRouteTable routeTable,
            IApplicationBuilder app)
        {
            this.routeTable = routeTable;
            this.app = app;
        }
        
        public ILiaisonRoute Route(
            string uri,
            string dataObjectKey = null,
            Type[] pipeline = null,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null )
        {
            return new LiaisonRoute( this.routeTable, uri, this.app );
        }

        internal void Build()
        {
            this.app.Use( next => new MapMediatRRequestMiddleware( next, app )
                                                                 .Invoke );
        }
    }
}