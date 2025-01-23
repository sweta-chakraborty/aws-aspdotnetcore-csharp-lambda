using Microsoft.EntityFrameworkCore;
using DemoLambdaFunction.Data;

namespace DemoLambdaFunction;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        
        services.AddDbContext<SurveyDbContext>(options =>
        {
            try
            {
                var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? throw new InvalidOperationException("POSTGRES_HOST environment variable is not set");
                var port = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? throw new InvalidOperationException("POSTGRES_PORT environment variable is not set");
                var database = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? throw new InvalidOperationException("POSTGRES_DB environment variable is not set");
                var username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? throw new InvalidOperationException("POSTGRES_USER environment variable is not set");
                var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? throw new InvalidOperationException("POSTGRES_PASSWORD environment variable is not set");

                var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};Include Error Detail=true;Trust Server Certificate=true";
                Console.WriteLine($"[Database] Attempting to connect to PostgreSQL at {host}:{port}/{database} as {username}");
                
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(3);
                    npgsqlOptions.CommandTimeout(30);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Database] Error configuring database context: {ex.Message}");
                throw;
            }
        });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseCors();
        app.UseStaticFiles();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
