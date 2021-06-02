namespace BalanceRoyale.Balance
{
    using System.Linq;
    using System.Threading.Tasks;

    using BalanceRoyale.Battles;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Primitives;

    public class WinnersResponseGameEndHandler : IGameEndHandler<HttpContext>
    {
        private readonly ILogger<WinnersResponseGameEndHandler> logger;
        private readonly IRequestHandler requestHandler;

        public WinnersResponseGameEndHandler(ILogger<WinnersResponseGameEndHandler> logger, IRequestHandler requestHandler)
        {
            this.logger = logger;
            this.requestHandler = requestHandler;
        }

        public async Task HandleGameEndAsync(GameReport<HttpContext> gameReport)
        {
            var winner = gameReport.Winners.First();

            logger.LogInformation("Winner: {}", winner.Context.TraceIdentifier);

            using var winnersResponse = await this.requestHandler.HandleRequest(winner.Context.Request);

            await Task.WhenAll(gameReport.Participants.Select(async player =>
            {
                player.Context.Response.StatusCode = (int)winnersResponse.StatusCode;

                foreach (var header in winnersResponse.Headers)
                {
                    player.Context.Response.Headers.Add(header.Key, new StringValues(header.Value.ToArray()));
                }

                await winnersResponse.Content.CopyToAsync(player.Context.Response.BodyWriter.AsStream());
            }).ToArray());
        }
    }
}
