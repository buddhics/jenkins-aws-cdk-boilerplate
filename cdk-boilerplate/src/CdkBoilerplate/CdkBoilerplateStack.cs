using Amazon.CDK;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Lambda;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CdkBoilerplate
{
    public class CdkBoilerplateStack : Stack
    {
        internal CdkBoilerplateStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var envName = Node.TryGetContext("envName").ToString();

            var buildPath = Node.TryGetContext("build_output_path").ToString();

            var environments = ((Dictionary<string, object>)Node.TryGetContext("environments"))[envName];

            var envParams = ((Dictionary<string, object>)environments);
            var bucket = Bucket.FromBucketAttributes(this, GetStringValue(envParams, "bucket_id"),
                new BucketAttributes { BucketArn = GetStringValue(envParams, "bucket_arn") });

            var securityGroups = Array.ConvertAll<string, ISecurityGroup>(GetArrayValue(envParams,
                "lambda_func_sec_group"), x => {
                    return SecurityGroup.FromSecurityGroupId(this, $"{envName}_secGroup_{x}", x);
                });

            Role myRole = new Role(this, GetStringValue(envParams, "role_name"), new RoleProps
            {
                AssumedBy = new ServicePrincipal("lambda.amazonaws.com")
            });

            myRole.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSLambdaBasicExecutionRole"));
            myRole.AddManagedPolicy(ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSLambdaVPCAccessExecutionRole"));

            var myFunc = new Function(this, GetStringValue(envParams, "lambda_func_id"), new FunctionProps
            {
                Runtime = Runtime.DOTNET_CORE_3_1,
                Code = Code.FromAsset(buildPath),
                Handler = GetStringValue(envParams, "lambda_func_handler"),
                MemorySize = GetIntValue(envParams, "lambda_func_memory_size"),
                Timeout = Duration.Seconds(GetIntValue(envParams, "lambda_func_timeout")),
                FunctionName = GetStringValue(envParams, "lambda_func_name"),
                Role = myRole,
                Vpc = Vpc.FromVpcAttributes(this, $"{envName}_vpc", new VpcAttributes
                {
                    VpcId = GetStringValue(envParams, "lambda_func_vpc"),
                    AvailabilityZones = GetArrayValue(envParams, "lambda_func_availability_zones"),
                    PrivateSubnetIds = GetArrayValue(envParams, "lambda_func_subnet")
                }),
                SecurityGroups = securityGroups
            });
        }

        private int GetIntValue(Dictionary<string, object> context, string key)
        {
            return int.Parse(context.First(c => c.Key == key).Value.ToString());
        }

        private string GetStringValue(Dictionary<string, object> context, string key)
        {
            return context.First(c => c.Key == key).Value.ToString();
        }

        private string[] GetArrayValue(Dictionary<string, object> context, string key)
        {
            var item = (object[])(context.First(c => c.Key == key).Value);
            return Array.ConvertAll<object, string>(item, x => x.ToString());
        }
    }
}
