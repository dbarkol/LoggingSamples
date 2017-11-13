using System;
using System.Threading.Tasks;
using Zohan.SampleWebCore.Models;
using Zohan.SampleWebCore.Repositories;

namespace Zohan.LoggingTests
{
    class Program
    {
        #region Data Members

        private static DocumentDbSettings _documentDbSettings;
        private static DocumentDbRepositoryCore _repository;

        #endregion

        static void Main(string[] args)
        {
            Initialize();

            // Tests
            TestGetCustomerDownloadRequestAsync().Wait();

            Console.WriteLine("Press <enter> to exit.");
            Console.ReadLine();
        }

        private static void Initialize()
        {
            _documentDbSettings = new DocumentDbSettings()
            {
                Database = "",
                Collection = "",
                AuthKey = "",
                Endpoint = ""
            };
            _repository = new DocumentDbRepositoryCore(_documentDbSettings);
        }

        private static async Task TestGetCustomerDownloadRequestAsync()
        {
            var results = await _repository.GetCustomerDownloadRequestAsync("contoso");
            foreach (var r in results)
            {
                Console.WriteLine("Result: {0} - {1}", r.CustomerName, r.RequestedDateTime.ToLongTimeString());
            }
        }
    }
}
