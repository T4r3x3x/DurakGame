namespace GameEngine.Shufflers
{
    public class FisherYatesShuffler<T> : IShuffler<T>
    {
        private Random _random = new Random();

        /// <summary>
        /// Fisher–Yates algorithm
        /// </summary>
        /// <param name="cards"></param>
        public void Shuffle(IList<T> collection)
        {
            for (int i = collection.Count - 1; i > 0; i--)
            {
                var j = _random.Next(i + 1);
                (collection[i], collection[j]) = (collection[j], collection[i]);
            }
        }
    }
}