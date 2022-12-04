using Microsoft.EntityFrameworkCore;
using MVCStoreData;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(config =>
{
    config.UseLazyLoadingProxies();

    var provider = builder.Configuration.GetValue<string>("DbProvider");
    switch (provider)
    {
        case "Npgsql":
            config.UseNpgsql(
                       builder.Configuration.GetConnectionString("Npgsql"),
                       options => options.MigrationsAssembly("MigrationsNpgsql")
                       );
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            break;
        case "SqlServer":
        default:
            config.UseSqlServer(
                    builder.Configuration.GetConnectionString("SqlServer"),
                    options => options.MigrationsAssembly("MigrationsSqlServer")
                    );
            break;
    }
});


builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors(p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
