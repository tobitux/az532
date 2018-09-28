<Query Kind="Statements">
  <NuGetReference>WindowsAzure.Storage</NuGetReference>
  <Namespace>Microsoft.WindowsAzure.Storage</Namespace>
  <Namespace>Microsoft.WindowsAzure.Storage.Blob</Namespace>
</Query>

var connectionString = Util.GetPassword("BlobConnectionString");

var storageAccount = CloudStorageAccount.Parse(connectionString);
var blobClient = storageAccount.CreateCloudBlobClient();
var conainerName = Guid.NewGuid().ToString();

var container = blobClient.GetContainerReference(conainerName);

container.CreateIfNotExists();