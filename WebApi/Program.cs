using Microsoft.EntityFrameworkCore;
using Negotiations;
using Negotiations.Infrastructure.Persistence;
using Serilog;
using Products;
using Products.Infrastructure.Persistence;
using Shared;
using Shared.Infrastructure.Middleware;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSharedModule(builder.Configuration);
builder.Services.AddProductsModule(builder.Configuration);
builder.Services.AddNegotiationsModule(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    var productDb = serviceProvider.GetRequiredService<ProductDbContext>();
    productDb.Database.Migrate();

    var negotiationDb = serviceProvider.GetRequiredService<NegotiationDbContext>();
    negotiationDb.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();
app.MapControllers();

app.Run();