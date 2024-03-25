namespace DurakClient.Utilities.Ranges
{
    public interface IRange<T>
    {
        public T Min { get; init; }
        public T Max { get; init; }
        public bool IsInRange(T value);
    }
}