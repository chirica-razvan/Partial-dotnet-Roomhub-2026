using System.Reflection;

namespace RoomHub.Api.Common.Cqrs;

public static class HandlerRegistrationExtensions
{
    // Scans the assembly once, at startup. No MediatR.
    // shortcut: no TransactionalCommandHandler yet; added together with EF Core + IUnitOfWork (level C).
    public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly assembly)
    {
        var implementations = assembly.GetTypes()
            .Where(type => type is { IsClass: true, IsAbstract: false, IsGenericTypeDefinition: false });

        foreach (var implementation in implementations)
        {
            var contracts = implementation.GetInterfaces()
                .Where(contract => contract.IsGenericType
                    && contract.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));

            foreach (var contract in contracts)
                services.AddScoped(contract, implementation);
        }

        return services;
    }
}
