using System;
using System.Globalization;

namespace Martijn.Extensions.JSON.Compat
{
    /// <summary>
    /// Port of the date recognition in Newtonsoft.Json's DateTimeUtils/DateTimeParser, using
    /// DateTimeZoneHandling.RoundtripKind (the default): ISO 8601 "yyyy-MM-ddTHH:mm:ss[.fffffff][zone]"
    /// and Microsoft "/Date(ticks[+-HHmm])/" strings.
    /// </summary>
    internal static class NewtonsoftDates
    {
        public const string IsoDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
        public const string IsoDateTimeOffsetFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFzzz";

        private static readonly long InitialJavaScriptDateTicks = 621355968000000000;

        private enum Zone { Unspecified, Utc, LocalWestOfUtc, LocalEastOfUtc }

        private struct Parsed
        {
            public int Year, Month, Day, Hour, Minute, Second, Fraction, ZoneHour, ZoneMinute;
            public Zone Zone;
        }

        /// <summary>What JsonTextReader does with a string token when DateParseHandling is DateTime.</summary>
        public static bool TryParseDateTime(string s, out DateTime dt)
        {
            dt = default;
            if (s.Length == 0)
            {
                return false;
            }

            if (s[0] == '/')
            {
                return s.Length >= 9 && s.StartsWith("/Date(", StringComparison.Ordinal) && s.EndsWith(")/", StringComparison.Ordinal)
                    && TryParseDateTimeMicrosoft(s, out dt);
            }

            if (s.Length >= 19 && s.Length <= 40 && char.IsDigit(s[0]) && s[10] == 'T')
            {
                return TryParseDateTimeIso(s, out dt);
            }

            return false;
        }

        public static bool TryParseDateTimeOffset(string s, out DateTimeOffset dt)
        {
            dt = default;
            if (s.Length == 0)
            {
                return false;
            }

            if (s[0] == '/')
            {
                return s.Length >= 9 && s.StartsWith("/Date(", StringComparison.Ordinal) && s.EndsWith(")/", StringComparison.Ordinal)
                    && TryParseDateTimeOffsetMicrosoft(s, out dt);
            }

            if (s.Length >= 19 && s.Length <= 40 && char.IsDigit(s[0]) && s[10] == 'T')
            {
                return TryParseDateTimeOffsetIso(s, out dt);
            }

            return false;
        }

        /// <summary>JsonReader.ReadDateTimeString: Newtonsoft's own parser, then DateTime.TryParse.</summary>
        public static bool TryReadDateTimeString(string s, out DateTime dt)
        {
            return TryParseDateTime(s, out dt) || DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dt);
        }

