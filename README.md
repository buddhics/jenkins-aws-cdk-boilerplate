# AWS CDK boilerplate in .Net with Jenkins pipeline configurations.

This is the jenkins pipeline configurations boilerplate with AWS CDK.

# Deploy AWS CDK stack to different AWS accounts.

Organizing your environments using multiple accounts in AWS is recommended for operational excellence, security, reliability, and cost optimization. Here, It will be elaborate on how we can parameterize and configure the Jenkins pipeline to deploy the CDK stack into different AWS environments(Accounts). We can think of several ideas to achieve this goal. Here are a few of them.

1. Manage AWS account-related configurations in the 'cdk.json' file.
2. Save account-related configurations in [AWS Systems Manager Parameter Store](https://docs.aws.amazon.com/systems-manager/latest/userguide/systems-manager-parameter-store.html) and retrieve values when needed.
3. Maintain a separate configuration file to have account-related configurations.

This boilerplate will show how to manage AWS account-related configurations in 'cdk.json' file.
