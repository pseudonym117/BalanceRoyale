namespace BalanceRoyale.Balance
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IBalancer
    {
        public Task HandleRequestAsync(HttpContext context);
    }
}
