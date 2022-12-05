using Application.Classes;
using Application.Interfaces;
using Hotel.Api.Middlewares;
using Repository.Classes;
using Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Depency Injection
builder.Services.AddTransient<IUserApp, UserApp>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
#endregion

var app = builder.Build();


#region Middlewares
app.UseMiddleware<ErrorHandlerMiddleware>();
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
