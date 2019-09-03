![Nuget](https://img.shields.io/nuget/v/Liaison?style=for-the-badge)
![Nuget](https://img.shields.io/nuget/dt/Liaison?style=for-the-badge)
[![CircleCI](https://circleci.com/gh/weareslate/liaison.svg?style=svg)](https://circleci.com/gh/weareslate/liaison)

# Liaison

### Introduction

Jimmy Bogard (@jbogard) created MediatR (https://github.com/jbogard/MediatR), one of my favourite frameworks. Whilst using it I always found myself creating "thin" controllers.

So I thought, lets bypass controllers - so here is Liaison.

Simply put Liaison gives you tight integration between ASP.NET Core WebApi and MediatR.

I try and achieve this through idomatic use of ASP.NET Core Middleware and idomatic use of MediatR.

### Motivation

As discussed above, the use of "thin" controllers just seemed pointless to me. 

Also, MediatR is based on Behaviour Driven Design, so separating your requests means you can separate your actions. ASP.NET Controllers tie together your actions, in my opinion a technical separation of concerns. 

##### Unintended Side Affect

Having all the actions in a Controller has a knock on effect of each action being executed requiring all the services passed in the Controller constructor.

So if you had a User controller, and someone "Signed Up" usually and email is sent because of that. If someone wanted to get all the Users, the communication service will be called into existence... for what?

### Getting Started

You should install Liaison with NuGet:

```text
Install-Package Liaison
```

Or via the .NET Core command line interface:

```text
dotnet add package Liaison
```

Either commands, from Package Manager Console or .NET Core CLI, will download and install Liaison and all required dependencies.

#### Code

REST architecture specifies:

> to access and manipulate textual representations of Web resources by using a uniform and predefined set of stateless operations

So, a resource being User, and a predefined set of stateless operations `GET, POST, PUT, PATCH, DELETE`.

Think of Liaison in the same way:

```csharp

app.UseLiaison( liaison =>
{
    // /api/todos
    liaison.Route( "users" )
            //  GET {id}
           .Get<GetUserRequest>("{id}")
            //  GET
           .Get<GetAllUsersRequest>()
            //  POST
           .Post<SaveUserRequest>();
} );

```

See the Example app for and example implementation.

By using `liaison.Route( ROUTE )` we're specifying the resource to scope the stateless operations to.

When we specify `.Get<REQUEST>()`, Liaison will set up a route for `GET /api/users` and bind the HTTP request to the MediatR request (specified in the Generic Arguments).

The rest is down to how you configure MediatR.

Things to watch out for
 - You still need MediatR Dependencies Registered
 - Not everything is supported, see the state of play below
 
 ### State of Play
 
  - [X] Http Methods Supported (GET, POST, PATCH, PUT, DELETE)
  - [ ] Support for standard error responses
     - [ ] 415 Method Not Allowed (when resource exists but no method registered)
     - [X] 404 Resource doesn't exist
     - [X] 500 Something within the MediatR Pipeline goes wrong
 - [ ] Full Content Negotiation (Only supports JSON)
 - [ ] Swagger Support 
 - [X] Binding Support (Route Params, Query Params, Body)
 - [ ] Full Test Coverage (72% according to Rider Unit Test Coverage Tool)
 - [ ] Full Performance Test
 
 ### Exception Handling
 
 The setup of Liaison allows you to add Middleware at any point, this will create a new branch for the requests to traverse down. But at the end of all branches is the `HttpApiMiddleware.cs`.
 
 This middleware is responsible for finding/building the correct `mediator.Send<TRequest>()` method and executing it. Obviously in your MediatR "application" you can have application specific errors thrown which will then relate in some way to a HTTP Response and Error Message for the user.
 
 Liaison has a `try..catch` around the `Send<>()` method that should catch all "application" specific errors. In the `catch` block there is the `IExceptionFlow` which will select the correct `IExceptionWriter` from dependency injection.
 
 Simplistically:
 
 ```csharp
catch ( Exception ex )
{
    var exceptionWriter = this.exceptionFlow
                              .GetWriter( ex );

    var canWrite = exceptionWriter.CanWrite( ex );

    exceptionWriter.WriteException( ex,
                                    context );
}
```

This means that if you throw something like a validation error in your validation pipeline handler you can create `ValidationExceptionWriter` that will customise the response:

```csharp
class ValidationExceptionWriter : IExceptionWriter
{
    public bool CanWrite(
        Exception ex )
        => ex is ValidationException;

    public void WriteException(
        Exception ex,
        HttpContext context )
    {
        context.Response.StatusCode = 400;
        context.Response.WriteJsonAsync( new { errors = ( ex as ValidationException ).ValidationResult } );
    }
}
```

Then simply hook it up in DI:

```csharp
services.AddTransient<IExceptionWriter, ValidationExceptionWriter>();
```
 
 #### Enjoy Coding!
