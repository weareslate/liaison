using System;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Liaison.Tests
{
    public abstract class AppSetupBase
    {
        protected readonly Func<Action<ServiceCollection>, ApplicationBuilder> ApplicationBuilderFactory;

        protected AppSetupBase()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            serviceCollection.AddLiaison();
            this.ApplicationBuilderFactory = serviceAction =>
            {
                serviceAction?.Invoke( serviceCollection );

                return new ApplicationBuilder( serviceCollection.BuildServiceProvider() );
            };
        }
    }
}