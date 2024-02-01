namespace GameEngine
{
    internal interface IGameManagerFactory
    {
        GameManager GetGameManager(GameSettings gameSettings);
    }
}
