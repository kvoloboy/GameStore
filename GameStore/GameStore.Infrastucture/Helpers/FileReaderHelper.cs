using System.IO;

namespace GameStore.Infrastructure.Helpers
{
    public static class FileReaderHelper
    {
        public static string ReadContent(string path)
        {
            var content = File.ReadAllText(path);

            return content;
        }
    }
}