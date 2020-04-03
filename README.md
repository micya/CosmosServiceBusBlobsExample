
# CosmosServiceBusBlobsExample

Example showing use of ComsosDB ServiceBus and StorageBlobs Helper Classes in .NET Standard and .NET Core

## Disclaimer

    This example is demo code, it is not intended to demonstrate production use.

    No error checking is attempted, it is meant to break while you use it to experiment with the API's

    All use of Azure API's have been abstracted into Helper Classes which are also demo code, this allows you to see in one place how I've used the API's.

    Code has been kept small and compact as much as possible to give a simple introduction to the API's, you should consult documentation on the API's before attempting to write production code.

## Installation

    To use this example you will need to:
    
        Create a ServiceBus in Azure and a queue to use for the example. Create these and note the connection string and the queue name, you will need to configure these in the two configuration files described below.

        Create a CosmosDB instance, the database and the container will be created by the application if they do not exist, you will need your Cosmos DB Endpoint URL and an auth key in the two configuration files described below, so note these down when you create the instance.

        Create an Azure Storage Blobs instance and record the connection string also used in the two configuration files described below.

## Configuring the examples for use

    NOTE DO NOT CHECK IN ANY CONFIGURATION to a public repository, as you will in the example code in this repository, all connection strings have been removed.

    You will need to edit the web.config appSettings values and ConnectionConfiguration.cs variable values in the configuration files for the server and client projects (OTE:THIS IS AN EXAMPLE, DO NOT STORE CREDENTIALS, CONNECTION STRINGS OR HOSTNAMES IN ANY PRODUCTION CODE). The paths are below

    NetStandardWebServer - \CosmosServiceBusBlobsExample\NetStandardWebServer\WS-BS-Cosmo-Example\web.config
    NetCoreClient - \CosmosServiceBusBlobsExample\NetCoreClient\SB_Cosmo_BlobStorage_JobMonitor\ConnectionConfiguration.cs

    For both the NetStandardWebServer and NetCoreClient

        Add your Azure Service Bus connection string to the variable or key named ServiceBusCS
        Add your Azure Storage Blobs connection string to the variable or key named BlobStorageCS
        Add your CosmosDB EndpointURL and Auth Key to the variables or keys named CosmosEndPointURL and CosmosDBAuthKey.

    For just the NetCoreClient

        Add a local path to LocalStorage to save downloaded blobs, remembering to escape any slashes.


## Running the example

    Once everything is configured, load up both solutions and run the WebServer (WS-BS-Cosmo-Example) first and then the Client (SB_Cosmo_BlobStorage_JobMonitor).
    Run both the solutions.
    Upload a file on the WebServer example server.
    Observe the Client example to see it process the Service Bus request and retrieve the Blob Id from Cosmo and then download from Storage Blobs

