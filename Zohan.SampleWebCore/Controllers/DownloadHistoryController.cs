using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using Zohan.SampleWebCore.Models;
using Zohan.SampleWebCore.Repositories;

namespace Zohan.SampleWebCore.Controllers
{
    [Produces("application/json")]
    [Route("api/DownloadHistory")]
    public class DownloadHistoryController : Controller
    {
        #region Data Members
       
        private readonly DocumentDbRepositoryCore _repository;

        #endregion

        #region Constructors

        public DownloadHistoryController(IOptions<DocumentDbSettings> documentDbSettings)
        {
            // Instantiate and save an instance of the 
            // repository for later use.
            _repository = new DocumentDbRepositoryCore(documentDbSettings.Value);                        
        }

        #endregion

        #region Public Methods

        // GET api/downloadhistory/acme
        [HttpGet("{customerName}")]
        public async Task<List<DownloadRequest>> GetDownloadRequestsAsync(string customerName)
        {
            //return _repository.GetCustomerDownloadRequests(customerName);
            return await _repository.GetCustomerDownloadRequestAsync(customerName);
        }

        // POST api/downloadhistory
        [HttpPost]
        public async Task Post([FromBody]DownloadRequest request)
        {
            await _repository.AddDownloadRequestAsync(request);
        }

        #endregion

    }
}