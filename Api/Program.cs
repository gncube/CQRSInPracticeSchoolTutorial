using Application;
using Infrastructure;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    //.ConfigureAppConfiguration((context, config) =>
    //{
    //    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    //    .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    //    .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
    //})
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.AddApplication(context.Configuration);
        services.AddInfrastructure(context.Configuration);
    })
    .Build();

host.Run();
