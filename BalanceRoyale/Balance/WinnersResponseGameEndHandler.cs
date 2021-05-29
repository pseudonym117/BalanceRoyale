namespace BalanceRoyale.Balance
{
    using System.Linq;
    using System.Threading.Tasks;

    using BalanceRoyale.Battles;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    public class WinnersResponseGameEndHandler : IGameEndHandler<HttpContext>
    {
        private readonly ILogger<WinnersResponseGameEndHandler> logger;

        public WinnersResponseGameEndHandler(ILogger<WinnersResponseGameEndHandler> logger)
        {
            this.logger = logger;
        }

        public async Task HandleGameEndAsync(GameReport<HttpContext> gameReport)
        {
            var winner = gameReport.Winners.First();

            logger.LogInformation("Winner: {}", winner.Context.TraceIdentifier);

            var winnersResponse = winner.Context.Request.Path;

            await Task.WhenAll(gameReport.Participants.Select(async player =>
            {
                player.Context.Response.ContentType = "text/plain";
                await player.Context.Response.WriteAsync(winnersResponse);
            }).ToArray());
        }
    }
}
