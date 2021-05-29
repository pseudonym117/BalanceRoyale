namespace BalanceRoyale.Battles
{
    using System.Collections.Generic;

    public class GameFactory : IGameFactory
    {
        public IGame<T> CreateGame<T>(IEnumerable<Player<T>> players)
        {
            return new FirstWinsGame<T>(players);
        }
    }
}
