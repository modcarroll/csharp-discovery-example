# C#/.NET Watson Discovery Sample Application

## NOTE: This readme is a work in progress

## Included components

* [Watson Discovery](https://www.ibm.com/watson/services/discovery/): A cognitive search and content analytics engine for applications to identify patterns, trends, and actionable insights.

## Steps

1. [Clone the repo](#1-clone-the-repo)
1. [Create IBM Cloud services](#2-create-ibm-cloud-services)
1. [Load the Discovery files](#3-load-the-discovery-files)
1. [Configure credentials](#4-configure-credentials)
1. [Run the application](#5-run-the-application)

### 1. Clone the repo

```bash
git clone https://github.com/modlanglais/csharp-discovery-example
```

### 2. Create IBM Cloud services

Create the following services:

* [**Watson Discovery**](https://cloud.ibm.com/catalog/services/discovery)

### 3. Load the Discovery files

Launch the **Watson Discovery** tool. Create a **new data collection**
by selecting the **Update your own data** option. Give the data collection a unique name.

When prompted to get started by **uploading your data**, select and upload your files. Once uploaded, you can then use the `Configure data` option to add the `Keyword Extraction` enrichment.

> Note: failure to do this will result in no `keywords` being shown in the app.

Once the enrichments are selected, use the `Apply changes to collection` button. **Warning** - this make take several minutes to complete.

> There may be a limit to the number of files you can upload, based on your IBM Cloud account permissions.

### 4. Configure credentials

Adding a .env file would be simpler, but for this iteration of this application the credentials will just be hard-corded. **Warning** Do NOT upload your application without first hiding your credentials!

Edit the `Program.cs` file to add your credentials on the following lines:

```bash
// Add the environment and collection IDs here
String environmentId = "{environment-id}";
String collectionId = "{collection-id}";

TokenOptions tokenOptions = new TokenOptions()
{
    // Add your URL and API key here
    IamApiKey = "{api-key}",
    ServiceUrl = "{url}"
};

// Add the Discovery version here
DiscoveryService service = new DiscoveryService(tokenOptions, "{version}");

// Add the query you would like to use here, i.e. "purple houses"
String inputQuery = "{your natural language query here}";
```

### 5. Run the application

1. Install .Net, etc.
1. Start the app by running `dotnet run`.
1. This is a command line application, so there is no UI.
