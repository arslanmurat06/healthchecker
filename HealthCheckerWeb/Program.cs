using AutoMapper.EquivalencyExpression;
using HealthChecker.Contracts.Interfaces.Repositories;
using HealthChecker.Core.MapperProfiles;
using HealthChecker.Core.Repositories;
using HealthChecker.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.UseApplicationDbContext(builder.Configuration);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.UseDefaultIdentity();

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IJobsRepository, JobsRepository>();

builder.Services.AddAutoMapper(cfg => { cfg.AddCollectionMappers(); }, typeof(MapProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
