namespace BalanceRoyale.Balance
{
    public interface ILoadBalancingStrategy<T>
    {
        public T Next();
    }
}
