using CourseManagementProject.Application;
using CourseManagementProject.Infrastructure;
using CourseManagementProject.Infrastructure.Implementation;

namespace CourseManagementProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services
                .AddApplication()
                .AddInfrastructure(builder.Configuration)
                .AddControllers();
            

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllers();

            app.Run();
        }
    }
}