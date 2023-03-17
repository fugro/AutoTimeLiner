using RoadmapLogic;
using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using System.Globalization;

namespace RoadmapApp
{
    class Program
    {
        private static readonly string colorSettingsFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "color_settings.json");

        static void Main(string[] args)
        {
            string inputFile = string.Empty;
            bool createCustom = false;

            foreach (var arg in args)
            {
                if ( arg == "-custom")
                {
                    createCustom = true;
                    CreateCustomColorJson();
                }
                else if (File.Exists(arg))
                {
                    inputFile = arg;
                }
            }

            if (string.IsNullOrEmpty(inputFile))
            {
                if (!createCustom)
                {
                    Console.WriteLine("Usage: RoadmapApp.exe [Path to input file] [-custom]");
                    Console.WriteLine("    -custom : used to create color_settings.json allowing customization of the roadmap colors.");
                }

                Console.WriteLine("Press any key to exit application:");
                Console.ReadKey();

                return;
            }

            Input input = JsonConvert.DeserializeObject<Input>(File.ReadAllText(args[0]));

            using var imageStream = new MemoryStream().MakeImage(input, File.Exists(colorSettingsFile)
                ? Settings.CreateWithCustomColors(colorSettingsFile)
                : Settings.Default);
            var bytes = imageStream.ToArray();
            string outputFile = Path.Combine(Path.GetTempPath(), $"{ DateTime.Now:yyyy-MM-dd-hh-mm-ss-fff}.png");
            
            File.WriteAllBytes(outputFile, bytes);

            if (File.Exists(outputFile))
            {
                Process process = new();
                process.StartInfo.FileName = "explorer";
                process.StartInfo.Arguments = outputFile;
                process.StartInfo.RedirectStandardOutput = false;
                process.Start();
            }
        }

        static void CreateCustomColorJson()
        {
            if (File.Exists(colorSettingsFile))
            {
                Console.WriteLine($"\nError: {colorSettingsFile} already exists! Delete or rename before trying again.\n");

                return;
            }

            try
            {
                using (var writer = new StreamWriter(colorSettingsFile))
                {
                    var colors = Settings.Default.ColorSettings;

                    writer.WriteLine("{");
                    writer.WriteLine($"\t\"LineColor\": {HexToRgb(colors.LineColor.ToHex())},");
                    writer.WriteLine($"\t\"HeadingColor\": {HexToRgb(colors.HeadingColor.ToHex())},");
                    writer.WriteLine($"\t\"TeamColor\": {HexToRgb(colors.TeamColor.ToHex())},");
                    writer.WriteLine("\t\"QuarterColors\": {");
                    writer.WriteLine($"\t\t\"Quarter1\": {HexToRgb(colors.QuarterColors["Quarter1"].ToHex())},");
                    writer.WriteLine($"\t\t\"Quarter2\": {HexToRgb(colors.QuarterColors["Quarter2"].ToHex())},");
                    writer.WriteLine($"\t\t\"Quarter3\": {HexToRgb(colors.QuarterColors["Quarter3"].ToHex())},");
                    writer.WriteLine($"\t\t\"Quarter4\": {HexToRgb(colors.QuarterColors["Quarter4"].ToHex())},");
                    writer.WriteLine($"\t\t\"Quarter5\": {HexToRgb(colors.QuarterColors["Quarter5"].ToHex())},");
                    writer.WriteLine($"\t\t\"Quarter6\": {HexToRgb(colors.QuarterColors["Quarter6"].ToHex())},");
                    writer.WriteLine("\t}");
                    writer.WriteLine("}");    
                }

                Console.WriteLine($"\n{colorSettingsFile} created successfully.");
            }
            catch (Exception ex)
            {
                if (File.Exists(colorSettingsFile))
                {
                    File.Delete(colorSettingsFile);
                }

                Console.WriteLine($"\nError: {ex.Message}\n");
            }
            
        }

        static string HexToRgb(string input)
        {
            try
            {
                string red = input[..2];
                string green = input.Substring(2, 2);
                string blue = input.Substring(4, 2);

                uint rhex = uint.Parse(red, NumberStyles.AllowHexSpecifier);
                uint ghex = uint.Parse(green, NumberStyles.AllowHexSpecifier);
                uint bhex = uint.Parse(blue, NumberStyles.AllowHexSpecifier);

                return $"[{rhex}, {ghex}, {bhex}]";
            }
            catch (Exception ex)
            {
                throw new Exception($"HexToRgb() converting {input} to RGB! - {ex.Message}");
            }
        }
    }
}
