using AutoMapper;
using JwtUser.Container;
using JwtUser.Helper;
using JwtUser.Modal;
using JwtUser.Repos;
using JwtUser.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //DBConnection
    builder.Services.AddDbContext<LearnDataContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    
    //DI
    builder.Services.AddScoped<ICustomerService, CustomerService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.Configure<ServiceConfiguration>(builder.Configuration.GetSection("ServiceConfiguration"));


    //Automapper
    var automapper = new MapperConfiguration(item => item.AddProfile(new AutoMapperHandler()));
    IMapper mapper = automapper.CreateMapper();
    builder.Services.AddSingleton(mapper);

    //SeriLogger
    string logPath = builder.Configuration.GetSection("Logging:Logpath").Value!;
    var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(logPath)
    .CreateLogger();
    builder.Logging.AddSerilog(logger);

    //CORS
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin", policy =>
        {
            policy.WithOrigins("http://localhost:4200") 
                  .AllowAnyMethod() // Allow all HTTP methods (GET, POST, PUT, DELETE)
                  .AllowAnyHeader();
        });
    });

    //builder.Services.AddCors(options =>
    //{
    //    options.AddDefaultPolicy(policy =>
    //    {
    //        policy.WithOrigins("*") //For All
    //              .AllowAnyMethod() // Allow all HTTP methods (GET, POST, PUT, DELETE)
    //              .AllowAnyHeader();
    //    });
    //});

    //Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = "BasicAuthentication"; // Set default authentication scheme
        options.DefaultChallengeScheme = "BasicAuthentication"; // Challenge scheme must be set
    }).AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
    //builder.Services.AddAuthentication("BasicAuthentication").AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
    //OR
    //JWT token
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
//app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
