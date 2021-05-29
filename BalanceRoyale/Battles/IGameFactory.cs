namespace BalanceRoyale.Battles
{
    using System.Collections.Generic;

    public interface IGameFactory
    {
        public IGame<T> CreateGame<T>(IEnumerable<Player<T>> players);
    }
}
