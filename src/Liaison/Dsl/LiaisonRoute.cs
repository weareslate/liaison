using System;
using System.Collections.Generic;
using Liaison.AspNetCore;
using Liaison.Extensions;
using Microsoft.AspNetCore.Builder;

namespace Liaison.Dsl
{
    internal class LiaisonRoute : ILiaisonRoute
    {
        private readonly IRouteTable routeTable;
        private readonly string baseUri;
        private readonly IApplicationBuilder app;

        public LiaisonRoute(
            IRouteTable routeTable,
            string baseUri,
            IApplicationBuilder app )
        {
            this.routeTable = routeTable;
            this.baseUri = baseUri;
            this.app = app;
        }
        
        public ILiaisonRoute Get< TRequest >(
            string template = null,
            Type[] pipeline = null,
            string dataObjectKey = null,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null )
        {
            this.AddRoute<TRequest>( HttpMethods.GET,
                                     template,
                                     pipeline,
                                     dataObjectKey,
                                     requestParams,
                                     wrapDelegate );
            return this;
        }

        public ILiaisonRoute Post< TRequest >(
            string template = null,
            Type[] pipeline = null,
            string dataObjectKey = null,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null )
        {
            this.AddRoute<TRequest>( HttpMethods.POST,
                                     template,
                                     pipeline,
                                     dataObjectKey,
                                     requestParams,
                                     wrapDelegate );
            return this;
        }

        public ILiaisonRoute Put< TRequest >(
            string template  = "{id}",
            Type[] pipeline = null,
            string dataObjectKey = null,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null )
        {
            this.AddRoute<TRequest>( HttpMethods.PUT,
                                     template,
                                     pipeline,
                                     dataObjectKey,
                                     requestParams,
                                     wrapDelegate );
            return this;
        }

        public ILiaisonRoute Patch< TRequest >(
            string template = "{id}",
            Type[] pipeline = null,
            string dataObjectKey = null,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null )
        {
            this.AddRoute<TRequest>( HttpMethods.PATCH,
                                     template,
                                     pipeline,
                                     dataObjectKey,
                                     requestParams,
                                     wrapDelegate );
            return this;
        }

        public ILiaisonRoute Delete< TRequest >(
            string template = "{id}",
            Type[] pipeline = null,
            string dataObjectKey = null,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null )
        {
            this.AddRoute<TRequest>( HttpMethods.DELETE,
                                     template,
                                     pipeline,
                                     dataObjectKey,
                                     requestParams,
                                     wrapDelegate );
            return this;
        }
        
        private void AddRoute< TRequest >(
            HttpMethods method,
            string template = null,
            Type[] pipeline = null,
            string dataObjectKey = null,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null )
        {
            var branchBuilder = this.app.New();
            pipeline
              ?.ForEach( p => branchBuilder.UseMiddleware( p ) );
            branchBuilder.UseMiddleware<HttpApiMiddleware>();
            var branch = branchBuilder.Build();

            var resultUri = this.baseUri;
            if ( !string.IsNullOrWhiteSpace( template ) )
            {
                resultUri = $"{resultUri}/{template}";
            }

            this.routeTable
                .AddRoute<TRequest>( method,
                                     $"/api/{resultUri}",
                                     branch,
                                     dataObjectKey,
                                     requestParams,
                                     wrapDelegate );
        }
    }
}