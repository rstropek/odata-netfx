using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HelloOData
{
    public static class Program
    {
        static void Main(string[] args)
        {
            const string baseUrl = "http://localhost:8082/";

            using (WebApp.Start<Startup>(url: baseUrl))
            {
                Console.WriteLine("The server is running...");
                Console.ReadKey();
            }
        }
    }
}
