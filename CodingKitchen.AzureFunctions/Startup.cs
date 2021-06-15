using System;
using AutoMapper;
using CodingKitchen.AzureFunctions;
using CodingKitchen.AzureFunctions.MappingProfiles;
using CodingKitchen.Homemoney.Clients;
using CodingKitchen.Homemoney.Models;
using CodingKitchen.Monobank.Client;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

[assembly: FunctionsStartup(typeof(Startup))]
namespace CodingKitchen.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;
            var monobankApiUrl = configuration["MonobankApiUrl"];
            var monobankApiKey = configuration["MonobankApiKey"];
            var homemoneyApiUrl = configuration["HomemoneyApiUrl"];

            builder.Services
                .AddRefitClient<IMonobankApiClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(monobankApiUrl);
                    c.DefaultRequestHeaders.Add("X-Token", monobankApiKey);
                });
            
            builder.Services
                .AddRefitClient<IHomemoneyApiClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(homemoneyApiUrl);
                });
            
            var homemoneyConfiguration =
                configuration.GetSection("HomemoneyConfiguration").Get<HomemoneyConfiguration>();
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TransactionMappingProfile(homemoneyConfiguration));
            });
            var mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
        }
    }
}
