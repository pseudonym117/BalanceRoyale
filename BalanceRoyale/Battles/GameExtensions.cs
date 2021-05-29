namespace BalanceRoyale.Battles
{
    public static class GameExtensions
    {
        public static bool CanPlay<T>(this IGame<T> game)
        {
            return game.Players.Count > 0;
        }
    }
}
