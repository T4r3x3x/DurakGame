namespace DurakClient.Results
{
    public class JoinResult
    {
        public JoinResultStatus Status { get; init; }

        public override string ToString()
        {
            return Status switch
            {
                JoinResultStatus.WrongPassword => "Inputed password is incorrect",
                JoinResultStatus.LobbyNotFound => "Something happend with lobby :(",
                JoinResultStatus.LobbyIsFull => "Lobby is already full =(",
                JoinResultStatus.UnkownException => "Unknown error =(",
                _ => "Unhalded exception",
            };
        }
    }
}