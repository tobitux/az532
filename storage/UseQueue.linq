<Query Kind="Statements">
  <NuGetReference>WindowsAzure.Storage</NuGetReference>
  <Namespace>Microsoft.WindowsAzure.Storage</Namespace>
  <Namespace>Microsoft.WindowsAzure.Storage.Queue</Namespace>
</Query>

var connectionString = Util.GetPassword("AzureStorageConnectionString");

var storageAccount = CloudStorageAccount.Parse(connectionString);

var client = storageAccount.CreateCloudQueueClient();

// Queue names must be lowercase!
var queue = client.GetQueueReference("myqueue");
queue.CreateIfNotExists();

// clear queue
queue.Clear();

// Add message to queue
var message = new CloudQueueMessage($"foo {DateTime.UtcNow.ToString()}");
queue.AddMessage(message);

// Read message from queue but not removing it
var receivedMessage = queue.PeekMessage();
receivedMessage.Dump();

// Read and remove message from queue
receivedMessage = queue.GetMessage();
receivedMessage.Dump();

