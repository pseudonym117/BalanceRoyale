namespace BalanceRoyale.Battles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GameReport<T>
    {
        public GameReport(IEnumerable<Player<T>> winners, IEnumerable<Player<T>> participants)
        {
            this.Winners = winners.ToList();
            this.Participants = participants.ToList();
        }

        public IList<Player<T>> Winners { get; }

        public IList<Player<T>> Participants { get; }
    }
}
