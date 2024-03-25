using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace DurakClient.Utilities.Ranges
{
    public class IntRange : ReactiveObject, IRange<int>
    {
        public IntRange(int min, int max)
        {
            Min = min;
            Max = max;
        }

        [Reactive] public int Min { get; init; }
        [Reactive] public int Max { get; init; }

        public bool IsInRange(int value) => Min <= value && value <= Max;

        public static implicit operator IntRange((int, int) tuple) => new IntRange(tuple.Item1, tuple.Item2);
    }
}