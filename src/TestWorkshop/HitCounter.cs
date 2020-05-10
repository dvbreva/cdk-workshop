using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.Lambda;

namespace TestWorkshop
{
    public class HitCounter : Construct
    {
        public HitCounter(Construct scope, string id, HitCounterProps props) : base(scope, id)
        {
            var table = new Table(this, "Hits", new TableProps
            {
                PartitionKey = new Attribute
                {
                    Name = "path",
                    Type = AttributeType.STRING
                }
            });

            MyTable = table;

            Handler = new Function(this, "HitCounterHandler", new FunctionProps
            {
                Runtime = Runtime.NODEJS_10_X,
                Handler = "hitcounter.handler",
                Code = Code.FromAsset("lambda"),
                Environment = new Dictionary<string, string>
                {
                    ["DOWNSTREAM_FUNCTION_NAME"] = props.Downstream.FunctionName,
                    ["HITS_TABLE_NAME"] = table.TableName
                }
            });

            // Grant the lambda role read/write permissions to our table
            table.GrantReadWriteData(Handler);

            // Grant the lambda role invoke permissions to the downstream function
            props.Downstream.GrantInvoke(Handler);

        }

        public Function Handler { get; }

        public Table MyTable { get; }
    }

    public class HitCounterProps
    {
        public IFunction Downstream { get; set; } // function for which we want to count url hits
    }
}
