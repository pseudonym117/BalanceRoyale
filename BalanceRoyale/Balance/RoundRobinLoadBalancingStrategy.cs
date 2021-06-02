namespace BalanceRoyale.Balance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class RoundRobinLoadBalancingStrategy<T> : ILoadBalancingStrategy<T>
    {
        private readonly T[] handlers;

        private readonly long resetAtValue;

        // start at -1 so that first increment gives us index 0
        private long currentIndex = -1;

        public RoundRobinLoadBalancingStrategy(IEnumerable<T> handlers)
        {
            this.handlers = handlers.ToArray();

            if (this.handlers.Length < 1)
            {
                throw new ArgumentException("1 or more handlers required", nameof(handlers));
            }

            // given use of memory barriers its rather unlikly we need this many requests to buffer, but just in case...
            this.resetAtValue = long.MaxValue - Environment.ProcessorCount * 3;
        }

        public T Next()
        {
            var reqNum = Interlocked.Increment(ref currentIndex);
            var index = (int) (reqNum % this.handlers.Length);

            // reset index if we are getting close to max value
            Interlocked.CompareExchange(ref this.currentIndex, 0, this.resetAtValue);

            return this.handlers[index];
        }
    }
}
