using System;
using System.IO;
//using System.Web.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
//using Owin;
using SwitchBladeInterface.API.DBContext;
using SwitchBladeInterface.API.Repositories;
using SwitchBladeInterface.API.Repositories.Interfaces;
using SwitchBladeInterface.API.Services.LocalServices;
using SwitchBladeInterface.API.Services.SecurityServices;

namespace SwitchBladeInterface.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Console.WriteLine("Version 210715");
        }

        public IConfiguration Configuration { get; }

   
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Connection String
            string dbLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "iHeartMedia", "SBInterface", "Database");
            string connectionStringSQLite = string.Concat("Data Source=", dbLocation, "\\ihmSwitchBladeInterface.db;");

            services.AddControllers();

            services.AddScoped<IServiceSettingsRepository, ServiceSettingsRepository>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IAccountsRepository, AccountsRepository>();
            services.AddScoped<IStationsRepository, StationsRepository>();
            services.AddScoped<IPanelsRepository, PanelsRepository>();
            services.AddScoped<IAccountsRepository, AccountsRepository>(); 
            services.AddScoped<ITokensRepository, TokensRepository>();
            services.AddScoped<IPanelsRepository, PanelsRepository>();
            services.AddScoped<IMacroPanelsRepository, MacroPanelsRepository>();
            services.AddScoped<IMacroElementsRepository, MacroElementsRepository>();
            services.AddScoped<IDevicesRepository, DevicesRepository>();
            services.AddScoped<IBladeIORepository, BladeIORepository>();

            services.AddScoped<IChannelInfoRepository, ChannelInfoRepository>();
            services.AddScoped<IPhonebookRepository, PhonebookRepository>();
            services.AddScoped<IRoomsRepository, RoomsRepository>();
            services.AddScoped<ISitesRepository, SitesRepository>();
            services.AddScoped<IDisplayEventsRepository, DisplayEventsRepository>();

            services.AddScoped<ILocalCallService, LocalCallService>();
            services.AddScoped<ILocalDisplayEventTakeService, LocalDisplayEventTakeService>();

            //services.AddSingleton<IUDPListener, UDPListener>();

            services.AddDbContext<SwitchBladeInterfaceContext>(options =>
            {
                options.UseSqlite(
                    connectionStringSQLite);
            });

            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
