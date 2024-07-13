using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using SmartFeedback.Scripts;
using SmartFeedback.Scripts.Interfaces;
using SmartFeedback.Scripts.Services.Module;
using SmartFeedback.Scripts.Services.MongoDB;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");
AppSettings.Initialize(builder.Configuration);

ConfigureServices(builder.Services);
ConfigureAuthentication(builder.Services);
ConfigureSwagger(builder.Services);

var app = builder.Build();

ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IMongoClient>(_ =>
    {
        Debug.Assert(AppSettings.MongoDb != null, "AppSettings.MongoDb != null");
        return new MongoClient(AppSettings.MongoDb.ConnectionString);
    });

    services.AddScoped(provider =>
    {
        var mongoClient = provider.GetRequiredService<IMongoClient>();
        Debug.Assert(AppSettings.MongoDb != null, "AppSettings.MongoDb != null");
        return mongoClient.GetDatabase(AppSettings.MongoDb.DatabaseName);
    });

    services.AddScoped<IUserService, UserService>();
    services.AddScoped<IProjectService, ProjectService>();
    services.AddScoped<ITextObjectService, TextObjectService>();
    services.AddScoped<IProcessingModuleService, ProcessingModuleService>();

    services.AddControllers();
    services.AddHttpClient("PythonModuleClient",client =>
    {
        Debug.Assert(AppSettings.PythonModule != null, "AppSettings.PythonModule != null");
        client.BaseAddress = new Uri(AppSettings.PythonModule.BaseUrl);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    });
    
    // Добавьте эту строку для добавления службы CORS
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}

void ConfigureAuthentication(IServiceCollection services)
{
    Debug.Assert(AppSettings.Authorization != null, "AppSettings.Authorization != null");
    var authorizationSettings = AppSettings.Authorization;

    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = authorizationSettings.Issuer,
                ValidAudience = authorizationSettings.Audience,
                IssuerSigningKey = authorizationSettings.GetKey
            };
        });
}

void ConfigureSwagger(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "SmartFeedback API", Version = "v1" });
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter into field the word 'Bearer' following by space and JWT",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });
}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    
    // Добавьте эту строку для включения CORS
    app.UseCors("AllowAllOrigins");
    
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
}