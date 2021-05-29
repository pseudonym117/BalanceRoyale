namespace BalanceRoyale.Battles
{
    using System.Collections.Generic;
    using System.Linq;

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
