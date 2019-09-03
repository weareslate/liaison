using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Liaison.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Liaison.Orchestrator
{
    internal class MediatRPipelineOrchestratorFactory : IMediatRPipelineOrchestratorFactory
    {
        private readonly ILogger<MediatRPipelineExecutorOrchestrator> orchestratorLogger;
        private readonly ILogger<MediatRPipelineOrchestratorFactory> logger;
        private readonly MethodInfo sendMethod;
        private readonly ConcurrentDictionary<Type, Type> requestCache;

        public MediatRPipelineOrchestratorFactory(
            ILogger<MediatRPipelineExecutorOrchestrator> orchestratorLogger,
            ILogger<MediatRPipelineOrchestratorFactory> logger )
        {
            this.sendMethod = typeof(Mediator)
                             .GetMethods()
                             .FirstOrDefault( x => x.Name == "Send" );

            if ( this.sendMethod == null )
            {
                logger.LogCritical( "MediatR has changed is Send method or API" );
                throw new InvalidOperationException( "MediatR has changed is Send method or API" );
            }

            this.orchestratorLogger = orchestratorLogger;
            this.logger = logger;

            // would be good to get this warmed up on the first run
            this.requestCache = new ConcurrentDictionary<Type, Type>();
        }

        public IMediatRPipelineExecutorOrchestrator Create< TRequest >(
            IMediator mediator )
        {
            var requestType = typeof(TRequest);

            this.TryLogDebug( "Creating MediatR Pipeline Executor for: {RequestType}",
                              requestType.Name );

            return this.Create( requestType, mediator );
        }

        public IMediatRPipelineExecutorOrchestrator Create(
            Type requestType,
            IMediator mediator )
        {
            var returnType = requestType.GetInterfaces()
                                        .First( x => x.IsClosingTypeOf( typeof(IRequest<>) )
                                                  || x == typeof(IRequest) )
                                        .GetGenericArguments().FirstOrDefault();

            var cachedReturnType = this.requestCache
                                       .GetOrAdd( requestType,
                                                  value => returnType ?? typeof(Unit) );

            var boundSendMethod = this.sendMethod
                                      .MakeGenericMethod( cachedReturnType );

            this.TryLogDebug( "Creating MediatR Pipeline Executor for: {RequestType}",
                              requestType.Name );

            return new MediatRPipelineExecutorOrchestrator( boundSendMethod,
                                                            requestType,
                                                            mediator,
                                                            this.orchestratorLogger );
        }

        private void TryLogDebug(
            string message,
            params object[] values )
        {
            this.logger.LogDebug( message, values );
        }
    }
}