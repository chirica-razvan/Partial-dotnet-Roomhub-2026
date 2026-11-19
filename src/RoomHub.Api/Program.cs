using System.Text.Json.Serialization;
using RoomHub.Api.Common.Cqrs;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Features.Bookings;
using RoomHub.Api.Features.Rooms;
using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Infrastructure.Persistence;
using RoomHub.Api.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Enums travel in JSON as text ("Active"), not as numbers.
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Any unexpected error also comes out as ProblemDetails (500), without a stack trace for the client.
builder.Services.AddProblemDetails();

builder.Services.AddSingleton(TimeProvider.System);

// The DbContext is Scoped: a new instance per HTTP request.
builder.Services.AddDbContext<RoomHubDbContext>((serviceProvider, options) =>
    options.UseSqlite(serviceProvider
        .GetRequiredService<IConfiguration>()
        .GetConnectionString("RoomHub")));

// shortcut: Singleton because the data is in memory; becomes Scoped with EF Core (level C).
builder.Services.AddSingleton<IRoomRepository, RoomRepository>();
builder.Services.AddSingleton<IBookingRepository, BookingRepository>();

builder.Services.AddHandlers(typeof(Program).Assembly);

var app = builder.Build();

app.UseExceptionHandler(); // exceptions -> 500 ProblemDetails
app.UseStatusCodePages();  // unknown route -> 404 ProblemDetails

app.MapGet("/", () => "RoomHub API is running.");
app.MapRooms();
app.MapBookings();

app.Run();
