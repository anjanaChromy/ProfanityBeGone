using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfanityBeGone.Api;
using ProfanityBeGone.Api.Repositories;
using ProfanityBeGone.Api.Repositories.Interfaces;

[assembly: FunctionsStartup(typeof(Startup))]
namespace ProfanityBeGone.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<AppSettings>().Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.Bind(settings);
            });

            builder.Services.AddHttpClient();
            builder.Services.AddTransient<IContentBlobRepository, ContentBlobRepository>();
        }
    }
}