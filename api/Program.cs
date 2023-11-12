using api.Data;
using api.Models;
using api.Repositories;
using api.Services;
using api.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

// cors
services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .Build());
});

// ioc
services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase(databaseName: "Test"));

services.AddScoped<DataSeeder>();
services.AddScoped<IClientRepository, ClientRepository>();
services.AddScoped<IEmailRepository, EmailRepository>();
services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddScoped<IValidator<Client>, ClientValidator>();
services.AddTransient<ClientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/clients", async (IClientRepository clientRepository) =>
{
    return await clientRepository.Get();
})
.WithName("get clients");

app.MapPost("/client", async (IValidator<Client> validator, Client client, ClientService clientService) =>
{
    var validationResult = await validator.ValidateAsync(client);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    return Results.Ok(clientService.CreateClient(client));
})
.WithName("create client");

app.MapPut("/client", async (IValidator<Client> validator, Client client, ClientService clientService) =>
{
    var validationResult = await validator.ValidateAsync(client);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }
    return Results.Ok(clientService.UpdateClient(client));
})
.WithName("update client");

app.MapGet("/clients/search", async ([FromQuery] string text, ClientService clientService) =>
{
    // Get the search results from the context item.
    var searchResults = clientService.SearchClients(text);

    // Return the search results.
    return searchResults;
})
.WithName("search client");


app.UseCors();

// seed data
using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

    dataSeeder.Seed();
}

// run app
app.Run();