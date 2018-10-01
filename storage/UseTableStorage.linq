<Query Kind="Program">
  <NuGetReference>WindowsAzure.Storage</NuGetReference>
  <Namespace>Microsoft.WindowsAzure.Storage</Namespace>
  <Namespace>Microsoft.WindowsAzure.Storage.Table</Namespace>
</Query>

void Main()
{
	var r = new Random();

	var cs = Util.GetPassword("AzureStorageConnectionString");

	CloudStorageAccount cloudStorage = CloudStorageAccount.Parse(cs);

	CloudTableClient tableClient = cloudStorage.CreateCloudTableClient();

	CloudTable table = tableClient.GetTableReference("table");

	table.CreateIfNotExists();

	var id = r.Next();
	var colors = new[] { "red", "green", "blue", "yellow" };

	var car = new CarEntity
	{
		Id = id,
		Year = r.Next(1900, DateTime.Today.Year),
		Make = "Ford",
		Model = "Mustang",
		Color = colors[r.Next(colors.Length)]
	};

	// insert car
	var insert = TableOperation.Insert(car);
	table.Execute(insert);

	// retrieve cars by query
	table.ExecuteQuery(table.CreateQuery<CarEntity>()).Where(c => c.Year < 2000).ToList().Dump();

<<<<<<< HEAD
	table.ExecuteQuery(table.CreateQuery<CarEntity>()).Where(c => c.Year >= 2000).ToList().Dump();

=======
	// retrieve single car
	var retrieve = TableOperation.Retrieve<CarEntity>("car", id.ToString());
>>>>>>> 13417b1fad5a34f59809ca124535b6a9b692877e
	var result = table.Execute(retrieve);
	result.Dump();

	var ca = (CarEntity)result.Result;
	ca.Dump();

	// Transactions
	var tbo = new TableBatchOperation();
	tbo.Insert(new CarEntity { Id = 1001, Year = 2012, Make = "Honda", Model = "Civic", Color = "red" });
	tbo.Insert(new CarEntity { Id = 1002, Year = 2018, Make = "BMW", Model = "X1", Color = "red" });
	// Try to add 2 Items with same Id
	tbo.Insert(new CarEntity { Id = 1003, Year = 2017, Make = "BMW", Model = "X2", Color = "white" });
	tbo.Insert(new CarEntity { Id = 1003, Year = 2016, Make = "BMW", Model = "X5", Color = "black" });

	try
	{
		table.ExecuteBatch(tbo);
	}
	catch (StorageException ex)
	{
		Console.WriteLine("No cars inserted.");
		ex.Dump();
	}
}

public class CarEntity : TableEntity
{
	private int _id;
	public CarEntity()
	{
		PartitionKey = "car";
		this.RowKey = Id.ToString();
	}
	public int Id
	{
		get
		{
			return _id;
		}
		set
		{
			_id = value;
			RowKey = _id.ToString();
		}
	}

	public int Year { get; set; }
	public string Make { get; set; }
	public string Model { get; set; }
	public string Color { get; set; }
}

// Define other methods and classes here