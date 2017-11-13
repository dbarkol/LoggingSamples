using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using Zohan.SampleWebCore.Models;

namespace Zohan.SampleWebCore.Repositories
{
    public class DocumentDbRepositoryCore
    {
        #region Data Members

        private readonly DocumentDbSettings _documentDbSettings;
        private readonly DocumentClient _documentClient;

        #endregion

        #region Constructors

        //public DocumentDbRepositoryCore(IOptions<DocumentDbSettings> docDbSettings)
        public DocumentDbRepositoryCore(DocumentDbSettings documentDbSettings)
        {
            // Save the document DB settings and instantiate an
            // instance of the client for later use.
            //_documentDbSettings = docDbSettings.Value;
            _documentDbSettings = documentDbSettings;
            _documentClient = new DocumentClient(
                new Uri(_documentDbSettings.Endpoint),
                _documentDbSettings.AuthKey);
        }

        public DocumentDbRepositoryCore() : this(DocumentDbSettings.GetDbSettings()){
        }

        #endregion

        #region Public Methods

        public async Task Initialize()
        {
            // Create the database and collection if they 
            // do not already exist.
            await CreateDatabaseIfNotExistsAsync();
            await CreateCollectionIfNotExistsAsync();

            // Seed the collection with some products to work with. 
            //var downloadRequest = new DownloadRequest()
            //{
            //   Id = "1",
            //   CustomerName = "Contoso",
            //   RequestType = "Download",
            //   RequestedDateTime = DateTime.Now,
            //   RequestedFileName = "contoso.lic"
            //};
            //await CreateDownloadRequestDocumentIfNotExists(downloadRequest);
        }

        public async Task<List<DownloadRequest>> GetCustomerDownloadRequestAsync(string customerName)
        {
            var queryOptions = new FeedOptions { MaxItemCount = -1 };
            var query = _documentClient.CreateDocumentQuery<DownloadRequest>(this.CollectionUri, queryOptions)
                .Where(x => x.CustomerName.ToLower() == customerName.ToLower())
                .AsDocumentQuery();

            var results = new List<DownloadRequest>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<DownloadRequest>());
            }
            return results.ToList();
        }

        public async Task AddDownloadRequestAsync(DownloadRequest request)
        {
            await CreateDownloadRequestDocumentIfNotExists(request);
        }

        #endregion

        #region Private Methods

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                var databaseUri = UriFactory.CreateDatabaseUri(_documentDbSettings.Database);
                await _documentClient.ReadDatabaseAsync(databaseUri);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var database = new Database { Id = _documentDbSettings.Database };
                    await _documentClient.CreateDatabaseAsync(database);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                var collectionUri = UriFactory.CreateDocumentCollectionUri(_documentDbSettings.Database, _documentDbSettings.Collection);
                await _documentClient.ReadDocumentCollectionAsync(collectionUri);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var databaseUri = UriFactory.CreateDatabaseUri(_documentDbSettings.Database);
                    await _documentClient.CreateDocumentCollectionAsync(databaseUri,
                        new DocumentCollection { Id = _documentDbSettings.Collection },
                        new RequestOptions { OfferThroughput = 400 });
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateDownloadRequestDocumentIfNotExists(DownloadRequest downloadRequest)
        {
            try
            {
                var documentUri = UriFactory.CreateDocumentUri(_documentDbSettings.Database, _documentDbSettings.Collection, downloadRequest.Id);
                await _documentClient.ReadDocumentAsync(documentUri);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    var documentCollectionUri = UriFactory.CreateDocumentCollectionUri(_documentDbSettings.Database, _documentDbSettings.Collection);
                    await _documentClient.CreateDocumentAsync(documentCollectionUri, downloadRequest);
                }
                else
                {
                    throw;
                }
            }
        }

        #endregion

        #region Properties

        private Uri CollectionUri => UriFactory.CreateDocumentCollectionUri(_documentDbSettings.Database, _documentDbSettings.Collection);

        #endregion

    }
}
