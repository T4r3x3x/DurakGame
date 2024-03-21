using Grpc.Core;
using Grpc.Net.Client;

using System.Net.Http;

namespace DurakClient.Services.ChannelProviders
{
    internal class ChannelProvider : IChannelProvider
    {
        // TODO: Доставать из конфига
        private const string SERVER_ADDRESS = "https://localhost:5058";

        public CallInvoker GetInvoker()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            var channel = GrpcChannel.ForAddress(SERVER_ADDRESS, new GrpcChannelOptions { HttpHandler = handler });
            return ExtractInvoker(channel);
        }

        protected virtual CallInvoker ExtractInvoker(GrpcChannel channel)
        {
            return channel.CreateCallInvoker();
        }
    }
}