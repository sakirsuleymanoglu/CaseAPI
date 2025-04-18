using CaseAPI.Abstractions.Accounts;
using CaseAPI.Abstractions.Authentication;
using CaseAPI.Abstractions.Encryption;
using CaseAPI.Abstractions.Hubs;
using CaseAPI.Abstractions.Jwt;
using CaseAPI.Abstractions.Users;
using CaseAPI.Data;
using CaseAPI.Entities;
using CaseAPI.Hubs;
using CaseAPI.Middlewares;
using CaseAPI.Options.Encryption;
using CaseAPI.Options.Jwt;
using CaseAPI.Services.Accounts;
using CaseAPI.Services.Authentication;
using CaseAPI.Services.Encryption;
using CaseAPI.Services.Hubs.Transfer;
using CaseAPI.Services.Jwt;
using CaseAPI.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
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

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ITransferHubService, TransferHubService>();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("SQLServer"));
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
        ClockSkew = TimeSpan.Zero,
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Query.TryGetValue("access_token", out StringValues value))
            {
                context.Token = value;
            }
            return Task.CompletedTask;
        }
    };

});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

using var scope = app.Services.CreateScope();

var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

if (!await userManager.Users.AnyAsync())
{
    string password = "1";

    AppUser user1 = new AppUser
    {
        UserName = "user1",
        Email = "user1@gmail.com"
    };

    AppUser user2 = new AppUser
    {
        UserName = "user2",
        Email = "user2@gmail.com"
    };

    await userManager.CreateAsync(user1, password);

    await userManager.CreateAsync(user2, password);


    await applicationDbContext.Accounts.AddAsync(new Account
    {
        Code = "12345678",
        AppUserId = user1.Id,
        Title = "Account 1",
        Balance = 1000
    });

    await applicationDbContext.Accounts.AddAsync(new Account
    {
        Code = "12345679",
        AppUserId = user1.Id,
        Title = "Account 2",
        Balance = 3000
    });


    await applicationDbContext.Accounts.AddAsync(new Account
    {
        Code = "12345638",
        AppUserId = user2.Id,
        Title = "Account 1",
        Balance = 1500
    });

    await applicationDbContext.Accounts.AddAsync(new Account
    {
        Code = "12345649",
        AppUserId = user2.Id,
        Title = "Account 2",
        Balance = 3500
    });

    await applicationDbContext.Accounts.AddAsync(new Account
    {
        Code = "12345249",
        AppUserId = user2.Id,
        Title = "Account 3",
        Balance = 5500
    });

    await applicationDbContext.SaveChangesAsync();
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

app.MapHub<TransferHub>("/transfer-hub");

app.Run();
