using System;
using System.Diagnostics;
using System.Threading.Tasks;
using mao.backend;
using mao.backend.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;

namespace mao.frontend;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddMudServices();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });

        Task.Run(() => MaoSetup(env));
    }

    public void MaoSetup(IWebHostEnvironment env)
    {
        var allAudioSourcePaths = Configuration.GetSection("AudioSourcePaths")?.Get<string[]>() ?? Array.Empty<string>();
        Utils.AudioSourcePaths = allAudioSourcePaths;
        CoreController.LoadAudioSources();
        
        if (!env.IsDevelopment())
        {
            var targetUrl = "https://localhost:5001";
            var configUrls = Configuration.GetValue<string>("Urls") ?? targetUrl;
            
            // Check if configUrls is not null or empty and set to targetUrl if it is
            if (!string.IsNullOrEmpty(configUrls)) configUrls = targetUrl;
            
            var targetUrls = configUrls.Split(";");
            foreach (var url in targetUrls)
            {
                if (!url.Contains("https")) continue;
            
                targetUrl = url;
                break;
            }
            
            Process.Start("explorer", targetUrl);
        }
    }
}
