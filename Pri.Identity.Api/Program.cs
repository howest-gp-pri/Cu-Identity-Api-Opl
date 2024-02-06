using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pri.Identity.Core.Data;
using Pri.Identity.Core.Entities;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace Pri.Identity.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity configuration
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //only for testing purposes!
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["JWTConfiguration:Issuer"],
                    ValidAudience = builder.Configuration["JWTConfiguration:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConfiguration:SigningKey"]))
                };
            });
            //add authorization
            builder.Services.AddAuthorization(options =>
            {
                //city policy
                options.AddPolicy("OnlyCitizensFromBruges",
                    policy =>
                    {
                        policy.RequireClaim("city", new List<string> { "brugge", "Brugge" });
                    });
                //admin policy
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, new List<string> { "admin", "Admin" });
                });
                //loyalmember policy
                options.AddPolicy("OnlyLoyalMembers", policy =>
                policy.RequireAssertion(context => 
                {
                    //controleer registration-date policy
                    var registrationClaimValue = context.User.Claims.FirstOrDefault(c =>
                    c.Type.Equals("registration-date"))?.Value;
                    if (DateTime.TryParseExact(registrationClaimValue, "yy-MM-dd",
                       CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal,
                       out var registrationTime))
                       {
                            return registrationTime.AddYears(1) < DateTime.UtcNow;
                       }
                    return false;
                })
                );
            });
                
                    
            
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}