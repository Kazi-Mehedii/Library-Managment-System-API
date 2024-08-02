using Library_managment_system_API.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//database connection string
builder.Services.AddDbContext<ContextDb>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("Database"));
});

//for connect with clintsite access
builder.Services.AddCors(o =>
{
    o.AddPolicy("myCorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});


// for register and email services
builder.Services.AddScoped<EmailService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//created corse
app.UseCors("myCorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
