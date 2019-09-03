using System;
using System.IO;
using FluentAssertions;
using Liaison.AspNetCore;
using Liaison.Tests.TestClasses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using HttpMethods = Liaison.AspNetCore.HttpMethods;

namespace Liaison.Tests
{
    public class Given_TestRequest : AppSetupBase
    {
        // this gets code coverage up because of extension methods
        [Theory( DisplayName = "HTTP Method Tests using Extension Methods" )]
        [ClassData( typeof(HttpMethodsTheoryData) )]
        public void When_Mapping_Api_Route_With_ExtensionMethods_Should_Add_to_Route_Table(
            object httpMethod )
        {
            var actualMethod = (HttpMethods) httpMethod;

            var app = this.ApplicationBuilderFactory( null );

            var baseUri = "/api/test";
            var baseRoute = "/api/test";
            switch ( actualMethod )
            {
                case HttpMethods.GET:
                    app.UseLiaison( liaisonApp => liaisonApp.Route( "test" ).Get<TestRequest>() );
                    break;
                case HttpMethods.POST:
                    app.UseLiaison( liaisonApp => liaisonApp.Route( "test" ).Post<TestRequest>() );
                    break;
                case HttpMethods.PATCH:
                    baseUri = $"{baseUri}/23";
                    baseRoute = $"{baseRoute}/{{id}}";
                    app.UseLiaison( liaisonApp => liaisonApp.Route( "test" ).Patch<TestRequest>() );
                    break;
                case HttpMethods.PUT:
                    baseUri = $"{baseUri}/23";
                    baseRoute = $"{baseRoute}/{{id}}";
                    app.UseLiaison( liaisonApp => liaisonApp.Route( "test" ).Put<TestRequest>() );
                    break;
                case HttpMethods.DELETE:
                    baseUri = $"{baseUri}/23";
                    baseRoute = $"{baseRoute}/{{id}}";
                    app.UseLiaison( liaisonApp => liaisonApp.Route( "test" ).Delete<TestRequest>() );
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var routeTable = app.ApplicationServices.GetRequiredService<IRouteTable>();

            var routeItem = routeTable.GetRequest( baseUri, actualMethod.ToString() );

            routeItem.Should().NotBeNull( "the Route Item needs to be setup" );
            routeItem.Method.Should().Be( actualMethod );
            routeItem.Route.Should().Be( baseRoute );
            routeItem.RequestType.Should().Be<TestRequest>( "the correct request should be linked to the route" );
        }

        [Fact]
        public void When_Mapping_Api_Route_With_Route_Parameters_Should_Add_to_Route_Table()
        {
            var app = this.ApplicationBuilderFactory( null );

            app.UseLiaison( liaisonApp => liaisonApp.Route( "test" ).Get<TestRequest>( "{name}" ) );

            var routeTable = app.ApplicationServices.GetRequiredService<IRouteTable>();

            var routeItem = routeTable.GetRequest( "/api/test/Jeff", "GET" );

            routeItem.Should().NotBeNull( "the Route Item needs to be setup" );
            routeItem.Method.Should().Be( HttpMethods.GET );
            routeItem.Route.Should().Be( "/api/test/{name}" );
            routeItem.RequestType.Should().Be<TestRequest>( "the correct request should be linked to the route" );
            routeItem.HasRouteParameters.Should().BeTrue();
        }

        [Fact]
        public void When_Mapping_Api_Route_Should_Execute_Handler()
        {
            var mockTestService = new Mock<ITestService>();
            var app = this.ApplicationBuilderFactory( services =>
            {
                services.AddMediatR( this.GetType() );
                services.AddTransient<ITestService>( ctx => mockTestService.Object );
            } );

            app.UseLiaison( liaisonApp => liaisonApp.Route( "test" ).Get<TestRequest>() );

            var defaultHttpContext = new DefaultHttpContext
                                     {
                                         Request =
                                         {
                                             Path = "/api/test",
                                             Method = "GET"
                                         },
                                         RequestServices = app.ApplicationServices
                                     };

            var task = app.Build()( defaultHttpContext );

            task.Wait();

            mockTestService.Verify( x => x.Callme(), Times.AtMostOnce );
        }

        [Fact]
        public void When_Mapping_Api_Route_Should_Execute_Handler_Wrapping_The_Response()
        {
            var mockTestService = new Mock<ITestService>();
            var app = this.ApplicationBuilderFactory( services =>
            {
                services.AddMediatR( this.GetType() );
                services.AddTransient<ITestService>( ctx => mockTestService.Object );
            } );

            object ResponseWrapper(
                object data ) =>
                new { myData = data };

            app.UseLiaison( liaisonApp =>
                                liaisonApp.Route( "test" ).Get<TestRequest>( wrapDelegate: ResponseWrapper ) );

            var defaultHttpContext = new DefaultHttpContext
                                     {
                                         Request =
                                         {
                                             Path = "/api/test",
                                             Method = "GET"
                                         },
                                         RequestServices = app.ApplicationServices,
                                         Response = { Body = new MemoryStream() }
                                     };

            var task = app.Build()( defaultHttpContext );

            task.Wait();

            defaultHttpContext.Response
                              .Body
                              .GetJson()
                              .ContainsKey( "myData" )
                              .Should()
                              .BeTrue( "this proves it has been wrapped" );

            mockTestService.Verify( x => x.Callme(), Times.AtMostOnce );
        }

        private sealed class HttpMethodsTheoryData : TheoryData
        {
            public HttpMethodsTheoryData()
            {
                this.AddRow( HttpMethods.GET );
                this.AddRow( HttpMethods.PUT );
                this.AddRow( HttpMethods.POST );
                this.AddRow( HttpMethods.PATCH );
                this.AddRow( HttpMethods.DELETE );
            }
        }
    }
}