namespace BalanceRoyale.Balance
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class BattleRoyaleBalancer : IBalancer
    {
        private readonly IGameManager<HttpContext> currentGameManager;

        public BattleRoyaleBalancer(IGameManager<HttpContext> currentGameManager)
        {
            this.currentGameManager = currentGameManager;
        }

        public async Task HandleRequestAsync(HttpContext context)
        {
            await this.currentGameManager.RunGameForPlayerAsync(context);
        }
    }
}
