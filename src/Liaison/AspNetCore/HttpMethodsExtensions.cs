using System;
using System.Linq;
using Liaison.AspNetCore.Extensions;

namespace Liaison.AspNetCore
{
    internal static class HttpMethodsExtensions
    {
        public static (bool, HttpMethods) ToHttpMethods(
            string httpMethod )
        {
            var normalisedHttpMethod = httpMethod.ToUpperInvariant();

            var knownHttpMethods = typeof(HttpMethods)
               .GetEnumNames();

            var exists = knownHttpMethods.FirstOrDefault( x => x == normalisedHttpMethod );
            if ( exists.IsNullOrWhiteSpace() )
            {
                return ( false, default(HttpMethods) );
            }
            
            var result = Enum.TryParse<HttpMethods>( normalisedHttpMethod, out var parsedMethod );

            return ( result, parsedMethod );
        }
    }
}