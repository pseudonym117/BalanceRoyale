namespace BalanceRoyale.Balance
{
    using System.Threading.Tasks;

    using BalanceRoyale.Battles;

    public interface IGameEndHandler<T>
    {
        public Task HandleGameEnd(GameReport<T> gameReport);
    }
}
