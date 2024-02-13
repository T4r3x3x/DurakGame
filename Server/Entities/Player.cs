namespace Server.Entities
{
    public class Player
    {
        public required Guid Guid { get; set; }
        public required string NickName { get; set; }
    }
}