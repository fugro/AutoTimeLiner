using System;
using System.IO;

namespace RoadmapLogic
{
    public static class Base64Converter
    {
        public static string ToBase64(MemoryStream memoryStream)
        {
            var bytes = memoryStream.ToArray();
            return ToBase64Helper(bytes);
        }

        public static string ToBase64(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            return ToBase64Helper(bytes);
        }

        private static string ToBase64Helper(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }
}
