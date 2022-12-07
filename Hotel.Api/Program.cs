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
builder.Services.AddTransient<IClientApp, ClientApp>();
builder.Services.AddTransient<IBookingApp, BookingApp>();
builder.Services.AddTransient<IRoomApp, RoomApp>();

builder.Services.AddTransient<IClientRepository, ClientRepository>();
builder.Services.AddTransient<IBookingRepository, BookingRepository>();
builder.Services.AddTransient<IRoomRepository, RoomRepository>();
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
