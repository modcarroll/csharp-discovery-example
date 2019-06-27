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
            string environmentId = "{environment ID}";
            string collectionId = "{collection ID}";
            string documentId = "{doc ID if needed}";
            string version = "{discovery version}";
            string inputQuery = "blue and yellow purple houses";
            string metadata = "{\"Creator\": \"Morgan Langlais\"}"; // for testing

            TokenOptions tokenOptions = new TokenOptions()
            {
                // Add your URL and API key here
                IamApiKey = "{discovery api key}",
                ServiceUrl = "{discovery api url}"
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


            /*
             * Get all Document IDs from a collection 
            */
            var getCollectionResponse = service.GetCollection(
                environmentId,
                collectionId
                );

            string collecResponse = getCollectionResponse.Response;
            // TO-DO: Deserialize instead of trimming the string
            int pFrom = collecResponse.IndexOf("available");

            string resultDocNum = collecResponse.Substring(pFrom, 20);
            resultDocNum = Regex.Replace(resultDocNum, "[^0-9]", "");

            // docsInCollection is the total number of documents in the collection
            int docsInCollection = Int32.Parse(resultDocNum);
            Console.WriteLine("The total number of documents in collection #" + collectionId + " is: " + docsInCollection);

            string allDocIds = "";

            // Will only work for collections of size 10000 or less
            // This gets a list of the document IDs
            var docidResult = service.Query(
            environmentId,
            collectionId,
            returnFields: "id",
            count: 10000
            );

            string docidString = docidResult.Response;

            var root = JsonConvert.DeserializeObject<Idreturn>(docidString);

            // allDocIds is a comma separated list of all document IDs in the collection
            allDocIds += string.Join(",", root.results.Select(item => item.id));

            string outputDir = @"./docIds.txt";
            System.IO.File.WriteAllText(outputDir, allDocIds);
            Console.WriteLine("All Document IDs have been placed in " + outputDir);
            /**/

            // Upload a document to Discovery, replace somefilenamehere.json with your filename
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


            // Update a specific document, replace austinreview.json with your filename
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
