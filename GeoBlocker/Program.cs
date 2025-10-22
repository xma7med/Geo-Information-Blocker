using GeoBlocker._Models;
using GeoBlocker._Services.BlockedCountries;
using GeoBlocker._Services.IPCheckUp;
using GeoBlocker.BackgroundServices;
using GeoBlocker.Repositories.InMemoryStore.BlockedCountryInMemoryStore;
using Microsoft.AspNetCore.HttpOverrides;

namespace GeoBlocker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region DI and Service Configuration  


            builder.Services.Configure<ForwardedHeadersOptions>(options =>
                {
                    // What headers to trust
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                    // (Optional) If you know your proxy addresses, you can trust only them:
                    // options.KnownProxies.Add(IPAddress.Parse("10.0.0.1"));
                    // options.KnownNetworks.Add(new IPNetwork(IPAddress.Parse("192.168.0.0"), 16));
                });
            builder.Services.Configure<IpAPIoption>(builder.Configuration.GetSection("IpApi"));


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();


            builder.Services.AddSingleton<IBlockedCountryInMemoryStore, BlockedCountriesInMemoryStore>();

            builder.Services.AddHttpClient<IBlockedCountriesService, BlockedCountriesService>();

            builder.Services.AddScoped<IBlockedCountriesService, BlockedCountriesService>();
            builder.Services.AddScoped<IIpInfoCheckUpService, IpInfoCheckUpService>();
            builder.Services.AddHostedService<TemporalBlockCleanupService>();

            builder.Services.AddHttpContextAccessor();

            #endregion


            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerGen();


            var app = builder.Build();


            app.UseForwardedHeaders();
            app.MapControllers();


            // Configure the HTTP request pipeline.

                app.UseSwagger();
                app.UseSwaggerUI();
            
            app.UseHttpsRedirection();

            //app.UseAuthorization();


            app.Run();
        }
    }
}
