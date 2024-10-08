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
        
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                //DEBUG INFO
                var identity = context.Principal.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    foreach (var claim in identity.Claims)
                    {
                        Console.WriteLine($"Claim: {claim.Type} - {claim.Value}");
                    }
                }
                
                List<AuthenticationToken> tokens = context.Properties!.GetTokens().ToList();
                ClaimsIdentity claimsIdentity = (ClaimsIdentity)context.Principal!.Identity!;

                var realm_access = claimsIdentity.FindFirst((claim) => claim.Type == "realm_access")?.Value;
                        
                JObject obj = JObject.Parse(realm_access);
                var roleAccess = obj.GetValue("roles");
                foreach (JToken role in roleAccess!)
                {
                    claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                }

                return Task.CompletedTask;
                
            }
        };
        
    });
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddHttpClient();


var app = builder.Build();
// app.UseMiddleware<TokenLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();