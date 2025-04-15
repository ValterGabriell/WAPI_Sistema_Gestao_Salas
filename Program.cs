
using Microsoft.EntityFrameworkCore;
using WAPI_GS.Infra.Professor;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;
using WAPI_GS.Repositorios.Disciplina;
using WAPI_GS.Repositorios.Email;
using WAPI_GS.Repositorios.Professor;
using WAPI_GS.Repositorios.ProfessorSala;
using WAPI_GS.Repositorios.Salas;
using WAPI_GS.Repositorios.Turma;
using WAPI_GS.Service;

var builder = WebApplication.CreateBuilder(args);
//add cors policy
var MyCorsPolicy = "_myCorsPolicy";
builder.Services.AddCors(op =>
{
    op.AddPolicy(name: MyCorsPolicy,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


if (builder.Environment.IsDevelopment())
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseNpgsql(connectionString);
    });
}
else
{
    var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
    var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");

    var options = new Supabase.SupabaseOptions
    {
        AutoConnectRealtime = true
    };

    var supabase = new Supabase.Client(url, key, options);
    await supabase.InitializeAsync();
}

builder.Services.AddScoped<IDisciplinaRepository, DisciplinaRepositoryImpl>();
builder.Services.AddScoped<IEmailRepository, EmailRepositoryImpl>();
builder.Services.AddScoped<IProfessorRepository, ProfessorRepositoryImpl>();
builder.Services.AddScoped<ITurmaRepository, TurmaRepositoryImpl>();
builder.Services.AddScoped<IProfessorSalaRepository, ProfessorSalaRepositoryImpl>();
builder.Services.AddScoped<ISalaRepository, SalaRepositoryImpl>();
builder.Services.AddScoped<IUnitOfWork, UOWService>();


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
