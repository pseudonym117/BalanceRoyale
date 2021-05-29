namespace BalanceRoyale.Balance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GameConfig
    {
        public uint MaxPlayers { get; set; }
        public TimeSpan MaxQueueTime { get; set; }
    }
}
