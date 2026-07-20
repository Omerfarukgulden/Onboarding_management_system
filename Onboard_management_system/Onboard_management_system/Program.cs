using Microsoft.EntityFrameworkCore;
using Onboard_management_system.OnboardingApplication.Interfaces;
using Onboard_management_system.OnboardingApplication.Services;
using Onboard_management_system.OnboardingInfrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<OnboardingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOnboardingTemplateService, OnboardingTemplateService>();
builder.Services.AddScoped<IOnboardingProcessService, OnboardingProcessService>();
builder.Services.AddScoped<IOnboardingTaskService, OnboardingTaskService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();