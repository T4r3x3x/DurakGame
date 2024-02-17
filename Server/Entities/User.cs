namespace Server.Entities
{
    public class User
    {
        public required Guid Guid { get; set; }
        public required string NickName { get; set; }
        public bool AreReady { get; set; }
    }
}