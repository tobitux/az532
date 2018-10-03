<Query Kind="Program">
  <NuGetReference>WindowsAzure.Storage</NuGetReference>
  <Namespace>Microsoft.WindowsAzure.Storage</Namespace>
  <Namespace>Microsoft.WindowsAzure.Storage.Table</Namespace>
  <Namespace>Microsoft.WindowsAzure.Storage.Table.Queryable</Namespace>
</Query>

/// https://docs.microsoft.com/en-us/azure/cosmos-db/table-storage-how-to-use-dotnet
async void Main()
{
	var connectionString = Util.GetPassword("AzureStorageConnectionString");

	var storageAccount = CloudStorageAccount.Parse(connectionString);

	var client = storageAccount.CreateCloudTableClient();

	var table = client.GetTableReference($"storage{Guid.NewGuid().ToString("N")}");
	table.CreateIfNotExists();

	var customer1 = new CustomerEntity("Knapp", "Tobias");
	customer1.Email = "mail@tobias.de";
	customer1.PhoneNumber = "03012334550";

	var insertOperation = TableOperation.Insert(customer1);

	table.Execute(insertOperation);

	var customer2 = new CustomerEntity("Mendau", "Johanna");
	customer2.Email = "mail@johanna.de";
	customer2.PhoneNumber = "03012334551";

	var customer3 = new CustomerEntity("Mendau", "Luis");
	customer3.Email = "mai@jakob.de";
	customer3.PhoneNumber = "03012334553";

	var batchOperation = new TableBatchOperation();
	batchOperation.Insert(customer2);
	batchOperation.Insert(customer3);

	table.ExecuteBatch(batchOperation);

	var query = new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Mendau"));
	table.ExecuteQuery(query).Dump();

	Console.WriteLine("Fluent");
	query = new TableQuery<CustomerEntity>().Where(
		TableQuery.CombineFilters(
			TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Mendau"),
			TableOperators.And,
			TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, "L")));

	table.ExecuteQuery(query).Dump();

	Console.WriteLine("LINQ");
	query = (from ce in table.CreateQuery<CustomerEntity>()
			 where ce.PartitionKey.Equals("Mendau") && ce.RowKey.CompareTo("L") < 0
			 select ce).AsTableQuery();

	table.ExecuteQuery(query).Dump();

	var retrieveOperation = TableOperation.Retrieve<CustomerEntity>("Mendau", "Luis");

	var retrievedResult = table.Execute(retrieveOperation);
	retrievedResult?.Dump();

	var updateEntity = (CustomerEntity)retrievedResult.Result;
	updateEntity.PhoneNumber = "0403435435";

	var updateOperation = TableOperation.Replace(updateEntity);

	table.Execute(updateOperation);

	var customer4 = new CustomerEntity("Knapp", "Tobias");
	customer4.Email = "mail@tobias.de";
	customer4.PhoneNumber = "21321321332";

	var insertOrReplaceOperation = TableOperation.InsertOrReplace(customer4);

	table.Execute(insertOrReplaceOperation);

	table.ExecuteQuery(new TableQuery<CustomerEntity>()).Dump();

	var projectionQuery = new TableQuery<DynamicTableEntity>().Select(new string[] { "Email" });

	EntityResolver<string> resolver = (pk, rk, ts, props, etag) => props.ContainsKey("Email") ? props["Email"].StringValue : null;

	table.ExecuteQuery(projectionQuery, resolver, null, null).Dump();

	var deleteOperation = TableOperation.Delete(customer4);

	table.Execute(deleteOperation);


	var tableQuery = new TableQuery<CustomerEntity>();

	TableContinuationToken continuationToken = null;

	do
	{
		var tableQueryResult = await table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

		continuationToken = tableQueryResult.ContinuationToken;

		tableQueryResult.Dump();

	} while (continuationToken != null);

	table.DeleteIfExists();
}

public class CustomerEntity : TableEntity
{
	public CustomerEntity() { }

	public CustomerEntity(string lastName, string firstName)
	{
		this.PartitionKey = lastName;
		this.RowKey = firstName;
	}

	public string Email { get; set; }

	public string PhoneNumber { get; set; }
}