using System;
using Newtonsoft.Json;

namespace GameStore.Infrastructure.Logging.Models
{
    public class LogEntry<TEntity> : BaseLogEntry
    {
        public LogEntry(Operation operation, TEntity oldVersion, TEntity newVersion = default)
        {
            EntityType = typeof(TEntity).Name;
            Operation = Enum.GetName(typeof(Operation), operation);
            Date = DateTime.UtcNow.ToShortDateString();
            Version = GetVersionInfo(oldVersion, newVersion);
        }

        private static string GetVersionInfo(TEntity oldVersion, TEntity newVersion)
        {
            const string oldVersionMessage = "Old: ";
            const string newVersionMessage = "\n New:";
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var oldVersionSerialized = JsonConvert.SerializeObject(oldVersion, settings);
            var newVersionSerialized = JsonConvert.SerializeObject(newVersion, settings);
            var result = newVersion != null
                ? $"{oldVersionMessage}{oldVersionSerialized}{newVersionMessage}{newVersionSerialized}"
                : $"{oldVersionSerialized}";

            return result;
        }
    }
}