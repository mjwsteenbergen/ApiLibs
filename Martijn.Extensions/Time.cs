using System;

namespace Martijn.Extensions.Time
{
    public static class Time {
        public static DateTime DateTimeFromJS(long milliseconds)
        {

            return new DateTime(1970, 1, 1).Add(new TimeSpan(milliseconds * TimeSpan.TicksPerMillisecond));

        }
    }
}