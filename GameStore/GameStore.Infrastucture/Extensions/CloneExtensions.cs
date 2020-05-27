using Newtonsoft.Json;

namespace GameStore.Infrastructure.Extensions
{
    public static class CloneExtensions
    {
        public static T Clone<T>(this T source)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var serialized = JsonConvert.SerializeObject(source, settings);
            
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}