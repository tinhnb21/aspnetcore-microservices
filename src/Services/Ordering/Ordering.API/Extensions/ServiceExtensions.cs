﻿using Infrastructure.Configurations;
using Infrastructure.Extensions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ordering.API.Application.IntegrationEvents.EventsHandler;
using Shared.Configurations;

namespace Ordering.API.Extensions
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection(nameof(SMTPEmailSetting)).Get<SMTPEmailSetting>();
            services.AddSingleton(emailSettings);

            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();
            services.AddSingleton(eventBusSettings);

            return services;
        }

        public static void ConfigureMassTransit(this IServiceCollection services)
        {
            var settings = services.GetOptions<EventBusSettings>(nameof(EventBusSettings));
            if (settings == null || string.IsNullOrEmpty(settings.HostAddress))
                throw new ArgumentNullException("EventBusSettings is not configured");

            var mqConnection = new Uri(settings.HostAddress);
            services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
            services.AddMassTransit(config =>
            {
                config.AddConsumersFromNamespaceContaining<BasketCheckoutEventHandler>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(mqConnection);
                    cfg.ConfigureEndpoints(ctx);
                    //cfg.ReceiveEndpoint("basket-checkout-queue", c =>
                    //{
                    //    c.ConfigureConsumer<BasketCheckoutEventHandler>(ctx);
                    //});
                });
            });

        }
    }
}
