using VehicleServiceCollectionSdk;
using VehicleServiceCollectionSdk.Config;

var builder = WebApplication.CreateBuilder(args);

// Register SDK client
builder.Services.AddSingleton<VehicleServiceCollectionSdkClient>(sp =>
{
    var baseUrl = builder.Configuration["VehicleService:BaseUrl"] ?? "http://localhost:3000";
    var client = new VehicleServiceCollectionSdkClient(new VehicleServiceCollectionSdkConfig());
    client.SetBaseUrl(baseUrl);
    return client;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Vehicle Service API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
