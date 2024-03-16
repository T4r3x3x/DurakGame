using Server.Entities;
using Server.Services;
using Server.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddSingleton(MappingProfilesRegister.GetMapper());
builder.Services.AddSingleton<ConnectionResources>();

var app = builder.Build();

app.MapGrpcService<ConnectionService>();
app.MapGrpcService<GameService>();
app.MapGrpcService<LobbyService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();