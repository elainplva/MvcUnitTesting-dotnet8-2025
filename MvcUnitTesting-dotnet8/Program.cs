using Microsoft.EntityFrameworkCore;
using MvcUnitTesting_dotnet8.Models;
using MvcUnitTesting_dotnet8.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tracker;
using Tracker.WebAPIClient;
using Microsoft.AspNetCore.Hosting;


namespace MvcUnitTesting_dotnet8
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<BookDbContext>(options =>
                options.UseSqlServer(connectionString));
            // Register the repository as a service
            builder.Services.AddScoped<IRepository<Book>, WorkingBookRepository<Book>>();

            // Register DbSeederTesting as a scoped service for database seeding
            builder.Services.AddScoped<DbSeederTesting>();

            var app = builder.Build();

            // Seed the database using scoped factory pattern
            using (var scope = app.Services.CreateScope())
            {
                var dbSeeder = scope.ServiceProvider.GetRequiredService<DbSeederTesting>();
                var hostEnvironment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                // Reinitialize with proper environment if needed
                dbSeeder.Seed();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            ActivityAPIClient.Track(StudentID: "s00250500", StudentName: "Elain Polakova",
                activityName: "Rad302 2026 Week 2 Lab 1", Task: "Implementing Production Repository Pattern");

            app.Run();
        }
    }
}
