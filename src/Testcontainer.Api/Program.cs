using Microsoft.AspNetCore.Mvc;
using Refit;
using Testcontainer.Api.Clients.BlogClient;

var builder = WebApplication.CreateBuilder(args);
var blogClientAddress = builder.Configuration["BlogClient:BaseAddress"];
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRefitClient<IBlogClient>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri(blogClientAddress!));
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/posts", async (int? userId, IBlogClient client) =>
    await client.GetPostsAsync(userId));

app.MapGet("/posts/{id:int}", async (int id, IBlogClient client) =>
    await client.GetPostAsync(id));

app.MapGet("/posts/{id:int}/comments", async (int id, IBlogClient client) =>
    await client.GetPostCommentsAsync(id));

app.MapPost("/posts", async ([FromBody] Post post, IBlogClient client) =>
    await client.CreatePostAsync(post));

app.MapPut("/posts/{id:int}", async (int id, [FromBody] Post post, IBlogClient client) =>
    await client.UpdatePostAsync(id, post));

app.MapDelete("/posts/{id:int}", async (int id, IBlogClient client) =>
    await client.DeletePostAsync(id));
    
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.UseHttpsRedirection();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }
