# Roadmap Generation

Uses [SixLabors.ImageSharp](https://docs.sixlabors.com/index.html) to produce images.

To test out the image generation logic, go to RoadmapLogic.Tests and run the single unit test. It should create a "test" .png image inside DotNet\RoadmapLogic.Tests\bin\Debug\netcoreapp3.1.

## Base64

Resources such as fonts and external images are represented in Base64 in this project. This removes any dependencies on a filesystem so that the image generator can run in the cloud on AWS Lambda (how it's currently used).

These resources are stored in ```RoadmapLogic.FontsBase64.cs```.

To generate Base64 code for a new file, see the ```FileToBase64()``` unit test in the solution. You can specify a local file in the ```imageFile``` variable, run the test, and the resulting Base64 will be saved into a .txt file in the same directory. Copy-paste this file's content into a new property in ```FontsBase64.cs``` to use it.