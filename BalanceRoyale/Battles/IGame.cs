namespace BalanceRoyale.Battles
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IGame<T>
    {
        public IList<Player<T>> Players { get; }

        public Task<GameReport<T>> PlayAsync();
    }
}
