<Query Kind="Program">
  <NuGetReference>Microsoft.Azure.ServiceBus</NuGetReference>
  <Namespace>Microsoft.Azure.ServiceBus</Namespace>
  <Namespace>Microsoft.Azure.ServiceBus.Core</Namespace>
  <Namespace>Microsoft.Azure.ServiceBus.Diagnostics</Namespace>
  <Namespace>Microsoft.Azure.ServiceBus.InteropExtensions</Namespace>
  <Namespace>Microsoft.Azure.ServiceBus.Management</Namespace>
  <Namespace>Microsoft.Azure.ServiceBus.Primitives</Namespace>
</Query>

private Random r = new Random();

void Main()
{
	
	
	var serviceBusConnectionString = Util.GetPassword("ServiceBusConnectionString");
	var queue ="test";
	
	var client = new QueueClient(serviceBusConnectionString, queue);
	
	for (int i = 0; i < 1000; i++)
	{
		var message = GetRandomMessage();
		
		message.Dump();
		
		var body = Encoding.Unicode.GetBytes(message);
	
		client.SendAsync(new Message(body)).Wait();
		
		Thread.Sleep(r.Next(1000));
	}	
}

public string GetRandomMessage()
{
	var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

	var sb = new StringBuilder();
	
	for (int i = 0; i < 100; i++)
		sb.Append(chars[r.Next(chars.Length)]);
		
	return sb.ToString();
}
