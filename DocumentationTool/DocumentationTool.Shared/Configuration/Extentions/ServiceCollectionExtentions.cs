using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DocumentationTool.Shared.Configuration.Extentions
{
    public static class ServiceCollectionExtentions
    {
        /// <summary>
        /// Adds configuration settings into the <see cref="IServiceCollection"/> container.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns>The associated <see cref="IServiceCollection"/> collection, with the added settings.</returns>
        /// <exception cref="ArgumentNullException" />
        public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration), "No valid configuration was given.");

            IWebsiteConfigurationSettings webSettings = configuration.GetSection("WebSiteSettings").Get<WebsiteConfigurationSettings>();
            IAuthenticationSettings authSettings = configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>();

            services.AddSingleton(webSettings);
            services.AddSingleton(authSettings);

            return services;
        }
    }
}
