using Microsoft.Extensions.DependencyInjection;
using Spectre.Cli;

namespace Spectre.Cli.Extensions.DependencyInjection
{
    internal class DependencyInjectionResolver : ITypeResolver, IDisposable
    {
        private ServiceProvider ServiceProvider { get; }

        internal DependencyInjectionResolver(ServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Dispose()
        {
            ServiceProvider.Dispose();
        }

        public object? Resolve(Type? type)
        {
            if (type is null) {
                return null;
            }

            return ServiceProvider.GetService(type) ?? Activator.CreateInstance(type);
        }
    }
}