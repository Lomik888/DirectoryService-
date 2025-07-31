using DirectoryService.Application.DependencyInjection;
using DirectoryService.Infrastructure.DependencyInjection;
using DirectoryService.Presentation.Middlewares;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

var seqConnection = builder.Configuration["ConnectionStrings:Seq"] ??
                    throw new NullReferenceException("Missing SEQ connection string");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
    .Enrich.WithThreadId()
    .Destructure.ToMaximumDepth(4)
    .WriteTo.Console(LogEventLevel.Error)
    .WriteTo.Seq(seqConnection, LogEventLevel.Information)
    .Enrich.WithProperty("Application", "DS")
    .CreateLogger();

builder.Services.AddSerilog();

builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    string basePath = AppContext.BaseDirectory;

    string xmlPath = Path.Combine(basePath, "DSApi.xml");
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();