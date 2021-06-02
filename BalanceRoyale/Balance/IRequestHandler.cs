namespace BalanceRoyale.Balance
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IRequestHandler
    {
        public Task<HttpResponseMessage?> HandleRequest(HttpRequest request);
    }
}
