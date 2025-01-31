
using Microsoft.EntityFrameworkCore;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;
using WAPI_GS.Service;

var builder = WebApplication.CreateBuilder(args);
//add cors policy
var MyCorsPolicy = "_myCorsPolicy";
builder.Services.AddCors(op =>
{
    op.AddPolicy(name: MyCorsPolicy,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});
builder.Services.AddScoped<ICS_UnitOfWork, UOWService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyCorsPolicy);
app.UseAuthorization();

app.MapControllers();

app.Run();
