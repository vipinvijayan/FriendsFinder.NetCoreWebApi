using DrCleanerAppWebApis.CustomMiddleware;
using CliniqonFindFriendsAppBuisinessLogic;
using CliniqonFindFriendsAppBuisinessLogic.Implementation;
using CliniqonFindFriendsAppBuisinessLogic.Interfaces;
using CliniqonFindFriendsAppBussinessLogic.Implementation;
using CliniqonFindFriendsAppBussinessLogic.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//To allow cross-origin request.
builder.Services.AddCors(options =>
{
    options.AddPolicy("All", builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    );
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Add this configuration to enable authorize tab for token in swagger UI
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Cliniqon Find Friends Web API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

//Added JWT Authentication Configuration 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = "CliniqonFindFriends",
        ValidIssuer = "CliniqonFindFriends",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSignInKey"]))

    };
});
// Read the connection string from appsettings.
string dbConnectionString = builder.Configuration.GetConnectionString("AppConnection");
//Injected Sql Connection String
builder.Services.AddTransient<IDbConnection>(sql => new SqlConnection(dbConnectionString));

builder.Services.AddRepositoryDependencies(); //Repository Layer Injection
builder.Services.AddScoped<IPasswordB, PasswordB>();// Custom Buisiness layer injection
builder.Services.AddScoped<IUserB, UserB>();// Custom Buisiness layer injection
builder.Services.AddSingleton<ISecurityB, SecurityB>();// Custom Buisiness layer injection using add singleton to use in custom middleware for token status check from DB
builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("All");
app.UseAuthentication();//this checks the user is valid
app.UseAuthorization();//this checks user has valid roles
//Use this middle ware we are validating the token from the header is valid on Database.
app.UseMiddleware<ValidateTokenMiddleware>();
app.MapControllers();

app.Run();
