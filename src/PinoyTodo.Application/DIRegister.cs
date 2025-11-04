using Microsoft.Extensions.DependencyInjection;
using PinoyCleanArch;

namespace PinoyTodo.Application;

public static class DIRegister
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddApplication(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}