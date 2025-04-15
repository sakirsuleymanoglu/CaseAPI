using CaseAPI.Abstractions.Accounts;
using CaseAPI.Abstractions.Authentication;
using CaseAPI.Abstractions.Encryption;
using CaseAPI.Abstractions.Jwt;
using CaseAPI.Data;
using CaseAPI.Entities;
using CaseAPI.Middlewares;
using CaseAPI.Options.Encryption;
using CaseAPI.Options.Jwt;
using CaseAPI.Services.Accounts;
using CaseAPI.Services.Authentication;
using CaseAPI.Services.Encryption;
using CaseAPI.Services.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

builder.Services.AddControllers();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IEncryptionService, EncryptionService>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IAccountTransactionService, AccountTransactionService>();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

builder.Services.AddIdentity<AppUser, AppRole>(identityOptions =>
{
    identityOptions.User.RequireUniqueEmail = true;
    identityOptions.Password.RequireDigit = false;
    identityOptions.Password.RequireLowercase = false;
    identityOptions.Password.RequireNonAlphanumeric = false;
    identityOptions.Password.RequireUppercase = false;
    identityOptions.Password.RequiredLength = 1;
    identityOptions.SignIn.RequireConfirmedAccount = false;
    identityOptions.SignIn.RequireConfirmedEmail = false;
    identityOptions.SignIn.RequireConfirmedPhoneNumber = false;
    identityOptions.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

}).AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.Configure<EncryptionOptions>(builder.Configuration.GetSection(nameof(EncryptionOptions)));

var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey)),
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

using var scope = app.Services.CreateScope();

var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

if (!await userManager.Users.AnyAsync())
{
    await userManager.CreateAsync(new AppUser
    {
        UserName = "admin",
        Email = "admin@gmail.com"
    }, "1");


    await userManager.CreateAsync(new AppUser
    {
        UserName = "user",
        Email = "user@gmail.com"
    }, "1");
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
