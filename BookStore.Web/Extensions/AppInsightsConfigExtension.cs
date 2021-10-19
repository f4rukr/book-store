using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Extensions
{
    /// <summary>
    // https://docs.microsoft.com/en-us/azure/azure-monitor/app/sampling
    /// </summary>
    public static class AppInsightsConfigExtension
    {
        public static void AddAppInsights(this IServiceCollection services)
        {
            ApplicationInsightsServiceOptions appInsightsOptions = new();

            appInsightsOptions.EnableAdaptiveSampling = false;
            services.AddApplicationInsightsTelemetry(appInsightsOptions);
        }

        public static void UseAppInsights(this IApplicationBuilder app)
        {
            var configuration = app.ApplicationServices.GetService<TelemetryConfiguration>();
            var builder = configuration.DefaultTelemetrySink.TelemetryProcessorChainBuilder;

            builder.UseAdaptiveSampling(maxTelemetryItemsPerSecond: 300, excludedTypes: "Trace;Exception");
            builder.Build();
        }
    }
}
