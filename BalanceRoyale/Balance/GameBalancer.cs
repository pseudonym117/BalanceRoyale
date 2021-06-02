namespace BalanceRoyale.Balance
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class GameBalancer : IBalancer
    {
        private readonly IGameManager<HttpContext> currentGameManager;

        public GameBalancer(IGameManager<HttpContext> currentGameManager)
        {
            this.currentGameManager = currentGameManager;
        }

        public async Task HandleRequestAsync(HttpContext context)
        {
            await this.currentGameManager.RunGameForPlayerAsync(context);
        }
    }
}
