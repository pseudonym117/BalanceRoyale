namespace BalanceRoyale
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BalanceRoyale.Balance;
    using BalanceRoyale.Battles;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var gameConfig = new GameConfig { MaxPlayers = 4, MaxQueueTime = TimeSpan.FromSeconds(10) };

            var gameFactory = new GameFactory();
            var gameEndHandler = new WinnersResponseGameEndHandler();
            var gameManager = new GameManager(gameEndHandler, gameFactory, gameConfig);
            var balancer = new BattleRoyaleBalancer(gameManager);

            app.Run(balancer.HandleRequest);
        }
    }
}
