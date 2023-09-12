using System.Configuration;
using Confluent.Kafka;
// using Microsoft.Extensions.Configuration;

class Program
{
	private static void RunConsumer()
	{
		ConsumerConfig config = new ConsumerConfig
		{
			BootstrapServers = ConfigurationManager.AppSettings["kafka"],
			GroupId = "test",
			AutoOffsetReset = AutoOffsetReset.Earliest
		};
		IConsumer<string, string> consumer = new ConsumerBuilder< string, string>(config).Build();
		consumer.Subscribe("test");

		var result = consumer.Consume();

		Console.WriteLine("Consumed: " + result.Value);
	}
	public static async  Task Main()
	{
		Console.WriteLine("kafka");
		try
		{
			Thread consume = new Thread(() =>
			{
				RunConsumer();
			});
			
			ProducerConfig config = new ProducerConfig()
			{
				BootstrapServers = ConfigurationManager.AppSettings["kafka"]
			};
			IProducer<string, string> producer = new ProducerBuilder<string, string>(config).Build();
			var result = await producer.ProduceAsync("test", new Message<string, string>() { Key = "first", Value = "first message" });
			Console.WriteLine("Result: " + result.Status.ToString());
			consume.Start();
		}
		catch(Exception e)
		{
			Console.WriteLine("Exception: " + e.Message);
		}

	}
}
	
