using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Zohan.SampleWebCore.Models;
using Zohan.SampleWebCore.Repositories;

namespace Zohan.SampleWebCore.Controllers
{
    [Produces("application/json")]
    [Route("api/Test")]
    public class TestController : Controller
    {
        #region Data Members

        private readonly DocumentDbRepositoryCore _repository;

        #endregion

        #region Constructors

        public TestController(IOptions<DocumentDbSettings> documentDbSettings)
        {
            // Instantiate and save an instance of the repository for 
            // later use within the controller.
            _repository = new DocumentDbRepositoryCore(documentDbSettings.Value);
        }

        #endregion

        #region Public Methods

        [HttpPost]
        public async Task Post([FromBody] string value)
        {
            await _repository.Initialize();            
        }

        #endregion
    }
}