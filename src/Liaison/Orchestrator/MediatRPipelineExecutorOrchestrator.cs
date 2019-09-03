using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Liaison.AspNetCore.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Liaison.Orchestrator
{
    internal class MediatRPipelineExecutorOrchestrator : IMediatRPipelineExecutorOrchestrator
    {
        private readonly MethodInfo boundSendMethod;
        private readonly Type requestType;
        private readonly IMediator mediator;
        private readonly ILogger<MediatRPipelineExecutorOrchestrator> logger;
        
        public MediatRPipelineExecutorOrchestrator(
            MethodInfo boundSendMethod,
            Type requestType,
            IMediator mediator,
            ILogger<MediatRPipelineExecutorOrchestrator> logger)
        {
            this.boundSendMethod = boundSendMethod;
            this.requestType = requestType;
            this.mediator = mediator;
            this.logger = logger;
        }
        
        public async Task<object> SendAsync(
            string rawRequestData,
            IDictionary<string, object> requestParams = null,
            string dataObjectKey = null,
            CancellationToken cancellationToken = default(CancellationToken) )
        {
            this.logger.LogDebug( "Constructing Request" );
            var request = this.ConstructRequest( rawRequestData, 
                                                 requestParams,
                                                 dataObjectKey );

            this.logger.LogDebug( "Invoking MediatR for built Request" );
            var task = (Task) this.boundSendMethod.Invoke( this.mediator,
                                                           new[]
                                                           {
                                                               request,
                                                               cancellationToken
                                                           } );

            await task;
            var propertyInfo = task.GetType()
                                   .GetProperty( "Result" );
            
            this.logger.LogDebug( "Returning MediatR response" );
            return propertyInfo?.GetValue( task );
        }

        public async Task<TReturn> SendAsync< TReturn >(
            string rawRequestData,
            IDictionary<string, object> requestParams = null,
            string dataObjectKey = null,
            CancellationToken cancellationToken = default(CancellationToken) )
        {
            var response = await this.SendAsync( rawRequestData,
                                                 requestParams,
                                                 dataObjectKey,
                                                 cancellationToken );

            if ( response == null )
            {
                return default;
            }

            return (TReturn) response;
        }

        private object ConstructRequest(
            string rawRequestData,
            IDictionary<string, object> requestParams,
            string dataObjectKey = null)
        {
            // handles if there is no string data in the request body
            var json = rawRequestData.HasValue() 
                           ? rawRequestData 
                           : "{}";
            
            object request;
            var requestObject = JObject.Parse( json );
            
            if ( !string.IsNullOrWhiteSpace( dataObjectKey ) 
              && requestObject.ContainsKey( dataObjectKey ) )
            {
                requestObject = (JObject) requestObject[ dataObjectKey ];
            }
            
            if ( requestParams != null 
              && requestParams.Count > 0 )
            {   
                foreach ( var param in requestParams )
                {
                    requestObject[ param.Key ] = JToken.FromObject( param.Value );
                }
                
                request = requestObject.ToObject( this.requestType );
            }
            else
            {
                request = JsonConvert.DeserializeObject( json, this.requestType );
            }

            // if after all of the above the request is still null.. try activator
            if ( request == null )
            {
                try
                {
                    request = Activator.CreateInstance( this.requestType );
                    
                }
                catch ( Exception e )
                {
                    this.logger
                        .LogCritical( e, 
                                     "Unable to create a request to send to MediatR, request type {RequestType}", 
                                     this.requestType.Name );
                    throw e;
                }
            }
            
            return request;
        }
    }
}