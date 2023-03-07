using App.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<KeyDbSettings>(builder.Configuration.GetRequiredSection("Storage"));
builder.Services.AddSingleton<IStorage, KeyDbStorage>();

var app = builder.Build();

app.MapPost("/queries", (string query, IStorage storage) => storage.AddAsync(query));
app.MapGet("/queries/top", async (IStorage storage) => await storage.GetTopQueries());

app.Run();