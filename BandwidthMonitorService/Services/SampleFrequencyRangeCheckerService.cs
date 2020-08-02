using BandwidthMonitorService.Domain.Models;
using BandwidthMonitorService.Dto.Enums;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BandwidthMonitorService.Services
{
    public class SampleFrequencyRangeCheckerService : ISampleFrequencyRangeCheckerService
    {
        private readonly ITimestampService _timestampService;
        private readonly Calendar _calendar;

        public SampleFrequencyRangeCheckerService(ITimestampService timestampService)
        {
            _timestampService = timestampService;
            _calendar = CultureInfo.InvariantCulture.Calendar;
        }

        public FrequencyRange GetRange(
            IEnumerable<Sample> orderedSamples,
            Frequency frequency)
        {
            var min = orderedSamples.First().Timestamp;
            var max = orderedSamples.Last().Timestamp;
            var minDateTime = _timestampService.FromUnixTimestamp(min);
            var maxDateTime = _timestampService.FromUnixTimestamp(max);

            switch (frequency)
            {
                case Frequency.HourOfDay:
                    {
                        min = minDateTime.Hour;
                        max = maxDateTime.Hour;

                        break;
                    }
                case Frequency.DayOfMonth:
                    {
                        min = minDateTime.DayOfYear;
                        max = maxDateTime.DayOfYear;

                        break;
                    }
                case Frequency.WeekOfYear:
                    {
                        min = _calendar.GetWeekOfYear(
                            minDateTime,
                            CalendarWeekRule.FirstDay,
                            System.DayOfWeek.Monday);
                        max = _calendar.GetWeekOfYear(
                            maxDateTime,
                            CalendarWeekRule.FirstDay,
                            System.DayOfWeek.Monday);

                        break;
                    }
                case Frequency.MonthOfYear:
                    {
                        min = minDateTime.Month;
                        max = maxDateTime.Month;

                        break;
                    }
                case Frequency.Year:
                    {
                        min = minDateTime.Year;
                        max = maxDateTime.Year;

                        break;
                    }
            }

            return new FrequencyRange()
            {
                Frequency = frequency,
                From = minDateTime,
                To = maxDateTime,
                Min = min,
                Max = max
            };
        }
    }
}
