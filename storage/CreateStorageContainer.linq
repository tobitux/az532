<Query Kind="Statements">
  <NuGetReference>WindowsAzure.Storage</NuGetReference>
  <Namespace>Microsoft.WindowsAzure.Storage</Namespace>
  <Namespace>Microsoft.WindowsAzure.Storage.Blob</Namespace>
</Query>

var connectionString = Util.GetPassword("BlobConnectionString");

var storageAccount = CloudStorageAccount.Parse(connectionString);
var blobClient = storageAccount.CreateCloudBlobClient();
var containerName = Guid.NewGuid().ToString();
var container = blobClient.GetContainerReference(containerName);
container.CreateIfNotExists();

var blockBlobName = Guid.NewGuid().ToString();
var blockBlob = container.GetBlockBlobReference(blockBlobName);
blockBlob.UploadText("Hello World");

container.FetchAttributes();
container.Dump();

container.Metadata.Add("Author","Tobias Knapp");
container.Metadata["AuthoredOn"] = DateTime.UtcNow.ToString();
container.SetMetadata();

container.Metadata.Dump();
