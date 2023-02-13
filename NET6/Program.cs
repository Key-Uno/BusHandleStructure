using Domain.Repository;
using Infrastructure.Manager;
using InmemoryInfrastructure.Manager;
using System.Runtime.CompilerServices;
using UseCase.Config;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    EnvironmentName = Environments.Development,
});

builder = GenerateServiceContainer(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

WebApplicationBuilder GenerateServiceContainer(WebApplicationBuilder builder)
{
    builder.Services.AddControllersWithViews();

    builder = ConfigureInfrastructure(builder);
    builder = ConfigureUseCase(builder);

    return builder;
}

WebApplicationBuilder ConfigureInfrastructure(WebApplicationBuilder builder)
{
    if (builder.Environment.EnvironmentName == Environments.Development)
    {
        builder.Services.AddTransient<IManagerRepository, InmemoryManagerRepository>();
    }
    else if (builder.Environment.EnvironmentName == Environments.Staging)
    {
        builder.Services.AddTransient<IManagerRepository, ManagerRepository>();
    }
    else if (builder.Environment.EnvironmentName == Environments.Production)
    {
        builder.Services.AddTransient<IManagerRepository, ManagerRepository>();
    }

    return builder;
}

WebApplicationBuilder ConfigureUseCase(WebApplicationBuilder builder)
{
    var useCasebuilder = new UseCaseBusBuilder(builder.Services);

    // Register UseCase
    //useCasebuilder.RegisterUseCase<GetPlayerByPlayerIdRequest, GetPlayerByPlayerId>();

    // Build UseCase
    var provider = builder.Services.BuildServiceProvider();
    var bus = useCasebuilder.Build(provider);
    builder.Services.AddSingleton(bus);

    return builder;
}
