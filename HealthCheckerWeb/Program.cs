using AutoMapper.EquivalencyExpression;
using HealthChecker.Contracts.Interfaces.NotificationService;
using HealthChecker.Contracts.Interfaces.Repositories;
using HealthChecker.Contracts.Interfaces.Services;
using HealthChecker.Core.MapperProfiles;
using HealthChecker.Core.Repositories;
using HealthChecker.Core.Services;
using HealthChecker.DataContext;
using HealthLifeCheckService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NotificationService;
using ScheduleJobManager;
using SchedulerService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.UseApplicationDbContext(builder.Configuration);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.UseDefaultIdentity();

builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IJobsRepository, JobsRepository>();
builder.Services.AddTransient<IJobService, JobService>();
builder.Services.AddTransient<INotificationService, MailService>();

builder.Services.AddAutoMapper(cfg => { cfg.AddCollectionMappers(); }, typeof(MapProfile));

builder.Services.UseHangFire(builder.Configuration);

builder.Services.AddSingleton<IScheduleJobManager<IHealthCheckJobManager>, ScheduleJobManager<IHealthCheckJobManager>>();
builder.Services.AddTransient<IHealthCheckJobManager, HealthCheckJobManager>();

builder.Services.AddHttpClient<IHealthCheckService, HealthCheckService>(client =>
{
    client.Timeout = TimeSpan.FromMilliseconds(10000);
});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHangFireDashboard(builder.Configuration);

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
