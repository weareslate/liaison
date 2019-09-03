using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Liaison.ExceptionHandling;
using Liaison.Tests.TestClasses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Liaison.Tests
{
    public class Given_TestErrorRequest : AppSetupBase
    {
        [Fact]
        public void When_Mapping_Api_Route_And_Throwing_ErroneousError_Should_Return_500()
        {
            var app = this.ApplicationBuilderFactory( services =>
            {
                services.AddMediatR( this.GetType() );
                services.AddTransient<IErrorChoser>( ctx => new ErrorChoser( new NotImplementedException() ) );
            } );

            app.UseLiaison( liaisonApp => liaisonApp.Route("test").Get<TestErrorRequest>() );

            var defaultHttpContext = new DefaultHttpContext
                                     {
                                         Request =
                                         {
                                             Path = "/api/test",
                                             Method = "GET"
                                         },
                                         RequestServices = app.ApplicationServices,
                                         // need this in order to read the response stream later
                                         Response = { Body = new MemoryStream()}
                                     };

            var task = app.Build()( defaultHttpContext );

            task.Wait();

            var response = defaultHttpContext.Response;
            response.StatusCode.Should().Be( 500, "erroneous errors get 500" );
            var json = response.Body.GetJson();
            json.ContainsKey( "success" ).Should().BeTrue(  );
            json.ContainsKey( "message" ).Should().BeTrue(  );
        }
        
        [Fact]
        public void When_Mapping_Api_Route_And_Throwing_ValidationError_Should_Return_400_With_Correct_Response()
        {
            var responseEx = (Exception) null;
            var exToThrow = new ValidationException(new List<ValidationFailure>() { new ValidationFailure( "name", "done broke" )});
            var app = this.ApplicationBuilderFactory( services =>
            {
                services.AddMediatR( this.GetType() );
                services.AddTransient<IErrorChoser>( ctx => new ErrorChoser( exToThrow ) );
                services.AddTransient<IExceptionWriter>( ctx => new ValidationExceptionWriter( ex => responseEx = ex ) );
            } );

            app.UseLiaison( liaisonApp => liaisonApp.Route("test").Get<TestErrorRequest>() );

            var defaultHttpContext = new DefaultHttpContext
                                     {
                                         Request =
                                         {
                                             Path = "/api/test",
                                             Method = "GET"
                                         },
                                         RequestServices = app.ApplicationServices,
                                         // need this in order to read the response stream later
                                         Response = { Body = new MemoryStream()}
                                     };

            var task = app.Build()( defaultHttpContext );

            task.Wait();

            var response = defaultHttpContext.Response;
            response.StatusCode.Should().Be( 400, "validation exceptions should throw 400 inline with exception writer" );
            responseEx.Should().NotBeNull();
            responseEx.Should().Be( exToThrow );
        }
    }
}