using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PinoyCleanArch;
using PinoyTodo.Application.Common.Interfaces;
using PinoyTodo.Infrastructure.Persistence;
using PinoyTodo.Infrastructure.Persistence.Repositories;

namespace PinoyTodo.Infrastructure;

public static partial class DiRegister
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EventStoreDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("EventStoreDbContext");
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<ITaskRepository, TaskRepository>();

        services.AddInfrastructure();
        return services;
    }
}