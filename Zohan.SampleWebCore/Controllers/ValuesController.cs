using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace Zohan.SampleWebCore.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        #region Data Members

        private readonly ITelemetryTracker _telemetryTracker;

        #endregion

        #region Constructors

        public ValuesController(ITelemetryTracker telemetryTracker)
        {
            _telemetryTracker = telemetryTracker;
        }

        #endregion

        #region Operations

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            if (id == 5)
                TrackExceptionExample();
            else if (id >= 6)
                TrackRequestExample(id);

            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion

        #region Private Methods

        private void TrackRequestExample(int id)
        {
            // Start the stopwatch
            var stopwatch = _telemetryTracker.StartTrackRequest("Asset Lookup Request");

            // Add an informational trace message for diagnostics
            var message = $"Processing for ID {id}";
            _telemetryTracker.TrackTrace(message,
                    SeverityLevel.Warning,
                    new Dictionary<string, string> { { "Asset ID", id.ToString() } });

            // Do some processing here....
            System.Threading.Thread.Sleep(id * 500);

            // Stop the watch
            _telemetryTracker.StopTrackRequest("Asset Lookup Request", stopwatch);
        }

        private void TrackExceptionExample()
        {
            try
            {
                throw new ApplicationException("This was thrown on purpose.");

            }
            catch (Exception e)
            {
                _telemetryTracker.TrackException(new ApplicationException("This is from the web site"));                
                throw;
            }
        }

        #endregion
    }
}
