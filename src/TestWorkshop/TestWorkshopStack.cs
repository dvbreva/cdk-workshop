using Amazon.CDK;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Eladb.DynamoTableViewer;

namespace TestWorkshop
{
    public class TestWorkshopStack : Stack
    {
        public TestWorkshopStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            var hello = new Function(this, "HelloHandler", new FunctionProps
            {
                Runtime = Runtime.NODEJS_10_X,
                Code = Code.FromAsset("lambda"), //code is loaded from the lambda directory
                Handler = "hello.handler" //file is 'hello' but function is 'handler'
            });

            var helloCounter = new HitCounter(this, "HelloHitCounter", new HitCounterProps
            {
                Downstream = hello
            });

            // defines an API Gateway REST API resource backed by our "hello" function.
            new LambdaRestApi(this, "Endpoint", new LambdaRestApiProps
            {
                Handler = helloCounter.Handler
            });

            // Defines a new TableViewer resource
            new TableViewer(this, "ViewerHitCount", new TableViewerProps
            {
                Title = "Hello Hits",
                Table = helloCounter.MyTable
            });
        }
    }
}
