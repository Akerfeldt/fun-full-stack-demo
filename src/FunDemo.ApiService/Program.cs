using FunDemo.Infrastructure;
using FunDemo.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var identityServer = builder.Configuration["services:identityserver:https:0"];
builder.Services.AddAuthentication("bearer")
    .AddJwtBearer("bearer", options =>
    {
        options.Authority = identityServer;
        options.RequireHttpsMetadata = true;
        options.Audience = "fun_api";
        options.MapInboundClaims = false;
    });

builder.Services.AddSwaggerGen(options =>
{
    var flows = new OpenApiOAuthFlows();
    flows.ClientCredentials = new OpenApiOAuthFlow
    {
        TokenUrl = new Uri(identityServer + "/connect/token"),
        Scopes = new Dictionary<string, string> { { "fun_api", "fun_api" } }
    };

    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Flows = flows,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.OAuth2,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

builder.Services.AddDbContext<FunContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("FunContext")));
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapDefaultEndpoints();
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.Run();