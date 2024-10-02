using System.Net;
using Kroker.Core;
using Kroker.Core.Protocol.Stomp;
using Microsoft.AspNetCore.Connections;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    options.ListenLocalhost(61613, listenOptions => { listenOptions.UseConnectionHandler<StompConnectionHandler>(); });
    options.ListenLocalhost(5000);
    options.ListenLocalhost(5001, listenOptions => listenOptions.UseHttps());
});
builder.Services.AddLogging();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();