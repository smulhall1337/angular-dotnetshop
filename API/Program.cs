using API.Extensions;
using API.Helpers;
using API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


#region Services/Repositories

// Scoped services are alive for the duration of the request.
// Need to specify the interface of the repo, plus the concrete class you want to instantiate.

// repository services found in Extensions/ApplicationsServiceExtensions.cs
builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddDbContext<StoreContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerDocumentation();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));
});
#endregion

var app = builder.Build();

#region DB migration
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context, loggerFactory);
}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An error occurred during migration");
}
#endregion

// Configure the HTTP request pipeline.
#region Middleware configuration
app.UseMiddleware<ExceptionMiddleware>(); // our custom error handling middleware
app.UseSwaggerDocumentation();
if (app.Environment.IsDevelopment())
{
    // app.UseDeveloperExceptionPage();
    // we're not using the dev exception page since we wrote our own handlers for errors
}

/*
     If a request comes in that doesnt match an endpoint
     this will redirect to our error handling controller and return 
     our exception with the status code
*/
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles(); // used to serve static files such as images
app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();

#endregion

app.Run();