        /// <summary>JsonReader.ReadDateTimeOffsetString: Newtonsoft's own parser, then DateTimeOffset.TryParse.</summary>
        public static bool TryReadDateTimeOffsetString(string s, out DateTimeOffset dt)
        {
            return TryParseDateTimeOffset(s, out dt) || DateTimeOffset.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dt);
        }

        public static string Format(DateTime value) => value.ToString(IsoDateTimeFormat, CultureInfo.InvariantCulture);

        public static string Format(DateTimeOffset value) => value.ToString(IsoDateTimeOffsetFormat, CultureInfo.InvariantCulture);

        private static bool TryParseDateTimeIso(string text, out DateTime dt)
        {
            dt = default;
            if (!TryParseIso(text, out var p))
            {
                return false;
            }

            DateTime d = Create(p);
            long ticks;
            switch (p.Zone)
            {
                case Zone.Utc:
                    d = new DateTime(d.Ticks, DateTimeKind.Utc);
                    break;
                case Zone.LocalWestOfUtc:
                {
                    var offset = new TimeSpan(p.ZoneHour, p.ZoneMinute, 0);
                    ticks = d.Ticks + offset.Ticks;
                    if (ticks <= DateTime.MaxValue.Ticks)
                    {
                        d = new DateTime(ticks, DateTimeKind.Utc).ToLocalTime();
                    }
                    else
                    {
                        ticks += TimeZoneInfo.Local.GetUtcOffset(d).Ticks;
                        if (ticks > DateTime.MaxValue.Ticks)
                        {
                            ticks = DateTime.MaxValue.Ticks;
                        }
                        d = new DateTime(ticks, DateTimeKind.Local);
                    }
                    break;
                }
                case Zone.LocalEastOfUtc:
                {
                    var offset = new TimeSpan(p.ZoneHour, p.ZoneMinute, 0);
                    ticks = d.Ticks - offset.Ticks;
                    if (ticks >= DateTime.MinValue.Ticks)
                    {
                        d = new DateTime(ticks, DateTimeKind.Utc).ToLocalTime();
                    }
                    else
                    {
                        ticks += TimeZoneInfo.Local.GetUtcOffset(d).Ticks;
                        if (ticks < DateTime.MinValue.Ticks)
                        {
                            ticks = DateTime.MinValue.Ticks;
                        }
                        d = new DateTime(ticks, DateTimeKind.Local);
                    }
                    break;
                }
            }

            dt = d;
            return true;
        }

        private static bool TryParseDateTimeOffsetIso(string text, out DateTimeOffset dt)
        {
            dt = default;
            if (!TryParseIso(text, out var p))
            {
                return false;
            }

            DateTime d = Create(p);
            TimeSpan offset = p.Zone switch
            {
                Zone.Utc => TimeSpan.Zero,
                Zone.LocalWestOfUtc => new TimeSpan(-p.ZoneHour, -p.ZoneMinute, 0),
                Zone.LocalEastOfUtc => new TimeSpan(p.ZoneHour, p.ZoneMinute, 0),
                _ => TimeZoneInfo.Local.GetUtcOffset(d),
            };

            long ticks = d.Ticks - offset.Ticks;
            if (ticks < 0 || ticks > 3155378975999999999)
            {
                return false;
            }

            try
            {
                dt = new DateTimeOffset(d, offset);
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        private static DateTime Create(Parsed p)
        {
            bool is24Hour = p.Hour == 24;
            DateTime d = new DateTime(p.Year, p.Month, p.Day, is24Hour ? 0 : p.Hour, p.Minute, p.Second);
            d = d.AddTicks(p.Fraction);
            if (is24Hour)
            {
                d = d.AddDays(1);
            }
            return d;
        }

        private static bool TryParseIso(string text, out Parsed p)
        {
            p = default;
            int end = text.Length;
            if (end < 19)
            {
                return false;
            }

            if (!(Parse4Digit(text, 0, out p.Year) && p.Year >= 1 && text[4] == '-' &&
                  Parse2Digit(text, 5, out p.Month) && p.Month >= 1 && p.Month <= 12 && text[7] == '-' &&
                  Parse2Digit(text, 8, out p.Day) && p.Day >= 1 && p.Day <= DateTime.DaysInMonth(p.Year, p.Month)))
            {
                return false;
            }

            if (text[10] != 'T')
            {
                return false;
            }

            int start = 11;
            if (!(Parse2Digit(text, start, out p.Hour) && p.Hour <= 24 && text[start + 2] == ':' &&
                  Parse2Digit(text, start + 3, out p.Minute) && p.Minute < 60 && text[start + 5] == ':' &&
                  Parse2Digit(text, start + 6, out p.Second) && p.Second < 60 &&
                  (p.Hour != 24 || (p.Minute == 0 && p.Second == 0))))
            {
                return false;
            }

            start += 8;
            if (start < end && text[start] == '.')
            {
                p.Fraction = 0;
                int digits = 0;
                while (++start < end && digits < 7)
                {
                    int digit = text[start] - '0';
                    if (digit < 0 || digit > 9)
                    {
                        break;
                    }
                    p.Fraction = (p.Fraction * 10) + digit;
                    digits++;
                }

                if (digits < 7)
                {
                    if (digits == 0)
                    {
                        return false;
                    }
                    for (int i = digits; i < 7; i++)
                    {
                        p.Fraction *= 10;
                    }
                }

                if (p.Hour == 24 && p.Fraction != 0)
                {
                    return false;
                }
            }

            if (start < end)
            {
                char ch = text[start];
                if (ch == 'z' || ch == 'Z')
                {
                    p.Zone = Zone.Utc;
                    start++;
                }
                else
                {
                    if (start + 2 < end && Parse2Digit(text, start + 1, out p.ZoneHour) && p.ZoneHour <= 99)
                    {
                        switch (ch)
                        {
                            case '-':
                                p.Zone = Zone.LocalWestOfUtc;
                                start += 3;
                                break;
                            case '+':
                                p.Zone = Zone.LocalEastOfUtc;
                                start += 3;
                                break;
                        }
                    }

                    if (start < end)
                    {
                        if (text[start] == ':')
                        {
                            start += 1;
                            if (start + 1 < end && Parse2Digit(text, start, out p.ZoneMinute) && p.ZoneMinute <= 99)
                            {
                                start += 2;
                            }
                        }
                        else
                        {
                            if (start + 1 < end && Parse2Digit(text, start, out p.ZoneMinute) && p.ZoneMinute <= 99)
                            {
                                start += 2;
                            }
                        }
                    }
                }
            }

            return start == end;
        }

        private static bool Parse4Digit(string text, int start, out int num)
        {
            num = 0;
            if (start + 3 >= text.Length)
            {
                return false;
            }
            int d1 = text[start] - '0', d2 = text[start + 1] - '0', d3 = text[start + 2] - '0', d4 = text[start + 3] - '0';
            if (d1 is >= 0 and < 10 && d2 is >= 0 and < 10 && d3 is >= 0 and < 10 && d4 is >= 0 and < 10)
            {
                num = (((((d1 * 10) + d2) * 10) + d3) * 10) + d4;
                return true;
            }
            return false;
        }

        private static bool Parse2Digit(string text, int start, out int num)
        {
            num = 0;
            if (start + 1 >= text.Length)
            {
                return false;
            }
            int d1 = text[start] - '0', d2 = text[start + 1] - '0';
            if (d1 is >= 0 and < 10 && d2 is >= 0 and < 10)
            {
                num = (d1 * 10) + d2;
                return true;
            }
            return false;
        }

        private static bool TryParseMicrosoftDate(string text, out long ticks, out TimeSpan offset, out DateTimeKind kind)
        {
            kind = DateTimeKind.Utc;
            offset = TimeSpan.Zero;
            ticks = 0;

            int length = text.Length;
            int index = text.IndexOf('+', 7, length - 8);
            if (index == -1)
            {
                index = text.IndexOf('-', 7, length - 8);
            }

            if (index != -1)
            {
                kind = DateTimeKind.Local;
                if (!TryReadOffset(text, index, out offset))
                {
                    return false;
                }
            }
            else
            {
                index = length - 2;
            }

            return long.TryParse(text.Substring(6, index - 6), NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out ticks);
        }

        private static bool TryReadOffset(string text, int startIndex, out TimeSpan offset)
        {
            bool negative = text[startIndex] == '-';
            offset = default;
            if (startIndex + 3 > text.Length || !int.TryParse(text.Substring(startIndex + 1, 2), NumberStyles.None, CultureInfo.InvariantCulture, out int hours))
            {
                return false;
            }

            int minutes = 0;
            if (text.Length - startIndex > 5)
            {
                if (!int.TryParse(text.Substring(startIndex + 3, 2), NumberStyles.None, CultureInfo.InvariantCulture, out minutes))
                {
                    return false;
                }
            }

            offset = TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes);
            if (negative)
            {
                offset = offset.Negate();
            }
            return true;
        }

        private static bool TryParseDateTimeMicrosoft(string text, out DateTime dt)
        {
            dt = default;
            if (!TryParseMicrosoftDate(text, out long ticks, out _, out DateTimeKind kind))
            {
                return false;
            }

            DateTime utc;
            try
            {
                utc = new DateTime((ticks * 10000) + InitialJavaScriptDateTicks, DateTimeKind.Utc);
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            dt = kind switch
            {
                DateTimeKind.Unspecified => DateTime.SpecifyKind(utc.ToLocalTime(), DateTimeKind.Unspecified),
                DateTimeKind.Local => utc.ToLocalTime(),
                _ => utc
            };
            return true;
        }

        private static bool TryParseDateTimeOffsetMicrosoft(string text, out DateTimeOffset dt)
        {
            dt = default;
            if (!TryParseMicrosoftDate(text, out long ticks, out TimeSpan offset, out _))
            {
                return false;
            }

            try
            {
                DateTime utc = new DateTime((ticks * 10000) + InitialJavaScriptDateTicks, DateTimeKind.Utc);
                dt = new DateTimeOffset(utc.Add(offset).Ticks, offset);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }
    }
}
