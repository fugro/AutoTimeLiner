using System;
using System.IO;

namespace RoadmapLogic
{
    public class Base64Converter
    {
        public string ToBase64(MemoryStream memoryStream)
        {
            var bytes = memoryStream.ToArray();
            return ToBase64Helper(bytes);
        }

        public string ToBase64(string filePath)
        {
            var bytes = File.ReadAllBytes(filePath);
            return ToBase64Helper(bytes);
        }

        private string ToBase64Helper(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
    }
}
