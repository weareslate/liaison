using System;
using System.Collections.Generic;
using Liaison.AspNetCore;

namespace Liaison.Dsl
{
    public interface ILiaisonApp
    {
        /// <summary>
        /// Starts the creation of Stateless Operations on a Web Resource
        /// </summary>
        /// <param name="uri">The Web Resource Name, such as User, Role... </param>
        /// <param name="dataObjectKey">If the data to bind to the MediatR request is embedded, use this to specify the key name in the JSON to get the data from</param>
        /// <param name="pipeline">If you want this specific route to be subject to any ASP.NET Core Middleware</param>
        /// <param name="requestParams">Request Parameters are like constants that can be used to inject data into MediatR Request objects. These will override any data from the HTTP Request</param>
        /// <param name="wrapDelegate">Wrap Response Delegate is a function that you can use to modify the output, so if you nest your response data, this will allow you to do that</param>
        /// <returns>Liaison Route object that allows the specification of Stateless Operations</returns>
        ILiaisonRoute Route(string uri, 
                            string dataObjectKey = null,
                            Type[] pipeline = null,
                            Dictionary<string, object> requestParams = null,
                            WrapResponseDelegate wrapDelegate = null);
    }
}