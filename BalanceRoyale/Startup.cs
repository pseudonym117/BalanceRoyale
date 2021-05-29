namespace BalanceRoyale
{
    using System;

    using BalanceRoyale.Balance;
    using BalanceRoyale.Battles;
    using BalanceRoyale.Middleware;

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
            services.AddSingleton(this.BuildGameConfig);
            services.AddSingleton<IGameFactory, GameFactory>();
            services.AddSingleton<IGameEndHandler<HttpContext>, WinnersResponseGameEndHandler>();
            services.AddSingleton<IGameManager<HttpContext>, GameManager<HttpContext>>();
            services.AddSingleton<IBalancer, BattleRoyaleBalancer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBalancer balancer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRequestLogging();

            app.Run(balancer.HandleRequestAsync);
        }

        private GameConfig BuildGameConfig(IServiceProvider provider)
        {
            return new GameConfig { MaxPlayers = 4, MaxQueueTime = TimeSpan.FromSeconds(10) };
        }
    }
}
