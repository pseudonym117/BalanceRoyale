namespace BalanceRoyale.Battles
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class FirstWinsGame<T> : IGame<T>
    {
        public IList<Player<T>> Players { get; }

        public FirstWinsGame(IEnumerable<Player<T>> players)
        {
            this.Players = players.ToList();
        }

        public Task<GameReport<T>> PlayAsync()
        {
            return Task.FromResult(new GameReport<T>(new List<Player<T>> { this.Players[0] }, this.Players));
        }
    }
}
