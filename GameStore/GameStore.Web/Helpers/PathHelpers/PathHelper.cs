using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;

namespace GameStore.Web.Helpers.PathHelpers
{
    public static class FilterPathHelper
    {
        public const string Path = "/games/filter";
        
        public static string Insert(
            string name,
            string value,
            string path,
            IEnumerable<KeyValuePair<string, StringValues>> queryCollection)
        {
            var queryItems = queryCollection.SelectMany(pair => pair.Value,
                (k, v) => new KeyValuePair<string, string>(k.Key, v));

            var queryBuilder = new QueryBuilder(queryItems)
            {
                {name, value}
            };

            return $"{path}{queryBuilder}";
        }
    }
}