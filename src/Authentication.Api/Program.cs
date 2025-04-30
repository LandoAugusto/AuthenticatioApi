
using AuthenticatioApi;
using Component.LogExtensions;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;
builder.Services.AddSingleton(configuration);

builder.Configuration.AddEnvironmentVariables();

//add logs
builder.InitLog(builder.Configuration);


//add services to the application and configure service provider
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

//configure the application HTTP request pipeline
var env = app.Environment;
startup.Configure(app, env);

app.Run();