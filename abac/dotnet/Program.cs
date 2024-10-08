using System.Security.Claims;
using dotnet;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);

var keycloakSettings = builder.Configuration.GetSection("Keycloak");

builder.Services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme =  JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.Authority = keycloakSettings["Authority"];
        options.Audience = keycloakSettings["ClientId"];
        options.RequireHttpsMetadata = false;
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidAudience = keycloakSettings["ClientId"],
            ValidateIssuer = false,
            ValidIssuer = keycloakSettings["Authority"],
        };
        
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddHttpClient();


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();