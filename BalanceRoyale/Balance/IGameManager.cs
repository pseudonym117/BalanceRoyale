namespace BalanceRoyale.Balance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IGameManager<T>
    {
        public Task AddPlayer(T playerContext);
    }
}
