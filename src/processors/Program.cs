using Processors;
using Processors.Workers;
using StackExchange.Redis;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(loggerFactory => loggerFactory.AddConsole())
    .ConfigureAppConfiguration((hostContext, configApp) =>
    {
        configApp.AddEnvironmentVariables();
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddStackExchangeRedisCache(o =>
        {
            o.Configuration = hostContext.Configuration["Redis"];
        });
        
        services
            .AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(hostContext.Configuration["Redis"]));

        //services.AddHostedService<Worker>();
        services.AddHostedService<ContactWorker>();
    })
    .Build();

host.Run();
