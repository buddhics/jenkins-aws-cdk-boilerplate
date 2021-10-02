using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CdkBoilerplate
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new CdkBoilerplateStack(app, "CdkBoilerplateStack", new StackProps());
            app.Synth();
        }
    }
}
