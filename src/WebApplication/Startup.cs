using Domain.Application;
using Domain.Domain.Users;
using Domain.UnitOfWorkSample;
using InMemoryInfrastructure.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using ProductInfrastructure.UnitOfWorkSample;
using ProductInfrastructure.Users;
using System.Data;
using UserRepository = ProductInfrastructure.Users.UserRepository;

namespace WebApplication;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc(options =>
        {
            options.EnableEndpointRouting = false;
        });

        //SetupByNormal(services);

        // UnitOfWork で動かしたい場合は↑をコメントにして、↓をコメントアウト
        SetupByUnitOfWork(services);
        // UserController で UserApplicationServiceByUow を利用するようにしてください
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMvc(routes =>
        {
            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        });
    }

    private void SetupByNormal(IServiceCollection services)
    {
        RegisterInMemory(services);
        // ↓SQL を利用して動かしたい場合
        //            RegisterSql(services);
        // ↓EntityFramework を利用して動かしたい場合
        //            RegisterEntityFramework(services);

        services.AddTransient<UserApplicationService>();
    }

    private void RegisterInMemory(IServiceCollection services)
    {
        services.AddSingleton<IUserFactory, UserFactory>();
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
    }

    private void RegisterSql(IServiceCollection services)
    {
        // IDbConnection の DI設定
        services.AddTransient<IDbConnection>(sp =>
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            return new MySqlConnection(connectionString);
        });

        services.AddSingleton<IUserFactory, UserFactory>();
        services.AddTransient<IUserRepository, UserRepository>();
    }

    //private void RegisterEntityFramework(IServiceCollection services)
    //{
    //    services.AddSingleton<IUserFactory, UserFactory>();
    //    services.AddTransient(provider =>
    //    {
    //        var context = new BottomUpDddDbContext();
    //        context.Database.AutoTransactionsEnabled = false; // 自動トランザクションを無効にして Nested transaction にならないようにする
    //        return context;
    //    });
    //    services.AddDbContext<BottomUpDddDbContext>();
    //    services.AddTransient<IUserRepository, EFUserRepository>();
    //}

    private void SetupByUnitOfWork(IServiceCollection services)
    {
        // IDbConnection の DI 設定
        services.AddTransient<IDbConnection>(sp =>
        {
            var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
            return new MySqlConnection(connectionString);
        });

        services.AddSingleton<IUserFactory, UserFactory>();
        services.AddTransient<IUserRepository, ProductInfrastructure.UnitOfWorkSample.UserRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        services.AddTransient<UserApplicationServiceByUow>();
    }
}
