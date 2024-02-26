using AutoMapper;

using Common.Utilities;

using Server.Entities;
using Server.Services;
using Server.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
var config = new MapperConfiguration(
    cfg =>
    {
        cfg.AddProfile<CommonMappingProfile>();
        cfg.AddProfile<ServerMappingProfile>();
    });
builder.Services.AddSingleton<ConnectionResources>();

var app = builder.Build();

app.MapGrpcService<ConnectionService>();
app.MapGrpcService<GameService>();
app.MapGrpcService<LobbyService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();