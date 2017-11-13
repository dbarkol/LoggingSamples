using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.AzureAppServices.Internal;

namespace Zohan.SampleWebCore.Models
{
    public class DocumentDbSettings
    {
        #region Properties

        public string Database { get; set; }

        public string Collection { get; set; }

        public string AuthKey { get; set; }

        public string Endpoint { get; set; }

        #endregion

        internal static DocumentDbSettings GetDbSettings()
        {
            var settings = new DocumentDbSettings
            {
                AuthKey = Environment.GetEnvironmentVariable("AuthKey"),
                Collection = Environment.GetEnvironmentVariable("Collection"),
                Endpoint = Environment.GetEnvironmentVariable("Endpoint"),
                Database = Environment.GetEnvironmentVariable("Database")
            };       

            return settings;
        }
    }
}

