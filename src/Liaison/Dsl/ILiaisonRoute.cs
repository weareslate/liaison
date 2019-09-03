using System;
using System.Collections.Generic;
using Liaison.AspNetCore;

namespace Liaison.Dsl
{
    public interface ILiaisonRoute
    {
        /// <summary>
        /// HTTP GET Stateless Operation
        /// </summary>
        /// <param name="template">Templates are Route Parameters or more targeted URI to resources</param>
        /// <param name="pipeline">If you want this specific route to be subject to any ASP.NET Core Middleware</param>
        /// <param name="dataObjectKey">If the data to bind to the MediatR request is embedded, use this to specify the key name in the JSON to get the data from</param>
        /// <param name="requestParams">Request Parameters are like constants that can be used to inject data into MediatR Request objects. These will override any data from the HTTP Request</param>
        /// <param name="wrapDelegate">Wrap Response Delegate is a function that you can use to modify the output, so if you nest your response data, this will allow you to do that</param>
        /// <typeparam name="TRequest">The MediatR request that should be bound to the HTTP Request</typeparam>
        /// <returns>Liaison Route object that allows the specification of Stateless Operations to the current Route</returns>
        ILiaisonRoute Get< TRequest >(
            string template = null,
            Type[] pipeline = null,
            string dataObjectKey = null,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null );
        
        /// <summary>
        /// HTTP POST Stateless Operation
        /// </summary>
        /// <param name="template">Templates are Route Parameters or more targeted URI to resources</param>
        /// <param name="pipeline">If you want this specific route to be subject to any ASP.NET Core Middleware</param>
        /// <param name="dataObjectKey">If the data to bind to the MediatR request is embedded, use this to specify the key name in the JSON to get the data from</param>
        /// <param name="requestParams">Request Parameters are like constants that can be used to inject data into MediatR Request objects. These will override any data from the HTTP Request</param>
        /// <param name="wrapDelegate">Wrap Response Delegate is a function that you can use to modify the output, so if you nest your response data, this will allow you to do that</param>
        /// <typeparam name="TRequest">The MediatR request that should be bound to the HTTP Request</typeparam>
        /// <returns>Liaison Route object that allows the specification of Stateless Operations to the current Route</returns>
        /// <returns></returns>
        ILiaisonRoute Post< TRequest >(
            string template = null,
            Type[] pipeline = null,
            string dataObjectKey = null,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null );
        
        /// <summary>
        /// HTTP PUT Stateless Operation
        /// </summary>
        /// <param name="template">Templates are Route Parameters or more targeted URI to resources</param>
        /// <param name="pipeline">If you want this specific route to be subject to any ASP.NET Core Middleware</param>
        /// <param name="dataObjectKey">If the data to bind to the MediatR request is embedded, use this to specify the key name in the JSON to get the data from</param>
        /// <param name="requestParams">Request Parameters are like constants that can be used to inject data into MediatR Request objects. These will override any data from the HTTP Request</param>
        /// <param name="wrapDelegate">Wrap Response Delegate is a function that you can use to modify the output, so if you nest your response data, this will allow you to do that</param>
        /// <typeparam name="TRequest">The MediatR request that should be bound to the HTTP Request</typeparam>
        /// <returns>Liaison Route object that allows the specification of Stateless Operations to the current Route</returns>
        ILiaisonRoute Put< TRequest >(
            string template = "{id}",
            Type[] pipeline = null,
            string dataObjectKey = null,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null );
        
        /// <summary>
        /// HTTP PATCH Stateless Operation
        /// </summary>
        /// <param name="template">Templates are Route Parameters or more targeted URI to resources</param>
        /// <param name="pipeline">If you want this specific route to be subject to any ASP.NET Core Middleware</param>
        /// <param name="dataObjectKey">If the data to bind to the MediatR request is embedded, use this to specify the key name in the JSON to get the data from</param>
        /// <param name="requestParams">Request Parameters are like constants that can be used to inject data into MediatR Request objects. These will override any data from the HTTP Request</param>
        /// <param name="wrapDelegate">Wrap Response Delegate is a function that you can use to modify the output, so if you nest your response data, this will allow you to do that</param>
        /// <typeparam name="TRequest">The MediatR request that should be bound to the HTTP Request</typeparam>
        /// <returns>Liaison Route object that allows the specification of Stateless Operations to the current Route</returns>
        ILiaisonRoute Patch< TRequest >(
            string template = "{id}",
            Type[] pipeline = null,
            string dataObjectKey = null,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null );
        
        /// <summary>
        /// HTTP DELETE Stateless Operation
        /// </summary>
        /// <param name="template">Templates are Route Parameters or more targeted URI to resources</param>
        /// <param name="pipeline">If you want this specific route to be subject to any ASP.NET Core Middleware</param>
        /// <param name="dataObjectKey">If the data to bind to the MediatR request is embedded, use this to specify the key name in the JSON to get the data from</param>
        /// <param name="requestParams">Request Parameters are like constants that can be used to inject data into MediatR Request objects. These will override any data from the HTTP Request</param>
        /// <param name="wrapDelegate">Wrap Response Delegate is a function that you can use to modify the output, so if you nest your response data, this will allow you to do that</param>
        /// <typeparam name="TRequest">The MediatR request that should be bound to the HTTP Request</typeparam>
        /// <returns>Liaison Route object that allows the specification of Stateless Operations to the current Route</returns>
        ILiaisonRoute Delete< TRequest >(
            string template = "{id}",
            Type[] pipeline = null,
            string dataObjectKey = null,
            Dictionary<string, object> requestParams = null,
            WrapResponseDelegate wrapDelegate = null );
    }
}