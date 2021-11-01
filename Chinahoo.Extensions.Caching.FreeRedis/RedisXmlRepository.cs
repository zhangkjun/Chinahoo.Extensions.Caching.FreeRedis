using FreeRedis;
using Microsoft.AspNetCore.DataProtection.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Chinahoo.Extensions.Caching.FreeRedis
{
    /// <summary>
    /// An XML repository backed by a Redis list entry.
    /// </summary>
    public class RedisXmlRepository : IXmlRepository
    {
        private readonly Func<RedisClient> _databaseFactory;
        private readonly string _key;

        /// <summary>
        /// Creates a <see cref="RedisXmlRepository"/> with keys stored at the given directory.
        /// </summary>
        /// <param name="databaseFactory">The delegate used to create <see cref="IDatabase"/> instances.</param>
        /// <param name="key">The <see cref="RedisKey"/> used to store key list.</param>
        public RedisXmlRepository(Func<RedisClient> databaseFactory, string key)
        {
            _databaseFactory = databaseFactory;
            _key = key;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<XElement> GetAllElements()
        {
            return GetAllElementsCore().ToList().AsReadOnly();
        }

        private IEnumerable<XElement> GetAllElementsCore()
        {
            var database = _databaseFactory();
            foreach (var value in database.LRange(_key, 0, -1))
            {
                yield return XElement.Parse(value);
            }
        }

        /// <inheritdoc />
        public void StoreElement(XElement element, string friendlyName)
        {
            var database = _databaseFactory();
            database.RPush(_key, element.ToString(SaveOptions.DisableFormatting));
        }
    }
}