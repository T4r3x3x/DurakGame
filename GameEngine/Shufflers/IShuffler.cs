namespace GameEngine.Shufflers
{
    public interface IShuffler<T>
    {
        void Shuffle(IList<T> collection);
    }
}
