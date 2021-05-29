namespace BalanceRoyale.Battles
{
    public class Player<T>
    {
        public Player(T context)
        {
            this.Context = context;
        }

        public T Context { get; }
    }
}
