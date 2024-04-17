using CallCredit.API.Interfaces;
using FinancialService.API.Services;
using FinancialService.Data;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Configure logging
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();
        builder.Logging.AddEventSourceLogger();
        builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

        // Add services to the container.
        builder.Services.AddControllers();

        builder.Services.AddDbContext<FinancialContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("FinancialDb")
            ));

        builder.Services.AddScoped<IAccountsService, AccountsService>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();
        Seed(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    private static void Seed(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<FinancialContext>();
            FinancialService.Data.Seed.SeedData(context);
        }
    }
}