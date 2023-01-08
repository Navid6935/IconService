using Application.Interfaces.General;
using Application.Services.General;
using Domain.Interfaces;
using Infra.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.IOC;

public static class DependencyInjection
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        

        services.AddScoped(typeof(ILogService<>), typeof(LogService<>));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddHttpClient();

    }
}