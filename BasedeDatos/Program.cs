using BasedeDatos.MysqlModels;
using BasedeDatos.OracleModels;
using BasedeDatos.PostgresqlModels;
using BasedeDatos.SqlServerModels;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register AppDbContext for MySQL with dependency injection
builder.Services.AddDbContext<BdmysqlContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"),
        new MySqlServerVersion(new Version(8, 4, 0)),
        mySqlOptions => mySqlOptions.EnableRetryOnFailure()
    ));

// Register SqlServerDbContext for SQL Server with dependency injection
builder.Services.AddDbContext<BdsqlserverContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServerConnection")
    ));

builder.Services.AddDbContext<BdpostgresqlContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresqlConnection")));

builder.Services.AddDbContext<ModelContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();