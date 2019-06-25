using IBM.Cloud.SDK.Core.Util;
using Environment = IBM.Watson.Discovery.v1.Model.Environment;
using System;
using DiscoveryService = IBM.Watson.Discovery.v1.DiscoveryService;
using System.Collections.Generic;

namespace DiscoveryExample
{
    class Program
    {
        static void Main(string[] args)
        {
            String environmentId = "{environment-id}";
            String collectionId = "{collection-id}";
            Console.WriteLine("Hello World!");

            TokenOptions tokenOptions = new TokenOptions()
            {
                IamApiKey = "{api-key}",
                ServiceUrl = "{url}"
            };

            DiscoveryService service = new DiscoveryService(tokenOptions, "{version}");

            // List environments
            Console.WriteLine("**********List Environments**********");
            var envResult = service.ListEnvironments();
            Console.WriteLine("My environments: " + envResult.Response); 
            foreach (Environment environment in envResult.Result.Environments)
            {
                if (environment._ReadOnly != true)
                {
                    Console.WriteLine("Environment ID: " + environment.EnvironmentId);
                }
            }

            // List Fields
            Console.WriteLine("**********List Fields**********");
            var fieldsResult = service.ListFields(environmentId, new List<string>() { collectionId });
            Console.WriteLine(fieldsResult.Response);

            // List Collections

            // Query()
            String inputQuery = "{your natural language query here}";
            Console.WriteLine("**********Natural Language Query**********");
            Console.WriteLine("Searching for " + inputQuery);
            var queryResult = service.Query(
                environmentId,
                collectionId,
                naturalLanguageQuery: inputQuery
                );

            String resultString = queryResult.Response;
            Console.WriteLine("output: " + resultString);

            System.IO.File.WriteAllText(@"WriteLines.json", resultString);
        }
    }
}
