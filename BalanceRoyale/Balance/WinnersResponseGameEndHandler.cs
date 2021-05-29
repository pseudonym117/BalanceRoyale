namespace BalanceRoyale.Balance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BalanceRoyale.Battles;

    using Microsoft.AspNetCore.Http;

    public class WinnersResponseGameEndHandler : IGameEndHandler<HttpContext>
    {
        public async Task HandleGameEnd(GameReport<HttpContext> gameReport)
        {
            var winner = gameReport.Winners.First();

            var winnersResponse = winner.Context.Request.Path;

            await Task.WhenAll(gameReport.Participants.Select(async player =>
            {
                player.Context.Response.ContentType = "text/plain";
                await player.Context.Response.WriteAsync(winnersResponse);
            }).ToArray());
        }
    }
}
