# Roadmap Generation

Generates product delivery roadmaps from JSON input and exports them as images.

The core of the code is in a class library. It is written in .NET Core and uses [SixLabors.ImageSharp](https://docs.sixlabors.com/index.html) to enable cross-platform compatibility. To demonstrate this, the project also includes a simple AWS Lambda project that responds to valid requests with roadmap images in Base64 format.

See [DotNet](https://github.com/fugro/AutoTimeLiner/tree/main/DotNet) project for the most up-to-date info.

Older [Python](https://github.com/fugro/AutoTimeLiner/blob/main/AutoTimeLiner) codebase is no longer actively developed.
