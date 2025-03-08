using Microsoft.AspNetCore.Cors.Infrastructure;
using MovieDb.Api.DbContexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MovieDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddProblemDetails();
builder.Services.AddLogging((builder) => builder.AddConsole());
builder.Services.AddCors((builder) =>
{
    builder.AddPolicy(name: "AllowLocalhost", policy =>
	{
		policy.WithOrigins("https://localhost:7151").AllowAnyMethod().AllowAnyHeader();
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add ProblemDetails middleware to the application pipeline
app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowLocalhost");

app.Run();
