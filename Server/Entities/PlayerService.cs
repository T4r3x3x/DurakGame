namespace Server.Entities
{
    public class PlayerService
    {
        private int _id = 0;
        private Dictionary<int, Player> _players = new Dictionary<int, Player>();

        //Заменить на void RegisterPlayer ?
        //public Player CreatePlayer(LoginModel loginModel)
        //{
        //    return new Player() { Id = ++_id, };
        //}
    }
}
