# Roadmap Generation

Generates product delivery roadmaps from JSON input and exports them as images.

The core of the code is in a class library. It is written in .NET Core and uses [SixLabors.ImageSharp](https://docs.sixlabors.com/index.html) to enable cross-platform compatibility. To demonstrate this, the project also includes a simple AWS Lambda project that responds to valid requests with roadmap images in Base64 format.

## Cloud Architecture

While the logic in this repo can be deployed to the AWS cloud, there is currently no infrastructure-as-code to do an automated deployment. The developers of this repo used the following pictured architecture successfully. All cloud components were manually deployed, and Auth0 was used to manage authenticating between components as needed.

![image](https://github.com/fugro/AutoTimeLiner/architecture.jpg)

## Tests

To test out the image generation logic, go to `RoadmapLogic.Tests` and run any of the tests inside the `ImageTest` class. It will create a PNG image inside `DotNet\RoadmapLogic.Tests\bin\Debug\netcoreapp3.1`.

## Files as Base64

Resources such as fonts and external images are represented in Base64 in this project. This removes any dependencies on a filesystem so that the image generator can run in the cloud on AWS Lambda (how it's currently used).

These resources are stored in ```RoadmapLogic\ReferenceFiles\```.

To generate Base64 code for a new file, see the ```FileToBase64()``` unit test in the solution. You can specify a local file in the ```imageFile``` variable, run the test, and the resulting Base64 will be saved into a .txt file in the same directory. Copy-paste this file's contents into file ```\ReferenceFiles\CompanyLogo.bak```.
