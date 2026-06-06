using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CursosPlatform.api.Data;
using CursosPlatform.api.Services;

var builder = WebApplication.CreateBuilder(args);

// ── Banco de Dados ─────────────────────────────────────────────────────────────
// Registra o AppDbContext com o provider do PostgreSQL (Npgsql)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Injeção de Dependência dos Services ───────────────────────────────────────
// Scoped: uma instância por requisição HTTP
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<EnrollmentService>();

// ── Autenticação JWT ──────────────────────────────────────────────────────────
var jwtSecret = builder.Configuration["Jwt:Secret"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Desativa o remapeamento automático de claims (necessário no .NET 10 com JsonWebTokenHandler)
        // Sem isso, o claim "role" seria remapeado para um URI longo e o [Authorize(Roles=...)] falharia
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            RoleClaimType            = "role",
            NameClaimType            = "name"
        };
    });

builder.Services.AddAuthorization();

// ── Controllers ───────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ── CORS ──────────────────────────────────────────────────────────────────────
// Permite que o frontend React (Vite na porta 5173) acesse a API
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

// ── Seed do Banco de Dados ────────────────────────────────────────────────────
// Cria os usuários e cursos de exemplo caso o banco esteja vazio
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Cria o schema no banco caso ainda não exista (necessário no Docker e em ambientes sem migrations)
    context.Database.EnsureCreated();
    DataSeeder.Seed(context);
}

// ── Pipeline HTTP ─────────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("FrontendPolicy");

// A ordem importa: primeiro autentica (valida o token), depois autoriza (verifica permissões)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
