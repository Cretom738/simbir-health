namespace Application.Helpers
{
    public static class DateTimeRoundingHelper
    {
        private static readonly long ThirtyMinutesTicks = TimeSpan.FromMinutes(30).Ticks;

        public static bool IsRounded(DateTime dateTime) => dateTime.Ticks % ThirtyMinutesTicks == 0;
    }
}
