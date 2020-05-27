using System.Threading;

namespace GameStore.Common.Models
{
    public static class Culture
    {
        public const string En = "en-US";
        public const string Ru = "ru-RU";
        public static string Current => Thread.CurrentThread.CurrentCulture.Name;
    }
}