using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace UrlShortener.IntegrationTests.HealthCheck;

// HealthReport has no public parameterless constructor and is sealed
public class SerializableHealthReport
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HealthStatus Status { get; set; }
    
    public TimeSpan TotalDuration { get; set; }
    
    public IReadOnlyDictionary<string, SerializableHealthReportEntry> Entries { get; set; }

    public SerializableHealthReport()
    {
    }

    public HealthReport ToHealthReport()
    {
        return new HealthReport(Entries.ToDictionary(kv => kv.Key, kv => kv.Value.ToHealthReportEntry()),
            Status, TotalDuration);
    }
}

public struct SerializableHealthReportEntry
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HealthStatus Status { get; set; }
    
    public TimeSpan Duration { get; set; }
    
    public HealthReportEntry ToHealthReportEntry()
    {
        return new HealthReportEntry(Status, null, Duration, null, null);
    }
}