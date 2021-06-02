namespace BalanceRoyale.Balance
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class ProxyingRequestHandler : IRequestHandler, IDisposable
    {
        private readonly HttpClient client = new HttpClient();

        private bool disposedValue;

        public ProxyingRequestHandler(Uri baseAddress)
        {
            this.client.BaseAddress = baseAddress;
        }

        public async Task<HttpResponseMessage?> HandleRequest(HttpRequest request)
        {
            var message = new HttpRequestMessage(TranslateMethod(request.Method), $"{request.Path}{request.QueryString}");
            
            foreach(var header in request.Headers)
            {
                message.Headers.Add(header.Key, header.Value.ToArray());
            }

            return await this.client.SendAsync(message);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.client.Dispose();
                }

                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private static HttpMethod TranslateMethod(string method)
        {
            return method.ToUpperInvariant() switch
            {
                "GET" => HttpMethod.Get,
                "POST" => HttpMethod.Post,
                "PUT" => HttpMethod.Put,
                "PATCH" => HttpMethod.Patch,
                "DELETE" => HttpMethod.Delete,
                "OPTIONS" => HttpMethod.Options,
                "TRACE" => HttpMethod.Trace,
                "HEAD" => HttpMethod.Head,
                _ => throw new ArgumentException($"{method} is not a valid HttpMethod"),
            };
        }
    }
}
