using RoadmapLogic;
using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace RoadmapApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 || File.Exists(args[0]) == false)
            {
                Console.WriteLine("Usage: RoadmapApp.exe [Path to input file]");
                Console.WriteLine("Press any key to Exit Application:");
                Console.ReadKey();
                return;
            }

            Input input = JsonConvert.DeserializeObject<Input>(File.ReadAllText(args[0]));

            var imageStream = RoadmapImage.MakeImage(input, Settings.Default);

            var bytes = imageStream.ToArray();
            string outputFile = Path.Combine(Path.GetTempPath(), $"{ DateTime.Now:yyyy-MM-dd-hh-mm-ss-fff}.png");
            
            File.WriteAllBytes(outputFile, bytes);

            if (File.Exists(outputFile))
            {
                Process process = new Process();
                process.StartInfo.FileName = "explorer";
                process.StartInfo.Arguments = outputFile;
                process.StartInfo.RedirectStandardOutput = false;
                process.Start();
            }
        }
    }
}
