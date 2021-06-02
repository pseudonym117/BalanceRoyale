namespace BalanceRoyale.Balance
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class LoadBalancingRequestHandler : IRequestHandler
    {
        private readonly ILoadBalancingStrategy<IRequestHandler> strategy;

        public LoadBalancingRequestHandler(ILoadBalancingStrategy<IRequestHandler> strategy)
        {
            this.strategy = strategy;
        }

        public Task<HttpResponseMessage?> HandleRequest(HttpRequest request)
        {
            var next = this.strategy.Next();
            return next.HandleRequest(request);
        }
    }
}
