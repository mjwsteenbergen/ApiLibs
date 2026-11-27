using System;
using NUnit.Framework;

/// <summary>
/// Runs every parity test in Europe/Amsterdam, so local-time conversions (where DateTimeKind.Local
/// and offsets matter) are exercised even on a UTC build agent.
/// </summary>
[SetUpFixture]
public class TimeZoneSetUp
{
    [OneTimeSetUp]
    public void UseAmsterdamTime()
    {
        Environment.SetEnvironmentVariable("TZ", "Europe/Amsterdam");
        TimeZoneInfo.ClearCachedData();
    }
}
