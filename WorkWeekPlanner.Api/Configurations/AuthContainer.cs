using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkWeekPlanner.Api.Features.Settings;

namespace WorkWeekPlanner.Api.Configurations;

public static class AuthContainer
{
    public static void ConfigureAuthentication(this IServiceCollection services, IAppSettings? appSettings)
    {
        ArgumentNullException.ThrowIfNull(appSettings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = appSettings.AppConfiguration.Issuer,
                ValidateAudience = true,
                ValidAudience = appSettings.AppConfiguration.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.AppConfiguration.Key)),
                ValidateLifetime = true
            };
        });
    }
}