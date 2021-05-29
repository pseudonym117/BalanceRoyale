namespace BalanceRoyale.Balance
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using BalanceRoyale.Battles;

    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.Threading;

    public class GameManager<T> : IGameManager<T>
    {
        private readonly IGameEndHandler<T> gameEndHandler;
        private readonly IGameFactory gameFactory;
        private readonly GameConfig gameConfig;

        private readonly SemaphoreSlim pendingPlayersLock = new SemaphoreSlim(1);
        private readonly IList<Player<T>> pendingPlayers;

        private readonly ManualResetEventSlim startGameEvent = new ManualResetEventSlim();

        private readonly SemaphoreSlim currentGameLock = new SemaphoreSlim(1);
        private Task? currentGameTask;

        public GameManager(IGameEndHandler<T> gameEndHandler, IGameFactory gameFactory, GameConfig gameConfig)
        {
            this.gameEndHandler = gameEndHandler;
            this.gameFactory = gameFactory;
            this.gameConfig = gameConfig;

            this.pendingPlayers = new List<Player<T>>((int) this.gameConfig.MaxPlayers);
        }

        public async Task RunGameForPlayerAsync(T playerContext)
        {
            var player = new Player<T>(playerContext);

            await this.pendingPlayersLock.WaitAsync().ConfigureAwait(false);
            try
            {
                this.pendingPlayers.Add(player);

                if (this.pendingPlayers.Count >= this.gameConfig.MaxPlayers)
                {
                    this.startGameEvent.Set();
                }
            }
            finally
            {
                this.pendingPlayersLock.Release();
            }

            await this.GetQueueTaskAsync().ConfigureAwait(false);
        }

        private async Task GetQueueTaskAsync()
        {
            if (this.currentGameTask == null || this.currentGameTask.IsCompleted)
            {
                await this.currentGameLock.WaitAsync();
                try
                {
                    if (this.currentGameTask == null || this.currentGameTask.IsCompleted)
                    {
                        this.currentGameTask = Task.Factory.StartNew(async () =>
                        {
                            this.startGameEvent.Wait((int)this.gameConfig.MaxQueueTime.TotalMilliseconds);

                            await this.pendingPlayersLock.WaitAsync().ConfigureAwait(false);
                            try
                            {
                                var players = this.pendingPlayers.ToList();
                                this.pendingPlayers.Clear();

                                // todo: with a bit of optimizing can probs do this w/o a lock. but cant right now.
                                await this.StartGameAsync(players).ConfigureAwait(false);

                                this.startGameEvent.Reset();
                            }
                            finally
                            {
                                this.pendingPlayersLock.Release();
                            }
                        }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                    }
                }
                finally
                {
                    this.currentGameLock.Release();
                }
            }

            await this.currentGameTask;
        }

        private async Task StartGameAsync(IList<Player<T>> players)
        {
            var game = this.gameFactory.CreateGame(players);

            if (game.CanPlay())
            {
                var result = await game.PlayAsync();

                await this.gameEndHandler.HandleGameEndAsync(result);
            }
        }
    }
}
