<Query Kind="Program">
  <NuGetReference>Microsoft.Azure.ServiceBus</NuGetReference>
  <Namespace>Microsoft.Azure.ServiceBus</Namespace>
  <Namespace>Microsoft.Azure.ServiceBus.Core</Namespace>
  <Namespace>Microsoft.Azure.ServiceBus.Diagnostics</Namespace>
  <Namespace>Microsoft.Azure.ServiceBus.InteropExtensions</Namespace>
  <Namespace>Microsoft.Azure.ServiceBus.Management</Namespace>
  <Namespace>Microsoft.Azure.ServiceBus.Primitives</Namespace>
  <Namespace>Microsoft.Azure.Services.AppAuthentication</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	var serviceBusConnectionString = Util.GetPassword("ServiceBusConnectionString");
	var queue = "test";

	var client = new QueueClient(serviceBusConnectionString, queue);

	client.RegisterMessageHandler((m, ct) =>
	{
		var body = m.Body;
		
		var message = Encoding.Unicode.GetString(body);
		
		message.Dump();
		
		return Task.CompletedTask;
		
	}, new MessageHandlerOptions((args) =>
	{
		return Task.CompletedTask;
	}));
	
	Util.ReadLine();
	
	client.CloseAsync();
}