using GameEngine.Entities.SystemEntites;

namespace DurakClient.Utilities
{
    public static class DefaultValues
    {
        public readonly static string DefaultName = string.Empty;
        public const int DefaultMinPlayersCount = 2;
        public const int DefaultMaxPlayersCount = 6;
        public const int DefaultMinStartCardsCount = 3;
        public const int DefaultMaxStartCardsCount = 7;
        public const bool DefaultAllowCommonDeckType = true;
        public const bool DefaultAllowExtendedDeckType = true;
        public const DeckType DefaultDeckTypeValue = DeckType.Common;
    }
}