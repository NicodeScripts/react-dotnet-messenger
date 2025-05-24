using OptimConnect.Chat.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OptimConnect.Chat.DataService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using OptimConnect.Chat.Users;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Add Swagger services

// Configure CORS
builder.Services.AddCors(opt => {
    opt.AddPolicy(name: "reactApp", configurePolicy: builder => {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    // Configure password settings
    options.Password.RequireDigit = true; // Require at least one digit
    options.Password.RequiredLength = 6; // Minimum length
    options.Password.RequireNonAlphanumeric = true; // Require a special character
    options.Password.RequireUppercase = true; // Require an uppercase letter
    options.Password.RequireLowercase = true; // Require a lowercase letter

    // Configue User Settings
    options.User.RequireUniqueEmail = false; // Do not require a unique email
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"; // Customize allowed characters for usernames
}).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "defaultIssuer",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "defaultAudience",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "defaultKey"))
    };
});
// Add Singleton for SharedDB
builder.Services.AddSingleton<SharedDB>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors("reactApp");
app.MapHub<ChatHub>("/chat");
app.Urls.Add("http://192.168.1.143:5000");
app.Urls.Add("https://localhost:5001");



app.Use(async (context, next) =>
{
    Console.WriteLine($"Request Path: {context.Request.Path}");
    await next.Invoke();
});
app.Run();

//Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;