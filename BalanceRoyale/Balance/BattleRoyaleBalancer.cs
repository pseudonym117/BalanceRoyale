namespace BalanceRoyale.Balance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class BattleRoyaleBalancer : IBalancer
    {
        private readonly IGameManager<HttpContext> currentGameManager;

        public BattleRoyaleBalancer(IGameManager<HttpContext> currentGameManager)
        {
            this.currentGameManager = currentGameManager;
        }

        public async Task HandleRequest(HttpContext context)
        {
            await this.currentGameManager.AddPlayer(context);
        }
    }
}
