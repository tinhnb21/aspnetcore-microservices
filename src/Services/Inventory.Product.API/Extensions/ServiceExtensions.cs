using Infrastructure.Extensions;
using MongoDB.Driver;

namespace Inventory.Product.API.Extensions
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();
            services.AddSingleton(databaseSettings);
            return services;
        }

        public static void ConfigureMongoDbClient(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(new MongoClient(getMongoConnectionString(services)))
                .AddScoped(x => x.GetService<IMongoClient>()?.StartSession());
        }

        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
        }

        private static string getMongoConnectionString(this IServiceCollection services)
        {
            var settings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
            if (settings == null || string.IsNullOrEmpty(settings.ConnectionString))
                throw new ArgumentNullException("DatabaseSettings is not configured.");

            var databaseName = settings.DatabaseName;
            var mongoDbConnectionString = settings.ConnectionString + "/" + databaseName + "?authSource=admin";
            return mongoDbConnectionString;
        }
    }
}
