using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Zohan.SampleWebCore
{
    public class TelemetryTracker : ITelemetryTracker
    {
        #region Data Members

        private readonly TelemetryClient _telemetry; 

        #endregion

        #region Constructors

        public TelemetryTracker(string instrumentationKey)
        {
            _telemetry = new TelemetryClient { InstrumentationKey = instrumentationKey };
        }

        #endregion

        #region ITelemetryTracker Methods

        public void TrackException(Exception ex)
        {
            _telemetry.TrackException(ex);
        }

        public void TrackTrace(string message, SeverityLevel severity)
        {
            _telemetry.TrackTrace(message, severity);
        }

        public void TrackTrace(string message, SeverityLevel severity, IDictionary<string, string> properties)
        {
            _telemetry.TrackTrace(message, severity, properties);
        }

        public void TrackEvent(string eventName, Dictionary<string, string> properties)
        {
            _telemetry.TrackEvent(eventName, properties);

        }

        public Stopwatch StartTrackRequest(string requestName)
        {
            // Operation Id is attached to all telemetry and helps you identify
            // telemetry associated with one request:
            _telemetry.Context.Operation.Id = Guid.NewGuid().ToString();
            return Stopwatch.StartNew();

        }

        public void StopTrackRequest(string requestName, Stopwatch stopwatch)
        {
            stopwatch.Stop();
            _telemetry.TrackRequest(requestName, DateTime.Now,
                stopwatch.Elapsed,
                "200", true);  // Response code, success
        }

        #endregion
    }
}
