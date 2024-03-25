using DurakClient.Utilities.Ranges;

using GameEngine.Entities.SystemEntites;

namespace DurakClient.Utilities
{
    public static class DefaultValuesProvider
    {
        public readonly static string DefaultFilterName = string.Empty;
        public readonly static string DefaultLobbyName = string.Empty;
        public readonly static string DefaultPassword = string.Empty;
        public const bool DefaultAllowCommonDeckType = true;
        public const bool DefaultAllowExtendedDeckType = true;
        public const DeckType DefaultDeckTypeValue = DeckType.Common;
        public static IntRange PlayersCountRange => (2, 6);
        public static IntRange CardsStartCountRange => (3, 6);
    }
}