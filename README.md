# Chinahoo.Extensions.Caching.FreeRedis
redis缓存freeredis的实现



services.AddDataProtection(options => options.ApplicationDiscriminator = "Chinahoo.Web")
                    .PersistKeysToFreeRedis("redis的链接", "Chinahoo.Web");
