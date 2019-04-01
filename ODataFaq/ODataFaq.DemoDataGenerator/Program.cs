using ODataFaq.DataModel;
using System;
using System.Threading.Tasks;

namespace ODataFaq.DemoDataGenerator
{
	class Program
	{
		static async Task Main(string[] _)
		{
			Console.WriteLine("Press any key to continue with clearing the database 'ODataFaq' on (localdb)\\v11.0 and filling it with demo data ...");
			Console.ReadKey();

			Console.WriteLine("Please be patient, generating data ...");
			using (var omc = new OrderManagementContext())
			{
				await omc.ClearAndFillWithDemoDataAsync();
			}

			Console.WriteLine("Done. Press any key to quit ...");
			Console.ReadKey();
		}
	}
}
