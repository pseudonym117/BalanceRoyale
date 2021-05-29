namespace BalanceRoyale.Battles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GameFactory : IGameFactory
    {
        public IGame<T> CreateGame<T>(IEnumerable<Player<T>> players)
        {
            return new FirstWinsGame<T>(players);
        }
    }
}
