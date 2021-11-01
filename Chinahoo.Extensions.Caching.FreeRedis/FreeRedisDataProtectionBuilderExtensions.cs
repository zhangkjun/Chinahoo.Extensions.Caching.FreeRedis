using FreeRedis;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chinahoo.Extensions.Caching.FreeRedis
{
    /// <summary>
    /// Contains Redis-specific extension methods for modifying a <see cref="IDataProtectionBuilder"/>.
    /// </summary>
    public static class FreeRedisDataProtectionBuilderExtensions
    {
        private const string DataProtectionKeysName = "DataProtection-Keys";

        /// <summary>
        /// Configures the data protection system to persist keys to specified key in Redis database
        /// </summary>
        /// <param name="builder">The builder instance to modify.</param>
        /// <param name="databaseFactory">The delegate used to create <see cref="IDatabase"/> instances.</param>
        /// <param name="key">The <see cref="RedisKey"/> used to store key list.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        public static IDataProtectionBuilder PersistKeysToFreeRedis(this IDataProtectionBuilder builder, Func<RedisClient> databaseFactory, string key)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (databaseFactory == null)
            {
                throw new ArgumentNullException(nameof(databaseFactory));
            }
            return PersistKeysToFreeRedisInternal(builder, databaseFactory, key);
        }

        /// <summary>
        /// Configures the data protection system to persist keys to the default key ('DataProtection-Keys') in Redis database
        /// </summary>
        /// <param name="builder">The builder instance to modify.</param>
        /// <param name="connectionMultiplexer">The <see cref="IConnectionMultiplexer"/> for database access.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        public static IDataProtectionBuilder PersistKeysToFreeRedis(this IDataProtectionBuilder builder, ConnectionStringBuilder connectionMultiplexer)
        {
            return PersistKeysToFreeRedis(builder, connectionMultiplexer, DataProtectionKeysName);
        }

        /// <summary>
        /// Configures the data protection system to persist keys to the specified key in Redis database
        /// </summary>
        /// <param name="builder">The builder instance to modify.</param>
        /// <param name="connectionMultiplexer">The <see cref="IConnectionMultiplexer"/> for database access.</param>
        /// <param name="key">The <see cref="RedisKey"/> used to store key list.</param>
        /// <returns>A reference to the <see cref="IDataProtectionBuilder" /> after this operation has completed.</returns>
        public static IDataProtectionBuilder PersistKeysToFreeRedis(this IDataProtectionBuilder builder, ConnectionStringBuilder connectionMultiplexer, string key)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (connectionMultiplexer == null)
            {
                throw new ArgumentNullException(nameof(connectionMultiplexer));
            }
            return PersistKeysToFreeRedisInternal(builder, () => new RedisClient(connectionMultiplexer), key);
        }

        private static IDataProtectionBuilder PersistKeysToFreeRedisInternal(IDataProtectionBuilder builder, Func<RedisClient> databaseFactory, string key)
        {
            builder.Services.Configure<KeyManagementOptions>(options =>
            {
                options.XmlRepository = new RedisXmlRepository(databaseFactory, key);
            });
            return builder;
        }
    }
}
