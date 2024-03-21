namespace DurakClient.MVVM.Models
{
    public class Filter
    {
        public string FilterName { get; set; }
        public int MinPlayersCount { get; set; }
        public int MaxPlayersCount { get; set; }
        public int MinStartCardsCount { get; set; }
        public int MaxStartCardsCount { get; set; }
        public bool IsAllowCommonDeckType { get; set; }
        public bool IsAllowExtendedDeckType { get; set; }

        public Filter(string filterName, int minPlayersCount, int maxPlayersCount, int minStartCardsCount, int maxStartCardsCount, bool isAllowCommonDeckType, bool isAllowExtendedDeckType)
        {
            FilterName = filterName;
            MinPlayersCount = minPlayersCount;
            MaxPlayersCount = maxPlayersCount;
            MinStartCardsCount = minStartCardsCount;
            MaxStartCardsCount = maxStartCardsCount;
            IsAllowCommonDeckType = isAllowCommonDeckType;
            IsAllowExtendedDeckType = isAllowExtendedDeckType;
        }
    }
}
