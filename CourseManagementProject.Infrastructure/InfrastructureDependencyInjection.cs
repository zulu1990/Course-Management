using CourseManagementProject.Application.Services;
using CourseManagementProject.Infrastructure.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace CourseManagementProject.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ITokenGenerator, TokenGenerator>();
        services.AddScoped<IPasswordHandler, PasswordHandler>();

        return services;
    }
}
