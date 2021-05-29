namespace BalanceRoyale.Balance
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using BalanceRoyale.Battles;

    using Microsoft.AspNetCore.Http;
    using Microsoft.VisualStudio.Threading;

    public class GameManager : IGameManager<HttpContext>
    {
        private readonly IGameEndHandler<HttpContext> gameEndHandler;
        private readonly IGameFactory gameFactory;
        private readonly GameConfig gameConfig;

        private readonly SemaphoreSlim pendingPlayersLock = new SemaphoreSlim(1);
        private readonly IList<Player<HttpContext>> pendingPlayers = new List<Player<HttpContext>>();

        private readonly CancellationTokenSource gameStartTokenSource = new CancellationTokenSource();
        private Task? gameStartTask;

        public GameManager(IGameEndHandler<HttpContext> gameEndHandler, IGameFactory gameFactory, GameConfig gameConfig)
        {
            this.gameEndHandler = gameEndHandler;
            this.gameFactory = gameFactory;
            this.gameConfig = gameConfig;
        }

        public async Task AddPlayer(HttpContext playerContext)
        {
            var player = new Player<HttpContext>(playerContext);

            await this.pendingPlayersLock.WaitAsync();
            try
            {
                this.pendingPlayers.Add(player);

                if (this.pendingPlayers.Count > this.gameConfig.MaxPlayers)
                {
                    this.CancelAndResetTimerNoLock();
                }
                else if (this.gameStartTask == null)
                {
                    this.gameStartTask = 
                        Task.Delay(this.gameConfig.MaxQueueTime)
                            .ContinueWith(async task => 
                            {
                                await this.pendingPlayersLock.WaitAsync();
                                try
                                {
                                    var players = this.GetAndClearPlayersNoLock();
                                    await this.StartGameAsync(players);
                                    this.gameStartTask = null;
                                }
                                finally
                                {
                                    this.pendingPlayersLock.Release();
                                }

                            }, this.gameStartTokenSource.Token, TaskContinuationOptions.None, TaskScheduler.Default);
                }
            }
            finally
            {
                this.pendingPlayersLock.Release();
            }

            if (this.gameStartTask != null)
            {
                try
                {
                    await this.gameStartTask;
                }
                catch (TaskCanceledException)
                {
                    // expected case when we hit lobby limit
                }
            }
        }

        private IList<Player<HttpContext>> GetAndClearPlayersNoLock()
        {
            var players = this.pendingPlayers.ToList();
            this.pendingPlayers.Clear();
            return players;
        }

        private void CancelAndResetTimerNoLock()
        {
            if (this.gameStartTask != null)
            {
                this.gameStartTokenSource.Cancel();
            }
        }

        private async Task StartGameAsync(IList<Player<HttpContext>> players)
        {
            var game = this.gameFactory.CreateGame(players);

            if (game.CanPlay())
            {
                var result = await game.PlayAsync();

                await this.gameEndHandler.HandleGameEnd(result);
            }
        }
    }
}
