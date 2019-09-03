using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Liaison.AspNetCore;
using Liaison.AspNetCore.Extensions;
using Liaison.ExceptionHandling;
using Liaison.Extensions;
using Liaison.Orchestrator;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Liaison
{
    internal class HttpApiMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IMediatRPipelineOrchestratorFactory pipelineOrchestratorFactory;
        private readonly ICancellationTokenHolder cancellationTokenHolder;
        private readonly ILogger<HttpApiMiddleware> logger;
        private readonly IExceptionFlow exceptionFlow;

        public HttpApiMiddleware(
            RequestDelegate next,
            IMediatRPipelineOrchestratorFactory pipelineOrchestratorFactory,
            ICancellationTokenHolder cancellationTokenHolder,
            ILogger<HttpApiMiddleware> logger,
            IExceptionFlow exceptionFlow )
        {
            this.next = next;
            this.pipelineOrchestratorFactory = pipelineOrchestratorFactory;
            this.cancellationTokenHolder = cancellationTokenHolder;
            this.logger = logger;
            this.exceptionFlow = exceptionFlow;
        }

        public async Task InvokeAsync(
            HttpContext context,
            IMediator mediator,
            IRouteTable routeTable )
        {
            this.logger
                .LogInformation( "Handling Request: {Path}",
                                 context.Request
                                        .Path
                                        .ToString() );

            var routeItem = routeTable
               .GetRequest( context.Request
                                   .Path,
                            context.Request
                                   .Method
                                   .ToUpperInvariant() );

            if ( routeItem == null )
            {
                context.Response.StatusCode = 404;
                return;
            }

            this.logger
                .LogInformation( "Executing Route: [ {Method} ] {Route} for {Request}",
                                 routeItem.Method
                                          .ToString(),
                                 routeItem.Route,
                                 routeItem.RequestType
                                          .Name );

            var json = await context.Request
                                    .ReadToEndAsync();
            var requestParameters = new Dictionary<string, object>();
            routeItem.RequestParameters
                    ?.CopyTo( requestParameters,
                              true );

            if ( routeItem.HasRouteParameters )
            {
                routeTable
                   .ExtrapolateTemplateValues( context.Request
                                                      .Path,
                                               routeItem )
                   .ForEach( routeParam =>
                    {
                        if ( !requestParameters.ContainsKey( routeParam.Key ) )
                        {
                            requestParameters.Add( routeParam.Key,
                                                   routeParam.Value );
                        }
                    } );
            }

            // here we want to be copying the query into the request parameters
            if ( context.Request
                        .Query
                        .Count > 0 )
            {
                foreach ( var (key, value) in context.Request
                                                     .Query )
                {
                    if ( !requestParameters.ContainsKey( key ) )
                    {
                        requestParameters.Add( key,
                                               value.ToString() );
                    }
                }
            }

            try
            {
                var mediatRPipelineExecutor = this.pipelineOrchestratorFactory
                                                  .Create( routeItem.RequestType,
                                                           mediator );

                var response = await mediatRPipelineExecutor.SendAsync( json,
                                                                        requestParameters,
                                                                        routeItem.DataObjectKey,
                                                                        this.cancellationTokenHolder
                                                                            .GetCancellationToken() );

                if ( response == null )
                {
                    await context.Response
                                 .WriteStatusCodeAsync( 204, null );
                    return;
                }

                if ( routeItem.WrapResponseDelegate != null )
                {
                    response = routeItem.WrapResponseDelegate( response );
                }

                await context.Response.WriteSuccessResponseAsync( response );
            }
catch ( Exception ex )
{
    var exceptionWriter = this.exceptionFlow
                              .GetWriter( ex );
    var canWrite = exceptionWriter?.CanWrite( ex );

    if ( canWrite.HasValue
      && canWrite.Value )
    {
        exceptionWriter.WriteException( ex,
                                        context );
    }
    else
    {
        await context.Response
                     .WriteStatusCodeAsync( 500,
                                            new
                                            {
                                                success = false,
                                                message = ex.Message
                                            } );
    }
}
        }
    }
}