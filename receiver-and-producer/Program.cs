using Azure.Messaging.ServiceBus;
using receiver_and_producer.Queue.AzureServiceBusSender;
using receiver_and_producer.Services.GenericDispatcherService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ServiceBusClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var connectionString = config.GetConnectionString("ServiceBus:ConnectionString");

    return new ServiceBusClient(connectionString);
});

builder.Services.AddScoped<IAzureServiceBusSender, AzureServiceBusSender>();
builder.Services.AddScoped<IGenericDispatcherService, GenericDispatcherService>();

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
