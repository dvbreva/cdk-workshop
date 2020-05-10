using Amazon.CDK;

namespace TestWorkshop
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new TestWorkshopStack(app, "TestWorkshopStack");

            app.Synth();
        }
    }
}
