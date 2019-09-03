using System;
using Liaison.AspNetCore;
using Liaison.Dsl;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Liaison
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Allows for the creation of HTTP APIs bound to MediatR Requests
        /// </summary>
        /// <param name="app">The ASP.NET Core ApplicationBuilder</param>
        /// <param name="liaisonAppBuilder">A method that allows configuration of the Liaison Routes</param>
        /// <returns>The same instance of IApplicationBuilder</returns>
        public static IApplicationBuilder UseLiaison(
            this IApplicationBuilder app,
            Action<ILiaisonApp> liaisonAppBuilder )
        {
            var routeTable = app.ApplicationServices
                                .GetRequiredService<IRouteTable>();

            var liaisonApp = new LiaisonApp( routeTable, app );

            liaisonAppBuilder( liaisonApp );

            liaisonApp.Build();
            
            return app;
        }
    }

    
}