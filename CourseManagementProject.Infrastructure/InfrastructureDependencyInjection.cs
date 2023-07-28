using CourseManagementProject.Application.Repositories;
using CourseManagementProject.Application.Services;
using CourseManagementProject.Infrastructure.Implementation;
using CourseManagementProject.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CourseManagementProject.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IPasswordHandler, PasswordHandler>();
        services.AddAuthorization(config);


        services.AddScoped<ITeachersRepository, TeachersRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();

        services.AddTransient<ExceptionMiddleware>();

        return services;
    }


    private static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<ITokenGenerator, TokenGenerator>();

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Secrets:JwtToken").Value)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        return services;
    }
}
