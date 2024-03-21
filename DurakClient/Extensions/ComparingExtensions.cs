namespace DurakClient.Extensions
{
    public static class ComparingExtensions
    {
        public static bool isInRange(this int num, int minValue, int maxValue) => minValue <= num && num <= maxValue;
    }
}
