# Roadmap Generation

Generates product delivery roadmaps from JSON input and exports them as images.

The core of the code is in a class library. It is written in .NET Core and uses [SixLabors.ImageSharp](https://docs.sixlabors.com/index.html) to enable cross-platform compatibility. To demonstrate this, the project also includes a simple AWS Lambda project that responds to valid requests with roadmap images in Base64 format.

See [DotNet](https://github.com/fugro/AutoTimeLiner/tree/main/DotNet) project for the most up-to-date info.

Older [Python](https://github.com/fugro/AutoTimeLiner/blob/main/AutoTimeLiner) codebase is no longer actively developed.

## JSON Input Format

```json
{
    "team": "Your Team",
    "start_date": "01/01/2021",
    "quarters": 4,
    "projects": [
        {
            "name": "Build Product",
            "label": "Ongoing",
            "date": "01/01/2021"
        },
        {
            "name": "Test Product",
            "label": "Not Started",
            "date": "06/01/2021"
        }
    ]
}
```

*start_date* - Defines the first calendar quarter to be used in the roadmap.
*quarters* - Between 1 and 4 (optional; defaults to 4).

All date values are accepted in a variety of formats:
* `2021/09/05`
* `05 Sep 2021`
* `09/05/2021`
* `09-05-2021`
etc...
