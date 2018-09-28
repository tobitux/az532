<Query Kind="Statements">
  <NuGetReference>WindowsAzure.Storage</NuGetReference>
  <Namespace>Microsoft.WindowsAzure.Storage</Namespace>
  <Namespace>Microsoft.WindowsAzure.Storage.Blob</Namespace>
</Query>

var connectionString = Util.GetPassword("AzureStorageConnectionString");

// Create a Container
var storageAccount = CloudStorageAccount.Parse(connectionString);
var blobClient = storageAccount.CreateCloudBlobClient();
var containerName = Guid.NewGuid().ToString();
var container = blobClient.GetContainerReference(containerName);
container.CreateIfNotExists();

// Upload a Blob
var blockBlobName = Guid.NewGuid().ToString();
var blockBlob = container.GetBlockBlobReference(blockBlobName);
blockBlob.UploadText("Hello World");

// Fetch Latest Container Attributes
container.FetchAttributes();
container.Dump();

// Add Metadata to Container
container.Metadata.Add("Author","Tobias Knapp");
container.Metadata["AuthoredOn"] = DateTime.UtcNow.ToString();
container.SetMetadata();

// Fetch Latest Container Attributes
container.FetchAttributes();
container.Metadata.Dump();

// Create subdirectory and copy existing blob into it
var directory = container.GetDirectoryReference("Folder");
var subdirectory = directory.GetDirectoryReference("Subfolder");

var blockBlob2 = subdirectory.GetBlobReference(blockBlob.Name + "-Copy");
var task = blockBlob2.StartCopyAsync(new Uri(blockBlob.Uri.AbsoluteUri));
task.Wait();
Console.WriteLine("Copied");