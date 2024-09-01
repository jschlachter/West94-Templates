using ConsoleWithSpectre.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Spectre.Cli;
using Spectre.Cli.Extensions.DependencyInjection;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.File("logs/host-.log",
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 1_000_000,
        rollOnFileSizeLimit: true,
        flushToDiskInterval: TimeSpan.FromSeconds(1),
        shared: true)
    .CreateBootstrapLogger();

try 
{
    Log.Information("Starting host");

    var host = Host.CreateApplicationBuilder(args);

    host.Services.AddSingleton<HelloCommand>();

    host.Services.AddSerilog((ctx, loggerConfiguration) => 
        loggerConfiguration
            .ReadFrom.Configuration(host.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.File("logs/application-.log",
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 1_000_000,
                rollOnFileSizeLimit: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1),
                shared: true)
    );

    using var registrar = new DependencyInjectionRegistrar(host.Services);
    var app = new CommandApp(registrar);

    app.Configure((config) => {
        config.AddCommand<HelloCommand>("hello")
            .WithAlias("hola")
            .WithDescription("Say hello")
            .WithExample(["hello", "Phil"])
            .WithExample(["hello", "Phil", "--count", "4"]);
    });

    app.Run(args);
}
catch (Exception ex) when (ex is not HostAbortedException) {
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally {
    Log.CloseAndFlush();
}