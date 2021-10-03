using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaFunction
{
    public class Function
    {
        
        /// <summary>
        /// A simple function to log object description.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public void FunctionHandler(S3Event input, ILambdaContext context)
        {
            var obj = input.Records.FirstOrDefault().S3.Object;

            LambdaLogger.Log($"ObjectKey:{obj.Key}, Object size:{obj.Size}");
        }
    }
}
