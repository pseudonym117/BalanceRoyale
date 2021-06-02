namespace BalanceRoyale.Balance
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class RepeatRequestHandler : IRequestHandler
    {
        private readonly int instanceId;

        public RepeatRequestHandler(int instanceId)
        {
            this.instanceId = instanceId;
        }

        public async Task<HttpResponseMessage?> HandleRequest(HttpRequest request)
        {
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new
                {
                    Path = request.Path,
                    Query = request.Query,
                    InstanceId = this.instanceId,
                }),
            };
        }
    }
}
