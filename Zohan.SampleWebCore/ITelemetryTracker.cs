using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights.DataContracts;

namespace Zohan.SampleWebCore
{
    public interface ITelemetryTracker
    {
        void TrackException(Exception ex);

        void TrackTrace(string message, SeverityLevel severity);

        void TrackTrace(string message, SeverityLevel severity, IDictionary<string, string> properties);

        void TrackEvent(string eventName, Dictionary<string, string> properties);

        Stopwatch StartTrackRequest(string requestName);

        void StopTrackRequest(string requestName, Stopwatch stopwatch);
    }
}
