using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using Liaison.Tests.TestClasses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace Liaison.Tests
{
    public class Given_TestBindRequest : AppSetupBase
    {
        [Fact]
        public void When_Mapping_Api_Route_Should_Execute_Handler_And_Bind_Request_Body()
        {
            var responseName = string.Empty;
            var app = this.ApplicationBuilderFactory( services =>
            {
                services.AddMediatR( this.GetType() );
                services.AddTransient<ITestService>( ctx => new TestService( nameData => responseName = nameData ) );
            } );

            app.UseLiaison( liaisonApp => liaisonApp.Route("test").Get<TestBindRequest>( ) );

            var requestName = "Jeff";
            var jsonBytes = Encoding.UTF8.GetBytes( JsonConvert.SerializeObject( new { name = requestName } ) );

            var defaultHttpContext = new DefaultHttpContext
                                     {
                                         Request =
                                         {
                                             Path = "/api/test",
                                             Method = "GET",
                                             Body = new MemoryStream(jsonBytes, 0, jsonBytes.Length, false)
                                         },
                                         RequestServices = app.ApplicationServices
                                     };

            var task = app.Build()( defaultHttpContext );

            task.Wait();

            responseName.Should().Be( requestName, "the name should have been pulled from the request stream" );
        }
        
        [Fact]
        public void When_Mapping_Api_Route_Should_Execute_Handler_And_Bind_Route_Parameters()
        {
            var responseName = string.Empty;
            var app = this.ApplicationBuilderFactory( services =>
            {
                services.AddMediatR( this.GetType() );
                services.AddTransient<ITestService>( ctx => new TestService( nameData => responseName = nameData ) );
            } );

            app.UseLiaison( liaisonApp => liaisonApp.Route("test").Get<TestBindRequest>( "{name}" ) );

            var requestName = "Jeff";

            var defaultHttpContext = new DefaultHttpContext
                                     {
                                         Request =
                                         {
                                             Path = $"/api/test/{requestName}",
                                             Method = "GET"
                                         },
                                         RequestServices = app.ApplicationServices
                                     };

            var task = app.Build()( defaultHttpContext );

            task.Wait();

            responseName.Should().Be( requestName, "the name should have been pulled from the request stream" );
        }
        
        [Fact]
        public void When_Mapping_Api_Route_Should_Execute_Handler_And_Bind_Query_String()
        {
            var responseName = string.Empty;
            var app = this.ApplicationBuilderFactory( services =>
            {
                services.AddMediatR( this.GetType() );
                services.AddTransient<ITestService>( ctx => new TestService( nameData => responseName = nameData ) );
            } );

            app.UseLiaison( liaisonApp => liaisonApp.Route("test").Get<TestBindRequest>( ) );

            var requestName = "Jeff";

            var defaultHttpContext = new DefaultHttpContext()
                                     {
                                         Request =
                                         {
                                             Path = $"/api/test",
                                             Method = "GET",
                                             Query = new QueryCollection(QueryHelpers.ParseQuery( $"?name={requestName}" ))
                                         },
                                         RequestServices = app.ApplicationServices
                                     };
            
            var task = app.Build()( defaultHttpContext );

            task.Wait();

            responseName.Should().Be( requestName, "the name should have been pulled from the request stream" );
        }
        
        [Fact]
        public void When_Mapping_Api_Route_Should_Execute_Handler_And_Bind_Route_Parameters_But_Will_Be_Overidden()
        {
            var responseName = string.Empty;
            var app = this.ApplicationBuilderFactory( services =>
            {
                services.AddMediatR( this.GetType() );
                services.AddTransient<ITestService>( ctx => new TestService( nameData => responseName = nameData ) );
            } );

            var overridingRequestName = "Michael";
            app.UseLiaison( liaisonApp => liaisonApp.Route("test").Get<TestBindRequest>( "{name}", requestParams: new Dictionary<string, object> { {"name", overridingRequestName} }) );

            var requestName = "Jeff";

            var defaultHttpContext = new DefaultHttpContext
                                     {
                                         Request =
                                         {
                                             Path = $"/api/test/{requestName}",
                                             Method = "GET"
                                         },
                                         RequestServices = app.ApplicationServices
                                     };

            var task = app.Build()( defaultHttpContext );

            task.Wait();

            responseName.Should().Be( overridingRequestName, "the name should have been pulled from the request stream" );
        }
    }
}