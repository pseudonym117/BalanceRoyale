namespace BalanceRoyale.Balance
{
    using System.Threading.Tasks;

    public interface IGameManager<T>
    {
        public Task RunGameForPlayerAsync(T playerContext);
    }
}
