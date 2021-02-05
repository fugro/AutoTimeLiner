using Amazon.Lambda.Core;
using RoadmapLogic;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace RoadmapLambda
{
    public class Function
    {
        public string FunctionHandler(Input input, ILambdaContext context)
        {
            var image = RoadmapImage.MakeImage(input);

            return new Base64Converter().ToBase64(image);
        }
    }
}
