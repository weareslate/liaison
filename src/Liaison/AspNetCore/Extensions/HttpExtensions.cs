using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Liaison.AspNetCore.Extensions
{
    public static class HttpExtensions
    {
        public static Task WriteJsonAsync< T >(
            this HttpResponse response,
            T responseData )
        {
            if ( responseData == null )
            {
                return Task.CompletedTask;
            }
            
            var dataString = JsonConvert.SerializeObject( responseData,
                                                          Formatting.None,
                                                          new JsonSerializerSettings
                                                          {
                                                              ContractResolver =
                                                                  new CamelCasePropertyNamesContractResolver()
                                                          } );
            var buffer = Encoding.UTF8.GetBytes( dataString );
            return response.Body.WriteAsync( buffer, 0, buffer.Length );
        }

        public static Task<string> ReadToEndAsync(
            this HttpRequest request )
        {
            using ( var sr = new StreamReader( request.Body ) )
            {
                return sr.ReadToEndAsync();
            }
        }

        public static Task WriteUnauthorisedResponseAsync(
            this HttpResponse response )
        {
            response.StatusCode = 401;
            return response.WriteJsonAsync( new
                                            {
                                                _unauthorised = new
                                                                {
                                                                    Message = "You're unauthorised to see this resource"
                                                                }
                                            } );
        }

        public static Task WriteSuccessResponseAsync(
            this HttpResponse response,
            object content )
        {
            response.StatusCode = 200;
            return response.WriteJsonAsync( content );
        }

        public static Task WriteStatusCodeAsync(
            this HttpResponse response,
            int statusCode,
            object content )
        {
            response.StatusCode = statusCode;
            return response.WriteJsonAsync( content );
        }

        public static Task WriteBadResponseAsync(
            this HttpResponse response,
            object errors )
        {
            response.StatusCode = 400;
            return response.WriteJsonAsync( new
                                            {
                                                success = false,
                                                errors
                                            } );
        }
        
        public static bool IsVerifiedApiRoute(
            this HttpRequest request )
        {
            var routePaths = request.GetRouteParts();
            return routePaths.FirstOrDefault() == "api";
        }

        public static IEnumerable<string> GetRouteParts(
            this HttpRequest request )
        {
            return request.Path.Value.Split( "/", StringSplitOptions.RemoveEmptyEntries );
        }
    }
}