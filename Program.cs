/* Author: Morgan Langlais, IBM, morganlanglais@ibm.com
 * Purpose: Tests re-ingestion of Watson Discovery datas
 * June 2019
 */

using IBM.Cloud.SDK.Core.Util;
using Environment = IBM.Watson.Discovery.v1.Model.Environment;
using System;
using DiscoveryService = IBM.Watson.Discovery.v1.DiscoveryService;
using IBM.Cloud.SDK.Core.Http;
using System.IO;
using IBM.Watson.Discovery.v1.Model;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;

namespace DiscoveryExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Add the environment and collection IDs here
            string environmentId = "ae1ce105-7144-4175-91c2-73b4d399d011";
            string collectionId = "44a6cc77-2c74-428b-9a8c-f03b1a09948e";
            string documentId = "b2a0a7e2-bfc9-4457-aa23-dad910b96554";
            string version = "2019-04-30";
            string inputQuery = "blue and yellow purple houses";
            string metadata = "{\"Creator\": \"Morgan Langlais\"}";

            TokenOptions tokenOptions = new TokenOptions()
            {
                // Add your URL and API key here
                IamApiKey = "N1qjIeEBK7C_GWefO5orKs2iBEhDnfWhGmrbTB-1f6yz",
                ServiceUrl = "https://gateway.watsonplatform.net/discovery/api"
            };

            // Add the Discovery version here
            DiscoveryService service = new DiscoveryService(tokenOptions, version);

            // List environments
            //Console.WriteLine("**********List Environments**********");
            //var envResult = service.ListEnvironments();
            //Console.WriteLine("My environments: " + envResult);
            //foreach (Environment environment in envResult.Result.Environments)
            //{
            //    if (environment._ReadOnly != true)
            //    {
            //        Console.WriteLine("Environment ID: " + environment.EnvironmentId);
            //    }
            //}

            // Query()
            // Add the query you would like to use here, i.e. "purple houses"
            //Console.WriteLine("**********Natural Language Query**********");
            //Console.WriteLine("Searching for " + inputQuery);
            //var queryResult = service.Query(
            //    environmentId,
            //    collectionId,
            //    naturalLanguageQuery: inputQuery
            //    );

            //string resultString = queryResult.Response;
            //Console.WriteLine("output: " + resultString);

            /* Get all Document IDs from a collection */
            var getCollectionResponse = service.GetCollection(
                environmentId,
                collectionId
                );

            string collecResponse = getCollectionResponse.Response;
            // TO-DO: Deserialize instead of trimming the string
            int pFrom = collecResponse.IndexOf("available");

            string resultDocNum = collecResponse.Substring(pFrom, 20);
            resultDocNum = Regex.Replace(resultDocNum, "[^0-9]", "");
            int docsInCollection = Int32.Parse(resultDocNum);

            int iterations = docsInCollection / 5000;
            string allDocIds = "";
            int offset = 0;
            int count = 10000;

            // Ok but what is the maximum number of documents you can fit in a collection?

            iterations = 5;

            for(int i = 0; i < iterations + 1; i++)
            {
                //var docidResult = service.Query(
                //environmentId,
                //collectionId,
                //returnFields: "id",
                //count: count,
                //sort: documentId, // Probably not working
                //offset: offset
                //);

                //string docidString = docidResult.Response;

                //var root = JsonConvert.DeserializeObject<Idreturn>(docidString);
                //allDocIds += string.Join(",", root.results.Select(item => item.id));

                // offset + count must be <= 10000

                // First call: 0 - 10000, subsequent calls should be 5000 count and increment offset by 5000
                // This is working so offset is: 5000, 10000, 15000, 20000, ...
                // BUT offset + count must be less than or equal to 10000
                if(i == 0) { offset = 10000;  count = 5000; } else { offset += 5000; }
                Console.WriteLine("*** " + i + ":");
                Console.WriteLine("offset: " + offset);
                Console.WriteLine("count: " + count);
                Console.WriteLine();

            }

            Console.WriteLine("All Document IDs: " + allDocIds);

            // Upload a document to Discovery
            //DetailedResponse<DocumentAccepted> uploadResult;
            //using (FileStream fs = File.OpenRead("./somefilenamehere.json"))
            //{
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        fs.CopyTo(ms);
            //        uploadResult = service.AddDocument(
            //        environmentId: environmentId,
            //        collectionId: collectionId,
            //        file: ms,
            //        filename: "somefilenamehere.json",
            //        fileContentType: "text/html",
            //        metadata: metadata
            //        );
            //    }
            //}
            //Console.WriteLine("******Upload Results******");
            //Console.WriteLine("Add results: " + uploadResult.Response);

            // Get a specific document's status
            //var getResult = service.GetDocumentStatus(
            //    environmentId: environmentId,
            //    collectionId: collectionId,
            //    documentId: documentIds
            //    );
            //Console.WriteLine("Get doc info results: " + getResult.Response);

            // Update a specific document
            //DetailedResponse<DocumentAccepted> updateResult;
            //using (FileStream fs = File.OpenRead("./austinreview.json"))
            //{
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        fs.CopyTo(ms);
            //        updateResult = service.UpdateDocument(
            //            environmentId: environmentId,
            //            collectionId: collectionId,
            //            documentId: documentId,
            //            file: ms,
            //            filename: "austinreview.json",
            //            fileContentType: "text/html",
            //            metadata: metadata
            //            );
            //    }
            //}

            //Console.WriteLine("Update results: ", updateResult.Response);

            // Delete a specific document
            //var deleteResult = service.DeleteDocument(
            //    environmentId: environmentId,
            //    collectionId: collectionId,
            //    documentId: documentId
            //    );

            //Console.WriteLine(deleteResult.Response);
        }
    }

    /* Classes used to deserialize response data for extracting document IDs */
    public class Idreturn
    {
        public int matching_results { get; set; }
        public string session_token { get; set; }
        public Result[] results { get; set; }
        public RetrievalDetails retrieval_details { get; set; }
    }

    public class RetrievalDetails
    {
        public string retrieval_details { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public Result_Metadata result_metadata { get; set; }
    }

    public class Result_Metadata
    {
        public int score { get; set; }
        public int confidence { get; set; }
    }
}
