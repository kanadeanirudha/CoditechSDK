using Coditech.Admin;

/// <summary>
/// Creates a WebApplication Builder with the given arguments.
/// </summary>
var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Registers common services.
/// </summary>
builder.RegisterCommonServices();

/// <summary>
/// Builds the application.
/// </summary>
var app = builder.Build();
/// <summary>
/// Registers application services with the specified builder.
/// </summary>
app.RegisterApplicationServices(builder);
/// <summary>
/// Executes the application's startup logic. 
/// </summary>
app.Run();


